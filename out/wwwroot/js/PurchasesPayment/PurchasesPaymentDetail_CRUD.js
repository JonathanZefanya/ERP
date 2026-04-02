var AddPurchasesPaymentDetail = function () {
    if (!FieldValidation('#ItemId')) {
        FieldValidationAlert('#ItemId', 'Payment Item is Required.', "warning");
        return;
    }
    var _Quantity = $("#Quantity").val();
    if (_Quantity === "" || _Quantity === null || parseFloat(_Quantity) < 1) {
        FieldValidationAlert('#Quantity', 'Quantity is Required', "warning");
        return;
    }

    //Check Stock
    var _ItemIdText = $("#ItemId option:selected").text();
    var splitArray = _ItemIdText.split(":");
    if (parseFloat(splitArray[5]) < parseFloat($("#Quantity").val())) {
        FieldValidationAlert('#Quantity', 'Stock limit crosses the selected quantity, please check the quantity.', "warning");
        return;
    }

    $("#btnPaymentsDetails").val("Please Wait");
    $("#btnPaymentsDetails").attr('disabled', 'disabled');


    //var _UnitPrice = $("#UnitPrice option:selected").text();
    //var splitUnitPrice = _UnitPrice.split(":");
    //_UnitPrice = splitUnitPrice[1];

    var _UnitPrice = $("#UnitPrice").val();


    var splitArrayForName = _ItemIdText.split(":");
    var PurchasesPaymentsDetailItem = {};
    PurchasesPaymentsDetailItem.ItemId = $("#ItemId").val();
    PurchasesPaymentsDetailItem.PaymentId = $("#Id").val();
    PurchasesPaymentsDetailItem.ItemName = splitArrayForName[0];
    PurchasesPaymentsDetailItem.Quantity = $("#Quantity").val();
    PurchasesPaymentsDetailItem.FreeQuantity = $("#FreeQuantity").val();

    PurchasesPaymentsDetailItem.UnitPrice = parseFloat(_UnitPrice);
    PurchasesPaymentsDetailItem.ItemVAT = $("#ItemVAT").val();
    PurchasesPaymentsDetailItem.ItemDiscount = $("#ItemDiscount").val();
    PurchasesPaymentsDetailItem.TotalAmount = $("#TotalAmount").val();

    var _PurchasesPaymentCRUDViewModel = PreparedFormObj();
    _PurchasesPaymentCRUDViewModel.Category = $("#Category").val();
    PurchasesPaymentsDetailItem.PurchasesPaymentCRUDViewModel = _PurchasesPaymentCRUDViewModel;

    $.ajax({
        type: "POST",
        url: "/PurchasesPayment/AddPurchasesPaymentDetail",
        data: PurchasesPaymentsDetailItem,
        dataType: "json",
        success: function (result) {
            setTimeout(function () {
                UpdatePurchasePaymentFields(result.PurchasesPaymentCRUDViewModel);
            }, 300);
            
            AddHTMLTableRow(result);
            $("#btnPaymentsDetails").val("Add Item");
            $('#btnPaymentsDetails').removeAttr('disabled');
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

function UpdatePurchasesPaymentDetail(button) {
    var row = $(button).closest("TR");
    var _PurchasesPaymentDetailId = $("TD", row).eq(0).html();
    var _ItemName = $("#ItemName" + _PurchasesPaymentDetailId).val();

    var _IsVat = $("#IsVat").val();
    var _ItemVAT;
    if (_IsVat == "Yes") {
        _ItemVAT = parseFloat($("#ItemVAT" + _PurchasesPaymentDetailId).val());
    }
    else {
        _ItemVAT = 0;
    }

    var _Quantity = parseFloat($("#Quantity" + _PurchasesPaymentDetailId).val());
    var _FreeQuantity = parseFloat($("#FreeQuantity" + _PurchasesPaymentDetailId).val());
    var _UnitPrice = parseFloat($("#UnitPrice" + _PurchasesPaymentDetailId).val());
    var _ItemDiscount = parseFloat($("#ItemDiscount" + _PurchasesPaymentDetailId).val());


    $("#btnUpdatePaymentsDetail" + _PurchasesPaymentDetailId).val("Please Wait");
    $("#btnUpdatePaymentsDetail" + _PurchasesPaymentDetailId).attr('disabled', 'disabled');

    var PurchasesPaymentDetailCRUDViewModel = {};
    PurchasesPaymentDetailCRUDViewModel.Id = _PurchasesPaymentDetailId;
    PurchasesPaymentDetailCRUDViewModel.ItemName = _ItemName;
    PurchasesPaymentDetailCRUDViewModel.Quantity = _Quantity;
    PurchasesPaymentDetailCRUDViewModel.FreeQuantity = _FreeQuantity;

    PurchasesPaymentDetailCRUDViewModel.UnitPrice = _UnitPrice;
    PurchasesPaymentDetailCRUDViewModel.ItemVAT = _ItemVAT;
    PurchasesPaymentDetailCRUDViewModel.ItemDiscount = _ItemDiscount;
    PurchasesPaymentDetailCRUDViewModel.TotalAmount = _Quantity * _UnitPrice;

    var _PurchasesPaymentDetailCRUDViewModel = PreparedFormObj();
    PurchasesPaymentDetailCRUDViewModel.PurchasesPaymentCRUDViewModel = _PurchasesPaymentDetailCRUDViewModel;
    
    $.ajax({
        type: "POST",
        url: "/PurchasesPayment/UpdatePurchasesPaymentDetail",
        data: PurchasesPaymentDetailCRUDViewModel,
        dataType: "json",
        success: function (result) {
            toastr.success("Update item successfully. Item ID: " + result.Id, 'Success');
            $("#TotalAmount" + result.Id).text(result.TotalAmount);
            $("#btnUpdatePaymentsDetail" + _PurchasesPaymentDetailId).val("Update");
            $('#btnUpdatePaymentsDetail' + _PurchasesPaymentDetailId).removeAttr('disabled');

            UpdatePurchasePaymentFields(result.PurchasesPaymentCRUDViewModel);
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}


var DeletePurchasesPaymentDetail = function (PurchasesPaymentDetailCRUDViewModel) {
    var _PurchasesPaymentCRUDViewModel = PreparedFormObj();
    PurchasesPaymentDetailCRUDViewModel.PurchasesPaymentCRUDViewModel = _PurchasesPaymentCRUDViewModel;

    $.ajax({
        type: "DELETE",
        url: "/PurchasesPayment/DeletePurchasesPaymentDetail",
        data: PurchasesPaymentDetailCRUDViewModel,
        dataType: "json",
        success: function (result) {
            toastr.success("Purchases Payment details item has been deleted successfully. ID: " + result.Id, 'Success');
            UpdatePurchasePaymentFields(result);
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}
