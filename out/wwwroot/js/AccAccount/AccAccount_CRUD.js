var Details = function (id) {
    var url = "/AccAccount/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Account Details');
};

var AddEdit = function (id) {
    var url = "/AccAccount/AddEdit?id=" + id;
    var ModalTitle = "Add ccount";
    if (id > 0) {
        ModalTitle = "Edit Account";
    }
    OpenModalView(url, "modal-lg", ModalTitle);

    setTimeout(function () {
        if (id > 0) {
            $('#Balance').prop('disabled', true);
        }
        $('#AccountName').focus();
    }, 400);
};

var Save = function () {
    const apiUrl = "/AccAccount/AddEditSave";
    SaveBase(apiUrl, "frmAccAccount", "tblAccAccount");
}

var Disable = function (id) {
    var _url = "/AccAccount/Delete?id=" + id;
    var _message = "Account has been Disabled successfully. Account ID: ";
    var _tableName = "tblAccAccount";
    DeleteBase(_url, _message, _tableName);
};
