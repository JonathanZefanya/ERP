// Helper functions
var getVal = selector => $(selector).val();
var setVal = (selector, value) => $(selector).val(value);
var getSelectedText = selector => $(`${selector} option:selected`).text();
var parseNum = val => parseFloat(val) || 0;

// Split helpers
var splitText = (str, idx) => (str.split(':')[idx] || '').trim();

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

// Generate Unit Price Dropdown
var GeneratePriceDropdownOLD = splitArray => {
    const prices = [
        { label: 'Normal', idx: 2 },
        { label: 'Cost', idx: 4 },
        { label: 'Trade', idx: 6 },
        { label: 'Premium', idx: 8 }
    ];
    const $unitPrice = $("#UnitPrice").empty();
    prices.forEach((p, i) =>
        $("<option/>", { value: i + 1, text: `${p.label}:${splitArray[p.idx]}` }).appendTo($unitPrice)
    );
};

// Update Unit Price and VAT
var UpdateUnitPrice = () => {
    const splitArray = getSelectedText("#ItemId").split(":");
    GeneratePriceDropdown(splitArray);
    setVal("#ItemVAT", getVal("#IsVat") === "Yes" ? parseNum(splitArray[10]) : 0);
    UpdateItemAllElement();
};

// Main calculation logic
var UpdateItemAllElement = () => {
    const qty = parseNum(getVal("#Quantity"));
    const freeQty = parseNum(getVal("#FreeQuantity"));
    //let unitPrice = parseNum(splitText(getSelectedText("#UnitPrice"), 1));
    const unitPrice = getInputNumber("#UnitPrice");
    const discount = parseNum(getVal("#ItemDiscount"));
    const vat = parseNum(getVal("#ItemVAT"));
    // Don't charge for free items
    const chargeableQty = Math.max(0, qty - freeQty);

    let discountedPrice = unitPrice - (discount / 100) * unitPrice;
    discountedPrice += (vat / 100) * discountedPrice;
    const total = discountedPrice * chargeableQty;
    setVal("#TotalAmount", total.toFixed(2));
};

// Add new row to table
var AddHTMLTableRow = result => {
    const tBody = $("#tblPurchasesPaymentDetail > TBODY")[0];
    const row = tBody.insertRow(-1);
    const _IsVat = getVal("#IsVat") === "Yes";
    const id = result.Id;

    // Helper for input creation
    const createInput = (type, id, value, width = 70, min = 0) =>
        $(`<input/>`, {
            type,
            id,
            value,
            min,
            style: `width:${width}px;`,
            onchange: "UpdateItemDynamicControl(this);"
        });

    // Add cells
    $(row.insertCell(-1)).html(id);
    $(row.insertCell(-1)).append($("<textarea/>", {
        rows: 1, style: "width:400px;", id: `ItemName${id}`, val: splitText(getSelectedText("#ItemId"), 0)
    }).val(splitText(getSelectedText("#ItemId"), 0)));
    $(row.insertCell(-1)).append(createInput("number", `Quantity${id}`, getVal("#Quantity"), 70, 1));
    $(row.insertCell(-1)).append(createInput("number", `FreeQuantity${id}`, getVal("#FreeQuantity"), 50, 0));

    //$(row.insertCell(-1)).append(createInput("number", `UnitPrice${id}`, parseNum(splitText(getSelectedText("#UnitPrice"), 1)), 70, 1));
    $(row.insertCell(-1)).append(createInput("number", `UnitPrice${id}`,  parseNum($("#UnitPrice").val()), 70, 0));

    if (_IsVat)
        $(row.insertCell(-1)).append(createInput("number", `ItemVAT${id}`, getVal("#ItemVAT"), 50, 0));
    $(row.insertCell(-1)).append(createInput("number", `ItemDiscount${id}`, getVal("#ItemDiscount"), 70, 0));
    $(row.insertCell(-1)).append($("<label/>", { class: "not-bold", id: `TotalAmount${id}` }).text(getVal("#TotalAmount")));

    // Action buttons
    const cell = $(row.insertCell(-1));
    cell.append($("<input/>", {
        type: "button", class: "btn btn-success btn-xs", id: `btnUpdatePaymentsDetail${id}`,
        onclick: "UpdatePurchasesPaymentDetail(this);", value: "Update"
    }));
    cell.append($("<input/>", {
        type: "button", class: "btn btn-danger btn-xs", onclick: "Remove(this);", value: " X "
    }));
    cell.append($("<a/>", {
        class: "btn fa fa-id-card fa-3x", onclick: "AddItemSerialNumber(this);"
    }));

    $("#ItemId").focus();
    ClearInvoiceItemTableRowData();
};

// Update row from DB
function LoadTableRowFromDB(item) {
    const tBody = $("#tblPurchasesPaymentDetail > TBODY")[0];
    const row = tBody.insertRow(-1);
    const _IsVat = getVal("#IsVat") === "Yes";
    const id = item.Id;

    const createInput = (type, id, value, width = 70, min = 0) =>
        $(`<input/>`, {
            type,
            id,
            value,
            min,
            style: `width:${width}px;`,
            onchange: "UpdateItemDynamicControl(this);"
        });

    $(row.insertCell(-1)).html(id);
    $(row.insertCell(-1)).append($("<textarea/>", {
        rows: 1, style: "width:450px;", id: `ItemName${id}`
    }).val(item.ItemName));
    $(row.insertCell(-1)).append(createInput("number", `Quantity${id}`, item.Quantity, 70, 1));
    $(row.insertCell(-1)).append(createInput("number", `FreeQuantity${id}`, item.FreeQuantity || 0, 50, 0));
    $(row.insertCell(-1)).append(createInput("number", `UnitPrice${id}`, item.UnitPrice, 70, 1));
    if (_IsVat)
        $(row.insertCell(-1)).append(createInput("number", `ItemVAT${id}`, item.ItemVAT, 50, 0));
    $(row.insertCell(-1)).append(createInput("number", `ItemDiscount${id}`, item.ItemDiscount, 70, 0));
    $(row.insertCell(-1)).append($("<label/>", { class: "not-bold", id: `TotalAmount${id}` }).text(item.TotalAmount));
    const cell = $(row.insertCell(-1));
    cell.append($("<input/>", {
        type: "button", class: "btn btn-success btn-xs", id: `btnUpdatePaymentsDetail${id}`,
        onclick: "UpdatePurchasesPaymentDetail(this);", value: "Update"
    }));
    cell.append($("<input/>", {
        type: "button", class: "btn btn-danger btn-xs", onclick: "Remove(this);", value: " X "
    }));
    cell.append($("<a/>", {
        class: "btn fa fa-id-card fa-3x", onclick: "AddItemSerialNumber(this);"
    }));
}

// Clear form
var ClearInvoiceItemTableRowData = () => {
    setVal("#PaymentsDetailsId", "");
    $('#ItemId').val(0).trigger('change');
    setVal("#Quantity", 1);
    setVal("#FreeQuantity", 0);
    //setVal("#UnitPrice", "");
    $('#UnitPrice').val(0).trigger('change');
    setVal("#ItemVAT", 0);
    setVal("#ItemDiscount", 0);
    setVal("#TotalAmount", "");
};

// Remove row
function Remove(button) {
    Swal.fire({
        title: "Do you want to delete this item?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes"
    }).then(result => {
        if (result.value) {
            const row = $(button).closest("TR");
            const table = $("#tblPurchasesPaymentDetail")[0];
            table.deleteRow(row[0].rowIndex);
            const id = $("TD", row).eq(0).html();
            DeletePurchasesPaymentDetail({ Id: id, PaymentId: getVal("#Id") });
        }
    });
}

// Update item dynamic controls (now includes FreeQuantity)
var UpdateItemDynamicControl = button => {
    const row = $(button).closest("TR");
    const id = $("TD", row).eq(0).html();
    const qty = parseNum(getVal(`#Quantity${id}`));
    const freeQty = parseNum(getVal(`#FreeQuantity${id}`));
    const unitPrice = parseNum(getVal(`#UnitPrice${id}`));
    const discount = parseNum(getVal(`#ItemDiscount${id}`));
    const isVat = getVal("#IsVat") === "Yes";
    const vat = isVat ? parseNum(getVal(`#ItemVAT${id}`)) : 0;
    const chargeableQty = Math.max(0, qty - freeQty);

    let discountedPrice = unitPrice - (discount / 100) * unitPrice;
    discountedPrice += (vat / 100) * discountedPrice;
    const total = discountedPrice * chargeableQty;

    // Find correct cell index for total (depends on VAT column)
    const totalCellIdx = isVat ? 7 : 6;
    $("TD", row).eq(totalCellIdx).find("label").text(total.toFixed(2));
};

// Keydown handler
var onkeydownChargeAmount = () => {
    if (event.keyCode === 13) {
        event.preventDefault();
        AddPurchasesPaymentDetail();
    }
};

var AddPaymentsDetailsHTMLRow = () => {
    AddPurchasesPaymentDetail();
    setVal("#ItemBarcode", "");
};
