var Details = function (id) {
    var url = "/AccTransfer/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Transfer Details');
};


var AddEdit = function (id) {
    var url = "/AccTransfer/AddEdit?id=" + id;
    var ModalTitle = "Add Transfer";
    if (id > 0) {
        ModalTitle = "Edit Transfer";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
    setTimeout(function () {
        if (id > 0) {
            $('#SenderId').prop('disabled', true);
            $('#ReceiverId').prop('disabled', true);
            $('#Amount').prop('disabled', true);
        }
        $('#TransferDate').focus();
    }, 400);
};

var Save = function () {
    var _SenderId = $("#SenderId").val();
    var _ReceiverId = $("#ReceiverId").val();
    if (parseFloat(_SenderId) == parseFloat(_ReceiverId)) {
        FieldValidationAlert('#SenderId', 'Sender and Receiver ID Can not be Same.', "warning");
        return;
    }

    const apiUrl = "/AccTransfer/AddEditSave";
    SaveBase(apiUrl, "frmAccTransfer", "tblAccTransfer");
}

var Delete = function (id) {
    var _url = "/AccTransfer/Delete?id=" + id;
    var _message = "Transfer has been deleted successfully. Transfer ID: ";
    var _tableName = "tblAccTransfer";
    DeleteBase(_url, _message, _tableName);
};
