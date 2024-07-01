using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "originoftotalassets")]
	public class Originoftotalassets
	{

		[XmlElement(ElementName = "id")]
		public int Id { get; set; }

		[XmlElement(ElementName = "originoftotalassets")]
		public string Originoftotalasset { get; set; }

		[XmlElement(ElementName = "otheroriginoftotalassets")]
		public object Otheroriginoftotalassets { get; set; }

		[XmlElement(ElementName = "amountoftotalassets")]
		public int Amountoftotalassets { get; set; }

		[XmlElement(ElementName = "status")]
		public string Status { get; set; }
	}

	[XmlRoot(ElementName = "originsoftotalassets")]
	public class Originsoftotalassets
	{

		[XmlElement(ElementName = "originoftotalassets")]
		public List<Originoftotalassets> Originoftotalassets { get; set; }
	}
}
