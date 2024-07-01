using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Applications.TaxDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class TaxDetailsProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

        private static readonly string _ApplicantsFolderName = "Tax Details";
        private static readonly string _ApplicantsDocumentName = "Tax";
        public static List<TaxDetailsViewModel> GetTaxDetails(IEnumerable<TreeNode> treeNode)
        {
            List<TaxDetailsViewModel> retVal = new List<TaxDetailsViewModel>();
            foreach (var item in treeNode)
            {
                TaxDetailsViewModel taxDetailsViewModel = new TaxDetailsViewModel();
                taxDetailsViewModel.TaxDetailsID = ValidationHelper.GetInteger(item.GetValue("TaxDetailsID"), 0);

                taxDetailsViewModel.TaxDetails_CountryOfTaxResidency = ValidationHelper.GetString(item.GetValue("TaxDetails_CountryOfTaxResidency"), "");
                taxDetailsViewModel.TaxDetails_CountryOfTaxResidencyName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(item.GetValue("TaxDetails_CountryOfTaxResidency"), 0));
                taxDetailsViewModel.TaxDetails_JustificationForTinUnavalability = ValidationHelper.GetString(item.GetValue("TaxDetails_JustificationForTinUnavalability"), "");
                taxDetailsViewModel.TaxDetails_TaxIdentificationNumber = ValidationHelper.GetString(item.GetValue("TaxDetails_TaxIdentificationNumber"), "");
                taxDetailsViewModel.TaxDetails_TinUnavailableReason = ValidationHelper.GetString(item.GetValue("TaxDetails_TinUnavailableReason"), "");
                taxDetailsViewModel.TaxDetails_TinUnavailableReasonName = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("TaxDetails_TinUnavailableReason"), ""), Constants.TIN_UNAVAILABLE_REASON);
                taxDetailsViewModel.StatusName = ValidationHelper.GetBoolean(item.GetValue("TaxDetails_Status"), false) == true ? "Complete" : "Pending";
                retVal.Add(taxDetailsViewModel);
            }
            return retVal;
        }

        public static TaxDetailsViewModel SaveTaxDetailsModel(TaxDetailsViewModel model, TreeNode treeNodeData)
        {
            TaxDetailsViewModel retVal = new TaxDetailsViewModel();

            if (model != null)
            {
                if (treeNodeData != null)
                {
                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                    TreeNode TaxDetailsfoldernode_parent = tree.SelectNodes()
                        .Path(treeNodeData.NodeAliasPath + "/Tax-Details")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                    if (TaxDetailsfoldernode_parent == null)
                    {

                        TaxDetailsfoldernode_parent = TreeNode.New("CMS.Folder", tree);
                        TaxDetailsfoldernode_parent.DocumentName = "Tax Details";
                        TaxDetailsfoldernode_parent.DocumentCulture = "en-US";
                        TaxDetailsfoldernode_parent.Insert(treeNodeData);
                    }
                    TreeNode taxDetails = TreeNode.New("Eurobank.TaxDetails", tree);
                    //taxDetails.DocumentName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(model.TaxDetails_CountryOfTaxResidency, 0));
                    taxDetails.DocumentName = _ApplicantsDocumentName;
                    //debitcardDetails.SetValue("AssociatedAccount", model.AssociatedAccount);
                    taxDetails.SetValue("TaxDetails_CountryOfTaxResidency", model.TaxDetails_CountryOfTaxResidency);
                    taxDetails.SetValue("TaxDetails_JustificationForTinUnavalability", model.TaxDetails_JustificationForTinUnavalability);
                    taxDetails.SetValue("TaxDetails_TaxIdentificationNumber", model.TaxDetails_TaxIdentificationNumber);
                    taxDetails.SetValue("TaxDetails_TinUnavailableReason", model.TaxDetails_TinUnavailableReason);
                    taxDetails.SetValue("TaxDetails_Status", model.Status);

                    taxDetails.Insert(TaxDetailsfoldernode_parent);
                    retVal.TaxDetailsID= ValidationHelper.GetInteger(taxDetails.GetValue("TaxDetailsID"), 0);
                }
            }
            retVal.TaxDetails_CountryOfTaxResidency = model.TaxDetails_CountryOfTaxResidency;
         
            retVal.TaxDetails_CountryOfTaxResidencyName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(model.TaxDetails_CountryOfTaxResidency, 0));
            retVal.TaxDetails_TinUnavailableReasonName = ServiceHelper.GetName(ValidationHelper.GetString(model.TaxDetails_TinUnavailableReason, ""), Constants.TIN_UNAVAILABLE_REASON);
            retVal.StatusName = model.Status == true ? "Complete" : "Pending";
            retVal.TaxDetails_TaxIdentificationNumber = ValidationHelper.GetString(model.TaxDetails_TaxIdentificationNumber,"");
            retVal.TaxDetails_TinUnavailableReason = ValidationHelper.GetString(model.TaxDetails_TinUnavailableReason,"");
            retVal.TaxDetails_JustificationForTinUnavalability = ValidationHelper.GetString(model.TaxDetails_JustificationForTinUnavalability,"");
            return retVal;
        }
        public static TaxDetailsViewModel UpdateTaxDetailsModel(TaxDetailsViewModel model, TreeNode treeNodeData)
        {
            TaxDetailsViewModel retVal = new TaxDetailsViewModel();


            if (treeNodeData != null)
            {

                treeNodeData.DocumentName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(model.TaxDetails_CountryOfTaxResidency, 0));
                //debitcardDetails.SetValue("AssociatedAccount", model.AssociatedAccount);
                treeNodeData.SetValue("TaxDetails_CountryOfTaxResidency", model.TaxDetails_CountryOfTaxResidency);
                treeNodeData.SetValue("TaxDetails_JustificationForTinUnavalability", model.TaxDetails_JustificationForTinUnavalability);
                treeNodeData.SetValue("TaxDetails_TaxIdentificationNumber", model.TaxDetails_TaxIdentificationNumber);
                treeNodeData.SetValue("TaxDetails_TinUnavailableReason", model.TaxDetails_TinUnavailableReason);
                treeNodeData.SetValue("TaxDetails_Status", model.Status);
                treeNodeData.NodeAlias = treeNodeData.DocumentName;
                treeNodeData.Update();


            }
            retVal.TaxDetailsID = model.TaxDetailsID;
            retVal.TaxDetails_CountryOfTaxResidency = model.TaxDetails_CountryOfTaxResidency;
            retVal.TaxDetails_CountryOfTaxResidencyName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(model.TaxDetails_CountryOfTaxResidency, 0));
            retVal.TaxDetails_TinUnavailableReasonName = ServiceHelper.GetName(ValidationHelper.GetString(model.TaxDetails_TinUnavailableReason, ""), Constants.TIN_UNAVAILABLE_REASON);
            retVal.StatusName = model.Status == true ? "Complete" : "Pending";
            retVal.TaxDetails_TaxIdentificationNumber = ValidationHelper.GetString(model.TaxDetails_TaxIdentificationNumber,"");
            retVal.TaxDetails_TinUnavailableReason = ValidationHelper.GetString(model.TaxDetails_TinUnavailableReason,"");
            retVal.TaxDetails_JustificationForTinUnavalability = ValidationHelper.GetString(model.TaxDetails_JustificationForTinUnavalability,"");
            
            return retVal;
        }

        public static List<TaxDetailsViewModel> GetTaxDetailsByApplicantId(int applicantId)
        {
            List<TaxDetailsViewModel> retVal = null;

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
                    TreeNode taxDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        taxDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (taxDetailsRoot != null)
                        {
                            List<TreeNode> taxDetailsNodes = taxDetailsRoot.Children.Where(u => u.ClassName == TaxDetails.CLASS_NAME).ToList();

                            if (taxDetailsNodes != null && taxDetailsNodes.Count > 0)
                            {
                                retVal = new List<TaxDetailsViewModel>();
                                taxDetailsNodes.ForEach(t =>
                                {
                                    TaxDetails taxDetails = TaxDetailsProvider.GetTaxDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (taxDetails != null)
                                    {
                                        TaxDetailsViewModel taxDetailsModel = BindTaxDetailsModel(taxDetails);
                                        if (taxDetailsModel != null)
                                        {
                                            retVal.Add(taxDetailsModel);
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

        private static TaxDetailsViewModel BindTaxDetailsModel(TaxDetails item)
        {
            TaxDetailsViewModel retVal = null;

            if (item != null)
            {
                var country = ServiceHelper.GetCountriesWithID();
                var tinUnavailableReason = ServiceHelper.GetCommonDropDown(Constants.TIN_UNAVAILABLE_REASON);
                retVal = new TaxDetailsViewModel()
                {
                    TaxDetailsID=item.TaxDetailsID,
                    TaxDetails_CountryOfTaxResidency=Convert.ToString( item.TaxDetails_CountryOfTaxResidency),
                    TaxDetails_CountryOfTaxResidencyName=(country != null && country.Count > 0 && item.TaxDetails_CountryOfTaxResidency != null && country.Any(f => f.Value == item.TaxDetails_CountryOfTaxResidency.ToString())) ? country.FirstOrDefault(f => f.Value == item.TaxDetails_CountryOfTaxResidency.ToString()).Text : string.Empty,
                    TaxDetails_JustificationForTinUnavalability = item.TaxDetails_JustificationForTinUnavalability,
                    TaxDetails_TaxIdentificationNumber=item.TaxDetails_TaxIdentificationNumber,
                    TaxDetails_TinUnavailableReason=item.TaxDetails_TinUnavailableReason.ToString(),
                    TaxDetails_TinUnavailableReasonName= (tinUnavailableReason != null && tinUnavailableReason.Count > 0 && item.TaxDetails_TinUnavailableReason != null && tinUnavailableReason.Any(f => f.Value == item.TaxDetails_TinUnavailableReason.ToString())) ? tinUnavailableReason.FirstOrDefault(f => f.Value == item.TaxDetails_TinUnavailableReason.ToString()).Text : string.Empty,
                    StatusName=item.TaxDetails_Status==true?"Complete":"Pending",
                };
            }
            return retVal;
        }

        public static List<TaxDetailsViewModel> GetTaxDetailsLegalByApplicantId(int applicantId)
        {
            List<TaxDetailsViewModel> retVal = null;

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
                    TreeNode taxDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        taxDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (taxDetailsRoot != null)
                        {
                            List<TreeNode> taxDetailsNodes = taxDetailsRoot.Children.Where(u => u.ClassName == TaxDetails.CLASS_NAME).ToList();

                            if (taxDetailsNodes != null && taxDetailsNodes.Count > 0)
                            {
                                retVal = new List<TaxDetailsViewModel>();
                                taxDetailsNodes.ForEach(t =>
                                {
                                    TaxDetails taxDetails = TaxDetailsProvider.GetTaxDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (taxDetails != null)
                                    {
                                        TaxDetailsViewModel taxDetailsModel = BindTaxDetailsModel(taxDetails);
                                        if (taxDetailsModel != null)
                                        {
                                            retVal.Add(taxDetailsModel);
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
    }
}
