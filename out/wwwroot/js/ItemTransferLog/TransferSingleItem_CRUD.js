var TransferItem = function () {
    OpenModalView('/ItemTransferLog/TransferItem', "modal-lg", 'Transfer Item');
};

var SaveSingleTranItem = function () {
    if (!$("#frmTransferItem").valid()) {
        return;
    }

    $('#FromWarehouseId').removeAttr('disabled');
    $("#btnSave").val("Transfering...");
    $('#btnSave').attr('disabled', 'disabled');
    var _frmTransferItem = $("#frmTransferItem").serialize();

    $.ajax({
        type: "POST",
        url: "/ItemTransferLog/SaveSingleTranItem",
        data: _frmTransferItem,
        dataType: "json",
        success: function (result) {
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $('#FromWarehouseId').attr('disabled', 'disabled');
                    $("#btnSave").val("Transfer Item");
                    $('#btnSave').removeAttr('disabled');

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
                    $("#btnSave").val("Transfer Item");
                    $('#btnSave').removeAttr('disabled');
                    setTimeout(function () {
                        $('#ItemId').focus();
                    }, 400);
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}


function GetByItem() {
    var SelectItemValue = $("#ItemId").val();
    $.ajax({
        type: "GET",
        url: "/ItemTransferLog/GetByItem?Id=" + SelectItemValue,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data === null) return;
            WarehouseValidate();
            $('#CurrentTotalStock').val(data.Quantity);
            $('#FromWarehouseId').val(data.WarehouseId);
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}