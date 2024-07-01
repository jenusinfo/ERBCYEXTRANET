
	function onSaveIndividualAddressDetails(e) {
		//console.log('on save start');
		//var grid = $('#AddressDetails').data('kendoGrid');
		//grid.dataSource.read();
		//console.log('on save end');
		//grid.dataSource.read();
		//grid.dataSource.sync();
		//$('#AddressDetails').data('kendoGrid').dataSource.read();
		//$('#AddressDetails').data('kendoGrid').refresh();
	}
	function Sync_handlerAddressDetailsApplicant(e) {
        if (!e.sender.preventSync)
	{
		e.sender.read();
        }
	e.sender.preventSync = false;
    }

    function onAddressDataBound(e) {
	$("#AddressDetails .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
	$("#AddressDetails .k-grid-content").attr("style", "max-height: 400px");
	deleteEmptyGridRecord('AddressDetails');
	isAllConfirmed($("#hdnApplication_Applicant_LeftMenu_AddressDetails").val(), "Address", "AddressDetails", true);
	}

	function addConfirmButton_AddressDetails(e) {
		$(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
	$("a.k-grid-update").addClass("SAVEASDRAFT");
	var model = e.model;
	model.Status = false;
	model.dirty = true;
	$("a.k-grid-cancel").addClass('btn btn-link');
	$("a.k-grid-update")[0].innerHTML = "SAVE AS DRAFT";
	$("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";

	$('<a class="k-button k-button-icontext k-primary k-grid-update custom" href="#" k-icon k-i-delete"></span>SAVE & CLOSE</a>').insertBefore(".k-grid-update");
$(".custom").click(function (e) {
	model.Status = true;
	$(".k-widget.k-window.k-display-inline-flex input").each(function (i, obj) {
		if (($(this).closest('div')).children('div.field-validation-error').length > 0) {
			var elementName = $(this).attr('name');
			$($(this).closest('div')).children('div.field-validation-error').remove();
			$($(this).closest('div')).append('<span class="field-validation-valid k-hidden" data-valmsg-for="' + elementName + '" data-valmsg-replace="true"></span>');
		}
	});
});
$(".SAVEASDRAFT").click(function (e) {
	model.Status = false;

});
$(".k-widget.k-window.k-display-inline-flex input").keyup(function (e) {
	var validatePopup = $(".k-popup-edit-form").kendoValidator().data("kendoValidator");
	validatePopup.validate();
	if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
		//debugger;
		if ($(this).val() == '') {
			if ($($($(this).closest('div')).children('div.field-validation-error')).children('span.k-tooltip-content').text().indexOf('Max length exceeded') < 0) {
				$($(this).closest('div')).children('div.field-validation-error').show();
			}
			else {
				$($(this).closest('div')).children('div.field-validation-error').hide();
			}
		}
		else {
			if ($(this).attr('id') == 'City' && $("#Country").data("kendoDropDownList") != undefined) {
				var countryddlValue = $("#Country").data("kendoDropDownList").text();
				var cityValue = $(this).val().trim().toLowerCase();
				if (countryddlValue == 'CYPRUS' && (cityValue != 'nicosia' && cityValue != 'limassol' && cityValue != 'paphos' && cityValue != 'larnaca' && cityValue != 'famagusta')) {
					$($(this).closest('div')).children('div.field-validation-error').show();
				}
				else {
					if ($($($(this).closest('div')).children('div.field-validation-error')).children('span.k-tooltip-content').text().indexOf('Max length exceeded') < 0) {
						$($(this).closest('div')).children('div.field-validation-error').hide();
					}
					else if ($(this).val().length <= 35) {
						$($(this).closest('div')).children('div.field-validation-error').hide();
					}
					else {
						$($(this).closest('div')).children('div.field-validation-error').show();
					}
				}
			}
			else {
				if ($($($(this).closest('div')).children('div.field-validation-error')).children('span.k-tooltip-content').text().indexOf('Max length exceeded') < 0) {
					$($(this).closest('div')).children('div.field-validation-error').hide();
				}
				else if ($(this).val().length <= 35) {
					$($(this).closest('div')).children('div.field-validation-error').hide();
				}
				else {
					$($(this).closest('div')).children('div.field-validation-error').show();
				}
			}

		}
	}
});

$(".k-widget.k-window.k-display-inline-flex input").change(function (e) {
	if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
		if ($(this).val() == '') {
			if ($($($(this).closest('div')).children('div.field-validation-error')).children('span.k-tooltip-content').text().indexOf('Max length exceeded') < 0) {
				$($(this).closest('div')).children('div.field-validation-error').show();
			}
			else {
				$($(this).closest('div')).children('div.field-validation-error').hide();
			}
		}
		else {
			if ($(this).attr('id') == 'City' && $("#Country").data("kendoDropDownList") != undefined) {
				var countryddlValue = $("#Country").data("kendoDropDownList").text();
				var cityValue = $(this).val().trim().toLowerCase();
				if (countryddlValue == 'CYPRUS' && (cityValue != 'nicosia' && cityValue != 'limassol' && cityValue != 'paphos' && cityValue != 'larnaca' && cityValue != 'famagusta')) {
					$($(this).closest('div')).children('div.field-validation-error').show();
				}
				else {
					if ($($($(this).closest('div')).children('div.field-validation-error')).children('span.k-tooltip-content').text().indexOf('Max length exceeded') < 0) {
						$($(this).closest('div')).children('div.field-validation-error').hide();
					}
					else if ($(this).val().length <= 35) {
						$($(this).closest('div')).children('div.field-validation-error').hide();
					}
					else {
						$($(this).closest('div')).children('div.field-validation-error').show();
					}
				}
			}
			else {
				if ($($($(this).closest('div')).children('div.field-validation-error')).children('span.k-tooltip-content').text().indexOf('Max length exceeded') < 0) {
					$($(this).closest('div')).children('div.field-validation-error').hide();
				}
				else if ($(this).val().length <= 35) {
					$($(this).closest('div')).children('div.field-validation-error').hide();
				}
				else {
					$($(this).closest('div')).children('div.field-validation-error').show();
				}
			}
		}
	}
});

if (isApplicationPermisssionEdit == 'False') {
	setAllPopUpInputElementDisabled();
}
if (model.SaveInRegistry) {
	$("#LocationID").css("display", "block");
}
		if ($("#hdnApplicantEntityType").val() == "Provident Fund" || $("#hdnApplicantEntityType").val() == "Pension Fund") {
	//$('#spanAddressType').attr('title', 'The address of the Sponsoring Entity should be provided. In case of multiple Sponsoring Entities the address of the principal administrator should be recorded');
	//$('#spanAddressType').addClass('fa fa-info-circle');
	document.getElementById('spanAddressType').innerHTML = '  (The address of the Sponsoring Entity should be provided. In case of multiple Sponsoring Entities the address of the principal administrator should be recorded)';
}
var selectedEntityValue = $("#EntityTypeID").data("kendoDropDownList") != undefined ? $("#EntityTypeID").data("kendoDropDownList").text() : '';
if (selectedEntityValue == "Provident Fund" || selectedEntityValue == "Pension Fund") {
	//$('#spanAddressType').attr('title', 'The address of the Sponsoring Entity should be provided. In case of multiple Sponsoring Entities the address of the principal administrator should be recorded');
	//$('#spanAddressType').addClass('fa fa-info-circle');
	/*document.getElementById('spanAddressType').innerHTML = '  (The address of the Sponsoring Entity should be provided. In case of multiple Sponsoring Entities the address of the principal administrator should be recorded)';*/
	$("#spanAddressType").text('  (The address of the Sponsoring Entity should be provided. In case of multiple Sponsoring Entities the address of the principal administrator should be recorded)');
}
else {
	/* document.getElementById('spanAddressType').innerHTML = (" ");*/
	$("#spanAddressType").text('');
}
ShowRespectedDiv();
ShowHideFieldsOnAddressType();
BindAddressTypeLegal();

	}
function BindAddressTypeLegal() {
	var selectedEntityValue = $("#EntityTypeID").data("kendoDropDownList") != undefined ? $("#EntityTypeID").data("kendoDropDownList").value() : '';
	if (selectedEntityValue != '') {
		$.ajax({
			url: $("#hdnRedirectTo_Applicant_AddRessTypeLegalRead").val(),
			cache: false,
			type: "POST",
			data: { entityType: selectedEntityValue }
		})
			.done(function (response) {
				var entityDDL = $("#AddressType").data("kendoDropDownList");
				entityDDL.setDataSource(response);
			});
	}
}
function error_handlerAddressDetailsApplicant(args) {
	//console.log('Error Event called');
	var errors = args.errors;
	console.log(errors);
	if (errors) {
		args.sender.preventSync = true;
		//console.log('Error Event if condition called');
		var grid = $("#AddressDetails").data("kendoGrid");
		grid.one("dataBinding", function (e) {
			e.preventDefault();
			$.each(errors, function (key, value) {
				var message = "";
				if ('errors' in value) {
					$.each(value.errors, function () {
						message += this + "\n";
					});
				}

				// As long as the key matches the field name, this line of code will be displayed as validation message in the popup.
				grid.editable.element.find("[data-valmsg-for='" + key + "']").replaceWith('<div class="k-tooltip k-tooltip-error k-validator-tooltip k-invalid-msg field-validation-error" ><span class="k-tooltip-icon k-icon k-i-warning"> </span><span class="k-tooltip-content">' + message + '</span><div class="k-callout k-callout-n"></div></div>').show();
			});
		});
	}
}

$("#accordionAddressDetails").kendoPanelBar({
	expandMode: "multiple" //options are 'single' and 'multiple'. 'multiple' is the default value
});

var accordion = $("#accordionAddressDetails").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuAddressDetails").kendoContextMenu({
	target: "#AddressDetails",
	showOn: "click",
	filter: "td:first-child",
	select: function (e) {
		//debugger;
		var row = $(e.target).parent()[0];
		var grid = $("#AddressDetails").data("kendoGrid");
		var tr = $(e.target).closest("tr"); //get the row for deletion
		var data = grid.dataItem(tr);//get the row data so it can be referred later
		var item = e.item.id;
		switch (item) {
			case "addRow":
				grid.addRow();
				break;
			case "editRowAdd":
				grid.editRow(row);
				break;
			case "removeRow":
				//grid.removeRow(row);
				DisplayDeleteConfirmationContextMenu(e, data, "AddressDetails");
				break;
			default:
				break;
		};
	}
});