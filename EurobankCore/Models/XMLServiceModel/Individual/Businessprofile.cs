using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "businessprofile")]
	public class Businessprofile
	{

		[XmlElement(ElementName = "employementdetails")]
		public Employementdetails Employementdetails { get; set; }

		[XmlElement(ElementName = "formeremployer")]
		public Formeremployer Formeremployer { get; set; }

		[XmlElement(ElementName = "grossannualsalary")]
		public object Grossannualsalary { get; set; }

		[XmlElement(ElementName = "totalotherincome")]
		public object Totalotherincome { get; set; }

		[XmlElement(ElementName = "originsofincome")]
		public Originsofincome Originsofincome { get; set; }

		[XmlElement(ElementName = "originsoftotalassets")]
		public Originsoftotalassets Originsoftotalassets { get; set; }
	}
}
