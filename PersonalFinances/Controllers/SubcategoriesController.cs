using System.Net;
using System.Web.Mvc;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Index ()
        {
            return View(await _service.GetAll());
        }

        // GET: Subcategories/View
        public async Task<ActionResult> View (int? Id)
        {
            try
            {
                return View(await _service.GetById(Id.GetValueOrDefault()));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        // GET: Subcategories/Edit
        public async Task<ActionResult> Edit (int? Id)
        {
            try
            {
                return View(await GetViewModel(await _service.GetById(Id.GetValueOrDefault())));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit (int Id, Subcategory subcategory)
        {
            if (ModelState.IsValid && Id.Equals(subcategory.Id))
            {
                try
                {
                    await _service.Update(subcategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("SubcategoryValidation", e.Message);
                    return View(await GetViewModel(subcategory));
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
                return View(await GetViewModel(subcategory));
            }
        }

        // GET: Subcategories/Delete
        public async Task<ActionResult> Delete (int? Id)
        {
            try
            {
                return View(await _service.GetById(Id.GetValueOrDefault()));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete (int Id)
        {
            try
            {
                await _service.Delete(Id);
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
        public async Task<ActionResult> New ()
        { 
            return View(await GetViewModel(null));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> New (Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Add(subcategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("SubcategoryValidation", e.Message);
                    return View(await GetViewModel(subcategory));
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return View(await GetViewModel(subcategory));
            }
        }

        private async Task<SubcategoryViewModel> GetViewModel (Subcategory subcategory)
        {
            return new SubcategoryViewModel
            {
                Subcategory = subcategory,
                Categories = await _categoryService.GetAll()
            };
        }
    }
}