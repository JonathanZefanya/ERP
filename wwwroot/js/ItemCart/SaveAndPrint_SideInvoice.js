var ItemCheckoutSideInvoice = function () {
    if (listItemCart.length < 1) {
        FieldValidationAlert("#tblItemCart", "Please add at least one item.", "info");
        return;
    }

    ButtonEDLoader(false, "btnCheckout", 'Creating Invoice...');

    var _data = GetSaveAndPrintPaymentSIData();
    _data.listPaymentDetail = listItemCart;

    $.ajax({
        ContentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        url: "/ItemCart/CreateDraftItemCart",
        data: _data,
        success: function (result) {
            ButtonEDLoader(true, "btnCheckout", 'Checkout');

            ClearAllCartItem();
            var url = "/Payment/AddEdit?id=" + result.Id;
            OpenModalView(url, "modal-xl", 'Add Payment');
        },
        error: function (errormessage) {
            ButtonEDLoader(true, "btnCheckout", 'Checkout');
            SwalSimpleAlert(errormessage.responseText, "warning");
        },
    });
};



var SaveAndPrintSI = function () {
    if (listItemCart.length < 1) {
        FieldValidationAlert("#tblItemCart", "Please add at least one item.", "info");
        return;
    }
    if (!FieldValidation('#Add_Paid_Amount_SI')) {
        FieldValidationAlert("#Add_Paid_Amount_SI", "Payment Amount is Required", "info");
        return;
    }

    ButtonEDLoader(false, "btnPrintSI", 'Wait');

    var _data = GetSaveAndPrintPaymentSIData();
    $.ajax({
        ContentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        url: "/ItemCart/CreateDraftItemCart",
        data: _data,
        success: function (result) {
            ButtonEDLoader(true, "btnPrintSI", '<span class="fa fa-print"></span>Print');
            ClearAllCartItem();
            location.href = "/Payment/PrintPaymentInvoice?_PaymentId=" + result.Id + "&IsSaveAndPrint=" + true;
        },
        error: function (errormessage) {
            ButtonEDLoader(true, "btnPrintSI", '<span class="fa fa-print"></span>Print');
            SwalSimpleAlert(errormessage.responseText, "warning");
        },
    });
};

var SaveAndPrintThermalSI = function () {
    if (listItemCart.length < 1) {
        FieldValidationAlert("#tblItemCart", "Please add at least one item.", "info");
        return;
    }
    if (!FieldValidation('#Add_Paid_Amount_SI')) {
        FieldValidationAlert("#Add_Paid_Amount_SI", "Payment Amount is Required", "info");
        return;
    }

    ButtonEDLoader(false, "btnThermalPrintSI", 'Creating Invoice...');

    var _data = GetSaveAndPrintPaymentSIData();
    $.ajax({
        ContentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        url: "/ItemCart/CreateDraftItemCart",
        data: _data,
        success: function (result) {
            ButtonEDLoader(true, "btnThermalPrintSI", '<span class="fa fa-print"></span>Thermal Print');
            ClearAllCartItem();
            location.href = "/Payment/PrintPOSReport?id=" + result.Id;
        },
        error: function (errormessage) {
            ButtonEDLoader(true, "btnThermalPrintSI", '<span class="fa fa-print"></span>Thermal Print');
            SwalSimpleAlert(errormessage.responseText, "warning");
        },
    });
};

var GetDefaultAmount500 = function () {
    $("#Add_Paid_Amount_SI").val(500);
    UpdateChangeAmountSI();
}
var GetDefaultAmount1000 = function () {
    $("#Add_Paid_Amount_SI").val(1000);
    UpdateChangeAmountSI();
}
var GetDefaultAmount2000 = function () {
    $("#Add_Paid_Amount_SI").val(2000);
    UpdateChangeAmountSI();
}
var GetDefaultAmount5000 = function () {
    $("#Add_Paid_Amount_SI").val(5000);
    UpdateChangeAmountSI();
}

var GetDefaultAmountFullPaid = function () {
    var _ItemChartGrandTotal = $("#ItemChartGrandTotal").text();
    const _ItemChartGrandTotalVal = _ItemChartGrandTotal.split(":");
    $("#Add_Paid_Amount_SI").val(_ItemChartGrandTotalVal[1]);
    UpdateChangeAmountSI();
}

var UpdateChangeAmountSI = function () {
    var _ItemChartGrandTotal = $("#ItemChartGrandTotal").text();
    const _ItemChartGrandTotalVal = _ItemChartGrandTotal.split(":");
    var _Add_Paid_Amount_SI = parseFloat($("#Add_Paid_Amount_SI").val());

    var _ChangeAmount = 0;
    if (_Add_Paid_Amount_SI > parseFloat(_ItemChartGrandTotalVal[1])) {
        _ChangeAmount = _Add_Paid_Amount_SI - parseFloat(_ItemChartGrandTotalVal[1]);
    }
    else {
        _ChangeAmount = 0;
    }
    $("#ChangeAmountSI").val(_ChangeAmount.toFixed(2));
}

var GetSaveAndPrintPaymentSIData = function () {
    var _CustomerId = $("#CustomerId_SideInvoice").val();
    var _ItemChartSubTotal = $("#ItemChartSubTotal").text();
    const _ItemChartSubTotalVal = _ItemChartSubTotal.split(":");

    var _ItemChartGrandTotal = $("#ItemChartGrandTotal").text();
    const _ItemChartGrandTotalVal = _ItemChartGrandTotal.split(":");

    var _AmountSI = $("#Add_Paid_Amount_SI").val();
    var _ChangeAmountSI = $("#ChangeAmountSI").val();

    var ItemCartSideInvoiceViewModel = {};
    ItemCartSideInvoiceViewModel.CustomerId = _CustomerId;
    ItemCartSideInvoiceViewModel.SubTotal = _ItemChartSubTotalVal[1];
    ItemCartSideInvoiceViewModel.GrandTotal = _ItemChartGrandTotalVal[1];
    ItemCartSideInvoiceViewModel.PaidAmount = _AmountSI;
    ItemCartSideInvoiceViewModel.ChangedAmount = _ChangeAmountSI;
    ItemCartSideInvoiceViewModel.listPaymentDetail = listItemCart;
    ItemCartSideInvoiceViewModel.IsSaveAndPrint = true;

    return ItemCartSideInvoiceViewModel;
}