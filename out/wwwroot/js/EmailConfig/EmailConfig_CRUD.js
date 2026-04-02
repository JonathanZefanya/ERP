var DetailsEmailConfig = function (id) {
    var url = "/EmailConfig/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Email Config Details');
};


var AddEditEmailConfig = function (id) {
    var url = "/EmailConfig/AddEdit?id=" + id;
    var ModalTitle = "Add Email Config";
    if (id > 0) {
        ModalTitle = "Edit Email Config";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var SaveEmailConfig = function () {
    if (!$("#frmEmailConfig").valid()) {
        return;
    }

    var _frmEmailConfig = $("#frmEmailConfig").serialize();
    $.ajax({
        type: "POST",
        url: "/EmailConfig/AddEdit",
        data: _frmEmailConfig,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnCloseEmailConfig").click();
                $('#tblEmailConfig').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var DeleteEmailConfig = function (id) {
    var _url = "/EmailConfig/Delete?id=" + id;
    var _message = "Email Config has been deleted successfully. Email Config ID: ";
    var _tableName = "tblEmailConfig";
    DeleteBase(_url, _message, _tableName);
};
