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




function onApplicantDataBound(e) {
    $("#Applicants .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#Applicants .k-grid-content").attr("style", "max-height: 400px");
    isAllConfirmed($("#hdnApplication_LeftMenu_Applicants").val(), "Applicants", "Applicants",);
    CheckApplicantCount();
    manageApplicantMenuPanel();
    BindReasonForopeningAcc();
}
function CheckApplicantCount() {
    var applicantionType = $("#ApplicationTypeName").val();
    var kendoGrid = $("#Applicants").data("kendoGrid");
    if ((applicantionType == "LEGAL ENTITY" || applicantionType == "INDIVIDUAL") && kendoGrid.dataSource.total() > 0) {
        $('#divCreateNewApplicant').hide();
        $('#lnkCreateNewApplicant').hide();

    }
    else {
        $('#divCreateNewApplicant').show();
        $('#lnkCreateNewApplicant').show();
    }
}

function BindReasonForopeningAcc() {
    var multiReasonForOpening = $("#ReasonForOpeningTheAccountGroup").data("kendoMultiSelect");
    if (multiReasonForOpening != undefined) {
        multiReasonForOpening.dataSource.read();
    }
}
$("#accordionApplicants").kendoPanelBar({
    expandMode: "multiple"
});


var accordion = $("#accordionApplicants").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuApplicants").kendoContextMenu({
    target: "#Applicants",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#Applicants").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                break;
            case "editRowApplicants":
                var url = $("#RedirectToApplicant").val();
                window.location.href = url + "?application=" + $("#ApplicationNodeGUID").val() + "&&applicant=" + data.NodeGUID;
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "Applicants");
                break;
            default:
                break;
        };
    }
});

$("#accordionApplicationDetails").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionApplicationDetails").data("kendoPanelBar");
accordion.collapse("#chartSection");
$(document).ready(function () {
    $(".checked-readonly input").prop('disabled', true);
    $(".readonly input").prop('disabled', true);
    $(".readonly input").prop('checked', false);
});
$(document).ready(function () {
    $('.toast').toast('show');
});
var isApplicationPermisssionEdit = $("#ApplicationIsEdit").val();
$("#accordionPurposeAndActivity").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionPurposeAndActivity").data("kendoPanelBar");
accordion.collapse("#chartSection");

$(document).ready(function () {
    if (isApplicationPermisssionEdit == "False") {
        setAllInputElementDisabled('app-body-per');
    }
    validatePurposeAndActivity();
    validateGroupStructure();
    showHideCardEbankingSection();
    if ($("#IsDecissionCommentsAllowed").val() == "True") {
        $("#DecisionHistory_DecisionID").data("kendoDropDownList").enable(true);
        $("#DecisionHistory_CommentsID").prop("disabled", false);
        $('#DecisionHistory_CommentsID').removeAttr("disabled")
    }

    // Format the value for all textboxes with the class 'thousands-separator' when the document is ready
    $('.thousands-separator').each(function () {
        var value = $(this).val();
        if (value) {
            $(this).val(formatNumber(parseFloat(value).toFixed(2)));
        }
    });
    // Format the value when any textbox with the class 'thousands-separator' is focused
    $('.thousands-separator').focus(function () {
        var value = $(this).val().replace(/,/g, ''); // Remove commas
        $(this).val(value);
    });

    // Format the value when the user is done editing (blur event) for any textbox with the class 'thousands-separator'
    $('.thousands-separator').blur(function () {
        if ($(this).val() != '') {        
            var value = parseFloat($(this).val()).toFixed(2); // Convert to float with 2 decimal places
            $(this).val(formatNumber(value));
        }
    });

    if ($('#ApplicationTypeName').val() == 'INDIVIDUAL') {
        $('#SignatureMandateTypeGroup > li:nth-child(1) > label:nth-child(2)').text($('#hdnIndividualSignatureMandateLabel').val())
    }
    

});
function formatNumber(num) {
    return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function limitlengththousandsseparator(obj, length, spanid) {
    //debugger;
    var maxlength = length;
    var texboxValue = Math.trunc(obj.value.replace(/,/g, ''));
    if (texboxValue.toString().length > maxlength) {
        document.getElementById(spanid).innerHTML = 'Maximum allowed length of the input text is ' + length;
        //obj.value = obj.value.substring(0, maxlength)
        //obj.value = formatNumber(texboxValue.toString().substring(0, maxlength));
        obj.value = texboxValue.toString().substring(0, maxlength);
    }
    else {
        document.getElementById(spanid).innerHTML = ' ';
    }
}
function isAllConfirmed(menuTitle, moduleName, gridName) {
    var searchName = "Pending";
    var isNotCompleted = false;
    var kendoGrid = $("#" + gridName).data("kendoGrid");
    var searchNameFound = kendoGrid.dataSource.data().some(
        function (dataItem) {
            if (moduleName == 'Applicants' || moduleName == 'RelatedParties') {
                if (dataItem.Status == searchName) {
                    isNotCompleted = true;
                }
            }
            else if (moduleName == 'Accounts') {
                if (dataItem.Account_Status_Name == searchName) {
                    isNotCompleted = true;
                }
            }
            else if (moduleName == 'Cards') {
                if (dataItem.DebitCardDetails_StatusName == searchName) {
                    isNotCompleted = true;
                }
            }
            else if (moduleName == 'EBanking' || moduleName == 'Notes' || moduleName == 'SignatureMandate') {
                if (dataItem.Status_Name == searchName) {
                    isNotCompleted = true;
                }
            }
            else if (moduleName == 'GroupStructure' || moduleName == 'SignatureMandateLegal') {
                if (dataItem.StatusName == searchName) {
                    isNotCompleted = true;
                }
            }

            if (kendoGrid.dataSource.indexOf(dataItem) == (kendoGrid.dataSource.total() - 1)) {
                var steps = $("#ApplicationStepper").data('kendoStepper')._steps;

                if (!isNotCompleted) {
                    $("#ApplicationStepper a[title='" + menuTitle + "' i]").parent().removeClass('k-step-error').addClass("k-step-done").addClass('k-step-success');

                    var arr = jQuery.grep(steps, function (n, i) {
                        if (n.options.label == menuTitle) {
                            $("#ApplicationStepper").data('kendoStepper')._steps[i].options.error = false;
                            $("#ApplicationStepper").data('kendoStepper')._steps[i].options.icon = "checkmark-circle";
                        }
                    });

                    $("#ApplicationStepper a[title='" + menuTitle + "' i]").find("span .k-step-indicator-icon.k-icon").removeClass('k-i-video-external').addClass('k-i-checkmark-circle');
                    $("#ApplicationStepper a[title='" + menuTitle + "' i]").removeAttr("aria-invalid");
                    $("#ApplicationStepper a[title='" + menuTitle + "' i]").find("span .k-icon.k-i-warning").remove();
                }
                else {
                    $("#ApplicationStepper a[title='" + menuTitle + "' i]").parent().removeClass("k-step-done").removeClass('k-step-success').addClass('k-step-error');
                    $("#ApplicationStepper a[title='" + menuTitle + "' i]").find("span .k-step-indicator-icon.k-icon").removeClass('k-i-checkmark-circle').addClass('k-i-video-external');
                    $("#ApplicationStepper a[title='" + menuTitle + "' i]").attr("aria-invalid", "true");

                    var arr = jQuery.grep(steps, function (n, i) {
                        if (n.options.label == menuTitle) {
                            $("#ApplicationStepper").data('kendoStepper')._steps[i].options.error = true;
                            $("#ApplicationStepper").data('kendoStepper')._steps[i].options.icon = "video-external";
                        }
                    });
                }
            }
        });
}

function setMenuSuccess(menuTitle) {
    if (menuTitle.indexOf('&amp;') > 0) {
        menuTitle = menuTitle.replace('&amp;', '&');
    }
    $("#ApplicationStepper a[title='" + menuTitle + "' i]").parent().removeClass('k-step-error').addClass("k-step-done").addClass('k-step-success');
    var steps = $("#ApplicationStepper").data('kendoStepper')._steps;

    var arr = jQuery.grep(steps, function (n, i) {
        if (n.options.label == menuTitle) {
            $("#ApplicationStepper").data('kendoStepper')._steps[i].options.error = false;
            $("#ApplicationStepper").data('kendoStepper')._steps[i].options.icon = "checkmark-circle";
        }
    });
    $("#ApplicationStepper a[title='" + menuTitle + "' i]").find("span .k-step-indicator-icon.k-icon").removeClass('k-i-video-external').addClass('k-i-checkmark-circle');
    $("#ApplicationStepper a[title='" + menuTitle + "' i]").removeAttr("aria-invalid");
    $("#ApplicationStepper a[title='" + menuTitle + "' i]").find("span .k-icon.k-i-warning").remove();
}
function setMenuError(menuTitle) {
    if (menuTitle.indexOf('&amp;') > 0) {
        menuTitle = menuTitle.replace('&amp;', '&');
    }
    $("#ApplicationStepper a[title='" + menuTitle + "' i]").parent().removeClass("k-step-done").removeClass('k-step-success').addClass('k-step-error');
    $("#ApplicationStepper a[title='" + menuTitle + "' i]").find("span .k-step-indicator-icon.k-icon").removeClass('k-i-checkmark-circle').addClass('k-i-video-external');
    $("#ApplicationStepper a[title='" + menuTitle + "' i]").attr("aria-invalid", "true");
    var steps = $("#ApplicationStepper").data('kendoStepper')._steps;

    var arr = jQuery.grep(steps, function (n, i) {
        if (n.options.label == menuTitle) {
            $("#ApplicationStepper").data('kendoStepper')._steps[i].options.error = true;
            $("#ApplicationStepper").data('kendoStepper')._steps[i].options.icon = "video-external";
        }
    });
}
function validatePurposeAndActivity() {
    $.ajax({
        url: $("#ValidatePurposeAndActivityUrl").val(),
        cache: false,
        type: "POST",
        data: { reasonsForOpeningValues: $('#ReasonForOpeningTheAccountGroup').data("kendoMultiSelect").value(), expectedNatureOfTranValues: $('#ExpectedNatureOfInAndOutTransactionGroup').data("kendoMultiSelect").value(), expectedFrequencyOfTranValues: $('#ExpectedFrequencyOfInAndOutTransactionGroup').data("kendoRadioGroup").value(), expectedIncome: $('#PurposeAndActivity_ExpectedIncomingAmount').val(), applicationId: $('#hdnapplicationId').val(), applicationNumber: $('#ApplicationNumber').val() },
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#Application_LeftMenu_PurposeAndActivity").val());
            }
            else {
                setMenuError($("#Application_LeftMenu_PurposeAndActivity").val());
            }
        }
    });
}
function validateGroupStructure() {
    $.ajax({
        url: $("#ValidateGroupStructureUrl").val(),
        cache: false,
        type: "POST",
        data: { applicantionId: $('#ApplicationId').val(), applicationNumber: $('#ApplicationNumber').val(), doesTheEntityBelongToAGroupName: $('#DoesTheEntityBelongToAGroupNameID').val(), groupActivities: $('#GroupStructureLegalParent_GroupActivities').val(), groupName: $('#GroupStructureLegalParent_GroupName').val() },
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#hdnApplication_LeftMenu_GroupStructure").val());
            }
            else {
                setMenuError($("#hdnApplication_LeftMenu_GroupStructure").val());
            }
        }
    });
}
function manageApplicantMenuPanel() {

    var kendoApplicantGrid = $("#Applicants").data("kendoGrid");

    if (kendoApplicantGrid.dataSource.total() > 0) {
        var innerHtml = '';
        var datasourcedata = kendoApplicantGrid.dataSource.data();
        for (var i = 0; i < datasourcedata.length; i++) {
            var fullApplicantName = datasourcedata[i].FullName;
            var url = $("#RedirectToApplicant").val() + "?application=" + $("#ApplicationNodeGUID").val() + "&&applicant=" + datasourcedata[i].NodeGUID;
            //console.log("url for appliction -- " + url);
            innerHtml = innerHtml + '<a class="appl-list" href="javascript:void(0);" onclick="showHideLoader();window.location.href=\'' + url +'\'"><span class="row viewall appl-item" style="margin-left: 20%;color: #27438c;">' + fullApplicantName.toUpperCase() + '</span></a>';
        }
        if ($('#ApplicationStepper li a[title="Applicants"]').parent().length > 0) {
            if ($('#appendedAppplicants').length > 0) {
                $('#appendedAppplicants').remove();
            }
            $('#ApplicationStepper li a[title="Applicants"]').parent().append('<div id="appendedAppplicants">' + innerHtml + '</div>');
        }
    }
    else if ($('#appendedAppplicants').length > 0) {
        $('#appendedAppplicants').remove();
    }
}

function manageRelatedPartyMenuPanel() {
    var kendoApplicantGrid = $("#RelatedParties").data("kendoGrid");
    if (kendoApplicantGrid.dataSource.total() > 0) {
        var innerHtml = '';
        var datasourcedata = kendoApplicantGrid.dataSource.data();
        for (var i = 0; i < datasourcedata.length; i++) {
            var fullApplicantName = datasourcedata[i].FullName;
            var url = $("#RedirectToRelatedParty").val() + "?application=" + $("#ApplicationNodeGUID").val() + "&&relatedParty=" + datasourcedata[i].NodeGUID + "&&type=" + datasourcedata[i].Type;
            innerHtml = innerHtml + '<a class="appl-list" href="javascript:void(0);" onclick="showHideLoader();window.location.href=\'' + url + '\'"><span class="row viewall appl-item" style="margin-left: 20%;color: #27438c;">' + fullApplicantName.toUpperCase() + '</span></a>';
        }
        if ($('#ApplicationStepper li a[title="Related Parties"]').parent().length > 0) {
            if ($('#appendedRelatedParties').length > 0) {
                $('#appendedRelatedParties').remove();
            }
            $('#ApplicationStepper li a[title="Related Parties"]').parent().append('<div id="appendedRelatedParties">' + innerHtml + '</div>');
        }
    }
    else if ($('#appendedRelatedParties').length > 0) {
        $('#appendedRelatedParties').remove();
    }
}

function manageAccountMenuPanel() {
    var kendoApplicantGrid = $("#AccountsDetails").data("kendoGrid");
    if (kendoApplicantGrid.dataSource.total() > 0) {
        var innerHtml = '';
        var datasourcedata = kendoApplicantGrid.dataSource.data();
        for (var i = 0; i < datasourcedata.length; i++) {
            var fullAccountName = datasourcedata[i].Accounts_AccountTypeName + ' - ' + datasourcedata[i].Accounts_CurrencyName;
            innerHtml = innerHtml + '<span class="row" style="margin-left: 20%;color: #27438c;">' + fullAccountName.toUpperCase() + '</span>';
        }
        if ($('#ApplicationStepper li a[title="Accounts"]').parent().length > 0) {
            if ($('#appendedAccountDetails').length > 0) {
                $('#appendedAccountDetails').remove();
            }
            $('#ApplicationStepper li a[title="Accounts"]').parent().append('<div id="appendedAccountDetails">' + innerHtml + '</div>');
        }
    }
    else if ($('#appendedAccountDetails').length > 0) {
        $('#appendedAccountDetails').remove();
    }
}
function manageEbankingSubscriberMenuPanel() {
    var kendoApplicantGrid = $("#EBankingSubscriber").data("kendoGrid");
    if (kendoApplicantGrid.dataSource.total() > 0) {
        var innerHtml = '';
        var datasourcedata = kendoApplicantGrid.dataSource.data();
        for (var i = 0; i < datasourcedata.length; i++) {
            var fullEbankingSubscriberName = datasourcedata[i].SubscriberName + ' - ' + datasourcedata[i].AccessLevelName;
            innerHtml = innerHtml + '<span class="row" style="margin-left: 20%;color: #27438c;">' + fullEbankingSubscriberName.toUpperCase() + '</span>';
        }
        if ($('#ApplicationStepper li a[title="Digital Banking"]').parent().length > 0) {
            if ($('#appendedfullEbankingSubscriberNameDetails').length > 0) {
                $('#appendedfullEbankingSubscriberNameDetails').remove();
            }
            $('#ApplicationStepper li a[title="Digital Banking"]').parent().append('<div id="appendedfullEbankingSubscriberNameDetails">' + innerHtml + '</div>');
        }
    }
    else if ($('#appendedfullEbankingSubscriberNameDetails').length > 0) {
        $('#appendedfullEbankingSubscriberNameDetails').remove();
    }
}
function manageDebitCardMenuPanel() {
    var kendoApplicantGrid = $("#DebitCardDetails").data("kendoGrid");
    if (kendoApplicantGrid.dataSource.total() > 0) {
        var innerHtml = '';
        var datasourcedata = kendoApplicantGrid.dataSource.data();
        for (var i = 0; i < datasourcedata.length; i++) {
            var fullDebitCardName = datasourcedata[i].DebitCardDetails_FullName + ' - ' + datasourcedata[i].DebitCardDetails_CardTypeName;
            innerHtml = innerHtml + '<span class="row" style="margin-left: 20%;color: #27438c;">' + fullDebitCardName.toUpperCase() + '</span>';
        }
        if ($('#ApplicationStepper li a[title="Debit Cards"]').parent().length > 0) {
            if ($('#appendedfullDebitCardDetails').length > 0) {
                $('#appendedfullDebitCardDetails').remove();
            }
            $('#ApplicationStepper li a[title="Debit Cards"]').parent().append('<div id="appendedfullDebitCardDetails">' + innerHtml + '</div>');
        }
    }
    else if ($('#appendedfullDebitCardDetails').length > 0) {
        $('#appendedfullDebitCardDetails').remove();
    }
}
function manageGroupStructureMenuPanel() {
    //debugger;
    var kendoGroupStructureGrid = $("#GroupStructure").data("kendoGrid");

    if (kendoGroupStructureGrid.dataSource.total() > 0) {
        var innerHtml = '';
        var datasourcedata = kendoGroupStructureGrid.dataSource.data();
        for (var i = 0; i < datasourcedata.length; i++) {
            var GroupStructureName = datasourcedata[i].EntityName;
            //var t = "#GroupStructure tbody>tr:eq(" + (i+1) + ")";
            //var url = $("#GroupStructure").data("kendoGrid").editRow($("#GroupStructure tbody>tr:eq(" + i + 1 + ")"));

            //innerHtml = innerHtml + '<a class="appl-list" href="javascript:void(0);" onclick="' + $("#GroupStructure").data("kendoGrid").editRow($("#GroupStructure tbody>tr:eq(" + i + ")")) +'"><span class="row viewall appl-item" style="margin-left: 20%;color: #27438c;">' + GroupStructureName.toUpperCase() + '</span></a>';
            innerHtml = innerHtml + '<span class="row" style="margin-left: 20%;color: #27438c;">' + GroupStructureName.toUpperCase() + '</span>';

        }

        if ($('#ApplicationStepper li a[title="Group Structure"]').parent().length > 0) {
            if ($('#appendedGroupStructure').length > 0) {
                $('#appendedGroupStructure').remove();
            }
            $('#ApplicationStepper li a[title="Group Structure"]').parent().append('<div id="appendedGroupStructure">' + innerHtml + '</div>');
        }

    }
    else if ($('#appendedGroupStructure').length > 0) {
        $('#appendedGroupStructure').remove();
    }
}
function RemoveAllValidationMessage() {
    $(".k-widget.k-window.k-display-inline-flex input").each(function (i, obj) {
        if (($(this).closest('div')).children('div.field-validation-error').length > 0) {
            var elementName = $(this).attr('name');
            $($(this).closest('div')).children('div.field-validation-error').remove();
            $($(this).closest('div')).append('<span class="field-validation-valid k-hidden" data-valmsg-for="' + elementName + '" data-valmsg-replace="true"></span>');
        }
    });
    $(".k-widget.k-window.k-display-inline-flex select").each(function (i, obj) {
        if ($($(this).closest('div')).next('div.field-validation-error').length > 0) {
            var elementName = $(this).attr('name');
            $($(this).closest('div')).next('div.field-validation-error').remove();
            $(this).closest('div').parent().closest('div').append('<span class="field-validation-valid k-hidden" data-valmsg-for="' + elementName + '" data-valmsg-replace="true"></span>');
        }
    });
}
function setTwoNumberDecimal(event) {
    this.value = parseFloat(this.value).toFixed(2);
}
$(document).on("input", ".custom-numeric", function () {
    this.value = this.value.replace(/[^\d-]/g, '');
});

$(document).on("input", ".custom-no-negetive-numeric", function () {
    this.value = this.value.replace(/\D/g, '');
});

$(document).on("input", ".custom-decimal", function () {
    this.value = this.value.replace(/[^\d.-]/g, '');
});

$(document).on("input", ".custom-negetive-decimal", function () {
    this.value = this.value.replace(/[^\d.-]/g, '');
});
function allowAlphaNumericSpaceN(e) {
    var code = ('charCode' in e) ? e.charCode : e.keyCode;
    if (!(code == 32) &&
        !(code > 47 && code < 58) &&
        !(code > 64 && code < 91) &&
        !(code > 96 && code < 123)) {
        e.preventDefault();
    }
}

function ChangeMaxLengthLegalCardPhone() {
    var selectedPhoneNo = $("#Country_Code").data("kendoDropDownList") != undefined ? $("#Country_Code").data("kendoDropDownList").text() : '';
    field = document.getElementById('DebitCardDetails_MobileNumber');

    if (selectedPhoneNo == 'CYPRUS +357') {
        field.value = field.value.substr(0, 8);
    }
    $("#DebitCardDetails_MobileNumber").keyup();
}
function limitlengthAccToCountry(obj, spanid, ddlid) {
    var selectedCountryNo = $('#' + ddlid).data("kendoDropDownList") != undefined ? $('#' + ddlid).data("kendoDropDownList").text() : '';
    if (selectedCountryNo == 'CYPRUS +357') {
        var maxlength = 8;
        var errormsg = 'In case of Cyprus Maximum allowed length is ' + maxlength;
    }
    else {
        var maxlength = 10;
        var errormsg = 'Maximum allowed length of the input text is ' + maxlength;
    }
    if (obj.value.length > maxlength) {
        document.getElementById(spanid).innerHTML = errormsg;
        obj.value = obj.value.substring(0, maxlength)
    }
    else {
        document.getElementById(spanid).innerHTML = ' ';
    }
}
function ShowRespectedFieldIndividualCard() {
    $("#DebitCardDetails_DeliveryAddress").data("kendoDropDownList").value('');
    var selectValue = $("#DebitCardDetails_DispatchMethod").data("kendoDropDownList").text();
    if (selectValue == "To be collected from the banking centre by me personally") {


        $("#CollectedByID").css("display", "none");
        $("#IdentityNumberID").css("display", "none");
        $("#DeliveryAddressID").css("display", "none");
    }
    else if (selectValue == "TO BE COLLECTED FROM THE BANKING CENTRE BY (FULL NAME AND IDENTITY CARD/PASSPORT NO.)") {
        $("#CollectedByID").css("display", "block");
        $("#IdentityNumberID").css("display", "block");


        $("#DeliveryAddressID").css("display", "none");
    }
    else if (selectValue == "TO BE DISPATCHED BY COURIER SERVICE TO THE CARDHOLDER’S MAILING ADDRESS (APPLICABLE FOR OVERSEAS ADDRESSES)") {

        $("#DeliveryAddressID").css("display", "none");

        $("#CollectedByID").css("display", "none");
        $("#IdentityNumberID").css("display", "none");
    }
    else if (selectValue == "TO BE DISPATCHED BY COURIER SERVICE TO THE FOLLOWING ADDRESS (APPLICABLE ONLY FOR OVERSEAS ADDRESSES)") {

        $("#DeliveryAddressID").css("display", "block");

        $("#CollectedByID").css("display", "none");
        $("#IdentityNumberID").css("display", "none");
    }
    else {
        $("#CollectedByID").css("display", "none");
        $("#IdentityNumberID").css("display", "none");
        $("#DeliveryAddressID").css("display", "none");
    }
    $('#DebitCardDetails_OtherDeliveryAddress').val('');
    $("#otherDeliveryAddress").css("display", "none");
}
function ShowRespectedFieldLegalCard() {
    //debugger;
    $("#DebitCardDetails_DeliveryAddress").data("kendoDropDownList").value('');
    var selectValue = $("#DebitCardDetails_DispatchMethod").data("kendoDropDownList").text();
    if (selectValue == "TO BE COLLECTED FROM THE BANKING CENTRE BY THE AUTHORISED CARDHOLDER") {


        $("#CollectedByID").css("display", "none");
        $("#IdentityNumberID").css("display", "none");
        $("#DeliveryAddressID").css("display", "none");
    }
    else if (selectValue == "TO BE COLLECTED FROM THE BANKING CENTRE BY (FULL NAME AND IDENTITY CARD/PASSPORT NO.)") {
        $("#CollectedByID").css("display", "block");
        $("#IdentityNumberID").css("display", "block");


        $("#DeliveryAddressID").css("display", "none");
    }
    else if (selectValue == "TO BE DISPATCHED BY COURIER SERVICE TO THE COMPANY’S MAILING ADDRESS (APPLICABLE FOR OVERSEAS ADDRESSES)") {

        $("#DeliveryAddressID").css("display", "none");

        $("#CollectedByID").css("display", "none");
        $("#IdentityNumberID").css("display", "none");
    }
    else if (selectValue == "TO BE DISPATCHED BY COURIER SERVICE TO THE FOLLOWING ADDRESS (APPLICABLE FOR OVERSEAS ADDRESSES)") {

        $("#DeliveryAddressID").css("display", "block");

        $("#CollectedByID").css("display", "none");
        $("#IdentityNumberID").css("display", "none");
    }
    else {
        $("#CollectedByID").css("display", "none");
        $("#IdentityNumberID").css("display", "none");
        $("#DeliveryAddressID").css("display", "none");
    }
    ShowHideOtherDeliveryAddress();
}

function onDeliveryAddressChange() {
    //debugger;
    ShowHideOtherDeliveryAddress();
}

function ShowHideOtherDeliveryAddress() {
    //debugger;
    var deliveryAddressSelectValue = $("#DebitCardDetails_DeliveryAddress").data("kendoDropDownList").text();
    if (deliveryAddressSelectValue != undefined && deliveryAddressSelectValue == 'Other Address') {
        $('#otherDeliveryAddress').show();
    }
    else {
        $('#DebitCardDetails_OtherDeliveryAddress').val('');
        $('#otherDeliveryAddress').hide();        
    }
}

function FillIdentification() {
    //debugger;
    GetIdentityCard();
    GetCardHolderAddresss();
    CopyName();
}
function GetIdentityCard() {
    //debugger;
    var ddlCardHolder = $('#DebitCardDetails_CardholderName').data("kendoDropDownList");
    if (ddlCardHolder != undefined) {
        var relatedPartyGuid = ddlCardHolder.value();
        $.ajax({
            url: $("#GetIdentityCardUrl").val(),
            cache: false,
            type: "POST",
            data: { relatedPartyGuid: relatedPartyGuid },
            success: function (result) {
                //debugger;
                var ddlHolderIdentityCountryOfIssue = $('#DebitCardDetails_CardHolderCountryOfIssue').data("kendoDropDownList");
                if (ddlHolderIdentityCountryOfIssue != undefined) {
                    ddlHolderIdentityCountryOfIssue.value(result.CountryOfIssue);
                    ddlHolderIdentityCountryOfIssue.trigger("change");
                    ddlHolderIdentityCountryOfIssue.focus();
                    if (result.CountryOfIssue != '') {
                        $($('#DebitCardDetails_CardHolderCountryOfIssue').closest('div')).children('div.field-validation-error').hide();
                    }
                }
                if ($('#DebitCardDetails_CardHolderIdentityNumber').length > 0) {
                    $('#DebitCardDetails_CardHolderIdentityNumber').val(result.IdentityNumber);
                    $('#DebitCardDetails_CardHolderIdentityNumber').focus();
                    $('#DebitCardDetails_CardHolderIdentityNumber').focusout();
                    if (result.IdentityNumber != '') {
                        $($('#DebitCardDetails_CardHolderIdentityNumber').closest('div')).children('div.field-validation-error').hide();
                    }
                }

            }
        });
    }
}

function CopyName() {
    var name = $("#DebitCardDetails_CardholderName").data("kendoDropDownList").text().toUpperCase();
    $("#DebitCardDetails_FullName").val(name).change().focus().focusout();
}

var loadingPanelVisible = false;
//debugger;
$("#loader").kendoLoader({
    visible: false
});
function showHideLoader() {
    var loader = $("#loader").data("kendoLoader");
    loadingPanelVisible = !loadingPanelVisible;
    if (loadingPanelVisible) {
        loader.show();
    } else {
        loader.hide();
    }
}
function showHideLoaderN(callback) {
    var loader = $("#loader").data("kendoLoader");
    loadingPanelVisible = !loadingPanelVisible;
    if (loadingPanelVisible) {
        loader.show();
    } else {
        loader.hide();
    }
    if (callback && typeof callback === 'function') {
        callback();
    }
}
$("#btnApplicationFinalSubmit").click(function () {
    showHideLoader();
})
//$("#btnApplicationSaveAsDraft").click(function () {
$(".btnShowLoader").click(function () {
    showHideLoader();
})
$("#btnApplicationClose").click(function () {
    showHideLoader();
})
$("#k-animation-container > ul > li").click(function () {
    showHideLoader();
})
$("#editRowApplicants").click(function () {
    showHideLoader();
})
$("#editRowRelatedP").click(function () {
    showHideLoader();
})
//logo
$(".push-hide").click(function () {
    showHideLoader();
})
//$('#ApplicationDetails_ApplicatonServices').change(function () {
//    showHideCardEbankingSection();
//});
function onChangeApplicatonServices(e) {
    //debugger;
    var appServiceVal = $(e.target[0]).val();
    var appServiceValIsChecked = $(e.target[0]).is(":checked");
    if (appServiceVal == '3fd36857-2751-44ea-9cdd-157d420661b7') {
        if (appServiceValIsChecked) {
            $("#accordionDebitCardDetails").show();
            $("#ApplicationStepper > ol > li > a[title|='Debit Cards']").parent().show();
        }
        else {
            if ($("#DebitCardDetails").data("kendoGrid").dataSource.total() > 0) {
                var dialog = $("#DebitcardConfirmation").data("kendoWindow");
                dialog.wrapper.addClass("middle-popup");
                dialog.center().open();
                $(".k-i-close").addClass('CLDebitCard');

            }
            else {
                $("#accordionDebitCardDetails").hide();
                $("#ApplicationStepper > ol > li > a[title|='Debit Cards']").parent().hide();
            }
        }
    }
    else if (appServiceVal == '38f530db-4db7-4aae-a105-f19f43a9fb9d') {
        if (appServiceValIsChecked) {
            $("#accordionEBankingSubscriber").show();
            $("#ApplicationStepper > ol > li > a[title|='Digital Banking']").parent().show();
        }
        else {
            if ($("#EBankingSubscriber").data("kendoGrid").dataSource.total() > 0) {
                var dialog = $("#EbankingConfirmation").data("kendoWindow");
                dialog.wrapper.addClass("middle-popup");
                dialog.center().open();
                $(".k-flat").addClass('CLEbanking');
            }
            else {
                $("#accordionEBankingSubscriber").hide();
                $("#ApplicationStepper > ol > li > a[title|='Digital Banking']").parent().hide();
            }
        }
    }
}
function showHideCardEbankingSection() {
    //debugger;
    var valueStore = [];
    $.each($("input[name='ApplicationDetails_ApplicatonServices']:checked"), function () {
        valueStore.push($(this).val());
    });
    if ($.inArray('3fd36857-2751-44ea-9cdd-157d420661b7', valueStore) != -1) {
        $("#accordionDebitCardDetails").show();
        $("#ApplicationStepper > ol > li > a[title|='Debit Cards']").parent().show();
    }
    else {
        if ($("#DebitCardDetails").data("kendoGrid").dataSource.total() > 0) {
            var dialog = $("#DebitcardConfirmation").data("kendoWindow");
            dialog.wrapper.addClass("middle-popup");
            dialog.center().open();
            $(".k-i-close").addClass('CLDebitCard');
            
        }
        else {
            $("#accordionDebitCardDetails").hide();
            $("#ApplicationStepper > ol > li > a[title|='Debit Cards']").parent().hide();
        }

    }

    if ($.inArray('38f530db-4db7-4aae-a105-f19f43a9fb9d', valueStore) != -1) {
        $("#accordionEBankingSubscriber").show();
        $("#ApplicationStepper > ol > li > a[title|='Digital Banking']").parent().show();
    }
    else {
        if ($("#EBankingSubscriber").data("kendoGrid").dataSource.total() > 0) {
            var dialog = $("#EbankingConfirmation").data("kendoWindow");
            dialog.wrapper.addClass("middle-popup");
            dialog.center().open();
            $(".k-flat").addClass('CLEbanking');
        }
        else {
            $("#accordionEBankingSubscriber").hide();
            $("#ApplicationStepper > ol > li > a[title|='Digital Banking']").parent().hide();
        }
    }
}
function SelectCardDetailsNo() {
    //debugger;
    var dialog = $("#DebitcardConfirmation").data("kendoWindow");
    dialog.close();
    $("input[type=checkbox][value=3fd36857-2751-44ea-9cdd-157d420661b7]").prop("checked", true);
}
function SelectDebitCardchk() {
    $("input[type=checkbox][value=3fd36857-2751-44ea-9cdd-157d420661b7]").prop("checked", true);
}
function SelectCardDetailsYes() {
    $.ajax({
        url: $("#DebitCardDestoryAllUrl").val(),
        cache: false,
        type: "POST",
        data: { id: $("#ApplicationId").val() },
        success: function (result) {
            $('#DebitCardDetails').data('kendoGrid').dataSource.read();
            $('#DebitCardDetails').data('kendoGrid').refresh();
            var dialog = $("#DebitcardConfirmation").data("kendoWindow");
            dialog.close();
            $("input[type=checkbox][value=3fd36857-2751-44ea-9cdd-157d420661b7]").prop("checked", false);
            $("#accordionDebitCardDetails").hide();
            $("#ApplicationStepper > ol > li > a[title|='Debit Cards']").parent().hide();

        }
    });
}

function SelectEbankingNo() {
    var dialog = $("#EbankingConfirmation").data("kendoWindow");
    dialog.close();
    $("input[type=checkbox][value=38f530db-4db7-4aae-a105-f19f43a9fb9d]").prop("checked", true);
}
function SelectEbankingchk() {
    $("input[type=checkbox][value=38f530db-4db7-4aae-a105-f19f43a9fb9d]").prop("checked", true);
}
function SelectEbankingYes() {
    $.ajax({
        url: $("#EbankingDestroyAllUrl").val(),
        cache: false,
        type: "POST",
        data: { id: $("#ApplicationId").val() },
        success: function (result) {
            $('#EBankingSubscriber').data('kendoGrid').dataSource.read();
            $('#EBankingSubscriber').data('kendoGrid').refresh();
            var dialog = $("#EbankingConfirmation").data("kendoWindow");
            dialog.close();
            $("input[type=checkbox][value=38f530db-4db7-4aae-a105-f19f43a9fb9d]").prop("checked", false);
            $("#accordionEBankingSubscriber").hide();
            $("#ApplicationStepper > ol > li > a[title|='EBanking']").parent().hide();
        }
    });
}
//function closeApplicantErroList() {
//    $('#errorList').hide();
//}
function applicationPrintSummary() {    
    $("#successDisplay .toastbody").html($("#PrintSummaryDownloadStartMessage").val());
    $("#successDisplay").show().delay(10000).fadeOut();
    var url = $("#RedirectPrintSummary").val();
    window.location.href = url + "?applicationNumber=" + $("#ApplicationNumber").val();
    //alert('Wait !! File will start downloading');
}
function applicationPrintFriendly() {
    $("#successDisplay .toastbody").html($("#PrintFriendlyDownloadStartMessage").val());
				$("#successDisplay").show().delay(10000).fadeOut(); 
    var url = $("#RedirectPrintFriendly").val();
    window.location.href = url + "?applicationNumber=" + $("#ApplicationNumber").val();
    //alert('Wait !! File will start downloading');
}
//function applicationExportXML() {
//    showHideLoader();
//    downloadXMLThroughJavaScript($("#hdnapplicationId").val(), $("#ApplicationNumber").val());
//}
function applicationExportXML() {
    //var loader = $("#loader").data("kendoLoader");
    
    $.ajax({
        async: false,
        type: 'GET',
        url: $("#ExportXMLUrlJS").val(),
        data: { appId: $("#hdnapplicationId").val() },
        //timeout: 180000, //180 seconds
        beforeSend: function () {
            $("#loader").data("kendoLoader").show();
        },
        success: function (result) {
            //debugger;
            
            // Your XML data as a string
            var xmlString = result;

            // Create a Blob with the XML data
            var blob = new Blob([xmlString], { type: 'application/xml' });

            // Create a link element to trigger the download
            var a = document.createElement('a');
            a.href = window.URL.createObjectURL(blob);
            a.download = $("#ApplicationNumber").val() + '.xml'; // Set the desired filename

            // Trigger a click event to initiate the download
            a.click();
            //downloadCSV(result, 'Applications.csv');
            $("#loader").data("kendoLoader").hide(); 
        },
        error: function (error) {
            //debugger;
            console.log(error);
            $("#loader").data("kendoLoader").hide(); 
        }
    });
       
}

function downloadXMLThroughJavaScript(appid, applicationNumber) {
    $.ajax({
        async: false,
        type: 'GET',
        url: $("#ExportXMLUrlJS").val(),
        data: { appId: appid },
        timeout: 180000, //180 seconds
        success: function (result) {
            //debugger;
            console.log(result);
            // Your XML data as a string
            var xmlString = result;

            // Create a Blob with the XML data
            var blob = new Blob([xmlString], { type: 'application/xml' });

            // Create a link element to trigger the download
            var a = document.createElement('a');
            a.href = window.URL.createObjectURL(blob);
            a.download = applicationNumber + '.xml'; // Set the desired filename

            // Trigger a click event to initiate the download
            a.click();
            //downloadCSV(result, 'Applications.csv');
            showHideLoader();
        },
        error: function (error) {
            //debugger;
            console.log(error);
            showHideLoader();
        }
    });
}
function isNumberWithoutDecimalKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function ShowRoles(id, entityName) {
    $.ajax({
        url: $("#GetRelatedPartyRolesUrl").val(),
        cache: false,
        type: "POST",
        data: { relatedPartyGuid: id, isLegalEntity: $("#IsLegalEntity").val() },
        success: function (result) {
            $("#EntityRoletbodyId").html('');
            for (var i = 0; i < result.length; i++) {
                var row = '<tr>';
                row += '<td>' + result[i] + '</td>';
                row += '</tr>';
                $("#EntityRoletbodyId").append(row);
            }
            var searchWindow = $("#EntityRoleWindow").data("kendoWindow");
            searchWindow.wrapper.addClass("middle-popup");
            searchWindow.center();
            searchWindow.open();
            searchWindow.title(entityName);
        }
    });
}

function bindRolesData(data) {
    if (data.EntityType_Name.toUpperCase() == 'APPLICANT') {
        return data.EntityRole_Name;
    }
    else {
        return `<a href="javascript:void(0);" onclick="ShowRoles('` + data.Entity + `','` + data.Entity_Name + `');" style="color: #0d6767"><b>View Roles</b></a>`;
    }
}
function onGridDataBoundBankdocuments(e) {
    $("#Bankdocuments .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#Bankdocuments .k-grid-content").attr("style", "max-height: 400px");
    deleteEmptyGridRecord('Bankdocuments');
}
function addConfirmButton(e) {
    $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
    var model = e.model;
    model.BankDocuments_Status = false;
    model.dirty = true;
    $("a.k-grid-cancel").addClass('btn btn-link');
    $("a.k-grid-update")[0].innerHTML = "SAVE AS DRAFT";
    $("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";

    $('<a class="k-button k-button-icontext k-primary k-grid-update custom" href="#" k-icon k-i-delete"></span>SAVE & CLOSE</a>').insertBefore(".k-grid-update");

    $(".custom").click(function (e) {
        model.BankDocuments_Status = true;
    });
    if (model.UploadFileName != undefined && model.UploadFileName != null && model.UploadFileName != '') {
        $('#prevUploadBankDoc').show();
        if (model.UploadFileName.indexOf('.pdf') > 0) {
            GetDocuments(model.FileUpload, model.UploadFileName, model.ExternalFileGuid);
        }
        else {
            GetDocumentsFile(model.FileUpload, model.UploadFileName, model.ExternalFileGuid);
        }

    }
    else {
        $('#prevUploadBankDoc').hide();
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
function Sync_handlerBankDocuments(e) {
    if (!e.sender.preventSync) {
        showHideLoader();
        window.location.reload();
    }
    e.sender.preventSync = false;
}
function error_handlerBankDocuments(args) {
    var errors = args.errors;
    if (errors) {
        args.sender.preventSync = true;
        var grid = $("#Bankdocuments").data("kendoGrid");
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
$("#accordionBankDocuments").kendoPanelBar({
    expandMode: "multiple"
});

var accordion = $("#accordionBankDocuments").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuBankdocuments").kendoContextMenu({
    target: "#Bankdocuments",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#Bankdocuments").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowBankDoc":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "Bankdocuments");
                break;
            default:
                break;
        };
    }
});
var downloadUrl = $("#DownloadFileUrl").val();
function GetDocuments(file, filename, fileGuid) {
    $('#pdfDocumets').show();
    $('#othersDocumets').hide();
    $('#documentModalLabel').html(filename);
    $('#embBankDocument').attr('src', file.substring(1, file.length));
    file = downloadUrl + '?applicationNumber=' + $("#ApplicationNumber").val() + '&fileGuid=' + fileGuid + '&filename=' + filename;
    $('#documentDownload').attr('href', file);


};
function GetDocumentsFile(file, filename, fileGuid) {
    $('#pdfDocumets').hide();
    $('#othersDocumets').show();
    $('#documentModalLabel').html(filename);
    $('#embBankDocument').attr('src', file);
    file = downloadUrl + '?applicationNumber=' + $("#ApplicationNumber").val() + '&fileGuid=' + fileGuid + '&filename=' + filename;
    $('#documentDownload').attr('href', file);
    $('#documentDownloadOthers').attr('href', file);


};

function onBankFileSuccess(e) {
    $.map(e.files, function (file) {
        var info = file.name;
        if (file.size > 0) {
            info += " (" + Math.ceil(file.size / 1024) + " KB)";
        }
        $('#FileName').val(file.name);
    })
    if ($('body > div.k-widget.k-window.k-display-inline-flex > div.k-popup-edit-form.k-window-content > div > div:nth-child(5) > div.k-tooltip.k-tooltip-error.k-validator-tooltip.k-invalid-msg.field-validation-error').length > 0) {
        $('body > div.k-widget.k-window.k-display-inline-flex > div.k-popup-edit-form.k-window-content > div > div:nth-child(5) > div.k-tooltip.k-tooltip-error.k-validator-tooltip.k-invalid-msg.field-validation-error').hide();
    }
}

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

$(document).ready(function () {
    if ($("#IsDisabledDecision").val() == "True") {
        $("#DecisionHistory_DecisionID").data("kendoDropDownList").enable(true);
        $("#DecisionHistory_CommentsID").removeAttr('disabled');
        $("#DecisionHistory_CommentsID").prop("disabled", false);

    }
});
function onDecisionDDLChange(e) {

    if (e.item) {
        var dataItem = this.dataItem(e.item);
        if (dataItem.Value != '') {
            $.ajax({
                url: $("#DecisionHistoryStageUrl").val(),
                cache: false,
                type: "POST",
                data: { applicationId: $("#ApplicationId").val(), decisionGuid: dataItem.Value },
                success: function (result) {
                    $('#DecisionHistory_StageID').val(result);
                }
            });
            console.log(dataItem);
            if (dataItem.Text == 'ESCALATE') {
                $('#divDecisionHistoryEscalateTo').show();
            }
            else {
                $('#divDecisionHistoryEscalateTo').hide();
                var dropdownlist = $("#DecisionHistory_EscalateToID").data("kendoDropDownList");
                dropdownlist.value('');
            }
        }
        else {
            $('#divDecisionHistoryEscalateTo').hide();
            var dropdownlist = $("#DecisionHistory_EscalateToID").data("kendoDropDownList");
            dropdownlist.value('');
        }
    }
}
$(function () {
    $('#DecisionHistoryBtn').on('click', function (evt) {
        var isFormValid = $("#DecisionHistoryForm").valid();
        if (isFormValid) {
            evt.preventDefault();
            $.post('/Applications/DecisionHistory', $('form').serialize(), function (response) {
                $("#DecisionHistoryDiv").html(response);
                show("#DecisionHistoryDiv");

                $("#DecisionHistory_DecisionID").val('');
                $("#DecisionHistory_StageID").val('');
                $("#DecisionHistory_CommentsID").val('');
            });
        }
    });
});
$(function () {
    $("form").kendoValidator();
});
$("#accordionDecisionHistorys").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionDecisionHistorys").data("kendoPanelBar");
accordion.collapse("#chartSection");
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

function ShowRoles(id, entityName) {
    $.ajax({
        url: $("#GetRelatedPartyRolesUrl").val(),
        cache: false,
        type: "POST",
        data: { relatedPartyGuid: id, isLegalEntity: $("#IsLegalEntity").val() },
        success: function (result) {
            $("#EntityRoletbodyId").html('');
            for (var i = 0; i < result.length; i++) {
                var row = '<tr>';
                row += '<td>' + result[i] + '</td>';
                row += '</tr>';
                $("#EntityRoletbodyId").append(row);
            }

            var searchWindow = $("#EntityRoleWindow").data("kendoWindow");
            searchWindow.wrapper.addClass("middle-popup");
            searchWindow.center();
            searchWindow.open();
            searchWindow.title(entityName);
        }
    });
}

function bindRolesData(data) {
    if (data.EntityType_Name.toUpperCase() == 'APPLICANT') {
        return data.EntityRole_Name;
    }
    else {
        return `<a href="javascript:void(0);" onclick="ShowRoles('` + data.Entity + `','` + data.Entity_Name + `');" style="color: #0d6767"><b>View Roles</b></a>`;
    }
}
function onGridDataBoundExpectedDocument(e) {
    $("#ExpectedDocuments .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#ExpectedDocuments .k-grid-content").attr("style", "max-height: 400px");
    deleteEmptyGridRecord('ExpectedDocuments');
}
function onSavechangeExpectedDocument(e) {
    console.log('OnSaveChange');
}
function Sync_handlerExpectedDocuments(e) {
    if (!e.sender.preventSync) {
        showHideLoader();
        window.location.reload();
    }
    e.sender.preventSync = false;
}
function addExpectedConfirmButton(e) {
    debugger;
    $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
    var model = e.model;
    model.BankDocuments_Status = false;
    model.dirty = true;
    $("a.k-grid-cancel").addClass('btn btn-link');
    $("a.k-grid-update")[0].innerHTML = "SAVE AS DRAFT";
    $("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";

    $('<a class="k-button k-button-icontext k-primary k-grid-update custom" href="#" k-icon k-i-delete"></span>SAVE & CLOSE</a>').insertBefore(".k-grid-update");
    $(".custom").click(function (e) {
        model.BankDocuments_Status = true;

    });
    if (model.UploadFileName != undefined && model.UploadFileName != null && model.UploadFileName != '') {
        $('#prevUploadExpectedDoc').show();
        if (model.UploadFileName.indexOf('.pdf') > 0) {
            GetExpectedDocuments(model.FileUpload, model.UploadFileName, model.ExternalFileGuid);
        }
        else {
            GetExpectedDocumentsFile(model.FileUpload, model.UploadFileName, model.ExternalFileGuid);
        }

    }
    else {
        $('#prevUploadExpectedDoc').hide();
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
function error_handlerExpectedDocuments(args) {
    var errors = args.errors;
    if (errors) {
        args.sender.preventSync = true;
        var grid = $("#ExpectedDocuments").data("kendoGrid");
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
$("#accordionExpectedDocuments").kendoPanelBar({
    expandMode: "multiple"
});

var accordion = $("#accordionExpectedDocuments").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuExpectedDocuments").kendoContextMenu({
    target: "#ExpectedDocuments",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#ExpectedDocuments").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowExpDoc":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "ExpectedDocuments");
                break;
            default:
                break;
        };
    }
});
var downloadURL = $("#DownloadFileUrl").val();
function GetExpectedDocuments(file, filename, fileGuid) {
    $('#pdfDocumetsExpected').show();
    $('#othersDocumetsExpected').hide();
    $('#ExpectedDocumentModalLabel').html(filename);
    $('#embDocumentExpected').attr('src', file.substring(1, file.length));
    file = downloadURL + '?applicationNumber=' + $("#ApplicationNumber").val() + '&fileGuid=' + fileGuid + '&filename=' + filename;
    $('#ExpectedDocumentDownload').attr('href', file);
};
function GetExpectedDocumentsFile(file, filename, fileGuid) {
    file = file.replace('~', '');
    $('#pdfDocumetsExpected').hide();
    $('#othersDocumetsExpected').show();
    $('#ExpectedDocumentModalLabel').html(filename);
    $('#embDocumentExpected').attr('src', file);
    file = downloadURL + '?applicationNumber=' + $("#ApplicationNumber").val() + '&fileGuid=' + fileGuid + '&filename=' + filename;
    $('#ExpectedDocumentDownload').attr('href', file);
    $('#ExpectedDocumentDownloadOthers').attr('href', file);
};

function onExpectedFileSuccess(e) {
    $.map(e.files, function (file) {
        var info = file.name;
        if (file.size > 0) {
            info += " (" + Math.ceil(file.size / 1024) + " KB)";
        }
        $('#FileName').val(file.name);
        if ($('body > div.k-widget.k-window.k-display-inline-flex > div.k-popup-edit-form.k-window-content > div > div:nth-child(1) > div:nth-child(5) > div.k-tooltip.k-tooltip-error.k-validator-tooltip.k-invalid-msg.field-validation-error').length > 0) {
            $('body > div.k-widget.k-window.k-display-inline-flex > div.k-popup-edit-form.k-window-content > div > div:nth-child(1) > div:nth-child(5) > div.k-tooltip.k-tooltip-error.k-validator-tooltip.k-invalid-msg.field-validation-error').hide();
        }
    })
}
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
function onNoteDataBound(e) {
    $("#NoteDetails .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#NoteDetails .k-grid-content").attr("style", "max-height: 400px");
    deleteEmptyGridRecord('NoteDetails');
    isAllConfirmed($("#hdnApplication_LeftMenu_Notes").val(), "Notes", "NoteDetails");
}

function addConfirmButton_Notes(e) {
    $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
    $("a.k-grid-update").addClass("SAVEASDRAFT");
    var model = e.model;
    model.NoteDetails_Status = false;
    model.dirty = true;
    $("a.k-grid-cancel").addClass('btn btn-link');
    $("a.k-grid-update")[0].innerHTML = "SAVE AS DRAFT";
    $("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";

    $('<a class="k-button k-button-icontext k-primary k-grid-update custom" href="#" k-icon k-i-delete"></span>SAVE & CLOSE</a>').insertBefore(".k-grid-update");
    $(".custom").click(function (e) {
        model.NoteDetails_Status = true;

    });
    $(".SAVEASDRAFT").click(function (e) {
        model.NoteDetails_Status = false;

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
function error_handlerNoteDetails(args) {
    var errors = args.errors;
    if (errors) {
        var grid = $("#NoteDetails").data("kendoGrid");
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
$("#accordionNoteDetails").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionNoteDetails").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuNoteDetails").kendoContextMenu({
    target: "#NoteDetails",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#NoteDetails").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowNotes":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "NoteDetails");
                break;
            default:
                break;
        };
    }
});
function onRelatedPartyDataBound(e) {
    $("#RelatedParties .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#RelatedParties .k-grid-content").attr("style", "max-height: 400px");
    isAllConfirmed($("#hdnApplication_LeftMenu_RelatedParties").val(), "RelatedParties", "RelatedParties");
    manageRelatedPartyMenuPanel();
}
$("#accordionRelatedParties").kendoPanelBar({
    expandMode: "multiple"
});

var accordion = $("#accordionRelatedParties").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuRelatedParties").kendoContextMenu({
    target: "#RelatedParties",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#RelatedParties").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                break;
            case "editRowRelatedP":
                var url = $("#RedirectToRelatedParty").val();
                window.location.href = url + "?application=" + $("#ApplicationNodeGUID").val() + "&&relatedParty=" + data.NodeGUID + "&&type=" + data.Type;
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "RelatedParties");
                break;
            default:
                break;
        };
    }
});
function OpenConfirmationAddRelatedParty() {

    var searchWindow = $("#AddrealtedPartyWindow").data("kendoWindow");
    searchWindow.wrapper.addClass("middle-popup");
    searchWindow.center();
    searchWindow.open();

}
function ShowUBO() {
    $("#UBOerrorMsg").css("display", "none");
    if ($("#RadioIndividual").is(":checked") || $("#RadioLegalEntity").is(":checked")) {
        $("#UBODiv").css("display", "block");
    }
}
function HideErrorDiv() {
    $("#UBOerrorMsg").css("display", "none");
}
function CreateRelatedParty() {
    var is_UBO = false;
    if ($("#RadioYes").is(":checked")) {
        is_UBO = true;
    }
    if ($("#RadioYes").is(":checked") || $("#RadioNo").is(":checked")) {
        $("#UBOerrorMsg").css("display", "none");
        var url = $("#RedirectToRelatedParty").val();
        if ($("#RadioIndividual").is(":checked")) {
            window.location.href = url + "?application=" + $("#ApplicationNodeGUID").val() + "&&type=INDIVIDUAL" + "&&isUBO=" + is_UBO;
        }
        if ($("#RadioLegalEntity").is(":checked")) {
            window.location.href = url + "?application=" + $("#ApplicationNodeGUID").val() + "&&type=LEGAL ENTITY" + "&&isUBO=" + is_UBO;
        }
    }
    else {
        $("#UBOerrorMsg").css("display", "block");
    }
}

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

function onGridDataBoundSourceOfOutgoingTran(e) {
    $("#SourceOfOutgoingTran .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#SourceOfOutgoingTran .k-grid-content").attr("style", "max-height: 400px");
    deleteEmptyGridRecord('SourceOfOutgoingTran');
    validatePurposeAndActivity();
}
function addConfirmButton_SOT(e) {
    $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
    $("a.k-grid-update").addClass("SAVEASDRAFT");
    var model = e.model;
    model.SourceOfOutgoingTransactions_Status = false;
    model.dirty = true;
    $("a.k-grid-cancel").addClass('btn btn-link');
    $("a.k-grid-update")[0].innerHTML = "SAVE AS DRAFT";
    $("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";

    $('<a class="k-button k-button-icontext k-primary k-grid-update custom" href="#" k-icon k-i-delete"></span>SAVE & CLOSE</a>').insertBefore(".k-grid-update");
    $(".custom").click(function (e) {
        model.SourceOfOutgoingTransactions_Status = true;

        $(".k-widget.k-window.k-display-inline-flex input").each(function (i, obj) {
            if (($(this).closest('div')).children('div.field-validation-error').length > 0) {
                var elementName = $(this).attr('name');
                $($(this).closest('div')).children('div.field-validation-error').remove();
                $($(this).closest('div')).append('<span class="field-validation-valid k-hidden" data-valmsg-for="' + elementName + '" data-valmsg-replace="true"></span>');
            }
        });
    });
    $(".SAVEASDRAFT").click(function (e) {
        model.SourceOfOutgoingTransactions_Status = false;

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
function error_handlerSourceOfOutcomingTransactions(args) {
    var errors = args.errors;
    if (errors) {
        var grid = $("#SourceOfOutgoingTran").data("kendoGrid");
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
$("#accordionSourceOfOutgoingTran").kendoPanelBar({
    expandMode: "multiple"
});

var accordion = $("#accordionSourceOfOutgoingTran").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuSourceOfOutgoingTran").kendoContextMenu({
    target: "#SourceOfOutgoingTran",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#SourceOfOutgoingTran").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowSOO":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "SourceOfOutgoingTran");
                break;
            default:
                break;
        };
    }
});
