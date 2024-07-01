    $("#accordionBankRelationship").kendoPanelBar({
        expandMode: "multiple" //options are 'single' and 'multiple'. 'multiple' is the default value
    });

    var accordion = $("#accordionBankRelationship").data("kendoPanelBar");
    accordion.collapse("#chartSection");