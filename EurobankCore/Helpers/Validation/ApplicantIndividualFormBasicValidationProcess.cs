using AngleSharp.Io;
using CMS.DataEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.SiteProvider;
using DocumentFormat.OpenXml.EMMA;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.Process;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Application.RelatedParty.ContactDetails;
using Eurobank.Models.Applications.TaxDetails;
using Eurobank.Models.IdentificationDetails;
using Eurobank.Models.KendoExtention;
using Eurobank.Models.PEPDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Validation
{
    public class ApplicantIndividualFormBasicValidationProcess
    {

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
                    validationError.ErrorMessage = ValidationConstant.DateOfBirthMinor;
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
            //if (Request.Form["InvitedpersonforonlineIDverification"].Any())
            //{
            //    model.PersonalDetails.InvitedpersonforonlineIDverification = new RadioGroupViewModel() { RadioGroupValue = Request.Form["InvitedpersonforonlineIDverification"] };
            //}
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
                ApplicationModuleName = ApplicationModule.PEP_DETAILS_APPLICANT
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

        #region---PEP Details----
        public static ValidationResultModel ValidatePepDetailsApplicant(PepApplicantViewModel pepApplicantViewModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.PEP_DETAILS_APPLICANT
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
                ApplicationModuleName = ApplicationModule.PEP_DETAILS_APPLICANT
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

        #region------TAX Details------
        public static ValidationResultModel ValidateTaxDetails(TaxDetailsViewModel taxDetailsViewModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.TAX_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_CountryOfTaxResidency))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.TaxDetails_CountryOfTaxResidency;
                validationError.PropertyName = "TaxDetails_CountryOfTaxResidency";
                lstvalidationError.Add(validationError);
            }
            else if (!string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_CountryOfTaxResidency))
            {
                var countryName = ServiceHelper.GetCountryNameById(Convert.ToInt32(taxDetailsViewModel.TaxDetails_CountryOfTaxResidency));
                if (!string.Equals(countryName,"CYPRUS",StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TaxIdentificationNumber) && string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TinUnavailableReason))
                    {
                        //ValidationError validationError = new ValidationError();
                        //retVal.IsValid = false;
                        //validationError.ErrorMessage = ValidationConstant.TaxDetails_TaxIdentificationNumber;
                        //validationError.PropertyName = "TaxDetails_TaxIdentificationNumber";
                        //lstvalidationError.Add(validationError);

                        if (string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TinUnavailableReason))
                        {
                            ValidationError ovalidationError = new ValidationError();
                            retVal.IsValid = false;
                            ovalidationError.ErrorMessage = ValidationConstant.TaxDetails_TinUnavailableReason;
                            ovalidationError.PropertyName = "TaxDetails_TinUnavailableReason";
                            lstvalidationError.Add(ovalidationError);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TaxIdentificationNumber) && !string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TinUnavailableReason))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.TaxDetails_TinAndIdentificationNumber;
                    validationError.PropertyName = "TaxDetails_TinUnavailableReason";
                    lstvalidationError.Add(validationError);
                }


            }
            if (!string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TinUnavailableReason))
            {
                var tinUnavailableReason = ServiceHelper.GetName(taxDetailsViewModel.TaxDetails_TinUnavailableReason, Constants.TIN_UNAVAILABLE_REASON).TrimEnd();
                if (string.Equals(tinUnavailableReason.TrimEnd(), "Customer is unable to obtain Tax Number", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_JustificationForTinUnavalability))
                    {
                        ValidationError validationError = new ValidationError();
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

        public static ValidationResultModel ValidateTaxDetailsExtended(PersonalDetailsModel personalDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.TAX_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (personalDetailsModel == null || string.IsNullOrEmpty(personalDetailsModel.IsLiableToPayDefenseTaxInCyprusName))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.TaxDetails_LiableToPayCyprus;
                validationError.PropertyName = "IsLiableToPayDefenseTaxInCyprusName";
                lstvalidationError.Add(validationError);
            }

            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region----Identification Details-----
        public static ValidationResultModel ValidateIdentificationDetails(IdentificationDetailsViewModel identificationDetailsViewModel, string applicationType, int ApplicantId)
        {
            var AllidentificationDetails = IdentificationDetailsProcess.GetIdentificationDetails(ApplicantId);
            var identificationDetails = AllidentificationDetails?.Where(y => y.IdentificationDetailsID != identificationDetailsViewModel.IdentificationDetailsID);
			bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.IDENTIFICATION
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(identificationDetailsViewModel.CitizenshipValue))
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
            if(true)//if (!isLegalEntity) // commented for 583 issue id
            {
                if (!string.IsNullOrEmpty(identificationDetailsViewModel.CitizenshipValue))
                {
                    var countryName = ServiceHelper.GetCountryNameById(Convert.ToInt32(identificationDetailsViewModel.CitizenshipValue));
                    if (string.Equals(countryName, "Cyprus", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(identificationDetailsViewModel.IdentificationDetails_TypeOfIdentification))
                        {
                            var identification = ServiceHelper.GetName(identificationDetailsViewModel.IdentificationDetails_TypeOfIdentification, Constants.IDENTIFICATION_TYPE);
                            if (!string.Equals(identification, "ID", StringComparison.OrdinalIgnoreCase))
                            {
                                var cypruscitizenshiplist = identificationDetails?.Where(y => string.Equals(y.IdentificationDetails_CitizenshipName, "CYPRUS", StringComparison.OrdinalIgnoreCase));
                                if(cypruscitizenshiplist == null || !(cypruscitizenshiplist.Any(y => string.Equals(y.IdentificationDetails_TypeOfIdentificationName, "ID", StringComparison.OrdinalIgnoreCase))))
                                {
                                    ValidationError validationError = new ValidationError();
                                    retVal.IsValid = false;
                                    validationError.ErrorMessage = ValidationConstant.IdentificationDetails_TypeOfIdentification_Selected_ID;
                                    validationError.PropertyName = "IdentificationDetails_TypeOfIdentification";
                                    lstvalidationError.Add(validationError);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(identificationDetailsViewModel.IdentificationDetails_TypeOfIdentification))
                        {
                            if (!ServiceHelper.IsCountryEU(Convert.ToInt32(identificationDetailsViewModel.CitizenshipValue)))
                            {
                                var identification = ServiceHelper.GetName(identificationDetailsViewModel.IdentificationDetails_TypeOfIdentification, Constants.IDENTIFICATION_TYPE);
                                if (!string.Equals(identification, "PASSPORT", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (identificationDetails == null)
                                    {
                                        ValidationError validationError = new ValidationError();
                                        retVal.IsValid = false;
                                        validationError.ErrorMessage = ValidationConstant.IdentificationDetails_TypeOfIdentification_Selected_Passport;
                                        validationError.PropertyName = "IdentificationDetails_TypeOfIdentification";
                                        lstvalidationError.Add(validationError);
                                    }
                                    else
                                    {
                                        var Othercitizenshiplist = identificationDetails?.Where(y => string.Equals(y.IdentificationDetails_Citizenship.ToString(), identificationDetailsViewModel.CitizenshipValue, StringComparison.OrdinalIgnoreCase) && string.Equals(y.IdentificationDetails_TypeOfIdentificationName, "PASSPORT", StringComparison.OrdinalIgnoreCase));

                                        if (!Othercitizenshiplist.Any())
                                        {
                                            ValidationError validationError = new ValidationError();
                                            retVal.IsValid = false;
                                            validationError.ErrorMessage = ValidationConstant.IdentificationDetails_TypeOfIdentification_Selected_Passport;
                                            validationError.PropertyName = "IdentificationDetails_TypeOfIdentification";
                                            lstvalidationError.Add(validationError);
                                        }
                                    }
                                    //var Othercitizenshiplist = identificationDetails?.Where(y => !string.Equals(y.IdentificationDetails_CitizenshipName, "CYPRUS", StringComparison.OrdinalIgnoreCase));
                                    //if (Othercitizenshiplist == null || !(Othercitizenshiplist.Any(y => string.Equals(y.IdentificationDetails_TypeOfIdentificationName, "PASSPORT", StringComparison.OrdinalIgnoreCase))))
                                    //{
                                    //    ValidationError validationError = new ValidationError();
                                    //    retVal.IsValid = false;
                                    //    validationError.ErrorMessage = ValidationConstant.IdentificationDetails_TypeOfIdentification_Selected_Passport;
                                    //    validationError.PropertyName = "IdentificationDetails_TypeOfIdentification";
                                    //    lstvalidationError.Add(validationError);
                                    //}
                                } 
                            }
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
                if (identificationDetailsViewModel.IdentificationDetails_IssueDate > identificationDetailsViewModel.IdentificationDetails_ExpiryDate)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.IdentificationDetails_ExpiryDate_Val;
                    validationError.PropertyName = "IdentificationDetails_ExpiryDate";
                    lstvalidationError.Add(validationError);
                }
                else if (DateTime.Now > identificationDetailsViewModel.IdentificationDetails_ExpiryDate)
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.IdentificationDetails_ExpiryDate_High;
                    validationError.PropertyName = "IdentificationDetails_ExpiryDate";
                    lstvalidationError.Add(validationError);
                }
            }
            if (!string.IsNullOrEmpty(identificationDetailsViewModel.CountryOfIssueValue) && !string.IsNullOrEmpty(identificationDetailsViewModel.CitizenshipValue))
            {
                if (!string.Equals(identificationDetailsViewModel.CountryOfIssueValue, identificationDetailsViewModel.CitizenshipValue,StringComparison.OrdinalIgnoreCase))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.IdentificationDetails_CitizenshipIssueCountry;
                    validationError.PropertyName = "CountryOfIssueValue";
                    lstvalidationError.Add(validationError); 
                }
            }
            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region-----Business and Financial Profile------
        public static ValidationResultModel ValidateBusinessAndFinancialProfile(EmploymentDetailsModel employmentDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.BUSINESS_AND_FINANCIAL_PROFILE_EMPLOYMENT_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();

            if (string.IsNullOrEmpty(employmentDetailsModel.EmploymentStatus))
            {
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.EmploymentStatus;
                validationError.PropertyName = "EmploymentStatus";
                lstvalidationError.Add(validationError);
            }

            if (!string.IsNullOrEmpty(employmentDetailsModel.EmploymentStatus))
            {
                var employmentStatus = ServiceHelper.GetName(employmentDetailsModel.EmploymentStatus, Constants.EMPLOYMENT_STATUS);
                if (!string.Equals(employmentStatus, "HOMEMAKER", StringComparison.OrdinalIgnoreCase) && !string.Equals(employmentStatus, "STUDENT/MILITARY SERVICES", StringComparison.OrdinalIgnoreCase))
                {
                    //Others Except Retired
                    if (!string.Equals(employmentStatus, "RETIRED", StringComparison.OrdinalIgnoreCase) && !string.Equals(employmentStatus, "UNEMPLOYED", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(employmentDetailsModel.Profession) || string.Equals(employmentDetailsModel.Profession, "00000000-0000-0000-0000-000000000000", StringComparison.OrdinalIgnoreCase))
                        {
                            retVal.IsValid = false;
                            validationError = new ValidationError();
                            validationError.ErrorMessage = ValidationConstant.Profession;
                            validationError.PropertyName = "Profession";
                            lstvalidationError.Add(validationError);
                        }
                        if (string.IsNullOrEmpty(employmentDetailsModel.YearsInBusiness))
                        {
                            retVal.IsValid = false;
                            validationError = new ValidationError();
                            validationError.ErrorMessage = ValidationConstant.YearsInBusiness;
                            validationError.PropertyName = "YearsInBusiness";
                            lstvalidationError.Add(validationError);
                        }
                        else if (employmentDetailsModel.YearsInBusiness.Length > 5)
                        {
                            retVal.IsValid = false;
                            validationError = new ValidationError();
                            validationError.ErrorMessage = ValidationConstant.MaxLength_YearsInBusiness_Exceeded;
                            validationError.PropertyName = "YearsInBusiness";
                            lstvalidationError.Add(validationError);
                        }
                        if (string.IsNullOrEmpty(employmentDetailsModel.EmployersName))
                        {
                            retVal.IsValid = false;
                            validationError = new ValidationError();
                            validationError.ErrorMessage = ValidationConstant.EmployersName;
                            validationError.PropertyName = "EmployersName";
                            lstvalidationError.Add(validationError);
                        }
                        if (string.IsNullOrEmpty(employmentDetailsModel.EmployersBusiness))
                        {
                            retVal.IsValid = false;
                            validationError = new ValidationError();
                            validationError.ErrorMessage = ValidationConstant.EmployersBusiness;
                            validationError.PropertyName = "EmployersBusiness";
                            lstvalidationError.Add(validationError);
                        }
                    }
                    //For Retired Status
                    else if (string.Equals(employmentStatus, "RETIRED", StringComparison.OrdinalIgnoreCase) || string.Equals(employmentStatus, "UNEMPLOYED", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(employmentDetailsModel.FormerProfession) || string.Equals(employmentDetailsModel.FormerProfession, "00000000-0000-0000-0000-000000000000", StringComparison.OrdinalIgnoreCase))
                        {
                            retVal.IsValid = false;
                            validationError = new ValidationError();
                            validationError.ErrorMessage = ValidationConstant.FormerProfession;
                            validationError.PropertyName = "FormerProfession";
                            lstvalidationError.Add(validationError);
                        }
                        if (employmentDetailsModel.FormerCountryOfEmployment == 0)
                        {
                            retVal.IsValid = false;
                            validationError = new ValidationError();
                            validationError.ErrorMessage = ValidationConstant.FormerCountryOfEmployment;
                            validationError.PropertyName = "FormerCountryOfEmployment";
                            lstvalidationError.Add(validationError);
                        }
                        if (string.IsNullOrEmpty(employmentDetailsModel.FormerEmployersName))
                        {
                            retVal.IsValid = false;
                            validationError = new ValidationError();
                            validationError.ErrorMessage = ValidationConstant.FormerEmployersName;
                            validationError.PropertyName = "FormerEmployersName";
                            lstvalidationError.Add(validationError);
                        }
                        if (string.IsNullOrEmpty(employmentDetailsModel.FormerEmployersBusiness))
                        {
                            retVal.IsValid = false;
                            validationError = new ValidationError();
                            validationError.ErrorMessage = ValidationConstant.FormerEmployersBusiness;
                            validationError.PropertyName = "FormerEmployersBusiness";
                            lstvalidationError.Add(validationError);
                        }
                    }
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
                validationError.ErrorMessage = ValidationConstant.ContactDetails_MobileTelNoNumber;
                validationError.PropertyName = "ContactDetails_MobileTelNoNumber";
                lstvalidationError.Add(validationError);
            }
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
            if (string.IsNullOrEmpty(contactDetailsViewModel.ContactDetails_EmailAddress))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ContactDetails_EmailAddress;
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
            if (string.IsNullOrEmpty(contactDetailsViewModel.ContactDetails_PreferredCommunicationLanguage))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ContactDetails_PreferredCommunicationLanguage;
                validationError.PropertyName = "ContactDetails_PreferredCommunicationLanguage";
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
            ValidationError validationError = new ValidationError();

            if (string.IsNullOrEmpty(originOfTotalAssetsModel.OriginOfTotalAssets))
            {
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
                        retVal.IsValid = false;
                        validationError = new ValidationError();
                        validationError.ErrorMessage = ValidationConstant.SpecifyOtherOrigin;
                        validationError.PropertyName = "SpecifyOtherOrigin";
                        lstvalidationError.Add(validationError);
                    }
                }
                if (originOfTotalAssetsModel.AmountOfTotalAsset == null)
                {
                    retVal.IsValid = false;
                    validationError = new ValidationError();
                    validationError.ErrorMessage = ValidationConstant.AmountOfTotalAsset;
                    validationError.PropertyName = "AmountOfTotalAsset";
                    lstvalidationError.Add(validationError);
                }
                else if (originOfTotalAssetsModel.AmountOfTotalAsset.ToString().Length > 22)
                {
                    validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.Amount_Max;
                    validationError.PropertyName = "AmountOfTotalAsset";
                    lstvalidationError.Add(validationError);
                }
                else if (originOfTotalAssetsModel.AmountOfTotalAsset < 0)
                {
                    validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.AmountOfTotalAsset_Negetive;
                    validationError.PropertyName = "AmountOfTotalAsset";
                    lstvalidationError.Add(validationError);
                }
            }

            retVal.Errors = lstvalidationError;
            return retVal;
        }
        #endregion

        #region---Address Details----
        public static ValidationResultModel ValidateAddresstDetails(AddressDetailsModel addressDetailsModel, bool isLegalEntity, int apID)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.ADDRESS_DETAILS
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();
            ValidationError validationError = new ValidationError();
            string addressTypeName = string.Empty;
            
            if (string.IsNullOrEmpty(addressDetailsModel.AddressType))
            {
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.AddressType;
                validationError.PropertyName = "AddressType";
                lstvalidationError.Add(validationError);
            }
            if (!string.IsNullOrEmpty(addressDetailsModel.AddressType))
            {
                addressTypeName = ServiceHelper.GetName(addressDetailsModel.AddressType, Constants.Address_Type);
            }
            if (addressTypeName != "MAILING ADDRESS" && (addressDetailsModel.POBox == "" || addressDetailsModel.POBox == null))
            {
                if (string.IsNullOrEmpty(addressDetailsModel.AddressLine1))
                {
                    retVal.IsValid = false;
                    validationError = new ValidationError();
                    validationError.ErrorMessage = ValidationConstant.AddressLine1;
                    validationError.PropertyName = "AddressLine1";
                    lstvalidationError.Add(validationError);
                }
            }
            //         else if(addressDetailsModel.AddressLine1.Length > 35)
            //{
            //             retVal.IsValid = false;
            //             validationError = new ValidationError();
            //             validationError.ErrorMessage = ValidationConstant.MaxLength_Exceeded;
            //             validationError.PropertyName = "AddressLine1";
            //             lstvalidationError.Add(validationError);
            //         }

            //if(!string.IsNullOrEmpty(addressDetailsModel.AddressLine2) && addressDetailsModel.AddressLine2.Length > 35)
            //{
            //    retVal.IsValid = false;
            //    validationError = new ValidationError();
            //    validationError.ErrorMessage = ValidationConstant.MaxLength_Exceeded;
            //    validationError.PropertyName = "AddressLine2";
            //    lstvalidationError.Add(validationError);
            //}

            //if(!string.IsNullOrEmpty(addressDetailsModel.PostalCode) && addressDetailsModel.PostalCode.Length > 35)
            //{
            //    retVal.IsValid = false;
            //    validationError = new ValidationError();
            //    validationError.ErrorMessage = ValidationConstant.MaxLength_Exceeded;
            //    validationError.PropertyName = "PostalCode";
            //    lstvalidationError.Add(validationError);
            //}

            if (string.IsNullOrEmpty(addressDetailsModel.City))
            {
                retVal.IsValid = false;
                validationError = new ValidationError();
                validationError.ErrorMessage = ValidationConstant.City;
                validationError.PropertyName = "City";
                lstvalidationError.Add(validationError);
            }
            //else if(addressDetailsModel.City.Length > 35)
            //{
            //    retVal.IsValid = false;
            //    validationError = new ValidationError();
            //    validationError.ErrorMessage = ValidationConstant.MaxLength_Exceeded;
            //    validationError.PropertyName = "City";
            //    lstvalidationError.Add(validationError);
            //}
            if (string.IsNullOrEmpty(addressDetailsModel.Country))
            {
                retVal.IsValid = false;
                validationError = new ValidationError();
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
                        retVal.IsValid = false;
                        validationError = new ValidationError();
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
                        validationError = new ValidationError();
                        validationError.ErrorMessage = ValidationConstant.City_Special;
                        validationError.PropertyName = "City";
                        lstvalidationError.Add(validationError);
                    }
                    List<string> cityList = new List<string>(new string[] { "Nicosia", "Limassol", "Paphos", "Larnaca", "Famagusta" });
                    if (string.Equals(countryCode, "cy", StringComparison.OrdinalIgnoreCase) && !cityList.Any(k => string.Equals(k, addressDetailsModel.City, StringComparison.OrdinalIgnoreCase)))
                    {
                        retVal.IsValid = false;
                        validationError = new ValidationError();
                        validationError.ErrorMessage = ValidationConstant.City_Cyprus;
                        validationError.PropertyName = "City";
                        lstvalidationError.Add(validationError);
                    }

                }

            }
            if (!string.IsNullOrEmpty(addressDetailsModel.Email) && !CommonValidation.IsEmailValid(addressDetailsModel.Email))
            {
                validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.ContactDetails_InvalidEmailAddress;
                validationError.PropertyName = "Email";
                lstvalidationError.Add(validationError);
            }
            if (isLegalEntity)
            {
                string EntityTypeName = ServiceHelper.GetName(CompanyDetailsProcess.GetCompanyDetailsModelById(apID).EntityType, Constants.COMPANY_ENTITY_TYPE); ;
                if (string.Equals(addressTypeName, "PRINCIPAL TRADING /BUSINESS OFFICE", StringComparison.OrdinalIgnoreCase))
                {

                    if (string.IsNullOrEmpty(addressDetailsModel.PhoneNo))
                    {
                        validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.Phone_No;
                        validationError.PropertyName = "PhoneNo";
                        lstvalidationError.Add(validationError);
                    }
                    else if (string.IsNullOrEmpty(addressDetailsModel.CountryCode_PhoneNo))
                    {
                        validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.PhoneCountryCode;
                        validationError.PropertyName = "CountryCode_PhoneNo";
                        lstvalidationError.Add(validationError);
                    }
                    if(!string.IsNullOrEmpty(addressDetailsModel.CountryCode_FaxNo) && string.IsNullOrEmpty(addressDetailsModel.FaxNo))
                    {
                        validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.Fax_No_Enter;
                        validationError.PropertyName = "FaxNo";
                        lstvalidationError.Add(validationError);
                    }
                    if (string.IsNullOrEmpty(addressDetailsModel.CountryCode_FaxNo) && !string.IsNullOrEmpty(addressDetailsModel.FaxNo))
                    {
                        validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.PhoneCountryCode;
                        validationError.PropertyName = "CountryCode_FaxNo";
                        lstvalidationError.Add(validationError);
                    }
                    //if (string.IsNullOrEmpty(addressDetailsModel.FaxNo))
                    //{
                    //    validationError = new ValidationError();
                    //    retVal.IsValid = false;
                    //    validationError.ErrorMessage = ValidationConstant.Fax_No;
                    //    validationError.PropertyName = "FaxNo";
                    //    lstvalidationError.Add(validationError);
                    //}
                    //if (string.IsNullOrEmpty(addressDetailsModel.Email))
                    //{
                    //    validationError = new ValidationError();
                    //    retVal.IsValid = false;
                    //    validationError.ErrorMessage = ValidationConstant.Email;
                    //    validationError.PropertyName = "Email";
                    //    lstvalidationError.Add(validationError);
                    //}
                }
                if (string.Equals(addressTypeName, "OFFICE IN CYPRUS", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(addressDetailsModel.PhoneNo))
                    {
                        validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.Phone_No;
                        validationError.PropertyName = "PhoneNo";
                        lstvalidationError.Add(validationError);
                    }
                    else if (string.IsNullOrEmpty(addressDetailsModel.CountryCode_PhoneNo))
                    {
                        validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.PhoneCountryCode;
                        validationError.PropertyName = "CountryCode_PhoneNo";
                        lstvalidationError.Add(validationError);
                    }
                    //if (!string.IsNullOrEmpty(addressDetailsModel.CountryCode_FaxNo) && string.IsNullOrEmpty(addressDetailsModel.FaxNo))
                    //{
                    //    validationError = new ValidationError();
                    //    retVal.IsValid = false;
                    //    validationError.ErrorMessage = ValidationConstant.Fax_No_Enter;
                    //    validationError.PropertyName = "FaxNo";
                    //    lstvalidationError.Add(validationError);
                    //}
                    //if (string.IsNullOrEmpty(addressDetailsModel.CountryCode_FaxNo) && !string.IsNullOrEmpty(addressDetailsModel.FaxNo))
                    //{
                    //    validationError = new ValidationError();
                    //    retVal.IsValid = false;
                    //    validationError.ErrorMessage = ValidationConstant.PhoneCountryCode;
                    //    validationError.PropertyName = "CountryCode_FaxNo";
                    //    lstvalidationError.Add(validationError);
                    //}
                    if (string.IsNullOrEmpty(addressDetailsModel.Email))
                    {
                        validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.Email;
                        validationError.PropertyName = "Email";
                        lstvalidationError.Add(validationError);
                    }
                    if (string.IsNullOrEmpty(addressDetailsModel.NumberOfStaffEmployed))
                    {
                        validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.NumberOfStaffEmployed;
                        validationError.PropertyName = "NumberOfStaffEmployed";
                        lstvalidationError.Add(validationError);
                    }
                }
                if (string.Equals(EntityTypeName, "TRUST", StringComparison.OrdinalIgnoreCase) && string.Equals(addressTypeName, "ADMINISTRATION OFFICE", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(addressDetailsModel.PhoneNo))
                    {
                        validationError = new ValidationError();
                        retVal.IsValid = false;
                        validationError.ErrorMessage = ValidationConstant.Phone_No;
                        validationError.PropertyName = "PhoneNo";
                        lstvalidationError.Add(validationError);
                    }
                }
            }
            if (addressDetailsModel.SaveInRegistry)
            {
                if (string.IsNullOrEmpty(addressDetailsModel.LocationName))
                {
                    validationError = new ValidationError();
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

        #region Banking Relationship

        public static ValidationResultModel ValidateBankingRelationshipIndividual(PersonalDetailsModel personalDetailsModel)
        {
            ValidationResultModel retVal = new ValidationResultModel()
            {
                IsValid = true,
                ApplicationModuleName = ApplicationModule.EXISTING_BANK_RELATIONSHIP
            };
            List<ValidationError> lstvalidationError = new List<ValidationError>();


            if (string.IsNullOrEmpty(personalDetailsModel.HasAccountInOtherBankName))
            {
                ValidationError validationError = new ValidationError();
                retVal.IsValid = false;
                validationError.ErrorMessage = ValidationConstant.BankRelationship_HasAccountInOtherBank;
                validationError.PropertyName = "HasAccountInOtherBankName";
                lstvalidationError.Add(validationError);
            }
            else if (string.Equals(personalDetailsModel.HasAccountInOtherBankName, "true", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(personalDetailsModel.NameOfBankingInstitution))
                {
                    ValidationError validationError = new ValidationError();
                    retVal.IsValid = false;
                    validationError.ErrorMessage = ValidationConstant.BankRelationship_NameOfBankingInstitution;
                    validationError.PropertyName = "NameOfBankingInstitution";
                    lstvalidationError.Add(validationError);
                }
                if (string.IsNullOrEmpty(personalDetailsModel.CountryOfBankingInstitution))
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
    }
}
