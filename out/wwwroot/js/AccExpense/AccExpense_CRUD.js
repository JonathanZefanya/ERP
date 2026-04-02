var Details = function (id) {
    var url = "/AccExpense/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Expense Details');
};

var AddEdit = function (id) {
    var url = "/AccExpense/AddEdit?id=" + id;
    var ModalTitle = "Add Expense";
    if (id > 0) {
        ModalTitle = "Edit Expense";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
    setTimeout(function () {
        if (id > 0) {
            $('#AccountId').prop('disabled', true);
            $('#Amount').prop('disabled', true);
        }
        $('#Name').focus();
    }, 400);
};

var Save = function () {
    const apiUrl = "/AccExpense/AddEditSave";
    SaveBase(apiUrl, "frmAccExpense", "tblAccExpense");
}

var Delete = function (id) {
    var _url = "/AccExpense/Delete?id=" + id;
    var _message = "Expense has been deleted successfully. Expense ID: ";
    var _tableName = "tblAccExpense";
    DeleteBase(_url, _message, _tableName);
};
