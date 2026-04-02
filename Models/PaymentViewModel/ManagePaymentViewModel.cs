using BusinessERP.Models.CustomerInfoViewModel;
using BusinessERP.Models.PaymentDetailViewModel;
using BusinessERP.Models.PaymentModeHistoryViewModel;
using System.Collections.Generic;

namespace BusinessERP.Models.PaymentViewModel
{
    public class ManagePaymentViewModel
    {
        public PaymentCRUDViewModel PaymentCRUDViewModel { get; set; }
        public PaymentDetailCRUDViewModel PaymentDetailCRUDViewModel { get; set; }
        public List<PaymentDetailCRUDViewModel> listPaymentDetailCRUDViewModel { get; set; }
        public List<PaymentModeHistoryCRUDViewModel> listPaymentModeHistoryCRUDViewModel { get; set; }
        public CustomerInfoCRUDViewModel CustomerInfoCRUDViewModel { get; set; }
    }
}
