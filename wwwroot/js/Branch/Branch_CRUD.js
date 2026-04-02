var Details = function (id) {
    var url = "/Branch/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Branch Details');
};


var AddEdit = function (id) {
    var url = "/Branch/AddEdit?id=" + id;
    var ModalTitle = "Add Branch";
    if (id > 0) {
        ModalTitle = "Edit Branch";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Save = function () {
    if (!$("#frmBranch").valid()) {
        return;
    }

    var _frmBranch = $("#frmBranch").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/Branch/AddEdit",
        data: _frmBranch,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblBranch').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var UIName = "Branch";
    var _url = `/${UIName}/Delete?id=` + id;
    var _tableName = `tbl${UIName}`;

    var subMessage = addSpaceToUppercase(UIName);
    var _message = `${subMessage} has been deleted successfully. ${subMessage} ID: `;
    DeleteBase(_url, _message, _tableName);
};
