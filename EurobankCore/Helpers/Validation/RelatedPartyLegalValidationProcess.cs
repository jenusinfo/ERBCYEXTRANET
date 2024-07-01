using Eurobank.Models.Application.RelatedParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
	public class RelatedPartyLegalValidationProcess
	{
        public static List<ValidationResultModel> ValidateRelatedPartyLegal(RelatedPartyModel realtedPartyModel)
        {
            List<ValidationResultModel> retVal = new List<ValidationResultModel>();

            ValidationResultModel companyDetaillsValidation = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.LEGAL_ENTITY_DETAILS
            };
            companyDetaillsValidation = RelatedPartyLegalFormBasicValidationProcess.ValidateCompanyDetails(realtedPartyModel.CompanyDetails);
            retVal.Add(companyDetaillsValidation);

            if(realtedPartyModel.CompanyDetails != null)
            {
                retVal.Add(RelatedPartyLegalGridValidationProcess.ValidateAddressDetails(realtedPartyModel.CompanyDetails.Id));
            }
            if(!realtedPartyModel.CompanyDetails.IsRelatedPartyUBO)
            retVal.Add(RelatedPartyLegalFormBasicValidationProcess.ValidatePartyRoles(realtedPartyModel.PartyRolesLegal));

            return retVal;
        }
    }
}
