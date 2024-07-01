$("#SaveInRegistry").click(function () {
    if (this.checked) {
        $("#LocationID").css("display", "block");
        $("#LocationName").focus();
    }
    else {
        $("#LocationID").css("display", "none");
    }
});
function searchAddresses() {
    //debugger;
    var value = $("#toolAddressLine1").val();
    var addressLine2value = $("#toolAddressLine2").val();
    var postalCodevalue = $("#toolPostalCode").val();
    var cityvalue = $("#toolCity").val();
    var poboxvalue = $("#toolPOBox").val();
    var countryNamevalue = $("#toolCountryName").val();
    var locationName = $("#toolLocationName").val();
    grid = $("#RegAddressDetails").data("kendoGrid");

    if (value || addressLine2value || postalCodevalue || cityvalue || poboxvalue || countryNamevalue || locationName) {
        console.log('in');
        grid.dataSource.filter({
            logic: "and",
            filters: [
                { field: "LocationName", operator: "contains", value: locationName },
                { field: "AddresLine1", operator: "contains", value: value },
                { field: "AddresLine2", operator: "contains", value: addressLine2value },
                { field: "PostalCode", operator: "contains", value: postalCodevalue },
                { field: "POBox", operator: "contains", value: poboxvalue },
                { field: "City", operator: "contains", value: cityvalue },
                { field: "CountryName", operator: "contains", value: countryNamevalue },
            ]
        });
    } else {
        grid.dataSource.filter({});
    }
}
$("#btnAddressSelect").click(function () {
    var grid = $("#RegAddressDetails").data("kendoGrid");
    var selectedItem = grid.dataItem(grid.select());
    $("#AddressLine1").val(selectedItem.AddresLine1.toUpperCase());
    $("#AddressLine1").trigger("change");
    $('#AddressLine2').val(selectedItem.AddresLine2.toUpperCase());
    $('#POBox').val(selectedItem.POBox.toUpperCase()).change();
    $('#PostalCode').val(selectedItem.PostalCode.toUpperCase());
    $('#City').val(selectedItem.City.toUpperCase());
    $('#LocationName').val(selectedItem.LocationName.toUpperCase());
    $("#LocationName").trigger("change");
    $("#LocationName").focusout();
    console.log(selectedItem.Country);
    var dropdownlist = $("#Country").data("kendoDropDownList");
    dropdownlist.value(selectedItem.Country);

    var PhoneCountryCodedropdownlist = $("#CountryCode_PhoneNo").data("kendoDropDownList");
    PhoneCountryCodedropdownlist.value(selectedItem.CountryCode_PhoneNo);
    PhoneCountryCodedropdownlist.trigger("change");
    $("#PhoneNo").val(selectedItem.PhoneNo);
    $("#PhoneNo").trigger("change");

    var FaxCountryCodedropdownlist = $("#CountryCode_FaxNo").data("kendoDropDownList");
    FaxCountryCodedropdownlist.value(selectedItem.CountryCode_FaxNo);
    FaxCountryCodedropdownlist.trigger("change");
    $("#FaxNo").val(selectedItem.FaxNo);
    $("#FaxNo").trigger("change");

    $("#SearchWindow").data("kendoWindow").close();
    $("#divRegisteredAddresss").css("display", "none");
    $('#AddressRegistryId').val(selectedItem.NodeID);
    //if (!$("#SaveInRegistry").prop("checked")) {
    //    $("#SaveInRegistry").trigger("click");
    //}
});

$("#btnAddressCancel").click(function () {
    $("#SearchWindow").data("kendoWindow").close();
    $("#divRegisteredAddresss").css("display", "none");
});

$("#btnSearch").click(function () {
    $("#RegAddressDetails").data("kendoGrid").dataSource.read();
    var searchWindow = $("#SearchWindow").data("kendoWindow");
    searchWindow.wrapper.addClass("middle-popup");
    searchWindow.center();
    searchWindow.open();
    $("#divRegisteredAddresss").css("display", "block");
});
$("#btnClear").click(function () {
    $('#AddressLine1').val("");
    $('#AddressLine2').val("");
    $('#POBox').val("");
    $('#PostalCode').val("");
    $('#City').val("");
    $('#LocationName').val('');
    var dropdownlist = $("#Country").data("kendoDropDownList");
    dropdownlist.value("0");

    var PhoneCountryCodedropdownlist = $("#CountryCode_PhoneNo").data("kendoDropDownList");
    if (PhoneCountryCodedropdownlist != undefined) {
        PhoneCountryCodedropdownlist.text("- Country Code - ");
        PhoneCountryCodedropdownlist.trigger("change");
    }
    $("#PhoneNo").val("");

    var FaxCountryCodedropdownlist = $("#CountryCode_FaxNo").data("kendoDropDownList");
    if (FaxCountryCodedropdownlist != undefined) {
        FaxCountryCodedropdownlist.text("- Country Code - ");
        FaxCountryCodedropdownlist.trigger("change");
    }
    $("#FaxNo").val("");

});
function onGridCheckClick(e) {
    var grid = $("#RegAddressDetails").data("kendoGrid");
    var row = $(e.target).closest("tr");

    if (row.hasClass("k-state-selected")) {
        setTimeout(function (e) {
            var grid = $("#RegAddressDetails").data("kendoGrid");
            grid.clearSelection();
        })
    } else {
        grid.clearSelection();
    };
};
var grid = $("#RegAddressDetails").data("kendoGrid");
grid.tbody.on("click", ".k-checkbox", onGridCheckClick);

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
function isNumberWithoutDecimalKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;

    return true;
}