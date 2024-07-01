$("#accordionPartyRoles").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionPartyRoles").data("kendoPanelBar");
accordion.collapse("#chartSection");