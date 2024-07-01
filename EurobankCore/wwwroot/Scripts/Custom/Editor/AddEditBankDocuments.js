
function SelectEntityType() {
    var entityType = $("#EntityRole1").data("kendoDropDownList");
    entityType.text('');
    entityType.trigger("change");
    var personNodeGUID = $("#Entity").data("kendoDropDownList").value();
    $.ajax({
        url: $("#SelectEntityTypeUrl").val(),
        cache: false,
        type: "POST",
        data: { personNodeGUID: personNodeGUID },
        success: function (result) {
            entityType.text(result);
            entityType.trigger("change");
            if ($($("#EntityRole1").closest('div')).children('div.field-validation-error').length > 0) {
                $($("#EntityRole1").closest('div')).children('div.field-validation-error').hide();
            }
        }
    });
}