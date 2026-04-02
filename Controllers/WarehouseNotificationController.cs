using BusinessERP.Data;
using BusinessERP.Models;
using BusinessERP.Models.WarehouseNotificationViewModel;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace InventoryMNM.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class WarehouseNotificationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public WarehouseNotificationController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = BusinessERP.Pages.MainMenu.Warehouse.RoleName)]
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

                var _GetGridItem = GetWarehouseNotificationList();
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
                    || obj.FromWarehouseDisplay.ToLower().Contains(searchValue)
                    || obj.ToWarehouseDisplay.ToLower().Contains(searchValue)
                    || obj.ItemId.ToString().ToLower().Contains(searchValue)
                    || obj.ReceiveQuantity.ToString().ToLower().Contains(searchValue)
                    || obj.SendQuantity.ToString().ToLower().Contains(searchValue)
                    || obj.Message.ToLower().Contains(searchValue)

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
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            WarehouseNotificationCRUDViewModel vm = await GetWarehouseNotificationList().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            WarehouseNotificationCRUDViewModel vm = new WarehouseNotificationCRUDViewModel();
            if (id > 0) vm = await _context.WarehouseNotification.Where(x => x.Id == id).FirstOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(WarehouseNotificationCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        WarehouseNotification _WarehouseNotification = new WarehouseNotification();
                        if (vm.Id > 0)
                        {
                            _WarehouseNotification = await _context.WarehouseNotification.FindAsync(vm.Id);

                            vm.CreatedDate = _WarehouseNotification.CreatedDate;
                            vm.CreatedBy = _WarehouseNotification.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_WarehouseNotification).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Warehouse Notification Updated Successfully. ID: " + _WarehouseNotification.Id;
                            return new JsonResult(_AlertMessage);
                        }
                        else
                        {
                            _WarehouseNotification = vm;
                            _WarehouseNotification.CreatedDate = DateTime.Now;
                            _WarehouseNotification.ModifiedDate = DateTime.Now;
                            _WarehouseNotification.CreatedBy = HttpContext.User.Identity.Name;
                            _WarehouseNotification.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_WarehouseNotification);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Warehouse Notification Created Successfully. ID: " + _WarehouseNotification.Id;
                            return new JsonResult(_AlertMessage);
                        }
                    }
                    return new JsonResult("Operation failed.");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return new JsonResult(ex.Message);
                }
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(Int64 id)
        {
            try
            {
                var _WarehouseNotification = await _context.WarehouseNotification.FindAsync(id);
                _WarehouseNotification.ModifiedDate = DateTime.Now;
                _WarehouseNotification.ModifiedBy = HttpContext.User.Identity.Name;
                _WarehouseNotification.Cancelled = true;

                _context.Update(_WarehouseNotification);
                await _context.SaveChangesAsync();
                return new JsonResult(_WarehouseNotification);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IQueryable<WarehouseNotificationCRUDViewModel> GetWarehouseNotificationList()
        {
            try
            {
                var result = (from _WarehouseNotification in _context.WarehouseNotification
                              join _WarehouseFrom in _context.Warehouse on _WarehouseNotification.FromWarehouseId equals _WarehouseFrom.Id
                              join _WarehouseTo in _context.Warehouse on _WarehouseNotification.ToWarehouseId equals _WarehouseTo.Id
                              where _WarehouseNotification.IsRead == false
                              select new WarehouseNotificationCRUDViewModel
                              {
                                  Id = _WarehouseNotification.Id,
                                  FromWarehouseId = _WarehouseNotification.FromWarehouseId,
                                  FromWarehouseDisplay = _WarehouseFrom.Name,
                                  ToWarehouseId = _WarehouseNotification.ToWarehouseId,
                                  ToWarehouseDisplay = _WarehouseTo.Name,
                                  ItemId = _WarehouseNotification.ItemId,
                                  ReceiveQuantity = _WarehouseNotification.ReceiveQuantity,
                                  SendQuantity = _WarehouseNotification.SendQuantity,
                                  Message = _WarehouseNotification.Message,

                                  IsRead = _WarehouseNotification.IsRead,
                                  CreatedDate = _WarehouseNotification.CreatedDate,
                                  ModifiedDate = _WarehouseNotification.ModifiedDate,
                                  CreatedBy = _WarehouseNotification.CreatedBy,
                                  ModifiedBy = _WarehouseNotification.ModifiedBy
                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkedAsRead(Int64 id)
        {
            try
            {
                var _Notification = await _context.WarehouseNotification.FindAsync(id);
                _Notification.ModifiedDate = DateTime.Now;
                _Notification.ModifiedBy = HttpContext.User.Identity.Name;
                _Notification.IsRead = true;

                _context.Update(_Notification);
                await _context.SaveChangesAsync();
                return new JsonResult("Notification marked as a read successful. Notification Id: " + _Notification.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkedAllAsRead()
        {
            try
            {              
                var _NotificationList = _context.WarehouseNotification.Where(x => x.IsRead == false).ToList();

                foreach (var item in _NotificationList)
                {
                    item.ModifiedDate = DateTime.Now;
                    item.ModifiedBy = HttpContext.User.Identity.Name;
                    item.IsRead = true;
                }

                _context.UpdateRange(_NotificationList);
                await _context.SaveChangesAsync();
                return new JsonResult("All notification marked as a read successful. . Total Notification Marked as Read: " + _NotificationList.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public IActionResult GetUnreadTotalNotification(int Id)
        {
            try
            {
                var _NotificationList = _context.WarehouseNotification.Where(x => x.IsRead == false).ToList();
                return new JsonResult(_NotificationList.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
