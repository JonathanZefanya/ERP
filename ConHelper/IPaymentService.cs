using BusinessERP.Models;
using BusinessERP.Models.ExpenseSummaryViewModel;
using BusinessERP.Models.PaymentDetailViewModel;
using BusinessERP.Models.PaymentModeHistoryViewModel;
using BusinessERP.Models.PaymentViewModel;
using BusinessERP.Models.PurchasesPaymentDetailViewModel;
using BusinessERP.Models.PurchasesPaymentViewModel;
using BusinessERP.Models.ReturnLogViewModel;
using BusinessERP.Models.SendEmailHistoryViewModel;

namespace BusinessERP.ConHelper
{
    public interface IPaymentService
    {
        Task<Payment> CreatePayment(AddPaymentViewModel _AddPaymentViewModel, string strUserName);
        Task<PaymentDetail> CreatePaymentsDetail(PaymentDetail _PaymentDetail, string UserName);
        Task<PaymentDetail> UpdatePaymentDetail(PaymentDetailCRUDViewModel vm);
        Task<PaymentModeHistory> CreatePaymentModeHistory(PaymentModeHistoryCRUDViewModel vm);
        Task<PaymentModeHistory> UpdatePaymentModeHistory(PaymentModeHistoryCRUDViewModel vm);
        string GetInvoiceNo(int _Category);
        string GetQuoteNo(int _Category);
        CustomerHistoryViewModel GetCustomerHistory(Int64 _CustomerId);
        Task<PaymentCRUDViewModel> UpdatePayment(PaymentCRUDViewModel vm, bool IsCustomerInfo);
        Task<PurchasesPaymentCRUDViewModel> UpdatePurchasesPayment(PurchasesPaymentCRUDViewModel vm);
        Task<PurchasesPaymentDetail> UpdatePurchasesPaymentDetail(PurchasesPaymentDetailCRUDViewModel vm);
        Task<Payment> CreateDraftInvoice(PaymentCRUDViewModel vm);
        Task<Payment> CreateManualInvoice(PaymentCRUDViewModel vm);
        string GetPurchasesPaymentNo(int _Category);
        string GetPurchasesQuoteNo(int _Category);
        Task<ExpenseSummaryCRUDViewModel> UpdateExpenseSummary(ExpenseSummaryCRUDViewModel vm);
        Task<ExpenseDetailsCRUDViewModel> AddExpenseDetails(ExpenseDetailsCRUDViewModel vm);
        Task<ExpenseDetailsCRUDViewModel> UpdateExpenseDetails(ExpenseDetailsCRUDViewModel vm);
        Task<SendEmailHistoryCRUDViewModel> AddSendEmailHistory(SendEmailHistoryCRUDViewModel vm);
        Task<ReturnLogCRUDViewModel> AddReturnLog(ReturnLogCRUDViewModel vm);
        Task<PaymentModeHistory> UpdateAccAccountInPaymentModeHistory(PaymentModeHistoryCRUDViewModel vm);
        Task<bool> UpdateAccAccountInDeletePaymentModeHistory(PaymentModeHistory _PaymentModeHistory);
    }
}
