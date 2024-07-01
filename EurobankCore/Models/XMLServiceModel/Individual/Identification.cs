using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "identification")]
	public class Identification
	{

		[XmlElement(ElementName = "id")]
		public int Id { get; set; }

		[XmlElement(ElementName = "citizenship")]
		public string Citizenship { get; set; }

		[XmlElement(ElementName = "typeofidentification")]
		public string Typeofidentification { get; set; }

		[XmlElement(ElementName = "identificationnumber")]
		public string Identificationnumber { get; set; }

		[XmlElement(ElementName = "issuingcountry")]
		public string Issuingcountry { get; set; }

		[XmlElement(ElementName = "issuedate")]
		public DateTime Issuedate { get; set; }

		[XmlElement(ElementName = "expirydate")]
		public DateTime Expirydate { get; set; }

		[XmlElement(ElementName = "status")]
		public string Status { get; set; }
	}

	[XmlRoot(ElementName = "identifications")]
	public class Identifications
	{

		[XmlElement(ElementName = "identification")]
		public List<Identification> Identification { get; set; }
	}
}
