var ResetPassword = function (ApplicationUserId) {
    if (DemoUserAccountLockAll() == 1) return;
    var url = "/UserProfile/ResetPassword?ApplicationUserId=" + ApplicationUserId;
    OpenModalView(url, "modal-md", '<h4>Reset Password</h4>');
};

var SaveResetPassword = function () {
    if (!$("#frmResetPassword").valid()) {
        return;
    }

    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');

    $.ajax({
        type: "POST",
        url: "/UserProfile/SaveResetPassword",
        data: UserProfile_PreparedFormObj(),
        processData: false,
        contentType: false,
        success: function (result) {
            var _error = result.substring(0, 5);
            if (_error == 'error') {
                var _result = result.slice(5);
                Swal.fire({
                    title: _result,
                    icon: "warning"
                }).then(function () {
                    setTimeout(function () {
                        $('#OldPassword').focus();
                        $("#btnSave").val("Save");
                        $('#btnSave').removeAttr('disabled');
                    }, 400);
                });
            }
            else {
                Swal.fire({
                    title: result,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var EditUserProfile = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
    var url = "/UserProfile/EditUserProfile?id=" + id;
    OpenModalView(url, "modal-xl", 'Edit User');

    setTimeout(function () {
        $('#FirstName').focus();
    }, 200);
};

var SaveUserProfile = function () {
    if (!$("#ApplicationUserForm").valid()) {
        return;
    }

    $("#btnSave").prop('value', 'Updating User');
    $('#btnSave').prop('disabled', true);

    $.ajax({
        type: "POST",
        url: "/UserProfile/SaveUserProfile",
        data: UserProfile_Edit_FormObj(),
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
                    $("#divUserProfile").load("/UserProfile/Index #divUserProfile");
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

var UserProfile_PreparedFormObj = function () {
    var _FormData = new FormData()
    _FormData.append('ApplicationUserId', $("#ApplicationUserId").val())
    _FormData.append('OldPassword', $("#OldPassword").val())
    _FormData.append('NewPassword', $("#NewPassword").val())
    _FormData.append('ConfirmPassword', $("#ConfirmPassword").val())
    return _FormData;
}

var UserProfile_Edit_FormObj = function () {
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
    _FormData.append('EmployeeId', $("#EmployeeId").val())

    _FormData.append('DateOfBirth', $("#DateOfBirth").val())
    _FormData.append('JoiningDate', $("#JoiningDate").val())
    _FormData.append('LeavingDate', $("#LeavingDate").val())

    _FormData.append('CurrentURL', $("#CurrentURL").val())
    return _FormData;
}