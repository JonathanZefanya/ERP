using System.Linq.Expressions;
using BusinessERP.Models;
using BusinessERP.Models.AccAccountViewModel;
using BusinessERP.Models.AccDepositViewModel;
using BusinessERP.Models.AccExpenseViewModel;
using BusinessERP.Models.AccTransactionViewModel;
using BusinessERP.Models.AccTransferViewModel;
using BusinessERP.Models.AttendanceViewModel;
using BusinessERP.Models.CommonViewModel;
using BusinessERP.Models.CompanyInfoViewModel;
using BusinessERP.Models.CustomerInfoViewModel;
using BusinessERP.Models.DashboardViewModel;
using BusinessERP.Models.EmployeeViewModel;
using BusinessERP.Models.ExpenseSummaryViewModel;
using BusinessERP.Models.ItemsHistoryViewModel;
using BusinessERP.Models.ItemsViewModel;
using BusinessERP.Models.ManageUserRolesVM;
using BusinessERP.Models.UserProfileViewModel;
using UAParser;

namespace BusinessERP.Services
{
    public interface ICommon
    {
        string UploadedFile(IFormFile ProfilePicture);
        string UploadedFileByReplace(IFormFile _IFormFile);
        Task<EmailConfig> GetEmailConfig();
        UserProfile GetByUserProfile(Int64 id);
        Task<UserProfileCRUDViewModel> GetByUserProfileInfo(Int64 id);
        Task<bool> InsertLoginHistory(LoginHistory _LoginHistory, ClientInfo _ClientInfo);
        Task CurrentItemsUpdate(ItemTranViewModel _ItemTranViewModel);
        IQueryable<ItemDropdownListViewModel> GetCommonddlData(string strTableName);
        //IEnumerable<T> GetTableData<T>() where T : class;
        IEnumerable<T> GetTableData<T>(Expression<Func<T, bool>> condition) where T : class;
        IQueryable<ItemDropdownListViewModel> LoadddlInventoryItem(bool IsVat);
        IQueryable<ItemDropdownListViewModel> LoadddlCustomerInfo();
        IQueryable<ItemDropdownListViewModel> LoadddlPaymentType();
        IQueryable<ItemDropdownListViewModel> LoadddlCurrencyItem();
        IQueryable<ItemDropdownListViewModel> LoadddlWarehouse();
        IQueryable<ItemDropdownListViewModel> LoadddlSupplier();

        IQueryable<ItemDropdownListViewModel> LoadddlDepartment();
        IQueryable<ItemDropdownListViewModel> LoadddlSubDepartment();
        IQueryable<ItemDropdownListViewModel> LoadddlEmployee();
        IQueryable<ItemDropdownListViewModel> LoadddlDesignation();
        IQueryable<ItemDropdownListViewModel> LoadddlCategories();
        IQueryable<ItemDropdownListViewModel> LoadddlExpenseType();


        Task<ItemsHistoryCRUDViewModel> AddItemHistory(ItemsHistoryCRUDViewModel vm);
        ItemsCRUDViewModel GetViewItemById(Int64 Id);
        IQueryable<ItemCartViewModel> GetAllCartItem();
        IQueryable<List<ItemCartViewModel>> GetAllCartItemForCustomDT();
        IQueryable<List<ItemCartViewModel>> GetItemCartDataList();
        IQueryable<AttendanceCRUDViewModel> GetAttendanceReportData();
        IQueryable<ItemGridViewModel> GetJoinDataItemsAndTranDetails();
        string GenerateDemoSales();
        List<ExpenseByViewModel> ExpenseByDate(string DateType);
        List<GroupByViewModel> GetItemDemandList();
        List<GroupByViewModel> GetItemEarningList();
        IQueryable<BarcodeViewModel> GetBarcodeList();
        Task<CompanyInfoCRUDViewModel> GetCompanyInfo();
        IQueryable<ItemsCRUDViewModel> GetItemsGridList();
        IQueryable<ItemDropdownListViewModel> GetddlEmailConfig();
        IQueryable<ItemDropdownListViewModel> GetddlCustomerEmail();
        IQueryable<ItemDropdownListViewModel> GetddlCustomerType();
        IQueryable<ItemDropdownListViewModel> GetddlPaymentStatus();
        IQueryable<CustomerInfoCRUDViewModel> GetCustomerList();
        IQueryable<EmployeeCRUDViewModel> GetEmployeeGridList();
        IQueryable<EmployeeCRUDViewModel> GetEmployeeList();
        IQueryable<ExpenseDetailsCRUDViewModel> GetExpenseDetailsList();
        IQueryable<ExpenseSummaryCRUDViewModel> GetExpenseSummaryGridItem();
        Task<Int64> GetBranchIdByUserName(string _UserName);
        Task<List<ManageUserRolesViewModel>> GetManageRoleDetailsList(Int64 id);
        IQueryable<UserProfileCRUDViewModel> GetUserProfileDetails();
        IQueryable<AccDepositCRUDViewModel> GetAllAccDeposit();
        IQueryable<AccExpenseCRUDViewModel> GetAllAccExpense();
        IQueryable<AccTransactionCRUDViewModel> GetAllAccTransaction();
        IQueryable<AccTransferCRUDViewModel> GetAllAccTransfer();
        IQueryable<AccAccount> GetAllAccAccount();
        Task<bool> UpdateAccoutDuringTran(UpdateAccountViewModel vm);
        Task<AccTransaction> AddAccTransaction(AccTransactionCRUDViewModel vm);
    }
}
