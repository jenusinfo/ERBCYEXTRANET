using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Globalization;
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
	public class SourceOfOutgoingTransactionProcess
	{
		private static readonly string _ApplicationRootNodePath = "/Applications-(1)";
		private static readonly string _OutgoingTranDocumentPath = "/Applications-(1)";

		public static List<SourceOfOutgoingTransactionsModel> GetSourceOfOutgoingTransactionModels(string applicationNumber)
		{
			List<SourceOfOutgoingTransactionsModel> retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if(!string.IsNullOrEmpty(applicationNumber))
			{
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type("Eurobank.ApplicationDetails")
					.WhereEquals("NodeName", applicationNumber)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					retVal = new List<SourceOfOutgoingTransactionsModel>();
					TreeNode sourceOfOutgoingFolderRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, "Source of Outgoing Transactions", StringComparison.OrdinalIgnoreCase)))
					{
						sourceOfOutgoingFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, "Source of Outgoing Transactions", StringComparison.OrdinalIgnoreCase));

						if(sourceOfOutgoingFolderRoot != null)
						{
							List<TreeNode> sourceOfOutgoingTransactionsNodes = sourceOfOutgoingFolderRoot.Children.Where(u => u.ClassName == "Eurobank.SourceOfOutgoingTransactions").ToList();

							if(sourceOfOutgoingTransactionsNodes != null && sourceOfOutgoingTransactionsNodes.Count > 0)
							{
								sourceOfOutgoingTransactionsNodes.ForEach(t => {
									SourceOfOutgoingTransactions sourceOfOutgoingTransaction = SourceOfOutgoingTransactionsProvider.GetSourceOfOutgoingTransactions(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if(sourceOfOutgoingTransaction != null)
									{
										SourceOfOutgoingTransactionsModel sourceOfOutgoingTransactionsModel = BindSourceOfOutgoingTransactionsModel(sourceOfOutgoingTransaction);
										if(sourceOfOutgoingTransactionsModel != null)
										{
											retVal.Add(sourceOfOutgoingTransactionsModel);
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

		public static SourceOfOutgoingTransactionsModel SaveSourceOfOutgoingTransactionsModel(string applicationNumber, SourceOfOutgoingTransactionsModel model)
		{
			SourceOfOutgoingTransactionsModel retVal = null;
			
			if(model != null && model.Id > 0)
			{
				SourceOfOutgoingTransactions sourceOfOutgoingTransactions = GetSourceOfOutgoingTransactionsById(model.Id);
				if(sourceOfOutgoingTransactions != null)
				{
					SourceOfOutgoingTransactions updatedsourceOfOutgoingTransactions = BindSourceOfOutgoingTransactions(sourceOfOutgoingTransactions, model);
					if(updatedsourceOfOutgoingTransactions != null)
					{
						updatedsourceOfOutgoingTransactions.Update();
						model = BindSourceOfOutgoingTransactionsModel(updatedsourceOfOutgoingTransactions);
						retVal = model;
					}
				}
			}
			else if(!string.IsNullOrEmpty(applicationNumber) && model != null)
			{
				TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type("Eurobank.ApplicationDetails")
					.WhereEquals("NodeName", applicationNumber)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					TreeNode sourceOfOutgoingFolderRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, "Source of Outgoing Transactions", StringComparison.OrdinalIgnoreCase)))
					{
						sourceOfOutgoingFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, "Source of Outgoing Transactions", StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						sourceOfOutgoingFolderRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
						sourceOfOutgoingFolderRoot.DocumentName = "Source of Outgoing Transactions";
						sourceOfOutgoingFolderRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						sourceOfOutgoingFolderRoot.Insert(applicationDetailsNode);
					}
					SourceOfOutgoingTransactions sourceOfOutgoingTransactions = BindSourceOfOutgoingTransactions(null, model);
					if(sourceOfOutgoingTransactions != null && sourceOfOutgoingFolderRoot != null)
					{
						sourceOfOutgoingTransactions.DocumentName = _OutgoingTranDocumentPath;
						sourceOfOutgoingTransactions.NodeName = _OutgoingTranDocumentPath;
						sourceOfOutgoingTransactions.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						sourceOfOutgoingTransactions.Insert(sourceOfOutgoingFolderRoot);
						model = BindSourceOfOutgoingTransactionsModel(sourceOfOutgoingTransactions);
						retVal = model;
					}
				}
			}
			

			return retVal;
		}

		private static SourceOfOutgoingTransactions GetSourceOfOutgoingTransactionsById(int sourceOfOutgoingTransactionsId)
		{
			SourceOfOutgoingTransactions retVal = null;

			if(sourceOfOutgoingTransactionsId > 0)
			{
				var sourceOfOutgoingTransactions = SourceOfOutgoingTransactionsProvider.GetSourceOfOutgoingTransactions();
				if(sourceOfOutgoingTransactions != null && sourceOfOutgoingTransactions.Count > 0)
				{
					retVal = sourceOfOutgoingTransactions.FirstOrDefault(o => o.SourceOfOutgoingTransactionsID == sourceOfOutgoingTransactionsId);
				}
			}

			return retVal;
		}

		private static SourceOfOutgoingTransactionsModel BindSourceOfOutgoingTransactionsModel(SourceOfOutgoingTransactions item)
		{
			SourceOfOutgoingTransactionsModel retVal = null;
			
			if(item!= null)
			{
				var countries = CountryInfoProvider.GetCountries().ToList();
				retVal = new SourceOfOutgoingTransactionsModel()
				{
					Id = item.SourceOfOutgoingTransactionsID,
					NameOfBeneficiary = item.SourceOfOutgoingTransactions_NameOfBeneficiary,
					CountryOfBeneficiary = item.SourceOfOutgoingTransactions_CountryOfBeneficiary,
					CountryOfBeneficiaryBank = item.SourceOfOutgoingTransactions_CountryOfBeneficiaryBank,
					SourceOfOutgoingTransactions_Status = item.SourceOfOutgoingTransactions_Status,
					StatusName = item.SourceOfOutgoingTransactions_Status == true ? "Complete" : "Pending",
					//CountryOfBeneficiaryName = (countries != null && countries.Count > 0 && countries.Any(h => string.Equals(h.CountryGUID.ToString(), item.SourceOfOutgoingTransactions_CountryOfBeneficiary, StringComparison.OrdinalIgnoreCase))) ? countries.FirstOrDefault(h => string.Equals(h.CountryGUID.ToString(), item.SourceOfOutgoingTransactions_CountryOfBeneficiary, StringComparison.OrdinalIgnoreCase)).CountryName: string.Empty,
					//CountryOfBeneficiaryBankName = (countries != null && countries.Count > 0 && countries.Any(h => string.Equals(h.CountryGUID.ToString(), item.SourceOfOutgoingTransactions_CountryOfBeneficiaryBank, StringComparison.OrdinalIgnoreCase))) ? countries.FirstOrDefault(h => string.Equals(h.CountryGUID.ToString(), item.SourceOfOutgoingTransactions_CountryOfBeneficiaryBank, StringComparison.OrdinalIgnoreCase)).CountryName : string.Empty
				};
			}

			return retVal;
		}

		private static SourceOfOutgoingTransactions BindSourceOfOutgoingTransactions(SourceOfOutgoingTransactions existSourceOfOutgoingTransactions, SourceOfOutgoingTransactionsModel item)
		{
			SourceOfOutgoingTransactions retVal = new SourceOfOutgoingTransactions();
			if(existSourceOfOutgoingTransactions != null)
			{
				retVal = existSourceOfOutgoingTransactions;
			}
			if(item != null)
			{
				retVal.SourceOfOutgoingTransactions_NameOfBeneficiary = item.NameOfBeneficiary;
				retVal.SourceOfOutgoingTransactions_CountryOfBeneficiary = item.CountryOfBeneficiary;
				retVal.SourceOfOutgoingTransactions_CountryOfBeneficiaryBank = item.CountryOfBeneficiaryBank;
				retVal.SourceOfOutgoingTransactions_Status = item.SourceOfOutgoingTransactions_Status;
			}

			return retVal;
		}
	}
}
