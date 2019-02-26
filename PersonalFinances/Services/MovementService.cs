using System;
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

        /// <summary>
        /// Add a new movement
        /// </summary>
        /// <param name="movement"></param>
        public async Task Add (Movement movement)
        {
            movement.Increase = movement.Increase ?? 0;
            movement.Decrease = movement.Decrease ?? 0;
            movement.InclusionDate = DateTime.Now;

            await _repository.Insert(movement);

            if (movement.MovementStatus.Equals(MovementStatus.Launched))
                await _accountService.AdjustBalance(movement.AccountId, movement.Type, movement.TotalValue);
        }

        /// <summary>
        /// Update an existing movement
        /// </summary>
        /// <param name="movement"></param>
        public async Task Update(Movement movement)
        {
            var oldMovement = await GetById(movement.Id);

            movement.Increase = movement.Increase ?? 0;
            movement.Decrease = movement.Decrease ?? 0;
            var dif = Math.Abs(movement.TotalValue - oldMovement.TotalValue);

            if(movement.MovementStatus.Equals(MovementStatus.Launched))
            {
                if(movement.AccountId.Equals(oldMovement.Account.Id))
                {
                    if (oldMovement.MovementStatus.Equals(MovementStatus.Pending))
                    {
                        await _accountService.AdjustBalance(movement.AccountId, movement.Type, movement.TotalValue);
                    }
                    else
                    {
                        string operation;

                        if (movement.Type.Equals("C"))
                            operation = (movement.TotalValue > oldMovement.TotalValue) ? "C" : "D";
                        else
                            operation = (movement.TotalValue > oldMovement.TotalValue) ? "D" : "C";

                        await _accountService.AdjustBalance(movement.AccountId, operation, dif);
                    }
                }
                else
                {
                    if (oldMovement.MovementStatus.Equals(MovementStatus.Launched))
                        await _accountService.AdjustBalance(oldMovement.Account.Id, GetInverseOperation(movement.Type), oldMovement.TotalValue);

                    await _accountService.AdjustBalance(movement.AccountId, movement.Type, movement.TotalValue);
                }
            }
            else
            {
                if(oldMovement.MovementStatus.Equals(MovementStatus.Launched))
                    await _accountService.AdjustBalance(oldMovement.AccountId, GetInverseOperation(oldMovement.Type), oldMovement.TotalValue);
            }
            
            await _repository.Update(movement);
        }

        /// <summary>
        /// Delete a movement by Id
        /// </summary>
        /// <param name="id"></param>
        public async Task Delete (int id)
        {
            var movement = await GetById(id);

            if (movement.MovementStatus.Equals(MovementStatus.Launched))
                await _accountService.AdjustBalance(movement.Account.Id, GetInverseOperation(movement.Type), movement.TotalValue);

            await _repository.Remove(movement);
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
        public async Task<IEnumerable<Movement>> GetAll (BankStatementViewModel bankStatement)
        {
            return await _repository.GetAllMovements(bankStatement);
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