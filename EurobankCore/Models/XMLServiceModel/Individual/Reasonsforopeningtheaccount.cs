using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "reasonsforopeningtheaccount")]
	public class Reasonsforopeningtheaccount
	{

		[XmlElement(ElementName = "reasonforopeningtheaccount")]
		public List<string> Reasonforopeningtheaccount { get; set; }
	}
}
