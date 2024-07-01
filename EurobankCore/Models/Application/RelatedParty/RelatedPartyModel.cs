using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Application.RelatedParty.ContactDetails;
using Eurobank.Models.Application.RelatedParty.PartyRoles;
using Eurobank.Models.Application.RelatedParty.PartyRolesLegal;
using Eurobank.Models.IdentificationDetails;
using Eurobank.Models.PEPDetails;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.RelatedParty
{
    public class RelatedPartyModel
    {
        public int rowID { get; set; }
        public int Id { get; set; }
        public string NodeGUID { get; set; }
        public string NodePath { get; set; }
        public int ApplicationID { get; set; }
        public string Application_NodeGUID { get; set; }
        public string ApplicationNumber { get; set; }

        public string RelatedPartyNumber { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        public string Invited { get; set; }
        public string HIDInviteFlag { get; set; }

        public string IdVerified { get; set; }

        public string Status { get; set; }

        public string ApplicationStatus { get; set; }

        public string Introducer { get; set; }
        public string ApplicationType { get; set; }
        public string Type { get; set; }
        public string ApplicationTypeName { get; set; }
        public bool IsRelatedPartyUBO { get; set; }

        public string FirstIdentificationNumber { get; set; }

        public PersonalDetailsModel PersonalDetails { get; set; }

        public CompanyDetailsRelatedPartyModel CompanyDetails { get; set; }

        public EmploymentDetailsRelatedPartyModel EmploymentDetails { get; set; }

        public AddressDetailsModel AddressDetails { get; set; }
        public ContactDetailsViewModel ContactDetails { get; set; }
        public PartyRolesViewModel PartyRoles { get; set; }
        //public List<StepperStep> LeftMenuApplicant { get; set; }
        public LeftMenuCommon LeftMenuApplicant { get; set; }
        public PartyRolesLegalViewModel PartyRolesLegal { get; set; }
        public List<AddressDetailsModel> _lst_AddressDetailsModel { get; set; }
        public List<IdentificationDetailsViewModel> _lst_IdentificationDetailsViewModel { get; set; }
        public List<SourceOfIncomeModel> _lst_SourceOfIncomeModel { get; set; }
        public List<OriginOfTotalAssetsModel> _lst_OriginOfTotalAssetsModel { get; set; }
        public List<PepApplicantViewModel> _lst_PepApplicantViewModel { get; set; }
        public List<PepAssociatesViewModel> _lst_PepAssociatesViewModel { get; set; }
        public bool IsEdit { get; set; }

        public string hdnCitizenship { get; set; }
        public string hdnTypeofIdentification { get; set; }
        public string hdnIdentificationNumber { get; set; }
        public string hdnIssuingCountry { get; set; }
        public string hdnIssueDateTime { get; set; }
        public string hdnExpiryDateTime { get; set; }
    }
}
