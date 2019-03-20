using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using PersonalFinances.Models;
using PersonalFinances.Models.Enums;
using PersonalFinances.Models.ViewModels;
using PersonalFinances.Repositories;
using PersonalFinances.Services.Exceptions;

namespace PersonalFinances.Services
{
    public class InvoiceService
    {
        private InvoiceRepository _repository = new InvoiceRepository();
        private CategoryService _categoryService = new CategoryService();
        private MovementService _movementService;

        /// <summary>
        /// Insert a new invoice
        /// </summary>
        /// <returns></returns>
        public void Insert (Invoice invoice)
        {
            _repository.Insert(invoice);
        }

        /// <summary>
        /// Pay an invoice
        /// </summary>
        /// <param name="invoice"></param>
        public async Task Pay (InvoicePaymentViewModel viewModel)
        {
            _movementService = new MovementService();
            var invoice = await _repository.GetInvoiceById(viewModel.Invoice.Id);

            invoice.InvoiceStatus = InvoiceStatus.Paid;
            invoice.PaymentDate = viewModel.PaymentDate;

            viewModel.Invoice = invoice;

            if (viewModel.PaidValue > invoice.TotalValue)
                throw new InvalidOperationException($"The max value to pay is {viewModel.Invoice.TotalValue.ToString("F2")}");

            await _movementService.Add(await CreateInvoicePaymentMovementObject(viewModel));
            await _movementService.Update(invoice.Movements.Select((m) => { m.MovementStatus = MovementStatus.Launched; return m; }), false);
            await _repository.Update(invoice);
        }

        /// <summary>
        /// Remove a movement from a invoice
        /// </summary>
        /// <returns></returns>
        public async Task RemoveFromInvoice (int movementId)
        {
            _movementService = new MovementService();

            var movement = await _movementService.GetById(movementId);

            if (movement.Invoice.InvoiceStatus != InvoiceStatus.NotClosed)
                throw new NotValidOperationException($"The invoice #{movement.Invoice.Id} ({movement.Invoice.Reference}) is already closed");

            movement.Invoice = null;
            movement.InvoiceId = null;

            await _movementService.Update(movement);
        }

        /// <summary>
        /// Create a new Invoice object
        /// </summary>
        /// <returns>A Invoice object</returns>
        public Invoice CreateInvoiceObject (CreditCard creditCard, DateTime accountingDate)
        {
            var reference = GetInvoiceReferenceByAccountingDate(int.Parse(creditCard.InvoiceClosure), accountingDate);
            var maturityDate = DateTime.ParseExact($"{creditCard.PayDay}/{reference}", "d/MMM/yyyy",
                System.Globalization.CultureInfo.InstalledUICulture).AddMonths(1);

            return new Invoice
            {
                Reference = reference,
                MaturityDate = maturityDate,
                PaymentDate = null,
                Closed = false,
                InvoiceStatus = InvoiceStatus.NotClosed,
                CreditCardId = creditCard.Id
            };
        }

        /// <summary>
        /// Create a Movement object base on a invoice payment
        /// </summary>
        /// <param name="invoicePaymentViewModel"></param>
        /// <returns></returns>
        private async Task<Movement> CreateInvoicePaymentMovementObject (InvoicePaymentViewModel invoicePaymentViewModel)
        {
            var paymentCategory = await _categoryService.GetByName("Payments", "D");
            var creditCardSubcategory = paymentCategory.Subcategories.Where(s => s.Name.Equals("Credit card")).First();
            var creditCardName = invoicePaymentViewModel.Invoice.CreditCard.Name.Substring(0,
                (invoicePaymentViewModel.Invoice.CreditCard.Name.Length > 10) ? 10 : invoicePaymentViewModel.Invoice.CreditCard.Name.Length);

            return new Movement
            {
                Type = "D",
                Description = $"Credit card invoice payment: {creditCardName} - {invoicePaymentViewModel.Invoice.Reference}",
                Value = invoicePaymentViewModel.PaidValue,
                AccountingDate = invoicePaymentViewModel.PaymentDate,
                AccountId = invoicePaymentViewModel.AccountId,
                CategoryId = paymentCategory.Id,
                SubcategoryId = creditCardSubcategory.Id,
                MovementStatus = MovementStatus.Launched,
                CanEdit = false,
                AutomaticallyLaunch = invoicePaymentViewModel.AutomaticallyLaunch
            };
        }

        /// <summary>
        /// Get an invoice reference (monthName/year) by credit card and accounting date
        /// </summary>
        /// <param name="creditCard"></param>
        /// <param name="accountingDate"></param>
        /// <returns></returns>
        public string GetInvoiceReferenceByAccountingDate (int creditCardClosureDay, DateTime accountingDate)
        {
            List<DateTime[]> ranges = new List<DateTime[]>();
            var accountingYear = accountingDate.Year;

            // Range (min date x max date)
            for (var monthNumber = 1; monthNumber <= 12; monthNumber++)
            {
                DateTime minDate, maxDate;
                int day, month, year;

                // Min date
                if (monthNumber.Equals(1))
                {
                    year = accountingYear - 1;
                    month = 12;
                }
                else
                {
                    year = accountingYear;
                    month = monthNumber - 1;
                }

                day = (DateTime.DaysInMonth(year, month) < creditCardClosureDay ? DateTime.DaysInMonth(year, month) : creditCardClosureDay);
                minDate = new DateTime(year, month, day);

                // Max date
                year = accountingYear;
                month = monthNumber;
                day = (DateTime.DaysInMonth(year, month)) < creditCardClosureDay ? DateTime.DaysInMonth(year, month) : creditCardClosureDay;

                maxDate = new DateTime(year, month, day);

                ranges.Add(new DateTime[] { minDate, maxDate });
            }

            foreach (var range in ranges)
                if (accountingDate > range[0] && accountingDate <= range[1])
                    return string.Concat(range[1].ToString("MMM"), "/", range[1].ToString("yyyy"));

            return string.Empty;
        }

        /// <summary>
        /// Get an invoice by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Invoice> GetById (int id)
        {
            var invoice = await _repository.GetInvoiceById(id);

            if (invoice != null)
                return invoice;
            else
                throw new NotFoundException("This invoice not exist");
        }
        
        /// <summary>
        /// Get a closed invoice by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Invoice> GetClosedInvoiceById (int id)
        {
            var invoice = await GetById(id);

            if (invoice.Closed)
                return invoice;
            else
                throw new NotFoundException($"The invoice #{invoice.Id} ({invoice.Reference}) is not closed yet");
        }
    }
}