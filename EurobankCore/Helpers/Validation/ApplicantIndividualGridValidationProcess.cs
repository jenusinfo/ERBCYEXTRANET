using CodeBeautify;
using DocumentFormat.OpenXml.EMMA;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.Process;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Common;
using Eurobank.Models.XMLServiceModel.Individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
    public class ApplicantIndividualGridValidationProcess
    {
        public static ValidationResultModel ValidateTaxDetails(int applicantId, PersonalDetailsModel personalDetails)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.TAX_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (applicantId > 0)
            {
                var taxDetails = TaxDetailsProcess.GetTaxDetailsByApplicantId(applicantId);
                if (taxDetails == null || taxDetails.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax;
                    lstvalidationError.Add(validationError);
                }
                if (taxDetails != null && taxDetails.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Tax_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
                ValidationResultModel taxDetailsValidationExtended = new ValidationResultModel()
                {
                    IsValid = true,
                    ApplicationModuleName = ApplicationModule.TAX_DETAILS
                };
                taxDetailsValidationExtended = ApplicantIndividualFormBasicValidationProcess.ValidateTaxDetailsExtended(personalDetails);
                if (taxDetailsValidationExtended.Errors != null && taxDetailsValidationExtended.Errors.Count > 0)
                {
                    retVal.IsValid = false;
                    lstvalidationError.AddRange(taxDetailsValidationExtended.Errors);
                }

            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidatePepDetailsApplicant(int applicantId)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.PEP_DETAILS_APPLICANT
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (applicantId > 0)
            {
                var pepDetailsDetails = PEPDetailsProcess.GetPepApplicantsByApplicantId(applicantId);
                if (pepDetailsDetails == null || pepDetailsDetails.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_PepDetailsApplicant;
                    lstvalidationError.Add(validationError);
                }
                if (pepDetailsDetails != null && pepDetailsDetails.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
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

        public static ValidationResultModel ValidatePepDetailsFmaily(int applicantId)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.PEP_DETAILS_FAMILY_MEMBER_ASSOCIATES
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (applicantId > 0)
            {
                var pepDetailsDetails = PEPDetailsProcess.GetPepAssociatesByApplicantId(applicantId);
                if (pepDetailsDetails == null || pepDetailsDetails.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_PepDetailsFamily;
                    lstvalidationError.Add(validationError);
                }
                if (pepDetailsDetails != null && pepDetailsDetails.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
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

        public static ValidationResultModel ValidateIdentificationDetails(int applicantId)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.IDENTIFICATION
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (applicantId > 0)
            {
                var identificationDetails = IdentificationDetailsProcess.GetIdentificationDetails(applicantId);
                if (identificationDetails == null || identificationDetails.Count == 0)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Identification;
                    lstvalidationError.Add(validationError);
                }
                if (identificationDetails != null && identificationDetails.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_Identification_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
                if (identificationDetails != null)
                {
                    var isDuplicateValue = identificationDetails
                        .GroupBy(x => new { x.IdentificationDetails_CitizenshipName, x.IdentificationDetails_TypeOfIdentificationName })
                        .Any(g => g.Count() > 1);

                    if (!isDuplicateValue)
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
                    else
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.Applicant_IdentificationDuplicate;
                        lstvalidationError.Add(validationError);
                    }
                    //var Othercitizenshiplist = identificationDetails?.Where(y => !string.Equals(y.IdentificationDetails_CitizenshipName, "CYPRUS", StringComparison.OrdinalIgnoreCase));
                    //if (Othercitizenshiplist.Any() && !(Othercitizenshiplist.Any(y => string.Equals(y.IdentificationDetails_TypeOfIdentificationName, "PASSPORT", StringComparison.OrdinalIgnoreCase))))
                    //{
                    //    retVal.IsValid = false;
                    //    validationError.ErrorMessage = ValidationConstant.Applicant_PassportMandetoryforNONCY;
                    //    lstvalidationError.Add(validationError);
                    //}

                }

            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateSourceOfIncome(int applicantId, EmploymentDetailsModel employmentDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ORGIN_OF_ANNUAL_INCOME
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (applicantId > 0)
            {
                var sourceOfIncomeViewModels = SourceOfIncomeProcess.GetSourceOfIncome(applicantId);
                if (sourceOfIncomeViewModels == null || sourceOfIncomeViewModels.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_SourceOfIncome;
                    lstvalidationError.Add(validationError);
                }
                if (sourceOfIncomeViewModels != null && sourceOfIncomeViewModels.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Applicant_SourceOfIncome_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
                if (employmentDetailsModel != null && sourceOfIncomeViewModels != null && sourceOfIncomeViewModels.Count > 0)
                {
                    var lstEmploymentStatus = ServiceHelper.GetEmploymentStatus();
                    string EmploymentStatusName = (lstEmploymentStatus != null && lstEmploymentStatus.Any(l => l.Value == employmentDetailsModel.EmploymentStatus.ToString())) ? lstEmploymentStatus.FirstOrDefault(l => l.Value == employmentDetailsModel.EmploymentStatus.ToString()).Text : string.Empty;
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

        public static ValidationResultModel ValidateOriginOfTotalAssets(int applicantId)
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
                var originOfTotalAssets = OriginOfTotalAssetsProcess.GetOriginOfTotalAssets(applicantId);
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

        public static ValidationResultModel ValidateAddressDetails(ApplicantModel applicantModel)
        {
            int applicantId = applicantModel.PersonalDetails.Id;
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ADDRESS_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (applicantId > 0)
            {
                var addressDetails = AddressDetailsProcess.GetApplicantAddressDetails(applicantId);
                if (addressDetails == null || addressDetails.Count == 0)
                {
                    retVal.IsValid = false;
                    validationError = new ValidationError();
                    validationError.ErrorMessage = ValidationConstant.Applicant_Address;
                    lstvalidationError.Add(validationError);
                }
                if (addressDetails != null && addressDetails.Any(y => string.Equals(y.StatusName, "Pending", StringComparison.OrdinalIgnoreCase)))
                {
                    retVal.IsValid = false;
                    validationError = new ValidationError();
                    validationError.ErrorMessage = ValidationConstant.Applicant_OriginOfTotalAssets_Grid_Invalid;
                    //validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
                //if (addressDetails != null && !(addressDetails.Any(x => string.Equals(x.AddressTypeName.Trim(), "RESIDENTIAL ADDRESS", StringComparison.OrdinalIgnoreCase)) && addressDetails.Any(x => string.Equals(x.AddressTypeName.Trim(), "MAILING ADDRESS", StringComparison.OrdinalIgnoreCase))))
                //{
                //    retVal.IsValid = false;
                //    validationError = new ValidationError();
                //    validationError.ErrorMessage = ValidationConstant.Applicant_AddressResMail;
                //    lstvalidationError.Add(validationError);
                //}
                if (addressDetails != null && !(addressDetails.Any(x => string.Equals(x.AddressTypeName.Trim(), "RESIDENTIAL ADDRESS", StringComparison.OrdinalIgnoreCase))))
                {
                    retVal.IsValid = false;
                    validationError = new ValidationError();
                    validationError.ErrorMessage = ValidationConstant.Applicant_OneResidentialAddress;
                    lstvalidationError.Add(validationError);
                }
                if (addressDetails != null && !(addressDetails.Any(x => string.Equals(x.AddressTypeName.Trim(), "MAILING ADDRESS", StringComparison.OrdinalIgnoreCase))))
                {
                    retVal.IsValid = false;
                    validationError = new ValidationError();
                    validationError.ErrorMessage = ValidationConstant.Applicant_OneMailingAddress;
                    lstvalidationError.Add(validationError);
                }
                if (applicantModel.EmploymentDetails != null)
                {
                    var employmentStatus = ServiceHelper.GetEmploymentStatus();
                    var employmentStatusName = "";
                    if (employmentStatus != null && employmentStatus.Any(t => !string.IsNullOrEmpty(applicantModel.EmploymentDetails.EmploymentStatus) && t.Value == applicantModel.EmploymentDetails.EmploymentStatus))
                    {
                        employmentStatusName = employmentStatus.FirstOrDefault(t => !string.IsNullOrEmpty(applicantModel.EmploymentDetails.EmploymentStatus) && t.Value == applicantModel.EmploymentDetails.EmploymentStatus).Text;
                    }

                    if ((string.Equals(employmentStatusName, "PUBLIC SECTOR EMPLOYEE", StringComparison.OrdinalIgnoreCase) || string.Equals(employmentStatusName, "SEMI-GOVERNMENT SECTOR EMPLOYEE", StringComparison.OrdinalIgnoreCase) || string.Equals(employmentStatusName, "PRIVATE SECTOR EMPLOYEE", StringComparison.OrdinalIgnoreCase) || string.Equals(employmentStatusName, "SELF-EMPLOYED", StringComparison.OrdinalIgnoreCase)))
                    {
                        if (addressDetails != null && !(addressDetails.Any(x => string.Equals(x.AddressTypeName.Trim(), "WORK ADDRESS", StringComparison.OrdinalIgnoreCase))))
                        {
                            retVal.IsValid = false;
                            validationError = new ValidationError();
                            validationError.ErrorMessage = ValidationConstant.Applicant_WorkAdderssMandetory;
                            lstvalidationError.Add(validationError);
                        }
                    }
                }
                if (addressDetails != null && (addressDetails.Count(x => string.Equals(x.AddressTypeName.Trim(), "MAILING ADDRESS", StringComparison.OrdinalIgnoreCase))) > 1)
                {
                    retVal.IsValid = false;
                    validationError = new ValidationError();
                    validationError.ErrorMessage = ValidationConstant.Applicant_SingleMailAddress;
                    lstvalidationError.Add(validationError);
                }

            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
    }
}
