using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "pepdetailsapplicants")]
	public class Pepdetailsapplicants
	{

		[XmlElement(ElementName = "pepapplicant")]
		public List<Pepapplicant> Pepapplicant { get; set; }
	}
}
