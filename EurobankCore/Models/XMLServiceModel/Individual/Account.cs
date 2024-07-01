using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "account")]
	public class Account
	{

		[XmlElement(ElementName = "accounttype")]
		public string Accounttype { get; set; }

		[XmlElement(ElementName = "currency")]
		public string Currency { get; set; }

		[XmlElement(ElementName = "statementoffrequence")]
		public string Statementoffrequence { get; set; }

		[XmlElement(ElementName = "status")]
		public string Status { get; set; }
	}
}
