using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.IncomeCategoryViewModel
{
    public class IncomeCategoryCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }


        public static implicit operator IncomeCategoryCRUDViewModel(IncomeCategory _IncomeCategory)
        {
            return new IncomeCategoryCRUDViewModel
            {
                Id = _IncomeCategory.Id,
                Name = _IncomeCategory.Name,
                Description = _IncomeCategory.Description,
                CreatedDate = _IncomeCategory.CreatedDate,
                ModifiedDate = _IncomeCategory.ModifiedDate,
                CreatedBy = _IncomeCategory.CreatedBy,
                ModifiedBy = _IncomeCategory.ModifiedBy,
                Cancelled = _IncomeCategory.Cancelled,
            };
        }

        public static implicit operator IncomeCategory(IncomeCategoryCRUDViewModel vm)
        {
            return new IncomeCategory
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
