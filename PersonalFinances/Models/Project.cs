using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using PersonalFinances.Models.Enums;

namespace PersonalFinances.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Project name")]
        public string Name { get; set; }

        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Finish date")]
        public DateTime? FinishDate { get; set; }

        public double? Budget { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Display(Name = "Status")]
        public ProjectStatus ProjectStatus { get; set; }

        public bool Enabled { get; set; }

        public virtual ICollection<Movement> Movements { get; set; } = new List<Movement>();

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

        [Display(Name = "Project balance")]
        public double ProjectBalance
        {
            get
            {
                return TotalCredit - TotalDebit;
            }
        }

        [Display(Name = "Left budget")]
        public double LeftBudget
        {
            get
            {
                return (Budget.HasValue) ? Budget.Value - TotalDebit : 0; 
            }
        }

        [Display(Name = "Bugdet used (percentage)")]
        public double BudgetUsedPercentage
        {
            get
            {
                return (Budget.HasValue) ? (TotalDebit / Budget.Value) * 100 : 0;
            }
        }
    }
}