using System;
using System.Linq;
using System.Collections.Generic;

using PersonalFinances.Models;
using PersonalFinances.Repositories;
using PersonalFinances.Services.Exceptions;

namespace PersonalFinances.Services
{
    public class SubcategoryService
    {
        private SubcategoryRepository _repository = new SubcategoryRepository();

        /// <summary>
        /// Get all subcategories
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Subcategory> GetAll ()
        {
            return _repository.GetSubcategories();
        }

        /// <summary>
        /// Get a subcategory by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Subcategory GetById (int id)
        {
            var subcategory = _repository.GetSubcategoryById(id);

            if (subcategory != null)
                return subcategory;
            else
                throw new NotFoundException("This subcategory not exist");
        }

        /// <summary>
        /// Insert a new subcategory
        /// </summary>
        /// <param name="subcategory"></param>
        public void Add (Subcategory subcategory)
        {
            subcategory.Enabled = true;
            var nameExists = _repository.GetSubcategoryByName(subcategory.Name, subcategory.CategoryId) != null;

            if (!nameExists)
                _repository.Insert(subcategory);
            else
                throw new AlreadyExistsException($"Already exists a subcategory {subcategory.Name} in this category");
        }

        /// <summary>
        /// Update an existing subcategory
        /// </summary>
        /// <param name="subcategory"></param>
        public void Update (Subcategory subcategory)
        {
            var currentSubcategory = GetById(subcategory.Id);
            var quantity = _repository.GetSubcategoriesByName(subcategory.Name, subcategory.CategoryId)
                .Count(s => !s.Id.Equals(currentSubcategory.Id));

            subcategory.Enabled = true;
            
            if (quantity.Equals(0))
                _repository.Update(subcategory);
            else
                throw new AlreadyExistsException($"Already exists a subcategory {subcategory.Name} in this category");
        }

        /// <summary>
        /// Delete an subcategory
        /// </summary>
        /// <param name="id"></param>
        public void Delete (int id)
        {
            var subcategory = GetById(id);
            subcategory.Enabled = false;

            _repository.Update(subcategory);
        }

        /// <summary>
        /// Delete a collection of subcategory
        /// </summary>
        /// <param name="subcategories"></param>
        public void Delete (ICollection<Subcategory> subcategories)
        {
            Func<Subcategory, Subcategory> disableSubcategoryAction = (s) => { s.Enabled = false; return s; };
            subcategories = subcategories.Select(disableSubcategoryAction).ToList();

            _repository.Update(subcategories);
        }
    }
}