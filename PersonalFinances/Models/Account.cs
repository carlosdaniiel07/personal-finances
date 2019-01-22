using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using PersonalFinances.Services;
using PersonalFinances.Models.Enums;

namespace PersonalFinances.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Account type")]
        public AccountType AccountType { get; set; }

        [Required]
        [Display(Name = "Initial balance")]
        public double InitialBalance { get; set; }

        public double Balance { get; set; }
        public ICollection<Movement> Movements { get; set; }
        public bool Enabled { get; set; }

        public Account ()
        {
            Movements = new List<Movement>();
        }

        public double MonthlyBalance
        {
            get
            {
                return _service.BalanceOnMonth(Movements);
            }
        }

        public double TotalCredit
        {
            get
            {
                return _service.TotalCrebit(Movements);
            }
        }

        public double TotalDebit
        {
            get
            {
                return _service.TotalDebit(Movements);
            }
        }

        private AccountService _service = new AccountService();
    }
}