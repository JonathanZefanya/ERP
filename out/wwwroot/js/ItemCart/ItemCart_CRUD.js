var GetAllCartItem = function () {
    $.ajax({
        type: "GET",
        url: "/ItemCart/GetAllCartItem",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            listPaymentDetail = result;
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        },
    });
};

var ItemCheckout = function () {
    if (listItemCart.length < 1) {
        FieldValidationAlert("#tblItemCart", "Please add at least one item.", "info");
        return;
    }

    ButtonEDLoader(false, "btnCheckout", 'Creating Invoice...');

    $.ajax({
        ContentType: "application/json; charset=utf-8",
        dataType: "json",
        type: "POST",
        url: "/ItemCart/CreateDraftItemCart",
        data: { listPaymentDetail: listItemCart },
        success: function (result) {
            ButtonEDLoader(true, "btnCheckout", 'Checkout');

            ClearAllCartItem();
            var url = "/Payment/AddEdit?id=" + result.Id;
            OpenModalView(url, "modal-xl", 'Add Payment');
        },
        error: function (errormessage) {
            ButtonEDLoader(true, "btnCheckout", 'Checkout');
            SwalSimpleAlert(errormessage.responseText, "warning");
        },
    });
};


var AddtoCart = function (_ItemId) {
    var _PaymentDetail = listPaymentDetail.filter((item) => item.Id == parseFloat(_ItemId));

    var AddedCartItem = {};
    AddedCartItem.Quantity = 1;
    AddedCartItem.ItemId = _PaymentDetail[0].Id;
    AddedCartItem.ItemName = _PaymentDetail[0].Name;
    AddedCartItem.UnitPrice = parseFloat(_PaymentDetail[0].SellPrice).toFixed(2);
    AddedCartItem.ItemVAT = _PaymentDetail[0].VatPercentage;
    AddedCartItem.ItemVATAmount = parseFloat(_PaymentDetail[0].NormalVAT);
    AddedCartItem.TotalAmount = parseFloat(_PaymentDetail[0].SellPrice);

    var _TotalPriceWithVAT = _PaymentDetail[0].SellPrice + _PaymentDetail[0].NormalVAT;
    AddedCartItem.TotalPriceWithVAT = parseFloat(_TotalPriceWithVAT).toFixed(2);

    var IsAddNewColumn = true;
    if (listItemCart.length > 0) {
        for (let i = 0; i < listItemCart.length; i++) {
            if (listItemCart[i].ItemId === parseFloat(_ItemId)) {
                IsAddNewColumn = false;

                var _Quantity_New = parseFloat($("#Quantity" + _ItemId).val()) + 1;
                $("#Quantity" + listItemCart[i].ItemId).val(_Quantity_New);


                var _TotalPrice = AddedCartItem.UnitPrice * _Quantity_New;
                var VatUnitAmount = ((AddedCartItem.ItemVAT / 100) * AddedCartItem.UnitPrice).toFixed(2);
                var _ItemVATAmount = VatUnitAmount * _Quantity_New
                var _TotalPriceWithVAT = _TotalPrice + _ItemVATAmount;
                $("#TotalPriceWithVAT" + listItemCart[i].ItemId).html(_TotalPriceWithVAT);

                listItemCart[i].Quantity = _Quantity_New;
                listItemCart[i].TotalAmount = _TotalPrice;
                listItemCart[i].TotalPriceWithVAT = _TotalPriceWithVAT;

                SubAndGrandTotal();
                localStorage.setItem("listItemCart", JSON.stringify(listItemCart));
                toastr.options.positionClass = "toast-bottom-right";
                toastr.success("Item added successfully. Item Id: " + _ItemId + ", Total Quantity: " + listItemCart[i].Quantity);
                return;
            }
        }
    }

    if (IsAddNewColumn) {
        listItemCart.push(AddedCartItem);
        AddTableRow(AddedCartItem, "#tblItemCart");
    }

    SubAndGrandTotal();
    $("#shoppingcart").html(listItemCart.length);
    toastr.options.positionClass = "toast-bottom-right";
    toastr.success("Item added successfully. Item Id: " + _ItemId);
    localStorage.setItem("listItemCart", JSON.stringify(listItemCart));
};

$("#ItemSearch").keyup(function () {
    var filter = $(this).val(),
        count = 0;
    $("#results div").each(function () {
        if ($(this).text().search(new RegExp(filter, "i")) < 0) {
            $(this).hide();
        } else {
            $(this).show();
            count++;
        }
    });
});

// Add Table Row
var AddTableRow = function (item, tableName) {
    const row = $(tableName + " > TBODY")[0].insertRow(-1);

    $(row).append(
        `<td>${item.ItemId}</td>
         <td>${item.ItemName}</td>
         <td><input type="number" id="Quantity${item.ItemId}" style="width:60px;" min="1" value="${item.Quantity}" onchange="UpdateQuantity(${item.ItemId})" /></td>
         <td>${item.UnitPrice}</td>
         <td>${item.ItemVAT}</td>

         <td><span id="TotalPriceWithVAT${item.ItemId}">${item.TotalPriceWithVAT}</span></td>
         <td><button class="btn btn-danger btn-xs" onclick="Remove(this)"><i class="fas fa-trash-alt"></i> Delete</button></td>`
    );
}

var Remove = function (button) {
    var row = $(button).closest("TR");
    var table = $("#tblItemCart")[0];
    table.deleteRow(row[0].rowIndex);
    var _ItemId = $("TD", row).eq(0).html();

    for (let i = 0; i < listItemCart.length; i++) {
        if (listItemCart[i].ItemId === parseFloat(_ItemId)) {
            listItemCart.splice(i, 1);
        }
    }

    SubAndGrandTotal();
    toastr.success("Item removed successfully.");
    $("#shoppingcart").html(listItemCart.length);
    localStorage.setItem("listItemCart", JSON.stringify(listItemCart));
}
var UpdateQuantity = function (itemId) {
    const cartItem = listItemCart.find((item) => item.ItemId === parseFloat(itemId));

    var _Quantity = parseFloat($("#Quantity" + itemId).val());

    var _TotalPrice = cartItem.UnitPrice * _Quantity;
    var VatUnitAmount = ((cartItem.ItemVAT / 100) * cartItem.UnitPrice).toFixed(2);
    var _ItemVATAmount = VatUnitAmount * _Quantity
    var _TotalPriceWithVAT = _TotalPrice + _ItemVATAmount;

    $("#TotalPriceWithVAT" + itemId).html(_TotalPriceWithVAT);

    for (let i = 0; i < listItemCart.length; i++) {
        if (listItemCart[i].ItemId === itemId) {
            listItemCart[i].Quantity = _Quantity;
            listItemCart[i].TotalAmount = _TotalPrice;
            listItemCart[i].ItemVATAmount = _ItemVATAmount;
            listItemCart[i].TotalPriceWithVAT = _TotalPriceWithVAT;
        }
    }
    localStorage.setItem("listItemCart", JSON.stringify(listItemCart));
    SubAndGrandTotal();
};

var SubAndGrandTotal = function () {
    let _SubTotal = 0;
    let _GrandTotal = 0;
    for (let i = 0; i < listItemCart.length; i++) {
        _SubTotal = _SubTotal + listItemCart[i].TotalAmount;
        _GrandTotal = _GrandTotal + parseFloat(listItemCart[i].TotalPriceWithVAT);
    }

    $("#ItemChartSubTotal").text("Sub Total: " + _SubTotal.toFixed(2));
    $("#ItemChartGrandTotal").text("Grand Total: " + _GrandTotal.toFixed(2));
};

var ClearAllCartItem = function () {
    if (listItemCart.length < 1) {
        SwalSimpleAlert("Cart alredy empty.", "info");
        return;
    }
    localStorage.removeItem("listItemCart");
    listItemCart = [];
    $("#tblItemCart > tbody").empty();
    $("#shoppingcart").html(0);

    $("#ItemChartSubTotal").text("Sub Total: 0.00");
    $("#ItemChartGrandTotal").text("Grand Total: 0.00");
    $("#Add_Paid_Amount_SI").val('');
    $("#ChangeAmountSI").val('');

    toastr.success("Cart clean successfully.");
};
