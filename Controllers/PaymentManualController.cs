using BusinessERP.ConHelper;
using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models;
using BusinessERP.Models.PaymentViewModel;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Dynamic.Core;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class PaymentManualController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly ISalesService _iSalesService;
        private readonly IPaymentService _iDBOperation;
        private string _StartDate = null;
        private string _EndDate = null;

        public PaymentManualController(ApplicationDbContext context, ICommon iCommon, IPaymentService iPaymentService, ISalesService iSalesService)
        {
            _context = context;
            _iCommon = iCommon;
            _iSalesService = iSalesService;
            _iDBOperation = iPaymentService;
        }
        [Authorize(Roles = Pages.MainMenu.ManualInvoice.RoleName)]
        [HttpGet]
        public IActionResult Index(string StartDate, string EndDate)
        {
            if (StartDate != null && EndDate != null)
            {
                HttpContext.Session.SetString("_StartDate", StartDate);
                HttpContext.Session.SetString("_EndDate", EndDate);
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;
            }
            else
            {
                HttpContext.Session.SetString("_StartDate", string.Empty);
                HttpContext.Session.SetString("_EndDate", string.Empty);
                ViewBag.StartDate = "Min";
                ViewBag.EndDate = "Max";
            }
            return View();
        }

        [HttpPost]
        public IActionResult GetDataTabelData()
        {
            try
            {
                _StartDate = this.HttpContext.Session.GetString("_StartDate");
                _EndDate = this.HttpContext.Session.GetString("_EndDate");

                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int resultTotal = 0;


                IQueryable<PaymentCRUDViewModel> _GetGridItem = null;
                if (_StartDate != null && _EndDate != null && _StartDate != "" && _EndDate != "")
                {
                    _GetGridItem = _iSalesService.GetPaymentGridData().Where(x => x.Category == InvoiceType.ManualInvoice && x.CreatedDate >= Convert.ToDateTime(_StartDate) && x.CreatedDate <= Convert.ToDateTime(_EndDate).AddDays(1));
                }
                else
                {
                    _GetGridItem = _iSalesService.GetPaymentGridData().Where(x => x.Category == InvoiceType.ManualInvoice);
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
                    || obj.CustomerName.ToLower().Contains(searchValue)
                    || obj.SubTotal.ToString().ToLower().Contains(searchValue)
                    || obj.DiscountAmount.ToString().ToLower().Contains(searchValue)
                    || obj.VATAmount.ToString().ToLower().Contains(searchValue)
                    || obj.GrandTotal.ToString().ToLower().Contains(searchValue)
                    || obj.PaidAmount.ToString().ToLower().Contains(searchValue)
                    || obj.DueAmount.ToString().ToLower().Contains(searchValue)
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
        public async Task<IActionResult> AddEdit(Int64 id)
        {
            try
            {
                var _IsVat = _context.CompanyInfo.FirstOrDefault(m => m.Cancelled == false).IsVat;
                //ViewBag.ddlInventoryItem = new SelectList(_iCommon.LoadddlInventoryItem(_IsVat), "Id", "Name");
                ViewBag._LoadddlCustomerInfo = new SelectList(_iCommon.LoadddlCustomerInfo(), "Id", "Name");
                ViewBag._LoadddlPaymentType = new SelectList(_iCommon.LoadddlPaymentType(), "Id", "Name");
                ViewBag.GetddlPaymentStatus = new SelectList(_iCommon.GetddlPaymentStatus(), "Id", "Name");
                ViewBag.GetddlCustomerType = new SelectList(_iCommon.GetddlCustomerType(), "Id", "Name");

                ViewBag.ddlCurrency = new SelectList(_iCommon.LoadddlCurrencyItem(), "Id", "Name");
                ViewBag.ddlBranch = new SelectList(_iCommon.GetTableData<Branch>(x => x.Cancelled == false).ToList(), "Id", "Name");

                ManagePaymentViewModel vm = new();
                if (id > 0)
                {
                    vm = await _iSalesService.GetByPaymentDetail(id);
                }
                else
                {
                    Payment _Payment = new();
                    vm.PaymentCRUDViewModel = new();
                    vm.PaymentCRUDViewModel.InvoiceNo = _iDBOperation.GetInvoiceNo(InvoiceType.RegularInvoice);
                    vm.PaymentCRUDViewModel.QuoteNo = _iDBOperation.GetQuoteNo(InvoiceType.QueoteInvoice);
                    vm.PaymentCRUDViewModel.CurrencySymbol = _context.Currency.FirstOrDefault(m => m.IsDefault == true).Symbol;

                    //Set Branch By User
                    var _UserName = User.Identity.Name;
                    vm.PaymentCRUDViewModel.BranchId = await _iCommon.GetBranchIdByUserName(_UserName);

                    vm.PaymentCRUDViewModel.UserName = _UserName;
                    _Payment = await _iDBOperation.CreateManualInvoice(vm.PaymentCRUDViewModel);

                    vm.PaymentCRUDViewModel.Id = _Payment.Id;
                    vm.PaymentCRUDViewModel.Category = InvoiceType.ManualInvoice;
                    vm.PaymentCRUDViewModel.CreatedDate = _Payment.CreatedDate;
                    vm.PaymentCRUDViewModel.CreatedBy = _Payment.CreatedBy;
                }

                vm.PaymentCRUDViewModel.IsVat = _IsVat == true ? "Yes" : "No";
                vm.PaymentCRUDViewModel.QuoteNoRef = id;
                var _IsInRole = User.IsInRole("Admin");
                vm.PaymentCRUDViewModel.RoleName = _IsInRole == true ? "Admin" : "Other";
                return PartialView("_AddEdit", vm);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
