using System;

namespace BusinessERP.Models
{
    public class AccTransaction : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 AccountId { get; set; }
        public string Type { get; set; }
        public string Reference { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }
}