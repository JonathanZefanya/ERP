/**
 * STATIC MENU CONFIGURATION - Data Only
 * All menu structure definitions in one place
 */
class MenuDataConfig {
    /**
     * ORDERED MENU ITEMS - Define your menu in the exact order you want
     * Mix individual items and sections in any sequence
     */
    static ORDERED_MENU_ITEMS = [
        // Individual Items - Core Business Modules
        { type: 'individual', pageRoleName: 'BusinessERP', title: 'Business ERP', url: '/Dashboard/Index', icon: 'fas fa-th', active: true, badge: 'New', badgeClass: 'badge-danger' },
        { type: 'individual', pageRoleName: 'CustomerInfo', title: 'Customer Info', url: '/CustomerInfo/Index', icon: 'fas fa-users', active: true, customStyle: 'background-color: green;', customImage: '/images/side_menu/customer3.png' },

        // POS System Section
        { type: 'section', sectionKey: 'pos', title: 'POS', icon: 'fas fa-cash-register', active: true, customStyle: 'background-color: green;', customImage: '/images/side_menu/pos.png' },

        // Manage Sales (Invoice Management) Section
        { type: 'section', sectionKey: 'manageSales', title: 'Manage Sales', icon: 'fas fa-file-invoice-dollar', active: true, customStyle: 'background-color: green;', customImage: '/images/side_menu/invoice.png' },

        // Manage Purchases Section
        { type: 'section', sectionKey: 'managePurchases', title: 'Manage Purchases', icon: 'fas fa-shopping-cart', active: true, customImage: '/images/side_menu/buy.png' },

        // Manage Expense Section
        { type: 'section', sectionKey: 'manageExpense', title: 'Manage Expense', icon: 'fas fa-desktop', active: true },

        // Manage Income Section
        { type: 'section', sectionKey: 'manageIncome', title: 'Manage Income', icon: 'fas fa-desktop', active: true, customStyle: 'background-color: green;', customImage: '/images/side_menu/income.png' },

        // Manage Branch Section
        { type: 'section', sectionKey: 'manageBranch', title: 'Manage Branch', icon: 'fas fa-solid fa-code-branch', active: true },

        // Inventory Management Section
        { type: 'section', sectionKey: 'inventory', title: 'Inventory Management', icon: 'fas fa-boxes', active: true, customImage: '/images/side_menu/inventory1.png' },

        // Accounting Module Section
        { type: 'section', sectionKey: 'accounting', title: 'Accounting Module', icon: 'fas fa-calculator', active: true, customStyle: 'background-color: grey', customImage: '/images/side_menu/acc/budget.png' },

        // Manage Warehouse Section
        { type: 'section', sectionKey: 'warehouse', title: 'Manage Warehouse', icon: 'fas fa-warehouse', active: true, customStyle: 'background-color: grey', customImage: '/images/side_menu/warehouse3.png' },

        // System Settings Section
        { type: 'section', sectionKey: 'systemSettings', title: 'System Settings', icon: 'fas fa-cogs', active: true, customImage: '/images/side_menu/settings.png' },

        // User Management Section
        { type: 'section', sectionKey: 'userManagement', title: 'Manage User', icon: 'fas fa-users-cog', active: true, customImage: '/images/side_menu/usersettings.png' },

        // Manage User Roles Section
        { type: 'section', sectionKey: 'manageUserRoles', title: 'Manage User Roles', icon: 'fas fa-user-tag', active: true, customImage: '/images/side_menu/access3.png' },

        // Human Resource Section
        { type: 'section', sectionKey: 'humanResource', title: 'Human Resource', icon: 'fas fa-desktop', active: true },

        // Item Report Section
        { type: 'section', sectionKey: 'itemReport', title: 'Item Report', icon: 'fas fa-signal', active: true, customStyle: 'background-color: rgb(73, 69, 73);' },

        // Sales Report Section
        { type: 'section', sectionKey: 'salesReport', title: 'Sales Report', icon: 'fas fa-database', active: true, customStyle: 'background-color: rgb(73, 69, 73);' },

        // Purchases Report Section
        { type: 'section', sectionKey: 'purchasesReport', title: 'Purchases Report', icon: 'fas fa-database', active: true, customStyle: 'background-color: rgb(73, 69, 73);' },

        // Expense Report Section
        { type: 'section', sectionKey: 'expenseReport', title: 'Expense Report', icon: 'fas fa-clock', active: true, customStyle: 'background-color: rgb(73, 69, 73);' },

        // Other Report Section
        { type: 'section', sectionKey: 'otherReport', title: 'Other Report', icon: 'fas fa-barcode', active: true, customStyle: 'background-color: rgb(73, 69, 73);' }
    ];

    static SECTION_ITEMS = [
        // POS System Section Items
        { sectionKey: 'pos', pageRoleName: 'ItemCartSideInvoice', title: 'Side Invoice', url: '/ItemCartSideInvoice/Index', icon: 'fas fa-list', active: true },
        { sectionKey: 'pos', pageRoleName: 'ItemCart', title: 'Item Cart', url: '/ItemCart/Index', icon: 'fas fa-heart', active: true },

        // Manage Sales Section Items
        { sectionKey: 'manageSales', pageRoleName: 'Invoice', title: 'Invoice', url: '/Invoice/Index', icon: 'fas fa-money-bill', active: true },
        { sectionKey: 'manageSales', pageRoleName: 'ManualInvoice', title: 'Manual Invoice', url: '/ManualInvoice/Index', icon: 'fas fa-desktop', active: true },
        { sectionKey: 'manageSales', pageRoleName: 'DraftInvoice', title: 'Draft Invoice', url: '/DraftInvoice/Index', icon: 'fas fa-user', active: true },
        { sectionKey: 'manageSales', pageRoleName: 'QuoteInvoice', title: 'Quote Invoice', url: '/QuoteInvoice/Index', icon: 'fas fa-user', active: true },
        { sectionKey: 'manageSales', pageRoleName: 'SalesReturnLog', title: 'Sales Return Log', url: '/SalesReturnLog/Index', icon: 'fas fa-solid fa-history', active: true },

        // Manage Purchases Section Items
        { sectionKey: 'managePurchases', pageRoleName: 'PurchasesPayment', title: 'Purchases Invoice', url: '/PurchasesPayment/Index', icon: 'fas fa-user-cog', active: true },
        { sectionKey: 'managePurchases', pageRoleName: 'PurchasesPaymentDraft', title: 'Purchases Payment Draft', url: '/PurchasesPaymentDraft/Index', icon: 'fas fa-history', active: true },
        { sectionKey: 'managePurchases', pageRoleName: 'PurchasesPaymentQuote', title: 'Purchases Payment Quote', url: '/PurchasesPaymentQuote/Index', icon: 'fas fa-star', active: true },
        { sectionKey: 'managePurchases', pageRoleName: 'Supplier', title: 'Supplier', url: '/Supplier/Index', icon: 'fas fa-user-cog', active: true },
        { sectionKey: 'managePurchases', pageRoleName: 'PurchaseReturnLog', title: 'Purchase Return Log', url: '/PurchaseReturnLog/Index', icon: 'fas fa-solid fa-history', active: true },

        // Manage Expense Section Items
        { sectionKey: 'manageExpense', pageRoleName: 'ExpenseSummary', title: 'Expense Summary', url: '/ExpenseSummary/Index', icon: 'fas fa-coffee', active: true },
        { sectionKey: 'manageExpense', pageRoleName: 'ExpenseType', title: 'Expense Type', url: '/ExpenseType/Index', icon: 'fas fa-users', active: true },

        // Manage Income Section Items
        { sectionKey: 'manageIncome', pageRoleName: 'IncomeSummary', title: 'Income Summary', url: '/IncomeSummary/Index', icon: 'fas fa-desktop', active: true },
        { sectionKey: 'manageIncome', pageRoleName: 'IncomeType', title: 'Income Type', url: '/IncomeType/Index', icon: 'fas fa-bars', active: true },
        { sectionKey: 'manageIncome', pageRoleName: 'IncomeCategory', title: 'Income Category', url: '/IncomeCategory/Index', icon: 'fas fa-coffee', active: true },
        { sectionKey: 'manageIncome', pageRoleName: 'IncomeSummaryReport', title: 'Income Summary Report', url: '/IncomeSummaryReport/Index', icon: 'fas fa-database', active: true },

        // Manage Branch Section Items
        { sectionKey: 'manageBranch', pageRoleName: 'Branch', title: 'Branch', url: '/Branch/Index', icon: 'fas fa-solid fa-list', active: true },

        // Inventory Management Section Items
        { sectionKey: 'inventory', pageRoleName: 'Items', title: 'Items', url: '/Items/Index', icon: 'fas fa-shopping-basket', active: true },
        { sectionKey: 'inventory', pageRoleName: 'OutOfStock', title: 'Out Of Stock', url: '/OutOfStock/Index', icon: 'fas fa-bell', active: true },
        { sectionKey: 'inventory', pageRoleName: 'LowInStock', title: 'Low In Stock', url: '/LowInStock/Index', icon: 'fas fa-clock', active: true },
        { sectionKey: 'inventory', pageRoleName: 'DamageItemDetails', title: 'Damage Item Details', url: '/DamageItemDetails/Index', icon: 'fas fa-flag', active: true },
        { sectionKey: 'inventory', pageRoleName: 'ItemsHistory', title: 'Items History', url: '/ItemsHistory/Index', icon: 'fas fa-history', active: true },

        // Accounting Module Section Items
        { sectionKey: 'accounting', pageRoleName: 'AccAccount', title: 'Account', url: '/AccAccount/Index', icon: 'fas fa-database', active: true },
        { sectionKey: 'accounting', pageRoleName: 'AccDeposit', title: 'Deposit', url: '/AccDeposit/Index', icon: 'fas fa-envelope', active: true },
        { sectionKey: 'accounting', pageRoleName: 'AccExpense', title: 'Expense', url: '/AccExpense/Index', icon: 'fas fa-desktop', active: true },
        { sectionKey: 'accounting', pageRoleName: 'AccTransfer', title: 'Transfer', url: '/AccTransfer/Index', icon: 'fas fa-flag', active: true },
        { sectionKey: 'accounting', pageRoleName: 'AccTransaction', title: 'Transaction', url: '/AccTransaction/Index', icon: 'fas fa-history', active: true },

        // Manage Warehouse Section Items
        { sectionKey: 'warehouse', pageRoleName: 'Warehouse', title: 'Warehouse', url: '/Warehouse/Index', icon: 'fas fa-database', active: true },
        { sectionKey: 'warehouse', pageRoleName: 'WarehouseWiseItems', title: 'Warehouse Wise Items', url: '/WarehouseWiseItems/Index', icon: 'fas fa-toolbox', active: true },
        { sectionKey: 'warehouse', pageRoleName: 'ItemRequest', title: 'Item Request', url: '/ItemRequest/Index', icon: 'fas fa-flag', active: true },
        { sectionKey: 'warehouse', pageRoleName: 'ItemTransferLog', title: 'Item Transfer Log', url: '/ItemTransferLog/Index', icon: 'fas fa-clock', active: true },
        { sectionKey: 'warehouse', pageRoleName: 'WarehouseNotification', title: 'Warehouse Notification', url: '/WarehouseNotification/Index', icon: 'fas fa-bell', active: true },

        // System Settings Section Items
        { sectionKey: 'systemSettings', pageRoleName: 'CompanyInfo', title: 'Company Info', url: '/CompanyInfo/Index', icon: 'fas fa-users', active: true },
        { sectionKey: 'systemSettings', pageRoleName: 'SystemSettings', title: 'System Settings', url: '/SystemSettings/Index', icon: 'fas fa-key', active: true },
        { sectionKey: 'systemSettings', pageRoleName: 'EmailConfig', title: 'Email Config', url: '/EmailConfig/Index', icon: 'fas fa-envelope', active: true },
        { sectionKey: 'systemSettings', pageRoleName: 'Currency', title: 'Currency', url: '/Currency/Index', icon: 'fas fa-heart', active: true },
        { sectionKey: 'systemSettings', pageRoleName: 'PaymentType', title: 'Payment Type', url: '/PaymentType/Index', icon: 'fas fa-heartbeat', active: true },
        { sectionKey: 'systemSettings', pageRoleName: 'PaymentStatus', title: 'Payment Status', url: '/PaymentStatus/Index', icon: 'fas fa-user', active: true },
        { sectionKey: 'systemSettings', pageRoleName: 'CustomerType', title: 'Customer Type', url: '/CustomerType/Index', icon: 'fas fa-user', active: true },
        { sectionKey: 'systemSettings', pageRoleName: 'VatPercentage', title: 'Vat Percentage', url: '/VatPercentage/Index', icon: 'fas fa-user', active: true },
        { sectionKey: 'systemSettings', pageRoleName: 'Categories', title: 'Categories', url: '/Categories/Index', icon: 'fas fa-tasks', active: true },
        { sectionKey: 'systemSettings', pageRoleName: 'UnitsofMeasure', title: 'Units of Measure', url: '/UnitsofMeasure/Index', icon: 'fas fa-bolt', active: true },

        // User Management Section Items
        { sectionKey: 'userManagement', pageRoleName: 'UserManagement', title: 'User Management', url: '/UserManagement/Index', icon: 'fas fa-user-shield', active: true },
        { sectionKey: 'userManagement', pageRoleName: 'IdentitySetting', title: 'Identity Setting', url: '/IdentitySetting/Index', icon: 'fas fa-user-secret', active: true },
        { sectionKey: 'userManagement', pageRoleName: 'SendEmailHistory', title: 'Send Email History', url: '/SendEmailHistory/Index', icon: 'fas fa-clock', active: true },
        { sectionKey: 'userManagement', pageRoleName: 'LoginHistory', title: 'Login History', url: '/LoginHistory/Index', icon: 'fas fa-history', active: true },
        { sectionKey: 'userManagement', pageRoleName: 'AuditLogs', title: 'Audit Logs', url: '/AuditLogs/Index', icon: 'fas fa-clock', active: true },

        // Manage User Roles Section Items
        { sectionKey: 'manageUserRoles', pageRoleName: 'ManageUserRoles', title: 'Manage User Roles', url: '/ManageUserRoles/Index', icon: 'fas fa-desktop', active: true },
        { sectionKey: 'manageUserRoles', pageRoleName: 'SystemRole', title: 'System Role', url: '/SystemRole/Index', icon: 'fas fa-toolbox', active: true },

        // Human Resource Section Items
        { sectionKey: 'humanResource', pageRoleName: 'Attendance', title: 'Attendance', url: '/Attendance/Index', icon: 'fas fa-industry', active: true },
        { sectionKey: 'humanResource', pageRoleName: 'Employee', title: 'Employee', url: '/Employee/Index', icon: 'fas fa-industry', active: true },
        { sectionKey: 'humanResource', pageRoleName: 'Designation', title: 'Designation', url: '/Designation/Index', icon: 'fas fa-heart', active: true },
        { sectionKey: 'humanResource', pageRoleName: 'Department', title: 'Department', url: '/Department/Index', icon: 'fas fa-star', active: true },
        { sectionKey: 'humanResource', pageRoleName: 'SubDepartment', title: 'Sub Department', url: '/SubDepartment/Index', icon: 'fas fa-desktop', active: true },

        // Item Report Section Items
        { sectionKey: 'itemReport', pageRoleName: 'StockItemReport', title: 'Stock Item Report', url: '/StockItemReport/Index', icon: 'fas fa-database', active: true },
        { sectionKey: 'itemReport', pageRoleName: 'HighInDemand', title: 'High In Demand', url: '/HighInDemand/Index', icon: 'fas fa-database', active: true },
        { sectionKey: 'itemReport', pageRoleName: 'LowInDemand', title: 'Low In Demand', url: '/LowInDemand/Index', icon: 'fas fa-database', active: true },
        { sectionKey: 'itemReport', pageRoleName: 'HighestEarning', title: 'Highest Earning', url: '/HighestEarning/Index', icon: 'fas fa-heartbeat', active: true },
        { sectionKey: 'itemReport', pageRoleName: 'LowestEarning', title: 'Lowest Earning', url: '/LowestEarning/Index', icon: 'fas fa-heartbeat', active: true },

        // Sales Report Section Items
        { sectionKey: 'salesReport', pageRoleName: 'CustomerSalesReport', title: 'Customer Sales Report', url: '/CustomerSalesReport/Index', icon: 'fas fa-envelope', active: true },
        { sectionKey: 'salesReport', pageRoleName: 'PaymentSummaryReport', title: 'Payment Summary Report', url: '/PaymentSummaryReport/Index', icon: 'fas fa-database', active: true },
        { sectionKey: 'salesReport', pageRoleName: 'ProductWiseSale', title: 'Product Wise Sale', url: '/ProductWiseSale/Index', icon: 'fas fa-database', active: true },
        { sectionKey: 'salesReport', pageRoleName: 'PaymentDetailReport', title: 'Payment Detail Report', url: '/PaymentDetailReport/Index', icon: 'fas fa-database', active: true },
        { sectionKey: 'salesReport', pageRoleName: 'TransactionByDay', title: 'Transaction By Day', url: '/TransactionByDay/Index', icon: 'fas fa-heart', active: true },
        { sectionKey: 'salesReport', pageRoleName: 'TransactionByMonth', title: 'Transaction By Month', url: '/TransactionByMonth/Index', icon: 'fas fa-heart', active: true },
        { sectionKey: 'salesReport', pageRoleName: 'TransactionByYear', title: 'Transaction By Year', url: '/TransactionByYear/Index', icon: 'fas fa-heart', active: true },

        // Purchases Report Section Items
        { sectionKey: 'purchasesReport', pageRoleName: 'PurchasesSummary', title: 'Purchases Summary', url: '/PurchasesSummary/Index', icon: 'fas fa-database', active: true },
        { sectionKey: 'purchasesReport', pageRoleName: 'PurchasesDetail', title: 'Purchases Detail', url: '/PurchasesDetail/Index', icon: 'fas fa-database', active: true },
        { sectionKey: 'purchasesReport', pageRoleName: 'PurchasesTransactionByDay', title: 'Purchases Transaction By Day', url: '/PurchasesTransactionByDay/Index', icon: 'fas fa-heart', active: true },
        { sectionKey: 'purchasesReport', pageRoleName: 'PurchasesTransactionByMonth', title: 'Purchases Transaction By Month', url: '/PurchasesTransactionByMonth/Index', icon: 'fas fa-heart', active: true },
        { sectionKey: 'purchasesReport', pageRoleName: 'PurchasesTransactionByYear', title: 'Purchases Transaction By Year', url: '/PurchasesTransactionByYear/Index', icon: 'fas fa-heart', active: true },

        // Expense Report Section Items
        { sectionKey: 'expenseReport', pageRoleName: 'ExpenseSummaryReport', title: 'Expense Summary Report', url: '/ExpenseSummaryReport/Index', icon: 'fas fa-database', active: true },
        { sectionKey: 'expenseReport', pageRoleName: 'ExpenseDetailsReport', title: 'Expense Details Report', url: '/ExpenseDetailsReport/Index', icon: 'fas fa-star', active: true },
        { sectionKey: 'expenseReport', pageRoleName: 'ExpenseByDay', title: 'Expense By Day', url: '/ExpenseByDay/Index', icon: 'fas fa-heart', active: true },
        { sectionKey: 'expenseReport', pageRoleName: 'ExpenseByMonth', title: 'Expense By Month', url: '/ExpenseByMonth/Index', icon: 'fas fa-heart', active: true },
        { sectionKey: 'expenseReport', pageRoleName: 'ExpenseByYear', title: 'Expense By Year', url: '/ExpenseByYear/Index', icon: 'fas fa-heart', active: true },

        // Other Report Section Items
        { sectionKey: 'otherReport', pageRoleName: 'SummaryReport', title: 'Summary Report', url: '/SummaryReport/Index', icon: 'fas fa-history', active: true },
        { sectionKey: 'otherReport', pageRoleName: 'AttendanceReport', title: 'Attendance Report', url: '/AttendanceReport/Index', icon: 'fas fa-clock', active: true },
        { sectionKey: 'otherReport', pageRoleName: 'PrintBarcode', title: 'Print Barcode', url: '/PrintBarcode/Index', icon: 'fas fa-star', active: true }
    ];

    // Core configuration constants
    static get CONFIG() {
        return {
            ELEMENTS: {
                userPanel: 'userPanel',
                userProfileImage: 'userProfileImage',
                userFullName: 'userFullName',
                sidebarMenu: 'sidebarMenu'
            }
        };
    }

    // Display settings
    static SETTINGS = {
        showUserPanel: true,
        defaultProfileImage: '/images/default-avatar.png',
        profileLinkUrl: '/UserProfile/Index',
        enableActiveLinks: true,
        enableTreeviewExpansion: true,
        fallbackUserName: 'User'
    };

    static getStorageData() {
        try {
            const data = EncryptedLocalStorage.getItem("LoginUserDataViewModel");
            return data ? data : null;
        } catch (error) {
            console.error('Error loading user data:', error);
            return null;
        }
    }
}
