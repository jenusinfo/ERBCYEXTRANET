function addConfirmButton_Accounts(e) {
    $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
    $("a.k-grid-update").addClass("SAVEASDRAFT");
    var model = e.model;
    model.Accounts_Status = false;
    model.dirty = true;
    $("a.k-grid-cancel").addClass('btn btn-link');
    $("a.k-grid-update")[0].innerHTML = "SAVE AS DRAFT";
    $("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";

    $('<a class="k-button k-button-icontext k-primary k-grid-update custom" href="#" k-icon k-i-delete"></span>SAVE & CLOSE</a>').insertBefore(".k-grid-update");
    $(".custom").click(function (e) {
        model.Accounts_Status = true;

    });
    $(".SAVEASDRAFT").click(function (e) {
        model.Accounts_Status = false;

    });
    if (isApplicationPermisssionEdit == 'False') {
        setAllPopUpInputElementDisabled();
    }
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
}

function onAccountDataBound(e) {
    $("#AccountsDetails .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#AccountsDetails .k-grid-content").attr("style", "max-height: 400px");
    var grid = $('#AccountsDetails').data('kendoGrid');
    var gridRows = grid.tbody.find("tr");

    gridRows.each(function (e) {
        var rowItem = grid.dataItem($(this));
        console.log(rowItem.Id);
        if (rowItem.AccountsID == 0) {
            grid.removeRow(rowItem);
        }
    });
    isAllConfirmed($("#hdnApplication_LeftMenu_AccountDetails").val(), "Accounts", "AccountsDetails");
    manageAccountMenuPanel();
}
function error_handlerApplicationDetails(args) {
    var errors = args.errors;
    if (errors) {
        var grid = $("#AccountsDetails").data("kendoGrid");
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
$("#accordionAccountsDetails").kendoPanelBar({
    expandMode: "multiple"
});

var accordion = $("#accordionAccountsDetails").data("kendoPanelBar");
accordion.collapse("#chartSection");

$("#context-menu").kendoContextMenu({
    target: "#AccountsDetails",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        //debugger;
        var row = $(e.target).parent()[0];
        var grid = $("#AccountsDetails").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowAccounts":
                var check = IsAccountUseInCard(data);
                var isApplicationPermisssionEdit = $("#ApplicationIsEdit").val();
                if (check == "True" && isApplicationPermisssionEdit=="True") {
                    AccountEditDeleteConfirmation(e);
                    break;
                }
                else {
                    grid.editRow(row);
                    break;
                }
               
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "AccountsDetails");
                break;
            default:
                break;
        };
    }
});
function AccountEditDeleteConfirmation(e) {
    //debugger;
    var dialog = $("#AccountEditDeleteConfirmation").data("kendoWindow");
    dialog.wrapper.addClass("middle-popup");
    dialog.center().open();
    $("#AccountEditYes").unbind().click(function () {
        //debugger;
        UpdateDebitCardRec();
        var dialog = $("#AccountEditDeleteConfirmation").data("kendoWindow");
        dialog.close();
        var row = $(e.target).parent()[0];
        var grid = $("#AccountsDetails").data("kendoGrid");
        grid.editRow(row);
    })
    $("#AccountEditNo").click(function () {
        var dialog = $("#AccountEditDeleteConfirmation").data("kendoWindow");
        dialog.close();
    })
}
function UpdateDebitCardRec() {
    $.ajax({
        url: $("#DebitCardStatusUpdateUrl").val(),
        cache: false,
        type: "POST",
        data: { id: $("#ApplicationId").val(), usedCurrency: $("#UsedCurrency").val() },
        success: function (result) {
            $('#DebitCardDetails').data('kendoGrid').dataSource.read();
            $('#DebitCardDetails').data('kendoGrid').refresh();
        }
    });
}
var isApplicationPermisssionEdit = $("#ApplicationIsEdit").val();
function IsAccountUseInCard(data) {
    //debugger;
    var res = "";

    var accountCurrency = data.Accounts_AccountTypeName + " - " + data.Accounts_CurrencyName;
    $("#UsedCurrency").val(accountCurrency.toUpperCase());
    $.ajax({
        async: false,
        type: "POST",
        url: $("#AccountUsedInCardUrl").val(),
        dataType: "json",
        cache: false,
        data: { id: $("#ApplicationId").val(), applicationType: $("#EntityTypeCode").val(), editCurrency: accountCurrency },
        success: function (result) {
            res= result;
        }
    });
    return res;
}


