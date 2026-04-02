using BusinessERP.Models.AccountViewModels;
using BusinessERP.Models.CommonViewModel;
using BusinessERP.Pages;

namespace BusinessERP.Models.DashboardViewModel
{
    public class SharedUIDataViewModel
    {
        public UserProfile UserProfile { get; set; }
        public ApplicationInfo ApplicationInfo { get; set; }
        public MainMenuViewModel MainMenuViewModel { get; set; }
        public LoginViewModel LoginViewModel { get; set; }
    }
}
