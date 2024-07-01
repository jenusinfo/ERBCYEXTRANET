using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "customercheck")]
	public class Customercheck
	{

		[XmlElement(ElementName = "customerexists")]
		public string Customerexists { get; set; }

		[XmlElement(ElementName = "customercode")]
		public object Customercode { get; set; }
	}
}
