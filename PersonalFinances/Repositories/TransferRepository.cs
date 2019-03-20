using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

using PersonalFinances.Models;
using PersonalFinances.Models.Enums;

namespace PersonalFinances.Repositories
{
    public class TransferRepository
    {
        /// <summary>
        /// Get all transfers
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Transfer>> GetAllTransfers ()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Transfers
                    .Include(t => t.Origin)
                    .Include(t => t.Target)
                .ToListAsync();
            }
        }

        /// <summary>
        /// Get all pending transfers
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Transfer>> GetAllPendingTransfers()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Transfers
                    .Include(t => t.Origin)
                    .Include(t => t.Target)
                .Where(t => t.TransferStatus == MovementStatus.Pending).ToListAsync();
            }
        }

        /// <summary>
        /// Get a transfer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Transfer> GetTransferById (int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Transfers
                    .Include(t => t.Origin)
                    .Include(t => t.Target)
                .SingleOrDefaultAsync(t => t.Id.Equals(id));
            }
        }

        /// <summary>
        /// Insert a new transfer
        /// </summary>
        /// <param name="transfer"></param>
        public async Task Insert (Transfer transfer)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Transfers.Add(transfer);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Update an existing transfer
        /// </summary>
        /// <param name="transfer"></param>
        public async Task Update (Transfer transfer)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(transfer).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Delete an existing transfer
        /// </summary>
        /// <param name="id"></param>
        public async Task Delete (Transfer transfer)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(transfer).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }
    }
}