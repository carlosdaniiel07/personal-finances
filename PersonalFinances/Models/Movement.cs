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
        [Range(0.01, double.MaxValue, ErrorMessage = "The minimum value is 0.01")]
        public double Value { get; set; }
        
        public double? Increase { get; set; }

        public double? Decrease { get; set; }

        [Display(Name = "Inclusion date")]
        public DateTime InclusionDate { get; set; }

        [Display(Name = "Accounting date")]
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

        public Project Project { get; set; }

        [Display(Name = "Project")]
        public int? ProjectId { get; set; }

        public Invoice Invoice { get; set; }

        [Display(Name = "Credit card")]
        public int? InvoiceId { get; set; }

        [Required]
        [Display(Name = "Status")]
        public MovementStatus MovementStatus { get; set; }

        [StringLength(100)]
        public string Observation { get; set; }

        public bool CanEdit { get; set; }

        [Display(Name = "Automatically launch")]
        public bool AutomaticallyLaunch { get; set; }

        [Display(Name = "Total value")]
        public double TotalValue
        {
            get
            {
                return Value + Increase.GetValueOrDefault() - Decrease.GetValueOrDefault();
            }
        }
    }
}