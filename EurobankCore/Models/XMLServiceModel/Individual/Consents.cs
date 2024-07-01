using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "consents")]
	public class Consents
	{

		[XmlElement(ElementName = "consentforprofilingformarketingpurposes")]
		public string Consentforprofilingformarketingpurposes { get; set; }
	}
}
