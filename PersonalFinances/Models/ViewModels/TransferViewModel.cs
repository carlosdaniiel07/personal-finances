using System.Collections.Generic;

namespace PersonalFinances.Models.ViewModels
{
    public class TransferViewModel
    {
        public Transfer Transfer { get; set; }
        public IEnumerable<Account> Accounts { get; set; }
    }
}