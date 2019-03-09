using System.Collections.Generic;

namespace PersonalFinances.Models.ViewModels
{
    public class CreditCardViewModel
    {
        public CreditCard CreditCard { get; set; }
        public IEnumerable<string> AvailableDays { get; set; }
    }
}