﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using PersonalFinances.Models;
using PersonalFinances.Models.Enums;
using PersonalFinances.Models.ViewModels;
using PersonalFinances.Repositories;
using PersonalFinances.Services.Exceptions;

namespace PersonalFinances.Services
{
    public class MovementService
    {
        private MovementRepository _repository = new MovementRepository();
        private AccountService _accountService = new AccountService();
        private CreditCardService _creditCardService = new CreditCardService();
        private InvoiceService _invoiceService = new InvoiceService();

        /// <summary>
        /// Add a new movement
        /// </summary>
        /// <param name="movement"></param>
        public async Task Add (Movement movement)
        {
            movement.Increase = movement.Increase ?? 0;
            movement.Decrease = movement.Decrease ?? 0;
            movement.InclusionDate = DateTime.Now;
            movement.CanEdit = true;

            var creditCardId = movement.InvoiceId;

            if (creditCardId.HasValue)
            {
                var creditCard = await _creditCardService.GetById(creditCardId.Value);

                _creditCardService.CanBeUsed(creditCard, movement);

                try
                {
                    movement.InvoiceId = (await _creditCardService.GetInvoiceByAccountingDate(creditCard.Id, movement.AccountingDate)).Id;
                }
                catch (InvoiceNotFoundException)
                {
                    var createdInvoice = _invoiceService.CreateInvoiceObject(creditCard, movement.AccountingDate);
                    _invoiceService.Insert(createdInvoice);
                    movement.InvoiceId = (await _creditCardService.GetInvoiceByAccountingDate(creditCard.Id, movement.AccountingDate)).Id;
                }

                movement.MovementStatus = MovementStatus.Pending;
            }

            await _repository.Insert(movement);

            if (movement.MovementStatus.Equals(MovementStatus.Launched) && !movement.InvoiceId.HasValue)
                await _accountService.AdjustBalance(movement.AccountId);
        }

        /// <summary>
        /// Update an existing movement
        /// </summary>
        /// <param name="movement"></param>
        public async Task Update (Movement movement)
        {
            var oldMovement = await GetById(movement.Id);

            if (oldMovement.CanEdit)
            {
                movement.Increase = movement.Increase ?? 0;
                movement.Decrease = movement.Decrease ?? 0;
                movement.CanEdit = oldMovement.CanEdit;

                var creditCardId = movement.InvoiceId;

                if (creditCardId.HasValue)
                {
                    var creditCard = await _creditCardService.GetById(creditCardId.Value);

                    _creditCardService.CanBeUsed(creditCard, movement);

                    try
                    {
                        var invoiceId = (await _creditCardService.GetInvoiceByAccountingDate(creditCard.Id, movement.AccountingDate)).Id;
                        movement.InvoiceId = invoiceId;
                    }
                    catch (InvoiceNotFoundException)
                    {
                        var createdInvoice = _invoiceService.CreateInvoiceObject(creditCard, movement.AccountingDate);
                        _invoiceService.Insert(createdInvoice);
                        movement.InvoiceId = _creditCardService.GetInvoiceByAccountingDate(creditCard.Id, movement.AccountingDate).Id;
                    }

                    movement.MovementStatus = MovementStatus.Pending;
                }

                await _repository.Update(movement);

                if (!oldMovement.Account.Id.Equals(movement.AccountId))
                    await _accountService.AdjustBalance(oldMovement.Account.Id);
                await _accountService.AdjustBalance(movement.AccountId);
            }
            else
            {
                throw new NotValidOperationException("This movement was generated by another routine and cannot be changed");
            }
        }

        /// <summary>
        /// Update a collection of movements
        /// </summary>
        /// <param name="movements"></param>
        /// <returns></returns>
        public async Task Update (IEnumerable<Movement> movements, bool adjustAccountsBalance)
        {
            await _repository.Update(movements);

            if (adjustAccountsBalance)
                await _accountService.AdjustBalance(movements.Select(m => m.Account));
        }

        /// <summary>
        /// Delete a movement by Id
        /// </summary>
        /// <param name="id"></param>
        public async Task Delete (int id)
        {
            var movement = await GetById(id);

            await _repository.Remove(movement);

            if (movement.MovementStatus.Equals(MovementStatus.Launched))
                await _accountService.AdjustBalance(movement.AccountId);
        }

        /// <summary>
        /// Update status of all movements that already is expired
        /// </summary>
        /// <returns></returns>
        public async Task UpdateMovementsStatusOfPendingMovements ()
        {
            var movements = await _repository.GetAllPendingMovements();

            var pendingMovements = movements.Where(m => m.AccountingDate.CompareTo(DateTime.Today) < 0 && m.Invoice == null
                && m.AutomaticallyLaunch)
            .Select((m) =>
            {
                m.MovementStatus = MovementStatus.Launched;
                return m;
            }).ToList();

            await Update(pendingMovements, true);
        }

        /// <summary>
        /// Get all movements
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Movement>> GetAll ()
        {
            return await _repository.GetAllMovements();
        }

        /// <summary>
        /// Get all movements (filter by account and accounting date range)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Movement>> GetAll(BankStatementViewModel bankStatement)
        {
            return await _repository.GetAllMovements(bankStatement);
        }

        /// <summary>
        /// Get all movements from a specified account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Movement>> GetAll (int accountId)
        {
            return await _repository.GetMovementsByAccount(accountId);
        }

        /// <summary>
        /// Get a movement by Id
        /// </summary>
        /// <returns></returns>
        public async Task<Movement> GetById (int id)
        {
            var movement = await _repository.GetMovementById(id);

            if (movement != null)
                return movement;
            else
                throw new NotFoundException("This movement not exists");
        }

        /// <summary>
        /// Get inverse operation (credit or debit) based in a movement type
        /// </summary>
        /// <returns></returns>
        private string GetInverseOperation (string movementType)
        {
            return (movementType.Equals("C")) ? "D" : "C";
        }
    }
}