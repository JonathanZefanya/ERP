using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.AccTransactionViewModel
{
    public class AccTransactionCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public Int64 AccountId { get; set; }
        public string AccountDisplay { get; set; }
        public string Type { get; set; }
        public string Reference { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }


        public static implicit operator AccTransactionCRUDViewModel(AccTransaction _AccTransaction)
        {
            return new AccTransactionCRUDViewModel
            {
                Id = _AccTransaction.Id,
                AccountId = _AccTransaction.AccountId,
                Type = _AccTransaction.Type,
                Reference = _AccTransaction.Reference,
                Credit = _AccTransaction.Credit,
                Debit = _AccTransaction.Debit,
                Amount = _AccTransaction.Amount,
                Description = _AccTransaction.Description,
                CreatedDate = _AccTransaction.CreatedDate,
                ModifiedDate = _AccTransaction.ModifiedDate,
                CreatedBy = _AccTransaction.CreatedBy,
                ModifiedBy = _AccTransaction.ModifiedBy,
                Cancelled = _AccTransaction.Cancelled,
            };
        }

        public static implicit operator AccTransaction(AccTransactionCRUDViewModel vm)
        {
            return new AccTransaction
            {
                Id = vm.Id,
                AccountId = vm.AccountId,
                Type = vm.Type,
                Reference = vm.Reference,
                Credit = vm.Credit,
                Debit = vm.Debit,
                Amount = vm.Amount,
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
