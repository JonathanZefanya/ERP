using BusinessERP.Data;
using BusinessERP.Models;
using BusinessERP.Models.CommonViewModel;
using BusinessERP.Models.UserProfileViewModel;
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
    public class UserProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileController(UserManager<ApplicationUser> userManager, ICommon iCommon,
            ApplicationDbContext context)
        {
            _context = context;
            _iCommon = iCommon;
            _userManager = userManager;
        }

        [Authorize(Roles = Pages.MainMenu.UserProfile.RoleName)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var _UserName = User.Identity.Name;
            var _UserProfile = _context.UserProfile.FirstOrDefault(x => x.Email == _UserName);
            var result = await _iCommon.GetUserProfileDetails().Where(x => x.UserProfileId == _UserProfile.UserProfileId).FirstOrDefaultAsync();
            return View(result);
        }

        [HttpGet]
        public IActionResult EditUserProfile(Int64 id)
        {
            UserProfileCRUDViewModel _UserProfileViewModel = _iCommon.GetByUserProfile(id);
            return PartialView("_EditUserProfile", _UserProfileViewModel);
        }
        [HttpPost]
        public async Task<JsonResult> SaveUserProfile(UserProfileCRUDViewModel _UserProfileViewModel)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                UserProfile _UserProfile = _iCommon.GetByUserProfile(_UserProfileViewModel.UserProfileId);
                if (_UserProfileViewModel.ProfilePictureDetails != null)
                    _UserProfileViewModel.ProfilePicture = "/upload/" + _iCommon.UploadedFile(_UserProfileViewModel.ProfilePictureDetails);
                else
                    _UserProfileViewModel.ProfilePicture = _UserProfile.ProfilePicture;

                _UserProfileViewModel.RoleId = _UserProfile.RoleId;
                _UserProfileViewModel.BranchId = _UserProfile.BranchId;
                _UserProfileViewModel.ModifiedDate = DateTime.Now;
                _UserProfileViewModel.ModifiedBy = HttpContext.User.Identity.Name;
                _UserProfileViewModel.CreatedDate = _UserProfile.CreatedDate;
                _UserProfileViewModel.CreatedBy = _UserProfile.CreatedBy;
                _context.Entry(_UserProfile).CurrentValues.SetValues(_UserProfileViewModel);
                await _context.SaveChangesAsync();

                _JsonResultViewModel.AlertMessage = "User info Updated Successfully. User Name: " + _UserProfile.Email;
                _JsonResultViewModel.CurrentURL = _UserProfileViewModel.CurrentURL;
                _JsonResultViewModel.IsSuccess = true;
                return new JsonResult(_JsonResultViewModel);
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                return new JsonResult(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string ApplicationUserId)
        {
            var _ApplicationUser = await _userManager.FindByIdAsync(ApplicationUserId);
            ResetPasswordViewModel _ResetPasswordViewModel = new();
            _ResetPasswordViewModel.ApplicationUserId = _ApplicationUser.Id;
            return PartialView("_ResetPassword", _ResetPasswordViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveResetPassword(ResetPasswordViewModel vm)
        {
            try
            {
                string AlertMessage = string.Empty;
                var _ApplicationUser = await _userManager.FindByIdAsync(vm.ApplicationUserId);
                if (vm.NewPassword.Equals(vm.ConfirmPassword))
                {
                    var result = await _userManager.ChangePasswordAsync(_ApplicationUser, vm.OldPassword, vm.NewPassword);
                    if (result.Succeeded)
                        AlertMessage = "Change Password Succeeded. User name: " + _ApplicationUser.Email;
                    else
                    {
                        string errorMessage = string.Empty;
                        foreach (var item in result.Errors)
                        {
                            errorMessage = errorMessage + " " + item.Description;
                        }
                        AlertMessage = "error" + errorMessage;
                    }
                }
                return new JsonResult(AlertMessage);
            }
            catch (Exception ex)
            {
                return new JsonResult("error" + ex.Message);
                throw;
            }
        }
    }
}
