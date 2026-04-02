var Details = function (id) {
    var url = "/IncomeCategory/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Income Category Details');
};

var AddEdit = function (id) {
    var url = "/IncomeCategory/AddEdit?id=" + id;
    var ModalTitle = "Add Income Category";
    if (id > 0) {
        ModalTitle = "Edit Income Category";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Save = function () {
    const apiUrl = "/IncomeCategory/AddEdit";
    SaveBase(apiUrl, "frmIncomeCategory", "tblIncomeCategory");
}

var Delete = function (id) {
    var _url = "/IncomeCategory/Delete?id=" + id;
    var _message = "Income Category has been deleted successfully. Income Category ID: ";
    var _tableName = "tblIncomeCategory";
    DeleteBase(_url, _message, _tableName);
};
