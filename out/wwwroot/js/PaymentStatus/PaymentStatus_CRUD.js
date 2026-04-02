var Details = function (id) {
    var url = "/PaymentStatus/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Payment Status Details');
};


var AddEdit = function (id) {
    var url = "/PaymentStatus/AddEdit?id=" + id;
    var ModalTitle = "Add Payment Status";
    if (id > 0) {
        ModalTitle = "Edit Payment Status";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Save = function () {
    if (!$("#frmPaymentStatus").valid()) {
        return;
    }

    var _frmPaymentStatus = $("#frmPaymentStatus").serialize();
    $.ajax({
        type: "POST",
        url: "/PaymentStatus/AddEdit",
        data: _frmPaymentStatus,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $('#tblPaymentStatus').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var UIName = "PaymentStatus";
    var _url = `/${UIName}/Delete?id=` + id;
    var _tableName = `tbl${UIName}`;

    var subMessage = addSpaceToUppercase(UIName);
    var _message = `${subMessage} has been deleted successfully. ${subMessage} ID: `;
    DeleteBase(_url, _message, _tableName);
};
