using System.Net;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;

using PersonalFinances.Models;
using PersonalFinances.Models.ViewModels;
using PersonalFinances.Services;
using PersonalFinances.Services.Exceptions;

namespace PersonalFinances.Controllers
{
    public class TransfersController : Controller
    {
        private TransferService _service = new TransferService();
        private AccountService _accountService = new AccountService();

        // GET: Transfers
        public ActionResult Index()
        {
            return View(_service.GetAll());
        }

        // GET: Transfers/New
        public ActionResult New ()
        {
            var viewModel = new TransferViewModel { Accounts = _accountService.GetAll() };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New (Transfer transfer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _service.Add(transfer);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("TransferValidation", e.Message);

                    var viewModel = new TransferViewModel
                    {
                        Transfer = transfer,
                        Accounts = _accountService.GetAll()
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
                var viewModel = new TransferViewModel
                {
                    Transfer = transfer,
                    Accounts = _accountService.GetAll()
                };
                return View(viewModel);
            }
        }

        // GET: Transfers/Details
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

        // GET: Transfers/Cancel
        public ActionResult Cancel (int? Id)
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
        public ActionResult Cancel (int Id)
        {
            try
            {
                _service.Cancel(Id);
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

        // GET: Transfers/Launch
        public ActionResult Launch (int? Id)
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
        public ActionResult Launch (int Id)
        {
            try
            {
                _service.Launch(Id);
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