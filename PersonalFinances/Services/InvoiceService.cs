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
            var reference = GetInvoiceReference(creditCard, accountingDate);
            var maturityDate = DateTime.ParseExact($"{creditCard.PayDay}/{reference}", "d/MMM/yy",
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
                MovementStatus = MovementStatus.Launched
            };
        }

        /// <summary>
        /// Get an invoice reference (monthName/year) by credit card and accounting date
        /// </summary>
        /// <param name="creditCard"></param>
        /// <param name="accountingDate"></param>
        /// <returns></returns>
        public string GetInvoiceReference (CreditCard creditCard, DateTime accountingDate)
        {
            Dictionary<string, DateTime> range = new Dictionary<string, DateTime>();

            range.Add("minDate", new DateTime((accountingDate.Month.Equals(1) ? accountingDate.Year - 1 : accountingDate.Year),
                    (accountingDate.Month.Equals(1) ? 12 : accountingDate.Month - 1), int.Parse(creditCard.InvoiceClosure)));
            range.Add("maxDate", new DateTime((accountingDate.Month.Equals(12) ? accountingDate.AddYears(1).Year : accountingDate.Year),
                    (accountingDate.Day > int.Parse(creditCard.InvoiceClosure) ? accountingDate.AddMonths(1).Month : accountingDate.Month), 
                        int.Parse(creditCard.InvoiceClosure)));

            if (accountingDate > range["minDate"] && accountingDate <= range["maxDate"])
                return string.Concat(range["maxDate"].ToString("MMM"), "/", range["maxDate"].ToString("yy"));
            else
                return string.Concat(accountingDate.AddMonths(1).ToString("MMM"), "/",
                    ((accountingDate.Month.Equals(12)) ? accountingDate.AddYears(1) : accountingDate).ToString("yy"));
        }

        /// <summary>
        /// Get an invoice by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Invoice> GetById (int id)
        {
            return await _repository.GetInvoiceById(id);
        }
        
        /// <summary>
        /// Get an closed invoice by Id
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