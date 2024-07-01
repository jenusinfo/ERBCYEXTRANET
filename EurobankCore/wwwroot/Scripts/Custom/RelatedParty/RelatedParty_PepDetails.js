function onPepDetailsDataBound(e) {
    $("#PepApplicant .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#PepApplicant .k-grid-content").attr("style", "max-height: 400px");
    var grid = $('#PepApplicant').data('kendoGrid');
    var gridRows = grid.tbody.find("tr");
    gridRows.each(function (e) {
        var rowItem = grid.dataItem($(this));
        if (rowItem.PepApplicantID == 0) {
            grid.removeRow(rowItem);
        }
    });
    isAllPepDetailsConfirmed();
}

function onPepAssociateDataBound(e) {
    $("#PEPAssociates .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#PEPAssociates .k-grid-content").attr("style", "max-height: 400px");
    var grid = $('#PEPAssociates').data('kendoGrid');
    var gridRows = grid.tbody.find("tr");

    gridRows.each(function (e) {
        var rowItem = grid.dataItem($(this));
        if (rowItem.PepAssociatesID == 0) {
            grid.removeRow(rowItem);
        }
    });
    isAllPepDetailsConfirmed();
}

function addConfirmButton_PEP_Details(e) {
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
        RemoveAllValidationMessage();
    });
    $(".SAVEASDRAFT").click(function (e) {
        model.Status = false;

    });
    $(".k-widget.k-window.k-display-inline-flex input").keyup(function (e) {
        var validatePopup = $(".k-popup-edit-form").kendoValidator().data("kendoValidator");
        validatePopup.validate();
        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div')).children('div.field-validation-error').show();
            }
            else {
                if ($(this).attr('id') == 'PepApplicant_Since' || $(this).attr('id') == 'PepApplicant_Untill') {
                    validatePepDetailsDates(this);
                }
                else {
                    $($(this).closest('div')).children('div.field-validation-error').hide();
                }

            }
        }
    });

    $(".k-widget.k-window.k-display-inline-flex input").change(function (e) {
        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div')).children('div.field-validation-error').show();
            }
            else {
                if ($(this).attr('id') == 'PepApplicant_Since' || $(this).attr('id') == 'PepApplicant_Untill') {
                    validatePepDetailsDates(this);
                }
                else {
                    $($(this).closest('div')).children('div.field-validation-error').hide();
                }

            }
        }
    });

    if (isApplicationPermisssionEdit == 'False') {
        setAllPopUpInputElementDisabled();
    }
}
function validatePepDetailsDates(currentElement) {

    $.ajax({
        url: $("#ValidatePepDatesUrl").val(),
        cache: false,
        type: "POST",
        data: { since: $("#PepApplicant_Since").val(), untill: $("#PepApplicant_Untill").val() },
        success: function (result) {
            if (result == true) {
                $($(currentElement).closest('div')).children('div.field-validation-error').hide();
            }
            else {
                $($(currentElement).closest('div')).children('div.field-validation-error').show();
            }
        }
    });
}
function addConfirmButton(e) {
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
        RemoveAllValidationMessage();
    });
    $(".SAVEASDRAFT").click(function (e) {
        model.Status = false;

    });
    $(".k-widget.k-window.k-display-inline-flex input").keyup(function (e) {
        var validatePopup = $(".k-popup-edit-form").kendoValidator().data("kendoValidator");
        validatePopup.validate();
        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div')).children('div.field-validation-error').show();
            }
            else {
                if ($(this).attr('id') == 'PepAssociates_Since' || $(this).attr('id') == 'PepAssociates_Until') {
                    validatePepAssociatesDetailsDates(this);
                }
                else {
                    $($(this).closest('div')).children('div.field-validation-error').hide();
                }

            }
        }
    });

    $(".k-widget.k-window.k-display-inline-flex input").change(function (e) {
        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div')).children('div.field-validation-error').show();
            }
            else {
                if ($(this).attr('id') == 'PepAssociates_Since' || $(this).attr('id') == 'PepAssociates_Until') {
                    validatePepAssociatesDetailsDates(this);
                }
                else {
                    $($(this).closest('div')).children('div.field-validation-error').hide();
                }

            }
        }
    });
    if (isApplicationPermisssionEdit == 'False') {
        setAllPopUpInputElementDisabled();
    }
}
function validatePepAssociatesDetailsDates(currentElement) {

    $.ajax({
        url: $("#ValidatePepDatesUrl").val(),
        cache: false,
        type: "POST",
        data: { since: $("#PepAssociates_Since").val(), untill: $("#PepAssociates_Until").val() },
        success: function (result) {
            if (result == true) {
                $($(currentElement).closest('div')).children('div.field-validation-error').hide();
            }
            else {
                $($(currentElement).closest('div')).children('div.field-validation-error').show();
            }
        }
    });
}
function error_handler(args) {
    var errors = args.errors;
    if (errors) {
        var grid = $("#PepApplicant").data("kendoGrid");
        grid.one("dataBinding", function (e) {
            e.preventDefault();
            $.each(errors, function (key, value) {
                var message = "";
                if ('errors' in value) {
                    $.each(value.errors, function () {
                        message += this + "\n";
                    });
                }
                grid.editable.element.find("[data-valmsg-for='" + key + "']").replaceWith('<div class="k-tooltip k-tooltip-error k-validator-tooltip k-invalid-msg field-validation-error" ><span class="k-tooltip-icon k-icon k-i-warning"> </span><span class="k-tooltip-content">' + message + '</span><div class="k-callout k-callout-n"></div></div>').show();
            });
        });
    }
}
function error_handlerPEPAssociates(args) {
    var errors = args.errors;
    if (errors) {
        var grid = $("#PEPAssociates").data("kendoGrid");
        grid.one("dataBinding", function (e) {
            e.preventDefault();
            $.each(errors, function (key, value) {
                var message = "";
                if ('errors' in value) {
                    $.each(value.errors, function () {
                        message += this + "\n";
                    });
                }
                grid.editable.element.find("[data-valmsg-for='" + key + "']").replaceWith('<div class="k-tooltip k-tooltip-error k-validator-tooltip k-invalid-msg field-validation-error" ><span class="k-tooltip-icon k-icon k-i-warning"> </span><span class="k-tooltip-content">' + message + '</span><div class="k-callout k-callout-n"></div></div>').show();
            });
        });
    }
}
$("#accordion").kendoPanelBar({
    expandMode: "multiple"
});
$("#accordion1").kendoPanelBar({
    expandMode: "multiple"
});
$("#accordion2").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordion1").data("kendoPanelBar");
accordion.collapse("#chartSection");
var accordion = $("#accordion2").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuPepApplicant").kendoContextMenu({
    target: "#PepApplicant",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#PepApplicant").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowPepApp":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "PepApplicant");
                break;
            default:
                break;
        };
    }
});
$("#context-menuPEPAssociates").kendoContextMenu({
    target: "#PEPAssociates",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#PEPAssociates").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowPepAss":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "PEPAssociates");
                break;
            default:
                break;
        };
    }
});
function onchangeIsPepApplicant() {
    if (IsPepApplicant == "true") {
        if ($("#PepApplicant").data("kendoGrid").dataSource.total() > 0) {
            $("#pepDetailsReselctConfirmation").kendoWindow(); //to activate the window
            var dialog = $("#pepDetailsReselctConfirmation").data("kendoWindow");
            dialog.wrapper.addClass("middle-popup");
            dialog.center().open();
        }
        else {
            ShowNewPepApplicantBtn();
        }
    }
    else {
        ShowNewPepApplicantBtn();
    }
}
function onchangeIsPerAssociate() {
    if (IsPepAssociate == "true") {
        if ($("#PEPAssociates").data("kendoGrid").dataSource.total() > 0) {
            $("#pepDetailsFamilyReselctConfirmation").kendoWindow(); //to activate the window
            var dialog = $("#pepDetailsFamilyReselctConfirmation").data("kendoWindow");
            dialog.wrapper.addClass("middle-popup");
            dialog.center().open();
        }
        else {
            ShowNewPepAssociateBtn();
        }
    }
    else {
        ShowNewPepAssociateBtn();
    }
}

function ShowNewPepApplicantBtn() {
    var IsPep = $('#IsPepID').val();
    if (IsPep == "true") {
        $("#newbtnPepApplicant").css("display", "block");
        $("#newbtnPepApplicantID").css("display", "block");
        $("#PepApplicantGridView").css("display", "block");
    }
    else {
        $("#newbtnPepApplicant").css("display", "none");
        $("#newbtnPepApplicantID").css("display", "none");
        $("#PepApplicantGridView").css("display", "none");

        $.ajax({
            url: $("#PEPApplicantPopup_DestroyAllUrl").val(),
            cache: false,
            type: "POST",
            data: { id: $("#PersonalDetailsID").val() },
            success: function (result) {
                $('#PepApplicant').data('kendoGrid').dataSource.read();
                $('#PepApplicant').data('kendoGrid').refresh();
            }
        });
    }
    IsPepApplicant = IsPep;
    isAllPepDetailsConfirmed();
}
function ShowNewPepAssociateBtn() {
    var IsrelatedToPep = $("#IsRelatedToPepID").val();
    if (IsrelatedToPep == "true") {
        $("#newbtnPepAssociate").css("display", "block");
        $("#newbtnPepAssociateID").css("display", "block");
        $("#PepAssociateGridView").css("display", "block");
    }
    else {
        $("#newbtnPepAssociate").css("display", "none");
        $("#newbtnPepAssociateID").css("display", "none");
        $("#PepAssociateGridView").css("display", "none");

        $.ajax({
            url: $("#PEPAssociatesPopup_DestroyAllUrl").val(),
            cache: false,
            type: "POST",
            data: { id: $("#PersonalDetailsID").val() },
            success: function (result) {
                $('#PEPAssociates').data('kendoGrid').dataSource.read();
                $('#PEPAssociates').data('kendoGrid').refresh();
            }
        });
    }
    IsPepAssociate = IsrelatedToPep;
    isAllPepDetailsConfirmed();
}

var IsPepApplicant = "";
var IsPepAssociate = "";
$(document).ready(function () {
    var IsPep = $('#IsPepID').val();
    if (IsPep == "true") {
        $("#PepApplicantGridView").css("display", "block");
    }
    var IsrelatedToPep = $("#IsRelatedToPepID").val();
    if (IsrelatedToPep == "true") {
        $("#PepAssociateGridView").css("display", "block");
    }
    IsPepApplicant = $('#IsPepID').val();
    IsPepAssociate = $("#IsRelatedToPepID").val();
});
//PEP DETAILS- APPLICANT
function SelectPepDetailsYes() {
    var dialog = $("#pepDetailsReselctConfirmation").data("kendoWindow");
    dialog.close();
    ShowNewPepApplicantBtn();
}
function SelectPepDetailsNo() {
    var dialog = $("#pepDetailsReselctConfirmation").data("kendoWindow");
    dialog.close();
    $("#IsPepID").data("kendoDropDownList").value(IsPepApplicant); //set the previous value
}
//PEP DETAILS-FAMILY MEMBER/ ASSOCIATE
function SelectPepDetailsFamilyYes() {

    var dialog = $("#pepDetailsFamilyReselctConfirmation").data("kendoWindow");
    dialog.close();
    ShowNewPepAssociateBtn();
}
function SelectPepDetailsFamilyNo() {
    var dialog = $("#pepDetailsFamilyReselctConfirmation").data("kendoWindow");
    dialog.close();
    $("#IsRelatedToPepID").data("kendoDropDownList").value(IsPepAssociate); //set the previous value
}