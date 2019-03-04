using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

using PersonalFinances.Models;

namespace PersonalFinances.Repositories
{
    public class InvoiceRepository
    {
        /// <summary>
        /// Insert a new Invoice
        /// </summary>
        /// <param name="invoice"></param>
        public void Insert (Invoice invoice)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Invoices.Add(invoice);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Insert an exsting Invoice
        /// </summary>
        /// <param name="invoice"></param>
        public async Task Update (Invoice invoice)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(invoice).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Get an invoice by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Invoice> GetInvoiceById (int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Invoices
                    .Include(i => i.CreditCard)
                    .Include(i => i.Movements)
                    .Include("Movements.Category")
                    .Include("Movements.Subcategory")
                    .Include("Movements.Project")
                .SingleOrDefaultAsync(i => i.Id.Equals(id));
            }
        }
    }
}