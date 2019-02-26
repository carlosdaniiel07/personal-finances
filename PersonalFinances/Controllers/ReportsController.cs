using System;
using System.Threading.Tasks;
using System.Web.Mvc;

using PersonalFinances.Models.ViewModels;
using PersonalFinances.Services;
using PersonalFinances.Services.Exceptions;
using System.Net;
using System.IO;

namespace PersonalFinances.Controllers
{
    public class ReportsController : Controller
    {
        private MovementService _movementService = new MovementService();
        private AccountService _accountService = new AccountService();
        private CategoryService _categoryService = new CategoryService();
        private SubcategoryService _subcategoryService = new SubcategoryService();
        private ProjectService _projectService = new ProjectService();

        // GET: Reports/BankStatement
        public async Task<ActionResult> BankStatement ()
        {
            return View(await GetBankStatementViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> BankStatement(BankStatementViewModel bankStatement)
        {
            try
            {
                var viewModel = await GetBankStatementViewModel();
                viewModel.Movements = await _movementService.GetAll(bankStatement);

                return View(viewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }
        
        private async Task<BankStatementViewModel> GetBankStatementViewModel ()
        {
            return new BankStatementViewModel
            {
                Accounts = await _accountService.GetAll(),
                Categories = await _categoryService.GetAll(),
                Subcategories = await _subcategoryService.GetAll(),
                Projects = await _projectService.GetAllProjects()
            };
        }
    }
}