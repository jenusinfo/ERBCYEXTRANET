﻿
function onGridDataBoundSourceOfIncomingTransactions(e) {

    $("#SourceOfIncomingTransactions .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#SourceOfIncomingTransactions .k-grid-content").attr("style", "max-height: 400px");
    var grid = $('#SourceOfIncomingTransactions').data('kendoGrid');
    var gridRows = grid.tbody.find("tr");

    gridRows.each(function (e) {
        var rowItem = grid.dataItem($(this));
        if (rowItem.SourceOfIncomingTransactionsID == 0) {
            grid.removeRow(rowItem);
        }
    });

    validatePurposeAndActivity();
}

function addConfirmButton_SIT(e) {
    $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
    $("a.k-grid-update").addClass("SAVEASDRAFT");
    var model = e.model;
    model.SourceOfIncomingTransactions_Status = false;
    model.dirty = true;
    $("a.k-grid-cancel").addClass('btn btn-link');
    $("a.k-grid-update")[0].innerHTML = "SAVE AS DRAFT";
    $("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";

    $('<a class="k-button k-button-icontext k-primary k-grid-update custom" href="#" k-icon k-i-delete"></span>SAVE & CLOSE</a>').insertBefore(".k-grid-update");
    $(".custom").click(function (e) {
        model.SourceOfIncomingTransactions_Status = true;

        $(".k-widget.k-window.k-display-inline-flex input").each(function (i, obj) {
            if (($(this).closest('div')).children('div.field-validation-error').length > 0) {
                var elementName = $(this).attr('name');
                $($(this).closest('div')).children('div.field-validation-error').remove();
                $($(this).closest('div')).append('<span class="field-validation-valid k-hidden" data-valmsg-for="' + elementName + '" data-valmsg-replace="true"></span>');
            }
        });
    });
    $(".SAVEASDRAFT").click(function (e) {
        model.SourceOfIncomingTransactions_Status = false;

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
function error_handlerSourceOfIncomingTransactions(args) {
    var errors = args.errors;
    if (errors) {
        var grid = $("#SourceOfIncomingTransactions").data("kendoGrid");
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
//for Accordian
$("#accordionSourceOfIncomingTransactions").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionSourceOfIncomingTransactions").data("kendoPanelBar");
accordion.collapse("#chartSection");
function onRequestEnd(e) {
    if (e.type === "update" || e.type === "create" || e.type === "destroy") {
        console.log(e.type)
        e.sender.read();
    }
}
$("#context-menuSourceOfIncomingTransactions").kendoContextMenu({
    target: "#SourceOfIncomingTransactions",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#SourceOfIncomingTransactions").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowSOI":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "SourceOfIncomingTransactions");
                break;
            default:
                break;
        };
    }
});
