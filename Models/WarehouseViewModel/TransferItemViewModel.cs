using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.WarehouseViewModel
{
    public class TransferItemViewModel : EntityBase
    {
        public Int64 ItemRequestId { get; set; }
        [Display(Name = "Item Name"), Required]
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
        public string CurrentURL { get; set; }
        public string UserName { get; set; }
    }
}