function onSignatureMandateDataBound(e) {
    $("#SignatureMandateIndividual .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#SignatureMandateIndividual .k-grid-content").attr("style", "max-height: 400px");
    deleteEmptyGridRecord('SignatureMandateIndividual');
    isAllConfirmed($("#hdnApplication_LeftMenu_SignatureMandate").val(), "SignatureMandate", "SignatureMandateIndividual");
}

function addConfirmButton_SMI(e) {
    $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
    $("a.k-grid-update").addClass("SAVEASDRAFT");
    var model = e.model;
    model.SignatureMandateIndividual_Status = false;
    model.dirty = true;
    $("a.k-grid-cancel").addClass('btn btn-link');
    $("a.k-grid-update")[0].innerHTML = "SAVE AS DRAFT";
    $("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";

    $('<a class="k-button k-button-icontext k-primary k-grid-update custom" href="#" k-icon k-i-delete"></span>SAVE & CLOSE</a>').insertBefore(".k-grid-update");

    $(".custom").click(function (e) {
        model.SignatureMandateIndividual_Status = true;
        GenerateSignatoryPersonValueList();
    });
    $(".SAVEASDRAFT").click(function (e) {
        model.SignatureMandateIndividual_Status = false;
        GenerateSignatoryPersonValueList();
    });
    $(".k-widget.k-window.k-display-inline-flex input").keyup(function (e) {
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
        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div')).children('div.field-validation-error').show();
            }
            else {
                $($(this).closest('div')).children('div.field-validation-error').hide();
            }
        }
    });

    if (isApplicationPermisssionEdit == 'False') {
        setAllPopUpInputElementDisabled();
    }
}
function error_handlerSignatureMandate(args) {
    var errors = args.errors;
    if (errors) {
        var grid = $("#SignatureMandateIndividual").data("kendoGrid");
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

$("#accordionSignatureMandateIndividual").kendoPanelBar({
    expandMode: "multiple"
});

var accordion = $("#accordionSignatureMandateIndividual").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuSignatureMandateIndividual").kendoContextMenu({
    target: "#SignatureMandateIndividual",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#SignatureMandateIndividual").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowSigManInd":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "SignatureMandateIndividual");
                break;
            default:
                break;
        };
    }
});

$(document).ready(function () {
    //debugger;
    var radioText = $("input[name='SignatureMandateTypeGroup']:checked").next('label').text();
    if (radioText == "Other") {
        $("#signatureMandateDIV").css("display", "block")
        isAllConfirmed($("#hdnApplication_LeftMenu_SignatureMandate").val(), "SignatureMandate", "SignatureMandateIndividual");
    }
    else {
        $("#signatureMandateDIV").css("display", "none")
        setMenuSuccess($("#hdnApplication_LeftMenu_SignatureMandateIndividual").val());
    }
});
$(document).on("change", "input[name='SignatureMandateTypeGroup']", function () {
    var radioText = $("input[name='SignatureMandateTypeGroup']:checked").next('label').text();
    if (radioText == "Other") {
        $("#signatureMandateDIV").css("display", "block")
        isAllConfirmed($("#hdnApplication_LeftMenu_SignatureMandate").val(), "SignatureMandate", "SignatureMandateIndividual");
    }
    else {
        $("#signatureMandateDIV").css("display", "none")
        setMenuSuccess($("#hdnApplication_LeftMenu_SignatureMandateIndividual").val());
    }
});

function runAfterLoad() {
    var radioText = $("input[name='SignatureMandateTypeGroup']:checked").next('label').text();
    if (radioText == "Other") {
        $("#signatureMandateDIV").css("display", "block")
        isAllConfirmed($("#hdnApplication_LeftMenu_SignatureMandate").val(), "SignatureMandate", "SignatureMandateIndividual");
    }
    else {
        $("#signatureMandateDIV").css("display", "none")
        setMenuSuccess($("#hdnApplication_LeftMenu_SignatureMandateIndividual").val());
    }
}
window.addEventListener("load", function () {
    window.setTimeout(runAfterLoad, 1000);
}, false);

