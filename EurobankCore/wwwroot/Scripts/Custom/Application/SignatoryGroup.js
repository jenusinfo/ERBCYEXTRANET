function onGridDataBoundSignatoryGroup(e) {
    $("#SignatoryGroup .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#SignatoryGroup .k-grid-content").attr("style", "max-height: 400px");
    deleteEmptyGridRecord('SignatoryGroup');
}
function addSnGroupConfirmButton(e) {
    $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
    $("a.k-grid-update").addClass("SAVEASDRAFT");
    var model = e.model;
    model.Status = false;
    model.dirty = true;
    $("a.k-grid-cancel").addClass('btn btn-link');
    $("a.k-grid-update")[0].innerHTML = "SAVE AS DRAFT";
    $("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";

    $('<a class="k-button k-button-icontext k-primary k-grid-update custom" href="#" k-icon k-i-delete"></span>SAVE & CLOSE</a>').insertBefore(".k-grid-update");
    $(".custom").click(function (e) {
        model.Status = true;

    });
    $(".SAVEASDRAFT").click(function (e) {
        model.Status = false;

    });

    $('#AllOrJointGroup').data("kendoRadioGroup").value($('#SignatureRightsValue').val());
    $('#SignatureRightsValue').trigger("change");

    $(".k-widget.k-window.k-display-inline-flex input").keyup(function (e) {
        var validatePopup = $(".k-popup-edit-form").kendoValidator().data("kendoValidator");
        validatePopup.validate();
        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div')).children('div.field-validation-error').show();
            }
            else {
                if ($(this).attr('id') == 'StartDate' || $(this).attr('id') == 'EndDate') {
                    validateSignatoryGroupDates(this);
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
                if (($(this).attr('id') == 'StartDate' || $(this).attr('id') == 'EndDate') && ($("#EndDate") != undefined && $("#EndDate").val() != '' && $("#StartDate") != undefined && $("#StartDate").val() != '')) {
                    validateSignatoryGroupDates(this);
                }
                else {
                    $($(this).closest('div')).children('div.field-validation-error').hide();
                }

            }
        }
    });
    $(".k-widget.k-window.k-display-inline-flex select").change(function (e) {
        if ($($(this).closest('div').parent()).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div').parent()).children('div.field-validation-error').show();
            }
            else {
                if (($(this).attr('id') == 'StartDate' || $(this).attr('id') == 'EndDate') && ($("#EndDate") != undefined && $("#EndDate").val() != '' && $("#StartDate") != undefined && $("#StartDate").val() != '')) {
                    validateSignatoryGroupDates(this);
                }
                else {
                    $($(this).closest('div').parent()).children('div.field-validation-error').hide();
                }

            }
        }
    });

    if (isApplicationPermisssionEdit == 'False') {
        setAllPopUpInputElementDisabled();
    }
    GetSignatoryPersons(model.SignatoryPersonsList);
    ShowHideSignatoryRights();
}

function GetSignatoryPersons(selectedItemValue) {
    var applicationNo = $("#ApplicationNumber").val();
    var ddlSignatoryName = $('#SignatoryGroupName').data("kendoDropDownList");
    if (ddlSignatoryName != undefined) {
        var signatoryNameValue = ddlSignatoryName.value();
        $.ajax({
            url: $("#SignatoryPersons_ReadUrl").val(),
            cache: false,
            type: "POST",
            data: { applicationNumber: applicationNo, signatoryName: signatoryNameValue },
            success: function (result) {
                //debugger;
                var ddlSignatoryPersonsList = $('#SignatoryPersonsList').data("kendoMultiSelect");
                ddlSignatoryPersonsList.setDataSource(result);
                ddlSignatoryPersonsList.dataSource.read();
                if (selectedItemValue != undefined && selectedItemValue != '' && selectedItemValue != null) {
                    ddlSignatoryPersonsList.value(selectedItemValue);
                }
            }
        });
    }
}

function ShowHideSignatoryRights() {
    //var ddlSignatoryName = $('#SignatoryGroupName').data("kendoDropDownList");
    //if (ddlSignatoryName != undefined) {
    //    if (ddlSignatoryName.text().toUpperCase() == 'AUTHORISED PERSONS') {
    //        $('#divSignatureRights').show();
    //        $('#AllOrJointGroup').data("kendoRadioGroup").value($('#SignatureRightsValue').val());
    //        $('#SignatureRightsValue').trigger("change");
    //    }
    //    else {
    //        $('#divSignatureRights').hide();
    //        $('#AllOrJointGroup').data('kendoRadioGroup').value(null);
    //    }
    //}
    $('#divSignatureRights').hide();
    $('#AllOrJointGroup').data('kendoRadioGroup').value(null);
}

function onSignatoryGroupNameChange() {
    GetSignatoryPersons();
    ShowHideSignatoryRights();
}

function onSignatureRightsDataBound(e) {
    $('#AllOrJointGroup').data("kendoRadioGroup").value($('#SignatureRightsValue').val());
    $('#SignatureRightsValue').trigger("change");
}

function onSignatoryGroupDataBound(e) {
    ShowHideSignatoryRights();
}
function validateSignatoryGroupDates(currentElement) {

    $.ajax({
        url: $("#ValidateSignatoryGroupDatesUrl").val(),
        cache: false,
        type: "POST",
        data: { startDate: $("#StartDate").val(), endDate: $("#EndDate").val() },
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

function error_handlerSignatoryGroup(args) {
    var errors = args.errors;
    if (errors) {
        args.sender.preventSync = true;
        var grid = $("#SignatoryGroup").data("kendoGrid");
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
$("#accordionSignatoryGroup").kendoPanelBar({
    expandMode: "multiple"
});

var accordion = $("#accordionSignatoryGroup").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuSignatoryGroup").kendoContextMenu({
    target: "#SignatoryGroup",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#SignatoryGroup").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowSignGp":
                var check = IsSignatoryUseInMandate(data);
                if (check == "True") {
                    SignatoryEditDeleteConfirmation(e);
                    break;
                }
                else {
                    grid.editRow(row);
                    break;
                }
            case "removeRow":
                //debugger;
                var check = IsSignatureValidateforDelete(data);
                if (check == "True") {
                    DisplayDeleteConfirmationContextMenu(e, data, "SignatoryGroup");
                    break;
                }
                else {
                    SignatoryGroupDeleteWarningMsg(e);
                    break;
                }
                IsSignatoryUseInMandate(data);
                //DisplayDeleteConfirmationContextMenu(e, data, "SignatoryGroup");
                break;
            default:
                break;
        };
    }
});
function IsSignatoryUseInMandate(data) {
    //debugger;
    var res = "";
    $("#HdnSignatoryGroupName").val(data.SignatoryGroup);
    $.ajax({
        async: false,
        type: "POST",
        url: $("#SignatoryUsedInMandateUrl").val(),
        dataType: "json",
        cache: false,
        data: { id: $("#ApplicationId").val(), editGroupName: data.SignatoryGroup },
        success: function (result) {
            res = result;
        }
    });
    return res;
}
function IsSignatureValidateforDelete(data) {
    //debugger;
    var res = "";
    $.ajax({
        async: false,
        type: "POST",
        url: $("#SignatureValidateforDeleteUrl").val(),
        dataType: "json",
        cache: false,
        data: { apID: $("#ApplicationId").val(), SignatoryGroupName: data.SignatoryGroup }, //signatoryGroupModel: data,
        success: function (result) {
            //debugger;
            res = result;
        }
    });
    return res;
}
function SignatoryEditDeleteConfirmation(e) {
    //debugger;
    var dialog = $("#SignatoryGroupEditDeleteConfirmation").data("kendoWindow");
    dialog.wrapper.addClass("middle-popup");
    dialog.center().open();
    $("#SignatoryEditYes").unbind().click(function () {
        //debugger;
        UpdateSignatureMandateRec();
        var dialog = $("#SignatoryGroupEditDeleteConfirmation").data("kendoWindow");
        dialog.close();
        var row = $(e.target).parent()[0];
        var grid = $("#SignatoryGroup").data("kendoGrid");
        grid.editRow(row);
    })
    $("#SignatoryEditNo").click(function () {
        var dialog = $("#SignatoryGroupEditDeleteConfirmation").data("kendoWindow");
        dialog.close();
    })
}
function SignatoryGroupDeleteWarningMsg(e) {
    //debugger;
    var dialog = $("#SignatoryGroupDeleteWarningMsg").data("kendoWindow");
    dialog.wrapper.addClass("middle-popup");
    dialog.center().open();
    $("#SignatoryGroupOk").click(function () {
        var dialog = $("#SignatoryGroupDeleteWarningMsg").data("kendoWindow");
        dialog.close();
    })
}
function UpdateSignatureMandateRec() {
    $.ajax({
        url: $("#SignatureMandateStatusUpdateUrl").val(),
        cache: false,
        type: "POST",
        data: { id: $("#ApplicationId").val(), editGroupName: $("#HdnSignatoryGroupName").val() },
        success: function (result) {
            $('#SignatureMandate').data('kendoGrid').dataSource.read();
            $('#SignatureMandate').data('kendoGrid').refresh();
        }
    });
}