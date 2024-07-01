using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.RelatedParty.ContactDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class ContactDetailsProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";
        public static ContactDetailsViewModel GetContactDetailsById(TreeNode treeNode)
        {
            ContactDetailsViewModel oContactDetailsViewModel = new ContactDetailsViewModel();

            if (treeNode != null)
            {
                oContactDetailsViewModel.ContactDetailsID = ValidationHelper.GetInteger(treeNode.GetValue("ContactDetailsID"), 0);
                oContactDetailsViewModel.ContactDetails_MobileTelNoNumber = ValidationHelper.GetString(treeNode.GetValue("ContactDetails_MobileTelNoNumber"), "");
                oContactDetailsViewModel.ContactDetails_HomeTelNoNumber = ValidationHelper.GetString(treeNode.GetValue("ContactDetails_HomeTelNoNumber"), "");
                oContactDetailsViewModel.ContactDetails_WorkTelNoNumber = ValidationHelper.GetString(treeNode.GetValue("ContactDetails_WorkTelNoNumber"), "");
                oContactDetailsViewModel.ContactDetails_FaxNoFaxNumber = ValidationHelper.GetString(treeNode.GetValue("ContactDetails_FaxNoFaxNumber"), "");
                oContactDetailsViewModel.ContactDetails_EmailAddress = ValidationHelper.GetString(treeNode.GetValue("ContactDetails_EmailAddress"), "");
                oContactDetailsViewModel.ContactDetails_PreferredCommunicationLanguage = ValidationHelper.GetString(treeNode.GetValue("ContactDetails_PreferredCommunicationLanguage"), "");
                oContactDetailsViewModel.Country_Code_MobileTelNoNumber = ValidationHelper.GetString(treeNode.GetValue("ContactDetails_Mobile_CountryCode"), "");
                oContactDetailsViewModel.Country_Code_HomeTelNoNumber = ValidationHelper.GetString(treeNode.GetValue("ContactDetails_Home_CountryCode"), "");
                oContactDetailsViewModel.Country_Code_WorkTelNoNumber = ValidationHelper.GetString(treeNode.GetValue("ContactDetails_Work_CountryCode"), "");
                oContactDetailsViewModel.Country_Code_FaxNoFaxNumber = ValidationHelper.GetString(treeNode.GetValue("ContactDetails_Fax_CountryCode"), "");
            }


            return oContactDetailsViewModel;
        }
        public static ContactDetailsViewModel SaveContactDetails(ContactDetailsViewModel model, TreeNode treeNodeData)
        {
            ContactDetailsViewModel retVal = new ContactDetailsViewModel();
            if (model != null)
            {
                if (treeNodeData != null)
                {
                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                    //TreeNode accountsfoldernode_parent = tree.SelectNodes()
                    //	.Path(treeNodeData.NodeAliasPath + "/Contact-Details")
                    //	.OnCurrentSite()
                    //	.Published(false)
                    //	.FirstOrDefault();
                    //if (accountsfoldernode_parent == null)
                    //{

                    //	accountsfoldernode_parent = TreeNode.New("CMS.Folder", tree);
                    //	accountsfoldernode_parent.DocumentName = "Contcat Details";
                    //	accountsfoldernode_parent.DocumentCulture = "en-US";
                    //	accountsfoldernode_parent.Insert(treeNodeData);
                    //}
                    TreeNode ContcatDetails = TreeNode.New("Eurobank.ContactDetails", tree);
                   // string DocumentName = ServiceHelper.GetName(ValidationHelper.GetString(model.ContactDetails_PreferredCommunicationLanguage, ""), Constants.COMMUNICATION_LANGUAGE);
                    string DocumentName ="Contact Details";
                    if(model.IsRetriveFromRegistry == true)
                    { 
                    ContcatDetails.DocumentName = ValidationHelper.GetString(DocumentName, "");
                    ContcatDetails.SetValue("ContactDetails_MobileTelNoNumber", model.hdnContactDetails_MobileTelNoNumber);
                    ContcatDetails.SetValue("ContactDetails_HomeTelNoNumber", model.hdnContactDetails_HomeTelNoNumber);
                    ContcatDetails.SetValue("ContactDetails_WorkTelNoNumber", model.hdnContactDetails_WorkTelNoNumber);
                    ContcatDetails.SetValue("ContactDetails_FaxNoFaxNumber", model.hdnContactDetails_FaxNoFaxNumber);
                    ContcatDetails.SetValue("ContactDetails_EmailAddress", model.ContactDetails_EmailAddress);
                    ContcatDetails.SetValue("ContactDetails_PreferredCommunicationLanguage", model.ContactDetails_PreferredCommunicationLanguage);
                    ContcatDetails.SetValue("ContactDetails_Mobile_CountryCode", model.hdnCountry_Code_MobileTelNoNumber);
                    ContcatDetails.SetValue("ContactDetails_Home_CountryCode", model.hdnCountry_Code_HomeTelNoNumber);
                    ContcatDetails.SetValue("ContactDetails_Work_CountryCode", model.hdnCountry_Code_WorkTelNoNumber);
                    ContcatDetails.SetValue("ContactDetails_Fax_CountryCode", model.hdnCountry_Code_FaxNoFaxNumber);
                    ContcatDetails.Insert(treeNodeData);
                    }
                    else
                    {
                        ContcatDetails.DocumentName = ValidationHelper.GetString(DocumentName, "");
                        ContcatDetails.SetValue("ContactDetails_MobileTelNoNumber", model.ContactDetails_MobileTelNoNumber);
                        ContcatDetails.SetValue("ContactDetails_HomeTelNoNumber", model.ContactDetails_HomeTelNoNumber);
                        ContcatDetails.SetValue("ContactDetails_WorkTelNoNumber", model.ContactDetails_WorkTelNoNumber);
                        ContcatDetails.SetValue("ContactDetails_FaxNoFaxNumber", model.ContactDetails_FaxNoFaxNumber);
                        ContcatDetails.SetValue("ContactDetails_EmailAddress", model.ContactDetails_EmailAddress);
                        ContcatDetails.SetValue("ContactDetails_PreferredCommunicationLanguage", model.ContactDetails_PreferredCommunicationLanguage);
                        ContcatDetails.SetValue("ContactDetails_Mobile_CountryCode", model.Country_Code_MobileTelNoNumber);
                        ContcatDetails.SetValue("ContactDetails_Home_CountryCode", model.Country_Code_HomeTelNoNumber);
                        ContcatDetails.SetValue("ContactDetails_Work_CountryCode", model.Country_Code_WorkTelNoNumber);
                        ContcatDetails.SetValue("ContactDetails_Fax_CountryCode", model.Country_Code_FaxNoFaxNumber);
                        ContcatDetails.Insert(treeNodeData);
                    }

                }

            }
            retVal.IsRetriveFromRegistry = model.IsRetriveFromRegistry;
            return retVal;
        }
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
        public static ContactDetailsViewModel UpdateContactDetails(ContactDetailsViewModel model, TreeNode ContcatDetails)
        {
            ContactDetailsViewModel retVal = new ContactDetailsViewModel();
            if (model != null)
            {
                if (ContcatDetails != null)
                {
                    //string DocumentName = ServiceHelper.GetName(ValidationHelper.GetString(model.ContactDetails_PreferredCommunicationLanguage, ""), Constants.COMMUNICATION_LANGUAGE);
                    // string DocumentName = model.ContactDetails_MobileTelNoNumber;
                    string DocumentName = "Contact Details";
                    ContcatDetails.DocumentName = ValidationHelper.GetString(DocumentName, "");
                    ContcatDetails.SetValue("ContactDetails_MobileTelNoNumber", model.ContactDetails_MobileTelNoNumber);
                    ContcatDetails.SetValue("ContactDetails_HomeTelNoNumber", model.ContactDetails_HomeTelNoNumber);
                    ContcatDetails.SetValue("ContactDetails_WorkTelNoNumber", model.ContactDetails_WorkTelNoNumber);
                    ContcatDetails.SetValue("ContactDetails_FaxNoFaxNumber", model.ContactDetails_FaxNoFaxNumber);
                    ContcatDetails.SetValue("ContactDetails_EmailAddress", model.ContactDetails_EmailAddress);
                    ContcatDetails.SetValue("ContactDetails_PreferredCommunicationLanguage", model.ContactDetails_PreferredCommunicationLanguage);
                    ContcatDetails.SetValue("ContactDetails_Mobile_CountryCode", model.Country_Code_MobileTelNoNumber);
                    ContcatDetails.SetValue("ContactDetails_Home_CountryCode", model.Country_Code_HomeTelNoNumber);
                    ContcatDetails.SetValue("ContactDetails_Work_CountryCode", model.Country_Code_WorkTelNoNumber);
                    ContcatDetails.SetValue("ContactDetails_Fax_CountryCode", model.Country_Code_FaxNoFaxNumber);
                    ContcatDetails.NodeAlias = ContcatDetails.DocumentName;
                    ContcatDetails.Update();

                }

            }
            return retVal;
        }

        public static ContactDetailsViewModel GetContactDetailsByApplicantId(int applicantId)
        {
            ContactDetailsViewModel retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (applicantId > 0)
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(PersonalDetails.CLASS_NAME)
                    .WhereEquals("PersonalDetailsID", applicantId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();
                if (applicationDetailsNode != null)
                {
                    TreeNode contactDetailsNodes = applicationDetailsNode.Children.Where(u => u.ClassName == ContactDetails.CLASS_NAME).FirstOrDefault();

                    if (contactDetailsNodes != null)
                    {
                        retVal = new ContactDetailsViewModel();
                        ContactDetails contactDetails = ContactDetailsProvider.GetContactDetails(contactDetailsNodes.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                        if (contactDetails != null)
                        {
                            ContactDetailsViewModel contactDetailsModel = BindContactDetailsModel(contactDetails);
                            if (contactDetailsModel != null)
                            {
                                retVal = contactDetailsModel;
                            }
                        }

                    }
                }
            }

            return retVal;
        }

        public static ContactDetailsViewModel BindContactDetailsModel(ContactDetails ContactDetails)
        {
            var preferredLanguage = ServiceHelper.GetCommunicationLanguage();
            ContactDetailsViewModel retVal = null;
            if (ContactDetails != null)
            {
                retVal = new ContactDetailsViewModel()
                {
                    ContactDetailsID = ContactDetails.ContactDetailsID,
                    ContactDetails_EmailAddress = ContactDetails.ContactDetails_EmailAddress,
                    ContactDetails_ConsentForMarketingPurposes = ContactDetails.ContactDetails_ConsentForMarketingPurposes,
                    ContactDetails_FaxNoFaxNumber = ContactDetails.ContactDetails_FaxNoFaxNumber,
                    ContactDetails_HomeTelNoNumber = ContactDetails.ContactDetails_HomeTelNoNumber,
                    ContactDetails_MobileTelNoNumber = ContactDetails.ContactDetails_MobileTelNoNumber,
                    ContactDetails_PreferredCommunicationLanguage = (preferredLanguage != null && preferredLanguage.Count > 0 && ContactDetails.ContactDetails_PreferredCommunicationLanguage != null && preferredLanguage.Any(f => f.Value == ContactDetails.ContactDetails_PreferredCommunicationLanguage.ToString())) ? preferredLanguage.FirstOrDefault(f => f.Value == ContactDetails.ContactDetails_PreferredCommunicationLanguage.ToString()).Text : string.Empty,
                    ContactDetails_WorkTelNoNumber = ContactDetails.ContactDetails_WorkTelNoNumber,
                    ContactDetails_SaveInRegistry = ContactDetails.ContactDetails_SaveInRegistry,
                    Country_Code_MobileTelNoNumber = ContactDetails.ContactDetails_Mobile_CountryCode,
                    Country_Code_FaxNoFaxNumber = ContactDetails.ContactDetails_Fax_CountryCode,
                    Country_Code_WorkTelNoNumber = ContactDetails.ContactDetails_Work_CountryCode,
                    Country_Code_HomeTelNoNumber = ContactDetails.ContactDetails_Home_CountryCode
                };
            }

            return retVal;
        }
    }
}
