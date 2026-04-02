var funAction = function (UserProfileId) {
    if (DemoUserAccountLockAll() == 1) return;
    var _Action = $("#" + UserProfileId).val();
    if (_Action == 1)
        AddEditUserAccount(UserProfileId);
    else if (_Action == 2)
        ResetPasswordAdmin(UserProfileId);
    else if (_Action == 3)
        UpdateUserRole(UserProfileId);
    else if (_Action == 4)
        DeleteUserAccount(UserProfileId);
    $("#" + UserProfileId).prop('selectedIndex', 0);
};

var ViewUserDetails = function (Id) {
    var url = "/UserManagement/ViewUserDetails?Id=" + Id;
    OpenModalView(url, "modal-lg", 'User Details');
};

var ResetPasswordAdmin = function (id) {
    var url = "/UserManagement/ResetPasswordAdmin?id=" + id;
    OpenModalView(url, "modal-md", '<h4>Reset Password</h4>');
};

var AddEditUserAccount = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
    var url = "/UserManagement/AddEditUserAccount?id=" + id;

    var ModalTitle = "Add User";
    if (id > 0) {
        ModalTitle = "Edit User";
    }
    OpenModalView(url, "modal-xl", ModalTitle);

    setTimeout(function () {
        $('#FirstName').focus();
    }, 200);
};

var SaveUser = function () {
    if (!$("#ApplicationUserForm").valid()) {
        return;
    }

    var _UserProfileId = $("#UserProfileId").val();
    if (_UserProfileId > 0) {
        $("#btnSave").prop('value', 'Updating User');
    }
    else {
        $("#btnSave").prop('value', 'Creating User');
    }
    $('#btnSave').prop('disabled', true);

    $.ajax({
        type: "POST",
        url: "/UserManagement/AddEditUserAccount",
        data: PreparedFormObj(),
        processData: false,
        contentType: false,
        success: function (result) {
            $('#btnSave').prop('disabled', false);
            $("#btnSave").prop('value', 'Save');
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnAddEditUserAccountClose").click();
                    if (result.CurrentURL == "/") {
                        setTimeout(function () {
                            $("#tblRecentRegisteredUser").load("/ #tblRecentRegisteredUser");
                        }, 1000);
                    }
                    else {
                        $('#tblUserAccount').DataTable().ajax.reload();
                    }
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                    setTimeout(function () {
                        $('#Email').focus();
                    }, 400);
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var DeleteUserAccount = function (id) {
    Swal.fire({
        title: 'Do you want to delete this user?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "DELETE",
                url: "/UserManagement/DeleteUserAccount?id=" + id,
                success: function (result) {
                    Swal.fire({
                        title: result,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblUserAccount').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};

var UpdateUserRole = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
    var url = "/ManageUserRoles/UpdateUserRole?id=" + id;
    OpenModalView(url, "modal-xl", '<h4>Manage Page Access</h4>');
};

var SaveUpdateUserRole = function () {
    $("#btnUpdateRole").val("Please Wait");
    $('#btnUpdateRole').attr('disabled', 'disabled');

    var _frmManageRole = $("#frmManageRole").serialize();
    $.ajax({
        type: "POST",
        url: "/ManageUserRoles/SaveUpdateUserRole",
        data: _frmManageRole,
        success: function (result) {
            $("#btnUpdateRole").val("Save");
            $('#btnUpdateRole').removeAttr('disabled');
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $('#tblUserAccount').DataTable().ajax.reload();
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var PreparedFormObj = function () {
    var _FormData = new FormData()
    _FormData.append('Id', $("#Id").val())
    _FormData.append('UserProfileId', $("#UserProfileId").val())
    _FormData.append('ApplicationUserId', $("#ApplicationUserId").val())
    _FormData.append('ProfilePictureDetails', $('#ProfilePictureDetails')[0].files[0])

    _FormData.append('FirstName', $("#FirstName").val())
    _FormData.append('LastName', $("#LastName").val())
    _FormData.append('PhoneNumber', $("#PhoneNumber").val())
    _FormData.append('Email', $("#Email").val())
    _FormData.append('PasswordHash', $("#PasswordHash").val())
    _FormData.append('ConfirmPassword', $("#ConfirmPassword").val())
    _FormData.append('Address', $("#Address").val())
    _FormData.append('Country', $("#Country").val())
    _FormData.append('RoleId', $("#RoleId").val())
    _FormData.append('BranchId', $("#BranchId").val())
    _FormData.append('EmployeeId', $("#EmployeeId").val())

    _FormData.append('DateOfBirth', $("#DateOfBirth").val())
    _FormData.append('JoiningDate', $("#JoiningDate").val())
    _FormData.append('LeavingDate', $("#LeavingDate").val())

    _FormData.append('CurrentURL', $("#CurrentURL").val())
    return _FormData;
}