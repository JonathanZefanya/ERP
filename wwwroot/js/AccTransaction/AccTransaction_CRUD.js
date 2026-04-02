var Details = function (id) {
    var url = "/AccTransaction/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Transaction Details');
};
