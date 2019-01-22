using System;
using System.Collections.Generic;

using PersonalFinances.Models;
using PersonalFinances.Models.Enums;
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
        public void Add (Movement movement)
        {
            movement.Increase = movement.Increase ?? 0;
            movement.Decrease = movement.Decrease ?? 0;
            movement.InclusionDate = DateTime.Now;

            _repository.Insert(movement);

            if (movement.MovementStatus.Equals(MovementStatus.Launched))
                _accountService.AdjustBalance(movement.AccountId, movement.Type, movement.TotalValue);
        }

        /// <summary>
        /// Update an existing movement
        /// </summary>
        /// <param name="movement"></param>
        public void Update(Movement movement)
        {
            var oldMovement = GetById(movement.Id);

            movement.Increase = movement.Increase ?? 0;
            movement.Decrease = movement.Decrease ?? 0;
            var dif = Math.Abs(movement.TotalValue - oldMovement.TotalValue);

            if(movement.MovementStatus.Equals(MovementStatus.Launched))
            {
                if(movement.AccountId.Equals(oldMovement.Account.Id))
                {
                    if (oldMovement.MovementStatus.Equals(MovementStatus.Pending))
                    {
                        _accountService.AdjustBalance(movement.AccountId, movement.Type, movement.TotalValue);
                    }
                    else
                    {
                        string operation;

                        if (movement.Type.Equals("C"))
                            operation = (movement.TotalValue > oldMovement.TotalValue) ? "C" : "D";
                        else
                            operation = (movement.TotalValue > oldMovement.TotalValue) ? "D" : "C";

                        _accountService.AdjustBalance(movement.AccountId, operation, dif);
                    }
                }
                else
                {
                    if (oldMovement.MovementStatus.Equals(MovementStatus.Launched))
                    {
                        var operation = (movement.Type.Equals("C")) ? "D" : "C";
                        _accountService.AdjustBalance(oldMovement.Account.Id, operation, oldMovement.TotalValue);
                    }

                    _accountService.AdjustBalance(movement.AccountId, movement.Type, movement.TotalValue);
                }
            }
            else
            {
                if(oldMovement.MovementStatus.Equals(MovementStatus.Launched))
                {
                    var operation = (oldMovement.Type.Equals("C")) ? "D" : "C";
                    _accountService.AdjustBalance(oldMovement.AccountId, operation, oldMovement.TotalValue);
                }
            }
            
            _repository.Update(movement);
        }

        /// <summary>
        /// Delete a movement by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete (int id)
        {
            var movement = GetById(id);

            if (movement.MovementStatus.Equals(MovementStatus.Launched))
            {
                string operation = (movement.Type.Equals("C")) ? "D" : "C";
                _accountService.AdjustBalance(movement.Account.Id, operation, movement.TotalValue);
            }

            _repository.Remove(movement);
        }

        /// <summary>
        /// Get all movements
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Movement> GetAll ()
        {
            return _repository.GetAllMovements();
        }

        /// <summary>
        /// Get a movement by Id
        /// </summary>
        /// <returns></returns>
        public Movement GetById (int id)
        {
            var movement = _repository.GetMovementById(id);

            if (movement != null)
                return movement;
            else
                throw new NotFoundException("This movement not exists");
        }
    }
}