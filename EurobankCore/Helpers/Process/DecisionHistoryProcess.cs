using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Applications.DecisionHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
	public class DecisionHistoryProcess
	{
		public static List<DecisionHistoryViewModel> GetDecisionHistorys(IEnumerable<TreeNode> treeNode)
		{
			List<DecisionHistoryViewModel> retVal = new List<DecisionHistoryViewModel>();
			foreach(var item in treeNode)
			{
				DecisionHistoryViewModel decisionHistoryViewModel = new DecisionHistoryViewModel();
				decisionHistoryViewModel.DecisionHistoryID = ValidationHelper.GetInteger(item.GetValue("DecisionHistoryID"), 0);
				decisionHistoryViewModel.DecisionHistory_When = Convert.ToDateTime(ValidationHelper.GetString(item.GetValue("DecisionHistory_When"), "")).ToString("dd/MM/yyyy HH:mm:ss");
				decisionHistoryViewModel.DecisionHistory_Stage = ValidationHelper.GetString(item.GetValue("DecisionHistory_Stage"), "");
				decisionHistoryViewModel.DecisionHistory_Who = ValidationHelper.GetString(item.GetValue("DecisionHistory_Who"), "");
				decisionHistoryViewModel.DecisionHistory_Decision = ValidationHelper.GetString(item.GetValue("DecisionHistory_Decision"), "");
				decisionHistoryViewModel.DecisionHistory_DecisionName = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("DecisionHistory_Decision"), ""), Constants.DECISION_TYPE);
				decisionHistoryViewModel.DecisionHistory_Comments = ValidationHelper.GetString(item.GetValue("DecisionHistory_Comments"), "");
				decisionHistoryViewModel.DecisionHistory_EscalateTo = ValidationHelper.GetString(item.GetValue("DecisionHistory_EscalateTo"), "");
				retVal.Add(decisionHistoryViewModel);
			}
			return retVal;
		}

		public static DecisionHistoryViewModel SaveDecisionHistorysModel(DecisionHistoryViewModel model, TreeNode treeNodeData, string username)
		{
			DecisionHistoryViewModel retVal = new DecisionHistoryViewModel();

			if(model != null)
			{
				if(treeNodeData != null)
				{
					TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
					TreeNode DecisionHistoryfoldernode_parent = tree.SelectNodes()
						.Path(treeNodeData.NodeAliasPath + "/Decision-History")
						.OnCurrentSite()
						.Published(false)
						.FirstOrDefault();
					if(DecisionHistoryfoldernode_parent == null)
					{

						DecisionHistoryfoldernode_parent = TreeNode.New("CMS.Folder", tree);
						DecisionHistoryfoldernode_parent.DocumentName = "Decision History";
						DecisionHistoryfoldernode_parent.DocumentCulture = "en-US";
						DecisionHistoryfoldernode_parent.Insert(treeNodeData);
					}
					TreeNode decisionHistory = TreeNode.New("Eurobank.DecisionHistory", tree);
					decisionHistory.DocumentName = ServiceHelper.GetName(ValidationHelper.GetString(model.DecisionHistory_Decision, ""), Constants.DECISION_TYPE);
					//debitcardDetails.SetValue("AssociatedAccount", model.AssociatedAccount);
					decisionHistory.SetValue("DecisionHistory_Decision", model.DecisionHistory_Decision);
					decisionHistory.SetValue("DecisionHistory_Stage", model.DecisionHistory_Stage);
					decisionHistory.SetValue("DecisionHistory_Comments", model.DecisionHistory_Comments);
					decisionHistory.SetValue("DecisionHistory_EscalateTo", model.DecisionHistory_EscalateTo);
					decisionHistory.SetValue("DecisionHistory_When", DateTime.Now);
					decisionHistory.SetValue("DecisionHistory_Who", username);
					decisionHistory.Insert(DecisionHistoryfoldernode_parent);

				}
			}
			retVal.DecisionHistory_Decision= ValidationHelper.GetString(model.DecisionHistory_Decision, "");
			retVal.DecisionHistory_Stage = model.DecisionHistory_Stage;
			retVal.DecisionHistory_Comments = model.DecisionHistory_Comments;
			retVal.DecisionHistory_EscalateTo = model.DecisionHistory_EscalateTo;
			retVal.DecisionHistory_When = model.DecisionHistory_When;
			retVal.DecisionHistory_Who = model.DecisionHistory_Who;
			//retVal.DebitCardDetails_CardTypeName = ServiceHelper.GetName(ValidationHelper.GetString(model.DebitCardDetails_CardType, ""), Constants.CARD_TYPE);
			//retVal.DebitCardDetails_DispatchMethodName = ServiceHelper.GetName(ValidationHelper.GetString(model.DebitCardDetails_DispatchMethod, ""), Constants.DISPATCH_METHOD);
			//retVal.DebitCardDetails_Status = model.DebitCardDetails_Status;
			return retVal;
		}

		
	}
}
