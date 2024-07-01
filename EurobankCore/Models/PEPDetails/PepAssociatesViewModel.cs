using CMS.Helpers;
using Eurobank.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.PEPDetails
{
	public class PepAssociatesViewModel
	{ 
		public int PepAssociatesID { get; set; }
		
		//[Required(ErrorMessageResourceName = "PepAssociates_FirstName", ErrorMessageResourceType = typeof(PepAssociatesViewModelErrorMassage))]
		[DisplayNameLocalized("Eurobank.PepAssociates.DisplayName.PepAssociates_FirstName", typeof(string))]
		[StringLength(50)]
		public string PepAssociates_FirstName { get; set; }
		//[Required(ErrorMessageResourceName = "PepAssociates_Surname", ErrorMessageResourceType = typeof(PepAssociatesViewModelErrorMassage))]
		[DisplayNameLocalized("Eurobank.PepAssociates.DisplayName.PepAssociates_Surname", typeof(string))]
		[StringLength(50)]
		public string PepAssociates_Surname { get; set; }
		//[Required(ErrorMessage = "Please select Relationship.")]
		[DisplayNameLocalized("Eurobank.PepAssociates.DisplayName.PepAssociates_Relationship", typeof(string))]
		public string PepAssociates_Relationship { get; set; }
		[DisplayNameLocalized("Eurobank.PepAssociates.DisplayName.PepAssociates_Relationship", typeof(string))]
		public string PepAssociates_RelationshipName { get; set; }
		//[Required(ErrorMessage = "Please enter Position/Organization.")]
		[DisplayNameLocalized("Eurobank.PepAssociates.DisplayName.PepAssociates_PositionOrganization", typeof(string))]
		[StringLength(65)]

		public string PepAssociates_PositionOrganization { get; set; }
		//[Required(ErrorMessage = "Please select Country.")]
		[DisplayNameLocalized("Eurobank.PepAssociates.DisplayName.PepAssociates_Country", typeof(string))]
		public string PepAssociates_Country { get; set; }
		[DisplayNameLocalized("Eurobank.PepAssociates.DisplayName.PepAssociates_Country", typeof(string))]
		public string PepAssociates_CountryName { get; set; }
		//[Required(ErrorMessage = "Please enter Since.")]
		[DisplayNameLocalized("Eurobank.PepAssociates.DisplayName.PepAssociates_Since", typeof(string))]
		public Nullable<DateTime> PepAssociates_Since { get; set; }
		//[Required(ErrorMessage = "Please enter Until.")]
		[DisplayNameLocalized("Eurobank.PepAssociates.DisplayName.PepAssociates_Until", typeof(string))]
		public Nullable<DateTime> PepAssociates_Until { get; set; }
		[DisplayNameLocalized("Eurobank.DisplayName.Status", typeof(string))]
		public bool Status { get; set; }
		[DisplayNameLocalized("Eurobank.DisplayName.Status", typeof(string))]
		public string StatusName { get; set; }


	}
	public class PepAssociatesViewModelErrorMassage
	{
		public static string PepAssociates_FirstName
		{
			get
			{
				return ResHelper.GetString("Eurobank.PepAssociates.Error.PepAssociates_FirstName");
			}
		}
		public static string PepAssociates_Surname
		{
			get
			{
				return ResHelper.GetString("Eurobank.PepAssociates.Error.PepAssociates_Surname");
			}
		}
		public const string PepAssociates_RelationshipError = "Eurobank.PepAssociates.Error.PepAssociates_Relationship";
		public const string PepAssociates_PositionOrganizationError = "Eurobank.PepAssociates.Error.PepAssociates_PositionOrganization";
		public const string PepAssociates_CountryError = "Eurobank.PepApplication.Error.PepApplicant_PositionOrganization.CountryError";
		public const string PepAssociates_SinceError = "Eurobank.PepApplication.Error.PepApplicant_PositionOrganization.SinceError";
		public const string PepAssociates_UntilError = "Eurobank.PepApplication.Error.PepApplicant_PositionOrganization.UntillError";
	}
}
