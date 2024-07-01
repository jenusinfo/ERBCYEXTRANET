function GetIdentificationNumber() {
    var id = $("#DebitCardDetails_CollectedBy").val();

    if (id != "") {
        $.ajax({
            url: '@Url.Action("GetIdentificationNumber","Applications")',
            data: { nodeGUID: id },
            dataType: "json",
            type: "GET",
            success: function (data) {
                $("#DebitCardDetails_IdentityNumber").val(data).change().focus().focusout();
            },
        });
    }
    else {
        $("#DebitCardDetails_IdentityNumber").val('');
    }
}
function isNumberKey1(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
