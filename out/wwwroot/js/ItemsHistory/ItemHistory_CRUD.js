var ViewItem = function (ItemId) {
    var url = "/Items/ViewItem?Id=" + ItemId;
    OpenModalView(url, "modal-lg", 'Item Details');
};

var ViewItemHistory = function (ItemId) {
    var url = "/ItemsHistory/ViewItemHistory?ItemId=" + ItemId;
    OpenModalView(url, "modal-lg", 'Item History. Item ID: ' + ItemId);
};