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
        public int OriginId { get; set; }

        public Account Target { get; set; }

        [Required]
        public int TargetId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "The minimum value is 0.01")]
        public double Value { get; set; }

        public double? Tax { get; set; }

        public DateTime InclusionDate { get; set; }
        public DateTime AccountingDate { get; set; }

        [Required]
        public MovementStatus TransferStatus { get; set; }

        public string Observation { get; set; }

        public double TotalValue
        {
            get
            {
                return Value - Tax.GetValueOrDefault();
            }
        }
    }
}