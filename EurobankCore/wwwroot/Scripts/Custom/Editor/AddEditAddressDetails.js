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
    $('#AddressLine1').val(selectedItem.AddresLine1.toUpperCase()).change();
    $('#AddressLine2').val(selectedItem.AddresLine2.toUpperCase()).change();
    $('#POBox').val(selectedItem.POBox.toUpperCase()).change();
    $('#PostalCode').val(selectedItem.PostalCode.toUpperCase()).change();
    $('#City').val(selectedItem.City.toUpperCase()).change();
    $('#LocationName').val(selectedItem.LocationName.toUpperCase());
    $("#LocationName").trigger("change");
    console.log(selectedItem.Country);
    var dropdownlist = $("#Country").data("kendoDropDownList");
    dropdownlist.value(selectedItem.Country);
    $("#SearchWindow").data("kendoWindow").close();
    $("#divRegisteredAddresss").css("display", "none");
    $('#AddressRegistryId').val(selectedItem.NodeID);
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