using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using PersonalFinances.Models;

namespace PersonalFinances.Repositories
{
    public class CategoryRepository
    {
        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetCategories ()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var categories = await context.Categories
                    .Include(c => c.Subcategories)
                    .Include(c => c.Movements)
                .Where(c => c.Enabled).ToListAsync();

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
        public async Task<Category> GetCategoryById (int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var category = await context.Categories
                    .Include(c => c.Subcategories)
                    .Include(c => c.Movements)
                .SingleOrDefaultAsync(c => c.Id.Equals(id) && c.Enabled);

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
        public async Task<Category> GetCategoryByName (string name, string type)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Categories
                    .Include(c => c.Subcategories)
                    .Include(c => c.Movements)
                .SingleOrDefaultAsync(c => c.Name.Equals(name) && c.Type.Equals(type) && c.Enabled);
            }
        }

        /// <summary>
        /// Get a collection of categories by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ICollection<Category>> GetCategoriesByName(string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Categories
                    .Include(c => c.Subcategories)
                .Where(c => c.Name.Equals(name) && c.Enabled).ToListAsync();
            }
        }

        /// <summary>
        /// Insert a category
        /// </summary>
        /// <param name="category"></param>
        public async Task Insert (Category category)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Categories.Add(category);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        /// <param name="category"></param>
        public async Task Update (Category category)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(category).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}