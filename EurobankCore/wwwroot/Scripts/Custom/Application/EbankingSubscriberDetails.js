function onEBankingDataBound(e) {
    $("#EBankingSubscriber .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#EBankingSubscriber .k-grid-content").attr("style", "max-height: 400px");
    deleteEmptyGridRecord('EBankingSubscriber');
    isAllConfirmed($("#hdnApplication_LeftMenu_EBanking").val(), "EBanking", "EBankingSubscriber");
    manageEbankingSubscriberMenuPanel();
}
function addConfirmButton_Ebanking(e) {
    $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
    $("a.k-grid-update").addClass("SAVEASDRAFT");
    var model = e.model;
    model.EbankingSubscriberDetails_Status = false;
    model.dirty = true;
    $("a.k-grid-cancel").addClass('btn btn-link');
    $("a.k-grid-update")[0].innerHTML = "SAVE AS DRAFT";
    $("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";

    $('<a class="k-button k-button-icontext k-primary k-grid-update custom" href="#" k-icon k-i-delete"></span>SAVE & CLOSE</a>').insertBefore(".k-grid-update");
    $(".custom").click(function (e) {
        model.EbankingSubscriberDetails_Status = true;

        $(".k-widget.k-window.k-display-inline-flex input").each(function (i, obj) {
            if (($(this).closest('div')).children('div.field-validation-error').length > 0) {
                var elementName = $(this).attr('name');
                $($(this).closest('div')).children('div.field-validation-error').remove();
                $($(this).closest('div')).append('<span class="field-validation-valid k-hidden" data-valmsg-for="' + elementName + '" data-valmsg-replace="true"></span>');
            }
        });

    });
    $(".SAVEASDRAFT").click(function (e) {
        model.EbankingSubscriberDetails_Status = false;

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
    var AddFuture = $('#AutomaticallyAddFuturePersonalAccounts').data("kendoDropDownList");
    if (AddFuture != undefined) {
        AddFuture.value("true");

        AddFuture.trigger("change");
        AddFuture.readonly();
    }
    if ($("#ApplicationDetails_ApplicationTypeName").val() == "LEGAL ENTITY") {
        $("#AutomaticallyAddFuturePersonalAccountsID").css("display", "block");
        ShowSignatoryGroup();
        FillSigGroupsLegal(model.Subscriber, model.SignatoryGroupName);
    }
    else {
        $("#AutomaticallyAddFuturePersonalAccountsID").css("display", "block");
    }

    if (isApplicationPermisssionEdit == 'False') {
        setAllPopUpInputElementDisabled();
    }
    if ($("#ApplicationDetails_ApplicationTypeName").val() == "INDIVIDUAL" || $("#ApplicationDetails_ApplicationTypeName").val()=="JOINT INDIVIDUAL") {
        GetPersonType();
        var ddlLimitAmount = $('#LimitAmount').data("kendoDropDownList");
        ddlLimitAmount.readonly();

    }
    if ($("#ApplicantEntityType").val() == "Trust") {
        //$('#spanAccessLevel').attr('title','In case of more than one trustees, then all trustees must have the same Access Level');
        //$('#spanAccessLevel').addClass('fa fa-info-circle');
        document.getElementById('spanAccessLevel').innerHTML = '  (In case of more than one trustees, then all trustees must have the same Access Level)';
    }

}

function ShowSignatoryGroup() {
    //debugger;
    var selectValue = $("#AccessLevel").data("kendoDropDownList").text();
    if (selectValue == "FULL" || selectValue == "AUTHORISE") {
        $("#SignatoryGroupDIV").css("display", "block");
        var dropdown = $("#SignatoryGroupName").data("kendoDropDownList");
        dropdown.dataSource.read();
        dropdown.refresh();
    }
    else {
        var ddlSignatoryGroupName = $('#SignatoryGroupName').data("kendoDropDownList");
        $("#SignatoryGroupDIV").css("display", "none");
        if (ddlSignatoryGroupName != undefined) {
            ddlSignatoryGroupName.value('');
        }
    }
}

function onAllPersonalAccountDataBound() {
    var ddlLimitAmount = $('#AccessToAllPersonalAccounts').data("kendoDropDownList");
    if (ddlLimitAmount != undefined) {
        ddlLimitAmount.value("true");

        ddlLimitAmount.trigger("change");
        ddlLimitAmount.readonly();
    }

}

function onLimitAmountDataBound() {
    var ddlLimitAmount = $('#LimitAmount').data("kendoDropDownList");
    if (ddlLimitAmount != undefined) {
        ddlLimitAmount.text("MAXIMUM ALLOWABLE LIMIT PER TRANSACTION TYPE");
        ddlLimitAmount.trigger("change");
        ddlLimitAmount.readonly();
    }
}

function onChangeEbankingSubscribersLegal() {
    FillSubscriberIdentityLegal();
    var ddlSignatoryName = $('#Subscriber').data("kendoDropDownList");
    if (ddlSignatoryName != undefined) {
        FillSigGroupsLegal(ddlSignatoryName.value());
    }

}
function onSubscriberSgGroupDataBound(e) {
    //alert($('#hdnSubscriber').val());
    //FillSigGroupsLegal(dataItem.Value);

}

function onSubscriberSgGroupSelect(e) {
    //alert($('#hdnSubscriber').val());
    //FillSigGroupsLegal(dataItem.Value);

}
function FillSubscriberIdentityLegal() {
    var ddlCardHolder = $('#Subscriber').data("kendoDropDownList");
    if (ddlCardHolder != undefined) {
        var relatedPartyGuid = ddlCardHolder.value();
        $.ajax({
            url: $("#GetSubscriberIdentityUrl").val(),
            cache: false,
            type: "POST",
            data: { relatedPartyGuid: relatedPartyGuid },
            success: function (result) {

                var ddlHolderIdentityCountryOfIssue = $('#CountryOfIssue').data("kendoDropDownList");
                if (ddlHolderIdentityCountryOfIssue != undefined) {
                    if (result != null && result != null) {
                        ddlHolderIdentityCountryOfIssue.value(result.CountryOfIssue);
                        ddlHolderIdentityCountryOfIssue.trigger("change");
                        ddlHolderIdentityCountryOfIssue.focus();
                    }
                    else {
                        ddlHolderIdentityCountryOfIssue.value('');
                        ddlHolderIdentityCountryOfIssue.trigger("change");
                        ddlHolderIdentityCountryOfIssue.focus();
                    }
                }
                if ($('#IdentityPassportNumber').length > 0) {
                    if (result != null && result != null) {
                        $('#IdentityPassportNumber').val(result.IdentityNumber).change();
                        $('#IdentityPassportNumber').focus();
                        $('#IdentityPassportNumber').focusout();
                    }
                    else {
                        $('#IdentityPassportNumber').val('').change();
                        $('#IdentityPassportNumber').focus();
                        $('#IdentityPassportNumber').focusout();
                    }

                }

            }
        });
    }
}

function FillSigGroupsLegal(signatoryNameValue, selectedItemValue) {
    $.ajax({
        url: $("#GetSignatoryGroupForEBankingBySeletedPersonUrl").val(),
        cache: false,
        type: "POST",
        data: { applicationId: $("#ApplicationId").val(), applicationNumber: $("#ApplicationNumber").val(), selectedPerson: signatoryNameValue },
        success: function (result) {
            var ddlSignatoryPersonsList = $('#SignatoryGroupName').data("kendoDropDownList");
            ddlSignatoryPersonsList.setDataSource(result);
            ddlSignatoryPersonsList.dataSource.read();
            if (selectedItemValue != undefined && selectedItemValue != '' && selectedItemValue != null) {
                ddlSignatoryPersonsList.value(selectedItemValue);
            }
        }
    });
}


function error_handlerEBankingSubscriber(args) {
    var errors = args.errors;
    if (errors) {
        var grid = $("#EBankingSubscriber").data("kendoGrid");
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
$("#accordionEBankingSubscriber").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionEBankingSubscriber").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuEBankingSubscriber").kendoContextMenu({
    target: "#EBankingSubscriber",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#EBankingSubscriber").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowEbank":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "EBankingSubscriber");
                break;
            default:
                break;
        };
    }
});
