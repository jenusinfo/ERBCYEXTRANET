using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "formeremployer")]
	public class Formeremployer
	{

		[XmlElement(ElementName = "formeremployername")]
		public object Formeremployername { get; set; }

		[XmlElement(ElementName = "formernatureofbusiness")]
		public object Formernatureofbusiness { get; set; }

		[XmlElement(ElementName = "formercountryofemployment")]
		public object Formercountryofemployment { get; set; }

		[XmlElement(ElementName = "formerprofession")]
		public object Formerprofession { get; set; }
	}
}
