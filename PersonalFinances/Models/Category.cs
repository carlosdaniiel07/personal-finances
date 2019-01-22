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
        public ICollection<Subcategory> Subcategories { get; set; }
        public ICollection<Movement> Movements { get; set; } 

        public Category ()
        {
            Subcategories = new List<Subcategory>();
            Movements = new List<Movement>();
        }

        public double CategoryBalance
        {
            get
            {
                return _service.CategoryBalance(Movements);
            }
        }

        public double TotalCredit
        {
            get
            {
                return _service.TotalCredit(Movements);
            }
        }

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