using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "pepassociate")]
	public class Pepassociate
	{

		[XmlElement(ElementName = "id")]
		public object Id { get; set; }

		[XmlElement(ElementName = "nameofpep")]
		public object Nameofpep { get; set; }

		[XmlElement(ElementName = "surnameofpep")]
		public object Surnameofpep { get; set; }

		[XmlElement(ElementName = "relationship")]
		public object Relationship { get; set; }

		[XmlElement(ElementName = "positionorganization")]
		public object Positionorganization { get; set; }

		[XmlElement(ElementName = "country")]
		public object Country { get; set; }

		[XmlElement(ElementName = "sincewhen")]
		public object Sincewhen { get; set; }

		[XmlElement(ElementName = "untilwhen")]
		public object Untilwhen { get; set; }
		[XmlElement(ElementName = "status")]
		public object status { get; set; }
	}
}
