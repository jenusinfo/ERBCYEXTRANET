$(document).ready(function () {
    if ($("#IsDisabledDecision").val() == "True") {
        $("#DecisionHistory_DecisionID").data("kendoDropDownList").enable(true);
        $("#DecisionHistory_CommentsID").removeAttr('disabled');
        $("#DecisionHistory_CommentsID").prop("disabled", false);

    }
});
function onDecisionDDLChange(e) {

    if (e.item) {
        var dataItem = this.dataItem(e.item);
        if (dataItem.Value != '') {
            $.ajax({
                url: $("#DecisionHistoryStageUrl").val(),
                cache: false,
                type: "POST",
                data: { applicationId: $("#ApplicationId").val(), decisionGuid: dataItem.Value },
                success: function (result) {
                    $('#DecisionHistory_StageID').val(result);
                }
            });
            console.log(dataItem);
            if (dataItem.Text == 'ESCALATE') {
                $('#divDecisionHistoryEscalateTo').show();
            }
            else {
                $('#divDecisionHistoryEscalateTo').hide();
                var dropdownlist = $("#DecisionHistory_EscalateToID").data("kendoDropDownList");
                dropdownlist.value('');
            }
        }
        else {
            $('#divDecisionHistoryEscalateTo').hide();
            var dropdownlist = $("#DecisionHistory_EscalateToID").data("kendoDropDownList");
            dropdownlist.value('');
        }
    }
}
$(function () {
    $('#DecisionHistoryBtn').on('click', function (evt) {
        var isFormValid = $("#DecisionHistoryForm").valid();
        if (isFormValid) {
            evt.preventDefault();
            $.post('/Applications/DecisionHistory', $('form').serialize(), function (response) {
                $("#DecisionHistoryDiv").html(response);
                show("#DecisionHistoryDiv");

                $("#DecisionHistory_DecisionID").val('');
                $("#DecisionHistory_StageID").val('');
                $("#DecisionHistory_CommentsID").val('');
            });
        }
    });
});
$(function () {
    $("form").kendoValidator();
});
$("#accordionDecisionHistorys").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionDecisionHistorys").data("kendoPanelBar");
accordion.collapse("#chartSection");