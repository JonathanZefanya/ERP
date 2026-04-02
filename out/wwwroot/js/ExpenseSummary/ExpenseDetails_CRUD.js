var AddExpenseDetails = function () {
    var obj = ObjExpenseDetails();

    if (!FieldValidation('#ExpenseTypeId')) {
        FieldValidationAlert('#ExpenseTypeId', 'Expense Type is Required.', "warning");
        return;
    }

    if (obj.Quantity === "" || obj.Quantity === null || parseFloat(obj.Quantity) < 1) {
        FieldValidationAlert('#Quantity', 'Quantity is Required', "warning");
        return;
    }
    if (obj.UnitPrice === "" || obj.UnitPrice === null || parseFloat(obj.UnitPrice) < 1) {
        FieldValidationAlert('#UnitPrice', 'Unit Price is Required', "warning");
        return;
    }

    $("#btnExpenseDetails").val("Please Wait");
    $("#btnExpenseDetails").attr('disabled', 'disabled');

    var ExpenseDetailsCRUDViewModel = {};
    ExpenseDetailsCRUDViewModel.ExpenseSummaryId = obj.ExpenseSummaryId;
    ExpenseDetailsCRUDViewModel.ExpenseTypeId = obj.ExpenseTypeId;
    ExpenseDetailsCRUDViewModel.ExpenseType = obj.ExpenseType;
    ExpenseDetailsCRUDViewModel.Description = obj.Description;
    ExpenseDetailsCRUDViewModel.Quantity = obj.Quantity;
    ExpenseDetailsCRUDViewModel.UnitPrice = obj.UnitPrice;
    ExpenseDetailsCRUDViewModel.TotalPrice = obj.TotalPrice;

    var _ExpenseSummaryCRUDViewModel = ObjExpenseSummary();
    ExpenseDetailsCRUDViewModel.ExpenseSummaryCRUDViewModel = _ExpenseSummaryCRUDViewModel;   
    $.ajax({
        type: "POST",
        url: "/ExpenseSummary/AddExpenseDetails",
        data: ExpenseDetailsCRUDViewModel,
        dataType: "json",
        success: function (result) {
            SyncExpenseSummaryData(result.ExpenseSummaryCRUDViewModel);        
            AddHTMLTableRow(result);
            $("#btnExpenseDetails").val("Add Item");
            $('#btnExpenseDetails').removeAttr('disabled');
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

function UpdateExpenseDetails(button) {
    var row = $(button).closest("TR");
    var _ExpenseDetailsId = $("TD", row).eq(0).html();

    var _ExpenseType = $("textarea#ExpenseType" + _ExpenseDetailsId).val();
    var _Description = $("textarea#Description" + _ExpenseDetailsId).val();
    var _Quantity = parseFloat($("#Quantity" + _ExpenseDetailsId).val());
    var _UnitPrice = parseFloat($("#UnitPrice" + _ExpenseDetailsId).val());
    var _TotalPrice = parseFloat($("#TotalPrice" + _ExpenseDetailsId).text());

    $("#btnUpdateExpenseDetails" + _ExpenseDetailsId).val("Please Wait");
    $("#btnUpdateExpenseDetails" + _ExpenseDetailsId).attr('disabled', 'disabled');

    var ExpenseDetailsCRUDViewModel = {};
    ExpenseDetailsCRUDViewModel.Id = _ExpenseDetailsId;

    ExpenseDetailsCRUDViewModel.ExpenseType = _ExpenseType;
    ExpenseDetailsCRUDViewModel.Description = _Description;
    ExpenseDetailsCRUDViewModel.Quantity = _Quantity;
    ExpenseDetailsCRUDViewModel.UnitPrice = _UnitPrice;
    ExpenseDetailsCRUDViewModel.TotalPrice = _Quantity * _UnitPrice;

    var _ExpenseSummaryCRUDViewModel = ObjExpenseSummary();
    ExpenseDetailsCRUDViewModel.ExpenseSummaryCRUDViewModel = _ExpenseSummaryCRUDViewModel;
    ExpenseDetailsCRUDViewModel.ExpenseSummaryId = ExpenseDetailsCRUDViewModel.ExpenseSummaryCRUDViewModel.Id;

    $.ajax({
        type: "POST",
        url: "/ExpenseSummary/UpdateExpenseDetails",
        data: ExpenseDetailsCRUDViewModel,
        dataType: "json",
        success: function (result) {
            toastr.success("Update item successfully. Item ID: " + result.Id, 'Success');
            $("#TotalAmount" + result.Id).text(result.TotalAmount);
            $("#btnUpdateExpenseDetails" + _ExpenseDetailsId).val("Update");
            $('#btnUpdateExpenseDetails' + _ExpenseDetailsId).removeAttr('disabled');

            $("#GrandTotal").val(result.ExpenseSummaryCRUDViewModel.GrandTotal);
            $("#PaidAmount").val(result.ExpenseSummaryCRUDViewModel.PaidAmount);
            $("#DueAmount").val(result.ExpenseSummaryCRUDViewModel.DueAmount);
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var DeleteExpenseDetails = function (_ExpenseDetailsId) {
    var ExpenseDetailsCRUDViewModel = {};
    ExpenseDetailsCRUDViewModel.Id = _ExpenseDetailsId;

    var _ExpenseSummaryCRUDViewModel = ObjExpenseSummary();
    ExpenseDetailsCRUDViewModel.ExpenseSummaryCRUDViewModel = _ExpenseSummaryCRUDViewModel;  

    $.ajax({
        type: "DELETE",
        url: "/ExpenseSummary/DeleteExpenseDetails",
        data: ExpenseDetailsCRUDViewModel,
        dataType: "json",
        success: function (result) {
            toastr.success("Expense details item has been deleted successfully. ID: " + result.Id, 'Success');
            $("#GrandTotal").val(result.ExpenseSummaryCRUDViewModel.GrandTotal);
            $("#PaidAmount").val(result.ExpenseSummaryCRUDViewModel.PaidAmount);
            $("#DueAmount").val(result.ExpenseSummaryCRUDViewModel.DueAmount);
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}
