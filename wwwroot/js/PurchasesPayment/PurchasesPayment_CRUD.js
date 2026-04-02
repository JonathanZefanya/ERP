var Details = function (id) {
    var url = "/PurchasesPayment/Details?id=" + id;
    OpenModalView(url, "modal-xl", 'Purchases Payment Details');
};

var ViewSupplierDetails = function (id) {
    var url = "/Supplier/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Supplier Details');
};

var POSReport = function (id) {
    var url = "/PurchasesPayment/POSReport?id=" + id;
    OpenModalView(url, "modal-md", 'POS Report');
};

var PrintPOSReport = function (id) {
    location.href = "/PurchasesPayment/PrintPOSReport?id=" + id;
};

var PrintPaymentInvoice = function (PaymentId) {
    location.href = "/PurchasesPayment/PrintPurchasesPaymentInvoice?_PaymentId=" + PaymentId;
};

var AddEdit = function (id) {
    var url = "/PurchasesPayment/AddEdit?id=" + id;
    var ModalTitle = "Add Purchases Payment";
    if (id > 0) {
        ModalTitle = "Edit Purchases Payment";
    }
    OpenModalView(url, "modal-xl", ModalTitle);

    localStorage.removeItem('PaymentId');
    localStorage.removeItem('CurrentURL');
};

var Delete = function (id) {
    Swal.fire({
        title: 'Do you want to delete this item?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "DELETE",
                url: "/PurchasesPayment/Delete?id=" + id,
                success: function (result) {
                    var message;
                    if (result.Category == 1) {
                        $('#tblPurchasesPayment').DataTable().ajax.reload();
                        message = "Purchases Invoice has been deleted successfully. Invoice ID: " + result.InvoiceNo;
                    }
                    else if (result.Category == 2) {
                        $('#tblPurchasesPaymentDraft').DataTable().ajax.reload();
                        message = "Purchases Draft Invoice has been deleted successfully. Draft InvoiceID: " + result.InvoiceNo;
                    }
                    else if (result.Category == 3) {
                        $('#tblPurchasesPaymentQuote').DataTable().ajax.reload();
                        message = "Purchases Quote has been deleted successfully. Quote ID: " + result.QuoteNo;
                    }

                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                        }
                    });
                }
            });
        }
    });
};

var AddNewCustomer = function () {
    activaTab('divAddNewCustomer');
};

$(document).ready(function () {
    $('.CustomerId').select2({
        ajax: {
            type: "GET",
            url: '/CustomerInfo/GetAllCustomerForDDL',
            dataType: 'json'
        },
    });
});


var GetPriceModel = function () {
    var _PriceModel = $("#PriceModel").val();
    $.ajax({
        type: "GET",
        url: "/PurchasesPayment/GetPriceModel?Id=" + _PriceModel,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data === null) return;
            var $dropdown = $('#ItemId');
            $('#ItemId').html('').select2({ data: [{ id: '', text: '' }] });
            $.each(data, function () {
                $dropdown.append($("<option />").val(this.Id).text(this.Name));
            });
        },
        error: function (response) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });

};


//$('#ItemBarcode').on('input', function (e) {
//    alert('Changed!')
//});

var GetItemByItemBarcode = function () {
    var _ItemBarcode = $("#ItemBarcode").val();

    if (_ItemBarcode == null || _ItemBarcode == '') return;

    $.ajax({
        type: "GET",
        url: "/PurchasesPayment/GetItemByItemBarcode?ItemBarcode=" + _ItemBarcode,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data === null) {
                Swal.fire({
                    title: "Item not found for Barcode: " + _ItemBarcode,
                    icon: 'info',
                    onAfterClose: () => {
                        setTimeout(function () {
                            $('#ItemBarcode').focus();
                        }, 300);
                    }
                });
                return;
            }

            $("#ItemId").val(data.Id);
            $('#ItemId').append(data.Id).trigger('change');
            UpdateUnitPrice();
            $('#Quantity').focus();
        },
        error: function (response) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });

};

var GetPaymentSummary = function (Id) {
    $.ajax({
        type: "GET",
        url: "/PurchasesPayment/GetPaymentSummary?Id=" + Id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            UpdatePurchasePaymentFields(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

var UpdatePurchasePaymentFields = function (PaymentCRUDViewModel) {
    $("#PaidAmount").val(PaymentCRUDViewModel.PaidAmount);
    $('#SumPaymentHistory').text(PaymentCRUDViewModel.PaidAmount);

    $("#ChangedAmount").val(PaymentCRUDViewModel.ChangedAmount);
    $("#SubTotal").val(PaymentCRUDViewModel.SubTotal);

    $("#GrandTotal").val(PaymentCRUDViewModel.GrandTotal.toFixed(2));
    $("#DueAmount").val(PaymentCRUDViewModel.DueAmount);


    $("#DiscountAmount").val(PaymentCRUDViewModel.DiscountAmount);
    $("#VATAmount").val(PaymentCRUDViewModel.VATAmount);

    var _SumPaymentItem = PaymentCRUDViewModel.GrandTotal - PaymentCRUDViewModel.CommonCharge;
    $("#SumPaymentItem").text(_SumPaymentItem);
}

//Future work
var GetSupplierHistory = function (CustomerId) {
    $.ajax({
        type: "GET",
        url: "/PurchasesPayment/GetCustomerHistory?CustomerId=" + CustomerId,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data === null) {
                $("textarea#CustomerNote").val('');
                return;
            }
            $("textarea#CustomerNote").val(data.CustomerNote);
            $("#PrevousBalance").val(data.PrevousBalance.toFixed(2));
        },
        error: function (response) {
            SwalSimpleAlert(response.responseText, "warning");
        }
    });
};