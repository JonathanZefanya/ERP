var UpdateDueAndChangeAmount = function (TotalPaid) {
    var _PaidAmount = $("#PaidAmount").val();
    var _GrandTotal = $("#GrandTotal").val();

    if (_PaidAmount > _GrandTotal) {
        $('#DueAmount').val(0);
        $('#ChangedAmount').val((_PaidAmount - _GrandTotal).toFixed(2));
    }
    else {
        var result = parseFloat(_GrandTotal) - parseFloat(_PaidAmount);
        $('#DueAmount').val(result.toFixed(2));
        $('#ChangedAmount').val(0);
    }
}

