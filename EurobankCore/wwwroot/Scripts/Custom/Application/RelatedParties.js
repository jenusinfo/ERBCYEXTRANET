function onRelatedPartyDataBound(e) {
    $("#RelatedParties .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#RelatedParties .k-grid-content").attr("style", "max-height: 400px");
    isAllConfirmed($("#hdnApplication_LeftMenu_RelatedParties").val(), "RelatedParties", "RelatedParties");
    manageRelatedPartyMenuPanel();
}
$("#accordionRelatedParties").kendoPanelBar({
    expandMode: "multiple"
});

var accordion = $("#accordionRelatedParties").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuRelatedParties").kendoContextMenu({
    target: "#RelatedParties",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#RelatedParties").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                break;
            case "editRowRelatedP":
                var url = $("#RedirectToRelatedParty").val();
                window.location.href = url + "?application=" + $("#ApplicationNodeGUID").val() + "&&relatedParty=" + data.NodeGUID + "&&type=" + data.Type;
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "RelatedParties");
                break;
            default:
                break;
        };
    }
});
function OpenConfirmationAddRelatedParty() {

    var searchWindow = $("#AddrealtedPartyWindow").data("kendoWindow");
    searchWindow.wrapper.addClass("middle-popup");
    searchWindow.center();
    searchWindow.open();

}
function ShowUBO() {
    $("#UBOerrorMsg").css("display", "none");
    if ($("#RadioIndividual").is(":checked") || $("#RadioLegalEntity").is(":checked")) {
        $("#UBODiv").css("display", "block");
    }
}
function HideErrorDiv() {
    $("#UBOerrorMsg").css("display", "none");
}
function CreateRelatedParty() {
    var is_UBO = false;
    if ($("#RadioYes").is(":checked")) {
        is_UBO = true;
    }
    if ($("#RadioYes").is(":checked") || $("#RadioNo").is(":checked")) {
        $("#UBOerrorMsg").css("display", "none");
        var url = $("#RedirectToRelatedParty").val();
        if ($("#RadioIndividual").is(":checked")) {
            window.location.href = url + "?application=" + $("#ApplicationNodeGUID").val() + "&&type=INDIVIDUAL" + "&&isUBO=" + is_UBO;
        }
        if ($("#RadioLegalEntity").is(":checked")) {
            window.location.href = url + "?application=" + $("#ApplicationNodeGUID").val() + "&&type=LEGAL ENTITY" + "&&isUBO=" + is_UBO;
        }
    }
    else {
        $("#UBOerrorMsg").css("display", "block");
    }
}
