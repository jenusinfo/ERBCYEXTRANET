using CMS.Helpers;
using Eurobank.Helpers.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Common
{
	public class AddressDetailsModel
	{
		public int Id { get; set; }

		public string AddressType { get; set; }

		public string AddressTypeName { get; set; }
		//[Required(ErrorMessageResourceName = "AddressLine1", ErrorMessageResourceType = typeof(AddressDetailsModelErrorMessage))]
		[MaxLength(35, ErrorMessage = ValidationConstant.MaxLength_Exceeded)]
		public string AddressLine1 { get; set; }
		[MaxLength(35, ErrorMessage = ValidationConstant.MaxLength_Exceeded)]
		public string AddressLine2 { get; set; }

		public string AddressNodeGuid { get; set; }

		public string POBox { get; set; }
		[MaxLength(35, ErrorMessage = ValidationConstant.MaxLength_Exceeded)]
		public string PostalCode { get; set; }
		[MaxLength(35, ErrorMessage = ValidationConstant.MaxLength_Exceeded)]
		public string City { get; set; }

		public string Country { get; set; }

		public string CountryName { get; set; }

		public bool SaveInRegistry { get; set; }
		public bool MainCorrespondenceAddress { get; set; }
		 public string MainCorrespondenceAddressText { get; set; }

		public string Email { get; set; }
		public int AddressRegistryId { get; set; }
		[Display(Name ="Location")]
		public string LocationName { get; set; }
		public bool Status { get; set; }
		[Display(Name = "Status")]
		public string StatusName { get; set; }
		public string CountryCode_PhoneNo { get; set; }
		public string PhoneNo { get; set; }
		public string CountryCode_FaxNo { get; set; }
		public string FaxNo { get; set; }
		public bool Is_Legal { get; set; }
		[RegularExpression(@"[0-9]+$", ErrorMessage = "Negative values are not allowed")]
		public string NumberOfStaffEmployed { get; set; }
		public bool MailingAddressSame { get; set; }
	}
	public class AddressDetailsModelErrorMessage
	{
		public static string AddressLine1
		{
			get
			{
				return ResHelper.GetString("Eurobank.Common.Error.AddressLine1");
			}
		}
		public const string AddressDetails_PostalCodeError = "Eurobank.Common.Error.PostalCode";
		public const string AddressDetails_CityError = "Eurobank.Common.Error.City";
		public const string AddressDetails_CountryError = "Eurobank.Common.Error.Country";
		
	}

}
