using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "relatedparty")]
	public class Relatedparty
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

		[XmlElement(ElementName = "businessprofile")]
		public Businessprofile Businessprofile { get; set; }

		[XmlElement(ElementName = "businessfinancialprofileoriginannualincome")]
		public Businessfinancialprofileoriginannualincome Businessfinancialprofileoriginannualincome { get; set; }

		[XmlElement(ElementName = "businessfinancialprofileorigintotalassets")]
		public Businessfinancialprofileorigintotalassets Businessfinancialprofileorigintotalassets { get; set; }

		[XmlElement(ElementName = "pepdetailsapplicants")]
		public Pepdetailsapplicants Pepdetailsapplicants { get; set; }

		[XmlElement(ElementName = "pepdetailsfamilymembersassociates")]
		public Pepdetailsfamilymembersassociates Pepdetailsfamilymembersassociates { get; set; }

		[XmlElement(ElementName = "roles")]
		public Roles Roles { get; set; }
	}

	[XmlRoot(ElementName = "relatedparties")]
	public class Relatedparties
	{

		[XmlElement(ElementName = "relatedparty")]
		public List<Relatedparty> Relatedparty { get; set; }
	}
}
