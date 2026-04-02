using System;

namespace BusinessERP.Models.PaymentViewModel
{
    public class CustomerReportViewModel
    {
        public Int64 Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string InvoiceNo { get; set; }
        public string ReferenceNumber { get; set; }
        public Int64 CustomerId { get; set; }
        public Int64 ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int SubQuantity { get; set; }
        public double? ItemPrice { get; set; }
        public double? Discount { get; set; }
        public double? SubTotal { get; set; }
        public double? GrandTotal { get; set; }
        public double? PaidAmount { get; set; }
        public double? DueAmount { get; set; }
    }

    public class CustomerReportDataViewModel
    {
        public List<CustomerReportViewModel> listCustomerReportViewModel { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
    }
}


