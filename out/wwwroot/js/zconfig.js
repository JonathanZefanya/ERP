let IsDemoUser = 1; // 1=Demo User, 0=Regular User
let IsAddress = 1;
// let AppPurchaseURL = 'https://1.envato.market/' + 'AoMzyj';

var DemoUserAccountLockAll = function () {
    if (IsDemoUser == 1) {
        SwalSimpleAlert("You are not allowed to change demo user info", "warning");
        return IsDemoUser;
    }
    else {
        return IsDemoUser;
    }
}

// var SetHeaderElements = function (ElementName) {
//     if (IsAddress == 1) {
//         var _WhatsAppElement = WhatsAppElement();
//         var _BuyNowElement = BuyNowElement();
//         $('#' + ElementName).empty().append(_BuyNowElement + _WhatsAppElement);
//     }
// }

// var SetLoginInfo = function (divName) {
//     if (IsAddress == 1) {
//         var _GetLoginInfoElement = GetLoginInfoElement();
//         var _GetContactDetails = GetContactDetails();
//         $('#' + divName).empty().append(_GetLoginInfoElement + _GetContactDetails);
//     }
// }

// var SetFooterElements = function (divName) {
//     if (IsAddress == 1) {
//         var _CopyrightElement = CopyrightElement();
//         var _GetContactDetails = GetContactDetails();
//         $('#' + divName).empty().append(_GetContactDetails + _CopyrightElement);
//     }
// }

// var GetContactDetails = function () {
//     var content = '';
//     var _WhatsAppElement = WhatsAppElement();
//     content += '<b>After purchase you will get following items,</b> <br />'
//         + '✅ Complete Source Code <br />'
//         + '✅ Complete Database (code first) <br />'
//         + '✅ Complete Project Documentation <br />'
//         + '✅ ERD Diagram <br />'
//         + '✅ Architecture Diagram <br />'
//         + '✅ Video Tutorials <br />'
//         + '✅ Free Tech Support <br />'
//         + '💛 <a href="' + AppPurchaseURL + '" target="_blank">Purchase Now</a><br />'

//         + '<b>If you want to modify or add new features, you can always contact us at,</b> <br />'
//         + _WhatsAppElement + '<br />'
//         + '📢 Telegram: <a href="#" id="linkTelegram" onclick="CopyToClipboard(this)">shahed71bd</a><br />'
//         + '📢 Skype: <a href="#" id="linkSkype" onclick="CopyToClipboard(this)">shahedatomap</a><br />'
//         + '🙏 Email: <a href="#" id="linkEmail" onclick="CopyToClipboard(this)">shahedbddev@gmail.com</a><br />'
//         + '♾️ Web: <a href="https://netspeedm.com/" target="_blank">Zero Byte Solutions</a>';

//     return content;
// }

// var CopyrightElement = function () {
//     var elements = '';
//     elements += '<hr /><strong>Copyright &copy; 2021-2022 '
//         + '<a href="https://github.com/shahedbd" target="_blank">Zero Byte</a>. </strong>'
//         + 'All rights reserved'
//         + '<a href="https://www.youtube.com/channel/UCdHAVwuNUtfqZRFVI6qf7mg?sub_confirmation=1" target="_blank">'
//         + '<img src="/images/dashboard/scnow2.png" class="imgCustom150px" alt="User Image">'
//         + '</a>'
//         + '';
//     return elements;
// }

// var WhatsAppElement = function () {
//     var elements = '';
//     elements = '<span>📲 WhatsApp: </span><a href="https://wa.me/8801674411603" target="_blank">8801674411603</a>';
//     return elements;
// }

// var BuyNowElement = function () {
//     var elements = '';
//     elements += '<a href="' + AppPurchaseURL + '" target="_blank">'
//         + '<img src="/images/dashboard/buynow.png" class="imgCustom150px" alt="User Image">'
//         + '</a>';
//     return elements;
// }

var CopyToClipboard = function (Element) {
    var _Element = $('#' + Element.id).html();
    navigator.clipboard.writeText(_Element);
    toastr.success('Address copy to clipboard, successfully.');
}


var GetLoginInfoElement = function () {
    var elements = '';
    elements += ''
        + '<h6 class="ms-1">Login Info</h6>'
        + '<table border="1">'
        + '<thead><tr><th>Email/Password</th><th></th></tr></thead>'
        + '<tbody><tr>'
        + '<td><label id="lblEmail">admin@gmail.com</label>/<label id="lblPassword">123</label></td>'
        + '<td><button id="btnCopy" class="btn btn-sm btn-info" onclick="GetLoginCred()">Copy</button>'
        + '<a href="#" onclick="GetLoginCredClear()">Clear</a></td>'
        + '</tr></tbody>'
        + '</table>'
        + '<hr />';
    return elements;
}

var GetLoginCred = function () {
    var _lblEmail = $("#lblEmail").text();
    var _lblPassword = $("#lblPassword").text();
    $("#Email").val(_lblEmail);
    $("#Password").val(_lblPassword);

    // Create a text to copy
    var loginCred = "Email: " + _lblEmail + "\nPassword: " + _lblPassword;

    // Copy the login info to the clipboard
    var tempTextArea = document.createElement("textarea");
    tempTextArea.value = loginCred;
    document.body.appendChild(tempTextArea);
    tempTextArea.select();
    document.execCommand("copy");
    document.body.removeChild(tempTextArea);

    // Change button text and color for 5 seconds
    var button = $("#btnCopy");
    button.text("Copied");
    button.removeClass("btn-info").addClass("btn-success");

    setTimeout(function () {
        button.text("Copy");
        button.removeClass("btn-success").addClass("btn-info");
        //$("#btnUserLogin").trigger('click');
    }, 1000);
}

var GetLoginCredClear = function () {
    $("#Email").val('');
    $("#Password").val('');
    $("#Email").focus();
}

var SaveMetaData = function () {
    if (IsDemoUser == 1) {
        var result = sessionStorage.getItem('GetDateWithRandomNumberBizERP');
        console.log(result);
        if (result == null) {
            SaveUserInfoFromBrowser();
        }
        var _GetDateWithRandomNumber = GetDateWithRandomNumber('INV_');
        sessionStorage.setItem('GetDateWithRandomNumberBizERP', _GetDateWithRandomNumber);
    }
}

var GetDateWithRandomNumber = function (Prefix) {
    //let _RandomNumber = Math.floor((Math.random() * 10000) + 1000);
    var _Date = new Date();
    var datestring = _Date.getFullYear() + ("0" + (_Date.getMonth() + 1)).slice(-2) + ("0" + _Date.getDate()).slice(-2)
        + ("0" + _Date.getHours()).slice(-2) + ("0" + _Date.getMinutes()).slice(-2) + ("0" + _Date.getSeconds()).slice(-2);

    var resut = Prefix + datestring;
    return resut;
}

var ButtonEDLoader = function (IsEnabled, FieldName, Title) {
    if (IsEnabled == true) {
        $('#buttonSpinner').empty();
        $("#" + FieldName).html(Title);
        $("#" + FieldName).removeAttr('disabled');
    }
    else {
        $("#" + FieldName).html(Title + ' ' + '<span id="buttonSpinner"><i class="fa fa-spinner fa-spin"></i></span>');
        $("#" + FieldName).attr('disabled', 'disabled');
    }
}

const storeCompanyInfo = (data) => localStorage.setItem('lsCompanyInfoData', JSON.stringify(data));
const getStoredCompanyInfo = () => JSON.parse(localStorage.getItem('lsCompanyInfoData'));

const GetCompanyDataFromLS = async () => {
    let companyInfo = getStoredCompanyInfo();
    if (!companyInfo) {
        companyInfo = await GetCompanyInfoData();

        if (companyInfo) {
            storeCompanyInfo(companyInfo);
        }
    }
    return companyInfo;
}

const GetCompanyInfoData = async () => {
    const apiUrl = '/CompanyInfo/GetCompanyInfoData';
    try {
        const response = await $.getJSON(apiUrl);
        return response;
    } catch (error) {
        console.error('Error fetching company info:', error);
        return null;
    }
};

var UpdateAppLogo = async function (imageId) {
    let _GetCompanyDataFromLS = await GetCompanyDataFromLS();
    if (_GetCompanyDataFromLS) {
        $(`#${imageId}`).attr("src", _GetCompanyDataFromLS.Logo);
    }
}