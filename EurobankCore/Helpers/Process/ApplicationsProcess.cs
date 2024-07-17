using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using DocumentFormat.OpenXml.Bibliography;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models;
using Eurobank.Models.Application;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Applications;
using Eurobank.Models.IdentificationDetails;
using Eurobank.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class ApplicationsProcess
    {

        private static readonly string _ApplicationRootNodePath = "/Applications-(1)/";

        public static ApplicationDetailsModelView SaveApplicationsModel(ApplicationDetailsModelView applicationDetailsModelView, string username)
        {
            ApplicationDetailsModelView retVal = new ApplicationDetailsModelView();
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            UserInfo user = UserInfoProvider.GetUserInfo(username);
            string currentYear = ValidationHelper.GetString(DateTime.Now.Year, "");
            string currentMonth = ValidationHelper.GetString(DateTime.Now.ToString("MMMM"), "");
            CMS.DocumentEngine.TreeNode applicationfoldernode_parent = tree.SelectNodes()
                .Path(_ApplicationRootNodePath + currentYear + "/" + currentMonth)
                .OnCurrentSite()
                .Published(false)
                .FirstOrDefault();
            var statuses = ServiceHelper.GetApplicationStatuses();
            if (statuses != null && statuses.Any(h => string.Equals(h.Text, ApplicationWorkflowStatus.DRAFT.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                applicationDetailsModelView.ApplicationDetails_ApplicationStatus = statuses.FirstOrDefault(h => string.Equals(h.Text, ApplicationWorkflowStatus.DRAFT.ToString(), StringComparison.OrdinalIgnoreCase)).Value;
            }
            //applicationDetailsModelView.ApplicationDetails_ApplicationStatus = "DRAFT";
            if (applicationfoldernode_parent != null)
            {

                CMS.DocumentEngine.TreeNode personsRegistryAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.ApplicationDetails", tree);
                personsRegistryAdd.DocumentName = "000000000";
                if (applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup != null)
                {
                    applicationDetailsModelView.ApplicationDetails_ApplicatonServices = (applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue != null && applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue.Length > 0) ? string.Join('|', applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue) : string.Empty;
                }
                var applicationServicesItemGroup = ServiceHelper.GetApplicationServiceItemGroup();
                if (applicationServicesItemGroup != null && applicationServicesItemGroup.Any(m => string.Equals(m.Label, "Account", StringComparison.OrdinalIgnoreCase)))
                {
                    if (!string.IsNullOrEmpty(applicationDetailsModelView.ApplicationDetails_ApplicatonServices))
                    {
                        applicationDetailsModelView.ApplicationDetails_ApplicatonServices += "|" + applicationServicesItemGroup.FirstOrDefault(m => string.Equals(m.Label, "Account", StringComparison.OrdinalIgnoreCase)).Value;
                    }
                    else
                    {
                        applicationDetailsModelView.ApplicationDetails_ApplicatonServices = applicationServicesItemGroup.FirstOrDefault(m => string.Equals(m.Label, "Account", StringComparison.OrdinalIgnoreCase)).Value;
                    }

                }

                personsRegistryAdd.SetValue("ApplicationDetails_ApplicationStatus", applicationDetailsModelView.ApplicationDetails_ApplicationStatus);
                personsRegistryAdd.SetValue("ApplicationDetails_ApplicationType", applicationDetailsModelView.ApplicationDetails_ApplicationType);
                personsRegistryAdd.SetValue("ApplicationDetails_ApplicatonServices", applicationDetailsModelView.ApplicationDetails_ApplicatonServices);
                personsRegistryAdd.SetValue("ApplicationDetails_IntroducerCIF", applicationDetailsModelView.ApplicationDetails_IntroducerCIF);
                personsRegistryAdd.SetValue("ApplicationDetails_IntroducerName", applicationDetailsModelView.ApplicationDetails_IntroducerName);
                personsRegistryAdd.SetValue("ApplicationDetails_ResponsibleBankingCenter", applicationDetailsModelView.ApplicationDetails_ResponsibleBankingCenter);
                personsRegistryAdd.SetValue("ApplicationDetails_ResponsibleOfficer", applicationDetailsModelView.ApplicationDetails_ResponsibleOfficer);
                personsRegistryAdd.SetValue("ApplicationDetails_SubmittedBy", user.FullName);
                personsRegistryAdd.SetValue("ApplicationDetails_DocumentSubmittedByUserID", user.UserID);
                personsRegistryAdd.SetValue("ApplicationDetails_UserOrganisation", ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserOrganisation"), ""));
                //personsRegistryAdd.SetValue("ApplicationDetails_SubmittedOn", DateTime.Now);
                personsRegistryAdd.Insert(applicationfoldernode_parent);
                personsRegistryAdd.DocumentName = ServiceHelper.GetApplicationNumber(ValidationHelper.GetInteger(personsRegistryAdd.GetValue("ApplicationDetailsID"), 0), ServiceHelper.GetName(applicationDetailsModelView.ApplicationDetails_ApplicationType, "/Lookups/General/APPLICATION-TYPE"));
                personsRegistryAdd.NodeAlias = personsRegistryAdd.DocumentName;
                personsRegistryAdd.SetValue("ApplicationDetails_ApplicationNumber", personsRegistryAdd.DocumentName);
                personsRegistryAdd.Update();
                retVal.ApplicationDetailsID = ValidationHelper.GetInteger(personsRegistryAdd.GetValue("ApplicationDetailsID"), 0);
                retVal.Application_NodeGUID = personsRegistryAdd.NodeGUID.ToString();
            }
            else
            {

                CMS.DocumentEngine.TreeNode apllicationsFolder = tree.SelectNodes()
           .Path("/Applications-(1)")
           .Published(false)
           .OnCurrentSite()
           .FirstOrDefault();
                CMS.DocumentEngine.TreeNode apllicationsYearFolder = tree.SelectNodes()
           .Path(_ApplicationRootNodePath + currentYear)
           .Published(false)
           .OnCurrentSite()
           .FirstOrDefault();
                if (apllicationsYearFolder == null)
                {
                    apllicationsYearFolder = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                    apllicationsYearFolder.DocumentName = currentYear;
                    apllicationsYearFolder.DocumentCulture = "en-US";
                    apllicationsYearFolder.Insert(apllicationsFolder);
                }
                CMS.DocumentEngine.TreeNode apllicationsMonthFolder = tree.SelectNodes()
           .Path(_ApplicationRootNodePath + currentYear + "/" + currentMonth)
           .Published(false)
           .OnCurrentSite()
           .FirstOrDefault();
                if (apllicationsMonthFolder == null)
                {
                    apllicationsMonthFolder = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                    apllicationsMonthFolder.DocumentName = currentMonth;
                    apllicationsMonthFolder.DocumentCulture = "en-US";
                    apllicationsMonthFolder.Insert(apllicationsYearFolder);
                }

                CMS.DocumentEngine.TreeNode personsRegistryAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.ApplicationDetails", tree);
                personsRegistryAdd.DocumentName = "000000000";

                if (applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup != null)
                {
                    applicationDetailsModelView.ApplicationDetails_ApplicatonServices = (applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue != null && applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue.Length > 0) ? string.Join('|', applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue) : string.Empty;
                }
                var applicationServicesItemGroup = ServiceHelper.GetApplicationServiceItemGroup();
                if (applicationServicesItemGroup != null && applicationServicesItemGroup.Any(m => string.Equals(m.Label, "Account", StringComparison.OrdinalIgnoreCase)))
                {
                    if (!string.IsNullOrEmpty(applicationDetailsModelView.ApplicationDetails_ApplicatonServices))
                    {
                        applicationDetailsModelView.ApplicationDetails_ApplicatonServices += "|" + applicationServicesItemGroup.FirstOrDefault(m => string.Equals(m.Label, "Account", StringComparison.OrdinalIgnoreCase)).Value;
                    }
                    else
                    {
                        applicationDetailsModelView.ApplicationDetails_ApplicatonServices = applicationServicesItemGroup.FirstOrDefault(m => string.Equals(m.Label, "Account", StringComparison.OrdinalIgnoreCase)).Value;
                    }

                }

                personsRegistryAdd.SetValue("ApplicationDetails_ApplicationStatus", applicationDetailsModelView.ApplicationDetails_ApplicationStatus);
                personsRegistryAdd.SetValue("ApplicationDetails_ApplicationType", applicationDetailsModelView.ApplicationDetails_ApplicationType);
                personsRegistryAdd.SetValue("ApplicationDetails_ApplicatonServices", applicationDetailsModelView.ApplicationDetails_ApplicatonServices);
                personsRegistryAdd.SetValue("ApplicationDetails_IntroducerCIF", applicationDetailsModelView.ApplicationDetails_IntroducerCIF);
                personsRegistryAdd.SetValue("ApplicationDetails_IntroducerName", applicationDetailsModelView.ApplicationDetails_IntroducerName);
                personsRegistryAdd.SetValue("ApplicationDetails_ResponsibleBankingCenter", applicationDetailsModelView.ApplicationDetails_ResponsibleBankingCenter);
                personsRegistryAdd.SetValue("ApplicationDetails_ResponsibleOfficer", applicationDetailsModelView.ApplicationDetails_ResponsibleOfficer);
                personsRegistryAdd.SetValue("ApplicationDetails_SubmittedBy", user.FullName);
                personsRegistryAdd.SetValue("ApplicationDetails_DocumentSubmittedByUserID", user.UserID);
                personsRegistryAdd.SetValue("ApplicationDetails_UserOrganisation", ValidationHelper.GetString(user.UserSettings.GetValue("Eurobank_UserOrganisation"), ""));
                //personsRegistryAdd.SetValue("ApplicationDetails_SubmittedOn", DateTime.Now);
                personsRegistryAdd.Insert(apllicationsMonthFolder);
                personsRegistryAdd.DocumentName = ServiceHelper.GetApplicationNumber(ValidationHelper.GetInteger(personsRegistryAdd.GetValue("ApplicationDetailsID"), 0), ServiceHelper.GetName(applicationDetailsModelView.ApplicationDetails_ApplicationType, "/Lookups/General/APPLICATION-TYPE"));
                personsRegistryAdd.NodeAlias = personsRegistryAdd.DocumentName;
                personsRegistryAdd.SetValue("ApplicationDetails_ApplicationNumber", personsRegistryAdd.DocumentName);
                personsRegistryAdd.Update();

                retVal.ApplicationDetailsID = ValidationHelper.GetInteger(personsRegistryAdd.GetValue("ApplicationDetailsID"), 0);
                retVal.Application_NodeGUID = personsRegistryAdd.NodeGUID.ToString();
            }

            return retVal;
        }
        public static ApplicationDetailsModelView UpdateApplicationsModel(ApplicationDetailsModelView applicationDetailsModelView, string username)
        {

            if (applicationDetailsModelView.ApplicationDetailsID > 0)
            {
                if (applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup != null)
                {
                    applicationDetailsModelView.ApplicationDetails_ApplicatonServices = (applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue != null && applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue.Length > 0) ? string.Join('|', applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup.CheckBoxGroupValue) : string.Empty;
                }
                var applicationServicesItemGroup = ServiceHelper.GetApplicationServiceItemGroup();
                if (applicationServicesItemGroup != null && applicationServicesItemGroup.Any(m => string.Equals(m.Label, "Account", StringComparison.OrdinalIgnoreCase)))
                {
                    if (!string.IsNullOrEmpty(applicationDetailsModelView.ApplicationDetails_ApplicatonServices))
                    {
                        applicationDetailsModelView.ApplicationDetails_ApplicatonServices += "|" + applicationServicesItemGroup.FirstOrDefault(m => string.Equals(m.Label, "Account", StringComparison.OrdinalIgnoreCase)).Value;
                    }
                    else
                    {
                        applicationDetailsModelView.ApplicationDetails_ApplicatonServices = applicationServicesItemGroup.FirstOrDefault(m => string.Equals(m.Label, "Account", StringComparison.OrdinalIgnoreCase)).Value;
                    }

                }

                ApplicationDetails applicationDetails = GetApplicationDetailsById(applicationDetailsModelView.ApplicationDetailsID);

                if (applicationDetails != null)
                {
                    applicationDetails.ApplicationDetails_ApplicatonServices = applicationDetailsModelView.ApplicationDetails_ApplicatonServices;
                    applicationDetails.Update();

                }

            }

            return applicationDetailsModelView;
        }
        public static void UpdateApplicationSubmitedOn(int applicationID, string username)
        {
            ApplicationDetails applicationDetails = GetApplicationDetailsById(applicationID);
            UserInfo user = UserInfoProvider.GetUserInfo(username);
            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_SubmittedOn = DateTime.Now;
                applicationDetails.ApplicationDetails_SubmittedBy = user.FullName;
                applicationDetails.Update();

            }

        }
        public static ApplicationDetailsModelView GetApplicationsDetails(CMS.DocumentEngine.Types.Eurobank.ApplicationDetails applicationDetails)
        {
            ApplicationDetailsModelView applicationDetailsModelView = new ApplicationDetailsModelView();
            applicationDetailsModelView.ApplicationDetailsID = applicationDetails.ApplicationDetailsID;
            applicationDetailsModelView.ApplicationDetails_ApplicationNumber = applicationDetails.ApplicationDetails_ApplicationNumber;
            applicationDetailsModelView.ApplicationDetails_SubmittedBy = applicationDetails.ApplicationDetails_SubmittedBy;
            applicationDetailsModelView.ApplicationDetails_ApplicationType = applicationDetails.ApplicationDetails_ApplicationType;
            applicationDetailsModelView.ApplicationDetails_ApplicationTypeName = ServiceHelper.GetName(ValidationHelper.GetString(applicationDetails.ApplicationDetails_ApplicationType, ""), "/Lookups/General/APPLICATION-TYPE");
            applicationDetailsModelView.ApplicationDetails_ApplicationStatusName = ServiceHelper.GetName(ValidationHelper.GetString(applicationDetails.ApplicationDetails_ApplicationStatus, ""), "/Lookups/General/APPLICATION-WORKFLOW-STATUS");
            applicationDetailsModelView.ApplicationDetails_SubmittedOn = applicationDetails.ApplicationDetails_SubmittedOn.ToString("dd/MM/yyyy HH:mm:ss");
            applicationDetailsModelView.ApplicationDetails_CreatedOn = applicationDetails.DocumentCreatedWhen.ToString("dd/MM/yyyy HH:mm:ss");
            applicationDetailsModelView.ApplicationDetails_ResponsibleBankingCenter = ValidationHelper.GetString(applicationDetails.ApplicationDetails_ResponsibleBankingCenter, "");
            applicationDetailsModelView.ApplicationDetails_ResponsibleOfficer = ValidationHelper.GetString(applicationDetails.ApplicationDetails_ResponsibleOfficer, "");
            applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup = ControlBinder.BindCheckBoxGroupItems(ServiceHelper.GetApplicationServiceItemGroup(), applicationDetails.ApplicationDetails_ApplicatonServices, '|');
            applicationDetailsModelView.ApplicationDetails_IntroducerName = applicationDetails.ApplicationDetails_IntroducerName;

            return applicationDetailsModelView;
        }

        public static ApplicationDetailsModelView GetApplicationsDetails(string userType, string userRole, CMS.DocumentEngine.Types.Eurobank.ApplicationDetails applicationDetails)
        {
            ApplicationDetailsModelView applicationDetailsModelView = new ApplicationDetailsModelView();
            if (applicationDetails != null)
            {
                applicationDetailsModelView = BindApplicationDetailsModelView(userType, userRole, applicationDetails);
                applicationDetailsModelView.ApplicationDetails_ResponsibleBankingCenter = ValidationHelper.GetString(applicationDetails.ApplicationDetails_ResponsibleBankingCenter, "");
                applicationDetailsModelView.ApplicationDetails_ResponsibleOfficer = ValidationHelper.GetString(applicationDetails.ApplicationDetails_ResponsibleOfficer, "");
                applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup = ControlBinder.BindCheckBoxGroupItems(ServiceHelper.GetApplicationServiceItemGroup(), applicationDetails.ApplicationDetails_ApplicatonServices, '|');
            }

            return applicationDetailsModelView;
        }

        public static ApplicationDetailsModelView GetApplicationDetailsModelById(int applicationDetailsId)
        {
            ApplicationDetailsModelView retVal = null;

            if (applicationDetailsId > 0)
            {
                ApplicationDetails applicationDetails = GetApplicationDetailsById(applicationDetailsId);
                if (applicationDetails != null)
                {
                    retVal = GetApplicationsDetails(applicationDetails);
                }
            }

            return retVal;
        }

        public static TreeNode GetRootApplicationTreeNode(string applicationNumber)
        {
            TreeNode retVal = null;
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            if (!string.IsNullOrEmpty(applicationNumber))
            {
                retVal = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type("Eurobank.ApplicationDetails")
                    .WhereEquals("NodeName", applicationNumber)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();
            }

            return retVal;
        }

        public static int GetApplicationId(string applicationNumber)
        {
            int retVal = 0;
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            if (!string.IsNullOrEmpty(applicationNumber))
            {
                var node = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type("Eurobank.ApplicationDetails")
                    .WhereEquals("NodeName", applicationNumber)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();
                if (node != null)
                {
                    retVal = node.GetIntegerValue("ApplicationDetailsID", 0);
                }
            }

            return retVal;
        }

        public static ApplicationDetails GetApplicationDetailsById(int applicationDetailsId)
        {
            ApplicationDetails retVal = null;
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);


            if (applicationDetailsId > 0)
            {
                var applicationDetails = ApplicationDetailsProvider.GetApplicationDetails();
                if (applicationDetails != null && applicationDetails.Count > 0)
                {
                    retVal = applicationDetails.FirstOrDefault(o => o.ApplicationDetailsID == applicationDetailsId);
                }
            }

            return retVal;
        }

        public static ApplicationModel GetApplicationModelByApplicationIdExtented(int applicationId)
        {
            ApplicationModel applicationModel = new ApplicationModel();
            ApplicationDetailsModelView applicationDetailsModelView = new ApplicationDetailsModelView();

            ApplicationDetails applicationDetails = GetApplicationDetailsById(applicationId);

            if (applicationDetails != null)
            {
                applicationDetailsModelView.ApplicationDetailsID = applicationDetails.ApplicationDetailsID;
                applicationDetailsModelView.ApplicationDetails_ApplicationNumber = applicationDetails.ApplicationDetails_ApplicationNumber;
                applicationDetailsModelView.ApplicationDetails_SubmittedBy = applicationDetails.ApplicationDetails_SubmittedBy;
                applicationDetailsModelView.ApplicationDetails_ApplicationType = applicationDetails.ApplicationDetails_ApplicationType;
                applicationDetailsModelView.ApplicationDetails_ApplicationTypeName = ServiceHelper.GetName(ValidationHelper.GetString(applicationDetails.ApplicationDetails_ApplicationType, ""), "/Lookups/General/APPLICATION-TYPE");
                applicationDetailsModelView.ApplicationDetails_SubmittedOn = applicationDetails.ApplicationDetails_SubmittedOn.ToString("MM/dd/yyyy HH:mm:ss");
                applicationDetailsModelView.ApplicationDetails_CreatedOn = applicationDetails.DocumentCreatedWhen.ToString("MM/dd/yyyy HH:mm:ss");
                applicationDetailsModelView.ApplicationDetails_ResponsibleBankingCenter = ValidationHelper.GetString(applicationDetails.ApplicationDetails_ResponsibleBankingCenter, "");
                applicationDetailsModelView.ApplicationDetails_ResponsibleOfficer = ValidationHelper.GetString(applicationDetails.ApplicationDetails_ResponsibleOfficer, "");
                applicationDetailsModelView.ApplicationDetails_ApplicatonServicesGroup = ControlBinder.BindCheckBoxGroupItems(ServiceHelper.GetApplicationServiceItemGroup(), applicationDetails.ApplicationDetails_ApplicatonServices, '|');
                applicationDetailsModelView.ApplicationDetails_IntroducerName = applicationDetails.ApplicationDetails_IntroducerName;
                applicationDetailsModelView.ApplicationDetails_IntroducerCIF = applicationDetails.ApplicationDetails_IntroducerCIF;
                applicationDetailsModelView.ApplicationDetails_ApplicationStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationModel.ApplicationDetails = applicationDetailsModelView;
                applicationModel.PurposeAndActivity = PurposeAndActivityProcess.GetPurposeAndActivityModel(applicationDetailsModelView.ApplicationDetails_ApplicationNumber);
                bool isLegalEntity = false;
                if (!string.IsNullOrEmpty(applicationDetailsModelView.ApplicationDetails_ApplicationTypeName))
                {
                    if (string.Equals(applicationDetailsModelView.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                    {
                        isLegalEntity = true;
                        applicationModel.Applicants = ApplicantProcess.GetApplicantModelsExtendedLegal(applicationDetailsModelView.ApplicationDetails_ApplicationNumber);
                        applicationModel.RelatedParties = RelatedPartyProcess.GetRelatedPartyModelsExtendedLegal(applicationDetailsModelView.ApplicationDetails_ApplicationNumber);
                        applicationModel.SignatoryGroup = SignatoryGroupProcess.GetApplicationSignatoryGroup(applicationDetailsModelView.ApplicationDetailsID);
                        applicationModel.SignatureMandateCompany = SignatureMandateLegalProcess.GetSignatureMandateLegalModels(applicationDetailsModelView.ApplicationDetails_ApplicationNumber);

                        applicationModel.GroupStructureLegalParent = GroupStructureLegalParentProcess.GetGroupStructureLegalParentModel(applicationDetailsModelView.ApplicationDetails_ApplicationNumber);
                        applicationModel.GroupStructure = CompanyGroupStructureProcess.GetCompanyGroupStructure(applicationDetailsModelView.ApplicationDetailsID, applicationDetailsModelView.ApplicationDetails_ApplicationNumber);

                    }
                    else
                    {
                        applicationModel.Applicants = ApplicantProcess.GetApplicantModelsExtended(applicationDetailsModelView.ApplicationDetails_ApplicationNumber);
                        applicationModel.RelatedParties = RelatedPartyProcess.GetRelatedPartyModelsExtended(applicationDetailsModelView.ApplicationDetails_ApplicationNumber);
                        applicationModel.SignatureMandates = SignatureMandateIndividualProcess.GetSignatureMandateIndividualModels(applicationDetailsModelView.ApplicationDetails_ApplicationNumber);

                    }

                }

                applicationModel.SourceOfIncomingTransactions = SourceOfIncomingTransactionsProcess.GetSourceOfIncomeByApplicationID(applicationDetailsModelView.ApplicationDetailsID);
                applicationModel.SourceOfOutgoingTransactions = SourceOfOutgoingTransactionProcess.GetSourceOfOutgoingTransactionModels(applicationDetailsModelView.ApplicationDetails_ApplicationNumber);
                applicationModel.Accounts = AccountsProcess.GetAccountsByApplicationID(applicationDetailsModelView.ApplicationDetailsID);

                applicationModel.EBankingSubscribers = EBankingSubscriberDetailsProcess.GetEBankingSubscriberDetailsModels(applicationDetailsModelView.ApplicationDetails_ApplicationNumber, isLegalEntity);
                applicationModel.DebitCards = DebitCardeDetailsProcess.GetDebitCardDetailsByApplicationID(applicationDetailsModelView.ApplicationDetailsID);
                //applicationModel.BankDocuments = BankDocumentsProcess.GetBankDocumentsDetailsByApplicationID(applicationDetailsModelView.ApplicationDetailsID);
                //applicationModel.ExpectedDocuments = ExpectedDocumentsProcess.GetExpectedDocumentsDetailsByApplicationID(applicationDetailsModelView.ApplicationDetailsID);
                //applicationModel.Notes = NoteDetailsProcess.GetNoteDetailsModels(applicationDetailsModelView.ApplicationDetails_ApplicationNumber);
            }

            return applicationModel;
        }

        #region Application by User permisson
        public static List<ApplicationDetailsModelView> GetApplicationByUser(UserModel user, ApplicationsRepository applicationsRepository, int pageNumber, int pageSize, out int totalCount, string sortMember, string sortDirection, string txtSearch, string filterColumn, string stausFilter)
        {
            List<ApplicationDetailsModelView> retVal = null;
            int totalCountApplication = 0;
            if (user != null && user.UserInformation != null && !string.IsNullOrEmpty(user.UserType) && applicationsRepository != null)
            {
                retVal = new List<ApplicationDetailsModelView>();
                IEnumerable<ApplicationDetails> applications = null;

                //if (string.Equals(user.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase) && string.Equals(user.UserRole, ApplicationUserRole.POWER.ToString(), StringComparison.OrdinalIgnoreCase))
                //{
                //	applications = applicationsRepository.GetAllApplicationDetails();
                //}
                if (string.Equals(user.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase) && string.Equals(user.UserRole, ApplicationUserRole.POWER.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    string userOrganization = ValidationHelper.GetString(user.UserInformation.UserSettings.GetValue("Eurobank_UserOrganisation"), "");
                    applications = applicationsRepository.GetApplicationDetailsForIntroducerPower(userOrganization, pageNumber, pageSize, out totalCountApplication, sortMember, sortDirection, txtSearch, filterColumn, stausFilter);
                }
                else if (string.Equals(user.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    string userOrganization = ValidationHelper.GetString(user.UserInformation.UserSettings.GetValue("Eurobank_UserOrganisation"), "");
                    applications = applicationsRepository.GetApplicationDetailsForIntroducerNormal(user.UserInformation.UserID, pageNumber, pageSize, out totalCountApplication, sortMember, sortDirection, txtSearch, filterColumn, stausFilter);
                }
                else if (string.Equals(user.UserType, ApplicationUserType.INTERNAL.ToString(), StringComparison.OrdinalIgnoreCase) && string.Equals(user.UserRole, ApplicationUserRole.POWER.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    //List<Guid> bankBranches = ServiceHelper.GetAllSubBankUnitsAlogWithParent(ValidationHelper.GetString(user.UserInformation.UserSettings.GetValue("Eurobank_UserOrganisation"), ""));
                    if (user.InternalUser != null && user.InternalUser.AssignBankBranches != null && user.InternalUser.AssignBankBranches.Count > 0)
                    {
                        applications = applicationsRepository.GetApplicationDetailsForInternalUserPower(user.InternalUser.AssignBankBranches, pageNumber, pageSize, out totalCountApplication, sortMember, sortDirection, txtSearch, filterColumn, stausFilter);
                    }
                }
                else if (string.Equals(user.UserType, ApplicationUserType.INTERNAL.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    //List<Guid> bankBranches = ServiceHelper.GetAllSubBankUnitsAlogWithParent(ValidationHelper.GetString(user.UserInformation.UserSettings.GetValue("Eurobank_UserOrganisation"), ""));
                    if (user.InternalUser != null && user.InternalUser.AssignBankBranches != null && user.InternalUser.AssignBankBranches.Count > 0)
                    {
                        applications = applicationsRepository.GetApplicationDetailsForInternalUserNormal(user.InternalUser.AssignBankBranches, pageNumber, pageSize, out totalCountApplication, sortMember, sortDirection, txtSearch, filterColumn, stausFilter);
                    }
                }
                if (applications != null)
                {
                    foreach (var application in applications)
                    {
                        retVal.Add(BindApplicationDetailsModelView(user.UserType, user.UserRole, application));
                    }
                }

            }
            totalCount = totalCountApplication;
            return retVal;
        }

        //public static string GetMatchingColumnOfDB(string ModelVariable)
        //{
        //    string retval = string.Empty;
        //    switch (ModelVariable)
        //    {
        //        case "ApplicationDetails_CreatedOn":
        //            retval = "DocumentCreatedWhen";
        //            break;
        //        case "ApplicationDetails_SubmittedOn":
        //            retval = "ApplicationDetails_SubmittedOn";
        //            break;
        //        case "ApplicationDetails_ApplicationTypeName":
        //            retval = "ApplicationDetails_ApplicationType";
        //            break;
        //        default:
        //            retval = "NodeOrder";
        //            break;
        //    }
        //    return retval;
        //}

        public static List<ApplicationDetailsModelView> GetApplicationByUserByApplication(UserModel user, ApplicationsRepository applicationsRepository, int ApplicationDetailsID)
        {
            List<ApplicationDetailsModelView> retVal = new List<ApplicationDetailsModelView>();
            if (user != null && user.UserInformation != null && !string.IsNullOrEmpty(user.UserType) && applicationsRepository != null)
            {
                retVal = new List<ApplicationDetailsModelView>();
                IEnumerable<ApplicationDetails> applications = null;

                //if (string.Equals(user.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase) && string.Equals(user.UserRole, ApplicationUserRole.POWER.ToString(), StringComparison.OrdinalIgnoreCase))
                //{
                //	applications = applicationsRepository.GetAllApplicationDetails();
                //}
                if (string.Equals(user.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase) && string.Equals(user.UserRole, ApplicationUserRole.POWER.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    string userOrganization = ValidationHelper.GetString(user.UserInformation.UserSettings.GetValue("Eurobank_UserOrganisation"), "");
                    applications = applicationsRepository.GetApplicationDetailsForIntroducerPowerByApplicationDetailsID(userOrganization, ApplicationDetailsID);
                }
                else if (string.Equals(user.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    string userOrganization = ValidationHelper.GetString(user.UserInformation.UserSettings.GetValue("Eurobank_UserOrganisation"), "");
                    applications = applicationsRepository.GetApplicationDetailsForIntroducerNormalByApplicationDetailsID(user.UserInformation.UserID, ApplicationDetailsID);
                }
                else if (string.Equals(user.UserType, ApplicationUserType.INTERNAL.ToString(), StringComparison.OrdinalIgnoreCase) && string.Equals(user.UserRole, ApplicationUserRole.POWER.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    //List<Guid> bankBranches = ServiceHelper.GetAllSubBankUnitsAlogWithParent(ValidationHelper.GetString(user.UserInformation.UserSettings.GetValue("Eurobank_UserOrganisation"), ""));
                    if (user.InternalUser != null && user.InternalUser.AssignBankBranches != null && user.InternalUser.AssignBankBranches.Count > 0)
                    {
                        applications = applicationsRepository.GetApplicationDetailsForInternalUserPowerByApplicationDetailsID(user.InternalUser.AssignBankBranches, ApplicationDetailsID);
                    }
                }
                else if (string.Equals(user.UserType, ApplicationUserType.INTERNAL.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    //List<Guid> bankBranches = ServiceHelper.GetAllSubBankUnitsAlogWithParent(ValidationHelper.GetString(user.UserInformation.UserSettings.GetValue("Eurobank_UserOrganisation"), ""));
                    if (user.InternalUser != null && user.InternalUser.AssignBankBranches != null && user.InternalUser.AssignBankBranches.Count > 0)
                    {
                        applications = applicationsRepository.GetApplicationDetailsForInternalUserNormalByApplicationDetailsID(user.InternalUser.AssignBankBranches, ApplicationDetailsID);
                    }
                }

                foreach (var application in applications)
                {
                    retVal.Add(BindApplicationDetailsModelView(user.UserType, user.UserRole, application));
                }
            }
            return retVal;
        }

        private static DocumentQuery<ApplicationDetails> GetApplicationDetailsForIntroducerNormal(int userId)
        {
            return ApplicationDetailsProvider.GetApplicationDetails().WhereEquals("ApplicationDetails_DocumentSubmittedByUserID", userId);
        }

        private static DocumentQuery<ApplicationDetails> GetApplicationDetailsForIntroducerPower(string organization)
        {
            return ApplicationDetailsProvider.GetApplicationDetails().WhereEquals("ApplicationDetails_UserOrganisation", organization);
        }

        private static DocumentQuery<ApplicationDetails> GetApplicationDetailsForInternalUserNormal(List<Guid> bankBranches)
        {
            return ApplicationDetailsProvider.GetApplicationDetails().WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches);
        }

        private static DocumentQuery<ApplicationDetails> GetApplicationDetailsForInternalUserPower(List<Guid> bankBranches)
        {
            return ApplicationDetailsProvider.GetApplicationDetails().WhereIn("ApplicationDetails_ResponsibleBankingCenter", bankBranches);
        }

		private static ApplicationDetailsModelView BindApplicationDetailsModelView(string userType, string userRole, ApplicationDetails item)
        {
            ApplicationDetailsModelView retVal = null;

            if (item != null)
            {
				string applicationStatus = ValidationHelper.GetString(item.ApplicationDetails_ApplicationStatus, "");
				string responsibleBankingCenter = ValidationHelper.GetString(item.GetValue("ApplicationDetails_ResponsibleBankingCenter"), "");

				retVal = new ApplicationDetailsModelView()
				{
					Application_NodeGUID = item.NodeGUID.ToString(),
					ApplicationDetails_ApplicationNumber = ValidationHelper.GetString(item.ApplicationDetails_ApplicationNumber, ""),
					ApplicationDetails_ApplicationStatus = ServiceHelper.GetName(applicationStatus, "/Lookups/General/APPLICATION-SERVICES"),
					ApplicationDetails_ApplicationStatusName = ServiceHelper.GetName(applicationStatus, "/Lookups/General/APPLICATION-WORKFLOW-STATUS"),
					ApplicationDetails_PreviousStage = ServiceHelper.GetName(ValidationHelper.GetString(item.ApplicationDetails_ApplicationLastStatus, ""), "/Lookups/General/APPLICATION-WORKFLOW-STATUS"),
					ApplicationDetails_ApplicationType = item.ApplicationDetails_ApplicationType,
					ApplicationDetails_ApplicationTypeName = ServiceHelper.GetName(ValidationHelper.GetString(item.ApplicationDetails_ApplicationType, ""), "/Lookups/General/APPLICATION-TYPE"),
					ApplicationDetails_ApplicatonServices = ValidationHelper.GetString(item.ApplicationDetails_ApplicatonServices, ""),

					ApplicationDetails_CurrentStage = ValidationHelper.GetString(item.ApplicationDetails_CurrentStage, ""),
					ApplicationDetails_IntroducerCIF = ValidationHelper.GetString(item.ApplicationDetails_IntroducerCIF, ""),
					ApplicationDetails_IntroducerName = ValidationHelper.GetString(item.ApplicationDetails_IntroducerName, ""),

					ApplicationDetails_ResponsibleBankingCenter = ServiceHelper.GetName(responsibleBankingCenter, "/Bank-Units"),

                    ApplicationDetails_ResponsibleOfficer = ServiceHelper.GetUserName(ValidationHelper.GetString(item.ApplicationDetails_ResponsibleOfficer, "")),

                    ApplicationDetailsID = ValidationHelper.GetInteger(item.ApplicationDetailsID, 0),
					ApplicationDetails_CreatedOn = ValidationHelper.GetString(Convert.ToDateTime(item.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss"), ""),
					ApplicationDetails_SubmittedOn = ValidationHelper.GetString(Convert.ToDateTime(item.ApplicationDetails_SubmittedOn).ToString("dd/MM/yyyy HH:mm:ss"), ""),
					ApplicationDetails_SubmittedBy = ValidationHelper.GetString(item.ApplicationDetails_SubmittedBy, ""),

				};

				retVal.FullNameOfApplicant = GetApplicantDetails(retVal.ApplicationDetails_ApplicationTypeName, retVal.ApplicationDetails_ApplicationNumber);

				if (!string.IsNullOrEmpty(responsibleBankingCenter))
				{
					BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(responsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName);
					if (bankUnit != null)
					{
						retVal.ApplicationDetails_ResponsibleBankingCenterUnit = bankUnit.BankUnitCode;
					}
				}

				if (!string.IsNullOrEmpty(retVal.ApplicationDetails_IntroducerName) && retVal.ApplicationDetails_IntroducerName.IndexOf('-') > -1)
				{
					retVal.ApplicationDetails_Introducer = retVal.ApplicationDetails_IntroducerName.Substring(retVal.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
				}



                //Assign Read and Edit Permission Along with Application Status Checking
                ApplicationWorkflowStatus currentStatus = ApplicationWorkflowStatus.NONE;
                currentStatus = ApplicationWorkFlowProcess.GetMatchedApplicationWorkflowStatusByStatusName(retVal.ApplicationDetails_ApplicationStatusName);

                ApplicationWorkflowStatus previousStatus = ApplicationWorkflowStatus.NONE;
                previousStatus = ApplicationWorkFlowProcess.GetMatchedApplicationWorkflowStatusByStatusName(retVal.ApplicationDetails_PreviousStage);
                

                //Logic for the stage Access
                retVal.IsEdit = false;
                retVal.IsView = false;
                retVal.IsBankDocAttachmentAllowed = false;
                retVal.IsExpectedDocAttachmentAllowed = false;
                retVal.IsDecissionCommentsAllowed = false;
                retVal.IsVisibleSaveAsDraftButton = false;
                retVal.IsVisibleSubmitButton = false;
                if (currentStatus != ApplicationWorkflowStatus.NONE)
                {
                    Enum.TryParse(userType, out ApplicationUserType userTypeEnum);
                    switch (userTypeEnum)
                    {
                        case ApplicationUserType.INTRODUCER:
                            switch (currentStatus)
                            {
                                case ApplicationWorkflowStatus.DRAFT:
                                    retVal.IsView = true;
                                    retVal.IsEdit = true;
                                    retVal.IsExpectedDocAttachmentAllowed = true;
                                    retVal.IsDecissionCommentsAllowed = true;
                                    retVal.IsVisibleSubmitButton = true;
                                    retVal.IsVisibleSaveAsDraftButton = true;
                                    break;
                                case ApplicationWorkflowStatus.PENDING_INITIATOR:
                                    if (previousStatus == ApplicationWorkflowStatus.PENDING_CHECKER || previousStatus == ApplicationWorkflowStatus.PENDING_VERIFICATION)
                                    {
                                        retVal.IsEdit = true;
                                        retVal.IsVisibleSaveAsDraftButton = true;
                                    }
                                    retVal.IsExpectedDocAttachmentAllowed = true;
                                    retVal.IsDecissionCommentsAllowed = true;
                                    retVal.IsVisibleSubmitButton = true;
                                    break;
                                case ApplicationWorkflowStatus.PENDING_CHECKER:
                                    retVal.IsView = true;
                                    retVal.IsExpectedDocAttachmentAllowed = true;
                                    retVal.IsVisibleSaveAsDraftButton = false;
                                    if (string.Equals(userRole, ApplicationUserRole.POWER.ToString(), StringComparison.OrdinalIgnoreCase))
                                    {
                                        retVal.IsEdit = true;
                                        retVal.IsDecissionCommentsAllowed = true;
                                        retVal.IsVisibleSubmitButton = true;
                                    }
                                    break;
                                case ApplicationWorkflowStatus.PENDING_SIGNATURES:
                                    retVal.IsView = true;
                                    retVal.IsExpectedDocAttachmentAllowed = true;
                                    retVal.IsDecissionCommentsAllowed = true;
                                    retVal.IsVisibleSubmitButton = true;
                                    break;
                                case ApplicationWorkflowStatus.PENDING_OMMISSIONS:
                                    retVal.IsView = true;
                                    retVal.IsExpectedDocAttachmentAllowed = true;
                                    retVal.IsDecissionCommentsAllowed = true;
                                    retVal.IsVisibleSubmitButton = true;
                                    break;
                                case ApplicationWorkflowStatus.PENDING_VERIFICATION:
                                case ApplicationWorkflowStatus.PENDING_EXECUTION:
                                case ApplicationWorkflowStatus.PENDING_BANK_DOCUMENTS:
                                case ApplicationWorkflowStatus.PENDING_COMPLETION:
                                    retVal.IsView = true;
                                    break;
                            }
                            break;
                        case ApplicationUserType.INTERNAL:
                            switch (currentStatus)
                            {
                                case ApplicationWorkflowStatus.DRAFT:
                                case ApplicationWorkflowStatus.PENDING_INITIATOR:
                                case ApplicationWorkflowStatus.PENDING_CHECKER:
                                case ApplicationWorkflowStatus.PENDING_SIGNATURES:
                                case ApplicationWorkflowStatus.PENDING_OMMISSIONS:
                                    retVal.IsView = true;
                                    break;
                                case ApplicationWorkflowStatus.PENDING_VERIFICATION:
                                case ApplicationWorkflowStatus.PENDING_EXECUTION:
                                case ApplicationWorkflowStatus.PENDING_BANK_DOCUMENTS:
                                    retVal.IsView = true;
                                    retVal.IsBankDocAttachmentAllowed = true;
                                    retVal.IsDecissionCommentsAllowed = true;
                                    retVal.IsVisibleSubmitButton = true;
                                    break;
                                case ApplicationWorkflowStatus.PENDING_COMPLETION:
                                    retVal.IsView = true;
                                    retVal.IsBankDocAttachmentAllowed = true;
                                    retVal.IsDecissionCommentsAllowed = true;
                                    retVal.IsVisibleSubmitButton = true;
                                    break;
                            }
                            break;
                    }
                }
            }

            return retVal;
        }

        private static string GetApplicantDetails(string applicationTypeName, string applicationNumber)
        {
            IEnumerable<ApplicantModel> applicantDetails;

            if (applicationTypeName == "LEGAL ENTITY")
            {
                applicantDetails = ApplicantProcess.GetLegalApplicantModels(applicationNumber);
            }
            else
            {
                // If GetApplicantModels returns a generic IEnumerable, we need to ensure it's ordered.
                applicantDetails = ApplicantProcess.GetApplicantModels(applicationNumber)?.OrderBy(m => m.CreatedDateTime);
            }

            if (applicantDetails == null)
            {
                return string.Empty;
            }

            // Use StringBuilder for efficient string concatenation
            StringBuilder fullNameBuilder = new StringBuilder();

            foreach (var applicantData in applicantDetails)
            {
                // Construct applicant detail strings
                string applicantInfo = string.IsNullOrEmpty(applicantData.FirstIdentificationNumber)
                    ? $"{applicantData.FullName}</br><span class=HeaderTextColor>{applicantData.FirstIdentificationNumber}</span>"
                    : $"{applicantData.FullName}</br><span class=HeaderTextColor>{applicantData.FirstIdentificationNumber}</span></br>";

                fullNameBuilder.Append(applicantInfo);
            }

            // Return the concatenated applicant details
            return fullNameBuilder.ToString().TrimEnd();
        }


		private static string GetApplicantDetailsNew(string applicationTypeName, string applicationNumber)
		{
			List<ApplicationNameModel> applicantDetails = new List<ApplicationNameModel>();

			TreeNode applicationDetailsNode = GetRootApplicationTreeNode(applicationNumber);

			if (applicationDetailsNode != null)
			{
				TreeNode companyDetailsRoot = null;
				if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, CompanyDetailsProcess._ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
				{
					companyDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, CompanyDetailsProcess._ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));

					if (companyDetailsRoot != null)
					{
                        if (applicationTypeName == "LEGAL ENTITY")
                        {
                            List<TreeNode> companyDetailsNodes = companyDetailsRoot.Children.Where(u => u.ClassName == CompanyDetails.CLASS_NAME).ToList();

                            if (companyDetailsNodes != null && companyDetailsNodes.Count > 0)
                            {
                                companyDetailsNodes.ForEach(t =>
                                {
                                    CompanyDetails companyDetails = CompanyDetailsProvider.GetCompanyDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (companyDetails != null)
                                    {
                                        applicantDetails.Add(new ApplicationNameModel
                                        {
                                            FirstIdentificationNumber = companyDetails.CompanyDetails_RegistrationNumber,
                                            FullName = companyDetails.CompanyDetails_RegisteredName
                                        });
                                    }
                                });
                            }
                        }
                        else
                        {
							List<TreeNode> personalDetailsNodes = companyDetailsRoot.Children.Where(u => u.ClassName == PersonalDetails.CLASS_NAME).ToList();

                            if (personalDetailsNodes != null && personalDetailsNodes.Count > 0)
                            {
                                personalDetailsNodes.ForEach(t =>
                                {
                                    PersonalDetails personalDetails = PersonalDetailsProvider.GetPersonalDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (personalDetails != null)
                                    {
                                        List<IdentificationDetailsViewModel> identificationDetails = IdentificationDetailsProcess.GetIdentificationDetails(personalDetails.PersonalDetailsID);
                                        applicantDetails.Add(new ApplicationNameModel
                                        {
                                            FirstIdentificationNumber = (identificationDetails != null && identificationDetails.Count > 0) ? identificationDetails.OrderBy(y => y.IdentificationDetailsID).FirstOrDefault().IdentificationDetails_IdentificationNumber : string.Empty,
                                            FullName = personalDetails.PersonalDetails_FirstName + " " + personalDetails.PersonalDetails_LastName
                                        });
                                    }
                                });
                            }
						}
					}
				}
			}

			if (applicantDetails == null)
			{
				return string.Empty;
			}

			// Use StringBuilder for efficient string concatenation
			StringBuilder fullNameBuilder = new StringBuilder();

			foreach (var applicantData in applicantDetails)
			{
				// Construct applicant detail strings
				string applicantInfo = string.IsNullOrEmpty(applicantData.FirstIdentificationNumber)
					? $"{applicantData.FullName}</br><span class=HeaderTextColor>{applicantData.FirstIdentificationNumber}</span>"
					: $"{applicantData.FullName}</br><span class=HeaderTextColor>{applicantData.FirstIdentificationNumber}</span></br>";

				fullNameBuilder.Append(applicantInfo);
			}

			// Return the concatenated applicant details
			return fullNameBuilder.ToString().TrimEnd();
		}
		#endregion

	}
}

