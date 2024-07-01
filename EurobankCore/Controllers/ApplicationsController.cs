using ClosedXML.Excel;
using CMS.Core;
using CMS.DataEngine;
using CMS.DataEngine.Query;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Eurobank.Controllers;
using Eurobank.Helpers.CustomHandler;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.External.SSP.Application.Individual;
using Eurobank.Helpers.External.SSP.Application.Legal;
using Eurobank.Helpers.Generics;
using Eurobank.Helpers.Process;
using Eurobank.Helpers.Validation;
using Eurobank.Models.Application;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Applicant.LegalEntity;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Application.RelatedParty;
using Eurobank.Models.Applications;
using Eurobank.Models.Applications.Accounts;
using Eurobank.Models.Applications.DebitCard;
using Eurobank.Models.Applications.DecisionHistory;
using Eurobank.Models.Applications.SourceofIncommingTransactions;
using Eurobank.Models.Documents;
using Eurobank.Models.KendoExtention;
using Eurobank.Models.User;
using Eurobank.Services;
using EurobankAccountSettings;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Lucene.Net.Util.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
//using org.pdfclown.documents.contents.objects;
using org.pdfclown.objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static Lucene.Net.Index.CheckIndex;

[assembly: RegisterPageRoute(Applications.CLASS_NAME, typeof(ApplicationsController))]
namespace Eurobank.Controllers
{
    [Authorize]
    [AuthorizeRoles(Role.PowerUser, Role.NormalUser)]
    [SessionAuthorization]
    //[ValidateAntiForgeryToken]
    public class ApplicationsController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly ApplicationsRepository applicationsRepository;
        private readonly SourceOfIncomingTransactionsRepository sourceOfIncomingTransactionsRepository;
        private readonly SourceOfOutgoingTransactionsRepository sourceOfOutgoingTransactionsRepository;
        private readonly AccountsRepository accountsRepository;
        private readonly EBankingSubscriberDetailsRepository eBankingSubscriberDetailsRepository;
        private readonly NoteDetailsRepository noteDetailsRepository;
        private readonly PersonalDetailsRepository personalDetailsRepository;
        private readonly CompanyDetailsRepository companyDetailsRepository;
        private readonly SignatureMandateIndividualRepository signatureMandateIndividualRepository;

        private readonly DebitCardDetailsRepository debitCardDetailsRepository;
        private readonly BankDocumentsRepository bankDocumentsRepository;
        private readonly ExpectedDocumentsRepository expectedDocumentsRepository;
        private readonly DecisionHistoryRepository decisionHistoryRepository;
        private readonly CompanyGroupStructureRepository companyGroupStructureRepository;
        private readonly SignatoryGroupRepository signatoryGroupRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly SignatureMandateCompanyRepository signatureMandateCompanyRepository;
        public ApplicationsController(
            IMemoryCache memoryCache,
        IPageDataContextRetriever pageDataContextRetriever,
            IPageUrlRetriever pageUrlRetriever,
             ApplicationsRepository applicationsRepository, SourceOfIncomingTransactionsRepository sourceOfIncomingTransactionsRepository, SourceOfOutgoingTransactionsRepository sourceOfOutgoingTransactionsRepository, AccountsRepository accountsRepository, EBankingSubscriberDetailsRepository eBankingSubscriberDetailsRepository, DebitCardDetailsRepository debitCardDetailsRepository, BankDocumentsRepository bankDocumentsRepository, ExpectedDocumentsRepository expectedDocumentsRepository, DecisionHistoryRepository decisionHistoryRepository, NoteDetailsRepository noteDetailsRepository, SignatureMandateIndividualRepository signatureMandateIndividualRepository, CompanyGroupStructureRepository companyGroupStructureRepository, SignatoryGroupRepository signatoryGroupRepository, IHttpContextAccessor httpContextAccessor
            , SignatureMandateCompanyRepository signatureMandateCompanyRepository, PersonalDetailsRepository personalDetailsRepository, CompanyDetailsRepository companyDetailsRepository)
        {
            _memoryCache = memoryCache;
            this.pageDataContextRetriever = pageDataContextRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
            this.applicationsRepository = applicationsRepository;
            this.sourceOfIncomingTransactionsRepository = sourceOfIncomingTransactionsRepository;
            this.sourceOfOutgoingTransactionsRepository = sourceOfOutgoingTransactionsRepository;
            this.accountsRepository = accountsRepository;
            this.eBankingSubscriberDetailsRepository = eBankingSubscriberDetailsRepository;
            this.debitCardDetailsRepository = debitCardDetailsRepository;
            this.bankDocumentsRepository = bankDocumentsRepository;
            this.expectedDocumentsRepository = expectedDocumentsRepository;
            this.decisionHistoryRepository = decisionHistoryRepository;
            this.noteDetailsRepository = noteDetailsRepository;
            this.signatureMandateIndividualRepository = signatureMandateIndividualRepository;
            this.companyGroupStructureRepository = companyGroupStructureRepository;
            this.signatoryGroupRepository = signatoryGroupRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.signatureMandateCompanyRepository = signatureMandateCompanyRepository;
            this.companyDetailsRepository = companyDetailsRepository;
            this.personalDetailsRepository = personalDetailsRepository;
        }
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)/";
        public IActionResult Index()
        {
            if (TempData["CopiedSuccessMsg"] != null)
            {
                ViewBag.CopiedSuccessMsg = TempData["CopiedSuccessMsg"].ToString();
            }
            return View();
        }
        #region Applications cerate
        public IActionResult Create()
        {
            bool IsLegalOnly = SettingsKeyInfoProvider.GetBoolValue(SiteContext.CurrentSiteName + ".LegalEntityOnly");
            ApplicationDetailsModelView applicationDetailsModelView = new ApplicationDetailsModelView();
            UserModel userModel = UserProcess.GetUser(User.Identity.Name);
            if (string.Equals(userModel.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                if (userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null)
                {
                    ViewBag.responsibleOfficer = ServiceHelper.GetResponsibleOfficerByGuid(userModel.IntroducerUser.Introducer.ResponsibleBankOfficer);
                    List<Guid> bankGuids = new List<Guid>();
                    bankGuids.Add(userModel.IntroducerUser.Introducer.DefaultBankUnit);
                    ViewBag.bankingService = ServiceHelper.GetBankingUnitsByBankGuids(bankGuids);
                }
                applicationDetailsModelView.IsEditingByIntroducer = true;

            }
            else
            {
                ViewBag.responsibleOfficer = ServiceHelper.GetResponsibleOfficer();
                if (userModel.InternalUser != null && userModel.InternalUser.AssignBankBranches != null)
                {
                    ViewBag.bankingService = ServiceHelper.GetBankingUnitsByBankGuids(userModel.InternalUser.AssignBankBranches);
                }
                else
                {
                    ViewBag.bankingService = ServiceHelper.GetBankingService();
                }

            }
            if (userModel.IntroducerUser.Introducer.IsLegalOnly && string.Equals(userModel.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.applicationType = ServiceHelper.GetApplicationTypeLegalOnly();
                applicationDetailsModelView.IsLegalOnly = true;
            }
            else
            {
                ViewBag.applicationType = ServiceHelper.GetApplicationType();
            }

            ViewBag.applicatonServices = ServiceHelper.GetApplicationService();

            CMS.Membership.UserInfo user = UserInfoProvider.GetUserInfo(User.Identity.Name);
            UserSettingsInfo userSettingsInfo = UserInfoProvider.GetUserInfo(User.Identity.Name).UserSettings;

            var applicationProductAndService = ServiceHelper.GetApplicationServiceItemGroup();
            applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup = ControlBinder.BindCheckBoxGroupItems(applicationProductAndService, null, '\0');
            applicationDetailsModelView.ApplicationDetails_SubmittedBy = user.FullName;
            var intermediary = applicationsRepository.GetIntermediary(ValidationHelper.GetString(userSettingsInfo.GetValue("Eurobank_UserOrganisation"), ""));
            if (intermediary != null)
            {
                applicationDetailsModelView.ApplicationDetails_IntroducerName = intermediary.RegistrationNumber + "-" + intermediary.IntermediaryName;
                applicationDetailsModelView.ApplicationDetails_ResponsibleOfficer = ValidationHelper.GetString(intermediary.ResponsibleBankOfficer, "");
                applicationDetailsModelView.ApplicationDetails_ResponsibleBankingCenter = ValidationHelper.GetString(intermediary.DefaultBankUnit, "");
                applicationDetailsModelView.ApplicationDetails_IntroducerCIF = ValidationHelper.GetString(intermediary.BankCIFNumber, "");
            }
            applicationDetailsModelView.ApplicationDetails_ApplicationTypeName = ServiceHelper.GetName(ValidationHelper.GetString(user.GetValue("Eurobank_UserType"), ""), "/Lookups/General/ENTITIES");
            applicationDetailsModelView.ApplicationDetails_SubmittedOn = ValidationHelper.GetString(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "");

            if (applicationProductAndService.Any(x => string.Equals(x.Label, "Account", StringComparison.OrdinalIgnoreCase)))
            {

                //string[] accountValue = new string[] { "370a6bd3-92cc-4a58-b2db-b38270dfa5bf" };
                applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue = new string[] { applicationProductAndService.FirstOrDefault(x => string.Equals(x.Label, "Account", StringComparison.OrdinalIgnoreCase)).Value };
                applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup.Items.FirstOrDefault(x => string.Equals(x.Label, "Account", StringComparison.OrdinalIgnoreCase)).CssClass = "checked-readonly";
            }


            return View(applicationDetailsModelView);
        }
        [HttpPost]
        public IActionResult Create(ApplicationDetailsModelView applicationDetailsModelView)
        {
            if (_memoryCache is MemoryCache memCache)
            {
                memCache.Compact(1.0);
            }
            if (Request.Form["ApplicationDetails_ApplicatonServices"].Any())
            {
                applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup = new CheckBoxGroupViewModel() { CheckBoxGroupValue = Request.Form["ApplicationDetails_ApplicatonServices"] };
            }
            //else
            //{
            //    ModelState.AddModelError("ApplicationDetails_ApplicatonServicesGroup", "Please check Products & Services.");
            //}
            if (applicationDetailsModelView != null && ModelState.IsValid)
            {
                ApplicationDetailsModelView applicationDetails = ApplicationsProcess.SaveApplicationsModel(applicationDetailsModelView, User.Identity.Name);
                ApplicationSyncService.SyncApplicationSearchRecord(ServiceHelper.GetApplicationNumber(applicationDetails.ApplicationDetailsID, ServiceHelper.GetName(applicationDetailsModelView.ApplicationDetails_ApplicationType))); //Sync application recored for search
                return RedirectToAction("Edit", "Applications", new { application = applicationDetails.Application_NodeGUID });
            }
            ViewBag.responsibleOfficer = ServiceHelper.GetResponsibleOfficer();
            ViewBag.applicationType = ServiceHelper.GetApplicationType();
            ViewBag.applicatonServices = ServiceHelper.GetApplicationService();
            ViewBag.bankingService = ServiceHelper.GetBankingService();
            applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup = ControlBinder.BindCheckBoxGroupItems(ServiceHelper.GetApplicationServiceItemGroup(), null, '\0');
            return View(applicationDetailsModelView);
        }
        #endregion
        public IActionResult Edit(string application)
        {
            //WorkFlowMailProcess.SendMailSubmitToPendingVerificationIntroducer("","","","","",true);
            string applicationNumber = "I-000000101";
            int id = 0;
            if (!string.IsNullOrEmpty(application))
            {
                var applicationdetails = applicationsRepository.GetApplicationDetailsByNodeGUID(application);
                if (applicationdetails != null)
                {
                    id = applicationdetails.ApplicationDetailsID;
                }
            }

            TempData["ApplicationID"] = id;

            CMS.Membership.UserInfo user = UserInfoProvider.GetUserInfo(User.Identity.Name);

            UserModel userModel = UserProcess.GetUser(User.Identity.Name);

            List<ApplicationDetailsModelView> applicationDetailsModels = ApplicationsProcess.GetApplicationByUserByApplication(userModel, applicationsRepository, id);

            if (applicationDetailsModels != null && applicationDetailsModels.Count == 0)//&& !applicationDetailsModels.Any(h => h.ApplicationDetailsID == id)
            {
                return Unauthorized();
            }

            ApplicationDetailsModelView applicationDetailsModelView = new ApplicationDetailsModelView();
            ApplicationViewModel model = new ApplicationViewModel();
            model.Id = id;
            model.UserFullName = user.FullName;
            model.UserNodeGUID = ValidationHelper.GetString(user.UserGUID, "");
            model.Application_NodeGUID = application;
            //ViewBag.responsibleOfficer = ServiceHelper.GetResponsibleOfficer();
            //ViewBag.applicationType = ServiceHelper.GetApplicationType();
            //ViewBag.applicatonServices = ServiceHelper.GetApplicationService();
            //ViewBag.bankingService = ServiceHelper.GetBankingService();
            //ViewBag.Country = ServiceHelper.GetCountries();
            //ViewBag.CountryIdentification = ServiceHelper.GetCountriesWithID();


            //ViewBag.AccessToAllPersonalAccount = ServiceHelper.GetBoolDropDownListDefaults();
            //ViewBag.AutomaticallyAddFuturePersonalAccount = ServiceHelper.GetBoolDropDownListDefaults();
            //ViewBag.DocumentType = ServiceHelper.GetDocumentTypes();

            //ViewBag.Subjects = ServiceHelper.GetDocumentSubjects();

            ////Below Value needs to be changed

            //ViewBag.AccessRights = ServiceHelper.GetAccessRights();
            //ViewBag.EntityTypes = ServiceHelper.GetEntityType();
            //ViewBag.GroupStructureEntityTypes = ServiceHelper.GetGroupStructureEntityType();
            //ViewBag.Parents = CompanyGroupStructureProcess.GetGroupStructures();
            //ViewBag.LimitAmount = ServiceHelper.GetLimitAmount();
            ViewBag.EntityBelongToAGroup = ServiceHelper.GetBoolDropDownListDefaults();
            //ViewBag.SignatureRights = ServiceHelper.GetSignatureRights();

            ViewBag.SignatoryGroup = ServiceHelper.GetSignatoryGroup();

            if (TempData["ErrorSummary"] != null)
            {
                ViewBag.ErrorSummaryMsgs = JsonConvert.DeserializeObject<List<ValidationResultModel>>(TempData["ErrorSummary"].ToString());
            }



            applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup = ControlBinder.BindCheckBoxGroupItems(ServiceHelper.GetApplicationServiceItemGroup(), null, '\0');

            if (id > 0 && userModel != null)
            {

                var applicationDetails = applicationsRepository.GetApplicationDetailsByID(id);
                if (applicationDetails != null)
                {
                    applicationNumber = applicationDetails.ApplicationDetails_ApplicationNumber;

                    model.ApplicationNumber = applicationNumber;

                    //model.ApplicationDetails = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                    model.ApplicationDetails = ApplicationsProcess.GetApplicationsDetails(userModel.UserType, userModel.UserRole, applicationDetails);
                    model.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.Items.FirstOrDefault(x => string.Equals(x.Label, "Account", StringComparison.OrdinalIgnoreCase)).CssClass = "checked-readonly";

                    string cardValue = applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup.Items.Where(x => string.Equals(x.Label, "CARD", StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).FirstOrDefault();
                    if (model.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue.Contains(cardValue))
                    {
                        model.IsCardShow = true;

                    }
                    string eBankingValue = applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup.Items.Where(x => string.Equals(x.Label, "DIGITAL BANKING", StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).FirstOrDefault();
                    if (model.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue.Contains(eBankingValue))
                    {
                        model.IsEbankingShow = true;
                    }
                    if (string.Equals(model.ApplicationDetails.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                    {
                        var applicantDetails = ApplicantProcess.GetLegalApplicantModels(applicationNumber);
                        if (applicantDetails != null && applicantDetails.Count > 0)
                        {
                            string applicantEntityType = applicantDetails.FirstOrDefault().CompanyDetails.EntityType;
                            string applicantEntityTypeName = ServiceHelper.GetName(applicantEntityType, Constants.COMPANY_ENTITY_TYPE);
                            model.ApplicantEntityType = applicantEntityTypeName;
                            if (string.Equals(applicantEntityTypeName, "Foundation") || string.Equals(applicantEntityTypeName, "Trade Union") || string.Equals(applicantEntityTypeName, "Club / Association") || string.Equals(applicantEntityTypeName, "City Council / Local Authority")
                                || string.Equals(applicantEntityTypeName, "Government Organization") || string.Equals(applicantEntityTypeName, "Semi - Government Organization") || string.Equals(applicantEntityTypeName, "Trust") || string.Equals(applicantEntityTypeName, "Provident Fund")
                                || string.Equals(applicantEntityTypeName, "Pension Fund") || string.Equals(applicantEntityTypeName, "General Partnership") || string.Equals(applicantEntityTypeName, "Limited Liability Partnership")
                                || string.Equals(applicantEntityTypeName, "Limited Partnership"))
                            {
                                model.IsCardNew = false;
                                model.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup.Items.FirstOrDefault(x => string.Equals(x.Label, "CARD", StringComparison.OrdinalIgnoreCase)).CssClass = "readonly";
                            }
                            else
                            {
                                model.IsCardNew = true;
                            }
                        }

                    }
                    else
                    {
                        model.IsCardNew = true;
                    }

                    model.GroupStructureLegalParent = GroupStructureLegalParentProcess.GetGroupStructureLegalParentModel(applicationNumber);


                    model.LeftMenuApplicant = new LeftMenuCommon();

                    model.LeftMenuApplicant.LeftMenuItems = LeftMenuProcess.GetApplicationConfiguration(model);
                    model.LeftMenuApplicant.LeftmenuClientMathodName = LeftMenuClientMathod.onApplicationStepperSelect;
                    // model.LeftMenuApplicant = LeftMenuProcess.GetApplicationConfiguration(model);

                    model.EntityType = applicationDetails.ApplicationDetails_ApplicationType;
                    model.EntityTypeCode = ValidationHelper.GetString(ServiceHelper.GetEntityType(applicationDetails.ApplicationDetails_ApplicationType, Constants.APPLICATION_TYPE), "");
                    //ViewBag.Subscribers = PersonalDetailsProcess.GetEBankingSubscribers(applicationNumber);

                    if (!string.Equals(model.EntityTypeCode, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase))
                    {
                        model.PurposeAndActivity = PurposeAndActivityProcess.GetPurposeAndActivityModel(applicationNumber);
                    }
                    else
                    {
                        string appplicantEntityType = string.Empty;
                        List<ApplicantModel> applicantLegal = ApplicantProcess.GetLegalApplicantModels(applicationNumber);
                        if (applicantLegal != null && applicantLegal.Count > 0 && applicantLegal.FirstOrDefault().CompanyDetails != null)
                        {
                            appplicantEntityType = applicantLegal.FirstOrDefault().CompanyDetails.EntityType;

                        }
                        model.PurposeAndActivity = PurposeAndActivityProcess.GetPurposeAndActivityModelLegal(applicationNumber, appplicantEntityType);
                    }



                    ViewBag.SelectedApplicationType = model.ApplicationDetails.ApplicationDetails_ApplicationTypeName;
                    ViewBag.DecisionList = ApplicationWorkFlowProcess.GetApplicationWorkflowDecisions(userModel, id);
                    model.DecisionHistoryViewModel = new DecisionHistoryViewModel();
                    ViewBag.AuthorizedGroup = SignatoryGroupProcess.GetDDLSignatoryGroupsByApplicationId(id);
                    ViewBag.ApplicationId = id;
                    ViewBag.ApplicationNumber = applicationNumber;
                    if (string.Equals(model.EntityTypeCode, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase))
                    {
                        ViewBag.CollectedByLegal = CommonProcess.GetDDLCollectedByLegal(applicationNumber);
                        ViewBag.DocumentsEntityName = CommonProcess.GetDocumentsEntityNameLegal(applicationNumber);
                        ViewBag.Subscribers = CommonProcess.GetEbankingSubscriberLegal(applicationNumber);
                        ViewBag.EbankSubscribers = CommonProcess.GetEbankingSubscriberLegalRoleConcat(applicationNumber);
                        ViewBag.CardHolderNameLegal = CommonProcess.GetCardHolderNameLegal(applicationNumber);
                        ViewBag.SignatoryPerson = CommonProcess.GetSignatoryPersonLegal(applicationNumber);
                        ViewBag.AccessLevels = ServiceHelper.GetAccessLevel();
                        ViewBag.DocumentEntityType = ServiceHelper.GetLegalPERSON_ROLE();
                    }
                    else
                    {
                        ViewBag.DocumentsEntityName = CommonProcess.GetDocumentsEntityName(applicationNumber);
                        ViewBag.Subscribers = PersonalDetailsProcess.GetEBankingSubscribers(applicationNumber);
                        ViewBag.CardHolderName = CommonProcess.GetCardHolderNameIndividual(applicationNumber);
                        ViewBag.SignatoryPersons = CommonProcess.GetSignatoryPersonIndividual(applicationNumber);
                        ViewBag.AccessLevels = ServiceHelper.GetAccessLevelIndividual();
                        ViewBag.DocumentEntityType = ServiceHelper.GetPERSON_ROLE();
                    }
                    //ViewBag.PendingOnUser = UserProcess.GetNotePendingUserDDL(applicationDetails.ApplicationDetails_ResponsibleBankingCenter, applicationDetails.ApplicationDetails_UserOrganisation.ToString());
                    //ViewBag.EscalateToUsers = UserProcess.GetEscalateToUsers(applicationDetails.ApplicationDetails_ResponsibleBankingCenter);
                    //List<IInputGroupItem> signatureGroupRights = ServiceHelper.SignatureMandateTypeGroup();
                    //if (signatureGroupRights != null)
                    //{
                    //    ViewBag.SignatureGroupRights = signatureGroupRights.Where(u => !string.Equals(u.Label, "Other", StringComparison.OrdinalIgnoreCase)).ToList();
                    //}


                }

            }
            TempData["Application"] = model;


            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(int id, ApplicationViewModel model, string appplicationButton)
        {
            if (_memoryCache is MemoryCache memCache)
            {
                memCache.Compact(1.0);
            }

            UserModel userModel = UserProcess.GetUser(User.Identity.Name);
            bool applicationConfirmValidation = false;
            List<ValidationResultModel> applicationValidationResult = null;
            bool IsApplicationDisabledMode = false;
            if (id == 0)
            {
                id = Convert.ToInt32(TempData["ApplicationID"]);
                IsApplicationDisabledMode = true;
            }
            TempData.Keep("Application");
            if (Request.Form["ReasonForOpeningTheAccountGroup"].Any())
            {
                model.PurposeAndActivity.ReasonForOpeningTheAccountGroup = new MultiselectDropDownViewModel() { MultiSelectValue = Request.Form["ReasonForOpeningTheAccountGroup"] };
            }
            if (Request.Form["ExpectedNatureOfInAndOutTransactionGroup"].Any())
            {
                model.PurposeAndActivity.ExpectedNatureOfInAndOutTransactionGroup = new MultiselectDropDownViewModel() { MultiSelectValue = Request.Form["ExpectedNatureOfInAndOutTransactionGroup"] };
            }
            if (Request.Form["ExpectedFrequencyOfInAndOutTransactionGroup"].Any())
            {
                model.PurposeAndActivity.ExpectedFrequencyOfInAndOutTransactionGroup = new RadioGroupViewModel() { RadioGroupValue = Request.Form["ExpectedFrequencyOfInAndOutTransactionGroup"] };
            }
            if (Request.Form["SignatureMandateTypeGroup"].Any())
            {
                model.PurposeAndActivity.SignatureMandateTypeGroup = new RadioGroupViewModel() { RadioGroupValue = Request.Form["SignatureMandateTypeGroup"] };
            }
            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(id);
            model.ApplicationDetails = ApplicationsProcess.GetApplicationsDetails(userModel.UserType, userModel.UserRole, applicationDetails);
            model.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup = new CheckBoxGroupViewModel() { CheckBoxGroupValue = Request.Form["ApplicationDetails_ApplicatonServices"] };
            //model.ApplicationDetails.ApplicationDetails_ApplicatonServicesGroup= ControlBinder.BindCheckBoxGroupItems(ServiceHelper.GetApplicationServiceItemGroup(), null, '\0');
            ApplicationDetailsModelView applicationResult = ApplicationsProcess.UpdateApplicationsModel(model.ApplicationDetails, User.Identity.Name);

            if (model != null && !string.IsNullOrEmpty(model.ApplicationNumber))
            {
                if (model.PurposeAndActivity != null)
                {
                    PurposeAndActivityProcess.SavePurposeAndActivityModel(model.ApplicationNumber, model.PurposeAndActivity);
                }
                if (model.GroupStructureLegalParent != null)
                {
                    //model.GroupStructureLegalParent.DoesTheEntityBelongToAGroup = Convert.ToBoolean(model.GroupStructureLegalParent.DoesTheEntityBelongToAGroupName);
                    GroupStructureLegalParentProcess.SaveGroupStructureLegalParentModel(model.ApplicationNumber, model.GroupStructureLegalParent);
                    if (string.Equals(model.GroupStructureLegalParent.DoesTheEntityBelongToAGroupName, "false", StringComparison.OrdinalIgnoreCase))
                    {
                        var groupStructure = CompanyGroupStructureProcess.GetCompanyGroupStructure(model.Id, model.ApplicationNumber);
                        if (groupStructure != null && groupStructure.Count > 0)
                        {
                            foreach (var item in groupStructure)
                            {
                                companyGroupStructureRepository.GetCompanyGroupStructure(item.Id).DeleteAllCultures();
                            }
                        }
                    }
                }

            }
            ApplicationWorkflowDecisionType decision = ApplicationWorkflowDecisionType.NONE;
            if (model != null && model.DecisionHistoryViewModel != null && id > 0 && string.Equals(appplicationButton, "CONFIRM", StringComparison.OrdinalIgnoreCase))
            {
                //Call Validation Logic Here and set isValid variable
                if (!IsApplicationDisabledMode)
                {
                    applicationValidationResult = ApplicationValidationProcess.ValidateApplication(model);
                    applicationConfirmValidation = !applicationValidationResult.Any(u => !u.IsValid);
                    if (!string.IsNullOrEmpty(model.DecisionHistoryViewModel.DecisionHistory_Decision))
                    {
                        decision = ApplicationWorkFlowProcess.GetMatchedApplicationWorkflowDecision(model.DecisionHistoryViewModel.DecisionHistory_Decision);

                    }
                    if (applicationConfirmValidation && decision == ApplicationWorkflowDecisionType.SUBMIT)
                    {
                        ApplicationsProcess.UpdateApplicationSubmitedOn(model.ApplicationDetails.ApplicationDetailsID, User.Identity.Name);
                    }
                    if (applicationConfirmValidation || decision == ApplicationWorkflowDecisionType.WITHDRAW)
                    {
                        ApplicationWorkFlowProcess.ExecuteWorkflow(userModel, id, model.DecisionHistoryViewModel.DecisionHistory_Decision, model.DecisionHistoryViewModel.DecisionHistory_Comments);
                        var applicatioData = applicationsRepository.GetApplicationDetailsByID(id);
                        var decisionHistory = DecisionHistoryProcess.SaveDecisionHistorysModel(model.DecisionHistoryViewModel, applicatioData, userModel.UserInformation.UserName);
                    }
                }
                else
                {
                    var ValidateDecision = ApplicationFormBasicValidationProcess.ValidateDecision(model.DecisionHistoryViewModel);
                    if (ValidateDecision.IsValid)
                    {
                        ApplicationWorkFlowProcess.ExecuteWorkflow(userModel, id, model.DecisionHistoryViewModel.DecisionHistory_Decision, model.DecisionHistoryViewModel.DecisionHistory_Comments);
                        var applicatioData = applicationsRepository.GetApplicationDetailsByID(id);
                        var decisionHistory = DecisionHistoryProcess.SaveDecisionHistorysModel(model.DecisionHistoryViewModel, applicatioData, userModel.UserInformation.UserName);
                    }
                    else
                    {
                        TempData["ErrorSummary"] = JsonConvert.SerializeObject(ValidateDecision);
                        return RedirectToAction("Edit", new { application = model.ApplicationDetails.Application_NodeGUID });
                    }

                }


            }

            if (model.ApplicationNumber != null)
            {
                ApplicationSyncService.SyncApplicationSearchRecord(model.ApplicationNumber); //Sync application recored for search
            }

            if (applicationValidationResult != null && applicationValidationResult.Any(u => !u.IsValid) && decision != ApplicationWorkflowDecisionType.WITHDRAW)
            {
                TempData["ErrorSummary"] = JsonConvert.SerializeObject(applicationValidationResult);
                return RedirectToAction("Edit", new { application = model.Application_NodeGUID });
            }

            if (string.Equals(appplicationButton, "SAVE_AS_DRAFT", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Edit", new { application = model.Application_NodeGUID });
            }
            else
            {
                return RedirectToAction("Index");
            }

            //return View(model);

            //}
        }

        public ActionResult Apllicants(int apllicationID)
        {
            return View();
        }

        public IActionResult Applications_Read([DataSourceRequest] DataSourceRequest request, string txtSearch, string filterColumn)
        {
            UserModel user = UserProcess.GetUser(User.Identity.Name);
            int totalCount = 0;
            string sortMember = string.Empty;
            string sortDirection = string.Empty;
            string stausFilter = string.Empty;
            List<ApplicationDetailsModelView> applicationDetailsModelViews = new List<ApplicationDetailsModelView>();

            if (user != null)
            {
                if (request.Sorts != null && request.Sorts.Count > 0)
                {
                    var sort = request.Sorts.FirstOrDefault();
                    //sortMember = sort.Member;
                    sortMember = ServiceHelper.GetFilterColumnName(sort.Member);
                    sortDirection = sort.SortDirection.ToString();
                }
                if (request.Filters != null && request.Filters.Count > 0)
                {
                    var filters = request.Filters.FirstOrDefault();
                    try
                    {
                        stausFilter = ((Kendo.Mvc.FilterDescriptor)filters).Value.ToString();
                    }
                    catch
                    {
                        stausFilter = "";
                    }
                }

                if (string.IsNullOrEmpty(txtSearch))
                {
                    txtSearch = string.Empty;
                }
                else if (string.Equals(filterColumn, "ApplicationDetails_SubmittedOn", StringComparison.OrdinalIgnoreCase) || string.Equals(filterColumn, "ApplicationDetails_CreatedOn", StringComparison.OrdinalIgnoreCase))
                {
                    txtSearch = ServiceHelper.ConvertDateStringToInt(txtSearch).ToString();
                }
                filterColumn = string.IsNullOrEmpty(filterColumn) == true ? string.Empty : ServiceHelper.GetFilterColumnName(filterColumn);
                if (!string.IsNullOrEmpty(txtSearch) || (request.Sorts != null && request.Sorts.Count > 0))
                {
                    if (_memoryCache is MemoryCache memCache)
                    {
                        memCache.Compact(1.0);
                    }
                }

                string cacheKey = user.UserInformation.UserGUID + "_" + stausFilter + "_" + request.Page.ToString();
                if (!_memoryCache.TryGetValue(cacheKey, out DataSourceResult cacheApplicationDetailsModelViews))
                {
                cacheApplicationDetailsModelViews = new DataSourceResult();
                List<ApplicationDetailsModelView> applicationDetailsModels = ApplicationsProcess.GetApplicationByUser(user, applicationsRepository, request.Page, request.PageSize, out totalCount, sortMember, sortDirection, txtSearch, filterColumn, stausFilter);
                if (applicationDetailsModels != null)
                {
                    cacheApplicationDetailsModelViews.Data = applicationDetailsModels;
                    cacheApplicationDetailsModelViews.Total = totalCount;
                    _memoryCache.Set(cacheKey, cacheApplicationDetailsModelViews);

                    var cacheEntryOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Set expiration time
                    _memoryCache.Set(cacheKey, cacheApplicationDetailsModelViews, cacheEntryOption);
                }
                }

                if (cacheApplicationDetailsModelViews.Data != null)
                {
                    totalCount = cacheApplicationDetailsModelViews.Total;
                    applicationDetailsModelViews = (List<ApplicationDetailsModelView>)cacheApplicationDetailsModelViews.Data;
                }
            }
            var result = new DataSourceResult()
            {
                Data = applicationDetailsModelViews,
                Total = totalCount
            };
            return Json(result);
            //return Json(applicationDetailsModelViews.ToDataSourceResult(request));
        }

        //     public IActionResult Applications_Read([DataSourceRequest] DataSourceRequest request)
        //     {
        //         UserInfo user = UserInfoProvider.GetUserInfo(User.Identity.Name);
        //         //user.UserSettings

        //         string userType = ServiceHelper.GetName(ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserType"), ""), "/Lookups/General/ENTITIES");
        //         var intermediary = applicationsRepository.GetIntermediary(ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserOrganisation"), ""));

        //         List<ApplicationDetailsModelView> applicationDetailsModelViews = new List<ApplicationDetailsModelView>();
        //         IEnumerable< CMS.DocumentEngine.Types.Eurobank.ApplicationDetails> applicationDetails = null;
        //         string userOrganisation = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        //         if(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value== Role.PowerUser && !string.Equals(userType, "Introducer", StringComparison.OrdinalIgnoreCase))
        //{
        //             //applicationDetails = applicationsRepository.GetApplicationDetailsPower(ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserOrganisation"), ""));
        //             //applicationDetails = applicationsRepository.GetApplicationDetailsPowerForBranch(ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserOrganisation"), ""));
        //             var allMainAndSubBranches = ServiceHelper.GetAllSubBankUnitsAlogWithParent(ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserOrganisation"), ""));
        //             if(allMainAndSubBranches != null && allMainAndSubBranches.Count > 0)
        //	{
        //                 applicationDetails = applicationsRepository.GetApplicationDetailsPowerForMainAndSubBranch(allMainAndSubBranches);
        //             }


        //         }
        //else
        //{
        //             applicationDetails = applicationsRepository.GetApplicationDetailsNormal(user.UserID);
        //         }

        //         if (applicationDetails != null)
        //         {

        //             foreach (var item in applicationDetails)
        //             {
        //                 ApplicationDetailsModelView applicationDetailsModelView = new ApplicationDetailsModelView();
        //                 applicationDetailsModelView.ApplicationDetails_ApplicationNumber = ValidationHelper.GetString(item.GetValue("ApplicationDetails_ApplicationNumber"), "");
        //                 applicationDetailsModelView.ApplicationDetails_ApplicationStatus = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ApplicationDetails_ApplicationStatus"), ""), "/Lookups/General/APPLICATION-SERVICES");
        //                 applicationDetailsModelView.ApplicationDetails_ApplicationType = item.ApplicationDetails_ApplicationType;
        //                 applicationDetailsModelView.ApplicationDetails_ApplicationTypeName = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ApplicationDetails_ApplicationType"), ""), "/Lookups/General/APPLICATION-TYPE");
        //                 applicationDetailsModelView.ApplicationDetails_ApplicatonServices = ValidationHelper.GetString(item.GetValue("ApplicationDetails_ApplicatonServices"), "");

        //                 applicationDetailsModelView.ApplicationDetails_CurrentStage = ValidationHelper.GetString(item.GetValue("ApplicationDetails_CurrentStage"), "");
        //                 applicationDetailsModelView.ApplicationDetails_IntroducerCIF = ValidationHelper.GetString(item.GetValue("ApplicationDetails_IntroducerCIF"), "");
        //                 applicationDetailsModelView.ApplicationDetails_IntroducerName = ValidationHelper.GetString(item.GetValue("ApplicationDetails_IntroducerName"), "");
        //                 applicationDetailsModelView.ApplicationDetails_ResponsibleBankingCenter = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ApplicationDetails_ResponsibleBankingCenter"), ""), "/Bank-Units");
        //                 applicationDetailsModelView.ApplicationDetails_ResponsibleOfficer = ServiceHelper.GetUserName(ValidationHelper.GetString(item.GetValue("ApplicationDetails_ResponsibleOfficer"), ""));

        //                 applicationDetailsModelView.ApplicationDetailsID = ValidationHelper.GetInteger(item.GetValue("ApplicationDetailsID"), 0);
        //                 applicationDetailsModelView.ApplicationDetails_CreatedOn = ValidationHelper.GetString(Convert.ToDateTime(item.DocumentCreatedWhen).ToString("MM/dd/yyyy HH:mm:ss"), "");
        //                 applicationDetailsModelView.ApplicationDetails_SubmittedOn = ValidationHelper.GetString(Convert.ToDateTime(item.ApplicationDetails_SubmittedOn).ToString("MM/dd/yyyy HH:mm:ss"), "");
        //                 applicationDetailsModelView.ApplicationDetails_SubmittedBy = ValidationHelper.GetString(item.ApplicationDetails_SubmittedBy, "");
        //                 applicationDetailsModelViews.Add(applicationDetailsModelView);

        //             }
        //         }
        //         return Json(applicationDetailsModelViews.ToDataSourceResult(request));
        //     }

        public IActionResult Applications_Destroy([DataSourceRequest] DataSourceRequest request, ApplicationDetailsModelView applicationModel)
        {
            if (applicationModel != null)
            {
                //registriesRepository.GetRegistryUserByNodeGUID(personsRegistry.NodeGUID).DeleteAllCultures();
                var UserRegistry = applicationsRepository.GetApplicationDetailsByID(applicationModel.ApplicationDetailsID);
                UserRegistry.DocumentPublishTo = DateTime.Now;
                UserRegistry.Update();
            }

            return Json(new[] { applicationModel }.ToDataSourceResult(request, ModelState));
        }

        //[HttpPost]
        //public ActionResult ApplicationPopup_Create([DataSourceRequest] DataSourceRequest request, ApplicationDetailsModelView applicationDetailsModelView)
        //{
        //	if(applicationDetailsModelView != null && ModelState.IsValid)
        //	{
        //		TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
        //		UserInfo user = UserInfoProvider.GetUserInfo(User.Identity.Name);
        //		string currentYear = ValidationHelper.GetString(DateTime.Now.Year, "");
        //		string currentMonth = ValidationHelper.GetString(DateTime.Now.ToString("MMMM"), "");
        //		CMS.DocumentEngine.TreeNode applicationfoldernode_parent = tree.SelectNodes()
        //			.Path("/Applications-(1)/" + currentYear + "/" + currentMonth)
        //			.OnCurrentSite()
        //			.Published(false)
        //			.FirstOrDefault();

        //		if(applicationfoldernode_parent != null)
        //		{
        //			CMS.DocumentEngine.TreeNode personsRegistryAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.ApplicationDetails", tree);
        //			personsRegistryAdd.DocumentName = "000000000";
        //			personsRegistryAdd.SetValue("ApplicationDetails_ApplicationStatus", applicationDetailsModelView.ApplicationDetails_ApplicationStatus);
        //			personsRegistryAdd.SetValue("ApplicationDetails_ApplicationType", applicationDetailsModelView.ApplicationDetails_ApplicationType);
        //			personsRegistryAdd.SetValue("ApplicationDetails_ApplicatonServices", applicationDetailsModelView.ApplicationDetails_ApplicatonServices);
        //			personsRegistryAdd.SetValue("ApplicationDetails_IntroducerCIF", applicationDetailsModelView.ApplicationDetails_IntroducerCIF);
        //			personsRegistryAdd.SetValue("ApplicationDetails_IntroducerName", applicationDetailsModelView.ApplicationDetails_IntroducerName);
        //			personsRegistryAdd.SetValue("ApplicationDetails_ResponsibleBankingCenter", applicationDetailsModelView.ApplicationDetails_ResponsibleBankingCenter);
        //			personsRegistryAdd.SetValue("ApplicationDetails_ResponsibleOfficer", applicationDetailsModelView.ApplicationDetails_ResponsibleOfficer);
        //			personsRegistryAdd.SetValue("ApplicationDetails_SubmittedBy", user.UserName);
        //			personsRegistryAdd.SetValue("ApplicationDetails_DocumentSubmittedByUserID", user.UserID);
        //			personsRegistryAdd.SetValue("ApplicationDetails_SubmittedOn", DateTime.Now);
        //			personsRegistryAdd.Insert(applicationfoldernode_parent);
        //			personsRegistryAdd.DocumentName = ServiceHelper.GetApplicationNumber(ValidationHelper.GetInteger(personsRegistryAdd.GetValue("ApplicationDetailsID"), 0), ServiceHelper.GetName(applicationDetailsModelView.ApplicationDetails_ApplicationType, "/Lookups/General/APPLICATION-TYPE"));
        //			personsRegistryAdd.SetValue("ApplicationDetails_ApplicationNumber", personsRegistryAdd.DocumentName);
        //			personsRegistryAdd.Update();
        //			applicationDetailsModelView.ApplicationDetails_CreatedOn = ValidationHelper.GetString(Convert.ToDateTime(personsRegistryAdd.DocumentCreatedWhen).ToString("MM/dd/yyyy HH:mm:ss"), "");
        //			applicationDetailsModelView.ApplicationDetails_SubmittedOn = ValidationHelper.GetString(Convert.ToDateTime(personsRegistryAdd.GetValue("ApplicationDetails_SubmittedOn")).ToString("MM/dd/yyyy HH:mm:ss"), "");
        //			applicationDetailsModelView.ApplicationDetails_ApplicationNumber = ValidationHelper.GetString((personsRegistryAdd.GetValue("ApplicationDetails_ApplicationNumber")), "");
        //		}
        //		else
        //		{
        //			CMS.DocumentEngine.TreeNode apllicationsFolder = tree.SelectNodes()
        //	   .Path("/Applications-(1)")
        //	   .Published(false)
        //	   .OnCurrentSite()
        //	   .FirstOrDefault();


        //			CMS.DocumentEngine.TreeNode yearfoldernode = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
        //			yearfoldernode.DocumentName = currentYear;
        //			yearfoldernode.DocumentCulture = "en-US";
        //			yearfoldernode.Insert(apllicationsFolder);
        //			CMS.DocumentEngine.TreeNode monthfoldernode = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
        //			monthfoldernode.DocumentName = currentMonth;
        //			monthfoldernode.DocumentCulture = "en-US";
        //			monthfoldernode.Insert(yearfoldernode);
        //			CMS.DocumentEngine.TreeNode personsRegistryAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.ApplicationDetails", tree);
        //			personsRegistryAdd.DocumentName = "000000000";
        //			personsRegistryAdd.SetValue("ApplicationDetails_ApplicationStatus", applicationDetailsModelView.ApplicationDetails_ApplicationStatus);
        //			personsRegistryAdd.SetValue("ApplicationDetails_ApplicationType", applicationDetailsModelView.ApplicationDetails_ApplicationType);
        //			personsRegistryAdd.SetValue("ApplicationDetails_ApplicatonServices", applicationDetailsModelView.ApplicationDetails_ApplicatonServices);
        //			personsRegistryAdd.SetValue("ApplicationDetails_IntroducerCIF", applicationDetailsModelView.ApplicationDetails_IntroducerCIF);
        //			personsRegistryAdd.SetValue("ApplicationDetails_IntroducerName", applicationDetailsModelView.ApplicationDetails_IntroducerName);
        //			personsRegistryAdd.SetValue("ApplicationDetails_ResponsibleBankingCenter", applicationDetailsModelView.ApplicationDetails_ResponsibleBankingCenter);
        //			personsRegistryAdd.SetValue("ApplicationDetails_ResponsibleOfficer", applicationDetailsModelView.ApplicationDetails_ResponsibleOfficer);
        //			personsRegistryAdd.SetValue("ApplicationDetails_SubmittedBy", user.UserName);
        //			personsRegistryAdd.SetValue("ApplicationDetails_DocumentSubmittedByUserID", user.UserID);
        //			personsRegistryAdd.SetValue("ApplicationDetails_SubmittedOn", DateTime.Now);
        //			personsRegistryAdd.Insert(monthfoldernode);
        //			personsRegistryAdd.DocumentName = ServiceHelper.GetApplicationNumber(ValidationHelper.GetInteger(personsRegistryAdd.GetValue("ApplicationDetailsID"), 0), ServiceHelper.GetName(applicationDetailsModelView.ApplicationDetails_ApplicationType, "/Lookups/General/APPLICATION-TYPE"));
        //			personsRegistryAdd.SetValue("ApplicationDetails_ApplicationNumber", personsRegistryAdd.DocumentName);
        //			personsRegistryAdd.Update();
        //			applicationDetailsModelView.ApplicationDetails_CreatedOn = ValidationHelper.GetString(Convert.ToDateTime(personsRegistryAdd.DocumentCreatedWhen).ToString("MM/dd/yyyy HH:mm:ss"), "");
        //			applicationDetailsModelView.ApplicationDetails_SubmittedOn = ValidationHelper.GetString(Convert.ToDateTime(personsRegistryAdd.GetValue("ApplicationDetails_SubmittedOn")).ToString("MM/dd/yyyy HH:mm:ss"), "");
        //			applicationDetailsModelView.ApplicationDetails_ApplicationNumber = ValidationHelper.GetString((personsRegistryAdd.GetValue("ApplicationDetails_ApplicationNumber")), "");

        //		}

        //		//}
        //		applicationDetailsModelView.ApplicationDetails_ApplicationTypeName = ServiceHelper.GetName(ValidationHelper.GetString(applicationDetailsModelView.ApplicationDetails_ApplicationType, ""), "/Lookups/General/APPLICATION-TYPE");
        //		applicationDetailsModelView.ApplicationDetails_SubmittedBy = user.FullName;
        //	}

        //	return Json(new[] { applicationDetailsModelView }.ToDataSourceResult(request, ModelState));
        //}
        //[HttpPost]
        //public ActionResult ApplicationPopup_Update([DataSourceRequest] DataSourceRequest request, ApplicationDetailsModelView applicationDetailsModelView)
        //{
        //	if(applicationDetailsModelView != null && ModelState.IsValid)
        //	{
        //		TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

        //		CMS.DocumentEngine.TreeNode apllicationsFolder = tree.SelectNodes()
        //	   .Path("//Applications-(1)")
        //	   .Published(false)
        //	   .OnCurrentSite()
        //	   .FirstOrDefault();
        //		string currentYear = ValidationHelper.GetString(DateTime.Now.Year, "");
        //		string currentMonth = ValidationHelper.GetString(DateTime.Now.ToString("MMMM"), "");
        //		UserInfo user = UserInfoProvider.GetUserInfo(User.Identity.Name);
        //		CMS.DocumentEngine.TreeNode yearfoldernode = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
        //		yearfoldernode.DocumentName = currentYear;
        //		yearfoldernode.DocumentCulture = "en-US";
        //		yearfoldernode.Insert(apllicationsFolder);
        //		CMS.DocumentEngine.TreeNode monthfoldernode = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
        //		monthfoldernode.DocumentName = currentMonth;
        //		monthfoldernode.DocumentCulture = "en-US";
        //		monthfoldernode.Insert(yearfoldernode);
        //		CMS.DocumentEngine.TreeNode personsRegistryAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.ApplicationDetails", tree);
        //		personsRegistryAdd.DocumentName = "000000000";
        //		personsRegistryAdd.SetValue("ApplicationDetails_ApplicationStatus", applicationDetailsModelView.ApplicationDetails_ApplicationStatus);
        //		personsRegistryAdd.SetValue("ApplicationDetails_ApplicationType", applicationDetailsModelView.ApplicationDetails_ApplicationType);
        //		personsRegistryAdd.SetValue("ApplicationDetails_ApplicatonServices", applicationDetailsModelView.ApplicationDetails_ApplicatonServices);
        //		personsRegistryAdd.SetValue("ApplicationDetails_IntroducerCIF", applicationDetailsModelView.ApplicationDetails_IntroducerCIF);
        //		personsRegistryAdd.SetValue("ApplicationDetails_IntroducerName", applicationDetailsModelView.ApplicationDetails_IntroducerName);
        //		personsRegistryAdd.SetValue("ApplicationDetails_ResponsibleBankingCenter", applicationDetailsModelView.ApplicationDetails_ResponsibleBankingCenter);
        //		personsRegistryAdd.SetValue("ApplicationDetails_ResponsibleOfficer", applicationDetailsModelView.ApplicationDetails_ResponsibleOfficer);
        //		personsRegistryAdd.SetValue("ApplicationDetails_SubmittedBy", user.UserID);
        //		personsRegistryAdd.SetValue("ApplicationDetails_SubmittedOn", DateTime.Now);
        //		personsRegistryAdd.Insert(monthfoldernode);
        //		personsRegistryAdd.DocumentName = ServiceHelper.GetApplicationNumber(ValidationHelper.GetInteger(personsRegistryAdd.GetValue("ApplicationDetails_ApplicationNumber"), 0), ServiceHelper.GetName(applicationDetailsModelView.ApplicationDetails_ApplicationType, "/Lookups/General/APPLICATION-TYPE"));
        //		personsRegistryAdd.SetValue("ApplicationDetails_ApplicationNumber", ServiceHelper.GetApplicationNumber(ValidationHelper.GetInteger(personsRegistryAdd.GetValue("ApplicationDetails_ApplicationNumber"), 0), ServiceHelper.GetName(applicationDetailsModelView.ApplicationDetails_ApplicationType, "")));
        //		personsRegistryAdd.Update();
        //		//}


        //	}

        //	return Json(new[] { applicationDetailsModelView }.ToDataSourceResult(request, ModelState));
        //}
        #region Source of Incomming transaction
        public IActionResult IncommingTransaction_Read([DataSourceRequest] DataSourceRequest request)
        {

            TempData.Keep("ApplicationID");
            int applicationID = ValidationHelper.GetInteger(TempData["ApplicationID"], 0);
            List<SourceOfIncomingTransactionsViewModel> sourceOfIncomingTransactionsViewModels = new List<SourceOfIncomingTransactionsViewModel>();
            var incommingTransactionData = sourceOfIncomingTransactionsRepository.GetSourceofIncommingTransaction(applicationID);
            if (incommingTransactionData != null)
            {
                int rowId = 0;
                foreach (var item in incommingTransactionData)
                {
                    rowId = rowId + 1;
                    SourceOfIncomingTransactionsViewModel sourceOfIncomingTransactionsViewModel = new SourceOfIncomingTransactionsViewModel();
                    sourceOfIncomingTransactionsViewModel.rowID = rowId;
                    sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactionsID = item.SourceOfIncomingTransactionsID;
                    sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitter = item.SourceOfIncomingTransactions_CountryOfRemitter;
                    sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitterBank = item.SourceOfIncomingTransactions_CountryOfRemitterBank;
                    sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_NameOfRemitter = item.SourceOfIncomingTransactions_NameOfRemitter;
                    //sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitterName = ServiceHelper.GetCountryName(sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitter);
                    //sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitterBankName = ServiceHelper.GetCountryName(sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitterBank);
                    sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_Status_Name = item.SourceOfIncomingTransactions_Status == true ? "Complete" : "Pending";
                    sourceOfIncomingTransactionsViewModels.Add(sourceOfIncomingTransactionsViewModel);

                }
            }
            return Json(sourceOfIncomingTransactionsViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult IncommingTransactionPopup_Create([DataSourceRequest] DataSourceRequest request, SourceOfIncomingTransactionsViewModel sourceOfIncomingTransactionsViewModel, int id)
        {
            if (sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateSourceOfIncomingTransaction(sourceOfIncomingTransactionsViewModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }

                }
            }
            int applicationID = ValidationHelper.GetInteger(id, 0);
            if (sourceOfIncomingTransactionsViewModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(sourceOfIncomingTransactionsViewModel))
            {
                var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                var successData = SourceOfIncomingTransactionsProcess.SaveIncomingTransactionsModel(sourceOfIncomingTransactionsViewModel, applicatioData.NodeAliasPath);
                sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitterBank = successData.SourceOfIncomingTransactions_CountryOfRemitterBank;
                sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitter = successData.SourceOfIncomingTransactions_CountryOfRemitter;
                sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_Status_Name = sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_Status == true ? "Complete" : "Pending";
                sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactionsID = successData.SourceOfIncomingTransactionsID;

            }


            return Json(new[] { sourceOfIncomingTransactionsViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult IncommingTransactionPopup_Update([DataSourceRequest] DataSourceRequest request, SourceOfIncomingTransactionsViewModel sourceOfIncomingTransactionsViewModel)
        {
            if (sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateSourceOfIncomingTransaction(sourceOfIncomingTransactionsViewModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (sourceOfIncomingTransactionsViewModel != null && ModelState.IsValid)
            {
                var incommingTransactionData = sourceOfIncomingTransactionsRepository.GetSourceOfIncomingTransactions(sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactionsID);
                if (incommingTransactionData != null)
                {
                    var successData = SourceOfIncomingTransactionsProcess.UpdateIncomingTransactionsModel(sourceOfIncomingTransactionsViewModel, incommingTransactionData);

                    sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitterBank = successData.SourceOfIncomingTransactions_CountryOfRemitterBank;
                    sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitter = successData.SourceOfIncomingTransactions_CountryOfRemitter;
                    sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_Status_Name = successData.SourceOfIncomingTransactions_Status == true ? "Complete" : "Pending";
                    sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactionsID = successData.SourceOfIncomingTransactionsID;
                }
            }

            return Json(new[] { sourceOfIncomingTransactionsViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult IncommingTransactionPopup_Destroy([DataSourceRequest] DataSourceRequest request, SourceOfIncomingTransactionsViewModel sourceOfIncomingTransactionsViewModel)
        {
            if (sourceOfIncomingTransactionsViewModel != null)
            {
                sourceOfIncomingTransactionsRepository.GetSourceOfIncomingTransactions(sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactionsID).DeleteAllCultures();
            }

            return Json(new[] { sourceOfIncomingTransactionsViewModel }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Source of Outgoing transaction
        public IActionResult OutgoingTransaction_Read([DataSourceRequest] DataSourceRequest request)
        {

            TempData.Keep("ApplicationID");
            int applicationID = ValidationHelper.GetInteger(TempData["ApplicationID"], 0);
            List<SourceOfOutgoingTransactionsModel> sourceOfOutgoingTransactionsViewModels = new List<SourceOfOutgoingTransactionsModel>();
            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationID);

            if (applicationDetails != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationNumber))
            {
                var result = SourceOfOutgoingTransactionProcess.GetSourceOfOutgoingTransactionModels(applicationDetails.ApplicationDetails_ApplicationNumber);
                int rowId = 0;
                foreach (var item in result)
                {
                    SourceOfOutgoingTransactionsModel _sourceOfOutgoingTransactionsModel = new SourceOfOutgoingTransactionsModel();
                    rowId++;
                    _sourceOfOutgoingTransactionsModel.rowID = rowId;
                    _sourceOfOutgoingTransactionsModel.Id = item.Id;
                    _sourceOfOutgoingTransactionsModel.NameOfBeneficiary = item.NameOfBeneficiary;
                    _sourceOfOutgoingTransactionsModel.CountryOfBeneficiaryBank = item.CountryOfBeneficiaryBank;
                    _sourceOfOutgoingTransactionsModel.CountryOfBeneficiary = item.CountryOfBeneficiary;
                    _sourceOfOutgoingTransactionsModel.StatusName = item.StatusName;
                    sourceOfOutgoingTransactionsViewModels.Add(_sourceOfOutgoingTransactionsModel);

                }
            }

            return Json(sourceOfOutgoingTransactionsViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult OutgoingTransactionPopup_Create([DataSourceRequest] DataSourceRequest request, SourceOfOutgoingTransactionsModel sourceOfOutgoingTransactionsModel, int apID)
        {

            if (sourceOfOutgoingTransactionsModel.SourceOfOutgoingTransactions_Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateSourceOfOutgoingTransaction(sourceOfOutgoingTransactionsModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            int applicationID = ValidationHelper.GetInteger(apID, 0);
            if (sourceOfOutgoingTransactionsModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(sourceOfOutgoingTransactionsModel))
            {
                var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);

                if (applicatioData != null && !string.IsNullOrEmpty(applicatioData.ApplicationDetails_ApplicationNumber))
                {
                    var sourceOfOutgoingTransaction = SourceOfOutgoingTransactionProcess.SaveSourceOfOutgoingTransactionsModel(applicatioData.ApplicationDetails_ApplicationNumber, sourceOfOutgoingTransactionsModel);
                    sourceOfOutgoingTransactionsModel.StatusName = sourceOfOutgoingTransactionsModel.SourceOfOutgoingTransactions_Status == true ? "Complete" : "Pending";
                    sourceOfOutgoingTransactionsModel.Id = sourceOfOutgoingTransaction.Id;
                    sourceOfOutgoingTransactionsModel.CountryOfBeneficiary = sourceOfOutgoingTransaction.CountryOfBeneficiary;
                    sourceOfOutgoingTransactionsModel.CountryOfBeneficiaryBank = sourceOfOutgoingTransaction.CountryOfBeneficiaryBank;
                }
            }

            return Json(new[] { sourceOfOutgoingTransactionsModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult OutgoingTransactionPopup_Update([DataSourceRequest] DataSourceRequest request, SourceOfOutgoingTransactionsModel sourceOfOutgoingTransactionsModel)
        {
            if (sourceOfOutgoingTransactionsModel.SourceOfOutgoingTransactions_Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateSourceOfOutgoingTransaction(sourceOfOutgoingTransactionsModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (sourceOfOutgoingTransactionsModel != null && ModelState.IsValid)
            {
                var incommingTransactionData = sourceOfOutgoingTransactionsRepository.GetSourceOfOutgoingTransactions(sourceOfOutgoingTransactionsModel.Id);
                if (incommingTransactionData != null)
                {
                    var sourceOfOutgoingTransaction = SourceOfOutgoingTransactionProcess.SaveSourceOfOutgoingTransactionsModel(null, sourceOfOutgoingTransactionsModel);
                    sourceOfOutgoingTransactionsModel.StatusName = sourceOfOutgoingTransaction.SourceOfOutgoingTransactions_Status == true ? "Complete" : "Pending";
                    sourceOfOutgoingTransactionsModel.Id = sourceOfOutgoingTransaction.Id;
                    sourceOfOutgoingTransactionsModel.CountryOfBeneficiary = sourceOfOutgoingTransaction.CountryOfBeneficiary;
                    sourceOfOutgoingTransactionsModel.CountryOfBeneficiaryBank = sourceOfOutgoingTransaction.CountryOfBeneficiaryBank;
                }
            }

            return Json(new[] { sourceOfOutgoingTransactionsModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult OutgoingTransactionPopup_Destroy([DataSourceRequest] DataSourceRequest request, SourceOfOutgoingTransactionsModel sourceOfOutgoingTransactionsModel)
        {
            if (sourceOfOutgoingTransactionsModel != null)
            {
                sourceOfOutgoingTransactionsRepository.GetSourceOfOutgoingTransactions(sourceOfOutgoingTransactionsModel.Id).DeleteAllCultures();
            }

            return Json(new[] { sourceOfOutgoingTransactionsModel }.ToDataSourceResult(request, ModelState));
        }

        //public ActionResult OutgoingTransactionPopup_Destroy(int id)
        //{
        //    if(id > 0)
        //    {
        //        sourceOfOutgoingTransactionsRepository.GetSourceOfOutgoingTransactions(id).DeleteAllCultures();
        //    }

        //    return Json(new[] { sourceOfOutgoingTransactionsModel }.ToDataSourceResult(request, ModelState));
        //}

        #endregion
        #region Applicant
        public ActionResult Applicant(int applicationId, int applicantId)
        {
            ApplicantModel model = new ApplicantModel();
            if (applicationId == 0 && applicantId == 0)
                return Redirect("/Applications");
            //var applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);
            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationId);
            if (applicationDetails == null && applicantId == 0)
                return Redirect("/Applications");
            if (applicationDetails != null)
            {
                model.ApplicationNumber = applicationDetails.ApplicationDetails_ApplicationNumber;
            }
            if (applicantId > 0)
            {
                model.PersonalDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicantId);
                model.Id = applicantId;
            }
            else
            {
                model.PersonalDetails = new PersonalDetailsModel();
            }


            ViewBag.PersonTitle = ServiceHelper.GetTitle();
            ViewBag.Gender = ServiceHelper.GetGendar();
            ViewBag.Education = ServiceHelper.GetEducationLevel();
            //ViewBag.Country = ServiceHelper.GetCountries();
            ViewBag.Country = ServiceHelper.GetCountriesWithID();
            return View(model);
        }


        public IActionResult Applicant_Read([DataSourceRequest] DataSourceRequest request)
        {

            TempData.Keep("ApplicationID");
            int applicationID = ValidationHelper.GetInteger(TempData["ApplicationID"], 0);
            List<ApplicantModel> applicantModels = null;
            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationID);
            string applicationType = string.Empty;
            var applicationTypes = ServiceHelper.GetApplicationType();

            if (applicationDetails != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationNumber))
            {
                applicationType = (applicationTypes != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationType) && applicationTypes.Any(k => string.Equals(k.Value, applicationDetails.ApplicationDetails_ApplicationType, StringComparison.OrdinalIgnoreCase))) ? applicationTypes.FirstOrDefault(k => string.Equals(k.Value, applicationDetails.ApplicationDetails_ApplicationType, StringComparison.OrdinalIgnoreCase)).Text : string.Empty;
                if (!string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicantModels = ApplicantProcess.GetApplicantModels(applicationDetails.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicantModels = ApplicantProcess.GetLegalApplicantModels(applicationDetails.ApplicationDetails_ApplicationNumber);
                }
            }
            if (applicantModels == null)
            {
                applicantModels = new List<ApplicantModel>();
            }
            else
            {
                applicantModels = applicantModels.OrderBy(k => k.CreatedDateTime).ToList();
            }

            return Json(applicantModels.ToDataSourceResult(request));
        }

        public ActionResult ApplicantPopup_Destroy([DataSourceRequest] DataSourceRequest request, ApplicantModel applicantModel)
        {
            //if(applicantModel != null)
            //{
            //	sourceOfOutgoingTransactionsRepository.GetSourceOfOutgoingTransactions(applicantModel.Id).DeleteAllCultures();
            //}

            if (applicantModel != null)
            {
                var applicationDetails = applicationsRepository.GetApplicationDetailsByApplicationNumber(applicantModel.ApplicationNumber);
                if (applicantModel.CompanyDetails != null)
                {
                    var UserRegistry = companyDetailsRepository.GetCompanyDetailsByID(applicantModel.CompanyDetails.Id);
                    UserRegistry.DocumentPublishTo = DateTime.Now;
                    UserRegistry.Update();

                    // Expected Document Delete
                    var expectedDocument = expectedDocumentsRepository.GetExpectedDocuments(applicationDetails.ApplicationDetailsID).Where(x => x.ExpectedDocuments_Entity.ToString() == applicantModel.CompanyDetails.NodeGUID).ToList();
                    if (expectedDocument != null && expectedDocument.Count > 0)
                    {
                        foreach (var item in expectedDocument)
                        {
                            item.Delete();
                        }
                    }
                    //Bank Document Delete
                    var bankDocument = bankDocumentsRepository.GetBankDocuments(applicationDetails.ApplicationDetailsID).Where(x => x.BankDocuments_Entity.ToString() == applicantModel.CompanyDetails.NodeGUID).ToList();
                    if (bankDocument != null && bankDocument.Count > 0)
                    {
                        foreach (var item in bankDocument)
                        {
                            item.Delete();
                        }
                    }
                }
                if (applicantModel.PersonalDetails != null)
                {
                    var UserRegistry = personalDetailsRepository.GetPersonalDetailsByID(applicantModel.PersonalDetails.Id);
                    UserRegistry.DocumentPublishTo = DateTime.Now;
                    UserRegistry.Update();

                    // Expected Document Delete
                    var expectedDocument = expectedDocumentsRepository.GetExpectedDocuments(applicationDetails.ApplicationDetailsID).Where(x => x.ExpectedDocuments_Entity.ToString() == applicantModel.PersonalDetails.NodeGUID).ToList();
                    if (expectedDocument != null && expectedDocument.Count > 0)
                    {
                        foreach (var item in expectedDocument)
                        {
                            item.Delete();
                        }
                    }
                    //Bank Document Delete
                    var bankDocument = bankDocumentsRepository.GetBankDocuments(applicationDetails.ApplicationDetailsID).Where(x => x.BankDocuments_Entity.ToString() == applicantModel.PersonalDetails.NodeGUID).ToList();
                    if (bankDocument != null && bankDocument.Count > 0)
                    {
                        foreach (var item in bankDocument)
                        {
                            item.Delete();
                        }
                    }
                }
                ApplicationSyncService.SyncApplicationSearchRecord(applicantModel.ApplicationNumber); //Sync application recored for search
                //registriesRepository.GetRegistryUserByNodeGUID(personsRegistry.NodeGUID).DeleteAllCultures();
                //var UserRegistry = Applicant applicationsRepository.GetApplicationDetailsByID(applicantModel.ApplicationDetailsID);
                //UserRegistry.DocumentPublishTo = DateTime.Now;
                //UserRegistry.Update();
            }

            return Json(new[] { applicantModel }.ToDataSourceResult(request, ModelState));
        }

        #endregion
        #region RelatedParty
        public ActionResult RelatedParty(int applicationId, int relatedPartyId)
        {
            RelatedPartyModel model = new RelatedPartyModel();
            if (applicationId == 0 && relatedPartyId == 0)
                return Redirect("/Applications");
            //var applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);
            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationId);
            if (applicationDetails == null && relatedPartyId == 0)
                return Redirect("/Applications");
            if (applicationDetails != null)
            {
                model.ApplicationNumber = applicationDetails.ApplicationDetails_ApplicationNumber;
            }
            if (relatedPartyId > 0)
            {
                model.PersonalDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(relatedPartyId);
                model.Id = relatedPartyId;
            }
            else
            {
                model.PersonalDetails = new PersonalDetailsModel();
            }


            ViewBag.PersonTitle = ServiceHelper.GetTitle();
            ViewBag.Gender = ServiceHelper.GetGendar();
            ViewBag.Education = ServiceHelper.GetEducationLevel();
            //ViewBag.Country = ServiceHelper.GetCountries();
            ViewBag.Country = ServiceHelper.GetCountriesWithID();
            return View(model);
        }
        [HttpPost]
        public ActionResult RelatedParty(int applicationId, RelatedPartyModel model)
        {
            if (!ModelState.IsValid || model == null || (string.IsNullOrEmpty(model.ApplicationNumber) && (model == null || model.PersonalDetails.Id == 0)))
            {
                return View(model);
            }
            else
            {
                if (model.PersonalDetails != null)
                {
                    model.PersonalDetails = PersonalDetailsProcess.SaveRelatedPartyPersonalDetailsModel(model.ApplicationNumber, model.PersonalDetails);
                    return Redirect("/Applications");
                }
            }
            return View(model);
        }

        public IActionResult RelatedParty_Read([DataSourceRequest] DataSourceRequest request)
        {
            TempData.Keep("ApplicationID");
            int applicationID = ValidationHelper.GetInteger(TempData["ApplicationID"], 0);
            List<RelatedPartyModel> applicantModels = null;
            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationID);
            string applicationType = string.Empty;
            var applicationTypes = ServiceHelper.GetApplicationType();

            if (applicationDetails != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationNumber))
            {
                applicationType = (applicationTypes != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationType) && applicationTypes.Any(k => string.Equals(k.Value, applicationDetails.ApplicationDetails_ApplicationType, StringComparison.OrdinalIgnoreCase))) ? applicationTypes.FirstOrDefault(k => string.Equals(k.Value, applicationDetails.ApplicationDetails_ApplicationType, StringComparison.OrdinalIgnoreCase)).Text : string.Empty;
                if (!string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicantModels = RelatedPartyProcess.GetRelatedPartyModels(applicationDetails.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicantModels = RelatedPartyProcess.GetLegalRelatedPartyModels(applicationDetails.ApplicationDetails_ApplicationNumber);
                }


                //int rowid = 0;
                //foreach (var item in result)
                //{
                //    rowid++;
                //    RelatedPartyModel relatedPartyModel = new RelatedPartyModel();
                //    relatedPartyModel.rowID = item.rowID;
                //    relatedPartyModel.FullName = item.FullName;
                //    relatedPartyModel.Role = item.Role;
                //    relatedPartyModel.Invited = item.Invited;
                //    relatedPartyModel.IdVerified = item.IdVerified;
                //    relatedPartyModel.Status = item.Status;
                //    applicantModels.Add(relatedPartyModel);
                //}

            }
            if (applicantModels == null)
                applicantModels = new List<RelatedPartyModel>();

            return Json(applicantModels.ToDataSourceResult(request));
        }

        public ActionResult RelatedPartyPopup_Destroy([DataSourceRequest] DataSourceRequest request, ApplicantModel applicantModel)
        {
            if (applicantModel != null)
            {
                var applicationDetails = applicationsRepository.GetApplicationDetailsByApplicationNumber(applicantModel.ApplicationNumber);
                if (applicantModel.CompanyDetails != null)
                {
                    //var UserRegistry = companyDetailsRepository.GetCompanyDetailsByID(applicantModel.CompanyDetails.Id);
                    var UserRegistry = CompanyDetailsRelatedPartyProcess.GetCompanyDetailsRelatedPartyById(applicantModel.CompanyDetails.Id);
                    UserRegistry.DocumentPublishTo = DateTime.Now;
                    UserRegistry.Update();

                    // Expected Document Delete
                    var expectedDocument = expectedDocumentsRepository.GetExpectedDocuments(applicationDetails.ApplicationDetailsID).Where(x => x.ExpectedDocuments_Entity.ToString() == applicantModel.CompanyDetails.NodeGUID).ToList();
                    if (expectedDocument != null && expectedDocument.Count > 0)
                    {
                        foreach (var item in expectedDocument)
                        {
                            item.Delete();
                        }
                    }
                    //Bank Document Delete
                    var bankDocument = bankDocumentsRepository.GetBankDocuments(applicationDetails.ApplicationDetailsID).Where(x => x.BankDocuments_Entity.ToString() == applicantModel.CompanyDetails.NodeGUID).ToList();
                    if (bankDocument != null && bankDocument.Count > 0)
                    {
                        foreach (var item in bankDocument)
                        {
                            item.Delete();
                        }
                    }
                }
                if (applicantModel.PersonalDetails != null)
                {
                    var UserRegistry = personalDetailsRepository.GetPersonalDetailsByID(applicantModel.PersonalDetails.Id);
                    UserRegistry.DocumentPublishTo = DateTime.Now;
                    UserRegistry.Update();

                    // Expected Document Delete
                    var expectedDocument = expectedDocumentsRepository.GetExpectedDocuments(applicationDetails.ApplicationDetailsID).Where(x => x.ExpectedDocuments_Entity.ToString() == applicantModel.PersonalDetails.NodeGUID).ToList();
                    if (expectedDocument != null && expectedDocument.Count > 0)
                    {
                        foreach (var item in expectedDocument)
                        {
                            item.Delete();
                        }
                    }
                    //Bank Document Delete
                    var bankDocument = bankDocumentsRepository.GetBankDocuments(applicationDetails.ApplicationDetailsID).Where(x => x.BankDocuments_Entity.ToString() == applicantModel.PersonalDetails.NodeGUID).ToList();
                    if (bankDocument != null && bankDocument.Count > 0)
                    {
                        foreach (var item in bankDocument)
                        {
                            item.Delete();
                        }
                    }
                }
            }
            //if(applicantModel != null)
            //{
            //	sourceOfOutgoingTransactionsRepository.GetSourceOfOutgoingTransactions(applicantModel.Id).DeleteAllCultures();
            //}

            return Json(new[] { applicantModel }.ToDataSourceResult(request, ModelState));
        }
        #endregion
        #region Accounts
        public IActionResult Accounts_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            int applicationID = ValidationHelper.GetInteger(id, 0);
            var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
            List<AccountsDetailsViewModel> accountsDetailsViewModels = new List<AccountsDetailsViewModel>();
            var accounts = accountsRepository.GetAccounts(applicatioData.NodeAliasPath);
            if (accounts != null)
            {
                accountsDetailsViewModels = AccountsProcess.GetAccounts(accounts);
            }
            return Json(accountsDetailsViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult AccountsPopup_Create([DataSourceRequest] DataSourceRequest request, AccountsDetailsViewModel accountsDetailsViewModel, int id)
        {
            int applicationID = ValidationHelper.GetInteger(id, 0);
            var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
            var accounts = accountsRepository.CheckAccount(applicatioData.NodeAliasPath, accountsDetailsViewModel.Accounts_Currency, accountsDetailsViewModel.Accounts_AccountType);
            if (accounts != null)
            {
                ModelState.AddModelError("AlreadyExists", "Account already exists!");
            }
            if (accountsDetailsViewModel.Accounts_Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateAccountDetails(accountsDetailsViewModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (accountsDetailsViewModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(accountsDetailsViewModel))
            {
                accountsDetailsViewModel = AccountsProcess.SaveAccountsModel(accountsDetailsViewModel, applicatioData);

            }

            return Json(new[] { accountsDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult AccountsPopup_Update([DataSourceRequest] DataSourceRequest request, AccountsDetailsViewModel accountsDetailsViewModel, int id)
        {
            int applicationID = ValidationHelper.GetInteger(id, 0);
            var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
            if (accountsDetailsViewModel.Accounts_Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateAccountDetails(accountsDetailsViewModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            var GetAccountDetails = accountsRepository.GetAccountsDetailsByID(accountsDetailsViewModel.AccountsID);
            if (accountsDetailsViewModel != null && ModelState.IsValid)
            {
                var successData = AccountsProcess.UpdateAccountsModel(accountsDetailsViewModel, GetAccountDetails);
                accountsDetailsViewModel.Account_Status_Name = successData.Account_Status_Name;
                accountsDetailsViewModel.AccountsID = successData.AccountsID;
                accountsDetailsViewModel.Accounts_AccountTypeName = successData.Accounts_AccountTypeName;
                accountsDetailsViewModel.Accounts_CurrencyName = successData.Accounts_CurrencyName;
                accountsDetailsViewModel.Accounts_StatementFrequencyName = successData.Accounts_StatementFrequencyName;
            }

            return Json(new[] { accountsDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult AccountsPopup_Destroy([DataSourceRequest] DataSourceRequest request, AccountsDetailsViewModel accountsDetailsViewModel)
        {
            if (accountsDetailsViewModel != null)
            {
                accountsRepository.GetAccountsDetailsByID(accountsDetailsViewModel.AccountsID).DeleteAllCultures();
            }

            return Json(new[] { accountsDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }
        public JsonResult TypeofAccount_Read()
        {
            var result = ServiceHelper.GetTypeofAccounts();
            return Json(result);
        }
        public JsonResult Accounts_Currency_Read()
        {
            var result = ServiceHelper.GetCurrency();
            return Json(result);
        }
        public JsonResult Accounts_StatementFrequency_Read()
        {
            var result = ServiceHelper.GetSTATEMENTFREQUENCY();
            return Json(result);
        }
        public ActionResult IsAccountUseInCard(int id, string applicationType, string editCurrency)
        {
            string isUsed = "False";
            int applicationID = ValidationHelper.GetInteger(id, 0);
            List<DebitCardDetailsViewModel> debitCardDetailsViewModels = new List<DebitCardDetailsViewModel>();
            var debitcards = debitCardDetailsRepository.GetDebitCards(applicationID);
            if (debitcards != null)
            {
                debitCardDetailsViewModels = DebitCardeDetailsProcess.GetDebitCardDetails(debitcards, applicationType, applicationID);
                if (debitCardDetailsViewModels != null)
                {
                    if (debitCardDetailsViewModels.Any(s => s.AssociatedAccountName.Contains(editCurrency.ToUpper())))
                    {
                        isUsed = "True";
                    }
                }
            }
            return Json(isUsed);
        }
        #endregion
        #region Debit Card Details
        public IActionResult DebitCardDetails_Read([DataSourceRequest] DataSourceRequest request, int id, string applicationType)
        {
            int applicationID = ValidationHelper.GetInteger(id, 0);
            List<DebitCardDetailsViewModel> debitCardDetailsViewModels = new List<DebitCardDetailsViewModel>();
            var debitcards = debitCardDetailsRepository.GetDebitCards(applicationID);
            if (debitcards != null)
            {
                debitCardDetailsViewModels = DebitCardeDetailsProcess.GetDebitCardDetails(debitcards, applicationType, applicationID);
            }
            return Json(debitCardDetailsViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult DebitCardDetailsPopup_Create([DataSourceRequest] DataSourceRequest request, DebitCardDetailsViewModel debitCardDetailsViewModel, int id, string applicationType)
        {
            int applicationID = ValidationHelper.GetInteger(id, 0);
            bool isLegalEntity = string.Equals(applicationType, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase);
            if (debitCardDetailsViewModel.DebitCardDetails_Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateDebitCardDetails(debitCardDetailsViewModel, applicationType, applicationID);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }

            }

            if (debitCardDetailsViewModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(debitCardDetailsViewModel))
            {
                var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                var successData = DebitCardeDetailsProcess.SaveDebitCardDetailsModel(debitCardDetailsViewModel, applicatioData, isLegalEntity);
                debitCardDetailsViewModel.DebitCardDetailsID = successData.DebitCardDetailsID;
                debitCardDetailsViewModel.DebitCardDetails_DispatchMethodName = successData.DebitCardDetails_DispatchMethodName;
                debitCardDetailsViewModel.DebitCardDetails_DispatchMethod = successData.DebitCardDetails_DispatchMethod;
                debitCardDetailsViewModel.DebitCardDetails_CardTypeName = successData.DebitCardDetails_CardTypeName;
                debitCardDetailsViewModel.DebitCardDetails_Status = successData.DebitCardDetails_Status;
                debitCardDetailsViewModel.DebitCardDetails_StatusName = successData.DebitCardDetails_StatusName;
                debitCardDetailsViewModel.DebitCardDetails_DeliveryAddress = successData.DebitCardDetails_DeliveryAddress;
                debitCardDetailsViewModel.DebitCardDetails_OtherDeliveryAddress = successData.DebitCardDetails_OtherDeliveryAddress;

            }

            return Json(new[] { debitCardDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult DebitCardDetailsPopup_Update([DataSourceRequest] DataSourceRequest request, DebitCardDetailsViewModel debitCardDetailsViewModel, int id, string applicationType)
        {
            int applicationID = ValidationHelper.GetInteger(id, 0);
            bool isLegalEntity = string.Equals(applicationType, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase);
            if (debitCardDetailsViewModel.DebitCardDetails_Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateDebitCardDetails(debitCardDetailsViewModel, applicationType, applicationID);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }

            }
            if (debitCardDetailsViewModel != null && ModelState.IsValid)
            {
                var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                var cardDetailsData = debitCardDetailsRepository.GetDebitsCardsDetailsByID(debitCardDetailsViewModel.DebitCardDetailsID);
                var successData = DebitCardeDetailsProcess.UpdateDebitCardDetailsModel(debitCardDetailsViewModel, cardDetailsData, isLegalEntity);

                debitCardDetailsViewModel.DebitCardDetails_DispatchMethodName = successData.DebitCardDetails_DispatchMethodName;
                debitCardDetailsViewModel.DebitCardDetails_CardTypeName = successData.DebitCardDetails_CardTypeName;
                debitCardDetailsViewModel.DebitCardDetails_Status = successData.DebitCardDetails_Status;
                debitCardDetailsViewModel.DebitCardDetails_StatusName = successData.DebitCardDetails_StatusName;
            }

            return Json(new[] { debitCardDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult DebitCardDetailsPopup_Destroy([DataSourceRequest] DataSourceRequest request, DebitCardDetailsViewModel debitCardDetailsViewModel)
        {
            if (debitCardDetailsViewModel != null)
            {
                debitCardDetailsRepository.GetDebitsCardsDetailsByID(debitCardDetailsViewModel.DebitCardDetailsID).DeleteAllCultures();
            }

            return Json(new[] { debitCardDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult DebitCardDestoryAll([DataSourceRequest] DataSourceRequest request, int id)
        {
            int applicationId = ValidationHelper.GetInteger(id, 0);
            var cards = debitCardDetailsRepository.GetDebitCards(applicationId);
            if (cards != null)
            {
                foreach (var item in cards)
                {
                    item.DeleteAllCultures();
                }
            }
            return Json("True");
        }
        public ActionResult DebitCardStatusChange([DataSourceRequest] DataSourceRequest request, int id, string usedCurrency)
        {
            int applicationId = ValidationHelper.GetInteger(id, 0);
            var cards = debitCardDetailsRepository.GetDebitCards(applicationId);
            if (cards != null)
            {
                string AssociatedAccountName = string.Empty;
                var associatedAccount = AccountsProcess.GetDebitCardAccountsByApplicationID(applicationId);
                foreach (var item in cards)
                {
                    if (associatedAccount != null && !string.IsNullOrEmpty(item.AssociatedAccount))
                    {
                        AssociatedAccountName = associatedAccount.Where(x => x.Value == item.AssociatedAccount).Select(x => x.Text).FirstOrDefault();
                    }
                    if (string.Equals(usedCurrency, AssociatedAccountName, StringComparison.OrdinalIgnoreCase))
                    {
                        item.AssociatedAccount = string.Empty;
                        item.DebitCardDetails_Status = false;
                        item.Update();
                    }
                }
            }
            return Json("True");
        }
        public JsonResult AssociatedAccount_Read()
        {
            TempData.Keep("ApplicationID");
            int applicationID = ValidationHelper.GetInteger(TempData["ApplicationID"], 0);
            var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
            var accounts = accountsRepository.GetAccounts(applicatioData.NodeAliasPath);
            var result = ServiceHelper.GetAssociatedAccount(accounts);
            return Json(result);
        }
        public JsonResult CardType_Read()
        {
            var result = ServiceHelper.GetCardType();
            return Json(result);
        }

        public JsonResult AccessToAllPersonalAccount_Read()
        {
            var result = ServiceHelper.GetBoolDropDownListDefaults();
            return Json(result);
        }

        public JsonResult LimitAmount_Read()
        {
            var result = ServiceHelper.GetLimitAmount();
            return Json(result);
        }

        public JsonResult DispatchMethod_Read()
        {
            var result = ServiceHelper.GetDispatchMethod();
            return Json(result);
        }
        public JsonResult Individual_DispatchMethod_Read()
        {
            var result = ServiceHelper.GetDispatchMethodIndividual();
            return Json(result);
        }
        public JsonResult DeliveryAddress_Read()
        {
            var result = ServiceHelper.GetDeliveryAddress();
            return Json(result);
        }
        public JsonResult CollectedBy_Read()
        {

            var result = ServiceHelper.GetCollectedBy();
            return Json(result);
        }

        public JsonResult CardHolderNameRead(string applicationNumber)
        {

            var result = CommonProcess.GetCardHolderNameIndividual(applicationNumber);
            return Json(result);
        }
        public JsonResult CardHolderNameLegalRead(string applicationNumber)
        {

            var result = CommonProcess.GetCardHolderNameIndividual(applicationNumber);
            return Json(result);
        }
        public JsonResult Country_Code_prefix_Read()
        {
            var result = ServiceHelper.GetCountryCodePrefix(); //ServiceHelper.GetCountriesWithID();
            return Json(result);
        }
        public JsonResult AssociatedAccountType_Read(int applicationId)
        {
            List<SelectListItem> retVal = null;
            var debitCardAccounts = AccountsProcess.GetDebitCardAccountsByApplicationID(applicationId);
            if (debitCardAccounts != null)
            {
                retVal = debitCardAccounts;
            }
            return Json(retVal);
            //var result = ServiceHelper.GetAssociatedAccountType();
            //return Json(result);
        }

        public JsonResult GetIdentityCard(string relatedPartyGuid)
        {
            IdentityCardModel result = null;

            if (!string.IsNullOrEmpty(relatedPartyGuid))
            {
                result = IdentificationDetailsProcess.GetIdentityCard(relatedPartyGuid);
            }

            return Json(result);
        }

        //public JsonResult GetIdentityCardLegal(string relatedPartyGuid)
        //{
        //    IdentityCardModel result = null;

        //    if(!string.IsNullOrEmpty(relatedPartyGuid))
        //    {
        //        IdentificationDetailsProcess.GetIdentityCardLegal(relatedPartyGuid);
        //    }

        //    return Json(result);
        //}
        #endregion

        #region Bank Documents
        public ActionResult AddNewBankDouments(int id, string entity)
        {
            int applicationID = ValidationHelper.GetInteger(id, 0);
            var bankDocuments = bankDocumentsRepository.GetBankDocuments(applicationID);
            IEnumerable<PersonalAndJointAccount_BankDocumentInfo> bankDoucumetsModulesIndividual = null;
            IEnumerable<CorporateAccount_BankDocumentInfo> bankDoucumetsModulesCorporate = null;
            if (entity == Constants.LegalEntity)
            {
                bankDoucumetsModulesCorporate = bankDocumentsRepository.GetCorporateDocumentsModules();
                foreach (var item in bankDoucumetsModulesCorporate)
                {
                    var checkBankDocuments = bankDocuments.ToList().Where(i => i.BankDocuments_EntityType == item.CorporateAccount_BankDocument_Type && i.BankDocuments_DocumentType == item.CorporateAccount_BankDocument_BankDocumentType);
                    if (checkBankDocuments.Count() == 0)
                    {
                        DocumentsViewModel documentsViewModel = new DocumentsViewModel();
                        var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                        documentsViewModel.Entity = ValidationHelper.GetString(item.CorporateAccount_BankDocument_PersonType, "");
                        documentsViewModel.EntityType = ValidationHelper.GetString(item.CorporateAccount_BankDocument_Type, "");
                        documentsViewModel.EntityRole = ValidationHelper.GetString(item.CorporateAccount_BankDocument_PersonRole, "");
                        documentsViewModel.DocumentType = ValidationHelper.GetString(item.CorporateAccount_BankDocument_BankDocumentType, "");
                        documentsViewModel.EntityTypeSection = entity;
                        DocumentsProcess.SaveBankDocumentsModel(documentsViewModel, applicatioData, "", "");
                    }
                }
            }
            else
            {
                bankDoucumetsModulesIndividual = bankDocumentsRepository.GetBankDocumentsModules();
                foreach (var item in bankDoucumetsModulesIndividual)
                {
                    var checkBankDocuments = bankDocuments.ToList().Where(i => i.BankDocuments_EntityType == item.PersonalAndJointAccount_BankDocument_Type && i.BankDocuments_DocumentType == item.PersonalAndJointAccount_BankDocument_BankDocumentType);
                    if (checkBankDocuments.Count() == 0)
                    {
                        DocumentsViewModel documentsViewModel = new DocumentsViewModel();
                        var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                        documentsViewModel.Entity = ValidationHelper.GetString(item.PersonalAndJointAccount_BankDocument_PersonType, "");
                        documentsViewModel.EntityType = ValidationHelper.GetString(item.PersonalAndJointAccount_BankDocument_Type, "");
                        documentsViewModel.EntityRole = ValidationHelper.GetString(item.PersonalAndJointAccount_BankDocument_PersonRole, "");
                        documentsViewModel.DocumentType = ValidationHelper.GetString(item.PersonalAndJointAccount_BankDocument_BankDocumentType, "");
                        DocumentsProcess.SaveBankDocumentsModel(documentsViewModel, applicatioData, "", "");
                    }
                }

            }

            return RedirectToAction("Edit", "Applications", new { id = applicationID });
        }
        public IActionResult BankDocuments_Read([DataSourceRequest] DataSourceRequest request, int id, string entityType, string applicationNumber)
        {
            int applicationID = ValidationHelper.GetInteger(id, 0);
            //         var bankDocuments = bankDocumentsRepository.GetBankDocuments(applicationID);

            //         var bankDoucumetsModules = bankDocumentsRepository.GetBankDocumentsModules();

            //         foreach(var item in bankDoucumetsModules)
            //{
            //             var checkBankDocuments = bankDocuments.ToList().Where(i => i.BankDocuments_EntityType == item.PersonalAndJointAccount_BankDocument_Type && i.BankDocuments_DocumentType == item.PersonalAndJointAccount_BankDocument_BankDocumentType);


            //		if(checkBankDocuments.Count()==0)
            //		{
            //                     DocumentsViewModel documentsViewModel = new DocumentsViewModel();
            //                     var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
            //                     documentsViewModel.Entity = ValidationHelper.GetString(item.PersonalAndJointAccount_BankDocument_PersonType, "");
            //                     documentsViewModel.EntityType = ValidationHelper.GetString(item.PersonalAndJointAccount_BankDocument_Type, "");
            //                     documentsViewModel.EntityRole = ValidationHelper.GetString(item.PersonalAndJointAccount_BankDocument_PersonRole, "");
            //                     documentsViewModel.DocumentType = ValidationHelper.GetString(item.PersonalAndJointAccount_BankDocument_BankDocumentType, "");
            //                     DocumentsProcess.SaveBankDocumentsModel(documentsViewModel, applicatioData);
            //                 }


            //         }
            var bankDocumentsList = bankDocumentsRepository.GetBankDocuments(applicationID);
            List<DocumentsViewModel> documentsViewModels = new List<DocumentsViewModel>();
            if (bankDocumentsList != null)
            {
                documentsViewModels = DocumentsProcess.GetBankDocuments(bankDocumentsList, entityType, applicationNumber);
            }
            return Json(documentsViewModels.ToDataSourceResult(request));
        }

        public static string GetFilename(IFormFile file)
        {
            return ContentDispositionHeaderValue.Parse(
                            file.ContentDisposition).FileName.ToString().Trim('"');
        }
        [HttpPost]
        public ActionResult BankDocumentsPopup_Create([DataSourceRequest] DataSourceRequest request, DocumentsViewModel documentsViewModel, int id, IEnumerable<IFormFile> files, string applicationNumber, string applicationType)
        {
            try
            {
                documentsViewModel.EntityType = documentsViewModel.EntityRole1;
                if (string.IsNullOrEmpty(documentsViewModel.Entity))
                {
                    ModelState.AddModelError("Entity", ResHelper.GetString(DocumentsViewModelErrorMessage.Entity));
                }
                if (string.IsNullOrEmpty(documentsViewModel.EntityType))
                {
                    ModelState.AddModelError("EntityType", ResHelper.GetString(DocumentsViewModelErrorMessage.EntityTypeError));
                }
                if (string.IsNullOrEmpty(documentsViewModel.DocumentType))
                {
                    ModelState.AddModelError("DocumentType", ResHelper.GetString(DocumentsViewModelErrorMessage.DocumentTypeError));
                }
                string file = httpContextAccessor.HttpContext.Session.GetString("DocumentsFileName");
                documentsViewModel.FileUpload = file;
                //httpContextAccessor.HttpContext.Session.Remove("DocumentsFileName");
                if (!string.IsNullOrEmpty(file) && file.LastIndexOf("\\") > 0)
                {
                    documentsViewModel.FileName = file.Substring(file.LastIndexOf("\\") + 1);
                }
                //External Upload

                string externalFileGuid = httpContextAccessor.HttpContext.Session.GetString("DocumentFileGuid");
                string uploadedFileName = httpContextAccessor.HttpContext.Session.GetString("UploadedFileName");
                documentsViewModel.ExternalFileGuid = externalFileGuid;
                if (!string.IsNullOrEmpty(uploadedFileName))
                {
                    documentsViewModel.UploadFileName = uploadedFileName.Replace("\"", "");
                }

                //httpContextAccessor.HttpContext.Session.Remove("DocumentFileGuid");
                //httpContextAccessor.HttpContext.Session.Remove("UploadedFileName");

                //External Upload
                int applicationID = ValidationHelper.GetInteger(id, 0);
                if (documentsViewModel.BankDocuments_Status)
                {
                    if (string.IsNullOrEmpty(documentsViewModel.FileUpload))
                    {
                        ModelState.AddModelError("Consent", "Please upload the documents!");
                    }
                }
                if (documentsViewModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(documentsViewModel))
                {
                    var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                    documentsViewModel = DocumentsProcess.SaveBankDocumentsModel(documentsViewModel, applicatioData, applicationNumber, applicationType);
                    httpContextAccessor.HttpContext.Session.Remove("DocumentsFileName");
                    httpContextAccessor.HttpContext.Session.Remove("DocumentFileGuid");
                    httpContextAccessor.HttpContext.Session.Remove("UploadedFileName");
                }

                return Json(new[] { documentsViewModel }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "ErrorLog/ExceptionLog.txt");
                if (!System.IO.File.Exists(filepath))
                {
                    System.IO.File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = System.IO.File.AppendText(filepath))
                {
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(ex.ToString());
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.Flush();
                    sw.Close();
                }
                return Json("");
            }
        }
        [HttpPost]
        public ActionResult BankDocumentsPopup_Update([DataSourceRequest] DataSourceRequest request, DocumentsViewModel documentsViewModel, int id, IEnumerable<IFormFile> files, string applicationNumber, string applicationType)
        {
            try
            {
                documentsViewModel.EntityType = documentsViewModel.EntityRole1;
                if (string.IsNullOrEmpty(documentsViewModel.Entity))
                {
                    ModelState.AddModelError("Entity", ResHelper.GetString(DocumentsViewModelErrorMessage.Entity));
                }
                if (string.IsNullOrEmpty(documentsViewModel.EntityType))
                {
                    ModelState.AddModelError("EntityType", ResHelper.GetString(DocumentsViewModelErrorMessage.EntityTypeError));
                }
                if (string.IsNullOrEmpty(documentsViewModel.DocumentType))
                {
                    ModelState.AddModelError("DocumentType", ResHelper.GetString(DocumentsViewModelErrorMessage.DocumentTypeError));
                }
                string file = httpContextAccessor.HttpContext.Session.GetString("DocumentsFileName");
                if (!string.IsNullOrEmpty(file))
                {
                    documentsViewModel.FileUpload = file;
                }
                //httpContextAccessor.HttpContext.Session.Remove("DocumentsFileName");
                if (!string.IsNullOrEmpty(file) && file.LastIndexOf("\\") > 0)
                {
                    documentsViewModel.FileName = file.Substring(file.LastIndexOf("\\") + 1);
                }
                //External Upload

                string externalFileGuid = httpContextAccessor.HttpContext.Session.GetString("DocumentFileGuid");
                string uploadedFileName = httpContextAccessor.HttpContext.Session.GetString("UploadedFileName");
                if (!string.IsNullOrEmpty(externalFileGuid))
                {
                    documentsViewModel.ExternalFileGuid = externalFileGuid;
                }

                if (!string.IsNullOrEmpty(uploadedFileName))
                {
                    documentsViewModel.UploadFileName = uploadedFileName.Replace("\"", "");
                }
                //httpContextAccessor.HttpContext.Session.Remove("DocumentFileGuid");
                //httpContextAccessor.HttpContext.Session.Remove("UploadedFileName");

                //External Upload
                int applicationID = ValidationHelper.GetInteger(id, 0);
                if (documentsViewModel.BankDocuments_Status)
                {
                    if (string.IsNullOrEmpty(documentsViewModel.FileUpload))
                    {
                        ModelState.AddModelError("Consent", "Please upload the documents!");
                    }
                }
                if (documentsViewModel != null && ModelState.IsValid)
                {
                    var bankData = bankDocumentsRepository.GetBankDocumentsByID(documentsViewModel.DocId);
                    documentsViewModel = DocumentsProcess.UpdateBankDocumentsModel(documentsViewModel, bankData, applicationNumber, applicationType);
                    httpContextAccessor.HttpContext.Session.Remove("DocumentsFileName");
                    httpContextAccessor.HttpContext.Session.Remove("DocumentFileGuid");
                    httpContextAccessor.HttpContext.Session.Remove("UploadedFileName");
                }

                return Json(new[] { documentsViewModel }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "ErrorLog/ExceptionLog.txt");
                if (!System.IO.File.Exists(filepath))
                {
                    System.IO.File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = System.IO.File.AppendText(filepath))
                {
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(ex.ToString());
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.Flush();
                    sw.Close();
                }
                return Json("");
            }
        }
        [HttpPost]
        public ActionResult BankDocumentsPopup_Destroy([DataSourceRequest] DataSourceRequest request, DocumentsViewModel documentsViewModel)
        {
            if (documentsViewModel != null)
            {
                bankDocumentsRepository.GetBankDocumentsByID(documentsViewModel.DocId).DeleteAllCultures();
            }

            return Json(new[] { documentsViewModel }.ToDataSourceResult(request, ModelState));
        }


        public JsonResult Entity_Read()
        {
            var result = ServiceHelper.GetEntity();
            return Json(result);
        }
        public JsonResult CorporateAccount_Read()
        {
            var result = ServiceHelper.GetCommonDropDown(Constants.CORPORATEACCOUNT_TYPE);
            return Json(result);
        }

        public JsonResult LegalBankDocumentType_Read()
        {
            var result = ServiceHelper.GetCommonDropDown(Constants.LegalBANK_DOCUMENT_TYPE);
            return Json(result);
        }
        public JsonResult LegalEntity_Read()
        {
            var result = ServiceHelper.GetCommonDropDown(Constants.LEGALPERSON_TYPE);
            return Json(result);
        }
        #endregion

        #region E Banking Subscribers

        public IActionResult EBankingSubscriberDetail_Read([DataSourceRequest] DataSourceRequest request, bool isLegalEntity)
        {

            TempData.Keep("ApplicationID");
            int applicationID = ValidationHelper.GetInteger(TempData["ApplicationID"], 0);
            List<EBankingSubscriberDetailsModel> eBankingSubscriberDetailsViewModels = new List<EBankingSubscriberDetailsModel>();
            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationID);

            if (applicationDetails != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationNumber))
            {
                eBankingSubscriberDetailsViewModels = EBankingSubscriberDetailsProcess.GetEBankingSubscriberDetailsModels(applicationDetails.ApplicationDetails_ApplicationNumber, isLegalEntity);
            }

            return Json(eBankingSubscriberDetailsViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult EBankingSubscriberDetailPopup_Create([DataSourceRequest] DataSourceRequest request, EBankingSubscriberDetailsModel eBankingSubscriberDetailsModel, int apID, bool isLegalEntity)
        {

            int applicationID = ValidationHelper.GetInteger(apID, 0);
            if (eBankingSubscriberDetailsModel.EbankingSubscriberDetails_Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateEBankingSuscriber(eBankingSubscriberDetailsModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (eBankingSubscriberDetailsModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(eBankingSubscriberDetailsModel))
            {
                var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);

                if (applicatioData != null && !string.IsNullOrEmpty(applicatioData.ApplicationDetails_ApplicationNumber))
                {
                    var eBankingSubscriberDetails = EBankingSubscriberDetailsProcess.SaveEBankingSubscriberDetailsModel(applicatioData.ApplicationDetails_ApplicationNumber, eBankingSubscriberDetailsModel, isLegalEntity);
                    if (eBankingSubscriberDetails != null)
                    {
                        //*******Added a pending record in signature mandate grid*******
                        //if (isLegalEntity)
                        //{
                        //    if (eBankingSubscriberDetails.SignatoryGroup == "AUTHORISED PERSONS")
                        //    {
                        //        var result = ServiceHelper.GetMandateType();

                        //        SignatureMandateCompanyModel signatureMandateCompanyModel = new SignatureMandateCompanyModel();
                        //        signatureMandateCompanyModel.AuthorizedSignatoryGroup = eBankingSubscriberDetails.SignatoryGroupName;
                        //        signatureMandateCompanyModel.Status = false;
                        //        signatureMandateCompanyModel.MandateType = (result != null && result.Any(k => k.Text == "FOR EBANKING TRANSACTIONS") ? result.FirstOrDefault(k => k.Text == "FOR EBANKING TRANSACTIONS").Value : string.Empty);

                        //        var signatureMandateCompany = SignatureMandateLegalProcess.SaveSignatureMandateLegalModel(applicatioData.ApplicationDetails_ApplicationNumber, signatureMandateCompanyModel, applicationID);
                        //    }
                        //}
                        //*****************************
                        eBankingSubscriberDetailsModel = eBankingSubscriberDetails;
                    }
                }
            }
            return Json(new[] { eBankingSubscriberDetailsModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult EBankingSubscriberDetailPopup_Update([DataSourceRequest] DataSourceRequest request, EBankingSubscriberDetailsModel eBankingSubscriberDetailsModel, int apID, bool isLegalEntity)
        {
            int applicationID = ValidationHelper.GetInteger(apID, 0);
            if (eBankingSubscriberDetailsModel.EbankingSubscriberDetails_Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateEBankingSuscriber(eBankingSubscriberDetailsModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (eBankingSubscriberDetailsModel != null && ModelState.IsValid)
            {
                var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                var incommingTransactionData = eBankingSubscriberDetailsRepository.GetEBankingSubscriberDetail(eBankingSubscriberDetailsModel.Id);
                if (incommingTransactionData != null)
                {
                    var eBankingSubscriberDetails = EBankingSubscriberDetailsProcess.SaveEBankingSubscriberDetailsModel(applicatioData.ApplicationDetails_ApplicationNumber, eBankingSubscriberDetailsModel, isLegalEntity);
                    if (eBankingSubscriberDetails != null)
                        eBankingSubscriberDetailsModel = eBankingSubscriberDetails;
                }
            }

            return Json(new[] { eBankingSubscriberDetailsModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult EBankingSubscriberDetailPopup_Destroy([DataSourceRequest] DataSourceRequest request, EBankingSubscriberDetailsModel eBankingSubscriberDetailsModel)
        {
            if (eBankingSubscriberDetailsModel != null)
            {
                eBankingSubscriberDetailsRepository.GetEBankingSubscriberDetail(eBankingSubscriberDetailsModel.Id).DeleteAllCultures();
            }

            return Json(new[] { eBankingSubscriberDetailsModel }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult EbankingDestroyAll(int id)
        {
            int applicationId = ValidationHelper.GetInteger(id, 0);
            var ebanking = eBankingSubscriberDetailsRepository.GetEBankingSubscriberDetails(applicationId);
            if (ebanking != null)
            {
                foreach (var item in ebanking)
                {
                    item.DeleteAllCultures();
                }
            }
            return Json("True");
        }
        public JsonResult GetpersonType(string personGUID)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(personGUID))
            {
                result = CommonProcess.GetPersonType(personGUID);
            }
            return Json(result);
        }
        public JsonResult GetSignatoryGroupForEBanking(string applicationNumber)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(applicationNumber))
            {
                result = CommonProcess.GetEbankingSignatoryGroupLegal(applicationNumber);
            }
            return Json(result);
        }

        public JsonResult GetSignatoryGroupForEBankingBySeletedPerson(int applicationId, string applicationNumber, string selectedPerson)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            List<SignatoryGroupModel> signatoryGroupViewModels = SignatoryGroupProcess.GetApplicationSignatoryGroup(applicationId);
            if (!string.IsNullOrEmpty(applicationNumber) && signatoryGroupViewModels != null && signatoryGroupViewModels.Count > 0 && signatoryGroupViewModels.Any(y => y.SignatoryPersonsList.Any(r => string.Equals(r, selectedPerson, StringComparison.OrdinalIgnoreCase))))
            {
                signatoryGroupViewModels = signatoryGroupViewModels.Where(y => y.SignatoryPersonsList.Any(r => string.Equals(r, selectedPerson, StringComparison.OrdinalIgnoreCase))).ToList();
                result = CommonProcess.GetEbankingSignatoryGroupLegal(applicationNumber);
                result = result.Where(y => signatoryGroupViewModels.Any(t => string.Equals(y.Value, t.SignatoryGroupName, StringComparison.OrdinalIgnoreCase))).ToList();
            }
            return Json(result);
        }
        #endregion

        #region Note Details

        public IActionResult NoteDetails_Read([DataSourceRequest] DataSourceRequest request)
        {

            TempData.Keep("ApplicationID");
            int applicationID = ValidationHelper.GetInteger(TempData["ApplicationID"], 0);
            List<NoteDetailsModel> noteDetailsViewModels = new List<NoteDetailsModel>();
            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationID);

            if (applicationDetails != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationNumber))
            {
                var result = NoteDetailsProcess.GetNoteDetailsModels(applicationDetails.ApplicationDetails_ApplicationNumber);
                int rowid = 0;
                foreach (var item in result)
                {
                    rowid++;
                    NoteDetailsModel noteDetailsModel = new NoteDetailsModel();
                    noteDetailsModel.rowID = rowid;
                    noteDetailsModel.Id = item.Id;
                    noteDetailsModel.NoteDetailsType = item.NoteDetailsType;
                    noteDetailsModel.NoteDetailsTypeName = item.NoteDetailsTypeName;
                    noteDetailsModel.Subject = item.Subject;
                    noteDetailsModel.SubjectName = item.SubjectName;
                    noteDetailsModel.Details = item.Details;
                    noteDetailsModel.PendingOn = item.PendingOn;
                    noteDetailsModel.PendingOnName = item.PendingOnName;
                    noteDetailsModel.ExpectedDate = item.ExpectedDate;
                    noteDetailsModel.Status_Name = item.Status_Name;
                    noteDetailsViewModels.Add(noteDetailsModel);
                }
            }

            return Json(noteDetailsViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult NoteDetailsPopup_Create([DataSourceRequest] DataSourceRequest request, NoteDetailsModel noteDetailsModel, int apID)
        {
            int applicationID = ValidationHelper.GetInteger(apID, 0);
            if (noteDetailsModel.NoteDetails_Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateNoteDetails(noteDetailsModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (noteDetailsModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(noteDetailsModel))
            {
                var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);

                if (applicatioData != null && !string.IsNullOrEmpty(applicatioData.ApplicationDetails_ApplicationNumber))
                {
                    var noteDetails = NoteDetailsProcess.SaveNoteDetailsModel(applicatioData.ApplicationDetails_ApplicationNumber, noteDetailsModel);
                    if (noteDetails != null)
                        noteDetailsModel = noteDetails;
                }
            }

            return Json(new[] { noteDetailsModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult NoteDetailsPopup_Update([DataSourceRequest] DataSourceRequest request, NoteDetailsModel noteDetailsModel)
        {
            if (noteDetailsModel.NoteDetails_Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateNoteDetails(noteDetailsModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (noteDetailsModel != null && ModelState.IsValid)
            {
                var incommingTransactionData = noteDetailsRepository.GetNoteDetail(noteDetailsModel.Id);
                if (incommingTransactionData != null)
                {
                    var noteDetails = NoteDetailsProcess.SaveNoteDetailsModel(null, noteDetailsModel);
                    if (noteDetails != null)
                        noteDetailsModel = noteDetails;
                }
            }

            return Json(new[] { noteDetailsModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult NoteDetailsPopup_Destroy([DataSourceRequest] DataSourceRequest request, NoteDetailsModel noteDetailsModel)
        {
            if (noteDetailsModel != null)
            {
                noteDetailsRepository.GetNoteDetail(noteDetailsModel.Id).DeleteAllCultures();
            }

            return Json(new[] { noteDetailsModel }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult EntityType_Read()
        {
            var result = ServiceHelper.GetEntityType();
            return Json(result);
        }
        public JsonResult LegalEntityType_Read()
        {
            var result = ServiceHelper.GetCommonDropDown(Constants.JURISDICTION);
            return Json(result);
        }
        public JsonResult EntityRole_Read()
        {
            var result = ServiceHelper.GetPERSON_ROLE();
            return Json(result);
        }
        public JsonResult LegalEntityRole_Read()
        {
            var result = ServiceHelper.GetCommonDropDown(Constants.LEGALPERSON_ROLE);
            return Json(result);
        }
        public JsonResult BankDocumentType_Read()
        {
            var result = ServiceHelper.GetDocumentsType(Constants.BANK_DOCUMENT_TYPE);
            return Json(result);

        }
        public JsonResult ExpectedDocumentType_Read()
        {

            var result = ServiceHelper.GetDocumentsType(Constants.EXPECTED_DOCUMENT_TYPE);
            return Json(result);
        }
        public JsonResult LegalExpectedDocumentType_Read()
        {

            var result = ServiceHelper.GetCommonDropDown(Constants.LEGALEXPECTED_DOCUMENT_TYPE);
            return Json(result);
        }
        public JsonResult RequiresSignature_Read()
        {

            var result = ServiceHelper.GetRequiresSignature();
            return Json(result);
        }
        #endregion
        #region Expected Documents
        public ActionResult AddNewExpectedDouments(int id, string entityType)
        {
            int applicationID = ValidationHelper.GetInteger(id, 0);
            var ExpectedDocuments = expectedDocumentsRepository.GetExpectedDocuments(applicationID);
            if (entityType == Constants.LegalEntity)
            {
                // Get Corporate expected documents
                var expectedDoucumetsModules = expectedDocumentsRepository.GetExpectedCorporateDocumentsModules();
                foreach (var item in expectedDoucumetsModules)
                {
                    var checkBankDocuments = ExpectedDocuments.ToList().Where(i => i.ExpectedDocuments_DocumentType == item.CorporateAccount_ExpectedDocument_ExpectedDocumentType);


                    if (checkBankDocuments.Count() == 0)
                    {
                        DocumentsViewModel documentsViewModel = new DocumentsViewModel();
                        var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                        documentsViewModel.Entity = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_PersonType, "");
                        documentsViewModel.EntityType = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_Jurisdiction, "");
                        documentsViewModel.EntityRole = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_PersonRole, "");
                        documentsViewModel.DocumentType = ValidationHelper.GetString(item.CorporateAccount_ExpectedDocument_ExpectedDocumentType, "");
                        documentsViewModel.EntityTypeSection = entityType;
                        var successData = DocumentsProcess.SaveExpectedDocumentsModel(documentsViewModel, applicatioData, "", "");
                    }
                }
            }
            else
            {
                // Get individual expected documents
                var expectedDoucumetsModules = expectedDocumentsRepository.GetExpectedDocumentsModules();
                foreach (var item in expectedDoucumetsModules)
                {
                    var checkBankDocuments = ExpectedDocuments.ToList().Where(i => i.ExpectedDocuments_EntityType == item.PersonalAndJointAccount_ExpectedDocument_Type && i.ExpectedDocuments_DocumentType == item.PersonalAndJointAccount_ExpectedDocument_ExpectedDocumentType);


                    if (checkBankDocuments.Count() == 0)
                    {
                        DocumentsViewModel documentsViewModel = new DocumentsViewModel();
                        var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                        documentsViewModel.Entity = ValidationHelper.GetString(item.PersonalAndJointAccount_ExpectedDocument_PersonType, "");
                        documentsViewModel.EntityType = ValidationHelper.GetString(item.PersonalAndJointAccount_ExpectedDocument_Type, "");
                        documentsViewModel.EntityRole = ValidationHelper.GetString(item.PersonalAndJointAccount_ExpectedDocument_PersonRole, "");
                        documentsViewModel.DocumentType = ValidationHelper.GetString(item.PersonalAndJointAccount_ExpectedDocument_ExpectedDocumentType, "");
                        var successData = DocumentsProcess.SaveExpectedDocumentsModel(documentsViewModel, applicatioData, "", "");
                    }
                }
            }
            return RedirectToAction("Edit", "Applications", new { id = applicationID });
        }
        public IActionResult ExpectedDocuments_Read([DataSourceRequest] DataSourceRequest request, int id, string applicationNumber, string applicationType)
        {
            int applicationID = ValidationHelper.GetInteger(id, 0);
            List<DocumentsViewModel> documentsViewModels = new List<DocumentsViewModel>();
            var ExpectedDocuments = expectedDocumentsRepository.GetExpectedDocuments(applicationID);
            if (ExpectedDocuments != null)
            {
                documentsViewModels = DocumentsProcess.GetExpectedDocuments(ExpectedDocuments, applicationNumber, applicationType);
            }
            return Json(documentsViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult ExpectedDocumentsPopup_Create([DataSourceRequest] DataSourceRequest request, DocumentsViewModel documentsViewModel, int id, string applicationNumber, string applicationType)
        {
            try
            {
                documentsViewModel.EntityType = documentsViewModel.EntityRole;
                if (string.IsNullOrEmpty(documentsViewModel.Entity))
                {
                    ModelState.AddModelError("Entity", ResHelper.GetString(DocumentsViewModelErrorMessage.Entity));
                }
                if (string.IsNullOrEmpty(documentsViewModel.EntityType))
                {
                    ModelState.AddModelError("EntityType", ResHelper.GetString(DocumentsViewModelErrorMessage.EntityTypeError));
                }
                if (string.IsNullOrEmpty(documentsViewModel.DocumentType))
                {
                    ModelState.AddModelError("DocumentType", ResHelper.GetString(DocumentsViewModelErrorMessage.DocumentTypeError));
                }
                string file = httpContextAccessor.HttpContext.Session.GetString("DocumentsFileName");
                documentsViewModel.FileUpload = file;
                //httpContextAccessor.HttpContext.Session.Remove("DocumentsFileName");
                if (!string.IsNullOrEmpty(file) && file.LastIndexOf("\\") > 0)
                {
                    documentsViewModel.FileName = file.Substring(file.LastIndexOf("\\") + 1);
                }
                //External Upload

                string externalFileGuid = httpContextAccessor.HttpContext.Session.GetString("DocumentFileGuid");
                string uploadedFileName = httpContextAccessor.HttpContext.Session.GetString("UploadedFileName");
                documentsViewModel.ExternalFileGuid = externalFileGuid;
                if (!string.IsNullOrEmpty(uploadedFileName))
                {
                    documentsViewModel.UploadFileName = uploadedFileName.Replace("\"", "");
                }
                //httpContextAccessor.HttpContext.Session.Remove("DocumentFileGuid");
                //httpContextAccessor.HttpContext.Session.Remove("UploadedFileName");

                //External Upload

                int applicationID = ValidationHelper.GetInteger(id, 0);
                if (documentsViewModel.BankDocuments_Status)
                {
                    if (string.IsNullOrEmpty(documentsViewModel.FileUpload))
                    {
                        ModelState.AddModelError("Consent", "Please upload the documents!");
                        //ModelState.AddModelError("files", "Please upload the documents!");
                    }
                }
                if (documentsViewModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(documentsViewModel))
                {
                    var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                    documentsViewModel = DocumentsProcess.SaveExpectedDocumentsModel(documentsViewModel, applicatioData, applicationNumber, applicationType);
                    httpContextAccessor.HttpContext.Session.Remove("DocumentsFileName");
                    httpContextAccessor.HttpContext.Session.Remove("DocumentFileGuid");
                    httpContextAccessor.HttpContext.Session.Remove("UploadedFileName");
                }

                return Json(new[] { documentsViewModel }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "ErrorLog/ExceptionLog.txt");
                if (!System.IO.File.Exists(filepath))
                {
                    System.IO.File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = System.IO.File.AppendText(filepath))
                {
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(ex.ToString());
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.Flush();
                    sw.Close();
                }
                return Json("");
            }
        }

        [HttpPost]
        public ActionResult ExpectedDocumentsPopup_Update([DataSourceRequest] DataSourceRequest request, DocumentsViewModel documentsViewModel, int id, IEnumerable<IFormFile> files, string applicationNumber, string applicationType)
        {
            try
            {
                documentsViewModel.EntityType = documentsViewModel.EntityRole;
                if (string.IsNullOrEmpty(documentsViewModel.Entity))
                {
                    ModelState.AddModelError("Entity", ResHelper.GetString(DocumentsViewModelErrorMessage.Entity));
                }
                if (string.IsNullOrEmpty(documentsViewModel.EntityType))
                {
                    ModelState.AddModelError("EntityType", ResHelper.GetString(DocumentsViewModelErrorMessage.EntityTypeError));
                }
                if (string.IsNullOrEmpty(documentsViewModel.DocumentType))
                {
                    ModelState.AddModelError("DocumentType", ResHelper.GetString(DocumentsViewModelErrorMessage.DocumentTypeError));
                }
                string file = httpContextAccessor.HttpContext.Session.GetString("DocumentsFileName");
                if (string.IsNullOrEmpty(file))
                {
                    file = documentsViewModel.UploadFileName;

                }
                documentsViewModel.FileUpload = file;
                //httpContextAccessor.HttpContext.Session.Remove("DocumentsFileName");
                //External Upload

                string externalFileGuid = httpContextAccessor.HttpContext.Session.GetString("DocumentFileGuid");
                string uploadedFileName = httpContextAccessor.HttpContext.Session.GetString("UploadedFileName");
                if (!string.IsNullOrEmpty(externalFileGuid))
                {
                    documentsViewModel.ExternalFileGuid = externalFileGuid;
                }

                if (!string.IsNullOrEmpty(uploadedFileName))
                {
                    documentsViewModel.UploadFileName = uploadedFileName.Replace("\"", "");
                }
                //httpContextAccessor.HttpContext.Session.Remove("DocumentFileGuid");
                //httpContextAccessor.HttpContext.Session.Remove("UploadedFileName");

                //External Upload


                if (!string.IsNullOrEmpty(file) && file.LastIndexOf("\\") > 0)
                {
                    documentsViewModel.FileName = file.Substring(file.LastIndexOf("\\") + 1);
                }
                int applicationID = ValidationHelper.GetInteger(id, 0);
                if (documentsViewModel.BankDocuments_Status)
                {
                    if (string.IsNullOrEmpty(documentsViewModel.FileUpload))
                    {
                        ModelState.AddModelError("Consent", "Please upload the documents!");
                    }
                }
                if (documentsViewModel != null && ModelState.IsValid)
                {
                    var bankData = expectedDocumentsRepository.GetExpectedDocumentsByID(documentsViewModel.DocId);
                    documentsViewModel = DocumentsProcess.UpdateExpectedDocumentsModel(documentsViewModel, bankData, applicationNumber, applicationType);
                    httpContextAccessor.HttpContext.Session.Remove("DocumentsFileName");
                    httpContextAccessor.HttpContext.Session.Remove("DocumentFileGuid");
                    httpContextAccessor.HttpContext.Session.Remove("UploadedFileName");
                }

                return Json(new[] { documentsViewModel }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "ErrorLog/ExceptionLog.txt");
                if (!System.IO.File.Exists(filepath))
                {
                    System.IO.File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = System.IO.File.AppendText(filepath))
                {
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(ex.ToString());
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.Flush();
                    sw.Close();
                }
                return Json("");
            }
        }
        [HttpPost]
        public ActionResult ExpectedDocumentsPopup_Destroy([DataSourceRequest] DataSourceRequest request, DocumentsViewModel documentsViewModel)
        {
            if (documentsViewModel != null)
            {
                expectedDocumentsRepository.GetExpectedDocumentsByID(documentsViewModel.DocId).DeleteAllCultures();
            }

            return Json(new[] { documentsViewModel }.ToDataSourceResult(request, ModelState));
        }

        //public JsonResult CardType_Read()
        //{
        //	var result = ServiceHelper.GetCardType();
        //	return Json(result);
        //}
        //public JsonResult DispatchMethod_Read()
        //{
        //	var result = ServiceHelper.GetDispatchMethod();
        //	return Json(result);
        //}
        //public JsonResult DeliveryAddress_Read()
        //{
        //	var result = ServiceHelper.GetDeliveryAddress();
        //	return Json(result);
        //}
        //public JsonResult CollectedBy_Read()
        //{

        //	var result = ServiceHelper.GetCollectedBy();
        //	return Json(result);
        //}
        #endregion
        #region DecisionHistory
        public JsonResult DecisionHistory_Decision_Read()
        {
            var result = ServiceHelper.GetDecision();
            return Json(result);
        }
        [HttpPost]
        public ActionResult DecisionHistory(ApplicationViewModel applicationViewModel)
        {
            if (ModelState.IsValid)
            {
                var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationViewModel.Id);
                applicationViewModel.DecisionHistoryViewModel = DecisionHistoryProcess.SaveDecisionHistorysModel(applicationViewModel.DecisionHistoryViewModel, applicatioData, ValidationHelper.GetString(applicatioData.GetValue("ApplicationDetails_SubmittedBy"), ""));

                //return RedirectToAction("Edit", "Applications", new { id = applicationViewModel.Id });
                //int applicationID = ValidationHelper.GetInteger(applicationViewModel.Id, 0);
                //List<DecisionHistoryViewModel> decisionHistory = new List<DecisionHistoryViewModel>();
                //var decisionHistoryData = decisionHistoryRepository.GetDecisionHistorys(applicationID);
                //if (decisionHistoryData != null)
                //{
                //    decisionHistory = DecisionHistoryProcess.GetDecisionHistorys(decisionHistoryData);
                //}
                //applicationViewModel._lst_DecisionHistoryViewModel = decisionHistory;

                return PartialView("_DecisionHistory", applicationViewModel);
            }
            return View();
        }
        public IActionResult DecisionHistorys_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            int applicationID = ValidationHelper.GetInteger(id, 0);
            List<DecisionHistoryViewModel> decisionHistory = new List<DecisionHistoryViewModel>();
            var decisionHistoryData = decisionHistoryRepository.GetDecisionHistorys(applicationID);
            if (decisionHistoryData != null)
            {
                decisionHistoryData = decisionHistoryData.OrderByDescending(y => y.DecisionHistory_When);
                decisionHistory = DecisionHistoryProcess.GetDecisionHistorys(decisionHistoryData);
            }
            return Json(decisionHistory.ToDataSourceResult(request));
        }
        #endregion

        #region Signature Mandate

        public IActionResult SignatureMandateIndividual_Read([DataSourceRequest] DataSourceRequest request)
        {

            TempData.Keep("ApplicationID");
            int applicationID = ValidationHelper.GetInteger(TempData["ApplicationID"], 0);
            List<SignatureMandateIndividualModel> signatureMandateIndividualViewModels = new List<SignatureMandateIndividualModel>();
            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationID);

            if (applicationDetails != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationNumber))
            {
                signatureMandateIndividualViewModels = SignatureMandateIndividualProcess.GetSignatureMandateIndividualModels(applicationDetails.ApplicationDetails_ApplicationNumber);
            }

            return Json(signatureMandateIndividualViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult SignatureMandateIndividual_Create([DataSourceRequest] DataSourceRequest request, SignatureMandateIndividualModel signatureMandateIndividualModel, int apID)
        {
            int applicationID = ValidationHelper.GetInteger(apID, 0);
            if (signatureMandateIndividualModel.SignatureMandateIndividual_Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateSignatureMandateIndividual(signatureMandateIndividualModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (signatureMandateIndividualModel != null && ModelState.IsValid)
            {
                var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);

                if (applicatioData != null && !string.IsNullOrEmpty(applicatioData.ApplicationDetails_ApplicationNumber))
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
                    signatureMandateIndividualModel.SignatoryPersons = multiSelectSignatoryPersons;

                    if (!ObjectValidation.IsAllNullOrEmptyOrZero(signatureMandateIndividualModel))
                    {
                        var signatureMandateIndividual = SignatureMandateIndividualProcess.SaveSignatureMandateIndividualModel(applicatioData.ApplicationDetails_ApplicationNumber, signatureMandateIndividualModel);
                        if (signatureMandateIndividual != null)
                            signatureMandateIndividualModel = signatureMandateIndividual;
                    }
                }
            }

            return Json(new[] { signatureMandateIndividualModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult SignatureMandateIndividualPopup_Update([DataSourceRequest] DataSourceRequest request, SignatureMandateIndividualModel signatureMandateIndividualModel, string applicationNumber)
        {
            if (signatureMandateIndividualModel.SignatureMandateIndividual_Status == true)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateSignatureMandateIndividual(signatureMandateIndividualModel);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (signatureMandateIndividualModel != null && ModelState.IsValid)
            {
                var signatureMandateIndividualData = signatureMandateIndividualRepository.GetSignatureMandateIndividual(signatureMandateIndividualModel.Id);
                if (signatureMandateIndividualData != null)
                {
                    int multiSelectCounter = 0;
                    string multiSelectSignatoryPersons = string.Empty;
                    //while (Request.Form["SignatoryPersonsList[" + multiSelectCounter + "].Value"].Count > 0)
                    //{
                    //    if (string.IsNullOrEmpty(multiSelectSignatoryPersons))
                    //    {
                    //        multiSelectSignatoryPersons = Request.Form["SignatoryPersonsList[" + multiSelectCounter + "].Value"];
                    //    }
                    //    else
                    //    {
                    //        multiSelectSignatoryPersons += "|" + Request.Form["SignatoryPersonsList[" + multiSelectCounter + "].Value"];
                    //    }
                    //    multiSelectCounter++;
                    //}
                    if (!string.IsNullOrEmpty(signatureMandateIndividualModel.SignatoryPersonsValueString))
                    {
                        multiSelectSignatoryPersons = signatureMandateIndividualModel.SignatoryPersonsValueString.TrimEnd('|');
                    }
                    signatureMandateIndividualModel.SignatoryPersons = multiSelectSignatoryPersons;
                    var signatureMandateIndividual = SignatureMandateIndividualProcess.SaveSignatureMandateIndividualModel(applicationNumber, signatureMandateIndividualModel);
                    if (signatureMandateIndividual != null)
                        signatureMandateIndividualModel = signatureMandateIndividual;
                }
            }

            return Json(new[] { signatureMandateIndividualModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult SignatureMandateIndividualPopup_Destroy([DataSourceRequest] DataSourceRequest request, SignatureMandateIndividualModel signatureMandateIndividualModel)
        {
            if (signatureMandateIndividualModel != null)
            {
                signatureMandateIndividualRepository.GetSignatureMandateIndividual(signatureMandateIndividualModel.Id).DeleteAllCultures();
            }

            return Json(new[] { signatureMandateIndividualModel }.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region Company Group Structure

        public IActionResult CompanyGroupStructure_Read([DataSourceRequest] DataSourceRequest request, int apID, string applicationNumber)
        {
            int applicationID = ValidationHelper.GetInteger(apID, 0);
            List<CompanyGroupStructureModel> companyGroupStructureViewModels = new List<CompanyGroupStructureModel>();

            if (applicationID > 0)
            {
                //var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicantId);
                var applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationID);
                if (applicationDetails != null && applicationDetails.ApplicationDetailsID > 0)
                {
                    companyGroupStructureViewModels = CompanyGroupStructureProcess.GetCompanyGroupStructure(applicationID, applicationNumber);
                }
            }

            if (companyGroupStructureViewModels == null)
                return Json("");

            return Json(companyGroupStructureViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult CompanyGroupStructurePopup_Create([DataSourceRequest] DataSourceRequest request, CompanyGroupStructureModel companyGroupStructureModel, int apID, string applicationNumber)
        {
            if (companyGroupStructureModel.Status)
            {
                if (string.IsNullOrEmpty(companyGroupStructureModel.EntityType))
                {
                    ModelState.AddModelError("EntityType", ResHelper.GetString(CompanyGroupStructureModelErrorMessage.EntityType));
                }
                if (string.IsNullOrEmpty(companyGroupStructureModel.EntityName))
                {
                    ModelState.AddModelError("EntityName", ResHelper.GetString(CompanyGroupStructureModelErrorMessage.EntityName));
                }
                //if(string.IsNullOrEmpty(companyGroupStructureModel.GroupName))
                //{
                //    ModelState.AddModelError("Ownership", ResHelper.GetString(CompanyGroupStructureModelErrorMessage.GroupName));
                //}
                //if(string.IsNullOrEmpty(companyGroupStructureModel.GroupActivities))
                //{
                //    ModelState.AddModelError("Participation", ResHelper.GetString(CompanyGroupStructureModelErrorMessage.GroupActivities));
                //}
            }
            int applicationID = ValidationHelper.GetInteger(apID, 0);
            if (companyGroupStructureModel != null && ModelState.IsValid && !ObjectValidation.IsAllNullOrEmptyOrZero(companyGroupStructureModel))
            {
                //var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicantId);
                var applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationID);
                if (applicationDetails != null && applicationDetails.ApplicationDetailsID > 0)
                {
                    var companyGroupStructure = CompanyGroupStructureProcess.SaveCompanyGroupStructureModel(applicationDetails.ApplicationDetailsID, companyGroupStructureModel, applicationNumber);
                    if (companyGroupStructure != null)
                        companyGroupStructureModel = companyGroupStructure;
                }
            }

            return Json(new[] { companyGroupStructureModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult CompanyGroupStructurePopup_Update([DataSourceRequest] DataSourceRequest request, CompanyGroupStructureModel companyGroupStructureModel, int apID, string applicationNumber)
        {
            if (companyGroupStructureModel.Status)
            {
                if (string.IsNullOrEmpty(companyGroupStructureModel.EntityType))
                {
                    ModelState.AddModelError("EntityType", ResHelper.GetString(CompanyGroupStructureModelErrorMessage.EntityType));
                }
                if (string.IsNullOrEmpty(companyGroupStructureModel.EntityName))
                {
                    ModelState.AddModelError("EntityName", ResHelper.GetString(CompanyGroupStructureModelErrorMessage.EntityName));
                }
                //if(string.IsNullOrEmpty(companyGroupStructureModel.GroupName))
                //{
                //    ModelState.AddModelError("Ownership", ResHelper.GetString(CompanyGroupStructureModelErrorMessage.GroupName));
                //}
                //if(string.IsNullOrEmpty(companyGroupStructureModel.GroupActivities))
                //{
                //    ModelState.AddModelError("Participation", ResHelper.GetString(CompanyGroupStructureModelErrorMessage.GroupActivities));
                //}
            }

            if (companyGroupStructureModel != null && ModelState.IsValid)
            {
                var companyGroupStructureData = companyGroupStructureRepository.GetCompanyGroupStructure(companyGroupStructureModel.Id);
                if (companyGroupStructureData != null)
                {
                    var groupStructure = CompanyGroupStructureProcess.SaveCompanyGroupStructureModel(apID, companyGroupStructureModel, applicationNumber);
                    if (groupStructure != null)
                        companyGroupStructureModel = groupStructure;
                }
            }

            return Json(new[] { companyGroupStructureModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult CompanyGroupStructurePopup_Destroy([DataSourceRequest] DataSourceRequest request, CompanyGroupStructureModel companyGroupStructureModel)
        {
            if (companyGroupStructureModel != null)
            {
                companyGroupStructureRepository.GetCompanyGroupStructure(companyGroupStructureModel.Id).DeleteAllCultures();
            }

            return Json(new[] { companyGroupStructureModel }.ToDataSourceResult(request, ModelState));
        }
        public JsonResult GroupStructureParent_Read(int applicationId, string applicationNumber)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            if (applicationId > 0 && !string.IsNullOrEmpty(applicationNumber))
            {
                result = CommonProcess.GetDDLGroupStructureParent(applicationNumber, applicationId);
            }
            return Json(result);
        }
        #endregion

        #region Signatory Group

        public IActionResult SignatoryGroup_Read([DataSourceRequest] DataSourceRequest request, int apID)
        {
            int applicationId = ValidationHelper.GetInteger(apID, 0);
            List<SignatoryGroupModel> signatoryGroupViewModels = new List<SignatoryGroupModel>();

            if (applicationId > 0)
            {
                signatoryGroupViewModels = SignatoryGroupProcess.GetApplicationSignatoryGroup(applicationId);
            }

            if (signatoryGroupViewModels == null)
                return Json("");

            return Json(signatoryGroupViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult SignatoryGroupPopup_Create([DataSourceRequest] DataSourceRequest request, SignatoryGroupModel signatoryGroupModel, int apID)
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
            int applicationId = ValidationHelper.GetInteger(apID, 0);

            if (signatoryGroupModel.Status)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateSignatoryGroup(signatoryGroupModel, applicationId);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }

            if (signatoryGroupModel != null && ModelState.IsValid)
            {

                if (!ObjectValidation.IsAllNullOrEmptyOrZero(signatoryGroupModel))
                {

                    var signatoryGroup = SignatoryGroupProcess.SaveApplicationSignatoryGroupModel(applicationId, signatoryGroupModel);
                    if (signatoryGroup != null)
                        signatoryGroupModel = signatoryGroup;
                }
            }


            return Json(new[] { signatoryGroupModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult SignatoryGroupPopup_Update([DataSourceRequest] DataSourceRequest request, SignatoryGroupModel signatoryGroupModel, int apID)
        {
            int multiSelectCounter = 0;
            if (signatoryGroupModel.SignatoryPersonsList != null && signatoryGroupModel.SignatoryPersonsList.Length > 0)
            {
                multiSelectCounter = signatoryGroupModel.SignatoryPersonsList.Length;

            }
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
            if (!string.IsNullOrEmpty(multiSelectSignatoryPersons))
            {
                signatoryGroupModel.SignatoryPersons = multiSelectSignatoryPersons;
            }
            if (signatoryGroupModel.SignatoryPersonsList != null)
            {
                List<string> signatoryGrouplist = signatoryGroupModel.SignatoryPersons?.Split('|').Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();

                signatoryGroupModel.SignatoryPersons = string.Join("|", signatoryGroupModel.SignatoryPersonsList) + (!string.IsNullOrEmpty(signatoryGroupModel.SignatoryPersons) && !signatoryGroupModel.SignatoryPersonsList.Any(x => signatoryGrouplist.Contains(x)) ? "|" + signatoryGroupModel.SignatoryPersons : string.Empty);
            }
            else
            {
                signatoryGroupModel.SignatoryPersons = string.Empty;
            }
            int applicationId = ValidationHelper.GetInteger(apID, 0);

            if (signatoryGroupModel.Status)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateSignatoryGroup(signatoryGroupModel, applicationId);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }


            if (signatoryGroupModel != null && ModelState.IsValid)
            {
                var signatoryGroupData = signatoryGroupRepository.GetSignatoryGroup(signatoryGroupModel.Id);
                if (signatoryGroupData != null)
                {

                    var signatoryGroups = SignatoryGroupProcess.SaveApplicationSignatoryGroupModel(applicationId, signatoryGroupModel);
                    if (signatoryGroups != null)
                        signatoryGroupModel = signatoryGroups;
                }
            }

            return Json(new[] { signatoryGroupModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult SignatoryGroupPopup_Destroy([DataSourceRequest] DataSourceRequest request, SignatoryGroupModel signatoryGroupModel, int apID)
        {
            if (signatoryGroupModel != null)
            {
                List<ValidationResultModel> retVal = new List<ValidationResultModel>();
                int applicationId = ValidationHelper.GetInteger(apID, 0);
                //            ValidationResultModel validationResultModel = new ValidationResultModel();
                //            validationResultModel = ApplicationFormBasicValidationProcess.ValidateDeleteSignatoryGroup(signatoryGroupModel.SignatoryGroupName, applicationId);
                //            retVal.Add(validationResultModel);
                //            bool applicationConfirmValidation = validationResultModel.IsValid; 
                //            if (!applicationConfirmValidation)
                //            {
                //                //TempData["ErrorSummary"] = JsonConvert.SerializeObject(retVal);
                //                //return RedirectToAction("Edit", new { id = apID });
                // //               string test = retVal.Select(m => m.Errors[0].ErrorMessage).FirstOrDefault();
                //	//ViewBag.JavaScriptFunction = string.Format("displayWarningMsg('{0}');", retVal.Select(m => m.Errors[0].ErrorMessage).FirstOrDefault());
                //}
                if (signatoryGroupModel != null)
                {
                    signatoryGroupRepository.GetSignatoryGroup(signatoryGroupModel.Id).DeleteAllCultures();
                }
            }

            return Json(new[] { signatoryGroupModel }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult SignatoryGroupDropdown_Read(int apID)
        {
            List<SelectListItem> result = ServiceHelper.GetSignatoryGroup();
            //var totalGroups = ServiceHelper.GetSignatoryGroup();
            //        int applicationId = ValidationHelper.GetInteger(apID, 0);
            //        List<SignatoryGroupModel> signatoryGroupViewModels = new List<SignatoryGroupModel>();

            //        if(applicationId > 0)
            //        {
            //            signatoryGroupViewModels = SignatoryGroupProcess.GetApplicationSignatoryGroup(applicationId);
            //            if(signatoryGroupViewModels != null && signatoryGroupViewModels.Count > 0)
            //{
            //                foreach(var item in totalGroups)
            //	{
            //                    if(!signatoryGroupViewModels.Any(y => string.Equals(y.SignatoryGroupName, item.Value, StringComparison.OrdinalIgnoreCase)))
            //		{
            //                        result.Add(item);
            //                    }
            //	}
            //}
            //else
            //{
            //                result = totalGroups;

            //            }
            //        }
            return Json(result);
        }

        #endregion

        #region Signature Mandate Legal

        public IActionResult SignatureMandateLegal_Read([DataSourceRequest] DataSourceRequest request, int apID)
        {
            int applicationId = ValidationHelper.GetInteger(apID, 0);
            List<SignatureMandateCompanyModel> signatureMandateCompanyViewModels = new List<SignatureMandateCompanyModel>();
            if (applicationId > 0)
            {
                var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationId);
                if (applicationDetails != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationNumber))
                {
                    signatureMandateCompanyViewModels = SignatureMandateLegalProcess.GetSignatureMandateLegalModels(applicationDetails.ApplicationDetails_ApplicationNumber);
                }
            }
            if (signatureMandateCompanyViewModels == null)
                return Json("");

            return Json(signatureMandateCompanyViewModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult SignatureMandateLegalPopup_Create([DataSourceRequest] DataSourceRequest request, SignatureMandateCompanyModel signatureMandateCompanyModel, int apID)
        {
            if (signatureMandateCompanyModel.Status)
            {
                var applicationNumber = applicationsRepository.GetApplicationDetailsByID(ValidationHelper.GetInteger(apID, 0)).ApplicationDetails_ApplicationNumber;
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateSignatureMandateLegal(signatureMandateCompanyModel, applicationNumber);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }

            }

            int applicationID = ValidationHelper.GetInteger(apID, 0);
            if (signatureMandateCompanyModel != null && ModelState.IsValid)
            {
                var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                if (applicatioData != null && !string.IsNullOrEmpty(applicatioData.ApplicationDetails_ApplicationNumber))
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
                    var signatureMandateCompany = SignatureMandateLegalProcess.SaveSignatureMandateLegalModel(applicatioData.ApplicationDetails_ApplicationNumber, signatureMandateCompanyModel, applicationID);
                    if (signatureMandateCompany != null)
                        signatureMandateCompanyModel = signatureMandateCompany;
                }
            }

            return Json(new[] { signatureMandateCompanyModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult SignatureMandateLegalPopup_Update([DataSourceRequest] DataSourceRequest request, SignatureMandateCompanyModel signatureMandateCompanyModel, int apID)
        {
            //bool IsTotalSignvalid = true;
            if (signatureMandateCompanyModel.Status)
            {
                var applicationNumber = applicationsRepository.GetApplicationDetailsByID(ValidationHelper.GetInteger(apID, 0)).ApplicationDetails_ApplicationNumber;
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = ApplicationFormBasicValidationProcess.ValidateSignatureMandateLegal(signatureMandateCompanyModel, applicationNumber);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }

                //if (signatureMandateCompanyModel.Rights == "716b2ddb-6837-4b8a-8aa3-ce1f6c577f44")
                //{
                //    int numberOfSignature = 0;
                //    int numberofSignatures1 = 0;
                //    if (signatureMandateCompanyModel.NumberofSignatures != null)
                //        numberOfSignature = Convert.ToInt32(signatureMandateCompanyModel.NumberofSignatures);
                //    if (signatureMandateCompanyModel.NumberofSignatures1 != null)
                //        numberofSignatures1 = Convert.ToInt32(signatureMandateCompanyModel.NumberofSignatures1);

                //    int numberOfSign = Convert.ToInt32(numberOfSignature + numberofSignatures1);
                //    if (signatureMandateCompanyModel.TotalNumberofSignature != numberOfSign)
                //    {
                //        IsTotalSignvalid = false;
                //        //ModelState.AddModelError("TotalNumberofSignature", ValidationConstant.CalculateTotalNumberOfSignature);
                //        ViewBag.TotalSignvalid = "Sum of number of signature must be equal to Total number of Signature";
                //    }
                //    else
                //    {
                //        IsTotalSignvalid = true;
                //        ViewBag.TotalSignvalid = "";
                //    } 
                //}
            }
            int applicationID = ValidationHelper.GetInteger(apID, 0);
            if (signatureMandateCompanyModel != null && ModelState.IsValid) // && IsTotalSignvalid
            {
                var signatureMandateCompanyData = signatureMandateCompanyRepository.GetSignatureMandateCompany(signatureMandateCompanyModel.Id);
                if (signatureMandateCompanyData != null)
                {
                    int multiSelectCounter = 0;

                    //while (Request.Form["AuthorizedSignatoryGroupList[" + multiSelectCounter + "].Value"].Count > 0)
                    //{
                    //    if (string.IsNullOrEmpty(multiSelectSignatoryPersons))
                    //    {
                    //        multiSelectSignatoryPersons = Request.Form["AuthorizedSignatoryGroupList[" + multiSelectCounter + "].Value"];
                    //    }
                    //    else
                    //    {
                    //        multiSelectSignatoryPersons += "|" + Request.Form["AuthorizedSignatoryGroupList[" + multiSelectCounter + "].Value"];
                    //    }
                    //    multiSelectCounter++;
                    //}

                    //int multiSelectCounter1 = 0;

                    //while (Request.Form["AuthorizedSignatoryGroup1List[" + multiSelectCounter1 + "].Value"].Count > 0)
                    //{
                    //    if (string.IsNullOrEmpty(multiSelectSignatoryPersons1))
                    //    {
                    //        multiSelectSignatoryPersons1 = Request.Form["AuthorizedSignatoryGroup1List[" + multiSelectCounter1 + "].Value"];
                    //    }
                    //    else
                    //    {
                    //        multiSelectSignatoryPersons1 += "|" + Request.Form["AuthorizedSignatoryGroup1List[" + multiSelectCounter1 + "].Value"];
                    //    }
                    //    multiSelectCounter1++;
                    //}
                    string multiSelectSignatoryPersons = string.Empty;
                    string multiSelectSignatoryPersons1 = string.Empty;
                    if (!string.IsNullOrEmpty(signatureMandateCompanyModel.AuthorizedSignatoryGroupValueList))
                    {
                        multiSelectSignatoryPersons = signatureMandateCompanyModel.AuthorizedSignatoryGroupValueList.TrimEnd('|');
                    }
                    if (!string.IsNullOrEmpty(signatureMandateCompanyModel.AuthorizedSignatoryGroupOneValueList))
                    {
                        multiSelectSignatoryPersons1 = signatureMandateCompanyModel.AuthorizedSignatoryGroupOneValueList.TrimEnd('|');
                    }
                    signatureMandateCompanyModel.AuthorizedSignatoryGroup = multiSelectSignatoryPersons;
                    signatureMandateCompanyModel.AuthorizedSignatoryGroup1 = multiSelectSignatoryPersons1;
                    var signatureMandateCompanys = SignatureMandateLegalProcess.SaveSignatureMandateLegalModel(null, signatureMandateCompanyModel, applicationID);
                    if (signatureMandateCompanys != null)
                        signatureMandateCompanyModel = signatureMandateCompanys;
                }
            }

            return Json(new[] { signatureMandateCompanyModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult SignatureMandateLegalPopup_Destroy([DataSourceRequest] DataSourceRequest request, SignatureMandateCompanyModel signatureMandateCompanyModel)
        {
            if (signatureMandateCompanyModel != null)
            {
                signatureMandateCompanyRepository.GetSignatureMandateCompany(signatureMandateCompanyModel.Id).DeleteAllCultures();
            }

            return Json(new[] { signatureMandateCompanyModel }.ToDataSourceResult(request, ModelState));
        }
        public JsonResult AuthorizeGroup_Read(int id)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            if (id > 0)
            {
                result = SignatoryGroupProcess.GetDDLSignatoryGroupsForSignatureMandate(id);
            }
            return Json(result);
        }

        public JsonResult AuthorizeGroup_ReadAll(int applicationid, string mandateType)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            var mandateTypes = ServiceHelper.GetMandateType();
            if (applicationid > 0 && mandateTypes != null && mandateTypes.Any(x => string.Equals(x.Value, mandateType, StringComparison.OrdinalIgnoreCase)))
            {
                string mandateTypeName = mandateTypes.FirstOrDefault(x => string.Equals(x.Value, mandateType, StringComparison.OrdinalIgnoreCase)).Text;
                if (string.Equals(mandateTypeName, "FOR AUTHORISED PERSONS", StringComparison.OrdinalIgnoreCase))
                {
                    result = SignatoryGroupProcess.GetDDLAuthorizePersonSignatoryGroupsForSignatureMandate(applicationid);
                }
                else
                {
                    result = SignatoryGroupProcess.GetDDLSignatoryGroupsForSignatureMandate(applicationid);
                }
            }
            return Json(result);
        }
        public JsonResult MandateType_Read()
        {
            var result = ServiceHelper.GetMandateType();
            return Json(result);
        }

        public ActionResult IsSignatoryUseInMandate(int id, string editGroupName)
        {
            string isUsed = "False";
            int applicationId = ValidationHelper.GetInteger(id, 0);
            List<SignatureMandateCompanyModel> signatureMandateCompanyViewModels = new List<SignatureMandateCompanyModel>();
            if (applicationId > 0)
            {
                var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationId);
                if (applicationDetails != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationNumber))
                {
                    signatureMandateCompanyViewModels = SignatureMandateLegalProcess.GetSignatureMandateLegalModels(applicationDetails.ApplicationDetails_ApplicationNumber);
                    if (signatureMandateCompanyViewModels != null)
                    {
                        if (signatureMandateCompanyViewModels.Any(s => s.AuthorizedSignatoryGroupName.Contains(editGroupName)) || signatureMandateCompanyViewModels.Any(s => s.AuthorizedSignatoryGroup1Name.Contains(editGroupName)))
                        {
                            isUsed = "True";
                        }
                    }
                }
            }
            return Json(isUsed);
        }
        public ActionResult SignatureValidateforDelete(int apID, string SignatoryGroupName)
        {
            string retVal = "False";
            if (!string.IsNullOrEmpty(SignatoryGroupName))
            {
                int applicationId = ValidationHelper.GetInteger(apID, 0);
                ValidationResultModel validationResultModel = ApplicationFormBasicValidationProcess.ValidateDeleteSignatoryGroup(SignatoryGroupName, applicationId);
                retVal = validationResultModel.IsValid.ToString();
                if (validationResultModel.IsValid)
                {
                    retVal = "True";
                }
            }
            return Json(retVal);
        }
        public ActionResult UpdateSignatureMandateStatus(int id, string editGroupName)
        {
            int applicationId = ValidationHelper.GetInteger(id, 0);
            List<string> SignatoryGroupName = null;
            List<string> SignatoryGroupName1 = null;
            string editGroupNameValue = string.Empty;
            var result = signatureMandateCompanyRepository.GetSignatureMandateLegal(applicationId);
            if (result != null)
            {
                var groupList = SignatoryGroupProcess.GetDDLSignatoryGroupsForSignatureMandate(applicationId);
                if (groupList != null)
                {
                    editGroupNameValue = groupList.Where(x => x.Text == editGroupName).Select(x => x.Value).FirstOrDefault();
                }

                foreach (var item in result)
                {

                    if (!string.IsNullOrEmpty(item.SignatureMandateCompany_AuthorizedSignatoryGroup))
                    {
                        SignatoryGroupName = item.SignatureMandateCompany_AuthorizedSignatoryGroup.Split('|').ToList();
                    }
                    if (!string.IsNullOrEmpty(item.SignatureMandateCompany_AuthorizedSignatoryGroup1))
                    {
                        SignatoryGroupName1 = item.SignatureMandateCompany_AuthorizedSignatoryGroup1.Split('|').ToList();
                    }
                    if (SignatoryGroupName != null && !string.IsNullOrEmpty(editGroupNameValue))
                    {
                        if (SignatoryGroupName.Contains(editGroupNameValue))
                        {
                            item.Status = false;
                            item.SignatureMandateCompany_AuthorizedSignatoryGroup = string.Empty;
                            item.SignatureMandateCompany_AuthorizedSignatoryGroup1 = string.Empty;
                            item.Update();
                        }
                    }
                    if (SignatoryGroupName1 != null && !string.IsNullOrEmpty(editGroupNameValue))
                    {
                        if (SignatoryGroupName1.Contains(editGroupNameValue))
                        {
                            item.Status = false;
                            item.SignatureMandateCompany_AuthorizedSignatoryGroup = string.Empty;
                            item.SignatureMandateCompany_AuthorizedSignatoryGroup1 = string.Empty;
                            item.Update();
                        }
                    }

                }
            }
            return Json("true");
        }

        #endregion

        public JsonResult GetIdentificationNumber(string nodeGUID)
        {
            string identificationNumber = string.Empty;
            var personalDetails = PersonalDetailsProvider.GetPersonalDetails(new Guid(nodeGUID), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName);
            if (personalDetails != null)
            {
                PersonalDetails person = personalDetails.FirstOrDefault();
                var identification = IdentificationDetailsProcess.GetIdentificationDetails(person.PersonalDetailsID);
                if (identification != null)
                {
                    List<string> mylist = new List<string>(new string[] { "ID", "PASSPORT" });
                    if (identification.Any(x => mylist.Contains(x.IdentificationDetails_TypeOfIdentificationName)))
                    {
                        identificationNumber = identification.FirstOrDefault(x => mylist.Contains(x.IdentificationDetails_TypeOfIdentificationName)).IdentificationDetails_IdentificationNumber;
                    }

                }
            }
            return Json(identificationNumber);
        }

        public JsonResult ResonForOpeningAccountLegal_Read(string applicationNumber, string applicationType)
        {
            List<SelectListItem> result = null;
            if (string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
            {

                string appplicantEntityType = string.Empty;
                List<ApplicantModel> applicantLegal = ApplicantProcess.GetLegalApplicantModels(applicationNumber);
                if (applicantLegal != null && applicantLegal.Count > 0 && applicantLegal.FirstOrDefault().CompanyDetails != null)
                {
                    appplicantEntityType = applicantLegal.FirstOrDefault().CompanyDetails.EntityType;

                }
                result = PurposeAndActivityProcess.GetResonForOpeningAccountLegal(appplicantEntityType);
            }
            else
            {
                result = ServiceHelper.GetReasonsForOpeningTheAccountIndividual();
            }

            return Json(result);
        }

        public JsonResult CardHoderAddresses_Read(string relatedPartyGuid)
        {
            List<SelectListItem> addressDetailsViewModels = null;
            if (!string.IsNullOrEmpty(relatedPartyGuid))
            {
                addressDetailsViewModels = AddressDetailsProcess.GetCardHolderAddresses(relatedPartyGuid);
            }

            if (addressDetailsViewModels == null)
            {
                return Json("");
            }

            return Json(addressDetailsViewModels);
        }

        public JsonResult SignatoryPersons_Read(string applicationNumber, string signatoryName)
        {
            var result = CommonProcess.GetSignatoryPersonLegal(applicationNumber);
            var signatoryGroups = ServiceHelper.GetSignatoryGroup();
            if (signatoryGroups != null && signatoryGroups.Any(r => string.Equals(r.Value, signatoryName, StringComparison.OrdinalIgnoreCase)))
            {
                string selectedSignatoryGroup = signatoryGroups.FirstOrDefault(r => string.Equals(r.Value, signatoryName, StringComparison.OrdinalIgnoreCase)).Text;
                if (string.Equals(selectedSignatoryGroup, "AUTHORISED PERSONS"))
                {
                    result = CommonProcess.GetSignatoryPersonAuthorizedPersonLegal(applicationNumber);
                }
                else
                {
                    result = CommonProcess.GetSignatoryPersonAuthorisedSignatoryLegal(applicationNumber);
                }

            }
            return Json(result);
        }

        public JsonResult GetAuthorizedSignatoryGroupSM()
        {
            return Json("");
        }

        //public JsonResult GetDebitCardAccounts(int applicationId)
        //{
        //    List<SelectListItem> retVal = null;
        //    var debitCardAccounts = AccountsProcess.GetDebitCardAccountsByApplicationID(applicationId);
        //    if(debitCardAccounts != null)
        //    {
        //        retVal = debitCardAccounts;
        //    }
        //    return Json(retVal);
        //}


        [HttpPost]
        public JsonResult GetNextStage(int applicationId, string decisionGuid)
        {
            string result = string.Empty;
            UserModel user = UserProcess.GetUser(User.Identity.Name);

            if (user != null && user.UserInformation != null && applicationId > 0 && !string.IsNullOrEmpty(decisionGuid))
            {
                result = ApplicationWorkFlowProcess.GetNextStage(user, applicationId, decisionGuid);
            }

            return Json(result);
        }

        //[HttpPost]
        //public JsonResult GetEscalateToUsers()
        //{
        //    string result = string.Empty;
        //    UserModel user = UserProcess.GetUser(User.Identity.Name);

        //    //if(user != null && user.UserInformation != null && applicationId > 0 && !string.IsNullOrEmpty(decisionGuid))
        //    //{
        //    //    result = ApplicationWorkFlowProcess.GetNextStage(user, applicationId, decisionGuid);
        //    //}

        //    return Json(result);
        //}

        #region Validation

        //[HttpPost]
        //public JsonResult ValidatePurposeActivity(ApplicationViewModel applicationViewModel)
        //{
        //    ValidationResultModel result = new ValidationResultModel()
        //    {
        //        IsValid = false
        //    };

        //    if(applicationViewModel != null && applicationViewModel.PurposeAndActivity != null)
        //    {
        //        result = ApplicationFormBasicValidationProcess.ValidatePurposeAndActivity(applicationViewModel.PurposeAndActivity);
        //    }

        //    return Json(result);
        //}

        [HttpPost]
        public JsonResult ValidatePurposeActivity(string[] reasonsForOpeningValues, string[] expectedNatureOfTranValues, string expectedFrequencyOfTranValues, string expectedIncome, string applicationId, string applicationNumber)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            PurposeAndActivityModel purposeAndActivityModel = new PurposeAndActivityModel();
            purposeAndActivityModel.ExpectedIncomingAmount = expectedIncome;
            purposeAndActivityModel.ReasonForOpeningTheAccountGroup = new MultiselectDropDownViewModel() { MultiSelectValue = reasonsForOpeningValues };
            purposeAndActivityModel.ExpectedNatureOfInAndOutTransactionGroup = new MultiselectDropDownViewModel() { MultiSelectValue = expectedNatureOfTranValues };
            purposeAndActivityModel.ExpectedFrequencyOfInAndOutTransactionGroup = new RadioGroupViewModel() { RadioGroupValue = expectedFrequencyOfTranValues };

            result = ApplicationFormBasicValidationProcess.ValidatePurposeAndActivity(purposeAndActivityModel);
            if (result.IsValid == true)
            {
                result = ApplicationGridValidationProcess.ValidateSourceOfInComingTransactions(Convert.ToInt32(applicationId));
                if (result.IsValid == true)
                {
                    result = ApplicationGridValidationProcess.ValidateSourceOfOutGoingTransactions(applicationNumber);
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidateGroupStructure(int applicantionId, string applicationNumber, string doesTheEntityBelongToAGroupName, string groupActivities, string groupName)
        {
            ValidationResultModel result = new ValidationResultModel()
            {
                IsValid = false
            };

            GroupStructureLegalParentModel groupStructureLegalParentModel = new GroupStructureLegalParentModel();
            //groupStructureLegalParentModel.DoesTheEntityBelongToAGroup = doesTheEntityBelongToAGroupName;
            groupStructureLegalParentModel.DoesTheEntityBelongToAGroupName = doesTheEntityBelongToAGroupName;
            groupStructureLegalParentModel.GroupName = groupName;
            groupStructureLegalParentModel.GroupActivities = groupActivities;

            result = ApplicationGridValidationProcess.ValidateGroupStructure(applicantionId, applicationNumber, groupStructureLegalParentModel);

            return Json(result);
        }

        public JsonResult ValidateSignatoryGroupDates(string startDate, string endDate)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(startDate))
            {
                isValid = false;
            }
            else if (!string.IsNullOrEmpty(startDate))
            {
                if (DateTime.Today < DateTime.ParseExact(startDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture))
                {
                    isValid = false;
                }
            }
            if (string.IsNullOrEmpty(endDate))
            {
                isValid = false;
            }
            else if (!string.IsNullOrEmpty(endDate))
            {
                if (DateTime.ParseExact(startDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) > DateTime.ParseExact(endDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture))
                {
                    isValid = false;
                }
            }
            return Json(isValid);
        }
        #endregion

        //[AllowAnonymous]
        #region----EXPORT XML-----
        public FileResult ExportXML(int applicationId)
        {
            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationId);
            string applicationType = ValidationHelper.GetString(ServiceHelper.GetEntityType(applicationDetails.ApplicationDetails_ApplicationType, Constants.APPLICATION_TYPE), "");
            string xml = string.Empty;
            if (string.Equals(applicationType, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase))
            {
                xml = ApplicationLegalService.GetApplicationLegalElement(applicationId);
            }
            else
            {
                xml = ApplicationIndividualService.GetApplicationIndividualElement(applicationId);
            }
            string fileName = applicationDetails.ApplicationDetails_ApplicationNumber + ".xml";
            return File(Encoding.UTF8.GetBytes(xml), "application/xml", fileName);
        }

        //[HttpPost]
        public string ExportXMLJS(string appId)
        {
            string xml = string.Empty;
            int applicationId = Convert.ToInt16(appId);

            CMS.Membership.UserInfo user = UserInfoProvider.GetUserInfo(User.Identity.Name);
            UserModel userModel = UserProcess.GetUser(User.Identity.Name);

            List<ApplicationDetailsModelView> applicationDetailsModels = ApplicationsProcess.GetApplicationByUserByApplication(userModel, applicationsRepository, applicationId);

            if (applicationDetailsModels != null && applicationDetailsModels.Count == 0)
            {
                return xml;

            }

            var applicationDetails = applicationsRepository.GetApplicationDetailsByID(applicationId);
            string applicationType = ValidationHelper.GetString(ServiceHelper.GetEntityType(applicationDetails.ApplicationDetails_ApplicationType, Constants.APPLICATION_TYPE), "");

            if (string.Equals(applicationType, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase))
            {
                xml = ApplicationLegalService.GetApplicationLegalElement(applicationId);
            }
            else
            {
                xml = ApplicationIndividualService.GetApplicationIndividualElement(applicationId);
            }
            return xml;
        }


        public ActionResult PrintSummary(string applicationNumber)
        {
            try
            {
                var applicationDetails = applicationsRepository.GetApplicationDetailsByApplicationNumber(applicationNumber);
                CMS.Membership.UserInfo user = UserInfoProvider.GetUserInfo(User.Identity.Name);
                UserModel userModel = UserProcess.GetUser(User.Identity.Name);

                List<ApplicationDetailsModelView> applicationDetailsModels = ApplicationsProcess.GetApplicationByUserByApplication(userModel, applicationsRepository, applicationDetails.ApplicationDetailsID);

                if (applicationDetailsModels != null && applicationDetailsModels.Count == 0)
                {
                    return Unauthorized();

                }
                //var OutputFileName = applicationNumber + "-Summary.pdf";  // here we need to rename <applicationnumber>-summary.pdf
                //var adminurl = "http://" + SiteContext.CurrentSite.DomainName; //http://localhost:51872
                //using (HttpClient client = new HttpClient())
                //{
                //    client.Timeout = TimeSpan.FromMinutes(30);

                //    client.BaseAddress = new Uri(adminurl);
                //    var response = client.GetAsync(string.Format("/customapi/CustomWebAPI?applicationNumber={0}", applicationNumber)).Result;
                //    return File(response.Content.ReadAsStreamAsync().Result, "application/pdf", OutputFileName);
                //}
                var OutputFileName = applicationNumber + "-Summary.pdf";  // here we need to rename <applicationnumber>-summary.pdf
                //var adminurl = "http://" + SiteContext.CurrentSite.DomainName; //http://localhost:51872
                var adminurl = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".admin_url");
                //exceptionlog("adminurl :- " + adminurl); ////////////////
                string FinalUrl = adminurl + "/customapi/CustomWebAPI?applicationNumber=" + applicationNumber;
                //exceptionlog("FinalUrl :- " + FinalUrl); ////////////////
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(30);
                var response = client.GetAsync(FinalUrl).Result;
                return File(response.Content.ReadAsStreamAsync().Result, "application/pdf", OutputFileName);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public void exceptionlog(string SaveString)
        {
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "ErrorLog/ExceptionLog.txt");
            if (!System.IO.File.Exists(filepath))
            {
                System.IO.File.Create(filepath).Dispose();
            }
            using (StreamWriter sw = System.IO.File.AppendText(filepath))
            {
                sw.WriteLine("-----------Url test " + " " + DateTime.Now.ToString() + "-----------------");
                sw.WriteLine("-------------------------------------------------------------------------------------");
                sw.WriteLine(SaveString);
                sw.WriteLine("--------------------------------*End*------------------------------------------");
                sw.Flush();
                sw.Close();
            }
        }
        public ActionResult PrintFriendly(string applicationNumber)
        {
            var applicationDetails = applicationsRepository.GetApplicationDetailsByApplicationNumber(applicationNumber);
            CMS.Membership.UserInfo user = UserInfoProvider.GetUserInfo(User.Identity.Name);
            UserModel userModel = UserProcess.GetUser(User.Identity.Name);

            List<ApplicationDetailsModelView> applicationDetailsModels = ApplicationsProcess.GetApplicationByUserByApplication(userModel, applicationsRepository, applicationDetails.ApplicationDetailsID);

            if (applicationDetailsModels != null && applicationDetailsModels.Count == 0)
            {
                return Unauthorized();

            }
            var OutputFileName = applicationNumber + "-Detailed" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".pdf";  // here we need to rename <applicationnumber>-Friendly.pdf
                                                                                                                                                                                                        //var adminurl = "http://" + SiteContext.CurrentSite.DomainName; 
            var adminurl = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".admin_url");

            string FinalUrl = adminurl + "/customapi/PrtFriendlyAPI?applicationNumber=" + applicationNumber;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(30);
            var response = client.GetAsync(FinalUrl).Result;
            return File(response.Content.ReadAsStreamAsync().Result, "application/pdf", OutputFileName);
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
        #endregion


        public JsonResult GetRelatedPartyRoles(string relatedPartyGuid, bool isLegalEntity)
        {

            List<string> roles = null;

            if (!string.IsNullOrEmpty(relatedPartyGuid))
            {
                if (isLegalEntity)
                {
                    bool IsRelatedPartyUBO = false;
                    int personalDetailsID = CommonProcess.GetPersonalDetailsLegalRelatedParty(relatedPartyGuid);
                    if (personalDetailsID != 0 && personalDetailsID != null)
                    {
                        var personalDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(personalDetailsID);
                        IsRelatedPartyUBO = personalDetails.IsRelatedPartyUBO;
                    }
                    roles = CommonProcess.GetEntitySeletedRolesLegal(relatedPartyGuid, IsRelatedPartyUBO);
                }
                else
                {
                    //int personDetailsId = CommonProcess.GetPersonalDetailsLegalRelatedParty(relatedPartyGuid);
                    //roles = CommonProcess.GetSelectedPartyRolesIndiVidual(personDetailsId);
                    roles = CommonProcess.GetEntitySeletedRolesIndividual(relatedPartyGuid);
                }

            }
            return Json(roles);
        }

        public JsonResult GetEntityTypeByPersonGUID(string personNodeGUID)
        {
            string result = string.Empty;
            result = CommonProcess.GetEntityRoleTypeByPersonGUID(personNodeGUID);
            return Json(result);
        }
        public ActionResult DuplicateApplication(string applicationGuid)
        {
            string ApplicationNumber = "";
            //int applicationId = 0;
            string NodePath = "";
            string parentpath = "";
            string ApplicationType = "";
            var NewApplicationNumber = "";
            if (!string.IsNullOrEmpty(applicationGuid))
            {
                var applicationdetails = applicationsRepository.GetApplicationDetailsByNodeGUID(applicationGuid);
                if (applicationdetails != null)
                {
                    ApplicationNumber = applicationdetails.ApplicationDetails_ApplicationNumber;
                    //applicationId = applicationdetails.ApplicationDetailsID;
                    NodePath = applicationdetails.NodeAliasPath;
                    parentpath = applicationdetails.Parent.NodeAliasPath;
                    ApplicationType = applicationdetails.ApplicationDetails_ApplicationType;
                }
            }
            // Gets the page that will be moved
            TreeNode page = new DocumentQuery<TreeNode>()
                .OnCurrentSite()
                    .Path(NodePath)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .TopN(1)
                    .FirstOrDefault();


            // Gets the page to copy under
            TreeNode targetPage = new DocumentQuery<TreeNode>()
                                      .Path(parentpath, PathTypeEnum.Single)
                                      .OnCurrentSite()
                                      .Culture(LocalizationContext.CurrentCulture.CultureCode)
                                      .TopN(1)
                                      .FirstOrDefault();

            if ((page != null) && (targetPage != null))
            {
                // Copies the page to the new location, including any child pages
                DocumentHelper.CopyDocument(page, targetPage, true);
            }
            // Gets the new copied page under targetPage where DocumentName = ApplicationNumber order by CreatedDate Descending
            IEnumerable<TreeNode> pagestest1 = new MultiDocumentQuery()
                                            .Path(parentpath, PathTypeEnum.Children)
                                            .WhereEquals("DocumentName", ApplicationNumber)
                                            .OrderByDescending("DocumentCreatedWhen")
                                            .OnCurrentSite()
                                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                                            .Published()
                                            .PublishedVersion()
                                            .TopN(1);

            foreach (TreeNode newCopiedPage in pagestest1)
            {

                IEnumerable<TreeNode> NewPageDecissionHistory = new MultiDocumentQuery()
                                            .Path(newCopiedPage.NodeAliasPath, PathTypeEnum.Children)
                                            .WhereEquals("ClassName", "Eurobank.DecisionHistory")
                                            .OnCurrentSite()
                                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                                            .Published()
                                            .PublishedVersion();
                foreach (TreeNode treeNode in NewPageDecissionHistory)
                {
                    // Indicates whether to delete all culture versions of the page or only the current one
                    bool deleteAllCultureVersions = true;
                    // Indicates whether the page will be deleted permanently with no restoration possible
                    bool deletePermanently = true;

                    // Permanently deletes all culture versions of the page and its version history
                    treeNode.Delete(deleteAllCultureVersions, deletePermanently);
                }

                IEnumerable<TreeNode> NewPageExpectedDocument = new MultiDocumentQuery()
                                            .Path(newCopiedPage.NodeAliasPath, PathTypeEnum.Children)
                                            .WhereEquals("ClassName", "Eurobank.ExpectedDocuments")
                                            .OnCurrentSite()
                                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                                            .Published()
                                            .PublishedVersion();
                foreach (TreeNode treeNode in NewPageExpectedDocument)
                {
                    bool deleteAllCultureVersions = true;
                    bool deletePermanently = true;
                    treeNode.Delete(deleteAllCultureVersions, deletePermanently);
                }

                IEnumerable<TreeNode> NewPageBankDocuments = new MultiDocumentQuery()
                                            .Path(newCopiedPage.NodeAliasPath, PathTypeEnum.Children)
                                            .WhereEquals("ClassName", "Eurobank.BankDocuments")
                                            .OnCurrentSite()
                                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                                            .Published()
                                            .PublishedVersion();
                foreach (TreeNode treeNode in NewPageBankDocuments)
                {
                    bool deleteAllCultureVersions = true;
                    bool deletePermanently = true;
                    treeNode.Delete(deleteAllCultureVersions, deletePermanently);
                }

                IEnumerable<TreeNode> NewPagePersonalDetails = new MultiDocumentQuery()
                                            .Path(newCopiedPage.NodeAliasPath, PathTypeEnum.Children)
                                            .WhereEquals("ClassName", "Eurobank.PersonalDetails")
                                            .OnCurrentSite()
                                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                                            .Published()
                                            .PublishedVersion();
                foreach (TreeNode treeNode in NewPagePersonalDetails)
                {
                    treeNode.SetValue("PersonalDetails_Invited", null);
                    treeNode.SetValue("PersonalDetails_IdVerified", null);
                    treeNode.SetValue("PersonalDetails_HIDInviteFlag", null);
                    treeNode.SetValue("PersonalDetails_Status", "Pending");
                    treeNode.Update();
                }
                IEnumerable<TreeNode> NewPageCompanyDetails = new MultiDocumentQuery()
                                            .Path(newCopiedPage.NodeAliasPath, PathTypeEnum.Children)
                                            .WhereEquals("ClassName", "Eurobank.CompanyDetails")
                                            .OnCurrentSite()
                                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                                            .Published()
                                            .PublishedVersion();
                foreach (TreeNode treeNode in NewPageCompanyDetails)
                {
                    treeNode.SetValue("CompanyDetails_Status", "Pending");
                    treeNode.Update();
                }
                IEnumerable<TreeNode> NewPageSignatoryGroup = new MultiDocumentQuery()
                                            .Path(newCopiedPage.NodeAliasPath, PathTypeEnum.Children)
                                            .WhereEquals("ClassName", "Eurobank.SignatoryGroup")
                                            .OnCurrentSite()
                                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                                            .Published()
                                            .PublishedVersion();
                foreach (TreeNode treeNode in NewPageSignatoryGroup)
                {
                    treeNode.SetValue("Status", "False");
                    treeNode.Update();
                }
                IEnumerable<TreeNode> NewPageSignatureMandateCompany = new MultiDocumentQuery()
                                            .Path(newCopiedPage.NodeAliasPath, PathTypeEnum.Children)
                                            .WhereEquals("ClassName", "Eurobank.SignatureMandateCompany")
                                            .OnCurrentSite()
                                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                                            .Published()
                                            .PublishedVersion();
                foreach (TreeNode treeNode in NewPageSignatureMandateCompany)
                {
                    treeNode.SetValue("Status", "False");
                    treeNode.Update();
                }
                IEnumerable<TreeNode> NewPageSignatureMandateIndividual = new MultiDocumentQuery()
                                            .Path(newCopiedPage.NodeAliasPath, PathTypeEnum.Children)
                                            .WhereEquals("ClassName", "Eurobank.SignatureMandateIndividual")
                                            .OnCurrentSite()
                                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                                            .Published()
                                            .PublishedVersion();
                foreach (TreeNode treeNode in NewPageSignatureMandateIndividual)
                {
                    treeNode.SetValue("SignatureMandateIndividual_Status", "False");
                    treeNode.Update();
                }

                IEnumerable<TreeNode> NewPageEbankingSubscriber = new MultiDocumentQuery()
                                            .Path(newCopiedPage.NodeAliasPath, PathTypeEnum.Children)
                                            .WhereEquals("ClassName", "Eurobank.EBankingSubscriberDetails")
                                            .OnCurrentSite()
                                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                                            .Published()
                                            .PublishedVersion();
                foreach (TreeNode treeNode in NewPageEbankingSubscriber)
                {
                    bool deleteAllCultureVersions = true;
                    bool deletePermanently = true;
                    treeNode.Delete(deleteAllCultureVersions, deletePermanently);
                }

                IEnumerable<TreeNode> NewPagedebitcardDetails = new MultiDocumentQuery()
                                            .Path(newCopiedPage.NodeAliasPath, PathTypeEnum.Children)
                                            .WhereEquals("ClassName", "Eurobank.DebitCardDetails")
                                            .OnCurrentSite()
                                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                                            .Published()
                                            .PublishedVersion();
                foreach (TreeNode treeNode in NewPagedebitcardDetails)
                {
                    treeNode.SetValue("DebitCardDetails_Status", "False");
                    treeNode.Update();
                }


                //IEnumerable<TreeNode> NewPageExpectedDocument = new MultiDocumentQuery()
                //                            .Path(newCopiedPage.NodeAliasPath, PathTypeEnum.Children)
                //                            .WhereEquals("ClassName", "Eurobank.ExpectedDocuments")
                //                            .OnCurrentSite()
                //                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                //                            .Published()
                //                            .PublishedVersion();
                //foreach (TreeNode treeNode in NewPageExpectedDocument)
                //{
                //    treeNode.SetValue("ExpectedDocuments_SyncDate", null);
                //    treeNode.Update();
                //}

                NewApplicationNumber = ServiceHelper.GetApplicationNumber(Convert.ToInt32(newCopiedPage.GetValue("ApplicationDetailsID")), ServiceHelper.GetName(ApplicationType, "/Lookups/General/APPLICATION-TYPE")); //New Application Number
                var NewCopiedApplicationPath1 = newCopiedPage.NodeAlias;
                newCopiedPage.DocumentName = NewApplicationNumber;
                newCopiedPage.SetValue("ApplicationDetails_ApplicationNumber", NewApplicationNumber);
                newCopiedPage.SetValue("ApplicationDetails_ApplicationStatus", ServiceHelper.GetApplicationStatuses().FirstOrDefault(h => string.Equals(h.Text, ApplicationWorkflowStatus.DRAFT.ToString(), StringComparison.OrdinalIgnoreCase)).Value); //Draft
                newCopiedPage.SetValue("ApplicationDetails_ApplicationLastStatus", "");
                newCopiedPage.SetValue("ApplicationDetails_SubmittedBy", "");
                newCopiedPage.SetValue("ApplicationDetails_DocumentSubmittedByUserID", UserProcess.GetUser(User.Identity.Name).UserInformation.UserID);
                newCopiedPage.SetValue("ApplicationDetails_SubmittedOn", null); //Convert.ToDateTime("0001/01/01 00:00:00")
                newCopiedPage.SetValue("ApplicationDetails_T24SyncDate", null);
                newCopiedPage.SetValue("ApplicationDetails_DMSyncStatus", null);
                newCopiedPage.Update();
            }
            TempData["CopiedSuccessMsg"] = "APPLICANT DETAILS COPIED SUCCESSFULLY, NEW REFERENCE NUMBER IS : " + NewApplicationNumber;
            return Redirect(Url.Action("Index", "Applications"));
        }

        #region-----------Export XLSX---------------
        public ActionResult DownloadApplications()
        {
            UserModel user = UserProcess.GetUser(User.Identity.Name);
            var listApplications = ApplicationExcelDownloadProcess.GetApplicationByUser(user, applicationsRepository);
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[10] { new DataColumn("Reference"),
                                        new DataColumn("Status"),
                                        new DataColumn("Type"),
                                        new DataColumn("Full Name"),
                                        new DataColumn("Indentification"),
                                        new DataColumn("Introducer"),
                                        new DataColumn("Bank Center"),
                                        new DataColumn("Submitted By"),
                                        new DataColumn("Submitted On"),
                                        new DataColumn("Created On")});
            foreach (var item in listApplications)
            {
                dt.Rows.Add(item.ApplicationDetails_ApplicationNumber, item.ApplicationDetails_ApplicationStatusName, item.ApplicationDetails_ApplicationTypeName, item.FullNameOfApplicant, item.ApplicantIdentificationNumber, item.ApplicationDetails_Introducer, item.ApplicationDetails_ResponsibleBankingCenterUnit, item.ApplicationDetails_SubmittedBy, item.ApplicationDetails_SubmittedOn, item.ApplicationDetails_CreatedOn);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Applications.xlsx");
                }
            }

        }

        public DataTable GetApplicationRecords()
        {
            UserModel user = UserProcess.GetUser(User.Identity.Name);
            var listApplications = ApplicationExcelDownloadProcess.GetApplicationByUser(user, applicationsRepository);
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[10] { new DataColumn("Application ID"),
                                        new DataColumn("Status"),
                                        new DataColumn("Type"),
                                        new DataColumn("Applicant's Name"),
                                        new DataColumn("Indentification"),
                                        new DataColumn("Business Associate"),
                                        new DataColumn("Banking Centre"),
                                        new DataColumn("Submitted By"),
                                        new DataColumn("Submitted On"),
                                        new DataColumn("Created On")});
            foreach (var item in listApplications)
            {
                dt.Rows.Add(item.ApplicationDetails_ApplicationNumber, item.ApplicationDetails_ApplicationStatusName.ToUpper(), item.ApplicationDetails_ApplicationTypeName.ToUpper(), item.FullNameOfApplicant?.Replace(',', '|'), item.ApplicantIdentificationNumber?.Replace(',', '|'), item.ApplicationDetails_Introducer.ToUpper(), item.ApplicationDetails_ResponsibleBankingCenterUnit.ToUpper(), item.ApplicationDetails_SubmittedBy.ToUpper(), item.ApplicationDetails_SubmittedOn, item.ApplicationDetails_CreatedOn);
            }
            return dt;
        }
        #endregion

        #region--Application data sync from main field to search field

        public ActionResult SyncApplicationData()
        {
            ApplicationSyncService.SyncApplicationSearchRecord("");
            return Redirect(Url.Action("Index", "Applications"));
        }

        #endregion
    }
}
