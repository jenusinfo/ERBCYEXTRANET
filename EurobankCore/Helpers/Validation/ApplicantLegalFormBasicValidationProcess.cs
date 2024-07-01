using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.Process;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Applicant.LegalEntity;
using Eurobank.Models.Application.Applicant.LegalEntity.ContactDetails;
using Eurobank.Models.Application.Applicant.LegalEntity.CRS;
using Eurobank.Models.Application.Applicant.LegalEntity.FATCACRS;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Applications.DebitCard;
using Eurobank.Models.Applications.TaxDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
    public class ApplicantLegalFormBasicValidationProcess
    {
        #region Company Details
        public static ValidationResultModel ValidateCompanyDetails(CompanyDetailsModel companyDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.LEGAL_ENTITY_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(companyDetailsModel.EntityType))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.CompanyDetails_EntityType;
                validationError.PropertyName = "EntityType";
                lstvalidationError.Add(validationError);
            }
            if (!string.IsNullOrEmpty(companyDetailsModel.EntityType))
            {
                var entityTypeName = ServiceHelper.GetName(companyDetailsModel.EntityType, Constants.COMPANY_ENTITY_TYPE);
                List<string> entityListText = new List<string>(new string[] { "Provident Fund", "Pension Fund", "Trust" });
                if (!entityListText.Contains(entityTypeName))
                {
                    if (string.IsNullOrEmpty(companyDetailsModel.ListingStatus))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.CompanyDetails_ListingStatus;
                        validationError.PropertyName = "ListingStatus";
                        lstvalidationError.Add(validationError);
                    }

                }
                if (string.Equals(entityTypeName, "Public Limited Company", StringComparison.OrdinalIgnoreCase) || string.Equals(entityTypeName, "Private Limited Company", StringComparison.OrdinalIgnoreCase))
                {
                    string countryName = ServiceHelper.GetCountryNameById(Convert.ToInt32( companyDetailsModel.CountryofIncorporation));
                    if (!string.IsNullOrEmpty(countryName) && (!string.Equals(countryName, "Greece", StringComparison.OrdinalIgnoreCase) && !string.Equals(countryName, "Cyprus", StringComparison.OrdinalIgnoreCase)) && string.IsNullOrEmpty(companyDetailsModel.SharesIssuedToTheBearerName))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.CompanyDetails_SharesIssuedBearer;
                        validationError.PropertyName = "SharesIssuedToTheBearerName";
                        lstvalidationError.Add(validationError);
                    }
                }
            }
            if (string.IsNullOrEmpty(companyDetailsModel.RegisteredName))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.CompanyDetails_RegisteredName;
                validationError.PropertyName = "RegisteredName";
                lstvalidationError.Add(validationError);
            }
            //else if (!CommonValidation.IsOnlyAlphabetsWithSpacesValid(companyDetailsModel.RegisteredName))
            //{
            //    ValidationError validationError = new ValidationError();
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.CompanyDetails_RegisteredName_Special;
            //    validationError.PropertyName = "RegisteredName";
            //    lstvalidationError.Add(validationError);
            //}
            if (companyDetailsModel.DateofIncorporation != null)
            {
                if (companyDetailsModel.DateofIncorporation > DateTime.Today)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.CompanyDetails_RegistrationDateFuture;
                    validationError.PropertyName = "DateofIncorporation";
                    lstvalidationError.Add(validationError);
                }
            }
            else
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.CompanyDetails_DateofIncorporation;
                validationError.PropertyName = "DateofIncorporation";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(companyDetailsModel.CountryofIncorporation))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.CompanyDetails_CountryOfIncorporation;
                validationError.PropertyName = "CountryofIncorporation";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(companyDetailsModel.RegistrationNumber))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.CompanyDetails_RegitrationNumber;
                validationError.PropertyName = "RegistrationNumber";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(companyDetailsModel.IsOfficeinCyprusName))
            {
                var entityTypeName = ServiceHelper.GetName(companyDetailsModel.EntityType, Constants.COMPANY_ENTITY_TYPE);
                if (entityTypeName == "Foundation" || entityTypeName == "Trade Union" || entityTypeName == "Club / Association" || entityTypeName == "City Council / Local Authority" || entityTypeName == "Government Organization"
                    || entityTypeName == "Semi - Government Organization" || entityTypeName == "Trust" || entityTypeName == "Provident Fund" || entityTypeName == "Pension Fund")
                {
                    //ValidationError validationError = new ValidationError();
                    //retVal.IsValid = false;
                    //validationError.ErrorMessage = ValidationConstant.CompanyDetails_OfficeInCyprus;
                    //validationError.PropertyName = "IsOfficeinCyprusName";
                    //lstvalidationError.Add(validationError);
                }
                else
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.CompanyDetails_OfficeInCyprus;
                    validationError.PropertyName = "IsOfficeinCyprusName";
                    lstvalidationError.Add(validationError);
                }

            }

            retVal.Errors = lstvalidationError;
            return retVal;
        }

        #endregion

        #region Business And Financial Profile

        public static ValidationResultModel ValidateBusinessProfile(CompanyDetailsModel companyDetails, CompanyBusinessProfileModel companyBusinessProfileModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.BUSINESS_PROFILE
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();

            string selectedEntityType = string.Empty;
            var companyEntities = ServiceHelper.GetCompanyEntityTypes();
            if (companyEntities != null && companyDetails != null && !string.IsNullOrEmpty(companyDetails.EntityType) && companyEntities.Any(r => string.Equals(r.Value, companyDetails.EntityType, StringComparison.OrdinalIgnoreCase)))
            {
                selectedEntityType = companyEntities.FirstOrDefault(r => string.Equals(r.Value, companyDetails.EntityType, StringComparison.OrdinalIgnoreCase)).Text;
            }

            if (string.IsNullOrEmpty(companyBusinessProfileModel.MainBusinessActivities))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.BusinessProfile_MainBusinessActivities;
                validationError.PropertyName = "MainBusinessActivities";
                lstvalidationError.Add(validationError);
            }
            if (!string.IsNullOrEmpty(selectedEntityType) && !string.Equals(selectedEntityType, "Provident Fund", StringComparison.OrdinalIgnoreCase) && !string.Equals(selectedEntityType, "Pension Fund", StringComparison.OrdinalIgnoreCase) && !string.Equals(selectedEntityType, "Trust", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(companyBusinessProfileModel.NumberofYearsinOperation))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.BusinessProfile_NumberofYearsinOperation;
                    validationError.PropertyName = "NumberofYearsinOperation";
                    lstvalidationError.Add(validationError);
                }
                else if (companyBusinessProfileModel.NumberofYearsinOperation.Length > 4)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.NoOfYesrsOpeMaxLength_4_Exceeded;
                    validationError.PropertyName = "NumberofYearsinOperation";
                    lstvalidationError.Add(validationError);
                }
                else if (Convert.ToInt32(companyBusinessProfileModel.NumberofYearsinOperation) < 0)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.NoOfYearsOpe_Negetive;
                    validationError.PropertyName = "NumberofYearsinOperation";
                    lstvalidationError.Add(validationError);
                }
            }

            if (!string.IsNullOrEmpty(selectedEntityType) && !string.Equals(selectedEntityType, "Trust", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(companyBusinessProfileModel.NumberofEmployes))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    if (string.Equals(selectedEntityType, "Provident Fund", StringComparison.OrdinalIgnoreCase) || string.Equals(selectedEntityType, "Pension Fund", StringComparison.OrdinalIgnoreCase))
                    {
                        validationError.ErrorMessage = ValidationConstant.BusinessProfile_NumberofMembers;
                    }
                    else
                    {
                        validationError.ErrorMessage = ValidationConstant.BusinessProfile_NumberofEmployes;
                    }


                    validationError.PropertyName = "NumberofEmployes";
                    lstvalidationError.Add(validationError);
                }
                else if (companyBusinessProfileModel.NumberofEmployes.Length > 4)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.NoOfEmployesMaxLength_4_Exceeded;
                    validationError.PropertyName = "NumberofEmployes";
                    lstvalidationError.Add(validationError);
                }
                else if (Convert.ToInt32(companyBusinessProfileModel.NumberofEmployes) < 0)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.NumberofEmployes_Negetive;
                    validationError.PropertyName = "NumberofEmployes";
                    lstvalidationError.Add(validationError);
                }
            }
            //if(companyBusinessProfileModel.CorporationIsengagedInTheProvision == null)
            //{
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.CompanyDetails_RegistrationDateFuture;
            //    validationError.PropertyName = "CorporationIsengagedInTheProvision";
            //    lstvalidationError.Add(validationError);
            //}
            if (!string.IsNullOrEmpty(selectedEntityType) && !string.Equals(selectedEntityType, "Trust", StringComparison.OrdinalIgnoreCase) && !string.Equals(selectedEntityType, "Provident Fund", StringComparison.OrdinalIgnoreCase) && !string.Equals(selectedEntityType, "Pension Fund", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(companyBusinessProfileModel.EconomicSectorIndustry))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.BusinessProfile_EconomicSectorIndustry;
                validationError.PropertyName = "EconomicSectorIndustry";
                lstvalidationError.Add(validationError);
            }
            if (companyBusinessProfileModel.CountryofOriginofWealthActivitiesValues == null || companyBusinessProfileModel.CountryofOriginofWealthActivitiesValues.Length == 0)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.BusinessProfile_CountryofOriginofWealthActivities;
                validationError.PropertyName = "CountryofOriginofWealthActivitiesValues";
                lstvalidationError.Add(validationError);
            }
            if (!string.IsNullOrEmpty(companyBusinessProfileModel.CorporationIsengagedInTheProvisionName) && string.Equals(companyBusinessProfileModel.CorporationIsengagedInTheProvisionName, "true", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(companyBusinessProfileModel.IssuingAuthority))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.BusinessProfile_IssuingAuthority;
                validationError.PropertyName = "IssuingAuthority";
                lstvalidationError.Add(validationError);
            }
            if (string.Equals(selectedEntityType, "Provident Fund", StringComparison.OrdinalIgnoreCase) || string.Equals(selectedEntityType, "Pension Fund", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(companyBusinessProfileModel.SponsoringEntityName))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.BusinessProfile_SponsoringEntityName;
                    validationError.PropertyName = "SponsoringEntityName";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(companyBusinessProfileModel.LineOfBusinessOfTheSponsoringEntity))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.BusinessProfile_LineOfBusinessOfTheSponsoringEntity;
                    validationError.PropertyName = "LineOfBusinessOfTheSponsoringEntity";
                    lstvalidationError.Add(validationError);
                }
            }
            if(string.IsNullOrEmpty(companyBusinessProfileModel.CorporationIsengagedInTheProvisionName))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.BusinessProfile_ProvisionOfFinancialandInvestmentServices;
                validationError.PropertyName = "CorporationIsengagedInTheProvisionName";
                lstvalidationError.Add(validationError);
            }
                retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateFinancialProfile(CompanyDetailsModel companyDetails, CompanyFinancialInformationModel companyFinancialInformationModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.FINANCIAL_INFORMATION
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            string selectedEntityType = string.Empty;
            var companyEntities = ServiceHelper.GetCompanyEntityTypes();
            if (companyEntities != null && companyDetails != null && !string.IsNullOrEmpty(companyDetails.EntityType) && companyEntities.Any(r => string.Equals(r.Value, companyDetails.EntityType, StringComparison.OrdinalIgnoreCase)))
            {
                selectedEntityType = companyEntities.FirstOrDefault(r => string.Equals(r.Value, companyDetails.EntityType, StringComparison.OrdinalIgnoreCase)).Text;
            }
            if (!string.IsNullOrEmpty(selectedEntityType) && !string.Equals(selectedEntityType, "Provident Fund", StringComparison.OrdinalIgnoreCase) && !string.Equals(selectedEntityType, "Pension Fund", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(companyFinancialInformationModel.Turnover))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.FinancialProfile_Turnover;
                    validationError.PropertyName = "Turnover";
                    lstvalidationError.Add(validationError);
                }
                //else if (companyFinancialInformationModel.Turnover.Length > 20) //16
                //{
                //    ValidationError validationError = new ValidationError();
                //    retVal.IsValid = false;
                //    validationError.ErrorMessage = ValidationConstant.Amount_Max;
                //    validationError.PropertyName = "Turnover";
                //    lstvalidationError.Add(validationError);
                //}
                else if (Convert.ToDecimal(companyFinancialInformationModel.Turnover) < 0)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Turnover_Negetive;
                    validationError.PropertyName = "Turnover";
                    lstvalidationError.Add(validationError);
                }
                else if (!Regex.IsMatch(companyFinancialInformationModel.Turnover, ValidationConstant.RegexPositiveNumeric_19_2))
                {
                    if(companyFinancialInformationModel.Turnover.Length > 17)
                    {
                        companyFinancialInformationModel.Turnover = companyFinancialInformationModel.Turnover.Substring(0, 17);
                    }
                    //ValidationError validationError = new ValidationError();
                    //retVal.IsValid = false;
                    //validationError.ErrorMessage = ValidationConstant.FinancialProfilePositiveNumeric_19_2_RegexTurnover;
                    //validationError.PropertyName = "Turnover";
                    //lstvalidationError.Add(validationError);
                }
            }
            if (string.IsNullOrEmpty(companyFinancialInformationModel.TotalAssets))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.FinancialProfile_TotalAssets;
                validationError.PropertyName = "TotalAssets";
                lstvalidationError.Add(validationError);
            }
            else if (Convert.ToDecimal(companyFinancialInformationModel.TotalAssets) < 0)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.TotalAssets_Negetive;
                validationError.PropertyName = "TotalAssets";
                lstvalidationError.Add(validationError);
            }
            else if (!Regex.IsMatch(companyFinancialInformationModel.TotalAssets, ValidationConstant.RegexPositiveNumeric_19_2))
            {
                if(companyFinancialInformationModel.TotalAssets.Length > 17)
                {
                    companyFinancialInformationModel.TotalAssets = companyFinancialInformationModel.TotalAssets.Substring(0, 17);
                }
                //ValidationError validationError = new ValidationError();
                //retVal.IsValid = false;
                //validationError.ErrorMessage = ValidationConstant.FinancialProfilePositiveNumeric_19_2_RegexTotalAssets;
                //validationError.PropertyName = "TotalAssets";
                //lstvalidationError.Add(validationError);
            }

            if (string.IsNullOrEmpty(companyFinancialInformationModel.NetProfitLoss))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.FinancialProfile_NetProfitLoss;
                validationError.PropertyName = "NetProfitLoss";
                lstvalidationError.Add(validationError);
            }
            //else if (companyFinancialInformationModel.NetProfitLoss.Length > 20) //16
            //{
            //    ValidationError validationError = new ValidationError();
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.Amount_Max;
            //    validationError.PropertyName = "NetProfitAndLoss";
            //    lstvalidationError.Add(validationError);
            //}
            else if (!Regex.IsMatch(companyFinancialInformationModel.NetProfitLoss, ValidationConstant.RegexAllNumeric_19_2))
            {
                if(companyFinancialInformationModel.NetProfitLoss.Length > 17)
                {
                    companyFinancialInformationModel.NetProfitLoss = companyFinancialInformationModel.NetProfitLoss.Substring(0, 17);
                }
                //ValidationError validationError = new ValidationError();
                //retVal.IsValid = false;
                //validationError.ErrorMessage = ValidationConstant.FinancialProfileAllNumeric_19_2_RegexNetProfitLoss;
                //validationError.PropertyName = "NetProfitLoss";
                //lstvalidationError.Add(validationError);
            }
            

            retVal.Errors = lstvalidationError;
            return retVal;
        }

        #endregion

        #region------TAX Details------
        public ValidationResultModel ValidateTaxDetails(TaxDetailsViewModel taxDetailsViewModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.TAX_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_CountryOfTaxResidency))
            {
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.TaxDetails_CountryOfTaxResidency;
                validationError.PropertyName = "TaxDetails_CountryOfTaxResidency";
                lstvalidationError.Add(validationError);
            }
            else if (!string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_CountryOfTaxResidency))
            {
                var countryName = ServiceHelper.GetCountryNameById(Convert.ToInt32(taxDetailsViewModel.TaxDetails_CountryOfTaxResidency));
                if (!string.Equals("Cyprus", countryName,StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TaxIdentificationNumber))
                    {
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.TaxDetails_TaxIdentificationNumber;
                        validationError.PropertyName = "TaxDetails_TaxIdentificationNumber";
                        lstvalidationError.Add(validationError);

                        if (string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TinUnavailableReason))
                        {
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.TaxDetails_TinUnavailableReason;
                            validationError.PropertyName = "TaxDetails_TinUnavailableReason";
                            lstvalidationError.Add(validationError);
                        }
                    }
                    if (!string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TaxIdentificationNumber) && !string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TinUnavailableReason))
                    {
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.TaxDetails_TinAndIdentificationNumber;
                        validationError.PropertyName = "TaxDetails_TinUnavailableReason";
                        lstvalidationError.Add(validationError);
                    }

                }
            }
            if (!string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TinUnavailableReason))
            {
                var tinUnavailableReason = ServiceHelper.GetName(taxDetailsViewModel.TaxDetails_TinUnavailableReason, Constants.TIN_UNAVAILABLE_REASON);
                if (string.Equals(tinUnavailableReason, "Entity  is unable to obtain Tax Number", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_JustificationForTinUnavalability))
                    {
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.TaxDetails_JustificationForTinUnavalability;
                        validationError.PropertyName = "TaxDetails_JustificationForTinUnavalability";
                        lstvalidationError.Add(validationError);
                    }
                }
            }


            retVal.Errors = lstvalidationError;
            return retVal;
        }

        //public static ValidationResultModel ValidateTaxDetailsExtended(CompanyDetailsModel companyDetailsModel)
        //{
        //    ValidationResultModel retVal = new ValidationResultModel()
        //    {
        //        IsValid = true,
        //        ApplicationModuleName = ApplicationModule.APPLICANT_TAX_DETAILS
        //    };
        //    List<ValidationError> lstvalidationError = new List<ValidationError>();


        //    if(companyDetailsModel == null || string.IsNullOrEmpty(companyDetailsModel.IsLiableToPayDefenseTaxInCyprus))
        //    {
        //        ValidationError validationError = new ValidationError();
        //        retVal.IsValid = false;
        //        validationError.ErrorMessage = ValidationConstant.TaxDetails_LiableToPayCyprus;
        //        validationError.PropertyName = "IsLiableToPayDefenseTaxInCyprusName";
        //        lstvalidationError.Add(validationError);
        //    }

        //    retVal.Errors = lstvalidationError;
        //    return retVal;
        //}
        #endregion

        #region---Contact Details----
        public static ValidationResultModel ValidateContactDetails(ContactDetailsLegalModel contactDetailsLegalModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.COMMUNICATION_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();



            //if (string.IsNullOrEmpty(contactDetailsLegalModel.ContactDetailsLegal_PreferredMailingAddress))
            //{
            //    ValidationError validationError = new ValidationError();
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.ContactDetailsLegal_PreferredMailingAddress;
            //    validationError.PropertyName = "ContactDetailsLegal_PreferredMailingAddress";
            //    lstvalidationError.Add(validationError);
            //}
            if (string.IsNullOrEmpty(contactDetailsLegalModel.ContactDetailsLegal_EmailAddressForSendingAlerts))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ContactDetails_EmailAddressForSendingAlerts;
                validationError.PropertyName = "ContactDetailsLegal_EmailAddressForSendingAlerts";
                lstvalidationError.Add(validationError);
            }
            else if (!CommonValidation.IsEmailValid(contactDetailsLegalModel.ContactDetailsLegal_EmailAddressForSendingAlerts))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ContactDetails_EmailAddressForSendingAlerts;
                validationError.PropertyName = "ContactDetailsLegal_EmailAddressForSendingAlerts";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(contactDetailsLegalModel.ContactDetailsLegal_PreferredCommunicationLanguage))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ContactDetails_PreferredCommunicationLanguage;
                validationError.PropertyName = "ContactDetailsLegal_PreferredCommunicationLanguage";
                lstvalidationError.Add(validationError);
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region----FATCA/CRS Details----
        public static ValidationResultModel ValidateFATCADetails(FATCACRSDetailsModel fATCACRSDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.FATCA_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            if (string.IsNullOrEmpty(fATCACRSDetailsModel.FATCADetails_FATCAClassification))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.FATCADetails_FATCAClassification;
                validationError.PropertyName = "FATCADetails_FATCAClassification";
                lstvalidationError.Add(validationError);
            }
            if (!string.IsNullOrEmpty(fATCACRSDetailsModel.FATCADetails_FATCAClassification))
            {
                var classification = ServiceHelper.GetName(fATCACRSDetailsModel.FATCADetails_FATCAClassification, Constants.FATCA_CLASSIFICATION);
                if (string.Equals(classification, "US Entity", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(fATCACRSDetailsModel.FATCADetails_USEtityType))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.FATCADetails_USEtityType;
                        validationError.PropertyName = "FATCADetails_USEtityType";
                        lstvalidationError.Add(validationError);
                    }
                }
                else if (string.Equals(classification, "Foreign Financial Institution", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(fATCACRSDetailsModel.FATCADetails_TypeofForeignFinancialInstitution))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.FATCADetails_TypeofForeignFinancialInstitution;
                        validationError.PropertyName = "FATCADetails_TypeofForeignFinancialInstitution";
                        lstvalidationError.Add(validationError);
                    }
                }
                else if (string.Equals(classification, "Non-Financial Foreign Entity (NFFE)", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(fATCACRSDetailsModel.FATCADetails_TypeofNonFinancialForeignEntity))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.FATCADetails_TypeofNonFinancialForeignEntity;
                        validationError.PropertyName = "FATCADetails_TypeofNonFinancialForeignEntity";
                        lstvalidationError.Add(validationError);
                    }
                }

            }
            if (!string.IsNullOrEmpty(fATCACRSDetailsModel.FATCADetails_TypeofForeignFinancialInstitution))
            {
                var foreignFinancialInstitution = ServiceHelper.GetName(fATCACRSDetailsModel.FATCADetails_TypeofForeignFinancialInstitution, Constants.TYPE_OF_FOREIGN_FINANCIAL_INSTITUTION);
                if (string.Equals(foreignFinancialInstitution, "Participating Foreign Financial Institution (PFFI)", StringComparison.OrdinalIgnoreCase) || string.Equals(foreignFinancialInstitution, "Registered Deemed Compliant Financial Institution", StringComparison.OrdinalIgnoreCase) || string.Equals(foreignFinancialInstitution, "Sponsored Financial Institution", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(fATCACRSDetailsModel.FATCADetails_GlobalIntermediaryIdentificationNumber))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.FATCADetails_GlobalIntermediaryIdentificationNumber;
                        validationError.PropertyName = "FATCADetails_GlobalIntermediaryIdentificationNumber";
                        lstvalidationError.Add(validationError);
                    }
                }
                else if (string.Equals(foreignFinancialInstitution, "Exempt Beneficial Owner", StringComparison.OrdinalIgnoreCase) || string.Equals(foreignFinancialInstitution, "Entity wholly owned by Exempt Beneficial Owners", StringComparison.OrdinalIgnoreCase) || string.Equals(foreignFinancialInstitution, "Certified Deemed Compliant Financial Institution", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(fATCACRSDetailsModel.ExemptionReason))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.ExemptionReason;
                        validationError.PropertyName = "ExemptionReason";
                        lstvalidationError.Add(validationError);
                    }
                }
            }
            if (!string.IsNullOrEmpty(fATCACRSDetailsModel.FATCADetails_TypeofNonFinancialForeignEntity))
            {
                var nonFinancialForeignEntity = ServiceHelper.GetName(fATCACRSDetailsModel.FATCADetails_TypeofNonFinancialForeignEntity, Constants.TYPE_OF_NON_FINANCIAL_FOREIGN_ENTITY_NFFE);
                if (string.Equals(nonFinancialForeignEntity, "Direct reporting Passive NFFE", StringComparison.OrdinalIgnoreCase) || string.Equals(nonFinancialForeignEntity, "Sponsored direct reporting Passive NFFE", StringComparison.OrdinalIgnoreCase) || string.Equals(nonFinancialForeignEntity, "Sponsored direct reporting NFFE", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(fATCACRSDetailsModel.FATCADetails_GlobalIntermediaryIdentificationNumber))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.FATCADetails_GlobalIntermediaryIdentificationNumber;
                        validationError.PropertyName = "FATCADetails_GlobalIntermediaryIdentificationNumber";
                        lstvalidationError.Add(validationError);
                    }
                }
            }


            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region---CRS Details----
        public static ValidationResultModel ValidateCRSDetails(CRSDetailsModel cRSDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.CRS_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();



            if (string.IsNullOrEmpty(cRSDetailsModel.CompanyCRSDetails_CRSClassification))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.CompanyCRSDetails_CRSClassification;
                validationError.PropertyName = "CompanyCRSDetails_CRSClassification";
                lstvalidationError.Add(validationError);
            }
            if (!string.IsNullOrEmpty(cRSDetailsModel.CompanyCRSDetails_CRSClassification))
            {
                var classification = ServiceHelper.GetName(cRSDetailsModel.CompanyCRSDetails_CRSClassification, Constants.CRS_CLASSIFICATION);
                if (string.Equals(classification, "Active Non-Financial Entity (NFE)", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(cRSDetailsModel.CompanyCRSDetails_TypeofActiveNonFinancialEntity))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.CompanyCRSDetails_TypeofActiveNonFinancialEntity;
                        validationError.PropertyName = "CompanyCRSDetails_TypeofActiveNonFinancialEntity";
                        lstvalidationError.Add(validationError);
                    }
                }
                else if (string.Equals(classification, "Financial Institution", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(cRSDetailsModel.CompanyCRSDetails_TypeofFinancialInstitution))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.CompanyCRSDetails_TypeofFinancialInstitution;
                        validationError.PropertyName = "CompanyCRSDetails_TypeofFinancialInstitution";
                        lstvalidationError.Add(validationError);
                    }
                }
            }
            if (!string.IsNullOrEmpty(cRSDetailsModel.CompanyCRSDetails_TypeofActiveNonFinancialEntity))
            {
                //var activeNonFinancialEntity = ServiceHelper.GetName(cRSDetailsModel.CompanyCRSDetails_TypeofActiveNonFinancialEntity, Constants.TYPE_OF_ACTIVE_NON_FINANCIAL_ENTITY_NFE);
                var activeNonFinancialEntity = ServiceHelper.GetNFEtype().Where(x => x.Value == cRSDetailsModel.CompanyCRSDetails_TypeofActiveNonFinancialEntity).Select(x => x.Text).FirstOrDefault();
                if (string.Equals(activeNonFinancialEntity.TrimEnd(), "A corporation the stock of which is regularly traded on an established securities market or a corporation which is a Related Entity of such a corporation", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(cRSDetailsModel.CompanyCRSDetails_NameofEstablishedSecuritiesMarket))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.CompanyCRSDetails_NameofEstablishedSecuritiesMarket;
                        validationError.PropertyName = "CompanyCRSDetails_NameofEstablishedSecuritiesMarket";
                        lstvalidationError.Add(validationError);
                    }
                }
            }
            if (!string.IsNullOrEmpty(cRSDetailsModel.CompanyCRSDetails_TypeofActiveNonFinancialEntity) && !string.IsNullOrEmpty(cRSDetailsModel.CompanyCRSDetails_TypeofFinancialInstitution))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.CompanyCRSDetails_TypeofFinancialInstitutionValid;
                validationError.PropertyName = "CompanyCRSDetails_TypeofFinancialInstitution";
                lstvalidationError.Add(validationError);
            }


            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region Banking Relationship

        public static ValidationResultModel ValidateBankingRelationshipLegal(CompanyDetailsModel companyDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.EXISTING_BANK_RELATIONSHIP
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(companyDetailsModel.HasAccountInOtherBankName))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.BankRelationship_HasAccountInOtherBank;
                validationError.PropertyName = "HasAccountInOtherBankName";
                lstvalidationError.Add(validationError);
            }
            else if (string.Equals(companyDetailsModel.HasAccountInOtherBankName, "true", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(companyDetailsModel.NameOfBankingInstitution))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.BankRelationship_NameOfBankingInstitution;
                    validationError.PropertyName = "NameOfBankingInstitution";
                    lstvalidationError.Add(validationError);
                }
                else if (companyDetailsModel.NameOfBankingInstitution.Length > 50)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.MaxLength_NameBankingInstitute;
                    validationError.PropertyName = "NameOfBankingInstitution";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(companyDetailsModel.CountryOfBankingInstitution))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.BankRelationship_CountryOfBankingInstitution;
                    validationError.PropertyName = "CountryOfBankingInstitution";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        #endregion

        public static ValidationResultModel ValidateLegalApplicantDetails(ApplicantModel applicantModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ADDRESS_DETAILS
            };
            int applicantId = applicantModel.CompanyDetails.Id;
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            if (applicantId > 0)
            {
                if (string.Equals(applicantModel.CompanyDetails.IsOfficeinCyprusName, "true", StringComparison.OrdinalIgnoreCase))
                {
                    var addressDetails = AddressDetailsProcess.GetApplicantAddressDetailsLegal(applicantId);
                    if (addressDetails != null || addressDetails.Count > 0)
                    {
                        if (!addressDetails.Any(y => string.Equals(y.AddressTypeName, "Office in Cyprus", StringComparison.OrdinalIgnoreCase) && string.Equals(y.CountryName, "Cyprus", StringComparison.OrdinalIgnoreCase)))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.Address_Details;
                            lstvalidationError.Add(validationError);
                        }
                    }
                }

            }



            retVal.Errors = lstvalidationError;
            return retVal;
        }
    }
}
