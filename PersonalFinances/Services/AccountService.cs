using System;
using System.Linq;
using System.Collections.Generic;

using PersonalFinances.Models;
using PersonalFinances.Repositories;
using PersonalFinances.Services.Exceptions;

namespace PersonalFinances.Services
{
    public class AccountService
    {
        private AccountRepository _repository = new AccountRepository();
        private MovementRepository _movementRepository = new MovementRepository();

        /// <summary>
        /// Get all accounts
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Account> GetAll ()
        {
            return _repository.GetAccounts();
        }

        /// <summary>
        /// Get account's movements
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IEnumerable<Movement> GetMovements (int accountId)
        {
            var account = GetById(accountId);
            return _movementRepository.GetMovementsByAccount(account.Id);
        }

        /// <summary>
        /// Insert a new account
        /// </summary>
        /// <param name="account"></param>
        public void Add (Account account)
        {
            account.Balance = account.InitialBalance;
            account.Enabled = true;

            var nameExists = _repository.GetAccountByName(account.Name) != null;

            if (!nameExists)
                _repository.Insert(account);
            else
                throw new AlreadyExistsException($"Already exists an account with the name {account.Name}");
        }

        /// <summary>
        /// Adjust the account balance
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="ammount"></param>
        public void AdjustBalance (int accountId, string movementType, double ammount)
        {
            if (movementType.Equals("C"))
                IncreaseBalance(accountId, ammount);
            else
                DecreaseBalance(accountId, ammount);
        }

        /// <summary>
        /// Increase the balance of an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="ammount"></param>
        private void IncreaseBalance (int accountId, double ammount)
        {
            var account = GetById(accountId);
            account.Balance += ammount;

            _repository.Update(account);
        }

        /// <summary>
        /// Decrease the balance of an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="ammount"></param>
        private void DecreaseBalance (int accountId, double ammount)
        {
            var account = GetById(accountId);
            account.Balance -= ammount;

            _repository.Update(account);
        }

        /// <summary>
        /// Update an existing accout
        /// </summary>
        /// <param name="account"></param>
        public void Update (Account account)
        {
            var currentAccount = GetById(account.Id);
            var quantity = _repository.GetAccountsByName(account.Name).Count(a => !a.Id.Equals(currentAccount.Id));

            account.Enabled = true;
            account.Balance = currentAccount.Balance;

            if (quantity.Equals(0))
                _repository.Update(account);
            else
                throw new AlreadyExistsException($"Already exists an account with the name {account.Name}");
        }

        /// <summary>
        /// Remove an account
        /// </summary>
        /// <param name="account"></param>
        public void Remove (int id)
        {
            var account = GetById(id);
            account.Enabled = false;

            _repository.Update(account);
        }

        /// <summary>
        /// Get account by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Account GetById (int id)
        {
            var account = _repository.GetAccountById(id);

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
        public string GetAccountNameById (int id)
        {
            var name = _repository.GetAccountNameById(id);

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

        /// <summary>
        /// Get total credit value from a Movement collection
        /// </summary>
        /// <param name="movements"></param>
        /// <returns></returns>
        public double TotalCrebit (IEnumerable<Movement> movements)
        {
            return movements.TotalCredit();
        }

        /// <summary>
        /// Get total debit value from a Movement collection
        /// </summary>
        /// <param name="movements"></param>
        /// <returns></returns>
        public double TotalDebit (IEnumerable<Movement> movements)
        {
            return movements.TotalDebit();
        }
    }
}