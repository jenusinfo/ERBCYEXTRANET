using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Applicant;
using Eurobank.Models.Application.RelatedParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
	public class EmploymentDetailsRelatedPartyProcess
	{
		private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

		private static readonly string _EmploymentDetailsRelatedPartyFolderName = "Business Profile";

		public static List<EmploymentDetailsRelatedPartyModel> GetEmploymentDetailsRelatedPartyModels(int relatedPartyId)
		{
			List<EmploymentDetailsRelatedPartyModel> retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if(relatedPartyId > 0)
			{
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(PersonalDetails.CLASS_NAME)
					.WhereEquals("PersonalDetailsID", relatedPartyId)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					retVal = new List<EmploymentDetailsRelatedPartyModel>();
					TreeNode employmentDetailsRelatedPartyFolderRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EmploymentDetailsRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						employmentDetailsRelatedPartyFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EmploymentDetailsRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase));

						if(employmentDetailsRelatedPartyFolderRoot != null)
						{
							List<TreeNode> employmentDetailsRelatedPartyNodes = employmentDetailsRelatedPartyFolderRoot.Children.Where(u => u.ClassName == EmploymentDetailsRelatedParty.CLASS_NAME).ToList();

							if(employmentDetailsRelatedPartyNodes != null && employmentDetailsRelatedPartyNodes.Count > 0)
							{
								employmentDetailsRelatedPartyNodes.ForEach(t => {
									EmploymentDetailsRelatedParty employmentDetailsRelatedParty = EmploymentDetailsRelatedPartyProvider.GetEmploymentDetailsRelatedParty(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if(employmentDetailsRelatedParty != null)
									{
										EmploymentDetailsRelatedPartyModel employmentDetailsRelatedPartyModel = BindEmploymentDetailsRelatedPartyModel(employmentDetailsRelatedParty);
										if(employmentDetailsRelatedPartyModel != null)
										{
											retVal.Add(employmentDetailsRelatedPartyModel);
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

		public static EmploymentDetailsRelatedPartyModel GetEmploymentDetailsRelatedPartyModelByRelatedPartyId(int relatedPartyId)
		{
			EmploymentDetailsRelatedPartyModel retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if(relatedPartyId > 0)
			{
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(PersonalDetails.CLASS_NAME)
					.WhereEquals("PersonalDetailsID", relatedPartyId)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					TreeNode employmentDetailsRelatedPartyFolderRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EmploymentDetailsRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						employmentDetailsRelatedPartyFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EmploymentDetailsRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase));

						if(employmentDetailsRelatedPartyFolderRoot != null)
						{
							List<TreeNode> employmentDetailsRelatedPartyNodes = employmentDetailsRelatedPartyFolderRoot.Children.Where(u => u.ClassName == EmploymentDetailsRelatedParty.CLASS_NAME).ToList();

							if(employmentDetailsRelatedPartyNodes != null && employmentDetailsRelatedPartyNodes.Count > 0)
							{
								EmploymentDetailsRelatedParty employmentDetailsRelatedParty = EmploymentDetailsRelatedPartyProvider.GetEmploymentDetailsRelatedParty(employmentDetailsRelatedPartyNodes.FirstOrDefault().NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

								if(employmentDetailsRelatedParty != null)
								{
									retVal = BindEmploymentDetailsRelatedPartyModel(employmentDetailsRelatedParty);
								}
							}
						}
					}
				}
			}

			return retVal;
		}

		public static EmploymentDetailsRelatedPartyModel GetEmploymentDetailsRelatedPartyModelById(int employmentDetailsRelatedPartyId)
		{
			EmploymentDetailsRelatedPartyModel retVal = null;
			if(employmentDetailsRelatedPartyId > 0)
			{
				retVal = BindEmploymentDetailsRelatedPartyModel(GetEmploymentDetailsRelatedPartyById(employmentDetailsRelatedPartyId));
			}

			return retVal;
		}

		public static EmploymentDetailsRelatedPartyModel SaveEmploymentDetailsRelatedPartyModel(int relatedPartyId, EmploymentDetailsRelatedPartyModel model)
		{
			EmploymentDetailsRelatedPartyModel retVal = null;

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
				EmploymentDetailsRelatedParty employmentDetailsRelatedParty = GetEmploymentDetailsRelatedPartyById(model.Id);
				if(employmentDetailsRelatedParty != null)
				{
					EmploymentDetailsRelatedParty updatedEmploymentDetailsRelatedParty = BindEmploymentDetailsRelatedParty(employmentDetailsRelatedParty, model);
					if(updatedEmploymentDetailsRelatedParty != null)
					{
						updatedEmploymentDetailsRelatedParty.Update();
						retVal = BindEmploymentDetailsRelatedPartyModel(updatedEmploymentDetailsRelatedParty);
					}
				}
			}
			else if(relatedPartyId > 0 && model != null)
			{
				TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
				TreeNode applicationDetailsNode = tree.SelectNodes()
					.Path(_ApplicationRootNodePath, PathTypeEnum.Children)
					.Type(PersonalDetails.CLASS_NAME)
					.WhereEquals("PersonalDetailsID", relatedPartyId)
					.OnCurrentSite()
					.Published(false)
					.FirstOrDefault();

				if(applicationDetailsNode != null)
				{
					TreeNode employmentDetailsRelatedPartyFolderRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EmploymentDetailsRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						employmentDetailsRelatedPartyFolderRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _EmploymentDetailsRelatedPartyFolderName, StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						employmentDetailsRelatedPartyFolderRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
						employmentDetailsRelatedPartyFolderRoot.DocumentName = _EmploymentDetailsRelatedPartyFolderName;
						employmentDetailsRelatedPartyFolderRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						employmentDetailsRelatedPartyFolderRoot.Insert(applicationDetailsNode);
					}
					EmploymentDetailsRelatedParty employmentDetailsRelatedParty = BindEmploymentDetailsRelatedParty(null, model);
					if(employmentDetailsRelatedParty != null && employmentDetailsRelatedPartyFolderRoot != null)
					{
						employmentDetailsRelatedParty.DocumentName = model.EmploymentStatusName;
						employmentDetailsRelatedParty.NodeName = model.EmploymentStatusName;
						employmentDetailsRelatedParty.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						employmentDetailsRelatedParty.Insert(employmentDetailsRelatedPartyFolderRoot);
						model = BindEmploymentDetailsRelatedPartyModel(employmentDetailsRelatedParty);
						retVal = model;
					}
				}
			}

			return retVal;
		}

		private static EmploymentDetailsRelatedParty GetEmploymentDetailsRelatedPartyById(int employmentDetailsRelatedPartyId)
		{
			EmploymentDetailsRelatedParty retVal = null;

			if(employmentDetailsRelatedPartyId > 0)
			{
				var employmentDetailsRelatedParty = EmploymentDetailsRelatedPartyProvider.GetEmploymentDetailsRelatedParties();
				if(employmentDetailsRelatedParty != null && employmentDetailsRelatedParty.Count > 0)
				{
					retVal = employmentDetailsRelatedParty.FirstOrDefault(o => o.EmploymentDetailsRelatedPartyID == employmentDetailsRelatedPartyId);
				}
			}

			return retVal;
		}

		private static EmploymentDetailsRelatedPartyModel BindEmploymentDetailsRelatedPartyModel(EmploymentDetailsRelatedParty item)
		{
			EmploymentDetailsRelatedPartyModel retVal = null;

			if(item != null)
			{
				var employmentProfessions = ServiceHelper.GetEmploymentProfessions();
				var employmentStatus = ServiceHelper.GetEmploymentStatus();
				retVal = new EmploymentDetailsRelatedPartyModel()
				{
					Id = item.EmploymentDetailsRelatedPartyID,
					EmploymentStatus=Convert.ToString( item.EmploymentDetails_EmploymentStatus),
					EmploymentStatusName= (employmentStatus != null && employmentStatus.Any(l => l.Value == item.EmploymentDetails_EmploymentStatus.ToString())) ? employmentStatus.FirstOrDefault(l => l.Value == item.EmploymentDetails_EmploymentStatus.ToString()).Text : string.Empty,
					Profession = item.EmploymentDetails_Profession.ToString(),
					ProfessionName = (employmentProfessions != null && employmentProfessions.Any(l => l.Value == item.EmploymentDetails_Profession.ToString())) ? employmentProfessions.FirstOrDefault(l => l.Value == item.EmploymentDetails_Profession.ToString()).Text : string.Empty,
					EmployersName = item.EmploymentDetails_EmployersName
				};
			}

			return retVal;
		}

		private static EmploymentDetailsRelatedParty BindEmploymentDetailsRelatedParty(EmploymentDetailsRelatedParty employmentDetailsRelatedParty, EmploymentDetailsRelatedPartyModel item)
		{
			EmploymentDetailsRelatedParty retVal = new EmploymentDetailsRelatedParty();
			if(employmentDetailsRelatedParty != null)
			{
				retVal = employmentDetailsRelatedParty;
			}
			if(item != null)
			{
				if(!string.IsNullOrEmpty(item.Profession))
				{
					retVal.EmploymentDetails_Profession = new Guid(item.Profession);
				}
				if (!string.IsNullOrEmpty(item.EmploymentStatus))
				{
					retVal.EmploymentDetails_EmploymentStatus =new Guid( item.EmploymentStatus);
				}
				retVal.EmploymentDetails_EmployersName = item.EmployersName;
			}

			return retVal;
		}
	}
}
