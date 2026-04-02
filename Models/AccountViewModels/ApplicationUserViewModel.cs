namespace BusinessERP.Models.AccountViewModels
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int Type { get; set; }
        public Int64 CustomerId { get; set; }

        public static implicit operator ApplicationUserViewModel(ApplicationUser vm)
        {
            return new ApplicationUserViewModel
            {
                Id = vm.Id,
                UserName = vm.UserName,
                Email = vm.Email,
                EmailConfirmed = vm.EmailConfirmed,
                PhoneNumber = vm.PhoneNumber,
                PhoneNumberConfirmed = vm.PhoneNumberConfirmed,
                LockoutEnabled = vm.LockoutEnabled,
                TwoFactorEnabled = vm.TwoFactorEnabled,
            };
        }
    }
}
