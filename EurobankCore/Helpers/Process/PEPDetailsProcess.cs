using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.PEPDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class PEPDetailsProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

        private static readonly string _PepDetailsFolderName = "PEP Details";
        private static readonly string _PepDetailsApplicantFolderName = "PEP Details Applicant";
        private static readonly string _PepAssociatesFolderName = "PEP Details - Family Members / Associates";
        private static readonly string _PepDetailsApplicantDocumentName = "PEP Applicant";
        private static readonly string _PepAssociatesDocumentName = "PEP Associates";
        #region Pep Application 
        public static List<PepApplicantViewModel> GetPepApplicants(IEnumerable<TreeNode> treeNode)
        {
            List<PepApplicantViewModel> retVal = new List<PepApplicantViewModel>();
            foreach (var item in treeNode)
            {
                PepApplicantViewModel pepApplicantViewModel = new PepApplicantViewModel();
                pepApplicantViewModel.PepApplicantID = ValidationHelper.GetInteger(item.GetValue("PepApplicantID"), 0);
                pepApplicantViewModel.PepApplicant_FirstName = ValidationHelper.GetString(item.GetValue("PepApplicant_FirstName"), "");
                pepApplicantViewModel.PepApplicant_Surname = ValidationHelper.GetString(item.GetValue("PepApplicant_Surname"), "");
                pepApplicantViewModel.PepApplicant_Country = ValidationHelper.GetString(item.GetValue("PepApplicant_Country"), "");
                pepApplicantViewModel.PepApplicant_CountryName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(item.GetValue("PepApplicant_Country"), 0));
                pepApplicantViewModel.PepApplicant_PositionOrganization = ValidationHelper.GetString(item.GetValue("PepApplicant_PositionOrganization"), "");
                if (ValidationHelper.GetString(item.GetValue("PepApplicant_Since"), "") != "" && item.GetValue("PepApplicant_Since").ToString() != Constants.DefaultDate)
                {
                    pepApplicantViewModel.PepApplicant_Since = ValidationHelper.GetDateTime(item.GetValue("PepApplicant_Since"), default(DateTime));

                }
                if (ValidationHelper.GetString(item.GetValue("PepApplicant_Untill"), "") != "" && item.GetValue("PepApplicant_Untill").ToString() != Constants.DefaultDate)
                {
                    pepApplicantViewModel.PepApplicant_Untill = ValidationHelper.GetDateTime(item.GetValue("PepApplicant_Untill"), default(DateTime));
                }

                pepApplicantViewModel.StatusName = ValidationHelper.GetBoolean(item.GetValue("PepApplicant_Status"), false) == true ? "Complete" : "Pending";
                retVal.Add(pepApplicantViewModel);
            }
            return retVal;
        }

        public static PepApplicantViewModel SavePepApplicantModel(PepApplicantViewModel model, TreeNode treeNodeData)
        {
            PepApplicantViewModel retVal = new PepApplicantViewModel();

            if (model != null)
            {
                if (treeNodeData != null)
                {
                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                    TreeNode pEPDetailsfoldernode_parent = tree.SelectNodes()
                        .Path(treeNodeData.NodeAliasPath + "/PEP-Details")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                    if (pEPDetailsfoldernode_parent == null)
                    {

                        pEPDetailsfoldernode_parent = TreeNode.New("CMS.Folder", tree);
                        pEPDetailsfoldernode_parent.DocumentName = "PEP Details";
                        pEPDetailsfoldernode_parent.DocumentCulture = "en-US";
                        pEPDetailsfoldernode_parent.Insert(treeNodeData);
                    }
                    TreeNode pEPDetailsApplicantfoldernode_parent = tree.SelectNodes()
                        .Path(pEPDetailsfoldernode_parent.NodeAliasPath + "/PEP-Details-Applicant")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                    if (pEPDetailsApplicantfoldernode_parent == null)
                    {

                        pEPDetailsApplicantfoldernode_parent = TreeNode.New("CMS.Folder", tree);
                        pEPDetailsApplicantfoldernode_parent.DocumentName = "PEP Details Applicant";
                        pEPDetailsApplicantfoldernode_parent.DocumentCulture = "en-US";
                        pEPDetailsApplicantfoldernode_parent.Insert(pEPDetailsfoldernode_parent);
                    }
                    TreeNode pepApplicant = TreeNode.New("Eurobank.PepApplicant", tree);

                    //debitcardDetails.SetValue("AssociatedAccount", model.AssociatedAccount);
                    //pepApplicant.DocumentName = model.PepApplicant_PositionOrganization;
                    pepApplicant.DocumentName = _PepDetailsApplicantDocumentName;
                    pepApplicant.SetValue("PepApplicant_FirstName", model.PepApplicant_FirstName);
                    pepApplicant.SetValue("PepApplicant_Surname", model.PepApplicant_Surname);
                    pepApplicant.SetValue("PepApplicant_PositionOrganization", model.PepApplicant_PositionOrganization);
                    pepApplicant.SetValue("PepApplicant_Country", ValidationHelper.GetInteger(model.PepApplicant_Country, 0));
                    pepApplicant.SetValue("PepApplicant_Since", model.PepApplicant_Since);
                    pepApplicant.SetValue("PepApplicant_Untill", model.PepApplicant_Untill);
                    pepApplicant.SetValue("PepApplicant_Status", model.Status);
                    pepApplicant.Insert(pEPDetailsApplicantfoldernode_parent);
                    retVal.PepApplicantID= ValidationHelper.GetInteger(pepApplicant.GetValue("PepApplicantID"), 0);

                }
            }
            retVal.PepApplicant_CountryName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(model.PepApplicant_Country, 0));
            retVal.PepApplicant_PositionOrganization = model.PepApplicant_PositionOrganization;
            if (model.PepApplicant_Untill != null)
            {
                retVal.PepApplicant_Untill = Convert.ToDateTime(model.PepApplicant_Untill);

            }
            if (model.PepApplicant_Since != null)
            {
                retVal.PepApplicant_Since = Convert.ToDateTime(model.PepApplicant_Since);


            }
            retVal.StatusName = model.Status == true ? "Complete" : "Pending";
            retVal.PepApplicant_FirstName = model.PepApplicant_FirstName;
            retVal.PepApplicant_Surname = model.PepApplicant_Surname;
            return retVal;
        }
        public static PepApplicantViewModel UpdatePepApplicantModel(PepApplicantViewModel model, TreeNode treeNodeData)
        {
            PepApplicantViewModel retVal = new PepApplicantViewModel();


            if (treeNodeData != null)
            {

                //treeNodeData.DocumentName = model.PepApplicant_PositionOrganization;
                //debitcardDetails.SetValue("AssociatedAccount", model.AssociatedAccount);
                treeNodeData.SetValue("PepApplicant_FirstName", model.PepApplicant_FirstName);
                treeNodeData.SetValue("PepApplicant_Surname", model.PepApplicant_Surname);
                treeNodeData.SetValue("PepApplicant_PositionOrganization", model.PepApplicant_PositionOrganization);
                treeNodeData.SetValue("PepApplicant_Country", ValidationHelper.GetInteger(model.PepApplicant_Country, 0));
                treeNodeData.SetValue("PepApplicant_Since", model.PepApplicant_Since);
                treeNodeData.SetValue("PepApplicant_Untill", model.PepApplicant_Untill);
                treeNodeData.SetValue("PepApplicant_Status", model.Status);
                treeNodeData.NodeAlias = treeNodeData.DocumentName;
                treeNodeData.Update();
            }
            retVal.PepApplicant_CountryName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(model.PepApplicant_Country, 0));
            if (model.PepApplicant_Untill != null)
            {
                retVal.PepApplicant_Untill = Convert.ToDateTime(model.PepApplicant_Untill);

            }
            if (model.PepApplicant_Since != null)
            {
                retVal.PepApplicant_Since = Convert.ToDateTime(model.PepApplicant_Since);


            }
            retVal.StatusName = model.Status == true ? "Complete" : "Pending";
            retVal.PepApplicant_FirstName = model.PepApplicant_FirstName;
            retVal.PepApplicant_Surname = model.PepApplicant_Surname;
            return retVal;
        }

        public static List<PepApplicantViewModel> GetPepApplicantsByApplicantId(int applicantId)
        {
            List<PepApplicantViewModel> retVal = null;

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
                    TreeNode pepApplicantRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _PepDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        applicationDetailsNode=applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _PepDetailsFolderName, StringComparison.OrdinalIgnoreCase));
                        pepApplicantRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _PepDetailsApplicantFolderName, StringComparison.OrdinalIgnoreCase));

                        if (pepApplicantRoot != null)
                        {
                            List<TreeNode> pepApplicantNodes = pepApplicantRoot.Children.Where(u => u.ClassName == PepApplicant.CLASS_NAME).ToList();

                            if (pepApplicantNodes != null && pepApplicantNodes.Count > 0)
                            {
                                retVal = new List<PepApplicantViewModel>();
                                pepApplicantNodes.ForEach(t =>
                                {
                                    PepApplicant pepApplicant = PepApplicantProvider.GetPepApplicant(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (pepApplicant != null)
                                    {
                                        PepApplicantViewModel pepApplicantModel = BindPepApplicantModel(pepApplicant);
                                        if (pepApplicantModel != null)
                                        {
                                            retVal.Add(pepApplicantModel);
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
        private static PepApplicantViewModel BindPepApplicantModel(PepApplicant item)
        {
            PepApplicantViewModel retVal = null;

            if (item != null)
            {
                var country = ServiceHelper.GetCountriesWithID();
                retVal = new PepApplicantViewModel()
                {
                    PepApplicantID = item.PepApplicantID,
                    PepApplicant_Country = item.PepApplicant_Country.ToString(),
                    PepApplicant_CountryName = (country != null && country.Count > 0 && item.PepApplicant_Country != null && country.Any(f => f.Value == item.PepApplicant_Country.ToString())) ? country.FirstOrDefault(f => f.Value == item.PepApplicant_Country.ToString()).Text : string.Empty,
                    PepApplicant_FirstName = item.PepApplicant_FirstName,
                    PepApplicant_PositionOrganization = item.PepApplicant_PositionOrganization,
                    PepApplicant_Since = item.PepApplicant_Since,
                    PepApplicant_Untill = item.PepApplicant_Untill,
                    PepApplicant_Surname = item.PepApplicant_Surname,
                    StatusName=item.PepApplicant_Status==true?"Complete":"Pending"

                };
            }
            return retVal;
        }

        public static List<PepApplicantViewModel> GetPepApplicantsRelatedPartyByApplicantId(int applicantId)
        {
            List<PepApplicantViewModel> retVal = null;
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
                    TreeNode pepApplicantRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _PepDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        applicationDetailsNode = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _PepDetailsFolderName, StringComparison.OrdinalIgnoreCase));
                        pepApplicantRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _PepDetailsApplicantFolderName, StringComparison.OrdinalIgnoreCase));

                        if (pepApplicantRoot != null)
                        {
                            List<TreeNode> pepApplicantNodes = pepApplicantRoot.Children.Where(u => u.ClassName == PepApplicant.CLASS_NAME).ToList();

                            if (pepApplicantNodes != null && pepApplicantNodes.Count > 0)
                            {
                                retVal = new List<PepApplicantViewModel>();
                                pepApplicantNodes.ForEach(t =>
                                {
                                    PepApplicant pepApplicant = PepApplicantProvider.GetPepApplicant(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (pepApplicant != null)
                                    {
                                        PepApplicantViewModel pepApplicantModel = BindPepApplicantModel(pepApplicant);
                                        if (pepApplicantModel != null)
                                        {
                                            retVal.Add(pepApplicantModel);
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
        #endregion

        #region Pep Family Application 
        public static List<PepAssociatesViewModel> GetPepAssociates(IEnumerable<TreeNode> treeNode)
        {
            List<PepAssociatesViewModel> retVal = new List<PepAssociatesViewModel>();
            foreach (var item in treeNode)
            {
                PepAssociatesViewModel pepAssociatesViewModel = new PepAssociatesViewModel();
                pepAssociatesViewModel.PepAssociates_FirstName = ValidationHelper.GetString(item.GetValue("PepAssociates_FirstName"), "");
                pepAssociatesViewModel.PepAssociates_Surname = ValidationHelper.GetString(item.GetValue("PepAssociates_Surname"), "");
                pepAssociatesViewModel.PepAssociates_Relationship = ValidationHelper.GetString(item.GetValue("PepAssociates_Relationship"), "");
                pepAssociatesViewModel.PepAssociatesID = ValidationHelper.GetInteger(item.GetValue("PepAssociatesID"), 0);
                pepAssociatesViewModel.PepAssociates_Country = ValidationHelper.GetString(item.GetValue("PepAssociates_Country"), "");
                pepAssociatesViewModel.PepAssociates_CountryName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(item.GetValue("PepAssociates_Country"), 0));
                pepAssociatesViewModel.PepAssociates_RelationshipName = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("PepAssociates_Relationship"), ""), Constants.RELATIONSHIPS);
                pepAssociatesViewModel.PepAssociates_PositionOrganization = ValidationHelper.GetString(item.GetValue("PepAssociates_PositionOrganization"), "");
                if (item.GetValue("PepAssociates_Since") != null && item.GetValue("PepAssociates_Since").ToString() != Constants.DefaultDate)
                {
                    pepAssociatesViewModel.PepAssociates_Since = ValidationHelper.GetDateTime(item.GetValue("PepAssociates_Since"), default(DateTime));
                }
                if (item.GetValue("PepAssociates_Until") != null && item.GetValue("PepAssociates_Until").ToString() != Constants.DefaultDate)
                {
                    pepAssociatesViewModel.PepAssociates_Until = ValidationHelper.GetDateTime(item.GetValue("PepAssociates_Until"), default(DateTime));
                }
                pepAssociatesViewModel.StatusName = ValidationHelper.GetBoolean(item.GetValue("PepAssociates_Status"), false) == true ? "Complete" : "Pending";
                retVal.Add(pepAssociatesViewModel);
            }
            return retVal;
        }
        public static PepAssociatesViewModel SavePepAssociatesModel(PepAssociatesViewModel model, TreeNode treeNodeData)
        {
            PepAssociatesViewModel retVal = new PepAssociatesViewModel();

            if (model != null)
            {
                if (treeNodeData != null)
                {

                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                    TreeNode pEPDetailsfoldernode_parent = tree.SelectNodes()
                        .Path(treeNodeData.NodeAliasPath + "/PEP-Details")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                    if (pEPDetailsfoldernode_parent == null)
                    {

                        pEPDetailsfoldernode_parent = TreeNode.New("CMS.Folder", tree);
                        pEPDetailsfoldernode_parent.DocumentName = "PEP Details";
                        pEPDetailsfoldernode_parent.DocumentCulture = "en-US";
                        pEPDetailsfoldernode_parent.Insert(treeNodeData);
                    }
                    TreeNode pEPDetailsAssociatesfoldernode_parent = tree.SelectNodes()
                        .Path(pEPDetailsfoldernode_parent.NodeAliasPath + "/PEP-Details-Family-Members-Associates")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                    if (pEPDetailsAssociatesfoldernode_parent == null)
                    {

                        pEPDetailsAssociatesfoldernode_parent = TreeNode.New("CMS.Folder", tree);
                        pEPDetailsAssociatesfoldernode_parent.DocumentName = "PEP Details - Family Members / Associates";
                        pEPDetailsAssociatesfoldernode_parent.DocumentCulture = "en-US";
                        pEPDetailsAssociatesfoldernode_parent.Insert(pEPDetailsfoldernode_parent);
                    }

                    TreeNode pepApplicant = TreeNode.New("Eurobank.PepAssociates", tree);

                    //debitcardDetails.SetValue("AssociatedAccount", model.AssociatedAccount);
                    //pepApplicant.DocumentName = model.PepAssociates_FirstName;
                    pepApplicant.DocumentName = _PepAssociatesDocumentName;
                    pepApplicant.SetValue("PepAssociates_FirstName", model.PepAssociates_FirstName);
                    pepApplicant.SetValue("PepAssociates_Surname", model.PepAssociates_Surname);
                    pepApplicant.SetValue("PepAssociates_Relationship", model.PepAssociates_Relationship);
                    pepApplicant.SetValue("PepAssociates_PositionOrganization", model.PepAssociates_PositionOrganization);
                    pepApplicant.SetValue("PepAssociates_Country", ValidationHelper.GetInteger(model.PepAssociates_Country, 0));
                    pepApplicant.SetValue("PepAssociates_Since", model.PepAssociates_Since);
                    pepApplicant.SetValue("PepAssociates_Until", model.PepAssociates_Until);
                    pepApplicant.SetValue("PepAssociates_Status", model.Status);
                    pepApplicant.Insert(pEPDetailsAssociatesfoldernode_parent);
                    retVal.PepAssociatesID= ValidationHelper.GetInteger(pepApplicant.GetValue("PepAssociatesID"), 0);
                }
            }
            retVal.PepAssociates_CountryName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(model.PepAssociates_Country, 0));
            retVal.PepAssociates_RelationshipName = ServiceHelper.GetName(ValidationHelper.GetString(model.PepAssociates_Relationship, ""), Constants.RELATIONSHIPS);
            if (model.PepAssociates_Until != null)
            {
                retVal.PepAssociates_Until = Convert.ToDateTime(model.PepAssociates_Until);

            }
            if (model.PepAssociates_Since != null)
            {
                retVal.PepAssociates_Since = Convert.ToDateTime(model.PepAssociates_Since);
            }
            retVal.StatusName = model.Status == true ? "Complete" : "Pending";
            retVal.PepAssociates_FirstName = model.PepAssociates_FirstName;
            retVal.PepAssociates_Surname = model.PepAssociates_Surname;
            retVal.PepAssociates_PositionOrganization = model.PepAssociates_PositionOrganization;
            return retVal;
        }
        public static PepAssociatesViewModel UpdatePepAssociatesModel(PepAssociatesViewModel model, TreeNode treeNodeData)
        {
            PepAssociatesViewModel retVal = new PepAssociatesViewModel();


            if (treeNodeData != null)
            {

                //treeNodeData.DocumentName = model.PepAssociates_FirstName;
                //debitcardDetails.SetValue("AssociatedAccount", model.AssociatedAccount);
                treeNodeData.SetValue("PepAssociates_FirstName", model.PepAssociates_FirstName);
                treeNodeData.SetValue("PepAssociates_Surname", model.PepAssociates_Surname);
                treeNodeData.SetValue("PepAssociates_Relationship", model.PepAssociates_Relationship);
                treeNodeData.SetValue("PepAssociates_PositionOrganization", model.PepAssociates_PositionOrganization);
                treeNodeData.SetValue("PepAssociates_Country", ValidationHelper.GetInteger(model.PepAssociates_Country, 0));
                treeNodeData.SetValue("PepAssociates_Since", model.PepAssociates_Since);
                treeNodeData.SetValue("PepAssociates_Until", model.PepAssociates_Until);
                treeNodeData.SetValue("PepAssociates_Status", model.Status);
                treeNodeData.NodeAlias = treeNodeData.DocumentName;
                treeNodeData.Update();
            }
            retVal.PepAssociates_CountryName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(model.PepAssociates_Country, 0));
            retVal.PepAssociates_RelationshipName = ServiceHelper.GetName(ValidationHelper.GetString(model.PepAssociates_Relationship, ""), Constants.RELATIONSHIPS);
            if (model.PepAssociates_Until != null)
            {
                retVal.PepAssociates_Until = Convert.ToDateTime(model.PepAssociates_Until);

            }
            if (model.PepAssociates_Since != null)
            {
                retVal.PepAssociates_Since = Convert.ToDateTime(model.PepAssociates_Since);
            }
            retVal.StatusName = model.Status == true ? "Complete" : "Pending";
            return retVal;
        }
        public static List<PepAssociatesViewModel> GetPepAssociatesByApplicantId(int applicantId)
        {
            List<PepAssociatesViewModel> retVal = null;

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
                    TreeNode pepAssociatesRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _PepDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        applicationDetailsNode= applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _PepDetailsFolderName, StringComparison.OrdinalIgnoreCase));
                        pepAssociatesRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _PepAssociatesFolderName, StringComparison.OrdinalIgnoreCase));

                        if (pepAssociatesRoot != null)
                        {
                            List<TreeNode> pepAssociatesNodes = pepAssociatesRoot.Children.Where(u => u.ClassName == PepAssociates.CLASS_NAME).ToList();

                            if (pepAssociatesNodes != null && pepAssociatesNodes.Count > 0)
                            {
                                retVal = new List<PepAssociatesViewModel>();
                                pepAssociatesNodes.ForEach(t =>
                                {
                                    PepAssociates pepAssociates = PepAssociatesProvider.GetPepAssociates(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (pepAssociates != null)
                                    {
                                        PepAssociatesViewModel pepAssociatedModel = BindPepAssociatesModel(pepAssociates);
                                        if (pepAssociatedModel != null)
                                        {
                                            retVal.Add(pepAssociatedModel);
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
        private static PepAssociatesViewModel BindPepAssociatesModel(PepAssociates item)
        {
            PepAssociatesViewModel retVal = null;

            if (item != null)
            {
                var country = ServiceHelper.GetCountriesWithID();
                var relationship= ServiceHelper.GetCommonDropDown(Constants.RELATIONSHIPS);
                retVal = new PepAssociatesViewModel()
                {
                    PepAssociatesID = item.PepAssociatesID,
                    PepAssociates_Country = item.PepAssociates_Country.ToString(),
                    PepAssociates_CountryName = (country != null && country.Count > 0 && item.PepAssociates_Country != null && country.Any(f => f.Value == item.PepAssociates_Country.ToString())) ? country.FirstOrDefault(f => f.Value == item.PepAssociates_Country.ToString()).Text : string.Empty,
                    PepAssociates_FirstName=item.PepAssociates_FirstName,
                    PepAssociates_Surname=item.PepAssociates_Surname,
                    PepAssociates_PositionOrganization=item.PepAssociates_PositionOrganization,
                    PepAssociates_Relationship=item.PepAssociates_Relationship.ToString(),
                    PepAssociates_RelationshipName= (relationship != null && relationship.Count > 0 && item.PepAssociates_Relationship != null && relationship.Any(f => f.Value == item.PepAssociates_Relationship.ToString())) ? relationship.FirstOrDefault(f => f.Value == item.PepAssociates_Relationship.ToString()).Text : string.Empty,
                    PepAssociates_Since=item.PepAssociates_Since,
                    PepAssociates_Until=item.PepAssociates_Until,
                    StatusName=item.PepAssociates_Status==true?"Complete":"Pending"

                };
            }
            return retVal;
        }

        public static List<PepAssociatesViewModel> GetPepAssociatesRelatedPartyByApplicantId(int applicantId)
        {
            List<PepAssociatesViewModel> retVal = null;

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
                    TreeNode pepAssociatesRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _PepDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        applicationDetailsNode = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _PepDetailsFolderName, StringComparison.OrdinalIgnoreCase));
                        pepAssociatesRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _PepAssociatesFolderName, StringComparison.OrdinalIgnoreCase));

                        if (pepAssociatesRoot != null)
                        {
                            List<TreeNode> pepAssociatesNodes = pepAssociatesRoot.Children.Where(u => u.ClassName == PepAssociates.CLASS_NAME).ToList();

                            if (pepAssociatesNodes != null && pepAssociatesNodes.Count > 0)
                            {
                                retVal = new List<PepAssociatesViewModel>();
                                pepAssociatesNodes.ForEach(t =>
                                {
                                    PepAssociates pepAssociates = PepAssociatesProvider.GetPepAssociates(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (pepAssociates != null)
                                    {
                                        PepAssociatesViewModel pepAssociatedModel = BindPepAssociatesModel(pepAssociates);
                                        if (pepAssociatedModel != null)
                                        {
                                            retVal.Add(pepAssociatedModel);
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
        #endregion
    }
}
