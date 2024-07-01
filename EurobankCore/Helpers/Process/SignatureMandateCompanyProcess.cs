using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models.Application.Applicant.LegalEntity;
using Eurobank.Models.Application.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
	public class SignatureMandateCompanyProcess
	{
		private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

		private static readonly string _SignatureMandateCompanyFolderName = "Signature Mandate";

		public static SignatureMandateCompanyModel GetSignatureMandateCompanyModelById(int signatureMandateCompanyId)
		{
			SignatureMandateCompanyModel retVal = null;
			if(signatureMandateCompanyId > 0)
			{
				retVal = BindSignatureMandateCompanyModel(GetSignatureMandateCompanyById(signatureMandateCompanyId));
			}

			return retVal;
		}

		public static List<SignatureMandateCompanyModel> GetSignatureMandateCompany(int applicantId)
		{
			List<SignatureMandateCompanyModel> retVal = null;

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
					TreeNode signatureMandateCompanyRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatureMandateCompanyFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						signatureMandateCompanyRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatureMandateCompanyFolderName, StringComparison.OrdinalIgnoreCase));

						if(signatureMandateCompanyRoot != null)
						{
							List<TreeNode> signatureMandateCompanyNodes = signatureMandateCompanyRoot.Children.Where(u => u.ClassName == SignatureMandateCompany.CLASS_NAME).ToList();

							if(signatureMandateCompanyNodes != null && signatureMandateCompanyNodes.Count > 0)
							{
								retVal = new List<SignatureMandateCompanyModel>();
								signatureMandateCompanyNodes.ForEach(t => {
									SignatureMandateCompany signatureMandateCompany = SignatureMandateCompanyProvider.GetSignatureMandateCompany(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if(signatureMandateCompany != null)
									{
										SignatureMandateCompanyModel signatureMandateCompanyModel = BindSignatureMandateCompanyModel(signatureMandateCompany);
										if(signatureMandateCompanyModel != null)
										{
											retVal.Add(signatureMandateCompanyModel);
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

		public static SignatureMandateCompanyModel SaveSignatureMandateCompanyModel(int applicantId, SignatureMandateCompanyModel model)
		{
			SignatureMandateCompanyModel retVal = null;

			if(model != null && model.Id > 0)
			{
				SignatureMandateCompany signatureMandateCompany = GetSignatureMandateCompanyById(model.Id);
				if(signatureMandateCompany != null)
				{
					SignatureMandateCompany updatedSignatureMandateCompany = BindSignatureMandateCompany(signatureMandateCompany, model);
					if(updatedSignatureMandateCompany != null)
					{
						updatedSignatureMandateCompany.Update();
						model = BindSignatureMandateCompanyModel(updatedSignatureMandateCompany);
						retVal = model;
					}
				}
			}
			else if(applicantId > 0)
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
					TreeNode signatureMandateCompanyRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatureMandateCompanyFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						signatureMandateCompanyRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _SignatureMandateCompanyFolderName, StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						signatureMandateCompanyRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
						signatureMandateCompanyRoot.DocumentName = _SignatureMandateCompanyFolderName;
						signatureMandateCompanyRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						signatureMandateCompanyRoot.Insert(applicationDetailsNode);
					}
					SignatureMandateCompany signatureMandateCompany = BindSignatureMandateCompany(null, model);
					if(signatureMandateCompany != null && signatureMandateCompanyRoot != null)
					{
						string signatureMandateDocumentName = string.Empty;
						var allSignatureMandateCompanies = SignatureMandateCompanyProvider.GetSignatureMandateCompanies();
						if(allSignatureMandateCompanies != null && allSignatureMandateCompanies.Count > 0)
						{
							signatureMandateDocumentName = _SignatureMandateCompanyFolderName + " " + allSignatureMandateCompanies.Max(u => u.SignatureMandateCompanyID) + 1;
						}
						else
						{
							signatureMandateDocumentName = _SignatureMandateCompanyFolderName + " 1";
						}
						signatureMandateCompany.DocumentName = signatureMandateDocumentName;
						signatureMandateCompany.NodeName = signatureMandateDocumentName;
						signatureMandateCompany.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;

						signatureMandateCompany.Insert(signatureMandateCompanyRoot);
						model = BindSignatureMandateCompanyModel(signatureMandateCompany);
						retVal = model;
					}
				}
			}

			return retVal;
		}

		public static SignatureMandateCompany GetSignatureMandateCompanyById(int signatureMandateCompanyId)
		{
			SignatureMandateCompany retVal = null;

			if(signatureMandateCompanyId > 0)
			{
				var signatureMandateCompany = SignatureMandateCompanyProvider.GetSignatureMandateCompanies();
				if(signatureMandateCompany != null && signatureMandateCompany.Count > 0)
				{
					retVal = signatureMandateCompany.FirstOrDefault(o => o.SignatureMandateCompanyID == signatureMandateCompanyId);
				}
			}

			return retVal;
		}

		//public static List<SelectListItem> GetSignatureMandateCompanies()
		//{
		//	List<SelectListItem> retVal = null;

		//	var signatureMandateCompany = SignatureMandateCompanyProvider.GetSignatureMandateCompanies();
		//	if(signatureMandateCompany != null && signatureMandateCompany.Count > 0)
		//	{
		//		retVal = signatureMandateCompany.Select(t => new SelectListItem() { Value = t.NodeGUID.ToString(), Text = t.SignatureMandateCompany_EntityName }).ToList();
		//	}

		//	return retVal;
		//}

		#region Bind Data

		private static SignatureMandateCompanyModel BindSignatureMandateCompanyModel(SignatureMandateCompany item)
		{
			SignatureMandateCompanyModel retVal = null;

			if(item != null)
			{
				string authorizedSignatoryGroupNames = string.Empty;
				string authorizedSignatoryGroup1Names = string.Empty;
				var signtoryGroups = SignatoryGroupProcess.GetSignatoryGroups();
				var authorizedSignatoryGroupGuidList = GetAuthorizedSignatoryGroupGuidList(item.SignatureMandateCompany_AuthorizedSignatoryGroup);
				var authorizedSignatoryGroup1GuidList = GetAuthorizedSignatoryGroupGuidList(item.SignatureMandateCompany_AuthorizedSignatoryGroup1);
				if(signtoryGroups != null && signtoryGroups.Count > 0)
				{
					if(authorizedSignatoryGroupGuidList != null && authorizedSignatoryGroupGuidList.Length > 0)
					{
						authorizedSignatoryGroupNames = string.Join(',', signtoryGroups.Where(t => authorizedSignatoryGroupGuidList.Any(r => string.Equals(t.Value, r, StringComparison.OrdinalIgnoreCase))).Select(e => e.Text));
					}
					if(authorizedSignatoryGroup1GuidList != null && authorizedSignatoryGroup1GuidList.Length > 0)
					{
						authorizedSignatoryGroup1Names = string.Join(',', signtoryGroups.Where(t => authorizedSignatoryGroup1GuidList.Any(r => string.Equals(t.Value, r, StringComparison.OrdinalIgnoreCase))).Select(e => e.Text));
					}
				}
				
				
				retVal = new SignatureMandateCompanyModel()
				{
					Id = item.SignatureMandateCompanyID,
					AuthorizedSignatoryGroup = item.SignatureMandateCompany_AuthorizedSignatoryGroup,
					AuthorizedSignatoryGroupList = authorizedSignatoryGroupGuidList,
					AuthorizedSignatoryGroupName = authorizedSignatoryGroupNames,
					AuthorizedSignatoryGroup1 = item.SignatureMandateCompany_AuthorizedSignatoryGroup1,
					AuthorizedSignatoryGroup1List = authorizedSignatoryGroup1GuidList,
					AuthorizedSignatoryGroup1Name = authorizedSignatoryGroup1Names,
					LimitFrom = item.SignatureMandateCompany_LimitFrom,
					LimitTo = item.SignatureMandateCompany_LimitTo,
					MandateType = item.SignatureMandateCompany_Mandatetype,
					Description = item.SignatureMandateCompany_Description,
					NumberofSignatures = item.SignatureMandateCompany_NumberofSignatures,
					NumberofSignatures1 = item.SignatureMandateCompany_NumberofSignatures1,
					Rights = item.SignatureMandateCompany_Rights,
					TotalNumberofSignature = item.SignatureMandateCompany_TotalNumberofSignature,
					Status = item.Status,
					StatusName = item.Status ? GridRecordStatus.Complete.ToString() : GridRecordStatus.Pending.ToString()
				};
			}

			return retVal;
		}

		private static SignatureMandateCompany BindSignatureMandateCompany(SignatureMandateCompany existSignatureMandateCompany, SignatureMandateCompanyModel item)
		{
			SignatureMandateCompany retVal = new SignatureMandateCompany();
			if(existSignatureMandateCompany != null)
			{
				retVal = existSignatureMandateCompany;
			}
			if(item != null)
			{
				retVal.SignatureMandateCompany_AuthorizedSignatoryGroup = item.AuthorizedSignatoryGroup;
				retVal.SignatureMandateCompany_AuthorizedSignatoryGroup1 = item.AuthorizedSignatoryGroup1;
				retVal.SignatureMandateCompany_LimitFrom = item.LimitFrom != null ? Convert.ToDecimal( item.LimitFrom) : 0;
				retVal.SignatureMandateCompany_LimitTo = item.LimitTo != null ? Convert.ToDecimal( item.LimitTo) : 0;
				retVal.SignatureMandateCompany_Mandatetype = item.MandateType;
				retVal.SignatureMandateCompany_Description = item.Description;
				retVal.SignatureMandateCompany_NumberofSignatures =Convert.ToInt32( item.NumberofSignatures);
				retVal.SignatureMandateCompany_NumberofSignatures1 =Convert.ToInt32( item.NumberofSignatures1);
				retVal.SignatureMandateCompany_Rights = item.Rights;
				retVal.SignatureMandateCompany_TotalNumberofSignature =Convert.ToInt32( item.TotalNumberofSignature);
				retVal.Status = item.Status;
			}

			return retVal;
		}

		#endregion

		private static string[] GetAuthorizedSignatoryGroupGuidList(string authorizedSignatoryGroupGuidValues)
		{
			if(!string.IsNullOrEmpty(authorizedSignatoryGroupGuidValues))
				return authorizedSignatoryGroupGuidValues.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
			return null;
		}
	}
}
