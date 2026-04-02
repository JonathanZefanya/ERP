using BusinessERP.Models.PaymentModeHistoryViewModel;

namespace BusinessERP.Helpers
{
    public static class Utility
    {
        public static string GetModeOfPaymentCommaSeparated(List<PaymentModeHistoryCRUDViewModel> list)
        {
            if (list == null || !list.Any())
            {
                return string.Empty;
            }
            return string.Join(", ", list.Select(x => x.ModeOfPayment));
        }
    }
}
