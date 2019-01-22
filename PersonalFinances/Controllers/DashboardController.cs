using System.Linq;
using System.Web.Mvc;

using PersonalFinances.Services;
using PersonalFinances.Models.ViewModels;

namespace PersonalFinances.Controllers
{
    public class DashboardController : Controller
    {
        private AccountService _accountService = new AccountService();

        // GET: Dashboard
        public ActionResult Index()
        {
            var accounts = _accountService.GetAll();

            var viewModel = new DashboardViewModel
            {
                TotalBalance = accounts.Sum(a => a.Balance),
                BalanceOnMonth = accounts.Sum(a => a.MonthlyBalance),
                Accounts = accounts.Count(),
                Movements = accounts.Sum(a => a.Movements.Count)
            };

            return View(viewModel);
        }
    }
}