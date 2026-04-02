var Details = function(id) {
    var url = "/AuditLogs/Details?id=" + id;
    OpenModalView(url, "modal-lg", 'Audit Logs Details');
};