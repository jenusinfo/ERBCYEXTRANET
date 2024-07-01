using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Applications.Accounts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class AccountsProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";
        private static readonly string _AccountsFolderName = "Accounts";
        private static readonly string _AccountDocumentName = "Account";
        public static List<AccountsDetailsViewModel> GetAccounts(IEnumerable<TreeNode> treeNode)
        {
            List<AccountsDetailsViewModel> retVal = new List<AccountsDetailsViewModel>();
            foreach (var item in treeNode)
            {
                AccountsDetailsViewModel accountsDetailsViewModel = new AccountsDetailsViewModel();
                accountsDetailsViewModel.AccountsID = ValidationHelper.GetInteger(item.GetValue("AccountsID"), 0);
                accountsDetailsViewModel.Accounts_AccountType = ValidationHelper.GetString(item.GetValue("Accounts_AccountType"), "");

                accountsDetailsViewModel.Accounts_Currency = ValidationHelper.GetString(item.GetValue("Accounts_Currency"), "");
                accountsDetailsViewModel.Accounts_StatementFrequency = ValidationHelper.GetString(item.GetValue("Accounts_StatementFrequency"), "");
                accountsDetailsViewModel.Accounts_AccountTypeName = ServiceHelper.GetName(ValidationHelper.GetString(accountsDetailsViewModel.Accounts_AccountType, ""), Constants.Accounts_AccountType);
                accountsDetailsViewModel.Accounts_CurrencyName = ServiceHelper.GetName(ValidationHelper.GetString(accountsDetailsViewModel.Accounts_Currency, ""), Constants.Accounts_Currency);
                accountsDetailsViewModel.Accounts_StatementFrequencyName = ServiceHelper.GetName(ValidationHelper.GetString(accountsDetailsViewModel.Accounts_StatementFrequency, ""), Constants.Accounts_StatementFrequency);
                accountsDetailsViewModel.Accounts_Status = ValidationHelper.GetBoolean(item.GetValue("Accounts_Status"), false);
                accountsDetailsViewModel.Account_Status_Name = accountsDetailsViewModel.Accounts_Status == true ? "Complete" : "Pending";
                retVal.Add(accountsDetailsViewModel);
            }
            return retVal;
        }

        public static AccountsDetailsViewModel SaveAccountsModel(AccountsDetailsViewModel model, TreeNode treeNodeData)
        {
            AccountsDetailsViewModel retVal = new AccountsDetailsViewModel();

            if (model != null)
            {
                if (treeNodeData != null)
                {
                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                    TreeNode accountsfoldernode_parent = tree.SelectNodes()
                        .Path(treeNodeData.NodeAliasPath + "/Accounts")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                    if (accountsfoldernode_parent == null)
                    {

                        accountsfoldernode_parent = TreeNode.New("CMS.Folder", tree);
                        accountsfoldernode_parent.DocumentName = "Accounts";
                        accountsfoldernode_parent.DocumentCulture = "en-US";
                        accountsfoldernode_parent.Insert(treeNodeData);
                    }
                    TreeNode identification = TreeNode.New("Eurobank.Accounts", tree);
                    //identification.DocumentName = ServiceHelper.GetName(ValidationHelper.GetString(model.Accounts_AccountType, ""), "/Lookups/General/TYPE-OF-ACCOUNTS");
                    identification.DocumentName = _AccountDocumentName;
                    identification.SetValue("Accounts_AccountType", model.Accounts_AccountType);
                    identification.SetValue("Accounts_Currency", model.Accounts_Currency);
                    identification.SetValue("Accounts_StatementFrequency", model.Accounts_StatementFrequency);
                    identification.SetValue("Accounts_Status", model.Accounts_Status);
                    identification.Insert(accountsfoldernode_parent);

                    retVal.AccountsID = ValidationHelper.GetInteger(identification.GetValue("AccountsID"), 0);
                    retVal.Accounts_Currency = ValidationHelper.GetString(identification.GetValue("Accounts_Currency"), "");
                    retVal.Accounts_StatementFrequency = ValidationHelper.GetString(identification.GetValue("Accounts_StatementFrequency"), "");
                }
            }
            retVal.Accounts_AccountTypeName = ServiceHelper.GetName(ValidationHelper.GetString(model.Accounts_AccountType, ""), Constants.Accounts_AccountType);
            retVal.Accounts_CurrencyName = ServiceHelper.GetName(ValidationHelper.GetString(model.Accounts_Currency, ""), Constants.Accounts_Currency);
            retVal.Accounts_StatementFrequencyName = ServiceHelper.GetName(ValidationHelper.GetString(model.Accounts_StatementFrequency, ""), Constants.Accounts_StatementFrequency);
            retVal.Account_Status_Name = model.Accounts_Status == true ? "Complete" : "Pending";
            retVal.Accounts_AccountType = model.Accounts_AccountType;

            return retVal;
        }
        public static AccountsDetailsViewModel UpdateAccountsModel(AccountsDetailsViewModel model, TreeNode treeNodeData)
        {
            AccountsDetailsViewModel retVal = new AccountsDetailsViewModel();
            treeNodeData.SetValue("Accounts_AccountType", model.Accounts_AccountType);
            treeNodeData.SetValue("Accounts_Currency", model.Accounts_Currency);
            treeNodeData.SetValue("Accounts_StatementFrequency", model.Accounts_StatementFrequency);
            treeNodeData.SetValue("Accounts_Status", model.Accounts_Status);
            treeNodeData.Update();
            retVal.Accounts_AccountTypeName = ServiceHelper.GetName(ValidationHelper.GetString(model.Accounts_AccountType, ""), Constants.Accounts_AccountType);
            retVal.Accounts_CurrencyName = ServiceHelper.GetName(ValidationHelper.GetString(model.Accounts_Currency, ""), Constants.Accounts_Currency);
            retVal.Accounts_StatementFrequencyName = ServiceHelper.GetName(ValidationHelper.GetString(model.Accounts_StatementFrequency, ""), Constants.Accounts_StatementFrequency);
            retVal.Account_Status_Name = model.Accounts_Status == true ? "Complete" : "Pending";
            retVal.AccountsID = model.AccountsID;
            return retVal;
        }

        public static List<AccountsDetailsViewModel> GetAccountsByApplicationID(int applicationID)
        {
            List<AccountsDetailsViewModel> retVal = null;

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
                    TreeNode accountsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _AccountsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        accountsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _AccountsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (accountsRoot != null)
                        {
                            List<TreeNode> accountsNodes = accountsRoot.Children.Where(u => u.ClassName == Accounts.CLASS_NAME).ToList();

                            if (accountsNodes != null && accountsNodes.Count > 0)
                            {
                                retVal = new List<AccountsDetailsViewModel>();
                                accountsNodes.ForEach(t =>
                                {
                                    Accounts accounts = AccountsProvider.GetAccounts(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (accounts != null)
                                    {
                                        AccountsDetailsViewModel accountsModel = BindAccountsModel(accounts);
                                        if (accountsModel != null)
                                        {
                                            retVal.Add(accountsModel);
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

        public static List<SelectListItem> GetDebitCardAccountsByApplicationID(int applicationID)
        {
            List<SelectListItem> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if(applicationID > 0)
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(ApplicationDetails.CLASS_NAME)
                    .WhereEquals("ApplicationDetailsID", applicationID)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if(applicationDetailsNode != null)
                {
                    TreeNode accountsRoot = null;
                    if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _AccountsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        accountsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _AccountsFolderName, StringComparison.OrdinalIgnoreCase));

                        if(accountsRoot != null)
                        {
                            List<TreeNode> accountsNodes = accountsRoot.Children.Where(u => u.ClassName == Accounts.CLASS_NAME).ToList();

                            if(accountsNodes != null && accountsNodes.Count > 0)
                            {
                                retVal = new List<SelectListItem>();
                                accountsNodes.ForEach(t =>
                                {
                                    Accounts accounts = AccountsProvider.GetAccounts(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if(accounts != null)
                                    {
                                        AccountsDetailsViewModel accountsModel = BindAccountsModel(accounts);
                                        if(accountsModel != null)
                                        {
                                            string currencyName = ServiceHelper.GetName(ValidationHelper.GetString(accounts.Accounts_Currency, ""), Constants.Accounts_Currency);
                                            if(string.Equals(currencyName, "EUR", StringComparison.OrdinalIgnoreCase) || string.Equals(currencyName, "USD", StringComparison.OrdinalIgnoreCase))
											{
                                                string accountText = ServiceHelper.GetName(ValidationHelper.GetString(accounts.Accounts_AccountType, ""), Constants.Accounts_AccountType) + " - " + currencyName;
                                                retVal.Add(new SelectListItem() { Text = accountText.ToUpper(), Value = t.NodeGUID.ToString() });
                                            }
                                            
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

        private static AccountsDetailsViewModel BindAccountsModel(Accounts item)
        {
            AccountsDetailsViewModel retVal = null;

            if (item != null)
            {
                //var accountType = ServiceHelper.GetAssociatedAccountType();
                //var currency = ServiceHelper.GetCurrency();
                //var statementFrequency = ServiceHelper.GetSTATEMENTFREQUENCY();
                retVal = new AccountsDetailsViewModel()
                {
                    AccountsID = item.AccountsID,
                    Accounts_AccountType = item.Accounts_AccountType.ToString(),
                    Accounts_Currency = item.Accounts_Currency.ToString(),
                    Accounts_StatementFrequency = item.Accounts_StatementFrequency.ToString(),

                    Accounts_AccountTypeName = ServiceHelper.GetName(ValidationHelper.GetString(item.Accounts_AccountType, ""), Constants.Accounts_AccountType),

                    Accounts_CurrencyName = ServiceHelper.GetName(ValidationHelper.GetString(item.Accounts_Currency, ""), Constants.Accounts_Currency),
                    Accounts_StatementFrequencyName = ServiceHelper.GetName(ValidationHelper.GetString(item.Accounts_StatementFrequency, ""), Constants.Accounts_StatementFrequency),

                    //Accounts_AccountTypeName = (accountType != null && accountType.Count > 0 && item.Accounts_AccountType != null) ? accountType.FirstOrDefault(f => f.Value == item.Accounts_AccountType.ToString()).Text : string.Empty,
                    //Accounts_CurrencyName = (currency != null && currency.Count > 0 && item.Accounts_Currency != null) ? currency.FirstOrDefault(f => f.Value == item.Accounts_Currency.ToString()).Text : string.Empty,
                    //Accounts_StatementFrequencyName = (statementFrequency != null && statementFrequency.Count > 0 && item.Accounts_StatementFrequency != null) ? statementFrequency.FirstOrDefault(f => f.Value == item.Accounts_StatementFrequency.ToString()).Text : string.Empty,

                    Accounts_Status = item.Accounts_Status,
                    Account_Status_Name = item.Accounts_Status == true ? "COMPLETE" : "PENDING",


                };
            }
            return retVal;
        }
    }
}
