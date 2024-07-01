function onReadApplications() {
    return {
        txtSearch: $("#searchbox").val(),
        filterColumn: $('#column-list').val()
    };
}
function onApplicationDataBound(e) {
    $("#GridApplications .k-grid-content").attr("style", "max-height: 400px");
    
    //var grid = this;

    //if ($(e.target).closest("td").index() === 0) {
    //    return;
    //}
    //grid.table.on("click", "tr", function (e) {
    //    var data = grid.dataItem(this);
    //    var url = $("#RedirectToApplication").val();
    //    window.location.href = url + "?application=" + data.Application_NodeGUID;
    //});
}
$("#context-menuApplications").kendoContextMenu({
    target: "#GridApplications",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#GridApplications").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        console.log(e);
        console.log(data);
        switch (item) {
            case "exportXML":
                var url = $("#ExportXMLUrl").val();
                //window.location.href = url + "?applicationId=" + data.ApplicationDetailsID;
                //showHideLoader();
                showHideFileLoader();
                downloadXMLThroughJavaScript(data.ApplicationDetailsID, data.ApplicationDetails_ApplicationNumber);
                break;
            case "editRow":
                var url = $("#RedirectToApplication").val();
                window.location.href = url + "?application=" + data.Application_NodeGUID;
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "GridApplications", "Appliction", "Appliction Deleted Successfully");
                break;
            case "printSummary":
                $("#successDisplay .toastbody").html($("#PrintSummaryDownloadStartMessage").val());
                $("#successDisplay").show().delay(10000).fadeOut(); 
                var url = $("#PrintSummaryUrl").val();
                window.location.href = url + "?applicationNumber=" + data.ApplicationDetails_ApplicationNumber;
                break;
            case "duplicateApplication":
                var url = $("#DuplicateApplication").val();
                window.location.href = url + "?applicationGuid=" + data.Application_NodeGUID;
                break;
            case "btnprintFriendly":
                $("#successDisplay .toastbody").html($("#PrintFriendlyDownloadStartMessage").val());
                $("#successDisplay").show().delay(10000).fadeOut(); 
                var url = $("#RedirectPrintFriendly").val();
                window.location.href = url + "?applicationNumber=" + data.ApplicationDetails_ApplicationNumber;
                break;
            default:
                break;
        };

    }
});
function downloadXLSX() {
    //window.location.href = $("#ApplicationDownloadUrl").val();
    showHideLoader();
    downloadExcelThroughJavaScript();
}

function applicationRedirectNew() {
    window.location.href = $("#ApplicationCreateUrl").val();
}
$('.StatusCl li').click(function () {
    $('.StatusCl li.HeaderTextColor').removeClass('HeaderTextColor')
    $(this).addClass('HeaderTextColor');
});

var loadingPanelVisible = false;
$("#loader").kendoLoader({
    visible: false
});
function showHideLoader() {
    var loader = $("#loader").data("kendoLoader");
    loadingPanelVisible = !loadingPanelVisible;
    if (loadingPanelVisible) {
        loader.show();
    } else {
        loader.hide();
    }
}
function HideLoader() {
    var loader = $("#loader").data("kendoLoader");
    loader.hide();
}
$("#editRow").click(function () {
    showHideLoader()
})
$("#GridApplications > div.k-toolbar.k-grid-toolbar > div.d-flex > a").click(function () {
    showHideLoader()
})
$("#navbarNavDropdown > ul > li").click(function () {
    showHideLoader()
})
$(".logo-image").click(function () {
    showHideLoader()
})
$("#duplicateApplication").click(function () {
    showHideLoader()
})

function downloadExcelThroughJavaScript() {
    $.ajax({
        type: 'GET',
        url: $("#ApplicationDownloadUrlJS").val(),
        success: function (result) {
            //debugger;
            console.log(result);
            downloadCSV(result, 'Applications.csv');
            showHideLoader();
        },
        error: function (error) {
            showHideLoader();
        }
    });



}

function arrayToCSV(data) {
    const csvRows = [];
    const headers = Object.keys(data[0]);
    csvRows.push(headers.join(','));

    for (const row of data) {
        const values = headers.map(header => row[header]);
        csvRows.push(values.join(','));
    }

    return csvRows.join('\n');
}

function downloadCSV(data, filename) {
    const csvData = arrayToCSV(data);
    const blob = new Blob([csvData], { type: 'text/csv' }); //

    const link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    link.download = filename;

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function downloadXMLThroughJavaScript(appid,applicationNumber) {
    $.ajax({
        type: 'GET',
        url: $("#ExportXMLUrlJS").val(),
        data: { appId: appid },
        timeout: 180000, //180 seconds
        success: function (result) {
            //debugger;
            console.log(result);
            // Your XML data as a string
            var xmlString = result;

            // Create a Blob with the XML data
            var blob = new Blob([xmlString], { type: 'application/xml' });

            // Create a link element to trigger the download
            var a = document.createElement('a');
            a.href = window.URL.createObjectURL(blob);
            a.download = applicationNumber +'.xml'; // Set the desired filename

            // Trigger a click event to initiate the download
            a.click();
            //downloadCSV(result, 'Applications.csv');
            //showHideLoader();
            hideFileLoader();
        },
        error: function (error) {
            //debugger;
            console.log(error);
            //showHideLoader();
            hideFileLoader();
        }
    });



}

function showHideFileLoader() {
    var $dialog = $("#dialog");
    var $progressbar = $("#progressbar");
    var $progressLabel = $(".progress-label");

    $progressbar.progressbar({
        value: false
    });

    
    $dialog.dialog({
        modal: true,
        closeOnEscape: false,
        resizable: false,
        buttons: {},
    });
}

function hideFileLoader() {
    var $dialog = $("#dialog");
    $dialog.dialog("close");
}