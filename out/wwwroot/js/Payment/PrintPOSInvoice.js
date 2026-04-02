$(document).ready(function () {
    GenerateQRCode();
    setTimeout(function () {
        $("#btnPrintReceipt").trigger('click');
    }, 500);
});

var GenerateQRCode = function () {
    var _CompanyName = 'Seller: ' + $("#CompanyName").html();
    var _VatNumber = 'Vat No: ' + $("#VatNumber").html();
    var _CreatedDate = 'Date: ' + $("#CreatedDate").html();

    var _GrandTotal = 'Total: ' + $("#GrandTotal").html();
    var _VATAmount = 'Tax: ' + $("#VATAmount").html();

    var margeTextForQRCode = _CompanyName + '\n' + _VatNumber + '\n' + _CreatedDate + '\n' + _GrandTotal + '\n' + _VATAmount;
    var encodedmargeTextForQRCode = Base64.encode(margeTextForQRCode);

    $('#divQRCode').empty(); // Clear previous QR code

    var qrcode = new QRCode("divQRCode", {
        text: encodedmargeTextForQRCode,
        width: 300, // Adjust size as needed
        height: 300,
        colorDark: "#000000",
        colorLight: "#ffffff",
        correctLevel: QRCode.CorrectLevel.H
    });

    // Convert the canvas to an image
    setTimeout(function () {
        var canvas = $('#divQRCode canvas')[0];
        if (canvas) {
            var img = document.createElement('img');
            img.src = canvas.toDataURL("image/png"); // Convert canvas to data URL
            $('#divQRCode').html(img); // Replace the canvas with the image
        }
    }, 500); // Wait for the QR code to be generated
};

function printReceipt(width = '58mm', scale = 0.69) {
    // Get the printable area content
    var printableArea = document.getElementById('printableArea').innerHTML;

    // Open a new window or iframe for printing
    var printWindow = window.open('', '_blank');

    // Check if window.open() was blocked
    if (!printWindow || printWindow.closed || typeof printWindow.closed == 'undefined') {
        SwalSimpleAlert('Pop-up blocked! Please allow pop-ups for this site to print the receipt.', "warning");
        return;
    }

    // Write the content into the new window
    printWindow.document.write(`
        <html>
            <head>
                <style>
                    /* Embed your CSS directly here */
                    body {
                        margin: 0;
                        padding: 0;
                        width: ${width}; /* Use the parameterized width */
                        font-size: 12px;
                        -webkit-print-color-adjust: exact;
                    }

                    .body_thum {
                        width: ${width}; /* Use the parameterized width */
                        margin: 0 auto;
                        padding: 0;
                        box-sizing: border-box;
                        font-family: Arial, sans-serif;
                        font-size: 12px;
                        transform: scale(${scale}); /* Use the parameterized scale */
                        transform-origin: top left; /* Set the scaling origin */
                    }

                    .receipt {
                        width: 100%;
                        margin: 0;
                        padding: 0;
                        box-sizing: border-box;
                    }

                    .header {
                        text-align: center;
                        margin-bottom: 5mm;
                    }

                    .header img {
                        width: 60px;
                        height: auto;
                        margin-bottom: 3mm;
                    }

                    .details, .items, .totals {
                        margin-bottom: 3mm;
                    }

                    .details p, .totals p {
                        margin: 0;
                        line-height: 1.4;
                        font-size: 11px;
                    }

                    .items table {
                        width: 100%;
                        border-collapse: collapse;
                        font-size: 11px;
                    }

                    .items th, .items td {
                        text-align: left;
                        padding: 2mm 0;
                    }

                    .items th {
                        border-bottom: 1px solid #000;
                        font-weight: bold;
                    }

                    .items td {
                        border-bottom: 1px dashed #ccc;
                    }

                    .totals {
                        text-align: right;
                    }

                    .totals p {
                        font-size: 12px;
                        font-weight: bold;
                    }

                    .barcode, #divQRCode {
                        text-align: center;
                        margin-top: 5mm;
                    }

                    #divQRCode img {
                        width: 80%;
                        height: auto;
                    }

                    .print-button {
                        display: none;
                    }

                    .no-print {
                        display: none;
                    }
                </style>
            </head>
            <body>
                <div class="body_thum">
                    ${printableArea}
                </div>
            </body>
        </html>
    `);

    // Wait for the content to load, then print
    printWindow.document.close();
    printWindow.focus();
    
    // Add a small delay to ensure content is fully loaded
    setTimeout(function() {
        printWindow.print();
        printWindow.close();
    }, 250);
}
