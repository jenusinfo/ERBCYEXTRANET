using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.Common;
using Eurobank.Models.IdentificationDetails;
using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
	public class IdentificationDetailsProcess
	{
		private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

		private static readonly string _IdentificationDetailsFolderName = "Identifications";

		private static readonly string _IdentificationDocumentName = "Identification";

		public static List<IdentificationDetailsViewModel> GetIdetifications(IEnumerable< TreeNode> treeNode)
		{
			List<IdentificationDetailsViewModel> retVal = new List<IdentificationDetailsViewModel>();
			foreach(var item in treeNode)
			{
				IdentificationDetailsViewModel identificationDetailsViewModel = new IdentificationDetailsViewModel();
				identificationDetailsViewModel.IdentificationDetailsID = ValidationHelper.GetInteger(item.GetValue("IdentificationDetailsID"), 0);
				identificationDetailsViewModel.IdentificationDetails_Citizenship = ValidationHelper.GetInteger(item.GetValue("IdentificationDetails_Citizenship"), 0);
				identificationDetailsViewModel.CitizenshipValue = ValidationHelper.GetString(item.GetValue("IdentificationDetails_Citizenship"), "");
				identificationDetailsViewModel.IdentificationDetails_CitizenshipName = ServiceHelper.GetCountryNameById( ValidationHelper.GetInteger(item.GetValue("IdentificationDetails_Citizenship"), 0));
				identificationDetailsViewModel.IdentificationDetails_CountryOfIssue = ValidationHelper.GetInteger(item.GetValue("IdentificationDetails_CountryOfIssue"), 0);
				identificationDetailsViewModel.CountryOfIssueValue = ValidationHelper.GetString(item.GetValue("IdentificationDetails_CountryOfIssue"),"");
				identificationDetailsViewModel.IdentificationDetails_CountryOfIssueName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(item.GetValue("IdentificationDetails_CountryOfIssue"), 0));
				identificationDetailsViewModel.IdentificationDetails_TypeOfIdentification = ValidationHelper.GetString(item.GetValue("IdentificationDetails_TypeOfIdentification"), "");
				identificationDetailsViewModel.IdentificationDetails_TypeOfIdentificationName = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("IdentificationDetails_TypeOfIdentification"), ""), "/Lookups/General/IDENTIFICATION-TYPE");
				identificationDetailsViewModel.IdentificationDetails_IdentificationNumber = ValidationHelper.GetString(item.GetValue("IdentificationDetails_IdentificationNumber"), "");
				//identificationDetailsViewModel.IdentificationDetails_ExpiryDate = ValidationHelper.GetString(Convert.ToDateTime(item.GetValue("IdentificationDetails_ExpiryDate")).ToString("MM/dd/yyyy"), "");
				//identificationDetailsViewModel.IdentificationDetails_IssueDate = ValidationHelper.GetString(Convert.ToDateTime(item.GetValue("IdentificationDetails_IssueDate")).ToString("MM/dd/yyyy"), "");
				identificationDetailsViewModel.IdentificationDetails_ExpiryDate = ValidationHelper.GetDateTime(Convert.ToDateTime(item.GetValue("IdentificationDetails_ExpiryDate")), default(DateTime));
				identificationDetailsViewModel.IdentificationDetails_IssueDate = ValidationHelper.GetDateTime(Convert.ToDateTime(item.GetValue("IdentificationDetails_IssueDate")), default(DateTime));
				identificationDetailsViewModel.StatusName = ValidationHelper.GetBoolean(item.GetValue("IdentificationDetails_Status"), false) == true ? "Complete" : "Pending";
				retVal.Add(identificationDetailsViewModel);
			}

			return retVal;
		}

		public static IdentificationDetailsViewModel SaveIdendificationsModel( IdentificationDetailsViewModel model, TreeNode treeNodeData)
		{
			IdentificationDetailsViewModel retVal = new IdentificationDetailsViewModel();

			if(model != null )
			{
				if(treeNodeData != null)
				{
					TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
					TreeNode applicationfoldernode_parent = tree.SelectNodes()
						.Path(model.NodeAliaspath + "/Identifications")
						.OnCurrentSite()
						.Published(false)
						.FirstOrDefault();
					if(applicationfoldernode_parent == null)
					{
						applicationfoldernode_parent = tree.SelectNodes()
					   .Path(model.NodeAliaspath)
					   .OnCurrentSite()
					   .Published(false)
					   .FirstOrDefault();
						applicationfoldernode_parent = TreeNode.New("CMS.Folder", tree);
						applicationfoldernode_parent.DocumentName = "Identifications";
						applicationfoldernode_parent.DocumentCulture = "en-US";
						applicationfoldernode_parent.Insert(treeNodeData);
					}
					TreeNode identification = TreeNode.New("Eurobank.IdentificationDetails", tree);
					//identification.DocumentName = model.IdentificationDetails_IdentificationNumber;
					identification.DocumentName = _IdentificationDocumentName;
					identification.SetValue("IdentificationDetails_Citizenship", ValidationHelper.GetInteger(model.CitizenshipValue, 0));
					identification.SetValue("IdentificationDetails_TypeOfIdentification",ValidationHelper.GetGuid( model.IdentificationDetails_TypeOfIdentification,Guid.Empty));
					identification.SetValue("IdentificationDetails_IdentificationNumber", model.IdentificationDetails_IdentificationNumber==null?"": ValidationHelper.GetString( model.IdentificationDetails_IdentificationNumber.ToUpper(),""));
					identification.SetValue("IdentificationDetails_CountryOfIssue", ValidationHelper.GetInteger(model.CountryOfIssueValue, 0));
					identification.SetValue("IdentificationDetails_IssueDate", ValidationHelper.GetDateTime(model.IdentificationDetails_IssueDate, DateTimeHelper.ZERO_TIME));
					identification.SetValue("IdentificationDetails_ExpiryDate", ValidationHelper.GetDateTime(model.IdentificationDetails_ExpiryDate, DateTimeHelper.ZERO_TIME));
					identification.SetValue("IdentificationDetails_Status", model.Status);
					identification.Insert(applicationfoldernode_parent);
					model.IdentificationDetailsID = ValidationHelper.GetInteger(identification.GetValue("IdentificationDetailsID"), 0);
				}
			}
			model.IdentificationDetails_CitizenshipName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(model.CitizenshipValue, 0));
			model.IdentificationDetails_CountryOfIssueName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(model.CountryOfIssueValue, 0));
			model.IdentificationDetails_TypeOfIdentificationName = ServiceHelper.GetName(ValidationHelper.GetString(model.IdentificationDetails_TypeOfIdentification, ""), "/Lookups/General/IDENTIFICATION-TYPE");
			model.StatusName = model.Status ? GridRecordStatus.Complete.ToString() : GridRecordStatus.Pending.ToString();
			retVal = model;
			return retVal;
		}

		public static IdentificationDetailsViewModel UpdateIdendificationsModel(IdentificationDetailsViewModel model, TreeNode identification)
		{
			IdentificationDetailsViewModel retVal = new IdentificationDetailsViewModel();

			if(model != null)
			{
				if(identification != null)
				{
					
					//identification.DocumentName = model.IdentificationDetails_IdentificationNumber;
					identification.SetValue("IdentificationDetails_Citizenship", ValidationHelper.GetInteger(model.CitizenshipValue, 0));
					identification.SetValue("IdentificationDetails_TypeOfIdentification", model.IdentificationDetails_TypeOfIdentification);
					identification.SetValue("IdentificationDetails_IdentificationNumber", model.IdentificationDetails_IdentificationNumber);
					identification.SetValue("IdentificationDetails_CountryOfIssue", ValidationHelper.GetInteger(model.CountryOfIssueValue, 0));
					identification.SetValue("IdentificationDetails_IssueDate", model.IdentificationDetails_IssueDate);
					identification.SetValue("IdentificationDetails_ExpiryDate", model.IdentificationDetails_ExpiryDate);
					identification.SetValue("IdentificationDetails_Status", model.Status);
					identification.NodeAlias = identification.DocumentName;
					identification.Update();

				}
			}
			model.IdentificationDetailsID = model.IdentificationDetailsID;
			model.IdentificationDetails_CitizenshipName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(model.CitizenshipValue, 0));
			model.IdentificationDetails_CountryOfIssueName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(model.CountryOfIssueValue, 0));
			model.IdentificationDetails_TypeOfIdentificationName = ServiceHelper.GetName(ValidationHelper.GetString(model.IdentificationDetails_TypeOfIdentification, ""), "/Lookups/General/IDENTIFICATION-TYPE");
			model.StatusName = model.Status ? GridRecordStatus.Complete.ToString() : GridRecordStatus.Pending.ToString();
			return retVal;
		}

		public static List<IdentificationDetailsViewModel> GetIdentificationDetails(int personalDetailsId)
		{
			List<IdentificationDetailsViewModel> retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if(personalDetailsId > 0)
			{
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(PersonalDetails.CLASS_NAME)
					.WhereEquals("PersonalDetailsID", personalDetailsId)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					TreeNode addressDetailsRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _IdentificationDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						addressDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _IdentificationDetailsFolderName, StringComparison.OrdinalIgnoreCase));

						if(addressDetailsRoot != null)
						{
							List<TreeNode> addressDetailsNodes = addressDetailsRoot.Children.Where(u => u.ClassName == IdentificationDetails.CLASS_NAME).ToList();

							if(addressDetailsNodes != null && addressDetailsNodes.Count > 0)
							{
								retVal = new List<IdentificationDetailsViewModel>();
								addressDetailsNodes.ForEach(t =>
								{
									IdentificationDetails addressDetails = IdentificationDetailsProvider.GetIdentificationDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if(addressDetails != null)
									{
										IdentificationDetailsViewModel personalDetailsModel = BindIdentificationDetailsModel(addressDetails);
										if(personalDetailsModel != null)
										{
											retVal.Add(personalDetailsModel);
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

		//public static IdentityCardModel GetIdentityCardLegal(string companyDetailsGuid)
		//{
		//	IdentityCardModel retVal = null;

		//	TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
		//	if(!string.IsNullOrEmpty(companyDetailsGuid))
		//	{
		//		TreeNode applicationDetailsNode = tree.SelectNodes()
		//			.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
		//			.Type(CompanyDetails.CLASS_NAME)
		//			.WhereEquals("NodeGUID", new Guid(companyDetailsGuid))
		//			.OnCurrentSite()
		//			.Published(false)
		//			.FirstOrDefault();

		//		if(applicationDetailsNode != null)
		//		{
		//			TreeNode identificationDetailsRoot = null;
		//			if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _IdentificationDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
		//			{
		//				identificationDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _IdentificationDetailsFolderName, StringComparison.OrdinalIgnoreCase));

		//				if(identificationDetailsRoot != null)
		//				{
		//					var identificationTypes = ServiceHelper.GetTypeIdentificationLegal();
		//					if(identificationTypes != null && identificationTypes.Count > 0)
		//					{
		//						string identityCardGuid = ServiceHelper.GetGuidValueByText(identificationTypes, "ID");
		//						string pasportGuid = ServiceHelper.GetGuidValueByText(identificationTypes, "PASSPORT");
		//						TreeNode identificationDetailsNode = identificationDetailsRoot.Children.Where(u => u.ClassName == IdentificationDetails.CLASS_NAME && (string.Equals(u.GetValue("IdentificationDetails_TypeOfIdentification", string.Empty), identityCardGuid) || string.Equals(u.GetValue("IdentificationDetails_TypeOfIdentification", string.Empty), identityCardGuid))).FirstOrDefault();

		//						if(identificationDetailsNode != null)
		//						{
		//							IdentificationDetails addressDetails = IdentificationDetailsProvider.GetIdentificationDetails(identificationDetailsNode.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

		//							if(addressDetails != null)
		//							{
		//								retVal = BindIdentityCardModel(addressDetails);
		//							}
		//						}
		//					}
							
		//				}
		//			}
		//		}
		//	}

		//	return retVal;
		//}

		public static IdentityCardModel GetIdentityCard(string personalDetailsGuid)
		{
			IdentityCardModel retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if(!string.IsNullOrEmpty(personalDetailsGuid))
			{
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(PersonalDetails.CLASS_NAME)
					.WhereEquals("NodeGUID", new Guid(personalDetailsGuid))
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					TreeNode identificationDetailsRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _IdentificationDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						identificationDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _IdentificationDetailsFolderName, StringComparison.OrdinalIgnoreCase));

						if(identificationDetailsRoot != null)
						{
							var identificationTypes = ServiceHelper.GetTypeIdentificationIndividual();
							if(identificationTypes != null && identificationTypes.Count > 0)
							{
								string identityCardGuid = ServiceHelper.GetGuidValueByText(identificationTypes, "ID");
								string pasportGuid = ServiceHelper.GetGuidValueByText(identificationTypes, "PASSPORT");
								//TreeNode identificationDetailsNode = identificationDetailsRoot.Children.Where(u => u.ClassName == IdentificationDetails.CLASS_NAME && (string.Equals(u.GetValue("IdentificationDetails_TypeOfIdentification", string.Empty), identityCardGuid) || string.Equals(u.GetValue("IdentificationDetails_TypeOfIdentification", string.Empty), pasportGuid))).FirstOrDefault();
								TreeNode identificationDetailsNode = identificationDetailsRoot.Children.Where(u => u.ClassName == IdentificationDetails.CLASS_NAME).FirstOrDefault();
								
								if (identificationDetailsNode != null)
								{
									IdentificationDetails addressDetails = IdentificationDetailsProvider.GetIdentificationDetails(identificationDetailsNode.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if(addressDetails != null)
									{
										retVal = BindIdentityCardModel(addressDetails);
									}
								}
							}

						}
					}
				}
			}

			return retVal;
		}

		private static IdentificationDetailsViewModel BindIdentificationDetailsModel(IdentificationDetails item)
		{
			IdentificationDetailsViewModel retVal = null;

			if(item != null)
			{
				var country = ServiceHelper.GetCountriesWithID();
				var typeOfIdentification = ServiceHelper.GetTypeofIdentification();
				retVal = new IdentificationDetailsViewModel()
				{
					IdentificationDetailsID = item.IdentificationDetailsID,
					IdentificationDetails_TypeOfIdentification=item.IdentificationDetails_TypeOfIdentification.ToString(),
					IdentificationDetails_TypeOfIdentificationName= (typeOfIdentification != null && typeOfIdentification.Count > 0 && item.IdentificationDetails_TypeOfIdentification != null && typeOfIdentification.Any(f => f.Value == item.IdentificationDetails_TypeOfIdentification.ToString())) ? typeOfIdentification.FirstOrDefault(f => f.Value == item.IdentificationDetails_TypeOfIdentification.ToString()).Text : string.Empty,
					IdentificationDetails_IdentificationNumber = item.IdentificationDetails_IdentificationNumber,
					IdentificationDetails_Citizenship=item.IdentificationDetails_Citizenship,
					IdentificationDetails_CitizenshipName = (country != null && country.Count > 0 && item.IdentificationDetails_Citizenship != null && country.Any(f => f.Value == item.IdentificationDetails_Citizenship.ToString())) ? country.FirstOrDefault(f => f.Value == item.IdentificationDetails_Citizenship.ToString()).Text : string.Empty,
					IdentificationDetails_CountryOfIssue =item.IdentificationDetails_CountryOfIssue,
					IdentificationDetails_CountryOfIssueName= (country != null && country.Count > 0 && item.IdentificationDetails_CountryOfIssue != null && country.Any(f => f.Value == item.IdentificationDetails_CountryOfIssue.ToString())) ? country.FirstOrDefault(f => f.Value == item.IdentificationDetails_CountryOfIssue.ToString()).Text : string.Empty,
					IdentificationDetails_IssueDate =item.IdentificationDetails_IssueDate,
					IdentificationDetails_ExpiryDate=item.IdentificationDetails_ExpiryDate,
					StatusName=item.IdentificationDetails_Status==true?"Complete":"Pending"

				};
			}

			return retVal;
		}

		private static IdentityCardModel BindIdentityCardModel(IdentificationDetails item)
		{
			IdentityCardModel retVal = null;

			if(item != null)
			{
				var country = ServiceHelper.GetCountriesWithID();
				retVal = new IdentityCardModel()
				{
					IdentityNumber = item.IdentificationDetails_IdentificationNumber,
					CountryOfIssue = item.IdentificationDetails_CountryOfIssue,
					CountryOfIssueName = (country != null && country.Count > 0 && item.IdentificationDetails_CountryOfIssue != null && country.Any(f => f.Value == item.IdentificationDetails_CountryOfIssue.ToString())) ? country.FirstOrDefault(f => f.Value == item.IdentificationDetails_CountryOfIssue.ToString()).Text : string.Empty

				};
			}

			return retVal;
		}
	}
}
