using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "sourceofoutgoingtxns")]
	public class Sourceofoutgoingtxns
	{

		[XmlElement(ElementName = "nameofbeneficiary")]
		public string Nameofbeneficiary { get; set; }

		[XmlElement(ElementName = "countryofbeneficiary")]
		public string Countryofbeneficiary { get; set; }

		[XmlElement(ElementName = "countryofbeneficiarybank")]
		public string Countryofbeneficiarybank { get; set; }

		[XmlElement(ElementName = "status")]
		public string Status { get; set; }
	}

	[XmlRoot(ElementName = "sourcesofoutgoingtxns")]
	public class Sourcesofoutgoingtxns
	{

		[XmlElement(ElementName = "sourceofoutgoingtxns")]
		public List<Sourceofoutgoingtxns> Sourceofoutgoingtxns { get; set; }
	}
}
