function onReadApplications() {
    return {
        txtSearch: $("#searchbox").val(),
        filterColumn: $('#column-list').val()
    };
}
function onApplicationHomeDataBound(e) {
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
$("#editRow").click(function () {
    showHideLoader()
})
//$("#removeRow").click(function () {
//    showHideLoader()
//})
$("#navbarNavDropdown > ul > li").click(function () {
    showHideLoader()
})
$(".logo-image").click(function () {
    showHideLoader()
})
function downloadXLSX() {
    //window.location.href = $("#ApplicationDownloadUrl").val();
    //window.open($("#ApplicationDownloadUrl").val(), '_blank');
    showHideLoader();
    downloadExcelThroughJavaScript();
    
}
$(window).resize(function () {
    $("#chart").data("kendoChart").refresh();
    $("#chart1").data("kendoChart").refresh();
});

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

$(document).ready(function () {
    // Spin all loading indicators on the page.
    kendo.ui.progress($(".chart-loading"), true);
});

async function onRender(e) {
    var loading = $(".chart-loading", e.sender.element.parent());

    kendo.ui.progress(loading, false); // Disable the loading indicator.

    var data = e.sender.dataSource.view();
    if (data.lenght === 0) {
        e.sender.element.parent().html("");
    }
}
