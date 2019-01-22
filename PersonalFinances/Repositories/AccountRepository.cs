using PersonalFinances.Models;

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PersonalFinances.Repositories
{
    public class AccountRepository
    {
        /// <summary>
        /// Get all accounts
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Account> GetAccounts ()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Accounts
                    .Include(a => a.Movements)    
                .Where(c => c.Enabled).ToList();
            }
        }

        /// <summary>
        /// Get an account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Account GetAccountById (int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Accounts
                    .Include(a => a.Movements)
                .SingleOrDefault(a => a.Id.Equals(id) && a.Enabled);
            }
        }

        /// <summary>
        /// Get an account name by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetAccountNameById (int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Accounts.Where(a => a.Id.Equals(id)).Select(a => a.Name).First();
            }
        }

        /// <summary>
        /// Get a collection of accounts by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ICollection<Account> GetAccountsByName (string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Accounts.Where(a => a.Name.Equals(name) && a.Enabled).ToList();
            }
        }

        /// <summary>
        /// Get an account by Name
        /// </summary>
        /// <param name="name">The account name</param>
        /// <returns></returns>
        public Account GetAccountByName(string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Accounts.Where(a => a.Name.Equals(name) && a.Enabled).FirstOrDefault();
            }
        }

        /// <summary>
        /// Insert a new account
        /// </summary>
        /// <param name="account">An account to be inserted</param>
        public void Insert (Account account)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Accounts.Add(account);
                context.SaveChanges();
            }

        } 

        /// <summary>
        /// Update an existing account
        /// </summary>
        /// <param name="account"></param>
        public void Update (Account account)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(account).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}