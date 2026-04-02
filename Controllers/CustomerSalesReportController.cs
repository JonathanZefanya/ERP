using BusinessERP.Data;
using BusinessERP.Models.CommonViewModel;
using BusinessERP.Models.PaymentViewModel;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class CustomerSalesReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISalesService _iSalesService;

        public CustomerSalesReportController(ApplicationDbContext context, ISalesService iSalesService)
        {
            _context = context;
            _iSalesService = iSalesService;
        }

        [Authorize(Roles = Pages.MainMenu.Admin.RoleName)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetByCustomerReportData(Int64 CustomerId)
        {
            CustomerReportDataViewModel _CustomerReportDataViewModel = new();
            var _CustomerInfo = await _context.CustomerInfo.Where(x => x.Id == CustomerId).FirstOrDefaultAsync();
            _CustomerReportDataViewModel.CustomerName = _CustomerInfo.Name;
            _CustomerReportDataViewModel.CustomerAddress = _CustomerInfo.BillingAddress;

            _CustomerReportDataViewModel.listCustomerReportViewModel = await _iSalesService.GetCustomerReportData().Where(obj => obj.CustomerId == CustomerId).ToListAsync();
            return new JsonResult(_CustomerReportDataViewModel);
        }
        [HttpGet]
        public JsonResult GetAllReportCustomer()
        {
            var result = from tblObj in _context.CustomerInfo.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                         select new ItemDropdownListViewModel
                         {
                             Id = tblObj.Id,
                             Name = tblObj.Name + ", Cell: " + tblObj.Phone,
                         };
            return new JsonResult(result);
        }
    }
}
