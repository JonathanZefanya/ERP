$(document).ready(function () {
  document.title = "Damage Item Deatil";

  $("#tblDamageItemDeatils").DataTable({
    paging: true,
    select: true,
    order: [[0, "desc"]],
    dom: "Bfrtip",

    buttons: ["pageLength"],

    processing: true,
    serverSide: true,
    filter: true, //Search Box
    orderMulti: false,
    stateSave: true,

    ajax: {
      url: "/DamageItemDetails/GetDataTabelData",
      type: "POST",
      datatype: "json",
    },

    columns: [
      {
        data: "Id",
        name: "Id",
        render: function (data, type, row) {
          return (
            "<a href='#' class='fa fa-eye' onclick=Details('" +
            row.Id +
            "');>" +
            row.Id +
            "</a>"
          );
        },
      },
      { data: "ItemName", name: "ItemName" },
      { data: "Supplier", name: "Supplier" },
      { data: "TotalDamageItem", name: "TotalDamageItem" },
      {
        data: "TotalAdjusted",
        name: "TotalAdjusted",
        render: function (data, type, row) {
          if (data === 0) {
            return '<span class="badge badge-secondary">0</span>';
          } else if (data >= row.TotalDamageItem) {
            return '<span class="badge badge-success">' + data + "</span>";
          } else {
            return '<span class="badge badge-info">' + data + "</span>";
          }
        },
      },
      { data: "ReasonOfDamage", name: "ReasonOfDamage" },
      {
        data: "CreatedDate",
        name: "CreatedDate",
        autoWidth: true,
        render: function (data) {
          var date = new Date(data);
          var month = date.getMonth() + 1;
          return (
            (month.length > 1 ? month : month) +
            "/" +
            date.getDate() +
            "/" +
            date.getFullYear()
          );
        },
      },
      {
        data: null,
        render: function (data, type, row) {
          return (
            "<a href='#' class='btn btn-info btn-xs' onclick=AddEdit('" +
            row.Id +
            "');>Edit</a>"
          );
        },
      },
      {
        data: null,
        render: function (data, type, row) {
          return (
            "<a href='#' class='btn btn-danger btn-xs' onclick=Delete('" +
            row.Id +
            "'); >Delete</a>"
          );
        },
      },
      {
        data: null,
        render: function (data, type, row) {
          // Disable button if fully adjusted
          if (row.Remaining === 0) {
            return "<button class='btn btn-success btn-xs' disabled title='Fully Adjusted'><i class='fa fa-check'></i> Adjusted</button>";
          } else {
            return (
              "<a href='#' class='btn btn-success btn-xs' onclick=Adjustment('" +
              row.Id +
              "');><i class='fa fa-edit'></i> Adjustment</a>"
            );
          }
        },
      },
    ],

    columnDefs: [
      {
        targets: [5, 6],
        orderable: false,
      },
    ],

    lengthMenu: [
      [20, 10, 15, 25, 50, 100, 200],
      [20, 10, 15, 25, 50, 100, 200],
    ],
  });
});
