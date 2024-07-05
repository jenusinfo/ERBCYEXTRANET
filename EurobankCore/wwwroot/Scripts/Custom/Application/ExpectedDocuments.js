function ShowRoles(id, entityName) {
    $.ajax({
        url: $("#GetRelatedPartyRolesUrl").val(),
        cache: false,
        type: "POST",
        data: { relatedPartyGuid: id, isLegalEntity: $("#IsLegalEntity").val() },
        success: function (result) {
            $("#EntityRoletbodyId").html('');
            for (var i = 0; i < result.length; i++) {
                var row = '<tr>';
                row += '<td>' + result[i] + '</td>';
                row += '</tr>';
                $("#EntityRoletbodyId").append(row);
            }

            var searchWindow = $("#EntityRoleWindow").data("kendoWindow");
            searchWindow.wrapper.addClass("middle-popup");
            searchWindow.center();
            searchWindow.open();
            searchWindow.title(entityName);
        }
    });
}

function bindRolesData(data) {
    if (data.EntityType_Name.toUpperCase() == 'APPLICANT') {
        return data.EntityRole_Name;
    }
    else {
        return `<a href="javascript:void(0);" onclick="ShowRoles('` + data.Entity + `','` + data.Entity_Name + `');" style="color: #0d6767"><b>View Roles</b></a>`;
    }
}
function onGridDataBoundExpectedDocument(e) {
    $("#ExpectedDocuments .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
    $("#ExpectedDocuments .k-grid-content").attr("style", "max-height: 400px");
    deleteEmptyGridRecord('ExpectedDocuments');
    $(".k-toolbar .k-textbox input").removeAttr("disabled");
}
function onSavechangeExpectedDocument(e) {
    console.log('OnSaveChange');
}
function Sync_handlerExpectedDocuments(e) {
    if (!e.sender.preventSync) {
        showHideLoader();
        window.location.reload();
    }
    e.sender.preventSync = false;
}
function addExpectedConfirmButton(e) {
    debugger;
    $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
    var model = e.model;
    model.BankDocuments_Status = false;
    model.dirty = true;
    $("a.k-grid-cancel").addClass('btn btn-link');
    $("a.k-grid-update")[0].innerHTML = "SAVE AS DRAFT";
    $("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";

    $('<a class="k-button k-button-icontext k-primary k-grid-update custom" href="#" k-icon k-i-delete"></span>SAVE & CLOSE</a>').insertBefore(".k-grid-update");
    $(".custom").click(function (e) {
        model.BankDocuments_Status = true;

    });
    if (model.UploadFileName != undefined && model.UploadFileName != null && model.UploadFileName != '') {
        $('#prevUploadExpectedDoc').show();
        if (model.UploadFileName.indexOf('.pdf') > 0) {
            GetExpectedDocuments(model.FileUpload, model.UploadFileName, model.ExternalFileGuid);
        }
        else {
            GetExpectedDocumentsFile(model.FileUpload, model.UploadFileName, model.ExternalFileGuid);
        }

    }
    else {
        $('#prevUploadExpectedDoc').hide();
    }
    $(".k-widget.k-window.k-display-inline-flex input").keyup(function (e) {
        var validatePopup = $(".k-popup-edit-form").kendoValidator().data("kendoValidator");
        validatePopup.validate();

        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div')).children('div.field-validation-error').show();
            }
            else {
                $($(this).closest('div')).children('div.field-validation-error').hide();
            }
        }
    });

    $(".k-widget.k-window.k-display-inline-flex input").change(function (e) {

        if ($($(this).closest('div')).children('div.field-validation-error').length > 0) {
            if ($(this).val() == '') {
                $($(this).closest('div')).children('div.field-validation-error').show();
            }
            else {
                $($(this).closest('div')).children('div.field-validation-error').hide();
            }
        }
    });
}
function error_handlerExpectedDocuments(args) {
    var errors = args.errors;
    if (errors) {
        args.sender.preventSync = true;
        var grid = $("#ExpectedDocuments").data("kendoGrid");
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
$("#accordionExpectedDocuments").kendoPanelBar({
    expandMode: "multiple"
});

var accordion = $("#accordionExpectedDocuments").data("kendoPanelBar");
accordion.collapse("#chartSection");
$("#context-menuExpectedDocuments").kendoContextMenu({
    target: "#ExpectedDocuments",
    showOn: "click",
    filter: "td:first-child",
    select: function (e) {
        var row = $(e.target).parent()[0];
        var grid = $("#ExpectedDocuments").data("kendoGrid");
        var tr = $(e.target).closest("tr");
        var data = grid.dataItem(tr);
        var item = e.item.id;
        switch (item) {
            case "addRow":
                grid.addRow();
                break;
            case "editRowExpDoc":
                grid.editRow(row);
                break;
            case "removeRow":
                DisplayDeleteConfirmationContextMenu(e, data, "ExpectedDocuments");
                break;
            default:
                break;
        };
    }
});
var downloadURL = $("#DownloadFileUrl").val();
function GetExpectedDocuments(file, filename, fileGuid) {
    $('#pdfDocumetsExpected').show();
    $('#othersDocumetsExpected').hide();
    $('#ExpectedDocumentModalLabel').html(filename);
    $('#embDocumentExpected').attr('src', file.substring(1, file.length));
    file = downloadURL + '?applicationNumber=' + $("#ApplicationNumber").val() + '&fileGuid=' + fileGuid + '&filename=' + filename;
    $('#ExpectedDocumentDownload').attr('href', file);
};
function GetExpectedDocumentsFile(file, filename, fileGuid) {
    file = file.replace('~', '');
    $('#pdfDocumetsExpected').hide();
    $('#othersDocumetsExpected').show();
    $('#ExpectedDocumentModalLabel').html(filename);
    $('#embDocumentExpected').attr('src', file);
    file = downloadURL + '?applicationNumber=' + $("#ApplicationNumber").val() + '&fileGuid=' + fileGuid + '&filename=' + filename;
    $('#ExpectedDocumentDownload').attr('href', file);
    $('#ExpectedDocumentDownloadOthers').attr('href', file);
};

function onExpectedFileSuccess(e) {
    $.map(e.files, function (file) {
        var info = file.name;
        if (file.size > 0) {
            info += " (" + Math.ceil(file.size / 1024) + " KB)";
        }
        $('#FileName').val(file.name);
        if ($('body > div.k-widget.k-window.k-display-inline-flex > div.k-popup-edit-form.k-window-content > div > div:nth-child(1) > div:nth-child(5) > div.k-tooltip.k-tooltip-error.k-validator-tooltip.k-invalid-msg.field-validation-error').length > 0) {
            $('body > div.k-widget.k-window.k-display-inline-flex > div.k-popup-edit-form.k-window-content > div > div:nth-child(1) > div:nth-child(5) > div.k-tooltip.k-tooltip-error.k-validator-tooltip.k-invalid-msg.field-validation-error').hide();
        }
    })
}