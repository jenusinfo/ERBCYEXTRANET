function onEditButtonChangesPersonRegistry(e) {
    $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
    var model = e.model;
    model.Status = false;
    model.dirty = true;
    var Type = $("#ApplicationTypeID").data("kendoDropDownList").text();
    if (Type == "INDIVIDUAL") {
        $("#PersonDetailsDIV").css("display", "block");
    }
    else if (Type == "LEGAL ENTITY") {
        $("#CompanyDetailsDIV").css("display", "block");
    }
    $("a.k-grid-cancel").addClass('btn btn-link');
    $("a.k-grid-update")[0].innerHTML = "SAVE & CLOSE";
    $("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";
    $(".k-grid-update").click(function (e) {
        model.Status = true;
        RemoveAllValidationMessage();
    });
    $(".k-widget.k-window.k-display-inline-flex input").keyup(function (e) {
        var validatePopup = $(".k-popup-edit-form").kendoValidator().data("kendoValidator");
        validatePopup.validate();
    });
    $(".k-widget.k-window.k-display-inline-flex input").keyup(function (e) {
        var validatePopup = $(".k-popup-edit-form").kendoValidator().data("kendoValidator");
        validatePopup.validate();
        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div')).children('div.field-validation-error').show();
            }
            else {
                if ($(this).attr('id') == 'IssueDate' || $(this).attr('id') == 'ExpiryDate') {
                    validateIdentificationDates(this);
                }
                else {
                    $($(this).closest('div')).children('div.field-validation-error').hide();
                }

            }
        }
    });
    $(".k-widget.k-window.k-display-inline-flex input").change(function (e) {
        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div')).children('div.field-validation-error').show();
            }
            else {
                if ($(this).attr('id') == 'IssueDate' || $(this).attr('id') == 'ExpiryDate') {
                    validateIdentificationDates(this);
                }
                else {
                    $($(this).closest('div')).children('div.field-validation-error').hide();
                }
            }
        }
    });
}
function validateIdentificationDates(currentElement) {
    $.ajax({
        url: $("#ValidateIdentificationDatesUrl").val(),
        cache: false,
        type: "POST",
        data: { issueDate: $("#IssueDate").val(), expiryDate: $("#ExpiryDate").val() },
        success: function (result) {
            if (result == true) {
                $($(currentElement).closest('div')).children('div.field-validation-error').hide();
            }
            else {
                $($(currentElement).closest('div')).children('div.field-validation-error').show();
            }
        }
    });
}
function error_handlerPersonRegistry(args) {
    var errors = args.errors;
    if (errors) {
        args.sender.preventSync = true;
        var grid = $("#Registry").data("kendoGrid");
        grid.one("dataBinding", function (e) {
            e.preventDefault();
            $.each(errors, function (key, value) {
                var message = "";
                if ('errors' in value) {
                    $.each(value.errors, function () {
                        message += this + "\n";
                    });
                }
                grid.editable.element.find("[data-valmsg-for='" + key + "']").replaceWith('<div class="k-tooltip k-tooltip-error k-validator-tooltip k-invalid-msg field-validation-error" ><span class="k-tooltip-icon k-icon k-i-warning"> </span><span class="k-tooltip-content">' + message + '</span><div class="k-callout k-callout-n"></div></div>').show();
            });
        });
    }
}
function onAddressGridEditing(e) {
    $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
    var model = e.model;
    model.Status = false;
    model.dirty = true;
    $("a.k-grid-cancel").addClass('btn btn-link');
    $("a.k-grid-update")[0].innerHTML = "SAVE & CLOSE";
    $("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";

    $(".k-widget.k-window.k-display-inline-flex input").keyup(function (e) {
        var validatePopup = $(".k-popup-edit-form").kendoValidator().data("kendoValidator");
        validatePopup.validate();
    });
    ShowRespectedDivAddressRegistry();
    ShowHideFieldsOnAddressType();

    $(".k-widget.k-window.k-display-inline-flex input").keyup(function (e) {
        var validatePopup = $(".k-popup-edit-form").kendoValidator().data("kendoValidator");
        validatePopup.validate();
        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                if ($($($(this).closest('div')).children('div.field-validation-error')).children('span.k-tooltip-content').text().indexOf('Max length exceeded') < 0) {
                    $($(this).closest('div')).children('div.field-validation-error').show();
                }
                else {
                    $($(this).closest('div')).children('div.field-validation-error').hide();
                }
            }
            else {
                if ($(this).attr('id') == 'City' && $("#Country").data("kendoDropDownList") != undefined) {
                    var countryddlValue = $("#Country").data("kendoDropDownList").text();
                    var cityValue = $(this).val().trim().toLowerCase();
                    if (countryddlValue == 'CYPRUS' && (cityValue != 'nicosia' && cityValue != 'limassol' && cityValue != 'paphos' && cityValue != 'larnaca' && cityValue != 'famagusta')) {
                        $($(this).closest('div')).children('div.field-validation-error').show();
                    }
                    else {
                        if ($($($(this).closest('div')).children('div.field-validation-error')).children('span.k-tooltip-content').text().indexOf('Max length exceeded') < 0) {
                            $($(this).closest('div')).children('div.field-validation-error').hide();
                        }
                        else if ($(this).val().length <= 35) {
                            $($(this).closest('div')).children('div.field-validation-error').hide();
                        }
                        else {
                            $($(this).closest('div')).children('div.field-validation-error').show();
                        }
                    }
                }
                else {
                    if ($($($(this).closest('div')).children('div.field-validation-error')).children('span.k-tooltip-content').text().indexOf('Max length exceeded') < 0) {
                        $($(this).closest('div')).children('div.field-validation-error').hide();
                    }
                    else if ($(this).val().length <= 35) {
                        $($(this).closest('div')).children('div.field-validation-error').hide();
                    }
                    else {
                        $($(this).closest('div')).children('div.field-validation-error').show();
                    }
                }
            }
        }
    });

    $(".k-widget.k-window.k-display-inline-flex input").change(function (e) {
        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                if ($($($(this).closest('div')).children('div.field-validation-error')).children('span.k-tooltip-content').text().indexOf('Max length exceeded') < 0) {
                    $($(this).closest('div')).children('div.field-validation-error').show();
                }
                else {
                    $($(this).closest('div')).children('div.field-validation-error').hide();
                }
            }
            else {
                if ($(this).attr('id') == 'City' && $("#Country").data("kendoDropDownList") != undefined) {
                    var countryddlValue = $("#Country").data("kendoDropDownList").text();
                    var cityValue = $(this).val().trim().toLowerCase();
                    if (countryddlValue == 'CYPRUS' && (cityValue != 'nicosia' && cityValue != 'limassol' && cityValue != 'paphos' && cityValue != 'larnaca' && cityValue != 'famagusta')) {
                        $($(this).closest('div')).children('div.field-validation-error').show();
                    }
                    else {
                        if ($($($(this).closest('div')).children('div.field-validation-error')).children('span.k-tooltip-content').text().indexOf('Max length exceeded') < 0) {
                            $($(this).closest('div')).children('div.field-validation-error').hide();
                        }
                        else if ($(this).val().length <= 35) {
                            $($(this).closest('div')).children('div.field-validation-error').hide();
                        }
                        else {
                            $($(this).closest('div')).children('div.field-validation-error').show();
                        }
                    }
                }
                else {
                    if ($($($(this).closest('div')).children('div.field-validation-error')).children('span.k-tooltip-content').text().indexOf('Max length exceeded') < 0) {
                        $($(this).closest('div')).children('div.field-validation-error').hide();
                    }
                    else if ($(this).val().length <= 35) {
                        $($(this).closest('div')).children('div.field-validation-error').hide();
                    }
                    else {
                        $($(this).closest('div')).children('div.field-validation-error').show();
                    }
                }
            }
        }
    });
}
$("#context-menu").kendoContextMenu({
    target: "#AddressRegistry",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#AddressRegistry").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editAddressRow":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "AddressRegistry", "Address Registry", "Address Registry deleted successfuly");
                break;
            default:
                break;
        };
    }
});

$("#context-menuRegistry").kendoContextMenu({
    target: "#Registry",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#Registry").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editPersonRow":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "Registry");
                break;
            default:
                break;
        };
    }
});
function error_handlerAddressRegistry(args) {
    var errors = args.errors;
    if (errors) {
        args.sender.preventSync = true;
        var grid = $("#AddressRegistry").data("kendoGrid");
        grid.one("dataBinding", function (e) {
            e.preventDefault();
            $.each(errors, function (key, value) {
                var message = "";
                if ('errors' in value) {
                    $.each(value.errors, function () {
                        message += this + "\n";
                    });
                }
                grid.editable.element.find("[data-valmsg-for='" + key + "']").replaceWith('<div class="k-tooltip k-tooltip-error k-validator-tooltip k-invalid-msg field-validation-error" ><span class="k-tooltip-icon k-icon k-i-warning"> </span><span class="k-tooltip-content">' + message + '</span><div class="k-callout k-callout-n"></div></div>').show();
            });
        });
    }
}
function ShowRespectedDivAddressRegistry() {
    var selectValue = $("#AddressType").data("kendoDropDownList").text();
    var dropdownlist = $("#Country").data("kendoDropDownList");
    var dropdownlistPhone = $("#CountryCode_PhoneNo").data("kendoDropDownList");
    var dropdownlistFax = $("#CountryCode_FaxNo").data("kendoDropDownList");
    if (selectValue == "OFFICE IN CYPRUS") {
        $("#PhoneNoDIV").css("display", "block");
        $("#FaxNoDIV").css("display", "block");
        $("#EmailDIV").css("display", "block");
        if (dropdownlist != undefined) {
            dropdownlist.text("CYPRUS");
            dropdownlist.readonly();
            dropdownlist.trigger("change");
        }
        if (dropdownlistPhone != undefined) {
            dropdownlistPhone.value("357");
            dropdownlistPhone.readonly();
            dropdownlistPhone.trigger("change");
        }
        if (dropdownlistFax != undefined) {
            dropdownlistFax.value("357");
            dropdownlistFax.readonly();
            dropdownlistFax.trigger("change");
        }
    }
    else if (selectValue == "PRINCIPAL TRADING /BUSINESS OFFICE") {
        $("#PhoneNoDIV").css("display", "block");
        $("#FaxNoDIV").css("display", "block");
        $("#EmailDIV").css("display", "block");
        if (dropdownlistPhone != undefined) {
            dropdownlistPhone.readonly(false);
        }

        if (dropdownlistFax != undefined) {
            dropdownlistFax.readonly(false);
        }
        if (dropdownlist != undefined) {
            dropdownlist.readonly(false);
        }
    }
    else {
        $("#PhoneNoDIV").css("display", "none");
        $("#FaxNoDIV").css("display", "none");
        $("#EmailDIV").css("display", "none");
        if (dropdownlistPhone != undefined) {
            dropdownlistPhone.readonly(false);
        }

        if (dropdownlistFax != undefined) {
            dropdownlistFax.readonly(false);
        }
        if (dropdownlist != undefined) {
            dropdownlist.readonly(false);
        }
    }

}
function ShowHideFieldsOnAddressType() {
    var selectedAddressType = $("#AddressType").data("kendoDropDownList");
    if (selectedAddressType != undefined) {
        if (selectedAddressType.text().trim() == 'MAILING ADDRESS') {
            $('#divAddPOBox').show();
        }
        else {
            $("#POBox").val('').trigger('change').focusin().focusout();
            $('#divAddPOBox').hide();
        }
    }
}
function limitlengthAccToCountry(obj, spanid, ddlid) {
    var selectedCountryNo = $('#' + ddlid).data("kendoDropDownList") != undefined ? $('#' + ddlid).data("kendoDropDownList").text() : '';
    if (selectedCountryNo == 'CYPRUS +357') {
        var maxlength = 8;
        var errormsg = 'In case of Cyprus Maximum allowed length is ' + maxlength;
    }
    else {
        var maxlength = 10;
        var errormsg = 'Maximum allowed length of the input text is ' + maxlength;
    }
    if (obj.value.length > maxlength) {
        document.getElementById(spanid).innerHTML = errormsg;
        obj.value = obj.value.substring(0, maxlength)
    }
    else {
        document.getElementById(spanid).innerHTML = ' ';
    }
}
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function ChangeMaxLengthLegalAddressPhone() {
    var selectedPhoneNo = $("#CountryCode_PhoneNo").data("kendoDropDownList") != undefined ? $("#CountryCode_PhoneNo").data("kendoDropDownList").text() : '';
    field = document.getElementById('PhoneNo');

    if (selectedPhoneNo == 'CYPRUS +357') {
        field.value = field.value.substr(0, 8);
    }
    $("#PhoneNo").keyup();
}

function ChangeMaxLengthLegalAddressFax() {
    var selectedPhoneNo = $("#CountryCode_FaxNo").data("kendoDropDownList") != undefined ? $("#CountryCode_FaxNo").data("kendoDropDownList").text() : '';
    field = document.getElementById('FaxNo');

    if (selectedPhoneNo == 'CYPRUS +357') {
        field.value = field.value.substr(0, 8);
    }
    $("#FaxNo").keyup();
}
function limitlength(obj, length, spanid) {
    var maxlength = length
    if (obj.value.length > maxlength) {
        document.getElementById(spanid).innerHTML = 'Maximum allowed length of the input text is ' + length;
        obj.value = obj.value.substring(0, maxlength)
    }
    else {
        document.getElementById(spanid).innerHTML = ' ';
    }
}

function ChangeMaxLengthIndividualHomeNo() {
    var selectedPhoneNo = $("#Country_Code_HomeTelNoNumber").data("kendoDropDownList") != undefined ? $("#Country_Code_HomeTelNoNumber").data("kendoDropDownList").text() : '';
    field = document.getElementById('ContactDetails_HomeTelNoNumber');

    if (selectedPhoneNo == 'CYPRUS +357') {
        field.value = field.value.substr(0, 8);
        field.maxLength = 8;
    } else {
        field.maxLength = 10;
    }
}
function ChangeMaxLengthIndividualMobileNo() {
    var selectedPhoneNo = $("#Country_Code_MobileTelNoNumber").data("kendoDropDownList") != undefined ? $("#Country_Code_MobileTelNoNumber").data("kendoDropDownList").text() : '';
    field = document.getElementById('ContactDetails_MobileTelNoNumber');

    if (selectedPhoneNo == 'CYPRUS +357') {
        field.value = field.value.substr(0, 8);
        field.maxLength = 8;
    } else {
        field.maxLength = 10;
    }
}
function ChangeMaxLengthIndividualWorkNo() {
    var selectedPhoneNo = $("#Country_Code_WorkTelNoNumber").data("kendoDropDownList") != undefined ? $("#Country_Code_WorkTelNoNumber").data("kendoDropDownList").text() : '';
    field = document.getElementById('ContactDetails_WorkTelNoNumber');

    if (selectedPhoneNo == 'CYPRUS +357') {
        field.value = field.value.substr(0, 8);
        field.maxLength = 8;
    } else {
        field.maxLength = 10;
    }
}
function ChangeMaxLengthIndividualFaxNo() {
    var selectedPhoneNo = $("#Country_Code_FaxNoFaxNumber").data("kendoDropDownList") != undefined ? $("#Country_Code_FaxNoFaxNumber").data("kendoDropDownList").text() : '';
    field = document.getElementById('ContactDetails_FaxNoFaxNumber');

    if (selectedPhoneNo == 'CYPRUS +357') {
        field.value = field.value.substr(0, 8);
        field.maxLength = 8;
    } else {
        field.maxLength = 10;
    }
}
function isNumberKeyWithSlash(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 47 || charCode > 57))
        return false;

    return true;
}

function RemoveAllValidationMessage() {
    $(".k-widget.k-window.k-display-inline-flex input").each(function (i, obj) {
        if (($(this).closest('div')).children('div.field-validation-error').length > 0) {
            var elementName = $(this).attr('name');
            $($(this).closest('div')).children('div.field-validation-error').remove();
            $($(this).closest('div')).append('<span class="field-validation-valid k-hidden" data-valmsg-for="' + elementName + '" data-valmsg-replace="true"></span>');
        }
    });
}
var loadingPanelVisible = false;
$("#loader").kendoLoader({
    visible: false
});
function showHideLoader() {
    var loader = $("#loader").data("kendoLoader");
    loadingPanelVisible = !loadingPanelVisible;
    if (loadingPanelVisible) {
        loader.show();
    } else {
        loader.hide();
    }
}
$("#navbarNavDropdown > ul > li").click(function () {
    showHideLoader()
})
$(".logo-image").click(function () {
    showHideLoader()
})
function Sync_handlerPersonRegistry(e) {
    if (!e.sender.preventSync) {
        e.sender.read();
    }
    e.sender.preventSync = false;
}
function Sync_handlerAddressRegistry(e) {
    if (!e.sender.preventSync) {
        e.sender.read();
    }
    e.sender.preventSync = false;
}