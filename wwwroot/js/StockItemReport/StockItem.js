
var GetStockItemReportData = function () {
    $.ajax({
        type: "GET",
        url: "/StockItemReport/GetAllStockItemReportData",
        success: function (data) {
            if (data.length > 0) {
                LoadHTMLTableData(data);
                AddSumRow(data);
                $('#NoDataMessage').html('');
            }
            else {
                $('#tblStockItem > tbody').empty();
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
    var trHTMLP1 = '';
    var trHTMLP2 = '';
    var trHTMLP3 = '';
    for (let i = 0; i < rowData.length; i = i + 3) {
        trHTMLP1 = '<tr>'
            + '<td>' + rowData[i].Id + '</td>'
            + '<td>' + rowData[i].SizeDisplay + '</td>'
            + '<td>' + rowData[i].Quantity + '</td>'
            + '<td>' + rowData[i].SubQuantity + '</td>'
            + '<td>' + rowData[i].WarehouseName + '</td>';

        if (i + 1 < rowData.length) {
            trHTMLP2 = '<td>' + rowData[i + 1].Id + '</td>'
                + '<td>' + rowData[i + 1].SizeDisplay + '</td>'
                + '<td>' + rowData[i + 1].Quantity + '</td>'
                + '<td>' + rowData[i + 1].SubQuantity + '</td>'
                + '<td>' + rowData[i + 1].WarehouseName + '</td>';
        }
        else {
            trHTMLP2 = '<td></td>'
                + '<td></td>'
                + '<td></td>'
                + '<td></td>'
                + '<td></td>';
        }

        if (i + 2 < rowData.length) {
            trHTMLP3 = '<td>' + rowData[i + 2].Id + '</td>'
                + '<td>' + rowData[i + 2].SizeDisplay + '</td>'
                + '<td>' + rowData[i + 2].Quantity + '</td>'
                + '<td>' + rowData[i + 2].SubQuantity + '</td>'
                + '<td>' + rowData[i + 2].WarehouseName + '</td>'
                + '</tr>';
        }
        else {
            trHTMLP3 = '<td></td>'
                + '<td></td>'
                + '<td></td>'
                + '<td></td>'
                + '<td></td>'
                + '</tr>';
        }
        trHTML += trHTMLP1 + trHTMLP2 + trHTMLP3;
    }

    $('#tblStockItem > tbody').empty();
    $('#tblStockItem > tbody').append(trHTML);
}

var AddSumRow = function (data) {
    if (data != null) {
        var _Quantity = data.reduce((n, { Quantity }) => n + Quantity, 0);
        var _SubQuantity = data.reduce((n, { SubQuantity }) => n + SubQuantity, 0);
        $('#TotalQuantity').html('Σ Total Quantity: ' + _Quantity);
        $('#TotalSubQuantity').html('Σ Total Sub Quantity: ' + _SubQuantity);
    }
}