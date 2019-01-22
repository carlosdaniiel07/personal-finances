using System;
using System.Collections.Generic;
using System.Linq;

using PersonalFinances.Models;
using PersonalFinances.Models.Enums;
using PersonalFinances.Services.Exceptions;
using PersonalFinances.Repositories;

namespace PersonalFinances.Services
{
    public class TransferService
    {
        private TransferRepository _repository = new TransferRepository();
        private MovementService _movementService = new MovementService();
        private CategoryService _categoryService = new CategoryService();
        private AccountService _accountService = new AccountService();

        /// <summary>
        /// Get all transfers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Transfer> GetAll ()
        {
            return _repository.GetAllTransfers();
        }

        /// <summary>
        /// Add a new transfer
        /// </summary>
        /// <param name="transfer"></param>
        public void Add (Transfer transfer)
        {
            transfer.Tax = transfer.Tax ?? 0;
            transfer.InclusionDate = DateTime.Now;

            if (transfer.OriginId.Equals(transfer.TargetId))
                throw new NotValidOperationException("You cannot transfer values between same accounts");

            if (transfer.TransferStatus.Equals(MovementStatus.Launched))
            {
                _movementService.Add(CreateTransferMovement(transfer, "C"));
                _movementService.Add(CreateTransferMovement(transfer, "D"));
            }

            _repository.Insert(transfer);
        }

        /// <summary>
        /// Cancel an existing transfer
        /// </summary>
        public void Cancel (int id)
        {
            var transfer = GetById(id);

            if (transfer.TransferStatus.Equals(MovementStatus.Launched))
            {
                _movementService.Add(CreateTransferMovement(transfer, "C", true));
                _movementService.Add(CreateTransferMovement(transfer, "D", true));
            }

            _repository.Delete(transfer);
        }

        /// <summary>
        /// Launch a pending transfer
        /// </summary>
        public void Launch (int id)
        {
            var transfer = GetById(id);

            if (transfer.TransferStatus.Equals(MovementStatus.Pending))
            {
                transfer.TransferStatus = MovementStatus.Launched;

                _movementService.Add(CreateTransferMovement(transfer, "C"));
                _movementService.Add(CreateTransferMovement(transfer, "D"));

                _repository.Update(transfer);
            }
        }

        /// <summary>
        /// Get a transfer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Transfer GetById (int id)
        {
            var transfer = _repository.GetTransferById(id);

            if (transfer != null)
                return transfer;
            else
                throw new NotFoundException("This transfer not exists");
        }

        /// <summary>
        /// Create a object Movement from a Transfer
        /// </summary>
        /// <param name="transfer"></param>
        /// <param name="type"></param>
        /// <param name="categoryId"></param>
        /// <param name="subcategoryId"></param>
        /// <returns></returns>
        private Movement CreateTransferMovement (Transfer transfer, string type)
        {
            var category = _categoryService.GetByName("Transfer", type);
            var subcategory = category.Subcategories.First();
            var accountName = _accountService.GetAccountNameById((type.Equals("C") ? transfer.OriginId : transfer.TargetId));
            accountName = accountName.Substring(0, (accountName.Length >= 27) ? 27 : accountName.Length).Trim();

            return new Movement
            {
                Type = type,
                Description = (type.Equals("C")) ? $"Transfer received from {accountName}" : $"Transfer sent to {accountName}",
                Value = (type.Equals("C")) ? transfer.Value : transfer.TotalValue,
                AccountingDate = transfer.AccountingDate,
                AccountId = (type.Equals("C")) ? transfer.TargetId : transfer.OriginId,
                CategoryId = category.Id,
                SubcategoryId = subcategory.Id,
                MovementStatus = transfer.TransferStatus,
                Observation = transfer.Observation
            };
        }

        /// <summary>
        /// Create a object Movement from a Transfer
        /// </summary>
        /// <param name="transfer"></param>
        /// <param name="type"></param>
        /// <param name="categoryId"></param>
        /// <param name="subcategoryId"></param>
        /// <param name="reversal"></param>
        /// <returns></returns>
        private Movement CreateTransferMovement(Transfer transfer, string type, bool reversal)
        {
            var category = _categoryService.GetByName("Transfer", type);
            var subcategory = category.Subcategories.First();
            var accountName = _accountService.GetAccountNameById((type.Equals("C") ? transfer.TargetId : transfer.OriginId));
            accountName = accountName.Substring(0, (accountName.Length >= 15) ? 15 : accountName.Length).Trim();

            return new Movement
            {
                Type = type,
                Description = (type.Equals("C") && reversal) ? $"Reversal of transfer sent to {accountName}" : $"Reversal of transfer received from {accountName}",
                Value = (type.Equals("C") && reversal) ? transfer.TotalValue : transfer.Value,
                AccountingDate = transfer.AccountingDate,
                AccountId = (type.Equals("C") && reversal) ? transfer.OriginId : transfer.TargetId,
                CategoryId = category.Id,
                SubcategoryId = subcategory.Id,
                MovementStatus = transfer.TransferStatus,
                Observation = transfer.Observation
            };
        }
    }
}