using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "originofincome")]
	public class Originofincome
	{

		[XmlElement(ElementName = "id")]
		public int Id { get; set; }

		[XmlElement(ElementName = "originofannualincome")]
		public string Originofannualincome { get; set; }

		[XmlElement(ElementName = "otheroriginofincome")]
		public object Otheroriginofincome { get; set; }

		[XmlElement(ElementName = "incomeamount")]
		public int Incomeamount { get; set; }

		[XmlElement(ElementName = "status")]
		public string Status { get; set; }
	}

	[XmlRoot(ElementName = "originsofincome")]
	public class Originsofincome
	{

		[XmlElement(ElementName = "originofincome")]
		public List<Originofincome> Originofincome { get; set; }
	}
}
