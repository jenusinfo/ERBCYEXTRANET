using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
//using Eurobank.Models.Account;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Applications;
using Eurobank.Models.User;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class ApplicationWorkFlowProcess
    {
        private static readonly List<ApplicationWorkflowStatus> _allApplicationWorkFlowStatus = Enum.GetValues(typeof(ApplicationWorkflowStatus)).Cast<ApplicationWorkflowStatus>().ToList();
        private static readonly List<ApplicationWorkflowDecisionType> _allApplicationDecisions = Enum.GetValues(typeof(ApplicationWorkflowDecisionType)).Cast<ApplicationWorkflowDecisionType>().ToList();





        public static ApplicationWorkFlowResult ExecuteWorkflow(UserModel user, int applicationId, string decisionGuid, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);

            if (user != null && user.UserInformation != null && !string.IsNullOrEmpty(user.UserType) && !string.IsNullOrEmpty(user.UserRole) && applicationDetails != null && !string.IsNullOrEmpty(decisionGuid))
            {
                ApplicationWorkflowDecisionType applicationWorkflowDecision = GetMatchedApplicationWorkflowDecision(decisionGuid);
                ApplicationWorkflowStatus currentApplicationStatus = GetMatchedApplicationWorkflowStatusByGuid(applicationDetails.ApplicationDetails_ApplicationStatus);

                if (applicationWorkflowDecision != ApplicationWorkflowDecisionType.NONE)
                {
                    if (string.Equals(user.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.Equals(user.UserRole, ApplicationUserRole.POWER.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            switch (applicationWorkflowDecision)
                            {
                                case ApplicationWorkflowDecisionType.RETURN:
                                    {
                                        if (currentApplicationStatus != ApplicationWorkflowStatus.PENDING_SIGNATURES && currentApplicationStatus != ApplicationWorkflowStatus.PENDING_OMMISSIONS)
                                        {
                                            retVal = IntroducerPowerCheckerApplicationReturn(user, applicationId, comments);
                                        }

                                        break;
                                    }
                                case ApplicationWorkflowDecisionType.CHECKED:
                                    {
                                        retVal = IntroducerPowerCheckerApplicationChecked(user, applicationId, comments);
                                        break;
                                    }
                                case ApplicationWorkflowDecisionType.WITHDRAW:
                                    {
                                        retVal = IntroducerPowerCheckerApplicationWithdraw(user, applicationId, comments);
                                        break;
                                    }

                                default: break;
                            }

                        }
                        switch (applicationWorkflowDecision)
                        {
                            case ApplicationWorkflowDecisionType.SUBMIT:
                                {
                                    if (user.IntroducerUser != null && user.IntroducerUser.Introducer != null)
                                    {
                                        retVal = IntroducerNormalDraftApplicationSubmit(user, applicationId, user.IntroducerUser.Introducer.MakerChecker, comments);
                                    }
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.RESUBMIT:
                                {
                                    ApplicationWorkflowStatus lastApplicationStatus = GetMatchedApplicationWorkflowStatusByGuid(applicationDetails.ApplicationDetails_ApplicationLastStatus);
                                    if (lastApplicationStatus == ApplicationWorkflowStatus.PENDING_CHECKER)
                                    {
                                        retVal = IntroducerNormalApplicationResubmitPendingChecker(user, applicationId, comments);
                                    }
                                    else if (lastApplicationStatus == ApplicationWorkflowStatus.PENDING_EXECUTION)
                                    {
                                        retVal = IntroducerNormalApplicationResubmitPendingExecution(user, applicationId, comments);
                                    }
                                    else if (lastApplicationStatus == ApplicationWorkflowStatus.PENDING_BANK_DOCUMENTS)
                                    {
                                        retVal = IntroducerNormalApplicationResubmitPendingBankDocuments(user, applicationId, comments);
                                    }
                                    else
                                    {
                                        retVal = IntroducerNormalApplicationResubmit(applicationId, comments);
                                    }

                                    break;
                                }
                            case ApplicationWorkflowDecisionType.WITHDRAW:
                                {
                                    retVal = IntroducerNormalDraftApplicationWithdraw(applicationId);
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.SIGNATURES_COMPLETED:
                                {
                                    retVal = IntroducerDocumentSignatureApplicationComplete(applicationId, comments);
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.OMMISSIONS_COMPLETED:
                                {
                                    retVal = IntroducerDocumentOmmisionApplicationOmmisionComplete(applicationId, comments);
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.RETURN:
                                {
                                    if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_SIGNATURES)
                                    {
                                        retVal = IntroducerDocumentSignatureApplicationReturn(applicationId, comments);
                                    }
                                    if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_OMMISSIONS)
                                    {
                                        retVal = IntroducerDocumentOmmisionApplicationReturn(applicationId, comments);
                                    }
                                    break;
                                }

                            default: break;
                        }
                    }
                    else if (string.Equals(user.UserType, ApplicationUserType.INTERNAL.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        switch (applicationWorkflowDecision)
                        {
                            case ApplicationWorkflowDecisionType.VERIFIED:
                                {
                                    retVal = BankUserReviewerApplicationVerified(user, applicationId, comments);
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.EXECUTED:
                                {
                                    retVal = BankUserExecutorApplicationExecuted(user, applicationId, comments);
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.ESCALATE:
                                {
                                    retVal = BankUserExecutorApplicationEscalate(applicationId);
                                    
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.SEND_FOR_SIGNATURES:
                                {
                                    retVal = BankUserDocumentsApplicationSendForSignature(user, applicationId, comments);
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.DOCUMENTS_PENDING:
                                {
                                    retVal = BankUserDocumentCompletionApplicationDocumentPending(user, applicationId, comments);
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.DOCUMENTS_RECEIVED:
                                {
                                    retVal = BankUserDocumentCompletionApplicationDocumentReceived(applicationId, comments);
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.COMPLETE:
                                {
                                    retVal = BankUserApplicationComplete(applicationId, comments);
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.RETURN_TO_INITIATOR:
                                {
                                    if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_BANK_DOCUMENTS)
                                    {
                                        retVal = BankUserDocumentsApplicationReturnToInitiator(user, applicationId, comments);
                                    }
                                    else if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_EXECUTION)
                                    {
                                        retVal = BankUserExecutorApplicationReturnToInitiator(user, applicationId, comments);
                                    }
                                    else if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_VERIFICATION)
                                    {
                                        retVal = BankUserReviewerApplicationReturn(user, applicationId, comments);
                                    }

                                    break;
                                }
                            case ApplicationWorkflowDecisionType.CANCEL:
                                {
                                    if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_COMPLETION)
                                    {
                                        retVal = BankUserDocumentCompletionApplicationCancel(applicationId, comments);
                                    }
                                    else if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_BANK_DOCUMENTS)
                                    {
                                        retVal = BankUserDocumentsApplicationCancel(applicationId, comments);
                                    }
                                    else if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_EXECUTION)
                                    {
                                        retVal = BankUserExecutorApplicationCancel(user, applicationId, comments);
                                    }
                                    else if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_VERIFICATION)
                                    {
                                        retVal = BankUserReviewerApplicationCancel(user, applicationId, comments);
                                    }

                                    break;
                                }
                            case ApplicationWorkflowDecisionType.RETURN:
                                {
                                    if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_EXECUTION)
                                    {
                                        retVal = BankUserExecutorApplicationReturn(user, applicationId, comments);
                                    }
                                    else if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_BANK_DOCUMENTS)
                                    {
                                        retVal = BankUserDocumentsApplicationReturn(applicationId);
                                    }
                                    else if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_COMPLETION)
                                    {
                                        retVal = BankUserDocumentCompletionApplicationReturn(user, applicationId, comments);
                                    }

                                    break;
                                }
                            case ApplicationWorkflowDecisionType.RETURN_TO_VERIFICATION:
                                {
                                    if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_EXECUTION)
                                    {
                                        retVal = BankUserExecutorApplicationReturn(user, applicationId, comments);
                                    }
                                    break;
                                }


                            default: break;
                        }
                    }

                }
            }

            return retVal;
        }

        public static string GetNextStage(UserModel user, int applicationId, string decisionGuid)
        {
            string retVal = string.Empty;

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);

            if (user != null && user.UserInformation != null && !string.IsNullOrEmpty(user.UserType) && !string.IsNullOrEmpty(user.UserRole) && applicationDetails != null && !string.IsNullOrEmpty(decisionGuid))
            {
                ApplicationWorkflowDecisionType applicationWorkflowDecision = GetMatchedApplicationWorkflowDecision(decisionGuid);
                ApplicationWorkflowStatus currentApplicationStatus = GetMatchedApplicationWorkflowStatusByGuid(applicationDetails.ApplicationDetails_ApplicationStatus);

                if (applicationWorkflowDecision != ApplicationWorkflowDecisionType.NONE)
                {
                    if (string.Equals(user.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.Equals(user.UserRole, ApplicationUserRole.POWER.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            switch (applicationWorkflowDecision)
                            {
                                case ApplicationWorkflowDecisionType.RETURN:
                                    {
                                        if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_SIGNATURES)
                                        {
                                            retVal = ApplicationWorkflowStatus.PENDING_BANK_DOCUMENTS.ToString();
                                        }
                                        else
                                        {
                                            retVal = ApplicationWorkflowStatus.PENDING_INITIATOR.ToString();
                                        }

                                        break;
                                    }
                                case ApplicationWorkflowDecisionType.CHECKED:
                                    {
                                        retVal = ApplicationWorkflowStatus.PENDING_VERIFICATION.ToString();
                                        break;
                                    }
                                case ApplicationWorkflowDecisionType.WITHDRAW:
                                    {
                                        retVal = ApplicationWorkflowStatus.WITHDRAWN.ToString();
                                        break;
                                    }

                                default: break;
                            }

                        }
                        switch (applicationWorkflowDecision)
                        {
                            case ApplicationWorkflowDecisionType.SUBMIT:
                                {
                                    if (user.IntroducerUser != null && user.IntroducerUser.Introducer != null)
                                    {
                                        if (user.IntroducerUser.Introducer.MakerChecker)
                                        {
                                            retVal = ApplicationWorkflowStatus.PENDING_CHECKER.ToString();
                                        }
                                        else
                                        {
                                            retVal = ApplicationWorkflowStatus.PENDING_VERIFICATION.ToString();
                                        }
                                    }
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.RESUBMIT:
                                {
                                    string lastStatus = applicationDetails.ApplicationDetails_ApplicationLastStatus;
                                    ApplicationWorkflowStatus lastStat = GetMatchedApplicationWorkflowStatusByGuid(lastStatus);
                                    retVal = lastStat.ToString();
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.WITHDRAW:
                                {
                                    retVal = ApplicationWorkflowStatus.WITHDRAWN.ToString();
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.SIGNATURES_COMPLETED:
                                {
                                    retVal = ApplicationWorkflowStatus.PENDING_COMPLETION.ToString();
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.OMMISSIONS_COMPLETED:
                                {
                                    retVal = ApplicationWorkflowStatus.PENDING_COMPLETION.ToString();
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.RETURN:
                                {
                                    if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_SIGNATURES)
                                    {
                                        retVal = ApplicationWorkflowStatus.PENDING_BANK_DOCUMENTS.ToString();
                                    }
                                    if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_OMMISSIONS)
                                    {
                                        retVal = ApplicationWorkflowStatus.PENDING_COMPLETION.ToString();
                                    }
                                    break;
                                }

                            default: break;
                        }
                    }
                    else if (string.Equals(user.UserType, ApplicationUserType.INTERNAL.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        switch (applicationWorkflowDecision)
                        {
                            case ApplicationWorkflowDecisionType.VERIFIED:
                                {
                                    retVal = ApplicationWorkflowStatus.PENDING_EXECUTION.ToString();
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.EXECUTED:
                                {
                                    retVal = ApplicationWorkflowStatus.PENDING_BANK_DOCUMENTS.ToString();
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.ESCALATE:
                                {
                                    retVal = ApplicationWorkflowStatus.PENDING_EXECUTION.ToString();
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.SEND_FOR_SIGNATURES:
                                {
                                    retVal = ApplicationWorkflowStatus.PENDING_SIGNATURES.ToString();
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.DOCUMENTS_PENDING:
                                {
                                    retVal = ApplicationWorkflowStatus.PENDING_OMMISSIONS.ToString();
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.DOCUMENTS_RECEIVED:
                                {
                                    retVal = ApplicationWorkflowStatus.COMPLETED.ToString();
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.COMPLETE:
                                {
                                    retVal = ApplicationWorkflowStatus.COMPLETED.ToString();
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.RETURN_TO_INITIATOR:
                                {
                                    retVal = ApplicationWorkflowStatus.PENDING_INITIATOR.ToString();
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.CANCEL:
                                {
                                    retVal = ApplicationWorkflowStatus.CANCELLED.ToString();
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.RETURN_TO_VERIFICATION:
                                {
                                    retVal = ApplicationWorkflowStatus.PENDING_VERIFICATION.ToString();
                                    break;
                                }
                            case ApplicationWorkflowDecisionType.RETURN:
                                {
                                    if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_EXECUTION)
                                    {
                                        retVal = ApplicationWorkflowStatus.PENDING_VERIFICATION.ToString();
                                    }
                                    else if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_BANK_DOCUMENTS)
                                    {
                                        retVal = ApplicationWorkflowStatus.PENDING_EXECUTION.ToString();
                                    }
                                    else if (currentApplicationStatus == ApplicationWorkflowStatus.PENDING_COMPLETION)
                                    {
                                        retVal = ApplicationWorkflowStatus.PENDING_SIGNATURES.ToString();
                                    }

                                    break;
                                }
                            default: break;
                        }
                    }

                }
            }
            if (!string.IsNullOrEmpty(retVal))
            {
                retVal = retVal.Replace("_", " ");
            }

            return retVal;
        }

        public static List<SelectListItem> GetApplicationWorkflowDecisions(UserModel user, int applicationId)
        {
            List<SelectListItem> retVal = null;


            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                //ApplicationWorkflowStatus lastStatus = GetMatchedApplicationWorkflowStatus(applicationDetails.ApplicationDetails_ApplicationLastStatus);

                ApplicationWorkflowStatus currentStatus = GetMatchedApplicationWorkflowStatusByGuid(applicationDetails.ApplicationDetails_ApplicationStatus);
                List<ApplicationWorkflowDecisionType> applicationWorkflowDecisions = new List<ApplicationWorkflowDecisionType>();

                if (string.Equals(user.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(user.UserRole, ApplicationUserRole.POWER.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        switch (currentStatus)
                        {
                            case ApplicationWorkflowStatus.PENDING_CHECKER:
                                {
                                    applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.CHECKED);
                                    applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.RETURN);
                                    applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.WITHDRAW);
                                    break;
                                }

                            default: break;
                        }

                    }
                    switch (currentStatus)
                    {
                        case ApplicationWorkflowStatus.DRAFT:
                            {
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.SUBMIT);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.WITHDRAW);
                                break;
                            }
                        case ApplicationWorkflowStatus.PENDING_INITIATOR:
                            {
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.RESUBMIT);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.WITHDRAW);
                                break;
                            }
                        case ApplicationWorkflowStatus.PENDING_SIGNATURES:
                            {
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.SIGNATURES_COMPLETED);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.RETURN);
                                break;
                            }
                        case ApplicationWorkflowStatus.PENDING_OMMISSIONS:
                            {
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.OMMISSIONS_COMPLETED);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.RETURN);
                                break;
                            }

                        default: break;
                    }
                }
                else if (string.Equals(user.UserType, ApplicationUserType.INTERNAL.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    switch (currentStatus)
                    {
                        case ApplicationWorkflowStatus.PENDING_VERIFICATION:
                            {
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.RETURN_TO_INITIATOR);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.VERIFIED);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.CANCEL);
                                break;
                            }
                        case ApplicationWorkflowStatus.PENDING_EXECUTION:
                            {
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.EXECUTED);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.RETURN_TO_VERIFICATION);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.RETURN_TO_INITIATOR);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.CANCEL);
                                //applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.ESCALATE);
                                break;
                            }
                        case ApplicationWorkflowStatus.PENDING_BANK_DOCUMENTS:
                            {
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.SEND_FOR_SIGNATURES);
                                //applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.RETURN);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.RETURN_TO_INITIATOR);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.CANCEL);
                                break;
                            }
                        case ApplicationWorkflowStatus.PENDING_COMPLETION:
                            {
                                //applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.COMPLETE);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.DOCUMENTS_RECEIVED);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.RETURN);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.DOCUMENTS_PENDING);
                                applicationWorkflowDecisions.Add(ApplicationWorkflowDecisionType.CANCEL);
                                break;
                            }

                        default: break;
                    }
                }

                if (applicationWorkflowDecisions != null && applicationWorkflowDecisions.Count > 0)
                {
                    retVal = GetApplicationWorkflowDecisionsListItems(applicationWorkflowDecisions);
                }
            }

            return retVal;
        }


        #region Introducer Normal User Create Application

        public static ApplicationWorkFlowResult IntroducerNormalDraftApplicationSubmit(UserModel userModel, int applicationId, bool checkerFlag, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);

            if (applicationDetails != null)
            {
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }

                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                if (checkerFlag)
                {
                    applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_CHECKER);
                    //Mail Send
                    if (userModel != null && userModel.UserInformation != null && userModel.UserInformation.UserSettings != null)
                    {
                        string introducerOrganization = ValidationHelper.GetString(userModel.UserInformation.UserSettings.GetValue("Eurobank_UserOrganisation"), "");
                        List<UserInfo> organizationpowerUsers = UserProcess.GetPowerUsersByIntroducerCompany(introducerOrganization);
                        if (organizationpowerUsers != null && organizationpowerUsers.Count > 0)
                        {
                            string powerUserEmailIds = string.Join(";", organizationpowerUsers.Where(p => !string.IsNullOrEmpty(p.Email)).Select(k => k.Email));
                            string ccedMailIds = string.Empty;
                            if (!string.IsNullOrEmpty(powerUserEmailIds))
                            {
                                if (applicationDetails.ApplicationDetails_DocumentSubmittedByUserID > 0)
                                {
                                    string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                                    UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                                    if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                                    {
                                        ccedMailIds = initiatorUserModel.UserInformation.Email;
                                    }
                                }
                            }
                            string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                            retVal.IsSuccess = WorkFlowMailProcess.SendMailSubmitPendingChecker(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, powerUserEmailIds, ccedMailIds, applicantNames, true, comments);
                        }
                    }

                }
                else
                {
                    applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_VERIFICATION);

                    BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    if (bankUnit != null)
                    {
                        string ccedMailIds = string.Empty;
                        if (applicationDetails.ApplicationDetails_DocumentSubmittedByUserID > 0)
                        {
                            string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                            UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                            if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                            {
                                ccedMailIds = initiatorUserModel.UserInformation.Email + ";";
                            }
                            if (!string.IsNullOrEmpty(userModel.UserInformation.Email) && !string.Equals(userModel.UserInformation.Email, initiatorUserModel.UserInformation.Email, StringComparison.OrdinalIgnoreCase))
                            {
                                ccedMailIds += userModel.UserInformation.Email;
                            }
                        }
                        string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                        retVal.IsSuccess = WorkFlowMailProcess.SendMailSubmitToPendingVerificationIntroducer(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, bankUnit.BankUnitEmail, ccedMailIds, true, comments);
                    }

                }
                applicationDetails.Update();
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult IntroducerNormalDraftApplicationWithdraw(int applicationId)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.WITHDRAWN);
                applicationDetails.Update();
                retVal.IsSuccess = true;
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult IntroducerNormalApplicationResubmit(int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                string lastStatus = applicationDetails.ApplicationDetails_ApplicationLastStatus;
                string currentStatus = applicationDetails.ApplicationDetails_ApplicationLastStatus;
                applicationDetails.ApplicationDetails_ApplicationLastStatus = currentStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = lastStatus;
                applicationDetails.Update();

                string LstatusName = ServiceHelper.GetApplicationStatuses().Where(x => x.Value == lastStatus).Select(x => x.Text).FirstOrDefault();
                string CstatusName = ServiceHelper.GetApplicationStatuses().Where(x => x.Value == currentStatus).Select(x => x.Text).FirstOrDefault();
                //Mail Send
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }

                BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                //Need to add Last Return By User
                string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                retVal.IsSuccess = WorkFlowMailProcess.SendMailReSubmitStepThatWasReturnedFromIntroducerInitiator(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, bankUnit.BankUnitEmail, true, comments);
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult IntroducerNormalApplicationResubmitPendingChecker(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                string lastStatus = applicationDetails.ApplicationDetails_ApplicationLastStatus;
                string currentStatus = applicationDetails.ApplicationDetails_ApplicationLastStatus;
                applicationDetails.ApplicationDetails_ApplicationLastStatus = currentStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = lastStatus;
                applicationDetails.Update();

                //Mail Send
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                string introducerOrganization = ValidationHelper.GetString(userModel.UserInformation.UserSettings.GetValue("Eurobank_UserOrganisation"), "");
                List<UserInfo> organizationpowerUsers = UserProcess.GetPowerUsersByIntroducerCompany(introducerOrganization);
                string powerUserEmailIds = string.Empty;
                if (organizationpowerUsers != null && organizationpowerUsers.Count > 0)
                {
                    powerUserEmailIds = string.Join(";", organizationpowerUsers.Where(p => !string.IsNullOrEmpty(p.Email)).Select(k => k.Email));
                }
                string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                retVal.IsSuccess = WorkFlowMailProcess.SendMailReSubmitPendigChecker(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, powerUserEmailIds, true, comments);
            }

            return retVal;
        }
        public static ApplicationWorkFlowResult IntroducerNormalApplicationResubmitPendingExecution(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                string lastStatus = applicationDetails.ApplicationDetails_ApplicationLastStatus;
                string currentStatus = applicationDetails.ApplicationDetails_ApplicationLastStatus;
                applicationDetails.ApplicationDetails_ApplicationLastStatus = currentStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = lastStatus;
                applicationDetails.Update();

                //Mail Send
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                retVal.IsSuccess = WorkFlowMailProcess.SendMailReSubmitPendigExecution(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, bankUnit.BankUnitEmail, true, comments);
            }

            return retVal;
        }
        public static ApplicationWorkFlowResult IntroducerNormalApplicationResubmitPendingBankDocuments(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                string lastStatus = applicationDetails.ApplicationDetails_ApplicationLastStatus;
                string currentStatus = applicationDetails.ApplicationDetails_ApplicationLastStatus;
                applicationDetails.ApplicationDetails_ApplicationLastStatus = currentStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = lastStatus;
                applicationDetails.Update();

                //Mail Send
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                retVal.IsSuccess = WorkFlowMailProcess.SendMailReSubmitPendigBankDocuments(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, bankUnit.BankUnitEmail, true, comments);
            }

            return retVal;
        }
        #endregion

        #region Introducer Power User Checker Application

        public static ApplicationWorkFlowResult IntroducerPowerCheckerApplicationChecked(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_VERIFICATION);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                if (bankUnit != null)
                {
                    string ccedMailIds = string.Empty;
                    if (applicationDetails.ApplicationDetails_DocumentSubmittedByUserID > 0)
                    {
                        string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                        UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                        if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                        {
                            ccedMailIds = initiatorUserModel.UserInformation.Email;// + ";";
                        }
                        //if (!string.IsNullOrEmpty(userModel.UserInformation.Email) && !string.Equals(userModel.UserInformation.Email, initiatorUserModel.UserInformation.Email, StringComparison.OrdinalIgnoreCase))
                        //{
                        //    ccedMailIds += userModel.UserInformation.Email;
                        //}
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailSubmitToPendingVerificationIntroducer(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, bankUnit.BankUnitEmail, ccedMailIds, true, comments);
                }
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult IntroducerPowerCheckerApplicationWithdraw(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.WITHDRAWN);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                {
                    string ccedMailIds = string.Empty;
                    //if (applicationDetails.ApplicationDetails_DocumentSubmittedByUserID > 0)
                    //{
                    //    if (!string.IsNullOrEmpty(userModel.UserInformation.Email))
                    //    {
                    //        ccedMailIds = userModel.UserInformation.Email;
                    //    }
                    //}
                    //no cc required
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailWithdrawEndWithdrawnIntroducerPendingChecker(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, initiatorUserModel.UserInformation.Email, ccedMailIds, applicantNames, true, comments);
                }

            }

            return retVal;
        }

        public static ApplicationWorkFlowResult IntroducerPowerCheckerApplicationReturn(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_INITIATOR);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                {
                    string ccedMailIds = string.Empty;
                    if (applicationDetails.ApplicationDetails_DocumentSubmittedByUserID > 0)
                    {
                        //Commented BY SB for Pending checker Return(Spreadsheet line no-11) 
                        //if (!string.IsNullOrEmpty(userModel.UserInformation.Email))
                        //{
                        //    ccedMailIds = userModel.UserInformation.Email;
                        //}
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailReturnInitiatorIntroducerPendingChecker(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, initiatorUserModel.UserInformation.Email, ccedMailIds, applicantNames, true, comments);
                }
            }

            return retVal;
        }

        #endregion

        #region Introducer User Initiator Application

        #endregion

        #region Bank User officer Reviwer Application

        public static ApplicationWorkFlowResult BankUserReviewerApplicationReturn(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_INITIATOR);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                {
                    string ccedMailIds = string.Empty;
                    if (applicationDetails.ApplicationDetails_DocumentSubmittedByUserID > 0)
                    {
                        BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                        if (!string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                        {
                            ccedMailIds = bankUnit.BankUnitEmail;
                        }
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailReturnInitiatorIntroducerPendingChecker(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, initiatorUserModel.UserInformation.Email, ccedMailIds, applicantNames, true, comments);
                }
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult BankUserReviewerApplicationCancel(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.CANCELLED);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                {
                    string ccedMailIds = string.Empty;
                    //if (applicationDetails.ApplicationDetails_DocumentSubmittedByUserID > 0)
                    //{
                    //    if (!string.IsNullOrEmpty(userModel.UserInformation.Email))
                    //    {
                    //        ccedMailIds = userModel.UserInformation.Email;
                    //    }
                    //}
                    BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    if (!string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                    {
                        ccedMailIds = bankUnit.BankUnitEmail;
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailCancelEndCancelledBankUser(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, initiatorUserModel.UserInformation.Email, ccedMailIds, applicantNames, true, comments);
                }
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult BankUserReviewerApplicationVerified(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_EXECUTION);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                {
                    string ccedMailIds = string.Empty;
                    BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                    if (bankUnit != null)
                    {
                        if (!string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                        {
                            ccedMailIds = bankUnit.BankUnitEmail;
                        }
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailVerifiedPendingExecutionBankUser(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, initiatorUserModel.UserInformation.Email, ccedMailIds, true, comments);
                }
            }

            return retVal;
        }

        #endregion

        #region Bank User officer Executor Application

        public static ApplicationWorkFlowResult BankUserExecutorApplicationReturn(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_VERIFICATION);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                if (bankUnit != null && !string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                {
                    string ccedMailIds = string.Empty;
                    if (!string.IsNullOrEmpty(applicationDetails.ApplicationDetails_SubmittedBy))
                    {
                        if (!string.IsNullOrEmpty(userModel.UserInformation.Email))
                        {
                            ccedMailIds = userModel.UserInformation.Email;
                        }
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailReturnPendingVerificationBankUser(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, bankUnit.BankUnitEmail, ccedMailIds, true, comments);
                }
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult BankUserExecutorApplicationCancel(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.CANCELLED);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                {
                    string ccedMailIds = string.Empty;
                    BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    if (!string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                    {
                        ccedMailIds = bankUnit.BankUnitEmail;
                    }
                    //if (applicationDetails.ApplicationDetails_DocumentSubmittedByUserID > 0)
                    //{
                    //    if (!string.IsNullOrEmpty(userModel.UserInformation.Email))
                    //    {
                    //        ccedMailIds = userModel.UserInformation.Email;
                    //    }
                    //}
                    string mailRecipients = initiatorUserModel.UserInformation.Email + ";" + ServiceHelper.GetIntroducerEmailByName(applicationDetailsModel.ApplicationDetails_IntroducerName);
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailCancelEndCancelledBankUser(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, mailRecipients, ccedMailIds, applicantNames, true, comments);
                }
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult BankUserExecutorApplicationReturnToInitiator(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_INITIATOR);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                {
                    string ccedMailIds = string.Empty;
                    if (applicationDetails.ApplicationDetails_DocumentSubmittedByUserID > 0)
                    {
                        if (!string.IsNullOrEmpty(userModel.UserInformation.Email))
                        {
                            ccedMailIds = userModel.UserInformation.Email;
                        }
                    }
                    BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    if (bankUnit != null)
                    {
                        if (!string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                        {
                            ccedMailIds += bankUnit.BankUnitEmail;
                        }
                    }
                    string mailRecipients = initiatorUserModel.UserInformation.Email + ";" + ServiceHelper.GetIntroducerEmailByName(applicationDetailsModel.ApplicationDetails_IntroducerName);
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailReturnInitiatorIntroducerPendingChecker(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, mailRecipients, ccedMailIds, applicantNames, true, comments);
                }
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult BankUserExecutorApplicationExecuted(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_BANK_DOCUMENTS);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                {
                    string ccedMailIds = string.Empty;
                    BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                    if (bankUnit != null)
                    {
                        if (!string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                        {
                            ccedMailIds = bankUnit.BankUnitEmail;
                        }
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailExecutedPendingDocumentsBankUser(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, initiatorUserModel.UserInformation.Email, ccedMailIds, true, comments);
                }
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult BankUserExecutorApplicationEscalate(int applicationId)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                //Need to confirm status after Escalate then remove this comment by kusal
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_EXECUTION);
                applicationDetails.Update();
                retVal.IsSuccess = true;
            }

            return retVal;
        }

        #endregion

        #region Bank User officer Documents Application

        public static ApplicationWorkFlowResult BankUserDocumentsApplicationReturn(int applicationId)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_EXECUTION);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                if (bankUnit != null && !string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                {
                    string ccedMailIds = string.Empty;
                    string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                    UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                    if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                    {
                        ccedMailIds = initiatorUserModel.UserInformation.Email;
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailReturnPendingExecutionBankUser(introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, bankUnit.BankUnitEmail, ccedMailIds, true);
                }
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult BankUserDocumentsApplicationCancel(int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.CANCELLED);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }

                string ccedMailIds = string.Empty;
                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                {
                    BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    if (bankUnit != null && !string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                    {
                        ccedMailIds = bankUnit.BankUnitEmail;
                    }
                    string mailRecipients = initiatorUserModel.UserInformation.Email + ";" + ServiceHelper.GetIntroducerEmailByName(applicationDetailsModel.ApplicationDetails_IntroducerName);
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailCancelEndCancelledBankUser(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, mailRecipients, ccedMailIds, applicantNames, true, comments);
                }

            }

            return retVal;
        }

        public static ApplicationWorkFlowResult BankUserDocumentsApplicationReturnToInitiator(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_INITIATOR);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                {
                    string ccedMailIds = string.Empty;
                    BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    if (bankUnit != null)
                    {
                        if (!string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                        {
                            ccedMailIds = bankUnit.BankUnitEmail;
                        }
                    }
                    string mailRecipients = initiatorUserModel.UserInformation.Email + ";" + ServiceHelper.GetIntroducerEmailByName(applicationDetailsModel.ApplicationDetails_IntroducerName);
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailReturnInitiatorIntroducerPendingChecker(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, mailRecipients, ccedMailIds, applicantNames, true, comments);
                }
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult BankUserDocumentsApplicationSendForSignature(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_SIGNATURES);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                {
                    string ccedMailIds = string.Empty;
                    //if (applicationDetails.ApplicationDetails_DocumentSubmittedByUserID > 0)
                    //{
                    //    if (!string.IsNullOrEmpty(userModel.UserInformation.Email))
                    //    {
                    //        ccedMailIds = userModel.UserInformation.Email;
                    //    }
                    //}
                    BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    if (bankUnit != null)
                    {
                        if (!string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                        {
                            ccedMailIds += bankUnit.BankUnitEmail;
                        }
                    }
                    //WorkFlowMailProcess.SendMailSendForSignaturePendingSignatureBankUser(applicationDetailsModel.ApplicationDetails_IntroducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, bankUnit.BankUnitEmail, ccedMailIds, applicantNames, true);
                    string mailRecipients = initiatorUserModel.UserInformation.Email + ";" + ServiceHelper.GetIntroducerEmailByName(applicationDetailsModel.ApplicationDetails_IntroducerName);
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailSendForSignaturePendingSignatureBankUser(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, mailRecipients, ccedMailIds, applicantNames, true, comments);
                }
                ///////
                //BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                //            if (bankUnit != null && !string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                //            {
                //                string ccedMailIds = string.Empty;
                //                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                //                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                //                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                //                {
                //                    ccedMailIds = initiatorUserModel.UserInformation.Email;
                //                }

                //                //WorkFlowMailProcess.SendMailSendForSignaturePendingSignatureBankUser(applicationDetailsModel.ApplicationDetails_IntroducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, bankUnit.BankUnitEmail, ccedMailIds, applicantNames, true);
                //                WorkFlowMailProcess.SendMailSendForSignaturePendingSignatureBankUser(applicationDetailsModel.ApplicationDetails_IntroducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, initiatorUserModel.UserInformation.Email, ccedMailIds, applicantNames, true);
                //            }
            }

            return retVal;
        }

        #endregion

        #region Initiator/Introducer User Document Signature Application

        public static ApplicationWorkFlowResult IntroducerDocumentSignatureApplicationReturn(int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_BANK_DOCUMENTS);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                if (bankUnit != null && !string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                {
                    string ccedMailIds = string.Empty;
                    string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                    UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                    if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                    {
                        ccedMailIds = initiatorUserModel.UserInformation.Email + ";" + ServiceHelper.GetIntroducerEmailByName(applicationDetailsModel.ApplicationDetails_IntroducerName);
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailReturnPendingDocuments(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, bankUnit.BankUnitEmail, ccedMailIds, true, comments);
                }
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult IntroducerDocumentSignatureApplicationComplete(int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_COMPLETION);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                if (bankUnit != null && !string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                {
                    string ccedMailIds = string.Empty;
                    string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                    UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                    if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                    {
                        ccedMailIds = initiatorUserModel.UserInformation.Email + ";" + ServiceHelper.GetIntroducerEmailByName(applicationDetailsModel.ApplicationDetails_IntroducerName);
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailSignatureCompletedPendingCompletion(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, bankUnit.BankUnitEmail, ccedMailIds, applicantNames, true, comments);
                }
            }

            return retVal;
        }

        #endregion

        #region Bank User officer Document Completion Application

        public static ApplicationWorkFlowResult BankUserDocumentCompletionApplicationReturn(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_SIGNATURES);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                {
                    string mailRecipients = initiatorUserModel.UserInformation.Email + ";" + ServiceHelper.GetIntroducerEmailByName(applicationDetailsModel.ApplicationDetails_IntroducerName);
                    string ccedMailIds = string.Empty;
                    BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    if (!string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                    {
                        ccedMailIds = bankUnit.BankUnitEmail;
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailReturnPendingSignature(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, mailRecipients, ccedMailIds, true, comments);
                }
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult BankUserDocumentCompletionApplicationCancel(int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.CANCELLED);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                string ccedMailIds = string.Empty;
                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                {
                    string mailRecipients = initiatorUserModel.UserInformation.Email + ";" + ServiceHelper.GetIntroducerEmailByName(applicationDetailsModel.ApplicationDetails_IntroducerName);
                    BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    if (!string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                    {
                        ccedMailIds = bankUnit.BankUnitEmail;
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailCancelEndCancelledBankUser(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, mailRecipients, ccedMailIds, applicantNames, true, comments);
                }
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult BankUserDocumentCompletionApplicationDocumentPending(UserModel userModel, int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_OMMISSIONS);
                applicationDetails.Update();
                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                {
                    string mailRecipients = initiatorUserModel.UserInformation.Email + ";" + ServiceHelper.GetIntroducerEmailByName(applicationDetailsModel.ApplicationDetails_IntroducerName);
                    string ccedMailIds = string.Empty;
                    BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    if (!string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                    {
                        ccedMailIds = bankUnit.BankUnitEmail;
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailDocumentsPendingOmission(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, mailRecipients, ccedMailIds, true, comments);
                }
            }

            return retVal;
        }

        public static ApplicationWorkFlowResult BankUserDocumentCompletionApplicationDocumentReceived(int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                //Need to confirm status after Escalate then remove this comment by kusal
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.COMPLETED);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                if (bankUnit != null && !string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                {
                    string mailRecipient = string.Empty;
                    string ccedMailIds = bankUnit.BankUnitEmail;
                    string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                    UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                    if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                    {
                        mailRecipient = initiatorUserModel.UserInformation.Email + ";" + ServiceHelper.GetIntroducerEmailByName(applicationDetailsModel.ApplicationDetails_IntroducerName);
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailDocumentReceivedEndCompleted(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, mailRecipient, ccedMailIds, applicantNames, true, comments);
                }
            }

            return retVal;
        }

        #endregion

        #region Initiator/Introducer User Document Ommision Application

        public static ApplicationWorkFlowResult IntroducerDocumentOmmisionApplicationReturn(int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_COMPLETION);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }

                BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                if (!string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                {
                    string ccedMailIds = string.Empty;
                    string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                    UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                    if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                    {
                        ccedMailIds = initiatorUserModel.UserInformation.Email + ";" + ServiceHelper.GetIntroducerEmailByName(applicationDetailsModel.ApplicationDetails_IntroducerName);
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailReturnPendingCompletion(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, bankUnit.BankUnitEmail, ccedMailIds, true, comments);
                }
            }

            return retVal;
        }

        //public static ApplicationWorkFlowResult BankUserDocumentOmmisionApplicationReturnToInitiator(int applicationId)
        //{
        //	ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
        //	{
        //		IsSuccess = false
        //	};

        //	ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


        //	if(applicationDetails != null)
        //	{
        //		applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
        //		applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_INITIATOR);
        //		applicationDetails.Update();
        //	}

        //	return retVal;
        //}

        public static ApplicationWorkFlowResult IntroducerDocumentOmmisionApplicationOmmisionComplete(int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.PENDING_COMPLETION);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                if (bankUnit != null && !string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                {
                    string ccedMailIds = string.Empty;
                    string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                    UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                    if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                    {
                        ccedMailIds = initiatorUserModel.UserInformation.Email + ";" + ServiceHelper.GetIntroducerEmailByName(applicationDetailsModel.ApplicationDetails_IntroducerName);
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailOmmissionCompletedPendingCompletion(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, bankUnit.BankUnitEmail, ccedMailIds, true, comments);
                }

            }

            return retVal;
        }

        #endregion
        #region Bank user pending Completion
        public static ApplicationWorkFlowResult BankUserApplicationComplete(int applicationId, string comments)
        {
            ApplicationWorkFlowResult retVal = new ApplicationWorkFlowResult()
            {
                IsSuccess = false
            };

            ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);


            if (applicationDetails != null)
            {
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                applicationDetails.ApplicationDetails_ApplicationStatus = GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus.COMPLETED);
                applicationDetails.Update();

                //Send Mail
                ApplicationDetailsModelView applicationDetailsModel = ApplicationsProcess.GetApplicationsDetails(applicationDetails);
                applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                string applicantNames = string.Empty;
                List<ApplicantModel> applicants = null;
                if (string.Equals(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
                {
                    applicants = ApplicantProcess.GetLegalApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                else
                {
                    applicants = ApplicantProcess.GetApplicantModels(applicationDetailsModel.ApplicationDetails_ApplicationNumber);
                }
                if (applicants != null && applicants.Count > 0)
                {
                    applicantNames = string.Join(",", applicants.Select(u => u.FullName));
                }
                BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(applicationDetails.ApplicationDetails_ResponsibleBankingCenter), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                if (bankUnit != null && !string.IsNullOrEmpty(bankUnit.BankUnitEmail))
                {
                    string ccedMailIds = string.Empty;
                    string submittedByUserName = UserInfoProvider.GetUserNameById(applicationDetails.ApplicationDetails_DocumentSubmittedByUserID);
                    UserModel initiatorUserModel = UserProcess.GetUser(submittedByUserName);
                    if (initiatorUserModel != null && initiatorUserModel.UserInformation != null && !string.IsNullOrEmpty(initiatorUserModel.UserInformation.Email))
                    {
                        ccedMailIds = initiatorUserModel.UserInformation.Email;
                    }
                    string introducerName = applicationDetailsModel.ApplicationDetails_IntroducerName.Substring(applicationDetailsModel.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                    retVal.IsSuccess = WorkFlowMailProcess.SendMailOmmissionCompletedPendingCompletion(applicationDetailsModel.ApplicationDetails_ApplicationTypeName, introducerName, applicationDetailsModel.ApplicationDetails_ApplicationNumber, applicantNames, bankUnit.BankUnitEmail, ccedMailIds, true, comments);
                }

            }

            return retVal;
        }
        #endregion
        #region private methods

        public static string GetMatchedApplicationWorkflowStatus(ApplicationWorkflowStatus workflowStatus)
        {
            string retVal = null;
            var applicationWorkflowStatuses = ServiceHelper.GetApplicationStatuses();
            string replacedStatus = workflowStatus.ToString().Replace("_", " ");

            if (applicationWorkflowStatuses.Any(o => string.Equals(o.Text, replacedStatus, StringComparison.OrdinalIgnoreCase)))
            {
                retVal = applicationWorkflowStatuses.FirstOrDefault(o => string.Equals(o.Text, replacedStatus, StringComparison.OrdinalIgnoreCase)).Value;
            }

            return retVal;
        }

        private static ApplicationWorkflowStatus GetMatchedApplicationWorkflowStatusByGuid(string workflowStatusGuid)
        {
            ApplicationWorkflowStatus retVal = ApplicationWorkflowStatus.NONE;

            if (!string.IsNullOrEmpty(workflowStatusGuid))
            {
                var applicationWorkflowStatuses = ServiceHelper.GetApplicationStatuses();
                if (applicationWorkflowStatuses.Any(o => string.Equals(o.Value, workflowStatusGuid, StringComparison.OrdinalIgnoreCase)))
                {
                    string statusText = applicationWorkflowStatuses.FirstOrDefault(o => string.Equals(o.Value, workflowStatusGuid, StringComparison.OrdinalIgnoreCase)).Text;
                    if (!string.IsNullOrEmpty(statusText))
                    {
                        retVal = _allApplicationWorkFlowStatus.FirstOrDefault(r => string.Equals(r.ToString().Replace("_", " "), statusText, StringComparison.OrdinalIgnoreCase));
                    }
                }

            }

            return retVal;
        }

        public static ApplicationWorkflowStatus GetMatchedApplicationWorkflowStatusByStatusName(string workflowStatusName)
        {
            ApplicationWorkflowStatus retVal = ApplicationWorkflowStatus.NONE;

            if (!string.IsNullOrEmpty(workflowStatusName))
            {
                retVal = _allApplicationWorkFlowStatus.FirstOrDefault(r => string.Equals(r.ToString().Replace("_", " "), workflowStatusName, StringComparison.OrdinalIgnoreCase));
            }

            return retVal;
        }

        public static ApplicationWorkflowDecisionType GetMatchedApplicationWorkflowDecision(string decisionGuid)
        {
            ApplicationWorkflowDecisionType retVal = ApplicationWorkflowDecisionType.NONE;

            if (!string.IsNullOrEmpty(decisionGuid))
            {
                var applicationDecisions = ServiceHelper.GetDecisions();
                if (applicationDecisions.Any(o => string.Equals(o.Value, decisionGuid, StringComparison.OrdinalIgnoreCase)))
                {
                    string statusText = applicationDecisions.FirstOrDefault(o => string.Equals(o.Value, decisionGuid, StringComparison.OrdinalIgnoreCase)).Text;
                    if (!string.IsNullOrEmpty(statusText))
                    {
                        retVal = _allApplicationDecisions.FirstOrDefault(r => string.Equals(r.ToString().Replace("_", " "), statusText, StringComparison.OrdinalIgnoreCase));
                    }
                }

            }

            return retVal;
        }

        public static string GetMatchedApplicationWorkflowDecisionGuid(string decisionName)
        {
            string retVal = string.Empty;

            if (!string.IsNullOrEmpty(decisionName))
            {
                var applicationDecisions = ServiceHelper.GetDecisions();
                if (applicationDecisions.Any(o => string.Equals(o.Text, decisionName, StringComparison.OrdinalIgnoreCase)))
                {
                    retVal = applicationDecisions.FirstOrDefault(o => string.Equals(o.Text, decisionName, StringComparison.OrdinalIgnoreCase)).Value;
                }

            }

            return retVal;
        }

        public static List<SelectListItem> GetApplicationWorkflowDecisionsListItems(List<ApplicationWorkflowDecisionType> decisionItems)
        {
            List<SelectListItem> retVal = null;

            var decisions = ServiceHelper.GetDecisions();
            if (decisionItems != null && decisionItems.Count > 0)
            {
                List<string> decisionList = decisionItems.Select(u => u.ToString().Replace("_", " ")).ToList();
                if (decisions != null && decisions.Count > 0 && decisions.Any(r => decisionList.Any(m => string.Equals(m, r.Text, StringComparison.OrdinalIgnoreCase))))
                {
                    retVal = decisions.Where(r => decisionList.Any(m => string.Equals(m, r.Text, StringComparison.OrdinalIgnoreCase))).Select(x => new SelectListItem { Value = x.Value, Text = x.Text }).ToList();
                }
            }


            return retVal;
        }




        #endregion

        #region For External API

        public static bool ChangeStatus(int applicationId, ApplicationWorkflowStatus applicationWorkflowStatus)
        {
            bool retVal = false;

            if (applicationId > 0 && applicationWorkflowStatus != ApplicationWorkflowStatus.NONE)
            {
                ApplicationDetails applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);

                if (applicationDetails != null && !string.IsNullOrEmpty(applicationDetails.ApplicationDetails_ApplicationNumber))
                {
                    string matchedStatus = GetMatchedApplicationWorkflowStatus(applicationWorkflowStatus);
                    if (!string.IsNullOrEmpty(matchedStatus))
                    {
                        applicationDetails.ApplicationDetails_ApplicationLastStatus = applicationDetails.ApplicationDetails_ApplicationStatus;
                        applicationDetails.ApplicationDetails_ApplicationStatus = matchedStatus;
                        applicationDetails.Update();
                    }
                }
            }

            return retVal;
        }

        #endregion
    }
}
