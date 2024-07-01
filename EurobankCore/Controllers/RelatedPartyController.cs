using AngleSharp.Text;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.SiteProvider;
using Eurobank.Helpers.CustomHandler;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.Generics;
using Eurobank.Helpers.Process;
using Eurobank.Helpers.Validation;
using Eurobank.Models;
using Eurobank.Models.Application;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Applicant.LegalEntity;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Application.RelatedParty;
using Eurobank.Models.Application.RelatedParty.ContactDetails;
using Eurobank.Models.Application.RelatedParty.PartyRoles;
using Eurobank.Models.Application.RelatedParty.PartyRolesLegal;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Eurobank.Controllers
{
    [AuthorizeRoles(Role.PowerUser, Role.NormalUser)]
	[SessionAuthorization]
	//[IgnoreAntiforgeryToken]
	public class RelatedPartyController : Controller
    {
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly ApplicationsRepository applicationsRepository;
        private readonly TaxDetailsRepository taxDetailsRepository;
        private readonly AddressDetailsRepository addressDetailsRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly PersonalDetailsRepository personalDetailsRepository;
        private readonly ContactDetailsRepository contactDetailsRepository;
        private readonly PepAssociatesRepository pepAssociatesRepository;
        private readonly PepApplicantRepository pepApplicantRepository;
        private readonly SignatoryGroupRepository signatoryGroupRepository;
        private readonly PartyRolesRepository partyRolesRepository;
        private readonly PartyRolesLegalRepository partyRolesLegalRepository;
        private readonly CompanyDetailsRelatedPartyRepository companyDetailsRelatedPartyRepository;
        private readonly SourceOfIncomeRepository sourceOfIncomeRepository;
        private readonly OriginOfTotalAssetsRepository originOfTotalAssetsRepository;
        private readonly RegistriesRepository registriesRepository;
        private readonly BankDocumentsRepository bankDocumentsRepository;
        private readonly ExpectedDocumentsRepository expectedDocumentsRepository;
		private readonly DebitCardDetailsRepository debitCardDetailsRepository;

		public RelatedPartyController(IPageDataContextRetriever pageDataContextRetriever,
            IPageUrlRetriever pageUrlRetriever,
             ApplicationsRepository applicationsRepository, TaxDetailsRepository taxDetailsRepository, AddressDetailsRepository addressDetailsRepository, IHttpContextAccessor httpContextAccessor, PersonalDetailsRepository personalDetailsRepository,
             ContactDetailsRepository contactDetailsRepository, PepApplicantRepository pepApplicantRepository,
             PepAssociatesRepository pepAssociatesRepository, PartyRolesRepository partyRolesRepository, PartyRolesLegalRepository partyRolesLegalRepository, SignatoryGroupRepository signatoryGroupRepository,
             CompanyDetailsRelatedPartyRepository companyDetailsRelatedPartyRepository, SourceOfIncomeRepository sourceOfIncomeRepository, OriginOfTotalAssetsRepository originOfTotalAssetsRepository, RegistriesRepository registriesRepository,
             ExpectedDocumentsRepository expectedDocumentsRepository, BankDocumentsRepository bankDocumentsRepository, DebitCardDetailsRepository debitCardDetailsRepository)
        {
            this.pageDataContextRetriever = pageDataContextRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
            this.applicationsRepository = applicationsRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.taxDetailsRepository = taxDetailsRepository;
            this.addressDetailsRepository = addressDetailsRepository;
            this.personalDetailsRepository = personalDetailsRepository;
            this.contactDetailsRepository = contactDetailsRepository;
            this.partyRolesRepository = partyRolesRepository;
            this.pepApplicantRepository = pepApplicantRepository;
            this.pepAssociatesRepository = pepAssociatesRepository;
            this.signatoryGroupRepository = signatoryGroupRepository;
            this.partyRolesLegalRepository = partyRolesLegalRepository;
            this.companyDetailsRelatedPartyRepository = companyDetailsRelatedPartyRepository;
            this.sourceOfIncomeRepository = sourceOfIncomeRepository;
            this.originOfTotalAssetsRepository = originOfTotalAssetsRepository;
            this.registriesRepository = registriesRepository;
            this.expectedDocumentsRepository = expectedDocumentsRepository;
            this.bankDocumentsRepository = bankDocumentsRepository;
			this.debitCardDetailsRepository = debitCardDetailsRepository;
		}
        public IActionResult Index(string application, string relatedParty, string type, bool isUBO)
        {
            //Get applicationID from NodeGUID
            var applicationDetail = applicationsRepository.GetApplicationDetailsByNodeGUID(application);
            //Get applicantID from NodeGUID
            int applicationId = applicationDetail.ApplicationDetailsID;
            string entityTypeCode = ValidationHelper.GetString(ServiceHelper.GetEntityType(applicationDetail.ApplicationDetails_ApplicationType, Constants.APPLICATION_TYPE), "");
            int relatedPartyId = 0;
            if (!string.IsNullOrEmpty(relatedParty))
            {
                if (string.Equals(entityTypeCode, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    bool iRealtedPartysLegalEntity = (type != null && type == "LEGAL ENTITY");
                    if (iRealtedPartysLegalEntity)
                    {

                        var relatedPartyDetailsLegal = CompanyDetailsRelatedPartyProvider.GetCompanyDetailsRelatedParty(new Guid(relatedParty), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                        if (relatedPartyDetailsLegal != null)
                            relatedPartyId = relatedPartyDetailsLegal.CompanyDetailsRelatedPartyID;
                    }
                    else
                    {
                        var relatedPartyIndividual = PersonalDetailsProvider.GetPersonalDetails(new Guid(relatedParty), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                        if (relatedPartyIndividual != null)
                            relatedPartyId = relatedPartyIndividual.PersonalDetailsID;
                    }
                }
                else
                {
                    var relatedPartyIndividual = PersonalDetailsProvider.GetPersonalDetails(new Guid(relatedParty), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    if (relatedPartyIndividual != null)
                        relatedPartyId = relatedPartyIndividual.PersonalDetailsID;
                }
            }
            //---------------------------------------------------
            RelatedPartyModel model = new RelatedPartyModel();
            if (applicationId == 0 && relatedPartyId == 0)
                return Redirect("/Applications");
            UserModel userModel = UserProcess.GetUser(User.Identity.Name);
            //var applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);
            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationId);
            if (applicationDetails == null && relatedPartyId == 0)
                return Redirect("/Applications");

            var applicationTypes = ServiceHelper.GetApplicationType();
            bool isLegalEntity = false;
            if (TempData["ErrorSummary"] != null)
            {
                ViewBag.ErrorSummaryMsgs = JsonConvert.DeserializeObject<List<ValidationResultModel>>(TempData["ErrorSummary"].ToString());
            }

            if (applicationDetails != null)
            {
                var applicationDetailModel = ApplicationsProcess.GetApplicationsDetails(userModel.UserType, userModel.UserRole, applicationDetails);
                model.ApplicationNumber = applicationDetails.ApplicationDetails_ApplicationNumber;
                model.Application_NodeGUID = applicationDetailModel.Application_NodeGUID;
                model.ApplicationID = applicationId;
                model.ApplicationTypeName = (applicationTypes != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationType) && applicationTypes.Any(k => string.Equals(k.Value, applicationDetails.ApplicationDetails_ApplicationType, StringComparison.OrdinalIgnoreCase))) ? applicationTypes.FirstOrDefault(k => string.Equals(k.Value, applicationDetails.ApplicationDetails_ApplicationType, StringComparison.OrdinalIgnoreCase)).Text : string.Empty;
                //isLegalEntity = string.Equals(model.Type, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
                isLegalEntity = (type != null && type == "LEGAL ENTITY");
                model.ApplicationStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                model.ApplicationStatus = ServiceHelper.GetName(ValidationHelper.GetString(applicationDetails.ApplicationDetails_ApplicationStatus, ""), "/Lookups/General/APPLICATION-WORKFLOW-STATUS");
                //model.Type = applicationDetails.ApplicationDetails_ApplicationTypeName;
                model.Introducer = applicationDetails.ApplicationDetails_IntroducerName;
                model.IsEdit = applicationDetailModel.IsEdit;
                model.LeftMenuApplicant = new LeftMenuCommon();

                if (isLegalEntity)
                {
                    if (relatedPartyId > 0)
                    {
                        model.LeftMenuApplicant.LeftMenuItems = LeftMenuProcess.GetRelatedPartyConfigurationLegal();
                    }
                    else
                    {
                        model.LeftMenuApplicant.LeftMenuItems = LeftMenuProcess.GetRelatedPartyConfigurationLegalCreate();
                    }

                    model.LeftMenuApplicant.LeftmenuClientMathodName = LeftMenuClientMathod.onRelatedPartyStepperSelect;
                }
                else
                {
                    if (relatedPartyId > 0)
                    {
                        model.LeftMenuApplicant.LeftMenuItems = LeftMenuProcess.GetRelatedPartyConfiguration();
                    }
                    else
                    {
                        model.LeftMenuApplicant.LeftMenuItems = LeftMenuProcess.GetRelatedPartyConfigurationCreate();
                    }

                    model.LeftMenuApplicant.LeftmenuClientMathodName = LeftMenuClientMathod.onRelatedPartyStepperSelect;
                }
            }
            if (relatedPartyId > 0)
            {
                string Path = string.Empty;

                if (isLegalEntity)
                {
                    model.CompanyDetails = CompanyDetailsRelatedPartyProcess.GetCompanyDetailsRelatedPartyModelById(relatedPartyId);

                    //model.CompanyDetails.IsRelatedPartyUBO = isUBO;
                    if (model.CompanyDetails != null)
                    {
                        if (model.CompanyDetails.DateofIncorporation == DateTime.MinValue)
                        {
                            model.CompanyDetails.DateofIncorporation = null;
                        }
                        model.CompanyDetails.HdnDateofIncorporation = model.CompanyDetails.DateofIncorporation.HasValue ? model.CompanyDetails.DateofIncorporation.Value.ToString("dd/MM/yyyy") : null;
                        model.NodeGUID = model.CompanyDetails.NodeGUID;
                        model.NodePath = model.CompanyDetails.NodePath;
                    }

                    var companyDetailsApplicationData = companyDetailsRelatedPartyRepository.GetCompanyDetailsRelatedParty(model.CompanyDetails.Id);
                    Path = companyDetailsApplicationData.NodeAliasPath;
                    var contactDetailsTreeNode = contactDetailsRepository.GetContactDetails(Path);
                    if (contactDetailsTreeNode != null && contactDetailsTreeNode.Any())
                    {
                        model.ContactDetails = ContactDetailsProcess.GetContactDetailsById(contactDetailsTreeNode.FirstOrDefault());
                    }
                    var partyRolesTreeNode = partyRolesLegalRepository.GetRelatedPartyRolesLegal(Path);

                    if (partyRolesTreeNode != null && partyRolesTreeNode.Any())
                    {
                        model.PartyRolesLegal = RelatedPartyRolesLegalProcess.GetPartyRolesLegalById(partyRolesTreeNode.FirstOrDefault());

                    }
                }
                else
                {
                    model.PersonalDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(relatedPartyId);
                    //model.PersonalDetails.IsRelatedPartyUBO = isUBO;
                    var applicatioData = personalDetailsRepository.GetPersonalDetailsByID(relatedPartyId);
                    Path = applicatioData.NodeAliasPath;

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
                    var contactDetailsTreeNode = contactDetailsRepository.GetContactDetails(Path);
                    if (contactDetailsTreeNode != null && contactDetailsTreeNode.Any())
                    {
                        model.ContactDetails = ContactDetailsProcess.GetContactDetailsById(contactDetailsTreeNode.FirstOrDefault());
                    }
                    //Application Type=Legal Entity and Related Party type= Individual
                    if (string.Equals(model.ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase) && string.Equals(type, "INDIVIDUAL", StringComparison.OrdinalIgnoreCase))
                    {
                        var partyRolesTreeNode = partyRolesLegalRepository.GetRelatedPartyRolesLegal(Path);

                        if (partyRolesTreeNode != null && partyRolesTreeNode.Any())
                        {
                            model.PartyRolesLegal = RelatedPartyRolesLegalProcess.GetPartyRolesLegalById(partyRolesTreeNode.FirstOrDefault());
                        }
                    }
                    else
                    {
                        var partyRolesTreeNode = partyRolesRepository.GetRelatedPartyRoles(Path);

                        if (partyRolesTreeNode != null && partyRolesTreeNode.Any())
                        {
                            model.PartyRoles = RelatedPartyRolesProcess.GetPartyRolesById(partyRolesTreeNode.FirstOrDefault());
                        }
                    }

                }
                model.NodePath = Path;

                model.Id = relatedPartyId;
                model.EmploymentDetails = EmploymentDetailsRelatedPartyProcess.GetEmploymentDetailsRelatedPartyModelByRelatedPartyId(relatedPartyId);
                if (model.EmploymentDetails == null)
                {
                    model.EmploymentDetails = new EmploymentDetailsRelatedPartyModel();
                }

                //model.LeftMenuApplicant = LeftMenuProcess.GetRelatedPartyConfiguration();
            }
            else
            {
                if (isLegalEntity)
                {
                    model.CompanyDetails = new CompanyDetailsRelatedPartyModel();
                    model.CompanyDetails.IsRelatedPartyUBO = isUBO;
                }
                else
                {
                    model.PersonalDetails = new PersonalDetailsModel();
                    model.PersonalDetails.IsRelatedPartyUBO = isUBO;
                    model.PersonalDetails.InvitedpersonforonlineIDverification = ControlBinder.BindRadioButtonGroupItems(ServiceHelper.GetHID(), "");
                }

            }

            if (isLegalEntity)
            {
                model.Type = "LEGAL ENTITY";
            }
            else
            {
                model.Type = "INDIVIDUAL";
            }
            ViewBag.PersonTitle = ServiceHelper.GetTitle();
            ViewBag.Gender = ServiceHelper.GetGendar();
            ViewBag.Education = ServiceHelper.GetEducationLevel();
            //ViewBag.Country = ServiceHelper.GetCountries();
            ViewBag.Country = ServiceHelper.GetCountriesWithID();
            ViewBag.CountryIdentification = ServiceHelper.GetCountriesWithID();

            ViewBag.SourcesOfAnnualIncome = ServiceHelper.GetSourcesOfAnnualIncome();

            ViewBag.EmploymentStatuses = ServiceHelper.GetEmploymentStatus();
            ViewBag.EmploymentProfessions = ServiceHelper.GetEmploymentProfessions();
            ViewBag.CountriesOfEmployment = ServiceHelper.GetCountriesWithID();


            //ViewBag.AddressTypes = ServiceHelper.GetAddressType();
            ViewBag.CompanyEntities = ServiceHelper.GetCompanyEntityTypes();
            ViewBag.Countries = ServiceHelper.GetCountries();
            ViewBag.SignatoryPersons = ServiceHelper.GetNoteDetailPendingOnUsers();

            if (model.PersonalDetails != null && model.PersonalDetails.IsRelatedPartyUBO)
            {
                ViewBag.OriginsOfTotalAssets = ServiceHelper.GetOriginOfTotalAssetsForIndividual();
            }
            else
            {
                ViewBag.OriginsOfTotalAssets = ServiceHelper.GetOriginOfTotalAssets();
            }

            //Header menu data
            ApplicationViewModel _model = new ApplicationViewModel();
            ApplicationDetailsModelView applicationDetailsModelView = new ApplicationDetailsModelView();
            applicationDetailsModelView.ApplicationDetails_ApplicationNumber = model.ApplicationNumber;
            applicationDetailsModelView.ApplicationDetails_ApplicationStatus = model.ApplicationStatus;
            applicationDetailsModelView.ApplicationDetails_ApplicationStatusName = ServiceHelper.GetName(ValidationHelper.GetString(applicationDetails.ApplicationDetails_ApplicationStatus, ""), "/Lookups/General/APPLICATION-WORKFLOW-STATUS");
            applicationDetailsModelView.ApplicationDetails_ApplicationTypeName = model.ApplicationTypeName;
            applicationDetailsModelView.ApplicationDetails_IntroducerName = model.Introducer;
            applicationDetailsModelView.ApplicationDetails_SubmittedOn = Convert.ToDateTime(applicationDetails.ApplicationDetails_SubmittedOn).ToString("dd/MM/yyyy");
            applicationDetailsModelView.ApplicationDetails_SubmittedBy = applicationDetails.ApplicationDetails_SubmittedBy;
            applicationDetailsModelView.ApplicationDetails_CreatedOn = Convert.ToDateTime(applicationDetails.DocumentCreatedWhen).ToString("dd/MM/yyyy");
            _model.ApplicationDetails = applicationDetailsModelView;
            _model.LeftMenuApplicant = model.LeftMenuApplicant;
            TempData["Application"] = _model;
            model.ApplicationType = applicationDetails.ApplicationDetails_ApplicationType;
            model.IsRelatedPartyUBO = isUBO;
            bool isLegal = string.Equals(model.ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            bool isRelatedLegal = string.Equals(model.Type, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            ViewBag.isRelatedLegal = isRelatedLegal;
            if (isLegal && isRelatedLegal)
            {
                ViewBag.AddressTypes = ServiceHelper.GetAddressTypeLegalRelatedParty();
            }
            else
            {
                ViewBag.AddressTypes = ServiceHelper.GetAddressTypeIndividualRelatedParty(model.PersonalDetails.IsRelatedPartyUBO);

            }

            ViewBag.Countries = ServiceHelper.GetCountriesWithID();

            //if (isLegal && !isRelatedLegal)
            //{
            var applicantDetails = ApplicantProcess.GetLegalApplicantModels(model.ApplicationNumber);
            if (applicantDetails != null && applicantDetails.Count > 0)
            {
                TempData["ApplicantEntityType"] = ServiceHelper.GetName(applicantDetails.FirstOrDefault().CompanyDetails.EntityType, Constants.COMPANY_ENTITY_TYPE);
                TempData["ApplicantCountryOfIncorporation"] = ServiceHelper.GetCountriesWithID().Where(x=>x.Value== applicantDetails.FirstOrDefault().CompanyDetails.CountryofIncorporation).Any() ?
                    ServiceHelper.GetCountriesWithID().Where(x => x.Value == applicantDetails.FirstOrDefault().CompanyDetails.CountryofIncorporation).FirstOrDefault().Text : "";
            }

            //}
            if (isRelatedLegal)
            {
                ViewBag.identificationType = ServiceHelper.GetTypeIdentificationLegal();
            }
            else
            {
                ViewBag.identificationType = ServiceHelper.GetTypeIdentificationIndividual();
            }
            ViewBag.IsRelatedParty = true;
            TempData["IsRelatedParty"] = true;
            return View(model);
        }
        [HttpPost]
        public ActionResult RelatedParty(int applicationId, RelatedPartyModel model, string relatedPartyButton)
        {
            bool isLegalEntity = string.Equals(model.Type, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            int relatedPartyID = 0;
            string relatedPartyNodeGUID = string.Empty;
            bool totalConfirmValidation = false;
            List<ValidationResultModel> applicantValidationResult = null;
            UserModel userModel = UserProcess.GetUser(User.Identity.Name);
            int applicationID = model.ApplicationID;
            bool isAuthorizeCardHolder = false;
            if(model.PartyRolesLegal != null)
            {
                isAuthorizeCardHolder = model.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedCardholder;
			}
            if (Request.Form["InvitedpersonforonlineIDverification"].Any())
            {
                model.PersonalDetails.InvitedpersonforonlineIDverification = new RadioGroupViewModel() { RadioGroupValue = Request.Form["InvitedpersonforonlineIDverification"] };
            }
            if (model == null || ((string.IsNullOrEmpty(model.ApplicationNumber) && (model.PersonalDetails.Id == 0) && model.CompanyDetails.Id == 0)))
            {
                return View(model);
            }
            else
            {
                if (model.PersonalDetails != null && !isLegalEntity)
                {
                    if (model.EmploymentDetails != null && model.PersonalDetails.Id > 0 && !string.IsNullOrEmpty(model.EmploymentDetails.EmploymentStatus))
                    {
                        model.EmploymentDetails = EmploymentDetailsRelatedPartyProcess.SaveEmploymentDetailsRelatedPartyModel(model.PersonalDetails.Id, model.EmploymentDetails);
                    }

                    //if (!string.IsNullOrEmpty(model.PersonalDetails.FirstName))
                    //{
                        model.PersonalDetails.Type = CommonProcess.GetApplicationValue(model.Type);
                        if (!string.IsNullOrEmpty(model.PersonalDetails.HdnDateOfBirth))
                        {
                            model.PersonalDetails.DateOfBirth = DateTime.ParseExact(model.PersonalDetails.HdnDateOfBirth, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            model.PersonalDetails.DateOfBirth = null;
                        }
                        if (string.Equals(relatedPartyButton, "SAVECLOSE", StringComparison.OrdinalIgnoreCase))
                        {
                            //Do Validation Here
                            //If Validate 
                            applicantValidationResult = RelatedPartyIndividualValidationProcess.ValidateRelatedPartyIndividual(model, isLegalEntity);
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
                    bool PersonalDetailsSaveInRegistry = model.PersonalDetails.SaveInRegistry;
                        model.PersonalDetails = PersonalDetailsProcess.SaveRelatedPartyPersonalDetailsModel(model.ApplicationNumber, model.PersonalDetails);
                        if (model.ContactDetails != null && (model.ContactDetails.ContactDetails_SaveInRegistry || PersonalDetailsSaveInRegistry))
                        {
                            if (userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null)
                            {
                                int personDetilsRegitryId = 0;
                                IdentificationDetailsViewModel selectedIdentification = null;
                                var applicantIdentifications = IdentificationDetailsProcess.GetIdentificationDetails(model.PersonalDetails.Id);
                                var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);
                                var personRegistries = registriesRepository.GetRegistries(UserRegistry.NodeAliasPath);
                                if (personRegistries != null && personRegistries.Any(y => y.DateofBirth == model.PersonalDetails.DateOfBirth) && applicantIdentifications != null)
                                {
                                    var selectedPersonRegistry = personRegistries.FirstOrDefault(y => y.DateofBirth == model.PersonalDetails.DateOfBirth);
                                    if (applicantIdentifications.Any(p => string.Equals(p.IdentificationDetails_IdentificationNumber, selectedPersonRegistry.IdentificationNumber, StringComparison.OrdinalIgnoreCase) && string.Equals(p.IdentificationDetails_CountryOfIssue.Value.ToString(), selectedPersonRegistry.IssuingCountry, StringComparison.OrdinalIgnoreCase) && string.Equals(p.IdentificationDetails_TypeOfIdentification, selectedPersonRegistry.TypeofIdentification, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        selectedIdentification = applicantIdentifications.FirstOrDefault(p => string.Equals(p.IdentificationDetails_IdentificationNumber, selectedPersonRegistry.IdentificationNumber, StringComparison.OrdinalIgnoreCase) && string.Equals(p.IdentificationDetails_CountryOfIssue.Value.ToString(), selectedPersonRegistry.IssuingCountry, StringComparison.OrdinalIgnoreCase) && string.Equals(p.IdentificationDetails_TypeOfIdentification, selectedPersonRegistry.TypeofIdentification, StringComparison.OrdinalIgnoreCase));
                                        personDetilsRegitryId = selectedPersonRegistry.PersonsRegistryID;
                                    }
                                }
                                if (applicantIdentifications != null)
                                {
                                    if (personDetilsRegitryId == 0 && applicantIdentifications.Any(k => string.Equals(k.IdentificationDetails_TypeOfIdentificationName, "ID", StringComparison.OrdinalIgnoreCase) || string.Equals(k.IdentificationDetails_TypeOfIdentificationName, "PASSPORT", StringComparison.OrdinalIgnoreCase)))
                                    {
                                        selectedIdentification = applicantIdentifications.FirstOrDefault(k => string.Equals(k.IdentificationDetails_TypeOfIdentificationName, "ID", StringComparison.OrdinalIgnoreCase) || string.Equals(k.IdentificationDetails_TypeOfIdentificationName, "PASSPORT", StringComparison.OrdinalIgnoreCase));

                                    }
                                }
                                //model.PersonalDetails.PersonRegistryId = personDetilsRegitryId;

                                if (true) //selectedIdentification != null
                            {
                                    var personRegistry = PersonalDetailsProcess.SavePersonRegistry(User.Identity.Name, userModel.IntroducerUser.Introducer.IntermediaryName, registriesRepository, model.PersonalDetails, model.ContactDetails, selectedIdentification);
                                    if (personRegistry != null)
                                    {
                                        model.PersonalDetails.PersonRegistryId = personRegistry.PersonsRegistryID;
                                        model.PersonalDetails = PersonalDetailsProcess.SaveRelatedPartyPersonalDetailsModel(model.ApplicationNumber, model.PersonalDetails);
                                    }
                                }

                            }

                        }
                        else if (PersonalDetailsSaveInRegistry && model.PersonalDetails != null)
                        {
                            if (userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null)
                            {
                                IdentificationDetailsViewModel selectedIdentification = null;
                                var personRegistry = PersonalDetailsProcess.SavePersonRegistry(User.Identity.Name, userModel.IntroducerUser.Introducer.IntermediaryName, registriesRepository, model.PersonalDetails, model.ContactDetails, selectedIdentification);
                                if (personRegistry != null)
                                {
                                    model.PersonalDetails.PersonRegistryId = personRegistry.PersonsRegistryID;
                                    model.PersonalDetails = PersonalDetailsProcess.SaveRelatedPartyPersonalDetailsModel(model.ApplicationNumber, model.PersonalDetails);
                                }
                            }
                        }
                        relatedPartyID = model.PersonalDetails.Id;
                        relatedPartyNodeGUID = model.PersonalDetails.NodeGUID;
                   // }
                    if (!isLegalEntity)
                    {
                        //Contcat Details SAVE and UPDATE for Individual
                        if (model.ContactDetails != null)
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
                            }
                        }
                        //Save Identification Details if retrive from registry on first time creation of applicant

                        if (!string.IsNullOrEmpty(model.hdnCitizenship) && model.hdnCitizenship != "0")
                        {
                            TreeNode treeNode = ServiceHelper.GetTreeNode(model.PersonalDetails.NodeGUID, model.PersonalDetails.NodePath);
                            if (treeNode != null)
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
                        if (!isLegalEntity && string.Equals(model.ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                        {
                            //Related Party type=Individual and Application Type=Legal Entity
                            if (model.PartyRolesLegal != null)
                            {
                                if (model.PersonalDetails.IsRelatedPartyUBO)
                                {
                                    model.PartyRolesLegal.RelatedPartyRoles_IsUltimateBeneficiaryOwner = true;
                                    // model.PartyRolesLegal.RelatedPartyRoles_IsBenificiary = true;
                                }
                                var applicatioData = personalDetailsRepository.GetPersonalDetailsByID(model.PersonalDetails.Id);
                                var partyRolesTreeNode = partyRolesLegalRepository.GetRelatedPartyRolesLegal(applicatioData.NodeAliasPath);
                                if (model.PartyRolesLegal.RelatedPartyRolesLegalID > 0)
                                {
                                    model.PartyRolesLegal = RelatedPartyRolesLegalProcess.UpdatePartyRolesLegal(model.PartyRolesLegal, partyRolesTreeNode.FirstOrDefault());
                                }
                                else
                                {
                                    model.PartyRolesLegal = RelatedPartyRolesLegalProcess.SavePartyRolesLegal(model.PartyRolesLegal, applicatioData);
                                }

                            }
                            else if (model.PersonalDetails.IsRelatedPartyUBO && model.PartyRolesLegal == null)
                            {
                                var applicatioData = personalDetailsRepository.GetPersonalDetailsByID(model.PersonalDetails.Id);
                                model.PartyRolesLegal = RelatedPartyRolesLegalProcess.SavePartyRolesLegalForIsUBO(applicatioData);
                            }
                        }
                        else
                        {
                            //Party Roles SAVE and UPDATE for Individual
                            if (model.PartyRoles != null)
                            {
                                var applicatioData = personalDetailsRepository.GetPersonalDetailsByID(model.PersonalDetails.Id);
                                var partyRolesTreeNode = partyRolesRepository.GetRelatedPartyRoles(applicatioData.NodeAliasPath);
                                if (model.PartyRoles.RelatedPartyRolesID > 0)
                                {

                                    model.PartyRoles = RelatedPartyRolesProcess.UpdatePartyRoles(model.PartyRoles, partyRolesTreeNode.FirstOrDefault());
                                }
                                else
                                {
                                    model.PartyRoles = RelatedPartyRolesProcess.SavePartyRoles(model.PartyRoles, applicatioData);
                                }

                            }
                        }

                    }

                }
                if (model.CompanyDetails != null && isLegalEntity)
                {
                    if (!string.IsNullOrEmpty(model.CompanyDetails.RegisteredName) || model.CompanyDetails.Id > 0)
                    {
                        model.CompanyDetails.Type = CommonProcess.GetApplicationValue(model.Type);
                        if (!string.IsNullOrEmpty(model.CompanyDetails.HdnDateofIncorporation))
                        {
                            model.CompanyDetails.DateofIncorporation = DateTime.ParseExact(model.CompanyDetails.HdnDateofIncorporation, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            model.CompanyDetails.DateofIncorporation = null;
                        }
                        if (string.Equals(relatedPartyButton, "SAVECLOSE", StringComparison.OrdinalIgnoreCase))
                        {
                            //Do Validation Here
                            //If Validate 
                            applicantValidationResult = RelatedPartyLegalValidationProcess.ValidateRelatedPartyLegal(model);
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
                        model.CompanyDetails = CompanyDetailsRelatedPartyProcess.SaveRelatedPartyCompanyDetailsRelatedPartyModel(model.ApplicationNumber, model.CompanyDetails);
                        relatedPartyID = model.CompanyDetails.Id;
                        relatedPartyNodeGUID = model.CompanyDetails.NodeGUID;
                        if (SaveInRegistry)
                        {
                            int companyRegitryId = 0;
                            var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);
                            var companyRegistries = registriesRepository.GetCompanyRegistries(UserRegistry.NodeAliasPath);
                            if (companyRegistries != null && companyRegistries.Any(h => string.Equals(h.CompanyDetails_RegistrationNumber, model.CompanyDetails.RegistrationNumber) && string.Equals(h.CompanyDetails_CountryofIncorporation, model.CompanyDetails.CountryofIncorporation) && h.CompanyDetails_DateofIncorporation == model.CompanyDetails.DateofIncorporation))
                            {
                                companyRegitryId = companyRegistries.FirstOrDefault(h => string.Equals(h.CompanyDetails_RegistrationNumber, model.CompanyDetails.RegistrationNumber) && string.Equals(h.CompanyDetails_CountryofIncorporation, model.CompanyDetails.CountryofIncorporation) && h.CompanyDetails_DateofIncorporation == model.CompanyDetails.DateofIncorporation).CompanyRegistryID;

                            }

                            model.CompanyDetails.Application_Type = model.ApplicationType;
                            model.CompanyDetails.RegistryId = companyRegitryId;
                            var companyRegistry = CompanyDetailsRelatedPartyProcess.SaveCompanyRegistryRelatedParty(User.Identity.Name, registriesRepository, model.CompanyDetails);
                            if (companyRegistry != null)
                            {
                                model.CompanyDetails.RegistryId = companyRegistry.CompanyRegistryID;
                                model.CompanyDetails = CompanyDetailsRelatedPartyProcess.SaveRelatedPartyCompanyDetailsRelatedPartyModel(model.ApplicationNumber, model.CompanyDetails);
                            }

                        }
                        if (isLegalEntity)
                        {
                            //Contcat Details SAVE and UPDATE for Legal
                            if (model.ContactDetails != null && model.ContactDetails.ContactDetails_PreferredCommunicationLanguage != null)
                            {
                                var companyDetailsApplicationData = companyDetailsRelatedPartyRepository.GetCompanyDetailsRelatedParty(model.CompanyDetails.Id);
                                string Path = companyDetailsApplicationData.NodeAliasPath;
                                var contactDetailsTreeNode = contactDetailsRepository.GetContactDetails(Path);
                                if (model.ContactDetails.ContactDetailsID > 0)
                                {
                                    model.ContactDetails = ContactDetailsProcess.UpdateContactDetails(model.ContactDetails, contactDetailsTreeNode.FirstOrDefault());
                                }
                                else
                                {
                                    model.ContactDetails = ContactDetailsProcess.SaveContactDetails(model.ContactDetails, companyDetailsApplicationData);
                                }
                            }
                            //Party Roles SAVE and UPDATE for Legal


                            if (model.PartyRolesLegal != null)
                            {
								//isAuthorizeCardHolder = model.PartyRolesLegal.RelatedPartyRoles_IsAuthorisedCardholder;
								if (model.CompanyDetails.IsRelatedPartyUBO)
                                {
                                    model.PartyRolesLegal.RelatedPartyRoles_IsUltimateBeneficiaryOwner = true;
                                    //model.PartyRolesLegal.RelatedPartyRoles_IsBenificiary = true;
                                }
                                var applicatioData = companyDetailsRelatedPartyRepository.GetCompanyDetailsRelatedParty(model.CompanyDetails.Id);
                                var partyRolesTreeNode = partyRolesLegalRepository.GetRelatedPartyRolesLegal(applicatioData.NodeAliasPath);
                                if (model.PartyRolesLegal.RelatedPartyRolesLegalID > 0)
                                {
                                    model.PartyRolesLegal = RelatedPartyRolesLegalProcess.UpdatePartyRolesLegal(model.PartyRolesLegal, partyRolesTreeNode.FirstOrDefault());
                                }
                                else
                                {
                                    model.PartyRolesLegal = RelatedPartyRolesLegalProcess.SavePartyRolesLegal(model.PartyRolesLegal, applicatioData);
                                }
                            }
                            else if (model.CompanyDetails.IsRelatedPartyUBO && model.PartyRolesLegal == null)
                            {
                                var applicatioData = companyDetailsRelatedPartyRepository.GetCompanyDetailsRelatedParty(model.CompanyDetails.Id);
                                model.PartyRolesLegal = RelatedPartyRolesLegalProcess.SavePartyRolesLegalForIsUBO(applicatioData);
                            }
                        }
                    }
                    else
                    {
                        List<ValidationResultModel> companyValidations = new List<ValidationResultModel>();
                        ValidationResultModel companyValidation = new ValidationResultModel()
                        {
                            IsValid = false,
                            ApplicationModuleName = ApplicationModule.LEGAL_ENTITY_DETAILS,
                            Errors = new List<ValidationError>() { new ValidationError() {
                        ErrorMessage = ValidationConstant.CompanyDetails_RegisteredName,
                        PropertyName = "RegisteredName"
                    } }
                        };
                        companyValidations.Add(companyValidation);
                        TempData["ErrorSummary"] = JsonConvert.SerializeObject(companyValidations);
                        return RedirectToAction("Index", "RelatedParty", new { application = model.Application_NodeGUID, relatedParty = relatedPartyNodeGUID, type = model.Type });
                    }

                }


                if (relatedPartyID > 0)
                {
                    if (string.Equals(relatedPartyButton, "SAVECLOSE", StringComparison.OrdinalIgnoreCase))
                    {
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
                            return RedirectToAction("Index", "RelatedParty", new { application = model.Application_NodeGUID, relatedParty = relatedPartyNodeGUID, type = model.Type });
                        }
                        else
                        {
                            //Generate Bank Document
                            DocumentsProcess.GenerateBankDocuments(applicationId, bankDocumentsRepository, applicationsRepository, model.ApplicationNumber, model.ApplicationType, personNodeGUID);
                            //Generate Expected Document
                            DocumentsProcess.GenerateExpectedDouments(applicationId, expectedDocumentsRepository, applicationsRepository, model.ApplicationNumber, model.ApplicationType, personNodeGUID);
                            //Generate Card Details if roleCardholder is true
                            if (model.PersonalDetails != null) // && isLegalEntity
							{
                                DebitCardeDetailsProcess.GenerateDebitCardDetails(isAuthorizeCardHolder, applicationID, debitCardDetailsRepository, applicationsRepository, "", "", ServiceHelper.GetCardType().FirstOrDefault().Value, model.PersonalDetails.NodeGUID, model.PersonalDetails.FirstName + " " + model.PersonalDetails.LastName, isLegalEntity); 
                            }
                            if (!string.Equals(model.ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                            {
                                var relatedPartySeletdRolesIndi = RelatedPartyRolesProcess.GetPartyRolesDetailsByApplicantId(relatedPartyID);
                                if (relatedPartySeletdRolesIndi != null && relatedPartySeletdRolesIndi.RelatedPartyRoles_IsEBankingUser)
                                {
                                    EBankingSubscriberDetailsProcess.SaveAutoEBankingSubscriberIndividual(model.ApplicationNumber, model.PersonalDetails.Id, false, isLegalEntity);
                                }
                            }
                            else
                            {
                                if (isLegalEntity)
                                {
                                    //var relatedPartySelectedRolesLegal = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalByApplicantId(relatedPartyID);
                                    //if(relatedPartySelectedRolesLegal != null && (relatedPartySelectedRolesLegal.RelatedPartyRoles_IsDesignatedEBankingUser || relatedPartySelectedRolesLegal.RelatedPartyRoles_IsAuthorisedPerson))
                                    //{
                                    //    EBankingSubscriberDetailsProcess.SaveAutoEBankingSubscriberLegal(model.ApplicationNumber, model.CompanyDetails.Id);
                                    //}
                                }
                                else
                                {
                                    var relatedPartySelectedRolesIndividual = RelatedPartyRolesLegalProcess.GetPartyRolesDetailsLegalForIndividualById(relatedPartyID);
                                    if (relatedPartySelectedRolesIndividual != null && (relatedPartySelectedRolesIndividual.RelatedPartyRoles_IsDesignatedEBankingUser || relatedPartySelectedRolesIndividual.RelatedPartyRoles_IsAuthorisedPerson))
                                    {
                                        EBankingSubscriberDetailsProcess.SaveAutoEBankingSubscriberIndividual(model.ApplicationNumber, model.PersonalDetails.Id, false, isLegalEntity);
                                    }
                                }
                            }

                            return RedirectToAction("Edit", "Applications", new { application = model.Application_NodeGUID });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "RelatedParty", new { application = model.Application_NodeGUID, relatedParty = relatedPartyNodeGUID, type = model.Type });
                    }

                }
                else
                {
                    return RedirectToAction("Edit", "Applications", new { application = model.Application_NodeGUID });
                }
            }
            return View(model);
        }

        #region Address

        public IActionResult AddressDetails_Read([DataSourceRequest] DataSourceRequest request, int apID, string applicationType)
        {
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            int applicantId = ValidationHelper.GetInteger(apID, 0);

            List<AddressDetailsModel> addressDetailsViewModels = new List<AddressDetailsModel>();

            if (applicantId > 0)
            {
                if (isLegalEntity)
                {
                    var applicationDetails = CompanyDetailsRelatedPartyProcess.GetCompanyDetailsRelatedPartyModelById(applicantId);

                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        addressDetailsViewModels = AddressDetailsProcess.GetRelatedPartyAddressDetailsLegal(applicantId);
                    }
                }
                else
                {
                    var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicantId);

                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        addressDetailsViewModels = AddressDetailsProcess.GetRelatedPartyAddressDetails(applicantId);
                    }
                }

            }

            if (addressDetailsViewModels == null)
            {
                return Json("");
            }

            return Json(addressDetailsViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult AddressDetailsPopup_Create([DataSourceRequest] DataSourceRequest request, AddressDetailsModel addressDetailsModel, int apID, string applicationType)
        {
            UserModel userModel = UserProcess.GetUser(User.Identity.Name);
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            addressDetailsModel.Is_Legal = isLegalEntity;
            string Location = addressDetailsModel.LocationName;
            if (isLegalEntity)
            {
                if (addressDetailsModel.Status)
                {
                    ValidationResultModel validationResultModel = new ValidationResultModel();
                    validationResultModel = RelatedPartyLegalFormBasicValidationProcess.ValidateAddresstDetails(addressDetailsModel);
                    if (validationResultModel != null)
                    {
                        foreach (var item in validationResultModel.Errors)
                        {
                            ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                        }
                    }
                }
                int applicantId = ValidationHelper.GetInteger(apID, 0);
                if (addressDetailsModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(addressDetailsModel))
                {
                    var applicationDetails = CompanyDetailsRelatedPartyProcess.GetCompanyDetailsRelatedPartyModelById(applicantId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        var addressDetails = AddressDetailsProcess.SaveRelatedPartyAddressDetailsLegalModel(applicationDetails.Id, addressDetailsModel);
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
                                    addressDetailsModel = AddressDetailsProcess.SaveRelatedPartyAddressDetailsLegalModel(applicationDetails.Id, addressDetailsModel);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (addressDetailsModel.Status)
                {
                    ValidationResultModel validationResultModel = new ValidationResultModel();
                    validationResultModel = RelatedPartyIndividualFormBasicValidationProcess.ValidateAddresstDetails(addressDetailsModel);
                    if (validationResultModel != null)
                    {
                        foreach (var item in validationResultModel.Errors)
                        {
                            ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                        }
                    }
                }
                int applicantId = ValidationHelper.GetInteger(apID, 0);
                if (addressDetailsModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(addressDetailsModel))
                {
                    var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicantId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        var addressDetails = AddressDetailsProcess.SaveRelatedPartyAddressDetailsModel(applicationDetails.Id, addressDetailsModel);
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
                                    addressDetailsModel = AddressDetailsProcess.SaveRelatedPartyAddressDetailsModel(applicationDetails.Id, addressDetailsModel);
                                }
                            }
                        }
                    }
                }
            }
            return Json(new[] { addressDetailsModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult AddressDetailsPopup_Update([DataSourceRequest] DataSourceRequest request, AddressDetailsModel addressDetailsModel, string applicationType)
        {
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            UserModel userModel = UserProcess.GetUser(User.Identity.Name);
            string Location = addressDetailsModel.LocationName;
            if (addressDetailsModel.Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                if (isLegalEntity)
                {
                    validationResultModel = RelatedPartyLegalFormBasicValidationProcess.ValidateAddresstDetails(addressDetailsModel);
                }
                else
                {
                    validationResultModel = RelatedPartyIndividualFormBasicValidationProcess.ValidateAddresstDetails(addressDetailsModel);
                }

                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (addressDetailsModel != null && ModelState.IsValid)
            {
                var addressDetailsData = addressDetailsRepository.GetAddressDetails(addressDetailsModel.Id);
                if (addressDetailsData != null)
                {
                    if (isLegalEntity)
                    {
                        var addressDetails = AddressDetailsProcess.SaveRelatedPartyAddressDetailsLegalModel(0, addressDetailsModel);
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
                                    addressDetailsModel = AddressDetailsProcess.SaveRelatedPartyAddressDetailsLegalModel(0, addressDetailsModel);
                                }
                            }
                        }
                    }
                    else
                    {
                        var addressDetails = AddressDetailsProcess.SaveRelatedPartyAddressDetailsModel(0, addressDetailsModel);
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
                                    addressDetailsModel = AddressDetailsProcess.SaveRelatedPartyAddressDetailsModel(0, addressDetailsModel);
                                }
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

        #endregion

        #region PEP Apllicant Details

        public IActionResult PEPApplicant_Read([DataSourceRequest] DataSourceRequest request, int id, bool isLegal = false)
        {
            int applicantID = ValidationHelper.GetInteger(id, 0);
            List<PepApplicantViewModel> pepApplicantViewModels = new List<PepApplicantViewModel>();
            IEnumerable<CMS.DocumentEngine.Types.Eurobank.PepApplicant> pepApplicants = null;
            if (isLegal)
            {
                //pepApplicants = pepApplicantRepository.GetPepApplicantsLegal(applicantID);
                pepApplicants = pepApplicantRepository.GetPepApplicantsLegalRealtedParty(applicantID);
            }
            else
            {
                pepApplicants = pepApplicantRepository.GetPepApplicants(applicantID);
            }

            if (pepApplicants != null)
            {
                pepApplicantViewModels = PEPDetailsProcess.GetPepApplicants(pepApplicants);
            }
            return Json(pepApplicantViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult PEPApplicantPopup_Create([DataSourceRequest] DataSourceRequest request, PepApplicantViewModel pepApplicantViewModel, int id, bool isLegal = false)
        {
            if (pepApplicantViewModel.Status)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = RelatedPartyIndividualFormBasicValidationProcess.ValidatePepDetailsApplicant(pepApplicantViewModel);
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
                if (isLegal)
                {
                    //var applicatioData = taxDetailsRepository.GetCompanyDetailsLegalByID(applicatID);
                    var applicatioData = companyDetailsRelatedPartyRepository.GetCompanyDetailsRelatedParty(applicatID);
                    pepApplicantViewModel = PEPDetailsProcess.SavePepApplicantModel(pepApplicantViewModel, applicatioData);

                }
                else
                {
                    var applicatioData = taxDetailsRepository.GetPersonalDetailsByID(applicatID);
                    pepApplicantViewModel = PEPDetailsProcess.SavePepApplicantModel(pepApplicantViewModel, applicatioData);

                }


            }
            return Json(new[] { pepApplicantViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult PEPApplicantPopup_Update([DataSourceRequest] DataSourceRequest request, PepApplicantViewModel pepApplicantViewModel, int id)
        {
            if (pepApplicantViewModel.Status)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = RelatedPartyIndividualFormBasicValidationProcess.ValidatePepDetailsApplicant(pepApplicantViewModel);
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
            int applicantID = ValidationHelper.GetInteger(id, 0);
            PepApplicantViewModel PepApplicantViewModel = new PepApplicantViewModel();
            var pepApplicants = pepApplicantRepository.GetPepApplicants(applicantID);
            if (pepApplicants != null)
            {
                foreach (var item in pepApplicants)
                {
                    item.DeleteAllCultures();
                }
            }
            return Json(new[] { PepApplicantViewModel }.ToDataSourceResult(request, ModelState));
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

        public IActionResult PEPAssociates_Read([DataSourceRequest] DataSourceRequest request, int id, bool isLegal = false)
        {
            int applicantID = ValidationHelper.GetInteger(id, 0);
            List<PepAssociatesViewModel> pepAssociatesViewModels = new List<PepAssociatesViewModel>();
            IEnumerable<CMS.DocumentEngine.Types.Eurobank.PepAssociates> pepAssociates = null;
            if (isLegal)
            {
                //pepAssociates = pepAssociatesRepository.GetPepAssociatesLegal(applicantID);
                pepAssociates = pepAssociatesRepository.GetPepAssociatesLegalRelatedParty(applicantID);
                if (pepAssociates != null)
                {
                    pepAssociatesViewModels = PEPDetailsProcess.GetPepAssociates(pepAssociates);
                }
            }
            else
            {
                pepAssociates = pepAssociatesRepository.GetPepAssociates(applicantID);
                if (pepAssociates != null)
                {
                    pepAssociatesViewModels = PEPDetailsProcess.GetPepAssociates(pepAssociates);
                }
            }

            return Json(pepAssociatesViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult PEPAssociatesPopup_Create([DataSourceRequest] DataSourceRequest request, PepAssociatesViewModel pepAssociatesViewModel, int id, bool isLegal = false)
        {

            int applicatID = ValidationHelper.GetInteger(id, 0);
            if (pepAssociatesViewModel.Status)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = RelatedPartyIndividualFormBasicValidationProcess.ValidatePepDetailsFamilyAssociate(pepAssociatesViewModel);
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
                if (isLegal)
                {

                    //var applicatioData = taxDetailsRepository.GetCompanyDetailsLegalByID(applicatID);
                    var applicatioData = companyDetailsRelatedPartyRepository.GetCompanyDetailsRelatedParty(applicatID);
                    pepAssociatesViewModel = PEPDetailsProcess.SavePepAssociatesModel(pepAssociatesViewModel, applicatioData);

                }
                else
                {
                    var applicatioData = taxDetailsRepository.GetPersonalDetailsByID(applicatID);
                    pepAssociatesViewModel = PEPDetailsProcess.SavePepAssociatesModel(pepAssociatesViewModel, applicatioData);

                }


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
                validationResultModel = RelatedPartyIndividualFormBasicValidationProcess.ValidatePepDetailsFamilyAssociate(pepAssociatesViewModel);
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

        #region-----------------Contact Details----------------------
        [HttpPost]
        //public ActionResult ContactDetails(RelatedPartyModel model)
        //{

        //    var applicatioData = personalDetailsRepository.GetPersonalDetailsByID(model.PersonalDetails.Id);

        //    if (model.ContactDetails.ContactDetailsID > 0)
        //    {
        //        string Path = applicatioData.NodeAliasPath;
        //        var contactDetailsTreeNode = contactDetailsRepository.GetContactDetails(Path);
        //        model.ContactDetails = ContactDetailsProcess.UpdateContactDetails(model.ContactDetails, contactDetailsTreeNode.FirstOrDefault());
        //    }
        //    else
        //    {
        //        model.ContactDetails = ContactDetailsProcess.SaveContactDetails(model.ContactDetails, applicatioData);
        //    }
        //    return RedirectToAction("Index", "RelatedParty", new { relatedPartyId = model.PersonalDetails.Id });
        //}
        public ActionResult ContactDetails(RelatedPartyModel model)
        {
            bool isLegalEntity = string.Equals(model.Type, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);

            if (isLegalEntity)
            {
                var companyDetailsApplicationData = companyDetailsRelatedPartyRepository.GetCompanyDetailsRelatedParty(model.CompanyDetails.Id);
                string Path = companyDetailsApplicationData.NodeAliasPath;
                var contactDetailsTreeNode = contactDetailsRepository.GetContactDetails(Path);
                if (model.ContactDetails.ContactDetailsID > 0)
                {
                    model.ContactDetails = ContactDetailsProcess.UpdateContactDetails(model.ContactDetails, contactDetailsTreeNode.FirstOrDefault());
                }
                else
                {
                    model.ContactDetails = ContactDetailsProcess.SaveContactDetails(model.ContactDetails, companyDetailsApplicationData);
                    contactDetailsTreeNode = contactDetailsRepository.GetContactDetails(Path);
                }
                model.ContactDetails = ContactDetailsProcess.GetContactDetailsById(contactDetailsTreeNode.FirstOrDefault());
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
        public JsonResult PreferredCommunicationLanguage_Read()
        {
            var Language = ServiceHelper.GetCommunicationLanguage();
            return Json(Language);
        }
        #endregion

        #region----------Party Roles------------------------------
        [HttpPost]
        public ActionResult PartyRoles(RelatedPartyModel model)
        {
            var applicatioData = personalDetailsRepository.GetPersonalDetailsByID(model.PersonalDetails.Id);
            var partyRolesTreeNode = partyRolesRepository.GetRelatedPartyRoles(applicatioData.NodeAliasPath);
            if (model.PartyRoles.RelatedPartyRolesID > 0)
            {

                model.PartyRoles = RelatedPartyRolesProcess.UpdatePartyRoles(model.PartyRoles, partyRolesTreeNode.FirstOrDefault());
            }
            else
            {
                model.PartyRoles = RelatedPartyRolesProcess.SavePartyRoles(model.PartyRoles, applicatioData);
                partyRolesTreeNode = partyRolesRepository.GetRelatedPartyRoles(applicatioData.NodeAliasPath);
            }
            model.PartyRoles = RelatedPartyRolesProcess.GetPartyRolesById(partyRolesTreeNode.FirstOrDefault());
            return PartialView("_PartyRoles", model);
        }

        #endregion

        #region------------------Party Roles Legal---------------
        //[HttpPost]
        //public ActionResult PartyRolesLegal(RelatedPartyModel model)
        //{
        //    var applicatioData = companyDetailsRelatedPartyRepository.GetCompanyDetailsRelatedParty(model.CompanyDetails.Id);
        //    var partyRolesTreeNode = partyRolesLegalRepository.GetRelatedPartyRolesLegal(applicatioData.NodeAliasPath);
        //    if (model.PartyRolesLegal.RelatedPartyRolesLegalID > 0)
        //    {

        //        model.PartyRolesLegal = RelatedPartyRolesLegalProcess.UpdatePartyRolesLegal(model.PartyRolesLegal, partyRolesTreeNode.FirstOrDefault());
        //    }
        //    else
        //    {
        //        model.PartyRolesLegal = RelatedPartyRolesLegalProcess.SavePartyRolesLegal(model.PartyRolesLegal, applicatioData);
        //        partyRolesTreeNode = partyRolesLegalRepository.GetRelatedPartyRolesLegal(applicatioData.NodeAliasPath);
        //    }
        //    model.PartyRolesLegal = RelatedPartyRolesLegalProcess.GetPartyRolesLegalById(partyRolesTreeNode.FirstOrDefault());
        //    return PartialView("_PartyRolesLegal", model);
        //}
        #endregion

        #region Signatory Group

        public IActionResult SignatoryGroup_Read([DataSourceRequest] DataSourceRequest request, int apID)
        {
            int applicantId = ValidationHelper.GetInteger(apID, 0);
            List<SignatoryGroupModel> signatoryGroupViewModels = new List<SignatoryGroupModel>();

            if (applicantId > 0)
            {
                var applicationDetails = CompanyDetailsRelatedPartyProcess.GetCompanyDetailsRelatedPartyModelById(applicantId);

                if (applicationDetails != null && applicationDetails.Id > 0)
                {
                    signatoryGroupViewModels = SignatoryGroupProcess.GetRelatedPartyLegalSignatoryGroup(applicantId);
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

            int realtedPartyId = ValidationHelper.GetInteger(apID, 0);
            if (signatoryGroupModel != null && ModelState.IsValid)
            {
                var applicationDetails = CompanyDetailsRelatedPartyProcess.GetCompanyDetailsRelatedPartyModelById(realtedPartyId);

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
                    var signatoryGroup = SignatoryGroupProcess.SaveRelatedPartyLegalSignatoryGroupModel(applicationDetails.Id, signatoryGroupModel);
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
                    var signatoryGroups = SignatoryGroupProcess.SaveRelatedPartyLegalSignatoryGroupModel(0, signatoryGroupModel);
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

        #region Source Of Annual Income

        public IActionResult SourceOfIncome_Read([DataSourceRequest] DataSourceRequest request, int apID, string applicationType)
        {
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            int relatedPartyId = ValidationHelper.GetInteger(apID, 0);
            List<SourceOfIncomeModel> sourceOfIncomeViewModels = new List<SourceOfIncomeModel>();

            if (relatedPartyId > 0)
            {
                if (isLegalEntity)
                {
                    var applicationDetails = CompanyDetailsRelatedPartyProcess.GetCompanyDetailsRelatedPartyModelById(relatedPartyId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        sourceOfIncomeViewModels = SourceOfIncomeProcess.GetSourceOfIncomeRelatedPartyLegal(relatedPartyId);
                    }
                }
                else
                {
                    var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(relatedPartyId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        sourceOfIncomeViewModels = SourceOfIncomeProcess.GetSourceOfIncomeRelatedParty(relatedPartyId);
                    }
                }
            }

            if (sourceOfIncomeViewModels == null)
                return Json("");

            return Json(sourceOfIncomeViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult SourceOfIncomePopup_Create([DataSourceRequest] DataSourceRequest request, SourceOfIncomeModel sourceOfIncomeModel, int apID, string applicationType)
        {
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            if (sourceOfIncomeModel.Status)
            {
                var existData = SourceOfIncomeProcess.GetSourceOfIncomeRelatedParty(apID);
                if (existData != null)
                {
                    string selectedSourceOfAnnualIncome = string.Empty;
                    var sourceOfAnnualIncomeList = ServiceHelper.GetSourcesOfAnnualIncome();
                    if (sourceOfAnnualIncomeList != null && sourceOfAnnualIncomeList.Count > 0 && sourceOfAnnualIncomeList.Any(k => k.Value == sourceOfIncomeModel.SourceOfAnnualIncome))
                    {
                        selectedSourceOfAnnualIncome = sourceOfAnnualIncomeList.FirstOrDefault(k => k.Value == sourceOfIncomeModel.SourceOfAnnualIncome).Text;
                    }
                    bool ifExist = existData.Any(x => x.SourceOfAnnualIncome == sourceOfIncomeModel.SourceOfAnnualIncome);
                    if (ifExist && !string.Equals(selectedSourceOfAnnualIncome, "OTHER", StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError("SourceOfAnnualIncome", "Source of income already exists.");
                    }
                }

                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = RelatedPartyIndividualFormBasicValidationProcess.ValidateSourceOfIncome(sourceOfIncomeModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }

            int relatedPartyId = ValidationHelper.GetInteger(apID, 0);
            if (sourceOfIncomeModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(sourceOfIncomeModel))
            {
                if (isLegalEntity)
                {
                    var applicationDetails = CompanyDetailsRelatedPartyProcess.GetCompanyDetailsRelatedPartyModelById(relatedPartyId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        var sourceOfIncome = SourceOfIncomeProcess.SaveSourceOfIncomeRelatedPartyLegalModel(applicationDetails.Id, sourceOfIncomeModel);
                        if (sourceOfIncome != null)
                            sourceOfIncomeModel = sourceOfIncome;
                    }
                }
                else
                {
                    var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(relatedPartyId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        var sourceOfIncome = SourceOfIncomeProcess.SaveSourceOfIncomeRelatedPartyModel(applicationDetails.Id, sourceOfIncomeModel);
                        if (sourceOfIncome != null)
                            sourceOfIncomeModel = sourceOfIncome;
                    }
                }

            }

            return Json(new[] { sourceOfIncomeModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult SourceOfIncomePopup_Update([DataSourceRequest] DataSourceRequest request, SourceOfIncomeModel sourceOfIncomeModel, int apID, string applicationType)
        {
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            if (sourceOfIncomeModel.Status)
            {
                var existData = SourceOfIncomeProcess.GetSourceOfIncomeRelatedParty(apID);
                if (existData != null)
                {
                    string selectedSourceOfAnnualIncome = string.Empty;
                    var sourceOfAnnualIncomeList = ServiceHelper.GetSourcesOfAnnualIncome();
                    if (sourceOfAnnualIncomeList != null && sourceOfAnnualIncomeList.Count > 0 && sourceOfAnnualIncomeList.Any(k => k.Value == sourceOfIncomeModel.SourceOfAnnualIncome))
                    {
                        selectedSourceOfAnnualIncome = sourceOfAnnualIncomeList.FirstOrDefault(k => k.Value == sourceOfIncomeModel.SourceOfAnnualIncome).Text;
                    }
                    bool ifExist = existData.Where(x => x.Id != sourceOfIncomeModel.Id).Any(x => x.SourceOfAnnualIncome == sourceOfIncomeModel.SourceOfAnnualIncome);
                    if (ifExist && !string.Equals(selectedSourceOfAnnualIncome, "OTHER", StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError("SourceOfAnnualIncome", "Source of income already exists.");
                    }
                }
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = RelatedPartyIndividualFormBasicValidationProcess.ValidateSourceOfIncome(sourceOfIncomeModel);
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
                if (isLegalEntity)
                {
                    var sourceOfIncomeData = sourceOfIncomeRepository.GetSourceOfIncome(sourceOfIncomeModel.Id);
                    if (sourceOfIncomeData != null)
                    {
                        var sourceOfIncome = SourceOfIncomeProcess.SaveSourceOfIncomeRelatedPartyLegalModel(0, sourceOfIncomeModel);
                        if (sourceOfIncome != null)
                            sourceOfIncomeModel = sourceOfIncome;
                    }
                }
                else
                {
                    var sourceOfIncomeData = sourceOfIncomeRepository.GetSourceOfIncome(sourceOfIncomeModel.Id);
                    if (sourceOfIncomeData != null)
                    {
                        var sourceOfIncome = SourceOfIncomeProcess.SaveSourceOfIncomeRelatedPartyModel(0, sourceOfIncomeModel);
                        if (sourceOfIncome != null)
                            sourceOfIncomeModel = sourceOfIncome;
                    }
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

        #region-----------Origin of Total Assests-------------
        public IActionResult OriginOfTotalAssets_Read([DataSourceRequest] DataSourceRequest request, int apID, string applicationType)
        {
            bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
            int applicantId = ValidationHelper.GetInteger(apID, 0);
            List<OriginOfTotalAssetsModel> originOfTotalAssetsViewModels = new List<OriginOfTotalAssetsModel>();

            if (applicantId > 0)
            {
                if (isLegalEntity)
                {
                    var applicationDetails = CompanyDetailsRelatedPartyProcess.GetCompanyDetailsRelatedPartyModelById(applicantId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        originOfTotalAssetsViewModels = OriginOfTotalAssetsProcess.GetOriginOfTotalAssetsLegalRelatedParty(applicantId);
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
                if (string.IsNullOrEmpty(originOfTotalAssetsModel.OriginOfTotalAssets))
                {
                    ModelState.AddModelError("OriginOfTotalAssets", ResHelper.GetString(OriginOfTotalAssetsErrorMassage.OriginOfTotalAssets));
                }
                if (originOfTotalAssetsModel.AmountOfTotalAsset == null)
                {
                    ModelState.AddModelError("AmountOfTotalAsset", ResHelper.GetString(OriginOfTotalAssetsErrorMassage.AmountOfTotalAsset));

                }
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
                    var applicationDetails = CompanyDetailsRelatedPartyProcess.GetCompanyDetailsRelatedPartyModelById(applicantId);
                    if (applicationDetails != null && applicationDetails.Id > 0)
                    {
                        var sourceOfIncome = OriginOfTotalAssetsProcess.SaveOriginOfTotalAssetsModelLegalRelatedParty(applicationDetails.Id, originOfTotalAssetsModel);
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
                if (originOfTotalAssetsModel.AmountOfTotalAsset == null)
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
                        var originOfTotalAssets = OriginOfTotalAssetsProcess.SaveOriginOfTotalAssetsModelLegalRelatedParty(0, originOfTotalAssetsModel);
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

        #region Validation

        [HttpPost]
        public JsonResult ValidateCompanyDetails(RelatedPartyModel relatedPartyModel)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };
            if (relatedPartyModel != null && relatedPartyModel.CompanyDetails != null)
            {
                //Need to implement compnay details validation
                result = RelatedPartyLegalFormBasicValidationProcess.ValidateCompanyDetails(relatedPartyModel.CompanyDetails);
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateContactDetailsIndividual(RelatedPartyModel relatedPartyModel)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (relatedPartyModel != null && relatedPartyModel.ContactDetails != null)
            {
                result = RelatedPartyIndividualFormBasicValidationProcess.ValidateContactDetails(relatedPartyModel.ContactDetails);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateBusinessProfile(RelatedPartyModel relatedPartyModel)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (relatedPartyModel != null && relatedPartyModel.EmploymentDetails != null)
            {
                result = RelatedPartyIndividualFormBasicValidationProcess.ValidateBusinessProfile(relatedPartyModel.EmploymentDetails);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidatePersonalDetails(RelatedPartyModel relatedPartyModel)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (relatedPartyModel != null && relatedPartyModel.PersonalDetails != null)
            {
                if (!string.IsNullOrEmpty(relatedPartyModel.PersonalDetails.HdnDateOfBirth))
                {
                    relatedPartyModel.PersonalDetails.DateOfBirth = DateTime.ParseExact(relatedPartyModel.PersonalDetails.HdnDateOfBirth, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    relatedPartyModel.PersonalDetails.DateOfBirth = null;
                }
                result = RelatedPartyIndividualFormBasicValidationProcess.ValidatePersonalDetails(relatedPartyModel.PersonalDetails);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidatePartyRolesIndividual(RelatedPartyModel relatedPartyModel)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (relatedPartyModel != null && relatedPartyModel.PartyRoles != null)
            {
                //Need to implement
                result = RelatedPartyIndividualFormBasicValidationProcess.ValidatePartyRoles(relatedPartyModel.PartyRoles);
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidatePartyRolesLegal(RelatedPartyModel relatedPartyModel)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            if (relatedPartyModel != null && relatedPartyModel.PartyRolesLegal != null)
            {
                //Need to implement
                result = RelatedPartyLegalFormBasicValidationProcess.ValidatePartyRoles(relatedPartyModel.PartyRolesLegal);
            }

            return Json(result);
        }

        public JsonResult ValidatePepDates(string since, string untill)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(since))
            {
                isValid = false;
            }
            else if (!string.IsNullOrEmpty(since))
            {
                if (DateTime.Today < DateTime.ParseExact(since, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture))
                {
                    isValid = false;
                }
            }
            if (string.IsNullOrEmpty(untill))
            {
                isValid = false;
            }
            else if (!string.IsNullOrEmpty(untill))
            {
                if (DateTime.ParseExact(since, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) > DateTime.ParseExact(untill, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture))
                {
                    isValid = false;
                }
            }
            return Json(isValid);
        }
        #endregion

        [HttpPost]
        public ActionResult GetApplicantNameofRelatedParty(string applicaitonNumber, string applicationType)
        {
            if(!string.IsNullOrEmpty(applicaitonNumber) && !string.IsNullOrEmpty(applicationType))
            {
                List<ApplicantModel> applicantModels = new List<ApplicantModel>();
                if (string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicantModels = ApplicantProcess.GetLegalApplicantModels(applicaitonNumber);
                }
                else //Individual and Joint Individual
                {
                    applicantModels = ApplicantProcess.GetApplicantModels(applicaitonNumber);
                }
                if(applicantModels != null && applicantModels.Any())
                {
                    List<object> result = new List<object>();

                    foreach (var item in applicantModels)
                    {
                        result.Add(new {nodeGuid = item.NodeGUID, fullname = item.FullName});
                    }

                    return Json(result);
                }
            }
            return Json("");
        }

    }
}
