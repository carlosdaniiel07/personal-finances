using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

using PersonalFinances.Models;

namespace PersonalFinances.Repositories
{
    public class SubcategoryRepository
    {
        /// <summary>
        /// Get all subcategories
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Subcategory>> GetSubcategories ()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Subcategories
                    .Include(s => s.Category)
                .Where(s => s.Enabled).ToListAsync();
            }
        }
        
        /// <summary>
        /// Get a collection of Subcategory by name and by category base
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Subcategory>> GetSubcategoriesByName (string name, int baseCategoryId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Subcategories
                    .Include(s => s.Category)
                .Where(s => s.Name.Equals(name) && s.Category.Id.Equals(baseCategoryId) && s.Enabled).ToListAsync();
            }
        }

        /// <summary>
        /// Get subcategory by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Subcategory> GetSubcategoryById (int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Subcategories
                    .Include(s => s.Category)
                    .Include(s => s.Movements)    
                .FirstOrDefaultAsync(s => s.Id.Equals(id) && s.Enabled);
            }
        }

        /// <summary>
        /// Get subcategory by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Subcategory> GetSubcategoryByName (string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Subcategories.Where(s => s.Name.Equals(name) && s.Enabled).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Get subcategory by name and by base category
        /// </summary>
        /// <param name="name"></param>
        /// <param name="baseCategoryId"></param>
        /// <returns></returns>
        public async Task<Subcategory> GetSubcategoryByName (string name, int baseCategoryId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return await context.Subcategories
                    .Include(s => s.Category)
                .Where(s => s.Category.Id.Equals(baseCategoryId) && s.Name.Equals(name) && s.Enabled).
                FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// A subcategory to be inserted
        /// </summary>
        /// <param name="subcategory"></param>
        public async Task Insert (Subcategory subcategory)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Subcategories.Add(subcategory);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Update an existing subcategory
        /// </summary>
        /// <param name="subcategory"></param>
        public async Task Update (Subcategory subcategory)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(subcategory).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Update a collection of subcategory
        /// </summary>
        /// <param name="subcategories"></param>
        public async Task Update (ICollection<Subcategory> subcategories)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                foreach (var subcategory in subcategories)
                    context.Entry(subcategory).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }
    }
}