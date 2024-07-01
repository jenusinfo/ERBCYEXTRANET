using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.FormModels
{

	public class AddressViewModel
	{
		[HiddenInput(DisplayValue = false)]
		public string AddressGuid { get; set; }
		[Display(Name = "Address Type")]
		[Required(ErrorMessage = "Address type is Required")]
		[UIHint("AddressType")]
		public string AddressType { get; set; }
		[Display(Name = "Street Lane1")]
		[Required(ErrorMessage = "Street is Required")]
		public string StreetLane1 { get; set; }
		[Display(Name = "Street Lane2")]
		public string StreetLane2 { get; set; }
		[Display(Name = "City")]
		[Required(ErrorMessage = "City name is Required")]
		public string City { get; set; }
		[Display(Name = "Postal Code")]
		public string PostalCode { get; set; }
		[UIHint("CountryEditor")]
		public string Country { get; set; }
		[Display(Name = "Address Document")]
		//[Required(ErrorMessage = "Address document is Required")]
		public string AddressDocument { get; set; }
		[Display(Name = "Country")]
		[HiddenInput(DisplayValue = false)]
		public string CountryName { get; set; }
		[Display(Name = "Addres Type")]
		public string AddresTypeName { get; set; }
		
		[HiddenInput(DisplayValue = false)]
		public string ApplicationID { get; set; }
	}
}
