var Details = function (id) {
    var url = "/Designation/Details?id=" + id;
    OpenModalView(url, "modal-md", 'Designation Details');
};

var AddEdit = function (id) {
    var url = "/Designation/AddEdit?id=" + id;
    var ModalTitle = "Add Designation";
    if (id > 0) {
        ModalTitle = "Edit Designation";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Save = function () {
    if (!$("#frmDesignation").valid()) {
        return;
    }

    var _frmDesignation = $("#frmDesignation").serialize();
    $.ajax({
        type: "POST",
        url: "/Designation/AddEdit",
        data: _frmDesignation,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $('#tblDesignation').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var _url = "/Designation/IncomeType?id=" + id;
    var _message = "Designation has been deleted successfully. Designation ID: ";
    var _tableName = "tblDesignation";
    DeleteBase(_url, _message, _tableName);
};
