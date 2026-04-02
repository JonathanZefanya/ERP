using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.AccAccountViewModel
{
    public class AccAccountCRUDViewModel : EntityBase
    {
        [Display(Name = "SL"), Required]
        public Int64 Id { get; set; }
        [Display(Name = "Account Name"), Required]
        public string AccountName { get; set; }
        [Display(Name = "Account Number"), Required]
        public string AccountNumber { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
        public double Balance { get; set; }
        public string Description { get; set; }


        public static implicit operator AccAccountCRUDViewModel(AccAccount _AccAccount)
        {
            return new AccAccountCRUDViewModel
            {
                Id = _AccAccount.Id,
                AccountName = _AccAccount.AccountName,
                AccountNumber = _AccAccount.AccountNumber,
                Credit = _AccAccount.Credit,
                Debit = _AccAccount.Debit,
                Balance = _AccAccount.Balance,
                Description = _AccAccount.Description,
                CreatedDate = _AccAccount.CreatedDate,
                ModifiedDate = _AccAccount.ModifiedDate,
                CreatedBy = _AccAccount.CreatedBy,
                ModifiedBy = _AccAccount.ModifiedBy,
                Cancelled = _AccAccount.Cancelled,
            };
        }

        public static implicit operator AccAccount(AccAccountCRUDViewModel vm)
        {
            return new AccAccount
            {
                Id = vm.Id,
                AccountName = vm.AccountName,
                AccountNumber = vm.AccountNumber,
                Credit = vm.Credit,
                Debit = vm.Debit,
                Balance = vm.Balance,
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
