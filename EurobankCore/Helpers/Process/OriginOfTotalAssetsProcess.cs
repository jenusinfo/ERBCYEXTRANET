using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.Generics;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
	public class OriginOfTotalAssetsProcess
	{
		private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

		private static readonly string _BusinessFinancialFolderName = "Business And Financial Profile";

		private static readonly string _BusinessFinancialRelatedPartyFolderName = "Business Profile";

		private static readonly string _OriginOfTotalAssetsFolderName = "Origin Of Total Assets";
		private static readonly string _OriginOfTotalAssetsDocumentName = "Origin Of Asset";

		public static OriginOfTotalAssetsModel GetOriginOfTotalAssetsModelById(int originOfTotalAssetsId)
		{
			OriginOfTotalAssetsModel retVal = null;
			if(originOfTotalAssetsId > 0)
			{
				retVal = BindOriginOfTotalAssetsModel(GetOriginOfTotalAssetsById(originOfTotalAssetsId));
			}

			return retVal;
		}

		public static List<OriginOfTotalAssetsModel> GetOriginOfTotalAssets(int applicantId)
		{
			List<OriginOfTotalAssetsModel> retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if(applicantId > 0)
			{
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(PersonalDetails.CLASS_NAME)
					.WhereEquals("PersonalDetailsID", applicantId)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					TreeNode businessFinancialRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						businessFinancialRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialFolderName, StringComparison.OrdinalIgnoreCase));

						if(businessFinancialRoot != null && businessFinancialRoot.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase)))
						{
							TreeNode originOfTotalAssetsRoot = businessFinancialRoot.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase));

							List<TreeNode> originOfTotalAssetsNodes = originOfTotalAssetsRoot.Children.Where(u => u.ClassName == OriginOfTotalAssets.CLASS_NAME).ToList();

							if(originOfTotalAssetsNodes != null && originOfTotalAssetsNodes.Count > 0)
							{
								retVal = new List<OriginOfTotalAssetsModel>();
								originOfTotalAssetsNodes.ForEach(t => {
									OriginOfTotalAssets originOfTotalAssets = OriginOfTotalAssetsProvider.GetOriginOfTotalAssets(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if(originOfTotalAssets != null)
									{
										OriginOfTotalAssetsModel originOfTotalAssetsModel = BindOriginOfTotalAssetsModel(originOfTotalAssets);
										if(originOfTotalAssetsModel != null)
										{
											retVal.Add(originOfTotalAssetsModel);
										}
									}
								});
							}
						}
					}
				}
			}

			if(retVal != null)
			{
				var sortedRecord = GenericSorter<OriginOfTotalAssetsModel>.Sort(retVal, "Id", SortDirection.asc);

				if(sortedRecord != null)
				{
					retVal = sortedRecord.ToList();
				}
			}

			return BindSerialNumber(retVal);
		}

		public static List<OriginOfTotalAssetsModel> GetOriginOfTotalAssetsRelatedParty(int relatedPartyId)
		{
			List<OriginOfTotalAssetsModel> retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if(relatedPartyId > 0)
			{
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(PersonalDetails.CLASS_NAME)
					.WhereEquals("PersonalDetailsID", relatedPartyId)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					TreeNode businessFinancialRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						businessFinancialRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase));

						if(businessFinancialRoot != null && businessFinancialRoot.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase)))
						{
							TreeNode sourceOfIncomeRoot = businessFinancialRoot.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase));

							List<TreeNode> originOfTotalAssetsNodes = sourceOfIncomeRoot.Children.Where(u => u.ClassName == OriginOfTotalAssets.CLASS_NAME).ToList();

							if(originOfTotalAssetsNodes != null && originOfTotalAssetsNodes.Count > 0)
							{
								retVal = new List<OriginOfTotalAssetsModel>();
								originOfTotalAssetsNodes.ForEach(t => {
									OriginOfTotalAssets originOfTotalAssets = OriginOfTotalAssetsProvider.GetOriginOfTotalAssets(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if(originOfTotalAssets != null)
									{
										OriginOfTotalAssetsModel originOfTotalAssetsModel = BindOriginOfTotalAssetsModel(originOfTotalAssets);
										if(originOfTotalAssetsModel != null)
										{
											retVal.Add(originOfTotalAssetsModel);
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

		public static OriginOfTotalAssetsModel SaveOriginOfTotalAssetsModel(int applicantId, OriginOfTotalAssetsModel model)
		{
			OriginOfTotalAssetsModel retVal = null;

			if(model != null)
			{
				var originsOfTotalAssets = ServiceHelper.GetOriginOfTotalAssets();
				if(!string.IsNullOrEmpty(model.OriginOfTotalAssets) && originsOfTotalAssets != null && originsOfTotalAssets.Any(u => u.Value == model.OriginOfTotalAssets))
				{
					model.OriginOfTotalAssetsName = originsOfTotalAssets.FirstOrDefault(u => u.Value == model.OriginOfTotalAssets).Text;
				}
			}
			if(model != null && model.Id > 0)
			{
				OriginOfTotalAssets originOfTotalAssets = GetOriginOfTotalAssetsById(model.Id);
				if(originOfTotalAssets != null)
				{
					OriginOfTotalAssets updatedOriginOfTotalAssets = BindOriginOfTotalAssets(originOfTotalAssets, model);
					if(updatedOriginOfTotalAssets != null)
					{
						updatedOriginOfTotalAssets.Update();
						retVal = BindOriginOfTotalAssetsModel(updatedOriginOfTotalAssets);
					}
				}
			}
			else if(applicantId > 0)
			{
				TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(PersonalDetails.CLASS_NAME)
					.WhereEquals("PersonalDetailsID", applicantId)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					TreeNode businessFinancialRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialFolderName, StringComparison.OrdinalIgnoreCase)))
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

					TreeNode originOfTotalAssetsRoot = null;
					if(businessFinancialRoot.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						originOfTotalAssetsRoot = businessFinancialRoot.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						originOfTotalAssetsRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
						originOfTotalAssetsRoot.DocumentName = _OriginOfTotalAssetsFolderName;
						originOfTotalAssetsRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						originOfTotalAssetsRoot.Insert(businessFinancialRoot);
					}

					OriginOfTotalAssets originOfTotalAssets = BindOriginOfTotalAssets(null, model);
					if(originOfTotalAssets != null && businessFinancialRoot != null)
					{
						//originOfTotalAssets.DocumentName = model.OriginOfTotalAssetsName;
						//originOfTotalAssets.NodeName = model.OriginOfTotalAssetsName;
						originOfTotalAssets.DocumentName = _OriginOfTotalAssetsDocumentName;
						originOfTotalAssets.NodeName = _OriginOfTotalAssetsDocumentName;
						originOfTotalAssets.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						originOfTotalAssets.Insert(originOfTotalAssetsRoot);
						model = BindOriginOfTotalAssetsModel(originOfTotalAssets);
						retVal = model;
					}
				}
			}

			return retVal;
		}

		public static OriginOfTotalAssetsModel SaveOriginOfTotalAssetsRelatedPartyModel(int relatedPartyId, OriginOfTotalAssetsModel model)
		{
			OriginOfTotalAssetsModel retVal = null;

			if(model != null)
			{
				var originsOfTotalAssets = ServiceHelper.GetOriginOfTotalAssets();
				if(!string.IsNullOrEmpty(model.OriginOfTotalAssets) && originsOfTotalAssets != null && originsOfTotalAssets.Any(u => u.Value == model.OriginOfTotalAssets))
				{
					model.OriginOfTotalAssetsName = originsOfTotalAssets.FirstOrDefault(u => u.Value == model.OriginOfTotalAssets).Text;
				}
			}
			if(model != null && model.Id > 0)
			{
				OriginOfTotalAssets originOfTotalAssets = GetOriginOfTotalAssetsById(model.Id);
				if(originOfTotalAssets != null)
				{
					OriginOfTotalAssets updatedOriginOfTotalAssets = BindOriginOfTotalAssets(originOfTotalAssets, model);
					if(updatedOriginOfTotalAssets != null)
					{
						updatedOriginOfTotalAssets.Update();
						model = BindOriginOfTotalAssetsModel(updatedOriginOfTotalAssets);
						retVal = model;
					}
				}
			}
			else if(relatedPartyId > 0)
			{
				TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(PersonalDetails.CLASS_NAME)
					.WhereEquals("PersonalDetailsID", relatedPartyId)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					TreeNode businessFinancialRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase)))
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
					if(businessFinancialRoot.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						sourceOfIncomeRoot = businessFinancialRoot.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						sourceOfIncomeRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
						sourceOfIncomeRoot.DocumentName = _OriginOfTotalAssetsFolderName;
						sourceOfIncomeRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						sourceOfIncomeRoot.Insert(businessFinancialRoot);
					}

					OriginOfTotalAssets sourceOfIncome = BindOriginOfTotalAssets(null, model);
					if(sourceOfIncome != null && businessFinancialRoot != null)
					{
						sourceOfIncome.DocumentName = model.OriginOfTotalAssetsName;
						sourceOfIncome.NodeName = model.OriginOfTotalAssetsName;
						sourceOfIncome.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						sourceOfIncome.Insert(sourceOfIncomeRoot);
						model = BindOriginOfTotalAssetsModel(sourceOfIncome);
						retVal = model;
					}
				}
			}

			return retVal;
		}

		public static OriginOfTotalAssets GetOriginOfTotalAssetsById(int originOfTotalAssetsId)
		{
			OriginOfTotalAssets retVal = null;

			if(originOfTotalAssetsId > 0)
			{
				var originOfTotalAssets = OriginOfTotalAssetsProvider.GetOriginOfTotalAssets();
				if(originOfTotalAssets != null && originOfTotalAssets.Count > 0)
				{
					retVal = originOfTotalAssets.FirstOrDefault(o => o.OriginOfTotalAssetsID == originOfTotalAssetsId);
				}
			}

			return retVal;
		}

		#region Bind Data

		private static OriginOfTotalAssetsModel BindOriginOfTotalAssetsModel(OriginOfTotalAssets item)
		{
			OriginOfTotalAssetsModel retVal = null;

			if(item != null)
			{
				var originsOfTotalAssets = ServiceHelper.GetOriginOfTotalAssets();
				retVal = new OriginOfTotalAssetsModel()
				{
					Id = item.OriginOfTotalAssetsID,
					OriginOfTotalAssets = item.OriginOfTotalAssets_OriginOfTotalAssets != null ? item.OriginOfTotalAssets_OriginOfTotalAssets.ToString() : string.Empty,
					OriginOfTotalAssetsName = ServiceHelper.GetName(ValidationHelper.GetString(item.OriginOfTotalAssets_OriginOfTotalAssets, ""), Constants.ORIGIN_OF_TOTAL_ASSETS),//(originsOfTotalAssets != null && originsOfTotalAssets.Count > 0 && item.OriginOfTotalAssets_OriginOfTotalAssets != null) && originsOfTotalAssets.Any(f => f.Value == item.OriginOfTotalAssets_OriginOfTotalAssets.ToString()) ? originsOfTotalAssets.FirstOrDefault(f => f.Value == item.OriginOfTotalAssets_OriginOfTotalAssets.ToString()).Text : string.Empty,
					SpecifyOtherOrigin = item.OriginOfTotalAssets_SpecifyOtherOrigin,
					AmountOfTotalAsset = item.OriginOfTotalAssets_AmountOfTotalAsset,
					Status = item.Status,
					StatusName = item.Status ? GridRecordStatus.Complete.ToString() : GridRecordStatus.Pending.ToString()
				};
			}

			return retVal;
		}

		private static OriginOfTotalAssets BindOriginOfTotalAssets(OriginOfTotalAssets existOriginOfTotalAssets, OriginOfTotalAssetsModel item)
		{
			OriginOfTotalAssets retVal = new OriginOfTotalAssets();
			if(existOriginOfTotalAssets != null)
			{
				retVal = existOriginOfTotalAssets;
			}
			if(item != null)
			{
				if(!string.IsNullOrEmpty(item.OriginOfTotalAssets))
				{
					retVal.OriginOfTotalAssets_OriginOfTotalAssets = new Guid(item.OriginOfTotalAssets);
				}
				retVal.OriginOfTotalAssets_SpecifyOtherOrigin = item.SpecifyOtherOrigin;
				retVal.OriginOfTotalAssets_AmountOfTotalAsset =Convert.ToDouble( item.AmountOfTotalAsset);
				retVal.Status = item.Status;
			}

			return retVal;
		}

		private static List<OriginOfTotalAssetsModel> BindSerialNumber(List<OriginOfTotalAssetsModel> items)
		{
			List<OriginOfTotalAssetsModel> retVal = null;

			if(items != null && items.Count > 0)
			{
				retVal = items;
				int counter = 1;
				items.ForEach(u => { u.SlNo = counter; counter++; });
			}

			return retVal;
		}

		#endregion

		#region-------------Origin of Total Assests Legal Applicant-----------------------
		public static List<OriginOfTotalAssetsModel> GetOriginOfTotalAssetsLegal(int applicantId)
		{
			List<OriginOfTotalAssetsModel> retVal = null;

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
					TreeNode businessFinancialRoot = null;
					if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						businessFinancialRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialFolderName, StringComparison.OrdinalIgnoreCase));

						if (businessFinancialRoot != null && businessFinancialRoot.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase)))
						{
							TreeNode originOfTotalAssetsRoot = businessFinancialRoot.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase));

							List<TreeNode> originOfTotalAssetsNodes = originOfTotalAssetsRoot.Children.Where(u => u.ClassName == OriginOfTotalAssets.CLASS_NAME).ToList();

							if (originOfTotalAssetsNodes != null && originOfTotalAssetsNodes.Count > 0)
							{
								retVal = new List<OriginOfTotalAssetsModel>();
								originOfTotalAssetsNodes.ForEach(t => {
									OriginOfTotalAssets originOfTotalAssets = OriginOfTotalAssetsProvider.GetOriginOfTotalAssets(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if (originOfTotalAssets != null)
									{
										OriginOfTotalAssetsModel originOfTotalAssetsModel = BindOriginOfTotalAssetsModel(originOfTotalAssets);
										if (originOfTotalAssetsModel != null)
										{
											retVal.Add(originOfTotalAssetsModel);
										}
									}
								});
							}
						}
					}
				}
			}

			if (retVal != null)
			{
				var sortedRecord = GenericSorter<OriginOfTotalAssetsModel>.Sort(retVal, "Id", SortDirection.asc);

				if (sortedRecord != null)
				{
					retVal = sortedRecord.ToList();
				}
			}

			return BindSerialNumber(retVal);
		}
		public static OriginOfTotalAssetsModel SaveOriginOfTotalAssetsModelLegal(int applicantId, OriginOfTotalAssetsModel model)
		{
			OriginOfTotalAssetsModel retVal = null;

			if (model != null)
			{
				var originsOfTotalAssets = ServiceHelper.GetOriginOfTotalAssets();
				if (!string.IsNullOrEmpty(model.OriginOfTotalAssets) && originsOfTotalAssets != null && originsOfTotalAssets.Any(u => u.Value == model.OriginOfTotalAssets))
				{
					model.OriginOfTotalAssetsName = originsOfTotalAssets.FirstOrDefault(u => u.Value == model.OriginOfTotalAssets).Text;
				}
			}
			if (model != null && model.Id > 0)
			{
				OriginOfTotalAssets originOfTotalAssets = GetOriginOfTotalAssetsById(model.Id);
				if (originOfTotalAssets != null)
				{
					OriginOfTotalAssets updatedOriginOfTotalAssets = BindOriginOfTotalAssets(originOfTotalAssets, model);
					if (updatedOriginOfTotalAssets != null)
					{
						updatedOriginOfTotalAssets.Update();
						retVal = BindOriginOfTotalAssetsModel(updatedOriginOfTotalAssets);
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

					TreeNode originOfTotalAssetsRoot = null;
					if (businessFinancialRoot.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						originOfTotalAssetsRoot = businessFinancialRoot.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						originOfTotalAssetsRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
						originOfTotalAssetsRoot.DocumentName = _OriginOfTotalAssetsFolderName;
						originOfTotalAssetsRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						originOfTotalAssetsRoot.Insert(businessFinancialRoot);
					}

					OriginOfTotalAssets originOfTotalAssets = BindOriginOfTotalAssets(null, model);
					if (originOfTotalAssets != null && businessFinancialRoot != null)
					{
						//originOfTotalAssets.DocumentName = model.OriginOfTotalAssetsName;
						//originOfTotalAssets.NodeName = model.OriginOfTotalAssetsName;
						originOfTotalAssets.DocumentName = _OriginOfTotalAssetsDocumentName;
						originOfTotalAssets.NodeName = _OriginOfTotalAssetsDocumentName;
						originOfTotalAssets.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						originOfTotalAssets.Insert(originOfTotalAssetsRoot);
						model = BindOriginOfTotalAssetsModel(originOfTotalAssets);
						retVal = model;
					}
				}
			}

			return retVal;
		}
		#endregion

		#region-------------Origin of Total Assests Legal Related Party-----------------------
		public static List<OriginOfTotalAssetsModel> GetOriginOfTotalAssetsLegalRelatedParty(int relatedPartyId)
		{
			List<OriginOfTotalAssetsModel> retVal = null;

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
					if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						businessFinancialRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _BusinessFinancialFolderName, StringComparison.OrdinalIgnoreCase));

						if (businessFinancialRoot != null && businessFinancialRoot.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase)))
						{
							TreeNode originOfTotalAssetsRoot = businessFinancialRoot.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase));

							List<TreeNode> originOfTotalAssetsNodes = originOfTotalAssetsRoot.Children.Where(u => u.ClassName == OriginOfTotalAssets.CLASS_NAME).ToList();

							if (originOfTotalAssetsNodes != null && originOfTotalAssetsNodes.Count > 0)
							{
								retVal = new List<OriginOfTotalAssetsModel>();
								originOfTotalAssetsNodes.ForEach(t => {
									OriginOfTotalAssets originOfTotalAssets = OriginOfTotalAssetsProvider.GetOriginOfTotalAssets(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if (originOfTotalAssets != null)
									{
										OriginOfTotalAssetsModel originOfTotalAssetsModel = BindOriginOfTotalAssetsModel(originOfTotalAssets);
										if (originOfTotalAssetsModel != null)
										{
											retVal.Add(originOfTotalAssetsModel);
										}
									}
								});
							}
						}
					}
				}
			}

			if (retVal != null)
			{
				var sortedRecord = GenericSorter<OriginOfTotalAssetsModel>.Sort(retVal, "Id", SortDirection.asc);

				if (sortedRecord != null)
				{
					retVal = sortedRecord.ToList();
				}
			}

			return BindSerialNumber(retVal);
		}
		public static OriginOfTotalAssetsModel SaveOriginOfTotalAssetsModelLegalRelatedParty(int relatedPartyId, OriginOfTotalAssetsModel model)
		{
			OriginOfTotalAssetsModel retVal = null;

			if (model != null)
			{
				var originsOfTotalAssets = ServiceHelper.GetOriginOfTotalAssets();
				if (!string.IsNullOrEmpty(model.OriginOfTotalAssets) && originsOfTotalAssets != null && originsOfTotalAssets.Any(u => u.Value == model.OriginOfTotalAssets))
				{
					model.OriginOfTotalAssetsName = originsOfTotalAssets.FirstOrDefault(u => u.Value == model.OriginOfTotalAssets).Text;
				}
			}
			if (model != null && model.Id > 0)
			{
				OriginOfTotalAssets originOfTotalAssets = GetOriginOfTotalAssetsById(model.Id);
				if (originOfTotalAssets != null)
				{
					OriginOfTotalAssets updatedOriginOfTotalAssets = BindOriginOfTotalAssets(originOfTotalAssets, model);
					if (updatedOriginOfTotalAssets != null)
					{
						updatedOriginOfTotalAssets.Update();
						retVal = BindOriginOfTotalAssetsModel(updatedOriginOfTotalAssets);
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

					TreeNode originOfTotalAssetsRoot = null;
					if (businessFinancialRoot.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						originOfTotalAssetsRoot = businessFinancialRoot.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _OriginOfTotalAssetsFolderName, StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						originOfTotalAssetsRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
						originOfTotalAssetsRoot.DocumentName = _OriginOfTotalAssetsFolderName;
						originOfTotalAssetsRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						originOfTotalAssetsRoot.Insert(businessFinancialRoot);
					}

					OriginOfTotalAssets originOfTotalAssets = BindOriginOfTotalAssets(null, model);
					if (originOfTotalAssets != null && businessFinancialRoot != null)
					{
						//originOfTotalAssets.DocumentName = model.OriginOfTotalAssetsName;
						//originOfTotalAssets.NodeName = model.OriginOfTotalAssetsName;
						originOfTotalAssets.DocumentName = _OriginOfTotalAssetsDocumentName;
						originOfTotalAssets.NodeName = _OriginOfTotalAssetsDocumentName;
						originOfTotalAssets.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						originOfTotalAssets.Insert(originOfTotalAssetsRoot);
						model = BindOriginOfTotalAssetsModel(originOfTotalAssets);
						retVal = model;
					}
				}
			}

			return retVal;
		}
		#endregion

	}
}
