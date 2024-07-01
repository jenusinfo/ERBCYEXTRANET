using ClosedXML.Excel;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Applications;
using Eurobank.Models.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace Eurobank.Helpers.Process
{
    public class ApplicationExcelDownloadProcess
    {
        
        public static List<ApplicationDetailsModelView> GetApplicationByUser(UserModel user, ApplicationsRepository applicationsRepository,bool IsGraph=false)
        {
            List<ApplicationDetailsModelView> retVal = null;
            //List<string> blanklist = new List<string>();

            if (user != null && user.UserInformation != null && !string.IsNullOrEmpty(user.UserType) && applicationsRepository != null)
            {
                retVal = new List<ApplicationDetailsModelView>();
                IEnumerable<ApplicationDetails> applications = null;
                if (string.Equals(user.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase) && string.Equals(user.UserRole, ApplicationUserRole.POWER.ToString(), StringComparison.OrdinalIgnoreCase))
                {                    
                    string userOrganization = ValidationHelper.GetString(user.UserInformation.UserSettings.GetValue("Eurobank_UserOrganisation"), "");
                    applications = applicationsRepository.GetApplicationDetailsForIntroducerPower(userOrganization,1,999999,out int dummyvar, "", "", "", "", "");
                }
                else if (string.Equals(user.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    string userOrganization = ValidationHelper.GetString(user.UserInformation.UserSettings.GetValue("Eurobank_UserOrganisation"), "");
                    applications = applicationsRepository.GetApplicationDetailsForIntroducerNormal(user.UserInformation.UserID, 1, 999999, out int dummyvar,"","","", "", "");
                }
                else if (string.Equals(user.UserType, ApplicationUserType.INTERNAL.ToString(), StringComparison.OrdinalIgnoreCase) && string.Equals(user.UserRole, ApplicationUserRole.POWER.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    if (user.InternalUser != null && user.InternalUser.AssignBankBranches != null && user.InternalUser.AssignBankBranches.Count > 0)
                    {
                        applications = applicationsRepository.GetApplicationDetailsForInternalUserPower(user.InternalUser.AssignBankBranches, 1, 999999, out int dummyvar, "", "", "", "", "");
                    }
                }
                else if (string.Equals(user.UserType, ApplicationUserType.INTERNAL.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    if (user.InternalUser != null && user.InternalUser.AssignBankBranches != null && user.InternalUser.AssignBankBranches.Count > 0)
                    {
                        applications = applicationsRepository.GetApplicationDetailsForInternalUserNormal(user.InternalUser.AssignBankBranches, 1, 999999, out int dummyvar, "", "", "", "", "");
                    }
                }
                foreach (var application in applications)
                {
                    retVal.Add(BindApplicationDetailsModelView(user.UserType, user.UserRole, application, IsGraph));
                }
            }
            return retVal;
        }
        private static ApplicationDetailsModelView BindApplicationDetailsModelView(string userType, string userRole, ApplicationDetails item,bool isGrapth=false)
        {
            ApplicationDetailsModelView retVal = null;

            if (item != null)
            {
                retVal = new ApplicationDetailsModelView();
                retVal.Application_NodeGUID = item.NodeGUID.ToString();
                retVal.ApplicationDetails_ApplicationNumber = ValidationHelper.GetString(item.GetValue("ApplicationDetails_ApplicationNumber"), "");
                retVal.ApplicationDetails_ApplicationStatus = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ApplicationDetails_ApplicationStatus"), ""), "/Lookups/General/APPLICATION-SERVICES");
                retVal.ApplicationDetails_ApplicationStatusName = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ApplicationDetails_ApplicationStatus"), ""), "/Lookups/General/APPLICATION-WORKFLOW-STATUS");
                retVal.ApplicationDetails_ApplicationType = item.ApplicationDetails_ApplicationType;
                retVal.ApplicationDetails_ApplicationTypeName = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ApplicationDetails_ApplicationType"), ""), "/Lookups/General/APPLICATION-TYPE");
                retVal.ApplicationDetails_ApplicatonServices = ValidationHelper.GetString(item.GetValue("ApplicationDetails_ApplicatonServices"), "");

                retVal.ApplicationDetails_CurrentStage = ValidationHelper.GetString(item.GetValue("ApplicationDetails_CurrentStage"), "");
                retVal.ApplicationDetails_IntroducerCIF = ValidationHelper.GetString(item.GetValue("ApplicationDetails_IntroducerCIF"), "");
                retVal.ApplicationDetails_IntroducerName = ValidationHelper.GetString(item.GetValue("ApplicationDetails_IntroducerName"), "");
                if (!string.IsNullOrEmpty(retVal.ApplicationDetails_IntroducerName) && retVal.ApplicationDetails_IntroducerName.IndexOf('-') > -1)
                {
                    retVal.ApplicationDetails_Introducer = retVal.ApplicationDetails_IntroducerName.Substring(retVal.ApplicationDetails_IntroducerName.IndexOf('-') + 1);
                }
                retVal.ApplicationDetails_ResponsibleBankingCenter = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ApplicationDetails_ResponsibleBankingCenter"), ""), "/Bank-Units");
                string bankingGuid = ValidationHelper.GetString(item.GetValue("ApplicationDetails_ResponsibleBankingCenter"), "");
                if (!string.IsNullOrEmpty(bankingGuid))
                {
                    BankUnit bankUnit = BankUnitProvider.GetBankUnit(new Guid(bankingGuid), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName);
                    if (bankUnit != null)
                    {
                        retVal.ApplicationDetails_ResponsibleBankingCenterUnit = bankUnit.BankUnitCode;

                    }

                }


                retVal.ApplicationDetails_ResponsibleOfficer = ServiceHelper.GetUserName(ValidationHelper.GetString(item.GetValue("ApplicationDetails_ResponsibleOfficer"), ""));

                retVal.ApplicationDetailsID = ValidationHelper.GetInteger(item.GetValue("ApplicationDetailsID"), 0);
                retVal.ApplicationDetails_CreatedOn = ValidationHelper.GetString(Convert.ToDateTime(item.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss"), "");
                retVal.ApplicationDetails_SubmittedOn = ValidationHelper.GetString(Convert.ToDateTime(item.ApplicationDetails_SubmittedOn).ToString("dd/MM/yyyy HH:mm:ss"), "");
                retVal.ApplicationDetails_SubmittedBy = ValidationHelper.GetString(item.ApplicationDetails_SubmittedBy, "");

                if(isGrapth) return retVal;

                if (retVal.ApplicationDetails_ApplicationTypeName == "LEGAL ENTITY")
                {
                    var getApplicantDetails = ApplicantProcess.GetLegalApplicantModels(retVal.ApplicationDetails_ApplicationNumber);
                    if (getApplicantDetails != null)
                    {
                        string fullName = "";
                        string identification = "";
                        foreach (var applicantData in getApplicantDetails)
                        {
                            fullName = fullName + applicantData.FullName.ToUpper() + ",";
                            identification = identification + applicantData.FirstIdentificationNumber.ToUpper() + ",";
                        }
                        retVal.FullNameOfApplicant = fullName.TrimEnd(',');
                        retVal.ApplicantIdentificationNumber = identification.TrimEnd(',');
                    }
                }
                else
                {
                    var getApplicantDetails = ApplicantProcess.GetApplicantModels(retVal.ApplicationDetails_ApplicationNumber);
                    if (getApplicantDetails != null)
                    {
                        string fullName = "";
                        string identification = "";
                        foreach (var applicantData in getApplicantDetails)
                        {
                            fullName = fullName + applicantData.FullName.ToUpper() + ",";
                            identification = identification + applicantData.FirstIdentificationNumber.ToUpper() + ",";
                        }

                        retVal.FullNameOfApplicant = fullName.TrimEnd(',');
                        retVal.ApplicantIdentificationNumber = identification.TrimEnd(',');
                    }
                }

            }

            return retVal;
        }

    }
}
