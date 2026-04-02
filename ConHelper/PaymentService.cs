using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models;
using BusinessERP.Models.AccTransactionViewModel;
using BusinessERP.Models.ExpenseSummaryViewModel;
using BusinessERP.Models.ItemsViewModel;
using BusinessERP.Models.PaymentDetailViewModel;
using BusinessERP.Models.PaymentModeHistoryViewModel;
using BusinessERP.Models.PaymentViewModel;
using BusinessERP.Models.PurchasesPaymentDetailViewModel;
using BusinessERP.Models.PurchasesPaymentViewModel;
using BusinessERP.Models.ReturnLogViewModel;
using BusinessERP.Models.SendEmailHistoryViewModel;
using BusinessERP.Services;
using Microsoft.EntityFrameworkCore;

namespace BusinessERP.ConHelper
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        public PaymentService(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        public async Task<Payment> CreatePayment(AddPaymentViewModel _AddPaymentViewModel, string strUserName)
        {
            var _CurrencyId = _context.Currency.Where(x => x.IsDefault == true).FirstOrDefault().Id;
            Payment _Payments = new Payment
            {
                CustomerId = _AddPaymentViewModel.CustomerId,
                CurrencyId = _CurrencyId,

                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CreatedBy = strUserName,
                ModifiedBy = strUserName
            };
            _context.Add(_Payments);
            await _context.SaveChangesAsync();
            return _Payments;
        }
        public async Task<PaymentCRUDViewModel> UpdatePayment(PaymentCRUDViewModel vm, bool IsCustomerInfo)
        {
            var _Payment = await _context.Payment.FindAsync(vm.Id);
            var _PaymentDetail = _context.PaymentDetail.Where(x => x.PaymentId == vm.Id && x.Cancelled == false).ToList();
            var _PaymentModeHistory = _context.PaymentModeHistory.Where(x => x.PaymentId == vm.Id && x.Cancelled == false).ToList();

            var _ItemDiscountAmount = (double)_PaymentDetail.Sum(x => x.ItemDiscountAmount);
            var _ItemVATAmount = (double)_PaymentDetail.Sum(x => x.ItemVATAmount);
            var _TotalAmount = (double)_PaymentDetail.Sum(x => x.TotalAmount);

            vm.DiscountAmount = Math.Round(_ItemDiscountAmount, 2);
            vm.VATAmount = Math.Round(_ItemVATAmount, 2);

            vm.SubTotal = Math.Round(((_TotalAmount + _ItemDiscountAmount) - _ItemVATAmount), 2);
            vm.GrandTotal = Math.Round((double)(_TotalAmount + vm.CommonCharge), 2);

            vm.PaidAmount = Math.Round((double)_PaymentModeHistory.Sum(x => x.Amount), 2);
            if (vm.PaidAmount >= vm.GrandTotal)
            {
                vm.DueAmount = 0;
                vm.PaymentStatus = PaymentStatusInfo.Paid;
                vm.ChangedAmount = Math.Round((double)(vm.PaidAmount - vm.GrandTotal), 2);
            }
            else
            {
                vm.DueAmount = Math.Round((double)(vm.GrandTotal - vm.PaidAmount), 2);
                vm.PaymentStatus = PaymentStatusInfo.Unpaid;
                vm.ChangedAmount = 0;
            }

            vm.ModifiedDate = DateTime.Now;
            vm.ModifiedBy = vm.UserName;
            _context.Entry(_Payment).CurrentValues.SetValues(vm);
            await _context.SaveChangesAsync();

            if (IsCustomerInfo)
            {
                var _CustomerInfoInfo = await _context.CustomerInfo.FindAsync(vm.CustomerId);
                _CustomerInfoInfo.Notes = vm.CustomerNote;
                _CustomerInfoInfo.CreatedDate = _CustomerInfoInfo.CreatedDate;
                _CustomerInfoInfo.CreatedBy = _CustomerInfoInfo.CreatedBy;
                _CustomerInfoInfo.ModifiedDate = DateTime.Now;
                _CustomerInfoInfo.ModifiedBy = vm.UserName;
                _context.Update(_CustomerInfoInfo);
                await _context.SaveChangesAsync();
            }

            return vm;
        }
        public async Task<PaymentDetail> CreatePaymentsDetail(PaymentDetail _PaymentDetail, string UserName)
        {
            var _CompanyInfo = await _context.CompanyInfo.FirstOrDefaultAsync(m => m.Id == 1);
            if (_CompanyInfo.IsItemDiscountPercentage)
            {
                _PaymentDetail.ItemDiscountAmount = Math.Round((double)((_PaymentDetail.ItemDiscount / 100) * _PaymentDetail.UnitPrice), 2);
            }

            double? _DiscountedUnitPrice = _PaymentDetail.UnitPrice - _PaymentDetail.ItemDiscountAmount;
            double? _DiscountedTotalPrice = _DiscountedUnitPrice * _PaymentDetail.Quantity;
            double? VatUnitAmount = Math.Round((double)((_PaymentDetail.ItemVAT / 100) * _DiscountedUnitPrice), 2);

            _PaymentDetail.ItemVATAmount = VatUnitAmount * _PaymentDetail.Quantity;
            _PaymentDetail.TotalAmount = _DiscountedTotalPrice + _PaymentDetail.ItemVATAmount;

            _PaymentDetail.CreatedDate = DateTime.Now;
            _PaymentDetail.ModifiedDate = DateTime.Now;
            _PaymentDetail.CreatedBy = UserName;
            _PaymentDetail.ModifiedBy = UserName;
            _context.Add(_PaymentDetail);
            var result = await _context.SaveChangesAsync();
            return _PaymentDetail;
        }
        public async Task<PaymentDetail> UpdatePaymentDetail(PaymentDetailCRUDViewModel vm)
        {
            var _PaymentDetail = await _context.PaymentDetail.FindAsync(vm.Id);
            double CurrentQuantity = _PaymentDetail.Quantity + _PaymentDetail.FreeQuantity;

            var _Total = vm.Quantity * vm.UnitPrice;
            var _ItemDiscountAmount = (vm.ItemDiscount / 100) * _Total;
            var WithoutDiscountTotal = _Total - _ItemDiscountAmount;
            var _ItemVATAmount = (vm.ItemVAT / 100) * WithoutDiscountTotal;
            _PaymentDetail.ItemVATAmount = Math.Round((double)_ItemVATAmount, 2);
            _PaymentDetail.ItemDiscountAmount = Math.Round((double)_ItemDiscountAmount, 2);
            _PaymentDetail.TotalAmount = Math.Round((double)(WithoutDiscountTotal + _ItemVATAmount), 2);

            _PaymentDetail.ItemName = vm.ItemName;
            _PaymentDetail.Quantity = vm.Quantity;
            _PaymentDetail.FreeQuantity = vm.FreeQuantity;
            _PaymentDetail.UnitPrice = vm.UnitPrice;
            _PaymentDetail.ItemDiscount = vm.ItemDiscount;
            _PaymentDetail.ModifiedDate = DateTime.Now;
            _PaymentDetail.ModifiedBy = vm.UserName;

            _context.Update(_PaymentDetail);
            await _context.SaveChangesAsync();

            vm.Quantity = vm.Quantity + vm.FreeQuantity;
            if (CurrentQuantity != vm.Quantity)
            {
                ItemTranViewModel _ItemTranViewModel = new();
                if (CurrentQuantity < vm.Quantity)
                {
                    _ItemTranViewModel.TranQuantity = (int)(vm.Quantity - CurrentQuantity);
                    _ItemTranViewModel.IsAddition = false;
                    _ItemTranViewModel.ActionMessage = "Update Sell Items by addition. Payment Detail Id: " + _PaymentDetail.Id;
                }
                else
                {
                    _ItemTranViewModel.TranQuantity = (int)(CurrentQuantity - vm.Quantity);
                    _ItemTranViewModel.IsAddition = true;
                    _ItemTranViewModel.ActionMessage = "Update Sell Items by return. Payment Detail Id: " + _PaymentDetail.Id;
                }

                _ItemTranViewModel.ItemId = _PaymentDetail.ItemId;
                _ItemTranViewModel.CurrentUserName = vm.UserName;
                await _iCommon.CurrentItemsUpdate(_ItemTranViewModel);
            }

            return _PaymentDetail;
        }
        public async Task<PaymentModeHistory> CreatePaymentModeHistory(PaymentModeHistoryCRUDViewModel vm)
        {
            try
            {
                PaymentModeHistory _PaymentModeHistory = vm;
                _PaymentModeHistory.CreatedDate = DateTime.Now;
                _PaymentModeHistory.ModifiedDate = DateTime.Now;
                _context.Add(_PaymentModeHistory);
                await _context.SaveChangesAsync();

                //Update Account
                await UpdateAccAccountInPaymentModeHistory(_PaymentModeHistory);
                return _PaymentModeHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<PaymentModeHistory> UpdatePaymentModeHistory(PaymentModeHistoryCRUDViewModel vm)
        {
            try
            {
                var _PaymentModeHistory = await _context.PaymentModeHistory.FindAsync(vm.Id);
                _PaymentModeHistory.ModifiedDate = DateTime.Now;
                _context.Entry(_PaymentModeHistory).CurrentValues.SetValues(vm);
                await _context.SaveChangesAsync();
                return _PaymentModeHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetInvoiceNo(int _Category)
        {
            string _InvoiceNo = null;
            List<Payment> listPayment = new();
            var _InvoiceNoPrefix = _context.CompanyInfo.Where(x => x.Id == 1).FirstOrDefault().InvoiceNoPrefix;
            if (_Category == InvoiceType.DraftInvoice)
            {
                _InvoiceNoPrefix = "D" + _InvoiceNoPrefix;
                listPayment = _context.Payment.Where(x => x.Cancelled == false && x.Category == _Category).ToList();
            }
            else
            {
                listPayment = _context.Payment.Where(x => x.Cancelled == false && x.Category == _Category || x.Category == InvoiceType.ManualInvoice).ToList();
            }

            var _PaymentCount = listPayment.Count();
            if (_PaymentCount == 0)
            {
                _InvoiceNo = _InvoiceNoPrefix + 1;
            }
            else
            {
                _InvoiceNo = _InvoiceNoPrefix + (_PaymentCount + 1);
            }

            return _InvoiceNo;
        }
        public string GetQuoteNo(int _Category)
        {
            string _InvoiceNo = null;
            var _Payment = _context.Payment.Where(x => x.Cancelled == false && x.Category == _Category);
            var _QuoteNoPrefix = _context.CompanyInfo.Where(x => x.Id == 1).FirstOrDefault().QuoteNoPrefix;

            var _PaymentCount = _Payment.Count();
            if (_PaymentCount < 1)
            {
                _InvoiceNo = _QuoteNoPrefix + 1;
            }
            else
            {
                _InvoiceNo = _QuoteNoPrefix + (_PaymentCount + 1);
            }

            return _InvoiceNo;
        }

        public CustomerHistoryViewModel GetCustomerHistory(Int64 _CustomerId)
        {
            CustomerHistoryViewModel _CustomerHistoryViewModel = new();

            var _Payment = _context.Payment.Where(x => x.CustomerId == _CustomerId);
            var _PaymentCount = _Payment.Count();
            if (_PaymentCount < 1)
            {
                _CustomerHistoryViewModel.PrevousBalance = 0;
            }
            else
            {
                _CustomerHistoryViewModel.PrevousBalance = _Payment.Sum(x => x.DueAmount);
            }

            var _CustomerInfo = _context.CustomerInfo.FirstOrDefault(x => x.Id == _CustomerId);
            if (_CustomerInfo != null)
            {
                _CustomerHistoryViewModel.CustomerNote = _CustomerInfo.Notes;
            }
            else
            {
                _CustomerHistoryViewModel.CustomerNote = string.Empty;
            }
            return _CustomerHistoryViewModel;
        }
        public async Task<PurchasesPaymentCRUDViewModel> UpdatePurchasesPayment(PurchasesPaymentCRUDViewModel vm)
        {
            var _PurchasesPayment = await _context.PurchasesPayment.FindAsync(vm.Id);
            var _PurchasesPaymentDetail = _context.PurchasesPaymentDetail.Where(x => x.PaymentId == vm.Id && x.Cancelled == false).ToList();
            var _PaymentModeHistory = _context.PaymentModeHistory.Where(x => x.PaymentId == vm.Id && x.Cancelled == false).ToList();

            var _ItemDiscountAmount = (double)_PurchasesPaymentDetail.Sum(x => x.ItemDiscountAmount);
            var _ItemVATAmount = (double)_PurchasesPaymentDetail.Sum(x => x.ItemVATAmount);
            var _TotalAmount = (double)_PurchasesPaymentDetail.Sum(x => x.TotalAmount);

            vm.DiscountAmount = Math.Round(_ItemDiscountAmount, 2);
            vm.VATAmount = Math.Round(_ItemVATAmount, 2);

            vm.SubTotal = Math.Round(((_TotalAmount + _ItemDiscountAmount) - _ItemVATAmount), 2);
            vm.GrandTotal = Math.Round((double)(_TotalAmount + vm.CommonCharge), 2);

            vm.PaidAmount = Math.Round((double)_PaymentModeHistory.Sum(x => x.Amount), 2);
            if (vm.PaidAmount >= vm.GrandTotal)
            {
                vm.DueAmount = 0;
                vm.PaymentStatus = PaymentStatusInfo.Paid;
                vm.ChangedAmount = Math.Round((double)(vm.PaidAmount - vm.GrandTotal), 2);
            }
            else
            {
                vm.DueAmount = Math.Round((double)(vm.GrandTotal - vm.PaidAmount), 2);
                vm.ChangedAmount = 0;
            }

            vm.ModifiedDate = DateTime.Now;
            vm.ModifiedBy = vm.UserName;
            _context.Entry(_PurchasesPayment).CurrentValues.SetValues(vm);
            await _context.SaveChangesAsync();

            return vm;
        }
        public async Task<PurchasesPaymentDetail> UpdatePurchasesPaymentDetail(PurchasesPaymentDetailCRUDViewModel vm)
        {
            var _PurchasesPaymentDetail = await _context.PurchasesPaymentDetail.FindAsync(vm.Id);
            double CurrentQuantity = _PurchasesPaymentDetail.Quantity + _PurchasesPaymentDetail.FreeQuantity;

            var _Total = vm.Quantity * vm.UnitPrice;
            var _ItemDiscountAmount = (vm.ItemDiscount / 100) * _Total;
            var WithoutDiscountTotal = _Total - _ItemDiscountAmount;
            var _ItemVATAmount = (vm.ItemVAT / 100) * WithoutDiscountTotal;
            _PurchasesPaymentDetail.ItemVATAmount = Math.Round((double)_ItemVATAmount, 2);
            _PurchasesPaymentDetail.ItemDiscountAmount = Math.Round((double)_ItemDiscountAmount, 2);
            _PurchasesPaymentDetail.TotalAmount = Math.Round((double)(WithoutDiscountTotal + _ItemVATAmount), 2);

            _PurchasesPaymentDetail.ItemName = vm.ItemName;
            _PurchasesPaymentDetail.Quantity = vm.Quantity;
            _PurchasesPaymentDetail.FreeQuantity = vm.FreeQuantity;
            _PurchasesPaymentDetail.UnitPrice = vm.UnitPrice;
            _PurchasesPaymentDetail.ItemDiscount = vm.ItemDiscount;
            _PurchasesPaymentDetail.ModifiedDate = DateTime.Now;
            _PurchasesPaymentDetail.ModifiedBy = vm.UserName;

            _context.Update(_PurchasesPaymentDetail);
            await _context.SaveChangesAsync();

            vm.Quantity = _PurchasesPaymentDetail.Quantity + _PurchasesPaymentDetail.FreeQuantity;
            if (CurrentQuantity != vm.Quantity)
            {
                ItemTranViewModel _ItemTranViewModel = new();
                if (CurrentQuantity < vm.Quantity)
                {
                    _ItemTranViewModel.TranQuantity = (int)(vm.Quantity - CurrentQuantity);
                    _ItemTranViewModel.IsAddition = true;
                    _ItemTranViewModel.ActionMessage = "Update Purchases Items by addition. Purchases Payment Detail Id: " + _PurchasesPaymentDetail.Id;
                }
                else
                {
                    _ItemTranViewModel.TranQuantity = (int)(CurrentQuantity - vm.Quantity);
                    _ItemTranViewModel.IsAddition = false;
                    _ItemTranViewModel.ActionMessage = "Update Purchases Items by return. Purchases Payment Detail Id: " + _PurchasesPaymentDetail.Id;
                }

                _ItemTranViewModel.ItemId = _PurchasesPaymentDetail.ItemId;
                _ItemTranViewModel.CurrentUserName = vm.UserName;
                await _iCommon.CurrentItemsUpdate(_ItemTranViewModel);
            }

            return _PurchasesPaymentDetail;
        }
        public async Task<Payment> CreateDraftInvoice(PaymentCRUDViewModel vm)
        {
            try
            {
                Payment _Payment = new();
                _Payment = vm;
                _Payment.QuoteNo = null;
                _Payment.GrandTotal = 0;
                _Payment.Category = InvoiceType.DraftInvoice;
                _Payment.CustomerId = 1;
                _Payment.CreatedDate = DateTime.Now;
                _Payment.ModifiedDate = DateTime.Now;
                _Payment.CreatedBy = vm.UserName;
                _Payment.ModifiedBy = vm.UserName;
                _context.Add(_Payment);
                await _context.SaveChangesAsync();
                return _Payment;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Payment> CreateManualInvoice(PaymentCRUDViewModel vm)
        {
            try
            {
                Payment _Payment = new();
                _Payment = vm;
                _Payment.QuoteNo = null;
                _Payment.GrandTotal = 0;
                _Payment.Category = InvoiceType.RegularInvoice;
                _Payment.CustomerId = 1;
                _Payment.CreatedDate = DateTime.Now;
                _Payment.ModifiedDate = DateTime.Now;
                _Payment.CreatedBy = vm.UserName;
                _Payment.ModifiedBy = vm.UserName;
                _context.Add(_Payment);
                await _context.SaveChangesAsync();
                return _Payment;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetPurchasesPaymentNo(int _Category)
        {
            string _InvoiceNo = null;
            string _PurchasesInvoiceNoPrefix = "PINV";
            var _PurchasesPayment = _context.PurchasesPayment.Where(x => x.Cancelled == false && x.Category == _Category);

            if (_Category == InvoiceType.DraftInvoice)
            {
                _PurchasesInvoiceNoPrefix = "D" + _PurchasesInvoiceNoPrefix;
            }

            var _PurchasesPaymentCount = _PurchasesPayment.Count();
            if (_PurchasesPaymentCount == 0)
            {
                _InvoiceNo = _PurchasesInvoiceNoPrefix + 1;
            }
            else
            {
                _InvoiceNo = _PurchasesInvoiceNoPrefix + (_PurchasesPaymentCount + 1);
            }

            return _InvoiceNo;
        }
        public string GetPurchasesQuoteNo(int _Category)
        {
            string _PurchasesInvoiceNo = null;
            var _PurchasesPayment = _context.PurchasesPayment.Where(x => x.Cancelled == false && x.Category == _Category);

            var _PurchasesPaymentCount = _PurchasesPayment.Count();
            if (_PurchasesPaymentCount < 1)
            {
                _PurchasesInvoiceNo = "QINV" + 1;
            }
            else
            {
                _PurchasesInvoiceNo = "QINV" + (_PurchasesPaymentCount + 1);
            }
            return _PurchasesInvoiceNo;
        }
        public async Task<ExpenseSummaryCRUDViewModel> UpdateExpenseSummary(ExpenseSummaryCRUDViewModel vm)
        {
            var expenseSummary = await _context.ExpenseSummary.FindAsync(vm.Id);

            var listExpenseDetails = _context.ExpenseDetails
                                            .Where(x => x.ExpenseSummaryId == vm.Id && !x.Cancelled)
                                            .ToList();
            var totalAmount = listExpenseDetails.Sum(x => x.TotalPrice);
            vm.GrandTotal = Math.Round((double)totalAmount, 2);

            var listPaymentModeHistory = _context.PaymentModeHistory
                                                .Where(x => x.PaymentId == vm.Id && !x.Cancelled)
                                                .ToList();
            var paidAmount = listPaymentModeHistory.Sum(x => x.Amount);
            vm.PaidAmount = Math.Round((double)paidAmount, 2);

            vm.DueAmount = Math.Max(vm.GrandTotal - vm.PaidAmount, 0);
            vm.ChangeAmount = Math.Max(vm.PaidAmount - vm.GrandTotal, 0);

            vm.ModifiedDate = DateTime.Now;
            vm.ModifiedBy = vm.UserName;

            _context.Entry(expenseSummary).CurrentValues.SetValues(vm);
            await _context.SaveChangesAsync();

            return vm;
        }
        public async Task<ExpenseDetailsCRUDViewModel> AddExpenseDetails(ExpenseDetailsCRUDViewModel vm)
        {
            ExpenseDetails _ExpenseDetails = new();
            _ExpenseDetails = vm;
            _ExpenseDetails.CreatedDate = DateTime.Now;
            _ExpenseDetails.ModifiedDate = DateTime.Now;
            _ExpenseDetails.CreatedBy = vm.UserName;
            _ExpenseDetails.ModifiedBy = vm.UserName;
            _context.Add(_ExpenseDetails);
            var result = await _context.SaveChangesAsync();
            vm = _ExpenseDetails;

            return vm;
        }
        public async Task<ExpenseDetailsCRUDViewModel> UpdateExpenseDetails(ExpenseDetailsCRUDViewModel vm)
        {
            var _ExpenseDetails = await _context.ExpenseDetails.FindAsync(vm.Id);
            vm.ExpenseTypeId = _ExpenseDetails.ExpenseTypeId;
            vm.Cancelled = false;
            vm.CreatedDate = _ExpenseDetails.CreatedDate;
            vm.CreatedBy = _ExpenseDetails.CreatedBy;

            vm.ModifiedDate = DateTime.Now;
            vm.ModifiedBy = vm.UserName;
            _context.Entry(_ExpenseDetails).CurrentValues.SetValues(vm);
            await _context.SaveChangesAsync();
            return vm;
        }
        public async Task<SendEmailHistoryCRUDViewModel> AddSendEmailHistory(SendEmailHistoryCRUDViewModel vm)
        {
            SendEmailHistory _SendEmailHistory = new();
            _SendEmailHistory = vm;
            _SendEmailHistory.CreatedDate = DateTime.Now;
            _SendEmailHistory.ModifiedDate = DateTime.Now;
            _SendEmailHistory.CreatedBy = vm.UserName;
            _SendEmailHistory.ModifiedBy = vm.UserName;
            _context.Add(_SendEmailHistory);
            var result = await _context.SaveChangesAsync();
            vm = _SendEmailHistory;
            return vm;
        }

        public async Task<ReturnLogCRUDViewModel> AddReturnLog(ReturnLogCRUDViewModel vm)
        {
            try
            {
                ReturnLog _ReturnLog = new();
                _ReturnLog = vm;
                _ReturnLog.CreatedDate = DateTime.Now;
                _ReturnLog.ModifiedDate = DateTime.Now;
                _ReturnLog.CreatedBy = vm.UserName;
                _ReturnLog.ModifiedBy = vm.UserName;
                _context.Add(_ReturnLog);
                var result = await _context.SaveChangesAsync();
                vm = _ReturnLog;
                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<PaymentModeHistory> UpdateAccAccountInPaymentModeHistory(PaymentModeHistoryCRUDViewModel vm)
        {
            try
            {
                PaymentModeHistory _PaymentModeHistory = vm;
                double _Debit = 0;
                double _Credit = 0;
                string _PaymentType = string.Empty;

                var _AccAccount = await _context.AccAccount.Where(x => x.Id == AccAccountInfo.DefaultAccount).FirstOrDefaultAsync();
                AccAccount _AccAccountNew = _AccAccount;

                if (vm.PaymentType == InvoicePaymentType.SalesInvoicePayment)
                {
                    _AccAccountNew.Balance = (double)(_AccAccount.Balance + _PaymentModeHistory.Amount);
                    _AccAccountNew.Credit = (double)(_AccAccount.Credit + _PaymentModeHistory.Amount);
                    _Credit = (double)_PaymentModeHistory.Amount;
                    _Debit = 0;
                    _PaymentType = "Sales Payment";
                }
                else if (vm.PaymentType == InvoicePaymentType.PurchasesInvoicePayment)
                {
                    _AccAccountNew.Balance = (double)(_AccAccount.Balance - _PaymentModeHistory.Amount);
                    _AccAccountNew.Debit = (double)(_AccAccount.Debit + _PaymentModeHistory.Amount);
                    _Credit = 0;
                    _Debit = (double)_PaymentModeHistory.Amount;
                    _PaymentType = "Purchases Payment";
                }
                else if (vm.PaymentType == InvoicePaymentType.RegularExpensePayment)
                {
                    _AccAccountNew.Balance = (double)(_AccAccount.Balance - _PaymentModeHistory.Amount);
                    _AccAccountNew.Debit = (double)(_AccAccount.Debit + _PaymentModeHistory.Amount);
                    _Credit = 0;
                    _Debit = (double)_PaymentModeHistory.Amount;
                    _PaymentType = "Regular Expense Payment";
                }

                _AccAccountNew.ModifiedDate = DateTime.Now;
                _AccAccountNew.ModifiedBy = vm.CreatedBy;
                _context.Entry(_AccAccount).CurrentValues.SetValues(_AccAccountNew);
                await _context.SaveChangesAsync();

                //AddAccTransaction
                AccTransactionCRUDViewModel _AccTransactionCRUDViewModel = new()
                {
                    AccountId = _AccAccount.Id,
                    Type = _PaymentType,
                    Reference = _PaymentType + ". Id: " + vm.PaymentId,
                    Credit = _Credit,
                    Debit = _Debit,
                    Amount = (double)_PaymentModeHistory.Amount,
                    Description = _PaymentType,
                    UserName = vm.CreatedBy
                };
                await _iCommon.AddAccTransaction(_AccTransactionCRUDViewModel);

                return _PaymentModeHistory;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> UpdateAccAccountInDeletePaymentModeHistory(PaymentModeHistory _PaymentModeHistory)
        {
            try
            {
                double _Debit = 0;
                double _Credit = 0;
                string _PaymentType = string.Empty;

                var _AccAccount = await _context.AccAccount.Where(x => x.Id == AccAccountInfo.DefaultAccount).FirstOrDefaultAsync();
                AccAccount _AccAccountNew = _AccAccount;

                if (_PaymentModeHistory.PaymentType == InvoicePaymentType.SalesInvoicePayment)
                {
                    _AccAccountNew.Balance = (double)(_AccAccountNew.Balance - _PaymentModeHistory.Amount);
                    _AccAccountNew.Credit = (double)(_AccAccountNew.Credit - _PaymentModeHistory.Amount);
                    _Credit = 0;
                    _Debit = (double)_PaymentModeHistory.Amount;
                    _PaymentType = "Sales Payment Deleted";
                }
                else if (_PaymentModeHistory.PaymentType == InvoicePaymentType.PurchasesInvoicePayment)
                {
                    _AccAccountNew.Balance = (double)(_AccAccountNew.Balance + _PaymentModeHistory.Amount);
                    _AccAccountNew.Debit = (double)(_AccAccountNew.Debit - _PaymentModeHistory.Amount);
                    _Credit = (double)_PaymentModeHistory.Amount;
                    _Debit = 0;
                    _PaymentType = "Purchases Payment Deleted";
                }
                else if (_PaymentModeHistory.PaymentType == InvoicePaymentType.RegularExpensePayment)
                {
                    _AccAccountNew.Balance = (double)(_AccAccountNew.Balance + _PaymentModeHistory.Amount);
                    _AccAccountNew.Debit = (double)(_AccAccountNew.Debit - _PaymentModeHistory.Amount);
                    _Credit = (double)_PaymentModeHistory.Amount;
                    _Debit = 0;
                    _PaymentType = "Regular Expense Payment Deleted";
                }

                _AccAccountNew.ModifiedDate = DateTime.Now;
                _AccAccountNew.ModifiedBy = _PaymentModeHistory.CreatedBy;
                _context.Entry(_AccAccount).CurrentValues.SetValues(_AccAccountNew);
                await _context.SaveChangesAsync();

                //AddAccTransaction
                AccTransactionCRUDViewModel _AccTransactionCRUDViewModel = new()
                {
                    AccountId = _AccAccount.Id,
                    Type = _PaymentType,
                    Reference = _PaymentType + ". Id: " + _PaymentModeHistory.PaymentId,
                    Credit = _Credit,
                    Debit = _Debit,
                    Amount = (double)_PaymentModeHistory.Amount,
                    Description = _PaymentType,
                    UserName = _PaymentModeHistory.CreatedBy
                };
                await _iCommon.AddAccTransaction(_AccTransactionCRUDViewModel);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
