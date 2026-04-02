using BusinessERP.Data;
using BusinessERP.Models;
using BusinessERP.Models.AttendanceViewModel;
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
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public AttendanceController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.Attendance.RoleName)]
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

                var _GetGridItem = _iCommon.GetAttendanceReportData();
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
                    || obj.EmployeeName.ToLower().Contains(searchValue)
                    //|| obj.CheckIn.ToString().ToLower().Contains(searchValue)
                    //|| obj.CheckOut.ToString().ToLower().Contains(searchValue)
                    || obj.StayTime.ToString().ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue)
                    || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)

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
        public IActionResult Details(long? id)
        {
            var result = _iCommon.GetAttendanceReportData().Where(x => x.Id == id).FirstOrDefault();
            return PartialView("_Details", result);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag._LoadddlEmployee = new SelectList(_iCommon.LoadddlEmployee(), "Id", "Name");
            AttendanceCRUDViewModel vm = new AttendanceCRUDViewModel();
            if (id > 0)
            {
                vm = await _context.Attendance.Where(x => x.Id == id).FirstOrDefaultAsync();
            }
            else
            {
                vm.CheckIn = DateTime.Today.AddHours(9);
                vm.CheckOut = DateTime.Today.AddHours(18);
            }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(AttendanceCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Attendance _Attendance = new Attendance();
                        if (vm.Id > 0)
                        {
                            _Attendance = await _context.Attendance.FindAsync(vm.Id);

                            vm.CreatedDate = _Attendance.CreatedDate;
                            vm.CreatedBy = _Attendance.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_Attendance).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Attendance Updated Successfully. ID: " + _Attendance.Id;
                            return new JsonResult(_AlertMessage);
                        }
                        else
                        {
                            var _StayTime = vm.CheckOut - vm.CheckIn;
                            vm.StayTime = _StayTime;

                            _Attendance = vm;
                            _Attendance.CreatedDate = DateTime.Now;
                            _Attendance.ModifiedDate = DateTime.Now;
                            _Attendance.CreatedBy = HttpContext.User.Identity.Name;
                            _Attendance.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_Attendance);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Attendance Created Successfully. ID: " + _Attendance.Id;
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
                var _Attendance = await _context.Attendance.FindAsync(id);
                _Attendance.ModifiedDate = DateTime.Now;
                _Attendance.ModifiedBy = HttpContext.User.Identity.Name;
                _Attendance.Cancelled = true;

                _context.Update(_Attendance);
                await _context.SaveChangesAsync();
                return new JsonResult(_Attendance);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsExists(long id)
        {
            return _context.Attendance.Any(e => e.Id == id);
        }
    }
}
