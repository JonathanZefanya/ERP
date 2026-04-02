using System.ComponentModel.DataAnnotations;
using BusinessERP.Helpers;

namespace BusinessERP.Models.UserProfileViewModel
{
    public class UserProfileCRUDViewModel : EntityBase
    {
        [Display(Name = "User Profile Id")]
        public Int64 UserProfileId { get; set; }
        [Display(Name = "Employee Id")]
        public string EmployeeId { get; set; } = "EMP-" + StaticData.RandomDigits(6);
        public string ApplicationUserId { get; set; }
        [Display(Name = "First Name"), Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name"), Required]
        public string LastName { get; set; }
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; } = DateTime.Today;
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; } = DateTime.Today;
        [Display(Name = "Leaving Date")]
        public DateTime LeavingDate { get; set; } = DateTime.Today;
        public string PhoneNumber { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        [Display(Name = "Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("PasswordHash", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePictureDetails { get; set; }
        public string ProfilePicture { get; set; } = "/upload/blank-person.png";
        [Display(Name = "User Role")]
        public Int64 RoleId { get; set; }
        public string RoleIdDisplay { get; set; }
        public string IsApproverDisplay { get; set; }
        [Display(Name = "Branch"), Required]
        public Int64 BranchId { get; set; }
        public string BranchDisplay { get; set; }
        public string CurrentURL { get; set; }


        public static implicit operator UserProfileCRUDViewModel(UserProfile vm)
        {
            return new UserProfileCRUDViewModel
            {
                UserProfileId = vm.UserProfileId,
                EmployeeId = vm.EmployeeId,
                ApplicationUserId = vm.ApplicationUserId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                PhoneNumber = vm.PhoneNumber,
                Email = vm.Email,
                Address = vm.Address,
                Country = vm.Country,
                ProfilePicture = vm.ProfilePicture,
                BranchId = vm.BranchId,
                RoleId = vm.RoleId,
                DateOfBirth = vm.DateOfBirth,
                JoiningDate = vm.JoiningDate,
                LeavingDate = vm.LeavingDate,

                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }

        public static implicit operator UserProfile(UserProfileCRUDViewModel vm)
        {
            return new UserProfile
            {
                UserProfileId = vm.UserProfileId,
                EmployeeId = vm.EmployeeId,
                ApplicationUserId = vm.ApplicationUserId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                PhoneNumber = vm.PhoneNumber,
                Email = vm.Email,
                Address = vm.Address,
                Country = vm.Country,
                ProfilePicture = vm.ProfilePicture,
                BranchId = vm.BranchId,
                RoleId = vm.RoleId,
                DateOfBirth = vm.DateOfBirth,
                JoiningDate = vm.JoiningDate,
                LeavingDate = vm.LeavingDate,

                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
