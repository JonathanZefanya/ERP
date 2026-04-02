var Details = function (id) {
    var url = "/Employee/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Employee Details');
};


var AddEdit = function (id) {
    var url = "/Employee/AddEdit?id=" + id;
    var ModalTitle = "Add Employee";
    if (id > 0) {
        ModalTitle = "Edit Employee";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Delete = function (id) {
    var _url = "/Employee/Delete?id=" + id;
    var _message = "Employee has been deleted successfully. Employee ID: ";
    var _tableName = "tblEmployee";
    DeleteBase(_url, _message, _tableName);
};

