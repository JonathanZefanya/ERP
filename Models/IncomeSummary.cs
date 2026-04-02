namespace BusinessERP.Models
{
    public class IncomeSummary : EntityBase
    {
        public Int64 Id { get; set; }
        public string Title { get; set; }
        public Int64 TypeId { get; set; }
        public Int64 CategoryId { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime IncomeDate { get; set; }
    }
}
