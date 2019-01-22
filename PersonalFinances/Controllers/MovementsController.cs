using System.Net;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;

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

        // GET: Movements
        public ActionResult Index()
        {
            return View(_service.GetAll());
        }

        // GET: Movements/New
        public ActionResult New ()
        {
            var viewModel = new MovementViewModel
            {
                Accounts = _accountService.GetAll(),
                Categories = _categoryService.GetAll(),
                Subcategories = _subcategoryService.GetAll()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New (Movement movement)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _service.Add(movement);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("MovementValidation", e.Message);

                    var viewModel = new MovementViewModel
                    {
                        Movement = movement,
                        Accounts = _accountService.GetAll(),
                        Categories = _categoryService.GetAll(),
                        Subcategories = _subcategoryService.GetAll()
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
                var viewModel = new MovementViewModel
                {
                    Movement = movement,
                    Accounts = _accountService.GetAll(),
                    Categories = _categoryService.GetAll(),
                    Subcategories = _subcategoryService.GetAll()
                };
                return View(viewModel);
            }
        }

        // GET: Movements/Delete
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
        public ActionResult Delete (int id)
        {
            try
            {
                _service.Delete(id);
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
        public ActionResult Edit (int? Id)
        {
            try
            {
                var viewModel = new MovementViewModel
                {
                    Movement = _service.GetById(Id.GetValueOrDefault()),
                    Accounts = _accountService.GetAll(),
                    Categories = _categoryService.GetAll(),
                    Subcategories = _subcategoryService.GetAll()
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
        public ActionResult Edit (int Id, Movement movement)
        {
            if(ModelState.IsValid && Id.Equals(movement.Id))
            {
                try
                {
                    _service.Update(movement);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("MovementValidation", e.Message);

                    var viewModel = new MovementViewModel
                    {
                        Movement = movement,
                        Accounts = _accountService.GetAll(),
                        Categories = _categoryService.GetAll(),
                        Subcategories = _subcategoryService.GetAll()
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
                var viewModel = new MovementViewModel
                {
                    Movement = movement,
                    Accounts = _accountService.GetAll(),
                    Categories = _categoryService.GetAll(),
                    Subcategories = _subcategoryService.GetAll()
                };
                return View(viewModel);
            }
        }

        // GET: Movements/Details
        public ActionResult Details (int? Id)
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
    }
}