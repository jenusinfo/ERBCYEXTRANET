var _selectedCardType;
function onDebitCardDataBound(e) {
    $("#DebitCardDetails .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#DebitCardDetails .k-grid-content").attr("style", "max-height: 400px");
    var grid = $('#DebitCardDetails').data('kendoGrid');
    var gridRows = grid.tbody.find("tr");
    gridRows.each(function (e) {
        var rowItem = grid.dataItem($(this));
        console.log(rowItem.Id);
        if (rowItem.DebitCardDetailsID == 0) {
            grid.removeRow(rowItem);
        }
    });
    isAllConfirmed($("#hdnApplication_LeftMenu_DebitCard").val(), "Cards", "DebitCardDetails");
    manageDebitCardMenuPanel();
}
function addConfirmButton_Debitcard(e) {
    $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
    $("a.k-grid-update").addClass("SAVEASDRAFT");
    var model = e.model;
    model.DebitCardDetails_Status = false;
    model.dirty = true;
    $("a.k-grid-cancel").addClass('btn btn-link');
    $("a.k-grid-update")[0].innerHTML = "SAVE AS DRAFT";
    $("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";

    $('<a class="k-button k-button-icontext k-primary k-grid-update custom" href="#" k-icon k-i-delete"></span>SAVE & CLOSE</a>').insertBefore(".k-grid-update");
    $(".custom").click(function (e) {
        model.DebitCardDetails_Status = true;

        $(".k-widget.k-window.k-display-inline-flex input").each(function (i, obj) {
            if (($(this).closest('div')).children('div.field-validation-error').length > 0) {
                var elementName = $(this).attr('name');
                $($(this).closest('div')).children('div.field-validation-error').remove();
                $($(this).closest('div')).append('<span class="field-validation-valid k-hidden" data-valmsg-for="' + elementName + '" data-valmsg-replace="true"></span>');
            }
        });
    });
    $(".SAVEASDRAFT").click(function (e) {
        model.DebitCardDetails_Status = false;

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
    //var selectValue = $("#DebitCardDetails_DispatchMethodName").val();
    ////Individual
    //if (selectValue.toUpperCase() == "TO BE COLLECTED FROM THE BANKING CENTRE BY (FULL NAME AND IDENTITY CARD/PASSPORT NO.)") {
    //    $("#CollectedByID").css("display", "block");
    //    $("#IdentityNumberID").css("display", "block");
    //}
    //else if (selectValue.toUpperCase() == "TO BE DISPATCHED BY COURIER SERVICE TO THE FOLLOWING ADDRESS (APPLICABLE ONLY FOR OVERSEAS ADDRESSES)") {
    //    $("#DeliveryAddressID").css("display", "block");
    //}
    ////Legal
    //else if (selectValue.toUpperCase() == "TO BE COLLECTED FROM THE BANKING CENTRE BY (FULL NAME AND IDENTITY CARD/PASSPORT NO.)") {
    //    $("#CollectedByID").css("display", "block");
    //    $("#IdentityNumberID").css("display", "block");
    //}
    //else if (selectValue.toUpperCase() == "TO BE DISPATCHED BY COURIER SERVICE TO THE COMPANY’S MAILING ADDRESS (APPLICABLE FOR OVERSEAS ADDRESSES)") {
    //    $("#DeliveryAddressID").css("display", "block");
    //}
    //else if (selectValue.toUpperCase() == "TO BE DISPATCHED BY COURIER SERVICE TO THE FOLLOWING ADDRESS (APPLICABLE FOR OVERSEAS ADDRESSES)") {
    //    $("#DeliveryAddressID").css("display", "block");
    //}

    ShowHideOtherDeliveryAddressBind(model.DebitCardDetails_DeliveryAddress);
    if (isApplicationPermisssionEdit == 'False') {
        setAllPopUpInputElementDisabled();
    }

    GetCardHolderAddresss(model.DebitCardDetails_DeliveryAddress);
    _selectedCardType = model.DebitCardDetails_CardType;
    FillIdentification();
}
function error_handlerDebitCardDetails(args) {
    var errors = args.errors;
    if (errors) {
        var grid = $("#DebitCardDetails").data("kendoGrid");
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
$("#accordionDebitCardDetails").kendoPanelBar({
    expandMode: "multiple"
});

function GetCardHolderAddresss(selectedItemValue) {
    var ddlCardHolder = $('#DebitCardDetails_CardholderName').data("kendoDropDownList");

    if (ddlCardHolder != undefined) {
        var relatedPartyGuid = ddlCardHolder.value();
        $.ajax({
            url: $("#CardHoderAddresses_ReadUrl").val(),
            cache: false,
            type: "POST",
            data: { relatedPartyGuid: relatedPartyGuid },
            success: function (result) {
                var ddlCardHolderddress = $('#DebitCardDetails_DeliveryAddress').data("kendoDropDownList");
                ddlCardHolderddress.setDataSource(result);
                ddlCardHolderddress.dataSource.read();
                if (selectedItemValue != undefined && selectedItemValue != '' && selectedItemValue != null) {
                    ddlCardHolderddress.value(selectedItemValue);
                }
            }
        });
    }
}

function onCardTypeDataBound() {
    SelectFistElement(_selectedCardType);
}

function IndividualDispatchMethodonDatabound() {
    ddlCardDispatchIndividual = $("#DebitCardDetails_DispatchMethod").data("kendoDropDownList");
    ddlCardDispatchIndividual.text("TO BE DISPATCHED BY POST TO THE CARDHOLDER’S MAILING ADDRESS (APPLICABLE FOR DOMESTIC AND OVERSEAS ADDRESSES).");
    ddlCardDispatchIndividual.trigger("change");
    ddlCardDispatchIndividual.readonly();
}
function LegalDispatchMethodOnDatabound() {
    ddlCardDispatchIndividual = $("#DebitCardDetails_DispatchMethod").data("kendoDropDownList");
    ddlCardDispatchIndividual.text("TO BE DISPATCHED BY POST TO THE COMPANY’S MAILING ADDRESS (APPLICABLE FOR DOMESTIC AND OVERSEAS ADDRESSES).");
    ddlCardDispatchIndividual.trigger("change");
    ddlCardDispatchIndividual.readonly();
}

function SelectFistElement(cardTypeValue) {
    ddlCardType = $("#DebitCardDetails_CardType").data("kendoDropDownList");
    if ((cardTypeValue == undefined || cardTypeValue == null || cardTypeValue == '') && ddlCardType.dataSource._total == 1) {
        ddlCardType.select(1);
        ddlCardType.trigger("change");
    }
}
function ShowHideOtherDeliveryAddressBind(modelDeliveryAddress) {

    var deliveryAddressSelectValue = $("#DebitCardDetails_DeliveryAddress").data("kendoDropDownList").text();
    if ((deliveryAddressSelectValue != undefined && deliveryAddressSelectValue == 'Other Address') || (modelDeliveryAddress != undefined && modelDeliveryAddress == 'Other Address')) {
        $('#otherDeliveryAddress').show();
    }
    else {
        $('#otherDeliveryAddress').hide();
    }
}
var accordion = $("#accordionDebitCardDetails").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuDebitCardDetails").kendoContextMenu({
    target: "#DebitCardDetails",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        debugger;
        var row = $(e.target).parent()[0];
        var grid = $("#DebitCardDetails").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowCards":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "DebitCardDetails");
                break;
            default:
                break;
        };
    }
});
