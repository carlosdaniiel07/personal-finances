using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net;

using PersonalFinances.Models;
using PersonalFinances.Models.ViewModels;
using PersonalFinances.Services;
using PersonalFinances.Services.Exceptions;

using System.Data.Entity.Infrastructure;

namespace PersonalFinances.Controllers
{
    public class CreditCardsController : Controller
    {
        private CreditCardService _service = new CreditCardService();

        // GET: CreditCards
        public async Task<ActionResult> Index()
        {
            return View(await _service.GetAll());
        }

        // GET: CreditCards/New
        public ActionResult New ()
        {
            return View(GetViewModel(null));
        }

        // GET: CreditCards/Edit
        public async Task<ActionResult> Edit (int? Id)
        {
            try
            {
                return View(GetViewModel(await _service.GetById(Id.GetValueOrDefault())));
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit (int Id, CreditCard creditCard)
        {
            if (Id.Equals(creditCard.Id) && ModelState.IsValid)
            {
                try
                {
                    await _service.Update(creditCard);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("CreditCardError", e.Message);
                    return View(creditCard);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return View(creditCard);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> New (CreditCard creditCard)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Insert(creditCard);
                    return RedirectToAction(nameof(Index));
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("CreditCardError", e.Message);
                    return View(creditCard);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return View(creditCard);
            }
        }

        // GET: CreditCards/Remove
        public async Task<ActionResult> Remove (int? Id)
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
        public async Task<ActionResult> Remove (int Id)
        {
            try
            {
                await _service.Remove(Id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        // GET: CreditCards/View
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

        /// <summary>
        /// Get a CreditCardViewModel view model
        /// </summary>
        /// <returns></returns>
        private CreditCardViewModel GetViewModel (CreditCard creditCard)
        {
            return new CreditCardViewModel
            {
                CreditCard = creditCard,
                AvailableDays = _service.GetAvailableDays()
            };            
        }
    }
}