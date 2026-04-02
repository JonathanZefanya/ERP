using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.AccExpenseViewModel
{
    public class AccExpenseCRUDViewModel : EntityBase
    {
        [Display(Name = "SL"), Required]
        public Int64 Id { get; set; }
        [Display(Name = "Account"), Required]
        public Int64 AccountId { get; set; }
        public string AccountDisplay { get; set; }
        public string Name { get; set; }
        [Display(Name = "Expense Date"), Required]
        public DateTime ExpenseDate { get; set; } = DateTime.Now;
        [Display(Name = "Amount"), Required]
        public double Amount { get; set; }
        public string Note { get; set; }


        public static implicit operator AccExpenseCRUDViewModel(AccExpense _AccExpense)
        {
            return new AccExpenseCRUDViewModel
            {
                Id = _AccExpense.Id,
                AccountId = _AccExpense.AccountId,
                Name = _AccExpense.Name,
                ExpenseDate = _AccExpense.ExpenseDate,
                Amount = _AccExpense.Amount,
                Note = _AccExpense.Note,

                CreatedDate = _AccExpense.CreatedDate,
                ModifiedDate = _AccExpense.ModifiedDate,
                CreatedBy = _AccExpense.CreatedBy,
                ModifiedBy = _AccExpense.ModifiedBy,
                Cancelled = _AccExpense.Cancelled,
            };
        }

        public static implicit operator AccExpense(AccExpenseCRUDViewModel vm)
        {
            return new AccExpense
            {
                Id = vm.Id,
                AccountId = vm.AccountId,
                Name = vm.Name,
                ExpenseDate = vm.ExpenseDate,
                Amount = vm.Amount,
                Note = vm.Note,

                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
