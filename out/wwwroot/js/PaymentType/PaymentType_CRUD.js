var Details = function (id) {
    var url = "/PaymentType/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Payment Type Details');
};


var AddEdit = function (id) {
    var url = "/PaymentType/AddEdit?id=" + id;
    var ModalTitle = "Add Payment Type";
    if (id > 0) {
        ModalTitle = "Edit Payment Type";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Save = function () {
    if (!$("#frmPaymentType").valid()) {
        return;
    }

    var _frmPaymentType = $("#frmPaymentType").serialize();
    $.ajax({
        type: "POST",
        url: "/PaymentType/AddEdit",
        data: _frmPaymentType,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $('#tblPaymentType').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var UIName = "PaymentType";
    var _url = `/${UIName}/Delete?id=` + id;
    var _tableName = `tbl${UIName}`;

    var subMessage = addSpaceToUppercase(UIName);
    var _message = `${subMessage} has been deleted successfully. ${subMessage} ID: `;
    DeleteBase(_url, _message, _tableName);
};
