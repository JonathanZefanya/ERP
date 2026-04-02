var onchangeExpenseType = function () {
    var _ExpenseTypeId = $("#ExpenseTypeId").val();
    $("#ExpenseDetailsId").val(_ExpenseTypeId);
}

var onchangeQuantity = function () {
    var _Quantity = $("#Quantity").val();
    var _UnitPrice = $("#UnitPrice").val();
    var _TotalAmount = parseFloat(_UnitPrice) * parseFloat(_Quantity);
    $("#TotalPrice").val(_TotalAmount.toFixed(2));
}

var AddExpenseDetails = function () {
    var _ExpenseTypeId = $("#ExpenseTypeId").val();
    
    AddHTMLTableRow(_ExpenseTypeId);
}

var onkeydownChargeAmount = function () {
    if (event.keyCode == 13) {
        event.preventDefault();
        AddExpenseDetails();
    }
}

var AddHTMLTableRow = function (result) {
    var tBody = $("#tblExpenseDetails > TBODY")[0];
    var row = tBody.insertRow(-1);

    var cell = $(row.insertCell(-1));
    cell.html(result.Id);

    var cell = $(row.insertCell(-1));
    var _ItemName = $("#ExpenseTypeId option:selected").text();
    var inputItemName = $("<textarea />");
    inputItemName.attr("rows", "1");
    inputItemName.attr('style', 'width:200px;');
    inputItemName.attr('id', 'ExpenseType' + result.Id);
    inputItemName.val(_ItemName);
    cell.append(inputItemName);

    var cell = $(row.insertCell(-1));
    var _Description = $("textarea#Description").val();
    var inputDescription = $("<textarea />");
    inputDescription.attr("rows", "1");
    inputDescription.attr('style', 'width:400px;');
    inputDescription.attr('id', 'Description' + result.Id);
    inputDescription.val(_Description);
    cell.append(inputDescription);


    var cell = $(row.insertCell(-1));
    var txtQuantity = $("<input />");
    txtQuantity.attr('style', 'width:70px;');
    txtQuantity.attr('type', 'number');
    txtQuantity.attr('min', '1');
    txtQuantity.attr('id', 'Quantity' + result.Id);
    txtQuantity.attr("onchange", "UpdateItemDynamicControl(this);");
    txtQuantity.val($("#Quantity").val());
    cell.append(txtQuantity);

    var cell = $(row.insertCell(-1));
    var _UnitPrice = $("#UnitPrice").val();
    var txtUnitPrice = $("<input />");
    txtUnitPrice.attr('style', 'width:70px;');
    txtUnitPrice.attr('type', 'number');
    txtUnitPrice.attr('min', '1');
    txtUnitPrice.attr('id', 'UnitPrice' + result.Id);
    txtUnitPrice.attr("onchange", "UpdateItemDynamicControl(this);");
    txtUnitPrice.val(parseFloat(_UnitPrice));
    cell.append(txtUnitPrice);

    var cell = $(row.insertCell(-1));
    var txtTotalPrice = $("<label />");
    txtTotalPrice.attr('class', 'not-bold');
    txtTotalPrice.attr('id', 'TotalPrice' + result.Id);
    txtTotalPrice.text($("#TotalPrice").val());
    cell.append(txtTotalPrice);


    cell = $(row.insertCell(-1));
    var btnUpdate = $("<input />");
    btnUpdate.attr("type", "button");
    btnUpdate.attr('class', 'btn btn-success btn-xs');
    btnUpdate.attr('id', 'btnUpdateExpenseDetails' + result.Id);
    btnUpdate.attr("onclick", "UpdateExpenseDetails(this);");
    btnUpdate.val("Update");
    cell.append(btnUpdate);

    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr('class', 'btn btn-danger btn-xs');
    btnRemove.attr("onclick", "Remove(this);");
    btnRemove.val(" X ");
    cell.append(btnRemove);

    $("#ExpenseTypeId").focus();
    ClearInvoiceItemTableRowData();
}


function LoadTableRowFromDB(item, index) {
    var tBody = $("#tblExpenseDetails > TBODY")[0];
    var row = tBody.insertRow(-1);

    var cell = $(row.insertCell(-1));
    cell.html(item.Id);

    var cell = $(row.insertCell(-1));
    var inputItemName = $("<textarea />");
    inputItemName.attr("rows", "1");
    inputItemName.attr('style', 'width:200px;');
    inputItemName.attr('id', 'ExpenseType' + item.Id);
    inputItemName.val(item.ExpenseType);
    cell.append(inputItemName);

    var cell = $(row.insertCell(-1));
    var inputDescription = $("<textarea />");
    inputDescription.attr("rows", "1");
    inputDescription.attr('style', 'width:400px;');
    inputDescription.attr('id', 'Description' + item.Id);
    inputDescription.val(item.Description);
    cell.append(inputDescription);


    var cell = $(row.insertCell(-1));
    var txtQuantity = $("<input />");
    txtQuantity.attr('style', 'width:70px;');
    txtQuantity.attr('type', 'number');
    txtQuantity.attr('min', '1');
    txtQuantity.attr('id', 'Quantity' + item.Id);
    txtQuantity.attr("onchange", "UpdateItemDynamicControl(this);");
    txtQuantity.val(item.Quantity);
    cell.append(txtQuantity);

    var cell = $(row.insertCell(-1));
    var txtUnitPrice = $("<input />");
    txtUnitPrice.attr('style', 'width:70px;');
    txtUnitPrice.attr('type', 'number');
    txtUnitPrice.attr('min', '1');
    txtUnitPrice.attr('id', 'UnitPrice' + item.Id);
    txtUnitPrice.attr("onchange", "UpdateItemDynamicControl(this);");
    txtUnitPrice.val(parseFloat(item.UnitPrice));
    cell.append(txtUnitPrice);

    var cell = $(row.insertCell(-1));
    var txtTotalAmount = $("<label />");
    txtTotalAmount.attr('class', 'not-bold');
    txtTotalAmount.attr('id', 'TotalPrice' + item.Id);
    txtTotalAmount.text(item.TotalPrice);
    cell.append(txtTotalAmount);


    cell = $(row.insertCell(-1));
    var btnUpdate = $("<input />");
    btnUpdate.attr("type", "button");
    btnUpdate.attr('class', 'btn btn-success btn-xs');
    btnUpdate.attr('id', 'btnUpdateExpenseDetails' + item.Id);
    btnUpdate.attr("onclick", "UpdateExpenseDetails(this);");
    btnUpdate.val("Update");
    cell.append(btnUpdate);

    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr('class', 'btn btn-danger btn-xs');
    btnRemove.attr("onclick", "Remove(this);");
    btnRemove.val(" X ");
    cell.append(btnRemove);

    $("#ExpenseTypeId").focus();
    ClearInvoiceItemTableRowData();
}


var ClearInvoiceItemTableRowData = function () {
    $("#ExpenseDetailsId").val("");
    $('#ExpenseTypeId').val(0).trigger('change');
    $("textarea#Description").val('');
    $("#Quantity").val(1);
    $("#UnitPrice").val(1);
    $("#TotalPrice").val(1);
}

function Remove(button) {
    Swal.fire({
        title: 'Do you want to delete this item?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            var row = $(button).closest("TR");
            var table = $("#tblExpenseDetails")[0];
            table.deleteRow(row[0].rowIndex);
            var _ExpenseDetailsId = $("TD", row).eq(0).html();          
            DeleteExpenseDetails(_ExpenseDetailsId);
        }
    });
}

var UpdateItemDynamicControl = function (button)
{
    var row = $(button).closest("TR");
    var _ItemId = $("TD", row).eq(0).html();
    var _Quantity = $("#Quantity" + _ItemId).val();
    var _UnitPrice = $("#UnitPrice" + _ItemId).val();

    var _TotalPrice = parseFloat(_Quantity) * parseFloat(_UnitPrice);
    $("TD", row).eq(5).html(_TotalPrice.toFixed(2));
}