using Eurobank.Models.Application.Applicant.LegalEntity.CRS;
using Eurobank.Models.Application.Applicant.LegalEntity.FATCACRS;
using Eurobank.Models.Application.Applicant.LegalEntity;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Application.RelatedParty.ContactDetails;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eurobank.Models.Application.Applicant.LegalEntity.ContactDetails;
using Eurobank.Models.Applications.TaxDetails;
using Eurobank.Models.PEPDetails;
using Eurobank.Models.IdentificationDetails;

namespace Eurobank.Models.Application.Applicant
{
	public class ApplicantModel
	{
		public int rowID { get; set; }
		public int Id { get; set; }
		public string NodeGUID { get; set; }
		public string NodePath { get; set; }
		public int ApplicationId { get; set; }
		public string Application_NodeGUID { get; set; }
		public int ApplicantId { get; set; }
		public string ApplicationNumber { get; set; }
		public string Application_Type { get; set; }
		public string ApplicantNumber { get; set; }

		public string FullName { get; set; }

		public string Type { get; set; }

		public string Invited { get; set; }
		public string HIDInviteFlag { get; set; }


        public string IdVerified { get; set; }

		public string FirstIdentificationNumber { get; set; }

		public string Status { get; set; }
		

		public string ApplicationStatus { get; set; }

		public string Introducer { get; set; }

		public PersonalDetailsModel PersonalDetails { get; set; }

		public EmploymentDetailsModel EmploymentDetails { get; set; }

		public CompanyDetailsModel CompanyDetails { get; set; }

		public CompanyBusinessProfileModel CompanyBusinessProfile { get; set; }

		public CompanyFinancialInformationModel CompanyFinancialInformation { get; set; }

		public AddressDetailsModel AddressDetails { get; set; }
		public List<AddressDetailsModel> _lst_AddressDetails { get; set; }
		//public StepperStepFactory LeftMenuApplicant { get; set; }
		//public List<StepperStep> LeftMenuApplicant { get; set; }
		public LeftMenuCommon LeftMenuApplicant { get; set; }
		public ContactDetailsViewModel ContactDetails { get; set; }
		public FATCACRSDetailsModel FATCACRSDetails { get; set; }
		public CRSDetailsModel CRSDetails { get; set; }
		public ContactDetailsLegalModel ContactDetailsLegal { get; set; }
		public List<TaxDetailsViewModel> _lst_TaxDetails { get; set; }
		public List<PepApplicantViewModel> _lst_PepApplicantViewModel { get; set; }
		public List<PepAssociatesViewModel> _lst_PepAssociatesViewModel { get; set; }
		public List<IdentificationDetailsViewModel> _lst_IdentificationDetails { get; set; }
		public List<SourceOfIncomeModel> _lst_SourceOfIncomeModel { get; set; }
		public List<OriginOfTotalAssetsModel> _lst_OriginOfTotalAssetsModel { get; set; }
		public bool IsEdit { get; set; }
		public DateTime CreatedDateTime { get; set; }

        public string hdnCitizenship { get; set; }
        public string hdnTypeofIdentification { get; set; }
        public string hdnIdentificationNumber { get; set; }
        public string hdnIssuingCountry { get; set; }
        public string hdnIssueDateTime { get; set; }
        public string hdnExpiryDateTime { get; set; }
    }
}
