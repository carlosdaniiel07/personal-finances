using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

using PersonalFinances.Models.Enums;

namespace PersonalFinances.Models.ViewModels
{
    public class BankStatementViewModel
    {
        public int Account { get; set; }
        public int? Category { get; set; }
        public int? Subcategory { get; set; }
        public int? Project { get; set; }
        
        [Display(Name = "Start date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Status")]
        public MovementStatus? MovementStatus { get; set; }

        public IEnumerable<Account> Accounts { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Subcategory> Subcategories { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Movement> Movements { get; set; } = new List<Movement>();
    }
}