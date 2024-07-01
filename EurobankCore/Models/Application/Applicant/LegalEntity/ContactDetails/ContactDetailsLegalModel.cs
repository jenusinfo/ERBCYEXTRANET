using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Applicant.LegalEntity.ContactDetails
{
    public class ContactDetailsLegalModel
    {
        public int ContactDetailsLegalID { get; set; }
        [Display(Name = "Preferred Mailing Address")]
        public string ContactDetailsLegal_PreferredMailingAddress { get; set; }

        //[EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address for Sending ERB Alerts")]
       
        public string ContactDetailsLegal_EmailAddressForSendingAlerts { get; set; }
        [Display(Name = "Preferred Communication Language")]
        public string ContactDetailsLegal_PreferredCommunicationLanguage { get; set; }
        public bool ContactDetailsLegal_SaveInRegistry { get; set; }
    }
}
