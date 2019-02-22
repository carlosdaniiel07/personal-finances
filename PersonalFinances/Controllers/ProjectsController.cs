using System.Net;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

using PersonalFinances.Models;
using PersonalFinances.Services;
using PersonalFinances.Services.Exceptions;

namespace PersonalFinances.Controllers
{
    public class ProjectsController : Controller
    {
        ProjectService _service = new ProjectService();

        // GET: Projects
        public async Task<ActionResult> Index()
        {
            return View(await _service.GetAllProjects());
        }

        // GET: Projects/New
        public ActionResult New ()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> New (Project project)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Add(project);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("ProjectValidation", e.Message);
                    return View(project);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return View();
            }
        }

        // GET: Projects/Edit
        public async Task<ActionResult> Edit (int? Id)
        {
            try
            {
                return View(await _service.GetProjectById(Id.GetValueOrDefault()));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit (int Id, Project project)
        {
            if (ModelState.IsValid && Id.Equals(project.Id))
            {
                try
                {
                    await _service.Update(project);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("ProjectValidation", e.Message);
                    return View(project);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
                catch (NotFoundException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
                }
            }
            else
            {
                return View(project);
            }
        }

        public async Task<ActionResult> Details(int? Id)
        {
            try
            {
                return View(await _service.GetProjectById(Id.GetValueOrDefault()));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        // GET: Projects/Delete
        public async Task<ActionResult> Delete (int? Id)
        {
            try
            {
                return View(await _service.GetProjectById(Id.GetValueOrDefault()));
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
    }
}