function onSignatureMandateDataBound(e) {
    $("#SignatureMandate .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#SignatureMandate .k-grid-content").attr("style", "max-height: 400px");
    deleteEmptyGridRecord('SignatureMandate');
    isAllConfirmed($("#hdnApplication_LeftMenu_SignatureMandate").val(), "SignatureMandateLegal", "SignatureMandate");
}

function addSMConfirmButton(e) {
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
        GenerateGroupValueList();
        GenerateGroupOneValueList();
        RemoveAllValidationMessage();
    });
    $(".SAVEASDRAFT").click(function (e) {
        model.Status = false;
        GenerateGroupValueList();
        GenerateGroupOneValueList();
    });
    $(".k-widget.k-window.k-display-inline-flex input").keyup(function (e) {
        //debugger;
        var validatePopup = $(".k-popup-edit-form").kendoValidator().data("kendoValidator");
        validatePopup.validate();
        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div')).children('div.field-validation-error').show();
            }
            else {
                $($(this).closest('div')).children('div.field-validation-error').hide();
            }
        }
    });

    $(".k-widget.k-window.k-display-inline-flex input").change(function (e) {
        //debugger;
        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div')).children('div.field-validation-error').show();
            }
            else {
                $($(this).closest('div')).children('div.field-validation-error').hide();
            }
        }
    });
    $(".k-widget.k-window.k-display-inline-flex select").change(function (e) {
        //debugger;
        if ($($(this).closest('div').closest('div')).next('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div').closest('div')).next('div.field-validation-error').show();
            }
            else {
                $($(this).closest('div').closest('div')).next('div.field-validation-error').hide();
            }
        }
    });


    if (isApplicationPermisssionEdit == 'False') {
        setAllPopUpInputElementDisabled();
    }
    setFieldsOnMandatetype();
    GetAuthorizeSignatoryGroupAll(model.AuthorizedSignatoryGroup); //authorizedSignatoryGroupList
}
function error_handlerSignatureMandateCompany(args) {
    var errors = args.errors;
    if (errors) {
        var grid = $("#SignatureMandate").data("kendoGrid");
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

$("#accordionSignatureMandate").kendoPanelBar({
    expandMode: "multiple"
});

var accordion = $("#accordionSignatureMandate").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuSignatureMandate").kendoContextMenu({
    target: "#SignatureMandate",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#SignatureMandate").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowSigManLeg":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "SignatureMandate");
                break;
            default:
                break;
        };
    }
});

function GenerateGroupValueList() {
    var multiselect = $("#AuthorizedSignatoryGroupList").data("kendoMultiSelect");
    var signame = "";
    var items = multiselect.value();
    for (var i = 0; i < items.length; i++) {
        signame = signame + items[i] + "|";
    }
    $("#AuthorizedSignatoryGroupValueList").val(signame).change().focus().focusout();
}
function GenerateGroupOneValueList() {
    var multiselect = $("#AuthorizedSignatoryGroup1List").data("kendoMultiSelect");
    var signame = "";
    var items = multiselect.value();
    for (var i = 0; i < items.length; i++) {
        signame = signame + items[i] + "|";
    }
    $("#AuthorizedSignatoryGroupOneValueList").val(signame).change().focus().focusout();
}

function onSignatoryMandateLegalTypeChange() {
    setFieldsOnMandatetype();
    GetAuthorizeSignatoryGroupAll();
}

function setFieldsOnMandatetype() {
    var ddlSignatoryName = $('#MandateType').data("kendoDropDownList");
    if (ddlSignatoryName != undefined) {
        if (ddlSignatoryName.text().toUpperCase() == 'FOR AUTHORISED PERSONS') {
            var limitfrom = $('#LimitFrom').data("kendoTextBox");
            limitfrom.value('0');
            limitfrom.trigger("change");

            var limitTo = $('#LimitTo').data("kendoTextBox");
            limitTo.value('0');
            limitTo.trigger("change");

            $('#LimitFrom').attr('disabled', 'disabled');
            $('#LimitTo').attr('disabled', 'disabled');
            $('#NumberofSignatures1').attr('disabled', 'disabled');
            var rights = $('#Rights').data("kendoDropDownList");
            rights.value('');
            rights.enable(false);

            var grouplist2 = $('#AuthorizedSignatoryGroup1List').data("kendoMultiSelect");
            grouplist2.value('');
            //grouplist2.value($('#AuthorizedSignatoryGroup1').val());
            grouplist2.enable(false);
            grouplist2.trigger("change");
            //$('#AuthorizedSignatoryGroup').trigger("change");
            $('#NumberofSignatures1').val('');

            //$('#divSignatureRights').show();
            //$('#AllOrJointGroup').data("kendoRadioGroup").value($('#SignatureRightsValue').val());
            //$('#SignatureRightsValue').trigger("change");
        }
        else {
            //$('#LimitFrom').val('');
            //$('#LimitTo').val('');
            $('#LimitFrom').removeAttr('disabled');
            $('#LimitTo').removeAttr('disabled');
            $('#NumberofSignatures1').removeAttr('disabled');
            var rights = $('#Rights').data("kendoDropDownList");
            //rights.value('');
            rights.enable(true);
            var grouplist2 = $('#AuthorizedSignatoryGroup1List').data("kendoMultiSelect");
            grouplist2.value($('#AuthorizedSignatoryGroup1').val());
            grouplist2.enable(true);
            //$('#NumberofSignatures1').val('');
            //$('#divSignatureRights').hide(); //.removeAttr('disabled');
            //$('#AllOrJointGroup').data('kendoRadioGroup').value(null);
        }
        //var grouplist1 = $('#AuthorizedSignatoryGroupList').data("kendoMultiSelect");
        //grouplist1.value($('#AuthorizedSignatoryGroup').val());
        //grouplist1.trigger("change");
    }
}


function GetAuthorizeSignatoryGroupAll(selectedItemValue) {
    debugger;
    var appId = $("#hdnapplicationId").val();
    //var applicationNo = $("#ApplicationNumber").val();
    var ddlMandateType = $('#MandateType').data("kendoDropDownList");
    //var ddlSignatoryName = $('#SignatoryGroupName').data("kendoDropDownList");
    if (ddlMandateType != undefined) {
        var mandateTypeValue = ddlMandateType.value();
        $.ajax({
            url: $("#hdnAuthorizeSignatoryGroupURL").val(),
            cache: false,
            type: "POST",
            data: { applicationid: appId, mandateType: mandateTypeValue },
            success: function (result) {
                //debugger;
                var ddlAuthorizeSignatoryList = $('#AuthorizedSignatoryGroupList').data("kendoMultiSelect");
                //var ddlSignatoryPersonsList = $('#SignatoryPersonsList').data("kendoMultiSelect");
                ddlAuthorizeSignatoryList.setDataSource(result);
                //ddlSignatoryPersonsList.setDataSource(result);
                ddlAuthorizeSignatoryList.dataSource.read();
                //ddlSignatoryPersonsList.dataSource.read();
                if (selectedItemValue != undefined && selectedItemValue != '' && selectedItemValue != null) {
                    ddlAuthorizeSignatoryList.value(selectedItemValue);
                }
            }
        });
    }
}
function onSignatoryMandateLegalTypeDataBound(e) {
    setFieldsOnMandatetype();
}
