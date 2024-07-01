using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "contactdetails")]
	public class Contactdetails
	{

		[XmlElement(ElementName = "countrycodemobile")]
		public string Countrycodemobile { get; set; }

		[XmlElement(ElementName = "mobilenumber")]
		public string Mobilenumber { get; set; }

		[XmlElement(ElementName = "countrycodehome")]
		public string Countrycodehome { get; set; }

		[XmlElement(ElementName = "homenumber")]
		public string Homenumber { get; set; }

		[XmlElement(ElementName = "countrycodework")]
		public string Countrycodework { get; set; }

		[XmlElement(ElementName = "worknumber")]
		public string Worknumber { get; set; }

		[XmlElement(ElementName = "countrycodefax")]
		public string Countrycodefax { get; set; }

		[XmlElement(ElementName = "faxnumber")]
		public string Faxnumber { get; set; }

		[XmlElement(ElementName = "emailaddress")]
		public string Emailaddress { get; set; }

		[XmlElement(ElementName = "correspondencelanguage")]
		public string Correspondencelanguage { get; set; }
	}
}
