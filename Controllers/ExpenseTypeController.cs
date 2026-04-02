using BusinessERP.Data;
using BusinessERP.Models;
using BusinessERP.Models.ExpenseTypeViewModel;
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
    public class ExpenseTypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public ExpenseTypeController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.ExpenseType.RoleName)]
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
                    || obj.Name.ToLower().Contains(searchValue)
                    || obj.Description.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue)
                    || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)
                    || obj.CreatedBy.ToLower().Contains(searchValue)
                    || obj.ModifiedBy.ToLower().Contains(searchValue)

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

        private IQueryable<ExpenseTypeCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _ExpenseType in _context.ExpenseType
                        where _ExpenseType.Cancelled == false
                        select new ExpenseTypeCRUDViewModel
                        {
                            Id = _ExpenseType.Id,
                            Name = _ExpenseType.Name,
                            Description = _ExpenseType.Description,
                            CreatedDate = _ExpenseType.CreatedDate,
                            ModifiedDate = _ExpenseType.ModifiedDate,
                            CreatedBy = _ExpenseType.CreatedBy,
                            ModifiedBy = _ExpenseType.ModifiedBy,

                        }).OrderByDescending(x => x.Id);
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
            ExpenseTypeCRUDViewModel vm = await _context.ExpenseType.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            ExpenseTypeCRUDViewModel vm = new ExpenseTypeCRUDViewModel();
            if (id > 0) vm = await _context.ExpenseType.Where(x => x.Id == id).FirstOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ExpenseTypeCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        ExpenseType _ExpenseType = new ExpenseType();
                        if (vm.Id > 0)
                        {
                            _ExpenseType = await _context.ExpenseType.FindAsync(vm.Id);

                            vm.CreatedDate = _ExpenseType.CreatedDate;
                            vm.CreatedBy = _ExpenseType.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_ExpenseType).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Expense Type Updated Successfully. ID: " + _ExpenseType.Id;
                            return new JsonResult(_AlertMessage);
                        }
                        else
                        {
                            _ExpenseType = vm;
                            _ExpenseType.CreatedDate = DateTime.Now;
                            _ExpenseType.ModifiedDate = DateTime.Now;
                            _ExpenseType.CreatedBy = HttpContext.User.Identity.Name;
                            _ExpenseType.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_ExpenseType);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Expense Type Created Successfully. ID: " + _ExpenseType.Id;
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
                var _ExpenseType = await _context.ExpenseType.FindAsync(id);
                _ExpenseType.ModifiedDate = DateTime.Now;
                _ExpenseType.ModifiedBy = HttpContext.User.Identity.Name;
                _ExpenseType.Cancelled = true;

                _context.Update(_ExpenseType);
                await _context.SaveChangesAsync();
                return new JsonResult(_ExpenseType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.ExpenseType.Any(e => e.Id == id);
        }
    }
}
