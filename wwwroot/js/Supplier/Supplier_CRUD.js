var DetailSupplier = function (id) {
    var url = "/Supplier/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Supplier Details');
};


var AddEditSupplier = function (id) {
    var url = "/Supplier/AddEdit?id=" + id;
    var ModalTitle = "Add Supplier";
    if (id > 0) {
        ModalTitle = "Edit Supplier";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var SaveSupplier = function () {
    if (!$("#frmSupplier").valid()) {
        return;
    }

    var _frmSupplier = $("#frmSupplier").serialize();
    $("#btnSaveSupplier").val("Please Wait");
    $('#btnSaveSupplier').attr('disabled', 'disabled');

    $.ajax({
        type: "POST",
        url: "/Supplier/AddEdit",
        data: _frmSupplier,
        success: function (result) {
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnCloseSupplier").click();
                    $("#btnSaveSupplier").val("Save");
                    $('#btnSaveSupplier').removeAttr('disabled');
                    if (result.CurrentURL == "/Items/Index") {
                        $('#tblItem').DataTable().ajax.reload();
                    }
                    else {
                        $('#tblSupplier').DataTable().ajax.reload();
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

var DeleteSupplier = function (id) {
    var UIName = "Supplier";
    var _url = `/${UIName}/Delete?id=` + id;
    var _tableName = `tbl${UIName}`;

    var subMessage = addSpaceToUppercase(UIName);
    var _message = `${subMessage} has been deleted successfully. ${subMessage} ID: `;
    DeleteBase(_url, _message, _tableName);
};
