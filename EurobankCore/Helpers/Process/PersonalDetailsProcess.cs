using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Application.RelatedParty.ContactDetails;
using Eurobank.Models.IdentificationDetails;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class PersonalDetailsProcess
    {
        private static readonly string _ApplicantsFolderName = "Applicants";

        private static readonly string _RealtedPartiesFolderName = "Related Parties";

        public static PersonalDetailsModel GetPersonalDetailsModelById(int personalDetailsId)
        {
            PersonalDetailsModel retVal = null;
            if (personalDetailsId > 0)
            {
                retVal = BindPersonalDetailsModel(GetPersonalDetailsById(personalDetailsId));
            }

            return retVal;
        }

        public static List<PersonalDetailsModel> GetApplicantPersonalDetails(string applicationNumber)
        {
            List<PersonalDetailsModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (!string.IsNullOrEmpty(applicationNumber))
            {
                TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

                if (applicationDetailsNode != null)
                {
                    TreeNode personalDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        personalDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (personalDetailsRoot != null)
                        {
                            List<TreeNode> personalDetailsNodes = personalDetailsRoot.Children.Where(u => u.ClassName == PersonalDetails.CLASS_NAME).ToList();

                            if (personalDetailsNodes != null && personalDetailsNodes.Count > 0)
                            {
                                retVal = new List<PersonalDetailsModel>();
                                personalDetailsNodes.ForEach(t =>
                                {
                                    PersonalDetails personalDetails = PersonalDetailsProvider.GetPersonalDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (personalDetails != null)
                                    {
                                        PersonalDetailsModel personalDetailsModel = BindPersonalDetailsModel(personalDetails);
                                        if (personalDetailsModel != null)
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

        public static List<PersonalDetailsModel> GetRelatedPartyPersonalDetails(string applicationNumber)
        {
            List<PersonalDetailsModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (!string.IsNullOrEmpty(applicationNumber))
            {
                TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

                if (applicationDetailsNode != null)
                {
                    TreeNode personalDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        personalDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase));

                        if (personalDetailsRoot != null)
                        {
                            List<TreeNode> personalDetailsNodes = personalDetailsRoot.Children.Where(u => u.ClassName == PersonalDetails.CLASS_NAME).ToList();

                            if (personalDetailsNodes != null && personalDetailsNodes.Count > 0)
                            {
                                retVal = new List<PersonalDetailsModel>();
                                personalDetailsNodes.ForEach(t =>
                                {
                                    PersonalDetails personalDetails = PersonalDetailsProvider.GetPersonalDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (personalDetails != null)
                                    {
                                        PersonalDetailsModel personalDetailsModel = BindPersonalDetailsModel(personalDetails);
                                        if (personalDetailsModel != null)
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

        public static List<SelectListItem> GetEBankingSubscribers(string applicationNumber)
        {
            List<SelectListItem> retVal = new List<SelectListItem>();

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (!string.IsNullOrEmpty(applicationNumber))
            {
                TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

                if (applicationDetailsNode != null)
                {
                    TreeNode personalDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        personalDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (personalDetailsRoot != null)
                        {
                            List<TreeNode> personalDetailsNodes = personalDetailsRoot.Children.Where(u => u.ClassName == PersonalDetails.CLASS_NAME).ToList();

                            if (personalDetailsNodes != null && personalDetailsNodes.Count > 0)
                            {
                                //retVal = new List<SelectListItem>();
                                personalDetailsNodes.ForEach(t =>
                                {
                                    PersonalDetails personalDetails = PersonalDetailsProvider.GetPersonalDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (personalDetails != null)
                                    {
                                        PersonalDetailsModel personalDetailsModel = BindPersonalDetailsModel(personalDetails);
                                        if (personalDetailsModel != null)
                                        {
                                            retVal.Add(new SelectListItem() { Value = Convert.ToString(personalDetails.NodeGUID), Text = personalDetails.PersonalDetails_FirstName.ToUpper() + " " + personalDetails.PersonalDetails_LastName.ToUpper() });
                                        }
                                    }
                                });
                            }
                        }
                    }
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        //Need to filter related party for 'e subscriber' role
                        personalDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase));

                        if (personalDetailsRoot != null)
                        {
                            List<TreeNode> personalDetailsNodes = personalDetailsRoot.Children.Where(u => u.ClassName == PersonalDetails.CLASS_NAME).ToList();

                            if (personalDetailsNodes != null && personalDetailsNodes.Count > 0)
                            {
                                //retVal = new List<SelectListItem>();
                                personalDetailsNodes.ForEach(t =>
                                {
                                    PersonalDetails personalDetails = PersonalDetailsProvider.GetPersonalDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (personalDetails != null)
                                    {
                                        PersonalDetailsModel personalDetailsModel = BindPersonalDetailsModel(personalDetails);
                                        if (personalDetailsModel != null)
                                        {
                                            retVal.Add(new SelectListItem() { Value = Convert.ToString(personalDetails.NodeGUID), Text = personalDetails.PersonalDetails_FirstName.ToUpper() + " " + personalDetails.PersonalDetails_LastName.ToUpper() });
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

        public static List<SelectListItem> GetEBankingSubscribersRoleConcat(string applicationNumber)
        {
            List<SelectListItem> retVal = new List<SelectListItem>();

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if(!string.IsNullOrEmpty(applicationNumber))
            {
                TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

                if(applicationDetailsNode != null)
                {
                    TreeNode personalDetailsRoot = null;
                    if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        personalDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));

                        if(personalDetailsRoot != null)
                        {
                            List<TreeNode> personalDetailsNodes = personalDetailsRoot.Children.Where(u => u.ClassName == PersonalDetails.CLASS_NAME).ToList();

                            if(personalDetailsNodes != null && personalDetailsNodes.Count > 0)
                            {
                                //retVal = new List<SelectListItem>();
                                personalDetailsNodes.ForEach(t =>
                                {
                                    PersonalDetails personalDetails = PersonalDetailsProvider.GetPersonalDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if(personalDetails != null)
                                    {
                                        PersonalDetailsModel personalDetailsModel = BindPersonalDetailsModel(personalDetails);
                                        if(personalDetailsModel != null)
                                        {
                                            retVal.Add(new SelectListItem() { Value = Convert.ToString(personalDetails.NodeGUID), Text = personalDetails.PersonalDetails_FirstName.ToUpper() + " " + personalDetails.PersonalDetails_LastName.ToUpper() });
                                        }
                                    }
                                });
                            }
                        }
                    }
                    if(applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        //Need to filter related party for 'e subscriber' role
                        personalDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase));

                        if(personalDetailsRoot != null)
                        {
                            List<TreeNode> personalDetailsNodes = personalDetailsRoot.Children.Where(u => u.ClassName == PersonalDetails.CLASS_NAME).ToList();

                            if(personalDetailsNodes != null && personalDetailsNodes.Count > 0)
                            {
                                //retVal = new List<SelectListItem>();
                                personalDetailsNodes.ForEach(t =>
                                {
                                    PersonalDetails personalDetails = PersonalDetailsProvider.GetPersonalDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if(personalDetails != null)
                                    {
                                        PersonalDetailsModel personalDetailsModel = BindPersonalDetailsModel(personalDetails);
                                        if(personalDetailsModel != null)
                                        {
                                            retVal.Add(new SelectListItem() { Value = Convert.ToString(personalDetails.NodeGUID), Text = personalDetails.PersonalDetails_FirstName.ToUpper() + " " + personalDetails.PersonalDetails_LastName.ToUpper() });
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

        public static PersonalDetailsModel SaveApplicantPersonalDetailsModel(string applicationNumber, PersonalDetailsModel model)
        {
            PersonalDetailsModel retVal = null;

            if (model != null && model.Id > 0)
            {
                PersonalDetails sourceOfOutgoingTransactions = GetPersonalDetailsById(model.Id);
                if (sourceOfOutgoingTransactions != null)
                {
                    PersonalDetails updatedPersonalDetails = BindPersonalDetails(sourceOfOutgoingTransactions, model);
                    if (updatedPersonalDetails != null)
                    {
                        updatedPersonalDetails.Update();
                        model = BindPersonalDetailsModel(updatedPersonalDetails);
                        retVal = model;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(applicationNumber) && model != null)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

                if (applicationDetailsNode != null)
                {
                    TreeNode personalDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        personalDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        personalDetailsRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        personalDetailsRoot.DocumentName = "Applicants";
                        personalDetailsRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        personalDetailsRoot.Insert(applicationDetailsNode);
                    }
                    PersonalDetails personalDetails = BindPersonalDetails(null, model);
                    if (personalDetails != null && personalDetailsRoot != null)
                    {
                        personalDetails.DocumentName = model.FirstName + " " + model.LastName;
                        personalDetails.NodeName = model.FirstName + " " + model.LastName;
                        personalDetails.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        personalDetails.Insert(personalDetailsRoot);
                        model = BindPersonalDetailsModel(personalDetails);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        public static PersonalDetailsModel SaveRelatedPartyPersonalDetailsModel(string applicationNumber, PersonalDetailsModel model)
        {
            PersonalDetailsModel retVal = null;
            //model.Type=applica
            if (model != null && model.Id > 0)
            {
                PersonalDetails sourceOfOutgoingTransactions = GetPersonalDetailsById(model.Id);
                if (sourceOfOutgoingTransactions != null)
                {
                    PersonalDetails updatedPersonalDetails = BindPersonalDetails(sourceOfOutgoingTransactions, model);
                    if (updatedPersonalDetails != null)
                    {
                        updatedPersonalDetails.Update();
                        model = BindPersonalDetailsModel(updatedPersonalDetails);
                        retVal = model;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(applicationNumber) && model != null)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = ApplicationsProcess.GetRootApplicationTreeNode(applicationNumber);

                if (applicationDetailsNode != null)
                {
                    TreeNode personalDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        personalDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _RealtedPartiesFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        personalDetailsRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        personalDetailsRoot.DocumentName = _RealtedPartiesFolderName;
                        personalDetailsRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        personalDetailsRoot.Insert(applicationDetailsNode);
                    }
                    PersonalDetails personalDetails = BindPersonalDetails(null, model);
                    if (personalDetails != null && personalDetailsRoot != null)
                    {
                        personalDetails.DocumentName = model.FirstName + " " + model.LastName;
                        personalDetails.NodeName = model.FirstName + " " + model.LastName;
                        personalDetails.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        personalDetails.Insert(personalDetailsRoot);
                        model = BindPersonalDetailsModel(personalDetails);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        public static PersonalDetails GetPersonalDetailsById(int personalDetailsId)
        {
            PersonalDetails retVal = null;

            if (personalDetailsId > 0)
            {
                var personalDetails = PersonalDetailsProvider.GetPersonalDetails();
                if (personalDetails != null && personalDetails.Count > 0)
                {
                    retVal = personalDetails.FirstOrDefault(o => o.PersonalDetailsID == personalDetailsId);
                }
            }

            return retVal;
        }

        #region Bind Data

        private static PersonalDetailsModel BindPersonalDetailsModel(PersonalDetails item)
        {
            PersonalDetailsModel retVal = null;

            if (item != null)
            {
                retVal = new PersonalDetailsModel()
                {
                    Id = item.PersonalDetailsID,
                    Title = item.PersonalDetails_Title != null ? item.PersonalDetails_Title.ToString() : string.Empty,
                    FirstName = item.PersonalDetails_FirstName,
                    LastName = item.PersonalDetails_LastName,
                    FathersName = item.PersonalDetails_FathersName,
                    Gender = item.PersonalDetails_Gender != null ? item.PersonalDetails_Gender.ToString() : string.Empty,
                    PlaceOfBirth = item.PersonalDetails_PlaceOfBirth,
                    DateOfBirth = item.PersonalDetails_DateOfBirth,
                    NameOfBankingInstitution = item.PersonalDetails_NameOfBankingInstitution,
                    CountryOfBankingInstitution = item.PersonalDetails_CountryOfBankingInstitution.ToString(),
                    CountryOfBirth = item.PersonalDetails_CountryOfBirth,
                    EducationLevel = item.PersonalDetails_EducationLevel != null ? item.PersonalDetails_EducationLevel.ToString() : string.Empty,
                    //HasAccountInOtherBank = item.PersonalDetails_HasAccountInOtherBank,
                    HasAccountInOtherBankName = item.PersonalDetails_HasAccountInOtherBank,
                    
                    //IsLiableToPayDefenseTaxInCyprus = item.PersonalDetails_IsLiableToPayDefenseTaxInCyprus,
                    IsLiableToPayDefenseTaxInCyprusName = item.PersonalDetails_IsLiableToPayDefenseTaxInCyprus,
                    IsPepName = item.PersonalDetails_IsPep,
                    IsRelatedToPepName = item.PersonalDetails_IsRelatedToPep,
                    NodeGUID = ValidationHelper.GetString(item.NodeGUID, ""),
                    NodePath = item.NodeAliasPath,
                    Type = item.PersonalDetails_Type,
                    IdVerified = item.PersonalDetails_IdVerified,
                    Invited=item.PersonalDetails_Invited,
                    IsRelatedPartyUBO=item.PersonalDetails_IsRelatedPartyUBO,
                    PersonRegistryId = item.PersonRegistryId,
                    Status = item.PersonalDetails_Status,
                    CreatedDateTime = item.DocumentCreatedWhen,
                    HIDInviteFlag = item.PersonalDetails_HIDInviteFlag,
                    PersonalDetails_CustomerCIF = item.PersonalDetails_CustomerCIF
                };
            }

            return retVal;
        }

        private static PersonalDetails BindPersonalDetails(PersonalDetails existPersonalDetails, PersonalDetailsModel item)
        {
            PersonalDetails retVal = new PersonalDetails();
            if (existPersonalDetails != null)
            {
                retVal = existPersonalDetails;
            }
            if (item != null)
            {
                if (item.Title != null)
                {
                    retVal.PersonalDetails_Title = new Guid(item.Title);
                }
                if(item.FirstName!=null)
                retVal.PersonalDetails_FirstName = item.FirstName;
                if (item.LastName!=null)
                retVal.PersonalDetails_LastName = item.LastName;

                retVal.PersonalDetails_FathersName = item.FathersName;
                if (item.Gender != null)
                {
                    retVal.PersonalDetails_Gender = new Guid(item.Gender);
                }

                retVal.PersonalDetails_PlaceOfBirth = item.PlaceOfBirth;
                retVal.PersonalDetails_DateOfBirth =Convert.ToDateTime( item.DateOfBirth);
                retVal.PersonalDetails_NameOfBankingInstitution = item.NameOfBankingInstitution;
                retVal.PersonalDetails_CountryOfBankingInstitution = ValidationHelper.GetInteger(item.CountryOfBankingInstitution, 0);
                retVal.PersonalDetails_CountryOfBirth = item.CountryOfBirth;
                if (item.EducationLevel != null)
                {
                    retVal.PersonalDetails_EducationLevel = new Guid(item.EducationLevel);
                }
                retVal.PersonalDetails_HasAccountInOtherBank = item.HasAccountInOtherBankName;
                retVal.PersonalDetails_IsLiableToPayDefenseTaxInCyprus = item.IsLiableToPayDefenseTaxInCyprusName;
                retVal.PersonalDetails_IsPep = item.IsPepName;
                retVal.PersonalDetails_IsRelatedToPep = item.IsRelatedToPepName;
                retVal.PersonalDetails_Type = item.Type;
                retVal.PersonalDetails_IdVerified = item.IdVerified;
                retVal.PersonalDetails_Invited = item.Invited;
                retVal.PersonRegistryId = item.PersonRegistryId;
                retVal.PersonalDetails_IsRelatedPartyUBO = item.IsRelatedPartyUBO;
                retVal.PersonalDetails_Status = item.Status;
                retVal.PersonalDetails_HIDInviteFlag = item.HIDInviteFlag;
            }

            return retVal;
        }

        public static PersonsRegistrySearchModel BindPersonRegistrySearchModel(Eurobank.Models.Registries.PersonsRegistry item)
        {
            PersonsRegistrySearchModel retVal = null;

            if (item != null)
            {
                retVal = new PersonsRegistrySearchModel()
                {
                    PersonRegistryId = !string.IsNullOrEmpty(item.NodeID) ? Convert.ToInt32(item.NodeID) : 0,
                    ApplicationTypeName=item.ApplicationTypeName,
                    Title = item.Title,
                    TitleName = item.TitleName,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    FullName = (!string.IsNullOrEmpty(item.FirstName) ? item.FirstName + " " : string.Empty) + (!string.IsNullOrEmpty(item.LastName) ? item.LastName : string.Empty),
                    FatherName = item.FatherName,
                    Gender = item.Gender,
                    GenderName = item.GenderName,
                    PlaceofBirth = item.PlaceofBirth,
                    DateofBirth =Convert.ToDateTime( item.DateofBirth).ToString("dd/MM/yyyy"),
                    CountryofBirth = item.CountryofBirth,
                    CountryofBirthName = item.CountryofBirthName,
                    EducationLevel = item.EducationLevel,
                    EducationLevelName = item.EducationLevelName,
                    Citizenship = item.Citizenship,
                    CitizenshipName = item.CitizenshipName,
                    TypeofIdentification = item.TypeofIdentification,
                    TypeofIdentificationName = item.TypeofIdentificationName,
                    IdentificationNumber = item.IdentificationNumber,
                    NodeGUID = item.NodeGUID,
                    IssuingCountry = item.IssuingCountry,
                    IssuingCountryName = item.IssuingCountryName,
                    IssueDate = Convert.ToDateTime(item.IssueDate).ToString("dd/MM/yyyy"),
                    ExpiryDate = Convert.ToDateTime(item.ExpiryDate).ToString("dd/MM/yyyy"),
                    IssueDateTime = item.IssueDate,
                    ExpiryDateTime = item.ExpiryDate,
                    MobileTelNoCountryCode = item.MobileTelNoCountryCode,
                    MobileTelNoNumber = item.MobileTelNoNumber,
                    HomeTelNoCountryCode = item.HomeTelNoCountryCode,
                    HomeTelNoNumber = item.HomeTelNoNumber,
                    WorkTelNoCountryCode = item.WorkTelNoCountryCode,
                    WorkTelNoNumber = item.WorkTelNoNumber,
                    FaxNoCountryCode = item.FaxNoCountryCode,
                    EmailAddress = item.EmailAddress,
                    FaxNoFaxNumber = item.FaxNoFaxNumber,
                    PreferredCommunicationLanguage = item.PreferredCommunicationLanguage,
                    PreferredCommunicationLanguageName = item.PreferredCommunicationLanguageName,
                    ConsentforMarketingPurposes = item.ConsentforMarketingPurposes,
                    CreatedDate = item.CreatedDate,
                    ModyfiedDate = item.ModyfiedDate,
                    NodeAliaspath = item.NodeAliaspath,



                };
            }

            return retVal;
        }

        public static Eurobank.Models.Registries.PersonsRegistry BindPersonRegistryModel(PersonalDetailsModel item, ContactDetailsViewModel contactDetails, IdentificationDetailsViewModel identificationDetails)
        {
            Eurobank.Models.Registries.PersonsRegistry retVal = null;

            if (item != null)
            {
                //IdentificationDetailsViewModel identificationDetails = null;
                //if (item.Id > 0)
                //{
                //  var identificationDetailsList=  IdentificationDetailsProcess.GetIdentificationDetails(item.Id);
                //    if (identificationDetailsList != null && identificationDetailsList.Count > 0)
                //    {
                //        identificationDetails= identificationDetailsList.FirstOrDefault();
                //    }
                //}
               
                retVal = new Eurobank.Models.Registries.PersonsRegistry()
                {
                    ApplicationType=item.Type,
                    Id = item.PersonRegistryId,
                    Title = item.Title,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    FatherName = item.FathersName,
                    Gender = item.Gender,
                    PlaceofBirth = item.PlaceOfBirth,
                    DateofBirth =Convert.ToDateTime( item.DateOfBirth),
                    CountryofBirth = item.CountryOfBirth.ToString(),
                    EducationLevel = item.EducationLevel,

                    

                };
                if (contactDetails != null)
                {
                    retVal.MobileTelNoCountryCode = contactDetails.Country_Code_MobileTelNoNumber;
                    retVal.MobileTelNoNumber = contactDetails.ContactDetails_MobileTelNoNumber;
                    retVal.HomeTelNoCountryCode = contactDetails.Country_Code_HomeTelNoNumber;
                    retVal.HomeTelNoNumber = contactDetails.ContactDetails_HomeTelNoNumber;
                    retVal.WorkTelNoCountryCode = contactDetails.Country_Code_WorkTelNoNumber;
                    retVal.WorkTelNoNumber = contactDetails.ContactDetails_WorkTelNoNumber;
                    retVal.FaxNoCountryCode = contactDetails.Country_Code_FaxNoFaxNumber;
                    retVal.EmailAddress = contactDetails.ContactDetails_EmailAddress;
                    retVal.FaxNoFaxNumber = contactDetails.ContactDetails_FaxNoFaxNumber;
                    retVal.PreferredCommunicationLanguage = contactDetails.ContactDetails_PreferredCommunicationLanguage;
                }
                if (identificationDetails != null)
                {
                    retVal.IdentificationNumber = identificationDetails.IdentificationDetails_IdentificationNumber;
                    retVal.Citizenship = identificationDetails.IdentificationDetails_Citizenship==null?"": identificationDetails.IdentificationDetails_Citizenship.ToString();
                    retVal.TypeofIdentification = identificationDetails.IdentificationDetails_TypeOfIdentification;
                    retVal.IssuingCountry = identificationDetails.IdentificationDetails_CountryOfIssue == null ? "" : identificationDetails.IdentificationDetails_CountryOfIssue.ToString();
                    retVal.IssueDate = identificationDetails.IdentificationDetails_IssueDate;
                    retVal.ExpiryDate = identificationDetails.IdentificationDetails_ExpiryDate;
                }
                
            }

            return retVal;
        }

        #endregion

        #region Person Registry Methods

        public static PersonsRegistry SavePersonRegistry(string userName,string IntroducerCompany, RegistriesRepository registriesRepository, PersonalDetailsModel perssonalDetailsModel, ContactDetailsViewModel contactDetails, IdentificationDetailsViewModel identificationDetails)
        {
            PersonsRegistry retVal = null;

            if (perssonalDetailsModel != null && registriesRepository != null)
            {
                Eurobank.Models.Registries.PersonsRegistry personsRegistry = BindPersonRegistryModel(perssonalDetailsModel, contactDetails, identificationDetails);
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

                if (personsRegistry.Id > 0)
                {
                    var UserRegistry = registriesRepository.GetRegistryUserById(personsRegistry.Id);
                    if (UserRegistry != null)
                    {
                        var manager = VersionManager.GetInstance(tree);
                        //if(!UserRegistry.IsCheckedOut)
                        //{
                        //	manager.CheckOut(UserRegistry);
                        //}
                        if(personsRegistry.FirstName != null)
						{
                            UserRegistry.DocumentName = (personsRegistry.FirstName + " " + (personsRegistry.LastName != null ? personsRegistry.LastName : string.Empty));
                        }
						else
						{
                            UserRegistry.DocumentName = "Person Registry";
                        }
                        if(personsRegistry.FirstName != null)
                        {
                            UserRegistry.SetValue("Name", (personsRegistry.FirstName + " " + (personsRegistry.LastName != null ? personsRegistry.LastName : string.Empty)));
                        }
                        else
                        {
                            UserRegistry.DocumentName = "Person Registry";
                        }
                        string entityTypeCode = ValidationHelper.GetString(ServiceHelper.GetEntityType(personsRegistry.ApplicationType, Constants.APPLICATION_TYPE), "");
                        
                        UserRegistry.SetValue("ApplicationType", personsRegistry.ApplicationType);
                        if(personsRegistry.FirstName != null && personsRegistry.FirstName.Length <= 50)
						{
                            UserRegistry.SetValue("FirstName", personsRegistry.FirstName);
                        }
                        if(personsRegistry.LastName != null && personsRegistry.LastName.Length <= 50)
                        {
                            UserRegistry.SetValue("LastName", personsRegistry.LastName);
                        }
                        if(personsRegistry.FatherName != null && personsRegistry.FatherName.Length <= 50)
                        {
                            UserRegistry.SetValue("FathersName", personsRegistry.FatherName);
                        }

                        
                        UserRegistry.SetValue("Title", personsRegistry.Title);
                        UserRegistry.SetValue("Gender", personsRegistry.Gender);
                        UserRegistry.SetValue("DateofBirth", personsRegistry.DateofBirth);
                        UserRegistry.SetValue("PlaceofBirth", personsRegistry.PlaceofBirth);
                        UserRegistry.SetValue("CountryofBirth", personsRegistry.CountryofBirth);
                        UserRegistry.SetValue("EducationLevel", personsRegistry.EducationLevel);
                        //Identification details
                        UserRegistry.SetValue("Citizenship", personsRegistry.Citizenship);
                        UserRegistry.SetValue("TypeofIdentification", personsRegistry.TypeofIdentification);
                        UserRegistry.SetValue("IdentificationNumber", personsRegistry.IdentificationNumber);
                        UserRegistry.SetValue("IssuingCountry", personsRegistry.IssuingCountry);
                        UserRegistry.SetValue("IssueDate", personsRegistry.IssueDate);
                        UserRegistry.SetValue("ExpiryDate", personsRegistry.ExpiryDate);
                        UserRegistry.DocumentCulture = "en-US";
                        //contact details
                        UserRegistry.SetValue("WorkTelNoNumber", personsRegistry.WorkTelNoNumber);
                        UserRegistry.SetValue("MobileTelNoNumber", personsRegistry.MobileTelNoNumber);
                        if(personsRegistry.MobileTelNoNumber != null)
						{
                            UserRegistry.SetValue("MobileTelNoCountryCode", personsRegistry.MobileTelNoNumber.Split(' ')[0]);
                        }
                        UserRegistry.SetValue("HomeTelNoNumber", personsRegistry.HomeTelNoNumber);
                        UserRegistry.SetValue("MobileTelNoCountryCode", personsRegistry.MobileTelNoCountryCode);
                        UserRegistry.SetValue("HomeTelNoCountryCode", personsRegistry.HomeTelNoCountryCode);
                        UserRegistry.SetValue("WorkTelNoCountryCode", personsRegistry.WorkTelNoCountryCode);
                        UserRegistry.SetValue("FaxNoCountryCode", personsRegistry.FaxNoCountryCode);

                        UserRegistry.SetValue("FaxNoFaxNumber", personsRegistry.FaxNoFaxNumber);
                        UserRegistry.SetValue("EmailAddress", personsRegistry.EmailAddress);
                        UserRegistry.SetValue("ConsentforMarketingPurposes", personsRegistry.ConsentforMarketingPurposes);
                        UserRegistry.SetValue("PreferredCommunicationLanguage", personsRegistry.PreferredCommunicationLanguage);
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
                    var UserRegistry = registriesRepository.GetRegistryUserByName(IntroducerCompany);
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
                        CMS.DocumentEngine.TreeNode personsRegistryAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.PersonsRegistry", tree);
                        personsRegistryAdd.DocumentName = personsRegistry.FirstName + " " + personsRegistry.LastName;
                        personsRegistryAdd.SetValue("ApplicationType", personsRegistry.ApplicationType);
                        personsRegistryAdd.SetValue("Name", personsRegistry.FirstName + " " + personsRegistry.LastName);
                        if(personsRegistry.FirstName != null && personsRegistry.FirstName.Length <= 50)
                        {
                            personsRegistryAdd.SetValue("FirstName", personsRegistry.FirstName);
                        }
                        if(personsRegistry.LastName != null && personsRegistry.LastName.Length <= 50)
                        {
                            personsRegistryAdd.SetValue("LastName", personsRegistry.LastName);
                        }
                        if(personsRegistry.FatherName != null && personsRegistry.FatherName.Length <= 50)
                        {
                            personsRegistryAdd.SetValue("FathersName", personsRegistry.FatherName);
                        }
                        //personsRegistryAdd.SetValue("FirstName", personsRegistry.FirstName);
                        //personsRegistryAdd.SetValue("LastName", personsRegistry.LastName);
                        //personsRegistryAdd.SetValue("FathersName", personsRegistry.FatherName);
                        personsRegistryAdd.SetValue("Title", personsRegistry.Title);
                        personsRegistryAdd.SetValue("Gender", personsRegistry.Gender);
                        personsRegistryAdd.SetValue("DateofBirth", personsRegistry.DateofBirth);
                        personsRegistryAdd.SetValue("PlaceofBirth", personsRegistry.PlaceofBirth);
                        personsRegistryAdd.SetValue("CountryofBirth", personsRegistry.CountryofBirth);
                        personsRegistryAdd.SetValue("EducationLevel", personsRegistry.EducationLevel);
                        //Identification details
                        personsRegistryAdd.SetValue("Citizenship", personsRegistry.Citizenship);
                        personsRegistryAdd.SetValue("TypeofIdentification", personsRegistry.TypeofIdentification);
                        personsRegistryAdd.SetValue("IdentificationNumber", personsRegistry.IdentificationNumber);
                        personsRegistryAdd.SetValue("IssuingCountry", personsRegistry.IssuingCountry);
                        personsRegistryAdd.SetValue("IssueDate", personsRegistry.IssueDate);
                        personsRegistryAdd.SetValue("ExpiryDate", personsRegistry.ExpiryDate);
                        personsRegistryAdd.DocumentCulture = "en-US";
                        //contact details
                        personsRegistryAdd.SetValue("WorkTelNoNumber", personsRegistry.WorkTelNoNumber);
                        personsRegistryAdd.SetValue("MobileTelNoNumber", personsRegistry.MobileTelNoNumber);
						if(!string.IsNullOrEmpty(personsRegistry.MobileTelNoNumber))
						{
                            personsRegistryAdd.SetValue("MobileTelNoCountryCode", personsRegistry.MobileTelNoNumber.Split(' ')[0]);
                        }
                        personsRegistryAdd.SetValue("HomeTelNoNumber", personsRegistry.HomeTelNoNumber);
                        personsRegistryAdd.SetValue("MobileTelNoCountryCode", personsRegistry.MobileTelNoCountryCode);
                        personsRegistryAdd.SetValue("HomeTelNoCountryCode", personsRegistry.HomeTelNoCountryCode);
                        personsRegistryAdd.SetValue("WorkTelNoCountryCode", personsRegistry.WorkTelNoCountryCode);
                        personsRegistryAdd.SetValue("FaxNoCountryCode", personsRegistry.FaxNoCountryCode);
                        personsRegistryAdd.SetValue("FaxNoFaxNumber", personsRegistry.FaxNoFaxNumber);
                        personsRegistryAdd.SetValue("EmailAddress", personsRegistry.EmailAddress);
                        personsRegistryAdd.SetValue("ConsentforMarketingPurposes", personsRegistry.ConsentforMarketingPurposes);
                        personsRegistryAdd.SetValue("PreferredCommunicationLanguage", personsRegistry.PreferredCommunicationLanguage);
                        personsRegistryAdd.Insert(personsfoldernode_parent);
                        personsRegistry.CreatedDate = Convert.ToDateTime(personsRegistryAdd.DocumentCreatedWhen).ToString("MM/dd/yyyyy HH:mm:ss");
                        personsRegistry.ModyfiedDate = Convert.ToDateTime(personsRegistryAdd.DocumentModifiedWhen).ToString("MM/dd/yyyyy HH:mm:ss");
                        retVal = PersonsRegistryProvider.GetPersonsRegistry(personsRegistryAdd.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
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
                        personsRegistryUser.DocumentName = IntroducerCompany;
                        personsRegistryUser.DocumentCulture = "en-US";
                        personsRegistryUser.SetValue("UserID", user.UserID);
                        personsRegistryUser.Insert(personsRegistryFolder);
                        CMS.DocumentEngine.TreeNode Personsfoldernode = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        Personsfoldernode.DocumentName = "Persons Registry";
                        Personsfoldernode.DocumentCulture = "en-US";
                        Personsfoldernode.Insert(personsRegistryUser);
                        CMS.DocumentEngine.TreeNode personsRegistryAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.PersonsRegistry", tree);
                        personsRegistryAdd.DocumentName = personsRegistry.FirstName + " " + personsRegistry.LastName;
                        personsRegistryAdd.SetValue("ApplicationType", personsRegistry.ApplicationType);
                        personsRegistryAdd.SetValue("Name", personsRegistry.FirstName + " " + personsRegistry.LastName);
                        if(personsRegistry.FirstName != null && personsRegistry.FirstName.Length <= 50)
                        {
                            personsRegistryAdd.SetValue("FirstName", personsRegistry.FirstName);
                        }
                        if(personsRegistry.LastName != null && personsRegistry.LastName.Length <= 50)
                        {
                            personsRegistryAdd.SetValue("LastName", personsRegistry.LastName);
                        }
                        if(personsRegistry.FatherName != null && personsRegistry.FatherName.Length <= 50)
                        {
                            personsRegistryAdd.SetValue("FathersName", personsRegistry.FatherName);
                        }
                        //personsRegistryAdd.SetValue("FirstName", personsRegistry.FirstName);
                        //personsRegistryAdd.SetValue("LastName", personsRegistry.LastName);
                        //personsRegistryAdd.SetValue("FathersName", personsRegistry.FatherName);
                        personsRegistryAdd.SetValue("Title", personsRegistry.Title);
                        personsRegistryAdd.SetValue("Gender", personsRegistry.Gender);
                        personsRegistryAdd.SetValue("DateofBirth", personsRegistry.DateofBirth);
                        personsRegistryAdd.SetValue("PlaceofBirth", personsRegistry.PlaceofBirth);
                        personsRegistryAdd.SetValue("CountryofBirth", personsRegistry.CountryofBirth);
                        personsRegistryAdd.SetValue("EducationLevel", personsRegistry.EducationLevel);
                        //Identification details
                        personsRegistryAdd.SetValue("Citizenship", personsRegistry.Citizenship);
                        personsRegistryAdd.SetValue("TypeofIdentification", personsRegistry.TypeofIdentification);
                        personsRegistryAdd.SetValue("IdentificationNumber", personsRegistry.IdentificationNumber);
                        personsRegistryAdd.SetValue("IssuingCountry", personsRegistry.IssuingCountry);
                        personsRegistryAdd.SetValue("IssueDate", personsRegistry.IssueDate);
                        personsRegistryAdd.SetValue("ExpiryDate", personsRegistry.ExpiryDate);
                        personsRegistryAdd.DocumentCulture = "en-US";
                        //contact details
                        personsRegistryAdd.SetValue("WorkTelNoNumber", personsRegistry.WorkTelNoNumber);
                        personsRegistryAdd.SetValue("MobileTelNoNumber", personsRegistry.MobileTelNoNumber);
						if(!string.IsNullOrEmpty(personsRegistry.MobileTelNoNumber))
						{
                            personsRegistryAdd.SetValue("MobileTelNoCountryCode", personsRegistry.MobileTelNoNumber.Split(' ')[0]);
                        }
                        
                        personsRegistryAdd.SetValue("HomeTelNoNumber", personsRegistry.HomeTelNoNumber);
                        personsRegistryAdd.SetValue("MobileTelNoCountryCode", personsRegistry.MobileTelNoCountryCode);
                        personsRegistryAdd.SetValue("HomeTelNoCountryCode", personsRegistry.HomeTelNoCountryCode);
                        personsRegistryAdd.SetValue("WorkTelNoCountryCode", personsRegistry.WorkTelNoCountryCode);
                        personsRegistryAdd.SetValue("FaxNoCountryCode", personsRegistry.FaxNoCountryCode);
                        personsRegistryAdd.SetValue("FaxNoFaxNumber", personsRegistry.FaxNoFaxNumber);
                        personsRegistryAdd.SetValue("EmailAddress", personsRegistry.EmailAddress);
                        personsRegistryAdd.SetValue("ConsentforMarketingPurposes", personsRegistry.ConsentforMarketingPurposes);
                        personsRegistryAdd.SetValue("PreferredCommunicationLanguage", personsRegistry.PreferredCommunicationLanguage);
                        personsRegistryAdd.Insert(Personsfoldernode);
                        personsRegistry.CreatedDate = Convert.ToDateTime(personsRegistryAdd.DocumentCreatedWhen).ToString("MM/dd/yyyyy HH:mm:ss");
                        personsRegistry.ModyfiedDate = Convert.ToDateTime(personsRegistryAdd.DocumentModifiedWhen).ToString("MM/dd/yyyyy HH:mm:ss");

                        retVal = PersonsRegistryProvider.GetPersonsRegistry(personsRegistryAdd.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                    }
                }


            }

            return retVal;
        }

		#endregion

		#region customhelper
        public static int GetPersonDetailsIdByGuid(string guid)
        {
			int retVal = 0;
			try
            {
				PersonalDetails personalDetails = PersonalDetailsProvider.GetPersonalDetails(new Guid(guid), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                CompanyDetails companyDetails = CompanyDetailsProvider.GetCompanyDetails(new Guid(guid), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                CompanyDetailsRelatedParty companyDetailsRelatedParty = CompanyDetailsRelatedPartyProvider.GetCompanyDetailsRelatedParty(new Guid(guid), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                if (personalDetails != null)
                {
                    retVal = personalDetails.PersonalDetailsID;                    
                }
                if(companyDetails != null)
                {
                    retVal = companyDetails.CompanyDetailsID;
                }
                if (companyDetailsRelatedParty != null)
                {
                    retVal = companyDetailsRelatedParty.CompanyDetailsRelatedPartyID;
                }
				return retVal;
			}
            catch
            {
				return retVal;
			}
        }
		public static string GetPersonDetailsNameByGuid(string guid)
		{
			string retVal = "";
			try
			{
				PersonalDetails personalDetails = PersonalDetailsProvider.GetPersonalDetails(new Guid(guid), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
				CompanyDetails companyDetails = CompanyDetailsProvider.GetCompanyDetails(new Guid(guid), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
				CompanyDetailsRelatedParty companyDetailsRelatedParty = CompanyDetailsRelatedPartyProvider.GetCompanyDetailsRelatedParty(new Guid(guid), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

				if (personalDetails != null)
				{
					retVal = personalDetails.DocumentName;
				}
				if (companyDetails != null)
				{
					retVal = companyDetails.DocumentName;
				}
				if (companyDetailsRelatedParty != null)
				{
					retVal = companyDetailsRelatedParty.DocumentName;
				}
				return retVal;
			}
			catch
			{
				return retVal;
			}
		}
		#endregion
	}
}
