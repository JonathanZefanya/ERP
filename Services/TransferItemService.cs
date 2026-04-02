using BusinessERP.Data;
using BusinessERP.Helpers;
using BusinessERP.Models;
using BusinessERP.Models.ItemsHistoryViewModel;
using BusinessERP.Models.ItemsViewModel;
using BusinessERP.Models.ItemTransferLogViewModel;
using BusinessERP.Models.WarehouseNotificationViewModel;
using BusinessERP.Models.WarehouseViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.SignalR;

namespace BusinessERP.Services
{
    public class TransferItemService : ITransferItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<SignalServer> _signalServer;
        public TransferItemService(ApplicationDbContext context, IHubContext<SignalServer> signalServer)
        {
            _context = context;
            _signalServer = signalServer;
        }
        public async Task<bool> ItemTran(TransferItemViewModel vm)
        {
            try
            {
                var TranItem = await _context.Items.Where(x => x.Id == vm.ItemId).FirstOrDefaultAsync();
                int _OldQuantity = TranItem.Quantity;
                ItemsCRUDViewModel _ItemsCRUDViewModel = new();
                ItemsHistoryCRUDViewModel _ItemHistoryCRUDViewModel = new();
                Items _Item = new();
                string _Message = string.Empty;
                string _UserName = vm.UserName;

                //Add ItemTransferLog
                ItemTransferLogCRUDViewModel _ItemTransferLogCRUDViewModel = vm;
                ItemTransferLog _ItemTransferLog = _ItemTransferLogCRUDViewModel;
                _ItemTransferLog.CreatedDate = DateTime.Now;
                _ItemTransferLog.ModifiedDate = DateTime.Now;
                _ItemTransferLog.CreatedBy = _UserName;
                _ItemTransferLog.ModifiedBy = _UserName;
                _context.Add(_ItemTransferLog);
                _context.SaveChanges();

                //Update Item: From Warehouse
                _Item = TranItem;
                TranItem.Quantity = TranItem.Quantity - vm.TotalTransferItem;
                TranItem.UpdateQntType = "Warehouse Tran: Send Item";
                TranItem.UpdateQntNote = "Warehouse Tran: Send Item";
                TranItem.ModifiedDate = DateTime.Now;
                TranItem.ModifiedBy = _UserName;
                _context.Entry(_Item).CurrentValues.SetValues(TranItem);
                await _context.SaveChangesAsync();

                //Add Item History: From Warehouse
                _ItemsCRUDViewModel = _Item;
                _ItemHistoryCRUDViewModel = _ItemsCRUDViewModel;
                _ItemHistoryCRUDViewModel.ItemId = _Item.Id;
                _ItemHistoryCRUDViewModel.Id = 0;
                _ItemHistoryCRUDViewModel.Action = "Warehouse Tran: Send Item: " + _Item.Name;
                _ItemHistoryCRUDViewModel.TranQuantity = vm.TotalTransferItem;
                _ItemHistoryCRUDViewModel.OldQuantity = _OldQuantity;
                _ItemHistoryCRUDViewModel.NewQuantity = TranItem.Quantity;
                ItemsHistory _ItemHistoryFromWarehouse = _ItemHistoryCRUDViewModel;
                _context.Add(_ItemHistoryFromWarehouse);
                _context.SaveChanges();


                //Item Already Exist in Warehouse
                var ToWarehouseItem = await _context.Items.Where(x => x.Name == TranItem.Name && x.WarehouseId == vm.ToWarehouseId).FirstOrDefaultAsync();
                if (ToWarehouseItem == null)
                {
                    //Add New Item: To Warehouse
                    _Message = "Add New Item By Warehouse Transfer -" + _Item.Name;
                    TranItem.Id = 0;
                    //Update Bar Code 
                    TranItem.Code = "ITM" + StaticData.RandomDigits(6);
                    TranItem.Barcode = SampleBarcode.Default + TranItem.Code;

                    TranItem.WarehouseId = vm.ToWarehouseId;
                    TranItem.Quantity = vm.TotalTransferItem;
                    TranItem.CreatedDate = DateTime.Now;
                    TranItem.ModifiedDate = DateTime.Now;
                    TranItem.CreatedBy = _UserName;
                    TranItem.ModifiedBy = _UserName;
                    _context.Add(TranItem);
                    _context.SaveChanges();

                    //Add Item History: To Warehouse
                    _ItemsCRUDViewModel = TranItem;
                    _ItemHistoryCRUDViewModel = _ItemsCRUDViewModel;
                    _ItemHistoryCRUDViewModel.ItemId = _Item.Id;
                    _ItemHistoryCRUDViewModel.Id = 0;
                    _ItemHistoryCRUDViewModel.Action = _Message;
                    _ItemHistoryCRUDViewModel.TranQuantity = 0;
                    _ItemHistoryCRUDViewModel.OldQuantity = _Item.Quantity;
                    _ItemHistoryCRUDViewModel.NewQuantity = _Item.Quantity;
                }
                else
                {
                    //Update Item: To Warehouse
                    TranItem = ToWarehouseItem;
                    TranItem.Quantity = ToWarehouseItem.Quantity + vm.TotalTransferItem;
                    TranItem.UpdateQntType = "Warehouse Tran: Receive Item";
                    TranItem.UpdateQntNote = "Warehouse Tran: Receive Item";
                    TranItem.ModifiedDate = DateTime.Now;
                    TranItem.ModifiedBy = _UserName;
                    _context.Entry(ToWarehouseItem).CurrentValues.SetValues(TranItem);
                    await _context.SaveChangesAsync();

                    //Add Item History: To Warehouse
                    _Message = "Receive Existing Item By Warehouse Transfer -" + ToWarehouseItem.Name;
                    _ItemsCRUDViewModel = TranItem;
                    _ItemHistoryCRUDViewModel = _ItemsCRUDViewModel;
                    _ItemHistoryCRUDViewModel.ItemId = ToWarehouseItem.Id;
                    _ItemHistoryCRUDViewModel.Id = 0;
                    _ItemHistoryCRUDViewModel.Action = _Message;
                    _ItemHistoryCRUDViewModel.TranQuantity = 0;
                    _ItemHistoryCRUDViewModel.OldQuantity = ToWarehouseItem.Quantity - vm.TotalTransferItem;
                    _ItemHistoryCRUDViewModel.NewQuantity = TranItem.Quantity;
                }


                ItemsHistory _ItemHistoryToWarehouse = _ItemHistoryCRUDViewModel;
                _context.Add(_ItemHistoryToWarehouse);
                _context.SaveChanges();

                //Add Notification
                WarehouseNotificationCRUDViewModel _WarehouseNotificationCRUDViewModel = new();
                _WarehouseNotificationCRUDViewModel.FromWarehouseId = vm.FromWarehouseId;
                _WarehouseNotificationCRUDViewModel.ToWarehouseId = vm.ToWarehouseId;
                _WarehouseNotificationCRUDViewModel.ItemId = vm.ItemId;
                _WarehouseNotificationCRUDViewModel.ReceiveQuantity = vm.TotalTransferItem;
                _WarehouseNotificationCRUDViewModel.SendQuantity = vm.TotalTransferItem;
                _WarehouseNotificationCRUDViewModel.Message = _Message;
                _WarehouseNotificationCRUDViewModel.IsRead = false;
                _WarehouseNotificationCRUDViewModel.UserName = _UserName;
                await AddWarehouseNotification(_WarehouseNotificationCRUDViewModel);

                await _signalServer.Clients.All.SendAsync("refreshWarehouseNotification");
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<WarehouseNotificationCRUDViewModel> AddWarehouseNotification(WarehouseNotificationCRUDViewModel vm)
        {
            try
            {
                WarehouseNotification _WarehouseNotification = new();
                _WarehouseNotification = vm;
                _WarehouseNotification.CreatedDate = DateTime.Now;
                _WarehouseNotification.ModifiedDate = DateTime.Now;
                _WarehouseNotification.CreatedBy = vm.UserName;
                _WarehouseNotification.ModifiedBy = vm.UserName;
                _context.Add(_WarehouseNotification);
                var result = await _context.SaveChangesAsync();
                vm = _WarehouseNotification;
                return vm;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
