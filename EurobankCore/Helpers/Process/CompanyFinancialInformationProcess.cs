using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Applicant.LegalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
	public class CompanyFinancialInformationProcess
	{
		private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

		private static readonly string _CompanyFinancialInformationFolderName = "Business And Financial Profile";

		private static readonly string _CompanyFinancialInformationName = "Financial Profile";

		public static CompanyFinancialInformationModel GetCompanyFinancialInformationModelByApplicantId(int applicantId)
		{
			CompanyFinancialInformationModel retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if(applicantId > 0)
			{
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(CompanyDetails.CLASS_NAME)
					.WhereEquals("CompanyDetailsID", applicantId)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					TreeNode companyFinancialInformationFolderRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyFinancialInformationFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						companyFinancialInformationFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyFinancialInformationFolderName, StringComparison.OrdinalIgnoreCase));

						if(companyFinancialInformationFolderRoot != null)
						{
							List<TreeNode> companyFinancialInformationNodes = companyFinancialInformationFolderRoot.Children.Where(u => u.ClassName == CompanyFinancialInformation.CLASS_NAME).ToList();

							if(companyFinancialInformationNodes != null && companyFinancialInformationNodes.Count > 0)
							{
								CompanyFinancialInformation companyFinancialInformation = CompanyFinancialInformationProvider.GetCompanyFinancialInformation(companyFinancialInformationNodes.FirstOrDefault().NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

								if(companyFinancialInformation != null)
								{
									retVal = BindCompanyFinancialInformationModel(companyFinancialInformation);
								}
							}
						}
					}
				}
			}

			return retVal;
		}

		public static CompanyFinancialInformationModel GetCompanyFinancialInformationModelById(int companyFinancialInformationId)
		{
			CompanyFinancialInformationModel retVal = null;
			if(companyFinancialInformationId > 0)
			{
				retVal = BindCompanyFinancialInformationModel(GetCompanyFinancialInformationById(companyFinancialInformationId));
			}

			return retVal;
		}

		public static CompanyFinancialInformationModel SaveCompanyFinancialInformationModel(int applicantId, CompanyFinancialInformationModel model)
		{
			CompanyFinancialInformationModel retVal = null;

			if(model != null && model.Id > 0)
			{
				CompanyFinancialInformation companyFinancialInformation = GetCompanyFinancialInformationById(model.Id);
				if(companyFinancialInformation != null)
				{
					CompanyFinancialInformation updatedCompanyFinancialInformation = BindCompanyFinancialInformation(companyFinancialInformation, model);
					if(updatedCompanyFinancialInformation != null)
					{
						updatedCompanyFinancialInformation.Update();
						retVal = BindCompanyFinancialInformationModel(updatedCompanyFinancialInformation);
					}
				}
			}
			else if(applicantId > 0 && model != null)
			{
				TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(CompanyDetails.CLASS_NAME)
					.WhereEquals("CompanyDetailsID", applicantId)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					TreeNode companyFinancialInformationFolderRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyFinancialInformationFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						companyFinancialInformationFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyFinancialInformationFolderName, StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						companyFinancialInformationFolderRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
						companyFinancialInformationFolderRoot.DocumentName = _CompanyFinancialInformationFolderName;
						companyFinancialInformationFolderRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						companyFinancialInformationFolderRoot.Insert(applicationDetailsNode);
					}
					CompanyFinancialInformation companyFinancialInformation = BindCompanyFinancialInformation(null, model);
					if(companyFinancialInformation != null && companyFinancialInformationFolderRoot != null)
					{
						companyFinancialInformation.DocumentName = _CompanyFinancialInformationName;
						companyFinancialInformation.NodeName = _CompanyFinancialInformationName;
						companyFinancialInformation.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						companyFinancialInformation.Insert(companyFinancialInformationFolderRoot);
						model = BindCompanyFinancialInformationModel(companyFinancialInformation);
						retVal = model;
					}
				}
			}

			return retVal;
		}

		private static CompanyFinancialInformation GetCompanyFinancialInformationById(int companyFinancialInformationId)
		{
			CompanyFinancialInformation retVal = null;

			if(companyFinancialInformationId > 0)
			{
				var companyFinancialInformation = CompanyFinancialInformationProvider.GetCompanyFinancialInformations();
				if(companyFinancialInformation != null && companyFinancialInformation.Count > 0)
				{
					retVal = companyFinancialInformation.FirstOrDefault(o => o.CompanyFinancialInformationID == companyFinancialInformationId);
				}
			}

			return retVal;
		}

		private static CompanyFinancialInformationModel BindCompanyFinancialInformationModel(CompanyFinancialInformation item)
		{
			CompanyFinancialInformationModel retVal = null;

			if(item != null)
			{
				retVal = new CompanyFinancialInformationModel()
				{
					Id = item.CompanyFinancialInformationID,
					Turnover = item.FinancialInformation_Turnover == 0 ? string.Empty : item.FinancialInformation_Turnover.ToString(),
					TotalAssets = item.FinancialInformation_TotalAssets == 0 ? string.Empty : item.FinancialInformation_TotalAssets.ToString(),
					//NetProfitAndLoss = item.FinancialInformation_NetProfitAndLoss
					NetProfitLoss = item.FinancialInformation_NetProfitAndLoss == 0 ? string.Empty : item.FinancialInformation_NetProfitAndLoss.ToString()
				};
			}

			return retVal;
		}

		private static CompanyFinancialInformation BindCompanyFinancialInformation(CompanyFinancialInformation companyFinancialInformation, CompanyFinancialInformationModel item)
		{
			CompanyFinancialInformation retVal = new CompanyFinancialInformation();
			if(companyFinancialInformation != null)
			{
				retVal = companyFinancialInformation;
			}
			if(item != null)
			{
				retVal.FinancialInformation_Turnover =Convert.ToDecimal( item.Turnover);
				retVal.FinancialInformation_TotalAssets =Convert.ToDecimal( item.TotalAssets);
				//retVal.FinancialInformation_NetProfitAndLoss =Convert.ToDecimal( item.NetProfitAndLoss);
				retVal.FinancialInformation_NetProfitAndLoss = Convert.ToDecimal(item.NetProfitLoss);
			}

			return retVal;
		}
	}
}
