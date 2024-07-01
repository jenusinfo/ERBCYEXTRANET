using CMS.Helpers;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Application.RelatedParty.ContactDetails;
using Eurobank.Models.Applications.TaxDetails;
using Eurobank.Models.IdentificationDetails;
using Eurobank.Models.PEPDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
	public class ApplicantIndividualValidationProcess
	{
        public static List<ValidationResultModel> ValidateApplicantIndividual(ApplicantModel applicantModel)
        {
            List<ValidationResultModel> retVal = new List<ValidationResultModel>();

            ValidationResultModel personalDetaillsValidation = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.PERSONAL_DETAILS
            };
            personalDetaillsValidation = ApplicantIndividualFormBasicValidationProcess.ValidatePersonalDetails(applicantModel.PersonalDetails);
            retVal.Add(personalDetaillsValidation);

            ValidationResultModel isPepDetaillsValidation = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.PEP_DETAILS_APPLICANT
            };
            isPepDetaillsValidation = ApplicantIndividualFormBasicValidationProcess.ValidateIsPep(applicantModel.PersonalDetails);
            retVal.Add(isPepDetaillsValidation);

            ValidationResultModel businessAndFinancialProfileValidation = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.BUSINESS_AND_FINANCIAL_PROFILE_EMPLOYMENT_DETAILS
            };
            if(applicantModel.EmploymentDetails != null)
            {
                businessAndFinancialProfileValidation = ApplicantIndividualFormBasicValidationProcess.ValidateBusinessAndFinancialProfile(applicantModel.EmploymentDetails);
            }
            retVal.Add(businessAndFinancialProfileValidation);

            ValidationResultModel contactDetailsValidation = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.CONTACT_DETAILS
            };
            if(applicantModel.ContactDetails != null)
            {
                contactDetailsValidation = ApplicantIndividualFormBasicValidationProcess.ValidateContactDetails(applicantModel.ContactDetails);
            }
            retVal.Add(contactDetailsValidation);

            ValidationResultModel bankingRelationshipValidation = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.EXISTING_BANK_RELATIONSHIP
            };
            if(applicantModel.PersonalDetails != null)
            {
                //ValidationResultModel taxDetailsValidationExtended = new ValidationResultModel()
                //{
                //    IsValid = true,
                //    ApplicationModuleName = ApplicationModule.APPLICANT_TAX_DETAILS
                //};
                //taxDetailsValidationExtended = ApplicantIndividualFormBasicValidationProcess.ValidateTaxDetailsExtended(applicantModel.PersonalDetails);
                //retVal.Add(taxDetailsValidationExtended);
                bankingRelationshipValidation = ApplicantIndividualFormBasicValidationProcess.ValidateBankingRelationshipIndividual(applicantModel.PersonalDetails);
                retVal.Add(ApplicantIndividualGridValidationProcess.ValidateAddressDetails(applicantModel)); //applicantModel.PersonalDetails.Id
                retVal.Add(ApplicantIndividualGridValidationProcess.ValidateIdentificationDetails(applicantModel.PersonalDetails.Id));
                retVal.Add(ApplicantIndividualGridValidationProcess.ValidateOriginOfTotalAssets(applicantModel.PersonalDetails.Id));
                if(string.Equals(applicantModel.PersonalDetails.IsPepName, "true", StringComparison.OrdinalIgnoreCase))
                {
                    retVal.Add(ApplicantIndividualGridValidationProcess.ValidatePepDetailsApplicant(applicantModel.PersonalDetails.Id));
                }
                if(string.Equals(applicantModel.PersonalDetails.IsRelatedToPepName, "true", StringComparison.OrdinalIgnoreCase))
                {
                    retVal.Add(ApplicantIndividualGridValidationProcess.ValidatePepDetailsFmaily(applicantModel.PersonalDetails.Id));
                }
                retVal.Add(ApplicantIndividualGridValidationProcess.ValidateSourceOfIncome(applicantModel.PersonalDetails.Id, applicantModel.EmploymentDetails));
                retVal.Add(ApplicantIndividualGridValidationProcess.ValidateTaxDetails(applicantModel.PersonalDetails.Id, applicantModel.PersonalDetails));
            }
            retVal.Add(bankingRelationshipValidation);


            return retVal;
        }
    }
}
