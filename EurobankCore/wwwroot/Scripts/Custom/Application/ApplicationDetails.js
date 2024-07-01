$("#accordionApplicationDetails").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionApplicationDetails").data("kendoPanelBar");
accordion.collapse("#chartSection");
$(document).ready(function () {
    $(".checked-readonly input").prop('disabled', true);
    $(".readonly input").prop('disabled', true);
    $(".readonly input").prop('checked', false);
});