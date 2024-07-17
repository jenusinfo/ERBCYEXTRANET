using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Applicant.LegalEntity.CRS;
using Eurobank.Models.Application.Applicant.LegalEntity.FATCACRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class FATCACRSDetailsProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";
        private static readonly string _FATCAFolderName = "FATCA Details";
        private static readonly string _CRSFolderName = "CRS Details";
        #region-------FATCA Details----------
        public static FATCACRSDetailsModel GetFATCACRSDetailsById(TreeNode treeNode)
        {
            FATCACRSDetailsModel oFATCACRSDetailsModel = new FATCACRSDetailsModel();
            if (treeNode != null)
            {
                oFATCACRSDetailsModel.FATCADetailsID = ValidationHelper.GetInteger(treeNode.GetValue("FATCADetailsID"), 0);
                oFATCACRSDetailsModel.FATCADetails_FATCAClassification = ValidationHelper.GetString(treeNode.GetValue("FATCADetails_FATCAClassification"), "");
                oFATCACRSDetailsModel.FATCADetails_USEtityType = ValidationHelper.GetString(treeNode.GetValue("FATCADetails_USEtityType"), "");
                oFATCACRSDetailsModel.FATCADetails_TypeofForeignFinancialInstitution = ValidationHelper.GetString(treeNode.GetValue("FATCADetails_TypeofForeignFinancialInstitution"), "");
                oFATCACRSDetailsModel.FATCADetails_TypeofNonFinancialForeignEntity = ValidationHelper.GetString(treeNode.GetValue("FATCADetails_TypeofNonFinancialForeignEntity"), "");
                oFATCACRSDetailsModel.FATCADetails_GlobalIntermediaryIdentificationNumber = ValidationHelper.GetString(treeNode.GetValue("FATCADetails_GlobalIntermediaryIdentificationNumber"), "");
                oFATCACRSDetailsModel.ExemptionReason = ValidationHelper.GetString(treeNode.GetValue("ExemptionReason"), "");
            }
            return oFATCACRSDetailsModel;
        }
        public static FATCACRSDetailsModel SaveFATCACRSDetails(FATCACRSDetailsModel model, TreeNode treeNodeData)
        {
            FATCACRSDetailsModel retVal = new FATCACRSDetailsModel();
            if (model != null)
            {
                if (treeNodeData != null)
                {
                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                    TreeNode accountsfoldernode_parent = tree.SelectNodes()
                        .Path(treeNodeData.NodeAliasPath + "/FATCA Details")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                    if (accountsfoldernode_parent == null)
                    {

                        accountsfoldernode_parent = TreeNode.New("CMS.Folder", tree);
                        accountsfoldernode_parent.DocumentName = "FATCA Details";
                        accountsfoldernode_parent.DocumentCulture = "en-US";
                        accountsfoldernode_parent.Insert(treeNodeData);
                    }
                    TreeNode FATCADetails = TreeNode.New("Eurobank.FATCADetails", tree);
                    string DocumentName = "FATCA Details";
                    FATCADetails.DocumentName = ValidationHelper.GetString(DocumentName, "");
                    FATCADetails.SetValue("FATCADetails_FATCAClassification", model.FATCADetails_FATCAClassification);
                    FATCADetails.SetValue("FATCADetails_USEtityType", model.FATCADetails_USEtityType);
                    FATCADetails.SetValue("FATCADetails_TypeofForeignFinancialInstitution", model.FATCADetails_TypeofForeignFinancialInstitution);
                    FATCADetails.SetValue("FATCADetails_TypeofNonFinancialForeignEntity", model.FATCADetails_TypeofNonFinancialForeignEntity);
                    FATCADetails.SetValue("FATCADetails_GlobalIntermediaryIdentificationNumber", model.FATCADetails_GlobalIntermediaryIdentificationNumber);
                    FATCADetails.SetValue("ExemptionReason", model.ExemptionReason);
                    FATCADetails.Insert(accountsfoldernode_parent);
                    retVal.FATCADetailsID = ValidationHelper.GetInteger(FATCADetails.GetValue("FATCADetailsID"), 0);
                }

            }
            return retVal;
        }
        public static FATCACRSDetailsModel UpdateFATCACRSDetails(FATCACRSDetailsModel model, TreeNode FATCADetails)
        {
            FATCACRSDetailsModel retVal = new FATCACRSDetailsModel();
            if (model != null)
            {
                if (FATCADetails != null)
                {
                    FATCADetails.SetValue("FATCADetails_FATCAClassification", model.FATCADetails_FATCAClassification);
                    FATCADetails.SetValue("FATCADetails_USEtityType", model.FATCADetails_USEtityType);
                    FATCADetails.SetValue("FATCADetails_TypeofForeignFinancialInstitution", model.FATCADetails_TypeofForeignFinancialInstitution);
                    FATCADetails.SetValue("FATCADetails_TypeofNonFinancialForeignEntity", model.FATCADetails_TypeofNonFinancialForeignEntity);
                    FATCADetails.SetValue("FATCADetails_GlobalIntermediaryIdentificationNumber", model.FATCADetails_GlobalIntermediaryIdentificationNumber);
                    FATCADetails.SetValue("ExemptionReason", model.ExemptionReason);
                    FATCADetails.Update();
                }
            }
            retVal.FATCADetailsID = model.FATCADetailsID;
            return retVal;
        }

        public static FATCACRSDetailsModel GetFATCACRSDetailsModelByApplicantId(int applicantId)
        {
            FATCACRSDetailsModel retVal = null;

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
                    TreeNode fatcaDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _FATCAFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        fatcaDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _FATCAFolderName, StringComparison.OrdinalIgnoreCase));

                        if (fatcaDetailsRoot != null)
                        {
                            TreeNode fatcaDetailsNodes = fatcaDetailsRoot.Children.Where(u => u.ClassName == FATCADetails.CLASS_NAME).FirstOrDefault();

                            if (fatcaDetailsNodes != null)
                            {
                                retVal = new FATCACRSDetailsModel();
                                FATCADetails fATCADetails = FATCADetailsProvider.GetFATCADetails(fatcaDetailsNodes.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                if (fATCADetails != null)
                                {
                                    FATCACRSDetailsModel fATCACRSDetailsModel = BindFATCADetailsModel(fATCADetails);
                                    if (fATCACRSDetailsModel != null)
                                    {
                                        retVal = fATCACRSDetailsModel;
                                    }
                                }

                            }
                        }
                    }
                }
            }

            return retVal;
        }

        private static FATCACRSDetailsModel BindFATCADetailsModel(FATCADetails item)
        {
            FATCACRSDetailsModel retVal = null;

            if (item != null)
            {
                var classification = ServiceHelper.GetFATCA_CLASSIFICATION();
                var entityType = ServiceHelper.GetUS_ENTITY_TYPE();
                var financialInstitution = ServiceHelper.GetTYPE_OF_FOREIGN_FINANCIAL_INSTITUTION();
                var nonFinancialForeignInstitution = ServiceHelper.GetTYPE_OF_NON_FINANCIAL_FOREIGN_ENTITY_NFFE();
                var globalIdentificationNumber = ServiceHelper.GetGLOBAL_INTERMEDIARY_IDENTIFICATION_NUMBER_GIIN();
                var examptionReason = ServiceHelper.GetEXEMPTION_REASON();

                var test = (globalIdentificationNumber != null 
                    && globalIdentificationNumber.Count > 0 
                    && item.FATCADetails_GlobalIntermediaryIdentificationNumber != null 
                    && globalIdentificationNumber.Any(f => f.Value == item.FATCADetails_GlobalIntermediaryIdentificationNumber.ToString())) 
                    ? globalIdentificationNumber.FirstOrDefault(f => f.Value == item.FATCADetails_GlobalIntermediaryIdentificationNumber.ToString()).Text 
                    : string.Empty;


                retVal = new FATCACRSDetailsModel()
                {
                    FATCADetailsID = item.FATCADetailsID,
                    FATCADetails_FATCAClassification = (classification != null && classification.Count > 0 && item.FATCADetails_FATCAClassification != null && classification.Any(f => f.Value == item.FATCADetails_FATCAClassification.ToString())) ? classification.FirstOrDefault(f => f.Value == item.FATCADetails_FATCAClassification.ToString()).Text : string.Empty,
                    FATCADetails_USEtityType= (entityType != null && entityType.Count > 0 && item.FATCADetails_USEtityType != null && entityType.Any(f => f.Value == item.FATCADetails_USEtityType.ToString())) ? entityType.FirstOrDefault(f => f.Value == item.FATCADetails_USEtityType.ToString()).Text : string.Empty,
                    FATCADetails_TypeofForeignFinancialInstitution= (financialInstitution != null && financialInstitution.Count > 0 && item.FATCADetails_TypeofForeignFinancialInstitution != null && financialInstitution.Any(f => f.Value == item.FATCADetails_TypeofForeignFinancialInstitution.ToString())) ? financialInstitution.FirstOrDefault(f => f.Value == item.FATCADetails_TypeofForeignFinancialInstitution.ToString()).Text : string.Empty,
                    FATCADetails_TypeofNonFinancialForeignEntity= (nonFinancialForeignInstitution != null && nonFinancialForeignInstitution.Count > 0 && item.FATCADetails_TypeofNonFinancialForeignEntity != null && nonFinancialForeignInstitution.Any(f => f.Value == item.FATCADetails_TypeofNonFinancialForeignEntity.ToString())) ? nonFinancialForeignInstitution.FirstOrDefault(f => f.Value == item.FATCADetails_TypeofNonFinancialForeignEntity.ToString()).Text : string.Empty,
                    FATCADetails_GlobalIntermediaryIdentificationNumber= (globalIdentificationNumber != null && globalIdentificationNumber.Count > 0 && item.FATCADetails_GlobalIntermediaryIdentificationNumber != null && globalIdentificationNumber.Any(f => f.Value == item.FATCADetails_GlobalIntermediaryIdentificationNumber.ToString())) ? globalIdentificationNumber.FirstOrDefault(f => f.Value == item.FATCADetails_GlobalIntermediaryIdentificationNumber.ToString()).Text : string.Empty,
                    ExemptionReason= string.IsNullOrEmpty(item.ExemptionReason) ? "" : item.ExemptionReason.Trim().ToUpper()
                };
            }
            return retVal;
        }
        #endregion

        #region-------CRS Details----------
        public static CRSDetailsModel GetCRSDetailsById(TreeNode treeNode)
        {
            CRSDetailsModel oCRSDetailsModel = new CRSDetailsModel();
            var activeNonFinancialEntity = ServiceHelper.GetTYPE_OF_ACTIVE_NON_FINANCIAL_ENTITY_NFE();
            if (treeNode != null)
            {
                oCRSDetailsModel.CompanyCRSDetailsID = ValidationHelper.GetInteger(treeNode.GetValue("CompanyCRSDetailsID"), 0);
                oCRSDetailsModel.CompanyCRSDetails_CRSClassification = ValidationHelper.GetString(treeNode.GetValue("CompanyCRSDetails_CRSClassification"), "");
                oCRSDetailsModel.CompanyCRSDetails_TypeofActiveNonFinancialEntity = ValidationHelper.GetString(treeNode.GetValue("CompanyCRSDetails_TypeofActiveNonFinancialEntity"), "");
                oCRSDetailsModel.CompanyCRSDetails_NameofEstablishedSecuritiesMarket = ValidationHelper.GetString(treeNode.GetValue("CompanyCRSDetails_NameofEstablishedSecuritiesMarket"), "");
                oCRSDetailsModel.CompanyCRSDetails_TypeofFinancialInstitution = ValidationHelper.GetString(treeNode.GetValue("CompanyCRSDetails_TypeofFinancialInstitution"), "");
                if(!string.IsNullOrEmpty(oCRSDetailsModel.CompanyCRSDetails_CRSClassification))
                oCRSDetailsModel.CompanyCRSDetails_CRSClassification_Name = ServiceHelper.GetName(oCRSDetailsModel.CompanyCRSDetails_CRSClassification, Constants.CRS_CLASSIFICATION);
                if (!string.IsNullOrEmpty(oCRSDetailsModel.CompanyCRSDetails_TypeofActiveNonFinancialEntity))
                    oCRSDetailsModel.CompanyCRSDetails_TypeofActiveNonFinancialEntity_Name = (activeNonFinancialEntity != null && activeNonFinancialEntity.Count > 0 && oCRSDetailsModel.CompanyCRSDetails_TypeofActiveNonFinancialEntity != null && activeNonFinancialEntity.Any(f => f.Value == oCRSDetailsModel.CompanyCRSDetails_TypeofActiveNonFinancialEntity.ToString())) ? activeNonFinancialEntity.FirstOrDefault(f => f.Value == oCRSDetailsModel.CompanyCRSDetails_TypeofActiveNonFinancialEntity.ToString()).Text : string.Empty;   
                //oCRSDetailsModel.CompanyCRSDetails_TypeofActiveNonFinancialEntity_Name= ServiceHelper.GetName(oCRSDetailsModel.CompanyCRSDetails_TypeofActiveNonFinancialEntity, Constants.TYPE_OF_ACTIVE_NON_FINANCIAL_ENTITY_NFE);
            }
            return oCRSDetailsModel;
        }
        public static CRSDetailsModel SaveCRSDetails(CRSDetailsModel model, TreeNode treeNodeData)
        {
            CRSDetailsModel retVal = new CRSDetailsModel();
            if (model != null)
            {
                if (treeNodeData != null)
                {
                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                    TreeNode accountsfoldernode_parent = tree.SelectNodes()
                        .Path(treeNodeData.NodeAliasPath + "/CRS Details")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                    if (accountsfoldernode_parent == null)
                    {

                        accountsfoldernode_parent = TreeNode.New("CMS.Folder", tree);
                        accountsfoldernode_parent.DocumentName = "CRS Details";
                        accountsfoldernode_parent.DocumentCulture = "en-US";
                        accountsfoldernode_parent.Insert(treeNodeData);
                    }
                    TreeNode CRSDetails = TreeNode.New("Eurobank.CompanyCRSDetails", tree);
                    string DocumentName = "CRS Details";
                    CRSDetails.DocumentName = ValidationHelper.GetString(DocumentName, "");
                    CRSDetails.SetValue("CompanyCRSDetails_CRSClassification", model.CompanyCRSDetails_CRSClassification);
                    CRSDetails.SetValue("CompanyCRSDetails_TypeofActiveNonFinancialEntity", model.CompanyCRSDetails_TypeofActiveNonFinancialEntity);
                    CRSDetails.SetValue("CompanyCRSDetails_NameofEstablishedSecuritiesMarket", model.CompanyCRSDetails_NameofEstablishedSecuritiesMarket);
                    CRSDetails.SetValue("CompanyCRSDetails_TypeofFinancialInstitution", model.CompanyCRSDetails_TypeofFinancialInstitution);
                    CRSDetails.Insert(accountsfoldernode_parent);
                    retVal.CompanyCRSDetailsID = ValidationHelper.GetInteger(CRSDetails.GetValue("CompanyCRSDetailsID"), 0);
                }

            }
            return retVal;
        }
        public static CRSDetailsModel UpdateCRSDetails(CRSDetailsModel model, TreeNode CRSDetails)
        {
            CRSDetailsModel retVal = new CRSDetailsModel();
            if (model != null)
            {
                if (CRSDetails != null)
                {
                    CRSDetails.SetValue("CompanyCRSDetails_CRSClassification", model.CompanyCRSDetails_CRSClassification);
                    CRSDetails.SetValue("CompanyCRSDetails_TypeofActiveNonFinancialEntity", model.CompanyCRSDetails_TypeofActiveNonFinancialEntity);
                    CRSDetails.SetValue("CompanyCRSDetails_NameofEstablishedSecuritiesMarket", model.CompanyCRSDetails_NameofEstablishedSecuritiesMarket);
                    CRSDetails.SetValue("CompanyCRSDetails_TypeofFinancialInstitution", model.CompanyCRSDetails_TypeofFinancialInstitution);
                    CRSDetails.Update();
                }
            }
            retVal.CompanyCRSDetailsID = model.CompanyCRSDetailsID;
            return retVal;
        }

        public static CRSDetailsModel GetCRSDetailsModelByApplicantId(int applicantId)
        {
            CRSDetailsModel retVal = null;

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
                    TreeNode cRSDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CRSFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        cRSDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CRSFolderName, StringComparison.OrdinalIgnoreCase));

                        if (cRSDetailsRoot != null)
                        {
                            TreeNode cRSDetailsNodes = cRSDetailsRoot.Children.Where(u => u.ClassName == CompanyCRSDetails.CLASS_NAME).FirstOrDefault();

                            if (cRSDetailsNodes != null)
                            {
                                retVal = new CRSDetailsModel();
                                CompanyCRSDetails companyCRSDetails = CompanyCRSDetailsProvider.GetCompanyCRSDetails(cRSDetailsNodes.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                if (companyCRSDetails != null)
                                {
                                    CRSDetailsModel cRSDetailsModel = BindCRSDetailsModel(companyCRSDetails);
                                    if (cRSDetailsModel != null)
                                    {
                                        retVal = cRSDetailsModel;
                                    }
                                }

                            }
                        }
                    }
                }
            }

            return retVal;
        }

        private static CRSDetailsModel BindCRSDetailsModel(CompanyCRSDetails item)
        {
            CRSDetailsModel retVal = null;

            if (item != null)
            {
                var classification = ServiceHelper.GetCRS_CLASSIFICATION();
                var activeNonFinancialEntity = ServiceHelper.GetTYPE_OF_ACTIVE_NON_FINANCIAL_ENTITY_NFE();
                var establishedSecurityMarket = ServiceHelper.GetNAME_OF_ESTABLISHED_SECURITES_MARKET();
                var financialInstitution = ServiceHelper.GetTYPE_OF_FINANCIAL_INSITUTION();

                retVal = new CRSDetailsModel()
                {
                    CompanyCRSDetailsID=item.CompanyCRSDetailsID,
                    CompanyCRSDetails_CRSClassification= (classification != null && classification.Count > 0 && item.CompanyCRSDetails_CRSClassification != null && classification.Any(f => f.Value == item.CompanyCRSDetails_CRSClassification.ToString())) ? classification.FirstOrDefault(f => f.Value == item.CompanyCRSDetails_CRSClassification.ToString()).Text : string.Empty,
                    CompanyCRSDetails_TypeofActiveNonFinancialEntity= (activeNonFinancialEntity != null && activeNonFinancialEntity.Count > 0 && item.CompanyCRSDetails_TypeofActiveNonFinancialEntity != null && activeNonFinancialEntity.Any(f => f.Value == item.CompanyCRSDetails_TypeofActiveNonFinancialEntity.ToString())) ? activeNonFinancialEntity.FirstOrDefault(f => f.Value == item.CompanyCRSDetails_TypeofActiveNonFinancialEntity.ToString()).Text : string.Empty,
                    //CompanyCRSDetails_NameofEstablishedSecuritiesMarket= (establishedSecurityMarket != null && establishedSecurityMarket.Count > 0 && item.CompanyCRSDetails_NameofEstablishedSecuritiesMarket != null && establishedSecurityMarket.Any(f => f.Value == item.CompanyCRSDetails_NameofEstablishedSecuritiesMarket.ToString())) ? establishedSecurityMarket.FirstOrDefault(f => f.Value == item.CompanyCRSDetails_NameofEstablishedSecuritiesMarket.ToString()).Text : string.Empty,
                    CompanyCRSDetails_NameofEstablishedSecuritiesMarket= !string.IsNullOrEmpty(item.CompanyCRSDetails_NameofEstablishedSecuritiesMarket) ? item.CompanyCRSDetails_NameofEstablishedSecuritiesMarket : string.Empty,
                    CompanyCRSDetails_TypeofFinancialInstitution = (financialInstitution != null && financialInstitution.Count > 0 && item.CompanyCRSDetails_TypeofFinancialInstitution != null && financialInstitution.Any(f => f.Value == item.CompanyCRSDetails_TypeofFinancialInstitution.ToString())) ? financialInstitution.FirstOrDefault(f => f.Value == item.CompanyCRSDetails_TypeofFinancialInstitution.ToString()).Text : string.Empty,
                };
            }
            return retVal;
        }
        #endregion
    }
}
