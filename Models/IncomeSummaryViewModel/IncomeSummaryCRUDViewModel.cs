using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.IncomeSummaryViewModel
{
    public class IncomeSummaryCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Required]
        [Display(Name = "Short Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Type")]
        public Int64 TypeId { get; set; }
        public string TypeDisplay { get; set; }
        [Required]
        [Display(Name = "Category")]
        public Int64 CategoryId { get; set; }
        public string CategoryDisplay { get; set; }
        [Required]
        public double Amount { get; set; }
        public string Description { get; set; }
        [Display(Name = "Income Date")]
        public DateTime IncomeDate { get; set; } = DateTime.Now;


        public static implicit operator IncomeSummaryCRUDViewModel(IncomeSummary _IncomeSummary)
        {
            return new IncomeSummaryCRUDViewModel
            {
                Id = _IncomeSummary.Id,
                Title = _IncomeSummary.Title,
                TypeId = _IncomeSummary.TypeId,
                CategoryId = _IncomeSummary.CategoryId,
                Amount = _IncomeSummary.Amount,
                Description = _IncomeSummary.Description,
                IncomeDate = _IncomeSummary.IncomeDate,
                CreatedDate = _IncomeSummary.CreatedDate,
                ModifiedDate = _IncomeSummary.ModifiedDate,
                CreatedBy = _IncomeSummary.CreatedBy,
                ModifiedBy = _IncomeSummary.ModifiedBy,
                Cancelled = _IncomeSummary.Cancelled,
            };
        }

        public static implicit operator IncomeSummary(IncomeSummaryCRUDViewModel vm)
        {
            return new IncomeSummary
            {
                Id = vm.Id,
                Title = vm.Title,
                TypeId = vm.TypeId,
                CategoryId = vm.CategoryId,
                Amount = vm.Amount,
                Description = vm.Description,
                IncomeDate = vm.IncomeDate,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
