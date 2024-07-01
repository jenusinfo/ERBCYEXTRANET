using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Models;
using Eurobank.Models.User;
using Eurobank.Models.Application.RelatedParty;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
	public class CompanyDetailsRelatedPartyProcess
	{
		private static readonly string _RealtedPartiesFolderName = "Related Parties";

		public static CompanyDetailsRelatedPartyModel GetCompanyDetailsRelatedPartyModelById(int companyDetailsRelatedPartyId)
		{
			CompanyDetailsRelatedPartyModel retVal = null;
			if(companyDetailsRelatedPartyId > 0)
			{
				retVal = BindCompanyDetailsRelatedPartyModel(GetCompanyDetailsRelatedPartyById(companyDetailsRelatedPartyId));
			}

			return retVal;
		}

		public static List<CompanyDetailsRelatedPartyModel> GetRelatedPartyCompanyDetailsRelatedParty(string applicationNumber)
		{
			List<CompanyDetailsRelatedPartyModel> retVal = null;

			TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
			if(!string.IsNullOrEmpty(applicationNumber))
			{
				TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

				if(applicationDetailsNode != null)
				{
					TreeNode companyDetailsRelatedPartyRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						companyDetailsRelatedPartyRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase));

						if(companyDetailsRelatedPartyRoot != null)
						{
							List<TreeNode> companyDetailsRelatedPartyNodes = companyDetailsRelatedPartyRoot.Children.Where(u => u.ClassName == CompanyDetailsRelatedParty.CLASS_NAME).ToList();

							if(companyDetailsRelatedPartyNodes != null && companyDetailsRelatedPartyNodes.Count > 0)
							{
								retVal = new List<CompanyDetailsRelatedPartyModel>();
								companyDetailsRelatedPartyNodes.ForEach(t => {
									CompanyDetailsRelatedParty companyDetailsRelatedParty = CompanyDetailsRelatedPartyProvider.GetCompanyDetailsRelatedParty(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

									if(companyDetailsRelatedParty != null)
									{
										CompanyDetailsRelatedPartyModel companyDetailsRelatedPartyModel = BindCompanyDetailsRelatedPartyModel(companyDetailsRelatedParty);
										if(companyDetailsRelatedPartyModel != null)
										{
											retVal.Add(companyDetailsRelatedPartyModel);
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

		public static CompanyDetailsRelatedPartyModel SaveRelatedPartyCompanyDetailsRelatedPartyModel(string applicationNumber, CompanyDetailsRelatedPartyModel model)
		{
			CompanyDetailsRelatedPartyModel retVal = null;

			if(model != null && model.Id > 0)
			{
				CompanyDetailsRelatedParty sourceOfOutgoingTransactions = GetCompanyDetailsRelatedPartyById(model.Id);
				if(sourceOfOutgoingTransactions != null)
				{
					CompanyDetailsRelatedParty updatedCompanyDetailsRelatedParty = BindCompanyDetailsRelatedParty(sourceOfOutgoingTransactions, model);
					if(updatedCompanyDetailsRelatedParty != null)
					{
						updatedCompanyDetailsRelatedParty.Update();
						model = BindCompanyDetailsRelatedPartyModel(updatedCompanyDetailsRelatedParty);
						retVal = model;
					}
				}
			}
			else if(!string.IsNullOrEmpty(applicationNumber) && model != null)
			{
				TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
				TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

				if(applicationDetailsNode != null)
				{
					TreeNode companyDetailsRelatedPartyRoot = null;
					if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase)))
					{
						companyDetailsRelatedPartyRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase));
					}
					else
					{
						companyDetailsRelatedPartyRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
						companyDetailsRelatedPartyRoot.DocumentName = _RealtedPartiesFolderName;
						companyDetailsRelatedPartyRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						companyDetailsRelatedPartyRoot.Insert(applicationDetailsNode);
					}
					CompanyDetailsRelatedParty companyDetailsRelatedParty = BindCompanyDetailsRelatedParty(null, model);
					if(companyDetailsRelatedParty != null && companyDetailsRelatedPartyRoot != null)
					{
						companyDetailsRelatedParty.DocumentName = model.RegisteredName;
						companyDetailsRelatedParty.NodeName = model.RegisteredName;
						companyDetailsRelatedParty.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
						companyDetailsRelatedParty.Insert(companyDetailsRelatedPartyRoot);
						model = BindCompanyDetailsRelatedPartyModel(companyDetailsRelatedParty);
						retVal = model;
					}
				}
			}

			return retVal;
		}

		public static CompanyDetailsRelatedParty GetCompanyDetailsRelatedPartyById(int companyDetailsRelatedPartyId)
		{
			CompanyDetailsRelatedParty retVal = null;

			if(companyDetailsRelatedPartyId > 0)
			{
				var companyDetailsRelatedParty = CompanyDetailsRelatedPartyProvider.GetCompanyDetailsRelatedParties();
				if(companyDetailsRelatedParty != null && companyDetailsRelatedParty.Count > 0)
				{
					retVal = companyDetailsRelatedParty.FirstOrDefault(o => o.CompanyDetailsRelatedPartyID == companyDetailsRelatedPartyId);
				}
			}

			return retVal;
		}

		#region Bind Data

		private static CompanyDetailsRelatedPartyModel BindCompanyDetailsRelatedPartyModel(CompanyDetailsRelatedParty item)
		{
			CompanyDetailsRelatedPartyModel retVal = null;

			if(item != null)
			{
				retVal = new CompanyDetailsRelatedPartyModel()
				{
					Id = item.CompanyDetailsRelatedPartyID,
					CountryofIncorporation = item.CompanyDetails_CountryofIncorporation,
					DateofIncorporation = item.CompanyDetails_DateofIncorporation,
					EntityType = item.CompanyDetails_EntityType,
					RegisteredName = item.CompanyDetails_RegisteredName,
					RegistrationNumber = item.CompanyDetails_RegistrationNumber,
					NodeGUID = ValidationHelper.GetString(item.NodeGUID, ""),
					NodePath = item.NodeAliasPath,
					IsPep=item.CompanyDetails_IsPep,
					IsRelatedToPep = item.CompanyDetails_IsRelatedToPep,
					Type=item.CompanyDetails_Type,
					IDVerified=item.CompanyDetails_IdVerified,
					Invited=item.CompanyDetails_Invited,
					IsRelatedPartyUBO=item.CompanyDetails_IsRelatedPartyUBO,
					Status = item.CompanyDetails_Status,
					CustomerCIF = item.CustomerCIF
				};
			}

			return retVal;
		}

		private static CompanyDetailsRelatedParty BindCompanyDetailsRelatedParty(CompanyDetailsRelatedParty existCompanyDetailsRelatedParty, CompanyDetailsRelatedPartyModel item)
		{
			CompanyDetailsRelatedParty retVal = new CompanyDetailsRelatedParty();
			if(existCompanyDetailsRelatedParty != null)
			{
				retVal = existCompanyDetailsRelatedParty;
			}
			if(item != null)
			{
				if (!string.IsNullOrEmpty(item.RegisteredName))
				{
					retVal.CompanyDetails_RegisteredName = item.RegisteredName;
				}
				retVal.CompanyDetails_EntityType = item.EntityType;
				retVal.CompanyDetails_DateofIncorporation =Convert.ToDateTime( item.DateofIncorporation);
				retVal.CompanyDetails_CountryofIncorporation = item.CountryofIncorporation;
				retVal.CompanyDetails_RegistrationNumber = item.RegistrationNumber;
				retVal.CompanyDetails_IsPep = item.IsPep;
				retVal.CompanyDetails_IsRelatedToPep = item.IsRelatedToPep;
				retVal.CompanyDetails_Type = item.Type;
				retVal.CompanyDetails_IdVerified = item.IDVerified;
				retVal.CompanyDetails_Invited = item.Invited;
				retVal.CompanyDetails_IsRelatedPartyUBO = item.IsRelatedPartyUBO;
				retVal.CompanyDetails_Status = item.Status;
				retVal.PersonsRegistryID = item.RegistryId;
			}

			return retVal;
		}

		#endregion
		#region Company Registry Methods for Related Party

		public static CompanyRegistry SaveCompanyRegistryRelatedParty(string userName, RegistriesRepository registriesRepository, CompanyDetailsRelatedPartyModel companyDetailsModel)
		{
			CompanyRegistry retVal = null;

			if (companyDetailsModel != null && registriesRepository != null)
			{
				Eurobank.Models.Registries.PersonsRegistry personsRegistry = BindCompanyPersonRegistryRelatedPartyModel(companyDetailsModel);
				TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

				if (personsRegistry.Id > 0)
				{
					var UserRegistry = registriesRepository.GetCompanyRegistryUserById(personsRegistry.Id);
					if (UserRegistry != null)
					{
						var manager = VersionManager.GetInstance(tree);

						UserRegistry.DocumentName = personsRegistry.RegisteredName;

						UserRegistry.SetValue("CompanyDetails_PersonType", personsRegistry.ApplicationType);
						UserRegistry.SetValue("CompanyDetails_RegisteredName", personsRegistry.RegisteredName);
						UserRegistry.SetValue("CompanyDetails_EntityType", personsRegistry.EntityType);
						UserRegistry.SetValue("CompanyDetails_CountryofIncorporation", personsRegistry.CountryofIncorporation);
						UserRegistry.SetValue("CompanyDetails_RegistrationNumber", personsRegistry.RegistrationNumber);
						UserRegistry.SetValue("CompanyDetails_DateofIncorporation", personsRegistry.DateofIncorporation);

						personsRegistry.CreatedDate = Convert.ToDateTime(UserRegistry.DocumentCreatedWhen).ToString("MM/dd/yyyyy HH:mm:ss");
						personsRegistry.ModyfiedDate = Convert.ToDateTime(UserRegistry.DocumentModifiedWhen).ToString("MM/dd/yyyyy HH:mm:ss");
						retVal = UserRegistry;
					}
				}
				else
				{
					UserModel userModel = UserProcess.GetUser(userName);
					var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);
					//var UserRegistry = registriesRepository.GetRegistryUserByName(userName);
					if (UserRegistry != null)
					{
						CMS.DocumentEngine.TreeNode personsfoldernode_parent = tree.SelectNodes()
						   .Path(UserRegistry.NodeAliasPath + "/" + "Persons-Registry")
						   .OnCurrentSite()
						   .Published(false)
						   .FirstOrDefault();
						if (personsfoldernode_parent == null)
						{
							CMS.DocumentEngine.TreeNode Personsfoldernode = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
							Personsfoldernode.DocumentName = "Persons Registry";
							Personsfoldernode.DocumentCulture = "en-US";
							Personsfoldernode.Insert(UserRegistry);
							personsfoldernode_parent = Personsfoldernode;
						}
						CMS.DocumentEngine.TreeNode personsRegistryAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.CompanyRegistry", tree);
						personsRegistryAdd.SetValue("CompanyDetails_PersonType", personsRegistry.ApplicationType);
						personsRegistryAdd.SetValue("CompanyDetails_RegisteredName", personsRegistry.RegisteredName);
						personsRegistryAdd.SetValue("CompanyDetails_EntityType", personsRegistry.EntityType);
						personsRegistryAdd.SetValue("CompanyDetails_CountryofIncorporation", personsRegistry.CountryofIncorporation);
						personsRegistryAdd.SetValue("CompanyDetails_RegistrationNumber", personsRegistry.RegistrationNumber);
						personsRegistryAdd.SetValue("CompanyDetails_DateofIncorporation", personsRegistry.DateofIncorporation);

						personsRegistryAdd.Insert(personsfoldernode_parent);
						personsRegistry.CreatedDate = Convert.ToDateTime(personsRegistryAdd.DocumentCreatedWhen).ToString("MM/dd/yyyyy HH:mm:ss");
						personsRegistry.ModyfiedDate = Convert.ToDateTime(personsRegistryAdd.DocumentModifiedWhen).ToString("MM/dd/yyyyy HH:mm:ss");
						retVal = CompanyRegistryProvider.GetCompanyRegistry(personsRegistryAdd.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
					}
					else
					{
						CMS.DocumentEngine.TreeNode personsRegistryFolder = tree.SelectNodes()
					   .Path("/Registries")
					   .Published(false)
					   .OnCurrentSite()
					   .FirstOrDefault();
						TreeNode personsRegistryUser = TreeNode.New("Eurobank.PersonsRegistryUser", tree);
						UserInfo user = UserInfoProvider.GetUserInfo(userName);
						personsRegistryUser.DocumentName = user.UserName;
						personsRegistryUser.DocumentCulture = "en-US";
						personsRegistryUser.SetValue("UserID", user.UserID);
						personsRegistryUser.Insert(personsRegistryFolder);
						CMS.DocumentEngine.TreeNode Personsfoldernode = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
						Personsfoldernode.DocumentName = "Persons Registry";
						Personsfoldernode.DocumentCulture = "en-US";
						Personsfoldernode.Insert(personsRegistryUser);
						CMS.DocumentEngine.TreeNode personsRegistryAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.CompanyRegistry", tree);
						personsRegistryAdd.SetValue("CompanyDetails_PersonType", personsRegistry.ApplicationType);
						personsRegistryAdd.SetValue("CompanyDetails_RegisteredName", personsRegistry.RegisteredName);
						personsRegistryAdd.SetValue("CompanyDetails_EntityType", personsRegistry.EntityType);
						personsRegistryAdd.SetValue("CompanyDetails_CountryofIncorporation", personsRegistry.CountryofIncorporation);
						personsRegistryAdd.SetValue("CompanyDetails_RegistrationNumber", personsRegistry.RegistrationNumber);
						personsRegistryAdd.SetValue("CompanyDetails_DateofIncorporation", personsRegistry.DateofIncorporation);

						personsRegistryAdd.Insert(Personsfoldernode);
						personsRegistry.CreatedDate = Convert.ToDateTime(personsRegistryAdd.DocumentCreatedWhen).ToString("MM/dd/yyyyy HH:mm:ss");
						personsRegistry.ModyfiedDate = Convert.ToDateTime(personsRegistryAdd.DocumentModifiedWhen).ToString("MM/dd/yyyyy HH:mm:ss");

						retVal = CompanyRegistryProvider.GetCompanyRegistry(personsRegistryAdd.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

					}
				}


			}

			return retVal;
		}
		public static Eurobank.Models.Registries.PersonsRegistry BindCompanyPersonRegistryRelatedPartyModel(CompanyDetailsRelatedPartyModel item)
		{
			Eurobank.Models.Registries.PersonsRegistry retVal = null;

			if (item != null)
			{
				retVal = new Eurobank.Models.Registries.PersonsRegistry()
				{
					Id = item.RegistryId,
					ApplicationType = item.Application_Type,
					RegisteredName = item.RegisteredName,
					EntityType = item.EntityType,
					CountryofIncorporation = item.CountryofIncorporation,
					RegistrationNumber = item.RegistrationNumber,
					DateofIncorporation = item.DateofIncorporation


				};
			}

			return retVal;
		}
		#endregion
	}
}
