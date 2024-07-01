using AngleSharp.Text;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Localization;
using CMS.SiteProvider;
using DocumentFormat.OpenXml.Presentation;
using Eurobank.Helpers.CustomHandler;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.External.SSP.Application.Individual;
using Eurobank.Helpers.External.SSP.Application.Legal;
using Eurobank.Helpers.Generics;
using Eurobank.Helpers.Process;
using Eurobank.Helpers.Validation;
using Eurobank.Models;
using Eurobank.Models.Application;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Applicant.LegalEntity;
using Eurobank.Models.Application.Applicant.LegalEntity.ContactDetails;
using Eurobank.Models.Application.Applicant.LegalEntity.CRS;
using Eurobank.Models.Application.Applicant.LegalEntity.FATCACRS;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Application.RelatedParty;
using Eurobank.Models.Application.RelatedParty.ContactDetails;
using Eurobank.Models.Applications;
using Eurobank.Models.Applications.DebitCard;
using Eurobank.Models.Applications.TaxDetails;
using Eurobank.Models.Documents;
using Eurobank.Models.IdentificationDetails;
using Eurobank.Models.KendoExtention;
using Eurobank.Models.PEPDetails;
using Eurobank.Models.User;
using Eurobank.Services;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Controllers
{
    [AuthorizeRoles(Role.PowerUser, Role.NormalUser)]
    [SessionAuthorization]
    //[AutoValidateAntiforgeryToken]
    public class ApplicantController : Controller
    {
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly ApplicationsRepository applicationsRepository;
        private readonly TaxDetailsRepository taxDetailsRepository;
        private readonly PepApplicantRepository pepApplicantRepository;
        private readonly PepAssociatesRepository pepAssociatesRepository;
        private readonly AddressDetailsRepository addressDetailsRepository;
        private readonly SourceOfIncomeRepository sourceOfIncomeRepository;
        private readonly OriginOfTotalAssetsRepository originOfTotalAssetsRepository;
        private readonly SignatureMandateCompanyRepository signatureMandateCompanyRepository;
        private readonly SignatoryGroupRepository signatoryGroupRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ContactDetailsRepository contactDetailsRepository;
        private readonly PersonalDetailsRepository personalDetailsRepository;
        private readonly CompanyDetailsRepository companyDetailsRepository;
        private readonly FATCACRSDetailsRepository fATCACRSDetailsRepository;
        private readonly CRSDetailsRepository cRSDetailsRepository;
        private readonly ContactDetailsLegalRepository contactDetailsLegalRepository;
        private readonly RegistriesRepository registriesRepository;

        private readonly BankDocumentsRepository bankDocumentsRepository;
        private readonly ExpectedDocumentsRepository expectedDocumentsRepository;
        private readonly DebitCardDetailsRepository debitCardDetailsRepository;
        public ApplicantController(IPageDataContextRetriever pageDataContextRetriever,
            IPageUrlRetriever pageUrlRetriever,
             ApplicationsRepository applicationsRepository, TaxDetailsRepository taxDetailsRepository, PepApplicantRepository pepApplicantRepository, PepAssociatesRepository pepAssociatesRepository, AddressDetailsRepository addressDetailsRepository, SourceOfIncomeRepository sourceOfIncomeRepository, OriginOfTotalAssetsRepository originOfTotalAssetsRepository, SignatureMandateCompanyRepository signatureMandateCompanyRepository
            , SignatoryGroupRepository signatoryGroupRepository, IHttpContextAccessor httpContextAccessor, ContactDetailsRepository contactDetailsRepository, PersonalDetailsRepository personalDetailsRepository, CompanyDetailsRepository companyDetailsRepository, FATCACRSDetailsRepository fATCACRSDetailsRepository, CRSDetailsRepository cRSDetailsRepository
            , ContactDetailsLegalRepository contactDetailsLegalRepository, RegistriesRepository registriesRepository, BankDocumentsRepository bankDocumentsRepository, ExpectedDocumentsRepository expectedDocumentsRepository, DebitCardDetailsRepository debitCardDetailsRepository)
        {
            this.pageDataContextRetriever = pageDataContextRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
            this.applicationsRepository = applicationsRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.pepApplicantRepository = pepApplicantRepository;
            this.pepAssociatesRepository = pepAssociatesRepository;
            this.taxDetailsRepository = taxDetailsRepository;
            this.addressDetailsRepository = addressDetailsRepository;
            this.sourceOfIncomeRepository = sourceOfIncomeRepository;
            this.originOfTotalAssetsRepository = originOfTotalAssetsRepository;
            this.signatureMandateCompanyRepository = signatureMandateCompanyRepository;
            this.signatoryGroupRepository = signatoryGroupRepository;
            this.contactDetailsRepository = contactDetailsRepository;
            this.personalDetailsRepository = personalDetailsRepository;
            this.companyDetailsRepository = companyDetailsRepository;
            this.fATCACRSDetailsRepository = fATCACRSDetailsRepository;
            this.cRSDetailsRepository = cRSDetailsRepository;
            this.contactDetailsLegalRepository = contactDetailsLegalRepository;
            this.registriesRepository = registriesRepository;
            this.bankDocumentsRepository = bankDocumentsRepository;
            this.expectedDocumentsRepository = expectedDocumentsRepository;
            this.debitCardDetailsRepository = debitCardDetailsRepository;
        }
        #region Applicant
        public IActionResult Index(string application, string applicant)
        
        {
            //Get applicationID from NodeGUID
            var applicationDetail = applicationsRepository.GetApplicationDetailsByNodeGUID(application);
            //Get applicantID from NodeGUID
            int applicationId = applicationDetail.ApplicationDetailsID;
            string entityTypeCode = ValidationHelper.GetString(ServiceHelper.GetEntityType(applicationDetail.ApplicationDetails_ApplicationType, Constants.APPLICATION_TYPE), "");
            int applicantId = 0;
            if (!string.IsNullOrEmpty(applicant))
            {
                if (string.Equals(entityTypeCode, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    var applicantDetailsLegal = CompanyDetailsProvider.GetCompanyDetails(new Guid(applicant), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    if (applicantDetailsLegal != null)
                        applicantId = applicantDetailsLegal.CompanyDetailsID;
                }
                else
                {
                    var applicantDetailsIndividual = PersonalDetailsProvider.GetPersonalDetails(new Guid(applicant), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    if (applicantDetailsIndividual != null)
                        applicantId = applicantDetailsIndividual.PersonalDetailsID;
                }
            }
            //---------------------------------------------------

            ApplicantModel model = new ApplicantModel();
            model.ApplicantId = applicantId;
            if (applicationId == 0 && applicantId == 0)
                return Redirect("/Applications");
            UserModel userModel = UserProcess.GetUser(User.Identity.Name);
            //var applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);
            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationId);
            if (applicationDetails == null && applicantId == 0)
                return Redirect("/Applications");

            var applicationTypes = ServiceHelper.GetApplicationType();
            bool isLegalEntity = false;
            if (TempData["ErrorSummary"] != null)
            {
                ViewBag.ErrorSummaryMsgs = JsonConvert.DeserializeObject<List<ValidationResultModel>>(TempData["ErrorSummary"].ToString());
            }
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMsg = TempData["SuccessMessage"].ToString();
            }

            if (applicationDetails != null)
            {
                var applicationDetailModel = ApplicationsProcess.GetApplicationsDetails(userModel.UserType, userModel.UserRole, applicationDetails);
                model.ApplicationNumber = applicationDetails.ApplicationDetails_ApplicationNumber;
                model.ApplicationId = applicationId;
                model.Application_NodeGUID = applicationDetailModel.Application_NodeGUID;
                model.Type = (applicationTypes != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationType) && applicationTypes.Any(k => string.Equals(k.Value, applicationDetails.ApplicationDetails_ApplicationType, StringComparison.OrdinalIgnoreCase))) ? applicationTypes.FirstOrDefault(k => string.Equals(k.Value, applicationDetails.ApplicationDetails_ApplicationType, StringComparison.OrdinalIgnoreCase)).Text : string.Empty;
                isLegalEntity = string.Equals(model.Type, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
                //model.ApplicationStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                model.ApplicationStatus = ServiceHelper.GetName(ValidationHelper.GetString(applicationDetails.ApplicationDetails_ApplicationStatus, ""), "/Lookups/General/APPLICATION-WORKFLOW-STATUS");
                //model.Type = applicationDetails.ApplicationDetails_ApplicationTypeName;
                model.Introducer = applicationDetails.ApplicationDetails_IntroducerName;
                model.IsEdit = applicationDetailModel.IsEdit;
                model.LeftMenuApplicant = new LeftMenuCommon();
                if (isLegalEntity)
                {
                    if (applicantId > 0)
                    {
                        model.LeftMenuApplicant.LeftMenuItems = LeftMenuProcess.GetApplicantConfigurationLegal();
                    }
                    else
                    {
                        model.LeftMenuApplicant.LeftMenuItems = LeftMenuProcess.GetApplicantConfigurationLegalCreate();
                    }
                    model.LeftMenuApplicant.LeftmenuClientMathodName = LeftMenuClientMathod.onApplicantStepperSelect;
                }
                else
                {
                    if (applicantId > 0)
                    {
                        model.LeftMenuApplicant.LeftMenuItems = LeftMenuProcess.GetApplicantConfiguration();
                    }
                    else
                    {
                        model.LeftMenuApplicant.LeftMenuItems = LeftMenuProcess.GetApplicantConfigurationCreate();
                    }
                    model.LeftMenuApplicant.LeftmenuClientMathodName = LeftMenuClientMathod.onApplicantStepperSelect;
                }
            }
            if (applicantId > 0)
            {

                model.EmploymentDetails = EmploymentDetailsProcess.GetEmploymentDetailsModelByApplicantId(applicantId);
                if (isLegalEntity)
                {
                    model.CompanyDetails = CompanyDetailsProcess.GetCompanyDetailsModelById(applicantId);


                    if (model.CompanyDetails != null)
                    {
                        if (model.CompanyDetails.DateofIncorporation == DateTime.MinValue)
                        {
                            model.CompanyDetails.DateofIncorporation = null;
                        }
                        model.CompanyDetails.HdnDateofIncorporation = model.CompanyDetails.DateofIncorporation.HasValue ? model.CompanyDetails.DateofIncorporation.Value.ToString("dd/MM/yyyy") : null;
                        model.NodeGUID = model.CompanyDetails.NodeGUID;
                        model.NodePath = model.CompanyDetails.NodePath;
                        model.CompanyDetails.SaveInRegistry = (model.CompanyDetails.RegistryId > 0);
                    }
                    //model.LeftMenuApplicant = LeftMenuProcess.GetApplicantConfigurationLegal();
                    var applicationData = companyDetailsRepository.GetCompanyDetailsByID(applicantId);
                    string Path = applicationData.NodeAliasPath;
                    var contactDetailsTreeNode = contactDetailsLegalRepository.GetContactDetailsLegal(Path);

                    if (contactDetailsTreeNode != null && contactDetailsTreeNode.Any())
                    {
                        model.ContactDetailsLegal = ContactDetailsLegalProcess.GetContactDetailsLegalById(contactDetailsTreeNode.FirstOrDefault());
                        if (model.CompanyDetails != null && model.ContactDetailsLegal != null)
                        {
                            model.ContactDetailsLegal.ContactDetailsLegal_SaveInRegistry = (model.CompanyDetails.RegistryId > 0);
                        }
                    }

                    model.CompanyBusinessProfile = CompanyBusinessProfileProcess.GetCompanyBusinessProfileModelByApplicantId(applicantId);
                    model.CompanyFinancialInformation = CompanyFinancialInformationProcess.GetCompanyFinancialInformationModelByApplicantId(applicantId);
                    if (model.CompanyBusinessProfile == null)
                    {
                        model.CompanyBusinessProfile = new CompanyBusinessProfileModel();
                    }
                    if (model.CompanyFinancialInformation == null)
                    {
                        model.CompanyFinancialInformation = new CompanyFinancialInformationModel();
                    }

                    var FATCADetailsTreeNode = fATCACRSDetailsRepository.GetFATCACRSDetails(Path);
                    if (FATCADetailsTreeNode != null && FATCADetailsTreeNode.Any())
                    {
                        model.FATCACRSDetails = FATCACRSDetailsProcess.GetFATCACRSDetailsById(FATCADetailsTreeNode.FirstOrDefault());
                    }
                    var CRSDetailsTreeNode = cRSDetailsRepository.GetCRSDetails(Path);
                    if (CRSDetailsTreeNode != null && CRSDetailsTreeNode.Any())
                    {
                        model.CRSDetails = FATCACRSDetailsProcess.GetCRSDetailsById(CRSDetailsTreeNode.FirstOrDefault());
                    }
                }
                else
                {
                    model.PersonalDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicantId);

                    //model.PersonalDetails.InvitedpersonforonlineIDverification = ControlBinder.BindRadioButtonGroupItems(ServiceHelper.GetHID(), "");
                    if (model.PersonalDetails != null)
                    {
                        if (model.PersonalDetails.DateOfBirth == DateTime.MinValue)
                        {
                            model.PersonalDetails.DateOfBirth = null;
                        }
                        model.PersonalDetails.HdnDateOfBirth = model.PersonalDetails.DateOfBirth.HasValue ? model.PersonalDetails.DateOfBirth.Value.ToString("dd/MM/yyyy") : null;
                        model.NodeGUID = model.PersonalDetails.NodeGUID;
                        model.NodePath = model.PersonalDetails.NodePath;
                        model.PersonalDetails.InvitedpersonforonlineIDverification = ControlBinder.BindRadioButtonGroupItems(ServiceHelper.GetHID(), model.PersonalDetails.HIDInviteFlag.ToString());
                    }
                    //model.LeftMenuApplicant = LeftMenuProcess.GetApplicantConfiguration();
                    //Contact Details get
                    var applicatioData = personalDetailsRepository.GetPersonalDetailsByID(applicantId);
                    string Path = applicatioData.NodeAliasPath;
                    var contactDetailsTreeNode = contactDetailsRepository.GetContactDetails(Path);

                    if (contactDetailsTreeNode != null && contactDetailsTreeNode.Any())
                    {
                        model.ContactDetails = ContactDetailsProcess.GetContactDetailsById(contactDetailsTreeNode.FirstOrDefault());
                        if (model.PersonalDetails != null && model.ContactDetails != null)
                        {
                            model.ContactDetails.ContactDetails_SaveInRegistry = (model.PersonalDetails.PersonRegistryId > 0);

                        }
                    }
                    model.PersonalDetails.SaveInRegistry = (model.PersonalDetails.PersonRegistryId > 0);
                }

                if (model.EmploymentDetails == null)
                {
                    model.EmploymentDetails = new EmploymentDetailsModel();
                }
                //if(model.CompanyDetails == null)
                //{
                //    model.CompanyDetails = new CompanyDetailsModel();
                //}

                model.Id = applicantId;
                //model.NodeGUID = model.PersonalDetails.NodeGUID;
                //model.NodePath = model.PersonalDetails.NodePath;

            }
            else
            {
                if (!string.Equals(model.Type, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    model.PersonalDetails = new PersonalDetailsModel();
                    model.PersonalDetails.InvitedpersonforonlineIDverification = ControlBinder.BindRadioButtonGroupItems(ServiceHelper.GetHID(), "");
                }
                else
                {
                    model.CompanyDetails = new CompanyDetailsModel();
                }


            }
            ViewBag.PersonTitle = ServiceHelper.GetTitle();
            ViewBag.Gender = ServiceHelper.GetGendar();
            ViewBag.Education = ServiceHelper.GetEducationLevel();
            //ViewBag.Country = ServiceHelper.GetCountries();
            ViewBag.Country = ServiceHelper.GetCountriesWithID();
            ViewBag.CountryIdentification = ServiceHelper.GetCountriesWithID();
            if (isLegalEntity)
            {
                ViewBag.identificationType = ServiceHelper.GetTypeIdentificationLegal();
                //ViewBag.AddressTypes = ServiceHelper.GetAddressType();
                ViewBag.AddressTypes = ServiceHelper.GetAddressTypeLegal(model.CompanyDetails.EntityType);
                ViewBag.OriginsOfTotalAssets = ServiceHelper.GetOriginOfTotalAssets();
                ViewBag.ApplicantEntityType = ServiceHelper.GetName(model.CompanyDetails.EntityType, Constants.COMPANY_ENTITY_TYPE);
            }
            else
            {
                ViewBag.identificationType = ServiceHelper.GetTypeIdentificationIndividual();
                ViewBag.AddressTypes = ServiceHelper.GetAddressTypePhysical();
                ViewBag.OriginsOfTotalAssets = ServiceHelper.GetOriginOfTotalAssetsForIndividual();

            }


            ViewBag.EmploymentStatuses = ServiceHelper.GetEmploymentStatus();
            ViewBag.EmploymentProfessions = ServiceHelper.GetEmploymentProfessions();
            ViewBag.CountriesOfEmployment = ServiceHelper.GetCountriesWithID();

            ViewBag.SourcesOfAnnualIncome = ServiceHelper.GetSourcesOfAnnualIncome();

            ViewBag.CompanyEntities = ServiceHelper.GetCompanyEntityTypes();
            ViewBag.ListingStatuses = ServiceHelper.GetListingStatuses();
            ViewBag.CorporationShares = ServiceHelper.GetYesNoDropDownListDefaults();
            ViewBag.EntityLocatedAndOperates = ServiceHelper.GetYesNoDropDownListDefaults();
            //ViewBag.Countries = ServiceHelper.GetCountries();
            ViewBag.Countries = ServiceHelper.GetCountriesWithID();
            ViewBag.SignatureRights = ServiceHelper.GetSignatureRights();
            ViewBag.AuthorizedGroup = SignatoryGroupProcess.GetSignatoryGroups();
            ViewBag.IsEngagedInTheProvisionOfFinance = ServiceHelper.GetBoolDropDownListDefaults();
            ViewBag.EconomicSectorIndustries = ServiceHelper.GetEconomicSectorIndustries();
            //Below Value needs to be changed
            ViewBag.SignatoryPersons = ServiceHelper.GetNoteDetailPendingOnUsers();
            ViewBag.IsLiableToPayDefenseTaxInCyprus = ServiceHelper.GetBoolDropDownListDefaults();
            ApplicationViewModel _model = new ApplicationViewModel();
            ApplicationDetailsModelView applicationDetailsModelView = new ApplicationDetailsModelView();
            applicationDetailsModelView.ApplicationDetails_ApplicationNumber = model.ApplicationNumber;
            applicationDetailsModelView.ApplicationDetails_ApplicationStatus = model.ApplicationStatus;
            applicationDetailsModelView.ApplicationDetails_ApplicationStatusName = ServiceHelper.GetName(ValidationHelper.GetString(applicationDetails.ApplicationDetails_ApplicationStatus, ""), "/Lookups/General/APPLICATION-WORKFLOW-STATUS");
            applicationDetailsModelView.ApplicationDetails_ApplicationTypeName = model.Type;
            applicationDetailsModelView.ApplicationDetails_IntroducerName = model.Introducer;
            applicationDetailsModelView.ApplicationDetails_SubmittedOn = Convert.ToDateTime(applicationDetails.ApplicationDetails_SubmittedOn).ToString("dd/MM/yyyy");
            applicationDetailsModelView.ApplicationDetails_SubmittedBy = applicationDetails.ApplicationDetails_SubmittedBy;
            applicationDetailsModelView.ApplicationDetails_CreatedOn = Convert.ToDateTime(applicationDetails.DocumentCreatedWhen).ToString("dd/MM/yyyy");
            _model.ApplicationDetails = applicationDetailsModelView;
            _model.LeftMenuApplicant = model.LeftMenuApplicant;
            TempData["Application"] = _model;
            model.Application_Type = applicationDetails.ApplicationDetails_ApplicationType;
            ViewBag.IsRelatedParty = false;
            TempData["IsRelatedParty"] = false;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(int applicationId, ApplicantModel model, string applicantButton)
        {
            int PersonID = 0;
            string applicantNodeGUID = string.Empty;
            bool totalConfirmValidation = false;
            string applicationType = ValidationHelper.GetString(ServiceHelper.GetEntityType(model.Application_Type, Constants.APPLICATION_TYPE), "");
            bool isLegalEntity = string.Equals(applicationType, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase);
            List<ValidationResultModel> applicantValidationResult = null;
            UserModel userModel = UserProcess.GetUser(User.Identity.Name);
            if (Request.Form["InvitedpersonforonlineIDverification"].Any())
            {
                model.PersonalDetails.InvitedpersonforonlineIDverification = new RadioGroupViewModel() { RadioGroupValue = Request.Form["InvitedpersonforonlineIDverification"] };
            }
            if (model != null && string.Equals(applicantButton, "PROCEED", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(applicationType, "INDIVIDUAL", StringComparison.OrdinalIgnoreCase))
                {
                    if (ApplicantProcess.GetApplicantModels(model.ApplicationNumber) != null)
                    {
                        List<ValidationResultModel> validationResultModels = new List<ValidationResultModel>();
                        List<ValidationError> lstvalidationError = new List<ValidationError>();
                        ValidationError validationError = new ValidationError();
                        ValidationResultModel applicationValidationModel = new ValidationResultModel()
                        {
                            IsValid = false,
                            ApplicationModuleName = ApplicationModule.APPLICANTS
                        };
                        validationError.ErrorMessage = ValidationConstant.Applicant_Already_Created;
                        lstvalidationError.Add(validationError);
                        applicationValidationModel.Errors = lstvalidationError;
                        validationResultModels.Add(applicationValidationModel);

                        applicantValidationResult = validationResultModels;
                        TempData["ErrorSummary"] = JsonConvert.SerializeObject(applicantValidationResult);
                        return RedirectToAction("Index", "Applicant", new { application = model.Application_NodeGUID, applicant = applicantNodeGUID });
                    }
                }
                else if (string.Equals(applicationType, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    if (ApplicantProcess.GetLegalApplicantModels(model.ApplicationNumber) != null)
                    {
                        List<ValidationResultModel> validationResultModels = new List<ValidationResultModel>();
                        List<ValidationError> lstvalidationError = new List<ValidationError>();
                        ValidationError validationError = new ValidationError();
                        ValidationResultModel applicationValidationModel = new ValidationResultModel()
                        {
                            IsValid = false,
                            ApplicationModuleName = ApplicationModule.APPLICANTS
                        };
                        validationError.ErrorMessage = ValidationConstant.Applicant_Already_Created;
                        lstvalidationError.Add(validationError);
                        applicationValidationModel.Errors = lstvalidationError;
                        validationResultModels.Add(applicationValidationModel);

                        applicantValidationResult = validationResultModels;
                        TempData["ErrorSummary"] = JsonConvert.SerializeObject(applicantValidationResult);
                        return RedirectToAction("Index", "Applicant", new { application = model.Application_NodeGUID, applicant = applicantNodeGUID });
                    }
                }

            }
            //if (!ModelState.IsValid || model == null || (string.IsNullOrEmpty(model.ApplicationNumber) && (model == null || model.PersonalDetails.Id == 0) && model.CompanyDetails.Id == 0))
            if (model == null || ((string.IsNullOrEmpty(model.ApplicationNumber) && (model.PersonalDetails.Id == 0) && model.CompanyDetails.Id == 0)))
            {
                return View(model);
            }
            else
            {
                if (model.PersonalDetails != null && !isLegalEntity)
                {
                    //if (!string.IsNullOrEmpty(model.PersonalDetails.FirstName))
                    //{
                    model.PersonalDetails.Type = ApplicationsProcess.GetApplicationDetailsById(applicationId).ApplicationDetails_ApplicationType;
                    var test = ApplicantProcess.GetApplicantModels(model.ApplicationNumber);
                    string entityTypeCode = ValidationHelper.GetString(ServiceHelper.GetEntityType(model.Application_Type, Constants.APPLICATION_TYPE), "");
                    if (!string.IsNullOrEmpty(model.PersonalDetails.HdnDateOfBirth))
                    {

                        DateTime resultOut = DateTime.MinValue;
                        if (DateTime.TryParseExact(model.PersonalDetails.HdnDateOfBirth, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out resultOut))
                        {
                            model.PersonalDetails.DateOfBirth = resultOut;
                        }
                    }
                    else
                    {
                        model.PersonalDetails.DateOfBirth = null;
                    }
                    if (string.Equals(applicantButton, "SAVECLOSE", StringComparison.OrdinalIgnoreCase))
                    {
                        //Do Validation Here
                        //If Validate totalConfirmValidation
                        applicantValidationResult = ApplicantIndividualValidationProcess.ValidateApplicantIndividual(model);
                        if (applicantValidationResult.Any(u => !u.IsValid))
                        {
                            totalConfirmValidation = false;
                            model.PersonalDetails.Status = "Pending";
                        }
                        else
                        {
                            totalConfirmValidation = true;
                            model.PersonalDetails.Status = "Complete";
                        }
                    }
                    else
                    {
                        model.PersonalDetails.Status = "Pending";
                    }
                    if (model.PersonalDetails.InvitedpersonforonlineIDverification != null)
                    {
                        model.PersonalDetails.HIDInviteFlag = Convert.ToInt16(model.PersonalDetails.InvitedpersonforonlineIDverification.RadioGroupValue);
                    }
                    if (model.EmploymentDetails != null && model.PersonalDetails.Id > 0)
                    {
                        model.EmploymentDetails = EmploymentDetailsProcess.SaveEmploymentDetailsModel(model.PersonalDetails.Id, model.EmploymentDetails);
                    }
                    bool personalSaveInRegistry = model.PersonalDetails.SaveInRegistry;
                    model.PersonalDetails = PersonalDetailsProcess.SaveApplicantPersonalDetailsModel(model.ApplicationNumber, model.PersonalDetails);
                    if (!string.IsNullOrEmpty(model.ContactDetails.ContactDetails_EmailAddress) && personalSaveInRegistry)
                    {
                        if (userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null)
                        {
                            int personDetilsRegitryId = 0;
                            IdentificationDetailsViewModel selectedIdentification = null;
                            var applicantIdentifications = IdentificationDetailsProcess.GetIdentificationDetails(model.PersonalDetails.Id);
                            var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);
                            var personRegistries = registriesRepository.GetRegistries(UserRegistry.NodeAliasPath);
                            if (personRegistries != null && personRegistries.Any(y => y.DateofBirth == model.PersonalDetails.DateOfBirth))
                            {
                                var selectedPersonRegistry = personRegistries.FirstOrDefault(y => y.DateofBirth == model.PersonalDetails.DateOfBirth);
                                if (applicantIdentifications.Any(p => string.Equals(p.IdentificationDetails_IdentificationNumber, selectedPersonRegistry.IdentificationNumber, StringComparison.OrdinalIgnoreCase) && string.Equals(p.IdentificationDetails_CountryOfIssue.Value.ToString(), selectedPersonRegistry.IssuingCountry, StringComparison.OrdinalIgnoreCase) && string.Equals(p.IdentificationDetails_TypeOfIdentification, selectedPersonRegistry.TypeofIdentification, StringComparison.OrdinalIgnoreCase)))
                                {
                                    selectedIdentification = applicantIdentifications.FirstOrDefault(p => string.Equals(p.IdentificationDetails_IdentificationNumber, selectedPersonRegistry.IdentificationNumber, StringComparison.OrdinalIgnoreCase) && string.Equals(p.IdentificationDetails_CountryOfIssue.Value.ToString(), selectedPersonRegistry.IssuingCountry, StringComparison.OrdinalIgnoreCase) && string.Equals(p.IdentificationDetails_TypeOfIdentification, selectedPersonRegistry.TypeofIdentification, StringComparison.OrdinalIgnoreCase));
                                    personDetilsRegitryId = selectedPersonRegistry.PersonsRegistryID;
                                }
                            }
                            if (personDetilsRegitryId == 0 && applicantIdentifications.Any(k => string.Equals(k.IdentificationDetails_TypeOfIdentificationName, "ID", StringComparison.OrdinalIgnoreCase) || string.Equals(k.IdentificationDetails_TypeOfIdentificationName, "PASSPORT", StringComparison.OrdinalIgnoreCase)))
                            {
                                selectedIdentification = applicantIdentifications.FirstOrDefault(k => string.Equals(k.IdentificationDetails_TypeOfIdentificationName, "ID", StringComparison.OrdinalIgnoreCase) || string.Equals(k.IdentificationDetails_TypeOfIdentificationName, "PASSPORT", StringComparison.OrdinalIgnoreCase));

                            }
                            //model.PersonalDetails.PersonRegistryId = personDetilsRegitryId;

                            if (selectedIdentification != null)
                            {
                                var personRegistry = PersonalDetailsProcess.SavePersonRegistry(User.Identity.Name, userModel.IntroducerUser.Introducer.IntermediaryName, registriesRepository, model.PersonalDetails, model.ContactDetails, selectedIdentification);
                                if (personRegistry != null)
                                {
                                    model.PersonalDetails.PersonRegistryId = personRegistry.PersonsRegistryID;
                                    model.PersonalDetails = PersonalDetailsProcess.SaveApplicantPersonalDetailsModel(model.ApplicationNumber, model.PersonalDetails);
                                }
                            }
                        }
                    }
                    else if (model.PersonalDetails != null && personalSaveInRegistry)
                    {
                        if (userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null)
                        {
                            IdentificationDetailsViewModel selectedIdentification = null;
                            var personRegistry = PersonalDetailsProcess.SavePersonRegistry(User.Identity.Name, userModel.IntroducerUser.Introducer.IntermediaryName, registriesRepository, model.PersonalDetails, model.ContactDetails, selectedIdentification);
                            if (personRegistry != null)
                            {
                                model.PersonalDetails.PersonRegistryId = personRegistry.PersonsRegistryID;
                                model.PersonalDetails = PersonalDetailsProcess.SaveApplicantPersonalDetailsModel(model.ApplicationNumber, model.PersonalDetails);
                            }
                        }
                    }

                    else if (model.PersonalDetails != null && model.PersonalDetails.PersonRegistryId > 0)
                    {
                        model.PersonalDetails.PersonRegistryId = 0;
                        model.PersonalDetails = PersonalDetailsProcess.SaveApplicantPersonalDetailsModel(model.ApplicationNumber, model.PersonalDetails);
                    }
                    PersonID = model.PersonalDetails.Id;
                    applicantNodeGUID = model.PersonalDetails.NodeGUID;
                    //}
                    //Contcat Details SAVE and UPDATE for Individual
                    if (model.PersonalDetails != null && model.PersonalDetails.Id > 0)
                    {
                        var applicatioData = personalDetailsRepository.GetPersonalDetailsByID(model.PersonalDetails.Id);
                        string Path = applicatioData.NodeAliasPath;
                        var contactDetailsTreeNode = contactDetailsRepository.GetContactDetails(Path);
                        if (model.ContactDetails != null && model.ContactDetails.ContactDetailsID > 0)
                        {
                            model.ContactDetails = ContactDetailsProcess.UpdateContactDetails(model.ContactDetails, contactDetailsTreeNode.FirstOrDefault());
                        }
                        else
                        {
                            model.ContactDetails = ContactDetailsProcess.SaveContactDetails(model.ContactDetails, applicatioData);
                        }
                    }
                    //Save Identification Details if retrive from registry on first time creation of applicant

                    if (model.ContactDetails.IsRetriveFromRegistry == true)
                    {
                        TreeNode treeNode = ServiceHelper.GetTreeNode(model.PersonalDetails.NodeGUID, model.PersonalDetails.NodePath);
                        if (treeNode != null && !string.IsNullOrEmpty(model.hdnTypeofIdentification))
                        {
                            IdentificationDetailsViewModel identificationDetailsViewModel = new IdentificationDetailsViewModel();
                            identificationDetailsViewModel.CitizenshipValue = model.hdnCitizenship;
                            identificationDetailsViewModel.IdentificationDetails_TypeOfIdentification = model.hdnTypeofIdentification;
                            identificationDetailsViewModel.IdentificationDetails_IdentificationNumber = model.hdnIdentificationNumber;
                            identificationDetailsViewModel.CountryOfIssueValue = model.hdnIssuingCountry;
                            //DateTime.ParseExact(model.hdnIssueDateTime.Substring(0, model.hdnIssueDateTime.IndexOf('(')).TrimEnd(), "ddd MMM dd yyyy HH:mm:ss 'GMT'K", System.Globalization.CultureInfo.InvariantCulture)
                            identificationDetailsViewModel.IdentificationDetails_IssueDate = DateTime.ParseExact(model.hdnIssueDateTime.Substring(0, model.hdnIssueDateTime.IndexOf('(')).TrimEnd(), "ddd MMM dd yyyy HH:mm:ss 'GMT'K", System.Globalization.CultureInfo.InvariantCulture); //Convert.ToDateTime(model.hdnIssueDateTime);
                            identificationDetailsViewModel.IdentificationDetails_ExpiryDate = DateTime.ParseExact(model.hdnExpiryDateTime.Substring(0, model.hdnExpiryDateTime.IndexOf('(')).TrimEnd(), "ddd MMM dd yyyy HH:mm:ss 'GMT'K", System.Globalization.CultureInfo.InvariantCulture); // Convert.ToDateTime(model.hdnExpiryDateTime);
                            identificationDetailsViewModel.PnodeGUID = model.PersonalDetails.NodeGUID;
                            identificationDetailsViewModel.NodeAliaspath = model.PersonalDetails.NodePath;
                            identificationDetailsViewModel.Status = false;
                            var successData = IdentificationDetailsProcess.SaveIdendificationsModel(identificationDetailsViewModel, treeNode);
                            identificationDetailsViewModel = successData;
                        }
                    }
                }
                if (model.CompanyDetails != null && isLegalEntity)
                {
                    var applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);
                    model.CompanyDetails.Type = applicationDetails.ApplicationDetails_ApplicationType;
                    if (!string.IsNullOrEmpty(model.CompanyDetails.HdnDateofIncorporation))
                    {
                        DateTime resultOut = DateTime.MinValue;
                        if (DateTime.TryParseExact(model.CompanyDetails.HdnDateofIncorporation, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out resultOut))
                        {
                            model.CompanyDetails.DateofIncorporation = resultOut;
                        }
                    }
                    else
                    {
                        model.CompanyDetails.DateofIncorporation = null;
                    }

                    var purposeAndActivity = PurposeAndActivityProcess.GetPurposeAndActivityModel(applicationDetails.ApplicationDetails_ApplicationNumber);
                    if (purposeAndActivity != null && purposeAndActivity.ReasonForOpeningTheAccountGroup != null && purposeAndActivity.ReasonForOpeningTheAccountGroup.MultiSelectValue != null && purposeAndActivity.ReasonForOpeningTheAccountGroup.MultiSelectValue.Length > 0)
                    {
                        bool isNeedToRemoveResonForOpening = true;
                        if (model.CompanyDetails.Id > 0)
                        {
                            var prevCompanyDetails = CompanyDetailsProcess.GetCompanyDetailsById(model.CompanyDetails.Id);
                            if (prevCompanyDetails != null && string.Equals(prevCompanyDetails.CompanyDetails_EntityType, model.CompanyDetails.EntityType, StringComparison.OrdinalIgnoreCase))
                            {
                                isNeedToRemoveResonForOpening = false;
                            }
                        }
                        if (isNeedToRemoveResonForOpening)
                        {
                            purposeAndActivity.ReasonForOpeningTheAccountGroup.MultiSelectValue = null;
                            PurposeAndActivityProcess.SavePurposeAndActivityModel(applicationDetails.ApplicationDetails_ApplicationNumber, purposeAndActivity);
                        }
                    }

                    if (string.Equals(applicantButton, "SAVECLOSE", StringComparison.OrdinalIgnoreCase))
                    {
                        //Do Validation Here
                        //If Validate 

                        applicantValidationResult = ApplicantLegalValidationProcess.ValidateApplicantLegal(model);
                        if (applicantValidationResult.Any(u => !u.IsValid))
                        {
                            totalConfirmValidation = false;
                            model.CompanyDetails.Status = "Pending";
                        }
                        else
                        {
                            totalConfirmValidation = true;
                            model.CompanyDetails.Status = "Complete";
                        }



                    }
                    else
                    {
                        model.CompanyDetails.Status = "Pending";
                    }
                    bool SaveInRegistry = model.CompanyDetails.SaveInRegistry;
                    model.CompanyDetails = CompanyDetailsProcess.SaveApplicantCompanyDetailsModel(model.ApplicationNumber, model.CompanyDetails);
                    if (SaveInRegistry)//model.ContactDetailsLegal != null &&
                    {
                        int companyRegitryId = 0;
                        var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);
                        if (UserRegistry != null)
                        {
                            var companyRegistries = registriesRepository.GetCompanyRegistries(UserRegistry.NodeAliasPath);
                            if (companyRegistries != null && companyRegistries.Any(h => string.Equals(h.CompanyDetails_RegistrationNumber, model.CompanyDetails.RegistrationNumber) && string.Equals(h.CompanyDetails_CountryofIncorporation, model.CompanyDetails.CountryofIncorporation) && h.CompanyDetails_DateofIncorporation == model.CompanyDetails.DateofIncorporation))
                            {
                                companyRegitryId = companyRegistries.FirstOrDefault(h => string.Equals(h.CompanyDetails_RegistrationNumber, model.CompanyDetails.RegistrationNumber) && string.Equals(h.CompanyDetails_CountryofIncorporation, model.CompanyDetails.CountryofIncorporation) && h.CompanyDetails_DateofIncorporation == model.CompanyDetails.DateofIncorporation).CompanyRegistryID;

                            }
                        }
                        model.CompanyDetails.Application_Type = model.Application_Type;
                        model.CompanyDetails.RegistryId = companyRegitryId;
                        var companyRegistry = CompanyDetailsProcess.SaveCompanyRegistry(User.Identity.Name, registriesRepository, model.CompanyDetails, model.ContactDetailsLegal);
                        if (companyRegistry != null)
                        {
                            model.CompanyDetails.RegistryId = companyRegistry.CompanyRegistryID;
                            model.CompanyDetails = CompanyDetailsProcess.SaveApplicantCompanyDetailsModel(model.ApplicationNumber, model.CompanyDetails);
                        }

                    }
                    else if (model.CompanyDetails.RegistryId > 0)
                    {
                        model.CompanyDetails.RegistryId = 0;
                        model.CompanyDetails = CompanyDetailsProcess.SaveApplicantCompanyDetailsModel(model.ApplicationNumber, model.CompanyDetails);
                    }
                    PersonID = model.CompanyDetails.Id;
                    applicantNodeGUID = model.CompanyDetails.NodeGUID;
                    if (PersonID > 0)
                    {
                        if (model.CompanyBusinessProfile != null && model.CompanyDetails.Id > 0)
                        {
                            //if (!string.IsNullOrEmpty(model.CompanyBusinessProfile.MainBusinessActivities))
                            //{
                            model.CompanyBusinessProfile = CompanyBusinessProfileProcess.SaveCompanyBusinessProfileModel(model.CompanyDetails.Id, model.CompanyBusinessProfile);
                            // }
                            model.CompanyFinancialInformation = CompanyFinancialInformationProcess.SaveCompanyFinancialInformationModel(model.CompanyDetails.Id, model.CompanyFinancialInformation);
                            //if (model.CompanyFinancialInformation.Turnover > 0)
                            //{
                            //    model.CompanyFinancialInformation = CompanyFinancialInformationProcess.SaveCompanyFinancialInformationModel(model.CompanyDetails.Id, model.CompanyFinancialInformation);
                            //}

                        }
                    }
                    if (model.FATCACRSDetails != null)
                    {
                        var applicationData = companyDetailsRepository.GetCompanyDetailsByID(model.CompanyDetails.Id);
                        string path = applicationData.NodeAliasPath;
                        var FATCADetailsTreeNode = fATCACRSDetailsRepository.GetFATCACRSDetails(path);
                        if (model.FATCACRSDetails.FATCADetailsID > 0)
                        {
                            model.FATCACRSDetails = FATCACRSDetailsProcess.UpdateFATCACRSDetails(model.FATCACRSDetails, FATCADetailsTreeNode.FirstOrDefault());
                        }
                        else
                        {
                            model.FATCACRSDetails = FATCACRSDetailsProcess.SaveFATCACRSDetails(model.FATCACRSDetails, applicationData);
                        }
                    }
                    if (model.CRSDetails != null)
                    {
                        var applicationData = companyDetailsRepository.GetCompanyDetailsByID(model.CompanyDetails.Id);
                        string path = applicationData.NodeAliasPath;
                        var CRSDetailsTreeNode = cRSDetailsRepository.GetCRSDetails(path);
                        if (model.CRSDetails.CompanyCRSDetailsID > 0)
                        {
                            model.CRSDetails = FATCACRSDetailsProcess.UpdateCRSDetails(model.CRSDetails, CRSDetailsTreeNode.FirstOrDefault());
                        }
                        else
                        {
                            model.CRSDetails = FATCACRSDetailsProcess.SaveCRSDetails(model.CRSDetails, applicationData);
                        }
                    }
                    //Contcat Details SAVE and UPDATE for Legal
                    if (model.ContactDetailsLegal != null)
                    {
                        var applicationData = companyDetailsRepository.GetCompanyDetailsByID(model.CompanyDetails.Id);
                        string path = applicationData.NodeAliasPath;
                        var contactDetailsTreeNode = contactDetailsLegalRepository.GetContactDetailsLegal(path);
                        if (model.ContactDetailsLegal.ContactDetailsLegalID > 0)
                        {
                            model.ContactDetailsLegal = ContactDetailsLegalProcess.UpdateContactDetailsLegal(model.ContactDetailsLegal, contactDetailsTreeNode.FirstOrDefault());
                        }
                        else
                        {
                            model.ContactDetailsLegal = ContactDetailsLegalProcess.SaveContactDetailsLegal(model.ContactDetailsLegal, applicationData);
                        }
                    }
                }

                ApplicationSyncService.SyncApplicationSearchRecord(model.ApplicationNumber); //Sync application recored for search
                if (PersonID > 0)
                {
                    if (string.Equals(applicantButton, "SAVECLOSE", StringComparison.OrdinalIgnoreCase))
                    {
                        //Document generate

                        string personNodeGUID = string.Empty;
                        if (isLegalEntity)
                        {
                            personNodeGUID = model.CompanyDetails.NodeGUID;
                        }
                        else
                        {
                            personNodeGUID = model.PersonalDetails.NodeGUID;
                        }

                        if (applicantValidationResult != null && applicantValidationResult.Any(u => !u.IsValid))
                        {
                            TempData["ErrorSummary"] = JsonConvert.SerializeObject(applicantValidationResult);
                            return RedirectToAction("Index", "Applicant", new { application = model.Application_NodeGUID, applicant = applicantNodeGUID });
                        }
                        else
                        {
                            //Generate Bank Document
                            DocumentsProcess.GenerateBankDocuments(model.ApplicationId, bankDocumentsRepository, applicationsRepository, model.ApplicationNumber, model.Application_Type, personNodeGUID);
                            //Generate Expected Document
                            DocumentsProcess.GenerateExpectedDouments(model.ApplicationId, expectedDocumentsRepository, applicationsRepository, model.ApplicationNumber, model.Application_Type, personNodeGUID);
                            if (!isLegalEntity)
                            {
                                EBankingSubscriberDetailsProcess.SaveAutoEBankingSubscriberIndividual(model.ApplicationNumber, model.PersonalDetails.Id, true, false);
                                DebitCardeDetailsProcess.GenerateDebitCardDetails(true, model.ApplicationId, debitCardDetailsRepository, applicationsRepository, "", "", ServiceHelper.GetCardType().FirstOrDefault().Value, model.PersonalDetails.NodeGUID, model.PersonalDetails.FirstName + " " + model.PersonalDetails.LastName, false);
                            }
                            return RedirectToAction("Edit", "Applications", new { application = model.Application_NodeGUID });
                        }
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Applicant saved successfuly";
                        return RedirectToAction("Index", "Applicant", new { application = model.Application_NodeGUID, applicant = applicantNodeGUID });
                    }

                }
                else
                {
                    return RedirectToAction("Edit", "Applications", new { application = model.Application_NodeGUID });
                }
                //xyz123
            }
            return View(model);
        }
        #endregion
        #region Tax Details
        public IActionResult TaxDetails_Read([DataSourceRequest] DataSourceRequest request, int id, string applicationType)
        {
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);

            int applicantID = ValidationHelper.GetInteger(id, 0);
            List<TaxDetailsViewModel> taxDetailsViewModels = new List<TaxDetailsViewModel>();
            if (isLegalEntity)
            {
                var taxDetails = taxDetailsRepository.GetTaxDetailsLegal(applicantID);
                if (taxDetails != null)
                {
                    taxDetailsViewModels = TaxDetailsProcess.GetTaxDetails(taxDetails);
                }
            }
            else
            {
                var taxDetails = taxDetailsRepository.GetTaxDetails(applicantID);
                if (taxDetails != null)
                {
                    taxDetailsViewModels = TaxDetailsProcess.GetTaxDetails(taxDetails);
                }
            }

            return Json(taxDetailsViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult TaxDetailsPopup_Create([DataSourceRequest] DataSourceRequest request, TaxDetailsViewModel taxDetailsViewModel, int id, string applicationType)
        {
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            if (taxDetailsViewModel.Status == true)
            {
                //if (string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TaxIdentificationNumber))
                //{
                //    ModelState.AddModelError("TaxDetails_TaxIdentificationNumber", ResHelper.GetString(TaxDetailsViewModelErrorMessage.TaxDetails_TaxIdentificationNumberError));
                //}
                //if (string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TinUnavailableReason))
                //{
                //    ModelState.AddModelError("TaxDetails_TinUnavailableReason", ResHelper.GetString(TaxDetailsViewModelErrorMessage.TaxDetails_TinUnavailableReasonError));
                //}
                //if (string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_JustificationForTinUnavalability))
                //{
                //    ModelState.AddModelError("TaxDetails_JustificationForTinUnavalability", ResHelper.GetString(TaxDetailsViewModelErrorMessage.TaxDetails_JustificationForTinUnavalabilityError));
                //}
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicantIndividualFormBasicValidationProcess.ValidateTaxDetails(taxDetailsViewModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            int applicatID = ValidationHelper.GetInteger(id, 0);
            if (taxDetailsViewModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(taxDetailsViewModel))
            {
                if (isLegalEntity)
                {
                    var applicatioData = taxDetailsRepository.GetCompanyDetailsLegalByID(applicatID);
                    taxDetailsViewModel = TaxDetailsProcess.SaveTaxDetailsModel(taxDetailsViewModel, applicatioData);
                }
                else
                {
                    var applicatioData = taxDetailsRepository.GetPersonalDetailsByID(applicatID);
                    taxDetailsViewModel = TaxDetailsProcess.SaveTaxDetailsModel(taxDetailsViewModel, applicatioData);
                }
            }


            return Json(new[] { taxDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult TaxDetailsPopup_Update([DataSourceRequest] DataSourceRequest request, TaxDetailsViewModel taxDetailsViewModel, int id)
        {
            if (taxDetailsViewModel.Status == true)
            {
                //if (string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TaxIdentificationNumber))
                //{
                //    ModelState.AddModelError("IdentificationDetails_Citizenship", ResHelper.GetString(TaxDetailsViewModelErrorMessage.TaxDetails_TaxIdentificationNumberError));
                //}
                //if (string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_TinUnavailableReason))
                //{
                //    ModelState.AddModelError("TaxDetails_TinUnavailableReason", ResHelper.GetString(TaxDetailsViewModelErrorMessage.TaxDetails_TinUnavailableReasonError));
                //}
                //if (string.IsNullOrEmpty(taxDetailsViewModel.TaxDetails_JustificationForTinUnavalability))
                //{
                //    ModelState.AddModelError("TaxDetails_JustificationForTinUnavalability", ResHelper.GetString(TaxDetailsViewModelErrorMessage.TaxDetails_JustificationForTinUnavalabilityError));
                //}
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicantIndividualFormBasicValidationProcess.ValidateTaxDetails(taxDetailsViewModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (taxDetailsViewModel != null && ModelState.IsValid)
            {
                var taxDetails = taxDetailsRepository.GetTaxDetailsByID(taxDetailsViewModel.TaxDetailsID);
                if (taxDetails != null)
                {
                    taxDetailsViewModel = TaxDetailsProcess.UpdateTaxDetailsModel(taxDetailsViewModel, taxDetails);
                }
            }

            return Json(new[] { taxDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult TaxDetailsPopup_Destroy([DataSourceRequest] DataSourceRequest request, TaxDetailsViewModel taxDetailsViewModel)
        {
            if (taxDetailsViewModel != null)
            {
                taxDetailsRepository.GetTaxDetailsByID(taxDetailsViewModel.TaxDetailsID).DeleteAllCultures();
            }
            return Json(new[] { taxDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }
        public JsonResult JustificationForTinUnavalability_Read()
        {
            var result = ServiceHelper.GetCommonDropDown(Constants.TIN_UNAVAILABLE_REASON);
            return Json(result);
        }
        public JsonResult CountryOfTaxResidency_Read()
        {
            var result = ServiceHelper.GetCountriesWithID();
            return Json(result);
        }

        public JsonResult ValidateTaxDetails(int taxDetails_CountryOfTaxResidency, string taxDetails_TaxIdentificationNumber, string taxDetails_TinUnavailableReason, string taxDetails_JustificationForTinUnavalability)
        {
            bool isValid = true;
            TaxDetailsViewModel taxDetailsViewModel = new TaxDetailsViewModel();
            taxDetailsViewModel.TaxDetails_CountryOfTaxResidency = Convert.ToString(taxDetails_CountryOfTaxResidency);
            taxDetailsViewModel.TaxDetails_TaxIdentificationNumber = taxDetails_TaxIdentificationNumber;
            taxDetailsViewModel.TaxDetails_TinUnavailableReason = taxDetails_TinUnavailableReason;
            taxDetailsViewModel.TaxDetails_JustificationForTinUnavalability = taxDetails_JustificationForTinUnavalability;

            ValidationResultModel validationResultModel = new ValidationResultModel();
            validationResultModel = ApplicantIndividualFormBasicValidationProcess.ValidateTaxDetails(taxDetailsViewModel);
            if (validationResultModel != null)
            {
                foreach (var item in validationResultModel.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            if (!ModelState.IsValid)
            {
                isValid = false;
            }
            return Json(isValid);
        }
        #endregion
        #region PEP Apllicant Details

        public IActionResult PEPApplicant_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            int applicantID = ValidationHelper.GetInteger(id, 0);
            List<PepApplicantViewModel> pepApplicantViewModels = new List<PepApplicantViewModel>();
            var pepApplicants = pepApplicantRepository.GetPepApplicants(applicantID);
            if (pepApplicants != null)
            {
                pepApplicantViewModels = PEPDetailsProcess.GetPepApplicants(pepApplicants);
            }
            return Json(pepApplicantViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult PEPApplicantPopup_Create([DataSourceRequest] DataSourceRequest request, PepApplicantViewModel pepApplicantViewModel, int id)
        {
            if (pepApplicantViewModel.Status)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicantIndividualFormBasicValidationProcess.ValidatePepDetailsApplicant(pepApplicantViewModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            int applicatID = ValidationHelper.GetInteger(id, 0);
            if (pepApplicantViewModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(pepApplicantViewModel))
            {
                var applicatioData = taxDetailsRepository.GetPersonalDetailsByID(applicatID);
                pepApplicantViewModel = PEPDetailsProcess.SavePepApplicantModel(pepApplicantViewModel, applicatioData);

            }
            return Json(new[] { pepApplicantViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult PEPApplicantPopup_Update([DataSourceRequest] DataSourceRequest request, PepApplicantViewModel pepApplicantViewModel, int id)
        {
            if (pepApplicantViewModel.Status)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicantIndividualFormBasicValidationProcess.ValidatePepDetailsApplicant(pepApplicantViewModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (pepApplicantViewModel != null && ModelState.IsValid)
            {
                var pepApplicantDetails = pepApplicantRepository.GetPepApplicantByID(pepApplicantViewModel.PepApplicantID);
                if (pepApplicantDetails != null)
                {
                    var successData = PEPDetailsProcess.UpdatePepApplicantModel(pepApplicantViewModel, pepApplicantDetails);
                    pepApplicantViewModel.PepApplicant_CountryName = successData.PepApplicant_CountryName;
                    pepApplicantViewModel.PepApplicant_Since = successData.PepApplicant_Since;
                    pepApplicantViewModel.PepApplicant_Untill = successData.PepApplicant_Untill;
                    pepApplicantViewModel.StatusName = successData.StatusName;
                    pepApplicantViewModel.PepApplicant_FirstName = successData.PepApplicant_FirstName;
                    pepApplicantViewModel.PepApplicant_Surname = successData.PepApplicant_Surname;
                }
            }

            return Json(new[] { pepApplicantViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult PEPApplicantPopup_Destroy([DataSourceRequest] DataSourceRequest request, PepApplicantViewModel pepApplicantViewModel)
        {
            if (pepApplicantViewModel != null)
            {
                pepApplicantRepository.GetPepApplicantByID(pepApplicantViewModel.PepApplicantID).DeleteAllCultures();
            }
            return Json(new[] { pepApplicantViewModel }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult PEPApplicantPopup_DestroyAll([DataSourceRequest] DataSourceRequest request, int id)
        {
            PepApplicantViewModel pepApplicantViewModel = new PepApplicantViewModel();
            int applicantID = ValidationHelper.GetInteger(id, 0);
            var pepApplicant = pepApplicantRepository.GetPepApplicants(applicantID);
            if (pepApplicant != null)
            {
                foreach (var item in pepApplicant)
                {
                    item.DeleteAllCultures();
                }
            }
            return Json(new[] { pepApplicantViewModel }.ToDataSourceResult(request, ModelState));
        }
        public JsonResult IsApplicant_Read()
        {
            var result = ServiceHelper.GetBoolDropDownListDefaults();
            return Json(result);
        }
        public JsonResult PepApplicant_Country_Read()
        {
            var result = ServiceHelper.GetCountriesWithID();
            return Json(result);
        }

        [HttpPost]
        public ActionResult PEPDetail(ApplicantModel applicantModel)
        {
            if (applicantModel.PersonalDetails.Id > 0)
            {
                var pepApplicants = pepApplicantRepository.GetPersonalDetailsByID(applicantModel.PersonalDetails.Id);
                if (pepApplicants != null)
                {
                    pepApplicants.SetValue("PersonalDetails_IsPep", applicantModel.PersonalDetails.IsPepName);
                    pepApplicants.SetValue("PersonalDetails_IsRelatedToPep", applicantModel.PersonalDetails.IsRelatedToPepName);
                    pepApplicants.Update();
                }

            }
            return RedirectToAction("Index", "Applicant", new { applicationId = 0, applicantId = applicantModel.PersonalDetails.Id });
        }
        #endregion
        #region PEP Associates Details

        public IActionResult PEPAssociates_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            int applicantID = ValidationHelper.GetInteger(id, 0);
            List<PepAssociatesViewModel> pepAssociatesViewModels = new List<PepAssociatesViewModel>();
            var pepApplicants = pepAssociatesRepository.GetPepAssociates(applicantID);
            if (pepApplicants != null)
            {
                pepAssociatesViewModels = PEPDetailsProcess.GetPepAssociates(pepApplicants);
            }
            return Json(pepAssociatesViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult PEPAssociatesPopup_Create([DataSourceRequest] DataSourceRequest request, PepAssociatesViewModel pepAssociatesViewModel, int id)
        {

            int applicatID = ValidationHelper.GetInteger(id, 0);
            if (pepAssociatesViewModel.Status)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicantIndividualFormBasicValidationProcess.ValidatePepDetailsFamilyAssociate(pepAssociatesViewModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (pepAssociatesViewModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(pepAssociatesViewModel))
            {
                var applicatioData = taxDetailsRepository.GetPersonalDetailsByID(applicatID);
                pepAssociatesViewModel = PEPDetailsProcess.SavePepAssociatesModel(pepAssociatesViewModel, applicatioData);

            }
            else
            {
                pepAssociatesViewModel.Status = false;
            }
            return Json(new[] { pepAssociatesViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult PEPAssociatesPopup_Update([DataSourceRequest] DataSourceRequest request, PepAssociatesViewModel pepAssociatesViewModel)
        {
            if (pepAssociatesViewModel.Status)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicantIndividualFormBasicValidationProcess.ValidatePepDetailsFamilyAssociate(pepAssociatesViewModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (pepAssociatesViewModel != null && ModelState.IsValid)
            {
                var pepApplicantDetails = pepAssociatesRepository.GetPepAssociatesByID(pepAssociatesViewModel.PepAssociatesID);
                if (pepApplicantDetails != null)
                {
                    var successData = PEPDetailsProcess.UpdatePepAssociatesModel(pepAssociatesViewModel, pepApplicantDetails);
                    pepAssociatesViewModel.PepAssociates_CountryName = successData.PepAssociates_CountryName;
                    pepAssociatesViewModel.PepAssociates_RelationshipName = successData.PepAssociates_RelationshipName;
                    pepAssociatesViewModel.PepAssociates_Since = successData.PepAssociates_Since;
                    pepAssociatesViewModel.PepAssociates_Until = successData.PepAssociates_Until;
                    pepAssociatesViewModel.StatusName = successData.StatusName;
                }
            }
            else
            {
                pepAssociatesViewModel.Status = false;
            }
            return Json(new[] { pepAssociatesViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult PEPAssociatesPopup_Destroy([DataSourceRequest] DataSourceRequest request, PepAssociatesViewModel pepAssociatesViewModel)
        {
            if (pepAssociatesViewModel != null)
            {
                pepAssociatesRepository.GetPepAssociatesByID(pepAssociatesViewModel.PepAssociatesID).DeleteAllCultures();
            }
            return Json(new[] { pepAssociatesViewModel }.ToDataSourceResult(request, ModelState));
        }
        public JsonResult PepApplicant_Relationship_Read()
        {
            var result = ServiceHelper.GetCommonDropDown(Constants.RELATIONSHIPS);
            return Json(result);
        }
        public ActionResult PEPAssociatesPopup_DestroyAll([DataSourceRequest] DataSourceRequest request, int id)
        {
            PepAssociatesViewModel pepAssociatesViewModel = new PepAssociatesViewModel();
            int applicantID = ValidationHelper.GetInteger(id, 0);
            var pepAssociates = pepAssociatesRepository.GetPepAssociates(applicantID);
            if (pepAssociates != null)
            {
                foreach (var item in pepAssociates)
                {
                    item.DeleteAllCultures();
                }
            }
            return Json(new[] { pepAssociatesViewModel }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Address

        public IActionResult AddressDetails_Read([DataSourceRequest] DataSourceRequest request, int apID, string applicationType)
        {
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            int applicantId = ValidationHelper.GetInteger(apID, 0);
            //TempData.Keep("ApplicationID");
            //int applicationID = ValidationHelper.GetInteger(TempData["ApplicationID"], 0);
            List<AddressDetailsModel> addressDetailsViewModels = new List<AddressDetailsModel>();

            if (applicantId > 0)
            {
                if (isLegalEntity)
                {
                    var applicationDetails = CompanyDetailsProcess.GetCompanyDetailsModelById(applicantId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        addressDetailsViewModels = AddressDetailsProcess.GetApplicantAddressDetailsLegal(applicantId);
                    }
                }
                else
                {
                    var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicantId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        addressDetailsViewModels = AddressDetailsProcess.GetApplicantAddressDetails(applicantId);
                    }
                }

            }

            if (addressDetailsViewModels == null)
                return Json("");

            return Json(addressDetailsViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult AddressDetailsPopup_Create([DataSourceRequest] DataSourceRequest request, AddressDetailsModel addressDetailsModel, int apID, string applicationType)
        {
            AddressDetailsModel addressdetailsModelCopy = new AddressDetailsModel();

            UserModel userModel = UserProcess.GetUser(User.Identity.Name);
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            addressDetailsModel.Is_Legal = isLegalEntity;
            var addressTypes = ServiceHelper.GetAddressType();
            if (addressTypes != null && addressTypes.Count > 0 && !string.IsNullOrEmpty(addressDetailsModel.AddressType) && addressTypes.Any(d => d.Value == addressDetailsModel.AddressType && string.Equals(d.Text, "OFFICE IN CYPRUS", StringComparison.OrdinalIgnoreCase)))
            {
                var countries = ServiceHelper.GetCountriesWithID();
                var countryCodes = ServiceHelper.GetCountryCodePrefix();
                //string addressType = addressTypes.FirstOrDefault(d => d.Value == addressDetailsModel.AddressType).Text;
                if (countries.Any(t => string.Equals(t.Text, "Cyprus", StringComparison.OrdinalIgnoreCase)))
                {
                    addressDetailsModel.Country = countries.FirstOrDefault(t => string.Equals(t.Text, "Cyprus", StringComparison.OrdinalIgnoreCase)).Value;
                }
                if (countryCodes.Any(t => string.Equals(t.Text, "Cyprus +357", StringComparison.OrdinalIgnoreCase)))
                {
                    addressDetailsModel.CountryCode_FaxNo = countryCodes.FirstOrDefault(t => string.Equals(t.Text, "Cyprus +357", StringComparison.OrdinalIgnoreCase)).Value;
                    addressDetailsModel.CountryCode_PhoneNo = countryCodes.FirstOrDefault(t => string.Equals(t.Text, "Cyprus +357", StringComparison.OrdinalIgnoreCase)).Value;
                }

            }
            if (addressDetailsModel.Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicantIndividualFormBasicValidationProcess.ValidateAddresstDetails(addressDetailsModel, isLegalEntity, apID);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
                if (isLegalEntity)
                {
                    //if (addressDetailsModel.SaveInRegistry)
                    //{
                    //    if (string.IsNullOrEmpty(addressDetailsModel.LocationName))
                    //    {
                    //        ModelState.AddModelError("LocationName", "Please enter location.");
                    //    }

                    //}
                    //if (string.IsNullOrEmpty(addressDetailsModel.Email))
                    //{
                    //    ModelState.AddModelError("Email", "Please enter email.");
                    //}
                    //if (string.IsNullOrEmpty(addressDetailsModel.CountryCode_PhoneNo))
                    //{
                    //    ModelState.AddModelError("CountryCode_PhoneNo", "Please enter phone contry code.");
                    //}
                    //if (string.IsNullOrEmpty(addressDetailsModel.PhoneNo))
                    //{
                    //    ModelState.AddModelError("PhoneNo", "Please enter phone number.");
                    //}
                    //if(string.IsNullOrEmpty(addressDetailsModel.CountryCode_FaxNo))
                    //{
                    //    ModelState.AddModelError("CountryCode_FaxNo", "Please enter fax contry code.");
                    //}
                    //if(string.IsNullOrEmpty(addressDetailsModel.FaxNo))
                    //{
                    //    ModelState.AddModelError("FaxNo", "Please enter fax number.");
                    //}
                }

            }
            int applicantId = ValidationHelper.GetInteger(apID, 0);
            if (addressDetailsModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(addressDetailsModel))
            {
                string Location = addressDetailsModel.LocationName;
                addressdetailsModelCopy = addressDetailsModel;
                if (isLegalEntity)
                {
                    var applicationDetails = CompanyDetailsProcess.GetCompanyDetailsModelById(applicantId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {

                        var addressDetails = AddressDetailsProcess.SaveApplicantAddressDetailsLegalModel(applicationDetails.Id, addressDetailsModel);
                        if (addressDetails != null)
                            addressDetailsModel = addressDetails;
                        if (addressDetails.SaveInRegistry)
                        {
                            if (userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null && !string.IsNullOrEmpty(Location))
                            {
                                addressDetails.LocationName = Location;
                                int addressRegistryId = 0;
                                IEnumerable<CMS.DocumentEngine.Types.Eurobank.AddressRegistry> existedAddressRegistry = null;
                                var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);
                                if (UserRegistry != null)
                                {
                                    existedAddressRegistry = registriesRepository.GetAddressRegistries(UserRegistry.NodeAliasPath);
                                    if (existedAddressRegistry != null && existedAddressRegistry.Any(r => string.Equals(r.LocationName, Location, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        addressRegistryId = existedAddressRegistry.FirstOrDefault(r => string.Equals(r.LocationName, Location, StringComparison.OrdinalIgnoreCase)).AddressRegistryID;

                                    }
                                }
                                addressDetails.AddressRegistryId = addressRegistryId;

                                var addressRegistry = AddressDetailsProcess.SaveAddressRegistry(User.Identity.Name, userModel.IntroducerUser.Introducer.IntermediaryName, registriesRepository, addressDetails);
                                if (addressRegistry != null)
                                {
                                    addressDetailsModel.AddressRegistryId = addressRegistry.AddressRegistryID;
                                    addressDetailsModel = AddressDetailsProcess.SaveApplicantAddressDetailsLegalModel(applicationDetails.Id, addressDetailsModel);
                                }
                            }
                        }
                        if (addressdetailsModelCopy.MailingAddressSame)
                        {
                            AddressDetailsProcess.UpdateApplicantLegalMainCorrespondingAddress(apID, addressDetailsModel.Id);
                            addressdetailsModelCopy.AddressType = ServiceHelper.GetGuidByName("MAILING ADDRESS", Constants.Address_Type);
                            AddressDetailsProcess.SaveApplicantAddressDetailsLegalModel(applicationDetails.Id, addressdetailsModelCopy);
                        }
                    }
                }
                else
                {
                    var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicantId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        var addressDetails = AddressDetailsProcess.SaveApplicantAddressDetailsModel(applicationDetails.Id, addressDetailsModel);
                        if (addressDetails != null)
                        {
                            addressDetailsModel = addressDetails;
                            if (addressDetails.SaveInRegistry)
                            {
                                if (userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null && !string.IsNullOrEmpty(Location))
                                {
                                    addressDetails.LocationName = Location;
                                    int addressRegistryId = 0;
                                    IEnumerable<CMS.DocumentEngine.Types.Eurobank.AddressRegistry> existedAddressRegistry = null;
                                    var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);
                                    if (UserRegistry != null)
                                    {
                                        existedAddressRegistry = registriesRepository.GetAddressRegistries(UserRegistry.NodeAliasPath);
                                        if (existedAddressRegistry != null && existedAddressRegistry.Any(r => string.Equals(r.LocationName, Location, StringComparison.OrdinalIgnoreCase)))
                                        {
                                            addressRegistryId = existedAddressRegistry.FirstOrDefault(r => string.Equals(r.LocationName, Location, StringComparison.OrdinalIgnoreCase)).AddressRegistryID;

                                        }
                                    }
                                    addressDetails.AddressRegistryId = addressRegistryId;
                                    var addressRegistry = AddressDetailsProcess.SaveAddressRegistry(User.Identity.Name, userModel.IntroducerUser.Introducer.IntermediaryName, registriesRepository, addressDetails);
                                    if (addressRegistry != null)
                                    {
                                        addressDetailsModel.AddressRegistryId = addressRegistry.AddressRegistryID;
                                        addressDetailsModel = AddressDetailsProcess.SaveApplicantAddressDetailsModel(applicationDetails.Id, addressDetailsModel);
                                    }
                                }
                            }
                            if (addressdetailsModelCopy.MailingAddressSame)
                            {
                                AddressDetailsProcess.UpdateApplicantIndividualMainCorrespondingAddress(apID, addressDetailsModel.Id);
                                addressdetailsModelCopy.AddressType = ServiceHelper.GetGuidByName("MAILING ADDRESS", Constants.Address_Type_Physical);
                                AddressDetailsProcess.SaveApplicantAddressDetailsModel(applicationDetails.Id, addressdetailsModelCopy);
                            }

                        }
                    }
                }

            }
            //        if(addressdetailsModelCopy.MailingAddressSame)
            //        {
            //            List<AddressDetailsModel> addresses = new List<AddressDetailsModel>();
            //            addresses.Add(addressDetailsModel);
            //            addresses.Add(addressdetailsModelCopy);
            //return new JsonResult(new[] { addresses }.ToDataSourceResult(request));
            //        }

            //return Json(new[] { addressDetailsModel }.ToDataSourceResult(request, ModelState), addressdetailsModelCopy);
            //return Json(addresses.ToDataSourceResult(request, ModelState));

            return Json(new[] { addressDetailsModel }.ToDataSourceResult(request, ModelState));

        }
        [HttpPost]
        public ActionResult AddressDetailsPopup_Update([DataSourceRequest] DataSourceRequest request, AddressDetailsModel addressDetailsModel, int apID, string applicationType)
        {
            AddressDetailsModel addressdetailsModelCopy = new AddressDetailsModel();
            UserModel userModel = UserProcess.GetUser(User.Identity.Name);
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            addressDetailsModel.Is_Legal = isLegalEntity;
            int applicantId = ValidationHelper.GetInteger(apID, 0);
            if (addressDetailsModel.Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicantIndividualFormBasicValidationProcess.ValidateAddresstDetails(addressDetailsModel, isLegalEntity, apID);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
                if (isLegalEntity)
                {
                    if (addressDetailsModel.SaveInRegistry)
                    {
                        if (string.IsNullOrEmpty(addressDetailsModel.LocationName))
                        {
                            ModelState.AddModelError("LocationName", "Please enter location.");
                        }

                    }
                }

            }
            if (addressDetailsModel != null && ModelState.IsValid)
            {
                string Location = addressDetailsModel.LocationName;
                var addressDetailsData = addressDetailsRepository.GetAddressDetails(addressDetailsModel.Id);
                if (addressDetailsData != null)
                {
                    addressdetailsModelCopy = addressDetailsModel;
                    if (isLegalEntity)
                    {
                        var addressDetails = AddressDetailsProcess.SaveApplicantAddressDetailsLegalModel(0, addressDetailsModel);
                        if (addressDetails != null)
                            addressDetailsModel = addressDetails;
                        if (addressDetails.SaveInRegistry)
                        {
                            if (userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null && !string.IsNullOrEmpty(Location))
                            {
                                addressDetails.LocationName = Location;
                                int addressRegistryId = 0;
                                IEnumerable<CMS.DocumentEngine.Types.Eurobank.AddressRegistry> existedAddressRegistry = null;
                                var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);
                                if (UserRegistry != null)
                                {
                                    existedAddressRegistry = registriesRepository.GetAddressRegistries(UserRegistry.NodeAliasPath);
                                    if (existedAddressRegistry != null && existedAddressRegistry.Any(r => string.Equals(r.LocationName, Location, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        addressRegistryId = existedAddressRegistry.FirstOrDefault(r => string.Equals(r.LocationName, Location, StringComparison.OrdinalIgnoreCase)).AddressRegistryID;

                                    }
                                }
                                addressDetails.AddressRegistryId = addressRegistryId;

                                var addressRegistry = AddressDetailsProcess.SaveAddressRegistry(User.Identity.Name, userModel.IntroducerUser.Introducer.IntermediaryName, registriesRepository, addressDetails);
                                if (addressRegistry != null)
                                {
                                    addressDetailsModel.AddressRegistryId = addressRegistry.AddressRegistryID;
                                    addressDetailsModel = AddressDetailsProcess.SaveApplicantAddressDetailsLegalModel(0, addressDetailsModel);
                                }
                            }
                        }
                        if (addressdetailsModelCopy.MailingAddressSame)
                        {
                            AddressDetailsProcess.UpdateApplicantLegalMainCorrespondingAddress(apID, addressDetailsModel.Id);

                            addressdetailsModelCopy.AddressType = ServiceHelper.GetGuidByName("MAILING ADDRESS", Constants.Address_Type);
                            addressdetailsModelCopy.Id = 0;
                            AddressDetailsProcess.SaveApplicantAddressDetailsLegalModel(apID, addressdetailsModelCopy);
                        }
                    }
                    else
                    {
                        var addressDetails = AddressDetailsProcess.SaveApplicantAddressDetailsModel(0, addressDetailsModel);
                        if (addressDetails != null)
                        {
                            addressDetailsModel = addressDetails;
                            if (addressDetails.SaveInRegistry)
                            {
                                if (userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null && !string.IsNullOrEmpty(Location))
                                {
                                    addressDetails.LocationName = Location;
                                    int addressRegistryId = 0;
                                    IEnumerable<CMS.DocumentEngine.Types.Eurobank.AddressRegistry> existedAddressRegistry = null;
                                    var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);
                                    if (UserRegistry != null)
                                    {
                                        existedAddressRegistry = registriesRepository.GetAddressRegistries(UserRegistry.NodeAliasPath);
                                        if (existedAddressRegistry != null && existedAddressRegistry.Any(r => string.Equals(r.LocationName, Location, StringComparison.OrdinalIgnoreCase)))
                                        {
                                            addressRegistryId = existedAddressRegistry.FirstOrDefault(r => string.Equals(r.LocationName, Location, StringComparison.OrdinalIgnoreCase)).AddressRegistryID;

                                        }
                                    }
                                    addressDetails.AddressRegistryId = addressRegistryId;
                                    var addressRegistry = AddressDetailsProcess.SaveAddressRegistry(User.Identity.Name, userModel.IntroducerUser.Introducer.IntermediaryName, registriesRepository, addressDetails);
                                    if (addressRegistry != null)
                                    {
                                        addressDetailsModel.AddressRegistryId = addressRegistry.AddressRegistryID;
                                        addressDetailsModel = AddressDetailsProcess.SaveApplicantAddressDetailsModel(0, addressDetailsModel);
                                    }
                                }
                            }
                            if (addressdetailsModelCopy.MailingAddressSame)
                            {
                                AddressDetailsProcess.UpdateApplicantIndividualMainCorrespondingAddress(apID, addressDetailsModel.Id);
                                addressdetailsModelCopy.AddressType = ServiceHelper.GetGuidByName("MAILING ADDRESS", Constants.Address_Type_Physical);
                                addressdetailsModelCopy.Id = 0;
                                AddressDetailsProcess.SaveApplicantAddressDetailsModel(apID, addressdetailsModelCopy);
                            }

                        }
                    }

                }
            }

            return Json(new[] { addressDetailsModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult AddressDetailsPopup_Destroy([DataSourceRequest] DataSourceRequest request, AddressDetailsModel addressDetailsModel)
        {
            if (addressDetailsModel != null)
            {
                addressDetailsRepository.GetAddressDetail(addressDetailsModel.Id).DeleteAllCultures();
            }

            return Json(new[] { addressDetailsModel }.ToDataSourceResult(request, ModelState));
        }
        public JsonResult AddRessTypeLegalRead(string entityType)
        {
            var result = ServiceHelper.GetAddressTypeLegal(entityType);
            return Json(result);
        }
        #endregion
        #region Source Of Annual Income

        public IActionResult SourceOfIncome_Read([DataSourceRequest] DataSourceRequest request, int apID)
        {
            int applicantId = ValidationHelper.GetInteger(apID, 0);
            List<SourceOfIncomeModel> sourceOfIncomeViewModels = new List<SourceOfIncomeModel>();

            if (applicantId > 0)
            {
                var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicantId);

                if (applicationDetails != null && applicationDetails.Id > 0)
                {
                    sourceOfIncomeViewModels = SourceOfIncomeProcess.GetSourceOfIncome(applicantId);
                }
            }

            if (sourceOfIncomeViewModels == null)
                return Json("");

            return Json(sourceOfIncomeViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult SourceOfIncomePopup_Create([DataSourceRequest] DataSourceRequest request, SourceOfIncomeModel sourceOfIncomeModel, int apID)
        {
            if (sourceOfIncomeModel.Status)
            {
                var existData = SourceOfIncomeProcess.GetSourceOfIncome(apID);//?.Where(x => x.StatusName.ToUpper() != "PENDING");
                if (existData != null)
                {
                    string selectedSourceOfAnnualIncome = string.Empty;
                    var sourceOfAnnualIncomeList = ServiceHelper.GetSourcesOfAnnualIncome();
                    if (sourceOfAnnualIncomeList != null && sourceOfAnnualIncomeList.Count > 0 && sourceOfAnnualIncomeList.Any(k => k.Value == sourceOfIncomeModel.SourceOfAnnualIncome))
                    {
                        selectedSourceOfAnnualIncome = sourceOfAnnualIncomeList.FirstOrDefault(k => k.Value == sourceOfIncomeModel.SourceOfAnnualIncome).Text;
                    }

                    bool ifExist = existData.Any(x => x.SourceOfAnnualIncome == sourceOfIncomeModel.SourceOfAnnualIncome && x.Id != sourceOfIncomeModel.Id);
                    if (ifExist && !string.Equals(selectedSourceOfAnnualIncome, "OTHER", StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError("SourceOfAnnualIncome", "Source of income already exists.");
                    }

                }

                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicantIndividualFormBasicValidationProcess.ValidateSourceOfIncome(sourceOfIncomeModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }

            int applicantId = ValidationHelper.GetInteger(apID, 0);
            if (sourceOfIncomeModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(sourceOfIncomeModel))
            {
                var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicantId);

                if (applicationDetails != null && applicationDetails.Id > 0)
                {
                    var sourceOfIncome = SourceOfIncomeProcess.SaveSourceOfIncomeModel(applicationDetails.Id, sourceOfIncomeModel);
                    if (sourceOfIncome != null)
                        sourceOfIncomeModel = sourceOfIncome;
                }
            }
            return Json(new[] { sourceOfIncomeModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult SourceOfIncomePopup_Update([DataSourceRequest] DataSourceRequest request, SourceOfIncomeModel sourceOfIncomeModel, int apID)
        {
            if (sourceOfIncomeModel.Status)
            {
                var existData = SourceOfIncomeProcess.GetSourceOfIncome(apID);//.Where(x => x.StatusName.ToUpper() != "PENDING");
                string selectedSourceOfAnnualIncome = string.Empty;
                var sourceOfAnnualIncomeList = ServiceHelper.GetSourcesOfAnnualIncome();
                if (sourceOfAnnualIncomeList != null && sourceOfAnnualIncomeList.Count > 0 && sourceOfAnnualIncomeList.Any(k => k.Value == sourceOfIncomeModel.SourceOfAnnualIncome))
                {
                    selectedSourceOfAnnualIncome = sourceOfAnnualIncomeList.FirstOrDefault(k => k.Value == sourceOfIncomeModel.SourceOfAnnualIncome).Text;
                }

                bool ifExist = existData.Any(x => x.SourceOfAnnualIncome == sourceOfIncomeModel.SourceOfAnnualIncome && x.Id != sourceOfIncomeModel.Id);
                if (ifExist && !string.Equals(selectedSourceOfAnnualIncome, "OTHER", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("SourceOfAnnualIncome", "Source of income already exists.");
                }
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicantIndividualFormBasicValidationProcess.ValidateSourceOfIncome(sourceOfIncomeModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (sourceOfIncomeModel != null && ModelState.IsValid)
            {
                var sourceOfIncomeData = sourceOfIncomeRepository.GetSourceOfIncome(sourceOfIncomeModel.Id);
                if (sourceOfIncomeData != null)
                {
                    var sourceOfIncome = SourceOfIncomeProcess.SaveSourceOfIncomeModel(0, sourceOfIncomeModel);
                    if (sourceOfIncome != null)
                        sourceOfIncomeModel = sourceOfIncome;
                }
            }

            return Json(new[] { sourceOfIncomeModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult SourceOfIncomePopup_Destroy([DataSourceRequest] DataSourceRequest request, SourceOfIncomeModel sourceOfIncomeModel)
        {
            if (sourceOfIncomeModel != null)
            {
                sourceOfIncomeRepository.GetSourceOfIncome(sourceOfIncomeModel.Id).DeleteAllCultures();
            }

            return Json(new[] { sourceOfIncomeModel }.ToDataSourceResult(request, ModelState));
        }

        #endregion
        #region Origin Of Total Assets

        public IActionResult OriginOfTotalAssets_Read([DataSourceRequest] DataSourceRequest request, int apID, string applicationType)
        {
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            int applicantId = ValidationHelper.GetInteger(apID, 0);
            List<OriginOfTotalAssetsModel> originOfTotalAssetsViewModels = new List<OriginOfTotalAssetsModel>();

            if (applicantId > 0)
            {
                if (isLegalEntity)
                {
                    var applicationDetails = CompanyDetailsProcess.GetCompanyDetailsModelById(applicantId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        originOfTotalAssetsViewModels = OriginOfTotalAssetsProcess.GetOriginOfTotalAssetsLegal(applicantId);
                    }
                }
                else
                {
                    var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicantId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        originOfTotalAssetsViewModels = OriginOfTotalAssetsProcess.GetOriginOfTotalAssets(applicantId);
                    }
                }

            }

            if (originOfTotalAssetsViewModels == null)
                return Json("");

            return Json(originOfTotalAssetsViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult OriginOfTotalAssetsPopup_Create([DataSourceRequest] DataSourceRequest request, OriginOfTotalAssetsModel originOfTotalAssetsModel, int apID, string applicationType)
        {
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            if (originOfTotalAssetsModel.Status)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicantIndividualFormBasicValidationProcess.ValidateOriginOfTotalAssets(originOfTotalAssetsModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
                //if (string.IsNullOrEmpty(originOfTotalAssetsModel.OriginOfTotalAssets))
                //{
                //    ModelState.AddModelError("OriginOfTotalAssets", ResHelper.GetString(OriginOfTotalAssetsErrorMassage.OriginOfTotalAssets));
                //}
                //if (originOfTotalAssetsModel.AmountOfTotalAsset == 0)
                //{
                //    ModelState.AddModelError("AmountOfTotalAsset", ResHelper.GetString(OriginOfTotalAssetsErrorMassage.AmountOfTotalAsset));

                //}
                //if(sourceOfIncomeModel.PepApplicant_Untill == null)
                //{
                //	ModelState.AddModelError("PepApplicant_Untill", ResHelper.GetString(SourceOfIncomeModelErrorMassage.PepApplicant_UntillError));

                //}
            }

            int applicantId = ValidationHelper.GetInteger(apID, 0);
            if (originOfTotalAssetsModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(originOfTotalAssetsModel))
            {
                if (isLegalEntity)
                {
                    var applicationDetails = CompanyDetailsProcess.GetCompanyDetailsModelById(applicantId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        var sourceOfIncome = OriginOfTotalAssetsProcess.SaveOriginOfTotalAssetsModelLegal(applicationDetails.Id, originOfTotalAssetsModel);
                        if (sourceOfIncome != null)
                            originOfTotalAssetsModel = sourceOfIncome;
                    }
                }
                else
                {
                    var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicantId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        var sourceOfIncome = OriginOfTotalAssetsProcess.SaveOriginOfTotalAssetsModel(applicationDetails.Id, originOfTotalAssetsModel);
                        if (sourceOfIncome != null)
                            originOfTotalAssetsModel = sourceOfIncome;
                    }
                }

            }

            return Json(new[] { originOfTotalAssetsModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult OriginOfTotalAssetsPopup_Update([DataSourceRequest] DataSourceRequest request, OriginOfTotalAssetsModel originOfTotalAssetsModel, string applicationType)
        {
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            if (originOfTotalAssetsModel.Status)
            {
                if (string.IsNullOrEmpty(originOfTotalAssetsModel.OriginOfTotalAssets))
                {
                    ModelState.AddModelError("OriginOfTotalAssets", ResHelper.GetString(OriginOfTotalAssetsErrorMassage.OriginOfTotalAssets));
                }
                if (originOfTotalAssetsModel.AmountOfTotalAsset == 0)
                {
                    ModelState.AddModelError("AmountOfTotalAsset", ResHelper.GetString(OriginOfTotalAssetsErrorMassage.AmountOfTotalAsset));

                }
                //if(sourceOfIncomeModel.PepApplicant_Untill == null)
                //{
                //	ModelState.AddModelError("PepApplicant_Untill", ResHelper.GetString(SourceOfIncomeModelErrorMassage.PepApplicant_UntillError));

                //}
            }
            if (originOfTotalAssetsModel != null && ModelState.IsValid)
            {
                var originOfTotalAssetsData = originOfTotalAssetsRepository.GetOriginOfTotalAsset(originOfTotalAssetsModel.Id);
                if (originOfTotalAssetsData != null)
                {
                    if (isLegalEntity)
                    {
                        var originOfTotalAssets = OriginOfTotalAssetsProcess.SaveOriginOfTotalAssetsModelLegal(0, originOfTotalAssetsModel);
                        if (originOfTotalAssets != null)
                            originOfTotalAssetsModel = originOfTotalAssets;
                    }
                    else
                    {
                        var originOfTotalAssets = OriginOfTotalAssetsProcess.SaveOriginOfTotalAssetsModel(0, originOfTotalAssetsModel);
                        if (originOfTotalAssets != null)
                            originOfTotalAssetsModel = originOfTotalAssets;
                    }

                }
            }

            return Json(new[] { originOfTotalAssetsModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult OriginOfTotalAssetsPopup_Destroy([DataSourceRequest] DataSourceRequest request, OriginOfTotalAssetsModel originOfTotalAssetsModel)
        {
            if (originOfTotalAssetsModel != null)
            {
                originOfTotalAssetsRepository.GetOriginOfTotalAsset(originOfTotalAssetsModel.Id).DeleteAllCultures();
            }

            return Json(new[] { originOfTotalAssetsModel }.ToDataSourceResult(request, ModelState));
        }

        #endregion
        #region Signature Mandate Company

        public IActionResult SignatureMandateCompany_Read([DataSourceRequest] DataSourceRequest request, int apID)
        {
            int applicantId = ValidationHelper.GetInteger(apID, 0);
            List<SignatureMandateCompanyModel> signatureMandateCompanyViewModels = new List<SignatureMandateCompanyModel>();

            if (applicantId > 0)
            {
                // var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicantId);
                var applicationDetails = CompanyDetailsProcess.GetCompanyDetailsModelById(applicantId);

                if (applicationDetails != null && applicationDetails.Id > 0)
                {
                    signatureMandateCompanyViewModels = SignatureMandateCompanyProcess.GetSignatureMandateCompany(applicantId);
                }
            }

            if (signatureMandateCompanyViewModels == null)
                return Json("");

            return Json(signatureMandateCompanyViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult SignatureMandateCompanyPopup_Create([DataSourceRequest] DataSourceRequest request, SignatureMandateCompanyModel signatureMandateCompanyModel, int apID)
        {
            if (signatureMandateCompanyModel.Status)
            {
                //if (signatureMandateCompanyModel.LimitFrom == 0)
                //{
                //    ModelState.AddModelError("LimitFrom", ResHelper.GetString(SignatureMandateCompanyErrorMassage.LimitFrom));
                //}
                //if (signatureMandateCompanyModel.LimitTo == 0)
                //{
                //    ModelState.AddModelError("LimitTo", ResHelper.GetString(SignatureMandateCompanyErrorMassage.LimitTo));
                //}
                if (signatureMandateCompanyModel.TotalNumberofSignature == 0)
                {
                    ModelState.AddModelError("TotalNumberofSignature", ResHelper.GetString(SignatureMandateCompanyErrorMassage.TotalNumberofSignature));
                }
                //if(sourceOfIncomeModel.PepApplicant_Untill == null)
                //{
                //	ModelState.AddModelError("PepApplicant_Untill", ResHelper.GetString(SourceOfIncomeModelErrorMassage.PepApplicant_UntillError));

                //}
            }

            int applicantId = ValidationHelper.GetInteger(apID, 0);
            if (signatureMandateCompanyModel != null && ModelState.IsValid)
            {
                var applicationDetails = CompanyDetailsProcess.GetCompanyDetailsModelById(applicantId);

                if (applicationDetails != null && applicationDetails.Id > 0)
                {
                    int multiSelectCounter = 0;
                    string multiSelectSignatoryPersons = string.Empty;
                    while (Request.Form["AuthorizedSignatoryGroupList[" + multiSelectCounter + "].Value"].Count > 0)
                    {
                        if (string.IsNullOrEmpty(multiSelectSignatoryPersons))
                        {
                            multiSelectSignatoryPersons = Request.Form["AuthorizedSignatoryGroupList[" + multiSelectCounter + "].Value"];
                        }
                        else
                        {
                            multiSelectSignatoryPersons += "|" + Request.Form["AuthorizedSignatoryGroupList[" + multiSelectCounter + "].Value"];
                        }
                        multiSelectCounter++;
                    }

                    int multiSelectCounter1 = 0;
                    string multiSelectSignatoryPersons1 = string.Empty;
                    while (Request.Form["AuthorizedSignatoryGroup1List[" + multiSelectCounter1 + "].Value"].Count > 0)
                    {
                        if (string.IsNullOrEmpty(multiSelectSignatoryPersons1))
                        {
                            multiSelectSignatoryPersons1 = Request.Form["AuthorizedSignatoryGroup1List[" + multiSelectCounter1 + "].Value"];
                        }
                        else
                        {
                            multiSelectSignatoryPersons1 += "|" + Request.Form["AuthorizedSignatoryGroup1List[" + multiSelectCounter1 + "].Value"];
                        }
                        multiSelectCounter1++;
                    }

                    signatureMandateCompanyModel.AuthorizedSignatoryGroup = multiSelectSignatoryPersons;
                    signatureMandateCompanyModel.AuthorizedSignatoryGroup1 = multiSelectSignatoryPersons1;
                    var signatureMandateCompany = SignatureMandateCompanyProcess.SaveSignatureMandateCompanyModel(applicationDetails.Id, signatureMandateCompanyModel);
                    if (signatureMandateCompany != null)
                        signatureMandateCompanyModel = signatureMandateCompany;
                }
            }

            return Json(new[] { signatureMandateCompanyModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult SignatureMandateCompanyPopup_Update([DataSourceRequest] DataSourceRequest request, SignatureMandateCompanyModel signatureMandateCompanyModel)
        {
            if (signatureMandateCompanyModel.Status)
            {
                //if (signatureMandateCompanyModel.LimitFrom == 0)
                //{
                //    ModelState.AddModelError("LimitFrom", ResHelper.GetString(SignatureMandateCompanyErrorMassage.LimitFrom));
                //}
                //if (signatureMandateCompanyModel.LimitTo == 0)
                //{
                //    ModelState.AddModelError("LimitTo", ResHelper.GetString(SignatureMandateCompanyErrorMassage.LimitTo));
                //}
                if (signatureMandateCompanyModel.TotalNumberofSignature == 0)
                {
                    ModelState.AddModelError("TotalNumberofSignature", ResHelper.GetString(SignatureMandateCompanyErrorMassage.TotalNumberofSignature));
                }
                //if(sourceOfIncomeModel.PepApplicant_Untill == null)
                //{
                //	ModelState.AddModelError("PepApplicant_Untill", ResHelper.GetString(SourceOfIncomeModelErrorMassage.PepApplicant_UntillError));

                //}
            }
            if (signatureMandateCompanyModel != null && ModelState.IsValid)
            {
                var signatureMandateCompanyData = signatureMandateCompanyRepository.GetSignatureMandateCompany(signatureMandateCompanyModel.Id);
                if (signatureMandateCompanyData != null)
                {
                    int multiSelectCounter = 0;
                    string multiSelectSignatoryPersons = string.Empty;
                    while (Request.Form["AuthorizedSignatoryGroupList[" + multiSelectCounter + "].Value"].Count > 0)
                    {
                        if (string.IsNullOrEmpty(multiSelectSignatoryPersons))
                        {
                            multiSelectSignatoryPersons = Request.Form["AuthorizedSignatoryGroupList[" + multiSelectCounter + "].Value"];
                        }
                        else
                        {
                            multiSelectSignatoryPersons += "|" + Request.Form["AuthorizedSignatoryGroupList[" + multiSelectCounter + "].Value"];
                        }
                        multiSelectCounter++;
                    }

                    int multiSelectCounter1 = 0;
                    string multiSelectSignatoryPersons1 = string.Empty;
                    while (Request.Form["AuthorizedSignatoryGroup1List[" + multiSelectCounter1 + "].Value"].Count > 0)
                    {
                        if (string.IsNullOrEmpty(multiSelectSignatoryPersons1))
                        {
                            multiSelectSignatoryPersons1 = Request.Form["AuthorizedSignatoryGroup1List[" + multiSelectCounter1 + "].Value"];
                        }
                        else
                        {
                            multiSelectSignatoryPersons1 += "|" + Request.Form["AuthorizedSignatoryGroup1List[" + multiSelectCounter1 + "].Value"];
                        }
                        multiSelectCounter1++;
                    }

                    signatureMandateCompanyModel.AuthorizedSignatoryGroup = multiSelectSignatoryPersons;
                    signatureMandateCompanyModel.AuthorizedSignatoryGroup1 = multiSelectSignatoryPersons1;
                    var signatureMandateCompanys = SignatureMandateCompanyProcess.SaveSignatureMandateCompanyModel(0, signatureMandateCompanyModel);
                    if (signatureMandateCompanys != null)
                        signatureMandateCompanyModel = signatureMandateCompanys;
                }
            }

            return Json(new[] { signatureMandateCompanyModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult SignatureMandateCompanyPopup_Destroy([DataSourceRequest] DataSourceRequest request, SignatureMandateCompanyModel signatureMandateCompanyModel)
        {
            if (signatureMandateCompanyModel != null)
            {
                signatureMandateCompanyRepository.GetSignatureMandateCompany(signatureMandateCompanyModel.Id).DeleteAllCultures();
            }

            return Json(new[] { signatureMandateCompanyModel }.ToDataSourceResult(request, ModelState));
        }

        #endregion
        #region Signatory Group

        public IActionResult SignatoryGroup_Read([DataSourceRequest] DataSourceRequest request, int apID)
        {
            int applicantId = ValidationHelper.GetInteger(apID, 0);
            List<SignatoryGroupModel> signatoryGroupViewModels = new List<SignatoryGroupModel>();

            if (applicantId > 0)
            {
                var applicationDetails = CompanyDetailsProcess.GetCompanyDetailsModelById(applicantId);

                if (applicationDetails != null && applicationDetails.Id > 0)
                {
                    signatoryGroupViewModels = SignatoryGroupProcess.GetSignatoryGroup(applicantId);
                }
            }

            if (signatoryGroupViewModels == null)
                return Json("");

            return Json(signatoryGroupViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult SignatoryGroupPopup_Create([DataSourceRequest] DataSourceRequest request, SignatoryGroupModel signatoryGroupModel, int apID)
        {
            if (signatoryGroupModel.Status)
            {
                if (string.IsNullOrEmpty(signatoryGroupModel.SignatoryGroupName))
                {
                    ModelState.AddModelError("SignatoryGroupName", ResHelper.GetString(SignatoryGroupErrorMassage.SignatoryGroupName));
                }
                if (signatoryGroupModel.StartDate == DateTime.MinValue)
                {
                    ModelState.AddModelError("StartDate", ResHelper.GetString(SignatoryGroupErrorMassage.StartDate));
                }
                if (signatoryGroupModel.EndDate == DateTime.MinValue)
                {
                    ModelState.AddModelError("EndDate", ResHelper.GetString(SignatoryGroupErrorMassage.EndDate));
                }
            }

            int applicantId = ValidationHelper.GetInteger(apID, 0);
            if (signatoryGroupModel != null && ModelState.IsValid)
            {
                var applicationDetails = CompanyDetailsProcess.GetCompanyDetailsModelById(applicantId);

                if (applicationDetails != null && applicationDetails.Id > 0)
                {
                    int multiSelectCounter = 0;
                    string multiSelectSignatoryPersons = string.Empty;
                    while (Request.Form["SignatoryPersonsList[" + multiSelectCounter + "].Value"].Count > 0)
                    {
                        if (string.IsNullOrEmpty(multiSelectSignatoryPersons))
                        {
                            multiSelectSignatoryPersons = Request.Form["SignatoryPersonsList[" + multiSelectCounter + "].Value"];
                        }
                        else
                        {
                            multiSelectSignatoryPersons += "|" + Request.Form["SignatoryPersonsList[" + multiSelectCounter + "].Value"];
                        }
                        multiSelectCounter++;
                    }
                    signatoryGroupModel.SignatoryPersons = multiSelectSignatoryPersons;
                    var signatoryGroup = SignatoryGroupProcess.SaveSignatoryGroupModel(applicationDetails.Id, signatoryGroupModel);
                    if (signatoryGroup != null)
                        signatoryGroupModel = signatoryGroup;
                }
            }

            return Json(new[] { signatoryGroupModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult SignatoryGroupPopup_Update([DataSourceRequest] DataSourceRequest request, SignatoryGroupModel signatoryGroupModel)
        {
            if (signatoryGroupModel.Status)
            {
                if (string.IsNullOrEmpty(signatoryGroupModel.SignatoryGroupName))
                {
                    ModelState.AddModelError("SignatoryGroupName", ResHelper.GetString(SignatoryGroupErrorMassage.SignatoryGroupName));
                }
                if (signatoryGroupModel.StartDate == DateTime.MinValue)
                {
                    ModelState.AddModelError("StartDate", ResHelper.GetString(SignatoryGroupErrorMassage.StartDate));
                }
                if (signatoryGroupModel.EndDate == DateTime.MinValue)
                {
                    ModelState.AddModelError("EndDate", ResHelper.GetString(SignatoryGroupErrorMassage.EndDate));
                }
            }

            if (signatoryGroupModel != null && ModelState.IsValid)
            {
                var signatoryGroupData = signatoryGroupRepository.GetSignatoryGroup(signatoryGroupModel.Id);
                if (signatoryGroupData != null)
                {
                    int multiSelectCounter = 0;
                    string multiSelectSignatoryPersons = string.Empty;
                    while (Request.Form["SignatoryPersonsList[" + multiSelectCounter + "].Value"].Count > 0)
                    {
                        if (string.IsNullOrEmpty(multiSelectSignatoryPersons))
                        {
                            multiSelectSignatoryPersons = Request.Form["SignatoryPersonsList[" + multiSelectCounter + "].Value"];
                        }
                        else
                        {
                            multiSelectSignatoryPersons += "|" + Request.Form["SignatoryPersonsList[" + multiSelectCounter + "].Value"];
                        }
                        multiSelectCounter++;
                    }
                    signatoryGroupModel.SignatoryPersons = multiSelectSignatoryPersons;
                    var signatoryGroups = SignatoryGroupProcess.SaveSignatoryGroupModel(0, signatoryGroupModel);
                    if (signatoryGroups != null)
                        signatoryGroupModel = signatoryGroups;
                }
            }

            return Json(new[] { signatoryGroupModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult SignatoryGroupPopup_Destroy([DataSourceRequest] DataSourceRequest request, SignatoryGroupModel signatoryGroupModel)
        {
            if (signatoryGroupModel != null)
            {
                signatoryGroupRepository.GetSignatoryGroup(signatoryGroupModel.Id).DeleteAllCultures();
            }

            return Json(new[] { signatoryGroupModel }.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region-----------------Contact Details----------------------
        [HttpPost]
        public ActionResult ContactDetails(ApplicantModel model)
        {
            bool isLegalEntity = string.Equals(model.Type, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);


            if (isLegalEntity)
            {
                var applicationData = companyDetailsRepository.GetCompanyDetailsByID(model.CompanyDetails.Id);
                string path = applicationData.NodeAliasPath;
                var contactDetailsTreeNode = contactDetailsRepository.GetContactDetails(path);
                if (model.ContactDetails.ContactDetailsID > 0)
                {
                    model.ContactDetailsLegal = ContactDetailsLegalProcess.UpdateContactDetailsLegal(model.ContactDetailsLegal, contactDetailsTreeNode.FirstOrDefault());
                }
                else
                {
                    model.ContactDetailsLegal = ContactDetailsLegalProcess.SaveContactDetailsLegal(model.ContactDetailsLegal, applicationData);
                    contactDetailsTreeNode = contactDetailsRepository.GetContactDetails(path);
                }

            }
            else
            {
                var applicatioData = personalDetailsRepository.GetPersonalDetailsByID(model.PersonalDetails.Id);
                string Path = applicatioData.NodeAliasPath;
                var contactDetailsTreeNode = contactDetailsRepository.GetContactDetails(Path);
                if (model.ContactDetails.ContactDetailsID > 0)
                {
                    model.ContactDetails = ContactDetailsProcess.UpdateContactDetails(model.ContactDetails, contactDetailsTreeNode.FirstOrDefault());
                }
                else
                {
                    model.ContactDetails = ContactDetailsProcess.SaveContactDetails(model.ContactDetails, applicatioData);
                    contactDetailsTreeNode = contactDetailsRepository.GetContactDetails(Path);
                }
                model.ContactDetails = ContactDetailsProcess.GetContactDetailsById(contactDetailsTreeNode.FirstOrDefault());
            }

            return PartialView("_ContactDetails", model);
        }
        public ActionResult ContactDetailsLegal(ApplicantModel model)
        {
            bool isLegalEntity = string.Equals(model.Type, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            if (isLegalEntity)
            {
                var applicationData = companyDetailsRepository.GetCompanyDetailsByID(model.CompanyDetails.Id);
                string path = applicationData.NodeAliasPath;
                var contactDetailsTreeNode = contactDetailsLegalRepository.GetContactDetailsLegal(path);
                if (model.ContactDetailsLegal.ContactDetailsLegalID > 0)
                {
                    model.ContactDetailsLegal = ContactDetailsLegalProcess.UpdateContactDetailsLegal(model.ContactDetailsLegal, contactDetailsTreeNode.FirstOrDefault());
                }
                else
                {
                    model.ContactDetailsLegal = ContactDetailsLegalProcess.SaveContactDetailsLegal(model.ContactDetailsLegal, applicationData);
                    contactDetailsTreeNode = contactDetailsLegalRepository.GetContactDetailsLegal(path);
                }
            }
            return PartialView("_ContactDetailsLegal", model);
        }
        public JsonResult PreferredCommunicationLanguage_Read()
        {
            var Language = ServiceHelper.GetCommunicationLanguage();
            return Json(Language);
        }
        public JsonResult PreferredMailingAddress_Read()
        {
            var Language = ServiceHelper.GetPreferred_Mailing_Address();
            return Json(Language);
        }
        #endregion

        #region---------FATCA CSR Details--------
        public JsonResult FATCA_CLASSIFICATION_Read()
        {
            var result = ServiceHelper.GetFATCA_CLASSIFICATION();
            return Json(result);
        }
        public JsonResult US_ENTITY_TYPE_Read()
        {
            var result = ServiceHelper.GetUS_ENTITY_TYPE();
            return Json(result);
        }
        public JsonResult TYPE_OF_FOREIGN_FINANCIAL_INSTITUTION_Read()
        {
            var result = ServiceHelper.GetTYPE_OF_FOREIGN_FINANCIAL_INSTITUTION();
            return Json(result);
        }
        public JsonResult TYPE_OF_NON_FINANCIAL_FOREIGN_ENTITY_NFFE_Read()
        {
            var result = ServiceHelper.GetTYPE_OF_NON_FINANCIAL_FOREIGN_ENTITY_NFFE();
            return Json(result);
        }
        public JsonResult GLOBAL_INTERMEDIARY_IDENTIFICATION_NUMBER_GIIN_Read()
        {
            var result = ServiceHelper.GetGLOBAL_INTERMEDIARY_IDENTIFICATION_NUMBER_GIIN();
            return Json(result);
        }
        public JsonResult EXEMPTION_REASON_Read()
        {
            var result = ServiceHelper.GetEXEMPTION_REASON();
            return Json(result);
        }

        public JsonResult CRS_CLASSIFICATION_Read()
        {
            var result = ServiceHelper.GetCRS_CLASSIFICATION();
            return Json(result);
        }
        public JsonResult TYPE_OF_ACTIVE_NON_FINANCIAL_ENTITY_NFE_Read()
        {
            var result = ServiceHelper.GetTYPE_OF_ACTIVE_NON_FINANCIAL_ENTITY_NFE();
            return Json(result);
        }
        public JsonResult NAME_OF_ESTABLISHED_SECURITES_MARKET_Read()
        {
            var result = ServiceHelper.GetNAME_OF_ESTABLISHED_SECURITES_MARKET();
            return Json(result);
        }
        public JsonResult TYPE_OF_FINANCIAL_INSITUTION_Read()
        {
            var result = ServiceHelper.GetTYPE_OF_FINANCIAL_INSITUTION();
            return Json(result);
        }
        #endregion

        #region Validation

        [HttpPost]
        public JsonResult ValidateCompanyDetails(ApplicantModel applicant)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (applicant != null && applicant.CompanyDetails != null)
            {
                //Need to implement compnay details validation
                result = ApplicantLegalFormBasicValidationProcess.ValidateCompanyDetails(applicant.CompanyDetails);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateFatcaDetails(ApplicantModel applicant)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (applicant != null && applicant.FATCACRSDetails != null)
            {
                //Need to implement compnay details validation
                result = ApplicantLegalFormBasicValidationProcess.ValidateFATCADetails(applicant.FATCACRSDetails);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateCrsDetails(ApplicantModel applicant)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (applicant != null && applicant.CRSDetails != null)
            {
                //Need to implement compnay details validation
                result = ApplicantLegalFormBasicValidationProcess.ValidateCRSDetails(applicant.CRSDetails);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateBusinessProfileLegal(ApplicantModel applicant)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (applicant != null && applicant.CompanyBusinessProfile != null)
            {
                //Need to implement
                result = ApplicantLegalFormBasicValidationProcess.ValidateBusinessProfile(applicant.CompanyDetails, applicant.CompanyBusinessProfile);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateFinancialProfileLegal(ApplicantModel applicant)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (applicant != null && applicant.CompanyFinancialInformation != null)
            {
                result = ApplicantLegalFormBasicValidationProcess.ValidateFinancialProfile(applicant.CompanyDetails, applicant.CompanyFinancialInformation);
                if (result.IsValid == true)
                {
                    result = ApplicantLegalGridValidationProcess.ValidateOriginOfTotalAssetsDetails(applicant.CompanyDetails.Id);
                }
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateContactDetailsLegal(ApplicantModel applicant)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (applicant != null && applicant.ContactDetailsLegal != null)
            {
                result = ApplicantLegalFormBasicValidationProcess.ValidateContactDetails(applicant.ContactDetailsLegal);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateContactDetailsIndividual(ApplicantModel applicant)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (applicant != null && applicant.ContactDetails != null)
            {
                result = ApplicantIndividualFormBasicValidationProcess.ValidateContactDetails(applicant.ContactDetails);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateBusinessAndFinancialProfile(ApplicantModel applicant)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (applicant != null && applicant.EmploymentDetails != null)
            {
                result = ApplicantIndividualFormBasicValidationProcess.ValidateBusinessAndFinancialProfile(applicant.EmploymentDetails);
                if(result.IsValid == true)
                {
                    result = ApplicantIndividualGridValidationProcess.ValidateOriginOfTotalAssets(applicant.PersonalDetails.Id);
                    if(result.IsValid == true)
                    {
                        result = ApplicantIndividualGridValidationProcess.ValidateSourceOfIncome(applicant.PersonalDetails.Id, applicant.EmploymentDetails);
					}
				}
			}

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidatePersonalDetails(ApplicantModel applicant)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (applicant != null && applicant.PersonalDetails != null)
            {
                if (!string.IsNullOrEmpty(applicant.PersonalDetails.HdnDateOfBirth))
                {
                    DateTime resultOut = DateTime.MinValue;
                    if (DateTime.TryParseExact(applicant.PersonalDetails.HdnDateOfBirth, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out resultOut))
                    {
                        applicant.PersonalDetails.DateOfBirth = resultOut;
                    }
                }
                else
                {
                    applicant.PersonalDetails.DateOfBirth = null;
                }
                result = ApplicantIndividualFormBasicValidationProcess.ValidatePersonalDetails(applicant.PersonalDetails);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateBankingRelationshipIndividual(ApplicantModel applicant)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (applicant != null && applicant.PersonalDetails != null)
            {
                //Need to implement
                result = ApplicantIndividualFormBasicValidationProcess.ValidateBankingRelationshipIndividual(applicant.PersonalDetails);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateBankingRelationshipLegal(ApplicantModel applicant)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (applicant != null && applicant.CompanyDetails != null)
            {
                result = ApplicantLegalFormBasicValidationProcess.ValidateBankingRelationshipLegal(applicant.CompanyDetails);
            }

            return Json(result);
        }

        #endregion

        public JsonResult GetLookupName(string nodeGuid, string path)
        {
            var name = ServiceHelper.GetName(nodeGuid, path);
            return Json(name);
        }
    }
}
