var Details = function (id) {
    var url = "/ManageUserRoles/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Role Details');
};


var AddEdit = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
    var url = "/ManageUserRoles/AddEdit?id=" + id;
    var ModalTitle = "Add Role";
    if (id > 0) {
        ModalTitle = "Edit Role";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Save = function () {
    if (DemoUserAccountLockAll() == 1) return;
    if (!$("#frmUserRoles").valid()) {
        return;
    }

    var _frmUserRoles = $("#frmUserRoles").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/ManageUserRoles/AddEdit",
        data: _frmUserRoles,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblManageUserRoles').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var _url = "/ManageUserRoles/Delete?id=" + id;
    var _message = "Role has been deleted successfully. Role ID: ";
    var _tableName = "tblManageUserRoles";
    DeleteBase(_url, _message, _tableName);
};
