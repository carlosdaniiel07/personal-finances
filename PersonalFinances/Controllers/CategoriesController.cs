using System.Net;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;

using PersonalFinances.Models;
using PersonalFinances.Models.ViewModels;
using PersonalFinances.Services;
using PersonalFinances.Services.Exceptions;

namespace PersonalFinances.Controllers
{
    public class CategoriesController : Controller
    {
        private CategoryService _service = new CategoryService();

        // GET: Categories
        public ActionResult Index()
        {
            return View(_service.GetAll());
        }

        // GET: Categories/View
        public ActionResult View (int? Id)
        {
            try
            {
                var viewModel = new ViewCategoryViewModel { Category = _service.GetById(Id.GetValueOrDefault()) };
                return View(viewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        // GET: Categories/New
        public ActionResult New ()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New (Category category)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    _service.Add(category);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("CategoryValidation", e.Message);
                    return View(category);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return View(category);
            }
        }

        // GET: Categories/Edit
        public ActionResult Edit (int? Id)
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
        public ActionResult Edit (int Id, Category category)
        {
            if(ModelState.IsValid && Id.Equals(category.Id))
            {
                try
                {
                    _service.Update(category);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("CategoryValidation", e.Message);
                    return View(category);
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
                return View(category);
            }
        }

        // GET: Categories/Delete
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
                _service.Remove(Id);
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
    }
}