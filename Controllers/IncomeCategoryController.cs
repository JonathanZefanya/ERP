using BusinessERP.Data;
using BusinessERP.Models;
using BusinessERP.Models.IncomeCategoryViewModel;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class IncomeCategoryController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public IncomeCategoryController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.IncomeCategory.RoleName)]
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
                var _GetDataTabelData = GetAllIncomeCategory();

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
        private IQueryable<IncomeCategory> ApplySearchOnData(IQueryable<IncomeCategory> query, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return query;
            }

            searchValue = searchValue.ToLower();
            return query.Where(obj => obj.Id.ToString().Contains(searchValue)
                || obj.Name.ToLower().Contains(searchValue)
                || obj.Description.ToLower().Contains(searchValue)
                || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)
                || obj.CreatedBy.ToLower().Contains(searchValue)
                || obj.ModifiedBy.ToLower().Contains(searchValue)

               || obj.CreatedDate.ToString().Contains(searchValue)
            );
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            IncomeCategoryCRUDViewModel vm = await _context.IncomeCategory.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            IncomeCategoryCRUDViewModel vm = new IncomeCategoryCRUDViewModel();
            if (id > 0) vm = await _context.IncomeCategory.Where(x => x.Id == id).FirstOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit([FromBody] IncomeCategoryCRUDViewModel vm)
        {
            try
            {
                IncomeCategory _IncomeCategory = new();
                string _UserName = HttpContext.User.Identity.Name;
                if (vm.Id > 0)
                {
                    _IncomeCategory = await _context.IncomeCategory.FindAsync(vm.Id);

                    vm.CreatedDate = _IncomeCategory.CreatedDate;
                    vm.CreatedBy = _IncomeCategory.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_IncomeCategory).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "IncomeCategory Updated Successfully. ID: " + _IncomeCategory.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    _IncomeCategory = vm;
                    _IncomeCategory.CreatedDate = DateTime.Now;
                    _IncomeCategory.ModifiedDate = DateTime.Now;
                    _IncomeCategory.CreatedBy = _UserName;
                    _IncomeCategory.ModifiedBy = _UserName;
                    _context.Add(_IncomeCategory);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "IncomeCategory Created Successfully. ID: " + _IncomeCategory.Id;
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
            try
            {
                var _IncomeCategory = await _context.IncomeCategory.FindAsync(id);
                _IncomeCategory.ModifiedDate = DateTime.Now;
                _IncomeCategory.ModifiedBy = HttpContext.User.Identity.Name;
                _IncomeCategory.Cancelled = true;

                _context.Update(_IncomeCategory);
                await _context.SaveChangesAsync();
                return new JsonResult(_IncomeCategory);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetAllCategory()
        {
            try
            {
                var result = await GetAllIncomeCategory().ToListAsync();
                return new JsonResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private IQueryable<IncomeCategory> GetAllIncomeCategory()
        {
            try
            {
                var result = _context.IncomeCategory.Where(x => x.Cancelled == false);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
