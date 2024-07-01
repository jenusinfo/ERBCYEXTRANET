using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "bankexpecteddocs")]
	public class Bankexpecteddocs
	{

		[XmlElement(ElementName = "bankexpecteddoc")]
		public List<Bankexpecteddoc> Bankexpecteddoc { get; set; }
	}
}
