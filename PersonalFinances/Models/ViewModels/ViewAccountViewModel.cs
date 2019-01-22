using System.Collections.Generic;

namespace PersonalFinances.Models.ViewModels
{
    public class ViewAccountViewModel
    {
        public Account Account { get; set; }
        public IEnumerable<Movement> AccountMovements { get; set; }
    }
}