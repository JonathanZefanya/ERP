using BusinessERP.Models;
using BusinessERP.Models.CommonViewModel;
using BusinessERP.Models.ManageUserRolesVM;
using BusinessERP.Models.UserProfileViewModel;
using BusinessERP.Pages;
using BusinessERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BusinessERP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class SystemRoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICommon _iCommon;
        private readonly IRoles _roles;

        public SystemRoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ICommon iCommon, IRoles roles)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _iCommon = iCommon;
            _roles = roles;
        }

        [Authorize(Roles = Pages.MainMenu.SystemRole.RoleName)]
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
                    _GetGridItem = _GetGridItem.Where(obj => obj.RoleId_SL.ToString().Contains(searchValue)
                    || obj.RoleName.ToLower().Contains(searchValue));
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
        private IQueryable<ManageUserRolesViewModel> GetGridItem()
        {
            List<ManageUserRolesViewModel> list = new List<ManageUserRolesViewModel>();
            try
            {
                var result = _roleManager.Roles.OrderBy(x => x.Name).ToList();
                int Count = 1;
                foreach (var role in result)
                {
                    var userRolesViewModel = new ManageUserRolesViewModel
                    {
                        RoleId = role.Id,
                        RoleId_SL = Count,
                        RoleName = role.Name
                    };
                    list.Add(userRolesViewModel);
                    Count++;
                }
                return list.AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult AddNewRole()
        {
            return PartialView("_AddNewRole");
        }
        [HttpPost]
        public async Task<IActionResult> SaveAddNewRole(AddNewRoleViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                var _CreateSingleRole = await _roles.CreateSingleRole(vm.RoleName);
                _JsonResultViewModel.AlertMessage = _CreateSingleRole;

                if (_CreateSingleRole.Contains("Created"))
                    _JsonResultViewModel.IsSuccess = true;
                else
                    _JsonResultViewModel.IsSuccess = false;
                return new JsonResult(_JsonResultViewModel);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<JsonResult> DeleteRole(string RoleName)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(RoleName);
                var result = await _roleManager.DeleteAsync(role);
                return new JsonResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}