using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Membership;
using Eurobank.Controllers;
using Eurobank.Helpers.CustomHandler;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.Process;
using Eurobank.Helpers.Validation;
using Eurobank.Models;
using Eurobank.Models.Application;
using Eurobank.Models.Application.Common;
using Eurobank.Models.Applications.DebitCard;
using Eurobank.Models.IdentificationDetails;
using Eurobank.Models.Registries;
using Eurobank.Models.User;
using Eurobank.Services;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
//using OfficeOpenXml;

[assembly: RegisterPageRoute(Registries.CLASS_NAME, typeof(RegistriesController))]
namespace Eurobank.Controllers
{
    [AuthorizeRoles(Role.PowerUser, Role.NormalUser)]
	[SessionAuthorization]
	//[IgnoreAntiforgeryToken]
	public class RegistriesController : Controller
    {
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;
        private readonly RegistriesRepository registriesRepository;
        private readonly IdentificationDetailsRepository identificationDetailsRepository;
        public RegistriesController(IPageDataContextRetriever pageDataContextRetriever,
            IPageUrlRetriever pageUrlRetriever,
            IPageAttachmentUrlRetriever attachmentUrlRetriever, RegistriesRepository registriesRepository, IdentificationDetailsRepository identificationDetailsRepository)
        {
            this.pageDataContextRetriever = pageDataContextRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
            this.registriesRepository = registriesRepository;
            this.identificationDetailsRepository = identificationDetailsRepository;
        }
        public IActionResult Index()
        {
            ViewBag.titleList = ServiceHelper.GetTitle();
            //ViewBag.Country = ServiceHelper.GetCountriesRegistry();
            ViewBag.Country = ServiceHelper.GetCountriesWithID();
            //ViewBag.addressType = ServiceHelper.GetAddressType();
            ViewBag.addressType = ServiceHelper.GetAddressTypePersonRegistry();

            ViewBag.gendar = ServiceHelper.GetGendar();
            ViewBag.educationLevel = ServiceHelper.GetEducationLevel();
            ViewBag.identificationType = ServiceHelper.GetTypeIdentification();
            ViewBag.communicationLanguage = ServiceHelper.GetCommunicationLanguage();
            ViewBag.applicationType = ServiceHelper.GetRegistryApplicationType();
            ViewBag.typeofIdentification = ServiceHelper.GetTypeofIdentification();

            ViewBag.CompanyEntities = ServiceHelper.GetCompanyEntityTypes();
            ViewBag.ListingStatuses = ServiceHelper.GetListingStatuses();
            ViewBag.CorporationShares = ServiceHelper.GetBoolDropDownListDefaults();
            ViewBag.EntityLocatedAndOperates = ServiceHelper.GetBoolDropDownListDefaults();
            ViewBag.Countries = ServiceHelper.GetCountries();
            ViewBag.PreferredMailingAddress = ServiceHelper.GetPreferred_Mailing_Address();

            return View();
        }
        #region Address Registry
        public IActionResult AddressRegistry_Read([DataSourceRequest] DataSourceRequest request)
        {

            List<AddressRegistryModel> addressRegistryModels = new List<AddressRegistryModel>();
            UserModel userModel = UserProcess.GetUser(User.Identity.Name);

            if (userModel != null && string.Equals(userModel.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase) && userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null)
            {
                var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);
                ////Hard Coded For testing
                //var UserRegistry = registriesRepository.GetRegistryUserByName("administrator");
                if (UserRegistry != null)
                {
                    int rowid = 0;
                    var registry = registriesRepository.GetAddressRegistries(UserRegistry.NodeAliasPath);
                    var country = ServiceHelper.GetCountriesWithID();
                    foreach (var item in registry)
                    {
                        rowid = rowid + 1;
                        AddressRegistryModel addressRegistryModel = new AddressRegistryModel();
                        addressRegistryModel.RowID = rowid;
                        addressRegistryModel.NodeID = ValidationHelper.GetString(item.GetValue("AddressRegistryID"), "");
                        addressRegistryModel.NodeGUID = ValidationHelper.GetString(item.GetValue("NodeGUID"), "");
                        addressRegistryModel.LocationName = ValidationHelper.GetString(item.GetValue("LocationName"), "").ToUpper();
                        addressRegistryModel.AddressType = ValidationHelper.GetString(item.GetValue("AddressType"), "");
                        //if(!string.IsNullOrEmpty(ValidationHelper.GetString(item.GetValue("AddressType"), "")))
                        //{
                        //	addressRegistryModel.AddressTypeName = ValidationHelper.GetString(item.GetValue("AddressType"), "").Split('|')[1];
                        //}
                        addressRegistryModel.AddressType = ValidationHelper.GetString(item.GetValue("AddressType"), "");
                        addressRegistryModel.AddresLine1 = ValidationHelper.GetString(item.GetValue("AddresLine1"), "").ToUpper();
                        addressRegistryModel.AddresLine2 = ValidationHelper.GetString(item.GetValue("AddresLine2"), "").ToUpper();
                        addressRegistryModel.PostalCode = ValidationHelper.GetString(item.GetValue("PostalCode"), "").ToUpper();
                        addressRegistryModel.Country = ValidationHelper.GetString(item.GetValue("Country"), "").ToUpper();
                        if (!string.IsNullOrEmpty(addressRegistryModel.Country))
                        {
                            //addressRegistryModel.CountryName =GetCountryName( ValidationHelper.GetString(item.GetValue("Country"), ""));
                            addressRegistryModel.CountryName = (country != null && country.Count > 0 && country.Any(f => f.Value == addressRegistryModel.Country)) ? country.FirstOrDefault(f => f.Value == addressRegistryModel.Country).Text.ToUpper() : string.Empty;
                        }
                        addressRegistryModel.City = ValidationHelper.GetString(item.GetValue("City"), "").ToUpper();
                        addressRegistryModel.POBox = ValidationHelper.GetString(item.GetValue("POBox"), "").ToUpper();
                        addressRegistryModel.CreatedDate = ValidationHelper.GetString(Convert.ToDateTime(item.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss"), "");
                        addressRegistryModel.ModyfiedDate = ValidationHelper.GetString(Convert.ToDateTime(item.DocumentModifiedWhen).ToString("dd/MM/yyyy HH:mm:ss"), "");
                        addressRegistryModel.CountryCode_PhoneNo = ValidationHelper.GetString(item.GetValue("CountryCode_PhoneNo"), "");
                        addressRegistryModel.PhoneNo = ValidationHelper.GetString(item.GetValue("PhoneNo"), "");
                        addressRegistryModel.CountryCode_FaxNo = ValidationHelper.GetString(item.GetValue("CountryCode_FaxNo"), "");
                        addressRegistryModel.FaxNo = ValidationHelper.GetString(item.GetValue("FaxNo"), "");

                        addressRegistryModels.Add(addressRegistryModel);

                    }
                    addressRegistryModels = addressRegistryModels.OrderByDescending(x => DateTime.ParseExact(x.CreatedDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).ToList();
                }
            }

            if (request != null && addressRegistryModels != null && addressRegistryModels.Count > 0 && request.Sorts != null && request.Sorts.Count > 0 && request.Sorts.Any(t => t.Member == "CreatedDate" || t.Member == "ModyfiedDate"))
            {
                var sort = request.Sorts.FirstOrDefault(t => t.Member == "CreatedDate" || t.Member == "ModyfiedDate");
                if (sort.Member == "CreatedDate")
                {
                    if (sort.SortDirection == Kendo.Mvc.ListSortDirection.Ascending)
                    {
                        addressRegistryModels = addressRegistryModels.OrderBy(x => DateTime.ParseExact(x.CreatedDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).ToList();
                    }
                    else
                    {
                        addressRegistryModels = addressRegistryModels.OrderByDescending(x => DateTime.ParseExact(x.CreatedDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).ToList();
                    }
                }
                else
                {
                    if (sort.SortDirection == Kendo.Mvc.ListSortDirection.Ascending)
                    {
                        addressRegistryModels = addressRegistryModels.OrderBy(x => DateTime.ParseExact(x.ModyfiedDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).ToList();
                    }
                    else
                    {
                        addressRegistryModels = addressRegistryModels.OrderByDescending(x => DateTime.ParseExact(x.ModyfiedDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).ToList();
                    }
                }
                request.Sorts = null;
            }

            return Json(addressRegistryModels.ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult AddressRegistryPopup_Create([DataSourceRequest] DataSourceRequest request, AddressRegistryModel addressRegistryModel)
        {
            ValidationResultModel validationResultModel = new ValidationResultModel();
            validationResultModel = RegistryValidation.ValidateAddresstDetails(addressRegistryModel);
            if (validationResultModel != null)
            {
                foreach (var item in validationResultModel.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            if (addressRegistryModel != null && ModelState.IsValid)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                UserModel userModel = UserProcess.GetUser(User.Identity.Name);

                if (userModel != null && string.Equals(userModel.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase) && userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null)
                {
                    var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);

                    if (UserRegistry != null)
                    {
                        CMS.DocumentEngine.TreeNode addressfoldernode_parent = tree.SelectNodes()
                        .Path(UserRegistry.NodeAliasPath + "/" + "Address-Registry")
                        .OnCurrentSite()
                        .Published(false)
                        .FirstOrDefault();
                        if (addressfoldernode_parent == null)
                        {
                            CMS.DocumentEngine.TreeNode branchfoldernode = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                            branchfoldernode.DocumentName = "Address Registry";
                            branchfoldernode.DocumentCulture = "en-US";
                            branchfoldernode.Insert(UserRegistry);
                            addressfoldernode_parent = branchfoldernode;
                        }

                        CMS.DocumentEngine.TreeNode personsRegistryAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.AddressRegistry", tree);
                        personsRegistryAdd.DocumentName = addressRegistryModel.LocationName;
                        personsRegistryAdd.SetValue("Name", addressRegistryModel.LocationName);
                        personsRegistryAdd.SetValue("LocationName", addressRegistryModel.LocationName);
                        personsRegistryAdd.SetValue("AddressType", addressRegistryModel.AddressType);
                        personsRegistryAdd.DocumentCulture = "en-US";
                        personsRegistryAdd.SetValue("AddresLine1", addressRegistryModel.AddresLine1);
                        personsRegistryAdd.SetValue("AddresLine2", addressRegistryModel.AddresLine2);
                        personsRegistryAdd.SetValue("PostalCode", addressRegistryModel.PostalCode);
                        personsRegistryAdd.SetValue("Country", addressRegistryModel.Country);
                        personsRegistryAdd.SetValue("City", addressRegistryModel.City);
                        personsRegistryAdd.SetValue("POBox", addressRegistryModel.POBox);
                        personsRegistryAdd.SetValue("UserOrganization", userModel.IntroducerUser.Introducer.NodeGUID);
                        personsRegistryAdd.SetValue("CountryCode_PhoneNo", addressRegistryModel.CountryCode_PhoneNo);
                        personsRegistryAdd.SetValue("PhoneNo", addressRegistryModel.PhoneNo);
                        personsRegistryAdd.SetValue("CountryCode_FaxNo", addressRegistryModel.CountryCode_FaxNo);
                        personsRegistryAdd.SetValue("FaxNo", addressRegistryModel.FaxNo);
                        personsRegistryAdd.Insert(addressfoldernode_parent);
                        addressRegistryModel.CreatedDate = ValidationHelper.GetString(Convert.ToDateTime(personsRegistryAdd.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss"), "");
                        addressRegistryModel.ModyfiedDate = ValidationHelper.GetString(Convert.ToDateTime(personsRegistryAdd.DocumentModifiedWhen).ToString("dd/MM/yyyy HH:mm:ss"), "");

                    }
                    else
                    {
                        CMS.DocumentEngine.TreeNode addressRegistryFolder = tree.SelectNodes()
                       .Path("/Registries")
                       .Published(false)
                       .OnCurrentSite()
                       .FirstOrDefault();
                        TreeNode personsRegistryUser = TreeNode.New("Eurobank.AddressRegistry", tree);
                        UserInfo user = UserInfoProvider.GetUserInfo(User.Identity.Name);


                        if (userModel != null && string.Equals(userModel.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase) && userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null)
                        {
                            personsRegistryUser.DocumentName = userModel.IntroducerUser.Introducer.DocumentName;
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
                            personsRegistryAdd.SetValue("LocationName", addressRegistryModel.LocationName);
                            personsRegistryAdd.SetValue("AddressType", addressRegistryModel.AddressType);
                            personsRegistryAdd.DocumentCulture = "en-US";
                            personsRegistryAdd.SetValue("AddresLine1", addressRegistryModel.AddresLine1);
                            personsRegistryAdd.SetValue("AddresLine2", addressRegistryModel.AddresLine2);
                            personsRegistryAdd.SetValue("POBox", addressRegistryModel.POBox);
                            personsRegistryAdd.SetValue("PostalCode", addressRegistryModel.PostalCode);
                            personsRegistryAdd.SetValue("Country", addressRegistryModel.Country);
                            personsRegistryAdd.SetValue("City", addressRegistryModel.City);
                            personsRegistryAdd.SetValue("UserOrganization", userModel.IntroducerUser.Introducer.NodeGUID);
                            personsRegistryAdd.SetValue("CountryCode_PhoneNo", addressRegistryModel.CountryCode_PhoneNo);
                            personsRegistryAdd.SetValue("PhoneNo", addressRegistryModel.PhoneNo);
                            personsRegistryAdd.SetValue("CountryCode_FaxNo", addressRegistryModel.CountryCode_FaxNo);
                            personsRegistryAdd.SetValue("FaxNo", addressRegistryModel.FaxNo);
                            personsRegistryAdd.Insert(addressfoldernode);
                            addressRegistryModel.CreatedDate = ValidationHelper.GetString(Convert.ToDateTime(personsRegistryAdd.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss"), "");
                            addressRegistryModel.ModyfiedDate = ValidationHelper.GetString(Convert.ToDateTime(personsRegistryAdd.DocumentModifiedWhen).ToString("dd/MM/yyyy HH:mm:ss"), "");
                        }

                    }
                }




            }
            if (!string.IsNullOrEmpty(addressRegistryModel.Country))
            {
                addressRegistryModel.CountryName = GetCountryName(addressRegistryModel.Country);
            }
            if (!string.IsNullOrEmpty(addressRegistryModel.AddressType))
            {
                addressRegistryModel.AddressTypeName = ServiceHelper.GetName(addressRegistryModel.AddressType, "/Lookups/General/Address-Type");
            }

            return Json(new[] { addressRegistryModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult AddressRegistryPopup_Update([DataSourceRequest] DataSourceRequest request, AddressRegistryModel addressRegistryModel)
        {
            ValidationResultModel validationResultModel = new ValidationResultModel();
            validationResultModel = RegistryValidation.ValidateAddresstDetails(addressRegistryModel);
            if (validationResultModel != null)
            {
                foreach (var item in validationResultModel.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            if (addressRegistryModel != null && ModelState.IsValid)
            {

                var userAddressRegistry = registriesRepository.GetAddressRegistryUserByNodeGUID(addressRegistryModel.NodeGUID);
                if (userAddressRegistry != null)
                {
                    TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                    var manager = VersionManager.GetInstance(tree);
                    //if(!UserRegistry.IsCheckedOut)
                    //{
                    //	manager.CheckOut(UserRegistry);
                    //}
                    userAddressRegistry.DocumentName = addressRegistryModel.LocationName;
                    userAddressRegistry.SetValue("Name", addressRegistryModel.LocationName);
                    userAddressRegistry.SetValue("LocationName", addressRegistryModel.LocationName);
                    userAddressRegistry.SetValue("AddressType", addressRegistryModel.AddressType);
                    userAddressRegistry.DocumentCulture = "en-US";
                    userAddressRegistry.SetValue("AddresLine1", addressRegistryModel.AddresLine1);
                    userAddressRegistry.SetValue("AddresLine2", addressRegistryModel.AddresLine2);
                    userAddressRegistry.SetValue("PostalCode", addressRegistryModel.PostalCode);
                    userAddressRegistry.SetValue("Country", addressRegistryModel.Country);
                    userAddressRegistry.SetValue("City", addressRegistryModel.City);
                    userAddressRegistry.SetValue("POBox", addressRegistryModel.POBox);
                    userAddressRegistry.SetValue("CountryCode_PhoneNo", addressRegistryModel.CountryCode_PhoneNo);
                    userAddressRegistry.SetValue("PhoneNo", addressRegistryModel.PhoneNo);
                    userAddressRegistry.SetValue("CountryCode_FaxNo", addressRegistryModel.CountryCode_FaxNo);
                    userAddressRegistry.SetValue("FaxNo", addressRegistryModel.FaxNo);
                    userAddressRegistry.Update();

                    addressRegistryModel.ModyfiedDate = Convert.ToDateTime(userAddressRegistry.DocumentModifiedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                    //if(UserRegistry.IsCheckedOut)
                    //{
                    //	manager.CheckIn(UserRegistry, null, null);
                    //}
                }

            }
            if (!string.IsNullOrEmpty(addressRegistryModel.Country))
            {
                addressRegistryModel.CountryName = GetCountryName(addressRegistryModel.Country);
            }
            if (!string.IsNullOrEmpty(addressRegistryModel.AddressType))
            {
                addressRegistryModel.AddressTypeName = ServiceHelper.GetName(addressRegistryModel.AddressType, "/Lookups/General/Address-Type");
            }
            return Json(new[] { addressRegistryModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult AddressRegistryPopup_Destroy([DataSourceRequest] DataSourceRequest request, AddressRegistryModel addressRegistryModel)
        {
            if (addressRegistryModel != null)
            {
                //registriesRepository.GetAddressRegistryUserByNodeGUID(personsRegistry.NodeGUID).DeleteAllCultures();
                var userAddressRegistry = registriesRepository.GetAddressRegistryUserByNodeGUID(addressRegistryModel.NodeGUID);
                userAddressRegistry.DocumentPublishTo = DateTime.Now;
                userAddressRegistry.Update();
            }

            return Json(new[] { addressRegistryModel }.ToDataSourceResult(request, ModelState));
        }

        public IActionResult RegisteredAddresses_Read([DataSourceRequest] DataSourceRequest request)
        {

            List<AddressDetailsModel> addressRegistryModels = new List<AddressDetailsModel>();
            addressRegistryModels = AddressDetailsProcess.GetAllAddressDetails();
            return Json(addressRegistryModels.ToDataSourceResult(request));
        }
        #endregion
        #region persons registry
        public IActionResult Registry_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<Eurobank.Models.Registries.PersonsRegistry> personsRegistries = new List<Eurobank.Models.Registries.PersonsRegistry>();
            UserModel userModel = UserProcess.GetUser(User.Identity.Name);

            if (userModel != null && string.Equals(userModel.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase) && userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null)
            {
                var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);
                if (UserRegistry != null)
                {
                    int rowid = 0;
                    var registry = registriesRepository.GetRegistries(UserRegistry.NodeAliasPath);
                    var companyRegistry = registriesRepository.GetCompanyRegistries(UserRegistry.NodeAliasPath);

                    foreach (var item in registry)
                    {
                        Eurobank.Models.Registries.PersonsRegistry personsRegistry = new Eurobank.Models.Registries.PersonsRegistry();
                        rowid = rowid + 1;
                        personsRegistry.RowID = rowid;
                        personsRegistry.NodeID = ValidationHelper.GetString(item.GetValue("PersonsRegistryID"), "");
                        personsRegistry.NodeGUID = ValidationHelper.GetString(item.GetValue("NodeGUID"), "");
                        personsRegistry.ApplicationType = ValidationHelper.GetString(item.GetValue("ApplicationType"), "");
                        personsRegistry.ApplicationTypeName = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ApplicationType"), ""), "/Lookups/General/APPLICATION-TYPE");
                        if (string.Equals(personsRegistry.ApplicationTypeName, "JOINT INDIVIDUAL", StringComparison.OrdinalIgnoreCase))
                        {
                            var lstEntityType = ServiceHelper.GetApplicationType();
                            string individualEntity = lstEntityType.Where(x => x.Text == "INDIVIDUAL").FirstOrDefault().Value.ToString();
                            personsRegistry.ApplicationTypeName = "INDIVIDUAL";
                            personsRegistry.ApplicationType = individualEntity;
                        }
                        personsRegistry.Name = ValidationHelper.GetString(item.GetValue("Name"), "").ToUpper();
                        personsRegistry.Title = ValidationHelper.GetString(item.GetValue("Title"), "");
                        personsRegistry.TitleName = ValidationHelper.GetString(item.GetValue("Title"), "");
                        personsRegistry.FirstName = ValidationHelper.GetString(item.GetValue("FirstName"), "");
                        personsRegistry.LastName = ValidationHelper.GetString(item.GetValue("LastName"), "");
                        personsRegistry.FatherName = ValidationHelper.GetString(item.GetValue("FathersName"), "");
                        personsRegistry.Gender = ValidationHelper.GetString(item.GetValue("Gender"), "");
                        personsRegistry.GenderName = ValidationHelper.GetString(item.GetValue("Gender"), "");
                        personsRegistry.DateofBirth = ValidationHelper.GetDateTime(item.GetValue("DateofBirth"), default(DateTime));
                        personsRegistry.PlaceofBirth = ValidationHelper.GetString(item.GetValue("PlaceofBirth"), "");
                        personsRegistry.CountryofBirth = ValidationHelper.GetString(item.GetValue("CountryofBirth"), "");
                        personsRegistry.CountryofBirthName = ValidationHelper.GetString(item.GetValue("CountryofBirth"), "");
                        personsRegistry.EducationLevel = ValidationHelper.GetString(item.GetValue("EducationLevel"), "");
                        personsRegistry.EducationLevelName = ValidationHelper.GetString(item.GetValue("EducationLevel"), "");
                        personsRegistry.Citizenship = ValidationHelper.GetString(item.GetValue("Citizenship"), "");
                        personsRegistry.CitizenshipName = ValidationHelper.GetString(item.GetValue("Citizenship"), "");
                        personsRegistry.TypeofIdentification = ValidationHelper.GetString(item.GetValue("TypeofIdentification"), "");
                        personsRegistry.TypeofIdentificationName = ValidationHelper.GetString(item.GetValue("TypeofIdentification"), "");
                        personsRegistry.IdentificationNumber = ValidationHelper.GetString(item.GetValue("IdentificationNumber"), "").ToUpper();
                        personsRegistry.IssuingCountry = ValidationHelper.GetString(item.GetValue("IssuingCountry"), "");
                        personsRegistry.IssuingCountryName = ValidationHelper.GetString(item.GetValue("IssuingCountry"), "");
                        personsRegistry.IssueDate = ValidationHelper.GetDateTime(item.GetValue("IssueDate"), default(DateTime));
                        personsRegistry.ExpiryDate = ValidationHelper.GetDateTime(item.GetValue("ExpiryDate"), default(DateTime));
                        personsRegistry.ConsentforMarketingPurposes = ValidationHelper.GetString(item.GetValue("ConsentforMarketingPurposes"), "");
                        personsRegistry.PreferredCommunicationLanguage = ValidationHelper.GetString(item.GetValue("PreferredCommunicationLanguage"), "");
                        personsRegistry.PreferredCommunicationLanguageName = ValidationHelper.GetString(item.GetValue("PreferredCommunicationLanguage"), "");
                        personsRegistry.WorkTelNoCountryCode = ValidationHelper.GetString(item.GetValue("WorkTelNoCountryCode"), "");
                        personsRegistry.WorkTelNoNumber = ValidationHelper.GetString(item.GetValue("WorkTelNoNumber"), "");
                        personsRegistry.EmailAddress = ValidationHelper.GetString(item.GetValue("EmailAddress"), "");
                        personsRegistry.FaxNoCountryCode = ValidationHelper.GetString(item.GetValue("FaxNoCountryCode"), "");
                        personsRegistry.FaxNoFaxNumber = ValidationHelper.GetString(item.GetValue("FaxNoFaxNumber"), "");
                        personsRegistry.MobileTelNoCountryCode = ValidationHelper.GetString(item.GetValue("MobileTelNoCountryCode"), "");
                        personsRegistry.MobileTelNoNumber = ValidationHelper.GetString(item.GetValue("MobileTelNoNumber"), "");
                        personsRegistry.HomeTelNoCountryCode = ValidationHelper.GetString(item.GetValue("HomeTelNoCountryCode"), "");
                        personsRegistry.HomeTelNoNumber = ValidationHelper.GetString(item.GetValue("HomeTelNoNumber"), "");
                        personsRegistry.NodeAliaspath = ValidationHelper.GetString(item.NodeAliasPath, "");
                        personsRegistry.CreatedDate = ValidationHelper.GetString(Convert.ToDateTime(item.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss"), "");
                        personsRegistry.ModyfiedDate = ValidationHelper.GetString(Convert.ToDateTime(item.DocumentModifiedWhen).ToString("dd/MM/yyyy HH:mm:ss"), "");
                        personsRegistries.Add(personsRegistry);

                    }
                    foreach (var cItem in companyRegistry)
                    {
                        Eurobank.Models.Registries.PersonsRegistry personsRegistry = new Eurobank.Models.Registries.PersonsRegistry();
                        rowid = rowid + 1;
                        personsRegistry.RowID = rowid;
                        personsRegistry.NodeID = ValidationHelper.GetString(cItem.GetValue("CompanyRegistryID"), "");
                        personsRegistry.NodeGUID = ValidationHelper.GetString(cItem.GetValue("NodeGUID"), "");
                        personsRegistry.ApplicationType = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_PersonType"), "");
                        personsRegistry.ApplicationTypeName = ServiceHelper.GetName(ValidationHelper.GetString(cItem.GetValue("CompanyDetails_PersonType"), ""), "/Lookups/General/APPLICATION-TYPE");
                        personsRegistry.Name = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_RegisteredName"), "").ToUpper();
                        personsRegistry.CreatedDate = ValidationHelper.GetString(Convert.ToDateTime(cItem.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss"), "");
                        personsRegistry.ModyfiedDate = ValidationHelper.GetString(Convert.ToDateTime(cItem.DocumentModifiedWhen).ToString("dd/MM/yyyy HH:mm:ss"), "");

                        personsRegistry.RegisteredName = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_RegisteredName"), "");
                        personsRegistry.TradingName = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_TradingName"), "");
                        personsRegistry.EntityType = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_EntityType"), "");
                        string Country = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_CountryofIncorporation"), "");
                        int countryID = 0;
                        if (int.TryParse(Country, out countryID))
                        {
                            personsRegistry.CountryofIncorporation = ServiceHelper.GetCountryNameById(countryID);
                        }
                        else
                        {
                            personsRegistry.CountryofIncorporation = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_CountryofIncorporation"), "");
                        }

                        personsRegistry.RegistrationNumber = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_RegistrationNumber"), "");
                        personsRegistry.DateofIncorporation = ValidationHelper.GetDateTime(cItem.GetValue("CompanyDetails_DateofIncorporation"), default(DateTime));
                        personsRegistry.ListingStatus = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_ListingStatus"), "");
                        personsRegistry.CorporationSharesIssuedToTheBearer = ValidationHelper.GetBoolean(cItem.GetValue("CompanyDetails_CorporationSharesIssuedToTheBearer"), false);
                        personsRegistry.IstheEntitylocatedandoperatesanofficeinCyprus = ValidationHelper.GetBoolean(cItem.GetValue("CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus"), false);
                        personsRegistry.SharesIssuedToTheBearerName = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_CorporationSharesIssuedToTheBearer"), "");
                        personsRegistry.IsOfficeinCyprusName = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus"), "");
                        personsRegistry.ContactDetailsLegal_PreferredMailingAddress = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_PreferredMailingAddress"), "");
                        personsRegistry.ContactDetailsLegal_EmailAddressForSendingAlerts = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_EmailAddressForSendingAlerts"), "");
                        personsRegistry.ContactDetailsLegal_PreferredCommunicationLanguage = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_PreferredCommunicationLanguage"), "");

                        personsRegistry.IdentificationNumber = personsRegistry.RegistrationNumber?.ToUpper();
                        personsRegistries.Add(personsRegistry);
                    }

                }
            }
            personsRegistries = personsRegistries.OrderByDescending(x => DateTime.ParseExact(x.CreatedDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).ToList();

            if (request != null && personsRegistries != null && personsRegistries.Count > 0 && request.Sorts != null && request.Sorts.Count > 0 && request.Sorts.Any(t => t.Member == "CreatedDate" || t.Member == "ModyfiedDate"))
            {
                var sort = request.Sorts.FirstOrDefault(t => t.Member == "CreatedDate" || t.Member == "ModyfiedDate");
                if (sort.Member == "CreatedDate")
                {
                    if (sort.SortDirection == Kendo.Mvc.ListSortDirection.Ascending)
                    {
                        personsRegistries = personsRegistries.OrderBy(x => DateTime.ParseExact(x.CreatedDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).ToList();
                    }
                    else
                    {
                        personsRegistries = personsRegistries.OrderByDescending(x => DateTime.ParseExact(x.CreatedDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).ToList();
                    }
                }
                else
                {
                    if (sort.SortDirection == Kendo.Mvc.ListSortDirection.Ascending)
                    {
                        personsRegistries = personsRegistries.OrderBy(x => DateTime.ParseExact(x.ModyfiedDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).ToList();
                    }
                    else
                    {
                        personsRegistries = personsRegistries.OrderByDescending(x => DateTime.ParseExact(x.ModyfiedDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).ToList();
                    }
                }
                request.Sorts = null;
            }

            return Json(personsRegistries.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult RegistryPopup_Create([DataSourceRequest] DataSourceRequest request, Eurobank.Models.Registries.PersonsRegistry personsRegistry)
        {
            string PersonType = ValidationHelper.GetString(ServiceHelper.GetEntityType(personsRegistry.ApplicationType, Constants.APPLICATION_TYPE), "");
            if (personsRegistry != null)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = RegistryValidation.ValidatePersonsRegistry(personsRegistry, PersonType);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (personsRegistry != null && ModelState.IsValid)
            {
                //PersonsRegistryCreate(personsRegistry);
                //personsRegistry.AddressTypeName = GetAddressName(personsRegistry.AddressType);
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                UserModel userModel = UserProcess.GetUser(User.Identity.Name);

                if (userModel != null && string.Equals(userModel.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase) && userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null)
                {
                    var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);
                    if (UserRegistry != null)
                    {
                        if (PersonType == "INDIVIDUAL")
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
                            personsRegistryAdd.SetValue("FirstName", personsRegistry.FirstName);
                            personsRegistryAdd.SetValue("LastName", personsRegistry.LastName);
                            personsRegistryAdd.SetValue("FathersName", personsRegistry.FatherName);
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
                            personsRegistryAdd.SetValue("WorkTelNoCountryCode", personsRegistry.WorkTelNoCountryCode);
                            personsRegistryAdd.SetValue("WorkTelNoNumber", personsRegistry.WorkTelNoNumber);
                            personsRegistryAdd.SetValue("MobileTelNoCountryCode", personsRegistry.MobileTelNoCountryCode);
                            personsRegistryAdd.SetValue("MobileTelNoNumber", personsRegistry.MobileTelNoNumber);
                            personsRegistryAdd.SetValue("HomeTelNoCountryCode", personsRegistry.HomeTelNoCountryCode);
                            personsRegistryAdd.SetValue("HomeTelNoNumber", personsRegistry.HomeTelNoNumber);
                            personsRegistryAdd.SetValue("FaxNoCountryCode", personsRegistry.FaxNoCountryCode);
                            personsRegistryAdd.SetValue("FaxNoFaxNumber", personsRegistry.FaxNoFaxNumber);
                            personsRegistryAdd.SetValue("EmailAddress", personsRegistry.EmailAddress);
                            personsRegistryAdd.SetValue("ConsentforMarketingPurposes", personsRegistry.ConsentforMarketingPurposes);
                            personsRegistryAdd.SetValue("PreferredCommunicationLanguage", personsRegistry.PreferredCommunicationLanguage);
                            personsRegistryAdd.SetValue("UserOrganization", userModel.IntroducerUser.Introducer.NodeGUID);
                            personsRegistryAdd.Insert(personsfoldernode_parent);
                            personsRegistry.CreatedDate = Convert.ToDateTime(personsRegistryAdd.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                            personsRegistry.ModyfiedDate = Convert.ToDateTime(personsRegistryAdd.DocumentModifiedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                            personsRegistry.Name = personsRegistry.FirstName + " " + personsRegistry.LastName;
                            personsRegistry.IdentificationNumber = personsRegistry.IdentificationNumber;
                        }
                        else
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
                            personsRegistryAdd.DocumentName = personsRegistry.RegisteredName;
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
                            personsRegistryAdd.SetValue("CompanyDetails_PreferredMailingAddress", personsRegistry.ContactDetailsLegal_PreferredMailingAddress);
                            personsRegistryAdd.SetValue("CompanyDetails_EmailAddressForSendingAlerts", personsRegistry.ContactDetailsLegal_EmailAddressForSendingAlerts);
                            personsRegistryAdd.SetValue("CompanyDetails_PreferredCommunicationLanguage", personsRegistry.ContactDetailsLegal_PreferredCommunicationLanguage);
                            personsRegistryAdd.Insert(personsfoldernode_parent);
                            personsRegistry.CreatedDate = Convert.ToDateTime(personsRegistryAdd.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                            personsRegistry.ModyfiedDate = Convert.ToDateTime(personsRegistryAdd.DocumentModifiedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                            personsRegistry.Name = personsRegistry.RegisteredName;
                            personsRegistry.IdentificationNumber = personsRegistry.RegistrationNumber;
                        }
                    }
                    else
                    {
                        if (PersonType == "INDIVIDUAL")
                        {
                            CMS.DocumentEngine.TreeNode personsRegistryFolder = tree.SelectNodes()
                       .Path("/Registries")
                       .Published(false)
                       .OnCurrentSite()
                       .FirstOrDefault();
                            TreeNode personsRegistryUser = TreeNode.New("Eurobank.PersonsRegistryUser", tree);
                            UserInfo user = UserInfoProvider.GetUserInfo(User.Identity.Name);
                            personsRegistryUser.DocumentName = userModel.IntroducerUser.Introducer.DocumentName;
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
                            personsRegistryAdd.SetValue("FirstName", personsRegistry.FirstName);
                            personsRegistryAdd.SetValue("LastName", personsRegistry.LastName);
                            personsRegistryAdd.SetValue("FathersName", personsRegistry.FatherName);
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
                            personsRegistryAdd.SetValue("WorkTelNoCountryCode", personsRegistry.WorkTelNoCountryCode);
                            personsRegistryAdd.SetValue("WorkTelNoNumber", personsRegistry.WorkTelNoNumber);
                            personsRegistryAdd.SetValue("MobileTelNoCountryCode", personsRegistry.MobileTelNoCountryCode);
                            personsRegistryAdd.SetValue("MobileTelNoNumber", personsRegistry.MobileTelNoNumber);
                            personsRegistryAdd.SetValue("FaxNoCountryCode", personsRegistry.FaxNoCountryCode);
                            personsRegistryAdd.SetValue("FaxNoFaxNumber", personsRegistry.FaxNoFaxNumber);
                            personsRegistryAdd.SetValue("HomeTelNoCountryCode", personsRegistry.HomeTelNoCountryCode);
                            personsRegistryAdd.SetValue("HomeTelNoNumber", personsRegistry.HomeTelNoNumber);
                            personsRegistryAdd.SetValue("EmailAddress", personsRegistry.EmailAddress);
                            personsRegistryAdd.SetValue("ConsentforMarketingPurposes", personsRegistry.ConsentforMarketingPurposes);
                            personsRegistryAdd.SetValue("PreferredCommunicationLanguage", personsRegistry.PreferredCommunicationLanguage);
                            personsRegistryAdd.SetValue("UserOrganization", userModel.IntroducerUser.Introducer.NodeGUID);
                            personsRegistryAdd.Insert(Personsfoldernode);
                            personsRegistry.CreatedDate = Convert.ToDateTime(personsRegistryAdd.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                            personsRegistry.ModyfiedDate = Convert.ToDateTime(personsRegistryAdd.DocumentModifiedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                            personsRegistry.Name = personsRegistry.FirstName + " " + personsRegistry.LastName;
                        }
                        else
                        {
                            CMS.DocumentEngine.TreeNode personsRegistryFolder = tree.SelectNodes()
                        .Path("/Registries")
                        .Published(false)
                        .OnCurrentSite()
                        .FirstOrDefault();
                            TreeNode personsRegistryUser = TreeNode.New("Eurobank.PersonsRegistryUser", tree);
                            UserInfo user = UserInfoProvider.GetUserInfo(User.Identity.Name);
                            personsRegistryUser.DocumentName = userModel.IntroducerUser.Introducer.DocumentName;
                            personsRegistryUser.DocumentCulture = "en-US";
                            personsRegistryUser.SetValue("UserID", user.UserID);
                            personsRegistryUser.Insert(personsRegistryFolder);
                            CMS.DocumentEngine.TreeNode Personsfoldernode = CMS.DocumentEngine.TreeNode.New("CMS.Folder", tree);
                            Personsfoldernode.DocumentName = "Persons Registry";
                            Personsfoldernode.DocumentCulture = "en-US";
                            Personsfoldernode.Insert(personsRegistryUser);
                            CMS.DocumentEngine.TreeNode personsRegistryAdd = CMS.DocumentEngine.TreeNode.New("Eurobank.CompanyRegistry", tree);
                            personsRegistryAdd.DocumentName = personsRegistry.RegisteredName;
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
                            personsRegistryAdd.SetValue("CompanyDetails_PreferredMailingAddress", personsRegistry.ContactDetailsLegal_PreferredMailingAddress);
                            personsRegistryAdd.SetValue("CompanyDetails_EmailAddressForSendingAlerts", personsRegistry.ContactDetailsLegal_EmailAddressForSendingAlerts);
                            personsRegistryAdd.SetValue("CompanyDetails_PreferredCommunicationLanguage", personsRegistry.ContactDetailsLegal_PreferredCommunicationLanguage);
                            personsRegistryAdd.Insert(Personsfoldernode);
                            personsRegistry.CreatedDate = Convert.ToDateTime(personsRegistryAdd.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                            personsRegistry.ModyfiedDate = Convert.ToDateTime(personsRegistryAdd.DocumentModifiedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                            personsRegistry.Name = personsRegistry.FirstName + " " + personsRegistry.LastName;
                        }
                    }
                }


            }
            if (!string.IsNullOrEmpty(personsRegistry.ApplicationType))
            {
                personsRegistry.ApplicationTypeName = ServiceHelper.GetName(personsRegistry.ApplicationType, "/Lookups/General/APPLICATION-TYPE");
            }



            return Json(new[] { personsRegistry }.ToDataSourceResult(request, ModelState));
        }

        private void PersonsRegistryCreate(Models.Registries.PersonsRegistry personsRegistry)
        {
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            var UserRegistry = registriesRepository.GetRegistryUserByName(User.Identity.Name);
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
                personsRegistryAdd.SetValue("FirstName", personsRegistry.FirstName);
                personsRegistryAdd.SetValue("LastName", personsRegistry.LastName);
                personsRegistryAdd.SetValue("FathersName", personsRegistry.FatherName);
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
                personsRegistryAdd.SetValue("MobileTelNoCountryCode", personsRegistry.MobileTelNoNumber.Split(' ')[0]);
                personsRegistryAdd.SetValue("FaxNoFaxNumber", personsRegistry.FaxNoFaxNumber);
                personsRegistryAdd.SetValue("EmailAddress", personsRegistry.EmailAddress);
                personsRegistryAdd.SetValue("ConsentforMarketingPurposes", personsRegistry.ConsentforMarketingPurposes);
                personsRegistryAdd.SetValue("PreferredCommunicationLanguage", personsRegistry.PreferredCommunicationLanguage);
                personsRegistryAdd.Insert(personsfoldernode_parent);

            }
            else
            {
                CMS.DocumentEngine.TreeNode personsRegistryFolder = tree.SelectNodes()
               .Path("/Registries")
               .Published(false)
               .OnCurrentSite()
               .FirstOrDefault();
                TreeNode personsRegistryUser = TreeNode.New("Eurobank.PersonsRegistryUser", tree);
                UserInfo user = UserInfoProvider.GetUserInfo(User.Identity.Name);
                personsRegistryUser.DocumentName = user.UserName;
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
                personsRegistryAdd.SetValue("FirstName", personsRegistry.FirstName);
                personsRegistryAdd.SetValue("LastName", personsRegistry.LastName);
                personsRegistryAdd.SetValue("FathersName", personsRegistry.FatherName);
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
                personsRegistryAdd.SetValue("MobileTelNoCountryCode", personsRegistry.MobileTelNoNumber.Split(' ')[0]);
                personsRegistryAdd.SetValue("FaxNoFaxNumber", personsRegistry.FaxNoFaxNumber);
                personsRegistryAdd.SetValue("EmailAddress", personsRegistry.EmailAddress);
                personsRegistryAdd.SetValue("ConsentforMarketingPurposes", personsRegistry.ConsentforMarketingPurposes);
                personsRegistryAdd.SetValue("PreferredCommunicationLanguage", personsRegistry.PreferredCommunicationLanguage);
                personsRegistryAdd.Insert(Personsfoldernode);

            }

        }

        [HttpPost]
        public ActionResult RegistryPopup_Update([DataSourceRequest] DataSourceRequest request, Eurobank.Models.Registries.PersonsRegistry personsRegistry)
        {
            string PersonType = ValidationHelper.GetString(ServiceHelper.GetEntityType(personsRegistry.ApplicationType, Constants.APPLICATION_TYPE), "");
            if (personsRegistry != null)
            {
                ValidationResultModel validationResultModel = new ValidationResultModel();
                validationResultModel = RegistryValidation.ValidatePersonsRegistry(personsRegistry, PersonType);
                if (validationResultModel != null)
                {
                    foreach (var item in validationResultModel.Errors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                }
            }
            if (personsRegistry != null && ModelState.IsValid)
            {
                if (PersonType == "INDIVIDUAL")
                {
                    var UserRegistry = registriesRepository.GetRegistryUserByNodeGUID(personsRegistry.NodeGUID);
                    if (UserRegistry != null)
                    {
                        TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                        var manager = VersionManager.GetInstance(tree);
                        //if(!UserRegistry.IsCheckedOut)
                        //{
                        //	manager.CheckOut(UserRegistry);
                        //}
                        UserRegistry.DocumentName = personsRegistry.FirstName + " " + personsRegistry.LastName;
                        UserRegistry.SetValue("Name", personsRegistry.FirstName + " " + personsRegistry.LastName);
                        UserRegistry.SetValue("ApplicationType", personsRegistry.ApplicationType);
                        UserRegistry.SetValue("FirstName", personsRegistry.FirstName);
                        UserRegistry.SetValue("LastName", personsRegistry.LastName);
                        UserRegistry.SetValue("FathersName", personsRegistry.FatherName);
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
                        UserRegistry.SetValue("WorkTelNoCountryCode", personsRegistry.WorkTelNoCountryCode);
                        UserRegistry.SetValue("WorkTelNoNumber", personsRegistry.WorkTelNoNumber);
                        UserRegistry.SetValue("MobileTelNoCountryCode", personsRegistry.MobileTelNoCountryCode);
                        UserRegistry.SetValue("MobileTelNoNumber", personsRegistry.MobileTelNoNumber);
                        UserRegistry.SetValue("FaxNoCountryCode", personsRegistry.FaxNoCountryCode);
                        UserRegistry.SetValue("FaxNoFaxNumber", personsRegistry.FaxNoFaxNumber);
                        UserRegistry.SetValue("HomeTelNoCountryCode", personsRegistry.HomeTelNoCountryCode);
                        UserRegistry.SetValue("HomeTelNoNumber", personsRegistry.HomeTelNoNumber);
                        UserRegistry.SetValue("EmailAddress", personsRegistry.EmailAddress);
                        UserRegistry.SetValue("ConsentforMarketingPurposes", personsRegistry.ConsentforMarketingPurposes);
                        UserRegistry.SetValue("PreferredCommunicationLanguage", personsRegistry.PreferredCommunicationLanguage);
                        UserRegistry.CreateNewVersion();
                        UserRegistry.Update();
                        //if(UserRegistry.IsCheckedOut)
                        //{
                        //	manager.CheckIn(UserRegistry, null, null);
                        //}
                        personsRegistry.CreatedDate = Convert.ToDateTime(UserRegistry.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                        personsRegistry.ModyfiedDate = Convert.ToDateTime(UserRegistry.DocumentModifiedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                        personsRegistry.Name = personsRegistry.FirstName + " " + personsRegistry.LastName;
                    }
                }
                else
                {
                    var UserRegistry = registriesRepository.GetCompanyRegistryUserByNodeGUID(personsRegistry.NodeGUID);
                    if (UserRegistry != null)
                    {
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
                        personsRegistry.CreatedDate = Convert.ToDateTime(UserRegistry.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                        personsRegistry.ModyfiedDate = Convert.ToDateTime(UserRegistry.DocumentModifiedWhen).ToString("dd/MM/yyyy HH:mm:ss");
                        personsRegistry.Name = personsRegistry.RegisteredName;
                    }
                }


            }
            if (!string.IsNullOrEmpty(personsRegistry.ApplicationType))
            {
                personsRegistry.ApplicationTypeName = ServiceHelper.GetName(personsRegistry.ApplicationType, "/Lookups/General/APPLICATION-TYPE");

                personsRegistry.IdentificationNumber = personsRegistry.IdentificationNumber;
            }
            //personsRegistry.AddressTypeName = GetAddressName(personsRegistry.AddressType);
            return Json(new[] { personsRegistry }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult RegistryPopup_Destroy([DataSourceRequest] DataSourceRequest request, Eurobank.Models.Registries.PersonsRegistry personsRegistry)
        {
            if (personsRegistry != null)
            {
                //registriesRepository.GetRegistryUserByNodeGUID(personsRegistry.NodeGUID).DeleteAllCultures();
                if (personsRegistry.ApplicationTypeName == "LEGAL ENTITY")
                {
                    var UserRegistry = registriesRepository.GetCompanyRegistryUserByNodeGUID(personsRegistry.NodeGUID);
                    UserRegistry.DocumentPublishTo = DateTime.Now;
                    UserRegistry.Update();
                }
                else if (personsRegistry.ApplicationTypeName == "INDIVIDUAL")
                {
                    var UserRegistry = registriesRepository.GetRegistryUserByNodeGUID(personsRegistry.NodeGUID);
                    UserRegistry.DocumentPublishTo = DateTime.Now;
                    UserRegistry.Update();
                }

            }

            return Json(new[] { personsRegistry }.ToDataSourceResult(request, ModelState));
        }

        public IActionResult PersonRegistry_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<PersonsRegistrySearchModel> personsRegistrySearchModels = new List<PersonsRegistrySearchModel>();
            List<Eurobank.Models.Registries.PersonsRegistry> personsRegistries = new List<Eurobank.Models.Registries.PersonsRegistry>();

            UserModel userModel = UserProcess.GetUser(User.Identity.Name);

            if (userModel != null && string.Equals(userModel.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase) && userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null)
            {
                var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);
                if (UserRegistry != null)
                {
                    int rowid = 0;
                    var registry = registriesRepository.GetRegistries(UserRegistry.NodeAliasPath);
                    foreach (var item in registry)
                    {
                        rowid = rowid + 1;
                        Eurobank.Models.Registries.PersonsRegistry personsRegistry = new Eurobank.Models.Registries.PersonsRegistry();
                        personsRegistry.RowID = rowid;
                        personsRegistry.NodeID = ValidationHelper.GetString(item.GetValue("PersonsRegistryID"), "");
                        personsRegistry.NodeGUID = ValidationHelper.GetString(item.GetValue("NodeGUID"), "");
                        personsRegistry.ApplicationType = ValidationHelper.GetString(item.GetValue("ApplicationType"), "");
                        personsRegistry.ApplicationTypeName = ServiceHelper.GetName(ValidationHelper.GetString(item.GetValue("ApplicationType"), ""), "/Lookups/General/APPLICATION-TYPE");
                        if (string.Equals(personsRegistry.ApplicationTypeName, "JOINT INDIVIDUAL", StringComparison.OrdinalIgnoreCase))
                        {
                            var lstEntityType = ServiceHelper.GetApplicationType();
                             string individualEntity = lstEntityType.Where(x => x.Text == "INDIVIDUAL").FirstOrDefault().Value.ToString();
                            personsRegistry.ApplicationTypeName = "INDIVIDUAL";
                            personsRegistry.ApplicationType = individualEntity;
                        }
                        personsRegistry.Name = ValidationHelper.GetString(item.GetValue("Name"), "");
                        personsRegistry.Title = ValidationHelper.GetString(item.GetValue("Title"), "");
                        personsRegistry.TitleName = ValidationHelper.GetString(item.GetValue("Title"), "");
                        personsRegistry.FirstName = ValidationHelper.GetString(item.GetValue("FirstName"), "");
                        personsRegistry.LastName = ValidationHelper.GetString(item.GetValue("LastName"), "");
                        personsRegistry.FatherName = ValidationHelper.GetString(item.GetValue("FathersName"), "");
                        personsRegistry.Gender = ValidationHelper.GetString(item.GetValue("Gender"), "");
                        personsRegistry.GenderName = ValidationHelper.GetString(item.GetValue("Gender"), "");
                        personsRegistry.DateofBirth = ValidationHelper.GetDateTime(item.GetValue("DateofBirth"), default(DateTime));// ValidationHelper.GetDateTime(Convert.ToDateTime(item.GetValue("DateofBirth")), default(DateTime));
                        personsRegistry.PlaceofBirth = ValidationHelper.GetString(item.GetValue("PlaceofBirth"), "");
                        personsRegistry.CountryofBirth = ValidationHelper.GetString(item.GetValue("CountryofBirth"), "");
                        personsRegistry.CountryofBirthName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(item.GetValue("CountryofBirth"), 0));
                        personsRegistry.EducationLevel = ValidationHelper.GetString(item.GetValue("EducationLevel"), "");
                        personsRegistry.EducationLevelName = ValidationHelper.GetString(item.GetValue("EducationLevel"), "");
                        personsRegistry.Citizenship = ValidationHelper.GetString(item.GetValue("Citizenship"), "");
                        personsRegistry.CitizenshipName = ServiceHelper.GetCountryNameById(ValidationHelper.GetInteger(item.GetValue("Citizenship"), 0));
                        personsRegistry.TypeofIdentification = ValidationHelper.GetString(item.GetValue("TypeofIdentification"), "");
                        personsRegistry.TypeofIdentificationName = ValidationHelper.GetString(item.GetValue("TypeofIdentification"), "");
                        personsRegistry.IdentificationNumber = ValidationHelper.GetString(item.GetValue("IdentificationNumber"), "");
                        personsRegistry.IssuingCountry = ValidationHelper.GetString(item.GetValue("IssuingCountry"), "");
                        personsRegistry.IssuingCountryName = ValidationHelper.GetString(item.GetValue("IssuingCountry"), "");
                        personsRegistry.IssueDate = ValidationHelper.GetDate(Convert.ToDateTime(item.GetValue("IssueDate")), default(DateTime));
                        personsRegistry.ExpiryDate = ValidationHelper.GetDate(item.GetValue("ExpiryDate"), default(DateTime));
                        personsRegistry.ConsentforMarketingPurposes = ValidationHelper.GetString(item.GetValue("ConsentforMarketingPurposes"), "");
                        personsRegistry.PreferredCommunicationLanguage = ValidationHelper.GetString(item.GetValue("PreferredCommunicationLanguage"), "");
                        personsRegistry.PreferredCommunicationLanguageName = ValidationHelper.GetString(item.GetValue("PreferredCommunicationLanguage"), "");
                        personsRegistry.WorkTelNoNumber = ValidationHelper.GetString(item.GetValue("WorkTelNoNumber"), "");
                        personsRegistry.EmailAddress = ValidationHelper.GetString(item.GetValue("EmailAddress"), "");
                        personsRegistry.FaxNoFaxNumber = ValidationHelper.GetString(item.GetValue("FaxNoFaxNumber"), "");
                        personsRegistry.MobileTelNoNumber = ValidationHelper.GetString(item.GetValue("MobileTelNoNumber"), "");
                        personsRegistry.HomeTelNoNumber = ValidationHelper.GetString(item.GetValue("HomeTelNoNumber"), "");

                        personsRegistry.FaxNoCountryCode = ValidationHelper.GetString(item.GetValue("FaxNoCountryCode"), "");
                        personsRegistry.MobileTelNoCountryCode = ValidationHelper.GetString(item.GetValue("MobileTelNoCountryCode"), "");
                        personsRegistry.HomeTelNoCountryCode = ValidationHelper.GetString(item.GetValue("HomeTelNoCountryCode"), "");
                        personsRegistry.WorkTelNoCountryCode = ValidationHelper.GetString(item.GetValue("WorkTelNoCountryCode"), "");

                        personsRegistry.NodeAliaspath = ValidationHelper.GetString(item.NodeAliasPath, "");
                        personsRegistry.CreatedDate = ValidationHelper.GetString(Convert.ToDateTime(item.DocumentCreatedWhen).ToString("MM/dd/yyyy HH:mm:ss"), "");
                        personsRegistry.ModyfiedDate = ValidationHelper.GetString(Convert.ToDateTime(item.DocumentModifiedWhen).ToString("MM/dd/yyyy HH:mm:ss"), "");
                        //personsRegistries.Add(personsRegistry);
                        personsRegistrySearchModels.Add(PersonalDetailsProcess.BindPersonRegistrySearchModel(personsRegistry));
                    }
                }
            }



            return Json(personsRegistrySearchModels.ToDataSourceResult(request));
        }

        public IActionResult PersonRegistryLegal_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<PersonRegistyLegalSearchModel> personRegistyLegalSearchModel = new List<PersonRegistyLegalSearchModel>();
            List<Eurobank.Models.Registries.PersonsRegistry> personsRegistries = new List<Eurobank.Models.Registries.PersonsRegistry>();

            UserModel userModel = UserProcess.GetUser(User.Identity.Name);

            if (userModel != null && string.Equals(userModel.UserType, ApplicationUserType.INTRODUCER.ToString(), StringComparison.OrdinalIgnoreCase) && userModel.IntroducerUser != null && userModel.IntroducerUser.Introducer != null)
            {
                var UserRegistry = registriesRepository.GetRegistryUserByName(userModel.IntroducerUser.Introducer.DocumentName);
                if (UserRegistry != null)
                {
                    int rowid = 0;
                    var companyRegistry = registriesRepository.GetCompanyRegistries(UserRegistry.NodeAliasPath);
                    foreach (var cItem in companyRegistry)
                    {
                        Eurobank.Models.Registries.PersonsRegistry personsRegistry = new Eurobank.Models.Registries.PersonsRegistry();
                        rowid = rowid + 1;
                        personsRegistry.RowID = rowid;
                        personsRegistry.NodeID = ValidationHelper.GetString(cItem.GetValue("CompanyRegistryID"), "");
                        personsRegistry.NodeGUID = ValidationHelper.GetString(cItem.GetValue("NodeGUID"), "");
                        personsRegistry.ApplicationType = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_PersonType"), "");
                        personsRegistry.ApplicationTypeName = ServiceHelper.GetName(ValidationHelper.GetString(cItem.GetValue("CompanyDetails_PersonType"), ""), "/Lookups/General/APPLICATION-TYPE");
                        personsRegistry.Name = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_RegisteredName"), "");
                        personsRegistry.CreatedDate = ValidationHelper.GetString(Convert.ToDateTime(cItem.DocumentCreatedWhen).ToString("dd/MM/yyyy HH:mm:ss"), "");
                        personsRegistry.ModyfiedDate = ValidationHelper.GetString(Convert.ToDateTime(cItem.DocumentModifiedWhen).ToString("dd/MM/yyyy HH:mm:ss"), "");

                        personsRegistry.RegisteredName = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_RegisteredName"), "");
                        personsRegistry.TradingName = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_TradingName"), "");
                        personsRegistry.EntityType = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_EntityType"), "");
                        string Country = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_CountryofIncorporation"), "");
                        int countryID = 0;
                        if (int.TryParse(Country, out countryID))
                        {
                            personsRegistry.CountryofIncorporation = ServiceHelper.GetCountryNameById(countryID);
                        }
                        else
                        {
                            personsRegistry.CountryofIncorporation = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_CountryofIncorporation"), "");
                        }
                        personsRegistry.RegistrationNumber = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_RegistrationNumber"), "");
                        personsRegistry.DateofIncorporation = ValidationHelper.GetDateTime(cItem.GetValue("CompanyDetails_DateofIncorporation"), default(DateTime));
                        personsRegistry.ListingStatus = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_ListingStatus"), "");
                        personsRegistry.CorporationSharesIssuedToTheBearer = ValidationHelper.GetBoolean(cItem.GetValue("CompanyDetails_CorporationSharesIssuedToTheBearer"), false);
                        personsRegistry.IstheEntitylocatedandoperatesanofficeinCyprus = ValidationHelper.GetBoolean(cItem.GetValue("CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus"), false);
                        personsRegistry.SharesIssuedToTheBearerName = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_CorporationSharesIssuedToTheBearer"), "");
                        personsRegistry.IsOfficeinCyprusName = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_IstheEntitylocatedandoperatesanofficeinCyprus"), "");
                        personsRegistry.ContactDetailsLegal_PreferredMailingAddress = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_PreferredMailingAddress"), "");
                        personsRegistry.ContactDetailsLegal_EmailAddressForSendingAlerts = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_EmailAddressForSendingAlerts"), "");
                        personsRegistry.ContactDetailsLegal_PreferredCommunicationLanguage = ValidationHelper.GetString(cItem.GetValue("CompanyDetails_PreferredCommunicationLanguage"), "");
                        personRegistyLegalSearchModel.Add(CompanyDetailsProcess.BindPersonRegistyLegalSearchModel(personsRegistry));

                    }
                }

            }
            return Json(personRegistyLegalSearchModel.ToDataSourceResult(request));
        }
        #endregion
        private string GetAddressName(string NodeGuid)
        {
            string addressName = string.Empty;
            var lookupAddressType = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path("/Lookups/General/Address-Type", PathTypeEnum.Children)
                    .Culture("en-us")
                    .PublishedVersion().WhereEquals("NodeGUID", ValidationHelper.GetGuid(NodeGuid, new Guid())).FirstOrDefault();
            if (lookupAddressType != null)
            {
                addressName = lookupAddressType.DocumentName;

            }

            return addressName;
        }

        private string GetCountryName(string NodeGuid)
        {
            string addressName = string.Empty;
            var countries = CountryInfoProvider.GetCountries().WhereEquals("CountryGUID", ValidationHelper.GetGuid(NodeGuid, new Guid())).FirstOrDefault();
            if (countries != null)

            {
                addressName = countries.CountryName;


            }
            return addressName;
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
        public ActionResult ImportWindowPersonsRegistry()
        {
            return PartialView();
        }
        public ActionResult ImportWindowAddressRegistry()
        {
            return PartialView();
        }
        //public ActionResult PersonsRegistryImprot(IEnumerable<IFormFile> files)
        //{
        //	if(files != null)
        //	{

        //		foreach(var file in files)
        //		{
        //			//Set file details.
        //			using(ExcelPackage package = new ExcelPackage(file.OpenReadStream()))
        //			{
        //				StringBuilder sb = new StringBuilder();
        //				ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
        //				int rowCount = worksheet.Dimension.Rows;
        //				int ColCount = worksheet.Dimension.Columns;
        //				for(int row = 1; row <= rowCount; row++)
        //				{
        //					if(row>1)
        //					{
        //						Models.Registries.PersonsRegistry personsRegistry = new Models.Registries.PersonsRegistry();
        //						//personsRegistry.AddressType =ValidationHelper.GetString( worksheet.Cells[row, 1],"");
        //						personsRegistry.Name = ValidationHelper.GetString(worksheet.Cells[row, 2], "");
        //						personsRegistry.MobileTelNoNumber = ValidationHelper.GetString(worksheet.Cells[row, 3], "");
        //						personsRegistry.EmailAddress = ValidationHelper.GetString(worksheet.Cells[row, 4], "");
        //						personsRegistry.FaxNoFaxNumber = ValidationHelper.GetString(worksheet.Cells[row, 5], "");
        //						PersonsRegistryCreate(personsRegistry);
        //					}

        //				}

        //			}

        //		}
        //	}
        //	return RedirectToAction("Index", "Registries");
        //}
        //public ActionResult AddressRegistryImprot(IEnumerable<IFormFile> files)
        //{
        //	if(files != null)
        //	{

        //		foreach(var file in files)
        //		{
        //			//Set file details.
        //			using(ExcelPackage package = new ExcelPackage(file.OpenReadStream()))
        //			{
        //				StringBuilder sb = new StringBuilder();
        //				ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
        //				int rowCount = worksheet.Dimension.Rows;
        //				int ColCount = worksheet.Dimension.Columns;
        //				for(int row = 1; row <= rowCount; row++)
        //				{
        //					if(row > 1)
        //					{
        //					var d=	worksheet.Cells[row, 1];
        //					}
        //					//for(int col = 1; col <= ColCount; col++)
        //					//{
        //					//	if(bHeaderRow)
        //					//	{
        //					//		sb.Append(worksheet.Cells[row, col].Value.ToString() + "\t");
        //					//	}
        //					//	else
        //					//	{
        //					//		string id = worksheet.Cells[row, col].Value.ToString();
        //					//	}
        //					//}
        //				}

        //			}
        //			//if(fileExtension == Constants.xls || fileExtension == Constants.xlsx)
        //			//{
        //			//	//Save the uploaded file to the application folder.
        //			//	string savedExcelFiles = Constants.UploadedFolder + fileName;
        //			//	f.SaveAs(Server.MapPath(savedExcelFiles));

        //			//	//Read Data From ExcelFiles.
        //			//	ReadDataFromExcelFiles(savedExcelFiles);
        //			//}
        //			//else
        //			//{
        //			//	//TODO: Send Alert to the users file not supported.
        //			//}
        //		}
        //	}
        //	return RedirectToAction("Index", "Registries");
        //}
        public JsonResult Country_Code_prefix_Read()
        {
            var result = ServiceHelper.GetCountryCodePrefix();
            return Json(result);
        }

    }
}
