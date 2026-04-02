using System;

namespace BusinessERP.Models.PurchasesPaymentDetailViewModel
{
    public class PurchasesPaymentDetailUpdateViewModel
    {
        public Int64 Id { get; set; }
        public int Quantity { get; set; }
        public int FreeQuantity { get; set; }
        public double? UnitPrice { get; set; }
        public double? TotalAmount { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }


        public static implicit operator PurchasesPaymentDetailUpdateViewModel(PurchasesPaymentDetail _PurchasesPaymentDetail)
        {
            return new PurchasesPaymentDetailUpdateViewModel
            {
                Id = _PurchasesPaymentDetail.Id,
                Quantity = _PurchasesPaymentDetail.Quantity,
                FreeQuantity = _PurchasesPaymentDetail.FreeQuantity,
                UnitPrice = _PurchasesPaymentDetail.UnitPrice,
                TotalAmount = _PurchasesPaymentDetail.TotalAmount,
                ModifiedDate = _PurchasesPaymentDetail.ModifiedDate,
                ModifiedBy = _PurchasesPaymentDetail.ModifiedBy,
            };
        }

        public static implicit operator PurchasesPaymentDetail(PurchasesPaymentDetailUpdateViewModel vm)
        {
            return new PurchasesPaymentDetail
            {
                Id = vm.Id,
                Quantity = vm.Quantity,
                FreeQuantity = vm.FreeQuantity,
                UnitPrice = vm.UnitPrice,
                TotalAmount = vm.TotalAmount,
                ModifiedDate = vm.ModifiedDate,
                ModifiedBy = vm.ModifiedBy,
            };
        }
    }
}
