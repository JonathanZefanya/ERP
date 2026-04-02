using BusinessERP.ConHelper;
using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models;
using BusinessERP.Models.AccAccountViewModel;
using BusinessERP.Models.CommonViewModel;
using BusinessERP.Models.ExpenseSummaryViewModel;
using BusinessERP.Models.PaymentModeHistoryViewModel;
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
    public class ExpenseSummaryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly ISalesService _iSalesService;
        private readonly IPaymentService _iPaymentService;

        public ExpenseSummaryController(ApplicationDbContext context, ICommon iCommon, IPaymentService iPaymentService, ISalesService iSalesService)
        {
            _context = context;
            _iCommon = iCommon;
            _iPaymentService = iPaymentService;
            _iSalesService = iSalesService;
        }

        [Authorize(Roles = Pages.MainMenu.ExpenseSummary.RoleName)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetDataTabelData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int resultTotal = 0;

                var _GetGridItem = _iCommon.GetExpenseSummaryGridItem();
                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.GrandTotal.ToString().ToLower().Contains(searchValue)
                    || obj.PaidAmount.ToString().ToLower().Contains(searchValue)
                    || obj.DueAmount.ToString().ToLower().Contains(searchValue)
                    || obj.CurrencyCode.ToString().ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue)
                    || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            ExpenseSummaryCRUDViewModel vm = await _iCommon.GetExpenseSummaryGridItem().Where(m => m.Id == id).FirstOrDefaultAsync();
            vm.listExpenseDetails = _iCommon.GetExpenseDetailsList().Where(x => x.ExpenseSummaryId == id).ToList();
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            try
            {
                ExpenseSummaryCRUDViewModel vm = new();
                ExpenseSummary _ExpenseSummary = new();

                ViewBag.LoadddlExpenseType = new SelectList(_iCommon.LoadddlExpenseType(), "Id", "Name");
                ViewBag.ddlCurrency = new SelectList(_iCommon.LoadddlCurrencyItem(), "Id", "Name");
                ViewBag.ddlBranch = new SelectList(_iCommon.GetTableData<Branch>(x => x.Cancelled == false).ToList(), "Id", "Name");
                ViewBag._LoadddlPaymentType = new SelectList(_iCommon.LoadddlPaymentType(), "Id", "Name");

                if (id > 0)
                {
                    vm = await _context.ExpenseSummary.Where(x => x.Id == id).FirstOrDefaultAsync();
                    _ExpenseSummary.Action = DBOperationType.Edit;
                    vm.listExpenseDetails = _iCommon.GetExpenseDetailsList().Where(x => x.ExpenseSummaryId == id).ToList();
                    vm.listPaymentModeHistoryCRUDViewModel = await _iSalesService.GetPaymentModeHistory(InvoicePaymentType.RegularExpensePayment).Where(x => x.PaymentId == id).ToListAsync();
                }
                else
                {
                    //Set Branch By User
                    var _UserName = User.Identity.Name;
                    _ExpenseSummary.BranchId = await _iCommon.GetBranchIdByUserName(_UserName);

                    _ExpenseSummary.Title = "Regular";
                    _ExpenseSummary.Action = DBOperationType.Add;
                    _ExpenseSummary.CreatedDate = DateTime.Now;
                    _ExpenseSummary.ModifiedDate = DateTime.Now;
                    _ExpenseSummary.CreatedBy = HttpContext.User.Identity.Name;
                    _ExpenseSummary.ModifiedBy = HttpContext.User.Identity.Name;
                    var result = _context.Add(_ExpenseSummary);
                    var result2 = await _context.SaveChangesAsync();
                    vm = _ExpenseSummary;
                }
                var _IsInRole = User.IsInRole("Admin");
                vm.RoleName = _IsInRole == true ? "Admin" : "Other";
                return PartialView("_AddEdit", vm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ExpenseSummaryCRUDViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            string _UserName = HttpContext.User.Identity.Name;
            try
            {
                ExpenseSummary _ExpenseSummary = new();
                _ExpenseSummary = await _context.ExpenseSummary.FindAsync(vm.Id);

                vm.CreatedDate = _ExpenseSummary.CreatedDate;
                vm.CreatedBy = _ExpenseSummary.CreatedBy;
                vm.ModifiedDate = DateTime.Now;
                vm.ModifiedBy = _UserName;
                _context.Entry(_ExpenseSummary).CurrentValues.SetValues(vm);
                await _context.SaveChangesAsync();

                _JsonResultViewModel.AlertMessage = "Expense Updated Successfully. ID: " + _ExpenseSummary.Id;
                _JsonResultViewModel.IsSuccess = true;
                _JsonResultViewModel.Id = _ExpenseSummary.Id;
                return new JsonResult(_JsonResultViewModel);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message + "Operation Failed.";
                return new JsonResult(_JsonResultViewModel);
                throw;
            }
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Int64 id)
        {
            string _UserName = HttpContext.User.Identity.Name;
            try
            {
                var _ExpenseSummary = await _context.ExpenseSummary.FindAsync(id);
                _ExpenseSummary.ModifiedDate = DateTime.Now;
                _ExpenseSummary.ModifiedBy = _UserName;
                _ExpenseSummary.Cancelled = true;
                _context.Update(_ExpenseSummary);
                await _context.SaveChangesAsync();

                if (_ExpenseSummary.PaidAmount > 0)
                {
                    UpdateAccountViewModel _UpdateAccountViewModel = new()
                    {
                        AccUpdateType = AccAccountUpdateType.DebitRevart,
                        AccAccountNo = AccAccountInfo.DefaultAccount,
                        Amount = _ExpenseSummary.PaidAmount,
                        Credit = _ExpenseSummary.PaidAmount,
                        Debit = 0,
                        Type = "Regular Expense-Revart",
                        Reference = "Regular Expense Id: " + _ExpenseSummary.Id,
                        Description = "Regular Expense Deleted.",
                        UserName = _UserName
                    };
                    await _iCommon.UpdateAccoutDuringTran(_UpdateAccountViewModel);
                }
                return new JsonResult(_ExpenseSummary);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddExpenseDetails(ExpenseDetailsCRUDViewModel vm)
        {
            var _ExpenseSummaryCRUDViewModel = vm.ExpenseSummaryCRUDViewModel;
            vm.UserName = HttpContext.User.Identity.Name;
            vm = await _iPaymentService.AddExpenseDetails(vm);

            vm.ExpenseSummaryCRUDViewModel = await _iPaymentService.UpdateExpenseSummary(_ExpenseSummaryCRUDViewModel);
            return new JsonResult(vm);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateExpenseDetails(ExpenseDetailsCRUDViewModel vm)
        {
            var _ExpenseSummaryCRUDViewModel = vm.ExpenseSummaryCRUDViewModel;
            vm.UserName = HttpContext.User.Identity.Name;
            vm = await _iPaymentService.UpdateExpenseDetails(vm);

            vm.ExpenseSummaryCRUDViewModel = await _iPaymentService.UpdateExpenseSummary(_ExpenseSummaryCRUDViewModel);
            return new JsonResult(vm);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteExpenseDetails(ExpenseDetailsCRUDViewModel vm)
        {
            try
            {
                var _ExpenseDetails = await _context.ExpenseDetails.Where(x => x.Id == vm.Id).FirstOrDefaultAsync();
                _ExpenseDetails.ModifiedDate = DateTime.Now;
                _ExpenseDetails.ModifiedBy = HttpContext.User.Identity.Name;
                _ExpenseDetails.Cancelled = true;
                _context.Update(_ExpenseDetails);
                await _context.SaveChangesAsync();

                vm.ExpenseSummaryCRUDViewModel = await _iPaymentService.UpdateExpenseSummary(vm.ExpenseSummaryCRUDViewModel);
                return new JsonResult(vm);
            }
            catch (Exception) { throw; }
        }
        [HttpPost]
        public async Task<IActionResult> SaveExpensePaymentModeHistory(PaymentModeHistoryCRUDViewModel vm)
        {
            string UserName = HttpContext.User.Identity.Name;
            var _ExpenseSummaryCRUDViewModel = vm.ExpenseSummaryCRUDViewModel;
            vm.PaymentType = InvoicePaymentType.RegularExpensePayment;
            vm.CreatedBy = UserName;
            vm.ModifiedBy = UserName;
            vm = await _iPaymentService.CreatePaymentModeHistory(vm);

            vm.ExpenseSummaryCRUDViewModel = await _iPaymentService.UpdateExpenseSummary(_ExpenseSummaryCRUDViewModel);
            return new JsonResult(vm);
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePaymentModeHistory(PaymentModeHistoryCRUDViewModel vm)
        {
            string _UserName = HttpContext.User.Identity.Name;
            try
            {
                var _ExpenseSummaryCRUDViewModel = vm.ExpenseSummaryCRUDViewModel;
                var _PaymentModeHistory = await _context.PaymentModeHistory.Where(x => x.Id == vm.Id).FirstOrDefaultAsync();
                _PaymentModeHistory.ModifiedDate = DateTime.Now;
                _PaymentModeHistory.ModifiedBy = _UserName;
                _PaymentModeHistory.Cancelled = true;
                _context.Update(_PaymentModeHistory);
                await _context.SaveChangesAsync();

                vm.ExpenseSummaryCRUDViewModel = await _iPaymentService.UpdateExpenseSummary(_ExpenseSummaryCRUDViewModel);

                //Update AccAccount
                await _iPaymentService.UpdateAccAccountInDeletePaymentModeHistory(_PaymentModeHistory);
                return new JsonResult(vm);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}