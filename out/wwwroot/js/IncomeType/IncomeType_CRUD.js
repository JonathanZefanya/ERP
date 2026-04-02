var Details = function (id) {
    var url = "/IncomeType/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Income Type Details');
};

var AddEdit = function (id) {
    var url = "/IncomeType/AddEdit?id=" + id;
    var ModalTitle = "Add Income Type";
    if (id > 0) {
        ModalTitle = "Edit Income Type";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Save = function () {
    const apiUrl = "/IncomeType/AddEdit";
    SaveBase(apiUrl, "frmIncomeType", "tblIncomeType");
}

var Delete = function (id) {
    var UIName = "IncomeType";
    var _url = `/${UIName}/Delete?id=` + id;
    var _tableName = `tbl${UIName}`;

    var subMessage = addSpaceToUppercase(UIName);
    var _message = `${subMessage} has been deleted successfully. ${subMessage} ID: `;
    DeleteBase(_url, _message, _tableName);
};
