var Details = function (id) {
    var url = "/RefreshToken/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Refresh Token Details');
};

var ViewUserDetails = function (Id) {
    var url = "/UserManagement/ViewUserDetails?Id=" + Id;
    OpenModalView(url, "modal-lg", 'User Details');
};

var Delete = function (id) {
    var _url = "/RefreshToken/Delete?id=" + id;
    var _message = "Refresh Token has been deleted successfully. Refresh Token ID: ";
    var _tableName = "tblRefreshToken";
    DeleteBase(_url, _message, _tableName);
};