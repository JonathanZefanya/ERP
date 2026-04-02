var Details = function (id) {
    var url = "/SendEmailHistory/Details?id=" + id;
    OpenModalView(url, "modal-xl", 'Send Email History Details');
};
