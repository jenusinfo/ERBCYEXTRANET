using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "signaturemandate")]
	public class Signaturemandate
	{

		[XmlElement(ElementName = "signatorypersons")]
		public Signatorypersons Signatorypersons { get; set; }

		[XmlElement(ElementName = "signatoryrights")]
		public string Signatoryrights { get; set; }

		[XmlElement(ElementName = "totalsignatures")]
		public int Totalsignatures { get; set; }

		[XmlElement(ElementName = "limitfrom")]
		public int Limitfrom { get; set; }

		[XmlElement(ElementName = "limitto")]
		public int Limitto { get; set; }

		[XmlElement(ElementName = "status")]
		public string Status { get; set; }
	}

	[XmlRoot(ElementName = "signaturemandates")]
	public class Signaturemandates
	{

		[XmlElement(ElementName = "signatoryrights")]
		public string Signatoryrights { get; set; }

		[XmlElement(ElementName = "signaturemandate")]
		public List<Signaturemandate> Signaturemandate { get; set; }
	}
}
