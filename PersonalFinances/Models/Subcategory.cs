using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinances.Models
{
    public class Subcategory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        public Category Category { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public bool Enabled { get; set; }

        public virtual ICollection<Movement> Movements { get; set; } = new List<Movement>();
    }
}