var Details = function (id) {
    var url = "/DamageItemDetails/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Damage Item Deatils');
};


var AddEdit = function (id) {
    var url = "/DamageItemDetails/AddEdit?id=" + id;
    var ModalTitle = "Add Damage Item";
    if (id > 0) {
        ModalTitle = "Edit Damage Item";
    }
    OpenModalView(url, "modal-lg", '');
};

var Save = function () {
    if (!$("#frmDamageItemDeatils").valid()) {
        return;
    }

    var _frmDamageItemDeatils = $("#frmDamageItemDeatils").serialize();
    $.ajax({
        type: "POST",
        url: "/DamageItemDetails/AddEdit",
        data: _frmDamageItemDeatils,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $('#tblDamageItemDeatils').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var UIName = "DamageItemDetails";
    var _url = `/${UIName}/Delete?id=` + id;
    var _tableName = `tbl${UIName}`;

    var subMessage = addSpaceToUppercase(UIName);
    var _message = `${subMessage} has been deleted successfully. ${subMessage} ID: `;
    DeleteBase(_url, _message, _tableName);
};


var Adjustment = function (id) {
    var url = "/DamageItemDetails/Adjustment?id=" + id;
    OpenModalView(url, "modal-lg", 'Damage Item Adjustment');
};
