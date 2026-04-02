var DetailUnitsofMeasure = function (id) {
    var url = "/UnitsofMeasure/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Unit of Measure Details');
};


var AddEditUnitsofMeasure = function (id) {
    var url = "/UnitsofMeasure/AddEdit?id=" + id;
    var ModalTitle = "Add Unit of Measure";
    if (id > 0) {
        ModalTitle = "Edit Unit of Measure";
    }
    OpenModalView(url, "modal-lg", ModalTitle);
};

var SaveUnitsofMeasure = function () {
    if (!$("#frmUnitsofMeasure").valid()) {
        return;
    }

    var _frmUnitsofMeasure = $("#frmUnitsofMeasure").serialize();
    $("#btnSaveUnitsofMeasure").val("Please Wait");
    $('#btnSaveUnitsofMeasure').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/UnitsofMeasure/AddEdit",
        data: _frmUnitsofMeasure,
        success: function (result) {
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnCloseUnitsofMeasure").click();
                    $("#btnSaveUnitsofMeasure").val("Save");
                    $('#btnSaveUnitsofMeasure').removeAttr('disabled');
                    if (result.CurrentURL == "/Items/Index") {
                        $('#tblItem').DataTable().ajax.reload();
                    }
                    else {
                        $('#tblUnitsofMeasure').DataTable().ajax.reload();
                    }
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                    setTimeout(function () {
                        $('#Name').focus();
                    }, 400);
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var DeleteUnitsofMeasure = function (id) {
    var UIName = "UnitsofMeasure";
    var _url = `/${UIName}/Delete?id=` + id;
    var _tableName = `tbl${UIName}`;

    var subMessage = addSpaceToUppercase(UIName);
    var _message = `${subMessage} has been deleted successfully. ${subMessage} ID: `;
    DeleteBase(_url, _message, _tableName);
};
