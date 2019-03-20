using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PersonalFinances.Services.Exceptions;
using PersonalFinances.Repositories;
using PersonalFinances.Models;
using PersonalFinances.Models.Enums;

namespace PersonalFinances.Services
{
    public class CreditCardService
    {
        private CreditCardRepository _repository = new CreditCardRepository();
        private InvoiceService _invoiceService = new InvoiceService();

        /// <summary>
        /// Get all credit cards
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CreditCard>> GetAll ()
        {
            return await _repository.GetCreditCards();
        }

        /// <summary>
        /// Insert a new credit card
        /// </summary>
        /// <param name="creditCard"></param>
        /// <returns></returns>
        public async Task Insert (CreditCard creditCard)
        {
            creditCard.Enabled = true;
            var nameExists = (await _repository.GetCreditCardsByName(creditCard.Name)).Count() > 0;

            if (!nameExists)
                await _repository.Insert(creditCard);
            else
                throw new AlreadyExistsException($"Already exists a credit card with the name {creditCard.Name}");
        }

        /// <summary>
        /// Update an existing credit card
        /// </summary>
        /// <param name="creditCard"></param>
        /// <returns></returns>
        public async Task Update (CreditCard creditCard)
        {
            creditCard.Enabled = true;
            var nameExists = (await _repository.GetCreditCardsByName(creditCard.Name))
                .Count(c => !c.Id.Equals(creditCard.Id)) > 0;

            if (!nameExists)
                await _repository.Update(creditCard);
            else
                throw new AlreadyExistsException($"Already exists a credit card with the name {creditCard.Name}");
        }

        /// <summary>
        /// Remove a credit card
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Remove (int id)
        {
            var creditCard = await GetById(id);

            creditCard.Enabled = false;
            await _repository.Update(creditCard);
        }

        /// <summary>
        /// Get an invoice from a credit card based on date reference (monthName/year)
        /// </summary>
        /// <param name="creditCardId"></param>
        /// <returns></returns>
        public async Task<Invoice> GetInvoiceByAccountingDate (int creditCardId, DateTime accountingDate)
        {
            var creditCard = await GetById(creditCardId);
            var reference = _invoiceService.GetInvoiceReferenceByAccountingDate(int.Parse(creditCard.InvoiceClosure), accountingDate);
            var currentInvoice = creditCard.Invoices.SingleOrDefault(i => i.Reference.Equals(reference));

            if (currentInvoice != null)
                return currentInvoice;
            else
                throw new InvoiceNotFoundException($"Not found a {reference} invoice from this credit card");
        }

        /// <summary>
        /// Get the next invoice to pay
        /// </summary>
        /// <returns></returns>
        public Invoice GeNextInvoiceToPay (CreditCard creditCard)
        {
            return creditCard.Invoices
                .Where(i => i.InvoiceStatus != InvoiceStatus.Paid)
            .OrderBy(i => i.MaturityDate).FirstOrDefault();
        }

        /// <summary>
        /// Get a collection of available days (for payment day and invoice closure day)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAvailableDays ()
        {
            int[] availableDays = new int[30];

            for (var i = 0; i < availableDays.Length; i++)
                availableDays[i] = i + 1;

            return availableDays.Select(day => day.ToString());
        }

        /// <summary>
        /// Check if a credit can be used 
        /// </summary>
        /// <param name="creditCard"></param>
        /// <returns></returns>
        public void CanBeUsed (CreditCard creditCard, Movement movement)
        {
            if (!movement.Type.Equals("D"))
                throw new NotValidOperationException("Credit cards can be used only with expenses movements");

            if (!(creditCard.RemainingLimit >= movement.TotalValue))
                throw new NotValidOperationException($"The remaining balance of {creditCard.Name} credit card is {creditCard.RemainingLimit.ToString("F2")}");
        }

        /// <summary>
        /// Get a credit card by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CreditCard> GetById (int id)
        {
            var creditCard = await _repository.GetCreditCardById(id);

            if (creditCard != null)
                return creditCard;
            else
                throw new NotFoundException("This credit card not exists");
        }
    }
}