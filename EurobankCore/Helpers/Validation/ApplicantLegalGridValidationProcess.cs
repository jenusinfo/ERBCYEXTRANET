using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.Process;
using Eurobank.Models.Application.Applicant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
    public class ApplicantLegalGridValidationProcess
    {
        public static ValidationResultModel ValidateTaxDetails(int applicantId, ApplicantModel applicantModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.TAX_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            List<ValidationInfo> lstvalidationInfo = new List<ValidationInfo>();
            
            if (applicantId > 0)
            {
                var taxDetails = TaxDetailsProcess.GetTaxDetailsLegalByApplicantId(applicantId);
                if (taxDetails == null || taxDetails.Count == 0)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax;
                    lstvalidationError.Add(validationError);
                }
                if (taxDetails != null && taxDetails.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
                if (string.Equals(applicantModel.CompanyDetails.CountryofIncorporation, "Cyprus", StringComparison.OrdinalIgnoreCase))
                {
                    if (taxDetails != null && !taxDetails.Any(y => string.Equals(y.TaxDetails_CountryOfTaxResidencyName, "Cyprus", StringComparison.OrdinalIgnoreCase)))
                    {
                        //ValidationError validationError = new ValidationError();
                        ValidationInfo validationInfo = new ValidationInfo();
                        //retVal.IsValid = true;
                        validationInfo.InfoMessage = ValidationConstant.Tax_Details_Country_Info;
                        //validationError.PropertyName = "Title";
                        lstvalidationInfo.Add(validationInfo);
                    }
                }
                if(string.IsNullOrEmpty(applicantModel.CompanyDetails.IsLiableToPayDefenseTaxInCyprusName))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Details_isApplicantLiableToPayDefenceTaxInCyprus;
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            retVal.Infos = lstvalidationInfo;
            return retVal;
        }

        public static ValidationResultModel ValidateAddressDetails(int applicantId, ApplicantModel applicantModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ADDRESS_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
           

            if (applicantId > 0)
            {
                var addressDetails = AddressDetailsProcess.GetApplicantAddressDetailsLegal(applicantId);
                if (addressDetails == null || addressDetails.Count == 0)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Address;
                    lstvalidationError.Add(validationError);
                }
                if (addressDetails != null && addressDetails.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_OriginOfTotalAssets_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
                if (string.Equals(applicantModel.CompanyDetails.IsOfficeinCyprusName, "YES", StringComparison.OrdinalIgnoreCase))
                {
                    if (addressDetails != null && addressDetails.Count > 0)
                    {
                        if (!addressDetails.Any(y => string.Equals(y.AddressTypeName, "Office in Cyprus", StringComparison.OrdinalIgnoreCase) && string.Equals(y.CountryName, "Cyprus", StringComparison.OrdinalIgnoreCase)))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.Address_Details;
                            lstvalidationError.Add(validationError);
                        }
                        else if (addressDetails.Any(y => string.Equals(y.AddressTypeName, "Office in Cyprus", StringComparison.OrdinalIgnoreCase) && !string.Equals(y.CountryCode_PhoneNo, "357", StringComparison.OrdinalIgnoreCase)))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.Address_Details_Phone_No;
                            lstvalidationError.Add(validationError);
                        }
                    }
					else
					{
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.Address_Details;
                        lstvalidationError.Add(validationError);
                    }
                }
                string companyEntityName = ValidationHelper.GetString(ServiceHelper.GetName(applicantModel.CompanyDetails.EntityType, Constants.COMPANY_ENTITY_TYPE), "");
                if (string.Equals(companyEntityName.TrimEnd(), "TRUST", StringComparison.OrdinalIgnoreCase))
                {
                    if (addressDetails != null && !(addressDetails.Any(x => string.Equals(x.AddressTypeName.Trim(), "MAILING ADDRESS", StringComparison.OrdinalIgnoreCase))))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.Address_Details_MailAddress;
                        lstvalidationError.Add(validationError);
                    }
                }
                //else if (addressDetails != null && !(addressDetails.Any(x => string.Equals(x.AddressTypeName.Trim(), "REGISTERED OFFICE", StringComparison.OrdinalIgnoreCase)) && addressDetails.Any(x => string.Equals(x.AddressTypeName.Trim(), "MAILING ADDRESS", StringComparison.OrdinalIgnoreCase))))
                //{
                //    ValidationError validationError = new ValidationError();
                //    retVal.IsValid = false;
                //    validationError.ErrorMessage = ValidationConstant.Address_Details_RegMail;
                //    lstvalidationError.Add(validationError);
                //}
                else if (addressDetails != null && !(addressDetails.Any(x => string.Equals(x.AddressTypeName.Trim(), "REGISTERED OFFICE", StringComparison.OrdinalIgnoreCase))))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Address_Details_RegisteredAddress;
                    lstvalidationError.Add(validationError);
                }
                else if (addressDetails != null && !(addressDetails.Any(x => string.Equals(x.AddressTypeName.Trim(), "MAILING ADDRESS", StringComparison.OrdinalIgnoreCase))))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Address_Details_MailAddress;
                    lstvalidationError.Add(validationError);
                }
                if (addressDetails != null && (addressDetails.Count(x => string.Equals(x.AddressTypeName.Trim(), "MAILING ADDRESS", StringComparison.OrdinalIgnoreCase))) > 1)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_SingleMailAddress;
                    lstvalidationError.Add(validationError);
                }

            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateOriginOfTotalAssetsDetails(int applicantId)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ORIGIN_OF_TOTAL_ASSETS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (applicantId > 0)
            {
                var originOfTotalAssets = OriginOfTotalAssetsProcess.GetOriginOfTotalAssetsLegal(applicantId);
                if (originOfTotalAssets == null || originOfTotalAssets.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_OriginOfAssets;
                    lstvalidationError.Add(validationError);
                }
                if (originOfTotalAssets != null && originOfTotalAssets.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_OriginOfTotalAssets_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
    }
}
