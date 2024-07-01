function onGroupStructureDataBound(e) {
    $("#GroupStructure .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#GroupStructure .k-grid-content").attr("style", "max-height: 400px");
    deleteEmptyGridRecord('GroupStructure');
    isAllConfirmed($("#hdnApplication_LeftMenu_GroupStructure").val(), "GroupStructure", "GroupStructure");
    manageGroupStructureMenuPanel();
    validateGroupStructure();
}

$(document).ready(function () {
    var entityBelong = $("#DoesTheEntityBelongToAGroupNameID").val();
    if (entityBelong == "true") {
        $("#GroupStructureGridID").css("display", "block");
    }
});
function ShowGroupStructureGrid() {
    var entityBelong = $("#DoesTheEntityBelongToAGroupNameID").val();
    if (entityBelong == "true") {
        $("#GroupStructureGridID").css("display", "block");
    }
    else {
        $("#GroupStructureGridID").css("display", "none");
        $("#GroupStructureLegalParent_GroupName").val('');
        $("#GroupStructureLegalParent_GroupActivities").val('');
    }
}
function addGSConfirmButton(e) {
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
function error_handlerCompanyGroupStructure(args) {
    var errors = args.errors;
    if (errors) {
        var grid = $("#GroupStructure").data("kendoGrid");
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

$("#accordionGroupStructure").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionGroupStructure").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuGroupStructure").kendoContextMenu({
    target: "#GroupStructure",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#GroupStructure").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowGpStruc":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "GroupStructure");
                break;
            default:
                break;
        };
    }
});