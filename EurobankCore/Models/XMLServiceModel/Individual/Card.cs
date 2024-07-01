using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "card")]
	public class Card
	{

		[XmlElement(ElementName = "visadebitcard")]
		public string Visadebitcard { get; set; }

		[XmlElement(ElementName = "associatedaccount")]
		public string Associatedaccount { get; set; }

		[XmlElement(ElementName = "associatedaccountccy")]
		public string Associatedaccountccy { get; set; }

		[XmlElement(ElementName = "associatedcardholder")]
		public string Associatedcardholder { get; set; }

		[XmlElement(ElementName = "fullnameoncard")]
		public string Fullnameoncard { get; set; }

		[XmlElement(ElementName = "mobileforalertscountry")]
		public int Mobileforalertscountry { get; set; }

		[XmlElement(ElementName = "mobileforalertsphone")]
		public int Mobileforalertsphone { get; set; }

		[XmlElement(ElementName = "dispatchmethodofthecard")]
		public object Dispatchmethodofthecard { get; set; }

		[XmlElement(ElementName = "deliveryaddress")]
		public string Deliveryaddress { get; set; }

		[XmlElement(ElementName = "collectedbyname")]
		public string Collectedbyname { get; set; }

		[XmlElement(ElementName = "collectedbyid")]
		public int Collectedbyid { get; set; }

		[XmlElement(ElementName = "deliveryDetails")]
		public object DeliveryDetails { get; set; }

		[XmlElement(ElementName = "status")]
		public string Status { get; set; }
	}
}
