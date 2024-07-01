using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "pepdetailsfamilymembersassociates")]
	public class Pepdetailsfamilymembersassociates
	{

		[XmlElement(ElementName = "pepfamilyassociate")]
		public List<Pepfamilyassociate> Pepfamilyassociate { get; set; }
	}
}
