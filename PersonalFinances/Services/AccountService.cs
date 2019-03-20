using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using PersonalFinances.Models;
using PersonalFinances.Repositories;
using PersonalFinances.Services.Exceptions;

namespace PersonalFinances.Services
{
    public class AccountService
    {
        private AccountRepository _repository = new AccountRepository();
        private MovementService _movementService;

        /// <summary>
        /// Get all accounts
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Account>> GetAll ()
        {
            return await _repository.GetAccounts();
        }

        /// <summary>
        /// Insert a new account
        /// </summary>
        /// <param name="account"></param>
        public async Task Add (Account account)
        {
            account.Balance = account.InitialBalance;
            account.Enabled = true;

            var nameExists = await _repository.GetAccountByName(account.Name) != null;

            if (!nameExists)
                await _repository.Insert(account);
            else
                throw new AlreadyExistsException($"Already exists an account with the name {account.Name}");
        }

        /// <summary>
        /// Adjust the account balance
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="ammount"></param>
        public async Task AdjustBalance (int accountId)
        {
            var account = await _repository.GetAccountById(accountId);
            var newBalance = (account.InitialBalance + account.TotalCredit) - account.TotalDebit;

            account.Balance = newBalance;

            await _repository.Update(account);
        }

        /// <summary>
        /// Adjust the balance of a collection of accounts
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        public async Task AdjustBalance (IEnumerable<Account> accounts)
        {
            var accountsCollection = new List<Account>();
            _movementService = new MovementService();

            foreach (var account in accounts.Distinct())
            {
                var obj = await GetById(account.Id);
                obj.Balance = (obj.InitialBalance + obj.TotalCredit) - obj.TotalDebit;
                accountsCollection.Add(obj);
            }

            await _repository.Update(accountsCollection);
        }

        /// <summary>
        /// Increase the balance of an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="ammount"></param>
        private async Task IncreaseBalance (int accountId, double ammount)
        {
            var account = await GetById(accountId);
            account.Balance += ammount;

            await _repository.Update(account);
        }

        /// <summary>
        /// Decrease the balance of an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="ammount"></param>
        private async Task DecreaseBalance (int accountId, double ammount)
        {
            var account = await GetById(accountId);
            account.Balance -= ammount;

            await _repository.Update(account);
        }

        /// <summary>
        /// Update an existing accout
        /// </summary>
        /// <param name="account"></param>
        public async Task Update (Account account)
        {
            var currentAccount = await GetById(account.Id);
            var quantity = (await _repository.GetAccountsByName(account.Name)).Count(a => !a.Id.Equals(currentAccount.Id));

            account.Enabled = true;
            account.Balance = currentAccount.Balance;

            if (quantity.Equals(0))
                await _repository.Update(account);
            else
                throw new AlreadyExistsException($"Already exists an account with the name {account.Name}");
        }

        /// <summary>
        /// Remove an account
        /// </summary>
        /// <param name="account"></param>
        public async Task Remove (int id)
        {
            var account = await GetById(id);
            account.Enabled = false;

            await _repository.Update(account);
        }

        /// <summary>
        /// Get account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Account> GetById (int id)
        {
            var account = await _repository.GetAccountById(id);

            if (account != null)
                return account;
            else
                throw new NotFoundException("This account not exists");
        }

        /// <summary>
        /// Get an account name by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetAccountNameById (int id)
        {
            var name = await _repository.GetAccountNameById(id);

            if (name != null)
                return name;
            else
                throw new NotFoundException("This account not exists");
        }

        /// <summary>
        /// Get account monthly balance
        /// </summary>
        /// <param name="movements"></param>
        /// <returns></returns>
        public double BalanceOnMonth (IEnumerable<Movement> movements)
        {
            Func<Movement, bool> condition = (m) =>
            {
                return m.AccountingDate.Month.Equals(DateTime.Now.Month) && m.AccountingDate.Year.Equals(DateTime.Now.Year);
            };

            movements = movements.Where(condition);
            double totalCredit = movements.TotalCredit();
            double totalDebit = movements.TotalDebit();

            return totalCredit - totalDebit;
        }
    }
}