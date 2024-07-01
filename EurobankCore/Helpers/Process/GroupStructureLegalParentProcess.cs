using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
	public class GroupStructureLegalParentProcess
	{
		private static readonly string _GroupStructureRoot = "Group Structures";

		public static GroupStructureLegalParentModel GetGroupStructureLegalParentModel(string applicationNumber)
		{
			GroupStructureLegalParentModel retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if(!string.IsNullOrEmpty(applicationNumber))
			{
				TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

				if(applicationDetailsNode != null)
				{
					retVal = new GroupStructureLegalParentModel();
					TreeNode purposeAndActivityNode = applicationDetailsNode.Children.Where(u => u.ClassName == GroupStructureLegalParent.CLASS_NAME).FirstOrDefault();

					if(purposeAndActivityNode != null)
					{
						GroupStructureLegalParent groupStructureLegalParent = GroupStructureLegalParentProvider.GetGroupStructureLegalParent(purposeAndActivityNode.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

						if(groupStructureLegalParent != null)
						{
							retVal = BindGroupStructureLegalParentModel(groupStructureLegalParent);
						}
					}
				}



			}

			return retVal;
		}

		public static GroupStructureLegalParentModel SaveGroupStructureLegalParentModel(string applicationNumber, GroupStructureLegalParentModel groupStructureLegalParentModel)
		{
			GroupStructureLegalParentModel retVal = null;
			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

			if(!string.IsNullOrEmpty(applicationNumber) && groupStructureLegalParentModel != null)
			{
				TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

				if(applicationDetailsNode != null)
				{
					TreeNode groupStructureLegalParentNode = applicationDetailsNode.Children.Where(u => u.ClassName == GroupStructureLegalParent.CLASS_NAME).FirstOrDefault();

					if(groupStructureLegalParentNode != null)
					{
						GroupStructureLegalParent groupStructureLegalParent = GroupStructureLegalParentProvider.GetGroupStructureLegalParent(groupStructureLegalParentNode.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

						if(groupStructureLegalParent != null)
						{
							groupStructureLegalParent = BindGroupStructureLegalParent(groupStructureLegalParentModel, groupStructureLegalParent);
							if(groupStructureLegalParent != null)
							{
								groupStructureLegalParent.DocumentName = _GroupStructureRoot;
								groupStructureLegalParent.Update();
								retVal = groupStructureLegalParentModel;
							}
						}
					}
					else
					{
						GroupStructureLegalParent groupStructureLegalParent = BindGroupStructureLegalParent(groupStructureLegalParentModel, null);
						if(groupStructureLegalParent != null)
						{
							groupStructureLegalParent.DocumentName = _GroupStructureRoot;
							groupStructureLegalParent.NodeName = _GroupStructureRoot;
							groupStructureLegalParent.Insert(applicationDetailsNode);
							retVal = groupStructureLegalParentModel;
						}
					}
				}
			}

			return retVal;
		}

		#region Bind Methods

		private static GroupStructureLegalParent BindGroupStructureLegalParent(GroupStructureLegalParentModel item, GroupStructureLegalParent groupStructureLegalParent)
		{
			GroupStructureLegalParent retVal = new GroupStructureLegalParent();

			if(groupStructureLegalParent == null && item == null)
			{
				retVal = null;
			}

			if(groupStructureLegalParent != null)
			{
				retVal = groupStructureLegalParent;
			}

			if(item != null)
			{
				retVal.DoesTheEntityBelongToAGroup = item.DoesTheEntityBelongToAGroupName;
				retVal.GroupName = item.GroupName;
				retVal.GroupActivities = item.GroupActivities;

			}

			return retVal;
		}

		private static GroupStructureLegalParentModel BindGroupStructureLegalParentModel(GroupStructureLegalParent item)
		{
			GroupStructureLegalParentModel retVal = null;

			if(item != null)
			{
				retVal = new GroupStructureLegalParentModel()
				{
					Id = item.GroupStructureLegalParentID,
					//DoesTheEntityBelongToAGroup = item.DoesTheEntityBelongToAGroup,
					DoesTheEntityBelongToAGroupName = item.DoesTheEntityBelongToAGroup,
					GroupName = item.GroupName,
					GroupActivities = item.GroupActivities
				};
			}

			return retVal;
		}
		#endregion
	}
}
