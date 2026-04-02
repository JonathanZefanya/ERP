using BusinessERP.Data;
using BusinessERP.Models;
using BusinessERP.Models.CommonViewModel;
using BusinessERP.Models.CompanyInfoViewModel;
using BusinessERP.Pages;
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
    public class CompanyInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public CompanyInfoController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = MainMenu.CompanyInfo.RoleName)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await GetByCompanyInfo();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            ViewBag.ddlCurrency = new SelectList(_iCommon.LoadddlCurrencyItem(), "Id", "Name");
            ViewBag.ddlVatPercentage = new SelectList(_iCommon.GetTableData<VatPercentage>(x => x.Cancelled == false).OrderByDescending(x => x.Id), "Id", "Name");
            ViewBag.ddlEmailConfig = new SelectList(_iCommon.GetTableData<EmailConfig>(x => x.Cancelled == false).OrderByDescending(x => x.Id), "Id", "Email");
            var result = await GetByCompanyInfo();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CompanyInfoCRUDViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                CompanyInfo _CompanyInfo = new();
                _CompanyInfo = await _context.CompanyInfo.FindAsync(vm.Id);
                if (vm.LogoDetails != null)
                {
                    vm.Logo = "/upload/" + _iCommon.UploadedFile(vm.LogoDetails);
                }
                else
                {
                    vm.Logo = _CompanyInfo.Logo;
                }

                vm.ModifiedDate = DateTime.Now;
                vm.ModifiedBy = HttpContext.User.Identity.Name;
                _context.Entry(_CompanyInfo).CurrentValues.SetValues(vm);
                await _context.SaveChangesAsync();

                await UpdateDefaultCurrency(vm.CurrencyId);
                await UpdateDefaultItemVATPercentage(vm.ItemVatPercentageId);
                await UpdateDefaultEmailConfig(vm.DefaultSMTPId);


                _JsonResultViewModel.IsSuccess = true;
                _JsonResultViewModel.ModelObject = _CompanyInfo;
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

        private async Task UpdateDefaultCurrency(Int64 CurrencyId)
        {
            try
            {
                //Update Current Default
                Currency _Currency = new();
                var _CurrencyVM1 = await _context.Currency.Where(x => x.IsDefault == true).FirstOrDefaultAsync();
                _Currency = _CurrencyVM1;
                _Currency.IsDefault = false;
                _Currency.CreatedDate = DateTime.Now;
                _Currency.ModifiedDate = DateTime.Now;
                _Currency.CreatedBy = HttpContext.User.Identity.Name;
                _Currency.ModifiedBy = HttpContext.User.Identity.Name;
                _context.Entry(_CurrencyVM1).CurrentValues.SetValues(_Currency);
                await _context.SaveChangesAsync();

                //Update New Default
                var _CurrencyVM2 = await _context.Currency.FindAsync(CurrencyId);
                _Currency = _CurrencyVM2;
                _Currency.IsDefault = true;
                _Currency.CreatedDate = DateTime.Now;
                _Currency.ModifiedDate = DateTime.Now;
                _Currency.CreatedBy = HttpContext.User.Identity.Name;
                _Currency.ModifiedBy = HttpContext.User.Identity.Name;
                _context.Entry(_CurrencyVM2).CurrentValues.SetValues(_Currency);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

            }
        }
        private async Task UpdateDefaultItemVATPercentage(Int64 VatPercentageId)
        {
            try
            {
                //Update Current Default
                VatPercentage _VatPercentage = new();
                var _CurrencyVM1 = await _context.VatPercentage.Where(x => x.IsDefault == true).FirstOrDefaultAsync();
                _VatPercentage = _CurrencyVM1;
                _VatPercentage.IsDefault = false;
                _VatPercentage.CreatedDate = DateTime.Now;
                _VatPercentage.ModifiedDate = DateTime.Now;
                _VatPercentage.CreatedBy = HttpContext.User.Identity.Name;
                _VatPercentage.ModifiedBy = HttpContext.User.Identity.Name;
                _context.Entry(_CurrencyVM1).CurrentValues.SetValues(_VatPercentage);
                await _context.SaveChangesAsync();

                //Update New Default
                var _CurrencyVM2 = await _context.VatPercentage.FindAsync(VatPercentageId);
                _VatPercentage = _CurrencyVM2;
                _VatPercentage.IsDefault = true;
                _VatPercentage.CreatedDate = DateTime.Now;
                _VatPercentage.ModifiedDate = DateTime.Now;
                _VatPercentage.CreatedBy = HttpContext.User.Identity.Name;
                _VatPercentage.ModifiedBy = HttpContext.User.Identity.Name;
                _context.Entry(_CurrencyVM2).CurrentValues.SetValues(_VatPercentage);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

            }
        }
        private async Task UpdateDefaultEmailConfig(Int64 DefaultSMTPId)
        {
            try
            {
                //Update Current Default
                EmailConfig _EmailConfig = new();
                var _CurrencyVM1 = await _context.EmailConfig.Where(x => x.IsDefault == true).FirstOrDefaultAsync();
                _EmailConfig = _CurrencyVM1;
                _EmailConfig.IsDefault = false;
                _EmailConfig.CreatedDate = DateTime.Now;
                _EmailConfig.ModifiedDate = DateTime.Now;
                _EmailConfig.CreatedBy = HttpContext.User.Identity.Name;
                _EmailConfig.ModifiedBy = HttpContext.User.Identity.Name;
                _context.Entry(_CurrencyVM1).CurrentValues.SetValues(_EmailConfig);
                await _context.SaveChangesAsync();

                //Update New Default
                var _CurrencyVM2 = await _context.EmailConfig.FindAsync(DefaultSMTPId);
                _EmailConfig = _CurrencyVM2;
                _EmailConfig.IsDefault = true;
                _EmailConfig.CreatedDate = DateTime.Now;
                _EmailConfig.ModifiedDate = DateTime.Now;
                _EmailConfig.CreatedBy = HttpContext.User.Identity.Name;
                _EmailConfig.ModifiedBy = HttpContext.User.Identity.Name;
                _context.Entry(_CurrencyVM2).CurrentValues.SetValues(_EmailConfig);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanyInfoData()
        {
            var result = await GetByCompanyInfo();
            return new JsonResult(result);
        }
        private async Task<CompanyInfoCRUDViewModel> GetByCompanyInfo()
        {
            CompanyInfoCRUDViewModel result = await _context.CompanyInfo.FirstOrDefaultAsync(m => m.Id == 1);
            return result;
        }
        //[HttpGet("GetByDropDownData")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByDropDownData(string entityName)
        {
            try
            {
                var entityType = _context.Model.GetEntityTypes().FirstOrDefault(t => t.ClrType.Name == entityName)?.ClrType;
                if (entityType == null)
                {
                    return BadRequest(new { message = "Invalid entity name." });
                }

                // Use reflection to get the correct generic Set<TEntity>() method
                var setMethod = typeof(DbContext).GetMethods()
                                .First(m => m.Name == "Set" && m.IsGenericMethod && m.GetParameters().Length == 0);

                // Make the generic method for the specific entity type
                var genericSetMethod = setMethod.MakeGenericMethod(entityType);

                // Invoke the method to get the DbSet<TEntity>
                var dbSet = genericSetMethod.Invoke(_context, null);
                var queryable = dbSet as IQueryable<object>;
                var filteredData = queryable.Where("Cancelled == false");

                // Get the ToListAsync method for IQueryable<TEntity>
                var toListAsyncMethod = typeof(EntityFrameworkQueryableExtensions)
                    .GetMethods()
                    .First(m => m.Name == "ToListAsync" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(entityType);

                // Invoke ToListAsync method
                var resultTask = (Task)toListAsyncMethod.Invoke(null, new object[] { filteredData, CancellationToken.None });
                await resultTask.ConfigureAwait(false);

                // Get the result of the task
                var resultProperty = resultTask.GetType().GetProperty("Result");
                var result = resultProperty.GetValue(resultTask);

                return Json(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching data.", details = ex.Message });
            }
        }
    }
}

