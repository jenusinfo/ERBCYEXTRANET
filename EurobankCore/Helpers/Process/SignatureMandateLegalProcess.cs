using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
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
    public class SignatureMandateLegalProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

        private static readonly string _SignatureMandateLegalFolderName = "Signature Mandate";

        public static List<SignatureMandateCompanyModel> GetSignatureMandateLegalModels(string applicationNumber)
        {
            List<SignatureMandateCompanyModel> retVal = null;

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
                    retVal = new List<SignatureMandateCompanyModel>();
                    TreeNode signatureMandateLegalFolderRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatureMandateLegalFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        signatureMandateLegalFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatureMandateLegalFolderName, StringComparison.OrdinalIgnoreCase));

                        if (signatureMandateLegalFolderRoot != null)
                        {
                            List<TreeNode> signatureMandateLegalNodes = signatureMandateLegalFolderRoot.Children.Where(u => u.ClassName == SignatureMandateCompany.CLASS_NAME).ToList();

                            if (signatureMandateLegalNodes != null && signatureMandateLegalNodes.Count > 0)
                            {
                                signatureMandateLegalNodes.ForEach(t =>
                                {
                                    SignatureMandateCompany signatureMandateCompany = SignatureMandateCompanyProvider.GetSignatureMandateCompany(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (signatureMandateCompany != null)
                                    {
                                        SignatureMandateCompanyModel signatureMandateCompanyModel = BindSignatureMandateCompanyModel(applicationDetailsNode.GetIntegerValue("ApplicationDetailsID", 0), signatureMandateCompany);
                                        if (signatureMandateCompanyModel != null)
                                        {
                                            retVal.Add(signatureMandateCompanyModel);
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

        public static SignatureMandateCompanyModel SaveSignatureMandateLegalModel(string applicationNumber, SignatureMandateCompanyModel model, int applicationId = 0)
        {
            SignatureMandateCompanyModel retVal = null;

            if (model != null && model.Id > 0)
            {
                SignatureMandateCompany signatureMandateCompany = GetSignatureMandateLegalById(model.Id);
                if (signatureMandateCompany != null)
                {
                    SignatureMandateCompany updatedSignatureMandateLegal = BindSignatureMandateCompany(signatureMandateCompany, model);
                    if (updatedSignatureMandateLegal != null)
                    {
                        updatedSignatureMandateLegal.Update();
                        retVal = BindSignatureMandateCompanyModel(applicationId, updatedSignatureMandateLegal);
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
                    TreeNode signatureMandateLegalFolderRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatureMandateLegalFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        signatureMandateLegalFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatureMandateLegalFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        signatureMandateLegalFolderRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        signatureMandateLegalFolderRoot.DocumentName = _SignatureMandateLegalFolderName;
                        signatureMandateLegalFolderRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        signatureMandateLegalFolderRoot.Insert(applicationDetailsNode);
                    }
                    SignatureMandateCompany signatureMandateCompany = BindSignatureMandateCompany(null, model);
                    if (signatureMandateCompany != null && signatureMandateLegalFolderRoot != null)
                    {
                        string documentName = string.Empty;
                        int idCounter = 1;
                        var allSignatureMandateCompany = SignatureMandateCompanyProvider.GetSignatureMandateCompanies();
                        if (allSignatureMandateCompany != null && allSignatureMandateCompany.Count > 0)
                        {
                            idCounter = allSignatureMandateCompany.Max(y => y.SignatureMandateCompanyID) + 1;
                        }
                        documentName = _SignatureMandateLegalFolderName + idCounter;
                        signatureMandateCompany.DocumentName = documentName;
                        signatureMandateCompany.NodeName = documentName;
                        signatureMandateCompany.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        signatureMandateCompany.Insert(signatureMandateLegalFolderRoot);
                        model = BindSignatureMandateCompanyModel(applicationId, signatureMandateCompany);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        private static SignatureMandateCompany GetSignatureMandateLegalById(int signatureMandateLegalId)
        {
            SignatureMandateCompany retVal = null;

            if (signatureMandateLegalId > 0)
            {
                var signatureMandateLegal = SignatureMandateCompanyProvider.GetSignatureMandateCompanies();
                if (signatureMandateLegal != null && signatureMandateLegal.Count > 0)
                {
                    retVal = signatureMandateLegal.FirstOrDefault(o => o.SignatureMandateCompanyID == signatureMandateLegalId);
                }
            }

            return retVal;
        }

        private static SignatureMandateCompanyModel BindSignatureMandateCompanyModel(int applicationId, SignatureMandateCompany item)
        {
            SignatureMandateCompanyModel retVal = null;

            if (item != null)
            {
                string authorizedSignatoryGroupNames = string.Empty;
                string authorizedSignatoryGroup1Names = string.Empty;
                //var signtoryGroups = SignatoryGroupProcess.GetSignatoryGroups();
                var signatureMandate = ServiceHelper.GetMandateType();
                var signtoryGroups = SignatoryGroupProcess.GetDDLSignatoryGroupsByApplicationId(applicationId);
                var authorizedSignatoryGroupGuidList = GetAuthorizedSignatoryGroupGuidList(item.SignatureMandateCompany_AuthorizedSignatoryGroup);
                var authorizedSignatoryGroup1GuidList = GetAuthorizedSignatoryGroupGuidList(item.SignatureMandateCompany_AuthorizedSignatoryGroup1);
                if (signtoryGroups != null && signtoryGroups.Count > 0)
                {
                    if (authorizedSignatoryGroupGuidList != null && authorizedSignatoryGroupGuidList.Length > 0)
                    {
                        authorizedSignatoryGroupNames = string.Join(',', signtoryGroups.Where(t => authorizedSignatoryGroupGuidList.Any(r => string.Equals(t.Value, r, StringComparison.OrdinalIgnoreCase))).Select(e => e.Text));
                    }
                    if (authorizedSignatoryGroup1GuidList != null && authorizedSignatoryGroup1GuidList.Length > 0)
                    {
                        authorizedSignatoryGroup1Names = string.Join(',', signtoryGroups.Where(t => authorizedSignatoryGroup1GuidList.Any(r => string.Equals(t.Value, r, StringComparison.OrdinalIgnoreCase))).Select(e => e.Text));
                    }
                }


                retVal = new SignatureMandateCompanyModel()
                {
                    Id = item.SignatureMandateCompanyID,
                    AuthorizedSignatoryGroup = item.SignatureMandateCompany_AuthorizedSignatoryGroup,
                    AuthorizedSignatoryGroupList = authorizedSignatoryGroupGuidList,
                    AuthorizedSignatoryGroupName = authorizedSignatoryGroupNames,
                    AuthorizedSignatoryGroup1 = item.SignatureMandateCompany_AuthorizedSignatoryGroup1,
                    AuthorizedSignatoryGroup1List = authorizedSignatoryGroup1GuidList,
                    AuthorizedSignatoryGroup1Name = authorizedSignatoryGroup1Names,
                    LimitFrom = item.SignatureMandateCompany_LimitFrom,
                    LimitTo = item.SignatureMandateCompany_LimitTo,
                    MandateType = item.SignatureMandateCompany_Mandatetype,
                    MandateTypeName = (!string.IsNullOrEmpty(item.SignatureMandateCompany_Mandatetype) && signatureMandate != null && signatureMandate.Any(u => string.Equals(u.Value, item.SignatureMandateCompany_Mandatetype, StringComparison.OrdinalIgnoreCase))) ? signatureMandate.FirstOrDefault(u => string.Equals(u.Value, item.SignatureMandateCompany_Mandatetype, StringComparison.OrdinalIgnoreCase)).Text : string.Empty,
                    Description = item.SignatureMandateCompany_Description,
                    NumberofSignatures = item.SignatureMandateCompany_NumberofSignatures,
                    NumberofSignatures1 = item.SignatureMandateCompany_NumberofSignatures1,
                    Rights = item.SignatureMandateCompany_Rights,
                    TotalNumberofSignature = item.SignatureMandateCompany_TotalNumberofSignature,
                    Status = item.Status,
                    StatusName = item.Status ? GridRecordStatus.Complete.ToString() : GridRecordStatus.Pending.ToString()
                };
            }

            return retVal;
        }

        private static SignatureMandateCompany BindSignatureMandateCompany(SignatureMandateCompany existSignatureMandateCompany, SignatureMandateCompanyModel item)
        {
            SignatureMandateCompany retVal = new SignatureMandateCompany();
            if (existSignatureMandateCompany != null)
            {
                retVal = existSignatureMandateCompany;
            }
            if (item != null)
            {
                retVal.SignatureMandateCompany_AuthorizedSignatoryGroup = item.AuthorizedSignatoryGroup;
                retVal.SignatureMandateCompany_AuthorizedSignatoryGroup1 = item.AuthorizedSignatoryGroup1;
                retVal.SignatureMandateCompany_LimitFrom =Convert.ToDecimal( item.LimitFrom);
                retVal.SignatureMandateCompany_LimitTo =Convert.ToDecimal( item.LimitTo);
                retVal.SignatureMandateCompany_Mandatetype = item.MandateType;
                retVal.SignatureMandateCompany_Description = item.Description;
                retVal.SignatureMandateCompany_NumberofSignatures =Convert.ToInt32( item.NumberofSignatures);
                retVal.SignatureMandateCompany_NumberofSignatures1 =Convert.ToInt32( item.NumberofSignatures1);
                retVal.SignatureMandateCompany_Rights = item.Rights;
                retVal.SignatureMandateCompany_TotalNumberofSignature =Convert.ToInt32( item.TotalNumberofSignature);
                retVal.Status = item.Status;
            }

            return retVal;
        }

        private static string GetSignaturePersonsString(string signaturePersonsGuidValues)
        {
            string retVal = string.Empty;

            if (!string.IsNullOrEmpty(signaturePersonsGuidValues))
            {
                //Need to change after clarification 
                List<SelectListItem> signatoryPersons = ServiceHelper.GetNoteDetailPendingOnUsers();
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
        private static string[] GetAuthorizedSignatoryGroupGuidList(string authorizedSignatoryGroupGuidValues)
        {
            if (!string.IsNullOrEmpty(authorizedSignatoryGroupGuidValues))
                return authorizedSignatoryGroupGuidValues.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            return null;
        }
    }
}
