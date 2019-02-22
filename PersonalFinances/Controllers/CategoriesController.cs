using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;

using PersonalFinances.Models;
using PersonalFinances.Services;
using PersonalFinances.Services.Exceptions;

namespace PersonalFinances.Controllers
{
    public class CategoriesController : Controller
    {
        private CategoryService _service = new CategoryService();

        // GET: Categories
        public async Task<ActionResult> Index()
        {
            return View(await _service.GetAll());
        }

        // GET: Categories/View
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

        // GET: Categories/New
        public ActionResult New ()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> New (Category category)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    await _service.Add(category);
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
        public async Task<ActionResult> Edit (int? Id)
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
        public async Task<ActionResult> Edit (int Id, Category category)
        {
            if(ModelState.IsValid && Id.Equals(category.Id))
            {
                try
                {
                    await _service.Update(category);
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
                await _service.Remove(Id);
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