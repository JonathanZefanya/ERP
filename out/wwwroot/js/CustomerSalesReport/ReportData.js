
var GetByCustomerReport = function (_CustomerId) {
    $.ajax({
        type: "GET",
        url: "/CustomerSalesReport/GetByCustomerReportData?CustomerId=" + _CustomerId,
        success: function (data) {
            var _Date = new Date().toISOString().slice(0, 10);

            $('#CustomerName').html(data.CustomerName);
            $('#CustomerAddress').html(data.CustomerAddress);
            $('#ReportDate').html(_Date);

            if (data.listCustomerReportViewModel.length > 0) {
                LoadHTMLTableData(data.listCustomerReportViewModel);
                AddSumRow(data.listCustomerReportViewModel);
                $('#NoDataMessage').html('');
            }
            else {
                $('#tblCustomerReport > tbody').empty();
                $('#NoDataMessage').html('<i class="fa fa-search" aria-hidden="true"> No Data Found</i>');
            }
        },
        error: function (response) {
            SwalSimpleAlert(response.responseText, "warning");
        }
    });
}

var LoadHTMLTableData = function (rowData) {
    var trHTML = '';
    $.each(rowData, function (i, item) {
        if (item != null) {
            trHTML += '<tr>'
                + '<td>' + item.Id + '</td>'
                + '<td>' + ConvertToShortDate(item.CreatedDate) + '</td>'
                + '<td>' + item.InvoiceNo + '</td>'
                + '<td>' + item.ReferenceNumber + '</td>'

                + '<td>' + item.ItemName + '</td>'
                + '<td>' + item.Quantity + '</td>'
                + '<td>' + item.SubQuantity + '</td>'
                + '<td>' + item.ItemPrice + '</td>'

                + '<td>' + item.Discount + '</td>'
                + '<td>' + item.SubTotal + '</td>'
                + '<td>' + item.GrandTotal + '</td>'
                + '<td>' + item.PaidAmount + '</td>'
                + '<td>' + item.DueAmount + '</td>'
                + '</tr>';
        }
    });
    $('#tblCustomerReport > tbody').empty();
    $('#tblCustomerReport > tbody').append(trHTML);
}


var AddSumRow = function (data) {
    var trHTML = '';
    if (data != null) {

        var _Quantity = data.reduce((n, { Quantity }) => n + Quantity, 0);
        var _SubQuantity = data.reduce((n, { SubQuantity }) => n + SubQuantity, 0);
        var _ItemPrice = data.reduce((n, { ItemPrice }) => n + ItemPrice, 0);
        var _Discount = data.reduce((n, { Discount }) => n + Discount, 0);

        var _SubTotal = data.reduce((n, { SubTotal }) => n + SubTotal, 0);
        var _GrandTotal = data.reduce((n, { GrandTotal }) => n + GrandTotal, 0);
        var _PaidAmount = data.reduce((n, { PaidAmount }) => n + PaidAmount, 0);
        var _DueAmount = data.reduce((n, { DueAmount }) => n + DueAmount, 0);

        trHTML += '<tr>'
            + '<td></td>'
            + '<td></td>'
            + '<td></td>'
            + '<td></td>'
            + '<td></td>'

            + '<td>Σ: ' + _Quantity + '</td>'
            + '<td>Σ: ' + _SubQuantity + '</td>'
            + '<td>Σ: ' + _ItemPrice.toFixed(2) + '</td>'
            + '<td>Σ: ' + _Discount.toFixed(2) + '</td>'

            + '<td>Σ: ' + _SubTotal.toFixed(2) + '</td>'
            + '<td>Σ: ' + _GrandTotal.toFixed(2) + '</td>'
            + '<td>Σ: ' + _PaidAmount.toFixed(2) + '</td>'
            + '<td>Σ: ' + _DueAmount.toFixed(2) + '</td>'
            + '</tr>';
    }

    $('#tblCustomerReport > tbody').append(trHTML);
}



