using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.RelatedParty.PartyRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class RelatedPartyRolesProcess
    {
		private static readonly string _ApplicationRootNodePath = "/Applications-(1)";
		public static PartyRolesViewModel GetPartyRolesById(TreeNode treeNode)
		{
			PartyRolesViewModel oPartyRolesViewModel = new PartyRolesViewModel();
			oPartyRolesViewModel.RelatedPartyRolesID = ValidationHelper.GetInteger(treeNode.GetValue("RelatedPartyRolesID"), 0);
			oPartyRolesViewModel.RelatedPartyRoles_IsContactPerson = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsContactPerson"), false);
			oPartyRolesViewModel.RelatedPartyRoles_IsEBankingUser = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsEBankingUser"),false);
			oPartyRolesViewModel.RelatedPartyRoles_HasPowerOfAttorney = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_HasPowerOfAttorney"), false);
			//oPartyRolesViewModel.RelatedPartyRoles_IsSignatory = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsSignatory"), false);
			//oPartyRolesViewModel.RelatedPartyRoles_IsDirector = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsDirector"), false); 
			//oPartyRolesViewModel.RelatedPartyRoles_IsShareholder = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsShareholder"), false);
			//oPartyRolesViewModel.RelatedPartyRoles_IsSecretary = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsSecretary"), false);
			//oPartyRolesViewModel.RelatedPartyRoles_IsPartner = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsPartner"), false);
			return oPartyRolesViewModel;
		}
		public static PartyRolesViewModel SavePartyRoles(PartyRolesViewModel model, TreeNode treeNodeData)
		{
			PartyRolesViewModel retVal = new PartyRolesViewModel();
			if (model != null)
			{
				if (treeNodeData != null)
				{
					TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
					
					TreeNode RelatedPartyRoles = TreeNode.New("Eurobank.RelatedPartyRoles", tree);
					string DocumentName = "Related Party Roles";
					RelatedPartyRoles.DocumentName = ValidationHelper.GetString(DocumentName, "");
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsContactPerson", model.RelatedPartyRoles_IsContactPerson);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsEBankingUser", model.RelatedPartyRoles_IsEBankingUser);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_HasPowerOfAttorney", model.RelatedPartyRoles_HasPowerOfAttorney);
					//RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSignatory", model.RelatedPartyRoles_IsSignatory);
					//RelatedPartyRoles.SetValue("RelatedPartyRoles_IsDirector", model.RelatedPartyRoles_IsDirector);
					//RelatedPartyRoles.SetValue("RelatedPartyRoles_IsShareholder", model.RelatedPartyRoles_IsShareholder);
					//RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSecretary", model.RelatedPartyRoles_IsSecretary);
					//RelatedPartyRoles.SetValue("RelatedPartyRoles_IsPartner", model.RelatedPartyRoles_IsPartner);
					RelatedPartyRoles.Insert(treeNodeData);

				}

			}
			return retVal;
		}
		
		public static PartyRolesViewModel UpdatePartyRoles(PartyRolesViewModel model, TreeNode RelatedPartyRoles)
		{
			PartyRolesViewModel retVal = new PartyRolesViewModel();
			if (model != null)
			{
				if (RelatedPartyRoles != null)
				{
					string DocumentName = "Related Party Roles";
					RelatedPartyRoles.DocumentName = ValidationHelper.GetString(DocumentName, "");
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsContactPerson", model.RelatedPartyRoles_IsContactPerson);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsEBankingUser", model.RelatedPartyRoles_IsEBankingUser);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_HasPowerOfAttorney", model.RelatedPartyRoles_HasPowerOfAttorney);
					//RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSignatory", model.RelatedPartyRoles_IsSignatory);
					//RelatedPartyRoles.SetValue("RelatedPartyRoles_IsDirector", model.RelatedPartyRoles_IsDirector);
					//RelatedPartyRoles.SetValue("RelatedPartyRoles_IsShareholder", model.RelatedPartyRoles_IsShareholder);
					//RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSecretary", model.RelatedPartyRoles_IsSecretary);
					//RelatedPartyRoles.SetValue("RelatedPartyRoles_IsPartner", model.RelatedPartyRoles_IsPartner);
					RelatedPartyRoles.NodeAlias = RelatedPartyRoles.DocumentName;
					RelatedPartyRoles.Update();

				}

			}
			return retVal;
		}
		public static PartyRolesViewModel GetPartyRolesDetailsByApplicantId(int applicantId)
		{
			PartyRolesViewModel retVal = null;

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
					TreeNode partyRolesDetailsNodes = applicationDetailsNode.Children.Where(u => u.ClassName == RelatedPartyRoles.CLASS_NAME).FirstOrDefault();

					if (partyRolesDetailsNodes != null)
					{
						retVal = new PartyRolesViewModel();
						RelatedPartyRoles relatedPartyRoles = RelatedPartyRolesProvider.GetRelatedPartyRoles(partyRolesDetailsNodes.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

						if (relatedPartyRoles != null)
						{
							PartyRolesViewModel partyRolesViewModel = BindRelatedPartyRolesModel(relatedPartyRoles);
							if (partyRolesViewModel != null)
							{
								retVal = partyRolesViewModel;
							}
						}

					}
				}
			}

			return retVal;
		}
		public static PartyRolesViewModel BindRelatedPartyRolesModel(RelatedPartyRoles item)
		{
			
			PartyRolesViewModel retVal = null;
			if (item != null)
			{
				retVal = new PartyRolesViewModel()
				{
					RelatedPartyRolesID=item.RelatedPartyRolesID,
					RelatedPartyRoles_HasPowerOfAttorney=item.RelatedPartyRoles_HasPowerOfAttorney,
					RelatedPartyRoles_IsContactPerson=item.RelatedPartyRoles_IsContactPerson,
					RelatedPartyRoles_IsEBankingUser=item.RelatedPartyRoles_IsEBankingUser
				};
			}

			return retVal;
		}

	}
}
