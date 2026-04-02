var Details = function (id) {
    var url = "/Currency/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Currency Details');
};

var AddEdit = function (id) {
    var url = "/Currency/AddEdit?id=" + id;
    var ModalTitle = "Add Currency";
    if (id > 0) {
        ModalTitle = "Edit Currency";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Save = function () {
    if (!$("#frmCurrency").valid()) {
        return;
    }

    var _frmCurrency = $("#frmCurrency").serialize();
    $.ajax({
        type: "POST",
        url: "/Currency/AddEdit",
        data: _frmCurrency,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $('#tblCurrency').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var UIName = "Currency";
    var _url = `/${UIName}/Delete?id=` + id;
    var _tableName = `tbl${UIName}`;

    var subMessage = addSpaceToUppercase(UIName);
    var _message = `${subMessage} has been deleted successfully. ${subMessage} ID: `;
    DeleteBase(_url, _message, _tableName);
};
