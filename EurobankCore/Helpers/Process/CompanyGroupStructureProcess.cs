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
	public class CompanyGroupStructureProcess
	{
		private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

		private static readonly string _CompanyGroupStructureFolderName = "Group Structure";

		private static readonly string _CompanyGroupStructureDcoumentName = "Group Struct";

		//public static CompanyGroupStructureModel GetCompanyGroupStructureModelById(int companyGroupStructureId)
		//{
		//	CompanyGroupStructureModel retVal = null;
		//	if(companyGroupStructureId > 0)
		//	{
		//		retVal = BindCompanyGroupStructureModel(GetCompanyGroupStructureById(companyGroupStructureId));
		//	}

		//	return retVal;
		//}
		#region------For Parent DDl------
		public static List<SelectListItem> GetCompanyGroupStructureParent(int applicantId)
		{
			List<SelectListItem> retVal = null;

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
					TreeNode companyGroupStructureRoot = null;
					if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyGroupStructureFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						companyGroupStructureRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyGroupStructureFolderName, StringComparison.OrdinalIgnoreCase));

						if (companyGroupStructureRoot != null)
						{
							List<TreeNode> companyGroupStructureNodes = companyGroupStructureRoot.Children.Where(u => u.ClassName == CompanyGroupStructure.CLASS_NAME).ToList();

							if (companyGroupStructureNodes != null && companyGroupStructureNodes.Count > 0)
							{
								retVal = new List<SelectListItem>();
								companyGroupStructureNodes.ForEach(t => {
									CompanyGroupStructure companyGroupStructure = CompanyGroupStructureProvider.GetCompanyGroupStructure(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if (companyGroupStructure != null)
									{
										SelectListItem companyGroupStructureModel = BindCompanyGroupStructureModelParent(companyGroupStructure);
										if (companyGroupStructureModel != null)
										{
											retVal.Add(companyGroupStructureModel);
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
		private static SelectListItem BindCompanyGroupStructureModelParent(CompanyGroupStructure item)
		{
			SelectListItem retVal = null;

			if (item != null)
			{
				retVal = new SelectListItem()
				{
					Value = item.GroupStructureID.ToString(),
					Text = item.CompanyGroupStructure_EntityName,
					
				};
			}

			return retVal;
		}
		#endregion
		public static List<CompanyGroupStructureModel> GetCompanyGroupStructure(int applicationId, string applicationNumber)
		{
			List<CompanyGroupStructureModel> retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if(applicationId > 0)
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
					TreeNode companyGroupStructureRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.DocumentName, _CompanyGroupStructureFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						companyGroupStructureRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.DocumentName, _CompanyGroupStructureFolderName, StringComparison.OrdinalIgnoreCase));

						if(companyGroupStructureRoot != null)
						{
							List<TreeNode> companyGroupStructureNodes = companyGroupStructureRoot.Children.Where(u => u.ClassName == CompanyGroupStructure.CLASS_NAME).ToList();

							if(companyGroupStructureNodes != null && companyGroupStructureNodes.Count > 0)
							{
								retVal = new List<CompanyGroupStructureModel>();
								companyGroupStructureNodes.ForEach(t => {
									CompanyGroupStructure companyGroupStructure = CompanyGroupStructureProvider.GetCompanyGroupStructure(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if(companyGroupStructure != null)
									{
										CompanyGroupStructureModel companyGroupStructureModel = BindCompanyGroupStructureModel(companyGroupStructure, applicationId, applicationNumber);
										if(companyGroupStructureModel != null)
										{
											retVal.Add(companyGroupStructureModel);
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

		public static List<CompanyGroupStructureModel> GetCompanyGroupStructure(string applicationNumber,int applicationId)
		{
			List<CompanyGroupStructureModel> retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if(!string.IsNullOrEmpty(applicationNumber))
			{
				TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

				if(applicationDetailsNode != null)
				{
					TreeNode companyGroupStructureRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyGroupStructureFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						companyGroupStructureRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyGroupStructureFolderName, StringComparison.OrdinalIgnoreCase));

						if(companyGroupStructureRoot != null)
						{
							List<TreeNode> companyGroupStructureNodes = companyGroupStructureRoot.Children.Where(u => u.ClassName == CompanyGroupStructure.CLASS_NAME).ToList();

							if(companyGroupStructureNodes != null && companyGroupStructureNodes.Count > 0)
							{
								retVal = new List<CompanyGroupStructureModel>();
								companyGroupStructureNodes.ForEach(t => {
									CompanyGroupStructure companyGroupStructure = CompanyGroupStructureProvider.GetCompanyGroupStructure(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if(companyGroupStructure != null)
									{
										CompanyGroupStructureModel companyGroupStructureModel = BindCompanyGroupStructureModel(companyGroupStructure, applicationId, applicationNumber);
										if(companyGroupStructureModel != null)
										{
											retVal.Add(companyGroupStructureModel);
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

		public static CompanyGroupStructureModel SaveCompanyGroupStructureModel(int applicationId, CompanyGroupStructureModel model,string applicationNumber)
		{
			CompanyGroupStructureModel retVal = null;

			if(model != null && model.Id > 0)
			{
				CompanyGroupStructure companyGroupStructure = GetCompanyGroupStructureById(model.Id);
				if(companyGroupStructure != null)
				{
					CompanyGroupStructure updatedCompanyGroupStructure = BindCompanyGroupStructure(companyGroupStructure, model);
					if(updatedCompanyGroupStructure != null)
					{
						updatedCompanyGroupStructure.Update();
						retVal = BindCompanyGroupStructureModel(updatedCompanyGroupStructure, applicationId, applicationNumber);
					}
				}
			}
			else if(applicationId > 0)
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
					TreeNode companyGroupStructureRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyGroupStructureFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						companyGroupStructureRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyGroupStructureFolderName, StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						companyGroupStructureRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
						companyGroupStructureRoot.DocumentName = _CompanyGroupStructureFolderName;
						companyGroupStructureRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						companyGroupStructureRoot.NodeName = _CompanyGroupStructureFolderName;
						companyGroupStructureRoot.Insert(applicationDetailsNode);
					}
					CompanyGroupStructure companyGroupStructure = BindCompanyGroupStructure(null, model);
					if(companyGroupStructure != null && companyGroupStructureRoot != null)
					{
						//companyGroupStructure.DocumentName = model.EntityName;
						//companyGroupStructure.NodeName = model.EntityName;
						companyGroupStructure.DocumentName = _CompanyGroupStructureDcoumentName;
						companyGroupStructure.NodeName = _CompanyGroupStructureDcoumentName;
						
						companyGroupStructure.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;

						companyGroupStructure.Insert(companyGroupStructureRoot);
						model = BindCompanyGroupStructureModel(companyGroupStructure, applicationId, applicationNumber);
						retVal = model;
					}
				}
			}

			return retVal;
		}

		public static CompanyGroupStructureModel SaveCompanyGroupStructureModel(string applicationNumber, CompanyGroupStructureModel model,int applicationId)
		{
			CompanyGroupStructureModel retVal = null;

			if(model != null && model.Id > 0)
			{
				CompanyGroupStructure companyGroupStructure = GetCompanyGroupStructureById(model.Id);
				if(companyGroupStructure != null)
				{
					CompanyGroupStructure updatedCompanyGroupStructure = BindCompanyGroupStructure(companyGroupStructure, model);
					if(updatedCompanyGroupStructure != null)
					{
						updatedCompanyGroupStructure.Update();
						model = BindCompanyGroupStructureModel(updatedCompanyGroupStructure, applicationId, applicationNumber);
					}
				}
			}
			else if(!string.IsNullOrEmpty(applicationNumber))
			{
				TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
				TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

				if(applicationDetailsNode != null)
				{
					TreeNode companyGroupStructureRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyGroupStructureFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						companyGroupStructureRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyGroupStructureFolderName, StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						companyGroupStructureRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
						companyGroupStructureRoot.DocumentName = _CompanyGroupStructureFolderName;
						companyGroupStructureRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						companyGroupStructureRoot.Insert(applicationDetailsNode);
					}
					CompanyGroupStructure companyGroupStructure = BindCompanyGroupStructure(null, model);
					if(companyGroupStructure != null && companyGroupStructureRoot != null)
					{
						companyGroupStructure.DocumentName = model.EntityName;
						companyGroupStructure.NodeName = model.EntityName;
						companyGroupStructure.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;

						companyGroupStructure.Insert(companyGroupStructureRoot);
						model = BindCompanyGroupStructureModel(companyGroupStructure,applicationId,applicationNumber);
						retVal = model;
					}
				}
			}

			return retVal;
		}

		public static CompanyGroupStructure GetCompanyGroupStructureById(int companyGroupStructureId)
		{
			CompanyGroupStructure retVal = null;

			if(companyGroupStructureId > 0)
			{
				var companyGroupStructure = CompanyGroupStructureProvider.GetCompanyGroupStructures();
				if(companyGroupStructure != null && companyGroupStructure.Count > 0)
				{
					retVal = companyGroupStructure.FirstOrDefault(o => o.GroupStructureID == companyGroupStructureId);
				}
			}

			return retVal;
		}

		public static List<SelectListItem> GetGroupStructures()
		{
			List<SelectListItem> retVal = null;

			var companyGroupStructure = CompanyGroupStructureProvider.GetCompanyGroupStructures();
			if(companyGroupStructure != null && companyGroupStructure.Count > 0)
			{
				retVal = companyGroupStructure.Select(t => new SelectListItem() { Value = t.NodeGUID.ToString(), Text = t.CompanyGroupStructure_EntityName }).ToList();
			}

			return retVal;
		}

		#region Bind Data

		private static CompanyGroupStructureModel BindCompanyGroupStructureModel(CompanyGroupStructure item,int applicationId,string applicationNumber)
		{
			CompanyGroupStructureModel retVal = null;

			if(item != null)
			{
				var parents =  CommonProcess.GetDDLGroupStructureParent(applicationNumber, applicationId);//CompanyGroupStructureProcess.GetGroupStructures();
				var entityTypes = ServiceHelper.GetGroupStructureEntityType();
				retVal = new CompanyGroupStructureModel()
				{
					Id = item.GroupStructureID,
					EntityType = item.CompanyGroupStructure_EntityType,
					EntityTypeName = (entityTypes != null && entityTypes.Any(j => string.Equals(j.Value, item.CompanyGroupStructure_EntityType, StringComparison.OrdinalIgnoreCase))) ? entityTypes.FirstOrDefault(j => string.Equals(j.Value, item.CompanyGroupStructure_EntityType, StringComparison.OrdinalIgnoreCase)).Text : string.Empty,
					EntityName = item.CompanyGroupStructure_EntityName,
					//DoestheEntitybelongtoaGroup = item.CompanyGroupStructure_DoestheEntitybelongtoaGroup,
					//GroupName = item.CompanyGroupStructure_GroupName,
					//GroupActivities = item.CompanyGroupStructure_GroupActivities,
					Parent = item.CompanyGroupStructure_Parent,
					ParentName = (parents != null && parents.Any(j => string.Equals(j.Value, item.CompanyGroupStructure_Parent, StringComparison.OrdinalIgnoreCase))) ? parents.FirstOrDefault(j => string.Equals(j.Value, item.CompanyGroupStructure_Parent, StringComparison.OrdinalIgnoreCase)).Text : string.Empty,
					BusinessActivities = item.CompanyGroupStructure_BusinessActivities,
					Status = item.Status,
					StatusName = item.Status ? GridRecordStatus.Complete.ToString() : GridRecordStatus.Pending.ToString()
				};
			}

			return retVal;
		}

		private static CompanyGroupStructure BindCompanyGroupStructure(CompanyGroupStructure existCompanyGroupStructure, CompanyGroupStructureModel item)
		{
			CompanyGroupStructure retVal = new CompanyGroupStructure();
			if(existCompanyGroupStructure != null)
			{
				retVal = existCompanyGroupStructure;
			}
			if(item != null)
			{
				retVal.CompanyGroupStructure_EntityType = item.EntityType;
				retVal.CompanyGroupStructure_EntityName = item.EntityName;
				//retVal.CompanyGroupStructure_DoestheEntitybelongtoaGroup = item.DoestheEntitybelongtoaGroup;
				//retVal.CompanyGroupStructure_GroupName = item.GroupName;
				//retVal.CompanyGroupStructure_GroupActivities = item.GroupActivities;
				retVal.CompanyGroupStructure_Parent = item.Parent;
				retVal.CompanyGroupStructure_BusinessActivities = item.BusinessActivities;
				retVal.Status = item.Status;
			}

			return retVal;
		}

		#endregion
	}
}
