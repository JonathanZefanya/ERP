var listTransferItemViewModel = new Array();

var AddItem = function () {
    if (!FieldValidation('#ItemIdMultiple')) {
        FieldValidationAlert('#ItemIdMultiple', 'Item is Required.', "warning");
        return;
    }
    if (!FieldValidation('#TotalTransferItemMultiple')) {
        FieldValidationAlert('#TotalTransferItemMultiple', 'Tran Quantity is Required.', "warning");
        return;
    }
    if (!FieldValidation('#FromWarehouseIdMultiple')) {
        FieldValidationAlert('#FromWarehouseIdMultiple', 'From Warehouse is Required.', "warning");
        return;
    }
    if (!FieldValidation('#ToWarehouseIdMultiple')) {
        FieldValidationAlert('#ToWarehouseIdMultiple', 'To Warehouse is Required.', "warning");
        return;
    }

    var _ItemIdMultiple = $("#ItemIdMultiple").val();
    for (var i = 0; i < listTransferItemViewModel.length; i++) {
        if (listTransferItemViewModel[i].ItemId == _ItemIdMultiple) {
            FieldValidationAlert('#ItemIdMultiple', 'Item Already Added.', "warning");
            return;
        }
    }

    AddHTMLTableRow();
}

var WarehouseValidateForMultiple = function () {
    var _FromWarehouseId = $("#FromWarehouseIdMultiple").val();
    var _ToWarehouseId = $("#ToWarehouseIdMultiple").val();
    if (parseFloat(_FromWarehouseId) == parseFloat(_ToWarehouseId)) {
        Swal.fire({
            title: "Warehouse can't be same, please check.",
            icon: "warning"
        }).then(function () {
            setTimeout(function () {
                $("#ToWarehouseIdMultiple").val(0);
            }, 500);
        });
    }
}

var CheckCurrentStockForMultiple = function () {
    var _CurrentTotalStock = $("#CurrentTotalStockMultiple").val();
    var _TotalTransferItem = $("#TotalTransferItemMultiple").val();

    if (parseFloat(_TotalTransferItem) > parseFloat(_CurrentTotalStock)) {
        strCurrentStockMessage = "Transfer item can not be greater than the current total stock.";
        FieldValidationAlert('#TotalTransferItemMultiple', strCurrentStockMessage, "warning");
    }
};

function GetByItemForMultiTran() {
    var SelectItemValue = $("#ItemIdMultiple").val();
    $.ajax({
        type: "GET",
        url: "/ItemTransferLog/GetByItem?Id=" + SelectItemValue,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data === null) return;
            WarehouseValidate();
            $('#CurrentTotalStockMultiple').val(data.Quantity);
            $('#FromWarehouseIdMultiple').val(data.WarehouseId);
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}


var SaveMultiTranItem = function () {
    if (listTransferItemViewModel.length == 0) {
        FieldValidationAlert('#ItemIdMultiple', 'Please add at least one item first.', "warning");
        return;
    }

    var SendObject = {
        listTransferItemViewModel: listTransferItemViewModel,
    };

    $("#btnTransferAllItem").val("Please Wait");
    $('#btnTransferAllItem').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/ItemTransferLog/SaveMultiTranItem",
        data: SendObject,
        dataType: "json",
        success: function (result) {
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $("#btnTransferAllItem").val("Transfer All");
                    $('#btnTransferAllItem').removeAttr('disabled');

                    //bugx
                    if (result.CurrentURL == "/ItemTransferLog/Index") {
                        $('#tblItemTransferLog').DataTable().ajax.reload();
                    }
                    else {
                        location.reload();
                    }
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                    $("#btnTransferAllItem").val("Transfer All");
                    $('#btnTransferAllItem').removeAttr('disabled');
                    setTimeout(function () {
                        $('#btnTransferAllItem').focus();
                    }, 400);
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}