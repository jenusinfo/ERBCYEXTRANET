using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Common;
using Eurobank.Models.IdentificationDetails;
using Eurobank.Models.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
    public class RegistryValidation
    {
        #region---Address Details----
        public static ValidationResultModel ValidateAddresstDetails(AddressRegistryModel addressDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ADDRESS_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            string addressTypeName = string.Empty;

            if (string.IsNullOrEmpty(addressDetailsModel.AddressType))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.AddressType;
                validationError.PropertyName = "AddressType";
                lstvalidationError.Add(validationError);
            }
            if (!string.IsNullOrEmpty(addressDetailsModel.AddressType))
            {
                addressTypeName = ServiceHelper.GetName(addressDetailsModel.AddressType, Constants.Address_Type);
            }
            if (string.IsNullOrEmpty(addressDetailsModel.LocationName))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.LocationName;
                validationError.PropertyName = "LocationName";
                lstvalidationError.Add(validationError);
            }
            if (addressTypeName != "MAILING ADDRESS" && (addressDetailsModel.POBox == "" || addressDetailsModel.POBox == null))
            {
                if (string.IsNullOrEmpty(addressDetailsModel.AddresLine1))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.AddressLine1;
                    validationError.PropertyName = "AddresLine1";
                    lstvalidationError.Add(validationError);
                }
                else if (addressDetailsModel.AddresLine1.Length > 35)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError = new ValidationError();
                    validationError.ErrorMessage = ValidationConstant.MaxLength_Exceeded;
                    validationError.PropertyName = "AddresLine1";
                    lstvalidationError.Add(validationError);
                }
            }
            if (!string.IsNullOrEmpty(addressDetailsModel.AddresLine2) && addressDetailsModel.AddresLine2.Length > 35)
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError = new ValidationError();
                validationError.ErrorMessage = ValidationConstant.MaxLength_Exceeded;
                validationError.PropertyName = "AddresLine2";
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
            if (!string.IsNullOrEmpty(addressDetailsModel.PostalCode) && addressDetailsModel.PostalCode.Length > 35)
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

            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region-----Person Details---------------
        public static ValidationResultModel ValidatePersonsRegistry(PersonsRegistry personsRegistry, string PersonType)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.PERSON_REGISTRY,
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            //Person Details Start
            if (string.IsNullOrEmpty(personsRegistry.ApplicationType))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.PersonType;
                validationError.PropertyName = "ApplicationType";
                lstvalidationError.Add(validationError);
            }
            bool isINDIVIDUAL = string.Equals(PersonType, "INDIVIDUAL", StringComparison.OrdinalIgnoreCase);
            if (isINDIVIDUAL)
            {
                if (string.IsNullOrEmpty(personsRegistry.Title))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Title;
                    validationError.PropertyName = "Title";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(personsRegistry.FirstName))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.FirstName;
                    validationError.PropertyName = "FirstName";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(personsRegistry.LastName))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.LastName;
                    validationError.PropertyName = "LastName";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(personsRegistry.FatherName))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.FathersName;
                    validationError.PropertyName = "FatherName";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(personsRegistry.Gender))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Gender;
                    validationError.PropertyName = "Gender";
                    lstvalidationError.Add(validationError);
                }

                if (personsRegistry.DateofBirth == null)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DateOfBirth;
                    validationError.PropertyName = "DateofBirth";
                    lstvalidationError.Add(validationError);
                }
                if (!string.IsNullOrEmpty(personsRegistry.Gender))//Male
                {
                    var gender = ServiceHelper.GetName(personsRegistry.Gender, Constants.GENDER);//Male
                    if (!string.IsNullOrEmpty(personsRegistry.Title))//Miss
                    {
                        var title = ServiceHelper.GetName(personsRegistry.Title, Constants.TITLES);
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
                if (personsRegistry.DateofBirth == null)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.DateOfBirth;
                    validationError.PropertyName = "DateOfBirth";
                    lstvalidationError.Add(validationError);
                }
                else if (personsRegistry.DateofBirth != null)
                {
                    DateTime dt = Convert.ToDateTime(personsRegistry.DateofBirth);
                    string birthYear = dt.Year.ToString();
                    var today = DateTime.Today;
                    // Calculate the age.
                    var age = today.Year - Convert.ToInt32(birthYear);
                    if (personsRegistry.DateofBirth > DateTime.Today)
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
                        validationError.ErrorMessage = ValidationConstant.DateOfBirthMinor;
                        validationError.PropertyName = "DateOfBirth";
                        lstvalidationError.Add(validationError);
                    }
                }
                if (string.IsNullOrEmpty(personsRegistry.PlaceofBirth))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.PlaceOfBirth;
                    validationError.PropertyName = "PlaceOfBirth";
                    lstvalidationError.Add(validationError);
                }
                if (personsRegistry.CountryofBirth == "" || personsRegistry.CountryofBirth == null)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.CountryOfBirth;
                    validationError.PropertyName = "CountryOfBirth";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(personsRegistry.EducationLevel))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.EducationLevel;
                    validationError.PropertyName = "EducationLevel";
                    lstvalidationError.Add(validationError);
                }
                //Person Details End
                //Identification Details Start
                if (string.IsNullOrEmpty(personsRegistry.Citizenship))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.IdentificationDetails_Citizenship;
                    validationError.PropertyName = "Citizenship";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(personsRegistry.TypeofIdentification))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.IdentificationDetails_TypeOfIdentification;
                    validationError.PropertyName = "TypeOfIdentification";
                    lstvalidationError.Add(validationError);
                }
                //if (!isLegalEntity)
                {
                    if (!string.IsNullOrEmpty(personsRegistry.Citizenship))
                    {
                        var countryName = ServiceHelper.GetCountryNameById(Convert.ToInt32(personsRegistry.Citizenship));
                        if (string.Equals(countryName, "Cyprus", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!string.IsNullOrEmpty(personsRegistry.TypeofIdentification))
                            {
                                var identification = ServiceHelper.GetName(personsRegistry.TypeofIdentification, Constants.IDENTIFICATION_TYPE);
                                if (!string.Equals(identification, "ID", StringComparison.OrdinalIgnoreCase))
                                {
                                    ValidationError validationError = new ValidationError();
                                    retVal.IsValid = false;
                                    validationError.ErrorMessage = ValidationConstant.IdentificationDetails_TypeOfIdentification_Selected_ID;
                                    validationError.PropertyName = "TypeOfIdentification";
                                    lstvalidationError.Add(validationError);
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(personsRegistry.TypeofIdentification))
                            {
                                var identification = ServiceHelper.GetName(personsRegistry.TypeofIdentification, Constants.IDENTIFICATION_TYPE);
                                if (!string.Equals(identification, "PASSPORT", StringComparison.OrdinalIgnoreCase))
                                {
                                    ValidationError validationError = new ValidationError();
                                    retVal.IsValid = false;
                                    validationError.ErrorMessage = ValidationConstant.IdentificationDetails_TypeOfIdentification_Selected_Passport;
                                    validationError.PropertyName = "TypeOfIdentification";
                                    lstvalidationError.Add(validationError);
                                }
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(personsRegistry.IdentificationNumber))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.IdentificationDetails_IdentificationNumber;
                    validationError.PropertyName = "IdentificationNumber";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(personsRegistry.IssuingCountry))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.CountryOfIssue;
                    validationError.PropertyName = "IssuingCountry";
                    lstvalidationError.Add(validationError);
                }
                if (personsRegistry.IssueDate == null)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.IdentificationDetails_IssueDate;
                    validationError.PropertyName = "IssueDate";
                    lstvalidationError.Add(validationError);
                }
                else if (personsRegistry.IssueDate != null)
                {
                    if (DateTime.Today < personsRegistry.IssueDate)
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.IdentificationDetails_IssueDate_Val;
                        validationError.PropertyName = "IssueDate";
                        lstvalidationError.Add(validationError);
                    }
                }
                if (personsRegistry.ExpiryDate == null)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.IdentificationDetails_ExpiryDate;
                    validationError.PropertyName = "ExpiryDate";
                    lstvalidationError.Add(validationError);
                }
                else if (personsRegistry.ExpiryDate != null)
                {
                    if (personsRegistry.IssueDate > personsRegistry.ExpiryDate)
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.IdentificationDetails_ExpiryDate_Val;
                        validationError.PropertyName = "ExpiryDate";
                        lstvalidationError.Add(validationError);
                    }
                    else if (DateTime.Now > personsRegistry.ExpiryDate)
                    {
                        ValidationError validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.IdentificationDetails_ExpiryDate_High;
                        validationError.PropertyName = "ExpiryDate";
                        lstvalidationError.Add(validationError);
                    }
                }
                //Identification Details End
                //Contact Details Start
                if (string.IsNullOrEmpty(personsRegistry.MobileTelNoCountryCode))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Country_Code_MobileTelNoNumber;
                    validationError.PropertyName = "MobileTelNoCountryCode";
                    lstvalidationError.Add(validationError);
                }
                if(string.IsNullOrEmpty(personsRegistry.MobileTelNoNumber))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.ContactDetails_MobileTelNoNumber;
                    validationError.PropertyName = "MobileTelNoNumber";
                    lstvalidationError.Add(validationError);
                }
                //Contact Details End
            }
            else // Legal Entity
            {                
                if (string.IsNullOrEmpty(personsRegistry.EntityType))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.CompanyDetails_EntityType;
                    validationError.PropertyName = "EntityType";
                    lstvalidationError.Add(validationError);
                }
                if (!string.IsNullOrEmpty(personsRegistry.EntityType))
                {
                    //var entityTypeName = ServiceHelper.GetName(personsRegistry.EntityType, Constants.COMPANY_ENTITY_TYPE);
                    //List<string> entityListText = new List<string>(new string[] { "Provident Fund", "Pension Fund", "Trust" });
                    //if (!entityListText.Contains(entityTypeName))
                    //{
                    //    if (string.IsNullOrEmpty(personsRegistry.ListingStatus))
                    //    {
                    //        ValidationError validationError = new ValidationError();
                    //        retVal.IsValid = false;
                    //        validationError.ErrorMessage = ValidationConstant.CompanyDetails_ListingStatus;
                    //        validationError.PropertyName = "ListingStatus";
                    //        lstvalidationError.Add(validationError);
                    //    }

                    //}
                    //if (string.Equals(entityTypeName, "Public Limited Company", StringComparison.OrdinalIgnoreCase) || string.Equals(entityTypeName, "Private Limited Company", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    string countryName = personsRegistry.CountryofIncorporation;//ServiceHelper.GetCountryNameById(Convert.ToInt32(personsRegistry.CountryofIncorporation));
                    //    if (!string.IsNullOrEmpty(countryName) && (!string.Equals(countryName, "Greece", StringComparison.OrdinalIgnoreCase) && !string.Equals(countryName, "Cyprus", StringComparison.OrdinalIgnoreCase)) && string.IsNullOrEmpty(personsRegistry.SharesIssuedToTheBearerName))
                    //    {
                    //        ValidationError validationError = new ValidationError();
                    //        retVal.IsValid = false;
                    //        validationError.ErrorMessage = ValidationConstant.CompanyDetails_SharesIssuedBearer;
                    //        validationError.PropertyName = "SharesIssuedToTheBearerName";
                    //        lstvalidationError.Add(validationError);
                    //    }
                    //}
                }
                if (string.IsNullOrEmpty(personsRegistry.RegisteredName))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.CompanyDetails_RegisteredName;
                    validationError.PropertyName = "RegisteredName";
                    lstvalidationError.Add(validationError);
                }
                //else if (!CommonValidation.IsOnlyAlphabetsWithSpacesValid(personsRegistry.RegisteredName))
                //{
                //    ValidationError validationError = new ValidationError();
                //    retVal.IsValid = false;
                //    validationError.ErrorMessage = ValidationConstant.CompanyDetails_RegisteredName_Special;
                //    validationError.PropertyName = "RegisteredName";
                //    lstvalidationError.Add(validationError);
                //}
                if (personsRegistry.DateofIncorporation != null)
                {
                    if (personsRegistry.DateofIncorporation > DateTime.Today)
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
                if (string.IsNullOrEmpty(personsRegistry.CountryofIncorporation))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.CompanyDetails_CountryOfIncorporation;
                    validationError.PropertyName = "CountryofIncorporation";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(personsRegistry.RegistrationNumber))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.CompanyDetails_RegitrationNumber;
                    validationError.PropertyName = "RegistrationNumber";
                    lstvalidationError.Add(validationError);
                }
                //if (string.IsNullOrEmpty(personsRegistry.IsOfficeinCyprusName))
                //{
                //    var entityTypeName = ServiceHelper.GetName(personsRegistry.EntityType, Constants.COMPANY_ENTITY_TYPE);
                //    if (entityTypeName == "Foundation" || entityTypeName == "Trade Union" || entityTypeName == "Club / Association" || entityTypeName == "City Council / Local Authority" || entityTypeName == "Government Organization"
                //        || entityTypeName == "Semi - Government Organization" || entityTypeName == "Trust" || entityTypeName == "Provident Fund" || entityTypeName == "Pension Fund")
                //    {
                //        //ValidationError validationError = new ValidationError();
                //        //retVal.IsValid = false;
                //        //validationError.ErrorMessage = ValidationConstant.CompanyDetails_OfficeInCyprus;
                //        //validationError.PropertyName = "IsOfficeinCyprusName";
                //        //lstvalidationError.Add(validationError);
                //    }
                //    else
                //    {
                //        ValidationError validationError = new ValidationError();
                //        retVal.IsValid = false;
                //        validationError.ErrorMessage = ValidationConstant.CompanyDetails_OfficeInCyprus;
                //        validationError.PropertyName = "IsOfficeinCyprusName";
                //        lstvalidationError.Add(validationError);
                //    }

                //}
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion
    }
}
