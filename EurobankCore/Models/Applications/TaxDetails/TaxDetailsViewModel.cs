using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Applications.TaxDetails
{
	public class TaxDetailsViewModel
	{
		public int TaxDetailsID { get; set; }
        //[Required(ErrorMessageResourceName = "TaxDetails_CountryOfTaxResidency", ErrorMessageResourceType = typeof(TaxDetailsViewModelErrorMessage))]
        [Display(Name = "Country of Tax Residency")]
        public string TaxDetails_CountryOfTaxResidency { get; set; }
        [Display(Name = "Country of Tax Residency")]
		public string TaxDetails_CountryOfTaxResidencyName { get; set; }
		[Display(Name = "Tax Identification Number")]
		[StringLength(35)]
		
		public string TaxDetails_TaxIdentificationNumber { get; set; }
		[Display(Name = "Tin Unavailable Reason")]
		public string TaxDetails_TinUnavailableReason { get; set; }
		[Display(Name = "Tin Unavailable Reason")]
		public string TaxDetails_TinUnavailableReasonName { get; set; }
		[Display(Name = "Justification For TIN Unavailability")]
		[StringLength(65)]
		public string TaxDetails_JustificationForTinUnavalability { get; set; }
		[Display(Name = "Status")]
		public string StatusName { get; set; }
		public bool Status { get; set; }
		

		
	}
	//public class TaxDetailsViewModelErrorMessage
	//{
	//	public static string TaxDetails_CountryOfTaxResidency
	//	{
	//		get
	//		{
	//			return ResHelper.GetString("Eurobank.TaxDetails.Error.TaxDetails_CountryOfTaxResidency");
	//		}
	//	}
		
	//	public const string TaxDetails_TaxIdentificationNumberError = "Eurobank.TaxDetails.Error.TaxDetails_TaxIdentificationNumber";
	//	public const string TaxDetails_TinUnavailableReasonError = "Eurobank.TaxDetails.Error.TaxDetails_TinUnavailableReason";
	//	public const string TaxDetails_JustificationForTinUnavalabilityError = "Eurobank.TaxDetails.Error.TaxDetails_JustificationForTinUnavalability";

	//}
}
