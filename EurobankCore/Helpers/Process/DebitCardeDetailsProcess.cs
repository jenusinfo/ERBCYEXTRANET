using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Applications;
using Eurobank.Models.Applications.DebitCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class DebitCardeDetailsProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";
        private static readonly string _DebitCardDetailsFolderName = "Debit Cards";
        private static readonly string _DebitCardDetailsDcoumentName = "Card";
        //public static string GetCountryCodePrefixValue(string CountryCodeText)
        //{
        //    var CountryCodePrefix = ServiceHelper.GetCountryCodePrefix();
        //    string Country_Code_MobileTelNoNumber = (CountryCodePrefix != null && CountryCodePrefix.Count > 0 && CountryCodeText != null && CountryCodePrefix.Any(f => f.Text == CountryCodeText.ToString())) ? CountryCodePrefix.FirstOrDefault(f => f.Text == CountryCodeText.ToString()).Value : string.Empty;
        //    return Country_Code_MobileTelNoNumber;
        //}
        //public static string GetCountryCodePrefixText(string CountryCodeValue)
        //{
        //    var CountryCodePrefix = ServiceHelper.GetCountryCodePrefix();
        //    string Country_Code_MobileTelNoNumber = (CountryCodePrefix != null && CountryCodePrefix.Count > 0 && CountryCodeValue != null && CountryCodePrefix.Any(f => f.Value == CountryCodeValue.ToString())) ? CountryCodePrefix.FirstOrDefault(f => f.Value == CountryCodeValue.ToString()).Text : string.Empty;
        //    return Country_Code_MobileTelNoNumber;
        //}
        public static List<DebitCardDetailsViewModel> GetDebitCardDetails(IEnumerable<TreeNode> treeNode, string applicationType, int applicationID)
        {
            bool isLegalEntity = string.Equals(applicationType, "LEGAL-ENTITY", StringComparison.OrdinalIgnoreCase);
            string dispatch_Method = string.Empty;
            if (isLegalEntity)
            {
                dispatch_Method = Constants.DISPATCH_METHOD;
            }
            else
            {
                dispatch_Method = Constants.DISPATCH_METHOD_INDIVIDUAL;
            }
            var associatedAccount = AccountsProcess.GetDebitCardAccountsByApplicationID(applicationID);
            List<DebitCardDetailsViewModel> retVal = new List<DebitCardDetailsViewModel>();
            foreach (var item in treeNode)
            {
                DebitCardDetailsViewModel debitCardDetailsViewModel = new DebitCardDetailsViewModel();
                debitCardDetailsViewModel.DebitCardDetailsID = ValidationHelper.GetInteger(item.GetValue("DebitCardDetailsID"), 0);
                debitCardDetailsViewModel.DebitCardDetails_FullName = ValidationHelper.GetString(item.GetValue("DebitCardDetails_FullName"), "");
                debitCardDetailsViewModel.DebitCardDetails_CardType = ValidationHelper.GetString(item.GetValue("DebitCardDetails_CardType"), "");
                debitCardDetailsViewModel.AssociatedAccount = ValidationHelper.GetString(item.GetValue("AssociatedAccount"), "");
                if (associatedAccount != null)
                {
                    debitCardDetailsViewModel.AssociatedAccountName = associatedAccount.Where(x => x.Value == debitCardDetailsViewModel.AssociatedAccount).Select(x => x.Text).FirstOrDefault();
                }

                debitCardDetailsViewModel.DebitCardDetails_DispatchMethod = ValidationHelper.GetString(item.GetValue("DebitCardDetails_DispatchMethod"), "");
                debitCardDetailsViewModel.DebitCardDetails_CardTypeName = ServiceHelper.GetName(ValidationHelper.GetString(debitCardDetailsViewModel.DebitCardDetails_CardType, ""), Constants.CARD_TYPE);
                if (!string.IsNullOrEmpty(debitCardDetailsViewModel.DebitCardDetails_DispatchMethod))
                {
                    if (isLegalEntity)
                    {
                        var result = ServiceHelper.GetDispatchMethod();
                        debitCardDetailsViewModel.DebitCardDetails_DispatchMethodName = (result != null && result.Count > 0 && debitCardDetailsViewModel.DebitCardDetails_DispatchMethod != null) ? result.FirstOrDefault(f => f.Value == debitCardDetailsViewModel.DebitCardDetails_DispatchMethod.ToString()).Text : string.Empty;
                    }
                    else
                    {
                        var result = ServiceHelper.GetDispatchMethodIndividual();
                        debitCardDetailsViewModel.DebitCardDetails_DispatchMethodName = (result != null && result.Count > 0 && debitCardDetailsViewModel.DebitCardDetails_DispatchMethod != null) ? result.FirstOrDefault(f => f.Value == debitCardDetailsViewModel.DebitCardDetails_DispatchMethod.ToString()).Text : string.Empty;
                    } 
                }

                debitCardDetailsViewModel.DebitCardDetails_IdentityNumber = ValidationHelper.GetString(item.GetValue("DebitCardDetails_IdentityNumber"), "");
                debitCardDetailsViewModel.DebitCardDetails_DeliveryDetails = ValidationHelper.GetString(item.GetValue("DebitCardDetails_DeliveryDetails"), "");
                debitCardDetailsViewModel.DebitCardDetails_CardholderName = ValidationHelper.GetString(item.GetValue("DebitCardDetails_CardholderName"), "");
                debitCardDetailsViewModel.Country_Code = ValidationHelper.GetString(item.GetValue("DebitCardDetails_MobileNumber_CountryCode"), "");
                debitCardDetailsViewModel.DebitCardDetails_MobileNumber = ValidationHelper.GetString(item.GetValue("DebitCardDetails_MobileNumber"), "");
                debitCardDetailsViewModel.DebitCardDetails_CompanyNameAppearOnCard = ValidationHelper.GetString(item.GetValue("DebitCardDetails_CompanyNameAppearOnCard"), "");
                debitCardDetailsViewModel.DebitCardDetails_DeliveryAddress = ValidationHelper.GetString(item.GetValue("DebitCardDetails_DeliveryAddress"), "");
                debitCardDetailsViewModel.DebitCardDetails_OtherDeliveryAddress = ValidationHelper.GetString(item.GetValue("DebitCardDetails_OtherDeliveryAddress"), "");
                debitCardDetailsViewModel.DebitCardDetails_CollectedBy = ValidationHelper.GetString(item.GetValue("DebitCardDetails_CollectedBy"), "");
                debitCardDetailsViewModel.DebitCardDetails_Status = ValidationHelper.GetBoolean(item.GetValue("DebitCardDetails_Status"), false);
                debitCardDetailsViewModel.DebitCardDetails_StatusName = debitCardDetailsViewModel.DebitCardDetails_Status == true ? "Complete" : "Pending";
                debitCardDetailsViewModel.DebitCardDetails_CollectedByName = ValidationHelper.GetString(item.GetValue("DebitCardDetails_CollectedByName"), "");
                debitCardDetailsViewModel.DebitCardDetails_CardHolderCountryOfIssue = ValidationHelper.GetString(item.GetValue("DebitCardDetails_CardHolderCountryOfIssue"), "");
                debitCardDetailsViewModel.DebitCardDetails_CardHolderIdentityNumber = ValidationHelper.GetString(item.GetValue("DebitCardDetails_CardHolderIdentityNumber"), "");
                retVal.Add(debitCardDetailsViewModel);
            }
            return retVal;
        }

        public static DebitCardDetailsViewModel SaveDebitCardDetailsModel(DebitCardDetailsViewModel model, TreeNode treeNodeData, bool isLegalEntity)
        {
            DebitCardDetailsViewModel retVal = new DebitCardDetailsViewModel();
            string dispatch_Method = string.Empty;
            if (isLegalEntity)
            {
                dispatch_Method = Constants.DISPATCH_METHOD;
                
            }
            else
            {
                dispatch_Method = Constants.DISPATCH_METHOD_INDIVIDUAL;
            }

            if (model != null)
            {
                if (treeNodeData != null)
                {
                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                    TreeNode accountsfoldernode_parent = tree.SelectNodes()
                        .Path(treeNodeData.NodeAliasPath + "/Debit-Cards")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                    if (accountsfoldernode_parent == null)
                    {

                        accountsfoldernode_parent = TreeNode.New("CMS.Folder", tree);
                        accountsfoldernode_parent.DocumentName = "Debit Cards";
                        accountsfoldernode_parent.DocumentCulture = "en-US";
                        accountsfoldernode_parent.Insert(treeNodeData);
                    }
                    TreeNode debitcardDetails = TreeNode.New("Eurobank.DebitCardDetails", tree);
                    //debitcardDetails.DocumentName = ValidationHelper.GetString(model.DebitCardDetails_FullName, "");
                    debitcardDetails.DocumentName = _DebitCardDetailsDcoumentName;
                    //debitcardDetails.SetValue("AssociatedAccount", model.AssociatedAccount);
                    debitcardDetails.SetValue("DebitCardDetails_CardType", model.DebitCardDetails_CardType);
                    debitcardDetails.SetValue("AssociatedAccount", model.AssociatedAccount);
                    debitcardDetails.SetValue("DebitCardDetails_CardholderName", model.DebitCardDetails_CardholderName);
                    debitcardDetails.SetValue("DebitCardDetails_FullName", model.DebitCardDetails_FullName);
                    debitcardDetails.SetValue("DebitCardDetails_MobileNumber_CountryCode", model.Country_Code);
                    debitcardDetails.SetValue("DebitCardDetails_MobileNumber", model.DebitCardDetails_MobileNumber);
                    debitcardDetails.SetValue("DebitCardDetails_CompanyNameAppearOnCard", model.DebitCardDetails_CompanyNameAppearOnCard);
                    debitcardDetails.SetValue("DebitCardDetails_DispatchMethod", model.DebitCardDetails_DispatchMethod);
                    if (!isLegalEntity)
                    {
                        var result = ServiceHelper.GetDispatchMethodIndividual();
                        model.DebitCardDetails_DispatchMethodName = (result != null && result.Count > 0 && model.DebitCardDetails_DispatchMethod != null) ? result.FirstOrDefault(f => f.Value == model.DebitCardDetails_DispatchMethod.ToString()).Text : string.Empty;
                        if (string.Equals(model.DebitCardDetails_DispatchMethodName, "TO BE COLLECTED FROM THE BANKING CENTRE.", StringComparison.OrdinalIgnoreCase) || string.Equals(model.DebitCardDetails_DispatchMethodName, "TO BE DISPATCHED BY POST TO THE CARDHOLDER’S MAILING ADDRESS (APPLICABLE FOR DOMESTIC AND OVERSEAS ADDRESSES).", StringComparison.OrdinalIgnoreCase) || string.Equals(model.DebitCardDetails_DispatchMethodName, "TO BE DISPATCHED BY COURIER SERVICE TO THE CARDHOLDER’S MAILING ADDRESS (APPLICABLE FOR OVERSEAS ADDRESSES).", StringComparison.OrdinalIgnoreCase))
                        {
                            model.DebitCardDetails_CollectedBy = string.Empty;
                            model.DebitCardDetails_IdentityNumber = string.Empty;
                            model.DebitCardDetails_DeliveryAddress = string.Empty;
                            model.DebitCardDetails_OtherDeliveryAddress = string.Empty;
                        }
                        if (string.Equals(model.DebitCardDetails_DispatchMethodName, "TO BE DISPATCHED BY COURIER SERVICE TO THE FOLLOWING ADDRESS (APPLICABLE ONLY FOR OVERSEAS ADDRESSES)", StringComparison.OrdinalIgnoreCase))
                        {
                            model.DebitCardDetails_CollectedBy = string.Empty;
                            model.DebitCardDetails_IdentityNumber = string.Empty;
                        }
                        if (string.Equals(model.DebitCardDetails_DispatchMethodName, "TO BE COLLECTED FROM THE BANKING CENTRE BY (FULL NAME AND IDENTITY CARD/PASSPORT NO.)", StringComparison.OrdinalIgnoreCase))
                        {
                            model.DebitCardDetails_DeliveryAddress = string.Empty;
                            model.DebitCardDetails_OtherDeliveryAddress = string.Empty;
                        }
                    }
                    else if (isLegalEntity)
                    {
                        var result = ServiceHelper.GetDispatchMethod();
                        model.DebitCardDetails_DispatchMethodName = (result != null && result.Count > 0 && model.DebitCardDetails_DispatchMethod != null) ? result.FirstOrDefault(f => f.Value == model.DebitCardDetails_DispatchMethod.ToString()).Text : string.Empty;
                        if (string.Equals("TO BE COLLECTED FROM THE BANKING CENTRE BY THE AUTHORISED CARDHOLDER.", model.DebitCardDetails_DispatchMethodName, StringComparison.OrdinalIgnoreCase) || string.Equals("TO BE COLLECTED FROM THE BANKING CENTRE BY THE AUTHORISED CARDHOLDER.", model.DebitCardDetails_DispatchMethodName, StringComparison.OrdinalIgnoreCase) || string.Equals("TO BE DISPATCHED BY POST TO THE COMPANY’S MAILING ADDRESS (APPLICABLE FOR DOMESTIC AND OVERSEAS ADDRESSES).", model.DebitCardDetails_DispatchMethodName, StringComparison.OrdinalIgnoreCase))
                        {
                            model.DebitCardDetails_CollectedBy = string.Empty;
                            model.DebitCardDetails_IdentityNumber = string.Empty;
                            model.DebitCardDetails_DeliveryAddress = string.Empty;
                            model.DebitCardDetails_OtherDeliveryAddress = string.Empty;
                        }
                        if (string.Equals("TO BE COLLECTED FROM THE BANKING CENTRE BY (FULL NAME AND IDENTITY CARD/PASSPORT NO.)", model.DebitCardDetails_DispatchMethodName, StringComparison.OrdinalIgnoreCase))
                        {
                            model.DebitCardDetails_DeliveryAddress = string.Empty;
                            model.DebitCardDetails_OtherDeliveryAddress = string.Empty;
                        }
                        if (string.Equals("TO BE DISPATCHED BY COURIER SERVICE TO THE FOLLOWING ADDRESS (APPLICABLE FOR OVERSEAS ADDRESSES)", model.DebitCardDetails_DispatchMethodName, StringComparison.OrdinalIgnoreCase))
                        {
                            model.DebitCardDetails_CollectedBy = string.Empty;
                            model.DebitCardDetails_IdentityNumber = string.Empty;
                        }
                    }
                    debitcardDetails.SetValue("DebitCardDetails_DeliveryAddress", model.DebitCardDetails_DeliveryAddress);
                    debitcardDetails.SetValue("DebitCardDetails_OtherDeliveryAddress", model.DebitCardDetails_OtherDeliveryAddress);
                    debitcardDetails.SetValue("DebitCardDetails_CollectedBy", model.DebitCardDetails_CollectedBy);
                    debitcardDetails.SetValue("DebitCardDetails_IdentityNumber", model.DebitCardDetails_IdentityNumber);
                    debitcardDetails.SetValue("DebitCardDetails_CardHolderIdentityNumber", model.DebitCardDetails_CardHolderIdentityNumber);
                    debitcardDetails.SetValue("DebitCardDetails_CardHolderCountryOfIssue", model.DebitCardDetails_CardHolderCountryOfIssue);
                    debitcardDetails.SetValue("DebitCardDetails_DeliveryDetails", model.DebitCardDetails_DeliveryDetails);
                    debitcardDetails.SetValue("DebitCardDetails_Status", model.DebitCardDetails_Status);
                    debitcardDetails.SetValue("DebitCardDetails_CollectedByName", model.DebitCardDetails_CollectedByName);
                    debitcardDetails.Insert(accountsfoldernode_parent);
                    retVal.DebitCardDetailsID = ValidationHelper.GetInteger(debitcardDetails.GetValue("DebitCardDetailsID"), 0);
                }
            }
            retVal.DebitCardDetails_CardTypeName = ServiceHelper.GetName(ValidationHelper.GetString(model.DebitCardDetails_CardType, ""), Constants.CARD_TYPE);
            if (isLegalEntity)
            {
                var result = ServiceHelper.GetDispatchMethod();
                retVal.DebitCardDetails_DispatchMethodName = (result != null && result.Count > 0 && model.DebitCardDetails_DispatchMethod != null) ? result.FirstOrDefault(f => f.Value == model.DebitCardDetails_DispatchMethod.ToString()).Text : string.Empty;
            }
            else
            {
                var result = ServiceHelper.GetDispatchMethodIndividual();
                retVal.DebitCardDetails_DispatchMethodName = (result != null && result.Count > 0 && model.DebitCardDetails_DispatchMethod != null) ? result.FirstOrDefault(f => f.Value == model.DebitCardDetails_DispatchMethod.ToString()).Text : string.Empty;
            }
            retVal.DebitCardDetails_DispatchMethod = ValidationHelper.GetString(model.DebitCardDetails_DispatchMethod, "");
            retVal.DebitCardDetails_DeliveryAddress = ServiceHelper.GetName(ValidationHelper.GetString(model.DebitCardDetails_DeliveryAddress, ""), Constants.ADDRESS_TYPE);
            retVal.DebitCardDetails_OtherDeliveryAddress = model.DebitCardDetails_OtherDeliveryAddress;
            retVal.DebitCardDetails_Status = model.DebitCardDetails_Status;
            retVal.DebitCardDetails_StatusName = model.DebitCardDetails_Status == true ? "Complete" : "Pending";
            retVal.DebitCardDetails_DeliveryAddress = ValidationHelper.GetString(model.DebitCardDetails_DeliveryAddress, "");
            return retVal;
        }
        public static DebitCardDetailsViewModel UpdateDebitCardDetailsModel(DebitCardDetailsViewModel model, TreeNode treeNodeData, bool isLegalEntity)
        {
            DebitCardDetailsViewModel retVal = new DebitCardDetailsViewModel();
            string dispatch_Method = string.Empty;
            if (isLegalEntity)
            {
                dispatch_Method = Constants.DISPATCH_METHOD;
            }
            else
            {
                dispatch_Method = Constants.DISPATCH_METHOD_INDIVIDUAL;
            }
            if (model != null)
            {
                if (treeNodeData != null)
                {

                    treeNodeData.DocumentName = ValidationHelper.GetString(model.DebitCardDetails_FullName, "");
                    treeNodeData.SetValue("DebitCardDetails_CardType", model.DebitCardDetails_CardType);
                    treeNodeData.SetValue("AssociatedAccount", model.AssociatedAccount);
                    treeNodeData.SetValue("DebitCardDetails_CardholderName", model.DebitCardDetails_CardholderName);
                    treeNodeData.SetValue("DebitCardDetails_FullName", model.DebitCardDetails_FullName);
                    treeNodeData.SetValue("DebitCardDetails_MobileNumber_CountryCode", model.Country_Code);
                    treeNodeData.SetValue("DebitCardDetails_MobileNumber", model.DebitCardDetails_MobileNumber);
                    treeNodeData.SetValue("DebitCardDetails_CompanyNameAppearOnCard", model.DebitCardDetails_CompanyNameAppearOnCard);
                    treeNodeData.SetValue("DebitCardDetails_DispatchMethod", model.DebitCardDetails_DispatchMethod);
                    if (!isLegalEntity)
                    {
                        var result = ServiceHelper.GetDispatchMethodIndividual();
                        string dispatchMethodName = (result != null && result.Count > 0 && model.DebitCardDetails_DispatchMethod != null) ? result.FirstOrDefault(f => f.Value == model.DebitCardDetails_DispatchMethod.ToString()).Text : string.Empty;
                        if (string.Equals(dispatchMethodName, "TO BE COLLECTED FROM THE BANKING CENTRE.", StringComparison.OrdinalIgnoreCase) || string.Equals(dispatchMethodName, "TO BE DISPATCHED BY POST TO THE CARDHOLDER’S MAILING ADDRESS (APPLICABLE FOR DOMESTIC AND OVERSEAS ADDRESSES).", StringComparison.OrdinalIgnoreCase) || string.Equals(dispatchMethodName, "TO BE DISPATCHED BY COURIER SERVICE TO THE CARDHOLDER’S MAILING ADDRESS (APPLICABLE FOR OVERSEAS ADDRESSES).", StringComparison.OrdinalIgnoreCase))
                        {
                            model.DebitCardDetails_CollectedBy = string.Empty;
                            model.DebitCardDetails_IdentityNumber = string.Empty;
                            model.DebitCardDetails_DeliveryAddress = string.Empty;
                            model.DebitCardDetails_OtherDeliveryAddress = string.Empty;
                        }
                        if (string.Equals(dispatchMethodName, "TO BE DISPATCHED BY COURIER SERVICE TO THE FOLLOWING ADDRESS (APPLICABLE ONLY FOR OVERSEAS ADDRESSES)", StringComparison.OrdinalIgnoreCase))
                        {
                            model.DebitCardDetails_CollectedBy = string.Empty;
                            model.DebitCardDetails_IdentityNumber = string.Empty;
                        }
                        if (string.Equals(dispatchMethodName, "TO BE COLLECTED FROM THE BANKING CENTRE BY (FULL NAME AND IDENTITY CARD/PASSPORT NO.)", StringComparison.OrdinalIgnoreCase))
                        {
                            model.DebitCardDetails_DeliveryAddress = string.Empty;
                            model.DebitCardDetails_OtherDeliveryAddress = string.Empty;
                        }
                    }
                    else if (isLegalEntity)
                    {
                        var result = ServiceHelper.GetDispatchMethod();
                        string dispatchMethodName = (result != null && result.Count > 0 && model.DebitCardDetails_DispatchMethod != null) ? result.FirstOrDefault(f => f.Value == model.DebitCardDetails_DispatchMethod.ToString()).Text : string.Empty;
                        if (string.Equals("TO BE COLLECTED FROM THE BANKING CENTRE BY THE AUTHORISED CARDHOLDER.", dispatchMethodName, StringComparison.OrdinalIgnoreCase) || string.Equals("TO BE COLLECTED FROM THE BANKING CENTRE BY THE AUTHORISED CARDHOLDER.", dispatchMethodName, StringComparison.OrdinalIgnoreCase) || string.Equals("TO BE DISPATCHED BY POST TO THE COMPANY’S MAILING ADDRESS (APPLICABLE FOR DOMESTIC AND OVERSEAS ADDRESSES).", dispatchMethodName, StringComparison.OrdinalIgnoreCase))
                        {
                            model.DebitCardDetails_CollectedBy = string.Empty;
                            model.DebitCardDetails_IdentityNumber = string.Empty;
                            model.DebitCardDetails_DeliveryAddress = string.Empty;
                            model.DebitCardDetails_OtherDeliveryAddress = string.Empty;
                        }
                        if (string.Equals("TO BE COLLECTED FROM THE BANKING CENTRE BY (FULL NAME AND IDENTITY CARD/PASSPORT NO.)", dispatchMethodName, StringComparison.OrdinalIgnoreCase))
                        {
                            model.DebitCardDetails_DeliveryAddress = string.Empty;
                            model.DebitCardDetails_OtherDeliveryAddress = string.Empty;
                        }
                        if (string.Equals("TO BE DISPATCHED BY COURIER SERVICE TO THE FOLLOWING ADDRESS (APPLICABLE FOR OVERSEAS ADDRESSES)", dispatchMethodName, StringComparison.OrdinalIgnoreCase))
                        {
                            model.DebitCardDetails_CollectedBy = string.Empty;
                            model.DebitCardDetails_IdentityNumber = string.Empty;
                        }
                    }

                    treeNodeData.SetValue("DebitCardDetails_DeliveryAddress", model.DebitCardDetails_DeliveryAddress);
                    treeNodeData.SetValue("DebitCardDetails_OtherDeliveryAddress", model.DebitCardDetails_OtherDeliveryAddress);
                    treeNodeData.SetValue("DebitCardDetails_CollectedBy", model.DebitCardDetails_CollectedBy);
                    treeNodeData.SetValue("DebitCardDetails_IdentityNumber", model.DebitCardDetails_IdentityNumber);
                    treeNodeData.SetValue("DebitCardDetails_CardHolderIdentityNumber", model.DebitCardDetails_CardHolderIdentityNumber);
                    treeNodeData.SetValue("DebitCardDetails_CardHolderCountryOfIssue", model.DebitCardDetails_CardHolderCountryOfIssue);
                    treeNodeData.SetValue("DebitCardDetails_DeliveryDetails", model.DebitCardDetails_DeliveryDetails);
                    treeNodeData.SetValue("DebitCardDetails_Status", model.DebitCardDetails_Status);
                    treeNodeData.SetValue("DebitCardDetails_CollectedByName", model.DebitCardDetails_CollectedByName);
                    treeNodeData.NodeAlias = treeNodeData.DocumentName;
                    treeNodeData.Update();

                }
            }
            retVal.DebitCardDetails_CardTypeName = ServiceHelper.GetName(ValidationHelper.GetString(model.DebitCardDetails_CardType, ""), Constants.CARD_TYPE);
            if (isLegalEntity)
            {
                var result = ServiceHelper.GetDispatchMethod();
                retVal.DebitCardDetails_DispatchMethodName = (result != null && result.Count > 0 && model.DebitCardDetails_DispatchMethod != null) ? result.FirstOrDefault(f => f.Value == model.DebitCardDetails_DispatchMethod.ToString()).Text : string.Empty;
            }
            else
            {
                var result = ServiceHelper.GetDispatchMethodIndividual();
                retVal.DebitCardDetails_DispatchMethodName = (result != null && result.Count > 0 && model.DebitCardDetails_DispatchMethod != null) ? result.FirstOrDefault(f => f.Value == model.DebitCardDetails_DispatchMethod.ToString()).Text : string.Empty;
            }
            retVal.DebitCardDetails_Status = model.DebitCardDetails_Status;
            retVal.DebitCardDetails_StatusName = model.DebitCardDetails_Status == true ? "Complete" : "Pending";
            retVal.DebitCardDetails_DeliveryAddress = ValidationHelper.GetString(model.DebitCardDetails_DeliveryAddress, "");
            return retVal;
        }

        public static List<DebitCardDetailsViewModel> GetDebitCardDetailsByApplicationID(int applicationID)
        {
            List<DebitCardDetailsViewModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (applicationID > 0)
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(ApplicationDetails.CLASS_NAME)
                    .WhereEquals("ApplicationDetailsID", applicationID)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode debitCardDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _DebitCardDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        debitCardDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _DebitCardDetailsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (debitCardDetailsRoot != null)
                        {
                            List<TreeNode> debitCardDetailsNodes = debitCardDetailsRoot.Children.Where(u => u.ClassName == DebitCardDetails.CLASS_NAME).ToList();

                            if (debitCardDetailsNodes != null && debitCardDetailsNodes.Count > 0)
                            {
                                retVal = new List<DebitCardDetailsViewModel>();
                                debitCardDetailsNodes.ForEach(t =>
                                {
                                    DebitCardDetails debitCardDetails = DebitCardDetailsProvider.GetDebitCardDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (debitCardDetails != null)
                                    {
                                        DebitCardDetailsViewModel debitCardDetailsModel = BindDebitCardDetailsModel(debitCardDetails);
                                        if (debitCardDetailsModel != null)
                                        {
                                            retVal.Add(debitCardDetailsModel);
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
        private static DebitCardDetailsViewModel BindDebitCardDetailsModel(DebitCardDetails item)
        {
            DebitCardDetailsViewModel retVal = null;

            if (item != null)
            {

                var country = ServiceHelper.GetCountriesWithID();
                var cardType = ServiceHelper.GetCardType();
                var dispatchMethod = ServiceHelper.GetDispatchMethod();
                retVal = new DebitCardDetailsViewModel()
                {
                    DebitCardDetailsID = item.DebitCardDetailsID,
                    DebitCardDetails_FullName = item.DebitCardDetails_FullName,
                    DebitCardDetails_CardType = item.DebitCardDetails_CardType.ToString(),
                    AssociatedAccount = item.AssociatedAccount,
                    AssociatedAccountName = item.AssociatedAccount,
                    DebitCardDetails_DispatchMethod = item.DebitCardDetails_DispatchMethod.ToString(),

                    DebitCardDetails_CardTypeName = (cardType != null && cardType.Count > 0 && item.DebitCardDetails_CardType != null) ? cardType.FirstOrDefault(f => f.Value == item.DebitCardDetails_CardType.ToString())?.Text : string.Empty,
                    DebitCardDetails_DispatchMethodName = item.DebitCardDetails_DispatchMethod.ToString(),
                    DebitCardDetails_IdentityNumber = item.DebitCardDetails_IdentityNumber,
                    DebitCardDetails_CardHolderIdentityNumber = item.DebitCardDetails_CardHolderIdentityNumber,
                    DebitCardDetails_CardHolderCountryOfIssue = item.DebitCardDetails_CardHolderCountryOfIssue.ToString(),
                    DebitCardDetails_DeliveryDetails = item.DebitCardDetails_DeliveryDetails,
                    DebitCardDetails_CardholderName = item.DebitCardDetails_CardholderName,
                    Country_Code = item.DebitCardDetails_MobileNumber_CountryCode,
                    DebitCardDetails_MobileNumber = item.DebitCardDetails_MobileNumber,
                    DebitCardDetails_CompanyNameAppearOnCard = item.DebitCardDetails_CompanyNameAppearOnCard,
                    DebitCardDetails_DeliveryAddress = item.DebitCardDetails_DeliveryAddress.ToString(),
                    DebitCardDetails_OtherDeliveryAddress = item.DebitCardDetails_OtherDeliveryAddress,
                    DebitCardDetails_CollectedBy = item.DebitCardDetails_CollectedBy,
                    DebitCardDetails_Status = item.DebitCardDetails_Status,
                    DebitCardDetails_StatusName = item.DebitCardDetails_Status == true ? "COMPLETE" : "PENDING",
                    DebitCardDetails_CollectedByName = item.DebitCardDetails_CollectedByName,
                };
            }
            return retVal;
        }

        public static bool GenerateDebitCardDetails(bool isAuthorizeCardHoderSelect, int applicationID, DebitCardDetailsRepository debitCardDetailsRepository, ApplicationsRepository applicationsRepository
            , string cardHolderCountryOfIssue, string cardHolderIdentityNumber, string cardType, string cardHolder, string fullName, bool isLegalEntity)
        {
			var DebitCardDetails = DebitCardeDetailsProcess.GetDebitCardDetailsByApplicationID(applicationID);
            if (DebitCardDetails != null && DebitCardDetails.Any())
            {
				var isCardRecordExist = DebitCardDetails.Where(x => string.Equals(x.DebitCardDetails_CardType, cardType, StringComparison.OrdinalIgnoreCase)
							 && string.Equals(x.DebitCardDetails_CardholderName, cardHolder, StringComparison.OrdinalIgnoreCase));

				if (isCardRecordExist != null && isCardRecordExist.Any())
                {
                    foreach(var item in isCardRecordExist)
                    {
						debitCardDetailsRepository.GetDebitsCardsDetailsByID(item.DebitCardDetailsID).DeleteAllCultures();
					}
                }
			}
            if (isAuthorizeCardHoderSelect)
            {
                DebitCardDetailsViewModel debitCardDetailsViewModel = new DebitCardDetailsViewModel();
                //debitCardDetailsViewModel.DebitCardDetails_CardHolderCountryOfIssue = cardHolderCountryOfIssue;
                //debitCardDetailsViewModel.DebitCardDetails_CardHolderIdentityNumber = cardHolderIdentityNumber;
                debitCardDetailsViewModel.DebitCardDetails_CardType = cardType;
                debitCardDetailsViewModel.DebitCardDetails_CardholderName = cardHolder;
                debitCardDetailsViewModel.DebitCardDetails_FullName = fullName;
                debitCardDetailsViewModel.DebitCardDetails_Status = false;

                var applicatioData = applicationsRepository.GetApplicationDetailsByID(applicationID);
                var successData = DebitCardeDetailsProcess.SaveDebitCardDetailsModel(debitCardDetailsViewModel, applicatioData, isLegalEntity); 
            }
			return true;
        }
    }
}
