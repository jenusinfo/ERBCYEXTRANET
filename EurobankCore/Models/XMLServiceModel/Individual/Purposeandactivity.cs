using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "purposeandactivity")]
	public class Purposeandactivity
	{

		[XmlElement(ElementName = "reasonsforopeningtheaccount")]
		public Reasonsforopeningtheaccount Reasonsforopeningtheaccount { get; set; }

		[XmlElement(ElementName = "expectednaturesofincomingandoutgoingtransactions")]
		public Expectednaturesofincomingandoutgoingtransactions Expectednaturesofincomingandoutgoingtransactions { get; set; }

		[XmlElement(ElementName = "expectedfrequencyofincomingandoutgoingtransactions")]
		public string Expectedfrequencyofincomingandoutgoingtransactions { get; set; }

		[XmlElement(ElementName = "expectedincomingamount")]
		public int Expectedincomingamount { get; set; }

		[XmlElement(ElementName = "sourcesofincomingtxns")]
		public Sourcesofincomingtxns Sourcesofincomingtxns { get; set; }

		[XmlElement(ElementName = "sourcesofoutgoingtxns")]
		public Sourcesofoutgoingtxns Sourcesofoutgoingtxns { get; set; }
	}
}
