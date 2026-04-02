var Details = function (id) {
    var url = "/ItemRequest/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Item Request Details');
};


var AddEdit = function (id) {
    var url = "/ItemRequest/AddEdit?id=" + id;
    var ModalTitle = "Add Item Request";
    if (id > 0) {
        ModalTitle = "Edit Item Request";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Save = function () {
    if (!$("#frmItemRequest").valid()) {
        return;
    }

    var _RequestQuantity = $("#RequestQuantity").val();
    var _CurrentStock = $("#CurrentStock").val();

    if (parseFloat(_RequestQuantity) > parseFloat(_CurrentStock)) {
        var message = "Stock limit exceeded. Please check. Current Stock: " + _CurrentStock;
        FieldValidationAlert('#RequestQuantity', message, "warning");
        return;
    }

    var _frmItemRequest = $("#frmItemRequest").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/ItemRequest/AddEdit",
        data: _frmItemRequest,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblItemRequest').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var _url = "/ItemRequest/Delete?id=" + id;
    var _message = "Item Request has been deleted successfully. Item Request ID: ";
    var _tableName = "tblItemRequest";
    DeleteBase(_url, _message, _tableName);
};


var UpdateCurrentStock = function () {
    var _ItemId = $("#ItemId option:selected").text();
    var splitArray = _ItemId.split(":");
    $("#CurrentStock").val(parseFloat(splitArray[12]));
}


var CheckCurrentStock = function () {
    var _CurrentTotalStock = $("#CurrentTotalStock").val();
    var _TotalTransferItem = $("#TotalTransferItem").val();

    if (parseFloat(_TotalTransferItem) > parseFloat(_CurrentTotalStock)) {
        strCurrentStockMessage = "Transfer item can not be greater than the current total stock.";
        FieldValidationAlert('#TotalTransferItem', strCurrentStockMessage, "warning");
    }
};

var TransferRequest = function (id) {
    var url = "/ItemRequest/TransferRequestItem?id=" + id;
    OpenModalView(url, "modal-xl", 'Transfer Item');
    //UpdateItemRequestStatus(id);
}

var SaveTransferRequestItem = function () {
    if (!$("#frmTransferItem").valid()) {
        return;
    }

    $("#btnSave").val("Transfering...");
    $('#btnSave').attr('disabled', 'disabled');
    var _frmTransferItem = $("#frmTransferItem").serialize();

    $.ajax({
        type: "POST",
        url: "/ItemRequest/SaveTransferRequestItem",
        data: _frmTransferItem,
        dataType: "json",
        success: function (result) {
            $("#btnSave").val("Transfer Item");
            $('#btnSave').removeAttr('disabled');

            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $('#tblItemRequest').DataTable().ajax.reload();
                });
            }
            else {
                FieldValidationAlert('#TotalTransferItem', result.AlertMessage, "warning");
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}


var UpdateItemRequestStatus = function (id) {
    $.ajax({
        type: "POST",
        url: "/ItemRequest/UpdateItemRequestStatus?id=" + id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data === null) return;
            $('#tblItemRequest').DataTable().ajax.reload();
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}