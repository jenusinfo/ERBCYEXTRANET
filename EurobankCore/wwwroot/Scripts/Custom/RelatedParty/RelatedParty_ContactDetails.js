$("#accordionContactDetails").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionContactDetails").data("kendoPanelBar");
accordion.collapse("#chartSection");
function SuccessPopup() {
    var window = $("#SuccessWindow").data("kendoWindow");
    window.content(windowTemplate(data));
    window.wrapper.addClass("middle-popup");
    window.center().open();
    $("#okButton").click(function () {
        window.close();
    })
}
function SetHomeCode() {
    $("#ContactDetails_HomeTelNoNumber").val("+" + $("#Country_Code_HomeTelNoNumber").val());
}
function SetMobileCode() {
    $("#ContactDetails_MobileTelNoNumber").val("+" + $("#Country_Code_MobileTelNoNumber").val());
}
function SetWorkCode() {
    $("#ContactDetails_WorkTelNoNumber").val("+" + $("#Country_Code_WorkTelNoNumber").val());
}
function SetFaxCode() {
    $("#ContactDetails_FaxNoFaxNumber").val("+" + $("#Country_Code_FaxNoFaxNumber").val());
}
function ChangeMaxLengthIndividualHomeNo() {
    var selectedPhoneNo = $("#Country_Code_HomeTelNoNumber").data("kendoDropDownList") != undefined ? $("#Country_Code_HomeTelNoNumber").data("kendoDropDownList").text() : '';
    field = document.getElementById('ContactDetails_HomeTelNoNumber');

    if (selectedPhoneNo == 'CYPRUS +357') {
        field.value = field.value.substr(0, 8);
        //field.maxLength = 8;
    } else {
        field.value = field.value.substr(0, 14);
        //field.maxLength = 10;
    }
}
function contactsMaxLengthWarningHome(e) {
    var selectedPhoneNo = $("#Country_Code_HomeTelNoNumber").data("kendoDropDownList") != undefined ? $("#Country_Code_HomeTelNoNumber").data("kendoDropDownList").text() : '';
    field = document.getElementById('ContactDetails_HomeTelNoNumber');

    if (selectedPhoneNo == 'CYPRUS +357') {
        limitlength(e, 8, 'ContactDetails_HomeTelNumberError')
    } else {
        limitlength(e, 14, 'ContactDetails_HomeTelNumberError')
    }
}
function ChangeMaxLengthIndividualMobileNo() {
    var selectedPhoneNo = $("#Country_Code_MobileTelNoNumber").data("kendoDropDownList") != undefined ? $("#Country_Code_MobileTelNoNumber").data("kendoDropDownList").text() : '';
    field = document.getElementById('ContactDetails_MobileTelNoNumber');

    if (selectedPhoneNo == 'CYPRUS +357') {
        field.value = field.value.substr(0, 8);
        //field.maxLength = 8;
    } else {
        field.value = field.value.substr(0, 14);
        //field.maxLength = 10;
    }
}
function contactsMaxLengthWarningMobile(e) {
    //debugger;
    var selectedPhoneNo = $("#Country_Code_MobileTelNoNumber").data("kendoDropDownList") != undefined ? $("#Country_Code_MobileTelNoNumber").data("kendoDropDownList").text() : '';
    field = document.getElementById('ContactDetails_MobileTelNoNumber');

    if (selectedPhoneNo == 'CYPRUS +357') {
        limitlength(e, 8, 'ContactDetails_MobileTelNumberError');
    } else {
        limitlength(e, 14, 'ContactDetails_MobileTelNumberError');
    }
}
function ChangeMaxLengthIndividualWorkNo() {
    var selectedPhoneNo = $("#Country_Code_WorkTelNoNumber").data("kendoDropDownList") != undefined ? $("#Country_Code_WorkTelNoNumber").data("kendoDropDownList").text() : '';
    field = document.getElementById('ContactDetails_WorkTelNoNumber');

    if (selectedPhoneNo == 'CYPRUS +357') {
        field.value = field.value.substr(0, 8);
        //field.maxLength = 8;
    } else {
        field.value = field.value.substr(0, 14);
        //field.maxLength = 10;
    }
}
function contactsMaxLengthWarningWork(e) {
    //debugger;
    var selectedPhoneNo = $("#Country_Code_WorkTelNoNumber").data("kendoDropDownList") != undefined ? $("#Country_Code_WorkTelNoNumber").data("kendoDropDownList").text() : '';
    field = document.getElementById('ContactDetails_WorkTelNoNumber');

    if (selectedPhoneNo == 'CYPRUS +357') {
        limitlength(e, 8, 'ContactDetails_WorkTelNumberError');
    } else {
        limitlength(e, 14, 'ContactDetails_WorkTelNumberError');
    }
}
function ChangeMaxLengthIndividualFaxNo() {
    var selectedPhoneNo = $("#Country_Code_FaxNoFaxNumber").data("kendoDropDownList") != undefined ? $("#Country_Code_FaxNoFaxNumber").data("kendoDropDownList").text() : '';
    field = document.getElementById('ContactDetails_FaxNoFaxNumber');

    if (selectedPhoneNo == 'CYPRUS +357') {
        field.value = field.value.substr(0, 8);
        //field.maxLength = 8;
    } else {
        field.value = field.value.substr(0, 14);
        //field.maxLength = 10;
    }
}
function contactsMaxLengthWarningFax(e) {
    //debugger;
    var selectedPhoneNo = $("#Country_Code_FaxNoFaxNumber").data("kendoDropDownList") != undefined ? $("#Country_Code_FaxNoFaxNumber").data("kendoDropDownList").text() : '';
    field = document.getElementById('ContactDetails_FaxNoFaxNumber');

    if (selectedPhoneNo == 'CYPRUS +357') {
        limitlength(e, 8, 'ContactDetails_FaxNumberError');
    } else {
        limitlength(e, 14, 'ContactDetails_FaxNumberError');
    }
}
function isNumberWithoutDecimalKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;

    return true;
}