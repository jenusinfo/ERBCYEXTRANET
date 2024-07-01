using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "personal")]
	public class Personal
	{

		[XmlElement(ElementName = "title")]
		public string Title { get; set; }

		[XmlElement(ElementName = "firstname")]
		public string Firstname { get; set; }

		[XmlElement(ElementName = "lastname")]
		public string Lastname { get; set; }

		[XmlElement(ElementName = "fathername")]
		public string Fathername { get; set; }

		[XmlElement(ElementName = "gender")]
		public string Gender { get; set; }

		[XmlElement(ElementName = "dateofbirth")]
		public DateTime Dateofbirth { get; set; }

		[XmlElement(ElementName = "placeofbirth")]
		public string Placeofbirth { get; set; }

		[XmlElement(ElementName = "countryofbirth")]
		public string Countryofbirth { get; set; }

		[XmlElement(ElementName = "educationlevel")]
		public string Educationlevel { get; set; }
	}
}
