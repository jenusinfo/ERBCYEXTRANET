using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Models;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Registries;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Helpers.Process
{
    public class AddressDetailsProcess
    {
        private static readonly string _ApplicationRootNodePath = "/Applications-(1)";

        private static readonly string _ApplicantsFolderName = "Address Details";
        private static readonly string _ApplicantsDocumentName = "Address";

        private static readonly string _RealtedPartiesFolderName = "Related Parties";

        public static AddressDetailsModel GetAddressDetailsModelById(int addressDetailsId)
        {
            AddressDetailsModel retVal = null;
            if (addressDetailsId > 0)
            {
                retVal = BindAddressDetailsModel(GetAddressDetailsById(addressDetailsId));
            }

            return retVal;
        }

        public static List<AddressDetailsModel> GetAllAddressDetails()
        {
            List<AddressDetailsModel> retVal = null;
            var addresses = AddressDetailsProvider.GetAddressDetails();
            if(addresses != null && addresses.Count > 0)
            {
                retVal = new List<AddressDetailsModel>();
                foreach(var address in addresses)
				{
                    if(address != null)
					{
                        retVal.Add(BindAddressDetailsModel(address));
                    }
				}
            }

            return retVal;
        }

        public static List<AddressDetailsModel> GetApplicantAddressDetails(int applicantId)
        {
            List<AddressDetailsModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (applicantId > 0)
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(PersonalDetails.CLASS_NAME)
                    .WhereEquals("PersonalDetailsID", applicantId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode addressDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        addressDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (addressDetailsRoot != null)
                        {
                            List<TreeNode> addressDetailsNodes = addressDetailsRoot.Children.Where(u => u.ClassName == AddressDetails.CLASS_NAME).ToList();

                            if (addressDetailsNodes != null && addressDetailsNodes.Count > 0)
                            {
                                retVal = new List<AddressDetailsModel>();
                                addressDetailsNodes.ForEach(t =>
                                {
                                    AddressDetails addressDetails = AddressDetailsProvider.GetAddressDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (addressDetails != null)
                                    {
                                        AddressDetailsModel personalDetailsModel = BindAddressDetailsModelPhysical(addressDetails);
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

        public static List<AddressDetailsModel> GetRelatedPartyAddressDetails(int applicantId)
        {
            List<AddressDetailsModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (applicantId > 0)
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(PersonalDetails.CLASS_NAME)
                    .WhereEquals("PersonalDetailsID", applicantId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode addressDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        addressDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (addressDetailsRoot != null)
                        {
                            List<TreeNode> addressDetailsNodes = addressDetailsRoot.Children.Where(u => u.ClassName == AddressDetails.CLASS_NAME).ToList();

                            if (addressDetailsNodes != null && addressDetailsNodes.Count > 0)
                            {
                                retVal = new List<AddressDetailsModel>();
                                addressDetailsNodes.ForEach(t =>
                                {
                                    AddressDetails addressDetails = AddressDetailsProvider.GetAddressDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (addressDetails != null)
                                    {
                                        AddressDetailsModel personalDetailsModel = BindAddressDetailsModel(addressDetails);
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

        public static AddressDetailsModel SaveApplicantAddressDetailsModel(int applicantId, AddressDetailsModel model)
        {
            AddressDetailsModel retVal = null;

            if (model != null && model.Id > 0)
            {
                AddressDetails addressDetails = GetAddressDetailsById(model.Id);
                if (addressDetails != null)
                {
                    AddressDetails updatedPersonalDetails = BindAddressDetails(addressDetails, model);
                    if (updatedPersonalDetails != null)
                    {
                        updatedPersonalDetails.Update();
                        model = BindAddressDetailsModelPhysical(updatedPersonalDetails);
                        retVal = model;
                    }
                }
            }
            else if (applicantId > 0)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(PersonalDetails.CLASS_NAME)
                    .WhereEquals("PersonalDetailsID", applicantId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode addressDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        addressDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        addressDetailsRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        addressDetailsRoot.DocumentName = "Address Details";
                        addressDetailsRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        addressDetailsRoot.Insert(applicationDetailsNode);
                    }
                    AddressDetails addressDetails = BindAddressDetails(null, model);
                    if (addressDetails != null && addressDetailsRoot != null)
                    {
                        //addressDetails.DocumentName = model.AddressLine1;
                        //addressDetails.NodeName = model.AddressLine1;
                        addressDetails.DocumentName = _ApplicantsDocumentName;
                        addressDetails.NodeName = _ApplicantsDocumentName;
                        addressDetails.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;

                        addressDetails.Insert(addressDetailsRoot);
                        model = BindAddressDetailsModelPhysical(addressDetails);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        public static AddressDetailsModel SaveRelatedPartyAddressDetailsModel(int applicantId, AddressDetailsModel model)
        {
            AddressDetailsModel retVal = null;

            if (model != null && model.Id > 0)
            {
                AddressDetails addressDetails = GetAddressDetailsById(model.Id);
                if (addressDetails != null)
                {
                    AddressDetails updatedAddressDetails = BindAddressDetails(addressDetails, model);
                    if (updatedAddressDetails != null)
                    {
                        updatedAddressDetails.Update();
                        model = BindAddressDetailsModel(updatedAddressDetails);
                        retVal = model;
                    }
                }
            }
            else if (applicantId > 0)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(PersonalDetails.CLASS_NAME)
                    .WhereEquals("PersonalDetailsID", applicantId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode addressDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        addressDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        addressDetailsRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        addressDetailsRoot.DocumentName = _ApplicantsFolderName;
                        addressDetailsRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        addressDetailsRoot.Insert(applicationDetailsNode);
                    }
                    AddressDetails addressDetails = BindAddressDetails(null, model);
                    if (addressDetails != null && addressDetailsRoot != null)
                    {
                        //addressDetails.DocumentName = model.AddressLine1;
                        //addressDetails.NodeName = model.AddressLine1;
                        addressDetails.DocumentName = _ApplicantsDocumentName;
                        addressDetails.NodeName = _ApplicantsDocumentName;
                        addressDetails.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        addressDetails.Insert(addressDetailsRoot);
                        model = BindAddressDetailsModel(addressDetails);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        public static AddressDetails GetAddressDetailsById(int addressDetailsId)
        {
            AddressDetails retVal = null;

            if (addressDetailsId > 0)
            {
                var addressDetails = AddressDetailsProvider.GetAddressDetails();
                if (addressDetails != null && addressDetails.Count > 0)
                {
                    retVal = addressDetails.FirstOrDefault(o => o.AddressDetailsID == addressDetailsId);
                }
            }

            return retVal;
        }

        public static List<SelectListItem> GetCardHolderAddresses(string relatedPartyGuid)
        {
            List<SelectListItem> retVal = null;

            List<AddressDetailsModel> addressDetails = new List<AddressDetailsModel>();

            var personalDetails = PersonalDetailsProvider.GetPersonalDetails(new Guid(relatedPartyGuid), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName);

            if(personalDetails != null && personalDetails.Count > 0)
            {
                PersonalDetails person = personalDetails.FirstOrDefault();
                addressDetails = GetRelatedPartyAddressDetails(person.PersonalDetailsID);
            }

            if(addressDetails != null && addressDetails.Count > 0)
            {
                retVal = new List<SelectListItem>();
                addressDetails.ForEach(u => {
                    retVal.Add(new SelectListItem() { Value = u.AddressNodeGuid, Text = "Cardholder's " + u.AddressTypeName });
                });
                retVal.Add(new SelectListItem() { Value = "Other Address", Text = "Other Address" });
            }

            return retVal;
        }

        public static bool UpdateApplicantIndividualMainCorrespondingAddress(int apID, int addressDetailsId)
        {
            bool retVal = false;
            var AddressDetailsList = AddressDetailsProcess.GetApplicantAddressDetails(apID);
            foreach (var AddressDetails in AddressDetailsList)
            {
                if (AddressDetails.Id == addressDetailsId)
                {
                    AddressDetails.MainCorrespondenceAddress = true;                    
                }
                else
                {
                    AddressDetails.MainCorrespondenceAddress = false;
                }
                var addDetails = SaveApplicantAddressDetailsModel(apID, AddressDetails);
                if (addDetails != null)
                {
                    retVal = true;
                }
            }
            return retVal;
        }

        //      public static List<SelectListItem> GetCardHolderAddresses(int apID, string applicationType)
        //{
        //          List<SelectListItem> retVal = null;

        //          bool isLegalEntity = string.Equals(applicationType, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase);
        //          int applicantId = ValidationHelper.GetInteger(apID, 0);

        //          List<AddressDetailsModel> addressDetails = new List<AddressDetailsModel>();

        //          if(applicantId > 0)
        //          {
        //              if(isLegalEntity)
        //              {
        //                  var applicationDetails = CompanyDetailsRelatedPartyProcess.GetCompanyDetailsRelatedPartyModelById(applicantId);

        //                  if(applicationDetails != null && applicationDetails.Id > 0)
        //                  {
        //                      addressDetails = GetRelatedPartyAddressDetailsLegal(applicantId);
        //                  }
        //              }
        //              else
        //              {
        //                  var applicationDetails = PersonalDetailsProcess.GetPersonalDetailsModelById(applicantId);

        //                  if(applicationDetails != null && applicationDetails.Id > 0)
        //                  {
        //                      addressDetails = GetRelatedPartyAddressDetails(applicantId);
        //                  }
        //              }

        //              if(addressDetails != null && addressDetails.Count > 0)
        //		{
        //                  retVal = new List<SelectListItem>();
        //                  addressDetails.ForEach(u => {
        //                      retVal.Add(new SelectListItem() { Value = u.AddressNodeGuid, Text = "Cardholder's " + u.AddressTypeName });
        //                  });
        //                  retVal.Add(new SelectListItem() { Value = "Other Address", Text = "Other Address" });
        //              }
        //          }

        //          return retVal;
        //      }

        #region Bind Data

        private static AddressDetailsModel BindAddressDetailsModel(AddressDetails item)
        {
            AddressDetailsModel retVal = null;

            if (item != null)
            {
                var country = ServiceHelper.GetCountriesWithID();
                var addressTypes = ServiceHelper.GetAddressType();
                if(addressTypes != null)
				{
                    var physicalAddressType = ServiceHelper.GetAddressTypePhysical();
                    if(physicalAddressType != null && physicalAddressType.Count > 0)
					{
                        addressTypes.AddRange(physicalAddressType);
                    }
                }
                retVal = new AddressDetailsModel()
                {
                    Id = item.AddressDetailsID,
                    AddressType = item.AddressDetails_AddressType != null ? item.AddressDetails_AddressType.ToString() : string.Empty,
                    AddressTypeName = (addressTypes != null && addressTypes.Count > 0 && item.AddressDetails_AddressType != null && addressTypes.Any(f => f.Value == item.AddressDetails_AddressType.ToString())) ? addressTypes.FirstOrDefault(f => f.Value == item.AddressDetails_AddressType.ToString()).Text : string.Empty,
                    AddressLine1 = item.AddressDetails_AddressLine1,
                    AddressLine2 = item.AddressDetails_AddressLine2,
                    PostalCode = item.AddressDetails_PostalCode,
                    City = item.AddressDetails_City,
                    CountryName = (country != null && country.Count > 0 && item.AddressDetails_Country != null && country.Any(f => f.Value == item.AddressDetails_Country.ToString())) ? country.FirstOrDefault(f => f.Value == item.AddressDetails_Country.ToString()).Text : string.Empty,
                    Country = item.AddressDetails_Country,
                    AddressNodeGuid = item.NodeGUID.ToString(),
                    SaveInRegistry = item.AddressDetails_SaveInRegistry,
                    AddressRegistryId = item.AddressDetails_RegistryId,
                    Status = item.AddressDetails_Status,
                    StatusName = item.AddressDetails_Status == true ? "Complete" : "Pending",
                    POBox= item.AddressDetails_POBox,
                    PhoneNo=item.AddressDetails_Phone_No,
                    Email = item.AddressDetails_Email,
                    FaxNo =item.AddressDetails_Fax_No,
                    Is_Legal=item.AddressDetails_Is_Legal,
                    LocationName = item.AddressDetails_LocationName,
                    CountryCode_PhoneNo=item.AddressDetails_CountryCode_PhoneNo,
                    CountryCode_FaxNo=item.AddressDetails_CountryCode_FaxNo,
                    MainCorrespondenceAddress=item.AddressDetails_MainCorrespondenceAddress,
                    MainCorrespondenceAddressText = item.AddressDetails_MainCorrespondenceAddress == true ? "YES" : "NO",
                    NumberOfStaffEmployed=item.AddressDetails_NumberOfStaffEmployed


                };
            }

            return retVal;
        }
        private static AddressDetailsModel BindAddressDetailsModelPhysical(AddressDetails item)
        {
            AddressDetailsModel retVal = null;

            if (item != null)
            {
                var country = ServiceHelper.GetCountriesWithID();
                var addressTypes = ServiceHelper.GetAddressTypePhysical();
                retVal = new AddressDetailsModel()
                {
                    Id = item.AddressDetailsID,
                    AddressType = item.AddressDetails_AddressType != null ? item.AddressDetails_AddressType.ToString() : string.Empty,
                    AddressTypeName = (addressTypes != null && addressTypes.Count > 0 && item.AddressDetails_AddressType != null && addressTypes.Any(f => f.Value == item.AddressDetails_AddressType.ToString())) ? addressTypes.FirstOrDefault(f => f.Value == item.AddressDetails_AddressType.ToString()).Text : string.Empty,
                    AddressLine1 = item.AddressDetails_AddressLine1,
                    AddressLine2 = item.AddressDetails_AddressLine2,
                    PostalCode = item.AddressDetails_PostalCode,
                    City = item.AddressDetails_City,
                    CountryName = (country != null && country.Count > 0 && item.AddressDetails_Country != null && country.Any(f => f.Value == item.AddressDetails_Country.ToString())) ? country.FirstOrDefault(f => f.Value == item.AddressDetails_Country.ToString()).Text : string.Empty,
                    Country = item.AddressDetails_Country,
                    SaveInRegistry = item.AddressDetails_SaveInRegistry,
                    AddressRegistryId = item.AddressDetails_RegistryId,
                    Status = item.AddressDetails_Status,
                    StatusName = item.AddressDetails_Status == true ? "Complete" : "Pending",
                    POBox = item.AddressDetails_POBox,
                    PhoneNo = item.AddressDetails_Phone_No,
                    FaxNo = item.AddressDetails_Fax_No,
                    Is_Legal = item.AddressDetails_Is_Legal,
                    CountryCode_PhoneNo = item.AddressDetails_CountryCode_PhoneNo,
                    CountryCode_FaxNo = item.AddressDetails_CountryCode_FaxNo,
                    MainCorrespondenceAddress=item.AddressDetails_MainCorrespondenceAddress,
                    MainCorrespondenceAddressText=item.AddressDetails_MainCorrespondenceAddress==true?"YES":"NO",
                    LocationName = item.AddressDetails_LocationName

                };
            }

            return retVal;
        }

        private static AddressRegistryModel BindAddressRegistryModel(AddressDetailsModel item)
        {
            AddressRegistryModel retVal = null;

            if(item != null)
            {
                var country = ServiceHelper.GetCountriesWithID();
                var addressTypes = ServiceHelper.GetAddressType();
                retVal = new AddressRegistryModel()
                {
                    Id = item.AddressRegistryId,
                    AddressType = item.AddressType != null ? item.AddressType.ToString() : string.Empty,
                    AddresLine1 = item.AddressLine1,
                    AddresLine2 = item.AddressLine2,
                    PostalCode = item.PostalCode,
                    City = item.City,
                    Country = item.Country,
                    POBox = item.POBox.ToString(),
                    ModyfiedDate = DateTime.Now.ToString(),
                    LocationName=item.LocationName,
                    CountryCode_PhoneNo=item.CountryCode_PhoneNo,
                    PhoneNo=item.PhoneNo,
                    CountryCode_FaxNo=item.CountryCode_FaxNo,
                    FaxNo=item.FaxNo
                };
            }

            return retVal;
        }

        private static AddressDetails BindAddressDetails(AddressDetails existAddressDetails, AddressDetailsModel item)
        {
            AddressDetails retVal = new AddressDetails();
            if (existAddressDetails != null)
            {
                retVal = existAddressDetails;
            }
            if (item != null)
            {
                if (item.AddressType != null)
                {
                    retVal.AddressDetails_AddressType = new Guid(item.AddressType);
                }
                retVal.AddressDetails_AddressLine1 = item.AddressLine1;
                retVal.AddressDetails_AddressLine2 = item.AddressLine2;
                retVal.AddressDetails_PostalCode = item.PostalCode;
                retVal.AddressDetails_City = item.City;
                retVal.AddressDetails_Country = item.Country;
                retVal.AddressDetails_SaveInRegistry = item.SaveInRegistry;
                retVal.AddressDetails_RegistryId = item.AddressRegistryId;
                retVal.AddressDetails_Status = item.Status;
                retVal.AddressDetails_POBox = item.POBox;
                retVal.AddressDetails_Phone_No = item.PhoneNo;
                retVal.AddressDetails_Email = item.Email;
                retVal.AddressDetails_Fax_No = item.FaxNo;
                retVal.AddressDetails_Is_Legal = item.Is_Legal;
                retVal.AddressDetails_CountryCode_PhoneNo = item.CountryCode_PhoneNo;
                retVal.AddressDetails_CountryCode_FaxNo = item.CountryCode_FaxNo;
                retVal.AddressDetails_MainCorrespondenceAddress = item.MainCorrespondenceAddress;
                retVal.AddressDetails_LocationName = item.LocationName;
                retVal.AddressDetails_NumberOfStaffEmployed = item.NumberOfStaffEmployed;
            }

            return retVal;
        }

        #endregion
        #region----Address Details Legal Related party-------
        public static List<AddressDetailsModel> GetRelatedPartyAddressDetailsLegal(int applicantId)
        {
            List<AddressDetailsModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (applicantId > 0)
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(CompanyDetailsRelatedParty.CLASS_NAME)
                    .WhereEquals("CompanyDetailsRelatedPartyID", applicantId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode addressDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        addressDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (addressDetailsRoot != null)
                        {
                            List<TreeNode> addressDetailsNodes = addressDetailsRoot.Children.Where(u => u.ClassName == AddressDetails.CLASS_NAME).ToList();

                            if (addressDetailsNodes != null && addressDetailsNodes.Count > 0)
                            {
                                retVal = new List<AddressDetailsModel>();
                                addressDetailsNodes.ForEach(t =>
                                {
                                    AddressDetails addressDetails = AddressDetailsProvider.GetAddressDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (addressDetails != null)
                                    {
                                        AddressDetailsModel personalDetailsModel = BindAddressDetailsModel(addressDetails);
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
        public static AddressDetailsModel SaveRelatedPartyAddressDetailsLegalModel(int applicantId, AddressDetailsModel model)
        {
            AddressDetailsModel retVal = null;

            if (model != null && model.Id > 0)
            {
                AddressDetails addressDetails = GetAddressDetailsById(model.Id);
                if (addressDetails != null)
                {
                    AddressDetails updatedAddressDetails = BindAddressDetails(addressDetails, model);
                    if (updatedAddressDetails != null)
                    {
                        updatedAddressDetails.Update();
                        model = BindAddressDetailsModel(updatedAddressDetails);
                        retVal = model;
                    }
                }
            }
            else if (applicantId > 0)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(CompanyDetailsRelatedParty.CLASS_NAME)
                    .WhereEquals("CompanyDetailsRelatedPartyID", applicantId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode addressDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        addressDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        addressDetailsRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        addressDetailsRoot.DocumentName = _ApplicantsFolderName;
                        addressDetailsRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        addressDetailsRoot.Insert(applicationDetailsNode);
                    }
                    AddressDetails addressDetails = BindAddressDetails(null, model);
                    if (addressDetails != null && addressDetailsRoot != null)
                    {
                        //addressDetails.DocumentName = model.AddressLine1;
                        //addressDetails.NodeName = model.AddressLine1;
                        addressDetails.DocumentName = _ApplicantsDocumentName;
                        addressDetails.NodeName = _ApplicantsDocumentName;
                        addressDetails.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        addressDetails.Insert(addressDetailsRoot);
                        model = BindAddressDetailsModel(addressDetails);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }
        #endregion

        #region----Address Details Legal Applicant-------
        public static List<AddressDetailsModel> GetApplicantAddressDetailsLegal(int applicantId)
        {
            List<AddressDetailsModel> retVal = null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            if (applicantId > 0)
            {
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(CompanyDetails.CLASS_NAME)
                    .WhereEquals("CompanyDetailsID", applicantId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode addressDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        addressDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));

                        if (addressDetailsRoot != null)
                        {
                            List<TreeNode> addressDetailsNodes = addressDetailsRoot.Children.Where(u => u.ClassName == AddressDetails.CLASS_NAME).ToList();

                            if (addressDetailsNodes != null && addressDetailsNodes.Count > 0)
                            {
                                retVal = new List<AddressDetailsModel>();
                                addressDetailsNodes.ForEach(t =>
                                {
                                    AddressDetails addressDetails = AddressDetailsProvider.GetAddressDetails(t.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();

                                    if (addressDetails != null)
                                    {
                                        AddressDetailsModel personalDetailsModel = BindAddressDetailsModel(addressDetails);
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
        public static AddressDetailsModel SaveApplicantAddressDetailsLegalModel(int applicantId, AddressDetailsModel model)
        {
            AddressDetailsModel retVal = null;

            if (model != null && model.Id > 0)
            {
                AddressDetails addressDetails = GetAddressDetailsById(model.Id);
                if (addressDetails != null)
                {
                    AddressDetails updatedPersonalDetails = BindAddressDetails(addressDetails, model);
                    if (updatedPersonalDetails != null)
                    {
                        updatedPersonalDetails.Update();
                        model = BindAddressDetailsModel(updatedPersonalDetails);
                        retVal = model;
                    }
                }
            }
            else if (applicantId > 0)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode applicationDetailsNode = tree.SelectNodes()
                    .Path(_ApplicationRootNodePath, PathTypeEnum.Children)
                    .Type(CompanyDetails.CLASS_NAME)
                    .WhereEquals("CompanyDetailsID", applicantId)
                    .OnCurrentSite()
                    .Published(false)
                    .FirstOrDefault();

                if (applicationDetailsNode != null)
                {
                    TreeNode addressDetailsRoot = null;
                    if (applicationDetailsNode.Children.Any(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase)))
                    {
                        addressDetailsRoot = applicationDetailsNode.Children.FirstOrDefault(y => y.ClassName == "CMS.Folder" && string.Equals(y.NodeName, _ApplicantsFolderName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        addressDetailsRoot = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        addressDetailsRoot.DocumentName = "Address Details";
                        addressDetailsRoot.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;
                        addressDetailsRoot.Insert(applicationDetailsNode);
                    }
                    AddressDetails addressDetails = BindAddressDetails(null, model);
                    if (addressDetails != null && addressDetailsRoot != null)
                    {
                        //addressDetails.DocumentName = model.AddressLine1;
                        //addressDetails.NodeName = model.AddressLine1;
                        addressDetails.DocumentName = _ApplicantsDocumentName;
                        addressDetails.NodeName = _ApplicantsDocumentName;
                        addressDetails.DocumentCulture = LocalizationContext.CurrentCulture.CultureCode;

                        addressDetails.Insert(addressDetailsRoot);
                        model = BindAddressDetailsModel(addressDetails);
                        retVal = model;
                    }
                }
            }

            return retVal;
        }

        public static bool UpdateApplicantLegalMainCorrespondingAddress(int apID, int addressDetailsId)
        {
            bool retVal = false;
            var AddressDetailsList = AddressDetailsProcess.GetApplicantAddressDetailsLegal(apID);
            foreach (var AddressDetails in AddressDetailsList)
            {
                if(AddressDetails.Id == addressDetailsId)
                {
                    AddressDetails.MainCorrespondenceAddress = true;                    
                }
                else
                {
                    AddressDetails.MainCorrespondenceAddress = false;
                }
                var addDetails = SaveApplicantAddressDetailsLegalModel(apID, AddressDetails);
                if (addDetails != null)
                {
                    retVal = true;
                }
            }
            return retVal;
        }
		#endregion

		#region Address Registry Methods

        public static AddressRegistry SaveAddressRegistry(string userName,string introducerCompany, RegistriesRepository registriesRepository, AddressDetailsModel addressDetailsModel)
		{
            AddressRegistry retVal = null;
            if(registriesRepository != null && addressDetailsModel != null)
            {
                AddressRegistryModel addressRegistryModel = BindAddressRegistryModel(addressDetailsModel);
                if(addressRegistryModel.Id > 0)
				{
                    var userAddressRegistry = registriesRepository.GetAddressRegistryById(addressRegistryModel.Id);
                    if(userAddressRegistry != null)
                    {
                        TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                        var manager = VersionManager.GetInstance(tree);
                        userAddressRegistry.DocumentName = addressRegistryModel.AddresLine1;
                        userAddressRegistry.SetValue("Name", addressRegistryModel.AddresLine1);
                        userAddressRegistry.SetValue("AddressType", addressRegistryModel.AddressType);
                        userAddressRegistry.DocumentCulture = "en-US";
                        userAddressRegistry.SetValue("AddresLine1", addressRegistryModel.AddresLine1);
                        userAddressRegistry.SetValue("AddresLine2", addressRegistryModel.AddresLine2);
                        userAddressRegistry.SetValue("PostalCode", addressRegistryModel.PostalCode);
                        userAddressRegistry.SetValue("Country", addressRegistryModel.Country);
                        userAddressRegistry.SetValue("City", addressRegistryModel.City);
                        userAddressRegistry.SetValue("POBox", addressRegistryModel.POBox);
                        userAddressRegistry.SetValue("LocationName", addressRegistryModel.LocationName);
                        userAddressRegistry.SetValue("CountryCode_PhoneNo", addressRegistryModel.CountryCode_PhoneNo);
                        userAddressRegistry.SetValue("PhoneNo", addressRegistryModel.PhoneNo);
                        userAddressRegistry.SetValue("CountryCode_FaxNo", addressRegistryModel.CountryCode_FaxNo);
                        userAddressRegistry.SetValue("FaxNo", addressRegistryModel.FaxNo);
                        userAddressRegistry.Update();
                        retVal = userAddressRegistry;

                    }
                }
				else if(!string.IsNullOrEmpty(userName))
				{
                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

                    var UserRegistry = registriesRepository.GetRegistryUserByName(introducerCompany);

                    if(UserRegistry != null)
                    {
                        CMS.DocumentEngine.TreeNode addressfoldernode_parent = tree.SelectNodes()
                        .Path(UserRegistry.NodeAliasPath + "/" + "Address-Registry")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                        if(addressfoldernode_parent == null)
                        {
                            CMS.DocumentEngine.TreeNode branchfoldernode = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                            branchfoldernode.DocumentName = "Address Registry";
                            branchfoldernode.DocumentCulture = "en-US";
                            branchfoldernode.Insert(UserRegistry);
                            addressfoldernode_parent = branchfoldernode;
                        }

                        CMS.DocumentEngine.TreeNode personsRegistryAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.AddressRegistry", tree);
                        personsRegistryAdd.DocumentName = addressRegistryModel.AddresLine1;
                        personsRegistryAdd.SetValue("Name", addressRegistryModel.AddresLine1);
                        personsRegistryAdd.SetValue("AddressType", addressRegistryModel.AddressType);
                        personsRegistryAdd.DocumentCulture = "en-US";
                        personsRegistryAdd.SetValue("AddresLine1", addressRegistryModel.AddresLine1);
                        personsRegistryAdd.SetValue("AddresLine2", addressRegistryModel.AddresLine2);
                        personsRegistryAdd.SetValue("PostalCode", addressRegistryModel.PostalCode);
                        personsRegistryAdd.SetValue("Country", addressRegistryModel.Country);
                        personsRegistryAdd.SetValue("City", addressRegistryModel.City);
                        personsRegistryAdd.SetValue("LocationName", addressRegistryModel.LocationName);
                        personsRegistryAdd.SetValue("CountryCode_PhoneNo", addressRegistryModel.CountryCode_PhoneNo);
                        personsRegistryAdd.SetValue("PhoneNo", addressRegistryModel.PhoneNo);
                        personsRegistryAdd.SetValue("CountryCode_FaxNo", addressRegistryModel.CountryCode_FaxNo);
                        personsRegistryAdd.SetValue("FaxNo", addressRegistryModel.FaxNo);
                        personsRegistryAdd.Insert(addressfoldernode_parent);

                        retVal = AddressRegistryProvider.GetAddressRegistry(personsRegistryAdd.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    }
                    else
                    {
                        CMS.DocumentEngine.TreeNode addressRegistryFolder = tree.SelectNodes()
                       .Path("/Registries")
                       .Published(false)
                       .OnCurrentSite()
                       .FirstOrDefault();
                        TreeNode personsRegistryUser = TreeNode.New("Eurobank.PersonsRegistryUser", tree);
                        UserInfo user = UserInfoProvider.GetUserInfo(userName);
                        personsRegistryUser.DocumentName = introducerCompany;
                        personsRegistryUser.DocumentCulture = "en-US";
                        personsRegistryUser.SetValue("UserID", user.UserID);
                        personsRegistryUser.Insert(addressRegistryFolder);
                        CMS.DocumentEngine.TreeNode addressfoldernode = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                        addressfoldernode.DocumentName = "Address Registry";
                        addressfoldernode.DocumentCulture = "en-US";
                        addressfoldernode.Insert(personsRegistryUser);
                        CMS.DocumentEngine.TreeNode personsRegistryAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.AddressRegistry", tree);
                        personsRegistryAdd.DocumentName = addressRegistryModel.LocationName;
                        personsRegistryAdd.SetValue("Name", addressRegistryModel.LocationName);
                        personsRegistryAdd.SetValue("AddressType", addressRegistryModel.AddressType);
                        personsRegistryAdd.DocumentCulture = "en-US";
                        personsRegistryAdd.SetValue("AddresLine1", addressRegistryModel.AddresLine1);
                        personsRegistryAdd.SetValue("AddresLine2", addressRegistryModel.AddresLine2);
                        personsRegistryAdd.SetValue("PostalCode", addressRegistryModel.PostalCode);
                        personsRegistryAdd.SetValue("Country", addressRegistryModel.Country);
                        personsRegistryAdd.SetValue("City", addressRegistryModel.City);
                        personsRegistryAdd.SetValue("LocationName", addressRegistryModel.LocationName);
                        personsRegistryAdd.SetValue("CountryCode_PhoneNo", addressRegistryModel.CountryCode_PhoneNo);
                        personsRegistryAdd.SetValue("PhoneNo", addressRegistryModel.PhoneNo);
                        personsRegistryAdd.SetValue("CountryCode_FaxNo", addressRegistryModel.CountryCode_FaxNo);
                        personsRegistryAdd.SetValue("FaxNo", addressRegistryModel.FaxNo);
                        personsRegistryAdd.Insert(addressfoldernode);
                        addressRegistryModel.CreatedDate = ValidationHelper.GetString(Convert.ToDateTime(personsRegistryAdd.DocumentCreatedWhen).ToString("MM/dd/yyyy HH:mm:ss"), "");
                        addressRegistryModel.ModyfiedDate = ValidationHelper.GetString(Convert.ToDateTime(personsRegistryAdd.DocumentModifiedWhen).ToString("MM/dd/yyyy HH:mm:ss"), "");

                        retVal = AddressRegistryProvider.GetAddressRegistry(addressRegistryModel.NodeGUID, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName).FirstOrDefault();
                    }
                }
            }

            return retVal;
        }

  //      public static void UpdateAddressRegistry(int addressRegistryId, RegistriesRepository registriesRepository, AddressRegistryModel addressRegistryModel)
		//{
  //          if(registriesRepository != null && addressRegistryModel != null && addressRegistryId > 0)
  //          {

  //              var userAddressRegistry = registriesRepository.GetAddressRegistryById(addressRegistryId);
  //              if(userAddressRegistry != null)
  //              {
  //                  TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
  //                  var manager = VersionManager.GetInstance(tree);
  //                  userAddressRegistry.DocumentName = addressRegistryModel.LocationName;
  //                  userAddressRegistry.SetValue("Name", addressRegistryModel.LocationName);
  //                  userAddressRegistry.SetValue("AddressType", addressRegistryModel.AddressType);
  //                  userAddressRegistry.DocumentCulture = "en-US";
  //                  userAddressRegistry.SetValue("AddresLine1", addressRegistryModel.AddresLine1);
  //                  userAddressRegistry.SetValue("AddresLine2", addressRegistryModel.AddresLine2);
  //                  userAddressRegistry.SetValue("PostalCode", addressRegistryModel.PostalCode);
  //                  userAddressRegistry.SetValue("Country", addressRegistryModel.Country);
  //                  userAddressRegistry.SetValue("City", addressRegistryModel.City);
  //                  userAddressRegistry.Update();

  //              }

  //          }

  //      }

        #endregion
    }
}
