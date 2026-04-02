var Details = function (id) {
    var url = "/VatPercentage/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Vat Percentage Details');
};


var AddEdit = function (id) {
    var url = "/VatPercentage/AddEdit?id=" + id;
    var ModalTitle = "Add Vat Percentage";
    if (id > 0) {
        ModalTitle = "Edit Vat Percentage";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Save = function () {
    if (!$("#frmVatPercentage").valid()) {
        return;
    }

    var _frmVatPercentage = $("#frmVatPercentage").serialize();
    $.ajax({
        type: "POST",
        url: "/VatPercentage/AddEdit",
        data: _frmVatPercentage,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $('#tblVatPercentage').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var UIName = "VatPercentage";
    var _url = `/${UIName}/Delete?id=` + id;
    var _tableName = `tbl${UIName}`;

    var subMessage = addSpaceToUppercase(UIName);
    var _message = `${subMessage} has been deleted successfully. ${subMessage} ID: `;
    DeleteBase(_url, _message, _tableName);
};