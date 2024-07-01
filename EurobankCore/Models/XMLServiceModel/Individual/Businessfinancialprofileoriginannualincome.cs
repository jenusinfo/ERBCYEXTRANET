using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "businessfinancialprofileoriginannualincome")]
	public class Businessfinancialprofileoriginannualincome
	{

		[XmlElement(ElementName = "id")]
		public object Id { get; set; }

		[XmlElement(ElementName = "originofannualincome")]
		public object Originofannualincome { get; set; }

		[XmlElement(ElementName = "otherdescription")]
		public object Otherdescription { get; set; }

		[XmlElement(ElementName = "incomeamount")]
		public object Incomeamount { get; set; }

		[XmlElement(ElementName = "status")]
		public object Status { get; set; }
	}
}
