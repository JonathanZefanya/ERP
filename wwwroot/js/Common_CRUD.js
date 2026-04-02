var AddNewCustomer = function () {
    activaTab('divAddNewCustomer');
};

var SaveCustomerInfo = function () {
    if (!FieldValidation('#Name')) {
        FieldValidationAlert('#Name', 'Name is Required.', "warning");
        return;
    }

    if (!FieldValidation('#Email')) {
        FieldValidationAlert('#Email', 'Email is Required.', "warning");
        return;
    }

    if (!FieldValidation('#Phone')) {
        FieldValidationAlert('#Phone', 'Phone is Required.', "warning");
        return;
    }

    $("#btnAddCustomerInfo").val("Please Wait");
    $('#btnAddCustomerInfo').attr('disabled', 'disabled');

    var CustomerInfoCRUDViewModel = {
        Name: $("#Name").val(),
        CompanyName: $("#CompanyName").val(),
        Type: $("#Type").val(),
        Phone: $("#Phone").val(),
        Email: $("#Email").val(),
        Address: $("textarea#Address").val(),
        AddressPostcode: $("#AddressPostcode").val(),
        BillingAddress: $("textarea#BillingAddress").val(),
        BillingAddressPostcode: $("#BillingAddressPostcode").val(),
        UseThisAsBillingAddress: $("#UseThisAsBillingAddress").val(),
        Notes: $("#Notes").val(),
    };

    $.ajax({
        type: "POST",
        url: "/CustomerInfo/AddEdit",
        data: CustomerInfoCRUDViewModel,
        success: function (result) {
            $("#btnAddCustomerInfo").val("Add Customer");
            $('#btnAddCustomerInfo').removeAttr('disabled');

            if (result.IsSuccess == true) {
                $('#CustomerId').append($('<option>', {
                    value: result.Id,
                    text: result.Name
                }));
                $('#CustomerId').val(result.Id);
                GetCustomerHistory(result.Id);
                toastr.success(result.AlertMessage, 'Success');
                activaTab('divBasicInfo');
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
};

var AddNewSupplier = function () {
    activaTab('divAddNewCustomer');
};

var SaveSupplier = function () {
    if (!FieldValidation('#Name')) {
        FieldValidationAlert('#Name', 'Name is Required.', "warning");
        return;
    }

    var SupplierCRUDViewModel = {
        Name: $("#Name").val(),
        ContactPerson: $("#ContactPerson").val(),
        Email: $("#Email").val(),
        Phone: $("#Phone").val(),
        Address: $("#Address").val(),
    };

    $.ajax({
        type: "POST",
        url: "/Supplier/AddEdit",
        data: SupplierCRUDViewModel,
        success: function (result) {
            $('#SupplierId').append($('<option>', {
                value: result.Id,
                text: result.Name
            }));
            $('#SupplierId').val(result.Id);
            toastr.success(result.AlertMessage, 'Success');
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
    activaTab('divBasicInfo');
};
