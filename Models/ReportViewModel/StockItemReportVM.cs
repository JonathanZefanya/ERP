
namespace BusinessERP.Models.ReturnLogViewModel
{
    public class StockItemReportVM
    {
        public Int64 Id { get; set; }
        public Int64? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int Size { get; set; }
        public string SizeDisplay { get; set; }
        public double Quantity { get; set; }
        public double SubQuantity { get; set; }
    }
}
