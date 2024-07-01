using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.RelatedParty.ContactDetails
{
    public class ContactDetailsViewModel
    {
        public int ContactDetailsID { get; set; }
        
        public bool IsRetriveFromRegistry { get; set; } 
        public string Country_Code_MobileTelNoNumber { get; set; }
        public string hdnCountry_Code_MobileTelNoNumber { get; set; }
        [Display(Name = "Mobile Tel. No.")]
        //[Required(ErrorMessage = "Please enter a Mobile Number.")]
        public string ContactDetails_MobileTelNoNumber { get; set; }
        public string hdnContactDetails_MobileTelNoNumber { get; set; }
        
        public string Country_Code_HomeTelNoNumber { get; set; }
        public string hdnCountry_Code_HomeTelNoNumber { get; set; }
        [Display(Name = "Home Tel. No.")]
        public string ContactDetails_HomeTelNoNumber { get; set; }
        public string hdnContactDetails_HomeTelNoNumber { get; set; }
       
        public string Country_Code_WorkTelNoNumber { get; set; }
        public string hdnCountry_Code_WorkTelNoNumber { get; set; }
        [Display(Name = "Work Tel. No.")]
        public string ContactDetails_WorkTelNoNumber { get; set; }
        public string hdnContactDetails_WorkTelNoNumber { get; set; }
        
        public string Country_Code_FaxNoFaxNumber { get; set; }
        public string hdnCountry_Code_FaxNoFaxNumber { get; set; }
        [Display(Name = "Fax No.")]
        public string ContactDetails_FaxNoFaxNumber { get; set; }
        public string hdnContactDetails_FaxNoFaxNumber { get; set; }
        //[Required(ErrorMessage = "Please enter a e - mail address.")]
        [Display(Name = "Email Address")]
        //[EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string ContactDetails_EmailAddress { get; set; }
        //[Required(ErrorMessage = "Please select Correspondence Language.")]
        [Display(Name = "Preferred Communication Language")]
        public string ContactDetails_PreferredCommunicationLanguage { get; set; }
        public bool ContactDetails_ConsentForMarketingPurposes { get; set; }
        public bool ContactDetails_SaveInRegistry { get; set; }

    }
}
