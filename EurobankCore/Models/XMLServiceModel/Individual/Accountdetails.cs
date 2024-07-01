using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eurobank.Models.XMLServiceModel.Individual
{
	[XmlRoot(ElementName = "accountdetails")]
	public class Accountdetails
	{

		[XmlElement(ElementName = "account")]
		public List<Account> Account { get; set; }
	}
}
