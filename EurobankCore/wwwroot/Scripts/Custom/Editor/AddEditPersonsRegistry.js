$(document).ready(function () {
    $(".sidebarscroll").niceScroll({
        cursorborder: "",
        cursorcolor: "#0F0",
        cursoropacitymax: 0.7,
        boxzoom: false
    });
});
function SetWorkCode() {
    $("#WorkTelNoNumber").val("+" + $("#WorkTelNoCountryCode").val());
}
function SetMobileCode() {
    $("#MobileTelNoNumber").val("+" + $("#MobileTelNoCountryCode").val());
}
function SetFaxCode() {
    $("#FaxNoFaxNumber").val("+" + $("#FaxNoCountryCode").val());
}
$(document).ready(function () {
    var Type = $("#ApplicationTypeID").data("kendoDropDownList").text();
    if (Type == "INDIVIDUAL") {
        $("#PersonDetailsDIV").css("display", "block");
    }
    else if (Type == "LEGAL ENTITY") {
        $("#CompanyDetailsDIV").css("display", "block");
    }
});
function ShowRespectedDiv() {
    var Type = $("#ApplicationTypeID").data("kendoDropDownList").text();
    if (Type == "INDIVIDUAL") {
        $("#CompanyDetailsDIV").css("display", "none");
        $("#PersonDetailsDIV").css("display", "block");
    }
    else if (Type == "LEGAL ENTITY") {
        $("#PersonDetailsDIV").css("display", "none");
        $("#CompanyDetailsDIV").css("display", "block");
    }
}