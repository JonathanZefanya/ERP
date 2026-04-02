var DetailCategories = function (id) {
    var url = "/Categories/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Categories Details');
};

var AddEditCategories = function (id) {
    var url = "/Categories/AddEdit?id=" + id;
    var ModalTitle = "Add Categories";
    if (id > 0) {
        ModalTitle = "Edit Categories";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var SaveCategories = function () {
    if (!$("#frmCategories").valid()) {
        return;
    }

    var _frmCategories = $("#frmCategories").serialize();
    $("#btnSaveCategories").val("Please Wait");
    $('#btnSaveCategories').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/Categories/AddEdit",
        data: _frmCategories,
        success: function (result) {
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnCloseCategories").click();
                    $("#btnSaveCategories").val("Save");
                    $('#btnSaveCategories').removeAttr('disabled');
                    if (result.CurrentURL == "/Items/Index") {
                        $('#tblItem').DataTable().ajax.reload();
                    }
                    else {
                        $('#tblCategories').DataTable().ajax.reload();
                    }
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                    setTimeout(function () {
                        $('#Name').focus();
                    }, 400);
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var UIName = "Categories";
    var _url = `/${UIName}/Delete?id=` + id;
    var _tableName = `tbl${UIName}`;

    var subMessage = addSpaceToUppercase(UIName);
    var _message = `${subMessage} has been deleted successfully. ${subMessage} ID: `;
    DeleteBase(_url, _message, _tableName);
};
