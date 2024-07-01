using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "sourceofincomingtxns")]
	public class Sourceofincomingtxns
	{

		[XmlElement(ElementName = "nameofremitter")]
		public string Nameofremitter { get; set; }

		[XmlElement(ElementName = "countryofremmiter")]
		public string Countryofremmiter { get; set; }

		[XmlElement(ElementName = "countryofremmiterbank")]
		public string Countryofremmiterbank { get; set; }

		[XmlElement(ElementName = "status")]
		public string Status { get; set; }
	}

	[XmlRoot(ElementName = "sourcesofincomingtxns")]
	public class Sourcesofincomingtxns
	{

		[XmlElement(ElementName = "sourceofincomingtxns")]
		public List<Sourceofincomingtxns> Sourceofincomingtxns { get; set; }
	}
}
