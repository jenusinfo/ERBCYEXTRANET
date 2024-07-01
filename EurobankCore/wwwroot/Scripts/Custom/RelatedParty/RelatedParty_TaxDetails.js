function onTaxDataBound(e) {
    $("#TaxDetails .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#TaxDetails .k-grid-content").attr("style", "max-height: 400px");
    var grid = $('#TaxDetails').data('kendoGrid');
    var gridRows = grid.tbody.find("tr");
    gridRows.each(function (e) {
        var rowItem = grid.dataItem($(this));
        console.log(rowItem.Id);
        if (rowItem.TaxDetailsID == 0) {
            grid.removeRow(rowItem);
        }
    });
    isAllConfirmed($("#hdnApplication_RelatedParty_LeftMenu_TaxDetails").val(), "Tax", "TaxDetails");
    if ($("#IsUBO") == "True") {
        TaxCount();
    }
}
function addConfirmButton_TaxDetails(e) {
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
                if ($(this).attr('id') == 'TaxDetails_CountryOfTaxResidency' || $(this).attr('id') == 'TaxDetails_TaxIdentificationNumber' || $(this).attr('id') == 'TaxDetails_TinUnavailableReason' || $(this).attr('id') == 'TaxDetails_JustificationForTinUnavalability') {
                    ValidateTaxDetails(this);
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
                if ($(this).attr('id') == 'TaxDetails_CountryOfTaxResidency' || $(this).attr('id') == 'TaxDetails_TaxIdentificationNumber' || $(this).attr('id') == 'TaxDetails_TinUnavailableReason' || $(this).attr('id') == 'TaxDetails_JustificationForTinUnavalability') {
                    ValidateTaxDetails(this);
                }

                else {
                    $($(this).closest('div')).children('div.field-validation-error').hide();
                }
            }
        }
    });
    var selectname = $("#TaxDetails_TinUnavailableReasonName").val();
    if (selectname == "Entity is unable to obtain Tax Number") {
        $("#JustificationForTinUnavalabilityID").css("display", "block");
    }
    if (isApplicationPermisssionEdit == 'False') {
        setAllPopUpInputElementDisabled();
    }

    var selectedCountry = $("#TaxDetails_CountryOfTaxResidency").data("kendoDropDownList").text();
    if (selectedCountry == "-Select-") {
        var dropdownlist = $("#TaxDetails_CountryOfTaxResidency").data("kendoDropDownList");
        dropdownlist.select(function (dataItem) {
            return dataItem.Text === "CYPRUS";
        });
        dropdownlist.trigger("change");
    }
}
function ValidateTaxDetails(currentElement) {
    $.ajax({
        url: $("$ValidateTaxDetailsUrl").val(),
        cache: false,
        type: "POST",
        data: { taxDetails_CountryOfTaxResidency: $("#TaxDetails_CountryOfTaxResidency").val(), taxDetails_TaxIdentificationNumber: $("#TaxDetails_TaxIdentificationNumber").val(), taxDetails_TinUnavailableReason: $("#TaxDetails_TinUnavailableReason").val(), taxDetails_JustificationForTinUnavalability: $("#TaxDetails_JustificationForTinUnavalability").val() },
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
$("#accordionTaxDetails").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionTaxDetails").data("kendoPanelBar");
accordion.collapse("#chartSection");
function error_handlerTaxDetails(args) {
    var errors = args.errors;
    if (errors) {
        var grid = $("#TaxDetails").data("kendoGrid");
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
$("#context-menuTaxDetails").kendoContextMenu({
    target: "#TaxDetails",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#TaxDetails").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowTax":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "TaxDetails");
                break;
            default:
                break;
        };
    }
});
function TaxCount() {
    var applicantionType = $("#Type").val();
    var kendoGrid = $("#TaxDetails").data("kendoGrid");
    if ((applicantionType == "LEGAL ENTITY" || applicantionType == "INDIVIDUAL") && kendoGrid.dataSource.total() == 3) {
        $('#TaxDetailsNew').hide();
    }
    else {
        $('#TaxDetailsNew').show();
    }

}