using BusinessERP.ConHelper;
using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models;
using BusinessERP.Models.AccAccountViewModel;
using BusinessERP.Models.CommonViewModel;
using BusinessERP.Models.DashboardViewModel;
using BusinessERP.Models.ItemsHistoryViewModel;
using BusinessERP.Models.ItemsViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace BusinessERP.Services
{
    public class Functional : IFunctional
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IRoles _roles;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;
        private readonly ApplicationInfo _applicationInfo;
        private readonly IAccount _iAccount;
        private readonly ICommon _iCommon;
        private readonly SeedData _SeedData = new SeedData();

        public Functional(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
           ApplicationDbContext context,
           IRoles roles,
           IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions,
           IOptions<ApplicationInfo> applicationInfo,
           IAccount iAccount,
           ICommon iCommon)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _roles = roles;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
            _applicationInfo = applicationInfo.Value;
            _iAccount = iAccount;
            _iCommon = iCommon;
        }

        public async Task SendEmailBySendGridAsync(string apiKey,
            string fromEmail,
            string fromFullName,
            string subject,
            string message,
            string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromFullName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email, email));
            await client.SendEmailAsync(msg);

        }

        public async Task SendEmailByGmailAsync(string fromEmail,
            string fromFullName,
            string subject,
            string messageBody,
            string toEmail,
            string toFullName,
            string smtpUser,
            string smtpPassword,
            string smtpHost,
            int smtpPort,
            bool smtpSSL)
        {
            var body = messageBody;
            var message = new MailMessage();
            message.To.Add(new MailAddress(toEmail, toFullName));
            message.From = new MailAddress(fromEmail, fromFullName);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                smtp.UseDefaultCredentials = false;
                var credential = new NetworkCredential
                {
                    UserName = smtpUser,
                    Password = smtpPassword
                };
                smtp.Credentials = credential;
                smtp.Host = smtpHost;
                smtp.Port = smtpPort;
                smtp.EnableSsl = smtpSSL;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message);

            }

        }
        public async Task InitAppData()
        {
            var _GetCategoriesList = _SeedData.GetCategoriesList();
            foreach (var item in _GetCategoriesList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.Categories.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetUnitsofMeasureList = _SeedData.GetUnitsofMeasureList();
            foreach (var item in _GetUnitsofMeasureList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.UnitsofMeasure.Add(item);
                await _context.SaveChangesAsync();
            }

            var _GetSupplierList = _SeedData.GetSupplierList();
            foreach (var item in _GetSupplierList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.Supplier.Add(item);
                await _context.SaveChangesAsync();
            }

            var _GetWarehouseList = _SeedData.GetWarehouseList();
            foreach (var item in _GetWarehouseList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.Warehouse.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetVatPercentageList = _SeedData.GetVatPercentageList();
            foreach (var item in _GetVatPercentageList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.VatPercentage.Add(item);
                await _context.SaveChangesAsync();
            }

            var _GetCustomerInfoList = _SeedData.GetCustomerInfoList();
            foreach (var item in _GetCustomerInfoList)
            {
                item.Type = 1;
                item.CompanyName = "ABC Limited.";
                item.Phone = "+" + StaticData.RandomDigits(9);
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.CustomerInfo.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetCustomerTypeList = _SeedData.GetCustomerTypeList();
            foreach (var item in _GetCustomerTypeList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.CustomerType.Add(item);
                await _context.SaveChangesAsync();
            }

            var _GetCurrencyList = _SeedData.GetCurrencyList();
            foreach (var item in _GetCurrencyList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.Currency.Add(item);
                await _context.SaveChangesAsync();
            }

            var _GetPaymentTypeList = _SeedData.GetPaymentTypeList();
            foreach (var item in _GetPaymentTypeList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.PaymentType.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetPaymentStatusList = _SeedData.GetPaymentStatusList();
            foreach (var item in _GetPaymentStatusList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.PaymentStatus.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetEmailConfigList = _SeedData.GetEmailConfigList();
            foreach (var item in _GetEmailConfigList)
            {
                item.SenderFullName = "Admin: Business ERP";
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.EmailConfig.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetDepartmentList = _SeedData.GetDepartmentList();
            foreach (var item in _GetDepartmentList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.Department.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetSubDepartmentList = _SeedData.GetSubDepartmentList();
            foreach (var item in _GetSubDepartmentList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.SubDepartment.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetEmployeeList = _SeedData.GetEmployeeList();
            foreach (var item in _GetEmployeeList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.Employee.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetDesignationList = _SeedData.GetDesignationList();
            foreach (var item in _GetDesignationList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.Designation.Add(item);
                await _context.SaveChangesAsync();
            }

            for (int i = 1; i <= 5; i++)
            {
                for (int j = 1; j <= 5; j++)
                {
                    Attendance item = new();
                    item.EmployeeId = i;
                    item.CheckIn = DateTime.Today.AddDays(j).AddHours(9);
                    item.CheckOut = DateTime.Today.AddDays(j).AddHours(18);
                    item.StayTime = item.CheckOut - item.CheckIn;

                    item.CreatedDate = DateTime.Today.AddDays(j);
                    item.ModifiedDate = DateTime.Today.AddDays(j);
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    _context.Attendance.Add(item);
                    await _context.SaveChangesAsync();
                }
            }

            var _GetExpenseTypeList = _SeedData.GetExpenseTypeList();
            foreach (var item in _GetExpenseTypeList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.ExpenseType.Add(item);
                await _context.SaveChangesAsync();
            }

            var _GetExpenseSummaryList = _SeedData.GetExpenseSummaryList();
            foreach (var item in _GetExpenseSummaryList)
            {
                item.Action = DBOperationType.Add;
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.ExpenseSummary.Add(item);
                await _context.SaveChangesAsync();
            }

            var _GetExpenseDetailsList = _SeedData.GetExpenseDetailsList();
            foreach (var item in _GetExpenseDetailsList)
            {
                item.TotalPrice = item.Quantity * item.UnitPrice;
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.ExpenseDetails.Add(item);
                await _context.SaveChangesAsync();
            }

            var _GetBranchList = _SeedData.GetBranchList();
            foreach (var item in _GetBranchList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.Branch.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetManageRoleList = _SeedData.GetManageRoleList();
            foreach (var item in _GetManageRoleList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.ManageUserRoles.Add(item);
                await _context.SaveChangesAsync();
            }

            for (int i = 1; i <= 5; i++)
            {
                var _DateTime = String.Format("{0:f}", DateTime.Now.AddDays(-i));
                ExpenseSummary _ExpenseSummary = new ExpenseSummary { Title = "Office Management Regular Expense: " + _DateTime, DueAmount = 0, ChangeAmount = 0, CurrencyCode = 1 };

                _ExpenseSummary.GrandTotal = _GetExpenseDetailsList.Where(x => x.ExpenseSummaryId == i).Sum(x => x.TotalPrice);
                _ExpenseSummary.PaidAmount = _ExpenseSummary.GrandTotal;
                _ExpenseSummary.CreatedDate = DateTime.Now;
                _ExpenseSummary.ModifiedDate = DateTime.Now;
                _ExpenseSummary.CreatedBy = "Admin";
                _ExpenseSummary.ModifiedBy = "Admin";
                _context.ExpenseSummary.Add(_ExpenseSummary);
                await _context.SaveChangesAsync();
            }

            var _GetIncomeTypeList = _SeedData.GetIncomeTypeList();
            foreach (var item in _GetIncomeTypeList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.IncomeType.Add(item);
                await _context.SaveChangesAsync();
            }

            var _GetIncomeCategoryList = _SeedData.GetIncomeCategoryList();
            foreach (var item in _GetIncomeCategoryList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.IncomeCategory.Add(item);
                await _context.SaveChangesAsync();
            }

            var _GetIncomeSummaryList = _SeedData.GetIncomeSummaryList();
            foreach (var item in _GetIncomeSummaryList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.IncomeSummary.Add(item);
                await _context.SaveChangesAsync();
            }

            var _GetAccAccountList = _SeedData.GetAccAccountList();
            foreach (var item in _GetAccAccountList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.AccAccount.Add(item);
                await _context.SaveChangesAsync();

                //Add tran
                UpdateAccountViewModel _UpdateAccountViewModel = new()
                {
                    AccUpdateType = AccAccountUpdateType.Credit,
                    AccAccountNo = item.Id,
                    Amount = item.Balance,
                    Credit = item.Balance,
                    Debit = 0,
                    Type = "Initial Account Deposit",
                    Reference = "Initial Amount. Id: " + item.Id,
                    Description = item.Description,
                    UserName = "Admin"
                };
                await _iCommon.UpdateAccoutDuringTran(_UpdateAccountViewModel);
            }

            var _GetCompanyInfo = _SeedData.GetCompanyInfo();
            _GetCompanyInfo.CreatedDate = DateTime.Now;
            _GetCompanyInfo.ModifiedDate = DateTime.Now;
            _GetCompanyInfo.CreatedBy = "Admin";
            _GetCompanyInfo.ModifiedBy = "Admin";
            _context.CompanyInfo.Add(_GetCompanyInfo);
            await _context.SaveChangesAsync();

            //Generate Demo Sales: Ex tblPayment.sql
            var result = _iCommon.GenerateDemoSales();
        }
        public async Task GenerateUserUserRole()
        {
            var _ManageRole = await _context.ManageUserRoles.ToListAsync();
            var _GetRoleList = await _roles.GetRoleList();

            foreach (var role in _ManageRole)
            {
                foreach (var item in _GetRoleList)
                {
                    ManageUserRolesDetails _ManageRoleDetails = new();
                    _ManageRoleDetails.ManageRoleId = role.Id;
                    _ManageRoleDetails.RoleId = item.RoleId;
                    _ManageRoleDetails.RoleName = item.RoleName;

                    if (role.Id == 1)
                    {
                        _ManageRoleDetails.IsAllowed = true;
                    }
                    else if (role.Id == 2 && item.RoleName == "User Profile" || item.RoleName == "Business ERP"
                    || item.RoleName == "Dashboard")
                    {
                        _ManageRoleDetails.IsAllowed = true;
                    }
                    else
                    {
                        _ManageRoleDetails.IsAllowed = false;
                    }

                    _ManageRoleDetails.CreatedDate = DateTime.Now;
                    _ManageRoleDetails.ModifiedDate = DateTime.Now;
                    _ManageRoleDetails.CreatedBy = "Admin";
                    _ManageRoleDetails.ModifiedBy = "Admin";
                    _context.Add(_ManageRoleDetails);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task CreateItem()
        {
            try
            {
                var _GetItemList = _SeedData.GetItemList();
                foreach (var item in _GetItemList)
                {
                    item.Code = "ITM" + StaticData.RandomDigits(6);
                    item.Barcode = SampleBarcode.Default;

                    item.SupplierId = 1;
                    item.WarehouseId = 1;
                    item.StockKeepingUnit = "SKU" + StaticData.RandomDigits(6);
                    item.Note = "Your item details are noted here. Please write notes if any.";

                    item.OldUnitPrice = item.CostPrice;
                    item.NormalPrice = item.CostPrice + (item.CostPrice) * (0.05);
                    item.OldSellPrice = item.CostPrice + (item.CostPrice) * (0.05);

                    item.TradePrice = item.CostPrice + (item.CostPrice) * (0.05);
                    item.PremiumPrice = item.CostPrice + (item.CostPrice) * (0.05);
                    item.OtherPrice = item.CostPrice + (item.CostPrice) * (0.05);

                    item.VatPercentage = 5;
                    item.CostVAT = Math.Round(0.05 * item.CostPrice, 2);
                    item.NormalVAT = Math.Round(0.05 * item.NormalPrice, 2);
                    item.TradeVAT = Math.Round(0.05 * (double)item.TradePrice, 2);
                    item.PremiumVAT = Math.Round(0.05 * (double)item.PremiumPrice, 2);
                    item.OtherVAT = Math.Round(0.05 * (double)item.OtherPrice, 2);

                    item.Size = 12;

                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    _context.Items.Add(item);
                    await _context.SaveChangesAsync();

                    ItemsCRUDViewModel _ItemsCRUDViewModel = item;
                    ItemsHistoryCRUDViewModel vm = _ItemsCRUDViewModel;
                    vm.ItemId = item.Id;
                    vm.Id = 0;
                    vm.Action = "Create New Item-" + item.Name;
                    vm.TranQuantity = 0;
                    vm.OldQuantity = item.Quantity;
                    vm.NewQuantity = item.Quantity;

                    vm.CreatedBy = "Admin";
                    vm.ModifiedBy = "Admin";
                    var result = await _iCommon.AddItemHistory(vm);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task CreateDefaultSuperAdmin()
        {
            try
            {
                await _roles.GenerateRolesFromPageList();

                ApplicationUser superAdmin = new();
                superAdmin.Email = _superAdminDefaultOptions.Email;
                superAdmin.UserName = superAdmin.Email;
                superAdmin.EmailConfirmed = true;

                var result = await _userManager.CreateAsync(superAdmin, _superAdminDefaultOptions.Password);

                if (result.Succeeded)
                {
                    UserProfile profile = new()
                    {
                        ApplicationUserId = superAdmin.Id,
                        FirstName = "Super",
                        LastName = "Admin",
                        PhoneNumber = "+8801674411603",
                        Email = superAdmin.Email,
                        Address = "R/A, Dhaka",
                        Country = "Bangladesh",
                        ProfilePicture = "/upload/DefaultUser/super-admin.jpg",
                        EmployeeId = "EMP_1010101",
                        BranchId = 1,
                        RoleId = 1,

                        DateOfBirth = DateTime.Now,
                        JoiningDate = DateTime.Now,
                        LeavingDate = DateTime.Now,

                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = "Admin",
                        ModifiedBy = "Admin"
                    };

                    await _context.UserProfile.AddAsync(profile);
                    await _context.SaveChangesAsync();

                    await _roles.AddToRoles(superAdmin);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task CreateDefaultOtherUser()
        {
            var _GetUserProfileList = _SeedData.GetUserProfileList();

            foreach (var item in _GetUserProfileList)
            {
                item.BranchId = 2;
                item.RoleId = 2;
                var _ApplicationUser = await _iAccount.CreateUserProfile(item, "Admin");
            }
        }
        public async Task<string> UploadFile(List<IFormFile> files, IWebHostEnvironment env, string uploadFolder)
        {
            var result = "";

            var webRoot = env.WebRootPath;
            var uploads = Path.Combine(webRoot, uploadFolder);
            var extension = "";
            var filePath = "";
            var fileName = "";


            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    extension = Path.GetExtension(formFile.FileName);
                    fileName = Guid.NewGuid().ToString() + extension;
                    filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    result = fileName;

                }
            }

            return result;
        }
        public async Task<SharedUIDataViewModel> GetSharedUIData(ClaimsPrincipal _ClaimsPrincipal)
        {
            SharedUIDataViewModel _SharedUIDataViewModel = new();
            ApplicationUser _ApplicationUser = await _userManager.GetUserAsync(_ClaimsPrincipal);
            _SharedUIDataViewModel.ApplicationInfo = _applicationInfo;

            if (_ApplicationUser == null)
            {
                return _SharedUIDataViewModel;
            }

            _SharedUIDataViewModel.UserProfile = _context.UserProfile.FirstOrDefault(x => x.ApplicationUserId == _ApplicationUser.Id);
            _SharedUIDataViewModel.MainMenuViewModel = await _roles.RolebaseMenuLoad(_ApplicationUser);
            return _SharedUIDataViewModel;
        }
        public async Task<DefaultIdentityOptions> GetDefaultIdentitySettings()
        {
            return await _context.DefaultIdentityOptions.Where(x => x.Id == 1).FirstOrDefaultAsync();
        }
        public async Task CreateDefaultIdentitySettings()
        {
            if (_context.DefaultIdentityOptions.Count() < 1)
            {
                DefaultIdentityOptions _DefaultIdentityOptions = new DefaultIdentityOptions
                {
                    PasswordRequireDigit = false,
                    PasswordRequiredLength = 3,
                    PasswordRequireNonAlphanumeric = false,
                    PasswordRequireUppercase = false,
                    PasswordRequireLowercase = false,
                    PasswordRequiredUniqueChars = 0,
                    LockoutDefaultLockoutTimeSpanInMinutes = 30,
                    LockoutMaxFailedAccessAttempts = 5,
                    LockoutAllowedForNewUsers = false,
                    UserRequireUniqueEmail = true,
                    SignInRequireConfirmedEmail = false,

                    CookieHttpOnly = true,
                    CookieExpiration = 150,
                    CookieExpireTimeSpan = 120,
                    LoginPath = "/Account/Login",
                    LogoutPath = "/Account/Logout",
                    AccessDeniedPath = "/Account/AccessDenied",
                    SlidingExpiration = true,

                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = "Admin",
                    ModifiedBy = "Admin",
                };
                _context.Add(_DefaultIdentityOptions);
                await _context.SaveChangesAsync();
            }
        }
    }
}
