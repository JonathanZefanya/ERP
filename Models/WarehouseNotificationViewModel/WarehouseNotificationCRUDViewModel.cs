using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.WarehouseNotificationViewModel
{
    public class WarehouseNotificationCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public Int64? FromWarehouseId { get; set; }
        public string FromWarehouseDisplay { get; set; }
        public Int64 ToWarehouseId { get; set; }
        public string ToWarehouseDisplay { get; set; }
        public Int64 ItemId { get; set; }
        public int ReceiveQuantity { get; set; }
        public int SendQuantity { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string UserName {get; set; }


        public static implicit operator WarehouseNotificationCRUDViewModel(WarehouseNotification _WarehouseNotification)
        {
            return new WarehouseNotificationCRUDViewModel
            {
                Id = _WarehouseNotification.Id,
                FromWarehouseId = _WarehouseNotification.FromWarehouseId,
                ToWarehouseId = _WarehouseNotification.ToWarehouseId,
                ItemId = _WarehouseNotification.ItemId,
                ReceiveQuantity = _WarehouseNotification.ReceiveQuantity,
                SendQuantity = _WarehouseNotification.SendQuantity,
                Message = _WarehouseNotification.Message,
                IsRead = _WarehouseNotification.IsRead,
                CreatedDate = _WarehouseNotification.CreatedDate,
                ModifiedDate = _WarehouseNotification.ModifiedDate,
                CreatedBy = _WarehouseNotification.CreatedBy,
                ModifiedBy = _WarehouseNotification.ModifiedBy,
                Cancelled = _WarehouseNotification.Cancelled,
            };
        }

        public static implicit operator WarehouseNotification(WarehouseNotificationCRUDViewModel vm)
        {
            return new WarehouseNotification
            {
                Id = vm.Id,
                FromWarehouseId = vm.FromWarehouseId,
                ToWarehouseId = vm.ToWarehouseId,
                ItemId = vm.ItemId,
                ReceiveQuantity = vm.ReceiveQuantity,
                SendQuantity = vm.SendQuantity,
                Message = vm.Message,
                IsRead = vm.IsRead,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
