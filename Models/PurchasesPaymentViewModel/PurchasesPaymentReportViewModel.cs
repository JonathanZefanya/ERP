using BusinessERP.Models.CompanyInfoViewModel;
using BusinessERP.Models.CustomerInfoViewModel;
using BusinessERP.Models.EmailConfigViewModel;
using BusinessERP.Models.PaymentModeHistoryViewModel;
using BusinessERP.Models.PurchasesPaymentDetailViewModel;
using BusinessERP.Models.SupplierViewModel;
using System.Collections.Generic;

namespace BusinessERP.Models.PurchasesPaymentViewModel
{
    public class PurchasesPaymentReportViewModel
    {
        public PurchasesPaymentCRUDViewModel PurchasesPaymentCRUDViewModel { get; set; }
        public List<PurchasesPaymentDetailCRUDViewModel> listPurchasesPaymentDetailCRUDViewModel { get; set; }
        public List<PaymentModeHistoryCRUDViewModel> listPaymentModeHistoryCRUDViewModel { get; set; }
        public SupplierCRUDViewModel SupplierCRUDViewModel { get; set; }
        public CompanyInfoCRUDViewModel CompanyInfoCRUDViewModel { get; set; }
        public SendEmailViewModel SendEmailViewModel { get; set; }
    }
}
