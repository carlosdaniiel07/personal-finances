using System.Collections.Generic;

namespace PersonalFinances.Models.ViewModels
{
    public class SubcategoryViewModel
    {
        public Subcategory Subcategory { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}