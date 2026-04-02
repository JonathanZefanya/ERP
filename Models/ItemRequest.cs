namespace BusinessERP.Models
{
    public class ItemRequest : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 ItemId { get; set; }
        public int RequestQuantity { get; set; }       
        public Int64 FromWarehouseId { get; set; }
        public RequestStatus Status { get; set; }
        public string Note { get; set; }
    }

    public enum RequestStatus
    {
        New = 1,
        Pending = 2,
        Send = 3,
        Rejected = 4,
        ItemNotAvailable = 5
    }
}