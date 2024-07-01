$("#accordionCRSDetails").kendoPanelBar({
        expandMode: "multiple" //options are 'single' and 'multiple'. 'multiple' is the default value
    });

    var accordion = $("#accordionCRSDetails").data("kendoPanelBar");
    accordion.collapse("#chartSection");
    $(document).ready(function () {
        if ($("#CompanyCRSDetails_CRSClassification_Hdn").val() == "Financial Institution") {
            $("#CompanyCRSDetails_TypeofFinancialInstitution_DIV").css("display", "block");
        }
        else if ($("#CompanyCRSDetails_CRSClassification_Hdn").val() == "Active Non-Financial Entity (NFE)") {
            $("#CompanyCRSDetails_TypeofActiveNonFinancialEntity_DIV").css("display", "block");
        }

        let text = $("#CompanyCRSDetails_TypeofActiveNonFinancialEntity_Hdn").val();
        if (text.match(/A corporation the stock of which is regularly traded on an established securities market or a corporation which is a Related Entity of such a corporation/gi)) {
            $("#CompanyCRSDetails_NameofEstablishedSecuritiesMarket_DIV").css("display", "block");
        }
    });
    function ShowRespectedCRSField() {
        debugger;

        var selectValue = $("#CompanyCRSDetails_CRSClassification").data("kendoDropDownList").text();
        if (selectValue == "Financial Institution") {
            $("#CompanyCRSDetails_TypeofFinancialInstitution_DIV").css("display", "block");

            $("#CompanyCRSDetails_TypeofActiveNonFinancialEntity_DIV").css("display", "none");
            $("#CompanyCRSDetails_NameofEstablishedSecuritiesMarket_DIV").css("display", "none");
            var TypeofActiveNonFinancialEntity = $('#CompanyCRSDetails_TypeofActiveNonFinancialEntity').data("kendoDropDownList");
            if (TypeofActiveNonFinancialEntity != undefined) {
                TypeofActiveNonFinancialEntity.value("");
            }
            var NameofEstablishedSecuritiesMarket = $('#CRSDetails_CompanyCRSDetails_NameofEstablishedSecuritiesMarket').data("kendoTextBox");
            if (NameofEstablishedSecuritiesMarket != undefined) {
                NameofEstablishedSecuritiesMarket.value("");
            }

            var FinancialEntity = $('#CompanyCRSDetails_TypeofActiveNonFinancialEntity').data("kendoDropDownList");
            if (FinancialEntity != undefined) {
                FinancialEntity.value("");
                FinancialEntity.trigger("change");
            }
        }
        else if (selectValue == "Active Non-Financial Entity (NFE)") {
            var FinancialInstitution = $('#CompanyCRSDetails_TypeofFinancialInstitution').data("kendoDropDownList");
            if (FinancialInstitution != undefined) {
                FinancialInstitution.value("");
                FinancialInstitution.trigger("change");
            }
            $("#CompanyCRSDetails_TypeofActiveNonFinancialEntity_DIV").css("display", "block");

            $("#CompanyCRSDetails_TypeofFinancialInstitution_DIV").css("display", "none");
            $("#CompanyCRSDetails_NameofEstablishedSecuritiesMarket_DIV").css("display", "none");

            var TypeofFinancialInstitution = $("#CompanyCRSDetails_TypeofFinancialInstitution").data("kendoDropDownList");
            if (TypeofFinancialInstitution != undefined) {
                TypeofFinancialInstitution.value("");
            }
            var NameofEstablishedSecuritiesMarket = $('#CRSDetails_CompanyCRSDetails_NameofEstablishedSecuritiesMarket').data("kendoTextBox");
            if (NameofEstablishedSecuritiesMarket != undefined) {
                NameofEstablishedSecuritiesMarket.value("");
            }

            var FinancialEntity = $('#CompanyCRSDetails_TypeofActiveNonFinancialEntity').data("kendoDropDownList");
            if (FinancialEntity != undefined) {
                FinancialEntity.value("");
                FinancialEntity.trigger("change");
            }
        }
        else {
            $("#CompanyCRSDetails_TypeofFinancialInstitution_DIV").css("display", "none");
            $("#CompanyCRSDetails_TypeofActiveNonFinancialEntity_DIV").css("display", "none");
            $("#CompanyCRSDetails_NameofEstablishedSecuritiesMarket_DIV").css("display", "none");

            var TypeofFinancialInstitution = $("#CompanyCRSDetails_TypeofFinancialInstitution").data("kendoDropDownList");
            if (TypeofFinancialInstitution != undefined) {
                TypeofFinancialInstitution.value("");
            }
            var TypeofActiveNonFinancialEntity = $('#CompanyCRSDetails_TypeofActiveNonFinancialEntity').data("kendoDropDownList");
            if (TypeofActiveNonFinancialEntity != undefined) {
                TypeofActiveNonFinancialEntity.value("");
            }
            var NameofEstablishedSecuritiesMarket = $('#CRSDetails_CompanyCRSDetails_NameofEstablishedSecuritiesMarket').data("kendoTextBox");
            if (NameofEstablishedSecuritiesMarket != undefined) {
                NameofEstablishedSecuritiesMarket.value("");
            }
        }
    }
    function ShowRespectedFieldNFE() {
        debugger;
        var activeNFE = $("#CompanyCRSDetails_TypeofActiveNonFinancialEntity").data("kendoDropDownList").text();
        let text = activeNFE;
        if (text.match(/A corporation the stock of which is regularly traded on an established securities market or a corporation which is a Related Entity of such a corporation/gi)) {
            $("#CompanyCRSDetails_NameofEstablishedSecuritiesMarket_DIV").css("display", "block");
        }
        else {
            $("#CompanyCRSDetails_NameofEstablishedSecuritiesMarket_DIV").css("display", "none");

            var NameofEstablishedSecuritiesMarket = $('#CRSDetails_CompanyCRSDetails_NameofEstablishedSecuritiesMarket').data("kendoTextBox");
            if (NameofEstablishedSecuritiesMarket != undefined) {
                NameofEstablishedSecuritiesMarket.value("");
            }
        }
    }