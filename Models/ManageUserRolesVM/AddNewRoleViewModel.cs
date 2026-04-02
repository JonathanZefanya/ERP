using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.ManageUserRolesVM
{
    public class AddNewRoleViewModel
    {
        public Int64 Id { get; set; }
        [Display(Name = "Role Name"), Required]
        public string RoleName { get; set; }
    }
}

