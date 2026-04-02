using BusinessERP.Models;
using BusinessERP.Models.AccountViewModels;
using BusinessERP.Models.UserProfileViewModel;
using Microsoft.AspNetCore.Identity;

namespace BusinessERP.ConHelper
{
    public interface IAccount
    {
        Task<Tuple<ApplicationUser, IdentityResult>> CreateUserAccount(CreateUserAccountViewModel _CreateUserAccountViewModel);
        Task<Tuple<ApplicationUser, string>> CreateUserProfile(UserProfileCRUDViewModel vm, string LoginUser);        
    }
}
