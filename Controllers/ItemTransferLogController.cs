using BusinessERP.Data;
using BusinessERP.Models.ItemTransferLogViewModel;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using BusinessERP.Models;
using BusinessERP.ConHelper;
using BusinessERP.Models.WarehouseViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessERP.Models.CommonViewModel;

namespace InventoryMNM.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ItemTransferLogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly IPaymentService _iDBOperation;
        private readonly ITransferItemService _iTransferItemService;

        public ItemTransferLogController(ApplicationDbContext context, ICommon iCommon, IPaymentService iPaymentService, ITransferItemService iTransferItemService)
        {
            _context = context;
            _iCommon = iCommon;
            _iDBOperation = iPaymentService;
            _iTransferItemService = iTransferItemService;
        }


        [Authorize(Roles = BusinessERP.Pages.MainMenu.ItemTransferLog.RoleName)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetDataTabelData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int resultTotal = 0;

                var _GetGridItem = GetGridItem();
                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.ItemDisplay.ToLower().Contains(searchValue)
                    || obj.CurrentTotalStock.ToString().ToLower().Contains(searchValue)
                    || obj.TotalTransferItem.ToString().ToLower().Contains(searchValue)
                    || obj.FromWarehouseDisplay.ToLower().Contains(searchValue)
                    || obj.ToWarehouseDisplay.ToLower().Contains(searchValue)
                    || obj.ReasonOfTransfer.ToLower().Contains(searchValue)

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult TransferItem(Int64 id)
        {
            var _IsVat = _context.CompanyInfo.FirstOrDefault(m => m.Cancelled == false).IsVat;
            ViewBag.LoadddlInventoryItem = new SelectList(_iCommon.LoadddlInventoryItem(_IsVat), "Id", "Name");
            ViewBag.ddlWarehouseList = new SelectList(_iCommon.LoadddlWarehouse(), "Id", "Name");

            TransferItemViewModel vm = new();
            return PartialView("_TransferItem", vm);
        }
        [HttpPost]
        public async Task<JsonResult> SaveSingleTranItem(TransferItemViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                if (ModelState.IsValid)
                {
                    vm.UserName = HttpContext.User.Identity.Name;
                    var result = await _iTransferItemService.ItemTran(vm);

                    _JsonResultViewModel.AlertMessage = "Item Transfer Successful. Item Id: " + vm.ItemId;
                    _JsonResultViewModel.IsSuccess = true;
                    _JsonResultViewModel.CurrentURL = vm.CurrentURL;
                    return new JsonResult(_JsonResultViewModel);
                }

                _JsonResultViewModel.AlertMessage = "Operation failed.";
                _JsonResultViewModel.IsSuccess = false;
                return new JsonResult(_JsonResultViewModel);
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message;
                return new JsonResult(_JsonResultViewModel);
                throw;
            }
        }
        [HttpPost]
        public async Task<JsonResult> SaveMultiTranItem(List<TransferItemViewModel> listTransferItemViewModel)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                string _CurrentURL = string.Empty;
                if (ModelState.IsValid)
                {
                    foreach (var item in listTransferItemViewModel)
                    {
                        item.UserName = HttpContext.User.Identity.Name;
                        var result = await _iTransferItemService.ItemTran(item);
                        _CurrentURL = item.CurrentURL;
                    }

                    _JsonResultViewModel.AlertMessage = "Transfer Multiple Item Successful. Total Item Transfer: " + listTransferItemViewModel.Count;
                    _JsonResultViewModel.IsSuccess = true;
                    _JsonResultViewModel.CurrentURL = _CurrentURL;
                    return new JsonResult(_JsonResultViewModel);
                }

                _JsonResultViewModel.AlertMessage = "Operation failed.";
                _JsonResultViewModel.IsSuccess = false;
                return new JsonResult(_JsonResultViewModel);
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message;
                return new JsonResult(_JsonResultViewModel);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetByItem(int Id)
        {
            try
            {
                Items result = (from _Item in _context.Items
                                join _UnitsofMeasure in _context.UnitsofMeasure on _Item.MeasureId equals _UnitsofMeasure.Id
                                where _Item.Id == Id
                                select new Items
                                {
                                    Id = _Item.Id,
                                    Name = _Item.Name,
                                    MeasureId = _Item.MeasureId,
                                    Quantity = _Item.Quantity,
                                    NormalPrice = _Item.NormalPrice,
                                    CostPrice = _Item.CostPrice,
                                    WarehouseId = _Item.WarehouseId
                                }).FirstOrDefault();
                return new JsonResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IQueryable<ItemTransferLogCRUDViewModel> GetGridItem()
        {
            try
            {
                var result = (from _ItemTransferLog in _context.ItemTransferLog
                              join _Items in _context.Items on _ItemTransferLog.ItemId equals _Items.Id
                              join _FromWarehouse in _context.Warehouse on _ItemTransferLog.FromWarehouseId equals _FromWarehouse.Id
                              join _ToWarehouse in _context.Warehouse on _ItemTransferLog.ToWarehouseId equals _ToWarehouse.Id
                              where _ItemTransferLog.Cancelled == false
                              select new ItemTransferLogCRUDViewModel
                              {
                                  Id = _ItemTransferLog.Id,
                                  ItemId = _ItemTransferLog.ItemId,
                                  ItemDisplay = _Items.Name,
                                  CurrentTotalStock = _ItemTransferLog.CurrentTotalStock,
                                  TotalTransferItem = _ItemTransferLog.TotalTransferItem,
                                  FromWarehouseId = _ItemTransferLog.FromWarehouseId,
                                  FromWarehouseDisplay = _FromWarehouse.Name,

                                  ToWarehouseId = _ItemTransferLog.ToWarehouseId,
                                  ToWarehouseDisplay = _ToWarehouse.Name,

                                  ReasonOfTransfer = _ItemTransferLog.ReasonOfTransfer,
                                  CreatedDate = _ItemTransferLog.CreatedDate,
                                  ModifiedDate = _ItemTransferLog.ModifiedDate,
                                  CreatedBy = _ItemTransferLog.CreatedBy,
                                  ModifiedBy = _ItemTransferLog.ModifiedBy,

                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            ItemTransferLogCRUDViewModel vm = await GetGridItem().FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
    }
}
