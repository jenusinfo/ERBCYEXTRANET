using Eurobank.Helpers.Validation;
using Eurobank.Models.KendoExtention;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Common
{
	public class PersonalDetailsModel
	{
		public int Id { get; set; }
		public bool IsRelatedPartyUBO { get; set; }
		public int PersonRegistryId { get; set; }

		[Required(ErrorMessage = "Please select Title.")]
		public string Title { get; set; }

		//[Required(ErrorMessage = "First Name Required.")]
		[MaxLength(50, ErrorMessage = ValidationConstant.MaxLength_FirstName_Exceeded)]
		public string FirstName { get; set; }

		//[Required(ErrorMessage = "Last Name Required.")]
		[MaxLength(50, ErrorMessage = ValidationConstant.MaxLength_LastName_Exceeded)]
		public string LastName { get; set; }

		//[Required(ErrorMessage = "Father's Name Required.")]
		[MaxLength(50, ErrorMessage = ValidationConstant.MaxLength_FatherName_Exceeded)]
		public string FathersName { get; set; }

		//[Required(ErrorMessage = "Please select Gender.")]
		public string Gender { get; set; }

		//[Required(ErrorMessage = "Date of Birth Required.")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "Please enter a valid Date of Birth")]
        public Nullable<DateTime> DateOfBirth { get; set; }

		public string HdnDateOfBirth { get; set; }

		//[Required(ErrorMessage = "Place of Birth Required.")]
		public string PlaceOfBirth { get; set; }

		//[Required(ErrorMessage = "Please select Country of Birth.")]
		public int CountryOfBirth { get; set; }

		//[Required(ErrorMessage = "Please select Education Level.")]
		public string EducationLevel { get; set; }
		[Display(Name = "Is the applicant liable to pay defence tax in Cyprus? ")]
		public bool IsLiableToPayDefenseTaxInCyprus { get; set; }
		[Display(Name = "Is the applicant liable to pay defence tax in Cyprus? ")]
		public string IsLiableToPayDefenseTaxInCyprusName { get; set; }
		[Display(Name = "Is Applicant a PEP?")]
		public bool IsPep { get; set; }

		[Display(Name = "Is Applicant a PEP?")]
		public string IsPepName { get; set; }
		[Display(Name = "Is any of Applicant's Family Member or Close Associate a PEP?")]

		public bool IsRelatedToPep { get; set; }
		[Display(Name = "Is any of Applicant's Family Member or Close Associate a PEP?")]

		public string IsRelatedToPepName { get; set; }
		[Display(Name = "Does the Applicant maintain Bank Account(s) with another Banking Institution?")]
		public bool HasAccountInOtherBank { get; set; }
		[Display(Name = "Does the Applicant maintain Bank Account(s) with another Banking Institution?")]
		public string HasAccountInOtherBankName { get; set; }
		[Display(Name = "Name of Banking Institution")]
		public string NameOfBankingInstitution { get; set; }
		[Display(Name = "Country of Banking Institution")]
		public string CountryOfBankingInstitution { get; set; }
		public string NodeGUID { get; set; }
		public string NodePath { get; set; }

		public string Type { get; set; }
		public bool IdVerified { get; set; }
		public int Invited { get; set; }
		public string Status { get; set; }

		public bool SaveInRegistry { get; set; }

		public DateTime CreatedDateTime { get; set; }
        public RadioGroupViewModel InvitedpersonforonlineIDverification { get; set; }	
		public int HIDInviteFlag { get; set; }

		public string PersonalDetails_CustomerCIF { get; set; }


	}
}
