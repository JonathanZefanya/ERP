var Details = function (id) {
    var url = "/SubDepartment/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Sub Department Details');
};


var AddEdit = function (id) {
    var url = "/SubDepartment/AddEdit?id=" + id;
    var ModalTitle = "Add Sub Department";
    if (id > 0) {
        ModalTitle = "Edit Sub Department";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Delete = function (id) {
    var _url = "/SubDepartment/Delete?id=" + id;
    var _message = "Sub Department has been deleted successfully. Sub Department ID: ";
    var _tableName = "tblSubDepartment";
    DeleteBase(_url, _message, _tableName);
};

