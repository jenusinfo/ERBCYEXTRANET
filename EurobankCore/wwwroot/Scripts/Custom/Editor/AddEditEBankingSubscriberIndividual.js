function FillSubscriberIdentity() {
    var ddlCardHolder = $('#Subscriber').data("kendoDropDownList");
    if (ddlCardHolder != undefined) {
        var relatedPartyGuid = ddlCardHolder.value();
        $.ajax({
            url: $("#GetIdentityCardUrl").val(),
            cache: false,
            type: "GET",
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
function GetPersonType() {
    //debugger;
    var ddlCardHolder = $('#Subscriber').data("kendoDropDownList");
    if (ddlCardHolder != undefined) {
        var personGUID = ddlCardHolder.value();
        $.ajax({
            url: $("#GetpersonTypeUrl").val(),
            cache: false,
            type: "GET",
            data: { personGUID: personGUID },
            success: function (res) {
                //debugger;
                var ddlAccessLevel = $('#AccessLevel').data("kendoDropDownList");
                if (res == "APPLICANT" && $("#ApplicationDetails_ApplicationTypeName").val() == "INDIVIDUAL") {
                    //debugger;
                    if (ddlAccessLevel != undefined) {
                        ddlAccessLevel.text('FULL');
                        ddlAccessLevel.trigger("change");
                        ddlAccessLevel.readonly();
                    }
                    var ddlLimitAmount = $('#LimitAmount').data("kendoDropDownList");
                    if (ddlLimitAmount != undefined) {
                        ddlLimitAmount.text("MAXIMUM ALLOWABLE LIMIT PER TRANSACTION TYPE");
                        ddlLimitAmount.trigger("change");
                        ddlLimitAmount.readonly();
                    }
                }
                else {
                    if ($("#AccessLevelName").val() != "FULL" && $("#AccessLevelName").val() != "VIEW") {
                    if (ddlAccessLevel != undefined) {
                        ddlAccessLevel.text('');
                        ddlAccessLevel.trigger("change");
                        ddlAccessLevel.readonly(false);
                    }
                    var ddlLimitAmount = $('#LimitAmount').data("kendoDropDownList");
                    if (ddlLimitAmount != undefined) {
                        ddlLimitAmount.text("");
                        ddlLimitAmount.trigger("change");
                        ddlLimitAmount.readonly(true);
                        }
                    }
                }
            }
        });
    }
}
function onchangeAccessLevel() {
    //debugger;
    var ddlAccessLevel = $('#AccessLevel').data("kendoDropDownList").text();
    var ddlLimitAmount = $('#LimitAmount').data("kendoDropDownList");
    if (ddlAccessLevel == "FULL") {

        if (ddlLimitAmount != undefined) {
            ddlLimitAmount.text("MAXIMUM ALLOWABLE LIMIT PER TRANSACTION TYPE");
            ddlLimitAmount.trigger("change");
            ddlLimitAmount.readonly();
        }
    }
    else {
        if (ddlLimitAmount != undefined) {
            ddlLimitAmount.text("");
            ddlLimitAmount.trigger("change");
            ddlLimitAmount.readonly(true);
        }
    }
}