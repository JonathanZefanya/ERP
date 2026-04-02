var Details = function (id) {
    var url = "/CustomerInfo/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Customer Info Details');
};

var TranHistory = function (id) {
    var url = "/CustomerInfo/CustomerTranHistory?id=" + id;
    OpenModalView(url, "modal-lg", 'Customer Tran Summary');
};

var AddEdit = function (id) {
    var url = "/CustomerInfo/AddEdit?id=" + id;
    var ModalTitle = "Add Customer Info";
    if (id > 0) {
        ModalTitle = "Edit Customer Info";
    }
    OpenModalView(url, "modal-xl", ModalTitle);
};

var Save = function () {
    if (!$("#frmCustomerInfo").valid()) {
        return;
    }

    $("#btnAddCustomerInfo").val("Please Wait");
    $('#btnAddCustomerInfo').attr('disabled', 'disabled');

    var _frmCustomerInfo = $("#frmCustomerInfo").serialize();
    $.ajax({
        type: "POST",
        url: "/CustomerInfo/AddEdit",
        data: _frmCustomerInfo,
        success: function (result) {
            $("#btnAddCustomerInfo").val("Add Customer");
            $('#btnAddCustomerInfo').removeAttr('disabled');

            if (result.IsSuccess == true) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $('#tblCustomerInfo').DataTable().ajax.reload();
                });
            }
            else {
                if (result.AlertMessage.includes("Phone")) {
                    FieldValidationAlert('#Phone', result.AlertMessage, "warning");
                } else {
                    FieldValidationAlert('#Email', result.AlertMessage, "warning");
                }
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var UIName = "CustomerInfo";
    var _url = `/${UIName}/Delete?id=` + id;
    var _tableName = `tbl${UIName}`;

    var subMessage = addSpaceToUppercase(UIName);
    var _message = `${subMessage} has been deleted successfully. ${subMessage} ID: `;
    DeleteBase(_url, _message, _tableName);
};


var CheckCompanyPhone = function () {
    var _Phone = $("#Phone").val();
    var IsPhone = false;
    $.ajax({
        type: "GET",
        url: "/CustomerInfo/CheckCompanyPhone?_Phone=" + _Phone,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            IsPhone = result;
            return IsPhone;
        },
        error: function (response) {
            SwalSimpleAlert(response.responseText, "warning");
        }
    });
    return IsPhone;
};