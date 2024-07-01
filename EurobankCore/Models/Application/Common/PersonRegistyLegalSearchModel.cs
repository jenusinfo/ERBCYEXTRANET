using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Common
{
    public class PersonRegistyLegalSearchModel
    {
        public int PersonRegistryId { get; set; }
        public string ApplicationNumber { get; set; }
        public string PersonType { get; set; }
        public string RegisteredName { get; set; }
        public string TradingName { get; set; }
        public string EntityType { get; set; }
        public string CountryofIncorporation { get; set; }
        public string RegistrationNumber { get; set; }
        public string DateofIncorporation { get; set; }
        public string ListingStatus { get; set; }
        public bool CorporationSharesIssuedToTheBearer { get; set; }
        public bool IstheEntitylocatedandoperatesanofficeinCyprus { get; set; }
        public string SharesIssuedToTheBearerName { get; set; }
        public string IsOfficeinCyprusName { get; set; }
        //Contact Details
        public string ContactDetailsLegal_PreferredMailingAddress { get; set; }
        public string ContactDetailsLegal_EmailAddressForSendingAlerts { get; set; }
        public string ContactDetailsLegal_PreferredCommunicationLanguage { get; set; }

        public string CreatedDate { get; set; }
        public string ModyfiedDate { get; set; }
        public string NodeAliaspath { get; set; }
    }
}
