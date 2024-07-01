using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Applications.DebitCard
{
	public class DebitCardDetailsViewModel
	{
		public int DebitCardDetailsID { get; set; }
		//[Required(ErrorMessage = "Please select Card Type.")]
		[Display(Name = "Card Type")]
		public string DebitCardDetails_CardType { get; set; }
		[Display(Name = "Type")]
		public string DebitCardDetails_CardTypeName { get; set; }
		//[Required(ErrorMessage = "Please select Associated Account.")]
		[Display(Name = "Associated Account")]
		public string AssociatedAccount { get; set; }
		[Display(Name = "Associated Account")]
		public string AssociatedAccountName { get; set; }
		//[Required(ErrorMessage = "Please select Card holder name.")]
		[Display(Name = "Card Holder Name")]
		public string DebitCardDetails_CardholderName { get; set; }
		
		//[Required(ErrorMessageResourceName = "DebitCardDetails_FullName", ErrorMessageResourceType = typeof(DebitCardDetailsViewModelErrorMessage))]
		[Display(Name = "Full Name as it will appear on card")]
		public string DebitCardDetails_FullName { get; set; }
		public string Country_Code { get; set; }
		[Display(Name = "Mobile of SMS Alerts")]
		//[Required(ErrorMessage = "Please enter Mobile Number.")]
		//[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Mobile Number.")]
		public string DebitCardDetails_MobileNumber { get; set; }

		[Display(Name = "Company's Name as it will appear on Card")]
		public string DebitCardDetails_CompanyNameAppearOnCard { get; set; }
		
		[Display(Name = "Dispatch Method")]
		//[Required(ErrorMessage = "Please select Dispatch Method.")]
		public string DebitCardDetails_DispatchMethod { get; set; }
		[Display(Name = "Dispatch Method")]
		public string DebitCardDetails_DispatchMethodName { get; set; }
		[Display(Name = "Delivery Address")]
		//[Required(ErrorMessage = "Please select Delivery Address.")]
		public string DebitCardDetails_DeliveryAddress { get; set; }
		[Display(Name = "Other Delivery Address")]
		//[Required(ErrorMessage = "Please select Delivery Address.")]
		public string DebitCardDetails_OtherDeliveryAddress { get; set; }
		[Display(Name = "Collected By")]
		//[Required(ErrorMessage = "Please select CollectedBy.")]
		public string DebitCardDetails_CollectedBy { get; set; }
		//[Required(ErrorMessage = "Please select Identity Number.")]
		[Display(Name = "Identity / Passport Number")]
		public string DebitCardDetails_CardHolderIdentityNumber { get; set; }
		[Display(Name = "Identity / Passport Number")]
		public string DebitCardDetails_IdentityNumber { get; set; }
		[Display(Name = "Delivery Details")]
		public string DebitCardDetails_DeliveryDetails { get; set; }
		public bool DebitCardDetails_Status { get; set; }
		[Display(Name = "Status")]
		public string DebitCardDetails_StatusName { get; set; }
		[Display(Name ="Collected By Name")]
		public string DebitCardDetails_CollectedByName { get; set; }
		[Display(Name = "Country of Issue")]
		public string DebitCardDetails_CardHolderCountryOfIssue { get; set; }
	}
	public class DebitCardDetailsViewModelErrorMessage
	{
		public static string DebitCardDetails_FullName
		{
			get
			{
				return ResHelper.GetString("Eurobank.DebitCardDetails.Error.DebitCardDetails_FullName");
			}
		}
		public const string DebitCardDetails_CardTypeError = "Eurobank.DebitCardDetails.Error.DebitCardDetails_CardType";
		public const string DebitCardDetails_CardholderNameError = "Eurobank.DebitCardDetails.Error.DebitCardDetails_CardholderName";
		public const string DebitCardDetails_MobileNumberError = "Eurobank.DebitCardDetails.Error.DebitCardDetails_MobileNumber";
		public const string DebitCardDetails_DispatchMethodError = "Eurobank.DebitCardDetails.Error.DebitCardDetails_DispatchMethod";
		public const string DebitCardDetails_DeliveryAddressError = "Eurobank.DebitCardDetails.Error.DebitCardDetails_DeliveryAddress";
		public const string DebitCardDetails_CollectedByError = "Eurobank.DebitCardDetails.Error.DebitCardDetails_CollectedBy";
		public const string DebitCardDetails_IdentityNumberError = "Eurobank.DebitCardDetails.Error.DebitCardDetails_IdentityNumber";
		public const string DebitCardDetails_Country_CodeError = "Eurobank.DebitCardDetails.Error.DebitCardDetails_Country_Code";

	}
}
