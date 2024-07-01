using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.Process;
using Eurobank.Models.Application.RelatedParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
	public class RelatedPartyIndividualGridValidationProcess
	{
        public static ValidationResultModel ValidatePepDetailsApplicant(int relatedPartyId)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.RELATED_PARTY_PEP_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if(relatedPartyId > 0)
            {
                var pepDetailsDetails = PEPDetailsProcess.GetPepApplicantsByApplicantId(relatedPartyId);
                if (pepDetailsDetails == null || pepDetailsDetails.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_PepDetailsApplicant;
                    lstvalidationError.Add(validationError);
                }
                if(pepDetailsDetails != null && pepDetailsDetails.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_PepDetailsApplicant_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidatePepDetailsFmaily(int relatedPartyId)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.RELATED_PARTY_PEP_DETAILS_FAMILY_MEMBER_ASSOCIATES
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if(relatedPartyId > 0)
            {
                var pepDetailsDetails = PEPDetailsProcess.GetPepAssociatesByApplicantId(relatedPartyId);
                if (pepDetailsDetails == null || pepDetailsDetails.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_PepDetailsFamily;
                    lstvalidationError.Add(validationError);
                }
                if(pepDetailsDetails != null && pepDetailsDetails.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_PepDetailsFamily_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateIdentificationDetails(int relatedPartyId)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.IDENTIFICATION
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            

            if(relatedPartyId > 0)
            {
                var identificationDetails = IdentificationDetailsProcess.GetIdentificationDetails(relatedPartyId);
                if (identificationDetails == null || identificationDetails.Count == 0)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Identification;
                    lstvalidationError.Add(validationError);
                }
                if(identificationDetails != null && identificationDetails.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Identification_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
                if (identificationDetails != null)
                {
                    var cypruscitizenshiplist = identificationDetails?.Where(y => string.Equals(y.IdentificationDetails_CitizenshipName, "CYPRUS", StringComparison.OrdinalIgnoreCase));
                    if (cypruscitizenshiplist.Any() && !(cypruscitizenshiplist.Any(y => string.Equals(y.IdentificationDetails_TypeOfIdentificationName, "ID", StringComparison.OrdinalIgnoreCase))))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.Applicant_IDmandatoryforCY;
                        lstvalidationError.Add(validationError);
                    }
                    var Othercitizenshiplist = identificationDetails?.Where(y => !string.Equals(y.IdentificationDetails_CitizenshipName, "CYPRUS", StringComparison.OrdinalIgnoreCase) && string.Equals(y.IdentificationDetails_TypeOfIdentificationName, "ID", StringComparison.OrdinalIgnoreCase));
                    if (Othercitizenshiplist.Any())
                    {
                        foreach (var nonCYCountryWithID in Othercitizenshiplist)
                        {
                            if (!ServiceHelper.IsCountryEU(nonCYCountryWithID.IdentificationDetails_Citizenship))
                            {
                                var IsPassportAvailableFornonCYCountryWithID = identificationDetails.Where(y => string.Equals(y.IdentificationDetails_CitizenshipName, nonCYCountryWithID.IdentificationDetails_CitizenshipName, StringComparison.OrdinalIgnoreCase) && string.Equals(y.IdentificationDetails_TypeOfIdentificationName, "PASSPORT", StringComparison.OrdinalIgnoreCase));
                                if (!IsPassportAvailableFornonCYCountryWithID.Any())
                                {
                                    ValidationError validationError = new ValidationError();
                                    retVal.IsValid = false;
                                    validationError.ErrorMessage = ValidationConstant.Applicant_PassportMandetoryforNONCY + " (" + nonCYCountryWithID.IdentificationDetails_CitizenshipName + ")";
                                    lstvalidationError.Add(validationError);
                                } 
                            }
                        }
                    }
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateSourceOfIncome(int relatedPartyId, EmploymentDetailsRelatedPartyModel employmentDetails)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ORGIN_OF_ANNUAL_INCOME
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if(relatedPartyId > 0)
            {
                var sourceOfIncomeViewModels = SourceOfIncomeProcess.GetSourceOfIncomeRelatedParty(relatedPartyId);
                if (sourceOfIncomeViewModels == null || sourceOfIncomeViewModels.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_SourceOfIncome;
                    lstvalidationError.Add(validationError);
                }
                if(sourceOfIncomeViewModels != null && sourceOfIncomeViewModels.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_SourceOfIncome_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
                if (employmentDetails != null && sourceOfIncomeViewModels != null && sourceOfIncomeViewModels.Count > 0)
                {
                    var lstEmploymentStatus = ServiceHelper.GetEmploymentStatus();
                    string EmploymentStatusName = (lstEmploymentStatus != null && lstEmploymentStatus.Any(l => l.Value == employmentDetails.EmploymentStatus.ToString())) ? lstEmploymentStatus.FirstOrDefault(l => l.Value == employmentDetails.EmploymentStatus.ToString()).Text : string.Empty;
                    if (!string.IsNullOrEmpty(EmploymentStatusName))
                    {
                        if (string.Equals("PUBLIC SECTOR EMPLOYEE", EmploymentStatusName, StringComparison.OrdinalIgnoreCase) || string.Equals("SEMI-GOVERNMENT SECTOR EMPLOYEE", EmploymentStatusName, StringComparison.OrdinalIgnoreCase) || string.Equals("PRIVATE SECTOR EMPLOYEE", EmploymentStatusName, StringComparison.OrdinalIgnoreCase) || string.Equals("SELF-EMPLOYED", EmploymentStatusName, StringComparison.OrdinalIgnoreCase))
                        {
                            if (!sourceOfIncomeViewModels.Any(x => x.SourceOfAnnualIncomeName == "GROSS SALARY"))
                            {
                                retVal.IsValid = false;
                                validationError.ErrorMessage = "The employment status of " + EmploymentStatusName + " selected, requires an entry for GROSS SALARY in Origin of Annual Income.";
                                lstvalidationError.Add(validationError);
                            }
                        }
                        if (string.Equals("RETIRED", EmploymentStatusName, StringComparison.OrdinalIgnoreCase) || string.Equals("HOMEMAKER", EmploymentStatusName, StringComparison.OrdinalIgnoreCase) || string.Equals("STUDENT/MILITARY SERVICES", EmploymentStatusName, StringComparison.OrdinalIgnoreCase) || string.Equals("UNEMPLOYED", EmploymentStatusName, StringComparison.OrdinalIgnoreCase))
                        {
                            if (sourceOfIncomeViewModels.Any(x => x.SourceOfAnnualIncomeName == "GROSS SALARY"))
                            {
                                retVal.IsValid = false;
                                //validationError.ErrorMessage = "Gross Salary is not applicable for the selected " + EmploymentStatusName;
                                validationError.ErrorMessage = ValidationConstant.Applicant_AnnualIncomeGrossSalaryInvalid;
                                lstvalidationError.Add(validationError);
                            }
                        }

                    }
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateOriginOfTotalAssets(int relatedPartyId)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ORIGIN_OF_TOTAL_ASSETS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if(relatedPartyId > 0)
            {
                var originOfTotalAssets = OriginOfTotalAssetsProcess.GetOriginOfTotalAssets(relatedPartyId);
                if (originOfTotalAssets == null || originOfTotalAssets.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_OriginOfAssets;
                    lstvalidationError.Add(validationError);
                }
                if(originOfTotalAssets != null && originOfTotalAssets.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
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
                var addressDetails = AddressDetailsProcess.GetRelatedPartyAddressDetails(relatedPartyId);
                if (addressDetails == null || addressDetails.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Address;
                    lstvalidationError.Add(validationError);
                }
                if(addressDetails != null && addressDetails.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
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
        public static ValidationResultModel ValidateAddressDetailsUBO(int relatedPartyId, string EmploymentStatusName)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ADDRESS_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (relatedPartyId > 0)
            {
                var addressDetails = AddressDetailsProcess.GetRelatedPartyAddressDetails(relatedPartyId);
                if (addressDetails == null || addressDetails.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Address;
                    lstvalidationError.Add(validationError);
                }
                if (addressDetails != null && addressDetails.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_OriginOfTotalAssets_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
                if (EmploymentStatusName == "RETIRED" || EmploymentStatusName == "UNEMPLOYED" || EmploymentStatusName == "HOMEMAKER" || EmploymentStatusName == "STUDENT/MILITARY SERVICES")
                {
                }
                else
                {
                    if (addressDetails != null && !addressDetails.Any(y => string.Equals(y.AddressTypeName.TrimEnd(), "WORK ADDRESS", StringComparison.OrdinalIgnoreCase)))
                    {
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.Work_Address;
                        //validationError.PropertyName = "Title";
                        lstvalidationError.Add(validationError);
                    }
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
    }
}
