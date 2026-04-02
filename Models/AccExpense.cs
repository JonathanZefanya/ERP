using System;

namespace BusinessERP.Models
{
    public class AccExpense : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 AccountId { get; set; }
        public string Name { get; set; }
        public DateTime ExpenseDate { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
    }
}