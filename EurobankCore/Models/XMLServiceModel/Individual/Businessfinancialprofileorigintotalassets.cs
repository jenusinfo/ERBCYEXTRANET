using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "businessfinancialprofileorigintotalassets")]
	public class Businessfinancialprofileorigintotalassets
	{

		[XmlElement(ElementName = "id")]
		public object Id { get; set; }

		[XmlElement(ElementName = "originoftotalassets")]
		public object Originoftotalassets { get; set; }

		[XmlElement(ElementName = "otherdescription")]
		public object Otherdescription { get; set; }

		[XmlElement(ElementName = "amountoftotalassets")]
		public object Amountoftotalassets { get; set; }

		[XmlElement(ElementName = "status")]
		public object Status { get; set; }
	}
}
