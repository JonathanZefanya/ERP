var DetailWarehouse = function (id) {
    var url = "/Warehouse/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Warehouse Details');
};


var AddEditWarehouse = function (id) {
    var url = "/Warehouse/AddEdit?id=" + id;
    var ModalTitle = "Add Warehouse";
    if (id > 0) {
        ModalTitle = "Edit Warehouse";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var SaveWarehouse = function () {
    if (!$("#frmWarehouse").valid()) {
        return;
    }

    var _frmWarehouse = $("#frmWarehouse").serialize();
    $("#btnSaveWarehouse").val("Please Wait");
    $('#btnSaveWarehouse').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/Warehouse/AddEdit",
        data: _frmWarehouse,
        success: function (result) {
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnCloseWarehouse").click();
                    $("#btnSaveWarehouse").val("Save");
                    $('#btnSaveWarehouse').removeAttr('disabled');
                    if (result.CurrentURL == "/Items/Index") {
                        $('#tblItem').DataTable().ajax.reload();
                    }
                    else {
                        $('#tblWarehouse').DataTable().ajax.reload();
                    }
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                    setTimeout(function () {
                        $('#Name').focus();
                    }, 400);
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var DeleteWarehouse = function (id) {
    var UIName = "Warehouse";
    var _url = `/${UIName}/Delete?id=` + id;
    var _tableName = `tbl${UIName}`;

    var subMessage = addSpaceToUppercase(UIName);
    var _message = `${subMessage} has been deleted successfully. ${subMessage} ID: `;
    DeleteBase(_url, _message, _tableName);
};