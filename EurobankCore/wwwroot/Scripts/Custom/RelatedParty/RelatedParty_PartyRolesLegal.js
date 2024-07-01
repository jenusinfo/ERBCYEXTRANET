$("#accordionPartyRoles").kendoPanelBar({
    expandMode: "multiple"
});
var accordion = $("#accordionPartyRoles").data("kendoPanelBar");
accordion.collapse("#chartSection");
$(document).ready(function () {
    ShowRelatedRoles();
});
function ShowRelatedRoles() {

    $("#RelatedPartyRoles_IsDirector_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsAlternativeDirector_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsSecretary_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsShareholder_DIV").css("display", "none");
    /* $("#RelatedPartyRoles_IsUltimateBeneficiaryOwner_DIV").css("display", "none");*/
    $("#RelatedPartyRoles_IsAuthorisedSignatory_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsAuthorisedPerson_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsDesignatedEBankingUser_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsAuthorisedCardholder_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsAuthorisedContactPerson_DIV").css("display", "none");
    $("#RelatedPartyRoles_GeneralPartner_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsAlternateSecretery_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsBenificiary_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsChairmanOfTheBoardOfDirector_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsFounder_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsFundAdministrator_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsFundMlco_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsHolderOfManagementShares_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsManagementCompany_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsMemberOfCouncil_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsMemeberOfBoardOfDirectors_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsMemeberOfCommittee_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsPartner_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsPresidentOfCommittee_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsPresidentOfCouncil_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsProtector_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsSecretaryOfCommittee_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsSecretaryOfCouncil_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsSettlor_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsTreasurerOfBoardOfDirectors_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsTreasurerOfCommittee_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsTreasurerOfCouncil_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsTrustee_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsVicePresidentOfCommittee_DIV").css("display", "none");
    $("#RelatedPartyRoles_IsVicePresidentOfCouncil_DIV").css("display", "none");
    $("#RelatedPartyRoles_LimitedPartner_DIV").css("display", "none");

    var dropdownlistEntity = '';
    var countrylistText = '';
    dropdownlistEntity = $("#ApplicantEntityType").val();
    countrylistText = $("#ApplicantCountryOfIncorporation").val();
    var isLegalEntity = $("#IsLegal").val();

    if (dropdownlistEntity.trim() == "Private Limited Company" || dropdownlistEntity.trim() == "Public Limited Company") {
        if (countrylistText.trim().toUpperCase() == "GREECE") {
            //Clear Roles
            $("#RelatedPartyRoles_IsDirector").prop('checked', false);
            $("#RelatedPartyRoles_IsAlternativeDirector").prop('checked', false);
            $("#RelatedPartyRoles_IsSecretary").prop('checked', false);
            $("#RelatedPartyRoles_IsAlternateSecretery").prop('checked', false);
            $("#RelatedPartyRoles_IsPartner").prop('checked', false);
            $("#RelatedPartyRoles_GeneralPartner").prop('checked', false);
            $("#RelatedPartyRoles_LimitedPartner").prop('checked', false);
            $("#RelatedPartyRoles_IsPresidentOfCommittee").prop('checked', false);
            $("#RelatedPartyRoles_IsVicePresidentOfCommittee").prop('checked', false);
            $("#RelatedPartyRoles_IsSecretaryOfCommittee").prop('checked', false);
            $("#RelatedPartyRoles_IsTreasurerOfCommittee").prop('checked', false);
            $("#RelatedPartyRoles_IsMemeberOfCommittee").prop('checked', false);
            $("#RelatedPartyRoles_IsTrustee").prop('checked', false);
            $("#RelatedPartyRoles_IsSettlor").prop('checked', false);
            $("#RelatedPartyRoles_IsProtector").prop('checked', false);
            $("#RelatedPartyRoles_IsBenificiary").prop('checked', false);
            $("#RelatedPartyRoles_IsFounder").prop('checked', false);
            $("#RelatedPartyRoles_IsPresidentOfCouncil").prop('checked', false);
            $("#RelatedPartyRoles_IsVicePresidentOfCouncil").prop('checked', false);
            $("#RelatedPartyRoles_IsSecretaryOfCouncil").prop('checked', false);
            $("#RelatedPartyRoles_IsTreasurerOfCouncil").prop('checked', false);
            $("#RelatedPartyRoles_IsMemberOfCouncil").prop('checked', false);
            $("#RelatedPartyRoles_IsFundMlco").prop('checked', false);
            $("#RelatedPartyRoles_IsFundAdministrator").prop('checked', false);
            $("#RelatedPartyRoles_IsManagementCompany").prop('checked', false);
            $("#RelatedPartyRoles_IsHolderOfManagementShares").prop('checked', false);

            $("#RelatedPartyRoles_IsChairmanOfTheBoardOfDirector_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsTreasurerOfBoardOfDirectors_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsMemeberOfBoardOfDirectors_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsShareholder_DIV").css("display", "block");

            $("#RelatedPartyRoles_IsAuthorisedSignatory_DIV").css("display", "block");
            if (isLegalEntity == 'False') {
                $("#RelatedPartyRoles_IsAuthorisedPerson_DIV").css("display", "block");
                $("#RelatedPartyRoles_IsDesignatedEBankingUser_DIV").css("display", "block");
                $("#RelatedPartyRoles_IsAuthorisedCardholder_DIV").css("display", "block");
                $("#RelatedPartyRoles_IsAuthorisedContactPerson_DIV").css("display", "block");
            }
            else {
                //Clear Roles
                $("#RelatedPartyRoles_IsAuthorisedPerson").prop('checked', false);
                $("#RelatedPartyRoles_IsDesignatedEBankingUser").prop('checked', false);
                $("#RelatedPartyRoles_IsAuthorisedCardholder").prop('checked', false);
                $("#RelatedPartyRoles_IsAuthorisedContactPerson").prop('checked', false);
            }

        }
        else {
            //Clear Roles
            $("#RelatedPartyRoles_IsChairmanOfTheBoardOfDirector").prop('checked', false);
            $("#RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector").prop('checked', false);
            $("#RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector").prop('checked', false);
            $("#RelatedPartyRoles_IsTreasurerOfBoardOfDirectors").prop('checked', false);
            $("#RelatedPartyRoles_IsMemeberOfBoardOfDirectors").prop('checked', false);
            $("#RelatedPartyRoles_IsPartner").prop('checked', false);
            $("#RelatedPartyRoles_GeneralPartner").prop('checked', false);
            $("#RelatedPartyRoles_LimitedPartner").prop('checked', false);
            $("#RelatedPartyRoles_IsPresidentOfCommittee").prop('checked', false);
            $("#RelatedPartyRoles_IsVicePresidentOfCommittee").prop('checked', false);
            $("#RelatedPartyRoles_IsSecretaryOfCommittee").prop('checked', false);
            $("#RelatedPartyRoles_IsTreasurerOfCommittee").prop('checked', false);
            $("#RelatedPartyRoles_IsMemeberOfCommittee").prop('checked', false);
            $("#RelatedPartyRoles_IsTrustee").prop('checked', false);
            $("#RelatedPartyRoles_IsSettlor").prop('checked', false);
            $("#RelatedPartyRoles_IsProtector").prop('checked', false);
            $("#RelatedPartyRoles_IsBenificiary").prop('checked', false);
            $("#RelatedPartyRoles_IsFounder").prop('checked', false);
            $("#RelatedPartyRoles_IsPresidentOfCouncil").prop('checked', false);
            $("#RelatedPartyRoles_IsVicePresidentOfCouncil").prop('checked', false);
            $("#RelatedPartyRoles_IsSecretaryOfCouncil").prop('checked', false);
            $("#RelatedPartyRoles_IsTreasurerOfCouncil").prop('checked', false);
            $("#RelatedPartyRoles_IsMemberOfCouncil").prop('checked', false);
            $("#RelatedPartyRoles_IsFundMlco").prop('checked', false);
            $("#RelatedPartyRoles_IsFundAdministrator").prop('checked', false);
            $("#RelatedPartyRoles_IsManagementCompany").prop('checked', false);
            $("#RelatedPartyRoles_IsHolderOfManagementShares").prop('checked', false);

            $("#RelatedPartyRoles_IsDirector_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAlternativeDirector_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsSecretary_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAlternateSecretery_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsShareholder_DIV").css("display", "block");

            $("#RelatedPartyRoles_IsAuthorisedSignatory_DIV").css("display", "block");
            if (isLegalEntity == 'False') {
                $("#RelatedPartyRoles_IsAuthorisedPerson_DIV").css("display", "block");
                $("#RelatedPartyRoles_IsDesignatedEBankingUser_DIV").css("display", "block");
                $("#RelatedPartyRoles_IsAuthorisedCardholder_DIV").css("display", "block");
                $("#RelatedPartyRoles_IsAuthorisedContactPerson_DIV").css("display", "block");
            }
            else {
                //Clear Roles
                $("#RelatedPartyRoles_IsAuthorisedPerson").prop('checked', false);
                $("#RelatedPartyRoles_IsDesignatedEBankingUser").prop('checked', false);
                $("#RelatedPartyRoles_IsAuthorisedCardholder").prop('checked', false);
                $("#RelatedPartyRoles_IsAuthorisedContactPerson").prop('checked', false);
            }
        }
    }
    else if (dropdownlistEntity.trim() == "General Partnership") {
        //Clear Roles
        $("#RelatedPartyRoles_IsDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternativeDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretary").prop('checked', false);
        $("#RelatedPartyRoles_IsShareholder").prop('checked', false);
        /*$("#RelatedPartyRoles_IsUltimateBeneficiaryOwner").prop('checked', false);*/
        $("#RelatedPartyRoles_IsAlternateSecretery").prop('checked', false);
        $("#RelatedPartyRoles_IsChairmanOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfBoardOfDirectors").prop('checked', false);
        $("#RelatedPartyRoles_IsMemeberOfBoardOfDirectors").prop('checked', false);
        $("#RelatedPartyRoles_LimitedPartner").prop('checked', false);
        $("#RelatedPartyRoles_IsPresidentOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsVicePresidentOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsMemeberOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsTrustee").prop('checked', false);
        $("#RelatedPartyRoles_IsSettlor").prop('checked', false);
        $("#RelatedPartyRoles_IsProtector").prop('checked', false);
        $("#RelatedPartyRoles_IsBenificiary").prop('checked', false);
        $("#RelatedPartyRoles_IsFounder").prop('checked', false);
        $("#RelatedPartyRoles_IsPresidentOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsVicePresidentOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsMemberOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsFundMlco").prop('checked', false);
        $("#RelatedPartyRoles_IsFundAdministrator").prop('checked', false);
        $("#RelatedPartyRoles_IsManagementCompany").prop('checked', false);
        $("#RelatedPartyRoles_IsHolderOfManagementShares").prop('checked', false);

        $("#RelatedPartyRoles_IsPartner_DIV").css("display", "block");
        $("#RelatedPartyRoles_GeneralPartner_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsAuthorisedSignatory_DIV").css("display", "block");

        if (isLegalEntity == 'False') {
            $("#RelatedPartyRoles_IsAuthorisedPerson_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsDesignatedEBankingUser_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAuthorisedCardholder_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAuthorisedContactPerson_DIV").css("display", "block");
        }
        else {
            //Clear Roles
            $("#RelatedPartyRoles_IsAuthorisedPerson").prop('checked', false);
            $("#RelatedPartyRoles_IsDesignatedEBankingUser").prop('checked', false);
            $("#RelatedPartyRoles_IsAuthorisedCardholder").prop('checked', false);
            $("#RelatedPartyRoles_IsAuthorisedContactPerson").prop('checked', false);
        }
    }
    else if (dropdownlistEntity.trim() == "Limited Partnership" || dropdownlistEntity.trim() == "Limited Liability Partnership") {
        //Clear Roles
        $("#RelatedPartyRoles_IsDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternativeDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretary").prop('checked', false);
        $("#RelatedPartyRoles_IsShareholder").prop('checked', false);
        /*$("#RelatedPartyRoles_IsUltimateBeneficiaryOwner").prop('checked', false);*/
        $("#RelatedPartyRoles_IsAlternateSecretery").prop('checked', false);
        $("#RelatedPartyRoles_IsChairmanOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfBoardOfDirectors").prop('checked', false);
        $("#RelatedPartyRoles_IsMemeberOfBoardOfDirectors").prop('checked', false);
        $("#RelatedPartyRoles_IsPresidentOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsVicePresidentOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsMemeberOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsTrustee").prop('checked', false);
        $("#RelatedPartyRoles_IsSettlor").prop('checked', false);
        $("#RelatedPartyRoles_IsProtector").prop('checked', false);
        $("#RelatedPartyRoles_IsBenificiary").prop('checked', false);
        $("#RelatedPartyRoles_IsFounder").prop('checked', false);
        $("#RelatedPartyRoles_IsPresidentOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsVicePresidentOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsMemberOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsFundMlco").prop('checked', false);
        $("#RelatedPartyRoles_IsFundAdministrator").prop('checked', false);
        $("#RelatedPartyRoles_IsManagementCompany").prop('checked', false);
        $("#RelatedPartyRoles_IsHolderOfManagementShares").prop('checked', false);

        //$("#RelatedPartyRoles_IsPartner_DIV").css("display", "block"); //#706
        $("#RelatedPartyRoles_GeneralPartner_DIV").css("display", "block");
        $("#RelatedPartyRoles_LimitedPartner_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsAuthorisedSignatory_DIV").css("display", "block");
        if (isLegalEntity == 'False') {
            $("#RelatedPartyRoles_IsAuthorisedPerson_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsDesignatedEBankingUser_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAuthorisedCardholder_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAuthorisedContactPerson_DIV").css("display", "block");
        }
        else {
            //Clear Roles
            $("#RelatedPartyRoles_IsAuthorisedPerson").prop('checked', false);
            $("#RelatedPartyRoles_IsDesignatedEBankingUser").prop('checked', false);
            $("#RelatedPartyRoles_IsAuthorisedCardholder").prop('checked', false);
            $("#RelatedPartyRoles_IsAuthorisedContactPerson").prop('checked', false);
        }
    }
    else if (dropdownlistEntity.trim() == "Provident Fund" || dropdownlistEntity.trim() == "Pension Fund") {
        //Clear Roles
        $("#RelatedPartyRoles_IsDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternativeDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretary").prop('checked', false);
        $("#RelatedPartyRoles_IsShareholder").prop('checked', false);
        /*$("#RelatedPartyRoles_IsUltimateBeneficiaryOwner").prop('checked', false);*/
        $("#RelatedPartyRoles_IsAuthorisedCardholder").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternateSecretery").prop('checked', false);
        $("#RelatedPartyRoles_IsChairmanOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfBoardOfDirectors").prop('checked', false);
        $("#RelatedPartyRoles_IsMemeberOfBoardOfDirectors").prop('checked', false);
        $("#RelatedPartyRoles_IsPartner").prop('checked', false);
        $("#RelatedPartyRoles_GeneralPartner").prop('checked', false);
        $("#RelatedPartyRoles_LimitedPartner").prop('checked', false);
        $("#RelatedPartyRoles_IsTrustee").prop('checked', false);
        $("#RelatedPartyRoles_IsSettlor").prop('checked', false);
        $("#RelatedPartyRoles_IsProtector").prop('checked', false);
        $("#RelatedPartyRoles_IsBenificiary").prop('checked', false);
        $("#RelatedPartyRoles_IsFounder").prop('checked', false);
        $("#RelatedPartyRoles_IsPresidentOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsVicePresidentOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsMemberOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsFundMlco").prop('checked', false);
        $("#RelatedPartyRoles_IsFundAdministrator").prop('checked', false);
        $("#RelatedPartyRoles_IsManagementCompany").prop('checked', false);
        $("#RelatedPartyRoles_IsHolderOfManagementShares").prop('checked', false);

        $("#RelatedPartyRoles_IsPresidentOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsVicePresidentOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretaryOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsTreasurerOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsMemeberOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsAuthorisedSignatory_DIV").css("display", "block");
        if (isLegalEntity == 'False') {
            $("#RelatedPartyRoles_IsAuthorisedPerson_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsDesignatedEBankingUser_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAuthorisedContactPerson_DIV").css("display", "block");
        }
        else {
            //Clear Roles
            $("#RelatedPartyRoles_IsAuthorisedPerson").prop('checked', false);
            $("#RelatedPartyRoles_IsDesignatedEBankingUser").prop('checked', false);
            $("#RelatedPartyRoles_IsAuthorisedContactPerson").prop('checked', false);
        }
    }
    else if (dropdownlistEntity.trim() == "Trade Union") {
        //Clear Roles
        $("#RelatedPartyRoles_IsDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternativeDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretary").prop('checked', false);
        $("#RelatedPartyRoles_IsShareholder").prop('checked', false);
        /*$("#RelatedPartyRoles_IsUltimateBeneficiaryOwner").prop('checked', false);*/
        $("#RelatedPartyRoles_IsAuthorisedCardholder").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternateSecretery").prop('checked', false);
        $("#RelatedPartyRoles_IsChairmanOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfBoardOfDirectors").prop('checked', false);
        $("#RelatedPartyRoles_IsMemeberOfBoardOfDirectors").prop('checked', false);
        $("#RelatedPartyRoles_IsPartner").prop('checked', false);
        $("#RelatedPartyRoles_GeneralPartner").prop('checked', false);
        $("#RelatedPartyRoles_LimitedPartner").prop('checked', false);
        $("#RelatedPartyRoles_IsTrustee").prop('checked', false);
        $("#RelatedPartyRoles_IsSettlor").prop('checked', false);
        $("#RelatedPartyRoles_IsProtector").prop('checked', false);
        $("#RelatedPartyRoles_IsBenificiary").prop('checked', false);
        $("#RelatedPartyRoles_IsFounder").prop('checked', false);
        $("#RelatedPartyRoles_IsFundMlco").prop('checked', false);
        $("#RelatedPartyRoles_IsFundAdministrator").prop('checked', false);
        $("#RelatedPartyRoles_IsManagementCompany").prop('checked', false);
        $("#RelatedPartyRoles_IsHolderOfManagementShares").prop('checked', false);

        $("#RelatedPartyRoles_IsPresidentOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsVicePresidentOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretaryOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsTreasurerOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsMemeberOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsAuthorisedSignatory_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsPresidentOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsVicePresidentOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretaryOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsTreasurerOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsMemberOfCouncil_DIV").css("display", "block");
        if (isLegalEntity == 'False') {
            $("#RelatedPartyRoles_IsAuthorisedPerson_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsDesignatedEBankingUser_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAuthorisedContactPerson_DIV").css("display", "block");
        }
        else {
            $("#RelatedPartyRoles_IsAuthorisedPerson").prop('checked', false);
            $("#RelatedPartyRoles_IsDesignatedEBankingUser").prop('checked', false);
            $("#RelatedPartyRoles_IsAuthorisedContactPerson").prop('checked', false);
        }
    }
    else if (dropdownlistEntity.trim() == "Trust") {
        //debugger;
        //Clear Roles
        $("#RelatedPartyRoles_IsDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternativeDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretary").prop('checked', false);
        $("#RelatedPartyRoles_IsShareholder").prop('checked', false);
        /*$("#RelatedPartyRoles_IsUltimateBeneficiaryOwner").prop('checked', false);*/
        $("#RelatedPartyRoles_IsAuthorisedCardholder").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternateSecretery").prop('checked', false);
        $("#RelatedPartyRoles_IsChairmanOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfBoardOfDirectors").prop('checked', false);
        $("#RelatedPartyRoles_IsMemeberOfBoardOfDirectors").prop('checked', false);
        $("#RelatedPartyRoles_IsPartner").prop('checked', false);
        $("#RelatedPartyRoles_GeneralPartner").prop('checked', false);
        $("#RelatedPartyRoles_LimitedPartner").prop('checked', false);
        $("#RelatedPartyRoles_IsPresidentOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsVicePresidentOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsMemeberOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsFounder").prop('checked', false);
        $("#RelatedPartyRoles_IsPresidentOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsVicePresidentOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsMemberOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsFundMlco").prop('checked', false);
        $("#RelatedPartyRoles_IsFundAdministrator").prop('checked', false);
        $("#RelatedPartyRoles_IsManagementCompany").prop('checked', false);
        $("#RelatedPartyRoles_IsHolderOfManagementShares").prop('checked', false);

        $("#RelatedPartyRoles_IsTrustee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSettlor_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsProtector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsBenificiary_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsAuthorisedSignatory_DIV").css("display", "block");
        if (isLegalEntity == 'False') {
            $("#RelatedPartyRoles_IsDesignatedEBankingUser_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAuthorisedContactPerson_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAuthorisedPerson_DIV").css("display", "block");
        }
        else {
            $("#RelatedPartyRoles_IsDesignatedEBankingUser").prop('checked', false);
            $("#RelatedPartyRoles_IsAuthorisedContactPerson").prop('checked', false);
        }
        //if ($("#IsUBO").val() == "True") {
        //    debugger;
        //    $("#RelatedPartyRoles_IsBenificiary_DIV").css("display", "block");
        //    $("#RelatedPartyRoles_IsBenificiary").prop('checked', true);
        //    $("#RelatedPartyRoles_IsBenificiary").prop('disabled', 'disabled');
        //}
        //else {
        //    $("#RelatedPartyRoles_IsBenificiary_DIV").css("display", "none");
        //}

    }
    else if (dropdownlistEntity.trim() == "Foundation") {
        //Clear Roles
        $("#RelatedPartyRoles_IsDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternativeDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretary").prop('checked', false);
        $("#RelatedPartyRoles_IsShareholder").prop('checked', false);
        /*$("#RelatedPartyRoles_IsUltimateBeneficiaryOwner").prop('checked', false);*/
        $("#RelatedPartyRoles_IsAuthorisedCardholder").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternateSecretery").prop('checked', false);
        $("#RelatedPartyRoles_IsPartner").prop('checked', false);
        $("#RelatedPartyRoles_GeneralPartner").prop('checked', false);
        $("#RelatedPartyRoles_LimitedPartner").prop('checked', false);
        $("#RelatedPartyRoles_IsPresidentOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsVicePresidentOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsMemeberOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsTrustee").prop('checked', false);
        $("#RelatedPartyRoles_IsSettlor").prop('checked', false);
        $("#RelatedPartyRoles_IsProtector").prop('checked', false);
        $("#RelatedPartyRoles_IsPresidentOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsVicePresidentOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsMemberOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsFundMlco").prop('checked', false);
        $("#RelatedPartyRoles_IsFundAdministrator").prop('checked', false);
        $("#RelatedPartyRoles_IsManagementCompany").prop('checked', false);
        $("#RelatedPartyRoles_IsHolderOfManagementShares").prop('checked', false);

        $("#RelatedPartyRoles_IsChairmanOfTheBoardOfDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsTreasurerOfBoardOfDirectors_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsMemeberOfBoardOfDirectors_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsBenificiary_DIV").css("display", "block");
        //$("#RelatedPartyRoles_IsBenificiary").removeProp('checked');
        //$("#RelatedPartyRoles_IsBenificiary").removeProp('disabled');
        $("#RelatedPartyRoles_IsFounder_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsAuthorisedSignatory_DIV").css("display", "block");
        if (isLegalEntity == 'False') {
            $("#RelatedPartyRoles_IsAuthorisedPerson_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsDesignatedEBankingUser_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAuthorisedContactPerson_DIV").css("display", "block");
        }
        else {
            $("#RelatedPartyRoles_IsAuthorisedPerson").prop('checked', false);
            $("#RelatedPartyRoles_IsDesignatedEBankingUser").prop('checked', false);
            $("#RelatedPartyRoles_IsAuthorisedContactPerson").prop('checked', false);
        }

    }
    else if (dropdownlistEntity.trim() == "Club / Association") {
        //Clear Roles
        $("#RelatedPartyRoles_IsDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternativeDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretary").prop('checked', false);
        $("#RelatedPartyRoles_IsShareholder").prop('checked', false);
        /*$("#RelatedPartyRoles_IsUltimateBeneficiaryOwner").prop('checked', false);*/
        $("#RelatedPartyRoles_IsAuthorisedCardholder").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternateSecretery").prop('checked', false);
        $("#RelatedPartyRoles_IsPartner").prop('checked', false);
        $("#RelatedPartyRoles_GeneralPartner").prop('checked', false);
        $("#RelatedPartyRoles_LimitedPartner").prop('checked', false);
        $("#RelatedPartyRoles_IsTrustee").prop('checked', false);
        $("#RelatedPartyRoles_IsSettlor").prop('checked', false);
        $("#RelatedPartyRoles_IsProtector").prop('checked', false);
        $("#RelatedPartyRoles_IsBenificiary").prop('checked', false);
        $("#RelatedPartyRoles_IsFounder").prop('checked', false);
        $("#RelatedPartyRoles_IsFundMlco").prop('checked', false);
        $("#RelatedPartyRoles_IsFundAdministrator").prop('checked', false);
        $("#RelatedPartyRoles_IsManagementCompany").prop('checked', false);
        $("#RelatedPartyRoles_IsHolderOfManagementShares").prop('checked', false);

        $("#RelatedPartyRoles_IsPresidentOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsVicePresidentOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretaryOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsTreasurerOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsMemeberOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsChairmanOfTheBoardOfDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsTreasurerOfBoardOfDirectors_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsMemeberOfBoardOfDirectors_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsAuthorisedSignatory_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsPresidentOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsVicePresidentOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretaryOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsTreasurerOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsMemberOfCouncil_DIV").css("display", "block");
        if (isLegalEntity == 'False') {
            $("#RelatedPartyRoles_IsAuthorisedPerson_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsDesignatedEBankingUser_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAuthorisedContactPerson_DIV").css("display", "block");
        }
        else {
            $("#RelatedPartyRoles_IsAuthorisedPerson").prop('checked', false);
            $("#RelatedPartyRoles_IsDesignatedEBankingUser").prop('checked', false);
            $("#RelatedPartyRoles_IsAuthorisedContactPerson").prop('checked', false);
        }

    }
    else if (dropdownlistEntity.trim() == "City Council / Local Authority") {
        //Clear Roles
        $("#RelatedPartyRoles_IsDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternativeDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretary").prop('checked', false);
        $("#RelatedPartyRoles_IsShareholder").prop('checked', false);
        /*$("#RelatedPartyRoles_IsUltimateBeneficiaryOwner").prop('checked', false);*/
        $("#RelatedPartyRoles_IsAuthorisedCardholder").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternateSecretery").prop('checked', false);
        $("#RelatedPartyRoles_IsPartner").prop('checked', false);
        $("#RelatedPartyRoles_GeneralPartner").prop('checked', false);
        $("#RelatedPartyRoles_LimitedPartner").prop('checked', false);
        $("#RelatedPartyRoles_IsTrustee").prop('checked', false);
        $("#RelatedPartyRoles_IsSettlor").prop('checked', false);
        $("#RelatedPartyRoles_IsProtector").prop('checked', false);
        $("#RelatedPartyRoles_IsBenificiary").prop('checked', false);
        $("#RelatedPartyRoles_IsFounder").prop('checked', false);
        $("#RelatedPartyRoles_IsFundMlco").prop('checked', false);
        $("#RelatedPartyRoles_IsFundAdministrator").prop('checked', false);
        $("#RelatedPartyRoles_IsManagementCompany").prop('checked', false);
        $("#RelatedPartyRoles_IsHolderOfManagementShares").prop('checked', false);

        $("#RelatedPartyRoles_IsPresidentOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsVicePresidentOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretaryOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsTreasurerOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsMemeberOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsChairmanOfTheBoardOfDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsTreasurerOfBoardOfDirectors_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsMemeberOfBoardOfDirectors_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsPresidentOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsVicePresidentOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretaryOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsTreasurerOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsMemberOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsAuthorisedSignatory_DIV").css("display", "block");
        if (isLegalEntity == 'False') {
            $("#RelatedPartyRoles_IsAuthorisedPerson_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsDesignatedEBankingUser_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAuthorisedContactPerson_DIV").css("display", "block");
        }
        else {
            $("#RelatedPartyRoles_IsAuthorisedPerson").prop('checked', false);
            $("#RelatedPartyRoles_IsDesignatedEBankingUser").prop('checked', false);
            $("#RelatedPartyRoles_IsAuthorisedContactPerson").prop('checked', false);
        }


    }
    else if (dropdownlistEntity.trim() == "Government Organization" || dropdownlistEntity.trim() == "Semi - Government Organization") {
        //Clear Roles
        $("#RelatedPartyRoles_IsDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternativeDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretary").prop('checked', false);
        $("#RelatedPartyRoles_IsAuthorisedCardholder").prop('checked', false);
        $("#RelatedPartyRoles_IsAlternateSecretery").prop('checked', false);
        $("#RelatedPartyRoles_IsPartner").prop('checked', false);
        $("#RelatedPartyRoles_GeneralPartner").prop('checked', false);
        $("#RelatedPartyRoles_LimitedPartner").prop('checked', false);
        $("#RelatedPartyRoles_IsTrustee").prop('checked', false);
        $("#RelatedPartyRoles_IsSettlor").prop('checked', false);
        $("#RelatedPartyRoles_IsProtector").prop('checked', false);
        $("#RelatedPartyRoles_IsBenificiary").prop('checked', false);
        $("#RelatedPartyRoles_IsFounder").prop('checked', false);
        $("#RelatedPartyRoles_IsFundMlco").prop('checked', false);
        $("#RelatedPartyRoles_IsFundAdministrator").prop('checked', false);
        $("#RelatedPartyRoles_IsManagementCompany").prop('checked', false);
        $("#RelatedPartyRoles_IsHolderOfManagementShares").prop('checked', false);

        $("#RelatedPartyRoles_IsPresidentOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsVicePresidentOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretaryOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsTreasurerOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsMemeberOfCommittee_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsPresidentOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsVicePresidentOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretaryOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsTreasurerOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsMemberOfCouncil_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsChairmanOfTheBoardOfDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsTreasurerOfBoardOfDirectors_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsMemeberOfBoardOfDirectors_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsShareholder_DIV").css("display", "block");

        $("#RelatedPartyRoles_IsAuthorisedSignatory_DIV").css("display", "block");
        if (isLegalEntity == 'False') {
            $("#RelatedPartyRoles_IsAuthorisedPerson_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsDesignatedEBankingUser_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAuthorisedContactPerson_DIV").css("display", "block");
        }
        else {
            $("#RelatedPartyRoles_IsAuthorisedPerson").prop('checked', false);
            $("#RelatedPartyRoles_IsDesignatedEBankingUser").prop('checked', false);
            $("#RelatedPartyRoles_IsAuthorisedContactPerson").prop('checked', false);
        }
    }
    else if (dropdownlistEntity.trim() == "Fund") {
        //Clear Roles
        $("#RelatedPartyRoles_IsAuthorisedCardholder").prop('checked', false);
        $("#RelatedPartyRoles_IsAuthorisedContactPerson").prop('checked', false);
        $("#RelatedPartyRoles_IsChairmanOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfBoardOfDirectors").prop('checked', false);
        $("#RelatedPartyRoles_IsMemeberOfBoardOfDirectors").prop('checked', false);
        $("#RelatedPartyRoles_IsPartner").prop('checked', false);
        $("#RelatedPartyRoles_GeneralPartner").prop('checked', false);
        $("#RelatedPartyRoles_LimitedPartner").prop('checked', false);
        $("#RelatedPartyRoles_IsPresidentOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsVicePresidentOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsMemeberOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsTrustee").prop('checked', false);
        $("#RelatedPartyRoles_IsSettlor").prop('checked', false);
        $("#RelatedPartyRoles_IsProtector").prop('checked', false);
        $("#RelatedPartyRoles_IsBenificiary").prop('checked', false);
        $("#RelatedPartyRoles_IsFounder").prop('checked', false);
        $("#RelatedPartyRoles_IsPresidentOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsVicePresidentOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsMemberOfCouncil").prop('checked', false);

        $("#RelatedPartyRoles_IsDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsAlternativeDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretary_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsAlternateSecretery_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsShareholder_DIV").css("display", "block");

        $("#RelatedPartyRoles_IsFundMlco_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsFundAdministrator_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsManagementCompany_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsHolderOfManagementShares_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsAuthorisedSignatory_DIV").css("display", "block");
        if (isLegalEntity == 'False') {
            $("#RelatedPartyRoles_IsAuthorisedPerson_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsDesignatedEBankingUser_DIV").css("display", "block");
        }
        else {
            $("#RelatedPartyRoles_IsAuthorisedPerson").prop('checked', false);
            $("#RelatedPartyRoles_IsDesignatedEBankingUser").prop('checked', false);
        }

    }
    else if (dropdownlistEntity.trim() == "Private Insurance Company" || dropdownlistEntity.trim() == "Public Insurance Company") {
        //Clear Roles
        $("#RelatedPartyRoles_IsChairmanOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfBoardOfDirectors").prop('checked', false);
        $("#RelatedPartyRoles_IsMemeberOfBoardOfDirectors").prop('checked', false);
        $("#RelatedPartyRoles_IsPartner").prop('checked', false);
        $("#RelatedPartyRoles_GeneralPartner").prop('checked', false);
        $("#RelatedPartyRoles_LimitedPartner").prop('checked', false);
        $("#RelatedPartyRoles_IsPresidentOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsVicePresidentOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsMemeberOfCommittee").prop('checked', false);
        $("#RelatedPartyRoles_IsTrustee").prop('checked', false);
        $("#RelatedPartyRoles_IsSettlor").prop('checked', false);
        $("#RelatedPartyRoles_IsProtector").prop('checked', false);
        $("#RelatedPartyRoles_IsBenificiary").prop('checked', false);
        $("#RelatedPartyRoles_IsFounder").prop('checked', false);
        $("#RelatedPartyRoles_IsPresidentOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsVicePresidentOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsSecretaryOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsTreasurerOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsMemberOfCouncil").prop('checked', false);
        $("#RelatedPartyRoles_IsFundMlco").prop('checked', false);
        $("#RelatedPartyRoles_IsFundAdministrator").prop('checked', false);
        $("#RelatedPartyRoles_IsManagementCompany").prop('checked', false);
        $("#RelatedPartyRoles_IsHolderOfManagementShares").prop('checked', false);

        $("#RelatedPartyRoles_IsDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsAlternativeDirector_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsSecretary_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsAlternateSecretery_DIV").css("display", "block");
        $("#RelatedPartyRoles_IsShareholder_DIV").css("display", "block");

        $("#RelatedPartyRoles_IsAuthorisedSignatory_DIV").css("display", "block");
        if (isLegalEntity == 'False') {
            $("#RelatedPartyRoles_IsAuthorisedPerson_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsDesignatedEBankingUser_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAuthorisedCardholder_DIV").css("display", "block");
            $("#RelatedPartyRoles_IsAuthorisedContactPerson_DIV").css("display", "block");
        }
        else {
            //Clear Roles
            $("#RelatedPartyRoles_IsAuthorisedPerson").prop('checked', false);
            $("#RelatedPartyRoles_IsDesignatedEBankingUser").prop('checked', false);
            $("#RelatedPartyRoles_IsAuthorisedCardholder").prop('checked', false);
            $("#RelatedPartyRoles_IsAuthorisedContactPerson").prop('checked', false);
        }
    }
    if ($("#IsUBO").val() == "True") {
        $("#RelatedPartyRoles_IsUltimateBeneficiaryOwner_DIV").css("display", "block");
    }
    if (dropdownlistEntity.trim() == "Trust") {
        $("#RelatedPartyRoles_IsUltimateBeneficiaryOwner_DIV").css("display", "none");
    }

}