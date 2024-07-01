function onGridDataBound(e) {
	setKendoGridButtons(e.sender.wrapper[0].id);
}

function onGridCancel(e) {
	setKendoGridButtons(e.sender.wrapper[0].id);
}

function onGridEditing(e) {
	setKendoGridButtons(e.sender.wrapper[0].id);

}

function setKendoGridButtons(gridName) {

	setTimeout(function() {
		$("#" + gridName + " .k-grid-edit").html("<span class='fa fa-edit'></span>").addClass("mr-2");
		$("#" + gridName + " .k-grid-Delete").html("<span class='fa fa-trash'></span>").addClass("mr-2 bg-none customdelete");
		$("#" + gridName + " .k-grid-Identification").html("<span class='fa fa-id-card'></span>").addClass("mr-2");
		$("#" + gridName + " .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
		$("#" + gridName + " .k-state-default").addClass("new-closebutton");
		

	});
}
