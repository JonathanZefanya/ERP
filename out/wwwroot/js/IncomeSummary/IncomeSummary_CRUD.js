var Details = function (id) {
    var url = "/IncomeSummary/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Income Summary Details');
};

var AddEdit = function (id) {
    var url = "/IncomeSummary/AddEdit?id=" + id;
    var ModalTitle = "Add Income Summary";
    if (id > 0) {
        ModalTitle = "Edit Income Summary";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
    setTimeout(function () {
        if (id > 0) {
            $('#Amount').prop('disabled', true);
        }
        $('#Title').focus();
    }, 400);
};

var Save = function () {
    const apiUrl = "/IncomeSummary/AddEditSave";
    SaveBase(apiUrl, "frmIncomeSummary", "tblIncomeSummary");
}

var Delete = function (id) {
    var _url = "/IncomeSummary/Delete?id=" + id;
    var _message = "Income Summary has been deleted successfully. Income Summary ID: ";
    var _tableName = "tblIncomeSummary";
    DeleteBase(_url, _message, _tableName);
};
