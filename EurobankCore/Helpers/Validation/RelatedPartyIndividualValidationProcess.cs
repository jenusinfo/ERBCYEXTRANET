using Eurobank.Models.Application.RelatedParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
    public class RelatedPartyIndividualValidationProcess
    {
        public static List<ValidationResultModel> ValidateRelatedPartyIndividual(RelatedPartyModel realtedPartyModel, bool isRelatedPartyTypeLegal)
        {
            List<ValidationResultModel> retVal = new List<ValidationResultModel>();
            //Personal Details
            ValidationResultModel personalDetaillsValidation = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.PERSONAL_DETAILS
            };
            personalDetaillsValidation = RelatedPartyIndividualFormBasicValidationProcess.ValidatePersonalDetails(realtedPartyModel.PersonalDetails);
            retVal.Add(personalDetaillsValidation);

            ValidationResultModel isPepDetaillsValidation = new ValidationResultModel()
            {   
                IsValid = true,
                ApplicationModuleName = ApplicationModule.PEP_DETAILS_RELATED_PARTY
            };
            isPepDetaillsValidation = RelatedPartyIndividualFormBasicValidationProcess.ValidateIsPep(realtedPartyModel.PersonalDetails);
            retVal.Add(isPepDetaillsValidation);

            ValidationResultModel businessAndFinancialProfileValidation = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.RELATED_PARTY_BUSINESS_AND_FINANCIAL_PROFILE
            };
            if (realtedPartyModel.EmploymentDetails != null)
            {
                businessAndFinancialProfileValidation = RelatedPartyIndividualFormBasicValidationProcess.ValidateBusinessProfile(realtedPartyModel.EmploymentDetails);
            }
            retVal.Add(businessAndFinancialProfileValidation);

            ValidationResultModel contactDetailsValidation = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.CONTACT_DETAILS
            };
            if (realtedPartyModel.ContactDetails != null)
            {
                contactDetailsValidation = RelatedPartyIndividualFormBasicValidationProcess.ValidateContactDetails(realtedPartyModel.ContactDetails);
            }
            retVal.Add(contactDetailsValidation);

            if (realtedPartyModel.PersonalDetails != null)
            {
                if (realtedPartyModel.PersonalDetails.IsRelatedPartyUBO)
                {
                    retVal.Add(RelatedPartyIndividualGridValidationProcess.ValidateAddressDetailsUBO(realtedPartyModel.PersonalDetails.Id, realtedPartyModel.EmploymentDetails.EmploymentStatusName));
                }
                else
                {
                    retVal.Add(RelatedPartyIndividualGridValidationProcess.ValidateAddressDetails(realtedPartyModel.PersonalDetails.Id));
                }
                retVal.Add(RelatedPartyIndividualGridValidationProcess.ValidateIdentificationDetails(realtedPartyModel.PersonalDetails.Id));
                if (realtedPartyModel.PersonalDetails.IsRelatedPartyUBO)
                {
                    retVal.Add(RelatedPartyIndividualGridValidationProcess.ValidateOriginOfTotalAssets(realtedPartyModel.PersonalDetails.Id));
                }
                if (string.Equals(realtedPartyModel.PersonalDetails.IsPepName, "true", StringComparison.OrdinalIgnoreCase))
                {
                    retVal.Add(RelatedPartyIndividualGridValidationProcess.ValidatePepDetailsApplicant(realtedPartyModel.PersonalDetails.Id));
                }
                if (string.Equals(realtedPartyModel.PersonalDetails.IsRelatedToPepName, "true", StringComparison.OrdinalIgnoreCase))
                {
                    retVal.Add(RelatedPartyIndividualGridValidationProcess.ValidatePepDetailsFmaily(realtedPartyModel.PersonalDetails.Id));
                }
                if (realtedPartyModel.PersonalDetails.IsRelatedPartyUBO)
                {
                    retVal.Add(RelatedPartyIndividualGridValidationProcess.ValidateSourceOfIncome(realtedPartyModel.PersonalDetails.Id, realtedPartyModel.EmploymentDetails));
                }
            }
            if (!realtedPartyModel.PersonalDetails.IsRelatedPartyUBO)
            {
                if (string.Equals(realtedPartyModel.ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase) && !isRelatedPartyTypeLegal)
                {
                    retVal.Add(RelatedPartyLegalFormBasicValidationProcess.ValidatePartyRoles(realtedPartyModel.PartyRolesLegal));
                }
                else
                {
                    retVal.Add(RelatedPartyIndividualFormBasicValidationProcess.ValidatePartyRoles(realtedPartyModel.PartyRoles));
                }
            }
            return retVal;
        }
    }
}
