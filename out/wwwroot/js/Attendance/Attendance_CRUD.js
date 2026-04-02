var Details = function (id) {
  var url = "/Attendance/Details?id=" + id;
  OpenModalView(url, "modal-lg", 'Attendance Details');
};

var AddEdit = function (id) {
  var url = "/Attendance/AddEdit?id=" + id;
  var ModalTitle = "Add Attendance";
    if (id > 0) {
        ModalTitle = "Edit Attendance";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var Save = function () {
  if (!$("#frmAttendance").valid()) {
    return;
  }

  var _frmAttendance = $("#frmAttendance").serialize();
  $.ajax({
    type: "POST",
    url: "/Attendance/AddEdit",
    data: _frmAttendance,
    success: function (result) {
      Swal.fire({
        title: result,
        icon: "success",
      }).then(function () {
        document.getElementById("btnClose").click();
        $("#tblAttendance").DataTable().ajax.reload();
      });
    },
    error: function (errormessage) {
      SwalSimpleAlert(errormessage.responseText, "warning");
    },
  });
};

var Delete = function (id) {
  var _url = "/Attendance/IncomeType?id=" + id;
  var _message = "Attendance has been deleted successfully. Attendance ID: ";
  var _tableName = "tblAttendance";
  DeleteBase(_url, _message, _tableName);
};
