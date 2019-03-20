using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using PersonalFinances.Models.Enums;

namespace PersonalFinances.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        
        [StringLength(8, MinimumLength = 8)]
        public string Reference { get; set; }

        [Display(Name = "Maturity date")]
        public DateTime MaturityDate { get; set; }

        [Display(Name = "Payment date")]
        public DateTime? PaymentDate { get; set; }

        public bool Closed { get; set; }

        [Display(Name = "Status")]
        public InvoiceStatus InvoiceStatus { get; set; }

        [Display(Name = "Credit card")]
        public CreditCard CreditCard { get; set; }

        [Required]
        public int CreditCardId { get; set; }

        public virtual ICollection<Movement> Movements { get; set; } = new List<Movement>();

        [Display(Name = "Total value")]
        public double TotalValue
        {
            get
            {
                return Movements.Sum(m => m.TotalValue);
            }
        }
    }
}