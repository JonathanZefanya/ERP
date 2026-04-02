var Details = function (id) {
    var url = "/Department/Details?id=" + id;
    OpenModalView(url, "modal-md", 'Department Details');
};


var AddEdit = function (id) {
    var url = "/Department/AddEdit?id=" + id;
    var ModalTitle = "Add Department";
    if (id > 0) {
        ModalTitle = "Edit Department";
    }
    OpenModalView(url, "modal-md", ModalTitle);
};

var Delete = function (id) {
    var _url = "/Department/Delete?id=" + id;
    var _message = "Department has been deleted successfully. Department ID: ";
    var _tableName = "tblDepartment";
    DeleteBase(_url, _message, _tableName);
};
