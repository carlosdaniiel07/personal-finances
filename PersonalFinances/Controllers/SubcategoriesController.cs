using System.Net;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;

using PersonalFinances.Models;
using PersonalFinances.Models.ViewModels;
using PersonalFinances.Services;
using PersonalFinances.Services.Exceptions;

namespace PersonalFinances.Controllers
{
    public class SubcategoriesController : Controller
    {
        private SubcategoryService _service = new SubcategoryService();
        private CategoryService _categoryService = new CategoryService();

        // GET: Subcategories
        public ActionResult Index ()
        {
            return View(_service.GetAll());
        }

        // GET: Subcategories/View
        public ActionResult View (int? Id)
        {
            try
            {
                return View(_service.GetById(Id.GetValueOrDefault()));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        // GET: Subcategories/Edit
        public ActionResult Edit (int? Id)
        {
            try
            {
                var viewModel = new SubcategoryViewModel
                {
                    Subcategory = _service.GetById(Id.GetValueOrDefault()),
                    Categories = _categoryService.GetAll()
                };

                return View(viewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit (int Id, Subcategory subcategory)
        {
            if (ModelState.IsValid && Id.Equals(subcategory.Id))
            {
                try
                {
                    _service.Update(subcategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("SubcategoryValidation", e.Message);

                    var viewModel = new SubcategoryViewModel
                    {
                        Subcategory = subcategory,
                        Categories = _categoryService.GetAll()
                    };

                    return View(viewModel);
                }
                catch (NotFoundException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                var viewModel = new SubcategoryViewModel
                {
                    Subcategory = subcategory,
                    Categories = _categoryService.GetAll()
                };

                return View(viewModel);
            }
        }

        // GET: Subcategories/Delete
        public ActionResult Delete (int? Id)
        {
            try
            {
                return View(_service.GetById(Id.GetValueOrDefault()));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete (int Id)
        {
            try
            {
                _service.Delete(Id);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
            catch (DbUpdateException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // GET: Subcategories/New
        public ActionResult New ()
        {
            var viewModel = new SubcategoryViewModel { Categories = _categoryService.GetAll() }; 
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New (Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _service.Add(subcategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("SubcategoryValidation", e.Message);

                    var viewModel = new SubcategoryViewModel
                    {
                        Subcategory = subcategory,
                        Categories = _categoryService.GetAll()
                    };

                    return View(viewModel);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                var viewModel = new SubcategoryViewModel
                {
                    Subcategory = subcategory,
                    Categories = _categoryService.GetAll()
                };

                return View(viewModel);
            }
        }
    }
}