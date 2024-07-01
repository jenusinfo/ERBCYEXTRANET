using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "address")]
	public class Address
	{

		[XmlElement(ElementName = "addresstype")]
		public string Addresstype { get; set; }

		[XmlElement(ElementName = "addressline1")]
		public string Addressline1 { get; set; }

		[XmlElement(ElementName = "addressline2")]
		public string Addressline2 { get; set; }

		[XmlElement(ElementName = "postalcode")]
		public int Postalcode { get; set; }

		[XmlElement(ElementName = "pobox")]
		public int Pobox { get; set; }

		[XmlElement(ElementName = "city")]
		public string City { get; set; }

		[XmlElement(ElementName = "country")]
		public string Country { get; set; }

		[XmlElement(ElementName = "maincorrespondenceaddress")]
		public string Maincorrespondenceaddress { get; set; }
	}

	[XmlRoot(ElementName = "addresses")]
	public class Addresses
	{

		[XmlElement(ElementName = "address")]
		public List<Address> Address { get; set; }
	}
}
