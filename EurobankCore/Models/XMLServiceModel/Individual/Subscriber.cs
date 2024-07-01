using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "subscriber")]
	public class Subscriber
	{

		[XmlElement(ElementName = "subscribername")]
		public string Subscribername { get; set; }

		[XmlElement(ElementName = "accesslevel")]
		public string Accesslevel { get; set; }

		[XmlElement(ElementName = "accesstoallpersonalaccounts")]
		public string Accesstoallpersonalaccounts { get; set; }

		[XmlElement(ElementName = "automaticallyaddfuturepersonalaccounts")]
		public string Automaticallyaddfuturepersonalaccounts { get; set; }

		[XmlElement(ElementName = "receiveestatements")]
		public string Receiveestatements { get; set; }

		[XmlElement(ElementName = "status")]
		public string Status { get; set; }
	}
}
