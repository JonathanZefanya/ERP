
var AddHTMLTableRow = function () {
    var tBody = $("#tblTransferMulipleItem > TBODY")[0];
    var row = tBody.insertRow(-1);

    var _ItemIdMultiple = $("#ItemIdMultiple").val();
    var cell = $(row.insertCell(-1));
    cell.html(_ItemIdMultiple);


    var _ItemIdMultipleText = $("#ItemIdMultiple option:selected").text();
    var splitArray = _ItemIdMultipleText.split(":");
    var cell = $(row.insertCell(-1));
    cell.html(splitArray[0]);

    var _FromWarehouseIdMultiple = $("#FromWarehouseIdMultiple option:selected").text();
    var cell = $(row.insertCell(-1));
    cell.html(_FromWarehouseIdMultiple);

    var _ToWarehouseIdMultiple = $("#ToWarehouseIdMultiple option:selected").text();
    var cell = $(row.insertCell(-1));
    cell.html(_ToWarehouseIdMultiple);

    var _TotalTransferItemMultiple = $("#TotalTransferItemMultiple").val();
    var cell = $(row.insertCell(-1));
    cell.html(_TotalTransferItemMultiple);

    var _ReasonOfTransferMultiple = $("#ReasonOfTransferMultiple").val();
    var cell = $(row.insertCell(-1));
    cell.html(_ReasonOfTransferMultiple);


    cell = $(row.insertCell(-1));
    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr('class', 'btn btn-danger btn-xs');
    btnRemove.attr("onclick", "Remove(this);");
    btnRemove.val(" X ");
    cell.append(btnRemove);

    var TranItem = {};
    TranItem.ItemId = _ItemIdMultiple;
    TranItem.FromWarehouseId = $("#FromWarehouseIdMultiple").val();
    TranItem.ToWarehouseId = $("#ToWarehouseIdMultiple").val();

    TranItem.CurrentTotalStock = $("#CurrentTotalStockMultiple").val();;
    TranItem.TotalTransferItem = _TotalTransferItemMultiple;
    TranItem.ReasonOfTransfer = _ReasonOfTransferMultiple;
    TranItem.CurrentURL = window.location.pathname;
    listTransferItemViewModel.push(TranItem);

    ClearTranItem();
}


var ClearTranItem = function () {
    $('#ItemIdMultiple').val(0).trigger('change');
    $('#FromWarehouseIdMultiple').val(0).trigger('change');
    $('#ToWarehouseIdMultiple').val(0).trigger('change');

    $("#CurrentTotalStockMultiple").val("");
    $("#TotalTransferItemMultiple").val("");
    $("#ReasonOfTransferMultiple").val("");

    setTimeout(function () {
        $("ItemIdMultiple").focus();
    }, 500);
}

function Remove(button) {
    var row = $(button).closest("TR");
    var table = $("#tblTransferMulipleItem")[0];
    table.deleteRow(row[0].rowIndex);

    var _ItemIdMultiple = $("TD", row).eq(0).html();
    var _ItemIdMultipleName = $("TD", row).eq(1).html();
    
    listTransferItemViewModel = listTransferItemViewModel.filter((item) => item.ItemId !== _ItemIdMultiple);
    toastr.success("Item removed successfully. Item Name: " + _ItemIdMultipleName, 'Success');
}