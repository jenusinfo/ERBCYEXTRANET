using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.SiteProvider;
using Eurobank.Helpers.Process;
using Eurobank.Models.Applications;
using Kentico.Content.Web.Mvc;
using System.Collections.Generic;
using System;
using CMS.Core;
using CMS.DocumentEngine;
using System.Linq;

namespace Eurobank.Helpers.DataAnnotation
{
    public class ApplicationSyncService
    {
        internal static void SyncApplicationSearchRecord(string ApplicationNumber)
        {
            IPageRetriever pageRetriever = Service.Resolve<IPageRetriever>();

            //Retriving all Application details
            IEnumerable<ApplicationDetails> applications = pageRetriever.Retrieve<ApplicationDetails>(query => query
                                .Path("/Applications-(1)", PathTypeEnum.Children)
                                .WhereContains("ApplicationDetails_ApplicationNumber" , string.IsNullOrEmpty(ApplicationNumber) ? "" : ApplicationNumber));

            foreach (var item in applications)
            {
                ApplicationDetailsModelView retVal = null;
                if (item != null)
                {
                    //Getting value for search fields 
                    retVal = new ApplicationDetailsModelView();
                    retVal.ApplicationDetails_ApplicationNumber = ValidationHelper.GetString(item.GetValue("ApplicationDetails_ApplicationNumber"), "");
                    retVal.ApplicationDetails_ApplicationTypeName = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ApplicationDetails_ApplicationType"), ""), "/Lookups/General/APPLICATION-TYPE");
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
                    retVal.ApplicationDetails_CreatedOn = ValidationHelper.GetString(Convert.ToDateTime(item.DocumentCreatedWhen).ToString("yyyyMMddHHmmss"), "");
                    retVal.ApplicationDetails_SubmittedOn = ValidationHelper.GetString(Convert.ToDateTime(item.ApplicationDetails_SubmittedOn).ToString("yyyyMMddHHmmss"), "");
                    retVal.ApplicationDetails_SubmittedBy = ValidationHelper.GetString(item.ApplicationDetails_SubmittedBy, "");
                    if (retVal.ApplicationDetails_ApplicationTypeName == "LEGAL ENTITY")
                    {
                        var getApplicantDetails = ApplicantProcess.GetLegalApplicantModels(retVal.ApplicationDetails_ApplicationNumber);
                        if (getApplicantDetails != null)
                        {
                            string fullName = "";
                            foreach (var applicantData in getApplicantDetails)
                            {
                                fullName = fullName + applicantData.FullName + " ";
                            }
                            retVal.FullNameOfApplicant = fullName.Trim();
                        }
                    }
                    else
                    {
                        var getApplicantDetails = ApplicantProcess.GetApplicantModels(retVal.ApplicationDetails_ApplicationNumber);
                        if (getApplicantDetails != null)
                        {
                            getApplicantDetails = getApplicantDetails.OrderBy(m => m.CreatedDateTime).ToList();
                            string fullName = "";
                            foreach (var applicantData in getApplicantDetails)
                            {
                                fullName = fullName + applicantData.FullName + " ";
                            }

                            retVal.FullNameOfApplicant = fullName.Trim();
                        }
                    }
                    retVal.ApplicationDetails_ApplicationStatusName = ServiceHelper.GetName(ValidationHelper.GetString(item.ApplicationDetails_ApplicationStatus, ""), Constants.APPLICATION_STATUS);
                    item.SetValue("Search_Type", retVal.ApplicationDetails_ApplicationTypeName);
                    item.SetValue("Search_Full_Name", retVal.FullNameOfApplicant);
                    item.SetValue("Search_Introducer", retVal.ApplicationDetails_Introducer);
                    item.SetValue("Search_Bank_Center", retVal.ApplicationDetails_ResponsibleBankingCenterUnit);
                    item.SetValue("Search_Submitted_By", retVal.ApplicationDetails_SubmittedBy);
                    item.SetValue("Search_Submitted_On", (string.Equals(retVal.ApplicationDetails_SubmittedOn, "00010101000000", StringComparison.OrdinalIgnoreCase) ? null : retVal.ApplicationDetails_SubmittedOn));
                    item.SetValue("Search_Created_On", retVal.ApplicationDetails_CreatedOn);
                    item.SetValue("Search_Status", retVal.ApplicationDetails_ApplicationStatusName);
                    item.Update();
                }
            }
        }
    }
}
