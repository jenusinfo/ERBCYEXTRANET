using Eurobank.Models.Application.Applicant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
	public class ApplicantLegalValidationProcess
	{
        public static List<ValidationResultModel> ValidateApplicantLegal(ApplicantModel applicantModel)
        {
            List<ValidationResultModel> retVal = new List<ValidationResultModel>();

            ValidationResultModel companyDetaillsValidation = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.LEGAL_ENTITY_DETAILS
            };
            companyDetaillsValidation = ApplicantLegalFormBasicValidationProcess.ValidateCompanyDetails(applicantModel.CompanyDetails);
            retVal.Add(companyDetaillsValidation);

            ValidationResultModel businessProfileValidation = new ValidationResultModel()
            {
                IsValid = false,
                ApplicationModuleName = ApplicationModule.BUSINESS_PROFILE
            };
            if(applicantModel.CompanyBusinessProfile != null)
            {
                businessProfileValidation = ApplicantLegalFormBasicValidationProcess.ValidateBusinessProfile(applicantModel.CompanyDetails, applicantModel.CompanyBusinessProfile);
            }
            retVal.Add(businessProfileValidation);

            ValidationResultModel financialProfileValidation = new ValidationResultModel()
            {
                IsValid = false,
                ApplicationModuleName = ApplicationModule.FINANCIAL_INFORMATION
            };
            if(applicantModel.CompanyFinancialInformation != null)
            {
                financialProfileValidation = ApplicantLegalFormBasicValidationProcess.ValidateFinancialProfile(applicantModel.CompanyDetails,applicantModel.CompanyFinancialInformation);
            }
            retVal.Add(financialProfileValidation);

            ValidationResultModel contactDetailsValidation = new ValidationResultModel()
            {
                IsValid = false,
                ApplicationModuleName = ApplicationModule.COMMUNICATION_DETAILS
            };
            if(applicantModel.ContactDetailsLegal != null)
            {
                contactDetailsValidation = ApplicantLegalFormBasicValidationProcess.ValidateContactDetails(applicantModel.ContactDetailsLegal);
            }
            retVal.Add(contactDetailsValidation);

            ValidationResultModel crsValidation = new ValidationResultModel()
            {
                IsValid = false,
                ApplicationModuleName = ApplicationModule.CRS_DETAILS
            };
            if(applicantModel.CRSDetails != null)
            {
                crsValidation = ApplicantLegalFormBasicValidationProcess.ValidateCRSDetails(applicantModel.CRSDetails);
            }
            retVal.Add(crsValidation);

            ValidationResultModel fatcaValidation = new ValidationResultModel()
            {
                IsValid = false,
                ApplicationModuleName = ApplicationModule.FATCA_DETAILS
            };
            if(applicantModel.FATCACRSDetails != null)
            {
                fatcaValidation = ApplicantLegalFormBasicValidationProcess.ValidateFATCADetails(applicantModel.FATCACRSDetails);
            }
            retVal.Add(fatcaValidation);

            ValidationResultModel bankingRelationshipValidation = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.EXISTING_BANK_RELATIONSHIP
            };
            if(applicantModel.CompanyDetails != null)
            {
                bankingRelationshipValidation = ApplicantLegalFormBasicValidationProcess.ValidateBankingRelationshipLegal(applicantModel.CompanyDetails);
                retVal.Add(ApplicantLegalGridValidationProcess.ValidateTaxDetails(applicantModel.CompanyDetails.Id, applicantModel));
                retVal.Add(ApplicantLegalGridValidationProcess.ValidateAddressDetails(applicantModel.CompanyDetails.Id, applicantModel));
                retVal.Add(ApplicantLegalGridValidationProcess.ValidateOriginOfTotalAssetsDetails(applicantModel.CompanyDetails.Id));
            }
            retVal.Add(bankingRelationshipValidation);
            

            return retVal;
        }
    }
}
