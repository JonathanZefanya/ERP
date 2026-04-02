
var UpdateQuantity = function (id) {
    var url = "/Items/UpdateQuantity?id=" + id;
    OpenModalView(url, "modal-lg", 'Update Quantity. Item ID: ' + id);
};

var SaveUpdateQuantity = function () {
    if (!$("#frmAddQuantity").valid()) {
        return;
    }

    $("#btnAddQuantity").val("Please Wait");
    $('#btnAddQuantity').attr('disabled', 'disabled');

    var _frmAddQuantity = $("#frmAddQuantity").serialize();
    $.ajax({
        type: "POST",
        url: "/Items/SaveUpdateQuantity",
        data: _frmAddQuantity,
        success: function (result) {
            $("#btnAddQuantity").val("Add Customer");
            $('#btnAddQuantity').removeAttr('disabled');

            if (result.IsSuccess == true) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnCloseAddQuantity").click();
                    $('#tblItem').DataTable().ajax.reload();
                });
            }
            else {
                FieldValidationAlert('#AddNewQuantity', result.AlertMessage, "warning");
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}