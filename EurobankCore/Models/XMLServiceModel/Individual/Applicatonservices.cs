using System.Collections.Generic;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "applicatonservices")]
	public class Applicatonservices
	{

		[XmlElement(ElementName = "applicatonservice")]
		public List<string> Applicatonservice { get; set; }
	}
}
