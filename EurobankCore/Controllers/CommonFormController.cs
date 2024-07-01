using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using CMS.Base;
using CMS.Core;
using CMS.CustomTables;
using CMS.CustomTables.Types.Eurobank;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Globalization;
using CMS.Helpers;
using CMS.MediaLibrary;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.WebAnalytics;
using Eurobank.Helpers.External.File;
using Eurobank.Infrastructure;
using Eurobank.Models;
using Eurobank.Models.FormModels;
using Eurobank.Models.Registries;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Text.RegularExpressions;

namespace Eurobank.Controllers
{
    [Authorize]
    public class CommonFormController : Controller
    {
        private readonly TypedSearchItemViewModelFactory searchItemViewModelFactory;
        private readonly IPagesActivityLogger pagesActivityLogger;
        private readonly IAnalyticsLogger analyticsLogger;
        private readonly IEventLogService eventLogService;
        private readonly ISiteService siteService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly RegistriesRepository registriesRepository;
        private readonly IConfiguration _config;


        public CommonFormController(TypedSearchItemViewModelFactory searchItemViewModelFactory, IPagesActivityLogger pagesActivityLogger,
            IAnalyticsLogger analyticsLogger, ISiteService siteService, IHttpContextAccessor httpContextAccessor, IEventLogService eventLogService, RegistriesRepository registriesRepository, IConfiguration config
)
        {
            this.searchItemViewModelFactory = searchItemViewModelFactory;
            this.pagesActivityLogger = pagesActivityLogger;
            this.analyticsLogger = analyticsLogger;
            this.siteService = siteService;
            this.eventLogService = eventLogService;
            this.httpContextAccessor = httpContextAccessor;
            this.registriesRepository = registriesRepository;
            this._config = config;
        }
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Address_Read([DataSourceRequest] DataSourceRequest request)
        {
            string applicationID = Convert.ToString(TempData["ApplicationID"]);
            TempData.Keep("ApplicationID");
            List<AddressViewModel> addressViewModels = new List<AddressViewModel>();
            string customTableClassName = "Eurobank.TempAddressDetails";
            DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
            if (customTable != null)
            {

                // Gets the first custom table record whose value in the 'ItemName' field is equal to "SampleName"
                var addressData = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("PersonalDetailsID", applicationID).OrderByDescending("ItemID").ToList();
                foreach (var item in addressData)
                {

                    AddressViewModel addressViewModel = new AddressViewModel();
                    addressViewModel.AddressDocument = ValidationHelper.GetString(item.GetValue("AddressDocuments"), "");
                    addressViewModel.AddressType = ValidationHelper.GetString(item.GetValue("AddressType"), "");
                    addressViewModel.AddresTypeName = GetAddressName(ValidationHelper.GetString(item.GetValue("AddressType"), ""));
                    addressViewModel.PostalCode = ValidationHelper.GetString(item.GetValue("PostalCode"), "");
                    addressViewModel.City = ValidationHelper.GetString(item.GetValue("City"), "");
                    addressViewModel.Country = ValidationHelper.GetString(item.GetValue("Country"), "");
                    addressViewModel.CountryName = GetCountryName(ValidationHelper.GetString(item.GetValue("Country"), ""));
                    addressViewModel.StreetLane1 = ValidationHelper.GetString(item.GetValue("StreetLane1"), "");
                    addressViewModel.StreetLane2 = ValidationHelper.GetString(item.GetValue("StreetLane2"), "");
                    addressViewModel.AddressGuid = ValidationHelper.GetString(item.GetValue("ItemID"), "");
                    addressViewModels.Add(addressViewModel);
                }
            }



            return Json(addressViewModels.ToDataSourceResult(request));
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

        public JsonResult Country_read([DataSourceRequest] DataSourceRequest request)
        {
            var countries = CountryInfoProvider.GetCountries().ToList();
            var data = countries.Select(x => new SelectListItem()
            {
                Text = x.CountryName,
                Value = x.CountryGUID.ToString()
            });



            return Json(data.ToList().ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult EditingPopup_Create([DataSourceRequest] DataSourceRequest request, AddressViewModel addressViewModel)
        {

            addressViewModel.ApplicationID = Convert.ToString(TempData["ApplicationID"]);
            string file = httpContextAccessor.HttpContext.Session.GetString("FileName");
            string fileOrgName = httpContextAccessor.HttpContext.Session.GetString("FileOrgName");
            string filePathname = UploadFileToMedialibrary("Eurobank", "Eurobank", "", "", file);
            httpContextAccessor.HttpContext.Session.SetString("FileName", "");
            TempData.Keep("ApplicationID");
            if (addressViewModel != null && ModelState.IsValid)
            {
                TempAddressDetailsItem tempAddressDetailsItem = new TempAddressDetailsItem();
                tempAddressDetailsItem.AddressType = addressViewModel.AddressType;
                tempAddressDetailsItem.City = addressViewModel.City;
                tempAddressDetailsItem.Country = addressViewModel.Country;
                tempAddressDetailsItem.StreetLane1 = addressViewModel.StreetLane1;
                tempAddressDetailsItem.StreetLane2 = addressViewModel.StreetLane2;
                tempAddressDetailsItem.PostalCode = addressViewModel.PostalCode;
                tempAddressDetailsItem.PersonalDetailsID = addressViewModel.ApplicationID;
                tempAddressDetailsItem.SetValue("AddressDocuments", filePathname);
                tempAddressDetailsItem.Insert();
            }
            addressViewModel.AddresTypeName = GetAddressName(addressViewModel.AddressType);
            addressViewModel.CountryName = GetCountryName(addressViewModel.Country);
            return Json(new[] { addressViewModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult EditingPopup_Update([DataSourceRequest] DataSourceRequest request, AddressViewModel addressViewModel, IEnumerable<IFormFile> Picture)
        {
            string file = httpContextAccessor.HttpContext.Session.GetString("FileName");
            string filePathname = UploadFileToMedialibrary("Eurobank", "Eurobank", "", "", file);
            httpContextAccessor.HttpContext.Session.SetString("FileName", "");

            //SaveFile("dd");
            //IEnumerable<IFormFile> existingPhotos1=(IEnumerable<IFormFile>)TempData["files"];
            if (addressViewModel != null && ModelState.IsValid)
            {
                string customTableClassName = "Eurobank.TempAddressDetails";
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (customTable != null)
                {
                    // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                    var customTableData = CustomTableItemProvider.GetItems(customTableClassName)
                                                                        .WhereStartsWith("ItemID", addressViewModel.AddressGuid);

                    // Loops through individual custom table records
                    foreach (CustomTableItem item in customTableData)
                    {

                        item.SetValue("AddressType", addressViewModel.AddressType);
                        item.SetValue("StreetLane1", addressViewModel.StreetLane1);
                        item.SetValue("StreetLane2", addressViewModel.StreetLane2);
                        item.SetValue("PostalCode", addressViewModel.PostalCode);
                        item.SetValue("City", addressViewModel.City);
                        item.SetValue("Country", addressViewModel.Country);
                        item.SetValue("AddressDocuments", filePathname);

                        // Saves the changes to the database
                        item.Update();
                    }
                }
            }
            addressViewModel.AddresTypeName = GetAddressName(addressViewModel.AddressType);
            addressViewModel.CountryName = GetCountryName(addressViewModel.Country);
            return Json(new[] { addressViewModel }.ToDataSourceResult(request, ModelState));
        }





        [HttpPost]
        public ActionResult EditingPopup_Destroy([DataSourceRequest] DataSourceRequest request, AddressViewModel addressViewModel)
        {
            if (addressViewModel != null)
            {
                string customTableClassName = "Eurobank.TempAddressDetails";

                // Gets the custom table
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (customTable != null)
                {
                    // Gets the first custom table record whose value in the 'ItemText' starts with 'New text'
                    CustomTableItem item = CustomTableItemProvider.GetItems(customTableClassName)
                                                                         .WhereStartsWith("ItemID", addressViewModel.AddressGuid)
                                                                        .TopN(1)
                                                                        .FirstOrDefault();

                    if (item != null)
                    {
                        // Deletes the custom table record from the database
                        item.Delete();
                    }
                }
            }

            return Json(new[] { addressViewModel }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Events_Save(IEnumerable<IFormFile> files)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/tempimg"));

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {

                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(fileContent.FileName.ToString().Trim('"')); ;

                    var savedPath1 = "\\" + Path.Combine($"Content\\tempimg", ImageName);
                    //Get url To Save
                    string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/tempimg", ImageName);

                    using (var stream = new FileStream(SavePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    httpContextAccessor.HttpContext.Session.SetString("FileName", SavePath);
                    // The files are not actually saved in this demo
                    // file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Async_Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    string physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/tempimg", fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }


        public ActionResult Documents_Save(IEnumerable<IFormFile> files, string applicationNumber)
        {
            string retval = ""; //return blank for success
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/UploadDoc"));

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                // The Name of the Upload component is "files"
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        bool isValid = true;
                        var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                        var permittedExtensions = _config.GetSection("FileExtensions:PermittedExtensions").Get<string[]>();
                        // var permittedExtensions = new[] { ".jpg", ".png", ".jpeg", ".pdf" };
                        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                        if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                        {
                            isValid = false;
                            retval = "Invalid file type.";
                        }

                        var mimeType = file.ContentType;
                        //var permittedMimeTypes = new[] { "image/jpeg", "image/png", "image/jpg", "application/pdf" };
                        var permittedMimeTypes = _config.GetSection("FileExtensions:PermittedMimeTypes").Get<string[]>();
                        if (!permittedMimeTypes.Contains(mimeType))
                        {
                            isValid = false;
                            retval = "Invalid MIME type.";
                        }
                        
                        if (isValid)
                        {
                            string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(fileContent.FileName.ToString().Trim('"'));
                            var savedPath1 = "\\" + Path.Combine($"Content\\UploadDoc", ImageName);
                            string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/UploadDoc", ImageName);
                            using (var stream = new FileStream(SavePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            bool isSanitaized = CheckPdfForXss(SavePath);
                            if (isSanitaized == false)
                            {
                                isValid = false;
                                retval = "Insecure file.";
                                return Content(retval);
                            }
                            string externalFileGuid = string.Empty;
                            var documentUploadResult = ExternalFileService.SendFile(file, applicationNumber, _config.GetValue<string>(
                        "EuroBankDocumentAPIUrl"));

                            if (documentUploadResult != null)
                            {
                                externalFileGuid = documentUploadResult.FileGuid;
                                if (!documentUploadResult.IsSuccess)
                                {
                                    eventLogService.LogInformation("ExternalFileService", "SendFile", documentUploadResult.ErrorMessage);
                                    retval = "Failed";
                                }
                                if (!string.IsNullOrEmpty(documentUploadResult.FileGuid))
                                {
                                    httpContextAccessor.HttpContext.Session.SetString("DocumentsFileName", SavePath);
                                    httpContextAccessor.HttpContext.Session.SetString("DocumentFileGuid", externalFileGuid);
                                    httpContextAccessor.HttpContext.Session.SetString("UploadedFileName", fileContent.FileName);
                                    retval = "";
                                }
                            }
                            else
                            {
                                retval = "Failed";
                            }
                        }
                    }
                }
                else
                {
                    retval = "Failed";
                }
            }
            catch (Exception ex)
            {
                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "ErrorLog/ExceptionLog.txt");
                if (!System.IO.File.Exists(filepath))
                {
                    System.IO.File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = System.IO.File.AppendText(filepath))
                {
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(ex.ToString());
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.Flush();
                    sw.Close();
                }
                retval = "";
            }
            return Content(retval);
        }
        public static bool CheckPdfForXss(string filePath)
        {
            try
            {
                using (PdfReader reader = new PdfReader(filePath))
                using (PdfDocument pdfDoc = new PdfDocument(reader))
                {
                    for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                    {
                        string pageContent = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i));

                        if (ContainsPotentialXss(pageContent))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking PDF: {ex.Message}");
                return false;
            }
        }

        public static bool ContainsPotentialXss(string content)
        {
            // Basic regex patterns to detect common XSS patterns
            string[] xssPatterns = {
            "<script[^>]*>.*?</script>", // Script tags
            "javascript:",              // JavaScript URLs
            "on\\w+\\s*=\\s*\"[^\"]*\"", // Event handlers
            "eval\\(",                  // eval() function
            "alert\\(",                 // alert() function
            "document.cookie",          // document.cookie access
            "document.write"            // document.write() function
        };

            foreach (var pattern in xssPatterns)
            {
                if (Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    return true;
                }
            }
            return false;
        }

        //[HttpGet("downloadfile/{docId}")]
        public FileResult DownloadFile(string applicationNumber, string fileGuid, string fileName)
        {
            byte[] rawFile = ExternalFileService.DowloadFile(fileGuid, applicationNumber, fileName, _config.GetValue<string>(
                "EuroBankDocumentAPIUrl"));
            return File(rawFile, ExternalFileService.GetMIMEType(fileName), fileName);
        }

        public static string UploadFileToMedialibrary(string mediaLibraryName, string mediaLibraryFolderName, string originalFileName, string filename, string filePath)
        {

            var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
            string ParmanentPath = "";
            try
            {
                MediaLibraryInfo library = MediaLibraryInfoProvider.GetMediaLibraryInfo(mediaLibraryName, SiteContext.CurrentSiteName);
                if (library != null)
                {
                    MediaFileInfo deleteFile = MediaFileInfoProvider.GetMediaFileInfo(library.LibraryID, filename);
                    if (deleteFile != null)
                    {
                        MediaFileInfoProvider.DeleteMediaFileInfo(deleteFile);
                    }
                    MediaFileInfo updateFile = MediaFileInfoProvider.GetMediaFileInfo(library.LibraryID, mediaLibraryFolderName + "/" + filename);
                    if (updateFile == null)
                    {
                        MediaFileInfo mediaFile = null;
                        CMS.IO.FileInfo file = CMS.IO.FileInfo.New(physicalPath);

                        if (file != null)
                        {
                            try
                            {
                                mediaFile = new MediaFileInfo(filePath, library.LibraryID);
                                mediaFile.FileName = file.Name;
                                mediaFile.FileTitle = "EurobankAdrresDocuments";
                                mediaFile.FileDescription = "This file was added through the API.";
                                mediaFile.FilePath = "Participant";
                                mediaFile.FileExtension = file.Extension;
                                mediaFile.FileMimeType = MimeTypeHelper.GetMimetype(file.Extension);
                                mediaFile.FileSiteID = SiteContext.CurrentSiteID;
                                mediaFile.FileLibraryID = library.LibraryID;
                                mediaFile.FileSize = file.Length;

                                MediaFileInfoProvider.SetMediaFileInfo(mediaFile);

                                ParmanentPath = MediaFileURLProvider.GetMediaFileUrl(mediaFile.FileGUID, file.Name);
                                if (System.IO.File.Exists(physicalPath))
                                {
                                    // The files are not actually removed in this demo
                                    System.IO.File.Delete(physicalPath);
                                }
                            }
                            catch (Exception ex)
                            {
                                CMS.EventLog.EventLogProvider.LogException("", "File_Upload_Inner", ex);
                            }
                        }
                    }
                    else
                    {
                        updateFile.FileDescription = updateFile.FileDescription.ToLower();
                        MediaFileInfoProvider.SetMediaFileInfo(updateFile);
                    }
                }
            }
            catch (Exception ex)
            {
                CMS.EventLog.EventLogProvider.LogException("Controller_CommonFormController", "uploadFileToMedialibrary", ex);
            }
            return ParmanentPath;
        }

        [HttpPost]
        public ActionResult Polls(PollsViewModel pollsViewModel)
        {
            if (string.IsNullOrEmpty(pollsViewModel.Answer))
            {
                TempData["Massage"] = ResHelper.GetString("Eubank.RadioMassage");
                return RedirectToAction("Index", "Home");
            }
            try
            {
                Polls_UsersItem polls_UsersItem = new Polls_UsersItem();
                polls_UsersItem.AnswerID = pollsViewModel.Answer;
                polls_UsersItem.PollsID = pollsViewModel.PollQuestionID;
                polls_UsersItem.UserID = User.Identity.Name;
                polls_UsersItem.Insert();
                // Prepares the code name (class name) of the custom table
                string customTableClassName = "Eurobank.Polls_PollAnswer";

                // Gets the custom table
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (customTable != null)
                {
                    // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                    var customTableData = CustomTableItemProvider.GetItems(customTableClassName)
                                                                        .WhereStartsWith("ItemID", pollsViewModel.Answer);

                    // Loops through individual custom table records
                    foreach (CustomTableItem item in customTableData)
                    {
                        // Gets the text value from the data record's 'ItemText' field
                        int answerCount = ValidationHelper.GetInteger(item.GetValue("AnswerCount"), 0);

                        // Sets a new 'ItemText' value based on the old one
                        item.SetValue("AnswerCount", (answerCount + 1));

                        // Saves the changes to the database
                        item.Update();
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {

                return RedirectToAction("Index", "Home");
            }
        }
    }
}
