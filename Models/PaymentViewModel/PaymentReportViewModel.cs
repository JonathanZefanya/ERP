using BusinessERP.Models.CompanyInfoViewModel;
using BusinessERP.Models.CustomerInfoViewModel;
using BusinessERP.Models.EmailConfigViewModel;
using BusinessERP.Models.PaymentDetailViewModel;
using BusinessERP.Models.PaymentModeHistoryViewModel;

namespace BusinessERP.Models.PaymentViewModel
{
    public class PaymentReportViewModel
    {
        public PaymentCRUDViewModel PaymentCRUDViewModel { get; set; }
        public List<PaymentDetailCRUDViewModel> listPaymentDetailCRUDViewModel { get; set; }
        public List<PaymentModeHistoryCRUDViewModel> listPaymentModeHistoryCRUDViewModel { get; set; }
        public CustomerInfoCRUDViewModel CustomerInfoCRUDViewModel { get; set; }
        public CompanyInfoCRUDViewModel CompanyInfoCRUDViewModel { get; set; }
        public SendEmailViewModel SendEmailViewModel { get; set; }
        public string PaymentMode { get; set; }
    }
}
