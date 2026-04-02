using System.Linq.Expressions;
using BusinessERP.Controllers;
using BusinessERP.Data;
using BusinessERP.Helpers;
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
using Microsoft.EntityFrameworkCore;
using UAParser;

namespace BusinessERP.Services
{
    public class Common : ICommon
    {
        private readonly IWebHostEnvironment _iHostingEnvironment;
        private readonly ApplicationDbContext _context;
        public Common(IWebHostEnvironment iHostingEnvironment,
            ApplicationDbContext context)
        {
            _iHostingEnvironment = iHostingEnvironment;
            _context = context;
        }
        public string UploadedFile(IFormFile _IFormFile)
        {
            try
            {
                string FileName = null;
                if (_IFormFile != null)
                {
                    string _FileServerDir = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload");
                    if (_FileServerDir.Contains("\n"))
                    {
                        _FileServerDir.Replace("\n", "/");
                    }

                    if (_IFormFile.FileName == null)
                        FileName = Guid.NewGuid().ToString() + "_" + "blank-person.png";
                    else
                    {
                        var _FileName = StaticData.ReplaceSpecialCharacters(_IFormFile.FileName);
                        FileName = Guid.NewGuid().ToString() + "_" + _FileName;
                    }

                    string filePath = Path.Combine(_FileServerDir, FileName);
                    //using (var fileStream = new FileStream(filePath, FileMode.Create))
                    using (var fileStream = new FileStream(Path.Combine(_FileServerDir, FileName), FileMode.Create))
                    {
                        _IFormFile.CopyTo(fileStream);
                    }
                }
                return FileName;
            }
            catch (Exception ex)
            {
                Syslog.Write(Syslog.Level.Warning, "BusinessERP", ex.Message);
                throw;
            }
        }
        public string UploadedFileByReplace(IFormFile _IFormFile)
        {
            try
            {
                string FileName = null;
                if (_IFormFile != null)
                {
                    string _FileServerDir = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload");
                    if (_FileServerDir.Contains("\n"))
                    {
                        _FileServerDir.Replace("\n", "/");
                    }

                    FileName = "company_logo.png";
                    string filePath = Path.Combine(_FileServerDir, FileName);
                    using var _FileStream = new FileStream(filePath, FileMode.Create);
                    _IFormFile.CopyTo(_FileStream);
                }
                return FileName;
            }
            catch (Exception ex)
            {
                Syslog.Write(Syslog.Level.Warning, "BusinessERP", ex.Message);
                throw;
            }
        }
        public async Task<EmailConfig> GetEmailConfig()
        {
            return await _context.Set<EmailConfig>().Where(x => x.IsDefault == true).FirstOrDefaultAsync();
        }

        public UserProfile GetByUserProfile(Int64 id)
        {
            var _UserProfile = _context.UserProfile.Where(x => x.UserProfileId == id).FirstOrDefault();
            return _UserProfile;
        }
        public async Task<UserProfileCRUDViewModel> GetByUserProfileInfo(Int64 id)
        {
            UserProfileCRUDViewModel _UserProfile = await _context.UserProfile.Where(x => x.UserProfileId == id).FirstOrDefaultAsync();
            var _Branch = _context.Branch.Where(x => x.Id == _UserProfile.BranchId).FirstOrDefault();
            _UserProfile.BranchDisplay = _Branch.Name;
            return _UserProfile;
        }
        public async Task<bool> InsertLoginHistory(LoginHistory _LoginHistory, ClientInfo _ClientInfo)
        {
            try
            {
                _LoginHistory.PublicIP = await GetPublicIP();
                _LoginHistory.CreatedDate = DateTime.Now;
                _LoginHistory.ModifiedDate = DateTime.Now;

                _context.Add(_LoginHistory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<string> GetPublicIP()
        {
            try
            {
                string url = "http://checkip.dyndns.org/";
                var _HttpClient = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
                var _GetAsync = await _HttpClient.GetAsync(url);
                var _Stream = await _GetAsync.Content.ReadAsStreamAsync();
                StreamReader _StreamReader = new StreamReader(_Stream);
                string result = _StreamReader.ReadToEnd();

                string[] a = result.Split(':');
                string a2 = a[1].Substring(1);
                string[] a3 = a2.Split('<');
                string a4 = a3[0];
                return a4;
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw new Exception("No network adapters with an IPv4 address in the system!");
            }
        }
        public async Task CurrentItemsUpdate(ItemTranViewModel _ItemTranViewModel)
        {
            int _OldQuantity = 0;
            int _NewQuantity = 0;
            ItemsHistoryCRUDViewModel _ItemsHistoryCRUDViewModel = new();
            Items _Items = new();

            _Items = _context.Items.Where(or => or.Id == _ItemTranViewModel.ItemId).FirstOrDefault();

            _Items.ModifiedDate = DateTime.Now;
            _Items.ModifiedBy = _ItemTranViewModel.CurrentUserName;
            ItemsCRUDViewModel vm = _Items;
            _ItemsHistoryCRUDViewModel = vm;
            _ItemsHistoryCRUDViewModel.Action = _ItemTranViewModel.ActionMessage;

            if (_ItemTranViewModel.IsAddition)
            {
                _OldQuantity = _Items.Quantity;
                _NewQuantity = _Items.Quantity + _ItemTranViewModel.TranQuantity;
                _Items.Quantity = _Items.Quantity + _ItemTranViewModel.TranQuantity;
            }
            else
            {
                _OldQuantity = _Items.Quantity;
                _NewQuantity = _Items.Quantity - _ItemTranViewModel.TranQuantity;
                _Items.Quantity = _Items.Quantity - _ItemTranViewModel.TranQuantity;
            }

            _context.Update(_Items);
            await _context.SaveChangesAsync();

            _ItemsHistoryCRUDViewModel.TranQuantity = _ItemTranViewModel.TranQuantity;
            _ItemsHistoryCRUDViewModel.OldQuantity = _OldQuantity;
            _ItemsHistoryCRUDViewModel.NewQuantity = _NewQuantity;
            _ItemsHistoryCRUDViewModel.CreatedDate = DateTime.Now;
            _ItemsHistoryCRUDViewModel.CreatedBy = _ItemTranViewModel.CurrentUserName;
            _context.ItemsHistory.Add(_ItemsHistoryCRUDViewModel);
            await _context.SaveChangesAsync();
        }
        public IQueryable<ItemDropdownListViewModel> GetCommonddlData(string strTableName)
        {
            var sql = "select Id, Name from " + strTableName + " where Cancelled = 0";
            var result = _context.ItemDropdownListViewModel.FromSqlRaw(sql);
            return result;
        }
        public IEnumerable<T> GetTableData<T>(Expression<Func<T, bool>> condition) where T : class
        {
            var result = _context.Set<T>().Where(condition);
            return result;
        }

        public IQueryable<ItemDropdownListViewModel> LoadddlInventoryItem(bool IsVat)
        {
            var resultWithVat = (from _Items in _context.Items
                                 where _Items.Cancelled == false && _Items.Quantity > 0
                                 select new ItemDropdownListViewModel
                                 {
                                     Id = _Items.Id,
                                     Name = _Items.Name
                                     + ": Cost: " + _Items.CostPrice
                                     + ": Normal: " + _Items.NormalPrice
                                     + ": Trade: " + _Items.TradePrice
                                     + ": Premium: " + _Items.PremiumPrice
                                     + ": VAT(%):" + _Items.VatPercentage
                                     + " : Avial: " + _Items.Quantity
                                 }).OrderByDescending(x => x.Id);

            var resultWithoutVat = (from _Items in _context.Items
                                    where _Items.Cancelled == false && _Items.Quantity > 0
                                    select new ItemDropdownListViewModel
                                    {
                                        Id = _Items.Id,
                                        Name = _Items.Name
                                        + ": Cost: " + _Items.CostPrice
                                        + ": Normal: " + _Items.NormalPrice
                                        + ": Trade: " + _Items.TradePrice
                                        + ": Premium: " + _Items.PremiumPrice
                                        + " : Avial: " + _Items.Quantity
                                    }).OrderByDescending(x => x.Id);

            if (IsVat)
                return resultWithVat;
            else
                return resultWithoutVat;
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlCustomerInfo()
        {
            return (from tblObj in _context.CustomerInfo.Where(x => x.Cancelled == false && x.Id != 1).OrderByDescending(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Name + ", Cell: " + tblObj.Phone,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlPaymentType()
        {
            return (from tblObj in _context.PaymentType.Where(x => x.Cancelled == false && x.Id != 1).OrderByDescending(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Name,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlCurrencyItem()
        {
            var result = from _Currency in _context.Currency.Where(x => x.Cancelled == false).OrderByDescending(x => x.IsDefault)
                         select new ItemDropdownListViewModel
                         {
                             Id = _Currency.Id,
                             Name = _Currency.Name + " <> " + _Currency.Code + " <> " + _Currency.Symbol,
                         };

            return result;
        }

        public IQueryable<ItemDropdownListViewModel> LoadddlWarehouse()
        {

            return (from _Warehouse in _context.Warehouse.Where(x => x.Cancelled == false).OrderByDescending(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = _Warehouse.Id,
                        Name = _Warehouse.Name,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlSupplier()
        {

            return (from _Supplier in _context.Supplier.Where(x => x.Cancelled == false).OrderByDescending(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = _Supplier.Id,
                        Name = _Supplier.Name,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlDepartment()
        {
            return (from tblObj in _context.Department.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Name,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlSubDepartment()
        {
            return (from tblObj in _context.SubDepartment.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Name,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlEmployee()
        {
            var result = (from tblObj in _context.Employee.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                          select new ItemDropdownListViewModel
                          {
                              Id = tblObj.Id,
                              Name = tblObj.FirstName + " " + tblObj.LastName,
                          });
            return result;
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlDesignation()
        {
            var result = (from tblObj in _context.Designation.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                          select new ItemDropdownListViewModel
                          {
                              Id = tblObj.Id,
                              Name = tblObj.Name,
                          });
            return result;
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlCategories()
        {
            var result = (from tblObj in _context.Categories.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                          select new ItemDropdownListViewModel
                          {
                              Id = tblObj.Id,
                              Name = tblObj.Name,
                          });
            return result;
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlExpenseType()
        {
            var result = (from tblObj in _context.ExpenseType.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                          select new ItemDropdownListViewModel
                          {
                              Id = tblObj.Id,
                              Name = tblObj.Name,
                          });
            return result;
        }


        public async Task<ItemsHistoryCRUDViewModel> AddItemHistory(ItemsHistoryCRUDViewModel vm)
        {
            try
            {
                ItemsHistory _ItemsHistory = new ItemsHistory();
                _ItemsHistory = vm;
                _ItemsHistory.CreatedDate = DateTime.Now;
                _ItemsHistory.ModifiedDate = DateTime.Now;
                _ItemsHistory.CreatedBy = vm.UserName;
                _ItemsHistory.ModifiedBy = vm.UserName;
                _context.Add(_ItemsHistory);
                await _context.SaveChangesAsync();
                return _ItemsHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ItemsCRUDViewModel GetViewItemById(Int64 Id)
        {
            try
            {
                var result = (from _Items in _context.Items
                              join _Categories in _context.Categories on _Items.CategoriesId equals _Categories.Id
                              into _Categories
                              from listCategories in _Categories.DefaultIfEmpty()
                              join _Supplier in _context.Supplier on _Items.SupplierId equals _Supplier.Id
                              into _Supplier
                              from listSupplier in _Supplier.DefaultIfEmpty()
                              join _Warehouse in _context.Warehouse on _Items.WarehouseId equals _Warehouse.Id
                              into _Warehouse
                              from listWarehouse in _Warehouse.DefaultIfEmpty()
                              join _UnitsofMeasure in _context.UnitsofMeasure on _Items.MeasureId equals _UnitsofMeasure.Id
                              into _UnitsofMeasure
                              from listUnitsofMeasure in _UnitsofMeasure.DefaultIfEmpty()

                              where _Items.Cancelled == false && _Items.Id == Id
                              select new ItemsCRUDViewModel
                              {
                                  Id = _Items.Id,
                                  Code = _Items.Code,
                                  Name = _Items.Name,

                                  CostPrice = _Items.CostPrice,
                                  NormalPrice = _Items.NormalPrice,
                                  TradePrice = _Items.TradePrice,
                                  PremiumPrice = _Items.PremiumPrice,
                                  OtherPrice = _Items.OtherPrice,

                                  VatPercentage = _Items.VatPercentage,
                                  CostVAT = _Items.CostVAT,
                                  NormalVAT = _Items.NormalVAT,
                                  TradeVAT = _Items.TradeVAT,
                                  PremiumVAT = _Items.PremiumVAT,
                                  OtherVAT = _Items.OtherVAT,

                                  OldUnitPrice = _Items.OldUnitPrice,
                                  OldSellPrice = _Items.OldSellPrice,


                                  Quantity = _Items.Quantity,
                                  CategoriesId = _Items.CategoriesId,
                                  CategoriesDisplay = listCategories.Name,
                                  WarehouseId = _Items.WarehouseId,
                                  WarehouseDisplay = listWarehouse.Name,
                                  SupplierId = _Items.SupplierId,
                                  SupplierDisplay = listSupplier.Name,
                                  MeasureId = _Items.MeasureId,
                                  MeasureDisplay = listUnitsofMeasure.Name,
                                  MeasureValue = _Items.MeasureValue,

                                  Note = _Items.Note,
                                  UpdateQntType = _Items.UpdateQntType,
                                  UpdateQntNote = _Items.UpdateQntNote,
                                  StockKeepingUnit = _Items.StockKeepingUnit,
                                  ManufactureDate = _Items.ManufactureDate,
                                  ExpirationDate = _Items.ExpirationDate,
                                  Barcode = _Items.Barcode,
                                  ImageURL = _Items.ImageURL,
                                  Size = _Items.Size,

                                  CreatedDate = _Items.CreatedDate,
                                  ModifiedDate = _Items.ModifiedDate,
                                  CreatedBy = _Items.CreatedBy,
                                  ModifiedBy = _Items.ModifiedBy,
                                  Cancelled = _Items.Cancelled
                              }).OrderByDescending(x => x.Id).FirstOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<ItemCartViewModel> GetAllCartItem()
        {
            try
            {
                var result = (from _Item in _context.Items
                              where _Item.Cancelled == false && _Item.Quantity > 0
                              select new ItemCartViewModel
                              {
                                  Id = _Item.Id,
                                  Name = _Item.Name.Length < 20 ? _Item.Name : _Item.Name.Substring(0, 20) + "..",
                                  ImageURL = _Item.ImageURL,
                                  SellPrice = _Item.NormalPrice,
                                  VatPercentage = _Item.VatPercentage,
                                  NormalVAT = _Item.NormalVAT,
                                  Quantity = _Item.Quantity,
                              }).OrderByDescending(x => x.Id);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<List<ItemCartViewModel>> GetAllCartItemForCustomDT()
        {
            try
            {
                var result = (from _Item in _context.Items
                              where _Item.Cancelled == false && _Item.Quantity > 0
                              select new ItemCartViewModel
                              {
                                  Id = _Item.Id,
                                  Name = _Item.Name.Length < 20 ? _Item.Name : _Item.Name.Substring(0, 20) + "..",
                                  Barcode = _Item.Code,
                                  ImageURL = _Item.ImageURL,
                                  SellPrice = _Item.NormalPrice,
                                  Quantity = _Item.Quantity,
                              }).OrderByDescending(x => x.Id).ToList();

                var _listBarcodeViewModel = result.Select((x, i) => new { Index = i, Value = x })
                    .GroupBy(x => x.Index / 4)
                    .Select(x => x.Select(v => v.Value).ToList()).ToList();

                var lastItem = _listBarcodeViewModel.LastOrDefault();
                if (lastItem.Count > 0)
                {
                    _listBarcodeViewModel.Remove(lastItem);
                    ItemCartViewModel _ItemCartViewModel = new();
                    _ItemCartViewModel.Name = "Empty";
                    _ItemCartViewModel.Barcode = "Empty";
                    _ItemCartViewModel.ImageURL = "/upload/blank-item.png";
                    if (lastItem.Count < 2)
                    {
                        lastItem.Add(_ItemCartViewModel);
                        lastItem.Add(_ItemCartViewModel);
                        lastItem.Add(_ItemCartViewModel);
                    }
                    else if (lastItem.Count < 3)
                    {
                        lastItem.Add(_ItemCartViewModel);
                        lastItem.Add(_ItemCartViewModel);
                    }
                    else if (lastItem.Count < 4)
                    {
                        lastItem.Add(_ItemCartViewModel);
                    }
                    _listBarcodeViewModel.Add(lastItem);
                }

                return _listBarcodeViewModel.AsQueryable();
            }
            catch (Exception) { throw; }
        }

        public IQueryable<List<ItemCartViewModel>> GetItemCartDataList()
        {
            try
            {
                var result = (from _Item in _context.Items
                              join _Categories in _context.Categories on _Item.CategoriesId equals _Categories.Id
                              into _Categories
                              from listCategories in _Categories.DefaultIfEmpty()
                              where _Item.Cancelled == false && _Item.Quantity > 0
                              select new ItemCartViewModel
                              {
                                  Id = _Item.Id,
                                  CategoriesId = _Item.CategoriesId,
                                  CategoriesName = listCategories.Name == null ? "Common" : listCategories.Name,
                                  //Name = _Item.Name,
                                  Name = _Item.Name.Length < 20 ? _Item.Name : _Item.Name.Substring(0, 20) + "..",
                                  Barcode = _Item.Code,
                                  ImageURL = _Item.ImageURL,
                                  SellPrice = _Item.NormalPrice,
                                  Quantity = _Item.Quantity,
                              }).OrderByDescending(x => x.Id).ToList();

                var _listBarcodeViewModel = result.Select((x, i) => new { Index = i, Value = x })
                    .GroupBy(x => x.Index / 3)
                    .Select(x => x.Select(v => v.Value).ToList()).ToList();

                var lastItem = _listBarcodeViewModel.LastOrDefault();
                if (lastItem.Count > 0)
                {
                    _listBarcodeViewModel.Remove(lastItem);
                    ItemCartViewModel _ItemCartViewModel = new();
                    _ItemCartViewModel.Name = "Empty";
                    _ItemCartViewModel.Barcode = "Empty";
                    _ItemCartViewModel.CategoriesName = "Empty";
                    _ItemCartViewModel.ImageURL = "/upload/blank-item.png";
                    if (lastItem.Count < 2)
                    {
                        lastItem.Add(_ItemCartViewModel);
                        lastItem.Add(_ItemCartViewModel);
                    }
                    else if (lastItem.Count < 3)
                    {
                        lastItem.Add(_ItemCartViewModel);
                    }
                    _listBarcodeViewModel.Add(lastItem);
                }
                return _listBarcodeViewModel.AsQueryable();
            }
            catch (Exception) { throw; }
        }
        public IQueryable<AttendanceCRUDViewModel> GetAttendanceReportData()
        {
            try
            {
                return (from _Attendance in _context.Attendance
                        join _Employee in _context.Employee on _Attendance.EmployeeId equals _Employee.Id
                        where _Attendance.Cancelled == false
                        select new AttendanceCRUDViewModel
                        {
                            Id = _Attendance.Id,
                            EmployeeId = _Attendance.EmployeeId,
                            EmployeeName = _Employee.FirstName + " " + _Employee.LastName,
                            CheckIn = _Attendance.CheckIn,
                            CheckInDisplay = String.Format("{0:f}", _Attendance.CheckIn),
                            CheckOut = _Attendance.CheckOut,
                            CheckOutDisplay = String.Format("{0:f}", _Attendance.CheckOut),
                            StayTime = _Attendance.StayTime,
                            CreatedDate = _Attendance.CreatedDate,
                            ModifiedDate = _Attendance.ModifiedDate,
                            CreatedBy = _Attendance.CreatedBy,
                            ModifiedBy = _Attendance.ModifiedBy

                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<ItemGridViewModel> GetJoinDataItemsAndTranDetails()
        {
            try
            {
                var result = _context.ItemGridViewModel.FromSqlRaw(@"select A.Id,A.Code, A.Name, C.Name SupplierName, D.Name MeasureName, A.StockKeepingUnit, A.CostPrice,A.NormalPrice,A.Quantity,B.TranQuantity TotalSold,
                (B.TranQuantity)*(A.CostPrice) TotalEarned,  
                A.CreatedDate,A.ImageURL from Items A left join (SELECT ItemId, sum(Quantity)TranQuantity 
                FROM PaymentDetail GROUP BY ItemId) B ON A.Id = B.ItemId 
                full join Supplier C on A.SupplierId=c.Id
                full join UnitsofMeasure D on A.MeasureId=D.Id
                where A.Cancelled=0");
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GenerateDemoSales()
        {
            try
            {
                string _FileServerDir = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/SQLScript/");
                var sqlPayment = System.IO.File.ReadAllText(_FileServerDir + "tblPayment.sql");
                var result1 = _context.Database.ExecuteSqlRaw(sqlPayment);

                var sqlPaymentDetail = System.IO.File.ReadAllText(_FileServerDir + "tblPaymentDetail.sql");
                var result2 = _context.Database.ExecuteSqlRaw(sqlPaymentDetail);

                var sqlcommon = System.IO.File.ReadAllText(_FileServerDir + "tblcommon.sql");
                var result3 = _context.Database.ExecuteSqlRaw(sqlcommon);

                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }
        public IQueryable<ItemsCRUDViewModel> GetItemsGridList()
        {
            try
            {
                var result = (from _Items in _context.Items
                              join _Categories in _context.Categories on _Items.CategoriesId equals _Categories.Id
                              into _Categories
                              from listCategories in _Categories.DefaultIfEmpty()
                              join _Supplier in _context.Supplier on _Items.SupplierId equals _Supplier.Id
                              into _Supplier
                              from listSupplier in _Supplier.DefaultIfEmpty()
                              join _UnitsofMeasure in _context.UnitsofMeasure on _Items.MeasureId equals _UnitsofMeasure.Id
                              into _UnitsofMeasure
                              from listUnitsofMeasure in _UnitsofMeasure.DefaultIfEmpty()
                              where _Items.Cancelled == false
                              select new ItemsCRUDViewModel
                              {
                                  Id = _Items.Id,
                                  ImageURL = _Items.ImageURL,
                                  Name = _Items.Name,
                                  Code = _Items.Code,
                                  StockKeepingUnit = _Items.StockKeepingUnit,
                                  CategoriesDisplay = listCategories.Name,
                                  SupplierDisplay = listSupplier.Name,
                                  MeasureDisplay = listUnitsofMeasure.Name,
                                  CostPrice = _Items.CostPrice,
                                  NormalPrice = _Items.NormalPrice,
                                  Quantity = _Items.Quantity,
                                  CreatedDate = _Items.CreatedDate,
                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ExpenseByViewModel> ExpenseByDate(string DateType)
        {
            List<ExpenseByViewModel> list = new();
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            DateTime[] DateList = null;

            var _ExpenseSummary = _context.ExpenseSummary.Where(x => x.Cancelled == false);
            var _ExpenseDetails = _context.ExpenseDetails.Where(x => x.Cancelled == false);

            if (DateType == "Day")
            {
                DateList = Enumerable.Range(0, 30).Select(i => DateTime.Now.Date.AddDays(-i)).ToArray();
            }
            else if (DateType == "Month")
            {
                DateTime _DateTimeMonth = DateTime.Now.AddMonths(1);
                DateList = Enumerable.Range(1, 12).Select(n => _DateTimeMonth.AddMonths(-n)).ToArray();
            }
            else if (DateType == "Year")
            {
                DateTime _DateTime = DateTime.Now.AddYears(1);
                DateList = Enumerable.Range(1, 10).Select(n => _DateTime.AddYears(-n)).ToArray();
            }


            int SL = 1;
            foreach (var item in DateList)
            {
                ExpenseByViewModel vm = new();

                vm.Id = SL;
                if (DateType == "Day")
                {
                    startDate = item;
                    endDate = item.AddDays(1).AddTicks(-1); ;
                    vm.TranDate = item.ToString("dddd, dd MMMM yyyy");
                }
                else if (DateType == "Month")
                {
                    startDate = new DateTime(item.Year, item.Month, 1);
                    endDate = startDate.AddMonths(1).AddTicks(-1);
                    vm.TranDate = item.ToString("MMMM") + "-" + item.Year;
                }
                else if (DateType == "Year")
                {
                    startDate = new DateTime(item.Year, 1, 1);
                    endDate = new DateTime(item.Year, 12, 31);
                    vm.TranDate = item.Year.ToString();
                }

                var SingleDayPayment = _ExpenseSummary.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate);

                vm.TotalTran = SingleDayPayment.Count();
                vm.TotalQuantity = _ExpenseDetails.Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate).Sum(x => x.Quantity);
                vm.TotalExpense = SingleDayPayment.Sum(x => x.GrandTotal);
                vm.TotalPaid = Math.Round(SingleDayPayment.Sum(x => x.PaidAmount), 2);
                vm.TotalDue = Math.Round(SingleDayPayment.Sum(x => x.DueAmount), 2);

                list.Add(vm);
                SL++;
            }
            return list;
        }
        public List<GroupByViewModel> GetItemDemandList()
        {
            var PaymentDetailGroupBy = _context.PaymentDetail.Where(x => x.Cancelled == false).GroupBy(p => p.ItemId).Select(g => new
            {
                ItemId = g.Key,
                TranQuantity = g.Sum(t => t.Quantity),
            }).ToList();

            var result = (from _PaymentDetail in PaymentDetailGroupBy
                          join _Items in _context.Items on _PaymentDetail.ItemId equals _Items.Id
                          where _PaymentDetail.ItemId == _Items.Id
                          select new GroupByViewModel
                          {
                              ItemName = _Items.Id + "-" + _Items.Name,
                              ItemTotal = _PaymentDetail.TranQuantity
                          }).ToList();

            return result;
        }
        public List<GroupByViewModel> GetItemEarningList()
        {
            var PaymentDetailGroupBy = _context.PaymentDetail.Where(x => x.Cancelled == false).GroupBy(p => p.ItemId).Select(g => new
            {
                ItemId = g.Key,
                TranQuantity = g.Sum(t => t.Quantity)
            }).ToList();

            var result = (from _TranDetails in PaymentDetailGroupBy
                          join _Items in _context.Items
                          on _TranDetails.ItemId equals _Items.Id
                          where (_TranDetails.ItemId == _Items.Id)
                          select new GroupByViewModel
                          {
                              ItemName = _Items.Id + "-" + _Items.Name,
                              ItemTotal = _TranDetails.TranQuantity * _Items.NormalPrice,
                          }).ToList();

            return result;
        }
        public IQueryable<BarcodeViewModel> GetBarcodeList()
        {
            var result = (from _Items in _context.Items
                          where _Items.Cancelled == false
                          select new BarcodeViewModel
                          {
                              Id = _Items.Id,
                              ItemName = _Items.Name,
                              Barcode = _Items.Barcode,
                          }).OrderBy(x => x.Id);
            return result;
        }
        public async Task<CompanyInfoCRUDViewModel> GetCompanyInfo()
        {
            CompanyInfoCRUDViewModel vm = await _context.CompanyInfo.FirstOrDefaultAsync(m => m.Id == 1);

            var _Currency = await _context.Currency.Where(m => m.Id == vm.CurrencyId).FirstOrDefaultAsync();
            if (_Currency != null)
                vm.CurrencyDisplay = _Currency.Name + " (" + _Currency.Symbol + ")";

            var _VatPercentage = await _context.VatPercentage.Where(m => m.Id == vm.ItemVatPercentageId).FirstOrDefaultAsync();
            if (_VatPercentage != null)
                vm.ItemVatPercentageDisplay = _VatPercentage.Name;

            var _EmailConfig = await _context.EmailConfig.Where(m => m.Id == vm.DefaultSMTPId).FirstOrDefaultAsync();
            if (_EmailConfig != null)
                vm.DefaultSMTPDisplay = _EmailConfig.Email;
            return vm;
        }
        public IQueryable<ItemDropdownListViewModel> GetddlEmailConfig()
        {
            return (from tblObj in _context.EmailConfig.Where(x => x.Cancelled == false)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Email
                    }).OrderBy(x => x.Id);
        }
        public IQueryable<ItemDropdownListViewModel> GetddlCustomerEmail()
        {
            return (from tblObj in _context.CustomerInfo.Where(x => x.Cancelled == false)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Email
                    }).OrderByDescending(x => x.Id);
        }
        public IQueryable<ItemDropdownListViewModel> GetddlCustomerType()
        {
            return (from tblObj in _context.CustomerType.Where(x => x.Cancelled == false)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Name
                    }).OrderByDescending(x => x.Id);
        }
        public IQueryable<ItemDropdownListViewModel> GetddlPaymentStatus()
        {
            return (from tblObj in _context.PaymentStatus.Where(x => x.Cancelled == false)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Name
                    }).OrderByDescending(x => x.Id);
        }
        public IQueryable<CustomerInfoCRUDViewModel> GetCustomerList()
        {
            try
            {
                var result = (from _CustomerInfo in _context.CustomerInfo
                              join _CustomerType in _context.CustomerType on _CustomerInfo.Type equals _CustomerType.Id
                              into _CustomerType
                              from listCustomerType in _CustomerType.DefaultIfEmpty()
                              where _CustomerInfo.Cancelled == false
                              select new CustomerInfoCRUDViewModel
                              {
                                  Id = _CustomerInfo.Id,
                                  Name = _CustomerInfo.Name,
                                  CompanyName = _CustomerInfo.CompanyName,
                                  Type = _CustomerInfo.Type,
                                  TypeDisplay = listCustomerType.Name,
                                  Phone = _CustomerInfo.Phone,
                                  Email = _CustomerInfo.Email,
                                  AccountNo = _CustomerInfo.AccountNo,
                                  Notes = _CustomerInfo.Notes,
                                  Address = _CustomerInfo.Address,
                                  BillingAddress = _CustomerInfo.BillingAddress,
                                  CreatedDate = _CustomerInfo.CreatedDate,
                                  ModifiedDate = _CustomerInfo.ModifiedDate,
                                  CreatedBy = _CustomerInfo.CreatedBy,
                                  ModifiedBy = _CustomerInfo.ModifiedBy,
                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception) { throw; }
        }
        public IQueryable<EmployeeCRUDViewModel> GetEmployeeGridList()
        {
            try
            {
                return (from _Employee in _context.Employee
                        join _Department in _context.Department on _Employee.Department equals _Department.Id
                        join _Designation in _context.Designation on _Employee.Designation equals _Designation.Id
                        where _Employee.Cancelled == false
                        select new EmployeeCRUDViewModel
                        {
                            Id = _Employee.Id,
                            EmployeeId = _Employee.EmployeeId,
                            FirstName = _Employee.FirstName,
                            LastName = _Employee.LastName,
                            DateOfBirth = _Employee.DateOfBirth,
                            Designation = _Employee.Designation,
                            DesignationDisplay = _Designation.Name,
                            Department = _Employee.Department,
                            DepartmentDisplay = _Department.Name,
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<EmployeeCRUDViewModel> GetEmployeeList()
        {
            try
            {
                return (from _Employee in _context.Employee
                        join _Department in _context.Department on _Employee.Department equals _Department.Id
                        //join _SubDepartment in _context.SubDepartment on _Employee.SubDepartment equals _SubDepartment.Id
                        join _Designation in _context.Designation on _Employee.Designation equals _Designation.Id
                        where _Employee.Cancelled == false
                        select new EmployeeCRUDViewModel
                        {
                            Id = _Employee.Id,
                            EmployeeId = _Employee.EmployeeId,
                            FirstName = _Employee.FirstName,
                            LastName = _Employee.LastName,
                            DateOfBirth = _Employee.DateOfBirth,
                            Designation = _Employee.Designation,
                            DesignationDisplay = _Designation.Name,
                            Department = _Employee.Department,
                            DepartmentDisplay = _Department.Name,

                            SubDepartment = _Employee.SubDepartment,
                            //SubDepartmentDisplay = _SubDepartment.Name,
                            JoiningDate = _Employee.JoiningDate,
                            LeavingDate = _Employee.LeavingDate,
                            Phone = _Employee.Phone,
                            Email = _Employee.Email,
                            Address = _Employee.Address,
                            CreatedDate = _Employee.CreatedDate,
                            ModifiedDate = _Employee.ModifiedDate,
                            CreatedBy = _Employee.CreatedBy,
                            ModifiedBy = _Employee.ModifiedBy,
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<ExpenseDetailsCRUDViewModel> GetExpenseDetailsList()
        {
            try
            {
                var _PaymentDetailCRUDViewModel = (from _ExpenseDetails in _context.ExpenseDetails
                                                   where _ExpenseDetails.Cancelled == false
                                                   select new ExpenseDetailsCRUDViewModel
                                                   {
                                                       Id = _ExpenseDetails.Id,
                                                       ExpenseSummaryId = _ExpenseDetails.ExpenseSummaryId,
                                                       ExpenseTypeId = _ExpenseDetails.ExpenseTypeId,
                                                       ExpenseType = _ExpenseDetails.ExpenseType,
                                                       Description = _ExpenseDetails.Description,
                                                       Quantity = _ExpenseDetails.Quantity,
                                                       UnitPrice = _ExpenseDetails.UnitPrice,
                                                       TotalPrice = _ExpenseDetails.TotalPrice,
                                                       CreatedDate = _ExpenseDetails.CreatedDate,
                                                       ModifiedDate = _ExpenseDetails.ModifiedDate,
                                                       CreatedBy = _ExpenseDetails.CreatedBy,
                                                       ModifiedBy = _ExpenseDetails.ModifiedBy,
                                                       Cancelled = _ExpenseDetails.Cancelled,
                                                   }).OrderByDescending(x => x.Id);

                return _PaymentDetailCRUDViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<ExpenseSummaryCRUDViewModel> GetExpenseSummaryGridItem()
        {
            try
            {
                return (from _ExpenseSummary in _context.ExpenseSummary
                        join _Currency in _context.Currency on _ExpenseSummary.CurrencyCode equals _Currency.Id
                        join _Branch in _context.Branch on _ExpenseSummary.BranchId equals _Branch.Id
                        where _ExpenseSummary.Cancelled == false
                        select new ExpenseSummaryCRUDViewModel
                        {
                            Id = _ExpenseSummary.Id,
                            Title = _ExpenseSummary.Title,
                            GrandTotal = _ExpenseSummary.GrandTotal,
                            PaidAmount = _ExpenseSummary.PaidAmount,
                            DueAmount = _ExpenseSummary.DueAmount,
                            CurrencyCode = _ExpenseSummary.CurrencyCode,
                            CurrencyName = _Currency.Name,
                            BranchId = _ExpenseSummary.BranchId,
                            BranchName = _Branch.Name,
                            CreatedDate = _ExpenseSummary.CreatedDate,
                            ModifiedDate = _ExpenseSummary.ModifiedDate,
                            CreatedBy = _ExpenseSummary.CreatedBy,
                            ModifiedBy = _ExpenseSummary.ModifiedBy,
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Int64> GetBranchIdByUserName(string _UserName)
        {
            try
            {
                var _UserProfile = await _context.UserProfile.FirstOrDefaultAsync(x => x.Email == _UserName);

                if (_UserProfile != null)
                    return _UserProfile.BranchId;
                else
                    return 1;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<ManageUserRolesViewModel>> GetManageRoleDetailsList(Int64 id)
        {
            var result = await (from tblObj in _context.ManageUserRolesDetails.Where(x => x.ManageRoleId == id)
                                select new Models.ManageUserRolesVM.ManageUserRolesViewModel
                                {
                                    ManageRoleDetailsId = tblObj.Id,
                                    RoleId = tblObj.RoleId,
                                    RoleName = tblObj.RoleName,
                                    IsAllowed = tblObj.IsAllowed,
                                }).OrderBy(x => x.RoleName).ToListAsync();
            return result;
        }
        public IQueryable<UserProfileCRUDViewModel> GetUserProfileDetails()
        {
            var result = (from vm in _context.UserProfile
                          join _Branch in _context.Branch on vm.BranchId equals _Branch.Id
                          into _Branch
                          from objBranch in _Branch.DefaultIfEmpty()
                          join _ManageRole in _context.ManageUserRoles on vm.RoleId equals _ManageRole.Id
                          into _ManageRole
                          from objManageRole in _ManageRole.DefaultIfEmpty()
                          where vm.Cancelled == false
                          select new UserProfileCRUDViewModel
                          {
                              UserProfileId = vm.UserProfileId,
                              ApplicationUserId = vm.ApplicationUserId,
                              FirstName = vm.FirstName,
                              LastName = vm.LastName,
                              PhoneNumber = vm.PhoneNumber,
                              Email = vm.Email,
                              Address = vm.Address,
                              Country = vm.Country,
                              ProfilePicture = vm.ProfilePicture,
                              BranchId = vm.BranchId,
                              BranchDisplay = objBranch.Name,
                              RoleId = vm.RoleId,
                              RoleIdDisplay = objManageRole.Name,
                              DateOfBirth = vm.DateOfBirth,
                              JoiningDate = vm.JoiningDate,
                              LeavingDate = vm.LeavingDate,

                              CreatedDate = vm.CreatedDate,
                              ModifiedDate = vm.ModifiedDate,
                              CreatedBy = vm.CreatedBy,
                              ModifiedBy = vm.ModifiedBy,
                              Cancelled = vm.Cancelled,
                          }).OrderByDescending(x => x.UserProfileId);
            return result;
        }
        public IQueryable<AccDepositCRUDViewModel> GetAllAccDeposit()
        {
            try
            {
                var result = (from vm in _context.AccDeposit
                              join _AccAccount in _context.AccAccount on vm.AccountId equals _AccAccount.Id
                              where vm.Cancelled == false
                              select new AccDepositCRUDViewModel
                              {
                                  Id = vm.Id,
                                  AccountId = vm.AccountId,
                                  AccountDisplay = _AccAccount.AccountName,
                                  DepositDate = vm.DepositDate,
                                  Amount = vm.Amount,
                                  Note = vm.Note,
                                  CreatedDate = vm.CreatedDate,
                                  ModifiedDate = vm.ModifiedDate,
                                  CreatedBy = vm.CreatedBy,
                                  ModifiedBy = vm.ModifiedBy,
                                  Cancelled = vm.Cancelled,
                              }).OrderByDescending(x => x.Id);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<AccExpenseCRUDViewModel> GetAllAccExpense()
        {
            try
            {
                var result = (from vm in _context.AccExpense
                              join _AccAccount in _context.AccAccount on vm.AccountId equals _AccAccount.Id
                              where vm.Cancelled == false
                              select new AccExpenseCRUDViewModel
                              {
                                  Id = vm.Id,
                                  AccountId = vm.AccountId,
                                  AccountDisplay = _AccAccount.AccountName,
                                  Name = vm.Name,
                                  ExpenseDate = vm.ExpenseDate,
                                  Amount = vm.Amount,
                                  Note = vm.Note,

                                  CreatedDate = vm.CreatedDate,
                                  ModifiedDate = vm.ModifiedDate,
                                  CreatedBy = vm.CreatedBy,
                                  ModifiedBy = vm.ModifiedBy,
                                  Cancelled = vm.Cancelled,
                              }).OrderByDescending(x => x.Id);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<AccTransactionCRUDViewModel> GetAllAccTransaction()
        {
            try
            {
                var result = (from _AccTransaction in _context.AccTransaction
                              join _AccAccount in _context.AccAccount on _AccTransaction.AccountId equals _AccAccount.Id
                              where _AccTransaction.Cancelled == false
                              select new AccTransactionCRUDViewModel
                              {
                                  Id = _AccTransaction.Id,
                                  AccountId = _AccTransaction.AccountId,
                                  AccountDisplay = _AccAccount.AccountName,
                                  Type = _AccTransaction.Type,
                                  Reference = _AccTransaction.Reference,
                                  Credit = _AccTransaction.Credit,
                                  Debit = _AccTransaction.Debit,
                                  Amount = _AccTransaction.Amount,
                                  Description = _AccTransaction.Description,

                                  CreatedDate = _AccTransaction.CreatedDate,
                                  ModifiedDate = _AccTransaction.ModifiedDate,
                                  CreatedBy = _AccTransaction.CreatedBy,
                                  ModifiedBy = _AccTransaction.ModifiedBy,
                                  Cancelled = _AccTransaction.Cancelled,
                              }).OrderByDescending(x => x.Id);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<AccTransferCRUDViewModel> GetAllAccTransfer()
        {
            try
            {
                var result = (from _AccTransfer in _context.AccTransfer
                              join _AccAccountSender in _context.AccAccount on _AccTransfer.SenderId equals _AccAccountSender.Id
                              join _AccAccountReceiver in _context.AccAccount on _AccTransfer.ReceiverId equals _AccAccountReceiver.Id
                              where _AccTransfer.Cancelled == false
                              select new AccTransferCRUDViewModel
                              {
                                  Id = _AccTransfer.Id,
                                  SenderId = _AccTransfer.SenderId,
                                  SenderDisplay = _AccAccountSender.AccountName,
                                  ReceiverId = _AccTransfer.ReceiverId,
                                  ReceiverDisplay = _AccAccountReceiver.AccountName,
                                  TransferDate = _AccTransfer.TransferDate,
                                  Amount = _AccTransfer.Amount,
                                  Note = _AccTransfer.Note,

                                  CreatedDate = _AccTransfer.CreatedDate,
                                  ModifiedDate = _AccTransfer.ModifiedDate,
                                  CreatedBy = _AccTransfer.CreatedBy,
                                  ModifiedBy = _AccTransfer.ModifiedBy,
                                  Cancelled = _AccTransfer.Cancelled,
                              }).OrderByDescending(x => x.Id);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<AccAccount> GetAllAccAccount()
        {
            try
            {
                var result = (from _AccAccount in _context.AccAccount
                              select new AccAccount
                              {
                                  Id = _AccAccount.Id,
                                  AccountName = _AccAccount.AccountName,
                                  AccountNumber = _AccAccount.AccountNumber,
                                  Credit = Math.Round(_AccAccount.Credit, 2),
                                  Debit = Math.Round(_AccAccount.Debit, 2),
                                  Balance = Math.Round(_AccAccount.Balance, 2),
                                  Description = _AccAccount.Description,

                                  CreatedDate = _AccAccount.CreatedDate,
                                  ModifiedDate = _AccAccount.ModifiedDate,
                                  CreatedBy = _AccAccount.CreatedBy,
                                  ModifiedBy = _AccAccount.ModifiedBy,
                                  Cancelled = _AccAccount.Cancelled,
                              }).OrderByDescending(x => x.Id);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> UpdateAccoutDuringTran(UpdateAccountViewModel vm)
        {
            try
            {
                //Update Account
                AccAccount _AccAccount = await _context.AccAccount.Where(x => x.Id == vm.AccAccountNo).FirstOrDefaultAsync();

                AccAccount _AccAccountNew = _AccAccount;
                if (vm.AccUpdateType == AccAccountUpdateType.Credit)
                {
                    _AccAccountNew.Balance = _AccAccountNew.Balance + vm.Amount;
                    _AccAccountNew.Credit = _AccAccountNew.Credit + vm.Amount;
                }
                else if (vm.AccUpdateType == AccAccountUpdateType.CreditRevart)
                {
                    _AccAccountNew.Balance = _AccAccountNew.Balance - vm.Amount;
                    _AccAccountNew.Credit = _AccAccountNew.Credit - vm.Amount;
                }
                else if (vm.AccUpdateType == AccAccountUpdateType.Debit)
                {
                    _AccAccountNew.Balance = _AccAccountNew.Balance - vm.Amount;
                    _AccAccountNew.Debit = _AccAccountNew.Debit + vm.Amount;
                }
                else if (vm.AccUpdateType == AccAccountUpdateType.DebitRevart)
                {
                    _AccAccountNew.Balance = _AccAccountNew.Balance + vm.Amount;
                    _AccAccountNew.Debit = _AccAccountNew.Debit - vm.Amount;
                }

                _AccAccountNew.ModifiedDate = DateTime.Now;
                _AccAccountNew.ModifiedBy = vm.UserName;
                _context.Entry(_AccAccount).CurrentValues.SetValues(_AccAccountNew);
                await _context.SaveChangesAsync();

                //AddAccTransaction
                AccTransactionCRUDViewModel _AccTransactionCRUDViewModel = new()
                {
                    AccountId = _AccAccount.Id,
                    Type = vm.Type,
                    Reference = vm.Reference,
                    Credit = vm.Credit,
                    Debit = vm.Debit,
                    Amount = vm.Amount,
                    Description = vm.Description,
                    UserName = vm.UserName,
                };
                await AddAccTransaction(_AccTransactionCRUDViewModel);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<AccTransaction> AddAccTransaction(AccTransactionCRUDViewModel vm)
        {
            try
            {
                AccTransaction _AccTransaction = vm;
                _AccTransaction.CreatedDate = DateTime.Now;
                _AccTransaction.ModifiedDate = DateTime.Now;
                _AccTransaction.CreatedBy = vm.UserName;
                _AccTransaction.ModifiedBy = vm.UserName;
                _context.Add(_AccTransaction);
                await _context.SaveChangesAsync();
                return _AccTransaction;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}