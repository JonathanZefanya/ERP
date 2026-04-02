using BusinessERP.Data;
using BusinessERP.Models;
using BusinessERP.Models.DashboardViewModel;
using BusinessERP.Models.PurchasesPaymentViewModel;
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
    public class PurchasesReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPurchaseService _iPurchaseService;
        private readonly ICommon _iCommon;
        public PurchasesReportController(ApplicationDbContext context, IPurchaseService iPurchaseService, ICommon iCommon)
        {
            _context = context;
            _iPurchaseService = iPurchaseService;
            _iCommon = iCommon;
        }

        [Authorize(Roles = MainMenu.PurchasesTransactionByDay.RoleName)]
        [HttpGet]
        public IActionResult PurchasesTransactionByDay()
        {
            try
            {
                TransactionByViewModel vm = new();
                vm.listTransactionByViewModel = _iPurchaseService.PurchasesTransactionBy("Day");
                ViewBag.ReportTitle = "Purchases Transaction By Day";
                return View("PurchasesTransactionBy", vm);
            }
            catch (Exception) { throw; }
        }
        [Authorize(Roles = MainMenu.PurchasesTransactionByMonth.RoleName)]
        [HttpGet]
        public IActionResult PurchasesTransactionByMonth()
        {
            try
            {
                TransactionByViewModel vm = new();
                vm.listTransactionByViewModel = _iPurchaseService.PurchasesTransactionBy("Month");
                ViewBag.ReportTitle = "Purchases Transaction By Month";
                return View("PurchasesTransactionBy", vm);
            }
            catch (Exception) { throw; }
        }
        [Authorize(Roles = MainMenu.PurchasesTransactionByYear.RoleName)]
        [HttpGet]
        public IActionResult PurchasesTransactionByYear()
        {
            try
            {
                TransactionByViewModel vm = new();
                vm.listTransactionByViewModel = _iPurchaseService.PurchasesTransactionBy("Year");
                ViewBag.ReportTitle = "Purchases Transaction By Year";
                return View("PurchasesTransactionBy", vm);
            }
            catch (Exception) { throw; }
        }


        [Authorize(Roles = MainMenu.PurchasesSummary.RoleName)]
        [HttpGet]
        public IActionResult PurchasesSummary()
        {
            try
            {
                ViewBag.ddlBranch = new SelectList(_iCommon.GetTableData<Branch>(x => x.Cancelled == false).ToList(), "Id", "Name");
                return View();
            }
            catch (Exception) { throw; }
        }
        [HttpPost]
        public IActionResult GetDataTablePurchasesSummaryList(bool IsFilterData, Int64 BranchId)
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

                IQueryable<PurchasesPaymentGridViewModel> _GetGridItem;
                if (IsFilterData)
                {
                    _GetGridItem = _iPurchaseService.GetPurchasesSummaryReportList().Where(obj => obj.BranchId == BranchId);
                }
                else
                {
                    _GetGridItem = _iPurchaseService.GetPurchasesSummaryReportList();
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
                    || obj.SupplierName.ToLower().Contains(searchValue)
                    || obj.Discount.ToString().Contains(searchValue)
                    || obj.VATAmount.ToString().Contains(searchValue)
                    || obj.SubTotal.ToString().Contains(searchValue)
                    || obj.GrandTotal.ToString().Contains(searchValue)
                    || obj.PaidAmount.ToString().Contains(searchValue)
                    || obj.DueAmount.ToString().Contains(searchValue)

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
        [Authorize(Roles = MainMenu.PurchasesDetail.RoleName)]
        [HttpGet]
        public IActionResult PurchasesDetail()
        {
            try
            {
                return View();
            }
            catch (Exception) { throw; }
        }
        [HttpPost]
        public IActionResult GetDataTablePurchasesDetailList()
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

                var _GetGridItem = _iPurchaseService.GetPurchasesPaymentDetailList().Where(x => x.IsReturn == false);
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
                    || obj.PaymentId.ToString().Contains(searchValue)
                    || obj.ItemId.ToString().Contains(searchValue)
                    || obj.ItemName.ToLower().Contains(searchValue)
                    || obj.Quantity.ToString().Contains(searchValue)
                    || obj.UnitPrice.ToString().Contains(searchValue)
                    || obj.TotalAmount.ToString().Contains(searchValue)

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
    }
}