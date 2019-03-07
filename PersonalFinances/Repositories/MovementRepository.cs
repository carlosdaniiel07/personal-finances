using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

using PersonalFinances.Models;
using PersonalFinances.Models.Enums;
using PersonalFinances.Models.ViewModels;

namespace PersonalFinances.Repositories
{
    public class MovementRepository
    {
        /// <summary>
        /// Get all movements
        /// </summary
        /// <returns></returns>
        public async Task<IEnumerable<Movement>> GetAllMovements()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Movements
                    .Include(m => m.Account)
                    .Include(m => m.Category)
                    .Include(m => m.Subcategory)
                    .Include(m => m.Project)
                    .Include(m => m.Invoice.CreditCard)
                .ToListAsync();
            }
        }

        /// <summary>
        /// Get all pending movements
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Movement>> GetAllPendingMovements ()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Movements
                    .Include(m => m.Account)
                    .Include(m => m.Category)
                    .Include(m => m.Subcategory)
                    .Include(m => m.Project)
                    .Include(m => m.Invoice.CreditCard)
                .Where(m => m.MovementStatus != MovementStatus.Launched).ToListAsync();
            }
        }

        /// <summary>
        /// Get all movements (filter by account and accounting date range)
        /// </summary
        /// <returns></returns>
        public async Task<IEnumerable<Movement>> GetAllMovements(BankStatementViewModel bankStatement)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                IQueryable<Movement> query = context.Movements
                    .Include(m => m.Account)
                    .Include(m => m.Category)
                    .Include(m => m.Subcategory)
                    .Include(m => m.Project)
                    .Include(m => m.Invoice.CreditCard)
                .Where(m => m.Account.Id.Equals(bankStatement.Account));

                if (bankStatement.Category.HasValue)
                    query = query.Where(m => m.Category.Id.Equals(bankStatement.Category.Value));
                if (bankStatement.Subcategory.HasValue)
                    query = query.Where(m => m.Subcategory.Id.Equals(bankStatement.Subcategory.Value));
                if (bankStatement.Project.HasValue)
                    query = query.Where(m => m.Project.Id.Equals(bankStatement.Project.Value));
                if (bankStatement.StartDate.HasValue)
                    query = query.Where(m => m.AccountingDate >= bankStatement.StartDate.Value);
                if (bankStatement.EndDate.HasValue)
                    query = query.Where(m => m.AccountingDate <= bankStatement.EndDate.Value);
                if (bankStatement.MovementStatus.HasValue)
                    query = query.Where(m => m.MovementStatus == bankStatement.MovementStatus.Value);

                return await query.ToListAsync();
            }
        }

        /// <summary>
        /// Get movements per account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Movement>> GetMovementsByAccount(int accountId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Movements.Where(m => m.Account.Id.Equals(accountId)).ToListAsync();
            }
        }

        /// <summary>
        /// Get movements per project
        /// </summary>
        /// <param name="projectIdId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Movement>> GetMovementsByProject (int projectId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Movements
                    .Include(m => m.Account)
                    .Include(m => m.Category)
                    .Include(m => m.Subcategory)
                    .Include(m => m.Project)
                .Where(m => m.Project.Id.Equals(projectId)).ToListAsync();
            }
        }

        /// <summary>
        /// Get a movement by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Movement> GetMovementById(int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Movements
                    .Include(m => m.Account)
                    .Include(m => m.Category)
                    .Include(m => m.Subcategory)
                    .Include(m => m.Project)
                    .Include(m => m.Invoice.CreditCard)
                .SingleOrDefaultAsync(m => m.Id.Equals(id));
            }
        }

        /// <summary>
        /// Insert a new movement
        /// </summary>
        /// <param name="movement"></param>
        public async Task Insert (Movement movement)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Movements.Add(movement);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Update an existing movement
        /// </summary>
        /// <param name="movement"></param>
        public async Task Update (Movement movement)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(movement).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Update a collection of movements
        /// </summary>
        /// <param name="movement"></param>
        public async Task Update (IEnumerable<Movement> movements)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                foreach (var movement in movements)
                    context.Entry(movement).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Delete a movement
        /// </summary>
        /// <param name="movement"></param>
        public async Task Remove (Movement movement)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(movement).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }
    }
}