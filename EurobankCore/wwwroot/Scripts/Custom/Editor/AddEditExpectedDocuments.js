
function SelectExpectedEntityType() {
    //debugger;
    var entityType = $("#EntityRoleID").data("kendoDropDownList");
    entityType.text('');
    entityType.trigger("change");
    var personNodeGUID = $("#Entity").data("kendoDropDownList").value();
    $.ajax({
        url: $("#SelectEntityTypeUrl").val(),
        cache: false,
        type: "GET",
        data: { personNodeGUID: personNodeGUID },
        success: function (result) {
            entityType.text(result);
            entityType.trigger("change");
            if ($($("#EntityRoleID").closest('div')).children('div.field-validation-error').length > 0) {
                $($("#EntityRoleID").closest('div')).children('div.field-validation-error').hide();
            }
        }

    });
}