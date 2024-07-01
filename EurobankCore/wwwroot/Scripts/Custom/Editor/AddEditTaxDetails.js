function ShowTaxDetailsRespectedField() {
    var selectReasonName = $("#TaxDetails_TinUnavailableReason").data("kendoDropDownList").text();
    if (selectReasonName == "CUSTOMER IS UNABLE TO OBTAIN TAX NUMBER") {
        $("#JustificationForTinUnavalabilityID").css("display", "block");
    }
    else {
        $("#JustificationForTinUnavalabilityID").css("display", "none");
    }
}