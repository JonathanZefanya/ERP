var LoginAction = async function () {
    var _Email = $('#Email').val();
    if (_Email == '') {
        FieldValidationAlert('#Email', 'Email Address is Required.', "warning");
        return;
    }
    var _Password = $('#Password').val();
    if (_Password == '') {
        FieldValidationAlert('#Password', 'Password is Required.', "warning");
        return;
    }

    var _frmLogin = $("#frmLogin").serialize();
    ButtonEDLoader(false, "btnUserLogin", 'Please Wait...');

    try {
        let result = await $.ajax({
            type: "POST",
            url: "/Account/Login",
            data: _frmLogin
        });

        if (result.IsSuccess) {
            sessionStorage.setItem("UserProfileId", result.UserProfieId);
            localStorage.setItem("UserInfo", JSON.stringify(result.ModelObject));
            location.href = "/Dashboard/Index";

            SaveUserInfoFromBrowser().catch(function (error) {
                toastr.error("Failed to save user info:" + error);
            });

        } else {
            toastr.error(result.AlertMessage);
            ButtonEDLoader(true, "btnUserLogin", 'Log In');
        }
    } catch (errormessage) {
        ButtonEDLoader(true, "btnUserLogin", 'Log In');
        console.error(errormessage);
        toastr.error("An error occurred during login. Please try again." + errormessage.responseText);
        //SwalSimpleAlert(errormessage.responseText, "warning");
    }
}


var SignOutAction = function () {
    var _UserProfileId = sessionStorage.getItem("UserProfileId");
    var _Email = sessionStorage.getItem("Email");

    var LoginViewModel = {};
    LoginViewModel.Latitude = '';
    LoginViewModel.Longitude = '';
    LoginViewModel.UserProfileId = _UserProfileId;
    LoginViewModel.Email = _Email;

    ButtonEDLoader(false, "btnSignOut", 'Wait...');
    $.ajax({
        type: "POST",
        url: "/Account/UserSignOut",
        data: LoginViewModel,
        dataType: "json",
        success: function (result) {
            if (result == true) {
                localStorage.removeItem("CompanyInfo");
                location.href = "/Account/Login";
            }
            else {
                ButtonEDLoader(true, "btnSignOut", 'Sign Out');
            }
        },
        error: function (errormessage) {
            ButtonEDLoader(true, "btnSignOut", 'Sign Out');
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var SaveUserInfoFromBrowser = async function () {
    var parser = new UAParser();
    var UserInfoFromBrowser = {};
    var _GetBrowserUniqueID = GetBrowserUniqueID();

    UserInfoFromBrowser.BrowserUniqueID = _GetBrowserUniqueID;
    UserInfoFromBrowser.Lat = '';
    UserInfoFromBrowser.Long = '';
    UserInfoFromBrowser.TimeZone = new Date();
    UserInfoFromBrowser.BrowserMajor = parser.getResult().browser.major;
    UserInfoFromBrowser.BrowserName = parser.getResult().browser.name;
    UserInfoFromBrowser.BrowserVersion = parser.getResult().browser.version;

    UserInfoFromBrowser.CPUArchitecture = parser.getResult().cpu.architecture;
    UserInfoFromBrowser.DeviceModel = parser.getResult().device.model;
    UserInfoFromBrowser.DeviceType = parser.getResult().device.type;
    UserInfoFromBrowser.DeviceVendor = parser.getResult().device.vendor;

    UserInfoFromBrowser.EngineName = parser.getResult().engine.name;
    UserInfoFromBrowser.EngineVersion = parser.getResult().engine.version;
    UserInfoFromBrowser.OSName = parser.getResult().os.name;
    UserInfoFromBrowser.OSVersion = parser.getResult().os.version;
    UserInfoFromBrowser.UA = parser.getResult().ua;

    var _UserEmailAddress = $('#UserEmailAddress').val();
    UserInfoFromBrowser.CreatedBy = _UserEmailAddress;
    UserInfoFromBrowser.ModifiedBy = _UserEmailAddress;

    $.ajax({
        type: "POST",
        url: "/Account/SaveUserInfoFromBrowser",
        data: UserInfoFromBrowser,
        dataType: "json",
        success: function (result) {
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
};

var GetBrowserUniqueID = function () {
    var navigator_info = window.navigator;
    var screen_info = window.screen;
    var uid = navigator_info.mimeTypes.length;
    uid += navigator_info.userAgent.replace(/\D+/g, '');
    uid += navigator_info.plugins.length;
    uid += screen_info.height || '';
    uid += screen_info.width || '';
    uid += screen_info.pixelDepth || '';
    return uid;
}

var FieldValidationAlert = function (FieldName, Message, icontype) {
    Swal.fire({
        title: Message,
        icon: icontype,
        onAfterClose: () => {
            $(FieldName).focus();
        }
    });
}

var ValidateEmail = function (email_id) {
    const regex_pattern = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (regex_pattern.test(email_id) == false) {
        return false;
    }
    return true;
}