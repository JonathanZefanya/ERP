using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.AccTransferViewModel
{
    public class AccTransferCRUDViewModel : EntityBase
    {
        [Display(Name = "SL"), Required]
        public Int64 Id { get; set; }
        [Display(Name = "Sender"), Required]
        public Int64 SenderId { get; set; }
        public string SenderDisplay { get; set; }
        [Display(Name = "Receiver"), Required]
        public Int64 ReceiverId { get; set; }
        public string ReceiverDisplay { get; set; }
        public DateTime TransferDate { get; set; } = DateTime.Now;
        public double Amount { get; set; }
        public string Note { get; set; }


        public static implicit operator AccTransferCRUDViewModel(AccTransfer _AccTransfer)
        {
            return new AccTransferCRUDViewModel
            {
                Id = _AccTransfer.Id,
                SenderId = _AccTransfer.SenderId,
                ReceiverId = _AccTransfer.ReceiverId,
                TransferDate = _AccTransfer.TransferDate,
                Amount = _AccTransfer.Amount,
                Note = _AccTransfer.Note,
                CreatedDate = _AccTransfer.CreatedDate,
                ModifiedDate = _AccTransfer.ModifiedDate,
                CreatedBy = _AccTransfer.CreatedBy,
                ModifiedBy = _AccTransfer.ModifiedBy,
                Cancelled = _AccTransfer.Cancelled,
            };
        }

        public static implicit operator AccTransfer(AccTransferCRUDViewModel vm)
        {
            return new AccTransfer
            {
                Id = vm.Id,
                SenderId = vm.SenderId,
                ReceiverId = vm.ReceiverId,
                TransferDate = vm.TransferDate,
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
