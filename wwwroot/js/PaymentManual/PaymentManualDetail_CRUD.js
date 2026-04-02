var onchangeCalculateTotal = function () {
    var _Quantity = $("#Quantity").val();
    var _UnitPrice = $("#UnitPrice").val();
    var _ItemDiscount = $("#ItemDiscount").val();
    var _ItemVAT = $("#ItemVAT").val();

    if (_UnitPrice == '')
        _UnitPrice = 0;
    if (_ItemVAT == '')
        _ItemVAT = 0;
    if (_ItemDiscount == '')
        _ItemDiscount = 0;

    var _ItemDiscountAmount = (parseFloat(_ItemDiscount) / 100) * parseFloat(_UnitPrice);
    _UnitPrice = parseFloat(_UnitPrice) - _ItemDiscountAmount;
    _UnitPrice = parseFloat(_UnitPrice) + (parseFloat(_ItemVAT) / 100) * parseFloat(_UnitPrice);
    var _TotalAmount = parseFloat(_UnitPrice) * parseFloat(_Quantity);
    $("#TotalAmount").val(_TotalAmount.toFixed(2));
}


var AddPaymentDetail = function () {
    if (!FieldValidation('#ItemName')) {
        FieldValidationAlert('#ItemId', 'Payment Item is Required.', "warning");
        return;
    }
    var _Quantity = $("#Quantity").val();
    if (_Quantity === "" || _Quantity === null || parseFloat(_Quantity) < 1) {
        FieldValidationAlert('#Quantity', 'Quantity is Required', "warning");
        return;
    }
    var _UnitPrice = $("#UnitPrice").val();
    if (_UnitPrice === "" || _UnitPrice === null || parseFloat(_UnitPrice) < 1) {
        FieldValidationAlert('#UnitPrice', 'Unit Price is Required', "warning");
        return;
    }

    $("#btnPaymentsDetails").val("Please Wait");
    $("#btnPaymentsDetails").attr('disabled', 'disabled');

    var _ItemName = $("#ItemName").val();
    var _ItemVAT = $("#ItemVAT").val();
    var _ItemDiscount = $("#ItemDiscount").val();
    if (_ItemVAT == '')
        _ItemVAT = 0;
    if (_ItemDiscount == '')
        _ItemDiscount = 0;


    var PaymentsDetailItem = {};
    PaymentsDetailItem.ItemId = '-1';
    PaymentsDetailItem.PaymentId = $("#Id").val();
    PaymentsDetailItem.ItemName = _ItemName;
    PaymentsDetailItem.Quantity = _Quantity;
    PaymentsDetailItem.UnitPrice = parseFloat(_UnitPrice);
    PaymentsDetailItem.ItemVAT = _ItemVAT;
    PaymentsDetailItem.ItemDiscount = _ItemDiscount;
    PaymentsDetailItem.TotalAmount = $("#TotalAmount").val();

    var _PaymentCRUDViewModel = PreparedFormObj();
    PaymentsDetailItem.PaymentCRUDViewModel = _PaymentCRUDViewModel;
    var _Category = $("#Category").val();
    PaymentsDetailItem.PaymentCRUDViewModel.Category = _Category;

    $.ajax({
        type: "POST",
        url: "/Payment/AddPaymentDetail",
        data: PaymentsDetailItem,
        dataType: "json",
        success: function (result) {
            UpdateSalesPaymentFields(result.PaymentCRUDViewModel);
            AddHTMLTableRow(result);
            $("#btnPaymentsDetails").val("Add Item");
            $('#btnPaymentsDetails').removeAttr('disabled');
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
            $("#btnPaymentsDetails").val("Add Item");
            $('#btnPaymentsDetails').removeAttr('disabled');
        }
    });
}


function UpdatePaymentDetail(button) {
    var row = $(button).closest("TR");
    var _PaymentDetailId = $("TD", row).eq(0).html();
    var _ItemName = $("#ItemName" + _PaymentDetailId).val();

    var _IsVat = $("#IsVat").val();
    var _ItemVAT;
    if (_IsVat == "Yes") {
        _ItemVAT = $("TD", row).eq(4).html();
        $("#ItemVAT").val(_ItemVAT);
    }
    else {
        _ItemVAT = 0;
    }

    var _Quantity = parseFloat($("#Quantity" + _PaymentDetailId).val());
    var _UnitPrice = parseFloat($("#UnitPrice" + _PaymentDetailId).val());
    var _ItemDiscount = parseFloat($("#ItemDiscount" + _PaymentDetailId).val());


    $("#btnUpdatePaymentsDetail" + _PaymentDetailId).val("Please Wait");
    $("#btnUpdatePaymentsDetail" + _PaymentDetailId).attr('disabled', 'disabled');

    var PaymentDetailCRUDViewModel = {};
    PaymentDetailCRUDViewModel.Id = _PaymentDetailId;
    PaymentDetailCRUDViewModel.ItemName = _ItemName;
    PaymentDetailCRUDViewModel.Quantity = _Quantity;
    PaymentDetailCRUDViewModel.UnitPrice = _UnitPrice;
    PaymentDetailCRUDViewModel.ItemVAT = _ItemVAT;
    PaymentDetailCRUDViewModel.ItemDiscount = _ItemDiscount;
    PaymentDetailCRUDViewModel.TotalAmount = _Quantity * _UnitPrice;

    var _PaymentCRUDViewModel = PreparedFormObj();
    PaymentDetailCRUDViewModel.PaymentCRUDViewModel = _PaymentCRUDViewModel;
    var _Category = $("#Category").val();
    PaymentDetailCRUDViewModel.PaymentCRUDViewModel.Category = _Category;

    $.ajax({
        type: "POST",
        url: "/Payment/UpdatePaymentDetail",
        data: PaymentDetailCRUDViewModel,
        dataType: "json",
        success: function (result) {
            toastr.success("Update item successfully. Item ID: " + result.Id, 'Success');
            $("#TotalAmount" + result.Id).text(result.TotalAmount);
            $("#btnUpdatePaymentsDetail" + _PaymentDetailId).val("Update");
            $('#btnUpdatePaymentsDetail' + _PaymentDetailId).removeAttr('disabled');

            UpdateSalesPaymentFields(result.PaymentCRUDViewModel);
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}


var DeletePaymentDetail = function (PaymentDetailCRUDViewModel) {
    var _PaymentCRUDViewModel = PreparedFormObj();
    PaymentDetailCRUDViewModel.PaymentCRUDViewModel = _PaymentCRUDViewModel;

    $.ajax({
        type: "DELETE",
        url: "/Payment/DeletePaymentDetail",
        data: PaymentDetailCRUDViewModel,
        dataType: "json",
        success: function (result) {
            toastr.success("Payment details item has been deleted successfully. ID: " + result.Id, 'Success');
            UpdateSalesPaymentFields(result);
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}
