using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "applicant")]
	public class Applicant
	{

		[XmlElement(ElementName = "customercheck")]
		public Customercheck Customercheck { get; set; }

		[XmlElement(ElementName = "personal")]
		public Personal Personal { get; set; }

		[XmlElement(ElementName = "identifications")]
		public Identifications Identifications { get; set; }

		[XmlElement(ElementName = "addresses")]
		public Addresses Addresses { get; set; }

		[XmlElement(ElementName = "contactdetails")]
		public Contactdetails Contactdetails { get; set; }

		[XmlElement(ElementName = "taxresidencies")]
		public Taxresidencies Taxresidencies { get; set; }

		[XmlElement(ElementName = "businessprofile")]
		public Businessprofile Businessprofile { get; set; }

		[XmlElement(ElementName = "pepdetails")]
		public Pepdetails Pepdetails { get; set; }

		[XmlElement(ElementName = "bankrelationship")]
		public Bankrelationship Bankrelationship { get; set; }

		[XmlElement(ElementName = "consents")]
		public Consents Consents { get; set; }
	}

	[XmlRoot(ElementName = "applicants")]
	public class Applicants
	{

		[XmlElement(ElementName = "applicant")]
		public List<Applicant> Applicant { get; set; }
	}
}
