using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "bankrelationship")]
	public class Bankrelationship
	{

		[XmlElement(ElementName = "othereuropeanbankinginstitution")]
		public string Othereuropeanbankinginstitution { get; set; }

		[XmlElement(ElementName = "nameofeuropeanbankinginstitution")]
		public string Nameofeuropeanbankinginstitution { get; set; }

		[XmlElement(ElementName = "countryofeuropeanbankinginstitution")]
		public string Countryofeuropeanbankinginstitution { get; set; }
	}
}
