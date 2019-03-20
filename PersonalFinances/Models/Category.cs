using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using PersonalFinances.Services;

namespace PersonalFinances.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Category name")]
        public string Name { get; set; }

        [Required]
        [StringLength(1)]
        public string Type { get; set; }

        public bool Enabled { get; set; }

        public bool CanEdit { get; set; }

        public virtual ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
        public virtual ICollection<Movement> Movements { get; set; } = new List<Movement>();

        [Display(Name = "Category balance")]
        public double CategoryBalance
        {
            get
            {
                return _service.CategoryBalance(Movements);
            }
        }

        [Display(Name = "Total credit")]
        public double TotalCredit
        {
            get
            {
                return _service.TotalCredit(Movements);
            }
        }

        [Display(Name = "Total debit")]
        public double TotalDebit
        {
            get
            {
                return _service.TotalDebit(Movements);
            }
        }

        private CategoryService _service = new CategoryService();
    }
}