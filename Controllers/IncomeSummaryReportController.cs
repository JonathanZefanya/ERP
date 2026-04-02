using BusinessERP.ConHelper;
using BusinessERP.Data;
using BusinessERP.Models.IncomeSummaryViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class IncomeSummaryReportController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IIncomeServie _iIncomeServie;

        public IncomeSummaryReportController(ApplicationDbContext context, IIncomeServie iIncomeServie)
        {
            _context = context;
            _iIncomeServie = iIncomeServie;
        }

        [Authorize(Roles = Pages.MainMenu.IncomeSummary.RoleName)]
        [HttpGet]
        public IActionResult Index(string StartDate, string EndDate)
        {
            if (StartDate != null && EndDate != null)
            {
                HttpContext.Session.SetString("_StartDate", StartDate);
                HttpContext.Session.SetString("_EndDate", EndDate);
                ViewBag.StartDate = StartDate;
                ViewBag.EndDate = EndDate;
            }
            else
            {
                HttpContext.Session.SetString("_StartDate", string.Empty);
                HttpContext.Session.SetString("_EndDate", string.Empty);
                ViewBag.StartDate = "Min";
                ViewBag.EndDate = "Max";
            }
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetDataTabelData(Int64 IncomeTypeId, Int64 IncomeCategoryId)
        {
            try
            {
                string _StartDate = HttpContext.Session.GetString("_StartDate");
                string _EndDate = HttpContext.Session.GetString("_EndDate");

                var _GetAllIncomeSummaryForGridItem = await _iIncomeServie.GetAllIncomeSummaryForGridItem(_StartDate, _EndDate, IncomeTypeId, IncomeCategoryId);
                IQueryable<IncomeSummaryCRUDViewModel> _GetDataTabelData = _GetAllIncomeSummaryForGridItem.AsQueryable();

                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                var filteredData = ApplySearchOnData(_GetDataTabelData, searchValue);

                var result = GetDataTablesProcessData(_GetDataTabelData, filteredData, searchValue);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private IQueryable<IncomeSummaryCRUDViewModel> ApplySearchOnData(IQueryable<IncomeSummaryCRUDViewModel> query, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return query;
            }

            searchValue = searchValue.ToLower();
            return query.Where(obj => obj.Id.ToString().Contains(searchValue)
                || obj.Title.ToLower().Contains(searchValue)
                || obj.TypeId.ToString().ToLower().Contains(searchValue)
                || obj.CategoryId.ToString().ToLower().Contains(searchValue)
                || obj.Amount.ToString().ToLower().Contains(searchValue)
                || obj.Description.ToLower().Contains(searchValue)
                || obj.IncomeDate.ToString().ToLower().Contains(searchValue)

                || obj.CreatedDate.ToString().Contains(searchValue)
            );
        }
    }
}
