function GenerateSignatoryPersonValueList() {
    //debugger;
    var multiselect = $("#SignatoryPersonsList").data("kendoMultiSelect");
    var signatoryPersonname = "";
    var items = multiselect.value();
    for (var i = 0; i < items.length; i++) {
        signatoryPersonname = signatoryPersonname + items[i] + "|";
    }
    $("#SignatoryPersonsValueString").val(signatoryPersonname).change().focus().focusout();
}