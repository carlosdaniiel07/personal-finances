using System.Net;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

using PersonalFinances.Services;
using PersonalFinances.Services.Exceptions;
using PersonalFinances.Models;
using PersonalFinances.Models.ViewModels;

namespace PersonalFinances.Controllers
{
    public class MovementsController : Controller
    {
        private MovementService _service = new MovementService();
        private AccountService _accountService = new AccountService();
        private CategoryService _categoryService = new CategoryService();
        private SubcategoryService _subcategoryService = new SubcategoryService();
        private ProjectService _projectService = new ProjectService();
        private CreditCardService _creditCardService = new CreditCardService();

        // GET: Movements
        public async Task<ActionResult> Index()
        {
            return View(await _service.GetAll());
        }

        // GET: Movements/New
        public async Task<ActionResult> New ()
        {
            return View(await GetViewModel(null));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> New (Movement movement)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Add(movement);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("MovementValidation", e.Message);
                    return View(await GetViewModel(movement));
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return View(await GetViewModel(movement));
            }
        }

        // GET: Movements/Delete
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

        // GET: Movements/Edit
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
        public async Task<ActionResult> Edit (int Id, Movement movement)
        {
            if(ModelState.IsValid && Id.Equals(movement.Id))
            {
                try
                {
                    await _service.Update(movement);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("MovementValidation", e.Message);
                    return View(await GetViewModel(movement));
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
                return View(await GetViewModel(movement));
            }
        }

        // GET: Movements/Details
        public async Task<ActionResult> Details (int? Id)
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

        /// <summary>
        /// Get a MovementViewModel object
        /// </summary>
        /// <param name="movement"></param>
        /// <returns></returns>
        private async Task<MovementViewModel> GetViewModel (Movement movement)
        {
            return new MovementViewModel
            {
                Movement = movement,
                Accounts = await _accountService.GetAll(),
                Categories = await _categoryService.GetAll(),
                Subcategories = await _subcategoryService.GetAll(),
                Projects = await _projectService.GetAllActiveProjects(),
                CreditCards = await _creditCardService.GetAll()
            };
        }
    }
}