using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Applicant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
	public class EmploymentDetailsProcess
	{
		private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

		private static readonly string _EmploymentDetailsFolderName = "Business And Financial Profile";

		private static readonly string _EmploymentDetailsName = "Employment";

		public static List<EmploymentDetailsModel> GetEmploymentDetailsModels(int applicantId)
		{
			List<EmploymentDetailsModel> retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if(applicantId > 0)
			{
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(PersonalDetails.CLASS_NAME)
					.WhereEquals("PersonalDetailsID", applicantId)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					retVal = new List<EmploymentDetailsModel>();
					TreeNode employmentDetailsFolderRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EmploymentDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						employmentDetailsFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EmploymentDetailsFolderName, StringComparison.OrdinalIgnoreCase));

						if(employmentDetailsFolderRoot != null)
						{
							List<TreeNode> employmentDetailsNodes = employmentDetailsFolderRoot.Children.Where(u => u.ClassName == EmploymentDetails.CLASS_NAME).ToList();

							if(employmentDetailsNodes != null && employmentDetailsNodes.Count > 0)
							{
								employmentDetailsNodes.ForEach(t => {
									EmploymentDetails employmentDetails = EmploymentDetailsProvider.GetEmploymentDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if(employmentDetails != null)
									{
										EmploymentDetailsModel employmentDetailsModel = BindEmploymentDetailsModel(employmentDetails);
										if(employmentDetailsModel != null)
										{
											retVal.Add(employmentDetailsModel);
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

		public static EmploymentDetailsModel GetEmploymentDetailsModelByApplicantId(int applicantId)
		{
			EmploymentDetailsModel retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if(applicantId > 0)
			{
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(PersonalDetails.CLASS_NAME)
					.WhereEquals("PersonalDetailsID", applicantId)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					TreeNode employmentDetailsFolderRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EmploymentDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						employmentDetailsFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EmploymentDetailsFolderName, StringComparison.OrdinalIgnoreCase));

						if(employmentDetailsFolderRoot != null)
						{
							List<TreeNode> employmentDetailsNodes = employmentDetailsFolderRoot.Children.Where(u => u.ClassName == EmploymentDetails.CLASS_NAME).ToList();

							if(employmentDetailsNodes != null && employmentDetailsNodes.Count > 0)
							{
								EmploymentDetails employmentDetails = EmploymentDetailsProvider.GetEmploymentDetails(employmentDetailsNodes.FirstOrDefault().NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

								if(employmentDetails != null)
								{
									retVal = BindEmploymentDetailsModel(employmentDetails);
								}
							}
						}
					}
				}
			}

			return retVal;
		}

		public static EmploymentDetailsModel GetEmploymentDetailsModelById(int employmentDetailsId)
		{
			EmploymentDetailsModel retVal = null;
			if(employmentDetailsId > 0)
			{
				retVal = BindEmploymentDetailsModel(GetEmploymentDetailsById(employmentDetailsId));
			}

			return retVal;
		}

		public static EmploymentDetailsModel SaveEmploymentDetailsModel(int applicantId, EmploymentDetailsModel model)
		{
			EmploymentDetailsModel retVal = null;

			if(model != null)
			{
				var employmentStatus = ServiceHelper.GetEmploymentStatus();
				if(employmentStatus != null && employmentStatus.Any(t => !string.IsNullOrEmpty(model.EmploymentStatus) && t.Value == model.EmploymentStatus))
				{
					model.EmploymentStatusName = employmentStatus.FirstOrDefault(t => !string.IsNullOrEmpty(model.EmploymentStatus) && t.Value == model.EmploymentStatus).Text;
				}
			}
			if(model != null && model.Id > 0)
			{
				EmploymentDetails employmentDetails = GetEmploymentDetailsById(model.Id);
				if(employmentDetails != null)
				{
					EmploymentDetails updatedEmploymentDetails = BindEmploymentDetails(employmentDetails, model);
					if(updatedEmploymentDetails != null)
					{
						updatedEmploymentDetails.Update();
						retVal = BindEmploymentDetailsModel(updatedEmploymentDetails);
					}
				}
			}
			else if(applicantId  > 0 && model != null)
			{
				TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(PersonalDetails.CLASS_NAME)
					.WhereEquals("PersonalDetailsID", applicantId)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					TreeNode employmentDetailsFolderRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EmploymentDetailsFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						employmentDetailsFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EmploymentDetailsFolderName, StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						employmentDetailsFolderRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
						employmentDetailsFolderRoot.DocumentName = _EmploymentDetailsFolderName;
						employmentDetailsFolderRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						employmentDetailsFolderRoot.Insert(applicationDetailsNode);
					}
					EmploymentDetails employmentDetails = BindEmploymentDetails(null, model);
					if(employmentDetails != null && employmentDetailsFolderRoot != null)
					{
						//employmentDetails.DocumentName = model.EmploymentStatusName;
						//employmentDetails.NodeName = model.EmploymentStatusName;
						employmentDetails.DocumentName = _EmploymentDetailsName;
						employmentDetails.NodeName = _EmploymentDetailsName;
						employmentDetails.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						employmentDetails.Insert(employmentDetailsFolderRoot);
						model = BindEmploymentDetailsModel(employmentDetails);
						retVal = model;
					}
				}
			}

			return retVal;
		}

		private static EmploymentDetails GetEmploymentDetailsById(int employmentDetailsId)
		{
			EmploymentDetails retVal = null;

			if(employmentDetailsId > 0)
			{
				var employmentDetails = EmploymentDetailsProvider.GetEmploymentDetails();
				if(employmentDetails != null && employmentDetails.Count > 0)
				{
					retVal = employmentDetails.FirstOrDefault(o => o.EmploymentDetailsID == employmentDetailsId);
				}
			}

			return retVal;
		}

		private static EmploymentDetailsModel BindEmploymentDetailsModel(EmploymentDetails item)
		{
			EmploymentDetailsModel retVal = null;

			if(item != null)
			{
				var employmentStatus = ServiceHelper.GetEmploymentStatus();
				var employmentProfessions = ServiceHelper.GetEmploymentProfessions();
				var countries = ServiceHelper.GetCountriesWithID();
				retVal = new EmploymentDetailsModel()
				{
					Id = item.EmploymentDetailsID,
					EmploymentStatus = item.EmploymentDetails_EmploymentStatus.ToString(),
					EmploymentStatusName = (employmentStatus != null && employmentStatus.Any(l => l.Value == item.EmploymentDetails_EmploymentStatus.ToString())) ? employmentStatus.FirstOrDefault(l => l.Value == item.EmploymentDetails_EmploymentStatus.ToString()).Text : string.Empty,
					Profession = item.EmploymentDetails_Profession.ToString(),
					ProfessionName = (employmentProfessions != null && employmentProfessions.Any(l => l.Value == item.EmploymentDetails_Profession.ToString())) ? employmentProfessions.FirstOrDefault(l => l.Value == item.EmploymentDetails_Profession.ToString()).Text : string.Empty,
					//YearsInBusiness = item.EmploymentDetails_YearsInBusiness != 0? Convert.ToString( item.EmploymentDetails_YearsInBusiness) : string.Empty, //new excel #137
					YearsInBusiness = Convert.ToString(item.EmploymentDetails_YearsInBusiness),
					EmployersName = item.EmploymentDetails_EmployersName.ToString(),
					EmployersBusiness = item.EmploymentDetails_EmployersBusiness,
					FormerProfession = item.EmploymentDetails_FormerProfession.ToString(),
					FormerProfessionName = (employmentProfessions != null && employmentProfessions.Any(l => l.Value == item.EmploymentDetails_FormerProfession.ToString())) ? employmentProfessions.FirstOrDefault(l => l.Value == item.EmploymentDetails_FormerProfession.ToString()).Text : string.Empty,
					FormerCountryOfEmployment = item.EmploymentDetails_FormerCountryOfEmployment,
					FormerCountryOfEmploymentName = (countries != null && countries.Any(l => l.Value == item.EmploymentDetails_FormerProfession.ToString())) ? countries.FirstOrDefault(l => l.Value == item.EmploymentDetails_FormerProfession.ToString()).Text : string.Empty,
					FormerEmployersName = item.EmploymentDetails_FormerEmployersName,
					FormerEmployersBusiness = item.EmploymentDetails_FormerEmployersBusiness,
				};
			}

			return retVal;
		}

		private static EmploymentDetails BindEmploymentDetails(EmploymentDetails employmentDetails, EmploymentDetailsModel item)
		{
			EmploymentDetails retVal = new EmploymentDetails();
			if(employmentDetails != null)
			{
				retVal = employmentDetails;
			}
			if(item != null)
			{
				if(!string.IsNullOrEmpty(item.EmploymentStatus))
				{
					retVal.EmploymentDetails_EmploymentStatus = new Guid(item.EmploymentStatus);
				}
				else
				{
                    retVal.EmploymentDetails_EmploymentStatus = Guid.Empty;
                }
				if(!string.IsNullOrEmpty(item.Profession))
				{
					retVal.EmploymentDetails_Profession = new Guid(item.Profession);
				}
				else
				{
                    retVal.EmploymentDetails_Profession = Guid.Empty;

                }
				if(!string.IsNullOrEmpty(item.YearsInBusiness) && item.YearsInBusiness.Length < 8)
				{
					retVal.EmploymentDetails_YearsInBusiness = item.YearsInBusiness;
				}
				else
				{
					retVal.EmploymentDetails_YearsInBusiness = item.YearsInBusiness;

                }
				
				retVal.EmploymentDetails_EmployersName = item.EmployersName;
				retVal.EmploymentDetails_EmployersBusiness = item.EmployersBusiness;
				if(!string.IsNullOrEmpty(item.FormerProfession))
				{
					retVal.EmploymentDetails_FormerProfession = new Guid(item.FormerProfession);
				}
				else
				{
					retVal.EmploymentDetails_FormerProfession = Guid.Empty;

                }
				retVal.EmploymentDetails_FormerCountryOfEmployment = item.FormerCountryOfEmployment;
				retVal.EmploymentDetails_FormerEmployersName = item.FormerEmployersName;
				retVal.EmploymentDetails_FormerEmployersBusiness = item.FormerEmployersBusiness;
			}

			return retVal;
		}
	}
}
