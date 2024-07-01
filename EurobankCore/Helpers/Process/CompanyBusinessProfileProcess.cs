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
	public class CompanyBusinessProfileProcess
	{
		private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

		private static readonly string _CompanyBusinessProfileFolderName = "Business And Financial Profile";

		public static CompanyBusinessProfileModel GetCompanyBusinessProfileModelByApplicantId(int applicantId)
		{
			CompanyBusinessProfileModel retVal = null;

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
					TreeNode companyBusinessProfileFolderRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyBusinessProfileFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						companyBusinessProfileFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyBusinessProfileFolderName, StringComparison.OrdinalIgnoreCase));

						if(companyBusinessProfileFolderRoot != null)
						{
							List<TreeNode> companyBusinessProfileNodes = companyBusinessProfileFolderRoot.Children.Where(u => u.ClassName == CompanyBusinessProfile.CLASS_NAME).ToList();

							if(companyBusinessProfileNodes != null && companyBusinessProfileNodes.Count > 0)
							{
								CompanyBusinessProfile companyBusinessProfile = CompanyBusinessProfileProvider.GetCompanyBusinessProfile(companyBusinessProfileNodes.FirstOrDefault().NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

								if(companyBusinessProfile != null)
								{
									retVal = BindCompanyBusinessProfileModel(companyBusinessProfile);
								}
							}
						}
					}
				}
			}

			return retVal;
		}

		public static CompanyBusinessProfileModel GetCompanyBusinessProfileModelById(int companyBusinessProfileId)
		{
			CompanyBusinessProfileModel retVal = null;
			if(companyBusinessProfileId > 0)
			{
				retVal = BindCompanyBusinessProfileModel(GetCompanyBusinessProfileById(companyBusinessProfileId));
			}

			return retVal;
		}

		public static CompanyBusinessProfileModel SaveCompanyBusinessProfileModel(int applicantId, CompanyBusinessProfileModel model)
		{
			CompanyBusinessProfileModel retVal = null;

			if(model != null && model.Id > 0)
			{
				CompanyBusinessProfile companyBusinessProfile = GetCompanyBusinessProfileById(model.Id);
				if(companyBusinessProfile != null)
				{
					CompanyBusinessProfile updatedCompanyBusinessProfile = BindCompanyBusinessProfile(companyBusinessProfile, model);
					if(updatedCompanyBusinessProfile != null)
					{
						updatedCompanyBusinessProfile.Update();
						retVal = BindCompanyBusinessProfileModel(updatedCompanyBusinessProfile);
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
					TreeNode companyBusinessProfileFolderRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyBusinessProfileFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						companyBusinessProfileFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _CompanyBusinessProfileFolderName, StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						companyBusinessProfileFolderRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
						companyBusinessProfileFolderRoot.DocumentName = _CompanyBusinessProfileFolderName;
						companyBusinessProfileFolderRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						companyBusinessProfileFolderRoot.Insert(applicationDetailsNode);
					}
					CompanyBusinessProfile companyBusinessProfile = BindCompanyBusinessProfile(null, model);
					if(companyBusinessProfile != null && companyBusinessProfileFolderRoot != null)
					{
						companyBusinessProfile.DocumentName = model.MainBusinessActivities;
						companyBusinessProfile.NodeName = model.MainBusinessActivities;
						companyBusinessProfile.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						companyBusinessProfile.Insert(companyBusinessProfileFolderRoot);
						model = BindCompanyBusinessProfileModel(companyBusinessProfile);
						retVal = model;
					}
				}
			}

			return retVal;
		}

		private static CompanyBusinessProfile GetCompanyBusinessProfileById(int companyBusinessProfileId)
		{
			CompanyBusinessProfile retVal = null;

			if(companyBusinessProfileId > 0)
			{
				var companyBusinessProfile = CompanyBusinessProfileProvider.GetCompanyBusinessProfiles();
				if(companyBusinessProfile != null && companyBusinessProfile.Count > 0)
				{
					retVal = companyBusinessProfile.FirstOrDefault(o => o.CompanyBusinessProfileID == companyBusinessProfileId);
				}
			}

			return retVal;
		}

		private static CompanyBusinessProfileModel BindCompanyBusinessProfileModel(CompanyBusinessProfile item)
		{
			CompanyBusinessProfileModel retVal = null;

			if(item != null)
			{
				var economicSectorIndustries = ServiceHelper.GetEconomicSectorIndustries();
				//var countries = ServiceHelper.GetCountriesWithID();
				retVal = new CompanyBusinessProfileModel()
				{
					Id = item.CompanyBusinessProfileID,
					WebsiteAddress = item.CompanyBusinessProfile_WebsiteAddress,
					NumberofYearsinOperation = item.CompanyBusinessProfile_NumberofYearsinOperation,
					NumberofEmployes = item.CompanyBusinessProfile_NumberofEmployes,
					MainBusinessActivities = item.CompanyBusinessProfile_MainBusinessActivities,
					CorporationIsengagedInTheProvisionName = item.CompanyBusinessProfile_Corporationisengagedintheprovisionoffinancialandinvestmentservices,
					CorporationIsengagedInTheProvisionValue = item.CompanyBusinessProfile_Corporationisengagedintheprovisionoffinancialandinvestmentservices == "true" ? "YES" : "NO",
					//CountryofOriginofWealthActivities = item.CompanyBusinessProfile_CountryofOriginofWealthActivities,
					CountryofOriginofWealthActivitiesValues = !string.IsNullOrEmpty(item.CompanyBusinessProfile_CountryofOriginofWealthActivities) ? item.CompanyBusinessProfile_CountryofOriginofWealthActivities.Split('|') : null,
					EconomicSectorIndustry = item.CompanyBusinessProfile_EconomicSectorIndustry,
					EconomicSectorIndustryName = (economicSectorIndustries != null && economicSectorIndustries.Any(l => l.Value == item.CompanyBusinessProfile_EconomicSectorIndustry)) ? economicSectorIndustries.FirstOrDefault(l => l.Value == item.CompanyBusinessProfile_EconomicSectorIndustry).Text : string.Empty,
					IssuingAuthority = item.CompanyBusinessProfile_IssuingAuthority,
					SponsoringEntityName=item.CompanyBusinessProfile_SponsoringEntityName,
					LineOfBusinessOfTheSponsoringEntity=item.CompanyBusinessProfile_LineOfBusinessOfTheSponsoringEntity,
					WebsiteOfTheSponsoringEntity=item.CompanyBusinessProfile_WebsiteOfTheSponsoringEntity
				};
			}

			return retVal;
		}

		private static CompanyBusinessProfile BindCompanyBusinessProfile(CompanyBusinessProfile companyBusinessProfile, CompanyBusinessProfileModel item)
		{
			CompanyBusinessProfile retVal = new CompanyBusinessProfile();
			if(companyBusinessProfile != null)
			{
				retVal = companyBusinessProfile;
			}
			if(item != null)
			{
				retVal.CompanyBusinessProfile_Corporationisengagedintheprovisionoffinancialandinvestmentservices = item.CorporationIsengagedInTheProvisionName;
				retVal.CompanyBusinessProfile_CountryofOriginofWealthActivities = item.CountryofOriginofWealthActivitiesValues != null && item.CountryofOriginofWealthActivitiesValues.Length > 0 ?  string.Join('|', item.CountryofOriginofWealthActivitiesValues) : null;
				//retVal.CompanyBusinessProfile_CountryofOriginofWealthActivities = item.CountryofOriginofWealthActivities;
				retVal.CompanyBusinessProfile_EconomicSectorIndustry = item.EconomicSectorIndustry;
				retVal.CompanyBusinessProfile_IssuingAuthority = item.IssuingAuthority;
				retVal.CompanyBusinessProfile_MainBusinessActivities = item.MainBusinessActivities;
				retVal.CompanyBusinessProfile_NumberofEmployes =item.NumberofEmployes;
				retVal.CompanyBusinessProfile_NumberofYearsinOperation =item.NumberofYearsinOperation;
				retVal.CompanyBusinessProfile_WebsiteAddress = item.WebsiteAddress;
				retVal.CompanyBusinessProfile_SponsoringEntityName = item.SponsoringEntityName;
				retVal.CompanyBusinessProfile_LineOfBusinessOfTheSponsoringEntity = item.LineOfBusinessOfTheSponsoringEntity;
				retVal.CompanyBusinessProfile_WebsiteOfTheSponsoringEntity = item.WebsiteOfTheSponsoringEntity;
				
			}

			return retVal;
		}
	}
}
