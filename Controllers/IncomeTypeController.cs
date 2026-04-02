using BusinessERP.Data;
using BusinessERP.Models;
using BusinessERP.Models.IncomeTypeViewModel;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class IncomeTypeController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public IncomeTypeController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.Admin.RoleName)]
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
                var _GetDataTabelData = GetAllIncomeType();

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
        private IQueryable<IncomeType> ApplySearchOnData(IQueryable<IncomeType> query, string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return query;
            }

            searchValue = searchValue.ToLower();
            return query.Where(obj => obj.Id.ToString().Contains(searchValue)
                || obj.Name.ToLower().Contains(searchValue)
                || obj.Description.ToLower().Contains(searchValue)
                || obj.CreatedDate.ToString().ToLower().Contains(searchValue)
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
            IncomeTypeCRUDViewModel vm = await _context.IncomeType.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            IncomeTypeCRUDViewModel vm = new IncomeTypeCRUDViewModel();
            if (id > 0) vm = await _context.IncomeType.Where(x => x.Id == id).FirstOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit([FromBody] IncomeTypeCRUDViewModel vm)
        {
            try
            {
                IncomeType _IncomeType = new();
                string _UserName = HttpContext.User.Identity.Name;
                if (vm.Id > 0)
                {
                    _IncomeType = await _context.IncomeType.FindAsync(vm.Id);

                    vm.CreatedDate = _IncomeType.CreatedDate;
                    vm.CreatedBy = _IncomeType.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_IncomeType).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "IncomeType Updated Successfully. ID: " + _IncomeType.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    _IncomeType = vm;
                    _IncomeType.CreatedDate = DateTime.Now;
                    _IncomeType.ModifiedDate = DateTime.Now;
                    _IncomeType.CreatedBy = _UserName;
                    _IncomeType.ModifiedBy = _UserName;
                    _context.Add(_IncomeType);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "IncomeType Created Successfully. ID: " + _IncomeType.Id;
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
                var _IncomeType = await _context.IncomeType.FindAsync(id);
                _IncomeType.ModifiedDate = DateTime.Now;
                _IncomeType.ModifiedBy = HttpContext.User.Identity.Name;
                _IncomeType.Cancelled = true;

                _context.Update(_IncomeType);
                await _context.SaveChangesAsync();
                return new JsonResult(_IncomeType);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetAllType()
        {
            try
            {
                var result = await GetAllIncomeType().ToListAsync();
                return new JsonResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private IQueryable<IncomeType> GetAllIncomeType()
        {
            try
            {
                var result = _context.IncomeType.Where(x => x.Cancelled == false);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
