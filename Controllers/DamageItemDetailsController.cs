using BusinessERP.Data;
using BusinessERP.Models;
using BusinessERP.Models.DamageItemDeatilsViewModel;
using BusinessERP.Models.ItemsViewModel;
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
    public class DamageItemDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public DamageItemDetailsController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.DamageItemDetails.RoleName)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDataTabelData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int resultTotal = 0;

                var damageItemData = GetGridItem();

                // Apply search filter
                if (!string.IsNullOrEmpty(searchValue))
                {
                    damageItemData = damageItemData.Where(m =>
                        m.ItemName.Contains(searchValue) ||
                        m.Supplier.Contains(searchValue) ||
                        m.ReasonOfDamage.Contains(searchValue) ||
                        m.CreatedBy.Contains(searchValue) ||
                        m.AdjustmentStatus.Contains(searchValue)
                    );
                }

                // Get total count before sorting
                resultTotal = damageItemData.Count();

                var result = damageItemData.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        private IQueryable<DamageItemDeatilsCRUDViewModel> GetGridItem()
        {
            try
            {
                var result = (from _DamageItemDeatils in _context.DamageItemDeatils
                              join _Items in _context.Items on _DamageItemDeatils.ItemId equals _Items.Id
                              where _DamageItemDeatils.Cancelled == false
                              select new DamageItemDeatilsCRUDViewModel
                              {
                                  Id = _DamageItemDeatils.Id,
                                  ItemId = _DamageItemDeatils.ItemId,
                                  ItemName = _Items.Name,
                                  Supplier = _context.Supplier
                                      .Where(x => x.Id == _Items.SupplierId)
                                      .Select(x => x.Name)
                                      .FirstOrDefault(),
                                  TotalDamageItem = _DamageItemDeatils.TotalDamageItem,

                                  // Calculate total adjusted
                                  TotalAdjusted = _context.DamageItemAdjustments
                                      .Where(x => x.DamageItemId == _DamageItemDeatils.Id && x.Cancelled == false)
                                      .Sum(x => (int?)x.AdjustmentQuantity) ?? 0,

                                  // Calculate remaining
                                  Remaining = _DamageItemDeatils.TotalDamageItem -(_context.DamageItemAdjustments
                                  .Where(x => x.DamageItemId == _DamageItemDeatils.Id && x.Cancelled == false)
                                  .Sum(x => (int?)x.AdjustmentQuantity) ?? 0),

                                  ReasonOfDamage = _DamageItemDeatils.ReasonOfDamage,
                                  CreatedDate = _DamageItemDeatils.CreatedDate,
                                  ModifiedDate = _DamageItemDeatils.ModifiedDate,
                                  CreatedBy = _DamageItemDeatils.CreatedBy,
                              })
                              .OrderByDescending(x => x.Id);

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
            DamageItemDeatilsCRUDViewModel vm = await _context.DamageItemDeatils.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag.ddlInventoryItem = new SelectList(_iCommon.LoadddlInventoryItem(true), "Id", "Name");
            DamageItemDeatilsCRUDViewModel vm = new DamageItemDeatilsCRUDViewModel();
            if (id > 0) vm = await _context.DamageItemDeatils.Where(x => x.Id == id).FirstOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(DamageItemDeatilsCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        DamageItemDeatils _DamageItemDeatils = new DamageItemDeatils();
                        ItemTranViewModel _ItemTranViewModel = new();

                        if (vm.Id > 0)
                        {
                            _DamageItemDeatils = await _context.DamageItemDeatils.FindAsync(vm.Id);
                            int tmpTotalDamageItem = _DamageItemDeatils.TotalDamageItem;

                            vm.ItemId = _DamageItemDeatils.ItemId;
                            vm.CreatedDate = _DamageItemDeatils.CreatedDate;
                            vm.CreatedBy = _DamageItemDeatils.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_DamageItemDeatils).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();

                            if (vm.TotalDamageItem > tmpTotalDamageItem)
                            {
                                _ItemTranViewModel.ItemId = _DamageItemDeatils.ItemId;
                                _ItemTranViewModel.TranQuantity = vm.TotalDamageItem - tmpTotalDamageItem;
                                _ItemTranViewModel.ActionMessage = "Damage Item added by update";
                                _ItemTranViewModel.IsAddition = false;
                            }
                            else
                            {
                                _ItemTranViewModel.ItemId = _DamageItemDeatils.ItemId;
                                _ItemTranViewModel.TranQuantity = tmpTotalDamageItem - vm.TotalDamageItem;
                                _ItemTranViewModel.ActionMessage = "Damage Item rollback by update";
                                _ItemTranViewModel.IsAddition = true;
                            }
                            await _iCommon.CurrentItemsUpdate(_ItemTranViewModel);
                            _ItemTranViewModel.CurrentUserName = HttpContext.User.Identity.Name;

                            var _AlertMessage = "Damage Item Updated Successfully. ID: " + _DamageItemDeatils.Id;
                            return new JsonResult(_AlertMessage);
                        }
                        else
                        {
                            _DamageItemDeatils = vm;
                            _DamageItemDeatils.CreatedDate = DateTime.Now;
                            _DamageItemDeatils.ModifiedDate = DateTime.Now;
                            _DamageItemDeatils.CreatedBy = HttpContext.User.Identity.Name;
                            _DamageItemDeatils.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_DamageItemDeatils);
                            await _context.SaveChangesAsync();

                            _ItemTranViewModel.ItemId = vm.ItemId;
                            _ItemTranViewModel.TranQuantity = vm.TotalDamageItem;
                            _ItemTranViewModel.ActionMessage = "Damage Item added by add new";
                            _ItemTranViewModel.IsAddition = false;
                            _ItemTranViewModel.CurrentUserName = HttpContext.User.Identity.Name;
                            await _iCommon.CurrentItemsUpdate(_ItemTranViewModel);

                            var _AlertMessage = "Damage Item Created Successfully. ID: " + _DamageItemDeatils.Id;
                            return new JsonResult(_AlertMessage);
                        }
                    }
                    return new JsonResult("Operation failed.");
                }
                catch (Exception ex)
                {
                    return new JsonResult(ex.Message);
                    throw;
                }
            }
            return View(vm);
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Int64 id)
        {
            try
            {
                var _DamageItemDeatils = await _context.DamageItemDeatils.FindAsync(id);
                _DamageItemDeatils.ModifiedDate = DateTime.Now;
                _DamageItemDeatils.ModifiedBy = HttpContext.User.Identity.Name;
                _DamageItemDeatils.Cancelled = true;
                _context.Update(_DamageItemDeatils);
                await _context.SaveChangesAsync();

                ItemTranViewModel _ItemTranViewModel = new();
                _ItemTranViewModel.ItemId = _DamageItemDeatils.ItemId;
                _ItemTranViewModel.TranQuantity = _DamageItemDeatils.TotalDamageItem;
                _ItemTranViewModel.ActionMessage = "Damage Item rollback by delete";
                _ItemTranViewModel.IsAddition = true;
                await _iCommon.CurrentItemsUpdate(_ItemTranViewModel);

                return new JsonResult(_DamageItemDeatils);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Adjustment(long id)
        {
            try
            {
                // Get total adjusted so far
                var totalAdjusted = await _context.DamageItemAdjustments
                    .Where(x => x.DamageItemId == id && x.Cancelled == false)
                    .SumAsync(x => (int?)x.AdjustmentQuantity) ?? 0;

                var damageItem = await (from _DamageItemDeatils in _context.DamageItemDeatils
                                        join _Items in _context.Items on _DamageItemDeatils.ItemId equals _Items.Id
                                        join _Supplier in _context.Supplier on _Items.SupplierId equals _Supplier.Id
                                        where _DamageItemDeatils.Id == id && _DamageItemDeatils.Cancelled == false
                                        select new DamageItemAdjustmentViewModel
                                        {
                                            DamageItemId = _DamageItemDeatils.Id,
                                            ItemId = _Items.Id,
                                            ItemName = _Items.Name,
                                            SupplierName = _Supplier.Name,
                                            TotalDamageItem = _DamageItemDeatils.TotalDamageItem,
                                            CurrentStock = _Items.Quantity,
                                            TotalAdjustedSoFar = totalAdjusted,  // ADD THIS
                                            RemainingToAdjust = _DamageItemDeatils.TotalDamageItem - totalAdjusted,  // ADD THIS
                                            AdjustmentDate = DateTime.Now
                                        }).FirstOrDefaultAsync();

                if (damageItem == null)
                {
                    return NotFound();
                }

                return PartialView("_Adjustment", damageItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveAdjustment(DamageItemAdjustmentViewModel vm)
        {
            try
            {
                // Log for debugging
                System.Diagnostics.Debug.WriteLine($"Received: DamageItemId={vm.DamageItemId}, ItemId={vm.ItemId}, Qty={vm.AdjustmentQuantity}");

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return new JsonResult("Validation failed: " + string.Join(", ", errors));
                }

                // Validate adjustment quantity
                if (vm.AdjustmentQuantity <= 0)
                {
                    return new JsonResult("Adjustment quantity must be greater than 0.");
                }

                // FETCH THE DAMAGE ITEM FROM DATABASE
                var damageItem = await _context.DamageItemDeatils
                    .FirstOrDefaultAsync(x => x.Id == vm.DamageItemId && x.Cancelled == false);

                if (damageItem == null)
                {
                    return new JsonResult("Damage item not found.");
                }

                // USE DATABASE VALUE INSTEAD OF vm.TotalDamageItem
                if (vm.AdjustmentQuantity > damageItem.TotalDamageItem)
                {
                    return new JsonResult($"Adjustment quantity ({vm.AdjustmentQuantity}) cannot exceed total damaged items ({damageItem.TotalDamageItem}).");
                }

                // Check if total adjustments would exceed damaged quantity
                var totalAdjusted = await _context.DamageItemAdjustments
                    .Where(x => x.DamageItemId == vm.DamageItemId && x.Cancelled == false)
                    .SumAsync(x => (int?)x.AdjustmentQuantity) ?? 0;

                if (totalAdjusted + vm.AdjustmentQuantity > damageItem.TotalDamageItem)
                {
                    return new JsonResult($"Total adjustments ({totalAdjusted + vm.AdjustmentQuantity}) would exceed total damaged items ({damageItem.TotalDamageItem}). Already adjusted: {totalAdjusted}");
                }

                // Create adjustment record
                DamageItemAdjustment adjustment = new DamageItemAdjustment
                {
                    DamageItemId = vm.DamageItemId,
                    ItemId = vm.ItemId,
                    AdjustmentQuantity = vm.AdjustmentQuantity,
                    AdjustmentReason = vm.AdjustmentReason,
                    AdjustmentDate = vm.AdjustmentDate,
                    CreatedBy = HttpContext.User.Identity.Name,
                    CreatedDate = DateTime.Now,
                    Cancelled = false
                };

                _context.DamageItemAdjustments.Add(adjustment);
                await _context.SaveChangesAsync();

                // Update inventory - Add received items back to stock
                ItemTranViewModel itemTran = new ItemTranViewModel
                {
                    ItemId = vm.ItemId,
                    TranQuantity = vm.AdjustmentQuantity,
                    ActionMessage = $"Damage adjustment - Received {vm.AdjustmentQuantity} items from supplier (Adjustment ID: {adjustment.Id})",
                    IsAddition = true,
                    CurrentUserName = HttpContext.User.Identity.Name
                };

                await _iCommon.CurrentItemsUpdate(itemTran);

                var message = $"Adjustment saved successfully! {vm.AdjustmentQuantity} items added back to inventory. Adjustment ID: {adjustment.Id}";
                return new JsonResult(message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in SaveAdjustment: {ex.Message}");
                return new JsonResult($"Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAdjustmentHistory(long damageItemId)
        {
            try
            {
                var history = await _context.DamageItemAdjustments
                    .Where(x => x.DamageItemId == damageItemId && x.Cancelled == false)
                    .OrderByDescending(x => x.CreatedDate)
                    .Select(x => new
                    {
                        x.Id,
                        x.AdjustmentQuantity,
                        x.AdjustmentReason,
                        AdjustmentDate = x.AdjustmentDate.ToString("MM/dd/yyyy"),
                        x.CreatedBy,
                        CreatedDate = x.CreatedDate.ToString("MM/dd/yyyy HH:mm")
                    })
                    .ToListAsync();

                return new JsonResult(history);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetByItem(int Id)
        {
            ItemsCRUDViewModel result = (from _Items in _context.Items
                                         join _UnitsofMeasure in _context.UnitsofMeasure on _Items.MeasureId equals _UnitsofMeasure.Id
                                         into _UnitsofMeasure
                                         from listUnitsofMeasure in _UnitsofMeasure.DefaultIfEmpty()

                                         where _Items.Id == Id
                                         select new ItemsCRUDViewModel
                                         {
                                             Id = _Items.Id,
                                             Name = _Items.Name,
                                             MeasureDisplay = listUnitsofMeasure.Name,
                                             Quantity = _Items.Quantity,
                                             CostPrice = _Items.CostPrice,
                                             NormalPrice = _Items.NormalPrice,
                                             WarehouseId = _Items.WarehouseId
                                         }).FirstOrDefault();
            return new JsonResult(result);
        }
    }
}
