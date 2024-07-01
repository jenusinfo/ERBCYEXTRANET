using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "expectednaturesofincomingandoutgoingtransactions")]
	public class Expectednaturesofincomingandoutgoingtransactions
	{

		[XmlElement(ElementName = "expectednatureofincomingandoutgoingtransactions")]
		public List<string> Expectednatureofincomingandoutgoingtransactions { get; set; }
	}
}
