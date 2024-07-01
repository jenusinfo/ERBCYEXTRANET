using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "carddetails")]
	public class Carddetails
	{

		[XmlElement(ElementName = "card")]
		public List<Card> Card { get; set; }
	}
}
