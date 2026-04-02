$(document).ready(function () {
    var _GetDivObject = GetDivObject();

    _GetDivObject.divCompanyBanner.style.display = "block";
    _GetDivObject.divCompanyBannerHideComInfo.style.display = "none";

    $('#titleInvoice').html("<b>INVOICE</b>");
    _GetDivObject.divPaymentItemDetails.style.display = "block";
    _GetDivObject.divTermsAndCondition.style.display = "block";
    _GetDivObject.divDeliveryNote.style.display = "none";
    _GetDivObject.divPickingList.style.display = "none";

    GenerateQRCode();

    setTimeout(function () {
        var _Category = $("#Category").val();
        if (_Category == 3) {
            _GetDivObject.divPaymentSummary.style.display = "none";
        }
        else {
            _GetDivObject.divPaymentSummary.style.display = "block";
        }

        var _IsSaveAndPrint = $("#IsSaveAndPrint").val();
        if (_IsSaveAndPrint == "True") {
            //window.print();
            $("#btnFullPagePrint").trigger('click');
        }
    }, 200);
});

var PrintPOSReport = function (id) {
    location.href = "/Payment/PrintPOSReport?id=" + id;
};

var OpenSendMailPaymentInvoice = function (id) {
    var _InvoiceDocType = $("#PrintDocType").val();
    var _HideCompanyInfo;

    if ($("#HideCompanyInfo").is(":checked")) {
        _HideCompanyInfo = 0;
    }
    else {
        _HideCompanyInfo = 1;
    }
    var url = "/PaymentShare/OpenSendMailPaymentInvoice?_PaymentId=" + id + "&_InvoiceDocType=" + _InvoiceDocType + "&_HideCompanyInfo=" + _HideCompanyInfo;
    OpenModalView(url, "modal-xl", 'Send Invoice');
};

var LoadDynamicPrintDoc = function () {
    var _PrintDocType = $("#PrintDocType").val();
    var _GetDivObject = GetDivObject();

    if (_PrintDocType == 1) {
        $('#titleInvoice').html("<b>INVOICE</b>");
        _GetDivObject.divPaymentItemDetails.style.display = "block";
        _GetDivObject.divPaymentSummary.style.display = "block";
        _GetDivObject.divTermsAndCondition.style.display = "block";

        _GetDivObject.divDeliveryNote.style.display = "none";
        _GetDivObject.divPickingList.style.display = "none";
    }
    else if (_PrintDocType == 2) {
        $('#titleInvoice').html("<b>Picking List</b>");
        _GetDivObject.divPaymentItemDetails.style.display = "block";
        _GetDivObject.divPaymentSummary.style.display = "none";
        _GetDivObject.divTermsAndCondition.style.display = "none";

        _GetDivObject.divDeliveryNote.style.display = "none";
        _GetDivObject.divPickingList.style.display = "block";
    }
    else if (_PrintDocType == 3) {
        $('#titleInvoice').html("<b>Delivery Note</b>");
        _GetDivObject.divPaymentItemDetails.style.display = "none";
        _GetDivObject.divPaymentSummary.style.display = "none";
        _GetDivObject.divTermsAndCondition.style.display = "none";

        _GetDivObject.divDeliveryNote.style.display = "block";
        _GetDivObject.divPickingList.style.display = "none";
    }

    var _Category = $("#Category").val();
    if (_Category == 3) {
        _GetDivObject.divPaymentSummary.style.display = "none";
    }
    else {
        _GetDivObject.divPaymentSummary.style.display = "block";
    }
}

$("body").on("click", "#HideCompanyInfo", function () {
    var _GetDivObject = GetDivObject();

    if ($('#HideCompanyInfo').is(":checked")) {
        _GetDivObject.divCompanyBanner.style.display = "none";
        _GetDivObject.divCompanyBannerHideComInfo.style.display = "block";
    }
    else {
        _GetDivObject.divCompanyBanner.style.display = "block";
        _GetDivObject.divCompanyBannerHideComInfo.style.display = "none";
    }
});

var GetDivObject = function () {
    var DivObject = {
        divPaymentItemDetails: document.getElementById("divPaymentItemDetails"),
        divPaymentSummary: document.getElementById("divPaymentSummary"),

        divDeliveryNote: document.getElementById("divDeliveryNote"),
        divPickingList: document.getElementById("divPickingList"),
        divTermsAndCondition: document.getElementById("divTermsAndCondition"),

        divCompanyBanner: document.getElementById("divCompanyBanner"),
        divCompanyBannerHideComInfo: document.getElementById("divCompanyBannerHideComInfo"),

    };
    return DivObject;
}

var GenerateQRCode = function () {
    //var _CompanyNumber = 'Company Reg: ' + $("#CompanyNumber").html();
    //var _InvoiceNo = 'INV: ' + $("#InvoiceNo").html();

    var _CompanyName = 'Seller: ' + $("#CompanyName").html();
    var _VatNumber = 'Vat No: ' + $("#VatNumber").html();
    var _CreatedDate = 'Date: ' + $("#CreatedDate").html();

    var _GrandTotal = 'Total: ' + $("#GrandTotal").html();
    var _VATAmount = 'Tax: ' + $("#VATAmount").html();


    var margeTextForQRCode = _CompanyName + '\n' + _VatNumber + '\n' + _CreatedDate + '\n' + _GrandTotal + '\n' + _VATAmount;
    var encodedmargeTextForQRCode = Base64.encode(margeTextForQRCode);
    //var decodedStr = Base64.decode(encodedStr);

    $('#divQRCode').empty();
    var qrcode = new QRCode("divQRCode", {
        text: encodedmargeTextForQRCode,
        width: 180, //default 128
        height: 180,
        colorDark: "#000000",
        colorLight: "#ffffff",
        correctLevel: QRCode.CorrectLevel.H
    });
}

