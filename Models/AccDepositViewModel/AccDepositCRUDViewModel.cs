using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.AccDepositViewModel
{
    public class AccDepositCRUDViewModel : EntityBase
    {
        [Display(Name = "SL"), Required]
        public Int64 Id { get; set; }
        [Display(Name = "Account"), Required]
        public Int64 AccountId { get; set; }
        public string AccountDisplay { get; set; }
        [Display(Name = "Deposit Date"), Required]
        public DateTime DepositDate { get; set; } = DateTime.Now;
        [Required]
        public double Amount { get; set; }
        public string Note { get; set; }


        public static implicit operator AccDepositCRUDViewModel(AccDeposit _AccDeposit)
        {
            return new AccDepositCRUDViewModel
            {
                Id = _AccDeposit.Id,
                AccountId = _AccDeposit.AccountId,
                DepositDate = _AccDeposit.DepositDate,
                Amount = _AccDeposit.Amount,
                Note = _AccDeposit.Note,
                CreatedDate = _AccDeposit.CreatedDate,
                ModifiedDate = _AccDeposit.ModifiedDate,
                CreatedBy = _AccDeposit.CreatedBy,
                ModifiedBy = _AccDeposit.ModifiedBy,
                Cancelled = _AccDeposit.Cancelled,
            };
        }

        public static implicit operator AccDeposit(AccDepositCRUDViewModel vm)
        {
            return new AccDeposit
            {
                Id = vm.Id,
                AccountId = vm.AccountId,
                DepositDate = vm.DepositDate,
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
