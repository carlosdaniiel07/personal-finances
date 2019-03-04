using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net;

using PersonalFinances.Models.ViewModels;
using PersonalFinances.Services;
using PersonalFinances.Services.Exceptions;

using System.Data.Entity.Infrastructure;
using PersonalFinances.Models;

namespace PersonalFinances.Controllers
{
    public class InvoicesController : Controller
    {
        InvoiceService _service = new InvoiceService();
        AccountService _accountService = new AccountService();
        MovementService _movementService = new MovementService();

        // GET: Invoices/Details
        public async Task<ActionResult> Details (int? Id)
        {
            try
            {
                return View(await _service.GetById(Id.GetValueOrDefault()));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        // GET: Invoices/Print
        public async Task<ActionResult> Print (int? Id)
        {
            try
            {
                return View(await _service.GetClosedInvoiceById(Id.GetValueOrDefault()));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        // GET: Invoices/Pay
        public async Task<ActionResult> Pay (int? Id)
        {
            try
            {
                return View(await GetInvoicePaymentViewModel(await _service.GetClosedInvoiceById(Id.GetValueOrDefault())));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Pay (InvoicePaymentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Pay(viewModel);
                    return RedirectToAction("View", "CreditCards", new { Id = viewModel.Invoice.CreditCardId });
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("InvoiceError", e.Message);
                    return View(await GetInvoicePaymentViewModel(await _service.GetClosedInvoiceById(viewModel.Invoice.Id)));
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
                catch (NotFoundException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
            }
            else
            {
                return View(await GetInvoicePaymentViewModel(await _service.GetClosedInvoiceById(viewModel.Invoice.Id)));
            }
        }

        // GET: Invoices/RemoveMovement
        public async Task<ActionResult> RemoveMovement (int? Id)
        {
            try
            {
                return View(await _movementService.GetById(Id.GetValueOrDefault()));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveMovement (int id)
        {
            try
            {
                await _service.RemoveFromInvoice(id);
                return RedirectToAction("Index", "CreditCards");
            }
            catch (DbUpdateException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Get a InvoicePaymentViewModel object
        /// </summary>
        /// <returns></returns>
        private async Task<InvoicePaymentViewModel> GetInvoicePaymentViewModel (Invoice invoice)
        {
            return new InvoicePaymentViewModel
            {
                Invoice = invoice,
                Accounts = await _accountService.GetAll(),
                PaidValue = invoice.TotalValue,
                PaymentDate = DateTime.Today
            };
        }
    }
}