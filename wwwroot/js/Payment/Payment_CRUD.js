var Details = function (id) {
    var url = "/Payment/Details?id=" + id;
    OpenModalView(url, "modal-xl", 'Payment Details');
};

var ViewCustomerDetails = function (id) {
    var url = "/CustomerInfo/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Customer Info Details');
};

var POSReport = function (id) {
    var url = "/Payment/PrintPOSReport?id=" + id;
    window.open(url, '_blank');
};

var PrintPOSReport = function (id) {
    location.href = "/Payment/PrintPOSReport?id=" + id;
};

var PrintPaymentInvoice = function (PaymentId) {
    location.href = "/Payment/PrintPaymentInvoice?_PaymentId=" + PaymentId;
};

var AddEdit = function (id) {
    var url = "/Payment/AddEdit?id=" + id;
    var ModalTitle = "Add Payment";
    if (id > 0) {
        ModalTitle = "Edit Payment";
    }
    OpenModalView(url, "modal-xl", ModalTitle);

    localStorage.removeItem('PaymentId');
    localStorage.removeItem('CurrentURL');
};


var IsDraft = false;
var _Category = 1;
var SavePaymentDraft = function () {
    IsDraft = true;
    SavePayment();
    IsDraft = false;
}

var _IsSaveAndPrint = false;
var SaveAndPrintPayment = function () {
    _IsSaveAndPrint = true;
    SavePayment();
}

var SavePayment = function () {
    if (IsDraft) {
        _Category = 2;
    }
    else {
        var _CategoryTMP = $("#Category").val();
        if (_CategoryTMP == 2) {
            _Category = 1;
        }
        else {
            _Category = $("#Category").val();
        }
    }
    PaymentSaveIntoDB();
}

var SaveManualPayment = function () {
    _Category = 4;
    PaymentSaveIntoDB();
}



var PaymentSaveIntoDB = function () {
    if (!$("#frmAddEditPayment").valid()) {
        return;
    }
    var _PreparedFormObj = PreparedFormObj();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');

    $.ajax({
        type: "POST",
        url: "/Payment/AddEdit",
        data: _PreparedFormObj,
        dataType: "json",
        success: function (result) {
            if (result.IsSuccess) {
                if (result.ModelObject.IsSaveAndPrint == true) {
                    location.href = "/Payment/PrintPaymentInvoice?_PaymentId=" + result.Id + "&IsSaveAndPrint=" + true;
                    return;
                }

                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $("#btnSave").val("Save");
                    $('#btnSave').removeAttr('disabled');
                    if (result.CurrentURL == "/Payment/Index" && result.ModelObject.Category == 1) {
                        $('#tblPayments').DataTable().ajax.reload();
                    }
                    else if (result.CurrentURL == "/Payment/Index" && result.ModelObject.Category == 2) {
                        location.href = "/PaymentDraft/Index";
                    }
                    else if (result.CurrentURL == "/PaymentDraft/Index" && result.ModelObject.Category == 1) {
                        location.href = "/Payment/PrintPaymentInvoice?_PaymentId=" + result.Id;
                    }
                    else if (result.CurrentURL == "/PaymentDraft/Index" && result.ModelObject.Category == 2) {
                        $('#tblPaymentDraft').DataTable().ajax.reload();
                    }
                    else if (result.CurrentURL == "/Payment/Index" && result.ModelObject.Category == 3) {
                        location.href = "/PaymentQuote/Index";
                    }
                    else if (result.CurrentURL == "/PaymentQuote/Index" && result.ModelObject.Category == 3) {
                        $('#tblQuoteInvoice').DataTable().ajax.reload();
                    }
                    else if (result.CurrentURL == "/PaymentManual/Index" && result.ModelObject.Category == 4) {
                        $('#tblPaymentsManual').DataTable().ajax.reload();
                    }
                    else if (result.CurrentURL == "/ItemCart/Index" || result.CurrentURL == "/ItemCart/ItemCartSideInvoice" && result.ModelObject.Category == 1) {
                        location.href = "/Payment/PrintPaymentInvoice?_PaymentId=" + result.Id;
                    }
                    else if (result.CurrentURL == "/") {
                        setTimeout(function () {
                            $("#tblPayments").load("/ #tblPayments");
                        }, 1000);
                    }
                    else {
                        $('#tblPayments').DataTable().ajax.reload();
                    }
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                    $("#btnSave").val("Save");
                    $('#btnSave').removeAttr('disabled');
                    setTimeout(function () {
                        $('#ItemId').focus();
                    }, 400);
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}


var Delete = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
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
                url: "/Payment/Delete?id=" + id,
                success: function (result) {
                    var message;
                    if (result.Category == 1) {
                        $('#tblPayments').DataTable().ajax.reload();
                        message = "Invoice has been deleted successfully. Invoice ID: " + result.InvoiceNo;
                    }
                    else if (result.Category == 2) {
                        $('#tblPaymentDraft').DataTable().ajax.reload();
                        message = "Draft Invoice has been deleted successfully. Draft InvoiceID: " + result.InvoiceNo;
                    }
                    else if (result.Category == 3) {
                        $('#tblQuoteInvoice').DataTable().ajax.reload();
                        message = "Quote has been deleted successfully. Quote ID: " + result.QuoteNo;
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
        url: "/Payment/GetPriceModel?Id=" + _PriceModel,
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


var GetItemByItemBarcode = function (_ItemBarcode) {
    if (_ItemBarcode == null || _ItemBarcode == '') return;

    $.ajax({
        type: "GET",
        url: "/Payment/GetItemByItemBarcode?ItemBarcode=" + _ItemBarcode,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (!data) {
                FieldValidationAlert('#ItemBarcode', 'Item not found for Barcode: ' + _ItemBarcode, "info");
                return;
            }

            $("#ItemId").val(data.Id);
            $('#ItemId').append(data.Id).trigger('change');
            UpdateUnitPrice();
            $("#ItemBarcode").val('');
            $('#Quantity').focus();
        },
        error: function (response) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });

};

var GetPaymentSummaryData = function () {
    $.ajax({
        type: "GET",
        url: "/Payment/GetPaymentSummaryData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data === null) return;

            $("#ReportSubTotal").text(data.SubTotal.toFixed(2));
            $("#ReportDiscountAmount").text(data.DiscountAmount.toFixed(2));
            $("#ReportVATAmount").text(data.VATAmount.toFixed(2));
            $("#ReportGrandTotal").text(data.GrandTotal.toFixed(2));

            $("#ReportDueAmount").text(data.DueAmount.toFixed(2));
            $("#ReportPaidAmount").text(data.PaidAmount.toFixed(2));

            $("#ReportChangedAmount").text(data.ChangedAmount.toFixed(2));
            $("#ReportGrandTotalCashflow").text(data.GrandTotal.toFixed(2));
        },
        error: function (response) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
};

var GetCustomerHistory = async function (CustomerId) {
    $.ajax({
        type: "GET",
        url: "/Payment/GetCustomerHistory?CustomerId=" + CustomerId,
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

var GetPaymentSummary = function (Id) {
    $.ajax({
        type: "GET",
        url: "/Payment/GetPaymentSummary?Id=" + Id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            UpdateSalesPaymentFields(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

var UpdateSalesPaymentFields = function (PaymentCRUDViewModel) {
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


var PreparedFormObj = function () {
    var _PaymentCRUDViewModel = {
        Id: $("#Id").val(),
        CustomerId: $("#CustomerId").val(),
        InvoiceNo: $("#InvoiceNo").val(),
        QuoteNo: $("#QuoteNo").val(),
        CreatedDate: $("#CreatedDate").val(),
        CreatedBy: $("#CreatedBy").val(),

        CommonCharge: $("#CommonCharge").val(),
        Discount: $("#Discount").val(),
        DiscountAmount: $("#DiscountAmount").text(),
        VAT: $("#VAT").val(),
        TaxAmount: $("#TaxAmount").text(),
        SubTotal: $("#SubTotal").val(),
        GrandTotal: $("#GrandTotal").val(),
        PaidAmount: $("#PaidAmount").val(),
        DueAmount: $("#DueAmount").val(),
        ChangedAmount: $("#ChangedAmount").val(),
        CurrencyId: $("#CurrencyId").val(),
        BranchId: $("#BranchId").val(),
        PaymentStatus: $("#PaymentStatus").val(),
        CurrentURL: $("#CurrentURL").val(),
        Category: _Category,

        PurchaseOrderNumber: $("#PurchaseOrderNumber").val(),
        CustomerNote: $("#CustomerNote").val(),
        PrivateNote: $("#PrivateNote").val(),
        ReferenceNumber: $("#ReferenceNumber").val(),
        IsSaveAndPrint: _IsSaveAndPrint,
    };
    return _PaymentCRUDViewModel;
}