
toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}


var AddCustomer = function (id) {
    OpenModalView("/CustomerInfo/AddEdit?id=" + id, "modal-md", 'Add Customer');
};

var AddSupplier = function (id) {
    OpenModalView("/Supplier/AddEdit?id=" + id, "modal-md", 'Add Supplier');
};

var AddCategories = function (id) {
    OpenModalView("/Categories/AddEdit?id=" + id, "modal-md", 'Add Categories');
};

var AddUnitsofMeasure = function (id) {
    OpenModalView("/UnitsofMeasure/AddEdit?id=" + id, "modal-md", 'Add Units of Measure');
};


var SearchInHTMLTable = function () {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("inputRoleSearch");
    filter = input.value.toUpperCase();
    table = document.getElementById("myTable");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[1];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

var SearchByInHTMLTable = function (TableName) {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("inputRoleSearch");
    filter = input.value.toUpperCase();

    tr = TableName.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[1];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

var FieldValidation = function (FieldName) {
    var _FieldName = $(FieldName).val();
    if (_FieldName == "" || _FieldName == null) {
        return false;
    }
    return true;
};

var FieldValidationAlert = function (FieldName, Message, icontype) {
    Swal.fire({
        title: Message,
        icon: icontype,
        onAfterClose: () => {
            $(FieldName).focus();
        }
    });
}

var SwalSimpleAlert = function (Message, icontype) {
    Swal.fire({
        title: Message,
        icon: icontype
    });
}

var FieldIdNullCheck = function (FieldId) {
    if (FieldId === null || FieldId === '')
        FieldId = 0;
    return FieldId;
}

var ViewImage = function (imageURL, Title) {
    $('#titleImageViewModal').html(Title);
    $("#UserImage").attr("src", imageURL);
    $("#ImageViewModal").modal("show");
};

var ViewImageByURLOnly = function (imageURL) {
    $('#titleImageViewModal').html('Item Image');
    $("#UserImage").attr("src", imageURL);
    $("#ImageViewModal").modal("show");
};

var activaTab = function (tab) {
    $('.nav-tabs a[href="#' + tab + '"]').tab('show');
};

var BacktoPreviousPage = function () {
    window.history.back();
}

var printDiv = function (divName, RemoveCssClass) {
    $("table").removeClass(RemoveCssClass);
    var printContents = document.getElementById(divName).innerHTML;
    var originalContents = document.body.innerHTML;
    document.body.innerHTML = printContents;
    window.print();
    document.body.innerHTML = originalContents;
}

var printDivWithCSS = function () {
    var printContents = document.getElementById("printableArea").innerHTML;
    var originalContents = document.body.innerHTML;
    document.body.innerHTML = printContents;
    window.print();
    document.body.innerHTML = originalContents;
}

var printBarcodeDiv = function (Barcode) {
    $("#imgSingleBarcode").attr("src", Barcode);

    var printContents = document.getElementById('divSingleBarcodeDIV').innerHTML;
    var originalContents = document.body.innerHTML;
    document.body.innerHTML = printContents;
    window.print();
    document.body.innerHTML = originalContents;

    $("#imgSingleBarcode").attr("src", '');
}

var DataTableCustomSearchBox = function (width, placeholder) {
    $('.dataTables_filter input[type="search"]').
        attr('placeholder', placeholder).
        css({ 'width': width, 'display': 'inline-block' });
};

var ConvertToShortDate = function (InputDate) {
    var _InputDate = new Date(InputDate).toLocaleDateString('en-US', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
    });
    return _InputDate;
}

var SetDateToday = function (FieldName) {
    var _Date = new Date().toISOString().slice(0, 10);
    $("#" + FieldName).val(_Date);
}

function ConvertDateToMMDDYYYY(UserInput) {
    var date = new Date(UserInput);
    var month = date.getMonth() + 1;
    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
}

function ConvertDateToDDMMYYYY(UserInput) {
    var date = new Date(UserInput);
    var month = date.getMonth() + 1;
    return date.getDate() + "/" + (month.length > 1 ? month : month) + "/" + date.getFullYear();
}

var displayThumbnail = function (fileInputId, thumbnailId) {
    var fileInput = document.getElementById(fileInputId);
    var thumbnail = document.getElementById(thumbnailId);
    if (fileInput.files && fileInput.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            thumbnail.src = e.target.result;
        };
        reader.readAsDataURL(fileInput.files[0]);
    }
}

function addSpaceToUppercase(inputString) {
    let output = '';
    for (let i = 0; i < inputString.length; i++) {
        let currentChar = inputString.charAt(i);
        if (currentChar === currentChar.toUpperCase() && i > 0) {
            output += ' ';
        }
        output += currentChar;
    }
    return output;
}

// Helper: safely parse a number from an input field
const getInputNumber = (selector, defaultValue = 0) => {
    const value = $(selector).val();
    const num = parseFloat(value);
    return Number.isFinite(num) ? num : defaultValue;
};

function initializeSelect2WithValidation(elementIds) {
    elementIds.forEach(function (id) {
        $("#" + id).select2({ placeholder: "--- SELECT ---" }).on("select2:select", function () {
            $(this).valid();
        });
    });
}
