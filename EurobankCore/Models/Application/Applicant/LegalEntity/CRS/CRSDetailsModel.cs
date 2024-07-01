using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Application.Applicant.LegalEntity.CRS
{
    public class CRSDetailsModel
    {
        public int CompanyCRSDetailsID { get; set; }
        [Display(Name = "CRS Classification")]
        public string CompanyCRSDetails_CRSClassification { get; set; }
        [Display(Name = "Type of Active Non-Financial Entity (NFE)")]
        public string CompanyCRSDetails_TypeofActiveNonFinancialEntity { get; set; }
        [Display(Name = "Name of Established Securities Market")]
        public string CompanyCRSDetails_NameofEstablishedSecuritiesMarket { get; set; }
        [Display(Name = "Type of Financial Institution")]
        public string CompanyCRSDetails_TypeofFinancialInstitution { get; set; }

        public string CompanyCRSDetails_CRSClassification_Name { get; set; }
        public string CompanyCRSDetails_TypeofActiveNonFinancialEntity_Name { get; set; }

    }
}
