namespace BusinessERP.Pages
{
    public class MainMenuViewModel
    {
        public bool Admin { get; set; }
        public bool SuperAdmin { get; set; }
        public bool Settings { get; set; }


        public bool Dashboard { get; set; }
        public bool UserManagement { get; set; }
        public bool UserProfile { get; set; }
        public bool ManageUserRoles { get; set; }
        public bool SystemRole { get; set; }

        public bool UserInfoFromBrowser { get; set; }
        public bool AuditLogs { get; set; }
        public bool ManagePageAccess { get; set; }
        public bool IdentitySetting { get; set; }
        public bool LoginHistory { get; set; }
        public bool SendEmailHistory { get; set; }
        public bool CompanyInfo { get; set; }


        //Sales Pro
        public bool Items { get; set; }
        public bool ItemCart { get; set; }
        public bool ItemCartSideInvoice { get; set; }
        public bool ItemRequest { get; set; }
        public bool ItemTransferLog { get; set; }
        public bool Categories { get; set; }
        public bool UnitsOfMeasure { get; set; }
        public bool OutOfStock { get; set; }
        public bool LowInStock { get; set; }
        public bool DamageItemDetails { get; set; }
        public bool DamageItemReport { get; set; }
        public bool ItemsHistory { get; set; }


        public bool Branch { get; set; }


        public bool Currency { get; set; }
        public bool CustomerInfo { get; set; }
        public bool CustomerType { get; set; }
        public bool Supplier { get; set; }
        public bool Warehouse { get; set; }
        public bool WarehouseWiseItems { get; set; }
        public bool WarehouseNotification { get; set; }

        public bool BusinessERP { get; set; }
        public bool ManageInvoice { get; set; }
        public bool Invoice { get; set; }
        public bool ManualInvoice { get; set; }
        public bool DraftInvoice { get; set; }
        public bool QuoteInvoice { get; set; }
        public bool PurchasesPayment { get; set; }
        public bool PurchasesPaymentDraft { get; set; }
        public bool PurchasesPaymentQuote { get; set; }

        //Expense
        public bool Expense { get; set; }
        public bool ExpenseType { get; set; }
        public bool ExpenseSummary { get; set; }


        public bool SalesReturnLog { get; set; }
        public bool PurchaseReturnLog { get; set; }


        //Sales Report
        public bool SalesReport { get; set; }
        public bool ProductWiseSale { get; set; }
        public bool PaymentSummaryReport { get; set; }
        public bool PaymentDetailReport { get; set; }
        public bool TransactionByDay { get; set; }
        public bool TransactionByMonth { get; set; }
        public bool TransactionByYear { get; set; }


        //Purchases Report
        public bool PurchasesReport { get; set; }
        public bool PurchasesSummary { get; set; }
        public bool PurchasesDetail { get; set; }
        public bool PurchasesTransactionByDay { get; set; }
        public bool PurchasesTransactionByMonth { get; set; }
        public bool PurchasesTransactionByYear { get; set; }


        //Item Report
        public bool ItemReport { get; set; }
        public bool HighInDemand { get; set; }
        public bool LowInDemand { get; set; }
        public bool HighestEarning { get; set; }
        public bool LowestEarning { get; set; }

        //Other Report
        public bool OtherReport { get; set; }
        public bool SummaryReport { get; set; }
        public bool PrintBarcode { get; set; }
        public bool AttendanceReport { get; set; }

        //Expense Report 
        public bool ExpenseReport { get; set; }
        public bool ExpenseSummaryReport { get; set; }
        public bool ExpenseDetailsReport { get; set; }
        public bool ExpenseReportByDay { get; set; }
        public bool ExpenseReportByMonth { get; set; }
        public bool ExpenseReportByYear { get; set; }


        public bool PaymentType { get; set; }
        public bool PaymentStatus { get; set; }
        public bool EmailConfig { get; set; }
        public bool VatPercentage { get; set; }

        //HRMS
        public bool Employee { get; set; }
        public bool Department { get; set; }
        public bool SubDepartment { get; set; }
        public bool Designation { get; set; }
        public bool Attendance { get; set; }

        //Income Module
        public bool IncomeSummary { get; set; }
        public bool IncomeType { get; set; }
        public bool IncomeCategory { get; set; }

        //Account Module
        public bool AccAccount { get; set; }
        public bool AccDeposit { get; set; }
        public bool AccExpense { get; set; }
        public bool AccTransfer { get; set; }
        public bool AccTransaction { get; set; }
    }
}