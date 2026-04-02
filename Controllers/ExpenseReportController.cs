using BusinessERP.ConHelper;
using BusinessERP.Data;
using BusinessERP.Models;
using BusinessERP.Models.DashboardViewModel;
using BusinessERP.Models.ExpenseSummaryViewModel;
using BusinessERP.Pages;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ExpenseReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly IPaymentService _iDBOperation;

        public ExpenseReportController(ApplicationDbContext context, ICommon iCommon, IPaymentService iPaymentService)
        {
            _context = context;
            _iCommon = iCommon;
            _iDBOperation = iPaymentService;
        }

        [Authorize(Roles = MainMenu.ExpenseSummaryReport.RoleName)]
        [HttpGet]
        public IActionResult ExpenseSummaryReport()
        {
            ViewBag.ddlBranch = new SelectList(_iCommon.GetTableData<Branch>(x => x.Cancelled == false).ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult GetExpenseSummaryReportData(bool IsFilterData, Int64 BranchId)
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


                IQueryable<ExpenseSummaryCRUDViewModel> _GetGridItem;
                if (IsFilterData)
                {
                    _GetGridItem = _iCommon.GetExpenseSummaryGridItem().Where(obj => obj.BranchId == BranchId);
                }
                else
                {
                    _GetGridItem = _iCommon.GetExpenseSummaryGridItem();
                }

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
        
        
        [Authorize(Roles = MainMenu.ExpenseDetailsReport.RoleName)]
        [HttpGet]
        public IActionResult ExpenseDetailsReport()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult GetExpenseDetailsReportData(bool IsFilterData, int ExpenseTypeId)
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

                IQueryable<ExpenseDetailsCRUDViewModel> _GetExpenseDetailsList;
                if(IsFilterData)
                {
                    _GetExpenseDetailsList = _iCommon.GetExpenseDetailsList().Where(obj => obj.ExpenseTypeId == ExpenseTypeId);
                }
                else 
                {
                    _GetExpenseDetailsList = _iCommon.GetExpenseDetailsList();
                }

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetExpenseDetailsList = _GetExpenseDetailsList.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetExpenseDetailsList = _GetExpenseDetailsList.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.ExpenseType.ToLower().Contains(searchValue)
                    || obj.Description.ToLower().Contains(searchValue)
                    || obj.Quantity.ToString().ToLower().Contains(searchValue)
                    || obj.UnitPrice.ToString().ToLower().Contains(searchValue)
                    || obj.TotalPrice.ToString().ToLower().Contains(searchValue)
                    
                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetExpenseDetailsList.Count();

                var result = _GetExpenseDetailsList.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public JsonResult GetExpenseTypedlList()
        {
            try
            {
                var result = _iCommon.LoadddlExpenseType().ToList();
                return new JsonResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [Authorize(Roles = MainMenu.ExpenseByDay.RoleName)]
        [HttpGet]
        public IActionResult ExpenseByDay()
        {
            try
            {
                ExpenseByViewModel vm = new();
                vm.listExpenseByViewModel = _iCommon.ExpenseByDate("Day");
                ViewBag.ReportTitle = "Expense By Day";
                return View("ExpenseByDate", vm);
            }
            catch (Exception) { throw; }
        }
        [Authorize(Roles = MainMenu.ExpenseByMonth.RoleName)]
        [HttpGet]
        public IActionResult ExpenseByMonth()
        {
            try
            {
                ExpenseByViewModel vm = new();
                vm.listExpenseByViewModel = _iCommon.ExpenseByDate("Month");
                ViewBag.ReportTitle = "Expense By Month";
                return View("ExpenseByDate", vm);
            }
            catch (Exception) { throw; }
        }
        [Authorize(Roles = MainMenu.ExpenseByYear.RoleName)]
        [HttpGet]
        public IActionResult ExpenseByYear()
        {
            try
            {
                ExpenseByViewModel vm = new();
                vm.listExpenseByViewModel = _iCommon.ExpenseByDate("Year");
                ViewBag.ReportTitle = "Expense By Year";
                return View("ExpenseByDate", vm);
            }
            catch (Exception) { throw; }
        }
    }
}