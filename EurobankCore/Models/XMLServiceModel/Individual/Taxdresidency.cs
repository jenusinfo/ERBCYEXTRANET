using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "taxdresidency")]
	public class Taxdresidency
	{

		[XmlElement(ElementName = "countryoftaxresidence")]
		public string Countryoftaxresidence { get; set; }

		[XmlElement(ElementName = "taxidentificationnumber")]
		public int Taxidentificationnumber { get; set; }

		[XmlElement(ElementName = "tinunavailablereason")]
		public object Tinunavailablereason { get; set; }

		[XmlElement(ElementName = "justificationfortin")]
		public object Justificationfortin { get; set; }

		[XmlElement(ElementName = "paydefencetax")]
		public string Paydefencetax { get; set; }
	}

	[XmlRoot(ElementName = "taxresidencies")]
	public class Taxresidencies
	{

		[XmlElement(ElementName = "taxdresidency")]
		public List<Taxdresidency> Taxdresidency { get; set; }
	}
}
