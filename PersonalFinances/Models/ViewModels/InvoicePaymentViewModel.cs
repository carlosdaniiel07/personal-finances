using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PersonalFinances.Models.ViewModels
{
    public class InvoicePaymentViewModel
    {
        public Invoice Invoice { get; set; }

        [Display(Name = "Payment date")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Value to pay")]
        public double PaidValue { get; set; }

        [Display(Name = "Automatically launch")]
        public bool AutomaticallyLaunch { get; set; }

        [Display(Name = "Account")]
        public int AccountId { get; set; }

        public IEnumerable<Account> Accounts { get; set; }
    }
}