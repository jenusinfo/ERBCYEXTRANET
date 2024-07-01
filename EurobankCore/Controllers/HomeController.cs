using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Io;
using CMS.CustomTables;
using CMS.CustomTables.Types.Eurobank;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.SiteProvider;
using Eurobank.Controllers;
using Eurobank.Helpers.CustomHandler;
using Eurobank.Helpers.DataAnnotation;
using Eurobank.Helpers.Generics;
using Eurobank.Helpers.Process;
using Eurobank.Models;
using Eurobank.Models.Applications;
using Eurobank.Models.FormModels;
using Eurobank.Models.Home;
using Eurobank.Models.User;
using Eurobank.Models.WhatsNew;
using Eurobank.Services;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
//using Microsoft.SqlServer.Dac.Model;

[assembly: RegisterPageRoute(Home.CLASS_NAME, typeof(HomeController))]

namespace Eurobank.Controllers
{
   [AuthorizeRoles(Role.PowerUser, Role.NormalUser)]
   [SessionAuthorization]
	//[IgnoreAntiforgeryToken]
	public class HomeController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;
        private readonly ReferenceRepository referenceRepository;
        private readonly HomeRepository homeSectionRepository;
        private readonly WhatsNewRepository whatsnewrepository;
        private readonly ApplicationsRepository applicationsRepository;

        public HomeController(IMemoryCache memoryCache,
            IPageDataContextRetriever pageDataContextRetriever,
            IPageUrlRetriever pageUrlRetriever,
            IPageAttachmentUrlRetriever attachmentUrlRetriever,
            HomeRepository homeSectionRepository,
            ReferenceRepository referenceRepository,
            WhatsNewRepository whatsnewrepository,
            ApplicationsRepository applicationsRepository)
        {
            _memoryCache = memoryCache;
            this.pageDataContextRetriever = pageDataContextRetriever;
            this.homeSectionRepository = homeSectionRepository;
            this.referenceRepository = referenceRepository;
            this.pageUrlRetriever = pageUrlRetriever;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
            this.whatsnewrepository = whatsnewrepository;
            this.applicationsRepository = applicationsRepository;
        }

        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            //var test1 = User.Identity.Name;
            //var test2 = UserProcess.GetUser(User.Identity.Name).UserInformation.UserID;
            //var test = SettingsKeyInfoProvider.GetIntValue(SiteContext.CurrentSiteName + ".Session_Timeout_key");
            //var home = pageDataContextRetriever.Retrieve<Home>().Page;
            //var homeSections = await homeSectionRepository.GetHomeSectionsAsync(home.NodeAliasPath, cancellationToken);
            //var reference = (await referenceRepository.GetReferencesAsync(home.NodeAliasPath, cancellationToken, 1)).FirstOrDefault();

            //var viewModel = new HomeIndexViewModel
            //{
            //    HomeSections = homeSections.Select(section => HomeSectionViewModel.GetViewModel(section, pageUrlRetriever, attachmentUrlRetriever)),
            //    Reference = ReferenceViewModel.GetViewModel(reference, attachmentUrlRetriever)
            //};
        List<ContactDetailViewModel> contactDetailViewModels = new List<ContactDetailViewModel>();
            string customTableClassName = "Eurobank.TempPersonalDetails";
            DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
            if(customTable != null)
            {

                // Gets the first custom table record whose value in the 'ItemName' field is equal to "SampleName"
                var addressData = CustomTableItemProvider.GetItems(customTableClassName).OrderByDescending("ItemID").ToList();
                foreach(var item in addressData)
                {
                    ContactDetailViewModel contactDetailViewModel  = new ContactDetailViewModel();
                    contactDetailViewModel.Title = GetTitleName( ValidationHelper.GetString(item.GetValue("Title"), ""));
                    contactDetailViewModel.FirstName = ValidationHelper.GetString(item.GetValue("FirstName"), "");
                    contactDetailViewModel.LastName = ValidationHelper.GetString(item.GetValue("LastName"), "");
                    contactDetailViewModel.DateofBirth = ValidationHelper.GetString(item.GetValue("DateofBirth"), "");
                    contactDetailViewModel.ApplicationID = ValidationHelper.GetString(item.GetValue("ItemID"), "");
					contactDetailViewModel.Isactive = ValidationHelper.GetBoolean(item.GetValue("Isactive"), false)==true?"Yes":"No";
                    contactDetailViewModels.Add(contactDetailViewModel);
                }
            }
            TempData["PersonalDetails"] = contactDetailViewModels;
            ContactDetailViewModel modelData = new ContactDetailViewModel();
            //UserModel user = UserProcess.GetUser(User.Identity.Name);
            //var listApplications = ApplicationExcelDownloadProcess.GetApplicationByUser(user, applicationsRepository);
            ///////////////////////////
            //HomeIndexViewModel homechartValue = new HomeIndexViewModel();


            //homechartValue.ApplicationTypeIndividualCount = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationTypeName, "INDIVIDUAL", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //homechartValue.ApplicationTypeJointCount = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationTypeName, "JOINT INDIVIDUAL", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //homechartValue.ApplicationTypeCompanyCount = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //homechartValue.ApplicationStatusCountDraft = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "DRAFT", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //homechartValue.ApplicationStatusCountCancelled = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "CANCELLED", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //homechartValue.ApplicationStatusCountCompleted = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "COMPLETED", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //homechartValue.ApplicationStatusCountPendingChecker = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING CHECKER", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //homechartValue.ApplicationStatusCountPendingCompletion = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING COMPLETION", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //homechartValue.ApplicationStatusCountPendingDocuments = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING DOCUMENTS", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //homechartValue.ApplicationStatusCountPendingExecution = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING EXECUTION", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //homechartValue.ApplicationStatusCountPendingInitiator = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING INITIATOR", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //homechartValue.ApplicationStatusCountPendingOmmissions = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING OMMISSIONS", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //homechartValue.ApplicationStatusCountPendingVerification = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING VERIFICATION", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //homechartValue.ApplicationStatusCountPendingSignatures = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING SIGNATURES", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //homechartValue.ApplicationStatusCountWithdrawn = listApplications.Count > 0 ? (listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "WITHDRAWN", StringComparison.OrdinalIgnoreCase)).Count() * 100) / listApplications.Count : 0;
            //ViewBag.HomeChartValue = homechartValue;

            //modelData.searchText = "Sampletext";
            var whatsnewimg = whatsnewrepository.GetWhatsNew("/Home/WhatsNew", 1);
            var sidebannerimg = whatsnewimg.Select(x => x.Teaser).FirstOrDefault().ToString();
            ViewBag.SideBannerHome = whatsnewimg.Select(x => x.Teaser).FirstOrDefault().ToString();
            return View(modelData);
        }
        private string GetTitleName(string NodeGuid)
        {
            string titleName = string.Empty;
            var lookupAddressType = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path("/Lookups/General/TITLES", PathTypeEnum.Children)
                    .Culture("en-us")
                    .PublishedVersion().WhereEquals("NodeGUID", ValidationHelper.GetGuid(NodeGuid, new Guid())).FirstOrDefault();
            if(lookupAddressType != null)
            {
                titleName = lookupAddressType.DocumentName;

            }

            return titleName;
        }

		[HttpPost]
        public ActionResult Index(ContactDetailViewModel model)
        {
            if(!ModelState.IsValid)
            {

                return View(model);
            }
            else
            {
                TempPersonalDetailsItem tempPersonalDetailsItem = new TempPersonalDetailsItem();
                tempPersonalDetailsItem.Title = model.Title;
                tempPersonalDetailsItem.FirstName = model.FirstName;
                tempPersonalDetailsItem.LastName = model.LastName;
                tempPersonalDetailsItem.FatherName = model.FatherName;
                tempPersonalDetailsItem.Gender = model.Gender;
                tempPersonalDetailsItem.DateofBirth = model.DateofBirth;
                tempPersonalDetailsItem.Insert();
                TempData["View"] = "Index";
                return View();
            }
        }

        //public ActionResult Address_Read([DataSourceRequest] DataSourceRequest request)
        //{
        //    List<AddressViewModel> addressViewModels = new List<AddressViewModel>();
        //    AddressViewModel addressViewModel = new AddressViewModel();
        //    addressViewModel.AddressType = "Test";
        //    addressViewModel.PostalCode = "12345678";
        //    addressViewModel.City = "Kolkata";
        //    addressViewModel.Country = "India";
        //    addressViewModels.Add(addressViewModel);
        //    return Json(addressViewModels.ToDataSourceResult(request));
        //}

        public IActionResult GetApplicationByStatus()
        {
            //            UserModel user = UserProcess.GetUser(User.Identity.Name);
            //            var listApplications = ApplicationExcelDownloadProcess.GetApplicationByUser(user, applicationsRepository);

            //            var statusCounts = new Dictionary<string, int>
            //{
            //    { "DRAFT", 0 },
            //    { "CANCELLED", 0 },
            //    { "COMPLETED", 0 },
            //    { "PENDING CHECKER", 0 },
            //    { "PENDING COMPLETION", 0 },
            //    { "PENDING DOCUMENTS", 0 },
            //    { "PENDING EXECUTION", 0 },
            //    { "PENDING INITIATOR", 0 },
            //    { "PENDING OMMISSIONS", 0 },
            //    { "PENDING VERIFICATION", 0 },
            //    { "PENDING SIGNATURES", 0 },
            //    // Add other status names here
            //    // ...
            //    { "WITHDRAWN", 0 }
            //};

            //            foreach (var application in listApplications)
            //            {
            //                string status = application.ApplicationDetails_ApplicationStatusName.ToUpperInvariant();
            //                if (statusCounts.ContainsKey(status))
            //                {
            //                    statusCounts[status]++;
            //                }
            //            }

            //            var result = statusCounts.Select(kv => new HomeChartViewModel
            //            {
            //                value = listApplications.Count > 0 ? (kv.Value * 100) / listApplications.Count : 0,
            //                category = ResHelper.GetString($"Eurobank.Home.Chart.ApplicationStatus.{kv.Key.Replace(" ","")}"),
            //                color = ResHelper.GetString($"Eurobank.Home.Chart.ApplicationStatus.color.{kv.Key.Replace(" ", "")}")
            //            }).ToList();

            //            return Json(result);

            UserModel user = UserProcess.GetUser(User.Identity.Name);

            string cacheKey = user.UserInformation.UserGUID + "_GetApplicationByStatus";
            if (!_memoryCache.TryGetValue(cacheKey, out List<ApplicationDetailsModelView> listApplications))
            {
                listApplications = ApplicationExcelDownloadProcess.GetApplicationByUser(user, applicationsRepository,true);
                _memoryCache.Set(cacheKey, listApplications);
                //var cacheEntryOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Set expiration time
                //_memoryCache.Set(cacheKey, listApplications, cacheEntryOption);
            }
            // List<ApplicationDetailsModelView> listApplications = ApplicationExcelDownloadProcess.GetApplicationByUser(user, applicationsRepository);
            decimal totalCount = listApplications.Count;
            var statuses = new List<string> {
                "DRAFT",
                "CANCELLED",
                "COMPLETED",
                "PENDING CHECKER",
                "PENDING COMPLETION",
                "PENDING BANK DOCUMENTS",
                "PENDING EXECUTION",
                "PENDING INITIATOR",
                "PENDING OMMISSIONS",
                "PENDING VERIFICATION",
                "PENDING SIGNATURES",
                "WITHDRAWN"
            };

            var statusCounts = statuses.ToDictionary(
        status => status,
        status => totalCount > 0 ?
                  Math.Round((listApplications.Count(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, status, StringComparison.OrdinalIgnoreCase)) * 100.0m) / totalCount, 2)
                  : 0m
    );

            var result = statuses.Select(status => new HomeChartViewModel
            {
                value = statusCounts[status],
                category = ResHelper.GetString($"Eurobank.Home.Chart.ApplicationStatus.{status.Replace(" ", "")}"),
                color = ResHelper.GetString($"Eurobank.Home.Chart.ApplicationStatus.color.{status.Replace(" ", "")}")
            }).ToList();

            //UserModel user = UserProcess.GetUser(User.Identity.Name);
            //var listApplications = ApplicationExcelDownloadProcess.GetApplicationByUser(user, applicationsRepository);
            //decimal totalCount = listApplications.Count;
            //decimal ApplicationStatusCountDraft = totalCount > 0 ? Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "DRAFT", StringComparison.OrdinalIgnoreCase)).Count() * 100.0m) / totalCount, 2) : 0m;
            //decimal ApplicationStatusCountCancelled = totalCount > 0 ? Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "CANCELLED", StringComparison.OrdinalIgnoreCase)).Count() * 100.0m) / totalCount, 2) : 0m;
            //decimal ApplicationStatusCountCompleted = totalCount > 0 ? Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "COMPLETED", StringComparison.OrdinalIgnoreCase)).Count() * 100.0m) / totalCount, 2) : 0m;
            //decimal ApplicationStatusCountPendingChecker = totalCount > 0 ? Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING CHECKER", StringComparison.OrdinalIgnoreCase)).Count() * 100.0m) / totalCount, 2) : 0m;
            //decimal ApplicationStatusCountPendingCompletion = totalCount > 0 ? Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING COMPLETION", StringComparison.OrdinalIgnoreCase)).Count() * 100.0m) / totalCount, 2) : 0m;
            //decimal ApplicationStatusCountPendingDocuments = totalCount > 0 ? Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING BANK DOCUMENTS", StringComparison.OrdinalIgnoreCase)).Count() * 100.0m) / totalCount, 2) : 0m;
            //decimal ApplicationStatusCountPendingExecution = totalCount > 0 ? Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING EXECUTION", StringComparison.OrdinalIgnoreCase)).Count() * 100.0m) / totalCount, 2) : 0m;
            //decimal ApplicationStatusCountPendingInitiator = totalCount > 0 ? Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING INITIATOR", StringComparison.OrdinalIgnoreCase)).Count() * 100.0m) / totalCount, 2) : 0m;
            //decimal ApplicationStatusCountPendingOmmissions = totalCount > 0 ? Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING OMMISSIONS", StringComparison.OrdinalIgnoreCase)).Count() * 100.0m) / totalCount, 2) : 0m;
            //decimal ApplicationStatusCountPendingVerification = totalCount > 0 ? Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING VERIFICATION", StringComparison.OrdinalIgnoreCase)).Count() * 100.0m) / totalCount, 2) : 0m;
            //decimal ApplicationStatusCountPendingSignatures = totalCount > 0 ? Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "PENDING SIGNATURES", StringComparison.OrdinalIgnoreCase)).Count() * 100.0m) / totalCount, 2) : 0m;
            //decimal ApplicationStatusCountWithdrawn = totalCount > 0 ? Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationStatusName, "WITHDRAWN", StringComparison.OrdinalIgnoreCase)).Count() * 100.0m) / totalCount, 2) : 0m;

            //var result = new List<HomeChartViewModel>()
            //{
            //    new HomeChartViewModel(){ value= ApplicationStatusCountDraft, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.Draft"),color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.color.Draft") },
            //    new HomeChartViewModel(){ value= ApplicationStatusCountCancelled, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.Cancelled"),color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.color.Cancelled") },
            //    new HomeChartViewModel(){ value= ApplicationStatusCountCompleted, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.Completed"),color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.color.Completed") },
            //    new HomeChartViewModel(){ value= ApplicationStatusCountPendingChecker, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.PendingChecker"),color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.color.PendingChecker") },
            //    new HomeChartViewModel(){ value= ApplicationStatusCountPendingCompletion, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.PendingCompletion"),color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.color.PendingCompletion") },
            //    new HomeChartViewModel(){ value= ApplicationStatusCountPendingDocuments, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.PendingDocuments"),color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.color.PendingDocuments") },
            //    new HomeChartViewModel(){ value= ApplicationStatusCountPendingExecution, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.PendingExecution"),color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.color.PendingExecution") },
            //    new HomeChartViewModel(){ value= ApplicationStatusCountPendingInitiator, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.PendingInitiator"),color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.color.PendingInitiator") },
            //    new HomeChartViewModel(){ value= ApplicationStatusCountPendingOmmissions, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.PendingOmmissions"),color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.color.PendingOmmissions") },
            //    new HomeChartViewModel(){ value= ApplicationStatusCountPendingVerification, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.PendingVerification"),color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.color.PendingVerification") },
            //    new HomeChartViewModel(){ value= ApplicationStatusCountPendingSignatures, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.PendingSignatures"),color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.color.PendingSignatures") },
            //    new HomeChartViewModel(){ value= ApplicationStatusCountWithdrawn, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.Withdrawn"),color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationStatus.color.Withdrawn") }
            // };
            Console.WriteLine("test123");
            return Json(result);
        }

        public IActionResult GetApplicationByType()
        {
            UserModel user = UserProcess.GetUser(User.Identity.Name);
            string cacheKey = user.UserInformation.UserGUID + "_GetApplicationByType";
            if (!_memoryCache.TryGetValue(cacheKey, out List<ApplicationDetailsModelView> listApplications))
            {
                listApplications = ApplicationExcelDownloadProcess.GetApplicationByUser(user, applicationsRepository, true);
                _memoryCache.Set(cacheKey, listApplications);
                //var cacheEntryOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(30)); // Set expiration time
                //_memoryCache.Set(cacheKey, listApplications, cacheEntryOption);
            }

            decimal totalCount = listApplications.Count;

            decimal individualCount = 0m;
            decimal jointCount = 0m;
            decimal companyCount = 0m;

            if (totalCount > 0)
            {
                foreach (var application in listApplications)
                {
                    switch (application.ApplicationDetails_ApplicationTypeName.ToUpperInvariant())
                    {
                        case "INDIVIDUAL":
                            individualCount++;
                            break;
                        case "JOINT INDIVIDUAL":
                            jointCount++;
                            break;
                        case "LEGAL ENTITY":
                            companyCount++;
                            break;
                    }
                }

                individualCount = Math.Round((individualCount * 100.0m) / totalCount, 2);
                jointCount = Math.Round((jointCount * 100.0m) / totalCount, 2);
                companyCount = Math.Round((companyCount * 100.0m) / totalCount, 2);
            }

            //decimal ApplicationTypeIndividualCount = totalCount > 0
            //    ? Math.Round((listApplications
            //        .Where(x => string.Equals(x.ApplicationDetails_ApplicationTypeName, "INDIVIDUAL", StringComparison.OrdinalIgnoreCase))
            //        .Count() * 100.0m) / totalCount, 2) // 2 decimal places
            //    : 0m; // 0.0m for decimal type
            //decimal ApplicationTypeJointCount = totalCount > 0
            //    ? Math.Round((listApplications
            //        .Where(x => string.Equals(x.ApplicationDetails_ApplicationTypeName, "JOINT INDIVIDUAL", StringComparison.OrdinalIgnoreCase))
            //        .Count() * 100.0m) / totalCount, 2)
            //    : 0m;
            //decimal ApplicationTypeCompanyCount = totalCount > 0
            //    ? Math.Round((listApplications
            //        .Where(x => string.Equals(x.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase))
            //        .Count() * 100.0m) / totalCount, 2)
            //    : 0m;

            //int ApplicationTypeIndividualCount = listApplications.Count > 0 ? (int)Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationTypeName, "INDIVIDUAL", StringComparison.OrdinalIgnoreCase)).Count() * 100.0) / listApplications.Count) : 0;
            //int ApplicationTypeJointCount = listApplications.Count > 0 ? (int)Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationTypeName, "JOINT INDIVIDUAL", StringComparison.OrdinalIgnoreCase)).Count() * 100.0) / listApplications.Count) : 0;
            //int ApplicationTypeCompanyCount = listApplications.Count > 0 ? (int)Math.Round((listApplications.Where(x => string.Equals(x.ApplicationDetails_ApplicationTypeName, "LEGAL ENTITY", StringComparison.OrdinalIgnoreCase)).Count() * 100.0) / listApplications.Count) : 0;

            //var result = new List<HomeChartViewModel>()
            //{
            //    new HomeChartViewModel(){ value= ApplicationTypeIndividualCount, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationType.Individual"), color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationType.color.Individual") },
            //    new HomeChartViewModel(){ value= ApplicationTypeJointCount, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationType.Joint"), color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationType.color.Joint") },
            //    new HomeChartViewModel(){ value= ApplicationTypeCompanyCount, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationType.Company"), color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationType.color.Company") }
            // };
            //return Json(result);
            var result = new List<HomeChartViewModel>() {
        new HomeChartViewModel() { value = individualCount, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationType.Individual"), color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationType.color.Individual") },
        new HomeChartViewModel() { value = jointCount, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationType.Joint"), color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationType.color.Joint") },
        new HomeChartViewModel() { value = companyCount, category = ResHelper.GetString("Eurobank.Home.Chart.ApplicationType.Company"), color = ResHelper.GetString("Eurobank.Home.Chart.ApplicationType.color.Company") }
    };

            return Json(result);
        }

    }
}
