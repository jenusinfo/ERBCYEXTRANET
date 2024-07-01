using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Applicant.LegalEntity.ContactDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class ContactDetailsLegalProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";
        public static ContactDetailsLegalModel GetContactDetailsLegalById(TreeNode treeNode)
        {
            ContactDetailsLegalModel oContactDetailsLegalModel = new ContactDetailsLegalModel();

            if (treeNode != null)
            {
                oContactDetailsLegalModel.ContactDetailsLegalID = ValidationHelper.GetInteger(treeNode.GetValue("ContactDetailsLegalID"), 0);
                oContactDetailsLegalModel.ContactDetailsLegal_PreferredMailingAddress = ValidationHelper.GetString(treeNode.GetValue("ContactDetailsLegal_PreferredMailingAddress"), "");
                oContactDetailsLegalModel.ContactDetailsLegal_EmailAddressForSendingAlerts = ValidationHelper.GetString(treeNode.GetValue("ContactDetailsLegal_EmailAddressForSendingAlerts"), "");
                oContactDetailsLegalModel.ContactDetailsLegal_PreferredCommunicationLanguage = ValidationHelper.GetString(treeNode.GetValue("ContactDetailsLegal_PreferredCommunicationLanguage"), "");
            }


            return oContactDetailsLegalModel;
        }
        public static ContactDetailsLegalModel SaveContactDetailsLegal(ContactDetailsLegalModel model, TreeNode treeNodeData)
        {
            ContactDetailsLegalModel retVal = new ContactDetailsLegalModel();
            if (model != null)
            {
                if (treeNodeData != null)
                {
                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                    
                    TreeNode ContcatDetailsLegal = TreeNode.New("Eurobank.ContactDetailsLegal", tree);
                    string DocumentName = ServiceHelper.GetName(ValidationHelper.GetString(model.ContactDetailsLegal_PreferredCommunicationLanguage, ""), Constants.COMMUNICATION_LANGUAGE);
                    ContcatDetailsLegal.DocumentName = ValidationHelper.GetString(DocumentName, "");
                    ContcatDetailsLegal.SetValue("ContactDetailsLegal_PreferredMailingAddress", model.ContactDetailsLegal_PreferredMailingAddress);
                    ContcatDetailsLegal.SetValue("ContactDetailsLegal_EmailAddressForSendingAlerts", model.ContactDetailsLegal_EmailAddressForSendingAlerts);
                    ContcatDetailsLegal.SetValue("ContactDetailsLegal_PreferredCommunicationLanguage", model.ContactDetailsLegal_PreferredCommunicationLanguage);
                    ContcatDetailsLegal.Insert(treeNodeData);
                }

            }
            return retVal;
        }
        public static ContactDetailsLegalModel UpdateContactDetailsLegal(ContactDetailsLegalModel model, TreeNode ContcatDetailsLegal)
        {
            ContactDetailsLegalModel retVal = new ContactDetailsLegalModel();
            if (model != null)
            {
                if (ContcatDetailsLegal != null)
                {
                    string DocumentName = ServiceHelper.GetName(ValidationHelper.GetString(model.ContactDetailsLegal_PreferredCommunicationLanguage, ""), Constants.COMMUNICATION_LANGUAGE);
					if(string.IsNullOrEmpty(DocumentName))
					{
                        DocumentName = "Contact Details";
                    }
                    ContcatDetailsLegal.DocumentName = ValidationHelper.GetString(DocumentName, "");
                    ContcatDetailsLegal.SetValue("ContactDetailsLegal_PreferredMailingAddress", model.ContactDetailsLegal_PreferredMailingAddress);
                    ContcatDetailsLegal.SetValue("ContactDetailsLegal_EmailAddressForSendingAlerts", model.ContactDetailsLegal_EmailAddressForSendingAlerts);
                    ContcatDetailsLegal.SetValue("ContactDetailsLegal_PreferredCommunicationLanguage", model.ContactDetailsLegal_PreferredCommunicationLanguage);
                    ContcatDetailsLegal.NodeAlias = ContcatDetailsLegal.DocumentName;
                    ContcatDetailsLegal.Update();
                }

            }
            return retVal;
        }

        public static ContactDetailsLegalModel GetContactDetailsByApplicantId(int applicantId)
        {
            ContactDetailsLegalModel retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (applicantId > 0)
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(CompanyDetails.CLASS_NAME)
                    .WhereEquals("CompanyDetailsID", applicantId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();
                if (applicationDetailsNode != null)
                {
                    TreeNode contactDetailsNodes = applicationDetailsNode.Children.Where(u => u.ClassName == ContactDetailsLegal.CLASS_NAME).FirstOrDefault();

                    if (contactDetailsNodes != null)
                    {
                        retVal = new ContactDetailsLegalModel();
                        ContactDetailsLegal contactDetailsLegal = ContactDetailsLegalProvider.GetContactDetailsLegal(contactDetailsNodes.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                        if (contactDetailsLegal != null)
                        {
                            ContactDetailsLegalModel contactDetailsModel = BindContactDetailsModel(contactDetailsLegal);
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

        public static ContactDetailsLegalModel BindContactDetailsModel(ContactDetailsLegal ContactDetails)
        {
            var preferredLanguage = ServiceHelper.GetCommunicationLanguage();
            var prefferedMailingAddress = ServiceHelper.GetPreferred_Mailing_Address();
            ContactDetailsLegalModel retVal = null;
            if (ContactDetails != null)
            {
                retVal = new ContactDetailsLegalModel()
                {
                    ContactDetailsLegalID = ContactDetails.ContactDetailsLegalID,
                    ContactDetailsLegal_EmailAddressForSendingAlerts = ContactDetails.ContactDetailsLegal_EmailAddressForSendingAlerts,
                    ContactDetailsLegal_PreferredMailingAddress = (prefferedMailingAddress != null && prefferedMailingAddress.Count > 0 && ContactDetails.ContactDetailsLegal_PreferredMailingAddress != null && prefferedMailingAddress.Any(f => f.Value == ContactDetails.ContactDetailsLegal_PreferredMailingAddress.ToString())) ? prefferedMailingAddress.FirstOrDefault(f => f.Value == ContactDetails.ContactDetailsLegal_PreferredMailingAddress.ToString()).Text : string.Empty,
                    ContactDetailsLegal_PreferredCommunicationLanguage = (preferredLanguage != null && preferredLanguage.Count > 0 && ContactDetails.ContactDetailsLegal_PreferredCommunicationLanguage != null && preferredLanguage.Any(f => f.Value == ContactDetails.ContactDetailsLegal_PreferredCommunicationLanguage.ToString())) ? preferredLanguage.FirstOrDefault(f => f.Value == ContactDetails.ContactDetailsLegal_PreferredCommunicationLanguage.ToString()).Text : string.Empty,
                };
            }

            return retVal;
        }
    }
}
