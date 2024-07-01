 function onGridDataBoundOriginOfTotal(e) {
        $("#OriginOfTotal .k-grid-add").html("<span class='k-icon k-i-plus-circle mr-1'></span>New").addClass("new-contact").removeClass("k-button k-button-icontext");
        $("#OriginOfTotal .k-grid-content").attr("style", "max-height: 400px");
     deleteEmptyGridRecord('OriginOfTotal');
     validateBusinessAndFinancialProfile();
     validateFinancialProfileLegal();
    }
    function addConfirmButton_OriginOfTotalAssests(e) {
        $(".k-widget k-window k-display-inline-flex").addClass("leftpopup");
        $("a.k-grid-update").addClass("SAVEASDRAFT");
        var model = e.model;
        model.Status = false;
        model.dirty = true;
        $("a.k-grid-cancel").addClass('btn btn-link');
        $("a.k-grid-update")[0].innerHTML = "SAVE AS DRAFT";
        $("a.k-grid-cancel")[0].innerHTML = "<span class='k-icon k-i-cancel'></span>CLOSE";

        $('<a class="k-button k-button-icontext k-primary k-grid-update custom"  href="#" k-icon k-i-delete"></span>SAVE & CLOSE</a>').insertBefore(".k-grid-update");
        $(".custom").click(function (e) {
            model.Status = true;

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

        var assetsName = $("#OriginOfTotalAssetsName").val();
        if (assetsName == "Other") {
            $("#SpecifyOtherOriginDIV").css("display", "block");
        }

        if (isApplicationPermisssionEdit == 'False') {
            setAllPopUpInputElementDisabled();
        }
        if ($("#hdnIsLegalEntity").val() == 'True') {
        var selectedEntityValue = $("#EntityTypeID").data("kendoDropDownList") != undefined ? $("#EntityTypeID").data("kendoDropDownList").text() : '';
        if (selectedEntityValue == "Private Limited Company" || selectedEntityValue == "Public Limited Company") {
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(9).show();// Sale of Business/Securities/Real Estate/Other Assets
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(10).show();// Corporate Earnings
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(11).show();// Loan
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(13).show();// Other
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(0).show();// Capital Contribution from Shareholder(s)

            $('#OriginOfTotalAssetsID_listbox .k-item').eq(12).hide();// Annual Contributions
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(7).hide();// Salary
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(5).hide();// Interest
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(1).hide();// Dividends
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(3).hide();// Inheritance
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(8).hide();// Sale of Business/Assets/Participation
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(2).hide();// Earnings
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(4).hide();// Inheritance/Gifts
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(6).hide();// Investments

        }
        else if (selectedEntityValue == "General Partnership" || selectedEntityValue == "Limited Partnership" || selectedEntityValue == "Limited Liability Partnership") {
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(9).show();// Sale of Business/Securities/Real Estate/Other Assets
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(10).show();// Corporate Earnings
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(11).show();// Loan
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(13).show();// Other

            $('#OriginOfTotalAssetsID_listbox .k-item').eq(0).hide();// Capital Contribution from Shareholder(s)
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(12).hide();// Annual Contributions
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(7).hide();// Salary
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(5).hide();// Interest
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(1).hide();// Dividends
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(3).hide();// Inheritance
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(8).hide();// Sale of Business/Assets/Participation
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(2).hide();// Earnings
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(4).hide();// Inheritance/Gifts
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(6).hide();// Investments
        }
        else if (selectedEntityValue == 'Provident Fund' || selectedEntityValue == 'Pension Fund') {
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(9).show();// Sale of Business/Securities/Real Estate/Other Assets
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(11).show();// Loan
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(12).show();// Annual Contributions
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(13).show();// Other

            $('#OriginOfTotalAssetsID_listbox .k-item').eq(10).hide();// Corporate Earnings
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(0).hide();// Capital Contribution from Shareholder(s)
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(7).hide();// Salary
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(5).hide();// Interest
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(1).hide();// Dividends
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(3).hide();// Inheritance
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(8).hide();// Sale of Business/Assets/Participation
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(2).hide();// Earnings
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(4).hide();// Inheritance/Gifts
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(6).hide();// Investments
        }
        else if (selectedEntityValue == 'Private Insurance Company' || selectedEntityValue == 'Public Insurance Company') {
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(9).show();// Sale of Business/Securities/Real Estate/Other Assets
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(11).show();// Loan
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(0).show();// Capital Contribution from Shareholder(s)
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(13).show();// Other

            $('#OriginOfTotalAssetsID_listbox .k-item').eq(10).hide();// Corporate Earnings
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(12).hide();// Annual Contributions
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(7).hide();// Salary
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(5).hide();// Interest
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(1).hide();// Dividends
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(3).hide();// Inheritance
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(8).hide();// Sale of Business/Assets/Participation
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(2).hide();// Earnings
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(4).hide();// Inheritance/Gifts
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(6).hide();// Investments
        }
        else if (selectedEntityValue == "Trust") {
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(13).show();// Other
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(9).show();// Sale of Business/Securities/Real Estate/Other Assets
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(7).show();// Salary
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(5).show();// Interest
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(1).show();// Dividends
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(3).show();// Inheritance

            $('#OriginOfTotalAssetsID_listbox .k-item').eq(0).hide();// Capital Contribution from Shareholder(s)
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(10).hide();// Corporate Earnings
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(12).hide();// Annual Contributions
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(11).hide();// Loan
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(8).hide();// Sale of Business/Assets/Participation
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(2).hide();// Earnings
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(4).hide();// Inheritance/Gifts
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(6).hide();// Investments
        }
        else {
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(9).show();// Sale of Business/Securities/Real Estate/Other Assets
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(10).show();// Corporate Earnings
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(11).show();// Loan
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(13).show();// Other

            $('#OriginOfTotalAssetsID_listbox .k-item').eq(12).hide();// Annual Contributions
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(0).hide();// Capital Contribution from Shareholder(s)
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(7).hide();// Salary
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(5).hide();// Interest
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(1).hide();// Dividends
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(3).hide();// Inheritance
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(8).hide();// Sale of Business/Assets/Participation
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(2).hide();// Earnings
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(4).hide();// Inheritance/Gifts
            $('#OriginOfTotalAssetsID_listbox .k-item').eq(6).hide();// Investments
            }
        }

    }
    function error_handlerOriginOfTotal(args) {
        var errors = args.errors;
        if (errors) {
            var grid = $("#OriginOfTotal").data("kendoGrid");
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

    $("#accordionOriginOfTotal").kendoPanelBar({
        expandMode: "multiple" //options are 'single' and 'multiple'. 'multiple' is the default value
    });

    var accordion = $("#accordionOriginOfTotal").data("kendoPanelBar");
    accordion.collapse("#chartSection");
    $("#context-menuOriginOfTotal").kendoContextMenu({
        target: "#OriginOfTotal",
        showOn: "click",
        filter: "td:first-child",
        select: function (e) {
            var row = $(e.target).parent()[0];
            var grid = $("#OriginOfTotal").data("kendoGrid");
            var tr = $(e.target).closest("tr"); //get the row for deletion
            var data = grid.dataItem(tr);//get the row data so it can be referred later
            var item = e.item.id;
            switch (item) {
                case "addRow":
                    grid.addRow();
                    break;
                case "editRowOOT":
                    grid.editRow(row);
                    break;
                case "removeRow":
                    //grid.removeRow(row);
                    DisplayDeleteConfirmationContextMenu(e, data, "OriginOfTotal");
                    break;
                default:
                    break;
            };
        }
    });