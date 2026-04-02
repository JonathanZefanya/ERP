using BusinessERP.Models;
using BusinessERP.Models.WarehouseViewModel;

namespace BusinessERP.Services
{
    public interface ITransferItemService
    {
        Task<bool> ItemTran(TransferItemViewModel vm);
    }
}
