using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

// using System.Xml.Serialization;
// XmlSerializer serializer = new XmlSerializer(typeof(Root));
// using (StringReader reader = new StringReader(xml))
// {
//    var test = (Root)serializer.Deserialize(reader);
// }

namespace Eurobank.Models.XMLServiceModel.Individual
{
	public class ApplicationIndividualXMLModel
	{
		public Root ApplictionXMLModel { get; set; }
	}
}





