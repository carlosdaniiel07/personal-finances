using System.Web.Mvc;

namespace PersonalFinances.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: Errors/Error
        public ActionResult Error ()
        {
            ViewData.Add("ErrorMessage", "Some error occurred when processing your request. Try again.");
            return View();
        }

        // GET: Errors/NotFound
        public ActionResult NotFound ()
        {
            if (TempData["ErrorMessage"] != null)
                ViewData.Add("ErrorMessage", TempData["ErrorMessage"].ToString());
            else
                ViewData.Add("ErrorMessage", "This resource was not found and cannot be displayed");
            return View();
        }

        // GET: Errors/ServerError
        public ActionResult ServerError ()
        {
            if (TempData["ErrorMessage"] != null)
                ViewData.Add("ErrorMessage", TempData["ErrorMessage"].ToString());
            else
                ViewData.Add("ErrorMessage", "An internal server error occurred. If this persists contact the system administrator");
            return View();
        }
    }
}