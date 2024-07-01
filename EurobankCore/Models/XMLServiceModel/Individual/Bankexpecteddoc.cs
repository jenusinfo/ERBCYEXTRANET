using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "bankexpecteddoc")]
	public class Bankexpecteddoc
	{

		[XmlElement(ElementName = "entitytypename")]
		public string Entitytypename { get; set; }

		[XmlElement(ElementName = "entitytype")]
		public string Entitytype { get; set; }

		[XmlElement(ElementName = "entityrole")]
		public string Entityrole { get; set; }

		[XmlElement(ElementName = "documenttype")]
		public string Documenttype { get; set; }

		[XmlElement(ElementName = "expecteddoccode")]
		public string Expecteddoccode { get; set; }

		[XmlElement(ElementName = "expecteddocprovided")]
		public string Expecteddocprovided { get; set; }

		[XmlElement(ElementName = "status")]
		public string Status { get; set; }

		[XmlElement(ElementName = "requiressignature")]
		public string Requiressignature { get; set; }

		[XmlElement(ElementName = "uploadedon")]
		public string Uploadedon { get; set; }

		[XmlElement(ElementName = "uploadedby")]
		public string Uploadedby { get; set; }
	}
}
