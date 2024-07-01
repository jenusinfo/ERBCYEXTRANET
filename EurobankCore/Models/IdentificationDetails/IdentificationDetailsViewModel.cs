using CMS.Helpers;
using Eurobank.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.IdentificationDetails
{
    public class IdentificationDetailsViewModel
    {
        public int IdentificationDetailsID { get; set; }

        [DisplayNameLocalized("Eurobank.PepApplication.DisplayName.PepApplicant.PositionOrganization", typeof(string))]
        [Display(Name = "Citizenship")]
        public int? IdentificationDetails_Citizenship { get; set; }

        [DisplayNameLocalized("Eurobank.PepApplication.DisplayName.PepApplicant.PositionOrganization", typeof(string))]
        [Display(Name = "Citizenship")]
        public string CitizenshipValue { get; set; }

        [Display(Name = "Citizenship")]
        public string IdentificationDetails_CitizenshipName { get; set; }

        [Display(Name = "Type of Identification")]
        public string IdentificationDetails_TypeOfIdentification { get; set; }
        [Display(Name = "Type of Identification")]
        public string IdentificationDetails_TypeOfIdentificationName { get; set; }
        //[Required(ErrorMessageResourceName = "IdentificationDetails_IdentificationNumber", ErrorMessageResourceType = typeof(IdentificationDetailsViewModelErrorMessage))]
        [StringLength(50)]
        [Display(Name = "Identification Number")]
        public string IdentificationDetails_IdentificationNumber { get; set; }

        [Display(Name = "Issuing Country")]
        public int? IdentificationDetails_CountryOfIssue { get; set; }

        [Display(Name = "Issuing Country")]
        public string CountryOfIssueValue { get; set; }

        [Display(Name = "Issuing Country")]
        public string IdentificationDetails_CountryOfIssueName { get; set; }

        [Display(Name = "Issue Date")]
        public Nullable<DateTime> IdentificationDetails_IssueDate { get; set; }

        [Display(Name = "Expiry Date")]
        public Nullable<DateTime> IdentificationDetails_ExpiryDate { get; set; }
        public string PnodeGUID { get; set; }
        public string NodeAliaspath { get; set; }
        public bool Status { get; set; }
        [Display(Name = "Status")]
        public string StatusName { get; set; }

    }
    public class IdentificationDetailsViewModelErrorMessage
    {
        public static string IdentificationDetails_IdentificationNumber
        {
            get
            {
                return ResHelper.GetString("Eurobank.IdentificationDetails.Error.IdentificationDetails_IdentificationNumber");
            }
        }
        public const string IdentificationDetails_CitizenshipError = "Eurobank.IdentificationDetails.Error.IdentificationDetails_Citizenship";
        public const string IdentificationDetails_TypeOfIdentificationError = "Eurobank.IdentificationDetails.Error.IdentificationDetails_TypeOfIdentification";
        public const string IdentificationDetails_CountryOfIssueError = "Eurobank.IdentificationDetails.Error.IdentificationDetails_CountryOfIssue";
        public const string IdentificationDetails_IssueDateError = "Eurobank.IdentificationDetails.Error.IdentificationDetails_IssueDate";
        public const string IdentificationDetails_ExpiryDateError = "Eurobank.IdentificationDetails.Error.IdentificationDetails_ExpiryDate";
    }
}
