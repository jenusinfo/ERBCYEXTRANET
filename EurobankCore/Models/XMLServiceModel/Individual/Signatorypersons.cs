using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "signatorypersons")]
	public class Signatorypersons
	{

		[XmlElement(ElementName = "signatoryperson")]
		public string Signatoryperson { get; set; }
	}
}
