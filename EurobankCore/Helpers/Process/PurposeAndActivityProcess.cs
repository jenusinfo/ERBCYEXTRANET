using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class PurposeAndActivityProcess
    {
        //private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

        public static PurposeAndActivityModel GetPurposeAndActivityModel(string applicationNumber)
        {
            PurposeAndActivityModel retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (!string.IsNullOrEmpty(applicationNumber))
            {
                TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

                if (applicationDetailsNode != null)
                {
                    retVal = new PurposeAndActivityModel();
                    var signatureMandateGroup = ServiceHelper.SignatureMandateTypeGroup();
                    string applicationtype = ServiceHelper.GetName(ValidationHelper.GetString(applicationDetailsNode.GetValue("ApplicationDetails_ApplicationType"), ""), "/Lookups/General/APPLICATION-TYPE");

                    retVal.ReasonForOpeningTheAccountGroup = ControlBinder.BindMultiselectDropdownItems(ServiceHelper.GetReasonsForOpeningTheAccount(), null, '\0');
                    retVal.ExpectedNatureOfInAndOutTransactionGroup = ControlBinder.BindMultiselectDropdownItems(ServiceHelper.GetExpectedNatureOfInAndOutTransactionsIndividual(), null, '\0');
                    retVal.ExpectedFrequencyOfInAndOutTransactionGroup = ControlBinder.BindRadioButtonGroupItems(ServiceHelper.GetExpectedFrequencyOfInAndOutTransactionsCheckGroup(), null);
                    if (string.Equals(applicationtype, "Joint Individual", StringComparison.OrdinalIgnoreCase))
                    {
                        string seletedValue = signatureMandateGroup.Any(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)) ? signatureMandateGroup.FirstOrDefault(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)).Value : null;
                        retVal.SignatureMandateTypeGroup = ControlBinder.BindRadioButtonGroupItems(signatureMandateGroup, seletedValue);
                    }
                    else
                    {
                        signatureMandateGroup = signatureMandateGroup.Where(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)).ToList();
                        string seletedValue = signatureMandateGroup.Any(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)) ? signatureMandateGroup.FirstOrDefault(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)).Value : null;
                        retVal.SignatureMandateTypeGroup = ControlBinder.BindRadioButtonGroupItems(signatureMandateGroup, seletedValue);
                    }

                    TreeNode purposeAndActivityNode = applicationDetailsNode.Children.Where(u => u.ClassName == "Eurobank.PurposeAndActivity").FirstOrDefault();

                    if (purposeAndActivityNode != null)
                    {
                        PurposeAndActivity purposeAndActivity = PurposeAndActivityProvider.GetPurposeAndActivity(purposeAndActivityNode.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                        if (purposeAndActivity != null)
                        {
                            retVal.ReasonForOpeningTheAccountGroup = ControlBinder.BindMultiselectDropdownItems(ServiceHelper.GetReasonsForOpeningTheAccount(), purposeAndActivity.PurposeAndActivity_ReasonForOpeningTheAccount, '|');
                            retVal.ExpectedNatureOfInAndOutTransactionGroup = ControlBinder.BindMultiselectDropdownItems(ServiceHelper.GetExpectedNatureOfInAndOutTransactions(), purposeAndActivity.PurposeAndActivity_ExpectedNatureOfIncomingAndOutgoingTransactions, '|');
                            retVal.ExpectedFrequencyOfInAndOutTransactionGroup = ControlBinder.BindRadioButtonGroupItems(ServiceHelper.GetExpectedFrequencyOfInAndOutTransactionsCheckGroup(), purposeAndActivity.PurposeAndActivity_ExpectedFrequencyOfIncomingAndOutgoingTransactions);
                            //retVal.SignatureMandateTypeGroup = ControlBinder.BindRadioButtonGroupItems(ServiceHelper.SignatureMandateTypeGroup(), purposeAndActivity.PurposeAndActivity_SignatureMandateType);
                            if (string.Equals(applicationtype, "Joint Individual", StringComparison.OrdinalIgnoreCase))
                            {
                                string seletedValue = signatureMandateGroup.Any(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)) ? signatureMandateGroup.FirstOrDefault(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)).Value : null;
                                if (!string.IsNullOrEmpty(purposeAndActivity.PurposeAndActivity_SignatureMandateType))
                                {
                                    seletedValue = purposeAndActivity.PurposeAndActivity_SignatureMandateType;
                                }
                                retVal.SignatureMandateTypeGroup = ControlBinder.BindRadioButtonGroupItems(signatureMandateGroup, seletedValue);
                            }
                            else
                            {
                                signatureMandateGroup = signatureMandateGroup.Where(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)).ToList();
                                string seletedValue = signatureMandateGroup.Any(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)) ? signatureMandateGroup.FirstOrDefault(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)).Value : null;
                                if (!string.IsNullOrEmpty(purposeAndActivity.PurposeAndActivity_SignatureMandateType))
                                {
                                    seletedValue = purposeAndActivity.PurposeAndActivity_SignatureMandateType;
                                }
                                retVal.SignatureMandateTypeGroup = ControlBinder.BindRadioButtonGroupItems(signatureMandateGroup, seletedValue);
                            }

                            retVal.ExpectedIncomingAmount = purposeAndActivity.PurposeAndActivity_ExpectedIncomingAmount;
                        }
                    }
                }



            }

            return retVal;
        }

        public static PurposeAndActivityModel GetPurposeAndActivityModelLegal(string applicationNumber, string applicantTypeEntityGuid)
        {
            PurposeAndActivityModel retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (!string.IsNullOrEmpty(applicationNumber))
            {
                TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

                if (applicationDetailsNode != null)
                {
                    retVal = new PurposeAndActivityModel();
                    var signatureMandateGroup = ServiceHelper.SignatureMandateTypeGroup();
                    string applicationtype = ServiceHelper.GetName(ValidationHelper.GetString(applicationDetailsNode.GetValue("ApplicationDetails_ApplicationType"), ""), "/Lookups/General/APPLICATION-TYPE");

                    retVal.ReasonForOpeningTheAccountGroup = ControlBinder.BindMultiselectDropdownItems(GetResonForOpeningAccountLegal(applicantTypeEntityGuid), null, '\0');
                    retVal.ExpectedNatureOfInAndOutTransactionGroup = ControlBinder.BindMultiselectDropdownItems(ServiceHelper.GetExpectedNatureOfInAndOutTransactions(), null, '\0');
                    retVal.ExpectedFrequencyOfInAndOutTransactionGroup = ControlBinder.BindRadioButtonGroupItems(ServiceHelper.GetExpectedFrequencyOfInAndOutTransactionsCheckGroup(), null);
                    if (string.Equals(applicationtype, "Joint Individual", StringComparison.OrdinalIgnoreCase))
                    {
                        string seletedValue = signatureMandateGroup.Any(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)) ? signatureMandateGroup.FirstOrDefault(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)).Value : null;
                        retVal.SignatureMandateTypeGroup = ControlBinder.BindRadioButtonGroupItems(signatureMandateGroup, seletedValue);
                    }
                    else
                    {
                        signatureMandateGroup = signatureMandateGroup.Where(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)).ToList();
                        string seletedValue = signatureMandateGroup.Any(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)) ? signatureMandateGroup.FirstOrDefault(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)).Value : null;
                        retVal.SignatureMandateTypeGroup = ControlBinder.BindRadioButtonGroupItems(signatureMandateGroup, seletedValue);
                    }

                    TreeNode purposeAndActivityNode = applicationDetailsNode.Children.Where(u => u.ClassName == "Eurobank.PurposeAndActivity").FirstOrDefault();

                    if (purposeAndActivityNode != null)
                    {
                        PurposeAndActivity purposeAndActivity = PurposeAndActivityProvider.GetPurposeAndActivity(purposeAndActivityNode.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                        if (purposeAndActivity != null)
                        {
                            retVal.ReasonForOpeningTheAccountGroup = ControlBinder.BindMultiselectDropdownItems(GetResonForOpeningAccountLegal(applicantTypeEntityGuid), purposeAndActivity.PurposeAndActivity_ReasonForOpeningTheAccount, '|');
                            retVal.ExpectedNatureOfInAndOutTransactionGroup = ControlBinder.BindMultiselectDropdownItems(ServiceHelper.GetExpectedNatureOfInAndOutTransactions(), purposeAndActivity.PurposeAndActivity_ExpectedNatureOfIncomingAndOutgoingTransactions, '|');
                            retVal.ExpectedFrequencyOfInAndOutTransactionGroup = ControlBinder.BindRadioButtonGroupItems(ServiceHelper.GetExpectedFrequencyOfInAndOutTransactionsCheckGroup(), purposeAndActivity.PurposeAndActivity_ExpectedFrequencyOfIncomingAndOutgoingTransactions);
                            //retVal.SignatureMandateTypeGroup = ControlBinder.BindRadioButtonGroupItems(ServiceHelper.SignatureMandateTypeGroup(), purposeAndActivity.PurposeAndActivity_SignatureMandateType);
                            if (string.Equals(applicationtype, "Joint Individual", StringComparison.OrdinalIgnoreCase))
                            {
                                string seletedValue = signatureMandateGroup.Any(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)) ? signatureMandateGroup.FirstOrDefault(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)).Value : null;
                                if (!string.IsNullOrEmpty(purposeAndActivity.PurposeAndActivity_SignatureMandateType))
                                {
                                    seletedValue = purposeAndActivity.PurposeAndActivity_SignatureMandateType;
                                }
                                retVal.SignatureMandateTypeGroup = ControlBinder.BindRadioButtonGroupItems(signatureMandateGroup, seletedValue);
                            }
                            else
                            {
                                signatureMandateGroup = signatureMandateGroup.Where(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)).ToList();
                                string seletedValue = signatureMandateGroup.Any(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)) ? signatureMandateGroup.FirstOrDefault(f => string.Equals(f.Label, "Any one alone can sign", StringComparison.OrdinalIgnoreCase)).Value : null;
                                if (!string.IsNullOrEmpty(purposeAndActivity.PurposeAndActivity_SignatureMandateType))
                                {
                                    seletedValue = purposeAndActivity.PurposeAndActivity_SignatureMandateType;
                                }
                                retVal.SignatureMandateTypeGroup = ControlBinder.BindRadioButtonGroupItems(signatureMandateGroup, seletedValue);
                            }

                            retVal.ExpectedIncomingAmount = purposeAndActivity.PurposeAndActivity_ExpectedIncomingAmount;
                        }
                    }
                }



            }

            return retVal;
        }

        public static PurposeAndActivityModel SavePurposeAndActivityModel(string applicationNumber, PurposeAndActivityModel purposeAndActivityModel)
        {
            PurposeAndActivityModel retVal = null;
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            if (!string.IsNullOrEmpty(applicationNumber) && purposeAndActivityModel != null)
            {
                TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

                if (applicationDetailsNode != null)
                {
                    TreeNode purposeAndActivityNode = applicationDetailsNode.Children.Where(u => u.ClassName == "Eurobank.PurposeAndActivity").FirstOrDefault();

                    if (purposeAndActivityNode != null)
                    {
                        PurposeAndActivity purposeAndActivity = PurposeAndActivityProvider.GetPurposeAndActivity(purposeAndActivityNode.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                        if (purposeAndActivity != null)
                        {
                            purposeAndActivity = BindPurposeAndActivity(purposeAndActivityModel, purposeAndActivity);
                            if (purposeAndActivity != null)
                            {
                                purposeAndActivity.Update();
                                retVal = purposeAndActivityModel;
                            }
                        }
                    }
                    else
                    {
                        PurposeAndActivity purposeAndActivity = BindPurposeAndActivity(purposeAndActivityModel, null);
                        if (purposeAndActivity != null)
                        {
                            purposeAndActivity.DocumentName = "Purpose And Activity";
                            purposeAndActivity.NodeName = "Purpose And Activity";
                            purposeAndActivity.Insert(applicationDetailsNode);
                            retVal = purposeAndActivityModel;
                        }
                    }
                }
            }

            return retVal;
        }

        //public static List<SelectListItem> GetResonForOpeningAccountLegal(string aplicantEntityTypeGuid)
        //{
        //    List<SelectListItem> retVal = null;

        //    List<SelectListItem> reasonsForOpeningAccountMaster = ServiceHelper.GetReasonsForOpeningTheAccount();

        //    var resonForOpeningConditinalValues = ReasonForOpeningAccountProvider.GetReasonForOpeningAccounts();
        //    if (!string.IsNullOrEmpty(aplicantEntityTypeGuid) && resonForOpeningConditinalValues != null && resonForOpeningConditinalValues.Count > 0)
        //    {
        //        retVal = new List<SelectListItem>();
        //        reasonsForOpeningAccountMaster.ForEach(y =>
        //        {
        //            if (resonForOpeningConditinalValues.Any(l => string.Equals(l.ReasonForOpeningAcc, y.Value, StringComparison.OrdinalIgnoreCase) && l.Company_EntityType.Contains(aplicantEntityTypeGuid)))
        //            {
        //                retVal.Add(y);
        //            }
        //        });
        //    }
        //    else
        //    {
        //        retVal = reasonsForOpeningAccountMaster;
        //    }

        //    return retVal;
        //}
        public static List<SelectListItem> GetResonForOpeningAccountLegal(string aplicantEntityTypeGuid)
        {
            List<SelectListItem> retVal = null;
            if (!string.IsNullOrEmpty(aplicantEntityTypeGuid))
            {
                List<string> mylist = null;
                string EntityName = ServiceHelper.GetName(aplicantEntityTypeGuid, Constants.COMPANY_ENTITY_TYPE);

                if (string.Equals(EntityName, "Provident Fund", StringComparison.OrdinalIgnoreCase) || string.Equals(EntityName, "Pension Fund", StringComparison.OrdinalIgnoreCase))
                {
                    mylist = new List<string>(new string[] { "DEPOSITS / SAVINGS", "INVESTMENT INCOME", "INCOME FROM INVESTMENTS", "INVESTMENT PURPOSES", "GRANTING LOANS TO MEMBERS", "RECEIVE MEMBER'S CONTRIBUTIONS" });
                }
                else
                {
                    mylist = new List<string>(new string[] { "COMMERCIAL PAYMENTS", "CREDIT FACILITIES", "DEPOSITS / SAVINGS", "RECEIVE DIVIDENDS", "INTRA GROUP FINANCING", "INVESTMENT INCOME", "INCOME FROM INVESTMENTS", "INVESTMENT PURPOSES", "TRADE FINANCE", "PAYROLL" });
                }
                 retVal = ServiceHelper.GetReasonsForOpeningTheAccount().Where(x => mylist.Contains(x.Text.Trim())).ToList();
            }
            else
            {
              retVal = ServiceHelper.GetReasonsForOpeningTheAccount();
            }
            return retVal;
        }
        #region Bind Methods

        private static PurposeAndActivity BindPurposeAndActivity(PurposeAndActivityModel item, PurposeAndActivity purposeAndActivity)
        {
            PurposeAndActivity retVal = new PurposeAndActivity();

            if (purposeAndActivity == null && item == null)
            {
                retVal = null;
            }

            if (purposeAndActivity != null)
            {
                retVal = purposeAndActivity;
            }

            if (item != null)
            {
                retVal.PurposeAndActivity_ReasonForOpeningTheAccount = (item.ReasonForOpeningTheAccountGroup != null && item.ReasonForOpeningTheAccountGroup.MultiSelectValue != null && item.ReasonForOpeningTheAccountGroup.MultiSelectValue.Length > 0) ? string.Join('|', item.ReasonForOpeningTheAccountGroup.MultiSelectValue) : string.Empty;
                retVal.PurposeAndActivity_ExpectedNatureOfIncomingAndOutgoingTransactions = (item.ExpectedNatureOfInAndOutTransactionGroup != null && item.ExpectedNatureOfInAndOutTransactionGroup.MultiSelectValue != null && item.ExpectedNatureOfInAndOutTransactionGroup.MultiSelectValue.Length > 0) ? string.Join('|', item.ExpectedNatureOfInAndOutTransactionGroup.MultiSelectValue) : string.Empty;
                retVal.PurposeAndActivity_ExpectedFrequencyOfIncomingAndOutgoingTransactions = item.ExpectedFrequencyOfInAndOutTransactionGroup != null && !string.IsNullOrEmpty(item.ExpectedFrequencyOfInAndOutTransactionGroup.RadioGroupValue) ? item.ExpectedFrequencyOfInAndOutTransactionGroup.RadioGroupValue : string.Empty;
                retVal.PurposeAndActivity_SignatureMandateType = item.SignatureMandateTypeGroup != null && !string.IsNullOrEmpty(item.SignatureMandateTypeGroup.RadioGroupValue) ? item.SignatureMandateTypeGroup.RadioGroupValue : string.Empty;
               
                if (item.ExpectedIncomingAmount != null)
                {
                    retVal.PurposeAndActivity_ExpectedIncomingAmount = item.ExpectedIncomingAmount.Replace(",", "");
                }
                else
                {
                    retVal.PurposeAndActivity_ExpectedIncomingAmount = item.ExpectedIncomingAmount;
                }
           
            }

            return retVal;
        }

        //public static List<IInputGroupItem> BindReasonForOpeningTheAccountGroup(List<IInputGroupItem> inputGroupItems, string selectedReasonForOpeningTheAccount)
        //{
        //	List<IInputGroupItem> retVal = null;

        //	if(inputGroupItems != null && inputGroupItems.Count > 0 && !string.IsNullOrEmpty(selectedReasonForOpeningTheAccount))
        //	{
        //		string[] selectedValues = selectedReasonForOpeningTheAccount.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        //		if(selectedValues != null && selectedValues.Length > 0)
        //		{
        //			inputGroupItems.ForEach(u => { if(selectedValues.Any(y => string.Equals(y, u.Value))) { u.Enabled = true; retVal.Add(u); } });
        //		}
        //	}
        //	else
        //	{
        //		retVal = inputGroupItems;
        //	}

        //	return retVal;
        //}

        #endregion
    }
}
