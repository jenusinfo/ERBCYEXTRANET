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