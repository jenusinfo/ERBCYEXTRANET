using CMS.DataEngine;
using CMS.EmailEngine;
using CMS.MacroEngine;
using CMS.SiteProvider;
using Eurobank.Helpers.Common.Communication;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class WorkFlowMailProcess
    {
        //private static string siteURL = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".Site_URL");
        public static string GetApplicationEditUrl(string applicationNumber)
        {
            string retval = string.Empty;
            string frontEndURL = SiteContext.CurrentSite.SitePresentationURL;
            if (!string.IsNullOrEmpty(frontEndURL))
            {
                string applicationNodeGuid = ServiceHelper.GetGuidByName(applicationNumber, "");
                if (!string.IsNullOrEmpty(applicationNodeGuid))
                {
                    retval = frontEndURL + "/applications/edit?application=" + applicationNodeGuid;
                }
            }
            return retval;
        }
        #region------------DRAFT------------
        //Mail Trigger 1
        public static bool SendMailSubmitToPendingVerificationIntroducer(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, bool isSendImmediately, string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.New_Application, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("customerName", applicantName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail( message, eti.TemplateName, resolver, isSendImmediately);
            return retval;
        }
        //Mail Trigger 2
        public static bool SendMailSubmitPendingChecker(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, string customerName,  bool isSendImmediately,string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.New_ApplicationChecker, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
            resolver.SetNamedSourceData("customerName", customerName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);
            return retval;
        }
        
        #endregion
        #region---------INITIATOR-----------
        //Mail Trigger 3
        public static bool SendMailReSubmitStepThatWasReturnedFromIntroducerInitiator(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, bool isSendImmediately,string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.ResubmitApplication, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("customerName", applicantName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);
            return retval;
        }
        //Mail Trigger 19
        public static bool SendMailReSubmitPendigChecker(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, bool isSendImmediately, string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.ResubmitApplicationChecker, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("customerName", applicantName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);
            return retval;
        }
        //Mail Trigger 20
        public static bool SendMailReSubmitPendigExecution(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, bool isSendImmediately, string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.ResubmitApplicationExecution, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("customerName", applicantName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);
            return retval;
        }
        //Mail Trigger 21
        public static bool SendMailReSubmitPendigBankDocuments(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, bool isSendImmediately, string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.ResubmitApplicationBankDocuments, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("customerName", applicantName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);
            return retval;
        }
        #endregion
        #region------PENDING CHECKER--------
        //Mail Trigger 1
        public static bool SendMailCheckedPendingReviewIntroducerPendingChecker()
        {
            bool retval = true;


            return retval;
        }
        //Mail Trigger 4
        public static bool SendMailReturnInitiatorIntroducerPendingChecker(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, string customerName, bool isSendImmediately,string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.ReturnedApplication, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
            resolver.SetNamedSourceData("customerName", customerName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);

            return retval;
        }
        //Mail Trigger 5
        public static bool SendMailWithdrawEndWithdrawnIntroducerPendingChecker(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, string customerName, bool isSendImmediately, string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.WithdrawnApplication, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
            resolver.SetNamedSourceData("customerName", customerName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);
            return retval;
        }
        #endregion
        #region-----PENDING VERIFICATION----
        //Mail Trigger 4
        public static bool SendMailReturnToInitiatorPendingInitiatorBankUser()
        {
            bool retval = true;


            return retval;
        }
        //Mail Trigger 6
        public static bool SendMailVerifiedPendingExecutionBankUser(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, bool isSendImmediately,string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.ApplicationReviewed, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("customerName", applicantName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);
            return retval;
        }
        //Mail Trigger 7
        public static bool SendMailCancelEndCancelledBankUser(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, string customerName, bool isSendImmediately,string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.ApplicationCancelled, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
            resolver.SetNamedSourceData("customerName", customerName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);

            return retval;
        }
        #endregion
        #region------PENDING EXECUTION------
        //Mail Trigger 8
        public static bool SendMailExecutedPendingDocumentsBankUser(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, bool isSendImmediately, string comments)
        {
            bool retval = false;

            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.ApplicationExecuted, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
            resolver.SetNamedSourceData("customerName", applicantName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));

            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);
            return retval;
        }
        //Mail Trigger 9
        public static bool SendMailReturnPendingVerificationBankUser(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, bool isSendImmediately,string comments)
        {
            bool retval = false;

            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.ApplicationExecution, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
            resolver.SetNamedSourceData("customerName", applicantName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);
            return retval;
        }
        //Mail Trigger 10
        public static bool SendMailEscalatePendingExecutionBankUser( string introducerName, string applicationNumber, string applicantName, string mailRecipients, string customerName, bool isSendImmediately)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.EscaltedForReview, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
            resolver.SetNamedSourceData("customerName", customerName.ToUpper());
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);

            return retval;
        }
        #endregion
        #region------PENDING DOCUMENTS------
        //Mail Trigger 11
        public static bool SendMailSendForSignaturePendingSignatureBankUser(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, string customerName, bool isSendImmediately, string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.DocumentsPrepared, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
			resolver.SetNamedSourceData("applicationType", applicationType);
			resolver.SetNamedSourceData("introducerName", introducerName);
			resolver.SetNamedSourceData("applicationNumber", applicationNumber);
			resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
			resolver.SetNamedSourceData("customerName", customerName.ToUpper());
			resolver.SetNamedSourceData("comments", comments);
			resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
			EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);

            return retval;
        }
        //Mail Trigger 12
        public static bool SendMailReturnPendingExecutionBankUser(string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, bool isSendImmediately)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.ApplicationExecution, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());

            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);

            return retval;
        }
        #endregion
        #region------PENDING SIGNATURE------
        //Mail Trigger 13
        public static bool SendMailSignatureCompletedPendingCompletion(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, string customerName, bool isSendImmediately,string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.DocumentsSigned, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
            resolver.SetNamedSourceData("customerName", customerName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);

            return retval;
        }
        //Mail Trigger 14
        public static bool SendMailReturnPendingDocuments(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, bool isSendImmediately, string comments)
        {
            bool retval = false;

            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.DocumentPreparation, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
            resolver.SetNamedSourceData("customerName", applicantName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));

            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);
            return retval;
        }
        #endregion
        #region------PENDING COMPLETION-----
        //Mail Trigger 15
        public static bool SendMailDocumentReceivedEndCompleted(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, string customerName, bool isSendImmediately,string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.ApplicationCompleted, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
            resolver.SetNamedSourceData("customerName", customerName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);

            return retval;
        }
        //Mail Trigger 16
        public static bool SendMailReturnPendingSignature(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, bool isSendImmediately, string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.DocumentSignatures, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
            resolver.SetNamedSourceData("customerName", applicantName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);

            return retval;
        }
        //Mail Trigger 22
        public static bool SendMailDocumentsPendingOmission(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, bool isSendImmediately, string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.DocumentsPendingOmmisions, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
            resolver.SetNamedSourceData("customerName", applicantName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));
            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);

            return retval;
        }
        #endregion
        #region------Pending Ommissions-----
        //Mail Trigger 17
        public static bool SendMailOmmissionCompletedPendingCompletion(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, bool isSendImmediately, string comments)
        {
            bool retval = false;
            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.OmmissionsCompleted, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
            resolver.SetNamedSourceData("customerName", applicantName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));

            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);

            return retval;
        }
        //Mail Trigger 18
        public static bool SendMailReturnPendingCompletion(string applicationType, string introducerName, string applicationNumber, string applicantName, string mailRecipients, string mailCced, bool isSendImmediately, string comments)
        {
            bool retval = false;

            EmailTemplateInfo eti = EmailTemplateProvider.GetEmailTemplate(Constants.PendingCompletion, 1);
            MacroResolver resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("applicationType", applicationType);
            resolver.SetNamedSourceData("introducerName", introducerName);
            resolver.SetNamedSourceData("applicationNumber", applicationNumber);
            resolver.SetNamedSourceData("applicantName", applicantName.ToUpper());
            resolver.SetNamedSourceData("customerName", applicantName.ToUpper());
            resolver.SetNamedSourceData("comments", comments);
            resolver.SetNamedSourceData("siteURL", GetApplicationEditUrl(applicationNumber));

            EmailMessage message = new EmailMessage();
            message.From = resolver.ResolveMacros(eti.TemplateFrom);
            message.Recipients = mailRecipients;
            message.Body = resolver.ResolveMacros(eti.TemplateText);
            message.Subject = resolver.ResolveMacros(eti.TemplateSubject);
            message.CcRecipients = mailCced;
            retval = MailProcess.SendEmail(message, eti.TemplateName, resolver, isSendImmediately);

            return retval;
        }
        #endregion
    }
}
