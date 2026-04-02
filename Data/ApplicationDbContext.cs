using BusinessERP.Models;
using BusinessERP.Models.CommonViewModel;
using BusinessERP.Models.ItemsViewModel;
using Microsoft.EntityFrameworkCore;

namespace BusinessERP.Data
{
    public class ApplicationDbContext : AuditableIdentityContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ItemGridViewModel>().HasNoKey();
            builder.Entity<ItemDropdownListViewModel>().HasNoKey();
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<ManageUserRoles> ManageUserRoles { get; set; }
        public DbSet<ManageUserRolesDetails> ManageUserRolesDetails { get; set; }
        public DbSet<DefaultIdentityOptions> DefaultIdentityOptions { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }


        //Business ERP
        public DbSet<Items> Items { get; set; }
        public DbSet<ItemsHistory> ItemsHistory { get; set; }
        public DbSet<ItemRequest> ItemRequest { get; set; }
        public DbSet<ItemTransferLog> ItemTransferLog { get; set; }
        public DbSet<DamageItemDeatils> DamageItemDeatils { get; set; }
        public DbSet<DamageItemAdjustment> DamageItemAdjustments { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<WarehouseNotification> WarehouseNotification { get; set; }
        public DbSet<PaymentType> PaymentType { get; set; }
        public DbSet<EmailConfig> EmailConfig { get; set; }

        public DbSet<CustomerInfo> CustomerInfo { get; set; }
        public DbSet<CustomerType> CustomerType { get; set; }
        public DbSet<UnitsofMeasure> UnitsofMeasure { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentDetail> PaymentDetail { get; set; }
        public DbSet<PaymentModeHistory> PaymentModeHistory { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        public DbSet<VatPercentage> VatPercentage { get; set; }
        public DbSet<ItemSerialNumber> ItemSerialNumber { get; set; }
        public DbSet<PurchasesPayment> PurchasesPayment { get; set; }
        public DbSet<PurchasesPaymentDetail> PurchasesPaymentDetail { get; set; }

        //HRMS
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<Department> Department { get; set; } 
        public DbSet<SubDepartment> SubDepartment { get; set; }
        public DbSet<Employee> Employee { get; set; }

        //Expense
        public DbSet<ExpenseType> ExpenseType { get; set; }
        public DbSet<ExpenseSummary> ExpenseSummary { get; set; }
        public DbSet<ExpenseDetails> ExpenseDetails { get; set; }
        public DbSet<UserInfoFromBrowser> UserInfoFromBrowser { get; set; }
        public DbSet<SendEmailHistory> SendEmailHistory { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<ReturnLog> ReturnLog { get; set; }

        public DbSet<IncomeSummary> IncomeSummary { get; set; }
        public DbSet<IncomeType> IncomeType { get; set; }
        public DbSet<IncomeCategory> IncomeCategory { get; set; }

        //Account Module
        public DbSet<AccAccount> AccAccount { get; set; }
        public DbSet<AccDeposit> AccDeposit { get; set; }
        public DbSet<AccExpense> AccExpense { get; set; }
        public DbSet<AccTransfer> AccTransfer { get; set; }
        public DbSet<AccTransaction> AccTransaction { get; set; }


        public DbSet<ItemGridViewModel> ItemGridViewModel { get; set; }
        public DbSet<ItemDropdownListViewModel> ItemDropdownListViewModel { get; set; }
    }
}
