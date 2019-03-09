using System.Collections.Generic;

namespace PersonalFinances.Models.ViewModels
{
    public class MovementViewModel
    {
        public Movement Movement { get; set; }
        public IEnumerable<Account> Accounts { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Subcategory> Subcategories { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<CreditCard> CreditCards { get; set; }
    }
}