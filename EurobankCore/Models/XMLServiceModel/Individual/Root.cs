using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "root")]
	public class Root
	{

		[XmlElement(ElementName = "application")]
		public Application Application { get; set; }

		[XmlElement(ElementName = "applicants")]
		public Applicants Applicants { get; set; }

		[XmlElement(ElementName = "accountdetails")]
		public Accountdetails Accountdetails { get; set; }

		[XmlElement(ElementName = "carddetails")]
		public Carddetails Carddetails { get; set; }

		[XmlElement(ElementName = "ebankingdetails")]
		public Ebankingdetails Ebankingdetails { get; set; }

		[XmlElement(ElementName = "signaturemandates")]
		public Signaturemandates Signaturemandates { get; set; }

		[XmlElement(ElementName = "relatedparties")]
		public Relatedparties Relatedparties { get; set; }

		[XmlElement(ElementName = "bankexpecteddocs")]
		public Bankexpecteddocs Bankexpecteddocs { get; set; }
	}
}
