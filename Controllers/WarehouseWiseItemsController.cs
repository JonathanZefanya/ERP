using BusinessERP.Data;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using BusinessERP.Models.WarehouseViewModel;

namespace InventoryMNM.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class WarehouseWiseItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WarehouseWiseItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = BusinessERP.Pages.MainMenu.Warehouse.RoleName)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var _WarehouseItemsGroupBy = await _context.Items.Where(x => x.Cancelled == false && x.Quantity > 0).GroupBy(p => p.WarehouseId).Select(g => new WarehouseWiseItemsViewModel
            {
                WarehouseId = (Int64)g.Key,
                TotalAvailableItem = g.Count(),
                TotalAvailableQuantity = g.Sum(t => t.Quantity),
            }).ToListAsync();

            var vm = (from _Obj in _WarehouseItemsGroupBy
                      join _Warehouse in _context.Warehouse
                      on _Obj.WarehouseId equals _Warehouse.Id
                      where (_Warehouse.Cancelled == false)
                      select new WarehouseWiseItemsViewModel
                      {
                          WarehouseId = _Obj.WarehouseId,
                          WarehouseName = _Warehouse.Name,
                          TotalAvailableItem = _Obj.TotalAvailableItem,
                          TotalAvailableQuantity = _Obj.TotalAvailableQuantity
                      }).ToList();

            return View(vm);
        }
    }
}
