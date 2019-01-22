using System.Linq;
using System.Collections.Generic;

using PersonalFinances.Models;
using PersonalFinances.Repositories;
using PersonalFinances.Services.Exceptions;

namespace PersonalFinances.Services
{
    public class CategoryService
    {
        private CategoryRepository _repository = new CategoryRepository();
        private SubcategoryService _subcategoryService = new SubcategoryService();

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> GetAll ()
        {
            return _repository.GetCategories();
        }

        /// <summary>
        /// Insert a category
        /// </summary>
        /// <param name="category"></param>
        public void Add (Category category)
        {
            category.Enabled = true;

            var nameExists = _repository.GetCategoryByName(category.Name, category.Type) != null;

            if (!nameExists)
                _repository.Insert(category);
            else
                throw new AlreadyExistsException($"Already exists a category with name {category.Name}");
        }

        /// <summary>
        /// Update a existing category
        /// </summary>
        /// <param name="category"></param>
        public void Update (Category category)
        {
            var currentCategory = GetById(category.Id);
            var quantity = _repository.GetCategoriesByName(category.Name).Count(c => !c.Id.Equals(currentCategory.Id));

            category.Enabled = true;

            if (quantity.Equals(0))
                _repository.Update(category);
            else
                throw new AlreadyExistsException($"Already exists a {category.Type} category with name {category.Name}");
        }

        /// <summary>
        /// Remove a category
        /// </summary>
        /// <param name="id"></param>
        public void Remove (int id)
        {
            var category = GetById(id);
            category.Enabled = false;

            _repository.Update(category);
            _subcategoryService.Delete(category.Subcategories);
        }

        /// <summary>
        /// Get a category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Category GetById (int id)
        {
            var category = _repository.GetCategoryById(id);

            if (category != null)
                return category;
            else
                throw new NotFoundException("This category not exists");
        }

        /// <summary>
        /// Get a category by Name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Category GetByName (string name, string type)
        {
            var category = _repository.GetCategoryByName(name, type);

            if (category != null)
                return category;
            else
                throw new NotFoundException("This category not exists");
        }

        /// <summary>
        /// Get the category balance
        /// </summary>
        /// <returns></returns>
        public double CategoryBalance (IEnumerable<Movement> movements)
        {
            return TotalCredit(movements) - TotalDebit(movements);
        }

        /// <summary>
        /// Get the category total credit value
        /// </summary>
        /// <param name="movements"></param>
        /// <returns></returns>
        public double TotalCredit (IEnumerable<Movement> movements)
        {
            return movements.TotalCredit();
        }

        /// <summary>
        /// Get the category total debit value
        /// </summary>
        /// <param name="movements"></param>
        /// <returns></returns>
        public double TotalDebit (IEnumerable<Movement> movements)
        {
            return movements.TotalDebit();
        }
    }
}