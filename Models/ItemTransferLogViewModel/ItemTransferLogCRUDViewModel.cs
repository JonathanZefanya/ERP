using System.ComponentModel.DataAnnotations;
using BusinessERP.Models.WarehouseViewModel;

namespace BusinessERP.Models.ItemTransferLogViewModel
{
    public class ItemTransferLogCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }

        [Required]
        [Display(Name = "Item Name")]
        public Int64 ItemId { get; set; }
        public string ItemDisplay { get; set; }
        [Display(Name = "Current Total Stock")]
        public int CurrentTotalStock { get; set; }
        [Display(Name = "Total Transfer Item")]
        [Required]
        public int TotalTransferItem { get; set; }
        [Display(Name = "From Warehouse")]
        [Required]
        public Int64? FromWarehouseId { get; set; }
        public string FromWarehouseDisplay { get; set; }
        [Display(Name = "To Warehouse")]
        [Required]
        public Int64 ToWarehouseId { get; set; }
        public string ToWarehouseDisplay { get; set; }
        [Display(Name = "Reason Of Transfer")]
        public string ReasonOfTransfer { get; set; }

        public static implicit operator ItemTransferLogCRUDViewModel(ItemTransferLog _ItemTransferLog)
        {
            return new ItemTransferLogCRUDViewModel
            {
                Id = _ItemTransferLog.Id,
                ItemId = _ItemTransferLog.ItemId,
                CurrentTotalStock = _ItemTransferLog.CurrentTotalStock,
                TotalTransferItem = _ItemTransferLog.TotalTransferItem,
                FromWarehouseId = _ItemTransferLog.FromWarehouseId,
                ToWarehouseId = _ItemTransferLog.ToWarehouseId,
                ReasonOfTransfer = _ItemTransferLog.ReasonOfTransfer,
                CreatedDate = _ItemTransferLog.CreatedDate,
                ModifiedDate = _ItemTransferLog.ModifiedDate,
                CreatedBy = _ItemTransferLog.CreatedBy,
                ModifiedBy = _ItemTransferLog.ModifiedBy,
                Cancelled = _ItemTransferLog.Cancelled,
            };
        }
        public static implicit operator ItemTransferLog(ItemTransferLogCRUDViewModel vm)
        {
            return new ItemTransferLog
            {
                Id = vm.Id,
                ItemId = vm.ItemId,
                CurrentTotalStock = vm.CurrentTotalStock,
                TotalTransferItem = vm.TotalTransferItem,
                FromWarehouseId = vm.FromWarehouseId,
                ToWarehouseId = vm.ToWarehouseId,
                ReasonOfTransfer = vm.ReasonOfTransfer,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }

        public static implicit operator ItemTransferLogCRUDViewModel(TransferItemViewModel vm)
        {
            return new ItemTransferLogCRUDViewModel
            {
                ItemId = vm.ItemId,
                CurrentTotalStock = vm.CurrentTotalStock,
                TotalTransferItem = vm.TotalTransferItem,
                FromWarehouseId = vm.FromWarehouseId,
                ToWarehouseId = vm.ToWarehouseId,
                ReasonOfTransfer = vm.ReasonOfTransfer,
            };
        }
    }
}
