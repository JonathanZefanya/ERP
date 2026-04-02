using BusinessERP.Models;
using BusinessERP.Models.IncomeSummaryViewModel;

namespace BusinessERP.ConHelper
{
    public interface IIncomeServie
    {
        IQueryable<IncomeSummaryCRUDViewModel> GetAllIncomeSummary();
        Task<List<IncomeSummaryCRUDViewModel>> GetAllIncomeSummaryForGridItem(string StartDate, string EndDate, Int64 IncomeTypeId, Int64 IncomeCategoryId);
    }
}
