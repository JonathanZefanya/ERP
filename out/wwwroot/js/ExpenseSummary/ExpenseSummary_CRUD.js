var Details = function (id) {
    var url = "/ExpenseSummary/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Expense Summary Details');
};


var AddEdit = function (id) {
    var url = "/ExpenseSummary/AddEdit?id=" + id;
    var ModalTitle = "Add Expense Summary";
    if (id > 0) {
        ModalTitle = "Edit Expense Summary";
    }
    OpenModalView(url, "modal-xl", ModalTitle);
};

var SaveExpenseSummary = function () {
    var _ObjExpenseSummary = ObjExpenseSummary();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/ExpenseSummary/AddEdit",
        data: _ObjExpenseSummary,
        dataType: "json",
        success: function (result) {
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $("#btnSave").val("Save");
                    $('#btnSave').removeAttr('disabled');
                    $('#tblExpenseSummary').DataTable().ajax.reload();
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                    $("#btnSave").val("Save");
                    $('#btnSave').removeAttr('disabled');
                    setTimeout(function () {
                        $('#Title').focus();
                    }, 400);
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var _url = "/ExpenseSummary/Delete?id=" + id;
    var _message = "Expense Summary has been deleted successfully. Expense Summary ID: ";
    var _tableName = "tblExpenseSummary";
    DeleteBase(_url, _message, _tableName);
};

var SyncExpenseSummaryData = function (data) {
    $("#GrandTotal").val(data.GrandTotal);
    $("#PaidAmount").val(data.PaidAmount);
    $("#DueAmount").val(data.DueAmount);
    $("#ChangeAmount").val(data.ChangeAmount);
}

var ObjExpenseSummary = function () {
    var ExpenseSummaryCRUDViewModel = {
        Id: $("#ExpenseSummaryId").text(),
        Title: $("textarea#Title").val(),
        GrandTotal: $("#GrandTotal").val(),
        PaidAmount: $("#PaidAmount").val(),
        DueAmount: $("#DueAmount").val(),
        ChangeAmount: $("#ChangeAmount").val(),

        BranchId: $("#BranchId").val(),
        CurrencyCode: $("#CurrencyCode").val(),

        Action: $("#Action").text(),
        CreatedDate: $("#CreatedDate").val(),
        CreatedBy: $("#CreatedBy").val(),
    };
    return ExpenseSummaryCRUDViewModel;
}
