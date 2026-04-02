using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models;
using BusinessERP.Models.AccAccountViewModel;
using BusinessERP.Models.AccDepositViewModel;
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
    public class AccDepositController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public AccDepositController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.AccDeposit.RoleName)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetDataTabelData()
        {
            try
            {
                var _GetDataTabelData = _iCommon.GetAllAccDeposit();

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
        private IQueryable<AccDepositCRUDViewModel> ApplySearchOnData(IQueryable<AccDepositCRUDViewModel> query, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return query;
            }

            searchValue = searchValue.ToLower();
            return query.Where(obj => obj.Id.ToString().Contains(searchValue)
                || obj.AccountDisplay.ToLower().Contains(searchValue)
                || obj.DepositDate.ToString().ToLower().Contains(searchValue)
                || obj.Amount.ToString().ToLower().Contains(searchValue)
                || obj.Note.ToLower().Contains(searchValue)
                || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)

               || obj.CreatedDate.ToString().Contains(searchValue)
            );
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            AccDepositCRUDViewModel vm = await _iCommon.GetAllAccDeposit().FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            AccDepositCRUDViewModel vm = new();
            var result = _iCommon.GetTableData<AccAccount>(x => x.Cancelled == false).ToList();
            ViewBag.ddlAccAccount = new SelectList(result.ToList(), "Id", "AccountName");
            if (id > 0) vm = await _context.AccDeposit.Where(x => x.Id == id).FirstOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditSave([FromBody] AccDepositCRUDViewModel vm)
        {
            try
            {
                AccDeposit _AccDeposit = new();
                string _UserName = HttpContext.User.Identity.Name;
                if (vm.Id > 0)
                {
                    _AccDeposit = await _context.AccDeposit.FindAsync(vm.Id);

                    vm.CreatedDate = _AccDeposit.CreatedDate;
                    vm.CreatedBy = _AccDeposit.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_AccDeposit).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Deposit Updated Successfully. ID: " + _AccDeposit.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    _AccDeposit = vm;
                    _AccDeposit.CreatedDate = DateTime.Now;
                    _AccDeposit.ModifiedDate = DateTime.Now;
                    _AccDeposit.CreatedBy = _UserName;
                    _AccDeposit.ModifiedBy = _UserName;
                    _context.Add(_AccDeposit);
                    await _context.SaveChangesAsync();

                    UpdateAccountViewModel _UpdateAccountViewModel = new()
                    {
                        AccUpdateType = AccAccountUpdateType.Credit,
                        AccAccountNo = _AccDeposit.AccountId,
                        Amount = _AccDeposit.Amount,
                        Credit = _AccDeposit.Amount,
                        Debit = 0,
                        Type = "Deposit",
                        Reference = "Deposit Id: " + _AccDeposit.Id,
                        Description = _AccDeposit.Note,
                        UserName = _UserName
                    };
                    await _iCommon.UpdateAccoutDuringTran(_UpdateAccountViewModel);

                    var _AlertMessage = "Deposit Created Successfully. ID: " + _AccDeposit.Id;
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
                var _AccDeposit = await _context.AccDeposit.FindAsync(id);
                _AccDeposit.ModifiedDate = DateTime.Now;
                _AccDeposit.ModifiedBy = _UserName;
                _AccDeposit.Cancelled = true;

                _context.Update(_AccDeposit);
                await _context.SaveChangesAsync();

                UpdateAccountViewModel _UpdateAccountViewModel = new()
                {
                    AccUpdateType = AccAccountUpdateType.CreditRevart,
                    AccAccountNo = _AccDeposit.AccountId,
                    Amount = _AccDeposit.Amount,
                    Credit = 0,
                    Debit = _AccDeposit.Amount,
                    Type = "Deposit-Revart",
                    Reference = "Deposit Id: " + _AccDeposit.Id,
                    Description = "Deposit Deleted.",
                    UserName = _UserName
                };
                await _iCommon.UpdateAccoutDuringTran(_UpdateAccountViewModel);
                return new JsonResult(_AccDeposit);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
