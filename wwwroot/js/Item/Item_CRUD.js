var ViewItem = function (Id) {
    var url = "/Items/ViewItem?Id=" + Id;
    OpenModalView(url, "modal-xl", 'Item Details');
};

var AddEdit = function (id) {
    var url = "/Items/AddEdit?id=" + id;
    var ModalTitle = "Add Item";
    if (id > 0) {
        ModalTitle = "Edit Item";
    }
    OpenModalView(url, "modal-xl", ModalTitle);
};

var Save = function () {
    if (!$("#frmItem").valid()) {
        return;
    }

    var _Name = $('#Name').val();
    if (_Name == '' ) {
        var _message = 'Please add item name first.';
        FieldValidationAlert('#Name', _message, "info");
        activaTab('divBasicInfo');
        return;
    }

    var _Quantity = $('#Quantity').val();
    if (parseFloat(_Quantity) < 1) {
        var _message = 'Please add quantity first.';
        FieldValidationAlert('#Quantity', _message, "info");
        return;
    }

    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');

    $.ajax({
        type: "POST",
        url: "/Items/AddEdit",
        data: PreparedFormObj(),
        processData: false,
        contentType: false,
        success: function (result) {
            $("#btnSave").val("Save");
            $('#btnSave').removeAttr('disabled');
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    if (result.CurrentURL == "/") {
                        setTimeout(function () {
                            $("#tblHome").load("/ #tblHome");
                        }, 1000);
                    } else if (result.CurrentURL == "/UserProfile/Index") {
                        $("#divUserProfile").load("/UserProfile/Index #divUserProfile");
                    } else {
                        $('#tblItem').DataTable().ajax.reload();
                    }
                });
            } else {
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

var DeleteItem = function (id) {
    if (DemoUserAccountLockAll() == 1) return;

    var _url = "/Items/Delete?id=" + id;
    var _message = "Item has been deleted successfully. Item Code: ";
    var _tableName = "tblItem";
    DeleteBase(_url, _message, _tableName);
};


var AddBulkItem = function () {
    OpenModalView('/Items/AddBulkItem', "modal-lg", 'Add Bulk Item');
};

var SaveBulkItem = function () {
    if (!$("#frmAttachmentFile").valid()) {
        return;
    }

    //E:\gitClone\posmt\AdvPOS\wwwroot\upload\BulkUploadItems

    $("#btnSaveBulkItem").val("Adding...");
    $('#btnSaveBulkItem').attr('disabled', 'disabled');
    var _frmAttachmentFile = $("#frmAttachmentFile").serialize();

    $.ajax({
        type: "POST",
        url: "/Items/SaveBulkItem",
        data: _frmAttachmentFile,
        success: function (result) {
            Swal.fire({
                title: result + ', Item added',
                icon: "success"
            }).then(function () {
                //$("#FilePath").val('');
                $("#btnSaveBulkItem").val("Add Bulk Item");
                $('#btnSaveBulkItem').removeAttr('disabled');
                $('#tblItem').DataTable().ajax.reload();
            });
        }
    });
};


var UpdateBarcode = function () {
    var _Code = $("#Code").val();
    if (_Code.length > 10) {
        FieldValidationAlert('#Code', 'Max lenght is 10.', "warning");
        return;
    }

    Swal.fire({
        title: 'Barcode Updated',
        icon: "success"
    }).then(function () {
        $("#Barcode").JsBarcode(_Code);
    });
}


var PreparedFormObj = function () {
    var _FormData = new FormData()
    _FormData.append('Id', $("#Id").val())
    _FormData.append('CreatedDate', $("#CreatedDate").val())
    _FormData.append('CreatedBy', $("#CreatedBy").val())
    _FormData.append('Name', $("#Name").val())
    _FormData.append('SupplierId', $("#SupplierId").val())
    _FormData.append('MeasureId', $("#MeasureId").val())
    _FormData.append('MeasureValue', $("#MeasureValue").val())

    _FormData.append('CostPrice', $("#CostPrice").val())
    _FormData.append('NormalPrice', $("#NormalPrice").val())
    _FormData.append('TradePrice', $("#TradePrice").val())
    _FormData.append('PremiumPrice', $("#PremiumPrice").val())
    _FormData.append('OtherPrice', $("#OtherPrice").val())

    _FormData.append('CostVAT', $("#CostVAT").val())
    _FormData.append('NormalVAT', $("#NormalVAT").val())
    _FormData.append('TradeVAT', $("#TradeVAT").val())
    _FormData.append('PremiumVAT', $("#PremiumVAT").val())
    _FormData.append('OtherVAT', $("#OtherVAT").val())


    _FormData.append('OldUnitPrice', $("#OldUnitPrice").val())
    _FormData.append('OldSellPrice', $("#OldSellPrice").val())
    _FormData.append('Quantity', $("#Quantity").val())
    _FormData.append('CategoriesId', $("#CategoriesId").val())
    _FormData.append('WarehouseId', $("#WarehouseId").val())
    _FormData.append('Note', $("#Note").val())
    _FormData.append('UpdateQntType', $("#UpdateQntType").val())

    _FormData.append('UpdateQntNote', $("#UpdateQntNote").val())
    _FormData.append('StockKeepingUnit', $("#StockKeepingUnit").val())
    _FormData.append('ManufactureDate', $("#ManufactureDate").val())
    _FormData.append('ExpirationDate', $("#ExpirationDate").val())
    _FormData.append('Code', $("#Code").val())
    _FormData.append('Barcode', $('#Barcode').attr('src'))
    _FormData.append('ProductLevel', $("#ProductLevel").val())
    _FormData.append('VatPercentage', $("#VatPercentage").val())
    _FormData.append('ImageURLDetails', $('#ImageURLDetails')[0].files[0])

    _FormData.append('Size', $("#Size").val())
    return _FormData;
}

var ViewItemHistory = function (ItemId) {
    var url = "/ItemsHistory/ViewItemHistory?ItemId=" + ItemId;
    OpenModalView(url, "modal-xl", 'Item History. Item ID: ' + ItemId);
};