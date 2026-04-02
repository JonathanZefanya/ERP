using BusinessERP.Data;
using BusinessERP.Models;
using BusinessERP.Models.AccTransactionViewModel;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccTransactionController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public AccTransactionController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.AccTransaction.RoleName)]
        [HttpGet]
        public IActionResult Index()
        {
            var result = _iCommon.GetTableData<AccAccount>(x => x.Cancelled == false).ToList();
            ViewBag.ddlAccAccount = new SelectList(result.ToList(), "Id", "AccountName");
            return View();
        }
        [HttpPost]
        public JsonResult GetDataTabelData(bool IsFilterData, Int64 AccAccountId)
        {
            try
            {
                //var _GetDataTabelData = _iCommon.GetAllAccTransaction();
                 IQueryable<AccTransactionCRUDViewModel> _GetDataTabelData;
                if (IsFilterData)
                {
                    _GetDataTabelData = _iCommon.GetAllAccTransaction().Where(obj => obj.AccountId == AccAccountId);
                }
                else
                {
                    _GetDataTabelData = _iCommon.GetAllAccTransaction();
                }

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
        private IQueryable<AccTransactionCRUDViewModel> ApplySearchOnData(IQueryable<AccTransactionCRUDViewModel> query, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return query;
            }

            searchValue = searchValue.ToLower();
            return query.Where(obj => obj.Id.ToString().Contains(searchValue)
                || obj.AccountDisplay.ToLower().Contains(searchValue)
                || obj.Type.ToLower().Contains(searchValue)
                || obj.Reference.ToLower().Contains(searchValue)
                || obj.Credit.ToString().ToLower().Contains(searchValue)
                || obj.Debit.ToString().ToLower().Contains(searchValue)
                || obj.Amount.ToString().ToLower().Contains(searchValue)

                || obj.CreatedDate.ToString().Contains(searchValue)
            );
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            AccTransactionCRUDViewModel vm = await _iCommon.GetAllAccTransaction().FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
    }
}
