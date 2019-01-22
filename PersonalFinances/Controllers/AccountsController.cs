using System.Net;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;

using PersonalFinances.Models;
using PersonalFinances.Models.ViewModels;
using PersonalFinances.Services;
using PersonalFinances.Services.Exceptions;

namespace PersonalFinances.Controllers
{
    public class AccountsController : Controller
    {
        private AccountService _service = new AccountService();

        // GET: Accounts
        public ActionResult Index ()
        {
            return View(_service.GetAll());
        }

        // GET: Accounts/View
        public ActionResult View (int? Id)
        {
            try
            {
                var viewModel = new ViewAccountViewModel
                {
                    Account = _service.GetById(Id.GetValueOrDefault()),
                    AccountMovements = _service.GetMovements(Id.GetValueOrDefault())
                };
                return View(viewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(404, e.Message);
            }
        }

        // GET: Accounts/Delete
        public ActionResult Delete (int? Id)
        {
            try
            {
                return View(_service.GetById(Id.GetValueOrDefault()));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(404, e.Message);
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

        // GET: Accounts/New
        public ActionResult New ()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New (Account account)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _service.Add(account);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("AccountValidation", e.Message);
                    return View(account);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return View(account);
            }
        }

        // GET: Accounts/Edit
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
        public ActionResult Edit (int Id, Account account)
        {
            if (ModelState.IsValid && Id.Equals(account.Id))
            {
                try
                {
                    _service.Update(account);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("AccountValidation", e.Message);
                    return View(account);
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
                return View(account);
            }
        }
    }
}