using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Application.RelatedParty;
using Eurobank.Models.Application.RelatedParty.PartyRolesLegal;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
    public class RelatedPartyLegalFormBasicValidationProcess
    {
        #region-----Party Roles-----
        public static ValidationResultModel ValidatePartyRoles(PartyRolesLegalViewModel partyRolesLegalViewModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.RELATED_PARTY_ROLES
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (partyRolesLegalViewModel.RelatedPartyRoles_IsDirector == false && partyRolesLegalViewModel.RelatedPartyRoles_IsAlternativeDirector == false && partyRolesLegalViewModel.RelatedPartyRoles_IsSecretary == false && 
                partyRolesLegalViewModel.RelatedPartyRoles_IsShareholder==false && partyRolesLegalViewModel.RelatedPartyRoles_IsUltimateBeneficiaryOwner==false && partyRolesLegalViewModel.RelatedPartyRoles_IsAuthorisedSignatory==false &&
                partyRolesLegalViewModel.RelatedPartyRoles_IsAuthorisedPerson==false && partyRolesLegalViewModel.RelatedPartyRoles_IsDesignatedEBankingUser==false && partyRolesLegalViewModel.RelatedPartyRoles_IsAuthorisedCardholder==false &&
                partyRolesLegalViewModel.RelatedPartyRoles_IsAuthorisedContactPerson==false && partyRolesLegalViewModel.RelatedPartyRoles_IsAlternateSecretery==false && partyRolesLegalViewModel.RelatedPartyRoles_IsChairmanOfTheBoardOfDirector==false &&
                partyRolesLegalViewModel.RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector==false && partyRolesLegalViewModel.RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector==false && partyRolesLegalViewModel.RelatedPartyRoles_IsTreasurerOfBoardOfDirectors==false &&
                partyRolesLegalViewModel.RelatedPartyRoles_IsMemeberOfBoardOfDirectors==false && partyRolesLegalViewModel.RelatedPartyRoles_IsPresidentOfCommittee == false && partyRolesLegalViewModel.RelatedPartyRoles_IsVicePresidentOfCommittee==false&&
                partyRolesLegalViewModel.RelatedPartyRoles_IsSecretaryOfCommittee==false && partyRolesLegalViewModel.RelatedPartyRoles_IsTreasurerOfCommittee==false && partyRolesLegalViewModel.RelatedPartyRoles_IsMemeberOfCommittee==false &&
                partyRolesLegalViewModel.RelatedPartyRoles_IsTrustee==false && partyRolesLegalViewModel.RelatedPartyRoles_IsSettlor ==false && partyRolesLegalViewModel.RelatedPartyRoles_IsProtector==false &&
                partyRolesLegalViewModel.RelatedPartyRoles_IsBenificiary==false && partyRolesLegalViewModel.RelatedPartyRoles_IsFounder==false && partyRolesLegalViewModel.RelatedPartyRoles_IsPresidentOfCouncil==false &&
                partyRolesLegalViewModel.RelatedPartyRoles_IsVicePresidentOfCouncil==false && partyRolesLegalViewModel.RelatedPartyRoles_IsSecretaryOfCouncil==false && partyRolesLegalViewModel.RelatedPartyRoles_IsTreasurerOfCouncil==false &&
                partyRolesLegalViewModel.RelatedPartyRoles_IsMemberOfCouncil==false && partyRolesLegalViewModel.RelatedPartyRoles_IsFundMlco==false && partyRolesLegalViewModel.RelatedPartyRoles_IsFundAdministrator==false &&
                partyRolesLegalViewModel.RelatedPartyRoles_IsManagementCompany==false && partyRolesLegalViewModel.RelatedPartyRoles_IsHolderOfManagementShares==false && partyRolesLegalViewModel.RelatedPartyRoles_GeneralPartner==false &&
                partyRolesLegalViewModel.RelatedPartyRoles_LimitedPartner==false && partyRolesLegalViewModel.RelatedPartyRoles_IsPartner==false)

            {
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.RelatedPartyRoles_IsDirector;
                validationError.PropertyName = "RelatedPartyRoles_IsDirector";
                lstvalidationError.Add(validationError);
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region---Address Details----
        public static ValidationResultModel ValidateAddresstDetails(AddressDetailsModel addressDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ADDRESS_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            

            if (string.IsNullOrEmpty(addressDetailsModel.AddressType))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.AddressType;
                validationError.PropertyName = "AddressType";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(addressDetailsModel.AddressLine1))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.AddressLine1;
                validationError.PropertyName = "AddressLine1";
                lstvalidationError.Add(validationError);
            }
            else if(addressDetailsModel.AddressLine1.Length > 35)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError = new ValidationError();
                validationError.ErrorMessage = ValidationConstant.MaxLength_Exceeded;
                validationError.PropertyName = "AddressLine1";
                lstvalidationError.Add(validationError);
            }

            if(!string.IsNullOrEmpty(addressDetailsModel.AddressLine2) && addressDetailsModel.AddressLine2.Length > 35)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError = new ValidationError();
                validationError.ErrorMessage = ValidationConstant.MaxLength_Exceeded;
                validationError.PropertyName = "AddressLine2";
                lstvalidationError.Add(validationError);
            }

            if (string.IsNullOrEmpty(addressDetailsModel.City))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.City;
                validationError.PropertyName = "City";
                lstvalidationError.Add(validationError);
            }
            else if(addressDetailsModel.City.Length > 35)
            {
                retVal.IsValid = false;
                ValidationError validationError = new ValidationError();
                validationError.ErrorMessage = ValidationConstant.MaxLength_Exceeded;
                validationError.PropertyName = "City";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(addressDetailsModel.Country))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.Country;
                validationError.PropertyName = "Country";
                lstvalidationError.Add(validationError);
            }
            if(!string.IsNullOrEmpty(addressDetailsModel.PostalCode) && addressDetailsModel.PostalCode.Length > 35)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError = new ValidationError();
                validationError.ErrorMessage = ValidationConstant.MaxLength_Exceeded;
                validationError.PropertyName = "PostalCode";
                lstvalidationError.Add(validationError);
            }
            if (!string.IsNullOrEmpty(addressDetailsModel.Country))
            {
                List<string> countryList = new List<string>(new string[] { "CY", "AT", "BE", "EE", "FI", "FR", "DE", "GR", "IE", "IT", "LV", "LT", "LU", "MT", "NL", "PT", "SK", "SI", "ES" });
                string countryCode = ServiceHelper.GetCountriesCode(Convert.ToInt32(addressDetailsModel.Country));
                if ((countryList.Contains(countryCode)))
                {
                    if (string.IsNullOrEmpty(addressDetailsModel.PostalCode))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.PostalCode;
                        validationError.PropertyName = "PostalCode";
                        lstvalidationError.Add(validationError);
                    }
                    
                }
                if(!string.IsNullOrEmpty(addressDetailsModel.City))
                {
                    addressDetailsModel.City = addressDetailsModel.City.Trim();
                    if(!Regex.IsMatch(addressDetailsModel.City, @"^[a-zA-Z][a-zA-Z0-9 ]*$"))
                    {
						retVal.IsValid = false;
                        ValidationError validationError = new ValidationError();
                        validationError.ErrorMessage = ValidationConstant.City_Special;
                        validationError.PropertyName = "City";
                        lstvalidationError.Add(validationError);
                    }
                    List<string> cityList = new List<string>(new string[] { "NICOSIA", "LIMASSOL", "PAPHOS", "LARNACA", "FAMAGUSTA" });
                    if(string.Equals(countryCode, "cy", StringComparison.OrdinalIgnoreCase) && !cityList.Any(k => string.Equals(k, addressDetailsModel.City, StringComparison.OrdinalIgnoreCase)))
                    {
                        retVal.IsValid = false;
                        ValidationError validationError = new ValidationError();
                        validationError.ErrorMessage = ValidationConstant.City_Cyprus;
                        validationError.PropertyName = "City";
                        lstvalidationError.Add(validationError);
                    }
                    
                }

            }
            if(!string.IsNullOrEmpty(addressDetailsModel.Email) && !CommonValidation.IsEmailValid(addressDetailsModel.Email))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ContactDetails_InvalidEmailAddress;
                validationError.PropertyName = "Email";
                lstvalidationError.Add(validationError);
            }
            if (addressDetailsModel.SaveInRegistry)
            {
                if (string.IsNullOrEmpty(addressDetailsModel.LocationName))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.SaveinRegistryLocation;
                    validationError.PropertyName = "LocationName";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region------Company Details-----
        public static ValidationResultModel ValidateCompanyDetails(CompanyDetailsRelatedPartyModel companyDetailsModel)
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
            if (string.IsNullOrEmpty(companyDetailsModel.RegisteredName))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.CompanyDetails_RegisteredName;
                validationError.PropertyName = "RegisteredName";
                lstvalidationError.Add(validationError);
            }
            //else if(!Regex.IsMatch(companyDetailsModel.RegisteredName, @"^[a-zA-Z][a-zA-Z0-9 ]*$"))
            //{
            //    retVal.IsValid = false;
            //    ValidationError validationError = new ValidationError();
            //    validationError.ErrorMessage = ValidationConstant.RegisteredName_Special;
            //    validationError.PropertyName = "RegisteredName";
            //    lstvalidationError.Add(validationError);
            //}
            if(string.IsNullOrEmpty(companyDetailsModel.RegistrationNumber))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.CompanyDetails_RegistrationNumber;
                validationError.PropertyName = "RegistrationNumber";
                lstvalidationError.Add(validationError);
            }
            DateTime DateofIncorporation;
            if (companyDetailsModel.HdnDateofIncorporation == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.DateofIncorporation;
                validationError.PropertyName = "DateofIncorporation";
                lstvalidationError.Add(validationError);
            }
            //else if(Convert.ToDateTime(companyDetailsModel.HdnDateofIncorporation) > DateTime.Today)
            else if (DateTime.TryParseExact(companyDetailsModel.HdnDateofIncorporation, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateofIncorporation))
            {
                if (DateofIncorporation > DateTime.Today)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.CompanyDetails_RegistrationDateFuture;
                    validationError.PropertyName = "DateofIncorporation";
                    lstvalidationError.Add(validationError); 
                }
            }
            if(string.IsNullOrEmpty(companyDetailsModel.CountryofIncorporation))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.CompanyDetails_CountryofIncorporation;
                validationError.PropertyName = "CountryofIncorporation";
                lstvalidationError.Add(validationError);
            }

            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion
    }
}
