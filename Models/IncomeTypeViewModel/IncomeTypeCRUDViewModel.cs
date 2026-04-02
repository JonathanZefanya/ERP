using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.IncomeTypeViewModel
{
    public class IncomeTypeCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public static implicit operator IncomeTypeCRUDViewModel(IncomeType _IncomeType)
        {
            return new IncomeTypeCRUDViewModel
            {
                Id = _IncomeType.Id,
                Name = _IncomeType.Name,
                Description = _IncomeType.Description,
                CreatedDate = _IncomeType.CreatedDate,
                ModifiedDate = _IncomeType.ModifiedDate,
                CreatedBy = _IncomeType.CreatedBy,
                ModifiedBy = _IncomeType.ModifiedBy,
                Cancelled = _IncomeType.Cancelled,
            };
        }

        public static implicit operator IncomeType(IncomeTypeCRUDViewModel vm)
        {
            return new IncomeType
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
