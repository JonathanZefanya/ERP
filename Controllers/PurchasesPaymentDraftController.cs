using BusinessERP.ConHelper;
using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class PurchasesPaymentDraftController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPurchaseService _iPurchaseService; 

        public PurchasesPaymentDraftController(ApplicationDbContext context, IPurchaseService iPurchaseService, IPaymentService iPaymentService, IEmailSender emailSender)
        {
            _context = context;
            _iPurchaseService = iPurchaseService;
        }

        [Authorize(Roles = Pages.MainMenu.PurchasesPaymentDraft.RoleName)]
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
              
                var _GetGridItem = _iPurchaseService.GetPurchasesPaymentGridData().Where(x => x.Category == InvoiceType.DraftInvoice);
                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.Discount.ToString().Contains(searchValue)
                    //|| obj.CustomerName.ToLower().Contains(searchValue)
                    //|| obj.Discount.ToString().ToLower().Contains(searchValue)
                    || obj.VAT.ToString().ToLower().Contains(searchValue)
                    || obj.SubTotal.ToString().ToLower().Contains(searchValue)
                    || obj.GrandTotal.ToString().ToLower().Contains(searchValue)
                    || obj.PaidAmount.ToString().ToLower().Contains(searchValue)
                    || obj.DueAmount.ToString().ToLower().Contains(searchValue));
                    //|| obj.CreatedDate.ToString().Contains(searchValue));
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

    }
}
