$(document).ready(function () {
    GetDatatableData(0, 0);
});


var CustomRangeDataFilter = function () {
    var _StartDate = $("#StartDate").val();
    var _EndDate = $("#EndDate").val();

    if (!FieldValidation('#StartDate')) {
        FieldValidationAlert('#StartDate', 'Start Date is Required.', "warning");
        return;
    }
    if (!FieldValidation('#EndDate')) {
        FieldValidationAlert('#EndDate', 'End Date is Required.', "warning");
        return;
    }

    ButtonEDLoader(false, "btnSubmit", 'Please Wait...');
    location.href = "/IncomeSummaryReport/Index?StartDate= " + _StartDate + "&EndDate= " + _EndDate;
};

var GetDatatableData = function (_IncomeTypeId, _IncomeCategoryId) {
    document.title = 'Income Summary Report';

    $("#tblIncomeSummaryReport").DataTable({
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;
            var intVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            };

            //Total over this page: 4
            pageTotal4 = api
                .column(4, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            //Update footer
            $(api.column(4).footer()).html(
                'Σ: ' + pageTotal4
            );
        },

        paging: true,
        select: true,
        "order": [[0, "desc"]],
        dom: 'Bfrtip',

        buttons: [
            'pageLength',
            {
                extend: 'collection',
                text: 'Export',
                buttons: [
                    {
                        extend: 'pdfHtml5',
                        customize: function (doc) {
                            //doc.content[1].margin = [100, 0, 100, 0];
                            //Remove the title created by datatTables
                            doc.content.splice(0, 1);
                            //Create a date string that we use in the footer. Format is dd-mm-yyyy
                            var now = new Date();
                            var jsDate = now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();

                            doc.pageMargins = [20, 60, 20, 30];
                            // Set the font size fot the entire document
                            doc.defaultStyle.fontSize = 7;
                            // Set the fontsize for the table header
                            doc.styles.tableHeader.fontSize = 10;


                            doc['header'] = (function () {
                                return {
                                    columns: [
                                        {
                                            alignment: 'left',  //center
                                            italics: true,
                                            text: 'Income Summary Report',
                                            fontSize: 18,
                                            margin: [0, 0]
                                        }
                                    ],
                                    margin: 20
                                }
                            });

                            // Create a footer object with 2 columns
                            doc['footer'] = (function (page, pages) {
                                return {
                                    columns: [
                                        {
                                            alignment: 'left',
                                            text: ['Created on: ', { text: jsDate.toString() }]
                                        },
                                        {
                                            alignment: 'right',
                                            text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
                                        }
                                    ],
                                    margin: 5
                                }
                            });
                            // Change dataTable layout (Table styling)
                            // To use predefined layouts uncomment the line below and comment the custom lines below
                            // doc.content[0].layout = 'lightHorizontalLines'; // noBorders , headerLineOnly
                            var objLayout = {};
                            objLayout['hLineWidth'] = function (i) { return .5; };
                            objLayout['vLineWidth'] = function (i) { return .5; };
                            objLayout['hLineColor'] = function (i) { return '#aaa'; };
                            objLayout['vLineColor'] = function (i) { return '#aaa'; };
                            objLayout['paddingLeft'] = function (i) { return 4; };
                            objLayout['paddingRight'] = function (i) { return 4; };
                            doc.content[0].layout = objLayout;
                        },


                        orientation: 'portrait', // landscape
                        customize: function (doc) {
                            doc.content[1].table.widths = Array(doc.content[1].table.body[0].length + 1).join('*').split('');
                            doc.content[1].table.border = 1;
                        },
                        pageSize: 'A4',
                        pageMargins: [0, 0, 0, 0], // try #1 setting margins
                        margin: [0, 0, 0, 0], // try #2 setting margins
                        text: '<u>PDF</u>',
                        key: { // press E for export PDF
                            key: 'e',
                            altKey: false
                        },
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6], //column id visible in PDF
                            modifier: {
                                // DataTables core
                                order: 'index',  // 'current', 'applied', 'index',  'original'
                                page: 'all',      // 'all',     'current'
                                search: 'none'     // 'none',    'applied', 'removed'
                            }
                        }
                    },
                    'copyHtml5',
                    'excelHtml5',
                    /*'csvHtml5',*/
                    {
                        extend: 'csvHtml5',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6],
                            page: 'all'
                        }
                    },
                    {
                        extend: 'print',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6],
                            page: 'all'
                        }
                    }
                ]
            }
        ],


        "processing": true,
        "language": { "processing": "<i class='fa fa-spinner fa-spin' style='font-size:30px; color:green;'></i>" },
        "serverSide": true,
        "serverSide": true,
        "filter": true, //Search Box
        "orderMulti": false,
        "stateSave": true,

        "ajax": {
            "url": "/IncomeSummaryReport/GetDataTabelData?IncomeTypeId=" + _IncomeTypeId + "&IncomeCategoryId=" + _IncomeCategoryId,
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            { "data": "Id", "name": "Id" },
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
            }
        ],

        'columnDefs': [{
            'targets': [5, 6],
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