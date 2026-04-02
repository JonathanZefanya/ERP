$(document).ready(function () {
    document.title = 'Item Request';

    $("#tblItemRequest").DataTable({
        paging: true,
        select: true,
        "order": [[0, "desc"]],
        dom: 'Bfrtip',


        buttons: [
            'pageLength',
        ],


        "processing": true,
        "serverSide": true,
        "filter": true, //Search Box
        "orderMulti": false,
        "stateSave": true,

        "ajax": {
            "url": "/ItemRequest/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            {
                data: "Id", "name": "Id", render: function (data, type, row) {
                    return "<a href='#' class='fa fa-eye' onclick=Details('" + row.Id + "');>" + row.Id + "</a>";
                }
            },
            { "data": "ItemDisplay", "name": "ItemDisplay" },
            { "data": "RequestQuantity", "name": "RequestQuantity" },
            { "data": "FromWarehouseDisplay", "name": "FromWarehouseDisplay" },
            { "data": "StatusDisplay", "name": "StatusDisplay" },
            { "data": "Note", "name": "Note" },


            {
                "data": "CreatedDate",
                "name": "CreatedDate",
                "autoWidth": true,
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            {
                data: null, render: function (data, type, row) {
                    if (row.Status == 1) {
                        return "<a href='#' class='btn btn-success btn-xs' onclick=TransferRequest('" + row.Id + "');>Transfer</a>";
                    }
                    else {
                        return "-";
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    if (row.Status == 1) {
                        return "<a href='#' class='btn btn-info btn-xs' onclick=AddEdit('" + row.Id + "');>Edit</a>";
                    }
                    else {
                        return "-";
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    if (row.Status == 1) {
                        return "<a href='#' class='btn btn-danger btn-xs' onclick=Delete('" + row.Id + "'); >Delete</a>";
                    }
                    else {
                        return "-";
                    }
                }
            }
        ],

        'columnDefs': [{
            'targets': [7, 8, 9],
            'orderable': false,
        }],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });

});

