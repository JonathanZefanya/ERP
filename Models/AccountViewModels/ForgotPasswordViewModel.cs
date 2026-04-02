using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
