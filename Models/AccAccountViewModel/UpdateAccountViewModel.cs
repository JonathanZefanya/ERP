
namespace BusinessERP.Models.AccAccountViewModel
{
    public class UpdateAccountViewModel
    {
        public double Amount { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public string UserName { get; set; }
        public int AccUpdateType { get; set; }
        public Int64 AccAccountNo { get; set; }
    }
}
