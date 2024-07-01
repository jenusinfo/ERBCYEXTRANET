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
    public class SignatureMandateIndividualProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

        private static readonly string _SignatureMandateIndividualFolderName = "Signature Mandate";

        private static readonly string _SignatureMandateIndividualDocumentName = "Sign Mandate";

        public static List<SignatureMandateIndividualModel> GetSignatureMandateIndividualModels(string applicationNumber)
        {
            List<SignatureMandateIndividualModel> retVal = null;

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
                    retVal = new List<SignatureMandateIndividualModel>();
                    TreeNode signatureMandateIndividualFolderRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatureMandateIndividualFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        signatureMandateIndividualFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatureMandateIndividualFolderName, StringComparison.OrdinalIgnoreCase));

                        if (signatureMandateIndividualFolderRoot != null)
                        {
                            List<TreeNode> signatureMandateIndividualNodes = signatureMandateIndividualFolderRoot.Children.Where(u => u.ClassName == SignatureMandateIndividual.CLASS_NAME).ToList();

                            if (signatureMandateIndividualNodes != null && signatureMandateIndividualNodes.Count > 0)
                            {
                                signatureMandateIndividualNodes.ForEach(t =>
                                {
                                    SignatureMandateIndividual signatureMandateIndividual = SignatureMandateIndividualProvider.GetSignatureMandateIndividual(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (signatureMandateIndividual != null)
                                    {
                                        SignatureMandateIndividualModel signatureMandateIndividualModel = BindSignatureMandateIndividualModel(signatureMandateIndividual, applicationNumber);
                                        if (signatureMandateIndividualModel != null)
                                        {
                                            retVal.Add(signatureMandateIndividualModel);
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

        public static SignatureMandateIndividualModel SaveSignatureMandateIndividualModel(string applicationNumber, SignatureMandateIndividualModel model)
        {
            SignatureMandateIndividualModel retVal = null;

            if (model != null && model.Id > 0)
            {
                SignatureMandateIndividual signatureMandateIndividual = GetSignatureMandateIndividualById(model.Id);
                if (signatureMandateIndividual != null)
                {
                    SignatureMandateIndividual updatedSignatureMandateIndividual = BindSignatureMandateIndividual(signatureMandateIndividual, model);
                    if (updatedSignatureMandateIndividual != null)
                    {
                        updatedSignatureMandateIndividual.Update();
                        retVal = BindSignatureMandateIndividualModel(updatedSignatureMandateIndividual, applicationNumber);
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
                    TreeNode signatureMandateIndividualFolderRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatureMandateIndividualFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        signatureMandateIndividualFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatureMandateIndividualFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        signatureMandateIndividualFolderRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        signatureMandateIndividualFolderRoot.DocumentName = _SignatureMandateIndividualFolderName;
                        signatureMandateIndividualFolderRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        signatureMandateIndividualFolderRoot.Insert(applicationDetailsNode);
                    }
                    SignatureMandateIndividual signatureMandateIndividual = BindSignatureMandateIndividual(null, model);
                    if (signatureMandateIndividual != null && signatureMandateIndividualFolderRoot != null)
                    {
                        string documentName = string.Empty;
                        int idCounter = 1;
                        var allSignatureMandateIndividual = SignatureMandateIndividualProvider.GetSignatureMandateIndividuals();
                        if (allSignatureMandateIndividual != null && allSignatureMandateIndividual.Count > 0)
                        {
                            idCounter = allSignatureMandateIndividual.Max(y => y.SignatureMandateIndividualID) + 1;
                        }
                        documentName = _SignatureMandateIndividualFolderName + idCounter;
                        signatureMandateIndividual.DocumentName = documentName;
                        signatureMandateIndividual.NodeName = documentName;
                        signatureMandateIndividual.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        signatureMandateIndividual.Insert(signatureMandateIndividualFolderRoot);
                        model = BindSignatureMandateIndividualModel(signatureMandateIndividual, applicationNumber);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        private static SignatureMandateIndividual GetSignatureMandateIndividualById(int signatureMandateIndividualId)
        {
            SignatureMandateIndividual retVal = null;

            if (signatureMandateIndividualId > 0)
            {
                var signatureMandateIndividual = SignatureMandateIndividualProvider.GetSignatureMandateIndividuals();
                if (signatureMandateIndividual != null && signatureMandateIndividual.Count > 0)
                {
                    retVal = signatureMandateIndividual.FirstOrDefault(o => o.SignatureMandateIndividualID == signatureMandateIndividualId);
                }
            }

            return retVal;
        }

        private static SignatureMandateIndividualModel BindSignatureMandateIndividualModel(SignatureMandateIndividual item,string applicationNumber)
        {
            SignatureMandateIndividualModel retVal = null;

            if (item != null)
            {
                List<SelectListItem> accesssLevels = ServiceHelper.GetAccessRights();

                retVal = new SignatureMandateIndividualModel()
                {
                    Id = item.SignatureMandateIndividualID,
                    SignatoryPersons = item.SignatureMandateIndividual_SignatoryPersons,
                    SignatoryPersonNames = GetSignaturePersonsString(item.SignatureMandateIndividual_SignatoryPersons, applicationNumber),
                    SignatoryPersonsList = GetSignaturePersonsGuidList(item.SignatureMandateIndividual_SignatoryPersons),
                    NumberOfSignatures = item.SignatureMandateIndividual_NumberOfSignatures,
                    AccessRights = item.SignatureMandateIndividual_AccessRights.ToString(),
                    AccessRightsName = (accesssLevels != null && accesssLevels.Any(k => k.Value == item.SignatureMandateIndividual_AccessRights.ToString()) ? accesssLevels.FirstOrDefault(k => k.Value == item.SignatureMandateIndividual_AccessRights.ToString()).Text : string.Empty),
                    AmountFrom = item.SignatureMandateIndividual_AmountFrom,
                    AmountTo = item.SignatureMandateIndividual_AmountTo,
                    SignatureMandateIndividual_Status = item.SignatureMandateIndividual_Status,
                    Status_Name = item.SignatureMandateIndividual_Status == true ? "Complete" : "Pending"
                };
            }

            return retVal;
        }

        private static SignatureMandateIndividual BindSignatureMandateIndividual(SignatureMandateIndividual existSignatureMandateIndividual, SignatureMandateIndividualModel item)
        {
            SignatureMandateIndividual retVal = new SignatureMandateIndividual();
            if (existSignatureMandateIndividual != null)
            {
                retVal = existSignatureMandateIndividual;
            }
            if (item != null)
            {
                retVal.SignatureMandateIndividual_SignatoryPersons = item.SignatoryPersons;
                retVal.SignatureMandateIndividual_NumberOfSignatures = Convert.ToInt32(item.NumberOfSignatures);
                retVal.SignatureMandateIndividual_AmountFrom =Convert.ToDecimal( item.AmountFrom);
                retVal.SignatureMandateIndividual_AmountTo =Convert.ToDecimal( item.AmountTo);
                if (!string.IsNullOrEmpty(item.AccessRights))
                    retVal.SignatureMandateIndividual_AccessRights = new Guid(item.AccessRights);
                retVal.SignatureMandateIndividual_Status = item.SignatureMandateIndividual_Status;

            }

            return retVal;
        }

        private static string GetSignaturePersonsString(string signaturePersonsGuidValues,string applicationNumber)
        {
            string retVal = string.Empty;

            if (!string.IsNullOrEmpty(signaturePersonsGuidValues))
            {
                //Need to change after clarification 
                List<SelectListItem> signatoryPersons = CommonProcess.GetSignatoryPersonIndividual(applicationNumber);
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
