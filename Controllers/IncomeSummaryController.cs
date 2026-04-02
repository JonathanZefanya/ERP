using BusinessERP.ConHelper;
using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models;
using BusinessERP.Models.AccAccountViewModel;
using BusinessERP.Models.AccTransactionViewModel;
using BusinessERP.Models.IncomeSummaryViewModel;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class IncomeSummaryController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IIncomeServie _iIncomeServie;
        private readonly ICommon _iCommon;

        public IncomeSummaryController(ApplicationDbContext context, IIncomeServie iIncomeServie, ICommon iCommon)
        {
            _context = context;
            _iIncomeServie = iIncomeServie;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.IncomeSummary.RoleName)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetDataTabelData(Int64 IncomeTypeId, Int64 IncomeCategoryId)
        {
            try
            {
                var _GetAllIncomeSummaryForGridItem = await _iIncomeServie.GetAllIncomeSummaryForGridItem("", "", IncomeTypeId, IncomeCategoryId);
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

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            IncomeSummaryCRUDViewModel vm = await _iIncomeServie.GetAllIncomeSummary().FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            IncomeSummaryCRUDViewModel vm = new();
            if (id > 0) vm = await _context.IncomeSummary.Where(x => x.Id == id).FirstOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditSave([FromBody] IncomeSummaryCRUDViewModel vm)
        {
            try
            {
                IncomeSummary _IncomeSummary = new();
                string _UserName = HttpContext.User.Identity.Name;
                if (vm.Id > 0)
                {
                    _IncomeSummary = await _context.IncomeSummary.FindAsync(vm.Id);

                    vm.CreatedDate = _IncomeSummary.CreatedDate;
                    vm.CreatedBy = _IncomeSummary.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_IncomeSummary).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Income Summary Updated Successfully. ID: " + _IncomeSummary.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    _IncomeSummary = vm;
                    _IncomeSummary.CreatedDate = DateTime.Now;
                    _IncomeSummary.ModifiedDate = DateTime.Now;
                    _IncomeSummary.CreatedBy = _UserName;
                    _IncomeSummary.ModifiedBy = _UserName;
                    _context.Add(_IncomeSummary);
                    await _context.SaveChangesAsync();

                    UpdateAccountViewModel _UpdateAccountViewModel = new()
                    {
                        AccUpdateType = AccAccountUpdateType.Credit,
                        AccAccountNo = AccAccountInfo.DefaultAccount,
                        Amount = _IncomeSummary.Amount,
                        Credit = _IncomeSummary.Amount,
                        Debit = 0,
                        Type = "Income",
                        Reference = "Income Id: " + _IncomeSummary.Id,
                        Description = _IncomeSummary.Description,
                        UserName = _UserName
                    };
                    await _iCommon.UpdateAccoutDuringTran(_UpdateAccountViewModel);
                    var _AlertMessage = "Income Summary Created Successfully. ID: " + _IncomeSummary.Id;
                    return new JsonResult(_AlertMessage);
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
                throw;
            }
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Int64 id)
        {
            string _UserName = HttpContext.User.Identity.Name;
            try
            {
                var _IncomeSummary = await _context.IncomeSummary.FindAsync(id);
                _IncomeSummary.ModifiedDate = DateTime.Now;
                _IncomeSummary.ModifiedBy = _UserName;
                _IncomeSummary.Cancelled = true;

                _context.Update(_IncomeSummary);
                await _context.SaveChangesAsync();

                UpdateAccountViewModel _UpdateAccountViewModel = new()
                {
                    AccUpdateType = AccAccountUpdateType.CreditRevart,
                    AccAccountNo = AccAccountInfo.DefaultAccount,
                    Amount = _IncomeSummary.Amount,
                    Credit = 0,
                    Debit = _IncomeSummary.Amount,
                    Type = "Income-Revart",
                    Reference = "Income Id: " + _IncomeSummary.Id,
                    Description = "Income Deleted.",
                    UserName = _UserName
                };
                await _iCommon.UpdateAccoutDuringTran(_UpdateAccountViewModel);
                return new JsonResult(_IncomeSummary);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
