using System;

namespace BusinessERP.Models
{
    public class ItemTransferLog : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 ItemId { get; set; }
        public int CurrentTotalStock { get; set; }
        public int TotalTransferItem { get; set; }
        public Int64? FromWarehouseId { get; set; }
        public Int64 ToWarehouseId { get; set; }
        public string ReasonOfTransfer { get; set; }
    }
}