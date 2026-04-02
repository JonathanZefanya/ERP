
$(document).ready(function () {
    var _divReceiverEmail = document.getElementById("divReceiverEmail");
    _divReceiverEmail.style.display = "none";
});

$("body").on("click", "#chkTypeReceiverEmail", function () {
    var _divReceiverEmailId = document.getElementById("divReceiverEmailId");
    var _divReceiverEmail = document.getElementById("divReceiverEmail");

    if ($('#chkTypeReceiverEmail').is(":checked")) {
        _divReceiverEmailId.style.display = "none";
        _divReceiverEmail.style.display = "block";
        $('#ReceiverEmail').focus();
    }
    else {
        _divReceiverEmailId.style.display = "block";
        _divReceiverEmail.style.display = "none";
        $('#ReceiverEmailId').focus();
    }
});


var SendMailPaymentInvoice = async function (Id) {
    if (!$("#frmShare").valid()) {
        return;
    }

    $("#btnSend").val("Sending...");
    $('#btnSend').attr('disabled', 'disabled');


    var _PreparedfrmShareObj = await PreparedfrmShareObj();

    $("#btnSend").LoadingOverlay("show", {
        background: "rgba(165, 190, 100, 0.5)"
    });
    $("#btnSend").LoadingOverlay("show");

    $.ajax({
        type: "POST",
        url: "/PaymentShare/SendMailPaymentInvoice",
        data: _PreparedfrmShareObj,
        processData: false,
        contentType: false,
        success: function (result) {
            $("#btnSend").LoadingOverlay("hide", true);

            Swal.fire({
                title: result.AlertMessage,
                icon: "success"
            }).then(function () {
                $("#btnSend").val("Send");
                $('#btnSend').removeAttr('disabled');
                document.getElementById("btnClose").click();
            });
        },
        error: function (errormessage) {
            $("#btnSend").val("Send");
            $('#btnSend').removeAttr('disabled');
            errormessage = '<p align="left">' + errormessage.responseText + '</p>';
            Swal.fire({ title: errormessage.responseText, icon: 'warning' });
        }
    });
};


var EmailConfig = function () {
    var _SenderEmailId = $("#SenderEmailId").val();
    $('#titleEmailConfigModal').html("Edit Email Config");

    var url = "/EmailConfig/AddEdit?id=" + _SenderEmailId;
    $("#bodyEmailConfig").load(url, function () {
        $("#EmailConfigModal").modal("show");
    });
}

var closeEmailConfig = function () {
    $('#EmailConfigModal').modal('hide');
}

function PreparedInvoceiJsPDFData() {
    return new Promise((resolve, reject) => {
        const { jsPDF } = window.jspdf;
        var doc = new jsPDF();
        var _divPrint = document.getElementById('divPrintInvoice');
        doc.html(_divPrint, {
            callback: function (pdf) {
                var pdfDataUri = pdf.output('datauristring');
                resolve(pdfDataUri);
            },
            x: 16,
            y: 16,
            html2canvas: { scale: 0.11 },
        });
    });
}


const PreparedfrmShareObj = async () => {
    const formData = new FormData();
    const appendFormData = (key, value) => formData.append(key, value);
    appendFormData('InvoiceId', $("#InvoiceId").val());
    appendFormData('IsHideCompanyInfo', $("#IsHideCompanyInfo").val());
    appendFormData('InvoiceDocType', $("#InvoiceDocType").val());
    appendFormData('SenderEmailId', $("#SenderEmailId").val());
    appendFormData('Subject', $("#Subject").val());
    const body = $('#Body').val().replace(/(\r\n|\n|\r)/gm, '<br />');
    appendFormData('Body', body);

    const receiverEmail = $('#chkTypeReceiverEmail').is(":checked") ? $("#ReceiverEmail").val() : $("#ReceiverEmailId option:selected").text();
    appendFormData('ReceiverEmail', receiverEmail);

    try {
        const pdfDataUri = await PreparedInvoceiJsPDFData();
        formData.append('PDFDataURI', pdfDataUri);
    } catch (error) {
        console.error("Error generating PDF:", error);
    }

    return formData;
};