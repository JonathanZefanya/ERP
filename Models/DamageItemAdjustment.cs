using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessERP.Models
{
    [Table("DamageItemAdjustment")]
    public class DamageItemAdjustment
    {
        [Key]
        public Int64 Id { get; set; }
        
        [Required]
        public long DamageItemId { get; set; }
        
        [Required]
        public Int64 ItemId { get; set; }
        
        [Required]
        public int AdjustmentQuantity { get; set; }
        
        [Required]
        [StringLength(500)]
        public string AdjustmentReason { get; set; }
        
        [Required]
        public DateTime AdjustmentDate { get; set; }
        
        [Required]
        [StringLength(256)]
        public string CreatedBy { get; set; }
        
        [Required]
        public DateTime CreatedDate { get; set; }
        
        public bool Cancelled { get; set; } = false;
        
        // Navigation Properties
        [ForeignKey("DamageItemId")]
        public virtual DamageItemDeatils DamageItem { get; set; }
    }
}
