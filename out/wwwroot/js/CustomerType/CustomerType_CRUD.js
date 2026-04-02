var Details = function (id) {
    var url = "/CustomerType/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Customer Type Details');
};


var AddEdit = function (id) {
    var url = "/CustomerType/AddEdit?id=" + id;
    var ModalTitle = "Add Customer Type";
    if (id > 0) {
        ModalTitle = "Edit Customer Type";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Save = function () {
    if (!$("#frmCustomerType").valid()) {
        return;
    }

    var _frmCustomerType = $("#frmCustomerType").serialize();
    $.ajax({
        type: "POST",
        url: "/CustomerType/AddEdit",
        data: _frmCustomerType,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $('#tblCustomerType').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var UIName = "CustomerType";
    var _url = `/${UIName}/Delete?id=` + id;
    var _tableName = `tbl${UIName}`;

    var subMessage = addSpaceToUppercase(UIName);
    var _message = `${subMessage} has been deleted successfully. ${subMessage} ID: `;
    DeleteBase(_url, _message, _tableName);
};