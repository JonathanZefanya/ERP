using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models;
using BusinessERP.Models.AccAccountViewModel;
using BusinessERP.Models.AccTransactionViewModel;
using BusinessERP.Models.AccTransferViewModel;
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
    public class AccTransferController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public AccTransferController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.AccTransfer.RoleName)]
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
                var _GetDataTabelData = _iCommon.GetAllAccTransfer();

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
        private IQueryable<AccTransferCRUDViewModel> ApplySearchOnData(IQueryable<AccTransferCRUDViewModel> query, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return query;
            }

            searchValue = searchValue.ToLower();
            return query.Where(obj => obj.Id.ToString().Contains(searchValue)
                || obj.SenderDisplay.ToLower().Contains(searchValue)
                || obj.ReceiverDisplay.ToLower().Contains(searchValue)
                || obj.TransferDate.ToString().ToLower().Contains(searchValue)
                || obj.Amount.ToString().ToLower().Contains(searchValue)
                || obj.Note.ToLower().Contains(searchValue)
                || obj.CreatedDate.ToString().ToLower().Contains(searchValue)

                || obj.CreatedDate.ToString().Contains(searchValue)
            );
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            AccTransferCRUDViewModel vm = await _iCommon.GetAllAccTransfer().FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            AccTransferCRUDViewModel vm = new();
            var result = _iCommon.GetTableData<AccAccount>(x => x.Cancelled == false).ToList();
            ViewBag.ddlAccAccount = new SelectList(result.ToList(), "Id", "AccountName");

            if (id > 0) vm = await _context.AccTransfer.Where(x => x.Id == id).FirstOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditSave([FromBody] AccTransferCRUDViewModel vm)
        {
            try
            {
                AccTransfer _AccTransfer = new();
                string _UserName = HttpContext.User.Identity.Name;
                if (vm.Id > 0)
                {
                    _AccTransfer = await _context.AccTransfer.FindAsync(vm.Id);

                    vm.CreatedDate = _AccTransfer.CreatedDate;
                    vm.CreatedBy = _AccTransfer.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_AccTransfer).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Transfer Updated Successfully. ID: " + _AccTransfer.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    _AccTransfer = vm;
                    _AccTransfer.CreatedDate = DateTime.Now;
                    _AccTransfer.ModifiedDate = DateTime.Now;
                    _AccTransfer.CreatedBy = _UserName;
                    _AccTransfer.ModifiedBy = _UserName;
                    _context.Add(_AccTransfer);
                    await _context.SaveChangesAsync();

                    //Update Account : Sender
                    var _AccAccountSender = await _context.AccAccount.FindAsync(vm.SenderId);
                    UpdateAccountViewModel _UpdateAccountViewModel_Sender = new()
                    {
                        AccUpdateType = AccAccountUpdateType.Debit,
                        AccAccountNo = vm.SenderId,
                        Amount = _AccTransfer.Amount,
                        Credit = 0,
                        Debit = _AccTransfer.Amount,
                        Type = "Transfer",
                        Reference = "Send from Account " + _AccAccountSender.AccountName,
                        Description = vm.Note,
                        UserName = _UserName
                    };
                    await _iCommon.UpdateAccoutDuringTran(_UpdateAccountViewModel_Sender);


                    //Update Account : Receiver
                    var _AccAccountReceiver = await _context.AccAccount.FindAsync(vm.ReceiverId);
                    UpdateAccountViewModel _UpdateAccountViewModel_Receiver = new()
                    {
                        AccUpdateType = AccAccountUpdateType.Credit,
                        AccAccountNo = vm.ReceiverId,
                        Amount = _AccTransfer.Amount,
                        Credit = _AccTransfer.Amount,
                        Debit = 0,
                        Type = "Transfer",
                        Reference = "Receive from Account " + _AccAccountReceiver.AccountName,
                        Description = vm.Note,
                        UserName = _UserName
                    };
                    await _iCommon.UpdateAccoutDuringTran(_UpdateAccountViewModel_Receiver);

                    var _AlertMessage = "Transfer Created Successfully. ID: " + _AccTransfer.Id;
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
            var _UserName = HttpContext.User.Identity.Name;
            try
            {
                var _AccTransfer = await _context.AccTransfer.FindAsync(id);
                _AccTransfer.ModifiedDate = DateTime.Now;
                _AccTransfer.ModifiedBy = _UserName;
                _AccTransfer.Cancelled = true;

                _context.Update(_AccTransfer);
                await _context.SaveChangesAsync();

                //Update Account : Sender
                var _AccAccountSender = await _context.AccAccount.FindAsync(_AccTransfer.SenderId);
                UpdateAccountViewModel _UpdateAccountViewModel_Sender = new()
                {
                    AccUpdateType = AccAccountUpdateType.DebitRevart,
                    AccAccountNo = _AccTransfer.SenderId,
                    Amount = _AccTransfer.Amount,
                    Credit = _AccTransfer.Amount,
                    Debit = 0,
                    Type = "Transfer-Revart",
                    Reference = "Send from Account " + _AccAccountSender.AccountName,
                    Description = "Transfer-Revart",
                    UserName = _UserName
                };
                await _iCommon.UpdateAccoutDuringTran(_UpdateAccountViewModel_Sender);


                //Update Account : Receiver
                var _AccAccountReceiver = await _context.AccAccount.FindAsync(_AccTransfer.ReceiverId);
                UpdateAccountViewModel _UpdateAccountViewModel_Receiver = new()
                {
                    AccUpdateType = AccAccountUpdateType.CreditRevart,
                    AccAccountNo = _AccTransfer.ReceiverId,
                    Amount = _AccTransfer.Amount,
                    Credit = 0,
                    Debit = _AccTransfer.Amount,
                    Type = "Transfer-Revart",
                    Reference = "Receive from Account " + _AccAccountReceiver.AccountName,
                    Description = "Transfer-Revart",
                    UserName = _UserName
                };
                await _iCommon.UpdateAccoutDuringTran(_UpdateAccountViewModel_Receiver);
                return new JsonResult(_AccTransfer);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
