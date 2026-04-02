using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
