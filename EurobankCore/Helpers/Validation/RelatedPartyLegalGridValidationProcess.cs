using Eurobank.Helpers.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
	public class RelatedPartyLegalGridValidationProcess
	{
        public static ValidationResultModel ValidateAddressDetails(int relatedPartyId)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ADDRESS_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if(relatedPartyId > 0)
            {
                var addressDetails = AddressDetailsProcess.GetRelatedPartyAddressDetailsLegal(relatedPartyId);
                if (addressDetails == null || addressDetails.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Address_RegOffice_Details;
                    lstvalidationError.Add(validationError);
                }
                if(addressDetails != null && addressDetails.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_OriginOfTotalAssets_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
                if (addressDetails != null && (addressDetails.Count(x => string.Equals(x.AddressTypeName.Trim(), "REGISTERED OFFICE", StringComparison.OrdinalIgnoreCase))) > 1)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.RelatedPartyAddressRegistered;
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
    }
}
