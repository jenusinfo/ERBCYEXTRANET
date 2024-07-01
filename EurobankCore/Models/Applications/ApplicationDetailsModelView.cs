using Eurobank.Models.KendoExtention;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Applications
{
	public class ApplicationDetailsModelView
	{
		public int ApplicationDetailsID { get; set; }
		//[Required(ErrorMessage = "Please enter Location Name.")]
		//[StringLength(50)]
		[Display(Name = "Application Number")]
		public string ApplicationDetails_ApplicationNumber { get; set; }
		[Required(ErrorMessage = "Please select Application Type.")]
		[Display(Name = "Application Type")]
		public string ApplicationDetails_ApplicationType { get; set; }
		[Display(Name = "Application Type")]
		public string ApplicationDetails_ApplicationTypeName { get; set; }
		[Display(Name = "Products & Services")]
		//[StringLength(200)]
		public string ApplicationDetails_ApplicatonServices { get; set; }
		[Display(Name = "Products & Services")]
		//[Required(ErrorMessage = "Please check Products & Services.")]
		public CheckBoxGroupViewModel ApplicationDetails_ApplicatonServicesGroup { get; set; }
		[Display(Name = "Applicaton Services")]
		public string ApplicationDetails_ApplicatonServicesName { get; set; }
		//[Required(ErrorMessage = "Please enter Application Status.")]
		[Display(Name = "Application Status")]
		//[StringLength(255)]
		public string ApplicationDetails_ApplicationStatus { get; set; }

		public string ApplicationDetails_ApplicationStatusName { get; set; }

		//[Required(ErrorMessage = "Please enter Introducer Name.")]
		[Display(Name = "Introducer Name")]
		//[StringLength(255)]
		public string ApplicationDetails_IntroducerName { get; set; }
		public string ApplicationDetails_Introducer { get; set; }
		//[Required(ErrorMessage = "Please enter Introducer CIF.")]
		[Display(Name = "Introducer CIF")]
		[StringLength(40)]
		public string ApplicationDetails_IntroducerCIF { get; set; }
		[Required(ErrorMessage = "Please select Responsible Officer.")]
		[Display(Name = "Responsible Officer")]
		public string ApplicationDetails_ResponsibleOfficer { get; set; }
		[Required(ErrorMessage = "Please enter Responsible Banking Center.")]
		[Display(Name = "Responsible Banking Center")]
		//[StringLength(100)]
		public string ApplicationDetails_ResponsibleBankingCenter { get; set; }

		public string ApplicationDetails_ResponsibleBankingCenterUnit { get; set; }

		public int ApplicationDetails_DocumentSubmittedByUserID { get; set; }
		[Display(Name = "Current Stage")]
		public string ApplicationDetails_CurrentStage { get; set; }
		public string ApplicationDetails_PreviousStage { get; set; }
		[Display(Name = "Submitted By")]
		public string ApplicationDetails_SubmittedBy { get; set; }
		[Display(Name = "Submitted On")]
		public string ApplicationDetails_SubmittedOn { get; set; }
		public string ApplicationDetails_CreatedOn { get; set; }
		public string IdentificationNumber { get; set; }

		public bool IsView { get; set; }

		public bool IsEdit { get; set; }
		public bool IsBankDocAttachmentAllowed { get; set; }
		public bool IsExpectedDocAttachmentAllowed { get; set; }
		public bool IsDecissionCommentsAllowed { get; set; }
		public bool IsVisibleSaveAsDraftButton { get; set; }
		public bool IsVisibleSubmitButton { get; set; }
		public bool IsEditingByIntroducer { get; set; }

		public string FullNameOfApplicant { get; set; }
		public string ApplicantIdentificationNumber { get; set; }
		public string Application_NodeGUID { get; set; }
		public bool IsLegalOnly { get; set; }
	}
	public class ApplicatonServices
	{
		public string NodeGUID { get; set; }
		public string NodeName { get; set; }
	}
}
