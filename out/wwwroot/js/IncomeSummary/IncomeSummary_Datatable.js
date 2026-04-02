$(document).ready(function () {
    GetDatatableData(0, 0);
});


var GetDatatableData = function (_IncomeTypeId, _IncomeCategoryId) {
    document.title = 'Income Summary';

    $("#tblIncomeSummary").DataTable({
        paging: true,
        select: true,
        "order": [[0, "desc"]],
        dom: 'Bfrtip',


        buttons: [
            'pageLength',
        ],


        "processing": true,
        "language": { "processing": "<i class='fa fa-spinner fa-spin' style='font-size:30px; color:green;'></i>" },
        "serverSide": true,
        "serverSide": true,
        "filter": true, //Search Box
        "orderMulti": false,
        "stateSave": true,

        "ajax": {
            "url": "/IncomeSummary/GetDataTabelData?IncomeTypeId=" + _IncomeTypeId + "&IncomeCategoryId=" + _IncomeCategoryId,
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            {
                data: "Id", "name": "Id", render: function (data, type, row) {
                    return "<a href='#' class='fa fa-eye' onclick=Details('" + row.Id + "');>" + row.Id + "</a>";
                }
            },
            { "data": "Title", "name": "Title" },
            {
                data: null, render: function (data, type, row) {
                    var _getType = getType(row);
                    return _getType;
                }
            },
            {
                data: null, render: function (data, type, row) {
                    var _getCategory = getCategory(row);
                    return _getCategory;
                }
            },
            { "data": "Amount", "name": "Amount" },
            { "data": "Description", "name": "Description" },

            {
                data: "CreatedDate", "name": "CreatedDate", render: function (data, type, row) {
                    return ConvertDateToMMDDYYYY(row.CreatedDate);
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-info btn-xs' onclick=AddEdit('" + row.Id + "');>Edit</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger btn-xs' onclick=Delete('" + row.Id + "'); >Delete</a>";
                }
            }
        ],

        'columnDefs': [{
            'targets': [7, 8],
            'orderable': false,
        }],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });
}


var getCategory = function (row) {
    var result = '<span class="badge bg-primary m-1 px-2 shadow-sm py-1">' + row.CategoryDisplay + '</span>';
    return result;
}

var getType = function (row) {
    var result = '<span class="badge bg-info m-1 px-2 shadow-sm py-1">' + row.TypeDisplay + '</span>';
    return result;
}