using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "employementdetails")]
	public class Employementdetails
	{

		[XmlElement(ElementName = "employmentstatus")]
		public string Employmentstatus { get; set; }

		[XmlElement(ElementName = "profession")]
		public string Profession { get; set; }

		[XmlElement(ElementName = "yearsinbusiness")]
		public int Yearsinbusiness { get; set; }

		[XmlElement(ElementName = "employername")]
		public string Employername { get; set; }

		[XmlElement(ElementName = "natureofbusiness")]
		public string Natureofbusiness { get; set; }
	}
}
