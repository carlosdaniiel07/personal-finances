using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

using PersonalFinances.Models;
using PersonalFinances.Models.Enums;

namespace PersonalFinances.Repositories
{
    public class CreditCardRepository
    {
        /// <summary>
        /// Get all credit cards
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CreditCard>> GetCreditCards ()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.CreditCards
                    .Include(c => c.Invoices)
                    .Include("Invoices.Movements")
                .Where(c => c.Enabled).ToListAsync();
            }
        }

        /// <summary>
        /// Get credit cards by Name
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CreditCard>> GetCreditCardsByName (string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.CreditCards.Where(c => c.Name.Equals(name) && c.Enabled).ToListAsync();
            }
        }

        /// <summary>
        /// Get a credit card by Id
        /// </summary>
        /// <returns></returns>
        public async Task<CreditCard> GetCreditCardById (int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.CreditCards
                    .Include(c => c.Invoices)
                    .Include("Invoices.Movements")
                .SingleOrDefaultAsync(c => c.Id.Equals(id) && c.Enabled);
            }
        }

        /// <summary>
        /// Insert a credit card
        /// </summary>
        /// <param name="creditCard"></param>
        /// <returns></returns>
        public async Task Insert (CreditCard creditCard)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.CreditCards.Add(creditCard);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Update an existing credit card
        /// </summary>
        /// <param name="creditCard"></param>
        /// <returns></returns>
        public async Task Update (CreditCard creditCard)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(creditCard).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}