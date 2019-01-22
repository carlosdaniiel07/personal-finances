using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

using PersonalFinances.Models;

namespace PersonalFinances.Repositories
{
    public class TransferRepository
    {
        /// <summary>
        /// Get all transfers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Transfer> GetAllTransfers ()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Transfers
                    .Include(t => t.Origin)
                    .Include(t => t.Target)
                .ToList();
            }
        }

        /// <summary>
        /// Get a transfer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Transfer GetTransferById (int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Transfers
                    .Include(t => t.Origin)
                    .Include(t => t.Target)
                .SingleOrDefault(t => t.Id.Equals(id));
            }
        }

        /// <summary>
        /// Insert a new transfer
        /// </summary>
        /// <param name="transfer"></param>
        public void Insert (Transfer transfer)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Transfers.Add(transfer);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update an existing transfer
        /// </summary>
        /// <param name="transfer"></param>
        public void Update (Transfer transfer)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(transfer).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete an existing transfer
        /// </summary>
        /// <param name="id"></param>
        public void Delete (Transfer transfer)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(transfer).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}