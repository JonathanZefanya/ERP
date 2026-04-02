using BusinessERP.Helpers;
using BusinessERP.Models;
using BusinessERP.Models.UserProfileViewModel;

namespace BusinessERP.Data
{
    public class SeedData
    {
        public IEnumerable<Items> GetItemList()
        {

            var result = new List<Items>
            {
                new Items { Name = "HDD", MeasureId = 14, MeasureValue = 1, CategoriesId = 8, CostPrice = 500, Quantity = 200, ImageURL = "/upload/DefaultItem/hdd.jpg"},
                new Items { Name = "Laptop", MeasureId = 14, MeasureValue = 1, CategoriesId = 8, CostPrice = 550, Quantity = 300, ImageURL = "/upload/DefaultItem/laptop.jpg"},
                new Items { Name = "Monitor", MeasureId = 14, MeasureValue = 1, CategoriesId = 8, CostPrice = 1110, Quantity = 500, ImageURL = "/upload/DefaultItem/monitor.jpg"},
                new Items { Name = "Mother-board", MeasureId = 14, MeasureValue = 1, CategoriesId = 8, CostPrice = 1150, Quantity = 200, ImageURL = "/upload/DefaultItem/motherboard.jpg"},
                new Items { Name = "Power-supply", MeasureId = 14, MeasureValue = 1, CategoriesId = 9, CostPrice = 550, Quantity = 100, ImageURL = "/upload/DefaultItem/powersupply.jpg"},

                new Items { Name = "Multi-plug", MeasureId = 14, MeasureValue = 1, CategoriesId = 9, CostPrice = 550, Quantity = 70, ImageURL = "/upload/DefaultItem/multiplug.jpg"},
                new Items { Name = "SSD", MeasureId = 14, MeasureValue = 1, CategoriesId = 8,  CostPrice = 2290, Quantity = 200, ImageURL = "/upload/DefaultItem/ssd.jpg"},
                new Items { Name = "RAM", MeasureId = 14, MeasureValue = 1, CategoriesId = 8, CostPrice = 1190, Quantity = 100, ImageURL = "/upload/DefaultItem/ram.jpg"},
                new Items { Name = "TV Remote", MeasureId = 14, MeasureValue = 1, CategoriesId = 9, CostPrice = 200, Quantity = 100, ImageURL = "/upload/DefaultItem/remote.jpg"},
                new Items { Name = "iPhone 14", MeasureId = 14, MeasureValue = 1, CategoriesId = 9, CostPrice = 1200, Quantity = 300, ImageURL = "/upload/DefaultItem/iphone.jpg"},

                new Items { Name = "S22 Ultra", MeasureId = 14, MeasureValue = 1, CategoriesId = 9, CostPrice = 850, Quantity = 270, ImageURL = "/upload/DefaultItem/s2ultra.jpg"},
                new Items { Name = "Xiaomi TV", MeasureId = 14, MeasureValue = 1, CategoriesId = 9,  CostPrice = 1590, Quantity = 200, ImageURL = "/upload/DefaultItem/xiaomitv.jpg"},
                new Items { Name = "Travel Camera", MeasureId = 14, MeasureValue = 1, CategoriesId = 9, CostPrice = 1600, Quantity = 100, ImageURL = "/upload/DefaultItem/travelcamera.jpg"},
                new Items { Name = "Coffee Mug", MeasureId = 14, MeasureValue = 1, CategoriesId = 1, CostPrice = 120, Quantity = 100, ImageURL = "/upload/DefaultItem/coffeemug.jpg"},
                new Items { Name = "Water Hitter", MeasureId = 14, MeasureValue = 1, CategoriesId = 9, CostPrice = 800, Quantity = 300, ImageURL = "/upload/DefaultItem/waterhitter.jpg"},

                new Items { Name = "Olive oil", MeasureId = 14, MeasureValue = 1, CategoriesId = 7, CostPrice = 170, Quantity = 370, ImageURL = "/upload/DefaultItem/oliveoil.jpg"},
                new Items { Name = "Xiaomi 9 Pro", MeasureId = 14, MeasureValue = 1, CategoriesId = 9,  CostPrice = 900, Quantity = 200, ImageURL = "/upload/DefaultItem/xiaomi9.jpg"},
                new Items { Name = "Rui Fish", MeasureId = 14, MeasureValue = 1, CategoriesId = 5,  CostPrice = 500, Quantity = 100, ImageURL = "/upload/DefaultItem/ruifish.jpg"},
                new Items { Name = "Orange", MeasureId = 14, MeasureValue = 1, CategoriesId = 1,  CostPrice = 150, Quantity = 100, ImageURL = "/upload/DefaultItem/orange.jpg"},
                new Items { Name = "Milk", MeasureId = 14, MeasureValue = 1, CategoriesId = 2,  CostPrice = 70, Quantity = 300, ImageURL = "/upload/DefaultItem/milk.jpg"},

                new Items { Name = "Cocacoal", MeasureId = 14, MeasureValue = 1, CategoriesId = 3,  CostPrice = 90, Quantity = 500, ImageURL = "/upload/DefaultItem/cocacoal.jpg"},
                new Items { Name = "Icecream", MeasureId = 14, MeasureValue = 1, CategoriesId = 4,  CostPrice = 220, Quantity = 50, ImageURL = "/upload/DefaultItem/icecream.jpg"},
                new Items { Name = "Tomato", MeasureId = 14, MeasureValue = 1, CategoriesId = 6,  CostPrice = 30, Quantity = 100, ImageURL = "/upload/DefaultItem/tomato.jpg"},
                new Items { Name = "Jonsonlotion", MeasureId = 14, MeasureValue = 1, CategoriesId = 7,  CostPrice = 340, Quantity = 150, ImageURL = "/upload/DefaultItem/jonsonlotion.jpg"},
                new Items { Name = "Steel Water Bottle", MeasureId = 14, MeasureValue = 1, CategoriesId = 10,  CostPrice = 160, Quantity = 200, ImageURL = "/upload/DefaultItem/steelwaterbottle.jpg"}
             };

            return result;
        }

        public IEnumerable<Categories> GetCategoriesList()
        {
            return new List<Categories>
            {
                new Categories { Name = "Fruits", Description = "Fruits Item"},
                new Categories { Name = "Dairy Products", Description = "Dairy Products"},
                new Categories { Name = "Beverages", Description = "Soft drinks, coffees, teas, etc"},
                new Categories { Name = "Freezer", Description = "Freezer"},
                new Categories { Name = "Meat & Fish", Description = "Meat & Fish"},
                new Categories { Name = "Vegetables", Description = "Vegetables"},
                 new Categories { Name = "Beauty and Cosmetic", Description = "Beauty and Cosmetic"},

                new Categories { Name = "IT", Description = "IT"},
                new Categories { Name = "Electronics", Description = "Electronics"},
                new Categories { Name = "Steels", Description = "Coated Steel Sheet"},
                new Categories { Name = "Common", Description = "For common all items"},
            };
        }

        public IEnumerable<UnitsofMeasure> GetUnitsofMeasureList()
        {
            return new List<UnitsofMeasure>
            {
                new UnitsofMeasure { Name = "Kg", Description = "Kilogram"},
                new UnitsofMeasure { Name = "gr", Description = "Milligram"},
                new UnitsofMeasure { Name = "qt", Description = "Quintal"},
                new UnitsofMeasure { Name = "t", Description = "Tonne"},
                new UnitsofMeasure { Name = "L", Description = "Liter"},
                new UnitsofMeasure { Name = "ML", Description = "Milliliter"},
                new UnitsofMeasure { Name = "bbl", Description = "Barrel"},
                new UnitsofMeasure { Name = "gl", Description = "Gallon"},
                new UnitsofMeasure { Name = "Meter", Description = "Meter"},
                new UnitsofMeasure { Name = "Centimeter", Description = "Centimeter"},
                new UnitsofMeasure { Name = "Kilometer", Description = "Kilometer"},
                new UnitsofMeasure { Name = "Foot", Description = "Foot"},
                new UnitsofMeasure { Name = "Inch", Description = "Inch"},
                new UnitsofMeasure { Name = "Piece", Description = "Piece"}
            };
        }
        public IEnumerable<Supplier> GetSupplierList()
        {
            return new List<Supplier>
            {
                new Supplier { Name = "Walk in Supplier", ContactPerson = "TBD", Email="walkin@gmail.com" ,Phone="01699000",Address="Washington" },
                new Supplier { Name = "Common Supplier", ContactPerson = "TBD", Email="dev@gmail.com" ,Phone="01699000",Address="Dhaka"},
                new Supplier { Name = "Google", ContactPerson = "TBD", Email="dev@gmail.com" ,Phone="01699000",Address="USA"},
                new Supplier { Name = "Amazon", ContactPerson = "TBD", Email="dev@gmail.com" ,Phone="01699000",Address="USA"},
                new Supplier { Name = "Microsoft", ContactPerson = "TBD", Email="dev@gmail.com" ,Phone="01699000",Address="USA"},
                new Supplier { Name = "PHP", ContactPerson = "TBD", Email="dev@gmail.com" ,Phone="01699000",Address="Dhaka"},
                new Supplier { Name = "Unilever", ContactPerson = "TBD", Email="dev@gmail.com" ,Phone="01699000",Address="Dhaka"}
            };
        }

        public IEnumerable<Warehouse> GetWarehouseList()
        {
            return new List<Warehouse>
            {
                new Warehouse { Name = "Dhaka, Bangladesh", Description = "TBD" },
                new Warehouse { Name = "Chittagone,  Bangladesh", Description = "TBD" },
                new Warehouse { Name = "California, USA", Description = "TBD" },
                new Warehouse { Name = "Berlin, Germany", Description = "TBD"  },
                new Warehouse { Name = "Paris , France", Description = "TBD" }
            };
        }
        public IEnumerable<VatPercentage> GetVatPercentageList()
        {
            return new List<VatPercentage>
            {
                new VatPercentage { Name = "VAT: 0%", Percentage = 0, IsDefault = false },
                new VatPercentage { Name = "VAT: 1%", Percentage = 1, IsDefault = false },
                new VatPercentage { Name = "VAT: 2%", Percentage = 2, IsDefault = false },
                new VatPercentage { Name = "VAT: 3%", Percentage = 3, IsDefault = false },
                new VatPercentage { Name = "VAT: 4%", Percentage = 4, IsDefault = false },

                new VatPercentage { Name = "VAT: 5%", Percentage = 5, IsDefault = true },
                new VatPercentage { Name = "VAT: 6%", Percentage = 6, IsDefault = false },
                new VatPercentage { Name = "VAT: 7%", Percentage = 7, IsDefault = false },
                new VatPercentage { Name = "VAT: 8%", Percentage = 8, IsDefault = false },
                new VatPercentage { Name = "VAT: 9%", Percentage = 9, IsDefault = false },
                new VatPercentage { Name = "VAT: 10%", Percentage = 10, IsDefault = false },

                new VatPercentage { Name = "VAT: 10%", Percentage = 10, IsDefault = false },
                new VatPercentage { Name = "VAT: 20%", Percentage = 20, IsDefault = false },
                new VatPercentage { Name = "VAT: 30%", Percentage = 30, IsDefault = false },
                new VatPercentage { Name = "VAT: 40%", Percentage = 40, IsDefault = false },
                new VatPercentage { Name = "VAT: 50%", Percentage = 50, IsDefault = false },
            };
        }

        public IEnumerable<CustomerInfo> GetCustomerInfoList()
        {
            return new List<CustomerInfo>
            {
                new CustomerInfo { Name = "Walk in Customer", Type = 1, Email = "walkincustomer@gmail.com", BillingAddress = "New York, USA" },
                new CustomerInfo { Name = "Mr. Bond", Type = 1, Email = "bond@gmail.com", BillingAddress = "Washing DC, USA" },
                new CustomerInfo { Name = "Alex Hill", Type = 1, Email = "hill@gmail.com", BillingAddress = "Lodon, UK" },
                new CustomerInfo { Name = "Games Underson", Type = 1, Email = "Underson@gmail.com", BillingAddress = "Cardif, UK" },
                new CustomerInfo { Name = "Albert Einstein", Type = 1, Email = "einstein@gmail.com", BillingAddress = "Harburg, Germany" },
                new CustomerInfo { Name = "Zahid Hasan", Type = 1, Email = "hasan@gmail.com", BillingAddress = "Dhaka, Bangladesh" },
                new CustomerInfo { Name = "Shahed", Type = 1, Email = "shahedbddev@gmail.com", BillingAddress = "Dhaka, Bangladesh" },
            };
        }
        public IEnumerable<CustomerType> GetCustomerTypeList()
        {
            return new List<CustomerType>
            {
                new CustomerType { Name = "Normal", Description = "Normal" },
                new CustomerType { Name = "Premium", Description = "Premium" },
                new CustomerType { Name = "Trader", Description = "Trader" },
                new CustomerType { Name = "Other", Description = "Other" },
            };
        }

        public IEnumerable<Currency> GetCurrencyList()
        {
            return new List<Currency>
            {
                new Currency { Name = "US Dollar", Code = "USD", Symbol = "$", Country = "United States", Description = "United States Dollar", IsDefault = true },
                new Currency { Name = "Euro", Code = "EUR", Symbol = "€", Country = "European Union", Description = "European Union Currency", IsDefault = false },
                new Currency { Name = "Pound Sterling", Code = "GBP", Symbol = "£", Country = "United Kingdom", Description = "British Pound", IsDefault = false },
                new Currency { Name = "Japanese Yen", Code = "JPY", Symbol = "¥", Country = "Japan", Description = "Japanese Yen", IsDefault = false },
                new Currency { Name = "Bangladeshi Taka", Code = "BDT", Symbol = "৳", Country = "Bangladesh", Description = "Bangladeshi Taka", IsDefault = false },
                new Currency { Name = "Australian Dollar", Code = "AUD", Symbol = "A$", Country = "Australia", Description = "Australian Dollar", IsDefault = false },
                new Currency { Name = "Canadian Dollar", Code = "CAD", Symbol = "C$", Country = "Canada", Description = "Canadian Dollar", IsDefault = false },
                new Currency { Name = "Swiss Franc", Code = "CHF", Symbol = "CHF", Country = "Switzerland", Description = "Swiss Franc", IsDefault = false },
                new Currency { Name = "Chinese Yuan Renminbi", Code = "CNY", Symbol = "¥", Country = "China", Description = "Chinese Yuan", IsDefault = false },
                new Currency { Name = "Indian Rupee", Code = "INR", Symbol = "₹", Country = "India", Description = "Indian Rupee", IsDefault = false },
                new Currency { Name = "Singapore Dollar", Code = "SGD", Symbol = "S$", Country = "Singapore", Description = "Singapore Dollar", IsDefault = false },
                new Currency { Name = "Hong Kong Dollar", Code = "HKD", Symbol = "HK$", Country = "Hong Kong", Description = "Hong Kong Dollar", IsDefault = false },
                new Currency { Name = "New Zealand Dollar", Code = "NZD", Symbol = "NZ$", Country = "New Zealand", Description = "New Zealand Dollar", IsDefault = false },
                new Currency { Name = "South Korean Won", Code = "KRW", Symbol = "₩", Country = "South Korea", Description = "South Korean Won", IsDefault = false },
                new Currency { Name = "Russian Ruble", Code = "RUB", Symbol = "₽", Country = "Russia", Description = "Russian Ruble", IsDefault = false },
                new Currency { Name = "Brazilian Real", Code = "BRL", Symbol = "R$", Country = "Brazil", Description = "Brazilian Real", IsDefault = false },
                new Currency { Name = "South African Rand", Code = "ZAR", Symbol = "R", Country = "South Africa", Description = "South African Rand", IsDefault = false },
                new Currency { Name = "Turkish Lira", Code = "TRY", Symbol = "₺", Country = "Turkey", Description = "Turkish Lira", IsDefault = false },
                new Currency { Name = "Mexican Peso", Code = "MXN", Symbol = "$", Country = "Mexico", Description = "Mexican Peso", IsDefault = false },
                new Currency { Name = "Saudi Riyal", Code = "SAR", Symbol = "﷼", Country = "Saudi Arabia", Description = "Saudi Riyal", IsDefault = false },
            };
        }

        public IEnumerable<PaymentType> GetPaymentTypeList()
        {
            return new List<PaymentType>
            {
                new PaymentType { Name = "Cash", Description = "Cash"},
                new PaymentType { Name = "Bank", Description = "Bank"},
                new PaymentType { Name = "POS", Description = "POS"},
                new PaymentType { Name = "Mobile-Banking", Description = "Mobile-Banking"},
                new PaymentType { Name = "Credit Card", Description = "Credit Card"},

                new PaymentType { Name = "Debit Card", Description = "Debit Card"},
                new PaymentType { Name = "Other", Description = "Other"},
            };
        }
        public IEnumerable<PaymentStatus> GetPaymentStatusList()
        {
            return new List<PaymentStatus>
            {
                new PaymentStatus { Name = "Paid", Description = "Paid"},
                new PaymentStatus { Name = "UnPaid", Description = "UnPaid"},
                new PaymentStatus { Name = "Partially Paid", Description = "Partially Paid"},
                new PaymentStatus { Name = "Deposit", Description = "Deposit"},
                new PaymentStatus { Name = "Pay within 7 Days", Description = "Pay within 7 Days"},

                new PaymentStatus { Name = "Pay within 14 Days", Description = "Pay within 14 Days"},
                new PaymentStatus { Name = "Pay within 30 Days", Description = "Pay within 30 Days"},
                new PaymentStatus { Name = "Custom Date", Description = "Custom Date"},
            };
        }

        public IEnumerable<UserProfileCRUDViewModel> GetUserProfileList()
        {
            return new List<UserProfileCRUDViewModel>
            {
                new UserProfileCRUDViewModel { FirstName = "Shop5", LastName = "User", Email = "Shop5@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DefaultUser/U1.png", Address = "California", Country = "USA", BranchId = 1 },
                new UserProfileCRUDViewModel { FirstName = "Shop4", LastName = "User", Email = "Shop4@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DefaultUser/U2.png", Address = "California", Country = "USA", BranchId = 1 },
                new UserProfileCRUDViewModel { FirstName = "Shop3", LastName = "User", Email = "Shop3@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DefaultUser/U3.png", Address = "California", Country = "USA", BranchId = 1 },
                new UserProfileCRUDViewModel { FirstName = "Shop2", LastName = "User", Email = "Shop2@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DefaultUser/U4.png", Address = "California", Country = "USA", BranchId = 1 },
                new UserProfileCRUDViewModel { FirstName = "Shop1", LastName = "User", Email = "Shop1@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DefaultUser/U5.png", Address = "California", Country = "USA", BranchId = 1 },

                new UserProfileCRUDViewModel { FirstName = "Accountants1", LastName = "User", Email = "accountants1@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DefaultUser/U6.png", Address = "California", Country = "USA", BranchId = 2 },
                new UserProfileCRUDViewModel { FirstName = "Accountants2", LastName = "User", Email = "accountants2@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DefaultUser/U7.png", Address = "California", Country = "USA", BranchId = 3 },
                new UserProfileCRUDViewModel { FirstName = "Accountants3", LastName = "User", Email = "accountants3@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DefaultUser/U8.png", Address = "California", Country = "USA", BranchId = 4 },
                new UserProfileCRUDViewModel { FirstName = "Accountants4", LastName = "User", Email = "accountants4@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DefaultUser/U9.png", Address = "California", Country = "USA", BranchId = 5 },
                new UserProfileCRUDViewModel { FirstName = "Accountants5", LastName = "User", Email = "accountants5@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/DefaultUser/U10.png", Address = "California", Country = "USA", BranchId = 6 },
            };
        }
        public IEnumerable<EmailConfig> GetEmailConfigList()
        {
            return new List<EmailConfig>
            {
                new EmailConfig { Email = "devmlbd@gmail.com", Password = "", Hostname = "smtp.gmail.com", Port = 587, SSLEnabled = true, IsDefault = true },
                new EmailConfig { Email = "exmapl1@gmail.com", Password = "123", Hostname = "smtp.gmail.com", Port = 587, SSLEnabled = false, IsDefault = false },
                new EmailConfig { Email = "exmapl2@gmail.com", Password = "123", Hostname = "smtp.gmail.com", Port = 587, SSLEnabled = false, IsDefault = false },
                new EmailConfig { Email = "exmapl3@gmail.com", Password = "123", Hostname = "smtp.gmail.com", Port = 587, SSLEnabled = false, IsDefault = false },
            };
        }
        public IEnumerable<Department> GetDepartmentList()
        {
            return new List<Department>
            {
                new Department { Name = "IT", Description = "IT Department"},
                new Department { Name = "HR", Description = "HR Department"},
                new Department { Name = "Finance", Description = "Finance Department"},
                new Department { Name = "Procurement", Description = "Procurement Department"},
                new Department { Name = "Legal", Description = "Procurement Department"},
            };
        }
        public IEnumerable<SubDepartment> GetSubDepartmentList()
        {
            return new List<SubDepartment>
            {
                new SubDepartment { DepartmentId = 1, Name = "QA", Description = "QA Department"},
                new SubDepartment { DepartmentId = 1, Name = "Software Development", Description = "Software Development Department"},
                new SubDepartment { DepartmentId = 1, Name = "Operation", Description = "Operation Department"},
                new SubDepartment { DepartmentId = 1, Name = "PM", Description = "Project Management Department"},
                new SubDepartment { DepartmentId = 2, Name = "Recruitment", Description = "Recruitment Department"},
            };
        }

        public IEnumerable<Employee> GetEmployeeList()
        {
            return new List<Employee>
            {
                new Employee { EmployeeId = StaticData.RandomDigits(6), FirstName = "Mr", LastName = "Tom", DateOfBirth = DateTime.Now.AddYears(-25), Designation = 1, Department = 1, SubDepartment = 4, JoiningDate = DateTime.Now.AddYears(-1), LeavingDate = DateTime.Now, Phone = StaticData.RandomDigits(11), Email="dev1@gmail.com", Address="USA"},
                new Employee { EmployeeId = StaticData.RandomDigits(6), FirstName = "Mr", LastName = "Bond", DateOfBirth = DateTime.Now.AddYears(-26), Designation = 2, Department = 1, SubDepartment = 2, JoiningDate = DateTime.Now.AddYears(-1), LeavingDate = DateTime.Now, Phone = StaticData.RandomDigits(11), Email="dev2@gmail.com", Address="UK"},
                new Employee { EmployeeId = StaticData.RandomDigits(6), FirstName = "Mr", LastName = "Hasan", DateOfBirth = DateTime.Now.AddYears(-27), Designation = 2, Department = 1, SubDepartment = 2, JoiningDate = DateTime.Now.AddYears(-1), LeavingDate = DateTime.Now, Phone = StaticData.RandomDigits(11), Email="dev3@gmail.com", Address="Germany"},
                new Employee { EmployeeId = StaticData.RandomDigits(6), FirstName = "Mr", LastName = "Alex", DateOfBirth = DateTime.Now.AddYears(-28), Designation = 2, Department = 1, SubDepartment = 2, JoiningDate = DateTime.Now.AddYears(-1), LeavingDate = DateTime.Now, Phone = StaticData.RandomDigits(11), Email="dev4@gmail.com", Address="Netherland"},
                new Employee { EmployeeId = StaticData.RandomDigits(6), FirstName = "Ms", LastName = "Merry", DateOfBirth = DateTime.Now.AddYears(-29), Designation = 3, Department = 1, SubDepartment = 1, JoiningDate = DateTime.Now.AddYears(-1), LeavingDate = DateTime.Now, Phone = StaticData.RandomDigits(11), Email="dev5@gmail.com", Address="Franch"},
            };
        }
        public IEnumerable<Designation> GetDesignationList()
        {
            return new List<Designation>
            {
                new Designation { Name = "Project Manager", Description = "Employee Job Designation"},
                new Designation { Name = "Software Engineer", Description = "Employee Job Designation"},
                new Designation { Name = "Head Of Engineering", Description = "Employee Job Designation"},
                new Designation { Name = "Software Architect", Description = "Employee Job Designation"},
                new Designation { Name = "QA Engineer", Description = "Employee Job Designation"},
                new Designation { Name = "DevOps Engineer", Description = "Employee Job Designation"},
            };
        }
        public IEnumerable<ExpenseType> GetExpenseTypeList()
        {
            return new List<ExpenseType>
            {
                new ExpenseType { Name = "General Expense", Description = "General Expense", },
                new ExpenseType { Name = "Staff Salary", Description = "Staff Salary Expense", },
                new ExpenseType { Name = "Office Maintenance", Description = "Office Maintenance Expense", },
                new ExpenseType { Name = "Buy New Product", Description = "Buy New Product Expense", },
                new ExpenseType { Name = "Food Cost", Description = "Food Cost Expense"},

                new ExpenseType { Name = "Transport Cost", Description = "Transport Expense"},
                new ExpenseType { Name = "Office Tour", Description = "Office Tour Expense"},
                new ExpenseType { Name = "Employee Bonus", Description = "Employee Bonus Expense"},
                new ExpenseType { Name = "House Rent", Description = "House Rent Expense"},
                new ExpenseType { Name = "Common Expense", Description = "Common Expense", },
            };
        }

        public IEnumerable<ExpenseSummary> GetExpenseSummaryList()
        {
            return new List<ExpenseSummary>
            {
                new ExpenseSummary { Title = "Regular", GrandTotal = 17500, CurrencyCode = 1, BranchId = 3 },
                new ExpenseSummary { Title = "Regular", GrandTotal = 25000, CurrencyCode = 1, BranchId = 2 },
                new ExpenseSummary { Title = "Regular", GrandTotal = 8000, CurrencyCode = 1, BranchId = 1 },
                new ExpenseSummary { Title = "Regular", GrandTotal = 12000, CurrencyCode = 1, BranchId = 4 },
                new ExpenseSummary { Title = "Regular", GrandTotal = 68000, CurrencyCode = 1, BranchId = 5 },
            };
        }

        public IEnumerable<ExpenseDetails> GetExpenseDetailsList()
        {
            return new List<ExpenseDetails>
            {
                new ExpenseDetails { ExpenseSummaryId = 1, ExpenseTypeId = 1, ExpenseType = "General Expense", Description = "General Expense: Monthly", Quantity = 1, UnitPrice = 5000, },
                new ExpenseDetails { ExpenseSummaryId = 1, ExpenseTypeId = 1, ExpenseType = "General Expense", Description = "General Expense: Monthly", Quantity = 1, UnitPrice = 5000, },
                new ExpenseDetails { ExpenseSummaryId = 1, ExpenseTypeId = 1, ExpenseType = "General Expense", Description = "General Expense: Monthly", Quantity = 1, UnitPrice = 5000, },

                new ExpenseDetails { ExpenseSummaryId = 1, ExpenseTypeId = 2, ExpenseType = "Staff Salary", Description = "Staff Salary: Monthly", Quantity = 10, UnitPrice = 25000, },
                new ExpenseDetails { ExpenseSummaryId = 2, ExpenseTypeId = 3, ExpenseType = "Office Maintenance", Description = "Office Maintenance: Monthly", Quantity = 1, UnitPrice = 8000, },
                new ExpenseDetails { ExpenseSummaryId = 3, ExpenseTypeId = 4, ExpenseType = "Buy New Product", Description = "Buy New Product: Regular", Quantity = 231, UnitPrice = 2800, },
                new ExpenseDetails { ExpenseSummaryId = 3, ExpenseTypeId = 5, ExpenseType = "Food Cost", Description = "Food Cost: Regular", Quantity = 1, UnitPrice = 4000, },

                new ExpenseDetails { ExpenseSummaryId = 4, ExpenseTypeId = 6, ExpenseType = "Transport Cost", Description = "Transport Cost: Monthly", Quantity = 30, UnitPrice = 2500, },
                new ExpenseDetails { ExpenseSummaryId = 4, ExpenseTypeId = 7, ExpenseType = "Office Tour", Description = "Office Tour: Yearly", Quantity = 1, UnitPrice = 54000, },
                new ExpenseDetails { ExpenseSummaryId = 4, ExpenseTypeId = 8, ExpenseType = "Employee Bonus", Description = "Employee Bonus: Festival", Quantity = 10, UnitPrice = 8000, },
                new ExpenseDetails { ExpenseSummaryId = 5, ExpenseTypeId = 9, ExpenseType = "House Rent", Description = "House Rent: MOnthly", Quantity = 2, UnitPrice = 29000, },
                new ExpenseDetails { ExpenseSummaryId = 5, ExpenseTypeId = 10, ExpenseType = "Common Expense", Description = "Common Expense: Regular", Quantity = 5, UnitPrice = 800, },
                new ExpenseDetails { ExpenseSummaryId = 5, ExpenseTypeId = 10, ExpenseType = "Common Expense", Description = "Common Expense: Regular", Quantity = 5, UnitPrice = 800, },
                new ExpenseDetails { ExpenseSummaryId = 5, ExpenseTypeId = 10, ExpenseType = "Common Expense", Description = "Common Expense: Regular", Quantity = 5, UnitPrice = 800, },
                new ExpenseDetails { ExpenseSummaryId = 5, ExpenseTypeId = 10, ExpenseType = "Common Expense", Description = "Common Expense: Regular", Quantity = 5, UnitPrice = 800, },
            };
        }

        public IEnumerable<Branch> GetBranchList()
        {
            return new List<Branch>
            {
                new Branch { Name = "Main Branch", ContactPerson = "Admin", PhoneNumber = "123456789", Address = "Berlin, Germany", ShortDescription = "TBD" },

                new Branch { Name = "Branch One", ContactPerson = "Person 01", PhoneNumber = "123456789", Address = "Hamburg, Germany", ShortDescription = "TBD" },
                new Branch { Name = "Branch Two", ContactPerson = "Person 02", PhoneNumber = "123456789", Address = "Munich, Germany", ShortDescription = "TBD" },
                new Branch { Name = "Branch Three", ContactPerson = "Person 03", PhoneNumber = "123456789", Address = "Frankfurt, Germany", ShortDescription = "TBD" },
                new Branch { Name = "Branch Four", ContactPerson = "Person 04", PhoneNumber = "123456789", Address = "Leipzig, Germany", ShortDescription = "TBD" },
                new Branch { Name = "Branch Five", ContactPerson = "Person 05", PhoneNumber = "123456789", Address = "Parish, French", ShortDescription = "TBD" },
            };
        }
        public IEnumerable<ManageUserRoles> GetManageRoleList()
        {
            return new List<ManageUserRoles>
            {
                new ManageUserRoles { Name = "Admin", Description = "User Role: New"},
                new ManageUserRoles { Name = "General", Description = "User Role: General"},
            };
        }
        public IEnumerable<IncomeType> GetIncomeTypeList()
        {
            return new List<IncomeType>
            {
                new IncomeType { Name = "Active Income", Description = "Active Income"},
                new IncomeType { Name = "Capital Gains", Description = "Capital Gains"},
                new IncomeType { Name = "Interest", Description = "Interest"},
                new IncomeType { Name = "Passive Income", Description = "Passive Income"},
                new IncomeType { Name = "Salary", Description = "Salary"},

                new IncomeType { Name = "Dividends", Description = "Dividends"},
                new IncomeType { Name = "Earned Income", Description = "Earned Income"},
                new IncomeType { Name = "Portfolio Income", Description = "Portfolio Income"},
                new IncomeType { Name = "Property Income", Description = "Property Income"},
            };
        }
        public IEnumerable<IncomeCategory> GetIncomeCategoryList()
        {
            return new List<IncomeCategory>
            {
                new IncomeCategory { Name = "Job", Description = "Job"},
                new IncomeCategory { Name = "Freelance", Description = "Freelance"},
                new IncomeCategory { Name = "Overtime", Description = "Overtime"},
                new IncomeCategory { Name = "Mentorship", Description = "Mentorship"},
                new IncomeCategory { Name = "Invest profit", Description = "Invest profit"},

                new IncomeCategory { Name = "Old Book Sale", Description = "Old Book Sale"},
                new IncomeCategory { Name = "Consultancy", Description = "Consultancy"},
                new IncomeCategory { Name = "Content Writing", Description = "Content Writing"},
                new IncomeCategory { Name = "Old Gadget Sale", Description = "Old Gadget Sale"},
                new IncomeCategory { Name = "Bonus", Description = "Bonus"},
            };
        }
        public IEnumerable<IncomeSummary> GetIncomeSummaryList()
        {
            return new List<IncomeSummary>
            {
                new IncomeSummary { Title = "Regular", TypeId = 1, CategoryId = 1, Amount = 500, Description = "TBD", IncomeDate = DateTime.Now },
                new IncomeSummary { Title = "Mentoring", TypeId = 2, CategoryId = 2, Amount = 700, Description = "TBD", IncomeDate = DateTime.Now },
                new IncomeSummary { Title = "Web Development", TypeId = 3, CategoryId = 3, Amount = 900, Description = "TBD", IncomeDate = DateTime.Now },
                new IncomeSummary { Title = "Commission for Referring New Clients", TypeId = 4, CategoryId = 4, Amount = 1500, Description = "TBD", IncomeDate = DateTime.Now },
                new IncomeSummary { Title = "Regular", TypeId = 5, CategoryId = 5, Amount = 2500, Description = "TBD", IncomeDate = DateTime.Now },

                new IncomeSummary { Title = "Article Writing", TypeId = 6, CategoryId = 6, Amount = 3500, Description = "TBD", IncomeDate = DateTime.Now },
                new IncomeSummary { Title = "Article Writing", TypeId = 7, CategoryId = 7, Amount = 4500, Description = "TBD", IncomeDate = DateTime.Now },
            };
        }
        public IEnumerable<AccAccount> GetAccAccountList()
        {
            return new List<AccAccount>
            {
                new AccAccount { AccountName = "Default Account", AccountNumber = "ACC10100001", Credit = 100000, Debit = 0, Balance = 100000, Description = "Default Account"},
                new AccAccount { AccountName = "Test Account 01", AccountNumber = "ACC10100002", Credit = 200000, Debit = 0, Balance = 200000, Description = "Test Account 01"},
                new AccAccount { AccountName = "Test Account 02", AccountNumber = "ACC10100003", Credit = 300000, Debit = 0, Balance = 300000, Description = "Test Account 02"},

                new AccAccount { AccountName = "Test Account 03", AccountNumber = "ACC10100004", Credit = 400000, Debit = 0, Balance = 400000, Description = "Test Account 03"},
                new AccAccount { AccountName = "Test Account 04", AccountNumber = "ACC10100005", Credit = 500000, Debit = 0, Balance = 500000, Description = "Test Account 04"},
                new AccAccount { AccountName = "Test Account 05", AccountNumber = "ACC10100006", Credit = 600000, Debit = 0, Balance = 600000, Description = "Test Account 05"},

                new AccAccount { AccountName = "Test Account 06", AccountNumber = "ACC10100007", Credit = 700000, Debit = 0, Balance = 700000, Description = "Test Account 06"},
            };
        }
        public CompanyInfo GetCompanyInfo()
        {
            return new CompanyInfo
            {
                Name = "XYZ Company Limited",
                Logo = "/upload/company_logo.png",
                InvoiceNoPrefix = "INV",
                QuoteNoPrefix = "QTO",
                Address = "Washington DC, USA",
                City = "Washington DC",
                Country = "USA",
                Phone = "132546789",
                Fax = "123",
                Website = "www.wyx.com",

                ShopNo = "S123",
                StreetName = "ST234",
                PostCode = "P123",
                Office = "123456789",
                Mobile = "123456789",
                HostName = "123456789",
                TermsAndCondition = "Terms and Conditions – General Site Usage Last Revised: December 16, 2013 Welcome to www.lorem-ipsum.info. This site is provided as a service to our visitors and may be used for informational purposes only. Because the Terms and Conditions contain legal obligations, please read them carefully. 1. YOUR AGREEMENT By using this Site, you agree to be bound by, and to comply with, these Terms and Conditions. If you do not agree to these Terms and Conditions, please do not use this site. PLEASE NOTE: We reserve the right, at our sole discretion, to change, modify or otherwise alter these Terms and Conditions at any time. Unless otherwise indicated, amendments will become effective immediately. Please review these Terms and Conditions periodically. Your continued use of the Site following the posting of changes and/or modifications will constitute your acceptance of the revised Terms and Conditions and the reasonableness of these standards for notice of changes. For your information, this page was last updated as of the date at the top of these terms and conditions. 2. PRIVACY Please review our Privacy Policy, which also governs your visit to this Site, to understand our practices. 3. LINKED SITES This Site may contain links to other independent third-party Web sites ('Linked Sites'). These Linked Sites are provided solely as a convenience to our visitors. Such Linked Sites are not under our control, and we are not responsible for and does not endorse the content of such Linked Sites, including any information or materials contained on such Linked Sites. You will need to make your own independent judgment regarding your interaction with these Linked Sites.",
                TermsAndConditionItemSale = "TBD",
                CompanyNumber = "N123456",

                VatNumber = "V123456",
                CardPercentage = "C123456",
                IsVat = true,
                IsItemDiscountPercentage = true,
                VatTitle = "VAT Amount",
                InvoiceRightMarginPercentage = 0,
                CurrencyId = 1,
                ItemVatPercentageId = 6,
                DefaultSMTPId = 1,
            };
        }
    }
}
