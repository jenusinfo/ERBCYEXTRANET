$(function() {
	$("form").kendoValidator();
});
$(".datepicker1").kendoDatePicker();

$(".datepicker1").change(function(e) {
	var currentDate = kendo.parseDate(this.value);
	if(!currentDate) {
		this.value = "";
	}
});
$(document).ready(function () {
	
	$("#FieldFilter").keyup(function() {

		var value = $("#FieldFilter").val();
		grid = $("#GridApplications").data("kendoGrid");

		if(value) {
			grid.dataSource.filter({ field: "Description", operator: "contains", value: value });
		} else {
			grid.dataSource.filter({});
		}
	});
	var grid = $("#GridApplications").data("kendoGrid");
	var gridColumns = grid.options.columns;
	console.log(gridColumns);
	//htmlOptions = "<option value=''>All</option>";	
	htmlOptions = "";	
	
	for(var i = 1; i < gridColumns.length; i++) {
		htmlOptions += "<option value='" + gridColumns[i].field + "'>" + gridColumns[i].title + "</option>";
	}
	htmlOptions += "<option value='ApplicationDetails_ApplicationStatusName'>Status</option>";
	
	$("#column-list")
		.append(htmlOptions);

	//change event
	$("#searchbox").keyup(function() {
		var val = $('.StatusCl li.HeaderTextColor a').text();//$('#searchbox').val();
		var fieldID = "ApplicationDetails_ApplicationStatusName";//$('#column-list').val();
		
		$("#GridApplications").data("kendoGrid").dataSource.filter({
			logic: "or",
			filters: [
				{
					field: fieldID,
					operator: "contains",
					value: val
				},
				
			]
		});


	});
});
function TitleKeyup(obj) {
	grid = $("#GridApplications").data("kendoGrid");
	if(obj.value != "") {
		grid.dataSource.filter({ field: "Title", operator: "contains", value: obj.value });
	}
	else {
		grid.dataSource.filter({});
	}
}
function NameKeyup(obj) {
	grid = $("#GridApplications").data("kendoGrid");
	if(obj.value != "") {
		grid.dataSource.filter({ field: "FirstName", operator: "contains", value: obj.value });
	}
	else {
		grid.dataSource.filter({});
	}
}
function onChange(e) {
	grid = $("#GridApplications").data("kendoGrid");
	if(e.sender.value() != "") {
		grid.dataSource.filter({ field: "Isactive", operator: "contains", value: e.sender.value().toString() });
	}
	else {
		grid.dataSource.filter({});
	}
}
function statusCommand(e) {
	//debugger;
	grid = $("#GridApplications").data("kendoGrid");
	if(e != "") {
		grid.dataSource.filter({ field: "ApplicationDetails_ApplicationStatusName", operator: "contains", value: e.toString() });
	}
	else {
		grid.dataSource.filter({});
	}
}
function applicationRedirect() {
	
	var url = $("#RedirectTo").val();
	location.href = url;
}
//context menu
$("#context-menu").kendoContextMenu({
	target: "#GridApplications",
	showOn: "click",
	filter: "td:first-child",
	select: function(e) {
		var row = $(e.target).parent()[0];
		var grid = $("#GridApplications").data("kendoGrid");
		var tr = $(e.target).closest("tr"); //get the row for deletion
		var data = grid.dataItem(tr);//get the row data so it can be referred later
		var item = e.item.id;
		var loader = $("#loader").data("kendoLoader");
		//debugger;
		switch(item) {
			
			case "editRow":
				var url = $("#RedirectToApplication").val();
				window.location.href = url + "?application=" + data.Application_NodeGUID ;
				break;
			case "removeRow":
				//alert();
				//grid.removeRow(row);
				DisplayDeleteConfirmationContextMenu(e, data, "GridApplications", "Appliction", "Appliction Deleted Successfully");
				break;
			case "printSummary":
				//loader.show();
				$("#successDisplay .toastbody").html($("#PrintSummaryDownloadStartMessage").val());
				$("#successDisplay").show().delay(10000).fadeOut(); 
				var url = $("#RedirectPrintSummary").val();
				window.location.href = url + "?applicationNumber=" + data.ApplicationDetails_ApplicationNumber;
				//alert('Wait !! File will start downloading');
				//window.onload = function () {
				//	loader.hide();
				//}
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