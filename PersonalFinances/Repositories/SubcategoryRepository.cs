using PersonalFinances.Models;

using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace PersonalFinances.Repositories
{
    public class SubcategoryRepository
    {
        /// <summary>
        /// Get all subcategories
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Subcategory> GetSubcategories ()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Subcategories
                    .Include(s => s.Category)
                .Where(s => s.Enabled).ToList();
            }
        }
        
        /// <summary>
        /// Get a collection of Subcategory by name and by category base
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Subcategory> GetSubcategoriesByName (string name, int baseCategoryId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Subcategories
                    .Include(s => s.Category)
                .Where(s => s.Name.Equals(name) && s.Category.Id.Equals(baseCategoryId) && s.Enabled).ToList();
            }
        }

        /// <summary>
        /// Get subcategory by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Subcategory GetSubcategoryById (int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Subcategories
                    .Include(s => s.Category)
                    .Include(s => s.Movements)    
                .FirstOrDefault(s => s.Id.Equals(id) && s.Enabled);
            }
        }

        /// <summary>
        /// Get subcategory by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Subcategory GetSubcategoryByName (string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Subcategories.Where(s => s.Name.Equals(name) && s.Enabled).FirstOrDefault();
            }
        }

        /// <summary>
        /// Get subcategory by name and by base category
        /// </summary>
        /// <param name="name"></param>
        /// <param name="baseCategoryId"></param>
        /// <returns></returns>
        public Subcategory GetSubcategoryByName (string name, int baseCategoryId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Subcategories
                    .Include(s => s.Category)
                .Where(s => s.Category.Id.Equals(baseCategoryId) && s.Name.Equals(name) && s.Enabled).FirstOrDefault();
            }
        }

        /// <summary>
        /// A subcategory to be inserted
        /// </summary>
        /// <param name="subcategory"></param>
        public void Insert (Subcategory subcategory)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Subcategories.Add(subcategory);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update an existing subcategory
        /// </summary>
        /// <param name="subcategory"></param>
        public void Update (Subcategory subcategory)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(subcategory).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update a collection of subcategory
        /// </summary>
        /// <param name="subcategories"></param>
        public void Update (ICollection<Subcategory> subcategories)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                foreach (var subcategory in subcategories)
                    context.Entry(subcategory).State = EntityState.Modified;

                context.SaveChanges();
            }
        }
    }
}