using CMS.Helpers;
using Eurobank.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.PEPDetails
{
	public class PepApplicantViewModel
	{
		public int PepApplicantID { get; set; }
		[DisplayNameLocalized("Eurobank.PepApplication.DisplayName.PepApplicant.FirstName", typeof(string))]
		public string PepApplicant_FirstName { get; set; }
		[DisplayNameLocalized("Eurobank.PepApplication.DisplayName.PepApplicant.Surname", typeof(string))]
		public string PepApplicant_Surname { get; set; }
		//[Required(ErrorMessageResourceName = "PepApplicant_PositionOrganization", ErrorMessageResourceType = typeof(PepApplicantViewModelErrorMassage))]
		[DisplayNameLocalized("Eurobank.PepApplication.DisplayName.PepApplicant.PositionOrganization", typeof(string))]
		[StringLength(65)]
		public string PepApplicant_PositionOrganization { get; set; }
		//[Required(ErrorMessage = "Please select Country.")]
		[DisplayNameLocalized("Eurobank.PepApplication.DisplayName.PepApplicant.Country", typeof(string))]
		public string PepApplicant_Country { get; set; }
		[DisplayNameLocalized("Eurobank.PepApplication.DisplayName.PepApplicant.Country", typeof(string))]
		public string PepApplicant_CountryName { get; set; }
		//[Required(ErrorMessage = "Please enter Since.")]
		[Display(Name = "Since")]
		[DisplayNameLocalized("Eurobank.PepApplication.DisplayName.PepApplicant.Since", typeof(string))]
		public Nullable< DateTime> PepApplicant_Since { get; set; }
		//[Required(ErrorMessage = "Please enter Until.")]
		[DisplayNameLocalized("Eurobank.PepApplication.DisplayName.PepApplicant.Untill", typeof(string))]
		public Nullable<DateTime> PepApplicant_Untill { get; set; }
		public bool Status { get; set; }
		[DisplayNameLocalized("Eurobank.DisplayName.Status", typeof(string))]
		public string StatusName { get; set; }
	}
	public class PepApplicantViewModelErrorMassage
	{
		public static string PepApplicant_PositionOrganization
		{
			get
			{
				return ResHelper.GetString("Eurobank.PepApplication.Error.PepApplicant_PositionOrganization");
			}
		}
		public const string PepApplicant_FirstNameError = "Eurobank.PepApplication.Error.PepApplicant_PositionOrganization.FirstNameError";
		public const string PepApplicant_SurnameError = "Eurobank.PepApplication.Error.PepApplicant_PositionOrganization.SurnameError";
		public const string PepApplicant_CountryError = "Eurobank.PepApplication.Error.PepApplicant_PositionOrganization.CountryError";
		public const string PepApplicant_SinceError = "Eurobank.PepApplication.Error.PepApplicant_PositionOrganization.SinceError";
		public const string PepApplicant_UntillError = "Eurobank.PepApplication.Error.PepApplicant_PositionOrganization.UntillError";
	}
}
