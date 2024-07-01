using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "roles")]
	public class Roles
	{

		[XmlElement(ElementName = "role")]
		public List<string> Role { get; set; }
	}
}
