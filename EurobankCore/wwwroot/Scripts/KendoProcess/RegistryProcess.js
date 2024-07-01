function error_handler(e) {
	if(e.errors) {
		var message = "Errors:\n";
		$.each(e.errors, function(key, value) {
			if('errors' in value) {
				$.each(value.errors, function() {
					message += this + "\n";
				});
			}
		});
		alert(message);
	}
}
$("#Registry").on("click", "#customButton", function(e) {
	e.preventDefault();  //prevents postback

	var window = $("#registryWindow").data("kendoWindow");
	window.refresh({
		url: "/Registries/ImportWindowPersonsRegistry"
	});
	window.center()
	window.open();
});
//var CountIt = 0
//var CountRegistryIt = 0
//function GetCountIt() {
//	var page = $("#Registry").data("kendoGrid").dataSource.page();
//	var pageSize = $("#Registry").data("kendoGrid").dataSource.pageSize();
//	CountIt++;
//	return (page * pageSize) - pageSize + CountIt
//}

function GetCountAddressRegistry() {
	var page = $("#AddressRegistry").data("kendoGrid").dataSource.page();
	var pageSize = $("#AddressRegistry").data("kendoGrid").dataSource.pageSize();
	CountRegistryIt++;
	return (page * pageSize) - pageSize + CountRegistryIt
}
function onRequestEnd(e) {
	console.log(e.type);
	if(e.type == "Registry") {
		$("#environmentGrid").data("kendoGrid").dataSource.read();
	}
}
function ShowRates(e) {
	
	var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
	//debugger;
	console.log(dataItem.NodeGUID + "|" + dataItem.NodeAliaspath);
	var prm = dataItem.NodeGUID + "|" + dataItem.NodeAliaspath;
	//var dataItem=	$(e.currentTarget).closest("[data-role='grid']").attr("id");
	var window = $("#identification").data("kendoWindow");
	window.refresh({

		url: "/Identifications/IdentificationDetails/",
		data: { NodeGUID: dataItem.NodeGUID, NodeAliaspath: dataItem.NodeAliaspath }
	});
	window.center()
	window.open();

}

$(document).ready(function() {
	//debugger;
	var grid = $("#Registry").data("kendoGrid");
	var gridColumns = grid.options.columns;
	console.log(gridColumns);
	htmlOptions = "<option value=''>All</option>";
	for(var i = 1; i < gridColumns.length-2 ; i++) {
		if(gridColumns[i].title != 'ID')
		{
			htmlOptions += "<option value='" + gridColumns[i].field + "'>" + gridColumns[i].title + "</option>";
		}
	}
	$("#column-list")
		.append(htmlOptions);

	//change event
	$("#searchbox").keyup(function() {
		var val = $('#searchbox').val();
		var fieldID = $('#column-list').val();

		$("#Registry").data("kendoGrid").dataSource.filter({
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
$(document).ready(function() {

	var grid = $("#AddressRegistry").data("kendoGrid");
	var gridColumns = grid.options.columns;
	console.log(gridColumns);
	htmlOptionsAddress = "<option value=''>All</option>";
	for(var i = 1; i < gridColumns.length-2; i++) {
	
		if (gridColumns[i].title != "ID") {
			htmlOptionsAddress += "<option value='" + gridColumns[i].field + "'>" + gridColumns[i].title + "</option>";
		}
		
	}
	$("#column-listAdress")
		.append(htmlOptionsAddress);

	//change event
	$("#searchboxAddres").keyup(function() {
		var val = $('#searchboxAddres').val();
		var fieldID = $('#column-listAdress').val();
		console.log(val);
		console.log(fieldID);
		$("#AddressRegistry").data("kendoGrid").dataSource.filter({
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

function onRequestEnd(e) {
	//RequestEnd handler code

	if(e.type === "update" || e.type === "create" || e.type === "destroy") {
		e.sender.read();

	}

}
//context menu

