using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models.DashboardViewModel;
using BusinessERP.Models.PaymentModeHistoryViewModel;
using BusinessERP.Models.PurchasesPaymentDetailViewModel;
using BusinessERP.Models.PurchasesPaymentViewModel;
using Microsoft.EntityFrameworkCore;
using UAParser;

namespace BusinessERP.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly ApplicationDbContext _context;
        public PurchaseService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ManagePurchasesPaymentViewModel> GetByPurchasesPaymentDetail(Int64 id)
        {
            try
            {
                ManagePurchasesPaymentViewModel vm = new ManagePurchasesPaymentViewModel();
                vm.PurchasesPaymentCRUDViewModel = await GetPurchasesPaymentList().Where(x => x.Id == id).FirstOrDefaultAsync();
                vm.listPurchasesPaymentDetailCRUDViewModel = await GetPurchasesPaymentDetailList().Where(x => x.PaymentId == id).ToListAsync();
                vm.listPaymentModeHistoryCRUDViewModel = GetPaymentModeHistory(InvoicePaymentType.PurchasesInvoicePayment).Where(x => x.PaymentId == id).ToList();
                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ManagePurchasesPaymentViewModel> GetByPurchasesPaymentDetailInReturn(Int64 id)
        {
            try
            {
                ManagePurchasesPaymentViewModel vm = new ManagePurchasesPaymentViewModel();
                vm.PurchasesPaymentCRUDViewModel = await GetPurchasesPaymentList().Where(x => x.Id == id).FirstOrDefaultAsync();
                vm.listPurchasesPaymentDetailCRUDViewModel = await GetPurchasesPaymentDetailListInReturn().Where(x => x.PaymentId == id).ToListAsync();
                vm.listPaymentModeHistoryCRUDViewModel = GetPaymentModeHistory(InvoicePaymentType.PurchasesInvoicePayment).Where(x => x.PaymentId == id).ToList();
                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<PurchasesPaymentReportViewModel> PrintPurchasesPaymentInvoice(Int64 id)
        {
            PurchasesPaymentReportViewModel vm = new()
            {
                PurchasesPaymentCRUDViewModel = GetPurchasesPaymentList().Where(x => x.Id == id).FirstOrDefault(),
                listPurchasesPaymentDetailCRUDViewModel = await GetPurchasesPaymentDetailList().Where(x => x.PaymentId == id).ToListAsync(),
                listPaymentModeHistoryCRUDViewModel = await GetPaymentModeHistory(InvoicePaymentType.PurchasesInvoicePayment).Where(x => x.PaymentId == id).ToListAsync()
            };
            vm.SupplierCRUDViewModel = _context.Supplier.Where(x => x.Id == vm.PurchasesPaymentCRUDViewModel.SupplierId).FirstOrDefault();
            vm.CompanyInfoCRUDViewModel = _context.CompanyInfo.FirstOrDefault(m => m.Id == 1);
            return vm;
        }
        public IQueryable<PurchasesPaymentCRUDViewModel> GetPurchasesPaymentGridData()
        {
            try
            {
                var result = (from _PurchasesPayment in _context.PurchasesPayment
                              join _Supplier in _context.Supplier on _PurchasesPayment.SupplierId equals _Supplier.Id
                              into _Supplier
                              from listSupplier in _Supplier.DefaultIfEmpty()
                              join _PaymentStatus in _context.PaymentStatus on _PurchasesPayment.PaymentStatus equals _PaymentStatus.Id
                              into _PaymentStatus
                              from listPaymentStatus in _PaymentStatus.DefaultIfEmpty()
                              where _PurchasesPayment.Cancelled == false && _PurchasesPayment.ReturnType != TranReturnType.FullReturn
                              select new PurchasesPaymentCRUDViewModel
                              {
                                  Id = _PurchasesPayment.Id,
                                  InvoiceNo = _PurchasesPayment.InvoiceNo,
                                  QuoteNo = _PurchasesPayment.QuoteNo,
                                  SupplierId = _PurchasesPayment.SupplierId,
                                  SupplierName = listSupplier.Name,

                                  SubTotal = _PurchasesPayment.SubTotal,
                                  DiscountAmount = _PurchasesPayment.DiscountAmount,
                                  VATAmount = _PurchasesPayment.VATAmount,
                                  GrandTotal = _PurchasesPayment.GrandTotal,
                                  PaidAmount = _PurchasesPayment.PaidAmount,
                                  DueAmount = _PurchasesPayment.DueAmount,
                                  Category = _PurchasesPayment.Category,
                                  CreatedDate = _PurchasesPayment.CreatedDate,
                                  PaymentStatus = _PurchasesPayment.PaymentStatus,
                                  PaymentStatusDisplay = listPaymentStatus.Name,
                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception) { throw; }
        }
        public IQueryable<PurchasesPaymentCRUDViewModel> GetPurchasesPaymentList()
        {
            try
            {
                var result = (from _PurchasesPayment in _context.PurchasesPayment
                              join _Supplier in _context.Supplier on _PurchasesPayment.SupplierId equals _Supplier.Id
                              into _Supplier
                              from listSupplier in _Supplier.DefaultIfEmpty()
                              join _PaymentStatus in _context.PaymentStatus on _PurchasesPayment.PaymentStatus equals _PaymentStatus.Id
                              into _PaymentStatus
                              from listPaymentStatus in _PaymentStatus.DefaultIfEmpty()
                              join _Currency in _context.Currency on _PurchasesPayment.CurrencyId equals _Currency.Id
                              into _Currency
                              from listCurrency in _Currency.DefaultIfEmpty()

                              join _Branch in _context.Branch on _PurchasesPayment.BranchId equals _Branch.Id
                              into _Branch
                              from listBranch in _Branch.DefaultIfEmpty()
                              where _PurchasesPayment.Cancelled == false
                              select new PurchasesPaymentCRUDViewModel
                              {
                                  Id = _PurchasesPayment.Id,
                                  SupplierId = _PurchasesPayment.SupplierId,
                                  SupplierName = listSupplier.Name,
                                  InvoiceNo = _PurchasesPayment.InvoiceNo,
                                  QuoteNo = _PurchasesPayment.QuoteNo,
                                  CommonCharge = _PurchasesPayment.CommonCharge,
                                  Discount = _PurchasesPayment.Discount,
                                  DiscountAmount = _PurchasesPayment.DiscountAmount,
                                  VAT = _PurchasesPayment.VAT,
                                  VATAmount = _PurchasesPayment.VATAmount,
                                  SubTotal = _PurchasesPayment.SubTotal,
                                  GrandTotal = _PurchasesPayment.GrandTotal,
                                  PaidAmount = _PurchasesPayment.PaidAmount,
                                  DueAmount = _PurchasesPayment.DueAmount,
                                  CurrencyId = _PurchasesPayment.CurrencyId,
                                  CurrencyName = listCurrency.Name,
                                  BranchId = _PurchasesPayment.BranchId,
                                  BranchName = listBranch.Name,
                                  CurrencySymbol = listCurrency.Symbol,
                                  PaymentStatus = _PurchasesPayment.PaymentStatus,
                                  PaymentStatusDisplay = listPaymentStatus.Name,
                                  Category = _PurchasesPayment.Category,
                                  PurchaseOrderNumber = _PurchasesPayment.PurchaseOrderNumber,
                                  CustomerNote = _PurchasesPayment.CustomerNote,
                                  PrivateNote = _PurchasesPayment.PrivateNote,
                                  ReturnType = _PurchasesPayment.ReturnType,

                                  CreatedDate = _PurchasesPayment.CreatedDate,
                                  CreatedBy = _PurchasesPayment.CreatedBy,
                                  ModifiedBy = _PurchasesPayment.ModifiedBy,

                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception) { throw; }
        }
        public IQueryable<PurchasesPaymentDetailCRUDViewModel> GetPurchasesPaymentDetailList()
        {
            try
            {
                var result = (from _PurchasesPaymentDetail in _context.PurchasesPaymentDetail
                              join _Items in _context.Items on _PurchasesPaymentDetail.ItemId equals _Items.Id
                              where _PurchasesPaymentDetail.Cancelled == false
                              select new PurchasesPaymentDetailCRUDViewModel
                              {
                                  Id = _PurchasesPaymentDetail.Id,
                                  PaymentId = _PurchasesPaymentDetail.PaymentId,
                                  ItemId = _PurchasesPaymentDetail.ItemId,
                                  ItemCode = _Items.Code,
                                  ItemName = _PurchasesPaymentDetail.ItemName,
                                  Quantity = _PurchasesPaymentDetail.Quantity,
                                  FreeQuantity = _PurchasesPaymentDetail.FreeQuantity,
                                  UnitPrice = _PurchasesPaymentDetail.UnitPrice,
                                  ItemVAT = _PurchasesPaymentDetail.ItemVAT,
                                  ItemVATAmount = _PurchasesPaymentDetail.ItemVATAmount,
                                  ItemDiscount = _PurchasesPaymentDetail.ItemDiscount,
                                  ItemDiscountAmount = _PurchasesPaymentDetail.ItemDiscountAmount,
                                  TotalAmount = _PurchasesPaymentDetail.TotalAmount,
                                  IsReturn = _PurchasesPaymentDetail.IsReturn,
                                  IsReturnDisplay = _PurchasesPaymentDetail.IsReturn == true ? "Yes" : "No",

                                  CreatedDate = _PurchasesPaymentDetail.CreatedDate
                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<PurchasesPaymentDetailCRUDViewModel> GetPurchasesPaymentDetailListInReturn()
        {
            try
            {
                var result = (from _PurchasesPaymentDetail in _context.PurchasesPaymentDetail
                              join _Items in _context.Items on _PurchasesPaymentDetail.ItemId equals _Items.Id
                              select new PurchasesPaymentDetailCRUDViewModel
                              {
                                  Id = _PurchasesPaymentDetail.Id,
                                  PaymentId = _PurchasesPaymentDetail.PaymentId,
                                  ItemId = _PurchasesPaymentDetail.ItemId,
                                  ItemCode = _Items.Code,
                                  ItemName = _PurchasesPaymentDetail.ItemName,
                                  Quantity = _PurchasesPaymentDetail.Quantity,
                                  FreeQuantity = _PurchasesPaymentDetail.FreeQuantity,
                                  UnitPrice = _PurchasesPaymentDetail.UnitPrice,
                                  ItemVAT = _PurchasesPaymentDetail.ItemVAT,
                                  ItemVATAmount = _PurchasesPaymentDetail.ItemVATAmount,
                                  ItemDiscount = _PurchasesPaymentDetail.ItemDiscount,
                                  ItemDiscountAmount = _PurchasesPaymentDetail.ItemDiscountAmount,
                                  TotalAmount = _PurchasesPaymentDetail.TotalAmount,
                                  IsReturn = _PurchasesPaymentDetail.IsReturn,
                                  IsReturnDisplay = _PurchasesPaymentDetail.IsReturn == true ? "Yes" : "No",

                                  CreatedDate = _PurchasesPaymentDetail.CreatedDate
                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<PurchasesPaymentGridViewModel> GetPurchasesSummaryReportList()
        {
            try
            {
                var result = (from _PurchasesPayment in _context.PurchasesPayment
                              join _Supplier in _context.Supplier on _PurchasesPayment.SupplierId equals _Supplier.Id
                              where _PurchasesPayment.Cancelled == false && _PurchasesPayment.Category == InvoiceType.RegularInvoice && _PurchasesPayment.ReturnType != TranReturnType.FullReturn
                              select new PurchasesPaymentGridViewModel
                              {
                                  Id = _PurchasesPayment.Id,
                                  SupplierName = _Supplier.Name,
                                  Discount = _PurchasesPayment.Discount,
                                  VATAmount = _PurchasesPayment.VATAmount,
                                  SubTotal = _PurchasesPayment.SubTotal,
                                  GrandTotal = _PurchasesPayment.GrandTotal,
                                  PaidAmount = _PurchasesPayment.PaidAmount,
                                  DueAmount = _PurchasesPayment.DueAmount,
                                  CreatedDate = _PurchasesPayment.CreatedDate,
                                  BranchId = _PurchasesPayment.BranchId,
                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception) { throw; }
        }
        public List<TransactionByViewModel> PurchasesTransactionBy(string DateType)
        {
            List<TransactionByViewModel> list = new();
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            DateTime[] DateList = null;

            var _PurchasesPayment = _context.PurchasesPayment.Where(x => x.Cancelled == false && x.Category == InvoiceType.RegularInvoice && x.ReturnType != TranReturnType.FullReturn);
            var _PurchasesPaymentDetail = _context.PurchasesPaymentDetail.Where(x => x.Cancelled == false && x.IsReturn == false);

            if (DateType == "Day")
            {
                DateList = Enumerable.Range(0, 30).Select(i => DateTime.Now.Date.AddDays(-i)).ToArray();
            }
            else if (DateType == "Month")
            {
                DateTime _DateTimeMonth = DateTime.Now.AddMonths(1);
                DateList = Enumerable.Range(1, 12).Select(n => _DateTimeMonth.AddMonths(-n)).ToArray();
            }
            else if (DateType == "Year")
            {
                DateTime _DateTime = DateTime.Now.AddYears(1);
                DateList = Enumerable.Range(1, 10).Select(n => _DateTime.AddYears(-n)).ToArray();
            }


            int SL = 1;
            foreach (var item in DateList)
            {
                TransactionByViewModel vm = new();

                vm.Id = SL;
                if (DateType == "Day")
                {
                    startDate = item;
                    endDate = item.AddDays(1).AddTicks(-1); ;
                    vm.TranDate = item.ToString("dddd, dd MMMM yyyy");
                }
                else if (DateType == "Month")
                {
                    startDate = new DateTime(item.Year, item.Month, 1);
                    endDate = startDate.AddMonths(1).AddTicks(-1);
                    vm.TranDate = item.ToString("MMMM") + "-" + item.Year;
                }
                else if (DateType == "Year")
                {
                    startDate = new DateTime(item.Year, 1, 1);
                    endDate = new DateTime(item.Year, 12, 31);
                    vm.TranDate = item.Year.ToString();
                }

                vm.QuantitySold = _PurchasesPaymentDetail.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate).Sum(x => x.Quantity);
                var SingleDayPayment = _PurchasesPayment.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate);
                vm.TotalTran = SingleDayPayment.Count();
                vm.TotalEarned = SingleDayPayment.Sum(x => x.PaidAmount - x.ChangedAmount);
                vm.TotalDue = SingleDayPayment.Sum(x => x.DueAmount);

                list.Add(vm);
                SL++;
            }
            return list;
        }
        public IQueryable<PaymentModeHistoryCRUDViewModel> GetPaymentModeHistory(int _InvoicePaymentType)
        {
            try
            {
                return (from _PaymentModeHistory in _context.PaymentModeHistory
                        where _PaymentModeHistory.Cancelled == false && _PaymentModeHistory.PaymentType == _InvoicePaymentType
                        select new PaymentModeHistoryCRUDViewModel
                        {
                            Id = _PaymentModeHistory.Id,
                            PaymentId = _PaymentModeHistory.PaymentId,
                            ModeOfPayment = _PaymentModeHistory.ModeOfPayment,
                            Amount = _PaymentModeHistory.Amount,
                            ReferenceNo = _PaymentModeHistory.ReferenceNo,
                            CreatedDate = _PaymentModeHistory.CreatedDate,
                            ModifiedDate = _PaymentModeHistory.ModifiedDate,
                            CreatedBy = _PaymentModeHistory.CreatedBy,
                            ModifiedBy = _PaymentModeHistory.ModifiedBy,
                            Cancelled = _PaymentModeHistory.Cancelled
                        }).OrderBy(x => x.CreatedDate);
            }
            catch (Exception) { throw; }
        }
    }
}