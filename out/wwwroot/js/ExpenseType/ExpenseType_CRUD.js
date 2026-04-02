var Details = function (id) {
    var url = "/ExpenseType/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Expense Type Details');
};

var AddEdit = function (id) {
    var url = "/ExpenseType/AddEdit?id=" + id;
    var ModalTitle = "Add Expense Type";
    if (id > 0) {
        ModalTitle = "Edit Expense Type";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Save = function () {
    if (!$("#frmExpenseType").valid()) {
        return;
    }

    var _frmExpenseType = $("#frmExpenseType").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/ExpenseType/AddEdit",
        data: _frmExpenseType,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblExpenseType').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var _url = "/ExpenseType/Delete?id=" + id;
    var _message = "Expense Type has been deleted successfully. Expense Type ID: ";
    var _tableName = "tblExpenseType";
    DeleteBase(_url, _message, _tableName);
};

