using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

using PersonalFinances.Services;

namespace PersonalFinances.Models
{
    public class CreditCard
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Credit card name")]
        public string Name { get; set; }

        [Display(Name = "Invoice closure")]
        [Required]
        [StringLength(2)]
        public string InvoiceClosure { get; set; }

        [Display(Name = "Pay day")]
        [Required]
        [StringLength(2)]
        public string PayDay { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "The minimum value is 0.01")]
        public double Limit { get; set; }

        public bool Enabled { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

        [Display(Name = "Total debit")]
        public double TotalDebit
        {
            get
            {
                return Invoices.Sum(i => i.TotalValue);
            }
        }

        [Display(Name = "Remaining limit")]
        public double RemainingLimit
        {
            get
            {
                return Limit - Invoices.Where(i => i.InvoiceStatus != Enums.InvoiceStatus.Paid).Sum(i => i.TotalValue);
            }
        }

        public Invoice NextInvoiceToPay
        {
            get
            {
                return _service.GeNextInvoiceToPay(this);
            }
        }

        private CreditCardService _service = new CreditCardService();
    }
}