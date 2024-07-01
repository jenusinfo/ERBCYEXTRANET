using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Models;
using Eurobank.Models.Application.Applicant.LegalEntity.ContactDetails;
using Eurobank.Models.Application.Common;
using Eurobank.Models.User;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class CompanyDetailsProcess
    {
        public const string _ApplicantsFolderName = "Applicants";

        private static readonly string _RealtedPartiesFolderName = "Related Parties";

        public static CompanyDetailsModel GetCompanyDetailsModelById(int companyDetailsId)
        {
            CompanyDetailsModel retVal = null;
            if (companyDetailsId > 0)
            {
                retVal = BindCompanyDetailsModel(GetCompanyDetailsById(companyDetailsId));
            }

            return retVal;
        }

        public static List<CompanyDetailsModel> GetApplicantCompanyDetails(string applicationNumber)
        {
            List<CompanyDetailsModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (!string.IsNullOrEmpty(applicationNumber))
            {
                TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

                if (applicationDetailsNode != null)
                {
                    TreeNode companyDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        companyDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (companyDetailsRoot != null)
                        {
                            List<TreeNode> companyDetailsNodes = companyDetailsRoot.Children.Where(u => u.ClassName == CompanyDetails.CLASS_NAME).ToList();

                            if (companyDetailsNodes != null && companyDetailsNodes.Count > 0)
                            {
                                retVal = new List<CompanyDetailsModel>();
                                companyDetailsNodes.ForEach(t =>
                                {
                                    CompanyDetails companyDetails = CompanyDetailsProvider.GetCompanyDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (companyDetails != null)
                                    {
                                        CompanyDetailsModel companyDetailsModel = BindCompanyDetailsModel(companyDetails);
                                        if (companyDetailsModel != null)
                                        {
                                            retVal.Add(companyDetailsModel);
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

        public static List<CompanyDetailsModel> GetRelatedPartyCompanyDetails(string applicationNumber)
        {
            List<CompanyDetailsModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (!string.IsNullOrEmpty(applicationNumber))
            {
                TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

                if (applicationDetailsNode != null)
                {
                    TreeNode companyDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        companyDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase));

                        if (companyDetailsRoot != null)
                        {
                            List<TreeNode> companyDetailsNodes = companyDetailsRoot.Children.Where(u => u.ClassName == CompanyDetails.CLASS_NAME).ToList();

                            if (companyDetailsNodes != null && companyDetailsNodes.Count > 0)
                            {
                                retVal = new List<CompanyDetailsModel>();
                                companyDetailsNodes.ForEach(t =>
                                {
                                    CompanyDetails companyDetails = CompanyDetailsProvider.GetCompanyDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (companyDetails != null)
                                    {
                                        CompanyDetailsModel companyDetailsModel = BindCompanyDetailsModel(companyDetails);
                                        if (companyDetailsModel != null)
                                        {
                                            retVal.Add(companyDetailsModel);
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

        //public static List<SelectListItem> GetEBankingSubscribers(string applicationNumber)
        //{
        //	List<SelectListItem> retVal = new List<SelectListItem>();

        //	TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
        //	if(!string.IsNullOrEmpty(applicationNumber))
        //	{
        //		TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

        //		if(applicationDetailsNode != null)
        //		{
        //			TreeNode companyDetailsRoot = null;
        //			if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
        //			{
        //				companyDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));

        //				if(companyDetailsRoot != null)
        //				{
        //					List<TreeNode> companyDetailsNodes = companyDetailsRoot.Children.Where(u => u.ClassName == CompanyDetails.CLASS_NAME).ToList();

        //					if(companyDetailsNodes != null && companyDetailsNodes.Count > 0)
        //					{
        //						//retVal = new List<SelectListItem>();
        //						companyDetailsNodes.ForEach(t => {
        //							CompanyDetails companyDetails = CompanyDetailsProvider.GetCompanyDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

        //							if(companyDetails != null)
        //							{
        //								CompanyDetailsModel companyDetailsModel = BindCompanyDetailsModel(companyDetails);
        //								if(companyDetailsModel != null)
        //								{
        //									retVal.Add(new SelectListItem() { Value = Convert.ToString(companyDetails.NodeGUID), Text = companyDetails.CompanyDetails_FirstName + " " + companyDetails.CompanyDetails_LastName });
        //								}
        //							}
        //						});
        //					}
        //				}
        //			}
        //			if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase)))
        //			{
        //				//Need to filter related party for 'e subscriber' role
        //				companyDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase));

        //				if(companyDetailsRoot != null)
        //				{
        //					List<TreeNode> companyDetailsNodes = companyDetailsRoot.Children.Where(u => u.ClassName == CompanyDetails.CLASS_NAME).ToList();

        //					if(companyDetailsNodes != null && companyDetailsNodes.Count > 0)
        //					{
        //						//retVal = new List<SelectListItem>();
        //						companyDetailsNodes.ForEach(t => {
        //							CompanyDetails companyDetails = CompanyDetailsProvider.GetCompanyDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

        //							if(companyDetails != null)
        //							{
        //								CompanyDetailsModel companyDetailsModel = BindCompanyDetailsModel(companyDetails);
        //								if(companyDetailsModel != null)
        //								{
        //									retVal.Add(new SelectListItem() { Value = Convert.ToString(companyDetails.NodeGUID), Text = companyDetails.CompanyDetails_FirstName + " " + companyDetails.CompanyDetails_LastName });
        //								}
        //							}
        //						});
        //					}
        //				}
        //			}
        //		}
        //	}

        //	return retVal;
        //}

        public static CompanyDetailsModel SaveApplicantCompanyDetailsModel(string applicationNumber, CompanyDetailsModel model)
        {
            CompanyDetailsModel retVal = null;

            if (model != null && model.Id > 0)
            {
                CompanyDetails sourceOfOutgoingTransactions = GetCompanyDetailsById(model.Id);
                if (sourceOfOutgoingTransactions != null)
                {
                    CompanyDetails updatedCompanyDetails = BindCompanyDetails(sourceOfOutgoingTransactions, model);
                    if (updatedCompanyDetails != null)
                    {
                        updatedCompanyDetails.Update();
                        retVal = BindCompanyDetailsModel(updatedCompanyDetails);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(applicationNumber) && model != null)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

                if (applicationDetailsNode != null)
                {
                    TreeNode companyDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        companyDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        companyDetailsRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        companyDetailsRoot.DocumentName = "Applicants";
                        companyDetailsRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        companyDetailsRoot.Insert(applicationDetailsNode);
                    }
                    CompanyDetails companyDetails = BindCompanyDetails(null, model);
                    if (companyDetails != null && companyDetailsRoot != null)
                    {
                        companyDetails.DocumentName = model.RegisteredName;
                        companyDetails.NodeName = model.RegisteredName;
                        companyDetails.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        companyDetails.Insert(companyDetailsRoot);
                        model = BindCompanyDetailsModel(companyDetails);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        public static CompanyDetailsModel SaveRelatedPartyCompanyDetailsModel(string applicationNumber, CompanyDetailsModel model)
        {
            CompanyDetailsModel retVal = null;

            if (model != null && model.Id > 0)
            {
                CompanyDetails sourceOfOutgoingTransactions = GetCompanyDetailsById(model.Id);
                if (sourceOfOutgoingTransactions != null)
                {
                    CompanyDetails updatedCompanyDetails = BindCompanyDetails(sourceOfOutgoingTransactions, model);
                    if (updatedCompanyDetails != null)
                    {
                        updatedCompanyDetails.Update();
                        model = BindCompanyDetailsModel(updatedCompanyDetails);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(applicationNumber) && model != null)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

                if (applicationDetailsNode != null)
                {
                    TreeNode companyDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        companyDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        companyDetailsRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        companyDetailsRoot.DocumentName = _RealtedPartiesFolderName;
                        companyDetailsRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        companyDetailsRoot.Insert(applicationDetailsNode);
                    }
                    CompanyDetails companyDetails = BindCompanyDetails(null, model);
                    if (companyDetails != null && companyDetailsRoot != null)
                    {
                        companyDetails.DocumentName = model.RegisteredName;
                        companyDetails.NodeName = model.RegisteredName;
                        companyDetails.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        companyDetails.Insert(companyDetailsRoot);
                        model = BindCompanyDetailsModel(companyDetails);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        public static CompanyDetails GetCompanyDetailsById(int companyDetailsId)
        {
            CompanyDetails retVal = null;

            if (companyDetailsId > 0)
            {
                var companyDetails = CompanyDetailsProvider.GetCompanyDetails();
                if (companyDetails != null && companyDetails.Count > 0)
                {
                    retVal = companyDetails.FirstOrDefault(o => o.CompanyDetailsID == companyDetailsId);
                }
            }

            return retVal;
        }

        #region Bind Data

        private static CompanyDetailsModel BindCompanyDetailsModel(CompanyDetails item)
        {
            CompanyDetailsModel retVal = null;

            if (item != null)
            {
                retVal = new CompanyDetailsModel()
                {
                    Id = item.CompanyDetailsID,
                    //CorporationSharesIssuedToTheBearer = item.CompanyDetails_CorporationSharesIssuedToTheBearer,
                    SharesIssuedToTheBearerName = item.CompanyDetails_CorporationSharesIssuedToTheBearer,
                    CountryofIncorporation = item.CompanyDetails_CountryofIncorporation,
                    DateofIncorporation = item.CompanyDetails_DateofIncorporation,
                    EntityType = item.CompanyDetails_EntityType,
                    //IstheEntitylocatedandoperatesanofficeinCyprus = item.CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus,
                    IsOfficeinCyprusName = item.CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus,
                    ListingStatus = item.CompanyDetails_ListingStatus,
                    RegisteredName = item.CompanyDetails_RegisteredName,
                    RegistrationNumber = item.CompanyDetails_RegistrationNumber,
                    TradingName = item.CompanyDetails_TradingName,
                    NodeGUID = ValidationHelper.GetString(item.NodeGUID, ""),
                    NodePath = item.NodeAliasPath,
                    //HasAccountInOtherBank = item.CompanyDetails_HasAccountInOtherBank,
                    HasAccountInOtherBankName = item.CompanyDetails_HasAccountInOtherBank,

                    NameOfBankingInstitution = item.CompanyDetails_NameOfBankingInstitution,
                    CountryOfBankingInstitution = item.CompanyDetails_CountryOfBankingInstitution.ToString(),
                    IsLiableToPayDefenseTaxInCyprusName = item.CompanyDetails_IsLiableToPayDefenseTaxInCyprus,
                    Type = item.CompanyDetails_Type,
                    IDVerified = item.CompanyDetails_IdVerified,
                    Invited = item.CompanyDetails_Invited,
                    RegistryId = item.PersonsRegistryID,
                    Status = item.CompanyDetails_Status,
                    CreatedDateTime = item.DocumentCreatedWhen,
                    CustomerCIF = item.CustomerCIF
                };
            }

            return retVal;
        }

        private static CompanyDetails BindCompanyDetails(CompanyDetails existCompanyDetails, CompanyDetailsModel item)
        {
            CompanyDetails retVal = new CompanyDetails();
            if (existCompanyDetails != null)
            {
                retVal = existCompanyDetails;
            }
            if (item != null)
            {
                if (!string.IsNullOrEmpty(item.RegisteredName))
                {
                    retVal.CompanyDetails_RegisteredName = item.RegisteredName;
                }

                retVal.CompanyDetails_EntityType = item.EntityType;
                if (item.DateofIncorporation.HasValue)
                {
                    retVal.CompanyDetails_DateofIncorporation = item.DateofIncorporation.Value;
                }
                retVal.CompanyDetails_CountryofIncorporation = item.CountryofIncorporation;
                //retVal.CompanyDetails_CorporationSharesIssuedToTheBearer = item.CorporationSharesIssuedToTheBearer;
                //retVal.CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus = item.IstheEntitylocatedandoperatesanofficeinCyprus;
                retVal.CompanyDetails_ListingStatus = item.ListingStatus;
                retVal.CompanyDetails_RegistrationNumber = item.RegistrationNumber;
                retVal.CompanyDetails_TradingName = item.TradingName;
                retVal.CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus = (string.IsNullOrEmpty(item.IsOfficeinCyprusName) ? "--SELECT--" : item.IsOfficeinCyprusName);
                retVal.CompanyDetails_CorporationSharesIssuedToTheBearer = (string.IsNullOrEmpty(item.SharesIssuedToTheBearerName) ? "--SELECT--" : item.SharesIssuedToTheBearerName);
                retVal.CompanyDetails_HasAccountInOtherBank = item.HasAccountInOtherBankName;
                retVal.CompanyDetails_NameOfBankingInstitution = item.NameOfBankingInstitution;
                retVal.CompanyDetails_IsLiableToPayDefenseTaxInCyprus = item.IsLiableToPayDefenseTaxInCyprusName;
                retVal.CompanyDetails_CountryOfBankingInstitution = ValidationHelper.GetInteger(item.CountryOfBankingInstitution, 0);
                retVal.CompanyDetails_Type = item.Type;
                retVal.CompanyDetails_IdVerified = item.IDVerified;
                retVal.CompanyDetails_Invited = item.Invited;
                retVal.PersonsRegistryID = item.RegistryId;
                retVal.CompanyDetails_Status = item.Status;
            }

            return retVal;
        }

        public static PersonRegistyLegalSearchModel BindPersonRegistyLegalSearchModel(Eurobank.Models.Registries.PersonsRegistry item)
        {
            PersonRegistyLegalSearchModel retVal = null;

            if (item != null)
            {
                retVal = new PersonRegistyLegalSearchModel()
                {
                    PersonRegistryId = !string.IsNullOrEmpty(item.NodeID) ? Convert.ToInt32(item.NodeID) : 0,
                    PersonType = item.ApplicationTypeName,
                    RegisteredName = item.RegisteredName,
                    TradingName = item.TradingName,
                    EntityType = item.EntityType,
                    CountryofIncorporation = item.CountryofIncorporation,
                    RegistrationNumber = item.RegistrationNumber,
                    DateofIncorporation = Convert.ToDateTime(item.DateofIncorporation).ToString("dd/MM/yyyy"),
                    ListingStatus = item.ListingStatus,
                    CorporationSharesIssuedToTheBearer = item.CorporationSharesIssuedToTheBearer,
                    IstheEntitylocatedandoperatesanofficeinCyprus = item.IstheEntitylocatedandoperatesanofficeinCyprus,
                    SharesIssuedToTheBearerName = item.SharesIssuedToTheBearerName,
                    IsOfficeinCyprusName = item.IsOfficeinCyprusName,
                    ContactDetailsLegal_PreferredMailingAddress = item.ContactDetailsLegal_PreferredMailingAddress,
                    ContactDetailsLegal_EmailAddressForSendingAlerts = item.ContactDetailsLegal_EmailAddressForSendingAlerts,
                    ContactDetailsLegal_PreferredCommunicationLanguage = item.ContactDetailsLegal_PreferredCommunicationLanguage,
                    CreatedDate = item.CreatedDate,
                    ModyfiedDate = item.ModyfiedDate,
                    NodeAliaspath = item.NodeAliaspath,



                };
            }

            return retVal;
        }
        #endregion

        #region Company Registry Methods

        public static CompanyRegistry SaveCompanyRegistry(string userName, RegistriesRepository registriesRepository, CompanyDetailsModel companyDetailsModel, ContactDetailsLegalModel contactDetails)
        {
            CompanyRegistry retVal = null;

            if (companyDetailsModel != null && registriesRepository != null)
            {
                Eurobank.Models.Registries.PersonsRegistry personsRegistry = BindCompanyPersonRegistryModel(companyDetailsModel, contactDetails);
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
                        UserRegistry.SetValue("CompanyDetails_TradingName", personsRegistry.TradingName);
                        UserRegistry.SetValue("CompanyDetails_EntityType", personsRegistry.EntityType);
                        UserRegistry.SetValue("CompanyDetails_CountryofIncorporation", personsRegistry.CountryofIncorporation);
                        UserRegistry.SetValue("CompanyDetails_RegistrationNumber", personsRegistry.RegistrationNumber);
                        UserRegistry.SetValue("CompanyDetails_DateofIncorporation", personsRegistry.DateofIncorporation);
                        UserRegistry.SetValue("CompanyDetails_ListingStatus", personsRegistry.ListingStatus);
                        UserRegistry.SetValue("CompanyDetails_CorporationSharesIssuedToTheBearer", personsRegistry.CorporationSharesIssuedToTheBearer);
                        UserRegistry.SetValue("CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus", personsRegistry.IstheEntitylocatedandoperatesanofficeinCyprus);
                        //Contact Details
                        UserRegistry.SetValue("CompanyDetails_PreferredMailingAddress", personsRegistry.ContactDetailsLegal_PreferredMailingAddress);
                        UserRegistry.SetValue("CompanyDetails_EmailAddressForSendingAlerts", personsRegistry.ContactDetailsLegal_EmailAddressForSendingAlerts);
                        UserRegistry.SetValue("CompanyDetails_PreferredCommunicationLanguage", personsRegistry.ContactDetailsLegal_PreferredCommunicationLanguage);
                        UserRegistry.CreateNewVersion();
                        UserRegistry.Update();
                        //if(UserRegistry.IsCheckedOut)
                        //{
                        //	manager.CheckIn(UserRegistry, null, null);
                        //}
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
                        personsRegistryAdd.SetValue("CompanyDetails_TradingName", personsRegistry.TradingName);
                        personsRegistryAdd.SetValue("CompanyDetails_EntityType", personsRegistry.EntityType);
                        personsRegistryAdd.SetValue("CompanyDetails_CountryofIncorporation", personsRegistry.CountryofIncorporation);
                        personsRegistryAdd.SetValue("CompanyDetails_RegistrationNumber", personsRegistry.RegistrationNumber);
                        personsRegistryAdd.SetValue("CompanyDetails_DateofIncorporation", personsRegistry.DateofIncorporation);
                        personsRegistryAdd.SetValue("CompanyDetails_ListingStatus", personsRegistry.ListingStatus);
                        personsRegistryAdd.SetValue("CompanyDetails_CorporationSharesIssuedToTheBearer", personsRegistry.CorporationSharesIssuedToTheBearer);
                        personsRegistryAdd.SetValue("CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus", personsRegistry.IstheEntitylocatedandoperatesanofficeinCyprus);
                        //Contact Details
                        if (!string.IsNullOrEmpty(personsRegistry.ContactDetailsLegal_PreferredMailingAddress))
                            personsRegistryAdd.SetValue("CompanyDetails_PreferredMailingAddress", personsRegistry.ContactDetailsLegal_PreferredMailingAddress);
                        if (!string.IsNullOrEmpty(personsRegistry.ContactDetailsLegal_EmailAddressForSendingAlerts))
                            personsRegistryAdd.SetValue("CompanyDetails_EmailAddressForSendingAlerts", personsRegistry.ContactDetailsLegal_EmailAddressForSendingAlerts);
                        if (!string.IsNullOrEmpty(personsRegistry.ContactDetailsLegal_PreferredCommunicationLanguage))
                            personsRegistryAdd.SetValue("CompanyDetails_PreferredCommunicationLanguage", personsRegistry.ContactDetailsLegal_PreferredCommunicationLanguage);

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
                        personsRegistryUser.DocumentName = userModel.IntroducerUser.Introducer.DocumentName;
                        personsRegistryUser.DocumentCulture = "en-US";
                        personsRegistryUser.SetValue("UserID", user.UserID);
                        personsRegistryUser.Insert(personsRegistryFolder);
                        CMS.DocumentEngine.TreeNode Personsfoldernode = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        Personsfoldernode.DocumentName = "Persons Registry";
                        Personsfoldernode.DocumentCulture = "en-US";
                        Personsfoldernode.Insert(personsRegistryUser);
                        CMS.DocumentEngine.TreeNode personsRegistryAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.CompanyRegistry", tree);
                        personsRegistryAdd.SetValue("CompanyDetails_PersonType", ValidationHelper.GetString(personsRegistry.ApplicationType, ""));
                        personsRegistryAdd.SetValue("CompanyDetails_RegisteredName", ValidationHelper.GetString(personsRegistry.RegisteredName, ""));
                        personsRegistryAdd.SetValue("CompanyDetails_TradingName", ValidationHelper.GetString(personsRegistry.TradingName, ""));
                        personsRegistryAdd.SetValue("CompanyDetails_EntityType", ValidationHelper.GetString(personsRegistry.EntityType, ""));
                        personsRegistryAdd.SetValue("CompanyDetails_CountryofIncorporation", ValidationHelper.GetString(personsRegistry.CountryofIncorporation, ""));
                        personsRegistryAdd.SetValue("CompanyDetails_RegistrationNumber", ValidationHelper.GetString(personsRegistry.RegistrationNumber, ""));
                        personsRegistryAdd.SetValue("CompanyDetails_DateofIncorporation", ValidationHelper.GetDateTime(personsRegistry.DateofIncorporation, default(DateTime)));
                        personsRegistryAdd.SetValue("CompanyDetails_ListingStatus", ValidationHelper.GetString(personsRegistry.ListingStatus, ""));
                        personsRegistryAdd.SetValue("CompanyDetails_CorporationSharesIssuedToTheBearer", ValidationHelper.GetBoolean(personsRegistry.CorporationSharesIssuedToTheBearer, false));
                        personsRegistryAdd.SetValue("CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus", ValidationHelper.GetBoolean(personsRegistry.IstheEntitylocatedandoperatesanofficeinCyprus, false));
                        //Contact Details
                        if (!string.IsNullOrEmpty(personsRegistry.ContactDetailsLegal_PreferredMailingAddress))
                            personsRegistryAdd.SetValue("CompanyDetails_PreferredMailingAddress", personsRegistry.ContactDetailsLegal_PreferredMailingAddress);
                        if (!string.IsNullOrEmpty(personsRegistry.ContactDetailsLegal_EmailAddressForSendingAlerts))
                            personsRegistryAdd.SetValue("CompanyDetails_EmailAddressForSendingAlerts", personsRegistry.ContactDetailsLegal_EmailAddressForSendingAlerts);
                        if (!string.IsNullOrEmpty(personsRegistry.ContactDetailsLegal_PreferredCommunicationLanguage))
                            personsRegistryAdd.SetValue("CompanyDetails_PreferredCommunicationLanguage", personsRegistry.ContactDetailsLegal_PreferredCommunicationLanguage);

                        personsRegistryAdd.Insert(Personsfoldernode);

                        personsRegistry.CreatedDate = Convert.ToDateTime(personsRegistryAdd.DocumentCreatedWhen).ToString("MM/dd/yyyyy HH:mm:ss");
                        personsRegistry.ModyfiedDate = Convert.ToDateTime(personsRegistryAdd.DocumentModifiedWhen).ToString("MM/dd/yyyyy HH:mm:ss");

                        retVal = CompanyRegistryProvider.GetCompanyRegistry(personsRegistryAdd.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                    }
                }


            }

            return retVal;
        }
        public static Eurobank.Models.Registries.PersonsRegistry BindCompanyPersonRegistryModel(CompanyDetailsModel item, ContactDetailsLegalModel contactDetails)
        {
            Eurobank.Models.Registries.PersonsRegistry retVal = null;

            if (item != null)
            {
                retVal = new Eurobank.Models.Registries.PersonsRegistry()
                {
                    Id = item.RegistryId,
                    ApplicationType = item.Application_Type,
                    RegisteredName = item.RegisteredName,
                    TradingName = item.TradingName,
                    EntityType = item.EntityType,
                    CountryofIncorporation = item.CountryofIncorporation,
                    RegistrationNumber = item.RegistrationNumber,
                    DateofIncorporation = item.DateofIncorporation,
                    ListingStatus = item.ListingStatus,
                    CorporationSharesIssuedToTheBearer = item.CorporationSharesIssuedToTheBearer,
                    IstheEntitylocatedandoperatesanofficeinCyprus = item.IstheEntitylocatedandoperatesanofficeinCyprus,
                    SharesIssuedToTheBearerName = item.SharesIssuedToTheBearerName,
                    IsOfficeinCyprusName = item.IsOfficeinCyprusName,
                    ContactDetailsLegal_PreferredMailingAddress = contactDetails == null ? "" : contactDetails.ContactDetailsLegal_PreferredMailingAddress,
                    ContactDetailsLegal_EmailAddressForSendingAlerts = contactDetails == null ? "" : contactDetails.ContactDetailsLegal_EmailAddressForSendingAlerts,
                    ContactDetailsLegal_PreferredCommunicationLanguage = contactDetails == null ? null : contactDetails.ContactDetailsLegal_PreferredCommunicationLanguage,


                };
            }

            return retVal;
        }
        #endregion

    }
}
