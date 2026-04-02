using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models;
using BusinessERP.Models.EmployeeViewModel;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public EmployeeController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.Employee.RoleName)]
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

                var _GetGridItem = _iCommon.GetEmployeeGridList();
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
                    || obj.EmployeeId.ToLower().Contains(searchValue)
                    || obj.FirstName.ToLower().Contains(searchValue)
                    || obj.LastName.ToLower().Contains(searchValue)
                    || obj.DateOfBirth.ToString().Contains(searchValue)
                    || obj.DesignationDisplay.ToLower().Contains(searchValue)
                    || obj.DepartmentDisplay.ToLower().Contains(searchValue));
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
            EmployeeCRUDViewModel vm = new EmployeeCRUDViewModel();
            if (id == null) return NotFound();
            vm = await _iCommon.GetEmployeeList().Where(m => m.Id == id).FirstOrDefaultAsync();
            //vm.listAssetCRUDViewModel = await _iCommon.GetGridAssetList().Where(x => x.AssignEmployeeId == id).ToListAsync();
            //vm.listAssetHistoryCRUDViewModel = _iCommon.GetAssetHistoryList().Where(x => x.AssignEmployeeId == id).ToList();

            if (vm == null) return NotFound();
            return PartialView("_AllInfo", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            EmployeeCRUDViewModel vm = new EmployeeCRUDViewModel();
            ViewBag._LoadddlDepartment = new SelectList(_iCommon.LoadddlDepartment(), "Id", "Name");
            ViewBag._LoadddlSubDepartment = new SelectList(_iCommon.LoadddlSubDepartment(), "Id", "Name");
            ViewBag._LoadddlDesignation = new SelectList(_iCommon.LoadddlDesignation(), "Id", "Name");

            if (id > 0)
            {
                vm = await _context.Employee.Where(x => x.Id == id).FirstOrDefaultAsync();
            }
            else
            {
                vm.EmployeeId = "EMP-" + StaticData.RandomDigits(6);
            }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(EmployeeCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Employee _Employee = new Employee();
                        if (vm.Id > 0)
                        {
                            _Employee = await _context.Employee.FindAsync(vm.Id);

                            vm.CreatedDate = _Employee.CreatedDate;
                            vm.CreatedBy = _Employee.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_Employee).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();
                            
                            var _AlertMessage = "Employee Updated Successfully. ID: " + _Employee.Id;
                            return new JsonResult(_AlertMessage);
                        }
                        else
                        {
                            _Employee = vm;
                            _Employee.CreatedDate = DateTime.Now;
                            _Employee.ModifiedDate = DateTime.Now;
                            _Employee.CreatedBy = HttpContext.User.Identity.Name;
                            _Employee.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_Employee);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Employee Created Successfully. ID: " + _Employee.Id;
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
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _Employee = await _context.Employee.FindAsync(id);
                _Employee.ModifiedDate = DateTime.Now;
                _Employee.ModifiedBy = HttpContext.User.Identity.Name;
                _Employee.Cancelled = true;

                _context.Update(_Employee);
                await _context.SaveChangesAsync();
                return new JsonResult(_Employee);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
