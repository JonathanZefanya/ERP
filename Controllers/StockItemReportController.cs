using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class StockItemReportController : Controller
    {
        private readonly ISalesService _iSalesService;

        public StockItemReportController(ISalesService iSalesService)
        {
            _iSalesService = iSalesService;
        }

        [Authorize(Roles = Pages.MainMenu.Admin.RoleName)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetAllStockItemReportData()
        {
            var result = await _iSalesService.GetStockItemReportData().ToListAsync();
            return new JsonResult(result);
        }
    }
}
