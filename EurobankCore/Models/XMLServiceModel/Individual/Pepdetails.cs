using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "pepdetails")]
	public class Pepdetails
	{

		[XmlElement(ElementName = "isapplicantapep")]
		public string Isapplicantapep { get; set; }

		[XmlElement(ElementName = "pepapplicant")]
		public List<Pepapplicant> Pepapplicant { get; set; }

		[XmlElement(ElementName = "isfamilymemberassociatepepe")]
		public string Isfamilymemberassociatepepe { get; set; }

		[XmlElement(ElementName = "pepassociate")]
		public List<Pepassociate> Pepassociate { get; set; }
	}
}
