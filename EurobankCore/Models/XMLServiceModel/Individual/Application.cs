using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "application")]
	public class Application
	{

		[XmlElement(ElementName = "general")]
		public General General { get; set; }

		[XmlElement(ElementName = "purposeandactivity")]
		public Purposeandactivity Purposeandactivity { get; set; }
	}
}
