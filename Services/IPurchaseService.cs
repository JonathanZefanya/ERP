using System.Linq;
using BusinessERP.Models.PaymentModeHistoryViewModel;
using BusinessERP.Models.PurchasesPaymentDetailViewModel;
using BusinessERP.Models.PurchasesPaymentViewModel;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using BusinessERP.Models.DashboardViewModel;

namespace BusinessERP.Services
{
    public interface IPurchaseService
    {
        Task<ManagePurchasesPaymentViewModel> GetByPurchasesPaymentDetail(Int64 id);
        Task<ManagePurchasesPaymentViewModel> GetByPurchasesPaymentDetailInReturn(Int64 id);
        Task<PurchasesPaymentReportViewModel> PrintPurchasesPaymentInvoice(Int64 id);
        IQueryable<PurchasesPaymentCRUDViewModel> GetPurchasesPaymentGridData();
        IQueryable<PurchasesPaymentCRUDViewModel> GetPurchasesPaymentList();
        IQueryable<PurchasesPaymentDetailCRUDViewModel> GetPurchasesPaymentDetailList();
        IQueryable<PurchasesPaymentGridViewModel> GetPurchasesSummaryReportList();
        List<TransactionByViewModel> PurchasesTransactionBy(string DateType);
        
        IQueryable<PaymentModeHistoryCRUDViewModel> GetPaymentModeHistory(int _InvoicePaymentType);
    }
}