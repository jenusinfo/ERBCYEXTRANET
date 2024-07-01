function onIdentificationDataBound(e) {
    $("#IdentificationDetails .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#IdentificationDetails .k-grid-content").attr("style", "max-height: 400px");
    var grid = $('#IdentificationDetails').data('kendoGrid');
    var gridRows = grid.tbody.find("tr");
    gridRows.each(function (e) {
        var rowItem = grid.dataItem($(this));
        if (rowItem.IdentificationDetailsID == 0) {
            grid.removeRow(rowItem);
        }
    });
    isAllConfirmed($("#hdnApplication_RelatedParty_LeftMenu_IdentificationDetails").val(), "Identification", "IdentificationDetails");
    if ($("#ApplicationTypeName").val() == "LEGAL ENTITY") {
        IdentificationCount();
    }
}
function addConfirmButton_IdentificationDetails(e) {
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
        RemoveAllValidationMessage();
    });
    $(".SAVEASDRAFT").click(function (e) {
        model.Status = false;

    });
    $(".k-widget.k-window.k-display-inline-flex input").keyup(function (e) {
        var validatePopup = $(".k-popup-edit-form").kendoValidator().data("kendoValidator");
        validatePopup.validate();

        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div')).children('div.field-validation-error').show();
            }
            else {
                if ($(this).attr('id') == 'IdentificationDetails_IssueDate' || $(this).attr('id') == 'IdentificationDetails_ExpiryDate') {
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
                if ($(this).attr('id') == 'IdentificationDetails_IssueDate' || $(this).attr('id') == 'IdentificationDetails_ExpiryDate') {
                    validateIdentificationDates(this);
                }

                else {
                    $($(this).closest('div')).children('div.field-validation-error').hide();
                }
            }
        }
    });

    if (isApplicationPermisssionEdit == 'False') {
        setAllPopUpInputElementDisabled();
    }
}
$("#accordionIdentificationDetails").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionIdentificationDetails").data("kendoPanelBar");
accordion.collapse("#chartSection");

function error_handleridentificationDetails(args) {
    var errors = args.errors;
    if (errors) {
        var grid = $("#IdentificationDetails").data("kendoGrid");
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
$("#context-menuIdentificationDetails").kendoContextMenu({
    target: "#IdentificationDetails",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#IdentificationDetails").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowIden":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "IdentificationDetails");
                break;
            default:
                break;
        };
    }
});
function validateIdentificationDates(currentElement) {
    $.ajax({
        url: $("#ValidateIdentificationDatesUrl").val(),
        cache: false,
        type: "POST",
        data: { issueDate: $("#IdentificationDetails_IssueDate").val(), expiryDate: $("#IdentificationDetails_ExpiryDate").val() },
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
function IdentificationCount() {
    var applicantionType = $("#Type").val();
    var kendoGrid = $("#IdentificationDetails").data("kendoGrid");
    if ((applicantionType == "LEGAL ENTITY" || applicantionType == "INDIVIDUAL") && kendoGrid.dataSource.total() == 3) {
        $('#IdentificationDetailsNew').hide();
    }
    else {
        $('#IdentificationDetailsNew').show();
    }
}