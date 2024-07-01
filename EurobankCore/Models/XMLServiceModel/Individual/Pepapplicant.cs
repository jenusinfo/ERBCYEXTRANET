using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "pepapplicant")]
	public class Pepapplicant
	{

		[XmlElement(ElementName = "id")]
		public int Id { get; set; }

		[XmlElement(ElementName = "positionorganization")]
		public object Positionorganization { get; set; }

		[XmlElement(ElementName = "country")]
		public object Country { get; set; }

		[XmlElement(ElementName = "sincewhen")]
		public object Sincewhen { get; set; }

		[XmlElement(ElementName = "untilwhen")]
		public object Untilwhen { get; set; }
	}
}
