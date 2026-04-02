using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models.DashboardViewModel;
using BusinessERP.Models.PaymentDetailViewModel;
using BusinessERP.Models.PaymentModeHistoryViewModel;
using BusinessERP.Models.PaymentViewModel;
using BusinessERP.Models.ReturnLogViewModel;
using Microsoft.EntityFrameworkCore;
using UAParser;

namespace BusinessERP.Services
{
    public class SalesService : ISalesService
    {
        private readonly ApplicationDbContext _context;
        public SalesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ManagePaymentViewModel> GetByPaymentDetail(Int64 id)
        {
            try
            {
                ManagePaymentViewModel vm = new();
                vm.PaymentCRUDViewModel = await GetPaymentList().Where(x => x.Id == id).FirstOrDefaultAsync();
                vm.listPaymentDetailCRUDViewModel = await GetPaymentDetailList().Where(x => x.PaymentId == id).ToListAsync();
                vm.listPaymentModeHistoryCRUDViewModel = GetPaymentModeHistory(InvoicePaymentType.SalesInvoicePayment).Where(x => x.PaymentId == id).ToList();
                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ManagePaymentViewModel> GetByPaymentDetailInReturn(Int64 id)
        {
            try
            {
                ManagePaymentViewModel vm = new ManagePaymentViewModel();
                vm.PaymentCRUDViewModel = await GetPaymentList().Where(x => x.Id == id).FirstOrDefaultAsync();
                vm.listPaymentDetailCRUDViewModel = await GetPaymentDetailListInReturn().Where(x => x.PaymentId == id).ToListAsync();
                vm.listPaymentModeHistoryCRUDViewModel = GetPaymentModeHistory(InvoicePaymentType.SalesInvoicePayment).Where(x => x.PaymentId == id).ToList();
                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<PaymentReportViewModel> PrintPaymentInvoice(Int64 id)
        {
            PaymentReportViewModel vm = new();
            vm.PaymentCRUDViewModel = GetPaymentList().Where(x => x.Id == id).FirstOrDefault();
            vm.listPaymentDetailCRUDViewModel = await GetPaymentDetailList().Where(x => x.PaymentId == id).ToListAsync();
            vm.listPaymentModeHistoryCRUDViewModel = await GetPaymentModeHistory(InvoicePaymentType.SalesInvoicePayment).Where(x => x.PaymentId == id).ToListAsync();

            vm.CustomerInfoCRUDViewModel = _context.CustomerInfo.Where(x => x.Id == vm.PaymentCRUDViewModel.CustomerId).FirstOrDefault();
            vm.CompanyInfoCRUDViewModel = _context.CompanyInfo.FirstOrDefault(m => m.Id == 1);
            vm.PaymentMode = Utility.GetModeOfPaymentCommaSeparated(vm.listPaymentModeHistoryCRUDViewModel);
            return vm;
        }
        public IQueryable<PaymentCRUDViewModel> GetPaymentGridData()
        {
            try
            {
                var result = (from _Payments in _context.Payment
                              join _CustomerInfo in _context.CustomerInfo on _Payments.CustomerId equals _CustomerInfo.Id
                              into _CustomerInfo
                              from listCustomerInfo in _CustomerInfo.DefaultIfEmpty()
                              join _PaymentStatus in _context.PaymentStatus on _Payments.PaymentStatus equals _PaymentStatus.Id
                              into _PaymentStatus
                              from listPaymentStatus in _PaymentStatus.DefaultIfEmpty()
                              join _Branch in _context.Branch on _Payments.BranchId equals _Branch.Id
                              into _Branch
                              from listBranch in _Branch.DefaultIfEmpty()
                              where _Payments.Cancelled == false && _Payments.ReturnType != TranReturnType.FullReturn
                              select new PaymentCRUDViewModel
                              {
                                  Id = _Payments.Id,
                                  InvoiceNo = _Payments.InvoiceNo,
                                  QuoteNo = _Payments.QuoteNo,
                                  CustomerId = _Payments.CustomerId,
                                  CustomerName = listCustomerInfo.Name,

                                  BranchId = _Payments.BranchId,
                                  BranchName = listBranch.Name,
                                  SubTotal = _Payments.SubTotal,
                                  DiscountAmount = _Payments.DiscountAmount,
                                  VATAmount = _Payments.VATAmount,
                                  GrandTotal = _Payments.GrandTotal,
                                  PaidAmount = _Payments.PaidAmount,
                                  DueAmount = _Payments.DueAmount,
                                  Category = _Payments.Category,
                                  CreatedDate = _Payments.CreatedDate,
                                  PaymentStatus = _Payments.PaymentStatus,
                                  PaymentStatusDisplay = listPaymentStatus.Name,
                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception) { throw; }
        }
        public IQueryable<PaymentCRUDViewModel> GetPaymentList()
        {
            try
            {
                var result = (from _Payments in _context.Payment
                              join _CustomerInfo in _context.CustomerInfo on _Payments.CustomerId equals _CustomerInfo.Id
                              into _CustomerInfo
                              from listCustomerInfo in _CustomerInfo.DefaultIfEmpty()
                              join _PaymentStatus in _context.PaymentStatus on _Payments.PaymentStatus equals _PaymentStatus.Id
                              into _PaymentStatus
                              from listPaymentStatus in _PaymentStatus.DefaultIfEmpty()
                              join _Currency in _context.Currency on _Payments.CurrencyId equals _Currency.Id
                              into _Currency
                              from listCurrency in _Currency.DefaultIfEmpty()

                              join _Branch in _context.Branch on _Payments.BranchId equals _Branch.Id
                              into _Branch
                              from listBranch in _Branch.DefaultIfEmpty()
                              where _Payments.Cancelled == false
                              select new PaymentCRUDViewModel
                              {
                                  Id = _Payments.Id,
                                  CustomerId = _Payments.CustomerId,
                                  CustomerName = listCustomerInfo.Name,
                                  InvoiceNo = _Payments.InvoiceNo,
                                  QuoteNo = _Payments.QuoteNo,
                                  CommonCharge = _Payments.CommonCharge,
                                  Discount = _Payments.Discount,
                                  DiscountAmount = _Payments.DiscountAmount,
                                  VAT = _Payments.VAT,
                                  VATAmount = _Payments.VATAmount,
                                  SubTotal = _Payments.SubTotal,
                                  GrandTotal = _Payments.GrandTotal,
                                  PaidAmount = _Payments.PaidAmount,
                                  ChangedAmount = _Payments.ChangedAmount,
                                  DueAmount = _Payments.DueAmount,
                                  CurrencyId = _Payments.CurrencyId,
                                  CurrencyName = listCurrency.Name,
                                  CurrencySymbol = listCurrency.Symbol,
                                  BranchId = _Payments.BranchId,
                                  BranchName = listBranch.Name,
                                  PaymentStatus = _Payments.PaymentStatus,
                                  PaymentStatusDisplay = listPaymentStatus.Name,
                                  Category = _Payments.Category,
                                  PurchaseOrderNumber = _Payments.PurchaseOrderNumber,
                                  CustomerNote = _Payments.CustomerNote,
                                  PrivateNote = _Payments.PrivateNote,
                                  ReturnType = _Payments.ReturnType,
                                  ReferenceNumber = _Payments.ReferenceNumber,

                                  CreatedDate = _Payments.CreatedDate,
                                  CreatedBy = _Payments.CreatedBy,
                                  ModifiedBy = _Payments.ModifiedBy,

                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception) { throw; }
        }
        public IQueryable<PaymentDetailCRUDViewModel> GetPaymentDetailList()
        {
            try
            {
                var _PaymentDetailCRUDViewModel = (from _PaymentsDetails in _context.PaymentDetail
                                                   join _Items in _context.Items on _PaymentsDetails.ItemId equals _Items.Id
                                                   into _Items
                                                   from listItems in _Items.DefaultIfEmpty()
                                                   where _PaymentsDetails.Cancelled == false
                                                   select new PaymentDetailCRUDViewModel
                                                   {
                                                       Id = _PaymentsDetails.Id,
                                                       PaymentId = _PaymentsDetails.PaymentId,
                                                       ItemId = _PaymentsDetails.ItemId,
                                                       //ItemCode = _Items.Code,
                                                       ItemCode = _PaymentsDetails.ItemId.ToString(),
                                                       ItemName = _PaymentsDetails.ItemName,
                                                       Quantity = _PaymentsDetails.Quantity,
                                                       FreeQuantity = _PaymentsDetails.FreeQuantity,
                                                       SubQuantity = listItems == null ? 0 : _PaymentsDetails.Quantity * listItems.Size,
                                                       UnitPrice = _PaymentsDetails.UnitPrice,
                                                       ItemVAT = _PaymentsDetails.ItemVAT,
                                                       ItemVATAmount = _PaymentsDetails.ItemVATAmount,
                                                       ItemDiscount = _PaymentsDetails.ItemDiscount,
                                                       ItemDiscountAmount = _PaymentsDetails.ItemDiscountAmount,
                                                       TotalAmount = _PaymentsDetails.TotalAmount,
                                                       IsReturn = _PaymentsDetails.IsReturn,
                                                       IsReturnDisplay = _PaymentsDetails.IsReturn == true ? "Yes" : "No",
                                                       CreatedDate = _PaymentsDetails.CreatedDate
                                                   }).OrderByDescending(x => x.Id);

                return _PaymentDetailCRUDViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<PaymentDetailCRUDViewModel> GetPaymentDetailListInReturn()
        {
            try
            {
                var _PaymentDetailCRUDViewModel = (from _PaymentsDetails in _context.PaymentDetail
                                                   join _Items in _context.Items on _PaymentsDetails.ItemId equals _Items.Id
                                                   select new PaymentDetailCRUDViewModel
                                                   {
                                                       Id = _PaymentsDetails.Id,
                                                       PaymentId = _PaymentsDetails.PaymentId,
                                                       ItemId = _PaymentsDetails.ItemId,
                                                       ItemCode = _Items.Code,
                                                       ItemName = _PaymentsDetails.ItemName,
                                                       Quantity = _PaymentsDetails.Quantity,
                                                       FreeQuantity = _PaymentsDetails.FreeQuantity,
                                                       UnitPrice = _PaymentsDetails.UnitPrice,
                                                       ItemVAT = _PaymentsDetails.ItemVAT,
                                                       ItemVATAmount = _PaymentsDetails.ItemVATAmount,
                                                       ItemDiscount = _PaymentsDetails.ItemDiscount,
                                                       ItemDiscountAmount = _PaymentsDetails.ItemDiscountAmount,
                                                       TotalAmount = _PaymentsDetails.TotalAmount,
                                                       IsReturn = _PaymentsDetails.IsReturn,
                                                       IsReturnDisplay = _PaymentsDetails.IsReturn == true ? "Yes" : "No",
                                                       CreatedDate = _PaymentsDetails.CreatedDate
                                                   }).OrderByDescending(x => x.Id);

                return _PaymentDetailCRUDViewModel;
            }
            catch (Exception)
            {
                throw;
            }
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
        public IQueryable<PaymentGridViewModel> GetPaymentSummaryReportList()
        {
            try
            {
                var result = (from _Payment in _context.Payment
                              join _CustomerInfo in _context.CustomerInfo on _Payment.CustomerId equals _CustomerInfo.Id
                              where _Payment.Cancelled == false && _Payment.Category == InvoiceType.RegularInvoice && _Payment.ReturnType != TranReturnType.FullReturn
                              select new PaymentGridViewModel
                              {
                                  Id = _Payment.Id,
                                  CustomerName = _CustomerInfo.Name,
                                  Discount = _Payment.Discount,
                                  VAT = _Payment.VATAmount,
                                  SubTotal = _Payment.SubTotal,
                                  GrandTotal = _Payment.GrandTotal,
                                  PaidAmount = _Payment.PaidAmount,
                                  DueAmount = _Payment.DueAmount,
                                  BranchId = _Payment.BranchId,
                                  CreatedDate = _Payment.CreatedDate,
                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception) { throw; }
        }
        public IQueryable<CustomerReportViewModel> GetCustomerReportData()
        {
            try
            {
                var result = (from _PaymentsDetails in _context.PaymentDetail
                              join _Payment in _context.Payment on _PaymentsDetails.PaymentId equals _Payment.Id
                              join _Items in _context.Items on _PaymentsDetails.ItemId equals _Items.Id
                              select new CustomerReportViewModel
                              {
                                  Id = _PaymentsDetails.Id,
                                  CustomerId = _Payment.CustomerId,
                                  CreatedDate = _PaymentsDetails.CreatedDate,
                                  InvoiceNo = _Payment.InvoiceNo,
                                  ReferenceNumber = _Payment.ReferenceNumber == null ? "" : _Payment.ReferenceNumber,

                                  ItemName = _PaymentsDetails.ItemName,
                                  Quantity = _PaymentsDetails.Quantity,
                                  SubQuantity = _PaymentsDetails.Quantity * _Items.Size,
                                  ItemPrice = _PaymentsDetails.UnitPrice,
                                  Discount = _PaymentsDetails.ItemDiscountAmount,

                                  SubTotal = _Payment.SubTotal,
                                  GrandTotal = _Payment.GrandTotal,
                                  PaidAmount = _Payment.PaidAmount,
                                  DueAmount = _Payment.DueAmount,
                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception) { throw; }
        }
        public List<SummaryReportViewModel> GetSummaryReportData(string DateType)
        {
            List<SummaryReportViewModel> list = new();
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            DateTime[] DateList = null;

            var _Payment = _context.Payment.Where(x => x.Cancelled == false && x.ReturnType != TranReturnType.FullReturn && x.Category == InvoiceType.RegularInvoice || x.Category == InvoiceType.ManualInvoice);
            var _PurchasesPayment = _context.PurchasesPayment.Where(x => x.Cancelled == false && x.Category == InvoiceType.RegularInvoice && x.ReturnType != TranReturnType.FullReturn);
            var _ExpenseSummary = _context.ExpenseSummary.Where(x => x.Cancelled == false);

            if (DateType == "Daily")
            {
                DateList = Enumerable.Range(0, 30).Select(i => DateTime.Now.Date.AddDays(-i)).ToArray();
            }
            else if (DateType == "Monthly")
            {
                DateTime _DateTimeMonth = DateTime.Now.AddMonths(1);
                DateList = Enumerable.Range(1, 12).Select(n => _DateTimeMonth.AddMonths(-n)).ToArray();
            }
            else if (DateType == "Yearly")
            {
                DateTime _DateTime = DateTime.Now.AddYears(1);
                DateList = Enumerable.Range(1, 10).Select(n => _DateTime.AddYears(-n)).ToArray();
            }


            int SL = 1;
            foreach (var item in DateList)
            {
                SummaryReportViewModel vm = new();

                vm.Id = SL;
                if (DateType == "Daily")
                {
                    startDate = item;
                    endDate = item.AddDays(1).AddTicks(-1); ;
                    vm.TranDate = item.ToString("dddd, dd MMMM yyyy");
                }
                else if (DateType == "Monthly")
                {
                    startDate = new DateTime(item.Year, item.Month, 1);
                    endDate = startDate.AddMonths(1).AddTicks(-1);
                    vm.TranDate = item.ToString("MMMM") + "-" + item.Year;
                }
                else if (DateType == "Yearly")
                {
                    startDate = new DateTime(item.Year, 1, 1);
                    endDate = new DateTime(item.Year, 12, 31);
                    vm.TranDate = item.Year.ToString();
                }

                //Sales
                var _SalesSingleDayPayment = _Payment.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate);
                vm.SalesTotal = Math.Round((double)_SalesSingleDayPayment.Sum(x => x.GrandTotal), 2);
                vm.SalesPaidTotal = Math.Round(_SalesSingleDayPayment.Sum(x => x.PaidAmount), 2);
                vm.SalesDueTotal = Math.Round(_SalesSingleDayPayment.Sum(x => x.DueAmount), 2);

                //Purchases
                var _PurchasesSingleDayPayment = _PurchasesPayment.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate);
                vm.PurchasesTotal = Math.Round((double)_PurchasesSingleDayPayment.Sum(x => x.GrandTotal), 2);
                vm.PurchasesPaidTotal = Math.Round(_PurchasesSingleDayPayment.Sum(x => x.PaidAmount), 2);
                vm.PurchasesDueTotal = Math.Round(_PurchasesSingleDayPayment.Sum(x => x.DueAmount), 2);

                //Expense
                var _ExpenseSingleDayPayment = _ExpenseSummary.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate);
                vm.ExpenseTotal = _ExpenseSingleDayPayment.Sum(x => Math.Round((double)x.GrandTotal, 2));
                vm.ExpensePaidTotal = Math.Round(_ExpenseSingleDayPayment.Sum(x => x.PaidAmount), 2);
                vm.ExpenseDueTotal = Math.Round(_ExpenseSingleDayPayment.Sum(x => x.DueAmount), 2);

                vm.ProfitOrLoss = Math.Round(vm.SalesTotal - (vm.PurchasesTotal + vm.ExpenseTotal), 2);

                list.Add(vm);
                SL++;
            }
            return list;
        }
        public List<TransactionByViewModel> SalesTransactionBy(string DateType)
        {
            List<TransactionByViewModel> list = new();
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            DateTime[] DateList = null;

            var _Payment = _context.Payment.Where(x => x.Cancelled == false && x.Category == InvoiceType.RegularInvoice && x.ReturnType != TranReturnType.FullReturn);
            var _PaymentDetail = _context.PaymentDetail.Where(x => x.Cancelled == false && x.IsReturn == false);

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

                vm.QuantitySold = _PaymentDetail.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate).Sum(x => x.Quantity);
                var SingleDayPayment = _Payment.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate);
                vm.TotalTran = SingleDayPayment.Count();
                vm.TotalEarned = Math.Round(SingleDayPayment.Sum(x => x.PaidAmount - x.ChangedAmount), 2);
                vm.TotalDue = Math.Round(SingleDayPayment.Sum(x => x.DueAmount), 2);

                list.Add(vm);
                SL++;
            }
            return list;
        }
        public IQueryable<ProductWiseSaleReportViewModel> GetProductWiseSaleList()
        {
            try
            {
                var _PaymentDetailCRUDViewModel = (from _PaymentsDetails in _context.PaymentDetail
                                                   join _Items in _context.Items on _PaymentsDetails.ItemId equals _Items.Id
                                                   join _Payment in _context.Payment on _PaymentsDetails.PaymentId equals _Payment.Id
                                                   join _CustomerInfo in _context.CustomerInfo on _Payment.CustomerId equals _CustomerInfo.Id
                                                   where _PaymentsDetails.Cancelled == false && _PaymentsDetails.IsReturn == false && _Payment.Category == InvoiceType.RegularInvoice
                                                   select new ProductWiseSaleReportViewModel
                                                   {
                                                       Id = _PaymentsDetails.Id,
                                                       SalesDate = _PaymentsDetails.CreatedDate,
                                                       ItemId = _PaymentsDetails.ItemId,
                                                       ItemName = _PaymentsDetails.ItemName,
                                                       InvoiceNo = _Payment.InvoiceNo,
                                                       CustomerName = _CustomerInfo.Name,
                                                       UnitPrice = _PaymentsDetails.UnitPrice,
                                                       Quantity = _PaymentsDetails.Quantity,
                                                       Discount = _PaymentsDetails.ItemDiscount,
                                                       VAT = _PaymentsDetails.ItemVAT,
                                                       Total = _PaymentsDetails.TotalAmount
                                                   }).OrderByDescending(x => x.Id);

                return _PaymentDetailCRUDViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<StockItemReportVM> GetStockItemReportData()
        {
            try
            {
                var result = (from _Items in _context.Items
                              join _Warehouse in _context.Warehouse on _Items.WarehouseId equals _Warehouse.Id
                              into _Warehouse
                              from listWarehouse in _Warehouse.DefaultIfEmpty()
                              select new StockItemReportVM
                              {
                                  Id = _Items.Id,
                                  WarehouseId = _Items.WarehouseId,
                                  WarehouseName = listWarehouse.Name == null ? "" : listWarehouse.Name,
                                  Size = _Items.Size,
                                  SizeDisplay = StaticData.GetSizeDisplay(_Items.Size),
                                  Quantity = _Items.Quantity,
                                  SubQuantity = _Items.Quantity * _Items.Size,
                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception) { throw; }
        }
        public Tuple<byte[], string> GetInvoiceReportPdfBytes(string pdfDataUri)
        {
            string Message = "Success";
            try
            {
                if (string.IsNullOrEmpty(pdfDataUri))
                {
                    Message = "PDF Data URI is null or empty";
                }

                var delimiter = ";base64,";
                int delimiterIndex = pdfDataUri.IndexOf(delimiter);
                if (delimiterIndex == -1)
                {
                    Message = "Invalid PDF Data URI format";
                }
                var base64Data = pdfDataUri.Substring(delimiterIndex + delimiter.Length);
                var pdfBytes = Convert.FromBase64String(base64Data);

                return Tuple.Create(pdfBytes, Message);
            }
            catch (Exception) { throw; }
        }
    }
}