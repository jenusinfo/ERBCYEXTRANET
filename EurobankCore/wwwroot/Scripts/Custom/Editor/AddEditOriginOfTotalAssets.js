function ShowSpecifyOtherOrigin() {
    //debugger;
    var assets = $("#OriginOfTotalAssetsID").data("kendoDropDownList").text();
    if (assets == "OTHER") {
        $("#SpecifyOtherOriginDIV").css("display", "block");
    }
    else {
        $("#SpecifyOtherOriginDIV").css("display", "none");
        $("#SpecifyOtherOrigin").val('').change().focus().focusout();
    }
}