using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.DamageItemDeatilsViewModel
{
    public class DamageItemReportViewModel
    {
        [Display(Name = "From Date")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "To Date")]
        public DateTime? ToDate { get; set; }

        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }

        [Display(Name = "Product")]
        public int? ItemId { get; set; }

        [Display(Name = "Report Type")]
        public string ReportType { get; set; }

        public List<DamageItemReportDetailViewModel> ReportData { get; set; }
        public DamageItemReportSummaryViewModel Summary { get; set; }
    }

    public class DamageItemReportDetailViewModel
    {
        public long DamageItemId { get; set; }
        public DateTime DamageDate { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string SupplierName { get; set; }
        public int TotalDamaged { get; set; }
        public int TotalAdjusted { get; set; }
        public int Remaining { get; set; }
        public decimal AdjustmentPercentage { get; set; }
        public string Status { get; set; }
        public string ReasonOfDamage { get; set; }
        public string CreatedBy { get; set; }
        
        // Adjustment details
        public List<AdjustmentDetailViewModel> Adjustments { get; set; }
    }

    public class AdjustmentDetailViewModel
    {
        public long AdjustmentId { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public string CreatedBy { get; set; }
    }

    public class DamageItemReportSummaryViewModel
    {
        public int TotalDamageRecords { get; set; }
        public int TotalItemsDamaged { get; set; }
        public int TotalItemsAdjusted { get; set; }
        public int TotalItemsRemaining { get; set; }
        public decimal TotalAdjustmentPercentage { get; set; }
        public int FullyAdjustedCount { get; set; }
        public int PartiallyAdjustedCount { get; set; }
        public int NotAdjustedCount { get; set; }
        
        // By Supplier
        public List<SupplierSummaryViewModel> SupplierSummaries { get; set; }
        
        // By Product
        public List<ProductSummaryViewModel> ProductSummaries { get; set; }
    }

    public class SupplierSummaryViewModel
    {
        public string SupplierName { get; set; }
        public int TotalDamaged { get; set; }
        public int TotalAdjusted { get; set; }
        public int Remaining { get; set; }
    }

    public class ProductSummaryViewModel
    {
        public string ItemName { get; set; }
        public int TotalDamaged { get; set; }
        public int TotalAdjusted { get; set; }
        public int Remaining { get; set; }
    }
}
