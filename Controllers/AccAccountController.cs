using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models;
using BusinessERP.Models.AccAccountViewModel;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccAccountController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public AccAccountController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.AccAccount.RoleName)]
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
                var _GetDataTabelData = _iCommon.GetAllAccAccount();

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
        private IQueryable<AccAccount> ApplySearchOnData(IQueryable<AccAccount> query, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return query;
            }

            searchValue = searchValue.ToLower();
            return query.Where(obj => obj.Id.ToString().Contains(searchValue)
                || obj.AccountName.ToLower().Contains(searchValue)
                || obj.AccountNumber.ToLower().Contains(searchValue)
                || obj.Credit.ToString().ToLower().Contains(searchValue)
                || obj.Debit.ToString().ToLower().Contains(searchValue)
                || obj.Balance.ToString().ToLower().Contains(searchValue)
                || obj.Description.ToLower().Contains(searchValue)

               || obj.CreatedDate.ToString().Contains(searchValue)
            );
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            AccAccountCRUDViewModel vm = await _context.AccAccount.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            AccAccountCRUDViewModel vm = new();
            if (id > 0) vm = await _context.AccAccount.Where(x => x.Id == id).FirstOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditSave([FromBody] AccAccountCRUDViewModel vm)
        {
            try
            {
                AccAccount _AccAccount = new();
                string _UserName = HttpContext.User.Identity.Name;
                if (vm.Id > 0)
                {
                    _AccAccount = await _context.AccAccount.FindAsync(vm.Id);

                    vm.Balance = _AccAccount.Balance;
                    vm.CreatedDate = _AccAccount.CreatedDate;
                    vm.CreatedBy = _AccAccount.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_AccAccount).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Account Updated Successfully. ID: " + _AccAccount.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    _AccAccount = vm;
                    _AccAccount.Credit = vm.Balance;
                    _AccAccount.Debit = 0;
                    _AccAccount.CreatedDate = DateTime.Now;
                    _AccAccount.ModifiedDate = DateTime.Now;
                    _AccAccount.CreatedBy = _UserName;
                    _AccAccount.ModifiedBy = _UserName;
                    _context.Add(_AccAccount);
                    await _context.SaveChangesAsync();

                    UpdateAccountViewModel _UpdateAccountViewModel = new()
                    {
                        AccUpdateType = AccAccountUpdateType.Credit,
                        AccAccountNo = _AccAccount.Id,
                        Amount = _AccAccount.Balance,
                        Credit = _AccAccount.Balance,
                        Debit = 0,
                        Type = "Initial Account Deposit",
                        Reference = "Initial Amount. Id: " + _AccAccount.Id,
                        Description = _AccAccount.Description,
                        UserName = _UserName
                    };
                    await _iCommon.UpdateAccoutDuringTran(_UpdateAccountViewModel);

                    var _AlertMessage = "Account Created Successfully. ID: " + _AccAccount.Id;
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
            try
            {
                var _AccAccount = await _context.AccAccount.FindAsync(id);
                _AccAccount.ModifiedDate = DateTime.Now;
                _AccAccount.ModifiedBy = HttpContext.User.Identity.Name;
                _AccAccount.Cancelled = true;

                _context.Update(_AccAccount);
                await _context.SaveChangesAsync();
                return new JsonResult(_AccAccount);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
