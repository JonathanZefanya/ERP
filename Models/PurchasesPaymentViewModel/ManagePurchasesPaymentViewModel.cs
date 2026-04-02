using BusinessERP.Models.CustomerInfoViewModel;
using BusinessERP.Models.PaymentModeHistoryViewModel;
using BusinessERP.Models.PurchasesPaymentDetailViewModel;
using BusinessERP.Models.SupplierViewModel;
using System.Collections.Generic;

namespace BusinessERP.Models.PurchasesPaymentViewModel
{
    public class ManagePurchasesPaymentViewModel
    {
        public PurchasesPaymentCRUDViewModel PurchasesPaymentCRUDViewModel { get; set; }
        public PurchasesPaymentDetailCRUDViewModel PurchasesPaymentDetailCRUDViewModel { get; set; }
        public List<PurchasesPaymentDetailCRUDViewModel> listPurchasesPaymentDetailCRUDViewModel { get; set; }
        public List<PaymentModeHistoryCRUDViewModel> listPaymentModeHistoryCRUDViewModel { get; set; }
        public SupplierCRUDViewModel SupplierCRUDViewModel { get; set; }
    }
}
