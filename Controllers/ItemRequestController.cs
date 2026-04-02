using BusinessERP.Data;
using BusinessERP.Models;
using BusinessERP.Models.ItemRequestViewModel;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using BusinessERP.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessERP.Models.WarehouseViewModel;
using BusinessERP.Models.CommonViewModel;

namespace InventoryMNM.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ItemRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly ITransferItemService _iTransferItemService;

        public ItemRequestController(ApplicationDbContext context, ICommon iCommon, ITransferItemService iTransferItemService)
        {
            _context = context;
            _iCommon = iCommon;
            _iTransferItemService = iTransferItemService;
        }

        [Authorize(Roles = BusinessERP.Pages.MainMenu.ItemRequest.RoleName)]
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
                    || obj.RequestQuantity.ToString().Contains(searchValue)
                    || obj.FromWarehouseDisplay.ToLower().Contains(searchValue)
                    //|| obj.StatusDisplay.ToLower().Contains(searchValue)
                    || obj.Note.ToLower().Contains(searchValue)

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

        private IQueryable<ItemRequestCRUDViewModel> GetGridItem()
        {
            try
            {
                var result = (from _ItemRequest in _context.ItemRequest
                              join _Items in _context.Items on _ItemRequest.ItemId equals _Items.Id
                              join _Warehouse in _context.Warehouse on _ItemRequest.FromWarehouseId equals _Warehouse.Id
                              where _ItemRequest.Cancelled == false
                              select new ItemRequestCRUDViewModel
                              {
                                  Id = _ItemRequest.Id,
                                  ItemId = _ItemRequest.ItemId,
                                  ItemDisplay = _Items.Name,
                                  RequestQuantity = _ItemRequest.RequestQuantity,
                                  FromWarehouseId = _ItemRequest.FromWarehouseId,
                                  FromWarehouseDisplay = _Warehouse.Name,
                                  Status = _ItemRequest.Status,
                                  StatusDisplay = StaticData.GetRequestStatus(_ItemRequest.Status),
                                  Note = _ItemRequest.Note,

                                  CreatedDate = _ItemRequest.CreatedDate,
                                  ModifiedDate = _ItemRequest.ModifiedDate,
                                  CreatedBy = _ItemRequest.CreatedBy,
                                  ModifiedBy = _ItemRequest.ModifiedBy,

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
            ItemRequestCRUDViewModel vm = await GetGridItem().FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            var _IsVat = _context.CompanyInfo.FirstOrDefault(m => m.Cancelled == false).IsVat;
            ViewBag.LoadddlInventoryItem = new SelectList(_iCommon.LoadddlInventoryItem(_IsVat), "Id", "Name");
            ViewBag.ddlWarehouseList = new SelectList(_iCommon.LoadddlWarehouse(), "Id", "Name");

            ItemRequestCRUDViewModel vm = new ItemRequestCRUDViewModel();
            if (id > 0) vm = await _context.ItemRequest.Where(x => x.Id == id).FirstOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ItemRequestCRUDViewModel vm)
        {
            try
            {
                ItemRequest _ItemRequest = new();
                if (vm.Id > 0)
                {
                    _ItemRequest = await _context.ItemRequest.FindAsync(vm.Id);

                    vm.CreatedDate = _ItemRequest.CreatedDate;
                    vm.CreatedBy = _ItemRequest.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = HttpContext.User.Identity.Name;
                    _context.Entry(_ItemRequest).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Item Request Updated Successfully. ID: " + _ItemRequest.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    _ItemRequest = vm;
                    _ItemRequest.Status = RequestStatus.New;
                    _ItemRequest.CreatedDate = DateTime.Now;
                    _ItemRequest.ModifiedDate = DateTime.Now;
                    _ItemRequest.CreatedBy = HttpContext.User.Identity.Name;
                    _ItemRequest.ModifiedBy = HttpContext.User.Identity.Name;
                    _context.Add(_ItemRequest);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Item Request Created Successfully. ID: " + _ItemRequest.Id;
                    return new JsonResult(_AlertMessage);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Delete(Int64 id)
        {
            try
            {
                var _ItemRequest = await _context.ItemRequest.FindAsync(id);
                _ItemRequest.ModifiedDate = DateTime.Now;
                _ItemRequest.ModifiedBy = HttpContext.User.Identity.Name;
                _ItemRequest.Cancelled = true;

                _context.Update(_ItemRequest);
                await _context.SaveChangesAsync();
                return new JsonResult(_ItemRequest);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> TransferRequestItem(Int64 id)
        {
            TransferItemViewModel vm = new();
            var _ItemRequest = await _context.ItemRequest.FindAsync(id);
            var _Items = await _context.Items.FindAsync(_ItemRequest.ItemId);

            vm.ItemRequestId = _ItemRequest.Id;
            vm.ItemId = _ItemRequest.ItemId;
            vm.ItemDisplay = _Items.Name;
            vm.CurrentTotalStock = _Items.Quantity;
            vm.TotalTransferItem = _ItemRequest.RequestQuantity;

            vm.ToWarehouseId = _ItemRequest.FromWarehouseId;
            var _ToWarehouse = await _context.Warehouse.Where(x => x.Id == _ItemRequest.FromWarehouseId).FirstOrDefaultAsync();
            vm.ToWarehouseDisplay = _ToWarehouse.Name;
            vm.FromWarehouseId = _Items.WarehouseId;
            var _FromWarehouse = await _context.Warehouse.Where(x => x.Id == _Items.WarehouseId).FirstOrDefaultAsync();
            vm.FromWarehouseDisplay = _FromWarehouse.Name;
            return PartialView("_SingleTransferItem", vm);
        }
        [HttpPost]
        public async Task<JsonResult> SaveTransferRequestItem(TransferItemViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                vm.UserName = HttpContext.User.Identity.Name;
                var result = await _iTransferItemService.ItemTran(vm);
                if (result)
                {
                    await UpdateItemRequestStatus(vm.ItemRequestId, vm.TotalTransferItem);
                    _JsonResultViewModel.AlertMessage = "Item Transfer Successful. Item Name: " + vm.ItemDisplay;
                    _JsonResultViewModel.IsSuccess = true;
                }
                else
                {
                    _JsonResultViewModel.AlertMessage = "Operation Failed. Item Name: " + vm.ItemDisplay;
                    _JsonResultViewModel.IsSuccess = false;
                }
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
        public async Task<JsonResult> UpdateItemRequestStatus(Int64 id, int _RequestQuantity)
        {
            try
            {
                var _ItemRequest = await _context.ItemRequest.FindAsync(id);
                _ItemRequest.ModifiedDate = DateTime.Now;
                _ItemRequest.ModifiedBy = HttpContext.User.Identity.Name;
                _ItemRequest.RequestQuantity = _RequestQuantity;
                _ItemRequest.Status = RequestStatus.Send;

                _context.Update(_ItemRequest);
                await _context.SaveChangesAsync();
                return new JsonResult(_ItemRequest);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
