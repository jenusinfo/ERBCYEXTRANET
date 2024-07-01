using System.Collections.Generic;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "ebankingdetails")]
	public class Ebankingdetails
	{

		[XmlElement(ElementName = "subscriber")]
		public List<Subscriber> Subscriber { get; set; }
	}
}
