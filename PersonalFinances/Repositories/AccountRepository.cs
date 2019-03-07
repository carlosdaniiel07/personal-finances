using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using PersonalFinances.Models;

namespace PersonalFinances.Repositories
{
    public class AccountRepository
    {
        /// <summary>
        /// Get all accounts
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Account>> GetAccounts ()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Accounts
                    .Include(a => a.Movements)
                    .Include("Movements.Category")
                .Where(c => c.Enabled).ToListAsync();
            }
        }

        /// <summary>
        /// Get an account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Account> GetAccountById (int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Accounts
                    .Include(a => a.Movements)
                    .Include("Movements.Category")
                    .Include("Movements.Invoice.CreditCard")
                .SingleOrDefaultAsync(a => a.Id.Equals(id) && a.Enabled);
            }
        }

        /// <summary>
        /// Get an account name by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetAccountNameById (int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Accounts.Where(a => a.Id.Equals(id)).Select(a => a.Name).FirstAsync();
            }
        }

        /// <summary>
        /// Get a collection of accounts by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ICollection<Account>> GetAccountsByName (string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Accounts.Where(a => a.Name.Equals(name) && a.Enabled).ToListAsync();
            }
        }

        /// <summary>
        /// Get an account by Name
        /// </summary>
        /// <param name="name">The account name</param>
        /// <returns></returns>
        public async Task<Account> GetAccountByName(string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Accounts.Where(a => a.Name.Equals(name) && a.Enabled).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Insert a new account
        /// </summary>
        /// <param name="account">An account to be inserted</param>
        public async Task Insert(Account account)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Accounts.Add(account);
                await context.SaveChangesAsync();
            }

        }

        /// <summary>
        /// Update an existing account
        /// </summary>
        /// <param name="account"></param>
        public async Task Update (Account account)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(account).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Update collection of account
        /// </summary>
        /// <param name="account"></param>
        public async Task Update (IEnumerable<Account> accounts)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                foreach (var account in accounts)
                    context.Entry(account).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}