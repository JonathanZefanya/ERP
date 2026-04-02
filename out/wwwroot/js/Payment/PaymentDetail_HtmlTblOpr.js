setTimeout(async () => {
    const companyInfo = await GetCompanyDataFromLS();
    $('#ItemDiscountTitle').html(companyInfo.IsItemDiscountPercentage ? 'Discount(%)' : 'Discount');
}, 300);

var UpdateUnitPrice = async () => {
    const selectedText = $("#ItemId option:selected").text();
    const splitArray = selectedText.split(":");
    GeneratePriceDropdown(splitArray);

    const isVat = $("#IsVat").val();
    $("#ItemVAT").val(isVat === "Yes" ? parseFloat(splitArray[10]) : 0);

    await onchangeQuantity();
};


var GeneratePriceDropdown = (splitArray) => {
    const priceOptions = [
        { value: splitArray[4], text: `Cost:${splitArray[4]}` },
        { value: splitArray[2], text: `Normal:${splitArray[2]}` },
        { value: splitArray[6], text: `Trade:${splitArray[6]}` },
        { value: splitArray[8], text: `Premium:${splitArray[8]}` }
    ];
    const $unitPrice = $("#UnitPrice").empty();
    priceOptions.forEach(option => {
        $("<option/>", option).appendTo($unitPrice);
    });
};

var onchangeUnitPrice = async () => {
    await onchangeQuantity();
};

var onchangeQuantity = async () => {
    const paidQuantity = getInputNumber("#Quantity");
    const freeQuantity = getInputNumber("#FreeQuantity");
    const totalQuantity = paidQuantity + freeQuantity;

    const unitPrice = getInputNumber("#UnitPrice");
    const itemDiscount = getInputNumber("#ItemDiscount");
    const itemVAT = getInputNumber("#ItemVAT");

    const companyInfo = await GetCompanyDataFromLS();

    const discountAmount = companyInfo && companyInfo.IsItemDiscountPercentage
        ? (itemDiscount / 100) * unitPrice
        : itemDiscount;

    let finalUnitPrice = unitPrice - discountAmount;
    finalUnitPrice += (itemVAT / 100) * finalUnitPrice;

    if (finalUnitPrice < 0) finalUnitPrice = 0;

    // Only paid quantity affects price
    const totalAmount = (finalUnitPrice * paidQuantity).toFixed(2);
    $("#TotalAmount").val(totalAmount);

    if ($("#TotalQuantity").length) {
        $("#TotalQuantity").val(totalQuantity);
    }
};

var onchangeItemDiscount = async () => {
    await onchangeQuantity();
};

var onkeydownChargeAmount = (event) => {
    if (event.keyCode === 13) {
        event.preventDefault();
        AddPaymentDetail();
    }
};

var AddPaymentsDetailsHTMLRow = () => {
    AddPaymentDetail();
    $("#ItemBarcode").val('');
};

var createInput = (type, id, value, style, min = null, onChange = null) => {
    const $input = $("<input />", { type, id, value, style });
    if (min !== null) $input.attr('min', min);
    if (onChange) $input.attr('onchange', onChange);
    return $input;
};

var AddHTMLTableRow = (result) => {
    const tBody = $("#tblPaymentDetail > TBODY")[0];
    const row = tBody.insertRow(-1);

    // ID Cell
    $(row.insertCell(-1)).html(result.Id);

    // Item Name Cell
    const splitArray = $("#ItemId option:selected").text().split(":");
    const $itemName = $("<textarea />", {
        rows: 1,
        style: 'width:400px;',
        id: `ItemName${result.Id}`,
        text: splitArray[0]
    });
    $(row.insertCell(-1)).append($itemName);

    // Paid Quantity Cell
    $(row.insertCell(-1)).append(
        createInput('number', `Quantity${result.Id}`, $("#Quantity").val(), 'width:70px;', 1, "UpdateItemDynamicControl(this);")
    );

    // Free Quantity Cell
    $(row.insertCell(-1)).append(
        createInput('number', `FreeQuantity${result.Id}`, $("#FreeQuantity").val() || 0, 'width:70px;', 0, "UpdateItemDynamicControl(this);")
    );

    // Unit Price Cell
    //let unitPrice = $("#UnitPrice option:selected").text().split(":")[1];
    let unitPrice = $("#UnitPrice").val();
    $(row.insertCell(-1)).append(
        createInput('number', `UnitPrice${result.Id}`, parseFloat(unitPrice), 'width:70px;', 1, "UpdateItemDynamicControl(this);")
    );

    // VAT Cell (if enabled)
    if ($("#IsVat").val() === "Yes") {
        $(row.insertCell(-1)).html($("#ItemVAT").val());
    }

    // Discount Cell
    $(row.insertCell(-1)).append(
        createInput('number', `ItemDiscount${result.Id}`, $("#ItemDiscount").val(), 'width:70px;', null, "UpdateItemDynamicControl(this);")
    );

    // Total Amount Cell
    $(row.insertCell(-1)).append(
        $("<label />", { class: 'not-bold', id: `TotalAmount${result.Id}`, text: $("#TotalAmount").val() })
    );

    // Action Buttons
    const $actionCell = $(row.insertCell(-1));
    $actionCell.append(
        createInput('button', `btnUpdatePaymentsDetail${result.Id}`, 'Update', 'btn btn-success btn-xs', null, null).attr('onclick', 'UpdatePaymentDetail(this);')
    );
    $actionCell.append(
        createInput('button', null, ' X ', 'btn btn-danger btn-xs', null, null).attr('onclick', 'Remove(this);')
    );
    $actionCell.append(
        $("<a />", { class: 'btn fa fa-id-card fa-3x', onclick: 'AddItemSerialNumber(this);' })
    );

    $("#ItemId").focus();
    ClearInvoiceItemTableRowData();
};

var LoadTableRowFromDB = (item) => {
    const tBody = $("#tblPaymentDetail > TBODY")[0];
    const row = tBody.insertRow(-1);

    $(row.insertCell(-1)).html(item.Id);

    $(row.insertCell(-1)).append(
        $("<textarea />", {
            rows: 1,
            style: 'width:450px;',
            id: `ItemName${item.Id}`,
            text: item.ItemName
        })
    );

    // Paid Quantity Cell
    $(row.insertCell(-1)).append(
        createInput('number', `Quantity${item.Id}`, item.Quantity, 'width:70px;', 1, "UpdateItemDynamicControl(this);")
    );

    // Free Quantity Cell
    $(row.insertCell(-1)).append(
        createInput('number', `FreeQuantity${item.Id}`, item.FreeQuantity || 0, 'width:50px;', 0, "UpdateItemDynamicControl(this);")
    );

    $(row.insertCell(-1)).append(
        createInput('number', `UnitPrice${item.Id}`, item.UnitPrice, 'width:70px;', 1, "UpdateItemDynamicControl(this);")
    );

    if ($("#IsVat").val() === "Yes") {
        $(row.insertCell(-1)).html(item.ItemVAT);
    }

    $(row.insertCell(-1)).append(
        createInput('number', `ItemDiscount${item.Id}`, item.ItemDiscount, 'width:70px;', null, "UpdateItemDynamicControl(this);")
    );

    $(row.insertCell(-1)).append(
        $("<label />", { class: 'not-bold', id: `TotalAmount${item.Id}`, text: item.TotalAmount })
    );

    const $actionCell = $(row.insertCell(-1));
    $actionCell.append(
        createInput('button', `btnUpdatePaymentsDetail${item.Id}`, 'Update', 'btn btn-success btn-xs').attr('onclick', 'UpdatePaymentDetail(this);')
    );
    $actionCell.append(
        createInput('button', null, ' X ', 'btn btn-danger btn-xs').attr('onclick', 'Remove(this);')
    );
    $actionCell.append(
        $("<a />", { class: 'btn fa fa-id-card fa-3x', onclick: 'AddItemSerialNumber(this);' })
    );
};

var ClearInvoiceItemTableRowData = () => {
    $("#PaymentsDetailsId").val("");
    $('#ItemId').val(0).trigger('change');
    $("#Quantity").val("1");
    $("#FreeQuantity").val("0");
    //$("#UnitPrice").val("");
    $('#UnitPrice').val(0).trigger('change');
    $("#ItemVAT").val("");
    $("#ItemDiscount").val(0);
    $("#TotalAmount").val("");
};

var Remove = (button) => {
    Swal.fire({
        title: 'Do you want to delete this item?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            const row = $(button).closest("TR");
            const table = $("#tblPaymentDetail")[0];
            table.deleteRow(row[0].rowIndex);
            const paymentDetailId = $("TD", row).eq(0).html();

            DeletePaymentDetail({
                Id: paymentDetailId,
                PaymentsId: $("#Id").val()
            });
        }
    });
};

var UpdateItemDynamicControl = (button) => {
    const row = $(button).closest("TR");
    const itemId = $("TD", row).eq(0).html();
    const quantity = parseFloat($(`#Quantity${itemId}`).val()) || 0;
    const freeQuantity = parseFloat($(`#FreeQuantity${itemId}`).val()) || 0;
    const unitPrice = parseFloat($(`#UnitPrice${itemId}`).val()) || 0;
    const itemDiscount = parseFloat($(`#ItemDiscount${itemId}`).val()) || 0;

    const isVat = $("#IsVat").val();
    const normalVAT = isVat === "Yes" ? parseFloat($("TD", row).eq(5).html()) || 0 : 0;

    // Only paid quantity affects price
    const totalPrice = quantity * unitPrice;
    const discountTotalAmount = (itemDiscount / 100) * totalPrice;
    const discountedTotalPrice = totalPrice - discountTotalAmount;
    const totalPriceWithVAT = discountedTotalPrice + (normalVAT / 100) * discountedTotalPrice;

    if (isVat === "Yes") {
        $("TD", row).eq(8).find('label').text(totalPriceWithVAT.toFixed(2));
    } else {
        $("TD", row).eq(7).find('label').text(totalPriceWithVAT.toFixed(2));
    }
};
