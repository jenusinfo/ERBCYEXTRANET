using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Applications.SourceofIncommingTransactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
	public class SourceOfIncomingTransactionsProcess
	{
		private static readonly string _ApplicationRootNodePath = "/Applications-(1)";
		private static readonly string _SourceOfIncomeFolderName = "Source of Incoming Transactions";
		private static readonly string _SourceOfIncomeDocumentName = "Incoming Transaction";
		public static SourceOfIncomingTransactionsViewModel SaveIncomingTransactionsModel(SourceOfIncomingTransactionsViewModel sourceOfIncomingTransactionsViewModel, string nodeAliasPath)
		{
			SourceOfIncomingTransactionsViewModel retVal = new SourceOfIncomingTransactionsViewModel();
			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			
			TreeNode applicationfoldernode_parent = tree.SelectNodes()
				.Path(nodeAliasPath + "/Source-of-Incoming-Transactions")
				.OnCurrentSite()
				.Published(false)
				.FirstOrDefault();
			if(applicationfoldernode_parent == null)
			{
				
				applicationfoldernode_parent = tree.SelectNodes()
				.Path(nodeAliasPath)
				.OnCurrentSite()
				.Published(false)
				.FirstOrDefault();
				TreeNode treeNode =TreeNode.New("CMS.Folder", tree);
				treeNode.DocumentName = "Source of Incoming Transactions";
				treeNode.DocumentCulture = "en-US";
				treeNode.Insert(applicationfoldernode_parent);
				applicationfoldernode_parent = null;
				applicationfoldernode_parent = treeNode;
			}
			if(applicationfoldernode_parent != null)
			{
				CMS.DocumentEngine.TreeNode incomingTransactionsAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.SourceOfIncomingTransactions", tree);
				incomingTransactionsAdd.DocumentName = _SourceOfIncomeDocumentName;
				incomingTransactionsAdd.SetValue("SourceOfIncomingTransactions_CountryOfRemitter", sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitter);
				incomingTransactionsAdd.SetValue("SourceOfIncomingTransactions_CountryOfRemitterBank", sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitterBank);
				incomingTransactionsAdd.SetValue("SourceOfIncomingTransactions_NameOfRemitter", sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_NameOfRemitter);
				incomingTransactionsAdd.SetValue("SourceOfIncomingTransactions_Status", sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_Status);
				incomingTransactionsAdd.Insert(applicationfoldernode_parent);
				retVal.SourceOfIncomingTransactionsID =ValidationHelper.GetInteger( incomingTransactionsAdd.GetValue("SourceOfIncomingTransactionsID"),0);

			}
			//retVal.SourceOfIncomingTransactions_CountryOfRemitterBankName = ServiceHelper.GetCountryName(sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitterBank);
			//retVal.SourceOfIncomingTransactions_CountryOfRemitterName = ServiceHelper.GetCountryName(sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitterName);
			retVal.SourceOfIncomingTransactions_CountryOfRemitterBank = ValidationHelper.GetString(sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitterBank,"");
			retVal.SourceOfIncomingTransactions_CountryOfRemitter = ValidationHelper.GetString(sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitter,"");
			retVal.SourceOfIncomingTransactions_Status = sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_Status;
			return retVal;
		}
		public static SourceOfIncomingTransactionsViewModel UpdateIncomingTransactionsModel(SourceOfIncomingTransactionsViewModel sourceOfIncomingTransactionsViewModel, TreeNode incomingTransactionsAdd)
		{
			SourceOfIncomingTransactionsViewModel retVal = new SourceOfIncomingTransactionsViewModel();

			incomingTransactionsAdd.SetValue("SourceOfIncomingTransactions_CountryOfRemitter", sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitter);
			incomingTransactionsAdd.SetValue("SourceOfIncomingTransactions_CountryOfRemitterBank", sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitterBank);
			incomingTransactionsAdd.SetValue("SourceOfIncomingTransactions_CountryOfRemitter", sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitter);
			incomingTransactionsAdd.SetValue("SourceOfIncomingTransactions_Status", sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_Status);
			incomingTransactionsAdd.Update();
			retVal.SourceOfIncomingTransactions_CountryOfRemitterBank =ValidationHelper.GetString( sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitterBank,"");
			retVal.SourceOfIncomingTransactions_CountryOfRemitter = ValidationHelper.GetString(sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_CountryOfRemitter,"");
			retVal.SourceOfIncomingTransactions_Status = sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactions_Status;
			retVal.SourceOfIncomingTransactionsID = sourceOfIncomingTransactionsViewModel.SourceOfIncomingTransactionsID;
			return retVal;
		}

		public static List<SourceOfIncomingTransactionsViewModel> GetSourceOfIncomeByApplicationID(int applicationID)
		{
			List<SourceOfIncomingTransactionsViewModel> retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if (applicationID > 0)
			{
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(ApplicationDetails.CLASS_NAME)
					.WhereEquals("ApplicationDetailsID", applicationID)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if (applicationDetailsNode != null)
				{
					TreeNode sourceOfIncomingTransactionRoot = null;
					if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SourceOfIncomeFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						sourceOfIncomingTransactionRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SourceOfIncomeFolderName, StringComparison.OrdinalIgnoreCase));

						if (sourceOfIncomingTransactionRoot != null)
						{
							List<TreeNode> sourceOfIncomingTransactionNodes = sourceOfIncomingTransactionRoot.Children.Where(u => u.ClassName == SourceOfIncomingTransactions.CLASS_NAME).ToList();

							if (sourceOfIncomingTransactionNodes != null && sourceOfIncomingTransactionNodes.Count > 0)
							{
								retVal = new List<SourceOfIncomingTransactionsViewModel>();
								sourceOfIncomingTransactionNodes.ForEach(t =>
								{
									SourceOfIncomingTransactions sourceOfIncomingTransactions = SourceOfIncomingTransactionsProvider.GetSourceOfIncomingTransactions(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if (sourceOfIncomingTransactions != null)
									{
										SourceOfIncomingTransactionsViewModel sourceOfIncomeModel = BindSourceOfIncomingTransactionsModel(sourceOfIncomingTransactions);
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
		private static SourceOfIncomingTransactionsViewModel BindSourceOfIncomingTransactionsModel(SourceOfIncomingTransactions item)
		{
			SourceOfIncomingTransactionsViewModel retVal = null;

			if (item != null)
			{
				var country = ServiceHelper.GetCountriesWithID();

				retVal = new SourceOfIncomingTransactionsViewModel()
				{
					SourceOfIncomingTransactionsID = item.SourceOfIncomingTransactionsID,
					SourceOfIncomingTransactions_NameOfRemitter = item.SourceOfIncomingTransactions_NameOfRemitter,
					SourceOfIncomingTransactions_CountryOfRemitter = item.SourceOfIncomingTransactions_CountryOfRemitter,
					SourceOfIncomingTransactions_CountryOfRemitterBank = item.SourceOfIncomingTransactions_CountryOfRemitterBank,
					SourceOfIncomingTransactions_Status = item.SourceOfIncomingTransactions_Status,
					SourceOfIncomingTransactions_Status_Name = item.SourceOfIncomingTransactions_Status == true ? "COMPLETE" : "PENDING",
					SourceOfIncomingTransactions_CountryOfRemitterName= (country != null && country.Count > 0 && item.SourceOfIncomingTransactions_CountryOfRemitter != null && country.Any(f => f.Value == item.SourceOfIncomingTransactions_CountryOfRemitter.ToString())) ? country.FirstOrDefault(f => f.Value == item.SourceOfIncomingTransactions_CountryOfRemitter.ToString()).Text : string.Empty,
					SourceOfIncomingTransactions_CountryOfRemitterBankName= (country != null && country.Count > 0 && item.SourceOfIncomingTransactions_CountryOfRemitterBank != null && country.Any(f => f.Value == item.SourceOfIncomingTransactions_CountryOfRemitterBank.ToString())) ? country.FirstOrDefault(f => f.Value == item.SourceOfIncomingTransactions_CountryOfRemitterBank.ToString()).Text : string.Empty,
				};
			}
			return retVal;
		}

	}
	
}


