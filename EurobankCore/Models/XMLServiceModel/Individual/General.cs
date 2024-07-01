using System.Collections.Generic;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "general")]
	public class General
	{

		[XmlElement(ElementName = "applicationnumber")]
		public string Applicationnumber { get; set; }

		[XmlElement(ElementName = "applicationstatus")]
		public string Applicationstatus { get; set; }

		[XmlElement(ElementName = "introducercif")]
		public int Introducercif { get; set; }

		[XmlElement(ElementName = "introducername")]
		public string Introducername { get; set; }

		[XmlElement(ElementName = "submittedBy")]
		public string SubmittedBy { get; set; }

		[XmlElement(ElementName = "submittedOn")]
		public string SubmittedOn { get; set; }

		[XmlElement(ElementName = "responsibleOfficer")]
		public string ResponsibleOfficer { get; set; }

		[XmlElement(ElementName = "responsiblebranch")]
		public int Responsiblebranch { get; set; }

		[XmlElement(ElementName = "applicationtype")]
		public string Applicationtype { get; set; }

		[XmlElement(ElementName = "applicatonservices")]
		public Applicatonservices Applicatonservices { get; set; }
	}
}
