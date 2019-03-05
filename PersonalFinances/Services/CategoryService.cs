﻿using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IEnumerable<Category>> GetAll ()
        {
            return await _repository.GetCategories();
        }

        /// <summary>
        /// Insert a category
        /// </summary>
        /// <param name="category"></param>
        public async Task Add (Category category)
        {
            category.Enabled = true;

            var nameExists = await _repository.GetCategoryByName(category.Name, category.Type) != null;

            if (!nameExists)
                await _repository.Insert(category);
            else
                throw new AlreadyExistsException($"Already exists a category with name {category.Name}");
        }

        /// <summary>
        /// Update a existing category
        /// </summary>
        /// <param name="category"></param>
        public async Task Update (Category category)
        {
            var currentCategory = await GetById(category.Id);
            
            if (currentCategory.CanEdit)
            {
                var quantity = (await _repository.GetCategoriesByName(category.Name)).Count(c => !c.Id.Equals(currentCategory.Id));

                category.Enabled = true;
                category.CanEdit = currentCategory.CanEdit;

                if (quantity.Equals(0))
                    await _repository.Update(category);
                else
                    throw new AlreadyExistsException($"Already exists a {category.Type} category with name {category.Name}");
            }
            else
            {
                throw new NotValidOperationException("This category was generated by other routine and cannot be changed");
            }
        }

        /// <summary>
        /// Remove a category
        /// </summary>
        /// <param name="id"></param>
        public async Task Remove (int id)
        {
            var category = await GetById(id);

            if (category.CanEdit)
            {
                category.Enabled = false;

                await _repository.Update(category);
                await _subcategoryService.Delete(category.Subcategories);
            }
            else
            {
                throw new NotValidOperationException("This category was generated by other routine and cannot be changed");
            }
        }

        /// <summary>
        /// Get a category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Category> GetById (int id)
        {
            var category = await _repository.GetCategoryById(id);

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
        public async Task<Category> GetByName (string name, string type)
        {
            var category = await _repository.GetCategoryByName(name, type);

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