using PersonalFinances.Models;

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PersonalFinances.Repositories
{
    public class CategoryRepository
    {
        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> GetCategories ()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var categories = context.Categories
                    .Include(c => c.Subcategories)
                    .Include(c => c.Movements)
                .Where(c => c.Enabled).ToList();

                foreach (var category in categories)
                    category.Subcategories = category.Subcategories.Where(s => s.Enabled).ToList();

                return categories;
            }
        }

        /// <summary>
        /// Get a category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Category GetCategoryById (int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var category =  context.Categories
                    .Include(c => c.Subcategories)
                    .Include(c => c.Movements)
                .SingleOrDefault(c => c.Id.Equals(id) && c.Enabled);

                if (category != null)
                    category.Subcategories = category.Subcategories.Where(s => s.Enabled).ToList();

                return category;
            }
        }

        /// <summary>
        /// Get a category by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Category GetCategoryByName (string name, string type)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Categories
                    .Include(c => c.Subcategories)
                    .Include(c => c.Movements)
                .SingleOrDefault(c => c.Name.Equals(name) && c.Type.Equals(type) && c.Enabled);
            }
        }

        /// <summary>
        /// Get a collection of categories by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ICollection<Category> GetCategoriesByName(string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Categories
                    .Include(c => c.Subcategories)
                .Where(c => c.Name.Equals(name) && c.Enabled).ToList();
            }
        }

        /// <summary>
        /// Insert a category
        /// </summary>
        /// <param name="category"></param>
        public void Insert (Category category)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        /// <param name="category"></param>
        public void Update (Category category)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(category).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}