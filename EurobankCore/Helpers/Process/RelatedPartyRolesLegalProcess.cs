using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Models.Application.RelatedParty.PartyRolesLegal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class RelatedPartyRolesLegalProcess
    {
		private static readonly string _ApplicationRootNodePath = "/Applications-(1)";
		public static PartyRolesLegalViewModel GetPartyRolesLegalById(TreeNode treeNode)
		{
			PartyRolesLegalViewModel oPartyRolesLegalViewModel = new PartyRolesLegalViewModel();
			oPartyRolesLegalViewModel.RelatedPartyRolesLegalID = ValidationHelper.GetInteger(treeNode.GetValue("RelatedPartyRolesLegalID"), 0);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsDirector = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsDirector"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsAlternativeDirector = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsAlternativeDirector"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsSecretary = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsSecretary"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsShareholder = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsShareholder"), false);
		    oPartyRolesLegalViewModel.RelatedPartyRoles_IsUltimateBeneficiaryOwner = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsUltimateBeneficiaryOwner"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsAuthorisedSignatory = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsAuthorisedSignatory"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsAuthorisedPerson = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsAuthorisedPerson"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsDesignatedEBankingUser = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsDesignatedEBankingUser"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsAuthorisedCardholder = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsAuthorisedCardholder"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsAuthorisedContactPerson = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsAuthorisedContactPerson"), false);
			//new
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsAlternateSecretery = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsAlternateSecretery"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsChairmanOfTheBoardOfDirector = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsChairmanOfTheBoardOfDirector"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsTreasurerOfBoardOfDirectors = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsTreasurerOfBoardOfDirectors"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsMemeberOfBoardOfDirectors = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsMemeberOfBoardOfDirectors"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsPartner = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsPartner"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_GeneralPartner = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_GeneralPartner"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_LimitedPartner = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_LimitedPartner"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsPresidentOfCommittee = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsPresidentOfCommittee"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsVicePresidentOfCommittee = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsVicePresidentOfCommittee"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsSecretaryOfCommittee = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsSecretaryOfCommittee"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsTreasurerOfCommittee = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsTreasurerOfCommittee"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsMemeberOfCommittee = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsMemeberOfCommittee"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsTrustee = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsTrustee"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsSettlor = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsSettlor"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsProtector = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsProtector"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsBenificiary = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsBenificiary"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsFounder = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsFounder"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsPresidentOfCouncil = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsPresidentOfCouncil"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsVicePresidentOfCouncil = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsVicePresidentOfCouncil"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsSecretaryOfCouncil = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsSecretaryOfCouncil"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsTreasurerOfCouncil = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsTreasurerOfCouncil"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsMemberOfCouncil = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsMemberOfCouncil"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsFundMlco = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsFundMlco"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsFundAdministrator = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsFundAdministrator"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsManagementCompany = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsManagementCompany"), false);
			oPartyRolesLegalViewModel.RelatedPartyRoles_IsHolderOfManagementShares = ValidationHelper.GetBoolean(treeNode.GetValue("RelatedPartyRoles_IsHolderOfManagementShares"), false);

			return oPartyRolesLegalViewModel;
		}
		public static PartyRolesLegalViewModel SavePartyRolesLegal(PartyRolesLegalViewModel model, TreeNode treeNodeData)
		{
			PartyRolesLegalViewModel retVal = new PartyRolesLegalViewModel();
			if (model != null)
			{
				if (treeNodeData != null)
				{
					TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

					TreeNode RelatedPartyRoles = TreeNode.New("Eurobank.RelatedPartyRolesLegal", tree);
					string DocumentName = "Related Party Roles";
					RelatedPartyRoles.DocumentName = ValidationHelper.GetString(DocumentName, "");
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsDirector", model.RelatedPartyRoles_IsDirector);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAlternativeDirector", model.RelatedPartyRoles_IsAlternativeDirector);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSecretary", model.RelatedPartyRoles_IsSecretary);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsShareholder", model.RelatedPartyRoles_IsShareholder);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsUltimateBeneficiaryOwner", model.RelatedPartyRoles_IsUltimateBeneficiaryOwner);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAuthorisedSignatory", model.RelatedPartyRoles_IsAuthorisedSignatory);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAuthorisedPerson", model.RelatedPartyRoles_IsAuthorisedPerson);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsDesignatedEBankingUser", model.RelatedPartyRoles_IsDesignatedEBankingUser);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAuthorisedCardholder", model.RelatedPartyRoles_IsAuthorisedCardholder);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAuthorisedContactPerson", model.RelatedPartyRoles_IsAuthorisedContactPerson);
					//new
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAlternateSecretery", model.RelatedPartyRoles_IsAlternateSecretery);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsChairmanOfTheBoardOfDirector", model.RelatedPartyRoles_IsChairmanOfTheBoardOfDirector);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector", model.RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector", model.RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsTreasurerOfBoardOfDirectors", model.RelatedPartyRoles_IsTreasurerOfBoardOfDirectors);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsMemeberOfBoardOfDirectors", model.RelatedPartyRoles_IsMemeberOfBoardOfDirectors);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsPartner", model.RelatedPartyRoles_IsPartner);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_GeneralPartner", model.RelatedPartyRoles_GeneralPartner);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_LimitedPartner", model.RelatedPartyRoles_LimitedPartner);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsPresidentOfCommittee", model.RelatedPartyRoles_IsPresidentOfCommittee);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsVicePresidentOfCommittee", model.RelatedPartyRoles_IsVicePresidentOfCommittee);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSecretaryOfCommittee", model.RelatedPartyRoles_IsSecretaryOfCommittee);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsTreasurerOfCommittee", model.RelatedPartyRoles_IsTreasurerOfCommittee);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsMemeberOfCommittee", model.RelatedPartyRoles_IsMemeberOfCommittee);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsTrustee", model.RelatedPartyRoles_IsTrustee);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSettlor", model.RelatedPartyRoles_IsSettlor);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsProtector", model.RelatedPartyRoles_IsProtector);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsBenificiary", model.RelatedPartyRoles_IsBenificiary);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsFounder", model.RelatedPartyRoles_IsFounder);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsPresidentOfCouncil", model.RelatedPartyRoles_IsPresidentOfCouncil);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsVicePresidentOfCouncil", model.RelatedPartyRoles_IsVicePresidentOfCouncil);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSecretaryOfCouncil", model.RelatedPartyRoles_IsSecretaryOfCouncil);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsTreasurerOfCouncil", model.RelatedPartyRoles_IsTreasurerOfCouncil);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsMemberOfCouncil", model.RelatedPartyRoles_IsMemberOfCouncil);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsFundMlco", model.RelatedPartyRoles_IsFundMlco);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsFundAdministrator", model.RelatedPartyRoles_IsFundAdministrator);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsManagementCompany", model.RelatedPartyRoles_IsManagementCompany);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsHolderOfManagementShares", model.RelatedPartyRoles_IsHolderOfManagementShares);
					RelatedPartyRoles.Insert(treeNodeData);

				}

			}
			return retVal;
		}
		public static PartyRolesLegalViewModel SavePartyRolesLegalForIsUBO(TreeNode treeNodeData)
		{
			PartyRolesLegalViewModel retVal = new PartyRolesLegalViewModel();
			
				if (treeNodeData != null)
				{
					TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

					TreeNode RelatedPartyRoles = TreeNode.New("Eurobank.RelatedPartyRolesLegal", tree);
					string DocumentName = "Related Party Roles";
					RelatedPartyRoles.DocumentName = ValidationHelper.GetString(DocumentName, "");
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsUltimateBeneficiaryOwner", true);
					RelatedPartyRoles.Insert(treeNodeData);

				}

			
			return retVal;
		}
		public static PartyRolesLegalViewModel UpdatePartyRolesLegal(PartyRolesLegalViewModel model, TreeNode RelatedPartyRoles)
		{
			PartyRolesLegalViewModel retVal = new PartyRolesLegalViewModel();
			if (model != null)
			{
				if (RelatedPartyRoles != null)
				{
					//Clear existing active roles
					string DocumentName = "Related Party Roles";
					RelatedPartyRoles.DocumentName = ValidationHelper.GetString(DocumentName, "");
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsDirector", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAlternativeDirector", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSecretary", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsShareholder", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsUltimateBeneficiaryOwner", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAuthorisedSignatory", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAuthorisedPerson", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsDesignatedEBankingUser", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAuthorisedCardholder", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAuthorisedContactPerson", false);
					//new
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAlternateSecretery", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsChairmanOfTheBoardOfDirector", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsTreasurerOfBoardOfDirectors", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsMemeberOfBoardOfDirectors", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsPartner", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_GeneralPartner", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_LimitedPartner", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsPresidentOfCommittee", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsVicePresidentOfCommittee", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSecretaryOfCommittee", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsTreasurerOfCommittee", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsMemeberOfCommittee", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsTrustee", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSettlor", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsProtector", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsBenificiary", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsFounder", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsPresidentOfCouncil", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsVicePresidentOfCouncil", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSecretaryOfCouncil", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsTreasurerOfCouncil", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsMemberOfCouncil", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsFundMlco", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsFundAdministrator", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsManagementCompany", false);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsHolderOfManagementShares", false);
					RelatedPartyRoles.NodeAlias = RelatedPartyRoles.DocumentName;
					RelatedPartyRoles.Update();
					//Add Current Roles
					
					RelatedPartyRoles.DocumentName = ValidationHelper.GetString(DocumentName, "");
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsDirector", model.RelatedPartyRoles_IsDirector);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAlternativeDirector", model.RelatedPartyRoles_IsAlternativeDirector);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSecretary", model.RelatedPartyRoles_IsSecretary);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsShareholder", model.RelatedPartyRoles_IsShareholder);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsUltimateBeneficiaryOwner", model.RelatedPartyRoles_IsUltimateBeneficiaryOwner);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAuthorisedSignatory", model.RelatedPartyRoles_IsAuthorisedSignatory);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAuthorisedPerson", model.RelatedPartyRoles_IsAuthorisedPerson);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsDesignatedEBankingUser", model.RelatedPartyRoles_IsDesignatedEBankingUser);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAuthorisedCardholder", model.RelatedPartyRoles_IsAuthorisedCardholder);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAuthorisedContactPerson", model.RelatedPartyRoles_IsAuthorisedContactPerson);
					//new
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsAlternateSecretery", model.RelatedPartyRoles_IsAlternateSecretery);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsChairmanOfTheBoardOfDirector", model.RelatedPartyRoles_IsChairmanOfTheBoardOfDirector);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector", model.RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector", model.RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsTreasurerOfBoardOfDirectors", model.RelatedPartyRoles_IsTreasurerOfBoardOfDirectors);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsMemeberOfBoardOfDirectors", model.RelatedPartyRoles_IsMemeberOfBoardOfDirectors);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsPartner", model.RelatedPartyRoles_IsPartner);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_GeneralPartner", model.RelatedPartyRoles_GeneralPartner);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_LimitedPartner", model.RelatedPartyRoles_LimitedPartner);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsPresidentOfCommittee", model.RelatedPartyRoles_IsPresidentOfCommittee);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsVicePresidentOfCommittee", model.RelatedPartyRoles_IsVicePresidentOfCommittee);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSecretaryOfCommittee", model.RelatedPartyRoles_IsSecretaryOfCommittee);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsTreasurerOfCommittee", model.RelatedPartyRoles_IsTreasurerOfCommittee);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsMemeberOfCommittee", model.RelatedPartyRoles_IsMemeberOfCommittee);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsTrustee", model.RelatedPartyRoles_IsTrustee);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSettlor", model.RelatedPartyRoles_IsSettlor);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsProtector", model.RelatedPartyRoles_IsProtector);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsBenificiary", model.RelatedPartyRoles_IsBenificiary);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsFounder", model.RelatedPartyRoles_IsFounder);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsPresidentOfCouncil", model.RelatedPartyRoles_IsPresidentOfCouncil);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsVicePresidentOfCouncil", model.RelatedPartyRoles_IsVicePresidentOfCouncil);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsSecretaryOfCouncil", model.RelatedPartyRoles_IsSecretaryOfCouncil);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsTreasurerOfCouncil", model.RelatedPartyRoles_IsTreasurerOfCouncil);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsMemberOfCouncil", model.RelatedPartyRoles_IsMemberOfCouncil);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsFundMlco", model.RelatedPartyRoles_IsFundMlco);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsFundAdministrator", model.RelatedPartyRoles_IsFundAdministrator);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsManagementCompany", model.RelatedPartyRoles_IsManagementCompany);
					RelatedPartyRoles.SetValue("RelatedPartyRoles_IsHolderOfManagementShares", model.RelatedPartyRoles_IsHolderOfManagementShares);
					RelatedPartyRoles.NodeAlias = RelatedPartyRoles.DocumentName;
					RelatedPartyRoles.Update();

				}

			}
			return retVal;
		}
		public static PartyRolesLegalViewModel GetPartyRolesDetailsLegalForIndividualById(int personId)
		{
			PartyRolesLegalViewModel retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if (personId > 0)
			{
				
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(PersonalDetails.CLASS_NAME)
					.WhereEquals("PersonalDetailsID", personId)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();
				if (applicationDetailsNode != null)
				{
					TreeNode partyRolesDetailsNodes = applicationDetailsNode.Children.Where(u => u.ClassName == RelatedPartyRolesLegal.CLASS_NAME).FirstOrDefault();

					if (partyRolesDetailsNodes != null)
					{
						retVal = new PartyRolesLegalViewModel();
						RelatedPartyRolesLegal relatedPartyRolesLegal = RelatedPartyRolesLegalProvider.GetRelatedPartyRolesLegal(partyRolesDetailsNodes.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

						if (relatedPartyRolesLegal != null)
						{
							PartyRolesLegalViewModel partyRolesViewModel = BindRelatedPartyRolesModelLegal(relatedPartyRolesLegal);
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
		public static PartyRolesLegalViewModel GetPartyRolesDetailsLegalByApplicantId(int applicantId)
		{
			PartyRolesLegalViewModel retVal = null;

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
					TreeNode partyRolesDetailsNodes = applicationDetailsNode.Children.Where(u => u.ClassName == RelatedPartyRolesLegal.CLASS_NAME).FirstOrDefault();

					if (partyRolesDetailsNodes != null)
					{
						retVal = new PartyRolesLegalViewModel();
						RelatedPartyRolesLegal relatedPartyRolesLegal = RelatedPartyRolesLegalProvider.GetRelatedPartyRolesLegal(partyRolesDetailsNodes.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

						if (relatedPartyRolesLegal != null)
						{
							PartyRolesLegalViewModel partyRolesViewModel = BindRelatedPartyRolesModelLegal(relatedPartyRolesLegal);
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
		public static PartyRolesLegalViewModel BindRelatedPartyRolesModelLegal(RelatedPartyRolesLegal item)
		{

			PartyRolesLegalViewModel retVal = null;
			if (item != null)
			{
				retVal = new PartyRolesLegalViewModel()
				{
					RelatedPartyRolesLegalID=item.RelatedPartyRolesLegalID,
					RelatedPartyRoles_IsDirector=item.RelatedPartyRoles_IsDirector,
					RelatedPartyRoles_IsAlternativeDirector=item.RelatedPartyRoles_IsAlternativeDirector,
					RelatedPartyRoles_IsSecretary=item.RelatedPartyRoles_IsSecretary,
					RelatedPartyRoles_IsShareholder=item.RelatedPartyRoles_IsShareholder,
					RelatedPartyRoles_IsUltimateBeneficiaryOwner=item.RelatedPartyRoles_IsUltimateBeneficiaryOwner,
					RelatedPartyRoles_IsAuthorisedSignatory=item.RelatedPartyRoles_IsAuthorisedSignatory,
					RelatedPartyRoles_IsAuthorisedPerson=item.RelatedPartyRoles_IsAuthorisedPerson,
					RelatedPartyRoles_IsDesignatedEBankingUser=item.RelatedPartyRoles_IsDesignatedEBankingUser,
					RelatedPartyRoles_IsAuthorisedCardholder=item.RelatedPartyRoles_IsAuthorisedCardholder,
					RelatedPartyRoles_IsAuthorisedContactPerson=item.RelatedPartyRoles_IsAuthorisedContactPerson,
					RelatedPartyRoles_GeneralPartner=item.RelatedPartyRoles_GeneralPartner,
					RelatedPartyRoles_IsAlternateSecretery=item.RelatedPartyRoles_IsAlternateSecretery,
					RelatedPartyRoles_IsBenificiary=item.RelatedPartyRoles_IsBenificiary,
					RelatedPartyRoles_IsChairmanOfTheBoardOfDirector=item.RelatedPartyRoles_IsChairmanOfTheBoardOfDirector,
					RelatedPartyRoles_IsFounder=item.RelatedPartyRoles_IsFounder,
					RelatedPartyRoles_IsFundAdministrator=item.RelatedPartyRoles_IsFundAdministrator,
					RelatedPartyRoles_IsFundMlco=item.RelatedPartyRoles_IsFundMlco,
					RelatedPartyRoles_IsHolderOfManagementShares=item.RelatedPartyRoles_IsHolderOfManagementShares,
					RelatedPartyRoles_IsManagementCompany=item.RelatedPartyRoles_IsManagementCompany,
					RelatedPartyRoles_IsMemberOfCouncil=item.RelatedPartyRoles_IsMemberOfCouncil,
					RelatedPartyRoles_IsMemeberOfBoardOfDirectors=item.RelatedPartyRoles_IsMemeberOfBoardOfDirectors,
					RelatedPartyRoles_IsMemeberOfCommittee=item.RelatedPartyRoles_IsMemeberOfCommittee,
					RelatedPartyRoles_IsPartner=item.RelatedPartyRoles_IsPartner,
					RelatedPartyRoles_IsPresidentOfCommittee=item.RelatedPartyRoles_IsPresidentOfCommittee,
					RelatedPartyRoles_IsPresidentOfCouncil=item.RelatedPartyRoles_IsPresidentOfCouncil,
					RelatedPartyRoles_IsProtector=item.RelatedPartyRoles_IsProtector,
					RelatedPartyRoles_IsSecretaryOfCommittee=item.RelatedPartyRoles_IsSecretaryOfCommittee,
					RelatedPartyRoles_IsSecretaryOfCouncil=item.RelatedPartyRoles_IsSecretaryOfCouncil,
					RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector=item.RelatedPartyRoles_IsSecretaryOfTheBoardOfDirector,
					RelatedPartyRoles_IsSettlor=item.RelatedPartyRoles_IsSettlor,
					RelatedPartyRoles_IsTreasurerOfBoardOfDirectors=item.RelatedPartyRoles_IsTreasurerOfBoardOfDirectors,
					RelatedPartyRoles_IsTreasurerOfCommittee=item.RelatedPartyRoles_IsTreasurerOfCommittee,
					RelatedPartyRoles_IsTreasurerOfCouncil=item.RelatedPartyRoles_IsTreasurerOfCouncil,
					RelatedPartyRoles_IsTrustee=item.RelatedPartyRoles_IsTrustee,
					RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector=item.RelatedPartyRoles_IsViceChairmanOfTheBoardOfDirector,
					RelatedPartyRoles_IsVicePresidentOfCommittee=item.RelatedPartyRoles_IsVicePresidentOfCommittee,
					RelatedPartyRoles_IsVicePresidentOfCouncil=item.RelatedPartyRoles_IsVicePresidentOfCouncil,
					RelatedPartyRoles_LimitedPartner=item.RelatedPartyRoles_LimitedPartner
					
				};
			}

			return retVal;
		}
	}
}
