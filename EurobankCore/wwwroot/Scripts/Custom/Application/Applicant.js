
function onApplicantDataBound(e) {
    $("#Applicants .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#Applicants .k-grid-content").attr("style", "max-height: 400px");
    isAllConfirmed($("#hdnApplication_LeftMenu_Applicants").val(), "Applicants", "Applicants",);
    CheckApplicantCount();
    manageApplicantMenuPanel();
    BindReasonForopeningAcc();
}
function CheckApplicantCount() {
    var applicantionType = $("#ApplicationTypeName").val();
    var kendoGrid = $("#Applicants").data("kendoGrid");
    if ((applicantionType == "LEGAL ENTITY" || applicantionType == "INDIVIDUAL") && kendoGrid.dataSource.total() > 0) {
        $('#divCreateNewApplicant').hide();
        $('#lnkCreateNewApplicant').hide();

    }
    else {
        $('#divCreateNewApplicant').show();
        $('#lnkCreateNewApplicant').show();
    }
}

function BindReasonForopeningAcc() {
    var multiReasonForOpening = $("#ReasonForOpeningTheAccountGroup").data("kendoMultiSelect");
    if (multiReasonForOpening != undefined) {
        multiReasonForOpening.dataSource.read();
    }
}
$("#accordionApplicants").kendoPanelBar({
    expandMode: "multiple"
});


var accordion = $("#accordionApplicants").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuApplicants").kendoContextMenu({
    target: "#Applicants",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#Applicants").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                break;
            case "editRowApplicants":
                var url = $("#RedirectToApplicant").val();
                window.location.href = url + "?application=" + $("#ApplicationNodeGUID").val() + "&&applicant=" + data.NodeGUID;
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "Applicants");
                break;
            default:
                break;
        };
    }
});
