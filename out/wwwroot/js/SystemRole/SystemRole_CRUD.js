var AddNewRole = function () {
    if (DemoUserAccountLockAll() == 1) return;
    var url = "/SystemRole/AddNewRole";
    OpenModalView(url, "modal-lg", 'Add New Role');
};

var SaveAddNewRole = function () {
    if (DemoUserAccountLockAll() == 1) return;
    if (!$("#frmAddNewRole").valid()) {
        return;
    }

    var _frmAddNewRole = $("#frmAddNewRole").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/SystemRole/SaveAddNewRole",
        data: _frmAddNewRole,
        success: function (result) {
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $("#btnSave").val("Save");
                    $('#btnSave').removeAttr('disabled');
                    $('#tblSystemRole').DataTable().ajax.reload();
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                    setTimeout(function () {
                        $('#RoleName').focus();
                        $("#btnSave").val("Save");
                        $('#btnSave').removeAttr('disabled');
                    }, 400);
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var DeleteRole = function (id) {
    var UIName = "SystemRole";
    var _url = `/${UIName}/Delete?id=` + id;
    var _tableName = `tbl${UIName}`;

    var subMessage = addSpaceToUppercase(UIName);
    var _message = `${subMessage} has been deleted successfully. ${subMessage} ID: `;
    DeleteBase(_url, _message, _tableName);
};