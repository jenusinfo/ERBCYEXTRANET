using CMS.DataEngine;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Application.RelatedParty;
using Eurobank.Models.Application.RelatedParty.ContactDetails;
using Eurobank.Models.Application.RelatedParty.PartyRoles;
using Eurobank.Models.IdentificationDetails;
using Eurobank.Models.PEPDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
    public class RelatedPartyIndividualFormBasicValidationProcess
    {
        #region-----Party Roles-----
        public static ValidationResultModel ValidatePartyRoles(PartyRolesViewModel partyRolesViewModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.RELATED_PARTY_ROLES
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (partyRolesViewModel == null || (partyRolesViewModel.RelatedPartyRoles_HasPowerOfAttorney == false && partyRolesViewModel.RelatedPartyRoles_IsContactPerson == false && partyRolesViewModel.RelatedPartyRoles_IsEBankingUser == false))
            {
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.RelatedPartyRoles_HasPowerOfAttorney;
                validationError.PropertyName = "RelatedPartyRoles_HasPowerOfAttorney";
                lstvalidationError.Add(validationError);
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region-----Personal Details------
        public static ValidationResultModel ValidatePersonalDetails(PersonalDetailsModel personalDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.PERSONAL_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(personalDetailsModel.Title))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.Title;
                validationError.PropertyName = "Title";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(personalDetailsModel.FirstName))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.FirstName;
                validationError.PropertyName = "FirstName";
                lstvalidationError.Add(validationError);
            }
            else if (personalDetailsModel.FirstName.Length > 50)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.MaxLength_FirstName_Exceeded;
                validationError.PropertyName = "FirstName";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(personalDetailsModel.LastName))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.LastName;
                validationError.PropertyName = "LastName";
                lstvalidationError.Add(validationError);
            }
            else if (personalDetailsModel.LastName.Length > 50)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.MaxLength_LastName_Exceeded;
                validationError.PropertyName = "LastName";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(personalDetailsModel.FathersName))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.FathersName;
                validationError.PropertyName = "FathersName";
                lstvalidationError.Add(validationError);
            }
            else if (personalDetailsModel.FathersName.Length > 50)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.MaxLength_FatherName_Exceeded;
                validationError.PropertyName = "FathersName";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(personalDetailsModel.Gender))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.Gender;
                validationError.PropertyName = "Gender";
                lstvalidationError.Add(validationError);
            }
            if (!string.IsNullOrEmpty(personalDetailsModel.Gender))//Male
            {
                var gender = ServiceHelper.GetName(personalDetailsModel.Gender, Constants.GENDER);//Male
                if (!string.IsNullOrEmpty(personalDetailsModel.Title))//Miss
                {
                    var title = ServiceHelper.GetName(personalDetailsModel.Title, Constants.TITLES);
                    if (string.Equals(gender, "MALE", StringComparison.OrdinalIgnoreCase))
                    {
                        List<string> titleListMale = new List<string>(new string[] { "MR", "DR" });
                        if (!(titleListMale.Contains(title)))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = "Title selected is" + " " + title + " " + "hence Gender should be FEMALE";
                            validationError.PropertyName = "Gender";
                            lstvalidationError.Add(validationError);
                        }
                    }
                    else if (string.Equals(gender, "FEMALE", StringComparison.OrdinalIgnoreCase))
                    {
                        List<string> titleListFeMale = new List<string>(new string[] { "MRS", "DR", "MISS", "MS" });
                        if (!(titleListFeMale.Contains(title)))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = "Title selected is" + " " + title + " " + "hence Gender should be MALE";
                            validationError.PropertyName = "Gender";
                            lstvalidationError.Add(validationError);
                        }
                    }

                }
            }
            if (personalDetailsModel.DateOfBirth == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.DateOfBirth;
                validationError.PropertyName = "DateOfBirth";
                lstvalidationError.Add(validationError);
            }
            else if (personalDetailsModel.DateOfBirth != null)
            {
                DateTime dt = Convert.ToDateTime(personalDetailsModel.DateOfBirth);
                string birthYear = dt.Year.ToString();
                var today = DateTime.Today;
                // Calculate the age.
                var age = today.Year - Convert.ToInt32(birthYear);
                if (personalDetailsModel.DateOfBirth > DateTime.Today)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DateOfBirthGreater;
                    validationError.PropertyName = "DateOfBirth";
                    lstvalidationError.Add(validationError);
                }
                else if (Convert.ToInt32(birthYear) < 1930)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DateOfBirthInvalid;
                    validationError.PropertyName = "DateOfBirth";
                    lstvalidationError.Add(validationError);
                }
                else if (age < 18)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DateOfBirthInvalid;
                    validationError.PropertyName = "DateOfBirth";
                    lstvalidationError.Add(validationError);
                }
            }
            if (string.IsNullOrEmpty(personalDetailsModel.PlaceOfBirth))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.PlaceOfBirth;
                validationError.PropertyName = "PlaceOfBirth";
                lstvalidationError.Add(validationError);
            }
            if (personalDetailsModel.CountryOfBirth == 0 || personalDetailsModel.CountryOfBirth == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.CountryOfBirth;
                validationError.PropertyName = "CountryOfBirth";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(personalDetailsModel.EducationLevel))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.EducationLevel;
                validationError.PropertyName = "EducationLevel";
                lstvalidationError.Add(validationError);
            }
            bool HIDInviteFlag = SettingsKeyInfoProvider.GetBoolValue(SiteContext.CurrentSiteName + ".HIDInviteFlag");
            if (HIDInviteFlag)
            {
                if (personalDetailsModel.InvitedpersonforonlineIDverification == null)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.InvitePerson;
                    validationError.PropertyName = "InvitedpersonforonlineIDverification";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }

        public static ValidationResultModel ValidateIsPep(PersonalDetailsModel personalDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.PEP_DETAILS_RELATED_PARTY
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(personalDetailsModel.IsPepName))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.IsPep;
                validationError.PropertyName = "IsPepName";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(personalDetailsModel.IsRelatedToPepName))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.IsPepFamily;
                validationError.PropertyName = "IsRelatedToPepName";
                lstvalidationError.Add(validationError);
            }

            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region----Identification Details-----
        public ValidationResultModel ValidateIdentificationDetails(IdentificationDetailsViewModel identificationDetailsViewModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.IDENTIFICATION
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (identificationDetailsViewModel.IdentificationDetails_Citizenship == 0 || identificationDetailsViewModel.IdentificationDetails_Citizenship == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.IdentificationDetails_Citizenship;
                validationError.PropertyName = "IdentificationDetails_Citizenship";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(identificationDetailsViewModel.IdentificationDetails_TypeOfIdentification))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.IdentificationDetails_TypeOfIdentification;
                validationError.PropertyName = "IdentificationDetails_TypeOfIdentification";
                lstvalidationError.Add(validationError);
            }
            if (identificationDetailsViewModel.IdentificationDetails_Citizenship > 0 || identificationDetailsViewModel.IdentificationDetails_Citizenship != null)
            {
                var countryName = ServiceHelper.GetCountryNameById(Convert.ToInt32(identificationDetailsViewModel.IdentificationDetails_Citizenship));
                if (string.Equals(countryName, "Cyprus", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(identificationDetailsViewModel.IdentificationDetails_TypeOfIdentification))
                    {
                        var identification = ServiceHelper.GetName(identificationDetailsViewModel.IdentificationDetails_TypeOfIdentification, Constants.IDENTIFICATION_TYPE);
                        if (!string.Equals(identification, "ID", StringComparison.OrdinalIgnoreCase))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.IdentificationDetails_TypeOfIdentification_Selected_ID;
                            validationError.PropertyName = "IdentificationDetails_TypeOfIdentification";
                            lstvalidationError.Add(validationError);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(identificationDetailsViewModel.IdentificationDetails_TypeOfIdentification))
                    {
                        var identification = ServiceHelper.GetName(identificationDetailsViewModel.IdentificationDetails_TypeOfIdentification, Constants.IDENTIFICATION_TYPE);
                        if (!string.Equals(identification, "PASSPORT", StringComparison.OrdinalIgnoreCase))
                        {
                            ValidationError validationError = new ValidationError();
                            retVal.IsValid = false;
                            validationError.ErrorMessage = ValidationConstant.IdentificationDetails_TypeOfIdentification_Selected_Passport;
                            validationError.PropertyName = "IdentificationDetails_TypeOfIdentification";
                            lstvalidationError.Add(validationError);
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(identificationDetailsViewModel.IdentificationDetails_IdentificationNumber))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.IdentificationDetails_IdentificationNumber;
                validationError.PropertyName = "IdentificationDetails_IdentificationNumber";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(identificationDetailsViewModel.CountryOfIssueValue))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.CountryOfIssueValue;
                validationError.PropertyName = "CountryOfIssueValue";
                lstvalidationError.Add(validationError);
            }
            if (identificationDetailsViewModel.IdentificationDetails_IssueDate == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.IdentificationDetails_IssueDate;
                validationError.PropertyName = "IdentificationDetails_IssueDate";
                lstvalidationError.Add(validationError);
            }
            else if (identificationDetailsViewModel.IdentificationDetails_IssueDate != null)
            {
                if (DateTime.Today < identificationDetailsViewModel.IdentificationDetails_IssueDate)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.IdentificationDetails_IssueDate_Val;
                    validationError.PropertyName = "IdentificationDetails_IssueDate";
                    lstvalidationError.Add(validationError);
                }
            }
            if (identificationDetailsViewModel.IdentificationDetails_ExpiryDate == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.IdentificationDetails_ExpiryDate;
                validationError.PropertyName = "IdentificationDetails_ExpiryDate";
                lstvalidationError.Add(validationError);
            }
            else if (identificationDetailsViewModel.IdentificationDetails_ExpiryDate != null)
            {
                if (DateTime.Today >= identificationDetailsViewModel.IdentificationDetails_ExpiryDate)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.IdentificationDetails_ExpiryDate_Val;
                    validationError.PropertyName = "IdentificationDetails_ExpiryDate";
                    lstvalidationError.Add(validationError);
                }
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
            else if (addressDetailsModel.AddressLine1.Length > 35)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError = new ValidationError();
                validationError.ErrorMessage = ValidationConstant.MaxLength_Exceeded;
                validationError.PropertyName = "AddressLine1";
                lstvalidationError.Add(validationError);
            }
            if (!string.IsNullOrEmpty(addressDetailsModel.AddressLine2) && addressDetailsModel.AddressLine2.Length > 35)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError = new ValidationError();
                validationError.ErrorMessage = ValidationConstant.MaxLength_Exceeded;
                validationError.PropertyName = "AddressLine2";
                lstvalidationError.Add(validationError);
            }
            if (!string.IsNullOrEmpty(addressDetailsModel.PostalCode) && addressDetailsModel.PostalCode.Length > 35)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.MaxLength_Exceeded;
                validationError.PropertyName = "PostalCode";
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
            else if (addressDetailsModel.City.Length > 35)
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
                if (!string.IsNullOrEmpty(addressDetailsModel.City))
                {
                    addressDetailsModel.City = addressDetailsModel.City.Trim();
                    if (!Regex.IsMatch(addressDetailsModel.City, @"^[a-zA-Z][a-zA-Z0-9 ]*$"))
                    {
                        retVal.IsValid = false;
                        ValidationError validationError = new ValidationError();
                        validationError.ErrorMessage = ValidationConstant.City_Special;
                        validationError.PropertyName = "City";
                        lstvalidationError.Add(validationError);
                    }
                    List<string> cityList = new List<string>(new string[] { "NICOSIA", "LIMASSOL", "PAPHOS", "LARNACA", "FAMAGUSTA" });
                    if (string.Equals(countryCode, "cy", StringComparison.OrdinalIgnoreCase) && !cityList.Any(k => string.Equals(k, addressDetailsModel.City, StringComparison.OrdinalIgnoreCase)))
                    {
                        retVal.IsValid = false;
                        ValidationError validationError = new ValidationError();
                        validationError.ErrorMessage = ValidationConstant.City_Cyprus;
                        validationError.PropertyName = "City";
                        lstvalidationError.Add(validationError);
                    }

                }
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

        #region---Contact Details----
        public static ValidationResultModel ValidateContactDetails(ContactDetailsViewModel contactDetailsViewModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.CONTACT_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(contactDetailsViewModel.Country_Code_MobileTelNoNumber))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.Country_Code_MobileTelNoNumber;
                validationError.PropertyName = "Country_Code_MobileTelNoNumber";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(contactDetailsViewModel.ContactDetails_MobileTelNoNumber))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ContactDetails_MobileTelNoNumberRelatedParty;
                validationError.PropertyName = "ContactDetails_MobileTelNoNumber";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(contactDetailsViewModel.ContactDetails_EmailAddress))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ContactDetails_EmailAddressRelatedParty;
                validationError.PropertyName = "ContactDetails_EmailAddress";
                lstvalidationError.Add(validationError);
            }
            else if (!CommonValidation.IsEmailValid(contactDetailsViewModel.ContactDetails_EmailAddress))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ContactDetails_InvalidEmailAddress;
                validationError.PropertyName = "ContactDetails_EmailAddress";
                lstvalidationError.Add(validationError);
            }
            //if (string.IsNullOrEmpty(contactDetailsViewModel.ContactDetails_PreferredCommunicationLanguage))
            //{
            //    ValidationError validationError = new ValidationError();
            //    retVal.IsValid = false;
            //    validationError.ErrorMessage = ValidationConstant.ContactDetails_PreferredCommunicationLanguage;
            //    validationError.PropertyName = "ContactDetails_PreferredCommunicationLanguage";
            //    lstvalidationError.Add(validationError);
            //}
            //if (!string.IsNullOrEmpty(contactDetailsViewModel.Country_Code_HomeTelNoNumber))
            //{
            //    if (string.IsNullOrEmpty(contactDetailsViewModel.ContactDetails_HomeTelNoNumber))
            //    {
            //        ValidationError validationError = new ValidationError();
            //        retVal.IsValid = false;
            //        validationError.ErrorMessage = ValidationConstant.ContactDetails_HomeTelNoNumber;
            //        validationError.PropertyName = "ContactDetails_HomeTelNoNumber";
            //        lstvalidationError.Add(validationError);
            //    }
            //}
            //if (!string.IsNullOrEmpty(contactDetailsViewModel.Country_Code_WorkTelNoNumber))
            //{
            //    if (string.IsNullOrEmpty(contactDetailsViewModel.ContactDetails_WorkTelNoNumber))
            //    {
            //        ValidationError validationError = new ValidationError();
            //        retVal.IsValid = false;
            //        validationError.ErrorMessage = ValidationConstant.ContactDetails_WorkTelNoNumber;
            //        validationError.PropertyName = "ContactDetails_WorkTelNoNumber";
            //        lstvalidationError.Add(validationError);
            //    }
            //}
            //if (!string.IsNullOrEmpty(contactDetailsViewModel.Country_Code_FaxNoFaxNumber))
            //{
            //    if (string.IsNullOrEmpty(contactDetailsViewModel.ContactDetails_FaxNoFaxNumber))
            //    {
            //        ValidationError validationError = new ValidationError();
            //        retVal.IsValid = false;
            //        validationError.ErrorMessage = ValidationConstant.ContactDetails_FaxNoFaxNumber;
            //        validationError.PropertyName = "ContactDetails_FaxNoFaxNumber";
            //        lstvalidationError.Add(validationError);
            //    }
            //}
            if (string.IsNullOrEmpty(contactDetailsViewModel.Country_Code_HomeTelNoNumber) && !string.IsNullOrEmpty(contactDetailsViewModel.ContactDetails_HomeTelNoNumber))
			{
				ValidationError validationError = new ValidationError();
				retVal.IsValid = false;
				validationError.ErrorMessage = ValidationConstant.Country_Code_HomeTelNo;
				validationError.PropertyName = "Country_Code_HomeTelNoNumber";
				lstvalidationError.Add(validationError);
			}
			if (!string.IsNullOrEmpty(contactDetailsViewModel.Country_Code_HomeTelNoNumber) && string.IsNullOrEmpty(contactDetailsViewModel.ContactDetails_HomeTelNoNumber))
			{
				ValidationError validationError = new ValidationError();
				retVal.IsValid = false;
				validationError.ErrorMessage = ValidationConstant.ContactDetails_HomeTelNoNumber;
				validationError.PropertyName = "ContactDetails_HomeTelNoNumber";
				lstvalidationError.Add(validationError);
			}
			if (string.IsNullOrEmpty(contactDetailsViewModel.Country_Code_WorkTelNoNumber) && !string.IsNullOrEmpty(contactDetailsViewModel.ContactDetails_WorkTelNoNumber))
			{
				ValidationError validationError = new ValidationError();
				retVal.IsValid = false;
				validationError.ErrorMessage = ValidationConstant.Country_Code_WorkTelNo;
				validationError.PropertyName = "Country_Code_WorkTelNoNumber";
				lstvalidationError.Add(validationError);
			}
			if (!string.IsNullOrEmpty(contactDetailsViewModel.Country_Code_WorkTelNoNumber) && string.IsNullOrEmpty(contactDetailsViewModel.ContactDetails_WorkTelNoNumber))
			{
				ValidationError validationError = new ValidationError();
				retVal.IsValid = false;
				validationError.ErrorMessage = ValidationConstant.ContactDetails_WorkTelNoNumber;
				validationError.PropertyName = "ContactDetails_WorkTelNoNumber";
				lstvalidationError.Add(validationError);
			}
			if (string.IsNullOrEmpty(contactDetailsViewModel.Country_Code_FaxNoFaxNumber) && !string.IsNullOrEmpty(contactDetailsViewModel.ContactDetails_FaxNoFaxNumber))
			{
				ValidationError validationError = new ValidationError();
				retVal.IsValid = false;
				validationError.ErrorMessage = ValidationConstant.Country_Code_FaxNo;
				validationError.PropertyName = "Country_Code_FaxNoFaxNumber";
				lstvalidationError.Add(validationError);
			}
			if (!string.IsNullOrEmpty(contactDetailsViewModel.Country_Code_FaxNoFaxNumber) && string.IsNullOrEmpty(contactDetailsViewModel.ContactDetails_FaxNoFaxNumber))
			{
				ValidationError validationError = new ValidationError();
				retVal.IsValid = false;
				validationError.ErrorMessage = ValidationConstant.ContactDetails_FaxNoFaxNumber;
				validationError.PropertyName = "ContactDetails_FaxNoFaxNumber";
				lstvalidationError.Add(validationError);
			}
			retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region---Source Of Income----
        public static ValidationResultModel ValidateSourceOfIncome(SourceOfIncomeModel sourceOfIncomeModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ORGIN_OF_ANNUAL_INCOME
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(sourceOfIncomeModel.SourceOfAnnualIncome))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.SourceOfAnnualIncome;
                validationError.PropertyName = "SourceOfAnnualIncome";
                lstvalidationError.Add(validationError);
            }
            if (!string.IsNullOrEmpty(sourceOfIncomeModel.SourceOfAnnualIncome))
            {
                var annualIncome = ServiceHelper.GetName(sourceOfIncomeModel.SourceOfAnnualIncome, Constants.SOURCE_OF_ANNUAL_INCOME);
                if (string.Equals(annualIncome, "OTHER", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(sourceOfIncomeModel.SpecifyOtherSource))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.SpecifyOtherSource;
                        validationError.PropertyName = "SpecifyOtherSource";
                        lstvalidationError.Add(validationError);
                    }
                }
            }
            if (sourceOfIncomeModel.AmountOfIncome == 0 || sourceOfIncomeModel.AmountOfIncome == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.AmountOfIncome;
                validationError.PropertyName = "AmountOfIncome";
                lstvalidationError.Add(validationError);
            }
            else if (sourceOfIncomeModel.AmountOfIncome.ToString().Length > 22)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.Amount_Max;
                validationError.PropertyName = "AmountOfIncome";
                lstvalidationError.Add(validationError);
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region---Origin Of Total Assets----
        public static ValidationResultModel ValidateOriginOfTotalAssets(OriginOfTotalAssetsModel originOfTotalAssetsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ORIGIN_OF_TOTAL_ASSETS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(originOfTotalAssetsModel.OriginOfTotalAssets))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.OriginOfTotalAssets;
                validationError.PropertyName = "OriginOfTotalAssets";
                lstvalidationError.Add(validationError);
            }


            if (!string.IsNullOrEmpty(originOfTotalAssetsModel.OriginOfTotalAssets))
            {
                var totalAsset = ServiceHelper.GetName(originOfTotalAssetsModel.OriginOfTotalAssets, Constants.ORIGIN_OF_TOTAL_ASSETS);
                if (string.Equals(totalAsset, "OTHER", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(originOfTotalAssetsModel.SpecifyOtherOrigin))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.SpecifyOtherOrigin;
                        validationError.PropertyName = "SpecifyOtherOrigin";
                        lstvalidationError.Add(validationError);
                    }
                }
                if (originOfTotalAssetsModel.AmountOfTotalAsset == 0 || originOfTotalAssetsModel.AmountOfTotalAsset == null)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.AmountOfTotalAsset;
                    validationError.PropertyName = "AmountOfTotalAsset";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region---PEP Details----
        public static ValidationResultModel ValidatePepDetailsApplicant(PepApplicantViewModel pepApplicantViewModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.RELATEDPARTY_PEP_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(pepApplicantViewModel.PepApplicant_PositionOrganization))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.PepApplicant_PositionOrganization;
                validationError.PropertyName = "PepApplicant_PositionOrganization";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(pepApplicantViewModel.PepApplicant_Country))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.PepApplicant_Country;
                validationError.PropertyName = "PepApplicant_Country";
                lstvalidationError.Add(validationError);
            }
            if (pepApplicantViewModel.PepApplicant_Since == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.PepApplicant_Since;
                validationError.PropertyName = "PepApplicant_Since";
                lstvalidationError.Add(validationError);
            }
            if (pepApplicantViewModel.PepApplicant_Since != null)
            {
                if (pepApplicantViewModel.PepApplicant_Since > DateTime.Today)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.PepApplicant_Since_Validate;
                    validationError.PropertyName = "PepApplicant_Since";
                    lstvalidationError.Add(validationError);
                }

            }
            if (pepApplicantViewModel.PepApplicant_Untill == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.PepApplicant_Untill;
                validationError.PropertyName = "PepApplicant_Untill";
                lstvalidationError.Add(validationError);
            }
            if (pepApplicantViewModel.PepApplicant_Untill != null)
            {
                if (pepApplicantViewModel.PepApplicant_Untill < pepApplicantViewModel.PepApplicant_Since)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.PepApplicant_Untill_Validate;
                    validationError.PropertyName = "PepApplicant_Untill";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        public static ValidationResultModel ValidatePepDetailsFamilyAssociate(PepAssociatesViewModel pepAssociatesViewModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.PEP_DETAILS_RELATED_PARTY
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();

            if (string.IsNullOrEmpty(pepAssociatesViewModel.PepAssociates_FirstName))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.PepAssociates_FirstName;
                validationError.PropertyName = "PepAssociates_FirstName";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(pepAssociatesViewModel.PepAssociates_Surname))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.PepAssociates_Surname;
                validationError.PropertyName = "PepAssociates_Surname";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(pepAssociatesViewModel.PepAssociates_Relationship))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.PepAssociates_Relationship;
                validationError.PropertyName = "PepAssociates_Relationship";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(pepAssociatesViewModel.PepAssociates_PositionOrganization))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.PepAssociates_PositionOrganization;
                validationError.PropertyName = "PepAssociates_PositionOrganization";
                lstvalidationError.Add(validationError);
            }
            if (string.IsNullOrEmpty(pepAssociatesViewModel.PepAssociates_Country))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.PepAssociates_Country;
                validationError.PropertyName = "PepAssociates_Country";
                lstvalidationError.Add(validationError);
            }
            if (pepAssociatesViewModel.PepAssociates_Since == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.PepAssociates_Since;
                validationError.PropertyName = "PepAssociates_Since";
                lstvalidationError.Add(validationError);
            }
            if (pepAssociatesViewModel.PepAssociates_Since != null)
            {
                if (DateTime.Today < pepAssociatesViewModel.PepAssociates_Since)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.PepAssociates_Since_Validate;
                    validationError.PropertyName = "PepAssociates_Since";
                    lstvalidationError.Add(validationError);
                }
            }
            if (pepAssociatesViewModel.PepAssociates_Until == null)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.PepAssociates_Until;
                validationError.PropertyName = "PepAssociates_Until";
                lstvalidationError.Add(validationError);
            }
            if (pepAssociatesViewModel.PepAssociates_Until != null)
            {
                if (pepAssociatesViewModel.PepAssociates_Until < pepAssociatesViewModel.PepAssociates_Since)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.PepAssociates_Until_Validate;
                    validationError.PropertyName = "PepAssociates_Until";
                    lstvalidationError.Add(validationError);
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region-----Business Profile------
        public static ValidationResultModel ValidateBusinessProfile(EmploymentDetailsRelatedPartyModel employmentDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.BUSINESS_AND_FINANCIAL_PROFILE_EMPLOYMENT_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            if (string.IsNullOrEmpty(employmentDetailsModel.EmploymentStatus))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.EmploymentStatus;
                validationError.PropertyName = "Profession";
                lstvalidationError.Add(validationError);
            }
            if (!string.IsNullOrEmpty(employmentDetailsModel.EmploymentStatus))
            {
                var empStatus = ServiceHelper.GetName(employmentDetailsModel.EmploymentStatus, Constants.EMPLOYMENT_STATUS);
                if (empStatus == "RETIRED" || empStatus == "UNEMPLOYED" || empStatus == "HOMEMAKER" || empStatus == "STUDENT/MILITARY SERVICES")
                {
                }
                else
                {
                    if (string.IsNullOrEmpty(employmentDetailsModel.Profession) || employmentDetailsModel.Profession== "00000000-0000-0000-0000-000000000000")
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.Profession;
                        validationError.PropertyName = "Profession";
                        lstvalidationError.Add(validationError);
                    }
                    if (string.IsNullOrEmpty(employmentDetailsModel.EmployersName))
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.EmployersName;
                        validationError.PropertyName = "EmployersName";
                        lstvalidationError.Add(validationError);
                    }
                }
            }


            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion
    }
}
