using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Applicant.LegalEntity.FATCACRS
{
    public class FATCACRSDetailsModel
    {
        public int FATCADetailsID { get; set; }
        [Display(Name = "FATCA Classification")]
        public string FATCADetails_FATCAClassification { get; set; }
        [Display(Name = "US Entity Type")]
        public string FATCADetails_USEtityType { get; set; }
        [Display(Name = "Type of Foreign Financial Institution")]
        public string FATCADetails_TypeofForeignFinancialInstitution { get; set; }
        [Display(Name = "Type of Non-Financial Foreign Entity (NFFE)")]
        public string FATCADetails_TypeofNonFinancialForeignEntity { get; set; }
        [Display(Name ="Global Intermediary Identification Number (GIIN)")]
        public string FATCADetails_GlobalIntermediaryIdentificationNumber { get; set; }
        [Display(Name ="Exemption Reason")]
        public string ExemptionReason { get; set; }
    }
}
