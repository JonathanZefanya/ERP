
using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.ItemsViewModel
{
    public class BulkItemViewModel
    {
        [Display(Name = "File Path"), Required]
        public string FilePath { get; set; } = "wwwroot/upload/BulkUploadItems/data.csv";
    }
}