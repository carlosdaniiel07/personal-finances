using System;
using System.ComponentModel.DataAnnotations;

using PersonalFinances.Models.Enums;

namespace PersonalFinances.Models
{
    public class Movement
    {
        public int Id { get; set; }

        [StringLength(1)]
        [Required]
        public string Type { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Required]
        public double Value { get; set; }
        
        public double? Increase { get; set; }

        public double? Decrease { get; set; }

        public DateTime InclusionDate { get; set; }

        public DateTime AccountingDate { get; set; }

        public Account Account { get; set; }

        [Required]
        [Display(Name = "Account")]
        public int AccountId { get; set; }

        public Category Category { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public Subcategory Subcategory { get; set; }

        [Required]
        [Display(Name = "Subcategory")]
        public int SubcategoryId { get; set; }

        [Required]
        public MovementStatus MovementStatus { get; set; }

        [StringLength(100)]
        public string Observation { get; set; }

        public double TotalValue
        {
            get
            {
                return Value + Increase.GetValueOrDefault() - Decrease.GetValueOrDefault();
            }
        }
    }
}