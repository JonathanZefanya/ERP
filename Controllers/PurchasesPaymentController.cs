using BusinessERP.ConHelper;
using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models;
using BusinessERP.Models.CommonViewModel;
using BusinessERP.Models.ItemsViewModel;
using BusinessERP.Models.PaymentModeHistoryViewModel;
using BusinessERP.Models.PaymentViewModel;
using BusinessERP.Models.PurchasesPaymentDetailViewModel;
using BusinessERP.Models.PurchasesPaymentViewModel;
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
    public class PurchasesPaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly IPaymentService _iPaymentService;
        private readonly IPurchaseService _iPurchaseService;
        private string _StartDate = null;
        private string _EndDate = null;

        public PurchasesPaymentController(ApplicationDbContext context, ICommon iCommon, IPaymentService iPaymentService, IPurchaseService iPurchaseService)
        {
            _context = context;
            _iCommon = iCommon;
            _iPaymentService = iPaymentService;
            _iPurchaseService = iPurchaseService;
        }

        [Authorize(Roles = Pages.MainMenu.PurchasesPayment.RoleName)]
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


                IQueryable<PurchasesPaymentCRUDViewModel> _GetGridItem = null;
                if (_StartDate != null && _EndDate != null && _StartDate != "" && _EndDate != "")
                {
                    _GetGridItem = _iPurchaseService.GetPurchasesPaymentGridData().Where(x => x.Category == InvoiceType.RegularInvoice && x.CreatedDate >= Convert.ToDateTime(_StartDate) && x.CreatedDate <= Convert.ToDateTime(_EndDate).AddDays(1));
                }
                else
                {
                    _GetGridItem = _iPurchaseService.GetPurchasesPaymentGridData().Where(x => x.Category == InvoiceType.RegularInvoice);
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
        public async Task<IActionResult> Details(Int64 id)
        {
            var result = await _iPurchaseService.GetByPurchasesPaymentDetail(id);
            if (result == null) return NotFound();
            return PartialView("_Detail", result);
        }
        [HttpGet]
        public async Task<IActionResult> GetPurchaseReturnItemDetails(Int64 id)
        {
            var result = await _iPurchaseService.GetByPurchasesPaymentDetailInReturn(id);
            if (result == null) return NotFound();
            return PartialView("_Detail", result);
        }
        [HttpGet]
        public async Task<IActionResult> GetPurchasesPaymentSummary(Int64 Id)
        {
            var result = await _context.PurchasesPayment.Where(x => x.Id == Id).FirstOrDefaultAsync();
            return new JsonResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> POSReport(Int64 id)
        {
            var _PrintPurchasesPaymentInvoice = await _iPurchaseService.PrintPurchasesPaymentInvoice(id);
            return PartialView("_PaymentInvoicePOS", _PrintPurchasesPaymentInvoice);
        }
        [HttpGet]
        public async Task<IActionResult> PrintPOSReport(Int64 id)
        {
            var _PrintPurchasesPaymentInvoice = await _iPurchaseService.PrintPurchasesPaymentInvoice(id);
            return View("ThermalPaymentInvoicePOS", _PrintPurchasesPaymentInvoice);
        }
        [HttpGet]
        public async Task<IActionResult> PrintPurchasesPaymentInvoice(Int64 _PaymentId)
        {
            var _PrintPurchasesPaymentInvoice = await _iPurchaseService.PrintPurchasesPaymentInvoice(_PaymentId);
            return View(_PrintPurchasesPaymentInvoice);
        }
        [HttpGet]
        public async Task<IActionResult> ThermalPrintPaymentInvoice(Int64 _PaymentId)
        {
            var _PrintPurchasesPaymentInvoice = await _iPurchaseService.PrintPurchasesPaymentInvoice(_PaymentId);
            return View(_PrintPurchasesPaymentInvoice);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(Int64 id)
        {
            var _IsVat = _context.CompanyInfo.FirstOrDefault(m => m.Cancelled == false).IsVat;
            ViewBag.ddlInventoryItem = new SelectList(_iCommon.LoadddlInventoryItem(_IsVat), "Id", "Name");
            ViewBag._LoadddlSupplier = new SelectList(_iCommon.LoadddlSupplier(), "Id", "Name");
            ViewBag._LoadddlPaymentType = new SelectList(_iCommon.LoadddlPaymentType(), "Id", "Name");
            ViewBag.GetddlPaymentStatus = new SelectList(_iCommon.GetddlPaymentStatus(), "Id", "Name");
            ViewBag.GetddlCustomerType = new SelectList(_iCommon.GetddlCustomerType(), "Id", "Name");

            ViewBag.ddlCurrency = new SelectList(_iCommon.LoadddlCurrencyItem(), "Id", "Name");
            ViewBag.ddlBranch = new SelectList(_iCommon.GetTableData<Branch>(x => x.Cancelled == false).ToList(), "Id", "Name");

            ManagePurchasesPaymentViewModel vm = new();
            PurchasesPayment _PurchasesPayment = new();
            vm.PurchasesPaymentCRUDViewModel = new();
            vm.PurchasesPaymentCRUDViewModel.InvoiceNo = _iPaymentService.GetPurchasesPaymentNo(InvoiceType.RegularInvoice);
            vm.PurchasesPaymentCRUDViewModel.QuoteNo = _iPaymentService.GetPurchasesQuoteNo(InvoiceType.QueoteInvoice);
            //vm.PaymentCRUDViewModel.VAT = _context.CompanyInfo.FirstOrDefault(m => m.Id == 1).VatPercentage;
            vm.PurchasesPaymentCRUDViewModel.CurrencySymbol = _context.Currency.FirstOrDefault(m => m.IsDefault == true).Symbol;

            if (id > 0)
            {
                vm = await _iPurchaseService.GetByPurchasesPaymentDetail(id);
            }
            else
            {
                string strUserName = HttpContext.User.Identity.Name;
                //Set Branch By User
                var _UserName = User.Identity.Name;
                vm.PurchasesPaymentCRUDViewModel.BranchId = await _iCommon.GetBranchIdByUserName(_UserName);
                _PurchasesPayment = vm.PurchasesPaymentCRUDViewModel;

                _PurchasesPayment.GrandTotal = 0;
                _PurchasesPayment.Category = InvoiceType.DraftInvoice;
                _PurchasesPayment.SupplierId = 1;
                _PurchasesPayment.CreatedDate = DateTime.Now;
                _PurchasesPayment.ModifiedDate = DateTime.Now;
                _PurchasesPayment.CreatedBy = strUserName;
                _PurchasesPayment.ModifiedBy = strUserName;
                _context.Add(_PurchasesPayment);
                await _context.SaveChangesAsync();

                vm.PurchasesPaymentCRUDViewModel.Id = _PurchasesPayment.Id;
                vm.PurchasesPaymentCRUDViewModel.CreatedDate = _PurchasesPayment.CreatedDate;
                vm.PurchasesPaymentCRUDViewModel.CreatedBy = _PurchasesPayment.CreatedBy;
            }

            vm.PurchasesPaymentCRUDViewModel.IsVat = _IsVat == true ? "Yes" : "No";
            vm.PurchasesPaymentCRUDViewModel.QuoteNoRef = id;
            var _IsInRole = User.IsInRole("Admin");
            vm.PurchasesPaymentCRUDViewModel.RoleName = _IsInRole == true ? "Admin" : "Other";
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(PurchasesPaymentCRUDViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                string strUserName = HttpContext.User.Identity.Name;

                if (vm.Category == InvoiceType.RegularInvoice)
                {
                    vm.QuoteNo = null;
                    if (vm.InvoiceNo.Contains("D"))
                    {
                        vm.InvoiceNo = _iPaymentService.GetPurchasesPaymentNo(InvoiceType.RegularInvoice);
                    }
                }
                else if (vm.Category == InvoiceType.QueoteInvoice)
                {
                    vm.InvoiceNo = null;
                }

                var result = await _iPaymentService.UpdatePurchasesPayment(vm);
                _JsonResultViewModel.CurrentURL = vm.CurrentURL;
                _JsonResultViewModel.IsSuccess = true;
                _JsonResultViewModel.Id = result.Id;
                _JsonResultViewModel.ModelObject = vm;

                if (vm.Category == InvoiceType.DraftInvoice)
                {
                    _JsonResultViewModel.AlertMessage = "Draft Purchases Invoice Saved Successfully. Invoice ID: " + vm.InvoiceNo;
                }
                else if (vm.Category == InvoiceType.RegularInvoice)
                {
                    _JsonResultViewModel.AlertMessage = "Purchases Invoice Saved Successfully. Invoice ID: " + vm.InvoiceNo;
                }
                else
                {
                    _JsonResultViewModel.AlertMessage = "Quote Purchases Invoice Saved Successfully. Quote ID: " + vm.QuoteNo;
                }
                return new JsonResult(_JsonResultViewModel);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message;
                return new JsonResult(_JsonResultViewModel);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPurchasesPaymentDetail(PurchasesPaymentDetailCRUDViewModel vm)
        {
            var _PurchasesPaymentCRUDViewModel = vm.PurchasesPaymentCRUDViewModel;
            vm = await CreatePurchasesPaymentsDetail(vm);

            var _PurchasesPayment = _context.PurchasesPayment.Where(x => x.Id == _PurchasesPaymentCRUDViewModel.Id).FirstOrDefault();
            _PurchasesPaymentCRUDViewModel.Category = _PurchasesPayment.Category;
            vm.PurchasesPaymentCRUDViewModel = await _iPaymentService.UpdatePurchasesPayment(_PurchasesPaymentCRUDViewModel);

            ItemTranViewModel _ItemTranViewModel = new()
            {
                ItemId = vm.ItemId,
                IsAddition = true,
                TranQuantity = vm.Quantity + vm.FreeQuantity,
                ActionMessage = "Add new Purchases, Supplier ID: " + vm.PurchasesPaymentCRUDViewModel.SupplierId,
                CurrentUserName = HttpContext.User.Identity.Name
            };
            await _iCommon.CurrentItemsUpdate(_ItemTranViewModel);
            return new JsonResult(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePurchasesPaymentDetail(PurchasesPaymentDetailCRUDViewModel vm)
        {
            var _PaymentCRUDViewModel = vm.PurchasesPaymentCRUDViewModel;
            vm.UserName = HttpContext.User.Identity.Name;

            vm = await _iPaymentService.UpdatePurchasesPaymentDetail(vm);
            vm.PurchasesPaymentCRUDViewModel = await _iPaymentService.UpdatePurchasesPayment(_PaymentCRUDViewModel);
            return new JsonResult(vm);
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePaymentDetailIMENo(PurchasesPaymentDetailCRUDViewModel vm)
        {
            var _PurchasesPaymentDetail = await _context.PurchasesPaymentDetail.FindAsync(vm.Id);
            _PurchasesPaymentDetail.ItemName = vm.ItemName;
            _PurchasesPaymentDetail.ModifiedDate = DateTime.Now;
            _PurchasesPaymentDetail.ModifiedBy = HttpContext.User.Identity.Name;
            _context.Update(_PurchasesPaymentDetail);
            await _context.SaveChangesAsync();
            return new JsonResult(_PurchasesPaymentDetail);
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePurchasesPaymentDetail(PurchasesPaymentDetailCRUDViewModel vm)
        {
            try
            {
                var _PurchasesPaymentDetail = await _context.PurchasesPaymentDetail.Where(x => x.Id == vm.Id).FirstOrDefaultAsync();
                _PurchasesPaymentDetail.ModifiedDate = DateTime.Now;
                _PurchasesPaymentDetail.ModifiedBy = HttpContext.User.Identity.Name;
                _PurchasesPaymentDetail.Cancelled = true;
                _context.Update(_PurchasesPaymentDetail);
                await _context.SaveChangesAsync();

                var result = await _iPaymentService.UpdatePurchasesPayment(vm.PurchasesPaymentCRUDViewModel);

                ItemTranViewModel _ItemTranViewModel = new ItemTranViewModel
                {
                    ItemId = _PurchasesPaymentDetail.ItemId,
                    TranQuantity = _PurchasesPaymentDetail.Quantity,
                    ActionMessage = "Delete Purchases item, Supplier ID: " + vm.PurchasesPaymentCRUDViewModel.SupplierId,
                    CurrentUserName = HttpContext.User.Identity.Name
                };
                await _iCommon.CurrentItemsUpdate(_ItemTranViewModel);
                return new JsonResult(result);
            }
            catch (Exception) { throw; }
        }

        private async Task<PurchasesPaymentDetail> CreatePurchasesPaymentsDetail(PurchasesPaymentDetail _PurchasesPaymentDetail)
        {
            var _Total = _PurchasesPaymentDetail.Quantity * _PurchasesPaymentDetail.UnitPrice;
            var _ItemDiscountAmount = (_PurchasesPaymentDetail.ItemDiscount / 100) * _Total;
            var WithoutDiscountTotal = _Total - _ItemDiscountAmount;
            var _ItemVATAmount = (_PurchasesPaymentDetail.ItemVAT / 100) * WithoutDiscountTotal;


            _PurchasesPaymentDetail.ItemVATAmount = Math.Round((double)_ItemVATAmount, 2);
            _PurchasesPaymentDetail.ItemDiscountAmount = Math.Round((double)_ItemDiscountAmount, 2);
            _PurchasesPaymentDetail.TotalAmount = Math.Round((double)(WithoutDiscountTotal + _ItemVATAmount), 2);

            _PurchasesPaymentDetail.CreatedDate = DateTime.Now;
            _PurchasesPaymentDetail.ModifiedDate = DateTime.Now;
            _PurchasesPaymentDetail.CreatedBy = HttpContext.User.Identity.Name;
            _PurchasesPaymentDetail.ModifiedBy = HttpContext.User.Identity.Name;
            _context.Add(_PurchasesPaymentDetail);
            var result = await _context.SaveChangesAsync();
            return _PurchasesPaymentDetail;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _PurchasesPayment = await _context.PurchasesPayment.FindAsync(id);
                _PurchasesPayment.ModifiedDate = DateTime.Now;
                _PurchasesPayment.ModifiedBy = HttpContext.User.Identity.Name;
                _PurchasesPayment.Cancelled = true;
                _context.Update(_PurchasesPayment);
                await _context.SaveChangesAsync();

                var _GetPaymentDetailList = await _iPurchaseService.GetPurchasesPaymentDetailList().Where(x => x.PaymentId == _PurchasesPayment.Id).ToListAsync();
                foreach (var item in _GetPaymentDetailList)
                {
                    ItemTranViewModel _ItemTranViewModel = new ItemTranViewModel
                    {
                        ItemId = item.ItemId,
                        TranQuantity = item.Quantity,
                        ActionMessage = "Deleted new purchase: " + _PurchasesPayment.InvoiceNo,
                        CurrentUserName = HttpContext.User.Identity.Name
                    };
                    await _iCommon.CurrentItemsUpdate(_ItemTranViewModel);
                }
                
                var listPaymentModeHistory = await _context.PaymentModeHistory.Where(x => x.PaymentId == id).ToListAsync();
                foreach (var itemPaymentModeHistory in listPaymentModeHistory)
                {
                    await _iPaymentService.UpdateAccAccountInDeletePaymentModeHistory(itemPaymentModeHistory);
                }
                return new JsonResult(_PurchasesPayment);
            }
            catch (Exception) { throw; }
        }
        [HttpPost]
        public async Task<IActionResult> SavePurchasesPaymentModeHistory(PaymentModeHistoryCRUDViewModel vm)
        {
            string UserName = HttpContext.User.Identity.Name;
            var _PurchasesPaymentCRUDViewModel = vm.PurchasesPaymentCRUDViewModel;
            vm.PaymentType = InvoicePaymentType.PurchasesInvoicePayment;
            vm.CreatedBy = UserName;
            vm.ModifiedBy = UserName;
            vm = await _iPaymentService.CreatePaymentModeHistory(vm);

            vm.PurchasesPaymentCRUDViewModel = await _iPaymentService.UpdatePurchasesPayment(_PurchasesPaymentCRUDViewModel);
            return new JsonResult(vm);
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePurchasesPaymentModeHistory(PaymentModeHistoryCRUDViewModel vm)
        {
            var _PurchasesPaymentCRUDViewModel = vm.PurchasesPaymentCRUDViewModel;
            var _PaymentModeHistory = await _context.PaymentModeHistory.Where(x => x.Id == vm.Id && vm.PaymentType == vm.PaymentType).FirstOrDefaultAsync();
            _PaymentModeHistory.ModifiedDate = DateTime.Now;
            _PaymentModeHistory.ModifiedBy = HttpContext.User.Identity.Name;
            _PaymentModeHistory.Cancelled = true;
            _context.Update(_PaymentModeHistory);
            await _context.SaveChangesAsync();

            vm.PurchasesPaymentCRUDViewModel = await _iPaymentService.UpdatePurchasesPayment(_PurchasesPaymentCRUDViewModel);

            //Update AccAccount
            await _iPaymentService.UpdateAccAccountInDeletePaymentModeHistory(_PaymentModeHistory);
            return new JsonResult(vm);
        }

        [HttpGet]
        public async Task<IActionResult> GetPriceModel(int Id)
        {
            var result = await _iCommon.LoadddlInventoryItem(true).ToListAsync();
            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetItemByItemBarcode(string ItemBarcode)
        {
            var _Items = await _context.Items.Where(x => x.Code == ItemBarcode).FirstOrDefaultAsync();
            return new JsonResult(_Items);
        }
        [HttpGet]
        public IActionResult GetCustomerHistory(Int64 CustomerId)
        {
            var result = _iPaymentService.GetCustomerHistory(CustomerId);
            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentSummaryData()
        {
            PaymentCRUDViewModel _PaymentCRUDViewModel = new();
            var _Payment = _context.Payment.Where(x => x.Cancelled == false);

            _PaymentCRUDViewModel.SubTotal = await _Payment.SumAsync(x => x.SubTotal);
            _PaymentCRUDViewModel.DiscountAmount = await _Payment.SumAsync(x => x.DiscountAmount);
            _PaymentCRUDViewModel.VATAmount = await _Payment.SumAsync(x => x.VATAmount);
            _PaymentCRUDViewModel.GrandTotal = await _Payment.SumAsync(x => x.GrandTotal);

            _PaymentCRUDViewModel.DueAmount = await _Payment.SumAsync(x => x.DueAmount);
            _PaymentCRUDViewModel.PaidAmount = await _Payment.SumAsync(x => x.PaidAmount);
            _PaymentCRUDViewModel.ChangedAmount = await _Payment.SumAsync(x => x.ChangedAmount);
            return new JsonResult(_PaymentCRUDViewModel);
        }
    }
}
