using CMS.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.Registries
{
    public class PersonsRegistryViewModel
    {

    }
    public class PersonsRegistry
    {
        public int Id { get; set; }
        public int RowID { get; set; }
        public string NodeGUID { get; set; }
        public string NodeID { get; set; }
        [StringLength(1000)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        // [Required(ErrorMessageResourceName = "ApplicationType", ErrorMessageResourceType = typeof(PersonsRegistryModelErrorMessage))]
        [Display(Name = "Person Type")]
        public string ApplicationType { get; set; }
        public string ApplicationTypeName { get; set; }

        //[TitleRequired]
        [Display(Name = "Title")]
        public string Title { get; set; }
        public string TitleName { get; set; }
        // [Required(ErrorMessage = "Please enter a First Name.")]
        [StringLength(50)]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Only alphabetical letters are allowed.Numeric or special characters are not allowed.")]

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        // [Required(ErrorMessage = "Please enter a Last Name.")]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Only alphabetical letters are allowed.Numeric or special characters are not allowed.")]
        public string LastName { get; set; }

        // [Required(ErrorMessage = "Please enter a Last Name.")]
        [StringLength(50)]
        [Display(Name = "Father's Name")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Only alphabetical letters are allowed.Numeric or special characters are not allowed.")]
        public string FatherName { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }
        public string GenderName { get; set; }

        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> DateofBirth { get; set; }
        // [Required(ErrorMessage = "Please enter a Place of Birth.")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Only alphabetical letters are allowed.Numeric or special characters are not allowed.")]
        [Display(Name = "Place of Birth")]
        public string PlaceofBirth { get; set; }
        // [Required(ErrorMessage = "Please select a Country of Birth.")]
        [Display(Name = "Country of Birth")]
        public string CountryofBirth { get; set; }
        public string CountryofBirthName { get; set; }
        //[Required(ErrorMessage = "Please select  Education Level")]
        [Display(Name = "Education Level")]
        public string EducationLevel { get; set; }
        public string EducationLevelName { get; set; }
        //	[Required(ErrorMessage = "Please select a Country of Birth.")]
        [Display(Name = "Citizenship")]
        public string Citizenship { get; set; }
        public string CitizenshipName { get; set; }
        // [Required(ErrorMessage = "Please select an Identification Type.")]
        [Display(Name = "Type of Identification")]
        public string TypeofIdentification { get; set; }
        public string TypeofIdentificationName { get; set; }
        // [Required(ErrorMessage = "Please enter an Identification number.")]
        [StringLength(50)]
        [Display(Name = "Identification Number")]
        public string IdentificationNumber { get; set; }

        [Display(Name = "Issuing Country")]
        public string IssuingCountry { get; set; }
        public string IssuingCountryName { get; set; }
        // [Required(ErrorMessage = "Please enter a  valid Issue Date.")]
        [Display(Name = "Issue Date")]
        public Nullable<DateTime> IssueDate { get; set; }
        //[Required(ErrorMessage = "Please enter a  valid Expiry Date.")]
        [Display(Name = "Expiry Date")]
        public Nullable<DateTime> ExpiryDate { get; set; }
        // [Required(ErrorMessage = "Please select country code.")]
        public string MobileTelNoCountryCode { get; set; }
        [Display(Name = "Mobile Tel No.")]
        // [Required(ErrorMessage = "Please enter a Mobile Number.")]
        public string MobileTelNoNumber { get; set; }

        // [Required(ErrorMessage = "Please select country code.")]
        public string HomeTelNoCountryCode { get; set; }
        // [Required(ErrorMessage = "Please enter a Home Number.")]
        [Display(Name = "Home Tel No.")]
        public string HomeTelNoNumber { get; set; }

        // [Required(ErrorMessage = "Please select country code.")]
        public string WorkTelNoCountryCode { get; set; }
        //[Required(ErrorMessage = "Please enter a Work Number.")]
        [Display(Name = "Work Tel No.")]
        public string WorkTelNoNumber { get; set; }
        // [Required(ErrorMessage = "Please select country code.")]
        public string FaxNoCountryCode { get; set; }
        // [Required(ErrorMessage = "Please enter a Fax Number.")]
        [Display(Name = "Fax No.")]
        public string FaxNoFaxNumber { get; set; }

        // [Required(ErrorMessage = "Please enter a valid e-mail address.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid e-mail address")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        // [Required(ErrorMessage = "Please select Correspondence Language.")]
        [Display(Name = "Correspondence Language")]
        public string PreferredCommunicationLanguage { get; set; }
        public string PreferredCommunicationLanguageName { get; set; }
        [Display(Name = "Email Address for alerts and notification purposes")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid e-mail address")]
        public string ConsentforMarketingPurposes { get; set; }
        [Display(Name = "Created On")]
        public string CreatedDate { get; set; }
        [Display(Name = "Last Modified On")]
        public string ModyfiedDate { get; set; }
        public string NodeAliaspath { get; set; }

        //Properties for Legal Entity
        //[Required(ErrorMessage = "Please enter registered name")]
        public string RegisteredName { get; set; }

        public string TradingName { get; set; }

        public string EntityType { get; set; }

        public string CountryofIncorporation { get; set; }

        public string RegistrationNumber { get; set; }

        public Nullable<DateTime> DateofIncorporation { get; set; }

        public string ListingStatus { get; set; }

        public bool CorporationSharesIssuedToTheBearer { get; set; }

        public bool IstheEntitylocatedandoperatesanofficeinCyprus { get; set; }

        public string SharesIssuedToTheBearerName { get; set; }

        public string IsOfficeinCyprusName { get; set; }
        //Contact Details
        [Display(Name = "Preferred Mailing Address")]
        public string ContactDetailsLegal_PreferredMailingAddress { get; set; }
        [Display(Name = "Email Address for Sending Alerts")]
        public string ContactDetailsLegal_EmailAddressForSendingAlerts { get; set; }
        [Display(Name = "Preferred Communication Language")]
        public string ContactDetailsLegal_PreferredCommunicationLanguage { get; set; }
    }

    public class PersonsRegistryModelErrorMessage
    {
        public static string ApplicationType
        {
            get
            {
                return ResHelper.GetString("Eurobank.PersonRegistry.Error.ApplicationType");
            }
        }

        public const string TitleError = "Eurobank.PersonRegistry.Error.Title";
        public const string FirstNameError = "Eurobank.PersonRegistry.Error.FirstName";
        public const string LastNameError = "Eurobank.PersonRegistry.Error.LastName";
        public const string DateofBirthError = "Eurobank.PersonRegistry.Error.DateofBirth";
        public const string IdentificationNumberError = "Eurobank.PersonRegistry.Error.IdentificationNumber";
        public const string IssuingCountryError = "Eurobank.PersonRegistry.Error.IssuingCountry";
        public const string RegisteredNameError = "Eurobank.PersonRegistry.Error.RegisteredName";
    }

    public class CustomTitleValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var personsRegistry = (PersonsRegistry)validationContext.ObjectInstance;
            bool status = false;
            if (personsRegistry.Title != null && personsRegistry.Gender != null)
            {
                //if(personsRegistry.Gender.Split('|')[1].ToLower() == "male" && (personsRegistry.Title.Split('|')[1].ToLower()== "mr" || personsRegistry.Title.Split('|')[1].ToLower() == "dr"))
                //{
                //	status = true;
                //}
                //else if(personsRegistry.Gender.Split('|')[1].ToLower() == "female" && (personsRegistry.Title.Split('|')[1].ToLower() == "mrs" || personsRegistry.Title.Split('|')[1].ToLower() == "dr" || personsRegistry.Title.Split('|')[1].ToLower() == "miss" || personsRegistry.Title.Split('|')[1].ToLower() == "ms"))
                //{
                //	status = true;
                //}
                //else
                //{
                //	status = false;
                //}
            }

            return (status == true)
                ? ValidationResult.Success
                : new ValidationResult("hence Please enter a valid Title.");
        }
    }

    public class TitleRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var PersonsRegistryFormData = (PersonsRegistry)validationContext.ObjectInstance;

            if (PersonsRegistryFormData.ApplicationType== "79effa44-e02d-4846-b42d-4cd5b9b5f756" && ( PersonsRegistryFormData.Title=="" || PersonsRegistryFormData.Title == null))
            {
                return new ValidationResult("Please Select a Title (validation message from custom Attribute)");
            }
            //else
            //{
            //    return new ValidationResult("");
            //}
            return ValidationResult.Success;
        }
    }

}
