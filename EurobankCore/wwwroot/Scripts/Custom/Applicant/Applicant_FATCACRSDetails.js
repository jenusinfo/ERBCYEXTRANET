    //for Accordian
    $("#accordionFATCACSR").kendoPanelBar({
        expandMode: "multiple" //options are 'single' and 'multiple'. 'multiple' is the default value
    });

    var accordion = $("#accordionFATCACSR").data("kendoPanelBar");
	accordion.collapse("#chartSection");