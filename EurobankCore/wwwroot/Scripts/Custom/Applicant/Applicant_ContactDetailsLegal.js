    //for Accordian
    $("#accordionContactDetails").kendoPanelBar({
        expandMode: "multiple" //options are 'single' and 'multiple'. 'multiple' is the default value
    });

    var accordion = $("#accordionContactDetails").data("kendoPanelBar");
    accordion.collapse("#chartSection");