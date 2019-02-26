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
    public class TransfersController : Controller
    {
        private TransferService _service = new TransferService();
        private AccountService _accountService = new AccountService();

        // GET: Transfers
        public async Task<ActionResult> Index()
        {
            return View(await _service.GetAll());
        }

        // GET: Transfers/New
        public async Task<ActionResult> New ()
        {
            return View(await GetViewModel(null));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> New (Transfer transfer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Add(transfer);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("TransferValidation", e.Message);
                    return View(await GetViewModel(transfer));
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return View(await GetViewModel(transfer));
            }
        }

        // GET: Transfers/Details
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

        // GET: Transfers/Cancel
        public async Task<ActionResult> Cancel (int? Id)
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
        public async Task<ActionResult> Cancel (int Id)
        {
            try
            {
                await _service.Cancel(Id);
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
        public async Task<ActionResult> Launch (int? Id)
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
        public async Task<ActionResult> Launch (int Id)
        {
            try
            {
                await _service.Launch(Id);
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

        private async Task<TransferViewModel> GetViewModel (Transfer transfer)
        {
            return new TransferViewModel
            {
                Transfer = transfer,
                Accounts = await _accountService.GetAll()
            };
        }
    }
}