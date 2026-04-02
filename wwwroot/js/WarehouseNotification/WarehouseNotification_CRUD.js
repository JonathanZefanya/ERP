var Details = function (id) {
    var url = "/WarehouseNotification/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Warehouse Notification Details');
};


var MarkedAsRead = function (id) {
    $.ajax({
        type: "POST",
        url: "/WarehouseNotification/MarkedAsRead?id=" + id,
        success: function (result) {
            GetUnreadTotalNotification();
            $('#tblWarehouseNotification').DataTable().ajax.reload();
            toastr.success(result, 'Success');
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
};

var MarkedAllAsRead = function () {
    Swal.fire({
        title: 'Are you sure, do you want to mark all notifications as read?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: "/WarehouseNotification/MarkedAllAsRead",
                success: function (result) {
                    $('#lblNotificationCount').text('0');
                    $('#tblWarehouseNotification').DataTable().ajax.reload();
                    toastr.success(result, 'Success');
                },
                error: function (errormessage) {
                    SwalSimpleAlert(errormessage.responseText, "warning");
                }
            });
        }
        else {
            $("#MarkedAllasRead").prop("checked", false);
        }
    });
};


var GetUnreadTotalNotification = function () {
    $.ajax({
        type: "GET",
        url: "/WarehouseNotification/GetUnreadTotalNotification",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data === null) return;
            $('#lblNotificationCount').text(data);
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

//AspNetCore SignalR
$(() => {
    let connection = new signalR.HubConnectionBuilder().withUrl("/notify").build();
    connection.start();
    connection.on("refreshWarehouseNotification", function () {
        GetUnreadTotalNotification();
        $('#tblWarehouseNotification').DataTable().ajax.reload();
    });
});