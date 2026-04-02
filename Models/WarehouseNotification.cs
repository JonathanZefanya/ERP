
namespace BusinessERP.Models
{
    public class WarehouseNotification : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64? FromWarehouseId { get; set; }
        public Int64 ToWarehouseId { get; set; }
        public Int64 ItemId { get; set; }
        public int ReceiveQuantity { get; set; }
        public int SendQuantity { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
