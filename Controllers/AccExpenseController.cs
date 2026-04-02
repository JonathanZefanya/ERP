using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models;
using BusinessERP.Models.AccAccountViewModel;
using BusinessERP.Models.AccExpenseViewModel;
using BusinessERP.Models.AccTransactionViewModel;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccExpenseController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public AccExpenseController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.AccExpense.RoleName)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetDataTabelData()
        {
            try
            {
                var _GetDataTabelData = _iCommon.GetAllAccExpense();

                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                var filteredData = ApplySearchOnData(_GetDataTabelData, searchValue);

                var result = GetDataTablesProcessData(_GetDataTabelData, filteredData, searchValue);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private IQueryable<AccExpenseCRUDViewModel> ApplySearchOnData(IQueryable<AccExpenseCRUDViewModel> query, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return query;
            }

            searchValue = searchValue.ToLower();
            return query.Where(obj => obj.Id.ToString().Contains(searchValue)
                || obj.AccountId.ToString().ToLower().Contains(searchValue)
                || obj.ExpenseDate.ToString().ToLower().Contains(searchValue)
                || obj.Amount.ToString().ToLower().Contains(searchValue)
                || obj.Note.ToLower().Contains(searchValue)
                || obj.CreatedDate.ToString().ToLower().Contains(searchValue)
                || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)

                || obj.CreatedDate.ToString().Contains(searchValue)
            );
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            AccExpenseCRUDViewModel vm = await _iCommon.GetAllAccExpense().FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            AccExpenseCRUDViewModel vm = new();
            var result = _iCommon.GetTableData<AccAccount>(x => x.Cancelled == false).ToList();
            ViewBag.ddlAccAccount = new SelectList(result.ToList(), "Id", "AccountName");
            if (id > 0) vm = await _context.AccExpense.Where(x => x.Id == id).FirstOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditSave([FromBody] AccExpenseCRUDViewModel vm)
        {
            try
            {
                AccExpense _AccExpense = new();
                string _UserName = HttpContext.User.Identity.Name;
                if (vm.Id > 0)
                {
                    _AccExpense = await _context.AccExpense.FindAsync(vm.Id);

                    vm.CreatedDate = _AccExpense.CreatedDate;
                    vm.CreatedBy = _AccExpense.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_AccExpense).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Expense Updated Successfully. ID: " + _AccExpense.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    _AccExpense = vm;
                    _AccExpense.CreatedDate = DateTime.Now;
                    _AccExpense.ModifiedDate = DateTime.Now;
                    _AccExpense.CreatedBy = _UserName;
                    _AccExpense.ModifiedBy = _UserName;
                    _context.Add(_AccExpense);
                    await _context.SaveChangesAsync();

                    UpdateAccountViewModel _UpdateAccountViewModel = new()
                    {
                        AccUpdateType = AccAccountUpdateType.Debit,
                        AccAccountNo = _AccExpense.AccountId,
                        Amount = _AccExpense.Amount,
                        Credit = 0,
                        Debit = _AccExpense.Amount,
                        Type = "Expense",
                        Reference = "Expense Id: " + _AccExpense.Id,
                        Description = vm.Note,
                        UserName = _UserName
                    };
                    await _iCommon.UpdateAccoutDuringTran(_UpdateAccountViewModel);

                    var _AlertMessage = "Expense Created Successfully. ID: " + _AccExpense.Id;
                    return new JsonResult(_AlertMessage);
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
                throw;
            }
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Int64 id)
        {
            string _UserName = HttpContext.User.Identity.Name;
            try
            {
                var _AccExpense = await _context.AccExpense.FindAsync(id);
                _AccExpense.ModifiedDate = DateTime.Now;
                _AccExpense.ModifiedBy = _UserName;
                _AccExpense.Cancelled = true;

                _context.Update(_AccExpense);
                await _context.SaveChangesAsync();

                UpdateAccountViewModel _UpdateAccountViewModel = new()
                {
                    AccUpdateType = AccAccountUpdateType.DebitRevart,
                    AccAccountNo = _AccExpense.AccountId,
                    Amount = _AccExpense.Amount,
                    Credit = _AccExpense.Amount,
                    Debit = 0,
                    Type = "Expense-Revart",
                    Reference = "Expense Id: " + _AccExpense.Id,
                    Description = "Expense Deleted.",
                    UserName = _UserName
                };
                await _iCommon.UpdateAccoutDuringTran(_UpdateAccountViewModel);
                return new JsonResult(_AccExpense);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
