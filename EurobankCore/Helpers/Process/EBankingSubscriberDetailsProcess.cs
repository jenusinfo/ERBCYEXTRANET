using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class EBankingSubscriberDetailsProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

        private static readonly string _EBankingSubscriberDetailsFolderName = "EBanking";
        private static readonly string _EBankingSubscriberDetailsDocumentName = "EBank";

        public static List<EBankingSubscriberDetailsModel> GetEBankingSubscriberDetailsModels(string applicationNumber, bool isLegalEntity)
        {
            List<EBankingSubscriberDetailsModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (!string.IsNullOrEmpty(applicationNumber))
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type("Eurobank.ApplicationDetails")
                    .WhereEquals("NodeName", applicationNumber)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    retVal = new List<EBankingSubscriberDetailsModel>();
                    TreeNode eBankingSubscriberDetailsFolderRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EBankingSubscriberDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        eBankingSubscriberDetailsFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EBankingSubscriberDetailsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (eBankingSubscriberDetailsFolderRoot != null)
                        {
                            List<TreeNode> eBankingSubscriberDetailsNodes = eBankingSubscriberDetailsFolderRoot.Children.Where(u => u.ClassName == EBankingSubscriberDetails.CLASS_NAME).ToList();

                            if (eBankingSubscriberDetailsNodes != null && eBankingSubscriberDetailsNodes.Count > 0)
                            {
                                eBankingSubscriberDetailsNodes.ForEach(t =>
                                {
                                    EBankingSubscriberDetails eBankingSubscriberDetails = EBankingSubscriberDetailsProvider.GetEBankingSubscriberDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (eBankingSubscriberDetails != null)
                                    {
                                        EBankingSubscriberDetailsModel eBankingSubscriberDetailsModel = BindEBankingSubscriberDetailsModel(applicationNumber, eBankingSubscriberDetails, isLegalEntity);
                                        if (eBankingSubscriberDetailsModel != null)
                                        {
                                            retVal.Add(eBankingSubscriberDetailsModel);
                                        }
                                    }
                                });
                            }
                        }
                    }
                }



            }

            return retVal;
        }

        public static EBankingSubscriberDetailsModel SaveEBankingSubscriberDetailsModel(string applicationNumber, EBankingSubscriberDetailsModel model, bool isLegalEntity)
        {
            EBankingSubscriberDetailsModel retVal = null;

            if (model != null)
            {
                List<SelectListItem> subscriberList = PersonalDetailsProcess.GetEBankingSubscribers(applicationNumber);
                model.SubscriberName = (subscriberList != null && subscriberList.Any(y => y.Value == model.Subscriber) ? subscriberList.FirstOrDefault(y => y.Value == model.Subscriber).Text : string.Empty);
            }
            if (model != null && model.Id > 0)
            {
                EBankingSubscriberDetails eBankingSubscriberDetails = GetEBankingSubscriberDetailsById(model.Id);
                if (eBankingSubscriberDetails != null)
                {
                    EBankingSubscriberDetails updatedEBankingSubscriberDetails = BindEBankingSubscriberDetails(eBankingSubscriberDetails, model, isLegalEntity);
                    if (updatedEBankingSubscriberDetails != null)
                    {
                        updatedEBankingSubscriberDetails.Update();
                        retVal = BindEBankingSubscriberDetailsModel(applicationNumber, updatedEBankingSubscriberDetails, isLegalEntity);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(applicationNumber) && model != null)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type("Eurobank.ApplicationDetails")
                    .WhereEquals("NodeName", applicationNumber)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode eBankingSubscriberDetailsFolderRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EBankingSubscriberDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        eBankingSubscriberDetailsFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EBankingSubscriberDetailsFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        eBankingSubscriberDetailsFolderRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        eBankingSubscriberDetailsFolderRoot.DocumentName = _EBankingSubscriberDetailsFolderName;
                        eBankingSubscriberDetailsFolderRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        eBankingSubscriberDetailsFolderRoot.Insert(applicationDetailsNode);
                    }
                    EBankingSubscriberDetails eBankingSubscriberDetails = BindEBankingSubscriberDetails(null, model, isLegalEntity);
                    if (eBankingSubscriberDetails != null && eBankingSubscriberDetailsFolderRoot != null)
                    {
                        //eBankingSubscriberDetails.DocumentName = model.SubscriberName;
                        //eBankingSubscriberDetails.NodeName = model.SubscriberName;
                        eBankingSubscriberDetails.DocumentName = _EBankingSubscriberDetailsDocumentName;
                        eBankingSubscriberDetails.NodeName = _EBankingSubscriberDetailsDocumentName;

                        eBankingSubscriberDetails.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        eBankingSubscriberDetails.Insert(eBankingSubscriberDetailsFolderRoot);
                        model = BindEBankingSubscriberDetailsModel(applicationNumber, eBankingSubscriberDetails, isLegalEntity);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        public static EBankingSubscriberDetailsModel SaveAutoEBankingSubscriberIndividual(string applicationNumber, int personalDetailsId, bool isApplicant,bool isLegalEntity)
        {
            EBankingSubscriberDetailsModel retVal = null;
            var limitAmount = ServiceHelper.GetLimitAmount();
           
            var personalDetails = PersonalDetailsProcess.GetPersonalDetailsById(personalDetailsId);
            var identifications = IdentificationDetailsProcess.GetIdentificationDetails(personalDetailsId);
            List<SelectListItem> accessLevels = null;
            if (isLegalEntity)
            {
                accessLevels = ServiceHelper.GetAccessLevel();
            }
            else
            {
                accessLevels = ServiceHelper.GetAccessLevelIndividual();
            }
            if (limitAmount != null && limitAmount.Any(y => string.Equals(y.Text, "MAXIMUM ALLOWABLE LIMIT PER TRANSACTION TYPE", StringComparison.OrdinalIgnoreCase)) && accessLevels != null && accessLevels.Any(y => string.Equals(y.Text, "FULL", StringComparison.OrdinalIgnoreCase)
             ) && personalDetails != null)
            {
                int existedEbankingId = 0;
                var ebankingSubscribers = GetEBankingSubscriberDetailsModels(applicationNumber, false);
                if (ebankingSubscribers != null && ebankingSubscribers.Any(z => string.Equals(z.Subscriber, personalDetails.NodeGUID.ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    existedEbankingId = ebankingSubscribers.FirstOrDefault(z => string.Equals(z.Subscriber, personalDetails.NodeGUID.ToString(), StringComparison.OrdinalIgnoreCase)).Id;
                }
                EBankingSubscriberDetailsModel eBankingSubscriberDetailsModel = new EBankingSubscriberDetailsModel();

                eBankingSubscriberDetailsModel.Id = existedEbankingId;
                eBankingSubscriberDetailsModel.AccessToAllPersonalAccounts = true;
                eBankingSubscriberDetailsModel.AutomaticallyAddFuturePersonalAccounts = true;
                eBankingSubscriberDetailsModel.LimitAmount = limitAmount.FirstOrDefault(y => string.Equals(y.Text, "MAXIMUM ALLOWABLE LIMIT PER TRANSACTION TYPE", StringComparison.OrdinalIgnoreCase)).Value;
                //AccessLevel = accessLevels.FirstOrDefault(y => string.Equals(y.Text, "FULL", StringComparison.OrdinalIgnoreCase)).Value,
                eBankingSubscriberDetailsModel.AccessLevel = null;
                if (isApplicant)
                {
                    eBankingSubscriberDetailsModel.AccessLevel = accessLevels.FirstOrDefault(y => string.Equals(y.Text, "FULL", StringComparison.OrdinalIgnoreCase)).Value;
                }
                eBankingSubscriberDetailsModel.Subscriber = personalDetails.NodeGUID.ToString();
                eBankingSubscriberDetailsModel.IdentityPassportNumber = (identifications != null && identifications.Count > 0) ? identifications.FirstOrDefault().IdentificationDetails_IdentificationNumber : string.Empty;
                eBankingSubscriberDetailsModel.CountryOfIssue = (identifications != null && identifications.Count > 0) ? identifications.FirstOrDefault().IdentificationDetails_CountryOfIssue.ToString() : string.Empty;
                eBankingSubscriberDetailsModel.EbankingSubscriberDetails_Status = false;

                var eBankingSubscriberDetails = EBankingSubscriberDetailsProcess.SaveEBankingSubscriberDetailsModel(applicationNumber, eBankingSubscriberDetailsModel, false);
            }


            return retVal;
        }

        public static EBankingSubscriberDetailsModel SaveAutoEBankingSubscriberLegal(string applicationNumber, int companyDetailsId)
        {
            EBankingSubscriberDetailsModel retVal = null;
            var limitAmount = ServiceHelper.GetLimitAmount();
            var accessLevels = ServiceHelper.GetAccessLevel();
            var companyDetails = CompanyDetailsProcess.GetCompanyDetailsById(companyDetailsId);
            //var identifications = IdentificationDetailsProcess.GetIdentificationDetails(personalDetailsId);

            if (limitAmount != null && limitAmount.Any(y => string.Equals(y.Text, "MAXIMUM ALLOWABLE LIMIT PER TRANSACTION TYPE", StringComparison.OrdinalIgnoreCase)) && accessLevels != null && accessLevels.Any(y => string.Equals(y.Text, "FULL", StringComparison.OrdinalIgnoreCase)
             ) && companyDetails != null)
            {
                EBankingSubscriberDetailsModel eBankingSubscriberDetailsModel = new EBankingSubscriberDetailsModel()
                {
                    AccessToAllPersonalAccounts = true,
                    AutomaticallyAddFuturePersonalAccounts = true,
                    LimitAmount = limitAmount.FirstOrDefault(y => string.Equals(y.Text, "MAXIMUM ALLOWABLE LIMIT PER TRANSACTION TYPE", StringComparison.OrdinalIgnoreCase)).Value,
                    AccessLevel = accessLevels.FirstOrDefault(y => string.Equals(y.Text, "FULL", StringComparison.OrdinalIgnoreCase)).Value,
                    Subscriber = companyDetails.NodeGUID.ToString(),
                    //IdentityPassportNumber = (identifications != null && identifications.Count > 0) ? identifications.FirstOrDefault().IdentificationDetails_IdentificationNumber : string.Empty,
                    //CountryOfIssue = (identifications != null && identifications.Count > 0) ? identifications.FirstOrDefault().IdentificationDetails_CountryOfIssue.ToString() : string.Empty,
                    EbankingSubscriberDetails_Status = true
                };
                var eBankingSubscriberDetails = EBankingSubscriberDetailsProcess.SaveEBankingSubscriberDetailsModel(applicationNumber, eBankingSubscriberDetailsModel, true);
            }


            return retVal;
        }

        private static EBankingSubscriberDetails GetEBankingSubscriberDetailsById(int eBankingSubscriberDetailsId)
        {
            EBankingSubscriberDetails retVal = null;

            if (eBankingSubscriberDetailsId > 0)
            {
                var eBankingSubscriberDetails = EBankingSubscriberDetailsProvider.GetEBankingSubscriberDetails();
                if (eBankingSubscriberDetails != null && eBankingSubscriberDetails.Count > 0)
                {
                    retVal = eBankingSubscriberDetails.FirstOrDefault(o => o.EbankingSubscriberDetailsID == eBankingSubscriberDetailsId);
                }
            }

            return retVal;
        }

        private static EBankingSubscriberDetailsModel BindEBankingSubscriberDetailsModel(string applicationNumber, EBankingSubscriberDetails item, bool isLegalEntity)
        {
            EBankingSubscriberDetailsModel retVal = null;

            if (item != null)
            {
                List<SelectListItem> subscriberList = PersonalDetailsProcess.GetEBankingSubscribers(applicationNumber);
                List<SelectListItem> limitAmount = ServiceHelper.GetLimitAmount();
                List<SelectListItem> signatoryGroup = ServiceHelper.GetSignatoryGroup();
                List<SelectListItem> accesssLevels = null;
                if (isLegalEntity)
                {
                    accesssLevels = ServiceHelper.GetAccessLevel();
                }
                else
                {
                    accesssLevels = ServiceHelper.GetAccessLevelIndividual();
                }
                retVal = new EBankingSubscriberDetailsModel()
                {
                    Id = item.EbankingSubscriberDetailsID,
                    Subscriber = item.EbankingSubscriberDetails_Subscriber,
                    SubscriberName = (subscriberList != null && subscriberList.Any(y => y.Value == item.EbankingSubscriberDetails_Subscriber) ? subscriberList.FirstOrDefault(y => y.Value == item.EbankingSubscriberDetails_Subscriber).Text : item.NodeName),
                    AccessLevel = item.EbankingSubscriberDetails_AccessLevel.ToString(),
                    AccessLevelName = (accesssLevels != null && accesssLevels.Any(k => k.Value == item.EbankingSubscriberDetails_AccessLevel.ToString()) ? accesssLevels.FirstOrDefault(k => k.Value == item.EbankingSubscriberDetails_AccessLevel.ToString()).Text : string.Empty),
                    AccessToAllPersonalAccounts = item.EbankingSubscriberDetails_AccessToAllPersonalAccounts,
                    AccessToAllPersonalAccountsValue = item.EbankingSubscriberDetails_AccessToAllPersonalAccounts ? "YES" : "NO",
                    AutomaticallyAddFuturePersonalAccounts = item.EbankingSubscriberDetails_AutomaticallyAddFuturePersonalAccounts,
                    AutomaticallyAddFuturePersonalAccountsValue = item.EbankingSubscriberDetails_AutomaticallyAddFuturePersonalAccounts ? "YES" : "NO",
                    ReceiveEStatements = item.EbankingSubscriberDetails_ReceiveEStatements,
                    IdentityPassportNumber = item.EBankingSubscriberDetails_IdentityPassportNumber,
                    CountryOfIssue = item.EBankingSubscriberDetails_CountryOfIssue,
                    EbankingSubscriberDetails_Status = item.EbankingSubscriberDetails_Status,
                    Status_Name = item.EbankingSubscriberDetails_Status == true ? "Complete" : "Pending",
                    LimitAmount = item.EbankingSubscriberDetails_LimitAmount.ToString(),
                    LimitAmountName = (limitAmount != null && limitAmount.Any(k => k.Value == item.EbankingSubscriberDetails_LimitAmount.ToString()) ? limitAmount.FirstOrDefault(k => k.Value == item.EbankingSubscriberDetails_LimitAmount.ToString()).Text : string.Empty),
                    SignatoryGroupName = item.EbankingSubscriberDetails_SignatoryGroup.ToString(),
                    SignatoryGroup = (signatoryGroup != null && signatoryGroup.Any(k => k.Value == item.EbankingSubscriberDetails_SignatoryGroup.ToString()) ? signatoryGroup.FirstOrDefault(k => k.Value == item.EbankingSubscriberDetails_SignatoryGroup.ToString()).Text : string.Empty),
                    PartyReferenceId = PersonalDetailsProcess.GetPersonDetailsIdByGuid(item.EbankingSubscriberDetails_Subscriber),
				};
            }

            return retVal;
        }

        private static EBankingSubscriberDetails BindEBankingSubscriberDetails(EBankingSubscriberDetails existEBankingSubscriberDetails, EBankingSubscriberDetailsModel item, bool isLegalEntity)
        {
            EBankingSubscriberDetails retVal = new EBankingSubscriberDetails();
            if (existEBankingSubscriberDetails != null)
            {
                retVal = existEBankingSubscriberDetails;
            }
            if (item != null)
            {
                retVal.EbankingSubscriberDetails_Subscriber = item.Subscriber;
                if (!string.IsNullOrEmpty(item.AccessLevel))
                    retVal.EbankingSubscriberDetails_AccessLevel = new Guid(item.AccessLevel);
                retVal.EbankingSubscriberDetails_AccessToAllPersonalAccounts = item.AccessToAllPersonalAccounts;
                retVal.EbankingSubscriberDetails_AutomaticallyAddFuturePersonalAccounts = item.AutomaticallyAddFuturePersonalAccounts;
                retVal.EbankingSubscriberDetails_ReceiveEStatements = item.ReceiveEStatements;
                retVal.EBankingSubscriberDetails_IdentityPassportNumber = item.IdentityPassportNumber;
                retVal.EBankingSubscriberDetails_CountryOfIssue = item.CountryOfIssue;
                retVal.EbankingSubscriberDetails_Status = item.EbankingSubscriberDetails_Status;
                if (item.LimitAmount != null)
                {
                    retVal.EbankingSubscriberDetails_LimitAmount = new Guid(item.LimitAmount);
                }
                if (isLegalEntity && !string.IsNullOrEmpty(item.SignatoryGroupName))
                {
                    retVal.EbankingSubscriberDetails_SignatoryGroup = new Guid(item.SignatoryGroupName);
                }

            }

            return retVal;
        }
    }
}
