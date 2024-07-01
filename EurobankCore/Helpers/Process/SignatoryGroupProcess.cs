using Amazon.S3.Model.Internal.MarshallTransformations;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Applicant.LegalEntity;
using Eurobank.Models.Application.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class SignatoryGroupProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

        private static readonly string _SignatoryGroupFolderName = "Signatory Group";

        private static readonly string _SignatoryGroupDocumentName = "Sign Group";

        //public static SignatoryGroupModel GetSignatoryGroupModelById(int signatoryGroupId)
        //{
        //	SignatoryGroupModel retVal = null;
        //	if(signatoryGroupId > 0)
        //	{
        //		retVal = BindSignatoryGroupModel(GetSignatoryGroupById(signatoryGroupId));
        //	}

        //	return retVal;
        //}

        public static List<SignatoryGroupModel> GetSignatoryGroup(int applicantId)
        {
            List<SignatoryGroupModel> retVal = null;

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
                    TreeNode signatoryGroupRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatoryGroupFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        signatoryGroupRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatoryGroupFolderName, StringComparison.OrdinalIgnoreCase));

                        if (signatoryGroupRoot != null)
                        {
                            List<TreeNode> signatoryGroupNodes = signatoryGroupRoot.Children.Where(u => u.ClassName == SignatoryGroup.CLASS_NAME).ToList();

                            if (signatoryGroupNodes != null && signatoryGroupNodes.Count > 0)
                            {
                                retVal = new List<SignatoryGroupModel>();
                                signatoryGroupNodes.ForEach(t =>
                                {
                                    SignatoryGroup signatoryGroup = SignatoryGroupProvider.GetSignatoryGroup(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (signatoryGroup != null)
                                    {
                                        SignatoryGroupModel signatoryGroupModel = BindSignatoryGroupModel(signatoryGroup, "");
                                        if (signatoryGroupModel != null)
                                        {
                                            retVal.Add(signatoryGroupModel);
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

        public static List<SignatoryGroupModel> GetRelatedPartyLegalSignatoryGroup(int applicantId)
        {
            List<SignatoryGroupModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (applicantId > 0)
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(CompanyDetailsRelatedParty.CLASS_NAME)
                    .WhereEquals("CompanyDetailsRelatedPartyID", applicantId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode signatoryGroupRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatoryGroupFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        signatoryGroupRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatoryGroupFolderName, StringComparison.OrdinalIgnoreCase));

                        if (signatoryGroupRoot != null)
                        {
                            List<TreeNode> signatoryGroupNodes = signatoryGroupRoot.Children.Where(u => u.ClassName == SignatoryGroup.CLASS_NAME).ToList();

                            if (signatoryGroupNodes != null && signatoryGroupNodes.Count > 0)
                            {
                                retVal = new List<SignatoryGroupModel>();
                                signatoryGroupNodes.ForEach(t =>
                                {
                                    SignatoryGroup signatoryGroup = SignatoryGroupProvider.GetSignatoryGroup(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (signatoryGroup != null)
                                    {
                                        SignatoryGroupModel signatoryGroupModel = BindSignatoryGroupModel(signatoryGroup, "");
                                        if (signatoryGroupModel != null)
                                        {
                                            retVal.Add(signatoryGroupModel);
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

        public static List<SignatoryGroupModel> GetApplicationSignatoryGroup(int applicationId)
        {
            var applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);
            string applicationNumber = applicationDetails.ApplicationDetails_ApplicationNumber;
            List<SignatoryGroupModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (applicationId > 0)
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(ApplicationDetails.CLASS_NAME)
                    .WhereEquals("ApplicationDetailsID", applicationId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode signatoryGroupRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatoryGroupFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        signatoryGroupRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatoryGroupFolderName, StringComparison.OrdinalIgnoreCase));

                        if (signatoryGroupRoot != null)
                        {
                            List<TreeNode> signatoryGroupNodes = signatoryGroupRoot.Children.Where(u => u.ClassName == SignatoryGroup.CLASS_NAME).ToList();

                            if (signatoryGroupNodes != null && signatoryGroupNodes.Count > 0)
                            {
                                retVal = new List<SignatoryGroupModel>();
                                signatoryGroupNodes.ForEach(t =>
                                {
                                    SignatoryGroup signatoryGroup = SignatoryGroupProvider.GetSignatoryGroup(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (signatoryGroup != null)
                                    {
                                        SignatoryGroupModel signatoryGroupModel = BindSignatoryGroupModel(signatoryGroup, applicationNumber);
                                        if (signatoryGroupModel != null)
                                        {
                                            retVal.Add(signatoryGroupModel);
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

        public static SignatoryGroupModel SaveSignatoryGroupModel(int applicantId, SignatoryGroupModel model)
        {
            SignatoryGroupModel retVal = null;

            if (model != null && model.Id > 0)
            {
                SignatoryGroup signatoryGroup = GetSignatoryGroupById(model.Id);
                if (signatoryGroup != null)
                {
                    SignatoryGroup updatedSignatoryGroup = BindSignatoryGroup(signatoryGroup, model);
                    if (updatedSignatoryGroup != null)
                    {
                        updatedSignatoryGroup.Update();
                        retVal = BindSignatoryGroupModel(updatedSignatoryGroup, "");
                    }
                }
            }
            else if (applicantId > 0)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(CompanyDetails.CLASS_NAME)
                    .WhereEquals("CompanyDetailsID", applicantId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode signatoryGroupRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatoryGroupFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        signatoryGroupRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatoryGroupFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        signatoryGroupRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        signatoryGroupRoot.DocumentName = _SignatoryGroupFolderName;
                        signatoryGroupRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        signatoryGroupRoot.Insert(applicationDetailsNode);
                    }
                    SignatoryGroup signatoryGroup = BindSignatoryGroup(null, model);
                    if (signatoryGroup != null && signatoryGroupRoot != null)
                    {
                        signatoryGroup.DocumentName = model.SignatoryGroupName;
                        signatoryGroup.NodeName = model.SignatoryGroupName;
                        signatoryGroup.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;

                        signatoryGroup.Insert(signatoryGroupRoot);
                        model = BindSignatoryGroupModel(signatoryGroup, "");
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        public static SignatoryGroupModel SaveRelatedPartyLegalSignatoryGroupModel(int relatedPartyId, SignatoryGroupModel model)
        {
            SignatoryGroupModel retVal = null;

            if (model != null && model.Id > 0)
            {
                SignatoryGroup signatoryGroup = GetSignatoryGroupById(model.Id);
                if (signatoryGroup != null)
                {
                    SignatoryGroup updatedSignatoryGroup = BindSignatoryGroup(signatoryGroup, model);
                    if (updatedSignatoryGroup != null)
                    {
                        updatedSignatoryGroup.Update();
                        retVal = BindSignatoryGroupModel(updatedSignatoryGroup, "");
                    }
                }
            }
            else if (relatedPartyId > 0)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(CompanyDetailsRelatedParty.CLASS_NAME)
                    .WhereEquals("CompanyDetailsRelatedPartyID", relatedPartyId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode signatoryGroupRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatoryGroupFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        signatoryGroupRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatoryGroupFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        signatoryGroupRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        signatoryGroupRoot.DocumentName = _SignatoryGroupFolderName;
                        signatoryGroupRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        signatoryGroupRoot.Insert(applicationDetailsNode);
                    }
                    SignatoryGroup signatoryGroup = BindSignatoryGroup(null, model);
                    if (signatoryGroup != null && signatoryGroupRoot != null)
                    {
                        signatoryGroup.DocumentName = model.SignatoryGroupName;
                        signatoryGroup.NodeName = model.SignatoryGroupName;
                        signatoryGroup.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;

                        signatoryGroup.Insert(signatoryGroupRoot);
                        model = BindSignatoryGroupModel(signatoryGroup, "");
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        public static SignatoryGroupModel SaveApplicationSignatoryGroupModel(int applicationId, SignatoryGroupModel model)
        {
            SignatoryGroupModel retVal = null;
            var applicationDetails = ApplicationsProcess.GetApplicationDetailsById(applicationId);
            string applicationNumber = applicationDetails.ApplicationDetails_ApplicationNumber;

            if (model != null && model.Id > 0)
            {
                SignatoryGroup signatoryGroup = GetSignatoryGroupById(model.Id);
                if (signatoryGroup != null)
                {
                    SignatoryGroup updatedSignatoryGroup = BindSignatoryGroup(signatoryGroup, model);
                    if (updatedSignatoryGroup != null)
                    {
                        updatedSignatoryGroup.Update();
                        retVal = BindSignatoryGroupModel(updatedSignatoryGroup, applicationNumber);
                    }
                }
            }
            else if (applicationId > 0)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(ApplicationDetails.CLASS_NAME)
                    .WhereEquals("ApplicationDetailsID", applicationId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode signatoryGroupRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatoryGroupFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        signatoryGroupRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatoryGroupFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        signatoryGroupRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        signatoryGroupRoot.DocumentName = _SignatoryGroupFolderName;
                        signatoryGroupRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        signatoryGroupRoot.Insert(applicationDetailsNode);
                    }
                    SignatoryGroup signatoryGroup = BindSignatoryGroup(null, model);
                    if (signatoryGroup != null && signatoryGroupRoot != null)
                    {
                        //signatoryGroup.DocumentName = model.SignatoryGroupName;
                        //signatoryGroup.NodeName = model.SignatoryGroupName;
                        signatoryGroup.DocumentName = _SignatoryGroupDocumentName;
                        signatoryGroup.NodeName = _SignatoryGroupDocumentName;

                        signatoryGroup.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;

                        signatoryGroup.Insert(signatoryGroupRoot);
                        model = BindSignatoryGroupModel(signatoryGroup, applicationNumber);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        public static SignatoryGroup GetSignatoryGroupById(int signatoryGroupId)
        {
            SignatoryGroup retVal = null;

            if (signatoryGroupId > 0)
            {
                var signatoryGroup = SignatoryGroupProvider.GetSignatoryGroups();
                if (signatoryGroup != null && signatoryGroup.Count > 0)
                {
                    retVal = signatoryGroup.FirstOrDefault(o => o.SignatoryGroupID == signatoryGroupId);
                }
            }

            return retVal;
        }

        public static List<SelectListItem> GetSignatoryGroups()
        {
            List<SelectListItem> retVal = null;

            var signatoryGroup = SignatoryGroupProvider.GetSignatoryGroups();
            if (signatoryGroup != null && signatoryGroup.Count > 0)
            {
                retVal = signatoryGroup.Select(t => new SelectListItem() { Value = t.NodeGUID.ToString(), Text = t.SignatoryGroupName }).ToList();
            }

            return retVal;
        }
        //public static List<SelectListItem> GetDDLSignatoryGroupsByApplicationId(int applicationId)
        //{
        //    List<SelectListItem> retVal = null;

        //    var signatoryGroup = GetApplicationSignatoryGroup(applicationId);
        //    if (signatoryGroup != null && signatoryGroup.Count > 0)
        //    {
        //        retVal = signatoryGroup.Select(t => new SelectListItem() { Value = t.SignatoryGroupName.ToString(), Text = t.SignatoryGroup }).ToList();
        //    }

        //    return retVal;
        //}
        public static List<SelectListItem> GetDDLSignatoryGroupsForSignatureMandate(int applicationId)
        {
            List<SelectListItem> retVal = new List<SelectListItem>();

            var signatoryGroup = GetApplicationSignatoryGroup(applicationId);
            if (signatoryGroup != null && signatoryGroup.Count > 0)
            {
                foreach (var item in signatoryGroup)
                {
                    if (!string.IsNullOrEmpty(item.SignatoryGroup) && item.SignatoryGroup != "AUTHORISED PERSONS")
                    {
                        retVal.Add(new SelectListItem { Text = item.SignatoryGroup, Value = item.SignatoryGroupName.ToString() });
                    }
                }
            }

            return retVal.GroupBy(x => x.Text).Select(y => y.First()).OrderBy(x => x.Text).ToList();
        }
        public static List<SelectListItem> GetDDLAuthorizePersonSignatoryGroupsForSignatureMandate(int applicationId)
        {
            List<SelectListItem> retVal = new List<SelectListItem>();

            var signatoryGroup = GetApplicationSignatoryGroup(applicationId);
            if (signatoryGroup != null && signatoryGroup.Count > 0)
            {
                foreach (var item in signatoryGroup)
                {
                    if (!string.IsNullOrEmpty(item.SignatoryGroup) && item.SignatoryGroup == "AUTHORISED PERSONS")
                    {
                        retVal.Add(new SelectListItem { Text = item.SignatoryGroup, Value = item.SignatoryGroupName.ToString() });
                    }
                }
            }

            return retVal.GroupBy(x => x.Text).Select(y => y.First()).OrderBy(x => x.Text).ToList();
        }
        public static List<SelectListItem> GetDDLSignatoryGroupsByApplicationId(int applicationId)
        {
            List<SelectListItem> retVal = new List<SelectListItem>();

            var signatoryGroup = GetApplicationSignatoryGroup(applicationId);
            if (signatoryGroup != null && signatoryGroup.Count > 0)
            {
                foreach (var item in signatoryGroup)
                {
                    if (!string.IsNullOrEmpty(item.SignatoryGroup))
                    {
                        retVal.Add(new SelectListItem { Text = item.SignatoryGroup, Value = item.SignatoryGroupName.ToString() });
                    }
                }
            }

            return retVal.GroupBy(x => x.Text).Select(y => y.First()).OrderBy(x=>x.Text).ToList();
        }
        #region Bind Data

        private static SignatoryGroupModel BindSignatoryGroupModel(SignatoryGroup item, string applicationNumber)
        {
            SignatoryGroupModel retVal = null;
            var groupName = ServiceHelper.GetSignatoryGroup();
            var signatureGroupRights = ServiceHelper.SignatureMandateTypeGroup();
            if (item != null)
            {
                var SignatoryGroup1 = groupName.Where(x => x.Value == item.SignatoryGroupName);
                retVal = new SignatoryGroupModel()
                {
                    Id = item.SignatoryGroupID,
					SignatoryGroup = (SignatoryGroup1 != null && SignatoryGroup1.Any()) ? groupName.FirstOrDefault(f => f.Value == item.SignatoryGroupName.ToString()).Text : string.Empty,
					//SignatoryGroup = (groupName != null && groupName.Count > 0 && !string.IsNullOrEmpty(item.SignatoryGroupName)) ? groupName.FirstOrDefault(f => f.Value == item.SignatoryGroupName.ToString()).Text : string.Empty,
                    SignatoryGroupName = item.SignatoryGroupName,
                    SignatoryPersons = item.SignatoryPersons,
                    SignatoryPersonNames = GetSignaturePersonsString(item.SignatoryPersons, applicationNumber),
                    SignatoryPersonsList = GetSignaturePersonsGuidList(item.SignatoryPersons),
                    SignatureRightsValue = item.SignatureRights,
                    SignatureRightsName = (signatureGroupRights != null && signatureGroupRights.Count > 0 && !string.IsNullOrEmpty(item.SignatureRights)) ? signatureGroupRights.FirstOrDefault(f => f.Value == item.SignatureRights).Label : string.Empty,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    StartDateString = item.StartDate.ToString("dd/MM/yyyy"),
                    EndDateString = item.EndDate.ToString("dd/MM/yyyy"),
                    Status = item.Status,
                    StatusName = item.Status ? GridRecordStatus.Complete.ToString() : GridRecordStatus.Pending.ToString()
                };
            }

            return retVal;
        }

        private static SignatoryGroup BindSignatoryGroup(SignatoryGroup existSignatoryGroup, SignatoryGroupModel item)
        {
            SignatoryGroup retVal = new SignatoryGroup();
            if (existSignatoryGroup != null)
            {
                retVal = existSignatoryGroup;
            }
            if (item != null)
            {
                retVal.SignatoryGroupName = item.SignatoryGroupName;
                retVal.SignatoryPersons = item.SignatoryPersons;
                retVal.SignatureRights = item.SignatureRightsValue;
                retVal.StartDate = Convert.ToDateTime(item.StartDate);
                retVal.EndDate = Convert.ToDateTime(item.EndDate);
                retVal.Status = item.Status;
            }

            return retVal;
        }

        #endregion

        //private static string GetSignaturePersonsString(string signaturePersonsGuidValues)
        //{
        //	string retVal = string.Empty;

        //	if(!string.IsNullOrEmpty(signaturePersonsGuidValues))
        //	{
        //		//Need to change after clarification 
        //		List<SelectListItem> signatoryPersons = ServiceHelper.GetNoteDetailPendingOnUsers();
        //		string[] signaturePersonGuidList = GetSignaturePersonsGuidList(signaturePersonsGuidValues);
        //		if(signaturePersonGuidList != null && signaturePersonGuidList.Length > 0)
        //		{
        //			foreach(string per in signaturePersonGuidList)
        //			{
        //				if(string.IsNullOrEmpty(retVal))
        //				{
        //					retVal = signatoryPersons.Any(o => o.Value == per) ? signatoryPersons.FirstOrDefault(o => o.Value == per).Text : string.Empty;
        //				}
        //				else
        //				{
        //					retVal += "," + (signatoryPersons.Any(o => o.Value == per) ? signatoryPersons.FirstOrDefault(o => o.Value == per).Text : string.Empty);
        //				}
        //			}
        //		}
        //	}

        //	return retVal;
        //}
        private static string GetSignaturePersonsString(string signaturePersonsGuidValues, string applicationNumber)
        {
            string retVal = string.Empty;

            if (!string.IsNullOrEmpty(signaturePersonsGuidValues))
            {
                //Need to change after clarification 
                //List<SelectListItem> signatoryPersons = ServiceHelper.GetNoteDetailPendingOnUsers();//Commented by SB
                List<SelectListItem> signatoryPersons = CommonProcess.GetSignatoryPersonLegal(applicationNumber);
                string[] signaturePersonGuidList = GetSignaturePersonsGuidList(signaturePersonsGuidValues);
                if (signaturePersonGuidList != null && signaturePersonGuidList.Length > 0)
                {
                    foreach (string per in signaturePersonGuidList)
                    {
                        if (string.IsNullOrEmpty(retVal))
                        {
                            retVal = signatoryPersons.Any(o => o.Value == per) ? signatoryPersons.FirstOrDefault(o => o.Value == per).Text : string.Empty;
                        }
                        else
                        {
                            retVal += "," + (signatoryPersons.Any(o => o.Value == per) ? signatoryPersons.FirstOrDefault(o => o.Value == per).Text : string.Empty);
                        }
                    }
                }
            }

            return retVal;
        }
        private static string[] GetSignaturePersonsGuidList(string signaturePersonsGuidValues)
        {
            if (!string.IsNullOrEmpty(signaturePersonsGuidValues))
                return signaturePersonsGuidValues.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            return null;
        }
    }
}
