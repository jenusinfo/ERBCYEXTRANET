function ShowRespectedEmployment() {
    var selectedValue = $("#EmploymentStatus").data("kendoDropDownList").text();
    if (selectedValue == "RETIRED" || selectedValue == "UNEMPLOYED" || selectedValue == "HOMEMAKER" || selectedValue == "STUDENT/MILITARY SERVICES") {
        var Profession = $('#Profession').data("kendoDropDownList");
        if (Profession != undefined) {
            Profession.text("");
            Profession.trigger("change");
        }
        $("#EmployersName").val("").trigger("change");
        $("#Profession_DIV").css("display", "none");
        $("#EmployersName_DIV").css("display", "none");
    }
    else {
        $("#Profession_DIV").css("display", "block");
        $("#EmployersName_DIV").css("display", "block");
    }
}
$(document).ready(function () {
    var selectedValue = $("#EmploymentStatusNameHdn").val();
    if (selectedValue != null && selectedValue != "") {
        if (selectedValue == "RETIRED" || selectedValue == "UNEMPLOYED" || selectedValue == "HOMEMAKER" || selectedValue == "STUDENT/MILITARY SERVICES") {
            $("#Profession_DIV").css("display", "none");
            $("#EmployersName_DIV").css("display", "none");
        }
        else {
            $("#Profession_DIV").css("display", "block");
            $("#EmployersName_DIV").css("display", "block");
        }
    }


});
var isApplicationPermisssionEdit = $("#IsApplicationPermissionEdit").val();
function onChangeCompanyDetailsDOI(e) {
    $('#CompanyDetails_HdnDateofIncorporation').val($('#CompanyDetails_DateofIncorporation').val());
    validateCompanyDetails();
}
function onChangePersonalDetailsDOB(e) {
    $('#PersonalDetails_HdnDateOfBirth').val($('#PersonalDetails_DateOfBirth').val());
    validatePersonalDetails();
}
function searchPersons() {
    var applicationTypevalue = $("#toolApplicationType").val();
    var referenceNumbervalue = $("#toolReferenceNumber").val();
    var fullNamevalue = $("#toolFullName").val();
    var identificationNumbervalue = $("#toolIdentificationNumber").val();
    var dateofBirthvalue = $("#toolDateofBirth").val();
    var issueDatevalue = $("#toolIssueDate").val();
    var citizenshipNamevalue = $("#toolCitizenshipName").val();
    grid = $("#RegPersons").data("kendoGrid");

    if (applicationTypevalue || referenceNumbervalue || fullNamevalue || identificationNumbervalue || dateofBirthvalue || issueDatevalue || citizenshipNamevalue) {
        console.log('in');
        grid.dataSource.filter({
            logic: "and",
            filters: [
                { field: "ApplicationTypeName", operator: "contains", value: applicationTypevalue },
                { field: "FullName", operator: "contains", value: fullNamevalue },
                { field: "IdentificationNumber", operator: "contains", value: identificationNumbervalue },
                { field: "DateofBirth", operator: "contains", value: dateofBirthvalue },
                { field: "IssueDate", operator: "contains", value: issueDatevalue },
                { field: "CitizenshipName", operator: "contains", value: citizenshipNamevalue },
            ]
        });
    } else {
        grid.dataSource.filter({});
    }
}
function selectPersonFromGrid() {
    var grid = $("#RegPersons").data("kendoGrid");
    var selectedItem = grid.dataItem(grid.select());

    var dropdownlistTitle = $("#PersonalDetails_Title").data("kendoDropDownList");
    dropdownlistTitle.value(selectedItem.Title);

    $('#PersonalDetails_FirstName').val(selectedItem.FirstName);
    $('#PersonalDetails_LastName').val(selectedItem.LastName);
    $('#PersonalDetails_FathersName').val(selectedItem.FatherName);

    var dropdownlistGender = $("#PersonalDetails_Gender").data("kendoDropDownList");
    dropdownlistGender.value(selectedItem.Gender);

    $('#PersonalDetails_DateOfBirth').val(selectedItem.DateofBirth);
    $('#PersonalDetails_HdnDateOfBirth').val(selectedItem.DateofBirth);
    $('#PersonalDetails_PlaceOfBirth').val(selectedItem.PlaceofBirth);

    var dropdownlistCOB = $("#PersonalDetails_CountryOfBirth").data("kendoDropDownList");
    dropdownlistCOB.value(selectedItem.CountryofBirth);

    var homeTelCountryCode = $("#Country_Code_HomeTelNoNumber").data("kendoDropDownList");
    if (typeof homeTelCountryCode !== 'undefined') {
        homeTelCountryCode.value(selectedItem.HomeTelNoCountryCode);
        $('#ContactDetails_HomeTelNoNumber').val(selectedItem.HomeTelNoNumber);

        var mobileTelCountryCode = $("#Country_Code_MobileTelNoNumber").data("kendoDropDownList");
        mobileTelCountryCode.value(selectedItem.MobileTelNoCountryCode);
        $('#ContactDetails_MobileTelNoNumber').val(selectedItem.MobileTelNoNumber);

        var workTelCountryCode = $("#Country_Code_WorkTelNoNumber").data("kendoDropDownList");
        workTelCountryCode.value(selectedItem.WorkTelNoCountryCode);
        $('#ContactDetails_WorkTelNoNumber').val(selectedItem.WorkTelNoNumber);

        var faxTelCountryCode = $("#Country_Code_FaxNoFaxNumber").data("kendoDropDownList");
        faxTelCountryCode.value(selectedItem.FaxNoCountryCode);
        $('#ContactDetails_FaxNoFaxNumber').val(selectedItem.FaxNoFaxNumber);

        $('#ContactDetails_ContactDetails_EmailAddress').val(selectedItem.EmailAddress);
    }
    else {
        $('#ContactDetails_IsRetriveFromRegistry').val('true');
        $('#ContactDetails_hdnCountry_Code_MobileTelNoNumber').val(selectedItem.MobileTelNoCountryCode);
        $('#ContactDetails_hdnContactDetails_MobileTelNoNumber').val(selectedItem.MobileTelNoNumber);
        $('#ContactDetails_hdnCountry_Code_HomeTelNoNumber').val(selectedItem.HomeTelNoCountryCode);
        $('#ContactDetails_hdnContactDetails_HomeTelNoNumber').val(selectedItem.HomeTelNoNumber);
        $('#ContactDetails_hdnCountry_Code_WorkTelNoNumber').val(selectedItem.WorkTelNoCountryCode);
        $('#ContactDetails_hdnContactDetails_WorkTelNoNumber').val(selectedItem.WorkTelNoNumber);
        $('#ContactDetails_hdnCountry_Code_FaxNoFaxNumber').val(selectedItem.FaxNoCountryCode);
        $('#ContactDetails_hdnContactDetails_FaxNoFaxNumber').val(selectedItem.FaxNoFaxNumber);
    }
    var educationLevel = $("#PersonalDetails_EducationLevel").data("kendoDropDownList");
    if (educationLevel != undefined) {
        educationLevel.value(selectedItem.EducationLevel);
    }

    var newRow = { CitizenshipValue: selectedItem.Citizenship, IdentificationDetails_TypeOfIdentification: selectedItem.TypeofIdentification, IdentificationDetails_IdentificationNumber: selectedItem.IdentificationNumber, CountryOfIssueValue: selectedItem.IssuingCountry, IdentificationDetails_IssueDate: selectedItem.IssueDateTime, IdentificationDetails_ExpiryDate: selectedItem.ExpiryDateTime, Status: false };
    console.log(newRow);
    var gridIdentification = $("#IdentificationDetails").data("kendoGrid");
    if (typeof gridIdentification != 'undefined') {
        var addedRow = gridIdentification.dataSource.add(newRow);
        console.log(addedRow);
        gridIdentification.saveChanges();
        rowIdenItem = addedRow;
    }
    else {
        $('#hdnCitizenship').val(selectedItem.Citizenship);
        $('#hdnTypeofIdentification').val(selectedItem.TypeofIdentification);
        $('#hdnIdentificationNumber').val(selectedItem.IdentificationNumber);
        $('#hdnIssuingCountry').val(selectedItem.IssuingCountry);
        $('#hdnIssueDateTime').val(selectedItem.IssueDateTime);
        $('#hdnExpiryDateTime').val(selectedItem.ExpiryDateTime);
    }

    $("#PersonSearchWindow").data("kendoWindow").close();
    $("#divRegisteredPerson").css("display", "none");
    $('#PersonalDetails_PersonRegistryId').val(selectedItem.NodeID);
    ChangeMaxLengthIndividualFaxNo();
    ChangeMaxLengthIndividualWorkNo();
    ChangeMaxLengthIndividualMobileNo();
    ChangeMaxLengthIndividualHomeNo();
}
function personCancelGrid() {
    $("#PersonSearchWindow").data("kendoWindow").close();
    $("#divRegisteredPerson").css("display", "none");
}

function personSearchGrid() {
    $("#RegPersons").data("kendoGrid").dataSource.read();
    var searchWindow = $("#PersonSearchWindow").data("kendoWindow");
    searchWindow.wrapper.addClass("middle-popup");
    searchWindow.center();
    searchWindow.open();
    $("#divRegisteredPerson").css("display", "block");
}
function limitlength(obj, length, spanid) {
    debugger;
    var maxlength = length;
    if (maxlength == 0) {
        maxlength = obj.maxlength;
    }
    if (obj.value.length > maxlength) {
        document.getElementById(spanid).innerHTML = 'Maximum allowed length of the input text is ' + length;
        obj.value = obj.value.substring(0, maxlength)
    }
    else {
        document.getElementById(spanid).innerHTML = ' ';
    }
}
function clearContact() {

    var dropdownlistTitle = $("#PersonalDetails_Title").data("kendoDropDownList");
    dropdownlistTitle.value('');

    $('#PersonalDetails_FirstName').val("");
    $('#PersonalDetails_LastName').val("");
    $('#PersonalDetails_FathersName').val("");

    var dropdownlistGender = $("#PersonalDetails_Gender").data("kendoDropDownList");
    dropdownlistGender.value('');

    $('#PersonalDetails_DateOfBirth').val("");
    $('#PersonalDetails_PlaceOfBirth').val("");

    var dropdownlistCOB = $("#PersonalDetails_CountryOfBirth").data("kendoDropDownList");
    dropdownlistCOB.value("");

    var homeTelCountryCode = $("#Country_Code_HomeTelNoNumber").data("kendoDropDownList");
    homeTelCountryCode.value('');
    $('#ContactDetails_HomeTelNoNumber').val("");

    var mobileTelCountryCode = $("#Country_Code_MobileTelNoNumber").data("kendoDropDownList");
    mobileTelCountryCode.value('');
    $('#ContactDetails_MobileTelNoNumber').val("");

    var workTelCountryCode = $("#Country_Code_WorkTelNoNumber").data("kendoDropDownList");
    workTelCountryCode.value('');
    $('#ContactDetails_WorkTelNoNumber').val("");

    var faxTelCountryCode = $("#Country_Code_FaxNoFaxNumber").data("kendoDropDownList");
    faxTelCountryCode.value('');
    $('#ContactDetails_FaxNoFaxNumber').val("");

    $('#ContactDetails_ContactDetails_EmailAddress').val("");
    var educationLevel = $("#PersonalDetails_EducationLevel").data("kendoDropDownList");
    if (educationLevel != undefined) {
        educationLevel.value('');
    }

}

function isAllConfirmed(menuTitle, moduleName, gridName, isRecordMandatory) {
    var searchName = "Pending";
    var isNotCompleted = false;
    var kendoGrid = $("#" + gridName).data("kendoGrid");
    var gridRecordCount = kendoGrid.dataSource.total();
    if (moduleName == 'Address') {
        gridRecordCount = $('#' + gridName + ' .k-grid-content .k-master-row').length;
    }
    var steps = $("#ApplicationStepper").data('kendoStepper')._steps;
    var searchNameFound = kendoGrid.dataSource.data().some(
        function (dataItem) {
            if (moduleName == 'Tax' || moduleName == 'Identification' || moduleName == 'Address') {
                if (dataItem.StatusName == searchName) {
                    isNotCompleted = true;
                }
            }

            if (kendoGrid.dataSource.indexOf(dataItem) == (gridRecordCount - 1)) {


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
    if (gridRecordCount == 0 && isRecordMandatory != undefined && isRecordMandatory == true) {
        setMenuError(menuTitle);
    }

}

function isAllPepDetailsConfirmed() {
    var menuTitle = $("#hdnApplication_RelatedParty_LeftMenu_PEPDetails").val();
    var searchName = "Pending";
    var isNotCompleted = false;

    var ddlPepApplicant = $('#IsPepID').val();
    var ddlPepFamily = $('#IsRelatedToPepID').val();
    var steps = $("#ApplicationStepper").data('kendoStepper')._steps;

    if (ddlPepApplicant == 'false' && ddlPepFamily == 'false') {
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
        if (($("#PepApplicant").data("kendoGrid").dataSource.total() == 0 && ddlPepApplicant == 'true') || ($("#PEPAssociates").data("kendoGrid").dataSource.total() == 0 && ddlPepFamily == 'true')) {
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
        else {
            if ($("#PepApplicant").data("kendoGrid").dataSource.total() >= $("#PEPAssociates").data("kendoGrid").dataSource.total()) {
                var kendoGrid = $("#PepApplicant").data("kendoGrid");
                var kendoGrid2 = $("#PEPAssociates").data("kendoGrid");
            }
            else {
                var kendoGrid = $("#PEPAssociates").data("kendoGrid");
                var kendoGrid2 = $("#PepApplicant").data("kendoGrid");
            }


            var searchNameFound = kendoGrid.dataSource.data().some(
                function (dataItem) {
                    if (dataItem.StatusName == searchName) {
                        isNotCompleted = true;
                    }

                    if (kendoGrid.dataSource.indexOf(dataItem) == (kendoGrid.dataSource.total() - 1)) {


                        if (!isNotCompleted) {

                            if (kendoGrid2.dataSource.total() > 0) {
                                var searchItem2 = kendoGrid2.dataSource.data().some(
                                    function (dataItem2) {
                                        if (dataItem2.StatusName == searchName) {
                                            isNotCompleted = true;
                                        }

                                        if (kendoGrid2.dataSource.indexOf(dataItem2) == (kendoGrid2.dataSource.total() - 1)) {
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
                            else {
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
    }
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
$("#accordionPersonalDetails").kendoPanelBar({
    expandMode: "multiple"
});

var accordion = $("#accordionPersonalDetails").data("kendoPanelBar");
if ($("#Id").val() > 0) {
    if (accordion != undefined)
        accordion.collapse("#chartSection");
}
function validateContactDetailsIndividual() {
    $.ajax({
        url: $("#ValidateContactDetailsIndividualUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#Application_RelatedParty_LegalEntity_LeftMenu_ContactDetails").val());
            }
            else {
                setMenuError($("#Application_RelatedParty_LegalEntity_LeftMenu_ContactDetails").val());
            }
        }
    });
}

function validatePersonalDetails() {
    $.ajax({
        url: $("#ValidatePersonalDetailsUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#Application_RelatedParty_LegalEntity_LeftMenu_PersonalDetails").val());
            }
            else {
                setMenuError($("#Application_RelatedParty_LegalEntity_LeftMenu_PersonalDetails").val());
            }
        }
    });
}


function validateBusinessAndFinancialProfile() {
    $.ajax({
        url: $("#validateBusinessAndFinancialProfileUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if ($("#IndividualIsRelatedPartyUBO").val() == "True") {
                var isValidOriginOfAnnualIncome = ValidateOriginOfAnnualIncome();
                var isValidOriginOfAssets = ValidateOriginOfAssets();
                if (result.IsValid == true && !isValidOriginOfAnnualIncome && !isValidOriginOfAssets) {
                    setMenuSuccess($("#Application_RelatedParty_LeftMenu_BusinessProfile").val());
                }
                else {
                    setMenuError($("#Application_RelatedParty_LeftMenu_BusinessProfile").val());
                }
            }

            else {
                if (result.IsValid == true) {
                    setMenuSuccess($("#Application_RelatedParty_LeftMenu_BusinessProfile").val());
                }
                else {
                    setMenuError($("#Application_RelatedParty_LeftMenu_BusinessProfile").val());
                }
            }

        }
    });
}

function validateCompanyDetails() {
    $.ajax({
        url: $("#ValidateCompanyDetailsUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#Application_Applicant_LeftMenu_CompanyDetails").val());
                var loader = $("#loader").data("kendoLoader");
                loader.hide();
            }
            else {
                setMenuError($("#Application_Applicant_LeftMenu_CompanyDetails").val());
                var loader = $("#loader").data("kendoLoader");
                loader.hide();
            }
        }
    });
}

function validatePartyRolesIndividual() {
    $.ajax({
        url: $("#ValidatePartyRolesIndividualUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#Eurobank_Application_RelatedParty_LeftMenu_Roles").val());
            }
            else {
                setMenuError($("#Eurobank_Application_RelatedParty_LeftMenu_Roles").val());
            }
        }
    });
}

function validatePartyRolesLegal() {
    $.ajax({
        url: $("#ValidatePartyRolesLegalUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#Eurobank_Application_RelatedParty_LeftMenu_Roles").val());
            }
            else {
                setMenuError($("#Eurobank_Application_RelatedParty_LeftMenu_Roles").val());
            }
        }
    });
}
function ValidateOriginOfAnnualIncome() {
    var searchName = "Pending";
    var isNotCompleted = false;
    var kendoGrid = $("#SourceOfIncome").data("kendoGrid");
    var gridRecordCount = kendoGrid.dataSource.view().length;
    var searchNameFound = kendoGrid.dataSource.data().some(
        function (dataItem) {
            if (dataItem.StatusName == searchName) {
                isNotCompleted = true;
            }
            if (kendoGrid.dataSource.indexOf(dataItem) == (gridRecordCount - 1)) {
                if (!isNotCompleted) {
                    isNotCompleted = false;
                }
                else {
                    isNotCompleted = true;
                }
            }
        });
    if (gridRecordCount == 0) {
        isNotCompleted = true;
    }
    return isNotCompleted;
}
function ValidateOriginOfAssets() {
    var searchName = "Pending";
    var isNotCompleted = false;
    var kendoGrid = $("#OriginOfTotal").data("kendoGrid");
    var gridRecordCount = kendoGrid.dataSource.view().length;
    var searchNameFound = kendoGrid.dataSource.data().some(
        function (dataItem) {
            if (dataItem.StatusName == searchName) {
                isNotCompleted = true;
            }
            if (kendoGrid.dataSource.indexOf(dataItem) == (gridRecordCount - 1)) {
                if (!isNotCompleted) {
                    isNotCompleted = false;
                }
                else {
                    isNotCompleted = true;
                }
            }
        });
    if (gridRecordCount == 0) {
        isNotCompleted = true;
    }
    return isNotCompleted;
}

$("#accordionCompanyDetails").kendoPanelBar({
    expandMode: "multiple"
});

var accordion = $("#accordionCompanyDetails").data("kendoPanelBar");

if ($("#Id").val() > 0) {
    if (accordion != undefined)
        accordion.collapse("#chartSection");
}
$("#accordionEmploymentDetails").kendoPanelBar({
    expandMode: "multiple"
});

var accordion = $("#accordionEmploymentDetails").data("kendoPanelBar");
if (accordion != undefined)
    accordion.collapse("#chartSection");

$(document).ready(function () {
    if (isApplicationPermisssionEdit == "False") {
        setAllInputElementDisabled('app-body-per');
    }
});

function onSelectAddressType(e) {
    if (e.item) {
        var dataItem = this.dataItem(e.item);
        console.log(dataItem);
        var dropdownlist = $("#Country").data("kendoDropDownList");
        var dropdownlistPhone = $("#CountryCode_PhoneNo").data("kendoDropDownList");
        var dropdownlistFax = $("#CountryCode_FaxNo").data("kendoDropDownList");


        if (dataItem.Text == 'OFFICE IN CYPRUS') {
            dropdownlist.text("CYPRUS");
            dropdownlist.readonly();
            if (dropdownlistPhone != undefined) {
                dropdownlistPhone.text("CYPRUS +357");
                dropdownlistPhone.readonly();
            }

            if (dropdownlistPhone != undefined) {
                dropdownlistFax.text("CYPRUS +357");
                dropdownlistFax.readonly();
            }

        }
        else {
            dropdownlist.readonly(false);
            if (dropdownlistPhone != undefined) {
                dropdownlistPhone.readonly(false);
            }

            if (dropdownlistFax != undefined) {
                dropdownlistFax.readonly(false);
            }

        }
    }
}

function ShowHideFieldsOnAddressType() {
    var selectedAddressType = $("#AddressType").data("kendoDropDownList");
    if (selectedAddressType != undefined) {
        if (selectedAddressType.text().trim() == 'MAILING ADDRESS') {
            $('#divAddPOBox').show();
        }
        else {
            $("#POBox").val('').trigger('change').focusin().focusout();
            $('#divAddPOBox').hide();
        }
    }
}

$(document).ready(function () {
    manageMenuPanel();
    AddApplicantToRelatedParyMenuPanel();
});
function manageMenuPanel() {
    var isLegalEntity = $("#IsLegalEnity").val();
    if (isLegalEntity == 'True') {
        manageLegalApplicantMenuPanel();
    }
    else {
        manageIndividualApplicantMenuPanel();
    }
}
function setTwoNumberDecimal(event) {
    this.value = parseFloat(this.value).toFixed(2);
}

$(document).on("input", ".custom-decimal-positive", function () {
    $(this).val($(this).val().replace(/[^\d.]/g, ''));
});

function RemoveAllValidationMessage() {
    $(".k-widget.k-window.k-display-inline-flex input").each(function (i, obj) {
        if (($(this).closest('div')).children('div.field-validation-error').length > 0) {
            var elementName = $(this).attr('name');
            $($(this).closest('div')).children('div.field-validation-error').remove();
            $($(this).closest('div')).append('<span class="field-validation-valid k-hidden" data-valmsg-for="' + elementName + '" data-valmsg-replace="true"></span>');
        }
    });
}
function SelectConfirmPersonComYes() {
    companySearchGrid();
    var dialog = $("#ReSelectConfirmPersonCom").data("kendoWindow");
    dialog.close();
}

function SelectConfirmPersonComNo() {
    var dialog = $("#ReSelectConfirmPersonCom").data("kendoWindow");
    dialog.close();
}

function OpenSelectConfirmPersonCom() {

    if ($("#Id").val() > 0) {
        $("#ReSelectConfirmPersonCom").kendoWindow();
        var dialog = $("#ReSelectConfirmPersonCom").data("kendoWindow");
        dialog.wrapper.addClass("middle-popup");
        dialog.center().open();
    }
    else {
        SelectConfirmPersonComYes();
    }
}
function companySearchGrid() {
    $("#RegPersonsCompany").data("kendoGrid").dataSource.read();
    var searchWindow = $("#CompanyPersonSearchWindow").data("kendoWindow");
    searchWindow.wrapper.addClass("middle-popup");
    searchWindow.center();
    searchWindow.open();
    $("#divRegisteredPersonCompany").css("display", "block");
}
function companyPersonCancelGrid() {
    $("#CompanyPersonSearchWindow").data("kendoWindow").close();
    $("#divRegisteredPersonCompany").css("display", "none");
}

function selectCompanyPersonFromGrid() {
    debugger;
    var grid = $("#RegPersonsCompany").data("kendoGrid");
    var selectedItem = grid.dataItem(grid.select());

    $('#CompanyDetails_RegisteredName').val(selectedItem.RegisteredName);
    var entityValue = $("#EntityType").data("kendoDropDownList");
    entityValue.value(selectedItem.EntityType);
    var countryOfCorporation = $("#CountryofIncorporation").data("kendoDropDownList");
    countryOfCorporation.text(selectedItem.CountryofIncorporation);
    $('#CompanyDetails_RegistrationNumber').val(selectedItem.RegistrationNumber);
    $('#CompanyDetails_DateofIncorporation').val(selectedItem.DateofIncorporation);
    $('#CompanyDetails_HdnDateofIncorporation').val(selectedItem.DateofIncorporation);

    $("#CompanyPersonSearchWindow").data("kendoWindow").close();
    $("#divRegisteredPersonCompany").css("display", "none");
    $('#RegistryId').val(selectedItem.NodeID);
}

function clearCompanyDetails() {
    $('#CompanyDetails_RegisteredName').val('');
    var entityValue = $("#EntityType").data("kendoDropDownList");
    if (entityValue != undefined) {
        entityValue.value('');
    }
    var countryOfCorporation = $("#CountryofIncorporation").data("kendoDropDownList");
    if (countryOfCorporation != undefined) {
        countryOfCorporation.value('');
    }
    $('#CompanyDetails_RegistrationNumber').val('');
    $('#CompanyDetails_DateofIncorporation').val('');
}

function searchCompanyPersons() {

    //var applicationTypevalue = $("#toolPersonType").val();
    var fullNamevalue = $("#toolRegisteredName").val();
    var registrationNumbervalue = $("#toolRegistrationNumber").val();
    var dateofIncorporationvalue = $("#toolDateofIncorporation").val();
    var countryValue = $("#toolCountryofIncorporation").val();
    grid = $("#RegPersonsCompany").data("kendoGrid");

    if (fullNamevalue || registrationNumbervalue || dateofIncorporationvalue || countryValue) {
        console.log('in');
        grid.dataSource.filter({
            logic: "and",
            filters: [
                //{ field: "PersonType", operator: "contains", value: applicationTypevalue },
                { field: "RegisteredName", operator: "contains", value: fullNamevalue },
                { field: "RegistrationNumber", operator: "contains", value: registrationNumbervalue },
                { field: "DateofIncorporation", operator: "contains", value: dateofIncorporationvalue },
                { field: "CountryofIncorporation", operator: "contains", value: countryValue },
            ]
        });
    } else {
        grid.dataSource.filter({});
    }
}
var loadingPanelVisible = false;
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
$("#btnRelatedPartyProceed").click(function () {
    showHideLoader();
})
$("#btnRelatedPartySaveAndClose").click(function () {
    showHideLoader();
})
$("#btnRelatedPartySaveAsDraft").click(function () {
    showHideLoader();
})
$("#btnRelatedPartyClose").click(function () {
    showHideLoader();
})
$(".push-hide").click(function () {
    showHideLoader();
})
function ValidateRelatedParty() {
    var islegal = $("#IsLegalEnity").val();
    var flag = 0;
    if (islegal == "True") {
        if ($("#CompanyDetails_RegisteredName").val() == "") {
            $("#CompanyDetailsRegisteredNameErrorrReq").text("Please enter Registered Name");
            flag = 1;
        }
        else {
            $("#CompanyDetailsRegisteredNameErrorrReq").text("");
        }
    }
    else {
        if ($("#PersonalDetails_FirstName").val() == "") {
            $("#PersonalDetailsFirstNameErrorrReq").text("Please enter First Name");
            flag = 1;
        }
        else {
            $("#PersonalDetailsFirstNameErrorrReq").text("");
        }
        if ($("#PersonalDetails_LastName").val() == "") {
            $("#PersonalDetailsLastNameErrorrReq").text("Please enter Last Name");
            flag = 1;
        }
        else {
            $("#PersonalDetailsLastNameErrorrReq").text("");
        }
    }
    if (flag == 1) {
        showHideLoader();
        return false;
    }
    else {
        return true;
    }
}
function manageLegalApplicantMenuPanel() {
    var applicantLegalName = $("#LegalApplicantName").val();
    if ($('#ApplicationStepper li a[title="Legal Entity Details"]').parent().length > 0) {
        $('#ApplicationStepper li a[title="Legal Entity Details"]').parent().append('<span class="row" style="margin-left: 20%;color: #27438c;">' + applicantLegalName.toUpperCase() + '</span>');
    }
}

function manageIndividualApplicantMenuPanel() {
    var applicantName = $("#ApplicantName").val();
    if ($('#ApplicationStepper li a[title="Personal Details"]').parent().length > 0) {
        $('#ApplicationStepper li a[title="Personal Details"]').parent().append('<span class="row" style="margin-left: 20%;color: #27438c;">' + applicantName.toUpperCase() + '</span>');
    }
}

function AddApplicantToRelatedParyMenuPanel() {
    debugger;
    var appNumber = $('#ApplicationNumber').val();
    var appType = $('#ApplicationTypeName').val();
    var innerHtml = '';
    $.ajax({
        url: $("#GetApplicantNameofRelatedPartyURL").val(),
        cache: false,
        type: "POST",
        data: {applicaitonNumber : appNumber, applicationType : appType}
,
        success: function (response) {
            console.log(response);
            if (response && response.length > 0) {
                var resultHtml = '<ol class="k-step-list k-step-list-vertical"><li class="k-step k-step-first k-step-done k-step-success" style="max-height: 8.33333%;"><a href="#" class="k-step-link" title="Applicants" tabindex="-1" aria-invalid="true"><span class="k-step-indicator" aria-hidden="true"><span class="k-step-indicator-icon k-icon k-i-user"></span></span><span class="k-step-label"><span class="k-step-text">Applicants</span> <span class="k-icon k-i-warning" aria-hidden="true"></span></span></a><div id="appendedAppplicants">';
                $.each(response, function (index, item) {
                    //resultHtml += '<li>' + value + '</li>';
                    var fullApplicantName = item.fullname;
                    var url = $("#RedirectToApplicant").val() + "?application=" + $("#Application_NodeGUID").val() + "&&applicant=" + item.nodeGuid;

                    innerHtml = innerHtml + '<a class="appl-list" href="javascript:void(0);" onclick="showHideLoader();window.location.href=\'' + url + '\'"><span class="row viewall appl-item" style="margin-left: 12%;color: #27438c;">' + fullApplicantName.toUpperCase() + '</span></a>';
                });
                resultHtml = resultHtml + innerHtml + '</div></li></ol>';
                $('#ApplicationStepper').prepend(resultHtml);
                $('#ApplicationStepper > ol:nth-child(2)').addClass("ml-4");
                $('#ApplicationStepper > div').addClass("ml-4");
                //$('#result').html(resultHtml);
            }            
        },
        error: function (error) {
            console.log(error);
        }
    });
}