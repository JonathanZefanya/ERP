using System;

namespace BusinessERP.Models
{
    public class AccAccount : EntityBase
    {
        public Int64 Id { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
        public double Balance { get; set; }
        public string Description { get; set; }
    }
}