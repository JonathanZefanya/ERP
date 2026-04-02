using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessERP.Models.DamageItemDeatilsViewModel
{
    public class DamageItemAdjustmentViewModel
    {
        public Int64 DamageItemId { get; set; }
        
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        
        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }
        
        [Display(Name = "Total Damaged Quantity")]
        public int TotalDamageItem { get; set; }
        
        [Display(Name = "Current Stock")]
        public int CurrentStock { get; set; }
        
        [Display(Name = "Total Adjusted So Far")]
        public int TotalAdjustedSoFar { get; set; }  // ADD THIS
        
        [Display(Name = "Remaining to Adjust")]
        public int RemainingToAdjust { get; set; }  // ADD THIS
        
        [Required(ErrorMessage = "Adjustment quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Adjustment quantity must be greater than 0")]
        [Display(Name = "Adjustment Quantity (Received from Supplier)")]
        public int AdjustmentQuantity { get; set; }
        
        [Required(ErrorMessage = "Adjustment reason is required")]
        [StringLength(500, ErrorMessage = "Reason cannot exceed 500 characters")]
        [Display(Name = "Adjustment Reason")]
        public string AdjustmentReason { get; set; }
        
        [Display(Name = "Adjustment Date")]
        public DateTime AdjustmentDate { get; set; }
        
        public Int64 ItemId { get; set; }
    }
}
