using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Registries
{
	public class AddressRegistryViewModel
	{
	}
	public class AddressRegistryModel
	{
		public int Id { get; set; }
		public int RowID { get; set; }
		public string NodeGUID { get; set; }
		public string NodeID { get; set; }
		
		[Display(Name = "Address Type")]
		public string AddressType { get; set; }
		public string AddressTypeName { get; set; }
		//[Required(ErrorMessage = "Please enter Location Name.")]
		[StringLength(50)]
		[Display(Name = "Registry Name")]
		public string LocationName { get; set; }
		//[Required(ErrorMessage = "Please enter a Street.")]
		[StringLength(50)]
		[Display(Name = "Address Line 1")]
		public string AddresLine1 { get; set; }
		[StringLength(50)]
		[Display(Name = "Address Line 2")]
		public string AddresLine2 { get; set; }
		//[Required(ErrorMessage = "Please enter a Postal Code.")]
		public string PostalCode { get; set; }
		//[Required(ErrorMessage = "Please enter a City.")]
		[Display(Name = "City")]
		public string City { get; set; }
		//[Required(ErrorMessage = "Please select country.")]
		[Display(Name = "Country")]
		public string Country { get; set; }
		[Display(Name = "Country")]
		public string CountryName { get; set; }
		[Display(Name = "P.O.Box")]
		public string POBox { get; set; }
		[Display(Name = "Created On")]
		public string CreatedDate { get; set; }
		[Display(Name = "Last Modified On")]
		public string ModyfiedDate { get; set; }
		public string CountryCode_PhoneNo { get; set; }
		public string PhoneNo { get; set; }
		public string CountryCode_FaxNo { get; set; }
		public string FaxNo { get; set; }
	}
}
