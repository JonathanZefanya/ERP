using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.ItemRequestViewModel
{
    public class ItemRequestCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Item"), Required]
        public Int64 ItemId { get; set; }
        public string ItemDisplay { get; set; }
        [Display(Name = "Request Quantity"), Required]
        public int RequestQuantity { get; set; }
        [Display(Name = "From Warehous"), Required]
        public Int64 FromWarehouseId { get; set; }
        public string FromWarehouseDisplay { get; set; }
        public RequestStatus Status { get; set; }
        public string StatusDisplay { get; set; }
        public string Note { get; set; }

        public static implicit operator ItemRequestCRUDViewModel(ItemRequest _ItemRequest)
        {
            return new ItemRequestCRUDViewModel
            {
                Id = _ItemRequest.Id,
                ItemId = _ItemRequest.ItemId,
                RequestQuantity = _ItemRequest.RequestQuantity,
                FromWarehouseId = _ItemRequest.FromWarehouseId,
                Status = _ItemRequest.Status,
                Note = _ItemRequest.Note,
                CreatedDate = _ItemRequest.CreatedDate,
                ModifiedDate = _ItemRequest.ModifiedDate,
                CreatedBy = _ItemRequest.CreatedBy,
                ModifiedBy = _ItemRequest.ModifiedBy,
                Cancelled = _ItemRequest.Cancelled,
            };
        }

        public static implicit operator ItemRequest(ItemRequestCRUDViewModel vm)
        {
            return new ItemRequest
            {
                Id = vm.Id,
                ItemId = vm.ItemId,
                RequestQuantity = vm.RequestQuantity,
                FromWarehouseId = vm.FromWarehouseId,
                Status = vm.Status,
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
