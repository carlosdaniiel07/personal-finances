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
        public virtual ICollection<Movement> Movements { get; set; } = new List<Movement>();
        public bool Enabled { get; set; }

        [Display(Name = "Monthly balance")]
        public double MonthlyBalance
        {
            get
            {
                return _service.BalanceOnMonth(Movements);
            }
        }

        [Display(Name = "Total credit")]
        public double TotalCredit
        {
            get
            {
                return Movements.TotalCredit();
            }
        }

        [Display(Name = "Total debit")]
        public double TotalDebit
        {
            get
            {
                return Movements.TotalDebit();
            }
        }

        private AccountService _service = new AccountService();
    }
}