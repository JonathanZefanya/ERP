using BusinessERP.Models.PaymentDetailViewModel;
using BusinessERP.Models.PaymentModeHistoryViewModel;
using BusinessERP.Models.PaymentViewModel;
using BusinessERP.Models.DashboardViewModel;
using BusinessERP.Models.ReturnLogViewModel;

namespace BusinessERP.Services
{
    public interface ISalesService
    {
        Task<ManagePaymentViewModel> GetByPaymentDetail(Int64 id);
        Task<ManagePaymentViewModel> GetByPaymentDetailInReturn(Int64 id);
        Task<PaymentReportViewModel> PrintPaymentInvoice(Int64 id);
        IQueryable<PaymentCRUDViewModel> GetPaymentGridData();
        IQueryable<PaymentCRUDViewModel> GetPaymentList();
        IQueryable<PaymentDetailCRUDViewModel> GetPaymentDetailList();
        IQueryable<PaymentModeHistoryCRUDViewModel> GetPaymentModeHistory(int _InvoicePaymentType);
        IQueryable<PaymentGridViewModel> GetPaymentSummaryReportList();
        IQueryable<CustomerReportViewModel> GetCustomerReportData();
        List<SummaryReportViewModel> GetSummaryReportData(string DateType);
        List<TransactionByViewModel> SalesTransactionBy(string DateType);
        IQueryable<ProductWiseSaleReportViewModel> GetProductWiseSaleList();
        IQueryable<StockItemReportVM> GetStockItemReportData();
        Tuple<byte[], string> GetInvoiceReportPdfBytes(string pdfDataUri);
    }
}