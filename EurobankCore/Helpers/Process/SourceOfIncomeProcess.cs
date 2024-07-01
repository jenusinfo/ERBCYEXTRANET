using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class SourceOfIncomeProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

        private static readonly string _BusinessFinancialFolderName = "Business And Financial Profile";

        private static readonly string _BusinessFinancialRelatedPartyFolderName = "Business Profile";

        private static readonly string _OriginOfAnnualIncomeFolderName = "Origin Of Annual Income";

        private static readonly string _SourceIncomeDocumentName = "Origin Of Annual Income";


        public static SourceOfIncomeModel GetSourceOfIncomeModelById(int sourceOfIncomeId)
        {
            SourceOfIncomeModel retVal = null;
            if (sourceOfIncomeId > 0)
            {
                retVal = BindSourceOfIncomeModel(GetSourceOfIncomeById(sourceOfIncomeId));
            }

            return retVal;
        }

        public static List<SourceOfIncomeModel> GetSourceOfIncome(int applicantId)
        {
            List<SourceOfIncomeModel> retVal = null;

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
                    TreeNode businessFinancialRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        businessFinancialRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialFolderName, StringComparison.OrdinalIgnoreCase));

                        if (businessFinancialRoot != null && businessFinancialRoot.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfAnnualIncomeFolderName, StringComparison.OrdinalIgnoreCase)))
                        {
                            TreeNode sourceOfIncomeRoot = businessFinancialRoot.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfAnnualIncomeFolderName, StringComparison.OrdinalIgnoreCase));

                            List<TreeNode> sourceOfIncomeNodes = sourceOfIncomeRoot.Children.Where(u => u.ClassName == SourceOfIncome.CLASS_NAME).ToList();

                            if (sourceOfIncomeNodes != null && sourceOfIncomeNodes.Count > 0)
                            {
                                retVal = new List<SourceOfIncomeModel>();
                                sourceOfIncomeNodes.ForEach(t =>
                                {
                                    SourceOfIncome sourceOfIncome = SourceOfIncomeProvider.GetSourceOfIncome(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (sourceOfIncome != null)
                                    {
                                        SourceOfIncomeModel sourceOfIncomeModel = BindSourceOfIncomeModel(sourceOfIncome);
                                        if (sourceOfIncomeModel != null)
                                        {
                                            retVal.Add(sourceOfIncomeModel);
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

        public static List<SourceOfIncomeModel> GetSourceOfIncomeRelatedParty(int relatedPartyId)
        {
            List<SourceOfIncomeModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (relatedPartyId > 0)
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(PersonalDetails.CLASS_NAME)
                    .WhereEquals("PersonalDetailsID", relatedPartyId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode businessFinancialRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        businessFinancialRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase));

                        if (businessFinancialRoot != null && businessFinancialRoot.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfAnnualIncomeFolderName, StringComparison.OrdinalIgnoreCase)))
                        {
                            TreeNode sourceOfIncomeRoot = businessFinancialRoot.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfAnnualIncomeFolderName, StringComparison.OrdinalIgnoreCase));

                            List<TreeNode> sourceOfIncomeNodes = sourceOfIncomeRoot.Children.Where(u => u.ClassName == SourceOfIncome.CLASS_NAME).ToList();

                            if (sourceOfIncomeNodes != null && sourceOfIncomeNodes.Count > 0)
                            {
                                retVal = new List<SourceOfIncomeModel>();
                                sourceOfIncomeNodes.ForEach(t =>
                                {
                                    SourceOfIncome sourceOfIncome = SourceOfIncomeProvider.GetSourceOfIncome(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (sourceOfIncome != null)
                                    {
                                        SourceOfIncomeModel sourceOfIncomeModel = BindSourceOfIncomeModel(sourceOfIncome);
                                        if (sourceOfIncomeModel != null)
                                        {
                                            retVal.Add(sourceOfIncomeModel);
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
        public static List<SourceOfIncomeModel> GetSourceOfIncomeRelatedPartyLegal(int relatedPartyId)
        {
            List<SourceOfIncomeModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (relatedPartyId > 0)
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(CompanyDetailsRelatedParty.CLASS_NAME)
                    .WhereEquals("CompanyDetailsRelatedPartyID", relatedPartyId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode businessFinancialRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        businessFinancialRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase));

                        if (businessFinancialRoot != null && businessFinancialRoot.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfAnnualIncomeFolderName, StringComparison.OrdinalIgnoreCase)))
                        {
                            TreeNode sourceOfIncomeRoot = businessFinancialRoot.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfAnnualIncomeFolderName, StringComparison.OrdinalIgnoreCase));

                            List<TreeNode> sourceOfIncomeNodes = sourceOfIncomeRoot.Children.Where(u => u.ClassName == SourceOfIncome.CLASS_NAME).ToList();

                            if (sourceOfIncomeNodes != null && sourceOfIncomeNodes.Count > 0)
                            {
                                retVal = new List<SourceOfIncomeModel>();
                                sourceOfIncomeNodes.ForEach(t =>
                                {
                                    SourceOfIncome sourceOfIncome = SourceOfIncomeProvider.GetSourceOfIncome(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (sourceOfIncome != null)
                                    {
                                        SourceOfIncomeModel sourceOfIncomeModel = BindSourceOfIncomeModel(sourceOfIncome);
                                        if (sourceOfIncomeModel != null)
                                        {
                                            retVal.Add(sourceOfIncomeModel);
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

        public static SourceOfIncomeModel SaveSourceOfIncomeModel(int applicantId, SourceOfIncomeModel model)
        {
            SourceOfIncomeModel retVal = null;

            if (model != null)
            {
                var sourcesOfAnnualIncome = ServiceHelper.GetSourcesOfAnnualIncome();
                if (!string.IsNullOrEmpty(model.SourceOfAnnualIncome) && sourcesOfAnnualIncome != null && sourcesOfAnnualIncome.Any(u => u.Value == model.SourceOfAnnualIncome))
                {
                    model.SourceOfAnnualIncomeName = sourcesOfAnnualIncome.FirstOrDefault(u => u.Value == model.SourceOfAnnualIncome).Text;
                }
            }
            if (model != null && model.Id > 0)
            {
                SourceOfIncome sourceOfIncome = GetSourceOfIncomeById(model.Id);
                if (sourceOfIncome != null)
                {
                    SourceOfIncome updatedSourceOfIncome = BindSourceOfIncome(sourceOfIncome, model);
                    if (updatedSourceOfIncome != null)
                    {
                        updatedSourceOfIncome.Update();
                        model = BindSourceOfIncomeModel(updatedSourceOfIncome);
                        retVal = model;
                    }
                }
            }
            else if (applicantId > 0)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(PersonalDetails.CLASS_NAME)
                    .WhereEquals("PersonalDetailsID", applicantId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode businessFinancialRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        businessFinancialRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        businessFinancialRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        businessFinancialRoot.DocumentName = _BusinessFinancialFolderName;
                        businessFinancialRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        businessFinancialRoot.Insert(applicationDetailsNode);
                    }

                    TreeNode sourceOfIncomeRoot = null;
                    if (businessFinancialRoot.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfAnnualIncomeFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        sourceOfIncomeRoot = businessFinancialRoot.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfAnnualIncomeFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        sourceOfIncomeRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        sourceOfIncomeRoot.DocumentName = _OriginOfAnnualIncomeFolderName;
                        sourceOfIncomeRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        sourceOfIncomeRoot.Insert(businessFinancialRoot);
                    }

                    SourceOfIncome sourceOfIncome = BindSourceOfIncome(null, model);
                    if (sourceOfIncome != null && businessFinancialRoot != null)
                    {
                        //sourceOfIncome.DocumentName = model.SourceOfAnnualIncomeName;
                        //sourceOfIncome.NodeName = model.SourceOfAnnualIncomeName;
                        sourceOfIncome.DocumentName = _SourceIncomeDocumentName;
                        sourceOfIncome.NodeName = _SourceIncomeDocumentName;

                        sourceOfIncome.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        sourceOfIncome.Insert(sourceOfIncomeRoot);
                        model = BindSourceOfIncomeModel(sourceOfIncome);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        public static SourceOfIncomeModel SaveSourceOfIncomeRelatedPartyModel(int relatedPartyId, SourceOfIncomeModel model)
        {
            SourceOfIncomeModel retVal = null;

            if (model != null)
            {
                var sourcesOfAnnualIncome = ServiceHelper.GetSourcesOfAnnualIncome();
                if (!string.IsNullOrEmpty(model.SourceOfAnnualIncome) && sourcesOfAnnualIncome != null && sourcesOfAnnualIncome.Any(u => u.Value == model.SourceOfAnnualIncome))
                {
                    model.SourceOfAnnualIncomeName = sourcesOfAnnualIncome.FirstOrDefault(u => u.Value == model.SourceOfAnnualIncome).Text;
                }
            }
            if (model != null && model.Id > 0)
            {
                SourceOfIncome sourceOfIncome = GetSourceOfIncomeById(model.Id);
                if (sourceOfIncome != null)
                {
                    SourceOfIncome updatedSourceOfIncome = BindSourceOfIncome(sourceOfIncome, model);
                    if (updatedSourceOfIncome != null)
                    {
                        updatedSourceOfIncome.Update();
                        model = BindSourceOfIncomeModel(updatedSourceOfIncome);
                        retVal = model;
                    }
                }
            }
            else if (relatedPartyId > 0)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(PersonalDetails.CLASS_NAME)
                    .WhereEquals("PersonalDetailsID", relatedPartyId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode businessFinancialRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        businessFinancialRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        businessFinancialRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        businessFinancialRoot.DocumentName = _BusinessFinancialRelatedPartyFolderName;
                        businessFinancialRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        businessFinancialRoot.Insert(applicationDetailsNode);
                    }

                    TreeNode sourceOfIncomeRoot = null;
                    if (businessFinancialRoot.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfAnnualIncomeFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        sourceOfIncomeRoot = businessFinancialRoot.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfAnnualIncomeFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        sourceOfIncomeRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        sourceOfIncomeRoot.DocumentName = _OriginOfAnnualIncomeFolderName;
                        sourceOfIncomeRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        sourceOfIncomeRoot.Insert(businessFinancialRoot);
                    }

                    SourceOfIncome sourceOfIncome = BindSourceOfIncome(null, model);
                    if (sourceOfIncome != null && businessFinancialRoot != null)
                    {
                        //sourceOfIncome.DocumentName = model.SourceOfAnnualIncomeName;
                        //sourceOfIncome.NodeName = model.SourceOfAnnualIncomeName;
                        sourceOfIncome.DocumentName = _SourceIncomeDocumentName;
                        sourceOfIncome.NodeName = _SourceIncomeDocumentName;
                        sourceOfIncome.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        sourceOfIncome.Insert(sourceOfIncomeRoot);
                        model = BindSourceOfIncomeModel(sourceOfIncome);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }
        public static SourceOfIncomeModel SaveSourceOfIncomeRelatedPartyLegalModel(int relatedPartyId, SourceOfIncomeModel model)
        {
            SourceOfIncomeModel retVal = null;

            if (model != null)
            {
                var sourcesOfAnnualIncome = ServiceHelper.GetSourcesOfAnnualIncome();
                if (!string.IsNullOrEmpty(model.SourceOfAnnualIncome) && sourcesOfAnnualIncome != null && sourcesOfAnnualIncome.Any(u => u.Value == model.SourceOfAnnualIncome))
                {
                    model.SourceOfAnnualIncomeName = sourcesOfAnnualIncome.FirstOrDefault(u => u.Value == model.SourceOfAnnualIncome).Text;
                }
            }
            if (model != null && model.Id > 0)
            {
                SourceOfIncome sourceOfIncome = GetSourceOfIncomeById(model.Id);
                if (sourceOfIncome != null)
                {
                    SourceOfIncome updatedSourceOfIncome = BindSourceOfIncome(sourceOfIncome, model);
                    if (updatedSourceOfIncome != null)
                    {
                        updatedSourceOfIncome.Update();
                        model = BindSourceOfIncomeModel(updatedSourceOfIncome);
                        retVal = model;
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
                    TreeNode businessFinancialRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        businessFinancialRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        businessFinancialRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        businessFinancialRoot.DocumentName = _BusinessFinancialRelatedPartyFolderName;
                        businessFinancialRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        businessFinancialRoot.Insert(applicationDetailsNode);
                    }

                    TreeNode sourceOfIncomeRoot = null;
                    if (businessFinancialRoot.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfAnnualIncomeFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        sourceOfIncomeRoot = businessFinancialRoot.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfAnnualIncomeFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        sourceOfIncomeRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        sourceOfIncomeRoot.DocumentName = _OriginOfAnnualIncomeFolderName;
                        sourceOfIncomeRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        sourceOfIncomeRoot.Insert(businessFinancialRoot);
                    }

                    SourceOfIncome sourceOfIncome = BindSourceOfIncome(null, model);
                    if (sourceOfIncome != null && businessFinancialRoot != null)
                    {
                        //sourceOfIncome.DocumentName = model.SourceOfAnnualIncomeName;
                        //sourceOfIncome.NodeName = model.SourceOfAnnualIncomeName;
                        sourceOfIncome.DocumentName = _SourceIncomeDocumentName;
                        sourceOfIncome.NodeName = _SourceIncomeDocumentName;
                        sourceOfIncome.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        sourceOfIncome.Insert(sourceOfIncomeRoot);
                        model = BindSourceOfIncomeModel(sourceOfIncome);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        public static SourceOfIncome GetSourceOfIncomeById(int sourceOfIncomeId)
        {
            SourceOfIncome retVal = null;

            if (sourceOfIncomeId > 0)
            {
                var sourceOfIncome = SourceOfIncomeProvider.GetSourceOfIncomes();
                if (sourceOfIncome != null && sourceOfIncome.Count > 0)
                {
                    retVal = sourceOfIncome.FirstOrDefault(o => o.SourceOfIncomeID == sourceOfIncomeId);
                }
            }

            return retVal;
        }

        #region Bind Data

        private static SourceOfIncomeModel BindSourceOfIncomeModel(SourceOfIncome item)
        {
            SourceOfIncomeModel retVal = null;

            if (item != null)
            {
                //var sourcesOfAnnualIncome = ServiceHelper.GetSourcesOfAnnualIncome();
                retVal = new SourceOfIncomeModel()
                {
                    Id = item.SourceOfIncomeID,
                    SourceOfAnnualIncome = item.SourceOfIncome_SourceOfAnnualIncome != null ? item.SourceOfIncome_SourceOfAnnualIncome.ToString() : string.Empty,
                    SourceOfAnnualIncomeName = ServiceHelper.GetName(ValidationHelper.GetString(item.SourceOfIncome_SourceOfAnnualIncome, ""), Constants.SOURCE_OF_ANNUAL_INCOME),// (sourcesOfAnnualIncome != null && sourcesOfAnnualIncome.Count > 0 && item.SourceOfIncome_SourceOfAnnualIncome != null) ? sourcesOfAnnualIncome.FirstOrDefault(f => f.Value == item.SourceOfIncome_SourceOfAnnualIncome.ToString()).Text : string.Empty,
                    SpecifyOtherSource = item.SourceOfIncome_SpecifyOtherSource,
                    AmountOfIncome = item.SourceOfIncome_AmountOfIncome,
                    Status = item.Status,
                    StatusName = item.Status ? GridRecordStatus.Complete.ToString() : GridRecordStatus.Pending.ToString()
                };
            }

            return retVal;
        }

        private static SourceOfIncome BindSourceOfIncome(SourceOfIncome existSourceOfIncome, SourceOfIncomeModel item)
        {
            SourceOfIncome retVal = new SourceOfIncome();
            if (existSourceOfIncome != null)
            {
                retVal = existSourceOfIncome;
            }
            if (item != null)
            {
                if (!string.IsNullOrEmpty(item.SourceOfAnnualIncome))
                {
                    retVal.SourceOfIncome_SourceOfAnnualIncome = new Guid(item.SourceOfAnnualIncome);
                }
                retVal.SourceOfIncome_SpecifyOtherSource = item.SpecifyOtherSource;
                retVal.SourceOfIncome_AmountOfIncome = Convert.ToDouble(item.AmountOfIncome);
                retVal.Status = item.Status;
            }

            return retVal;
        }

        #endregion


    }
}
