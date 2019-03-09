using System;
using System.ComponentModel.DataAnnotations;

using PersonalFinances.Models.Enums;

namespace PersonalFinances.Models
{
    public class Transfer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        public Account Origin { get; set; }

        [Required]
        [Display(Name = "Origin account")]
        public int OriginId { get; set; }

        public Account Target { get; set; }

        [Required]
        [Display(Name = "Target account")]
        public int TargetId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "The minimum value is 0.01")]
        public double Value { get; set; }

        public double? Tax { get; set; }

        [Display(Name = "Inclusion date")]
        public DateTime InclusionDate { get; set; }

        [Display(Name = "Accounting date")]
        public DateTime AccountingDate { get; set; }

        [Required]
        [Display(Name = "Status")]
        public MovementStatus TransferStatus { get; set; }

        [Display(Name = "Automatically launch")]
        public bool AutomaticallyLaunch { get; set; }

        public string Observation { get; set; }

        [Display(Name = "Total value")]
        public double TotalValue
        {
            get
            {
                return Value - Tax.GetValueOrDefault();
            }
        }
    }
}