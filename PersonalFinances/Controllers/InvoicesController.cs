using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net;

using PersonalFinances.Services;
using PersonalFinances.Services.Exceptions;

namespace PersonalFinances.Controllers
{
    public class InvoicesController : Controller
    {
        InvoiceService _service = new InvoiceService();

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
    }
}