var rowIdenItem;
var isApplicationPermisssionEdit = $("#ApplicationIsEdit").val();
function onChangeCompanyDetailsDOI(e) {
    $('#CompanyDetails_HdnDateofIncorporation').val($('#CompanyDetails_DateofIncorporation').val());
}
function onChangePersonalDetailsDOB(e) {
    $('#PersonalDetails_HdnDateOfBirth').val($('#PersonalDetails_DateOfBirth').val());
    validatePersonalDetails();
}
function SelectConfirmPersonIndYes() {
    personSearchGrid();
    var dialog = $("#ReSelectConfirmPersonInd").data("kendoWindow");
    dialog.close();
}
function SelectConfirmPersonIndNo() {
    var dialog = $("#ReSelectConfirmPersonInd").data("kendoWindow");
    dialog.close();
}
function OpenSelectConfirmPersonInd() {
    if ($("#ApplicantId").val() > 0) {
        var dialog = $("#ReSelectConfirmPersonInd").data("kendoWindow");
        var window = $("#DeleteWindow").data("kendoWindow");
        dialog.wrapper.addClass("middle-popup");
        dialog.center().open();
    }
    else {
        SelectConfirmPersonIndYes();
    }
}
function searchPersons() {
    var applicationTypevalue = $("#toolApplicationType").val();
    /*var referenceNumbervalue = $("#toolReferenceNumber").val();*/
    var fullNamevalue = $("#toolFullName").val();
    var identificationNumbervalue = $("#toolIdentificationNumber").val();
    var dateofBirthvalue = $("#toolDateofBirth").val();
    var issueDatevalue = $("#toolIssueDate").val();
    var citizenshipNamevalue = $("#toolCitizenshipName").val();
    grid = $("#RegPersons").data("kendoGrid");
    if (applicationTypevalue || fullNamevalue || identificationNumbervalue || dateofBirthvalue || issueDatevalue || citizenshipNamevalue) {
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
    var educationLevel = $("#PersonalDetails_EducationLevel").data("kendoDropDownList");
    if (educationLevel != undefined) {
        educationLevel.value(selectedItem.EducationLevel);
    }
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

        var preferredCommunicationLang = $("#ContactDetails_ContactDetails_PreferredCommunicationLanguage").data("kendoDropDownList");
        preferredCommunicationLang.value(selectedItem.PreferredCommunicationLanguage);
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
    var newRow = { CitizenshipValue: selectedItem.Citizenship, IdentificationDetails_TypeOfIdentification: selectedItem.TypeofIdentification, IdentificationDetails_IdentificationNumber: selectedItem.IdentificationNumber, CountryOfIssueValue: selectedItem.IssuingCountry, IdentificationDetails_IssueDate: selectedItem.IssueDateTime, IdentificationDetails_ExpiryDate: selectedItem.ExpiryDateTime, Status: false };
    var gridIdentification = $("#IdentificationDetails").data("kendoGrid");
    if (typeof gridIdentification != 'undefined') {
        var addedRow = gridIdentification.dataSource.add(newRow);
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
function clearContact() {

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

    var preferredCommunicationLang = $("#ContactDetails_ContactDetails_PreferredCommunicationLanguage").data("kendoDropDownList");
    preferredCommunicationLang.value('');
}

function clearPersonallDetailsIndividual() {
    var personallDetailsTitle = $("#PersonalDetails_Title").data("kendoDropDownList");
    personallDetailsTitle.value('');

    $('#PersonalDetails_FirstName').val('');
    $('#PersonalDetails_LastName').val('');
    $('#PersonalDetails_FathersName').val('');

    var personallDetailsGender = $("#PersonalDetails_Gender").data("kendoDropDownList");
    personallDetailsGender.value('');

    $('#PersonalDetails_DateOfBirth').val('');


    $('#PersonalDetails_PlaceOfBirth').val('');

    var personallDetailsContry = $("#PersonalDetails_CountryOfBirth").data("kendoDropDownList");
    personallDetailsContry.value('');


    var educationLevel = $("#PersonalDetails_EducationLevel").data("kendoDropDownList");
    if (educationLevel != undefined) {
        educationLevel.value('');
    }

    var gridIdentity = $("#IdentificationDetails").data("kendoGrid");
    id = gridIdentity.dataSource.total();
    if (id > 0) {
        gridIdentity.removeRow("tr:eq(" + id + ")");
    }
}

function showHideEmpStatusBasedBlock(e) {
    var specialEmpStatuses = $("#hdnSpecialEmpStatuses").val();
    var formerStatuses = $("#hdnFormerEmpStatuses").val();
    var formerStatusesArray = formerStatuses.split(',');
    var specialEmpStatusesArray = specialEmpStatuses.split(',');
    var dataItem = this.dataItem(e.item);
    if (dataItem.Value != null && dataItem.Value != undefined && dataItem.Value !== "") {
        if (specialEmpStatusesArray.indexOf(dataItem.Text) > -1) {
            $('.specialStatusBlock').hide();
        }
        else {
            $('.specialStatusBlock').show();
        }
        if (formerStatusesArray.indexOf(dataItem.Text) > -1) {
            $('.formarStatusBlock').show();
        }
        else {
            $('.formarStatusBlock').hide();
        }
    }
    else {
        $('.specialStatusBlock').hide();
        $('.formarStatusBlock').hide();
    }
}

function onChangeEmploymentStatus(e) {
    var selectedValue = $("#EmploymentStatus").data("kendoDropDownList").text();
    if (selectedValue == "HOMEMAKER" || selectedValue == "STUDENT/MILITARY SERVICES") {
        $("#DIVProfession").css("display", "none");
        $("#DIVFormerProfession").css("display", "none");
    }
    else if (selectedValue == "PUBLIC SECTOR EMPLOYEE" || selectedValue == "SEMI-GOVERNMENT SECTOR EMPLOYEE" || selectedValue == "PRIVATE SECTOR EMPLOYEE" || selectedValue == "SELF-EMPLOYED") {
        $("#DIVProfession").css("display", "block");
        $("#DIVFormerProfession").css("display", "none");
    }
    else if (selectedValue == "RETIRED" || selectedValue == "UNEMPLOYED") {
        $("#DIVProfession").css("display", "none");
        $("#DIVFormerProfession").css("display", "block");
    }
    validateBusinessAndFinancialProfile();
}
$(document).ready(function () {
    var selectedValue = $("#EmploymentStatusNameHdn").val();
    if (selectedValue != null && selectedValue != "") {
        if (selectedValue == "HOMEMAKER" || selectedValue == "STUDENT/MILITARY SERVICES") {
            $("#DIVProfession").css("display", "none");
            $("#DIVFormerProfession").css("display", "none");
        }
        else if (selectedValue == "PUBLIC SECTOR EMPLOYEE" || selectedValue == "SEMI-GOVERNMENT SECTOR EMPLOYEE" || selectedValue == "PRIVATE SECTOR EMPLOYEE" || selectedValue == "SELF-EMPLOYED") {
            $("#DIVProfession").css("display", "block");
            $("#DIVFormerProfession").css("display", "none");
        }
        else if (selectedValue == "RETIRED" || selectedValue == "UNEMPLOYED") {
            $("#DIVProfession").css("display", "none");
            $("#DIVFormerProfession").css("display", "block");
        }
    }
});
function onDataBound(e) {
    var specialEmpStatuses = $("#hdnSpecialEmpStatuses").val();
    var formerStatuses = $("#hdnFormerEmpStatuses").val();
    var formerStatusesArray = formerStatuses.split(',');
    var specialEmpStatusesArray = specialEmpStatuses.split(',');
    var dataItem = this.dataItem(e.item);
    if (dataItem.Value != null && dataItem.Value != undefined && dataItem.Value !== "") {
        if (specialEmpStatusesArray.indexOf(dataItem.Text) > -1) {
            $('.specialStatusBlock').hide();
        }
        else {
            $('.specialStatusBlock').show();
        }
        if (formerStatusesArray.indexOf(dataItem.Text) > -1) {
            $('.formarStatusBlock').show();
            $('.specialStatusBloMainBusinessActivitiesck').hide();
        }
        else {
            $('.formarStatusBlock').hide();
            $('.specialStatusBloMainBusinessActivitiesck').show();
        }
    }
    else {
        $('.specialStatusBlock').hide();
        $('.formarStatusBlock').hide();
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
            if (moduleName == 'Identification' || moduleName == 'Address' || moduleName == 'Tax') {
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
    var menuTitle = $("#hdnApplication_Applicant_LeftMenu_PEPDetails").val();
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
                        var steps = $("#ApplicationStepper").data('kendoStepper')._steps;

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
$("#accordionPersonDetails").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionPersonDetails").data("kendoPanelBar");
if ($("#Id").val() > 0) {
    accordion.collapse("#chartSection");
}
function validatePersonalDetails() {
    $.ajax({
        url: $("#ValidatePersonalDetailsUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#hdnApplication_Applicant_LeftMenu_PersonalDetails").val());
            }
            else {
                setMenuError($("#hdnApplication_Applicant_LeftMenu_PersonalDetails").val());
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
                setMenuSuccess($("#hdnApplication_Applicant_LeftMenu_CompanyDetails").val());
            }
            else {
                setMenuError($("#hdnApplication_Applicant_LeftMenu_CompanyDetails").val());
            }
        }
    });
}

function validateFatcaDetails() {
    $.ajax({
        url: $("#ValidateFatcaDetailsUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#hdnApplication_Applicant_LegalEntity_LeftMenu_FATCADetails").val());
            }
            else {
                setMenuError($("#hdnApplication_Applicant_LegalEntity_LeftMenu_FATCADetails").val());
            }
        }
    });
}

function validateCrsDetails() {
    //debugger;
    $.ajax({
        url: $("#ValidateCrsDetailsUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#hdnApplication_Applicant_LegalEntity_LeftMenu_CRSDetails").val());
            }
            else {
                setMenuError($("#hdnApplication_Applicant_LegalEntity_LeftMenu_CRSDetails").val());
            }
        }
    });
}

function validateBusinessProfileLegal() {
    $.ajax({
        url: $("#ValidateBusinessProfileLegalUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#hdnApplication_Applicant_LegalEntity_LeftMenu_BusinessProfile").val());
            }
            else {
                setMenuError($("#hdnApplication_Applicant_LegalEntity_LeftMenu_BusinessProfile").val());
            }
        }
    });
}

function validateFinancialProfileLegal() {
    //debugger;
    $.ajax({
        url: $("#ValidateFinancialProfileLegalUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#hdnApplication_Applicant_LeftMenu_FinancialProfile").val());
            }
            else {
                setMenuError($("#hdnApplication_Applicant_LeftMenu_FinancialProfile").val());
            }
        }
    });
}

function validateBankingRelationshipLegal() {
    $.ajax({
        url: $("#ValidateBankingRelationshipLegalUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#hdnApplication_Applicant_LeftMenu_BankingRelationship").val());
            }
            else {
                setMenuError($("#hdnApplication_Applicant_LeftMenu_BankingRelationship").val());
            }
        }
    });
}

function validateBankingRelationshipIndividual() {
    $.ajax({
        url: $("#ValidateBankingRelationshipIndividualUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#hdnApplication_Applicant_LeftMenu_BankingRelationship").val());
            }
            else {
                setMenuError($("#hdnApplication_Applicant_LeftMenu_BankingRelationship").val());
            }
        }
    });
}

function validateContactDetailsLegal() {
    $.ajax({
        url: $("#ValidateContactDetailsLegalUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            console.log(result);
            if (result.IsValid == true) {
                setMenuSuccess($("#hdnApplication_Applicant_LegalEntity_LeftMenu_ContactDetails").val());
            }
            else {
                setMenuError($("#hdnApplication_Applicant_LegalEntity_LeftMenu_ContactDetails").val());
            }
        }
    });
}

function validateContactDetailsIndividual() {
    $.ajax({
        url: $("#ValidateContactDetailsIndividualUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#hdnApplication_Applicant_LeftMenu_ContactDetails").val());
            }
            else {
                setMenuError($("#hdnApplication_Applicant_LeftMenu_ContactDetails").val());
            }
        }
    });
}
function validateBusinessAndFinancialProfile() {
    $.ajax({
        url: $("#ValidateBusinessAndFinancialProfileUrl").val(),
        cache: false,
        type: "POST",
        data: $('form').serialize(),
        success: function (result) {
            if (result.IsValid == true) {
                setMenuSuccess($("#hdnApplication_Applicant_LeftMenu_BusinessAndFinancialProfile").val());
            }
            else {
                setMenuError($("#hdnApplication_Applicant_LeftMenu_BusinessAndFinancialProfile").val());
            }
            
        }
    });
}
$("#accordionCompanyDetails").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionCompanyDetails").data("kendoPanelBar");
if ($("#Id").val() > 0) {
    if (accordion != undefined) {
        accordion.collapse("#chartSection");
    }
}
$("#accordionEmploymentDetails").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionEmploymentDetails").data("kendoPanelBar");
if (accordion != undefined) {
    accordion.collapse("#chartSection");
}
$("#accordionCompanyBusinessProfile").kendoPanelBar({
    expandMode: "multiple"
});

var accordion = $("#accordionCompanyBusinessProfile").data("kendoPanelBar");
if (accordion != undefined) {
    accordion.collapse("#chartSection");
}
$("#accordionFinancialDetails").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionFinancialDetails").data("kendoPanelBar");
if (accordion != undefined) {
    accordion.collapse("#chartSection");
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
    if ($("#ApplicantId").val() > 0) {
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
    $('#CompanyDetails_TradingName').val(selectedItem.TradingName);

    var entityValue = $("#EntityTypeID").data("kendoDropDownList");
    entityValue.value(selectedItem.EntityType);

    var countryOfCorporation = $("#CountryofIncorporationID").data("kendoDropDownList");
    countryOfCorporation.text(selectedItem.CountryofIncorporation);

    $('#CompanyDetails_RegistrationNumber').val(selectedItem.RegistrationNumber);
    $('#CompanyDetails_DateofIncorporation').val(selectedItem.DateofIncorporation);
    $('#CompanyDetails_HdnDateofIncorporation').val(selectedItem.DateofIncorporation);


    var listingStatus = $("#CompanyDetails_ListingStatus").data("kendoDropDownList");
    listingStatus.value(selectedItem.ListingStatus);

    var corporationSharesIssuedToTheBearer = $("#CompanyDetails_SharesIssuedToTheBearerName").data("kendoDropDownList");
    corporationSharesIssuedToTheBearer.value(selectedItem.CorporationSharesIssuedToTheBearer);
    var PreferredMailingAddress = $("#ContactDetailsLegal_ContactDetailsLegal_PreferredMailingAddress").data("kendoDropDownList");
    if (typeof PreferredMailingAddress !== 'undefined') {
        PreferredMailingAddress.value(selectedItem.ContactDetailsLegal_PreferredMailingAddress);
        $('#ContactDetailsLegal_EmailAddressForSendingAlerts').val(selectedItem.ContactDetailsLegal_EmailAddressForSendingAlerts);
        var preferredCommunicationLang = $("#ContactDetailsLegal_PreferredCommunicationLanguage").data("kendoDropDownList");
        preferredCommunicationLang.value(selectedItem.ContactDetailsLegal_PreferredCommunicationLanguage);
    }
    $("#CompanyPersonSearchWindow").data("kendoWindow").close();
    $("#divRegisteredPersonCompany").css("display", "none");
    $('#RegistryId').val(selectedItem.NodeID);
}
function clearCompanyContact() {
    var PreferredMailingAddress = $("#ContactDetailsLegal_ContactDetailsLegal_PreferredMailingAddress").data("kendoDropDownList");
    if (PreferredMailingAddress != undefined) {
        PreferredMailingAddress.value('');
    }
    $('#ContactDetailsLegal_EmailAddressForSendingAlerts').val('');
    var preferredCommunicationLang = $("#ContactDetailsLegal_PreferredCommunicationLanguage").data("kendoDropDownList");
    preferredCommunicationLang.value('');
    validateCompanyDetails();
}
function clearCompanyDetails() {
    $('#CompanyDetails_RegisteredName').val('');
    $('#CompanyDetails_TradingName').val('');

    var entityValue = $("#EntityTypeID").data("kendoDropDownList");
    if (entityValue != undefined) {
        entityValue.value('');
    }
    var countryOfCorporation = $("#CountryofIncorporationID").data("kendoDropDownList");
    if (countryOfCorporation != undefined) {
        countryOfCorporation.value('');
    }
    $('#CompanyDetails_RegistrationNumber').val('');
    $('#CompanyDetails_DateofIncorporation').val('');

    var listingStatus = $("#CompanyDetails_ListingStatus").data("kendoDropDownList");
    if (listingStatus != undefined) {
        listingStatus.value('');
    }
    var corporationSharesIssuedToTheBearer = $("#CompanyDetails_SharesIssuedToTheBearerName").data("kendoDropDownList");
    if (corporationSharesIssuedToTheBearer != undefined) {
        corporationSharesIssuedToTheBearer.value('');
    }
    var istheEntitylocatedandoperatesanofficeinCyprus = $("#CompanyDetails_IsOfficeinCyprusName").data("kendoDropDownList");
    if (istheEntitylocatedandoperatesanofficeinCyprus != undefined) {
        istheEntitylocatedandoperatesanofficeinCyprus.value('');
    }
}
function searchCompanyPersons() {
    //debugger;
    var applicationTypevalue = '';//$("#toolPersonType").val();
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
                { field: "PersonType", operator: "contains", value: applicationTypevalue },
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
$(document).ready(function () {
    if (isApplicationPermisssionEdit == "False") {
        setAllInputElementDisabled('app-body-per');
    }
});
function ShowRespectedFieldUsingEntityType() {
    //debugger;
    var selectedEntityValue = $("#EntityTypeID").data("kendoDropDownList") != undefined ? $("#EntityTypeID").data("kendoDropDownList").text() : '';

    if (selectedEntityValue == 'Provident Fund' || selectedEntityValue == 'Pension Fund') {
        $("#divSponsoringEntity").css("display", "block");
    }
    else {
        $("#divSponsoringEntity").css("display", "none");
    }
    if (selectedEntityValue == "Foundation" || selectedEntityValue == "Trade Union" || selectedEntityValue == "Club / Association" || selectedEntityValue == "City Council / Local Authority" || selectedEntityValue == "Government Organization"
        || selectedEntityValue == "Semi - Government Organization" || selectedEntityValue == "Trust" || selectedEntityValue == "Provident Fund" || selectedEntityValue == "Pension Fund" || selectedEntityValue == "General Partnership"
        || selectedEntityValue == "Limited Liability Partnership" || selectedEntityValue == "Limited Partnership") {
        $("#IsOfficeinCyprusNameDiv").css("display", "none");
        var officeInCyprus = $("#IsOfficeinCyprusNameID").data("kendoDropDownList");
        officeInCyprus.text('No');
        officeInCyprus.trigger('change');
    }
    else {
        $("#IsOfficeinCyprusNameDiv").css("display", "block");
    }
    var selectedCountryOfIncorporationValue = $("#CountryofIncorporationID").data("kendoDropDownList") != undefined ? $("#CountryofIncorporationID").data("kendoDropDownList").text() : '';

    if ((selectedEntityValue == "Private Limited Company" || selectedEntityValue == "Public Limited Company") && (selectedCountryOfIncorporationValue != "GREECE" && selectedCountryOfIncorporationValue != "CYPRUS")) {
        $("#SharesIssuedToTheBearerDIV").css("display", "block");
    }
    else {
        $("#SharesIssuedToTheBearerDIV").css("display", "none");
    }

    if (selectedEntityValue == 'Provident Fund' || selectedEntityValue == 'Pension Fund' || selectedEntityValue == 'Trust') {
        $('#divNumberofYearsinOperation').hide();
        $("#ListingStatusDIV").css("display", "none");
    }
    else {
        $('#divNumberofYearsinOperation').show();
        $("#ListingStatusDIV").css("display", "block");
    }

    if (selectedEntityValue == 'Trust') {
        $('#divNumberofEmployes').hide();
        $('#CompanyBusinessProfile_MainBusinessActivities').attr('title', $("#ResourceStringTitle_Trust").val());

    }
    else {
        $('#divNumberofEmployes').show();
        if ((selectedEntityValue == "Private Limited Company" || selectedEntityValue == "Public Limited Company")) {
            $('#CompanyBusinessProfile_MainBusinessActivities').attr('title', $("#ResourceStringTitle_All").val());
        }
        else {
            $('#CompanyBusinessProfile_MainBusinessActivities').attr('title', '');
        }
    }

    if (selectedEntityValue == 'Provident Fund' || selectedEntityValue == 'Pension Fund') {
        $('#divEconomicSectorIndustry').hide();
        $('#divTurnover').hide();
        $('#lblNumberofEmployes').text($("#ResourceStringLabel_NumberofEmployes2").val());
    }
    else {
        $('#divEconomicSectorIndustry').show();
        $('#divTurnover').show();
        $('#lblNumberofEmployes').text($("#ResourceStringLabel_NumberofEmployes").val());
    }

    if (selectedEntityValue == 'Provident Fund' || selectedEntityValue == 'Pension Fund') {
        $('#lblMainBusinessActivities').text($("#ResourceStringLabel_MainBusinessActivities3").val());
        $("#divSponsoringEntity").css("display", "block");
    }
    else if (selectedEntityValue == 'Trust') {
        $('#divEconomicSectorIndustry').hide();
        $('#lblMainBusinessActivities').text($("#ResourceStringLabel_MainBusinessActivities4").val());
        $('#lblTurnover').text('Total Income');
        $('#lblNetProfitLoss').text('Net Income/Loss');
    }
    else if (selectedEntityValue == 'Foundation' || selectedEntityValue == 'Trade Union' || selectedEntityValue == 'Club / Association' || selectedEntityValue == 'City Council / Local Authority' || selectedEntityValue == 'Government Organization'
        || selectedEntityValue == 'Semi - Government Organization') {
        $('#lblMainBusinessActivities').text($("#ResourceStringLabel_MainBusinessActivities2").val());
    }
    else {
        $('#lblMainBusinessActivities').text($("#ResourceStringLabel_MainBusinessActivities").val());
    }
    if (selectedEntityValue == 'Provident Fund' || selectedEntityValue == 'Pension Fund' || selectedEntityValue == 'Trust') {
        $('#divTradingName').hide();
        $('#divCorporationIsengaged').hide();
        $('#divWebsiteAddress').hide();
        $('#divIssuingAuthority').hide();
    }
    else {
        if (selectedEntityValue != "Private Insurance Company" && selectedEntityValue != "Public Insurance Company") {
            $('#lblCorporationIsengagedInTheProvisionName').text("Provision of Financial and Investment Services");
        }
        else {
            $('#lblCorporationIsengagedInTheProvisionName').text($("#ResourceStringLabel_CorporationIsengagedInTheProvisionName").val());
        }
        $('#divTradingName').show();
        $('#divCorporationIsengaged').show();
        $('#divWebsiteAddress').show();
    }
    ShowRespectedFieldUsingCorporationIsengaged();
}
function ShowRespectedFieldUsingCorporationIsengaged() {
    //debugger;
    var selectedEntityValue = $("#EntityTypeID").data("kendoDropDownList") != undefined ? $("#EntityTypeID").data("kendoDropDownList").text() : '';
    if (selectedEntityValue != "Private Insurance Company" && selectedEntityValue != "Public Insurance Company" && selectedEntityValue != 'Provident Fund' && selectedEntityValue != 'Pension Fund' && selectedEntityValue != 'Trust') {
        var selectedCorporationIsengaged = $("#CompanyBusinessProfile_CorporationIsengagedInTheProvisionName").data("kendoDropDownList") != undefined ? $("#CompanyBusinessProfile_CorporationIsengagedInTheProvisionName").data("kendoDropDownList").text() : '';
        if (selectedCorporationIsengaged == 'YES') {
            $('#divIssuingAuthority').show();
        }
        else {
            $('#divIssuingAuthority').hide();
        }
    }
    else {
        $('#divIssuingAuthority').hide();
    }
}
$(document).ready(function () {
    ShowRespectedFieldUsingEntityType();
    manageMenuPanel();
    ShowRespectedFieldUsingCorporationIsengaged();
    if ($("#IsLegalEntity").val() == 'False' && $('#accordionContactDetailsContentOne').length > 0) {
        ChangeMaxLengthIndividualFaxNo();
        ChangeMaxLengthIndividualWorkNo();
        ChangeMaxLengthIndividualMobileNo();
        ChangeMaxLengthIndividualHomeNo();
    }
    //$(document).ready(function () {
    //    $("#CompanyBusinessProfile_CountryofOriginofWealthActivitiesValues").kendoMultiSelect({
    //        change: function (e) {
    //            e.sender.wrapper.find('input').val("");
    //        },
    //    });
    //});

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
        var value = parseFloat($(this).val()).toFixed(2); // Convert to float with 2 decimal places
        $(this).val(formatNumber(value));
    });
});
function formatNumber(num) {
    return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function onSelectAddressType(e) {
    if (e.item) {
        var dataItem = this.dataItem(e.item);
        console.log(dataItem);
        var dropdownlist = $("#Country").data("kendoDropDownList");
        var dropdownlistPhone = $("#CountryCode_PhoneNo").data("kendoDropDownList");
        var dropdownlistFax = $("#CountryCode_FaxNo").data("kendoDropDownList");


        if (dataItem.Text == 'OFFICE IN CYPRUS') {
            dropdownlist.text("CYPRUS");
            dropdownlist.trigger("change");
            dropdownlist.focus();
            dropdownlist.readonly();
            if (dropdownlistPhone != undefined) {
                dropdownlistPhone.text("CYPRUS +357");
                dropdownlistPhone.trigger("change");
                dropdownlistPhone.focus();
                dropdownlistPhone.readonly();
            }

            if (dropdownlistPhone != undefined) {
                dropdownlistFax.text("CYPRUS +357");
                dropdownlistFax.trigger("change");
                dropdownlistFax.focus();
                dropdownlistFax.readonly();
            }
            $('#AddressLine1').focus();
            $('#AddressLine1').focusout();
        }
        else {
            dropdownlist.text("-Select-");
            dropdownlist.trigger("change");
            dropdownlist.readonly(false);
            dropdownlist.focus();
            if (dropdownlistPhone != undefined) {
                dropdownlistPhone.readonly(false);
            }

            if (dropdownlistFax != undefined) {
                dropdownlistFax.readonly(false);
            }

        }
    }
}

function manageMenuPanel() {
    var isLegalEntity = $("#IsLegalEntity").val();
    if (isLegalEntity == 'True') {
        manageLegalApplicantMenuPanel();
    }
    else {
        manageIndividualApplicantMenuPanel();
    }
}
function onSelectFATCAClassification(e) {
    if (e.item) {
        var dataItem = this.dataItem(e.item);
        changeClassificationTypes(dataItem.Text);
        var seletedType = $("#FATCACRSDetails_FATCADetails_TypeofForeignFinancialInstitution").data("kendoDropDownList").text();
        var selectedNoNForgien = $("#FATCACRSDetails_FATCADetails_TypeofNonFinancialForeignEntity").data("kendoDropDownList").text();
        changeGIINExemp(seletedType, selectedNoNForgien);
    } else {

    }
}

function onSelectForeignFinancialIns(e) {
    if (e.item) {
        var dataItem = this.dataItem(e.item);
        var selectedNoNForgien = $("#FATCACRSDetails_FATCADetails_TypeofNonFinancialForeignEntity").data("kendoDropDownList").text();
        changeGIINExemp(dataItem.Text, selectedNoNForgien);
    } else {

    }
}

function onSelectNonForeignFinancialIns(e) {
    if (e.item) {
        var dataItem = this.dataItem(e.item);
        var seletedType = $("#FATCACRSDetails_FATCADetails_TypeofForeignFinancialInstitution").data("kendoDropDownList").text();

        changeGIINExemp(seletedType, dataItem.Text);
    } else {

    }
}

function onDataBoundFATCAClassi() {
    changeClassificationTypes($("#FATCACRSDetails_FATCADetails_FATCAClassification").data("kendoDropDownList").text());
}

function onDataBoundForeignFinan(seletedType, selectedNoNForgien) {
    var seletedType = $("#FATCACRSDetails_FATCADetails_TypeofForeignFinancialInstitution").data("kendoDropDownList").text();
    var selectedNoNForgien = $("#FATCACRSDetails_FATCADetails_TypeofNonFinancialForeignEntity").data("kendoDropDownList").text();
    changeGIINExemp(seletedType, selectedNoNForgien);
}

function onDataBoundNonForeignFinan(seletedType, selectedNoNForgien) {
    var seletedType = $("#FATCACRSDetails_FATCADetails_TypeofForeignFinancialInstitution").data("kendoDropDownList").text();
    var selectedNoNForgien = $("#FATCACRSDetails_FATCADetails_TypeofNonFinancialForeignEntity").data("kendoDropDownList").text();
    changeGIINExemp(seletedType, selectedNoNForgien);
}

function changeClassificationTypes(seletedType) {
    if (seletedType == 'US Entity') {
        $('#divFatcaUSForeignFinancialType').hide();
        $('#divFatcaNonForeignFinancialType').hide();

        $("#FATCACRSDetails_FATCADetails_TypeofForeignFinancialInstitution").data("kendoDropDownList").value('');
        $("#FATCACRSDetails_FATCADetails_TypeofNonFinancialForeignEntity").data("kendoDropDownList").value('');
        $('#divFatcaUSEntityType').show();
    }
    else if (seletedType == 'Foreign Financial Institution') {
        $('#divFatcaUSEntityType').hide();
        $('#divFatcaNonForeignFinancialType').hide();

        $("#FATCACRSDetails_FATCADetails_USEtityType").data("kendoDropDownList").value('');
        $("#FATCACRSDetails_FATCADetails_TypeofNonFinancialForeignEntity").data("kendoDropDownList").value('');
        $('#divFatcaUSForeignFinancialType').show();
    }
    else if (seletedType == 'Non-Financial Foreign Entity (NFFE)') {
        $('#divFatcaUSEntityType').hide();
        $('#divFatcaUSForeignFinancialType').hide();

        $("#FATCACRSDetails_FATCADetails_USEtityType").data("kendoDropDownList").value('');
        $("#FATCACRSDetails_FATCADetails_TypeofForeignFinancialInstitution").data("kendoDropDownList").value('');
        $('#divFatcaNonForeignFinancialType').show();
    }
    else if (seletedType == '- Select - ') {
        $('#divFatcaUSEntityType').hide();
        $('#divFatcaUSForeignFinancialType').hide();
        $('#divFatcaNonForeignFinancialType').hide();

        $("#FATCACRSDetails_FATCADetails_USEtityType").data("kendoDropDownList").value('');
        $("#FATCACRSDetails_FATCADetails_TypeofForeignFinancialInstitution").data("kendoDropDownList").value('');
        $("#FATCACRSDetails_FATCADetails_TypeofNonFinancialForeignEntity").data("kendoDropDownList").value('');

    }
}

function changeGIINExemp(seletedType, selectedNoNForgien) {
    //debugger;
    if (seletedType == 'Participating Foreign Financial Institution (PFFI)' || seletedType == 'Registered Deemed Compliant Financial Institution' || seletedType == 'Sponsored Financial Institution' || selectedNoNForgien == 'Direct reporting Passive NFFE' || selectedNoNForgien == 'Sponsored direct reporting Passive NFFE' || selectedNoNForgien == 'Sponsored direct reporting NFFE') {
        $('#divGIINNumber').show();
    }
    else {
        $('#divGIINNumber').hide();
        $("#FATCACRSDetails_FATCADetails_GlobalIntermediaryIdentificationNumber").val('');
    }
}

function ShowHideFieldsOnAddressType() {
    //debugger;
    var selectedAddressType = $("#AddressType").data("kendoDropDownList");
    if (selectedAddressType != undefined) {
        if (selectedAddressType.text().trim() == 'MAILING ADDRESS') {
            $('#divAddPOBox').show();
        }
        else {
            $("#POBox").val('').trigger('change').focusin().focusout();
            $('#divAddPOBox').hide();
        }
        if (selectedAddressType.text().trim() != 'MAILING ADDRESS') {
            $('#divMailingAddressSame').show();
        }
        else {
            $("#MailingAddressSame").prop("checked", false);
            $('#divMailingAddressSame').hide();
        }
        if (selectedAddressType.text().trim() == '-Select-') {
            $("#MailingAddressSame").prop("checked", false);
            $('#divMailingAddressSame').hide();
        }
    }
}

function ShowRespectedDiv() {
    if ($("#IsRelatedParty").val() == "False") {
        var selectValue = $("#AddressType").data("kendoDropDownList").text();
        if (selectValue == "OFFICE IN CYPRUS") {
            $("#PhoneNoDIV").css("display", "block")
            $("#FaxNoDIV").css("display", "block")
            $("#EmailDIV").css("display", "block")
            $("#NumberOfStaffEmployedDIV").css("display", "block")
        }
        else if (selectValue == "PRINCIPAL TRADING /BUSINESS OFFICE" || selectValue == "BUSINESS OFFICE" || selectValue == "ADMINISTRATION OFFICE") {
            $("#PhoneNoDIV").css("display", "block")
            $("#FaxNoDIV").css("display", "block")
            $("#EmailDIV").css("display", "block")
            $("#NumberOfStaffEmployedDIV").css("display", "none")
        }
        else {
            $("#PhoneNoDIV").css("display", "none")
            $("#FaxNoDIV").css("display", "none")
            $("#EmailDIV").css("display", "none")
            $("#NumberOfStaffEmployedDIV").css("display", "none")
        }
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
    $(this).val($(this).val().replace(/[^\d.-]/g, ''));
});
$(document).on("input", ".custom-decimal-positive", function () {
    $(this).val($(this).val().replace(/[^\d.]/g, ''));
});

$(document).on("input", ".custom-negetive-decimal", function () {
    this.value = this.value.replace(/[^\d.-]/g, '');
});
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

function ChangeMaxLengthLegalAddressPhone() {
    var selectedPhoneNo = $("#CountryCode_PhoneNo").data("kendoDropDownList") != undefined ? $("#CountryCode_PhoneNo").data("kendoDropDownList").text() : '';
    field = document.getElementById('PhoneNo');

    if (selectedPhoneNo == 'CYPRUS +357') {
        field.value = field.value.substr(0, 8);
    }
    $("#PhoneNo").keyup();
}

function ChangeMaxLengthLegalAddressFax() {
    var selectedPhoneNo = $("#CountryCode_FaxNo").data("kendoDropDownList") != undefined ? $("#CountryCode_FaxNo").data("kendoDropDownList").text() : '';
    field = document.getElementById('FaxNo');

    if (selectedPhoneNo == 'CYPRUS +357') {
        field.value = field.value.substr(0, 8);
    }
    $("#FaxNo").keyup();
}

function limitlengthAccToCountry(obj, spanid, ddlid) {
    var selectedCountryNo = $('#' + ddlid).data("kendoDropDownList") != undefined ? $('#' + ddlid).data("kendoDropDownList").text() : '';
    if (selectedCountryNo == 'CYPRUS +357') {
        var maxlength = 8;
        var errormsg = 'In case of Cyprus Maximum allowed length is ' + maxlength;
    }
    else {
        var maxlength = 14;
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
function ValidateMenuTaxDetailsLegal() {
    var selectedAddressType = $("#CompanyDetails_IsLiableToPayDefenseTaxInCyprusName").data("kendoDropDownList");
    if (selectedAddressType != undefined) {
        if (selectedAddressType.text().toUpperCase() == 'YES' || selectedAddressType.text().toUpperCase() == 'NO') {
            isAllConfirmed($("#hdnApplicant_LeftMenu_TaxDetails").val(), "Tax", "TaxDetails");
        }
        else {
            setMenuError($("#hdnApplicant_LeftMenu_TaxDetails").val());
        }
    }
    var selectedAddressTypeInd = $("#PersonalDetails_IsLiableToPayDefenseTaxInCyprusName").data("kendoDropDownList");
    if (selectedAddressTypeInd != undefined) {
        if (selectedAddressTypeInd.text().toUpperCase() == 'YES' || selectedAddressTypeInd.text().toUpperCase() == 'NO') {
            isAllConfirmed($("#hdnApplicant_LeftMenu_TaxDetails").val(), "Tax", "TaxDetails");
        }
        else {
            setMenuError($("#hdnApplicant_LeftMenu_TaxDetails").val());
        }
    }
}
window.addEventListener("load", function () {
    window.setTimeout(ValidateMenuTaxDetailsLegal, 800);
}, false);
function TurnoverValidateRegex(obj) {
    //debugger;
    //var txtval = document.getElementById("CompanyFinancialInformation_Turnover").value;
    var lblError = document.getElementById("CompanyFinancialInformationTurnoverError");
    lblError.innerHTML = "";
    var expr = /^\d{0,17}(\.\d{1,2})?$/;
    //var texboxValue = Math.trunc(obj.value.replace(/,/g, ''));
    var texboxValue = obj.value.replace(/,/g, '');
    //if (texboxValue > 0) {
    if (!(texboxValue == "" || texboxValue == 'NaN')) {
        if (expr.test(texboxValue.toString())) {
            lblError.innerHTML = "";  // Reset the error message if the input is valid
        } else {
            // Check for specific conditions and set appropriate error messages
            if (texboxValue.toString().charAt(0) === '-') {
                lblError.innerHTML = "Negative value is not allowed.";
            } else {
                // Check for the condition of only two digits after the decimal point
                var decimalIndex = texboxValue.toString().indexOf(".");
                if (decimalIndex !== -1 && texboxValue.toString().substring(decimalIndex + 1).length > 2) {
                    lblError.innerHTML = "Only two digits are allowed after the decimal point.";
                } else {
                    //lblError.innerHTML = "Please enter a valid amount (up to 17 digits with up to 2 decimal places).";
                    lblError.innerHTML = "Maximum allowed length is 17.";
                    obj.value = texboxValue.toString().substring(0, 17);
                }
            }
        }
    }
    else {
        $("#CompanyFinancialInformation_Turnover").val("");
    }
}

//function NetProfitLossValidateRegex(obj) {
//    debugger;
//    //var txtval = document.getElementById("CompanyFinancialInformation_NetProfitLoss").value;
//    var lblError = document.getElementById("CompanyFinancialInformationNetProfitLossError");
//    lblError.innerHTML = "";
//    var expr = /^-?\d{0,17}(\.\d{1,2})?$/;
//    var texboxValue = Math.trunc(obj.value.replace(/,/g, ''));
//    if (texboxValue >0) {
//        if (expr.test(texboxValue.toString())) {
//            lblError.innerHTML = "";  // Reset the error message if the input is valid
//        } else {
//            // Check for specific conditions and set appropriate error messages
//            // Check for the condition of only two digits after the decimal point
//            var decimalIndex = texboxValue.toString().indexOf(".");
//            if (decimalIndex !== -1 && texboxValue.toString().substring(decimalIndex + 1).length > 2) {
//                lblError.innerHTML = "Only two digits are allowed after the decimal point.";
//            } else {
//                //lblError.innerHTML = "Please enter a valid amount (up to 17 digits with up to 2 decimal places)";
//                lblError.innerHTML = "Maximum allowed length is 17.";
//                obj.value = texboxValue.toString().substring(0, 17);
//            }
//        }
//    }
//    else {
//        $("#CompanyFinancialInformation_NetProfitLoss").val("");
//    }
//}
function NetProfitLossValidateRegex(obj) {
    var lblError = document.getElementById("CompanyFinancialInformationNetProfitLossError");
    lblError.innerHTML = "";
    var expr = /^-?\d{0,17}(\.\d{1,2})?$/;
    var texboxValue = obj.value.replace(/,/g, ''); // Remove commas
    if (!(texboxValue == "" || texboxValue == 'NaN')) {
        if (expr.test(texboxValue)) {
            lblError.innerHTML = "";  // Reset the error message if the input is valid
        } else {
            // Check for specific conditions and set appropriate error messages        
            // Check for the condition of only two digits after the decimal point
            var decimalIndex = texboxValue.indexOf(".");
            if (decimalIndex !== -1 && texboxValue.substring(decimalIndex + 1).length > 2) {
                lblError.innerHTML = "Only two digits are allowed after the decimal point.";
            } else {
                lblError.innerHTML = "Maximum allowed length is 17.";
                obj.value = texboxValue.substring(0, 17);
            }
        }
    } else {
        $("#CompanyFinancialInformation_NetProfitLoss").val(""); // Clear textbox if input is empty
    }
}

function TotalAssetsValidateRegex(obj) {
    //var txtval = document.getElementById("CompanyFinancialInformation_TotalAssets").value;
    var lblError = document.getElementById("CompanyFinancialInformationTotalAssetsError");
    lblError.innerHTML = "";
    var expr = /^\d{0,17}(\.\d{1,2})?$/;
    //var texboxValue = Math.trunc(obj.value.replace(/,/g, ''));
    var texboxValue = obj.value.replace(/,/g, '');
    //if (texboxValue >0) {
    if (!(texboxValue == "" || texboxValue == 'NaN')) {
        if (expr.test(texboxValue.toString())) {
            lblError.innerHTML = "";  // Reset the error message if the input is valid
        } else {
            // Check for specific conditions and set appropriate error messages
            if (texboxValue.toString().charAt(0) === '-') {
                lblError.innerHTML = "Negative value is not allowed.";
            } else {
                // Check for the condition of only two digits after the decimal point
                var decimalIndex = texboxValue.toString().indexOf(".");
                if (decimalIndex !== -1 && texboxValue.toString().substring(decimalIndex + 1).length > 2) {
                    lblError.innerHTML = "Only two digits are allowed after the decimal point.";
                } else {
                    //lblError.innerHTML = "Please enter a valid amount (up to 17 digits with up to 2 decimal places).";
                    lblError.innerHTML = "Maximum allowed length is 17.";
                    obj.value = texboxValue.toString().substring(0, 17);
                }
            }
        }
    }
    else {
        $("#CompanyFinancialInformation_TotalAssets").val("");
    }
}

function OriginOfAnnualIncomeAmountValidateRegex() {
    var txtval = document.getElementById("AmountOfIncome").value;
    //var lblError = document.getElementById("IndividualAmountOfIncomeError");
    var lblError = document.getElementById("AmountOfIncome_validationMessage");
    lblError.innerHTML = "";
    var expr = /^\d{0,16}(\.\d{1,2})?$/;
    if (expr.test(txtval)) {
        lblError.innerHTML = "";  // Reset the error message if the input is valid
    } else {
        // Check for specific conditions and set appropriate error messages
        if (txtval.charAt(0) === '-') {
            lblError.innerHTML = "Negative value is not allowed.";
        } else {
            // Check for the condition of only two digits after the decimal point
            var decimalIndex = txtval.indexOf(".");
            if (decimalIndex !== -1 && txtval.substring(decimalIndex + 1).length > 2) {
                lblError.innerHTML = "Only two digits are allowed after the decimal point.";
            } else {
                //lblError.innerHTML = "Please enter a valid amount (up to 17 digits with up to 2 decimal places).";
                lblError.innerHTML = "Maximum allowed length of the input text is 16.";
            }
        }
    }
}

//for Loader
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
//$("#btnFullSave").click(function () {
$(".btnSaveAsDraft").click(function () {
    if (CheckNetProfitLossValidate() && CheckTurnoverValidate() && CheckTotalAssetsValidate()) {
        showHideLoader();
    }
    else {
        HideLoader();
    }

})
$("#btnFullConfirm").click(function () {
    if (CheckNetProfitLossValidate() && CheckTurnoverValidate() && CheckTotalAssetsValidate()) {
        showHideLoader();
    }
    else {
        HideLoader();
    }
})
$("#btnApplicantClose").click(function () {
    showHideLoader();
})
//logo
$(".push-hide").click(function () {
    showHideLoader();
})
function HideLoader() {
    var loader = $("#loader").data("kendoLoader");
    loader.hide();
}
function CheckNetProfitLossValidate() {
    //debugger;
    var lblError = document.getElementById("CompanyFinancialInformationNetProfitLossError");
    if (lblError != undefined && lblError != null) {
        if (lblError.innerHTML == "") {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        return true;
    }
}
function CheckTurnoverValidate() {
    var lblError = document.getElementById("CompanyFinancialInformationTurnoverError");
    if (lblError != undefined && lblError != null) {
        if (lblError.innerHTML == "") {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        return true;
    }
}
function CheckTotalAssetsValidate() {
    //debugger;
    var lblError = document.getElementById("CompanyFinancialInformationTotalAssetsError");
    if (lblError != undefined && lblError != null) {
        if (lblError.innerHTML == "") {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        return true;
    }
}
function ValidateApplicant() {
    //debugger;
    var islegal = $("#IsLegalEntity").val();
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
            $("#PersonalDetailsFirstNameError").text("Please enter First Name");
            flag = 1;
        }
        else {
            $("#PersonalDetailsFirstNameError").text("");
        }
        if ($("#PersonalDetails_LastName").val() == "") {
            $("#PersonalDetailsLastNameError").text("Please enter Last Name");
            flag = 1;
        }
        else {
            $("#PersonalDetailsLastNameError").text("");
        }
    }
    if (flag == 1) {
        return false;
    }
    else {
        showHideLoader();
        return true;
    }
}

function isNumberWithoutDecimalKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function showhideFieldofBankingRelationshipIndividual() {
    var selectedValue = $("#PersonalDetails_HasAccountInOtherBankName").data("kendoDropDownList").text();
    if (selectedValue == "YES") {
        $("#divNameOfBankingInstitution").css("display", "block");
        $("#divCountryOfBankingInstitution").css("display", "block");
    }
    else {
        $("#divNameOfBankingInstitution").css("display", "none");
        $("#divCountryOfBankingInstitution").css("display", "none");
        $("#PersonalDetails_NameOfBankingInstitution").val("");
        var dropdownlistTitle = $("#PersonalDetails_CountryOfBankingInstitution").data("kendoDropDownList");
        dropdownlistTitle.value('');
    }
    validateBankingRelationshipIndividual();
}
function onDataBoundBankingRelationshipIndividual() {
    showhideFieldofBankingRelationship();
}

function showhideFieldofBankingRelationshipLegal() {
    var selectedValue = $("#CompanyDetails_HasAccountInOtherBankName").data("kendoDropDownList").text();
    if (selectedValue == "YES") {
        $("#divCompanyDetailsNameOfBankingInstitution").css("display", "block");
        $("#divCompanyDetailsCountryOfBankingInstitution").css("display", "block");
    }
    else {
        $("#divCompanyDetailsNameOfBankingInstitution").css("display", "none");
        $("#divCompanyDetailsCountryOfBankingInstitution").css("display", "none");
        $("#CompanyDetails_NameOfBankingInstitution").val("");
        var dropdownlistTitle = $("#CompanyDetails_CountryOfBankingInstitution").data("kendoDropDownList");
        dropdownlistTitle.value('');
    }
    validateBankingRelationshipLegal();
}
function onDataBoundBankingRelationshipLegal() {
    showhideFieldofBankingRelationshipLegal();
}