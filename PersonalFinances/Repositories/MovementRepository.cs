using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

using PersonalFinances.Models;

namespace PersonalFinances.Repositories
{
    public class MovementRepository
    {
        /// <summary>
        /// Get all movements
        /// </summary
        /// <returns></returns>
        public IEnumerable<Movement> GetAllMovements()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Movements
                    .Include(m => m.Account)
                    .Include(m => m.Category)
                    .Include(m => m.Subcategory)
                .ToList();
            }
        }

        /// <summary>
        /// Get movements per account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IEnumerable<Movement> GetMovementsByAccount(int accountId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Movements
                    .Include(m => m.Account)
                    .Include(m => m.Category)
                    .Include(m => m.Subcategory)
                .Where(m => m.Account.Id.Equals(accountId)).ToList();
            }
        }

        /// <summary>
        /// Get a movement by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Movement GetMovementById(int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Movements
                    .Include(m => m.Account)
                    .Include(m => m.Category)
                    .Include(m => m.Subcategory)
                .SingleOrDefault(m => m.Id.Equals(id));
            }
        }

        /// <summary>
        /// Insert a new movement
        /// </summary>
        /// <param name="movement"></param>
        public void Insert (Movement movement)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Movements.Add(movement);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update an existing movement
        /// </summary>
        /// <param name="movement"></param>
        public void Update (Movement movement)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(movement).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete a movement
        /// </summary>
        /// <param name="movement"></param>
        public void Remove (Movement movement)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(movement).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}