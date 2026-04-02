var Details = function (id) {
    var url = "/AccDeposit/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Deposit Details');
};

var AddEdit = function (id) {
    var url = "/AccDeposit/AddEdit?id=" + id;
    var ModalTitle = "Add Deposit";
    if (id > 0) {
        ModalTitle = "Edit Deposit";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
    setTimeout(function () {
        if (id > 0) {
            $('#AccountId').prop('disabled', true);
            $('#Amount').prop('disabled', true);
        }
        $('#DepositDate').focus();
    }, 400);
};

var Save = function () {
    const apiUrl = "/AccDeposit/AddEditSave";
    SaveBase(apiUrl, "frmAccDeposit", "tblAccDeposit");
}

var Delete = function (id) {
    var _url = "/AccDeposit/Delete?id=" + id;
    var _message = "Deposit has been deleted successfully. Deposit ID: ";
    var _tableName = "tblAccDeposit";
    DeleteBase(_url, _message, _tableName);
};
