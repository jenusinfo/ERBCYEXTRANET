using CMS.CustomTables;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Localization;
using CMS.MediaLibrary;
using CMS.Membership;
using CMS.SiteProvider;
using Eurobank.Models.Applications;
using Eurobank.Models.CountryCodePrefix;
using Eurobank.Models.KendoExtention;
using Kendo.Mvc.UI;
using Kentico.Content.Web.Mvc;
using MaxMind.GeoIP2.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eurobank.Helpers.DataAnnotation
{
    public class ServiceHelper
    {
        private static readonly ObjectCache CacheDocumentName = MemoryCache.Default;
        internal static string GetName(string nodeGuid, string path)
        {
            string cacheKey = $"{nodeGuid}_{path}";
            if (CacheDocumentName.Contains(cacheKey))
            {
                return CacheDocumentName[cacheKey] as string;
            }

            string name = FetchNameFromDatabase(nodeGuid, path);

            // Cache the result for future use
            CacheDocumentName.Set(cacheKey, name, DateTimeOffset.UtcNow.AddHours(1)); // Adjust the expiration time as needed

            return name;
        }
        private static string FetchNameFromDatabase(string nodeGuid, string path)
        {
            string name = string.Empty;
            //var lookupName = new MultiDocumentQuery()
            //        .OnCurrentSite()
            //        .Path(path, PathTypeEnum.Children)
            //        .Culture(LocalizationContext.CurrentCulture.CultureCode)
            //        .PublishedVersion()
            //        .WhereEquals("NodeGUID", ValidationHelper.GetGuid(nodeGuid, Guid.NewGuid()))
            //        .FirstOrDefault();

            //if (lookupName != null)
            //{
            //    name = lookupName.DocumentName;
            //}
            var documents = DocumentHelper.GetDocuments()
            .Path(path, PathTypeEnum.Children)
            .Culture(LocalizationContext.CurrentCulture.CultureCode)
            .PublishedVersion()
            .WhereEquals("NodeGUID", ValidationHelper.GetGuid(nodeGuid, Guid.NewGuid()))
            .Columns("DocumentName")
            .TopN(1)
            .ToList();

            if (documents != null && documents.Count > 0)
            {
                name = documents[0].DocumentName;
            }
            return name;
        }

        internal static List<SelectListItem> GetCountries()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var countries = CountryInfoProvider.GetCountries().ToList();

            if (countries != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (countries.Count() > 0)
                {
                    retValues = countries.Select(x => new SelectListItem { Value = Convert.ToString(x.CountryName), Text = x.CountryDisplayName.ToUpper() }).ToList();

                }

            }

            return retValues;
        }
        internal static string GetCountriesCode(int CountryID)
        {
            string CountryCode = "";
            CountryCode = CountryInfoProvider.GetCountryInfo(CountryID).CountryTwoLetterCode;
            return CountryCode;
        }
        internal static List<SelectListItem> GetCountriesRegistry()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var countries = CountryInfoProvider.GetCountries().ToList();

            if (countries != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (countries.Count() > 0)
                {
                    retValues = countries.Select(x => new SelectListItem { Value = Convert.ToString(x.CountryGUID), Text = x.CountryDisplayName }).ToList();

                }

            }

            return retValues;
        }
        internal static List<SelectListItem> GetCountriesWithID()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var countries = CountryInfoProvider.GetCountries().ToList();

            if (countries != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (countries.Count() > 0)
                {
                    retValues = countries.Select(x => new SelectListItem { Value = Convert.ToString(x.CountryID), Text = x.CountryDisplayName.ToUpper() }).ToList();

                }

            }

            return retValues;
        }

        internal static string GetCountryTwoletterCode(string CountryName)
        {
            string CountryCode = "";
            try
            {
                if (CountryName != "")
                {
                    CountryCode = CountryInfoProvider.GetCountryInfo(ServiceHelper.RemoveSpecialCharacters(CountryName)).CountryTwoLetterCode;
                }
            }
            catch
            { }
            return CountryCode;
        }

        internal static List<SelectListItem> GetAddressTypeIndividualRelatedParty(bool isRelatedPartyUBO)
        {
            List<string> mylist = null;
            if (isRelatedPartyUBO)
            {
                mylist = new List<string>(new string[] { "RESIDENTIAL ADDRESS", "WORK ADDRESS" });
            }
            else
            {
                mylist = new List<string>(new string[] { "RESIDENTIAL ADDRESS" });
            }

            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var lookupAddressType = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.Address_Type_Physical, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            if (lookupAddressType != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (lookupAddressType.Count() > 0)
                {
                    retValues = lookupAddressType.Where(x => mylist.Contains(x.DocumentName)).Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetAddressTypeLegalRelatedParty()
        {
            List<string> mylist = new List<string>(new string[] { "REGISTERED OFFICE" });
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var lookupAddressType = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.Address_Type, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            if (lookupAddressType != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (lookupAddressType.Count() > 0)
                {
                    retValues = lookupAddressType.Where(x => mylist.Contains(x.DocumentName.Trim())).Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetAddressType()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var lookupAddressType = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.Address_Type, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            if (lookupAddressType != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (lookupAddressType.Count() > 0)
                {
                    retValues = lookupAddressType.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetAddressTypePersonRegistry()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var lookupAddressTypeLegal = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.Address_Type, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            if (lookupAddressTypeLegal != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (lookupAddressTypeLegal.Count() > 0)
                {
                    retValues = lookupAddressTypeLegal.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }

            var lookupAddressTypeIndividual = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.ADDRESS_TYPE, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();
            foreach (var item in lookupAddressTypeIndividual)
            {
                if (!string.Equals(item.DocumentName.Trim(), "MAILING ADDRESS"))
                {
                    retValues.Add(new SelectListItem { Text = item.DocumentName, Value = item.NodeGUID.ToString() });
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetAddressTypeLegal(string entityType)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var lookupAddressType = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.Address_Type, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            string EntityTypeText = GetName(entityType, Constants.COMPANY_ENTITY_TYPE);
            if (string.Equals(EntityTypeText.Trim(), "Foundation") || string.Equals(EntityTypeText.Trim(), "Trade Union") || string.Equals(EntityTypeText.Trim(), "Club / Association") || string.Equals(EntityTypeText.Trim(), "City Council / Local Authority")
                || string.Equals(EntityTypeText.Trim(), "Government Organization") || string.Equals(EntityTypeText.Trim(), "Semi - Government Organization") || string.Equals(EntityTypeText.Trim(), "General Partnership")
                || string.Equals(EntityTypeText.Trim(), "Limited Liability Partnership") || string.Equals(EntityTypeText.Trim(), "Limited Partnership"))
            {
                foreach (var item in lookupAddressType)
                {
                    if (!string.Equals(item.DocumentName.Trim(), "OFFICE IN CYPRUS") && !string.Equals(item.DocumentName.Trim(), "HEAD OFFICE") && !string.Equals(item.DocumentName.Trim(), "ADMINISTRATION OFFICE") && !string.Equals(item.DocumentName.Trim(), "BUSINESS OFFICE"))
                    {
                        retValues.Add(new SelectListItem { Text = item.DocumentName, Value = item.NodeGUID.ToString() });
                    }
                }
            }
            else if (string.Equals(EntityTypeText.Trim(), "Provident Fund") || string.Equals(EntityTypeText.Trim(), "Pension Fund"))
            {
                foreach (var item in lookupAddressType)
                {
                    if (!string.Equals(item.DocumentName.Trim(), "OFFICE IN CYPRUS") && !string.Equals(item.DocumentName.Trim(), "HEAD OFFICE") && !string.Equals(item.DocumentName.Trim(), "ADMINISTRATION OFFICE") && !string.Equals(item.DocumentName.Trim(), "PRINCIPAL TRADING /BUSINESS OFFICE"))
                    {
                        retValues.Add(new SelectListItem { Text = item.DocumentName, Value = item.NodeGUID.ToString() });
                    }
                }
            }
            else if (string.Equals(EntityTypeText.Trim(), "Trust"))
            {
                foreach (var item in lookupAddressType)
                {
                    if (string.Equals(item.DocumentName.Trim(), "MAILING ADDRESS") || string.Equals(item.DocumentName.Trim(), "ADMINISTRATION OFFICE"))
                    {
                        retValues.Add(new SelectListItem { Text = item.DocumentName, Value = item.NodeGUID.ToString() });
                    }
                }
            }
            else
            {
                foreach (var item in lookupAddressType)
                {
                    if (!string.Equals(item.DocumentName.Trim(), "ADMINISTRATION OFFICE") && !string.Equals(item.DocumentName.Trim(), "BUSINESS OFFICE"))
                    {
                        retValues.Add(new SelectListItem { Text = item.DocumentName, Value = item.NodeGUID.ToString() });
                    }
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetAddressTypePhysical()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var lookupAddressType = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.Address_Type_Physical, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            if (lookupAddressType != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (lookupAddressType.Count() > 0)
                {
                    retValues = lookupAddressType.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetCommonDropDown(string path)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var gender = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(path, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (gender != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (gender.Count() > 0)
                {
                    retValues = gender.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName.ToUpper() }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetGendar()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var gender = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.GENDER, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (gender != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (gender.Count() > 0)
                {
                    retValues = gender.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetTitle()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var title = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.TITLES, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (title != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (title.Count() > 0)
                {
                    retValues = title.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        //internal static List<SelectListItem> GetPlaceofBirth()
        //{
        //	List<SelectListItem> retValues = new List<SelectListItem>();
        //	Gets the custom table

        //	var placeofbirth = new MultiDocumentQuery()
        //			.OnCurrentSite()
        //			.Path("/Lookups/General/Address-Type", PathTypeEnum.Children)
        //			.Culture("en-us")
        //			.PublishedVersion();
        //	if(placeofbirth != null)
        //	{
        //		Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'

        //		if(placeofbirth.Count() > 0)
        //		{
        //			retValues = placeofbirth.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
        //		}
        //	}
        //	return retValues;
        //}
        internal static List<SelectListItem> GetEducationLevel()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Education, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetSignatureRights()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.SIGNATURE_RIGHTS, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetTypeIdentification()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var identificationType = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.IDENTIFICATION_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (identificationType != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (identificationType.Count() > 0)
                {
                    retValues = identificationType.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetTypeIdentificationIndividual()
        {
            List<string> mylist = new List<string>(new string[] { "ID", "PASSPORT" });
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var identificationType = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.IDENTIFICATION_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (identificationType != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (identificationType.Count() > 0)
                {
                    retValues = identificationType.Where(x => mylist.Contains(x.DocumentName)).Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetTypeIdentificationLegal()
        {
            List<string> mylist = new List<string>(new string[] { "REGISTRATION NUMBER" });
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var identificationType = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.IDENTIFICATION_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (identificationType != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (identificationType.Count() > 0)
                {
                    retValues = identificationType.Where(x => mylist.Contains(x.DocumentName)).Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetCommunicationLanguage()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.COMMUNICATION_LANGUAGE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetApplicationType()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.APPLICATION_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'

                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetApplicationTypeLegalOnly()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.APPLICATION_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'

                if (communicationLanguage.Count() > 0)
                {

                    retValues = communicationLanguage.Where(x => x.DocumentName == "LEGAL ENTITY").Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName,Selected=true }).ToList();

                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetRegistryApplicationType()
        {
            List<string> mylist = new List<string>(new string[] { "INDIVIDUAL", "LEGAL ENTITY" });
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var applicationType = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.APPLICATION_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (applicationType != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'

                if (applicationType.Count() > 0)
                {
                    //retValues = applicationType.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                    retValues = applicationType.Where(x => mylist.Contains(x.DocumentName)).Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetTypeofIdentification()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.IDENTIFICATION_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetBankingService()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Bank_Units, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetBankingUnitsByBankGuids(List<Guid> bankGuids)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table

            if (bankGuids != null && bankGuids.Count > 0)
            {
                var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Bank_Units, PathTypeEnum.Children)
                    .WhereIn("NodeGUID", bankGuids)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage != null && communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }


        internal static List<Guid> GetAllSubBankUnitsAlogWithParent(string parentBankId)
        {
            List<Guid> retValues = new List<Guid>();
            // Gets the custom table
            var bankUnits = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Bank_Units, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (bankUnits != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (bankUnits.Count() > 0)
                {
                    retValues = bankUnits.Where(u => u.NodeGUID.ToString() == parentBankId || (u.Parent != null && u.Parent.NodeGUID.ToString() == parentBankId)).Select(x => x.NodeGUID).AsEnumerable().ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetResponsibleOfficer()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var responsibleOfficer = UserInfoProvider.GetUsers();

            if (responsibleOfficer != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (responsibleOfficer.Count() > 0)
                {
                    retValues = responsibleOfficer.Select(x => new SelectListItem { Value = Convert.ToString(x.UserGUID), Text = x.FullName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetResponsibleOfficerByGuid(Guid officerGuid)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var responsibleOfficer = UserInfoProvider.GetUsers();

            if (responsibleOfficer != null)
            {
                var responsibleOfficers = responsibleOfficer.Where(t => t.UserGUID == officerGuid);
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (responsibleOfficers != null && responsibleOfficer.Count() > 0)
                {
                    retValues = responsibleOfficers.Select(x => new SelectListItem { Value = Convert.ToString(x.UserGUID), Text = x.FullName }).ToList();
                }
            }
            return retValues;
        }


        internal static List<SelectListItem> GetTypeofAccounts()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Accounts_AccountType, PathTypeEnum.Children)
                    .Culture("en-us")
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName.ToUpper() }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetApplicationStatuses()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.APPLICATION_STATUS, PathTypeEnum.Children)
                    .Culture("en-us")
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetCurrency()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Accounts_Currency, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetSTATEMENTFREQUENCY()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Accounts_StatementFrequency, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetAccessLevel()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Access_Level, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetAccessLevelIndividual()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Access_Level_Individual, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetAccessRights()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Access_Right, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetBoolDropDownListDefaults()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();

            retValues.Add(new SelectListItem { Text = "YES", Value = "true" });
            retValues.Add(new SelectListItem { Text = "NO", Value = "false" });

            return retValues;
        }

        internal static List<SelectListItem> GetYesNoDropDownListDefaults()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();

            retValues.Add(new SelectListItem { Text = "YES", Value = "YES" });
            retValues.Add(new SelectListItem { Text = "NO", Value = "NO" });

            return retValues;
        }

        internal static List<SelectListItem> GetDocumentTypes()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var documentTypes = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Document_Type, PathTypeEnum.Children)
                    .Culture("en-us")
                    .PublishedVersion();
            if (documentTypes != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (documentTypes.Count() > 0)
                {
                    retValues = documentTypes.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName.ToUpper() }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetNoteDetailPendingOnUsers()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table



            var users = UserInfoProvider.GetUsers();
            if (users != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (users.Count() > 0)
                {
                    retValues = users.Select(x => new SelectListItem { Value = Convert.ToString(x.UserGUID), Text = x.FullName }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetDocumentSubjects()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var documentTypes = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Document_Subject, PathTypeEnum.Children)
                    .Culture("en-us")
                    .PublishedVersion();
            if (documentTypes != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (documentTypes.Count() > 0)
                {
                    retValues = documentTypes.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetEmploymentStatus()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var documentTypes = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.EMPLOYMENT_STATUS, PathTypeEnum.Children)
                    .Culture("en-us")
                    .PublishedVersion();
            if (documentTypes != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (documentTypes.Count() > 0)
                {
                    retValues = documentTypes.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetEconomicSectorIndustries()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var documentTypes = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.ECONOMIC_SECTOR_INDUSTRIES, PathTypeEnum.Children)
                    .Culture("en-us")
                    .PublishedVersion()
                    .OrderBy("NodeOrder");
            if (documentTypes != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (documentTypes.Count() > 0)
                {
                    retValues = documentTypes.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetEmploymentProfessions()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var documentTypes = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.PROFESSION, PathTypeEnum.Children)
                    .Culture("en-us")
                    .PublishedVersion();
            if (documentTypes != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (documentTypes.Count() > 0)
                {
                    retValues = documentTypes.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetSourcesOfAnnualIncome()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var documentTypes = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.SOURCE_OF_ANNUAL_INCOME, PathTypeEnum.Children)
                    .Culture("en-us")
                    .PublishedVersion()
                    .OrderBy("NodeOrder");
            if (documentTypes != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (documentTypes.Count() > 0)
                {
                    retValues = documentTypes.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetOriginOfTotalAssets()
        {

            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var documentTypes = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.ORIGIN_OF_TOTAL_ASSETS, PathTypeEnum.Children)
                    .Culture("en-us")
                    .PublishedVersion()
                    .OrderBy("NodeOrder");
            if (documentTypes != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (documentTypes.Count() > 0)
                {
                    retValues = documentTypes.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName.ToUpper() }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetOriginOfTotalAssetsForIndividual()
        {
            List<string> mylist = new List<string>(new string[] { "Earnings", "Loan", "Other", "Salary", "Investments", "Inheritance/Gifts", "Sale of Business/Assets/Participation" });
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var assetsTypes = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.ORIGIN_OF_TOTAL_ASSETS, PathTypeEnum.Children)
                    .Culture("en-us")
                    .PublishedVersion()
                    .OrderBy("NodeOrder");
            if (assetsTypes != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (assetsTypes.Count() > 0)
                {
                    retValues = assetsTypes.Where(x => mylist.Contains(x.DocumentName)).Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName.ToUpper() }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetListingStatuses()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var documentTypes = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.LISTING_STATUS, PathTypeEnum.Children)
                    .Culture("en-us")
                    .PublishedVersion();
            if (documentTypes != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (documentTypes.Count() > 0)
                {
                    retValues = documentTypes.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }

        internal static List<ApplicatonServices> GetApplicationService()
        {
            List<ApplicatonServices> retValues = new List<ApplicatonServices>();
            // Gets the custom table
            var applicationService = new MultiDocumentQuery()
                   .OnCurrentSite()
                   .Path(Constants.APPLICATION_SERVICES, PathTypeEnum.Children)
                   .Culture(LocalizationContext.CurrentCulture.CultureCode)
                   .PublishedVersion();

            if (applicationService != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (applicationService.Count() > 0)
                {
                    retValues = applicationService.Select(x => new ApplicatonServices { NodeGUID = Convert.ToString(x.NodeGUID), NodeName = x.NodeName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetAssociatedAccount(IEnumerable<TreeNode> treeNodes)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table

            if (treeNodes != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (treeNodes.Count() > 0)
                {
                    retValues = treeNodes.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName + " " + GetName(ValidationHelper.GetString(x.GetValue("Accounts_Currency"), ""), ValidationHelper.GetString(Constants.Accounts_Currency, "")) }).ToList();
                }
            }
            return retValues;
        }


        internal static List<SelectListItem> GetCardType()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.CARD_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion().Published();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetEntity()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.PERSON_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetGroupStructureEntityType()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Entity_TYPE_Group_Structure, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetEntityType()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Entity_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetCompanyEntityTypes()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.COMPANY_ENTITY_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .Published()
                    .OrderBy("NodeOrder");
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList(); //.OrderBy(O => O.Text)
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetDocumentsType()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.BANK_DOCUMENT_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetDocumentsType(string documenttype)
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(documenttype, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetRequiresSignature()
        {

            var items = new List<SelectListItem>
        {
            new SelectListItem() {Text = "NO", Value ="false"},
            new SelectListItem() {Text = "YES", Value = "true"}
        };
            return items;

        }
        internal static List<SelectListItem> GetDecision()
        {

            var items = new List<SelectListItem>
        {
            new SelectListItem() {Text = "ACCEPTED", Value ="ACCEPTED"},
            new SelectListItem() {Text = "EXECUTED", Value = "EXECUTED"},
            new SelectListItem() {Text = "SUBMIT", Value = "SUBMIT"}
        };
            return items;

        }

        internal static List<SelectListItem> GetDecisions()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.DECISION_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetPERSON_ROLE()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.PERSON_ROLE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetLegalPERSON_ROLE()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.LEGALPERSON_ROLE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetCorporatePersonType()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Corporate_PERSON_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetIndividualJointPersonType()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.IndividualJoint_PERSON_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetCorporateDocumentTypeValue()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var corporateDocument = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Corporate_Entity_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (corporateDocument != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (corporateDocument.Count() > 0)
                {
                    retValues = corporateDocument.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetCorporate_Sub_Type()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Corporate_Entity_SUB_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetDispatchMethod()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            //var communicationLanguage = new MultiDocumentQuery()
            //        .OnCurrentSite()
            //        .Path(Constants.DISPATCH_METHOD, PathTypeEnum.Children)
            //        .Culture(LocalizationContext.CurrentCulture.CultureCode)
            //        .PublishedVersion();
            //if (communicationLanguage != null)
            //{
            //    // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
            //    if (communicationLanguage.Count() > 0)
            //    {
            //        retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
            //    }
            //}
            //var dispatchMethod = LookupItemProvider.GetLookupItems().Where(x=>x.NodeAliasPath== Constants.DISPATCH_METHOD); 
            //var dispatchMethod = LookupItemProvider.GetLookupItem(Constants.DISPATCH_METHOD, LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName);
            var dispatchMethod = LookupItemProvider.GetLookupItems().Path(Constants.DISPATCH_METHOD, PathTypeEnum.Children).OnCurrentSite().Culture(LocalizationContext.CurrentCulture.CultureCode).PublishedVersion();
            if (dispatchMethod != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (dispatchMethod.Count() > 0)
                {
                    retValues = dispatchMethod.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.GetValue("LookupItemName").ToString() }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetDispatchMethodIndividual()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            //var communicationLanguage = new MultiDocumentQuery()
            //        .OnCurrentSite()
            //        .Path(Constants.DISPATCH_METHOD_INDIVIDUAL, PathTypeEnum.Children)
            //        .Culture(LocalizationContext.CurrentCulture.CultureCode)
            //        .PublishedVersion();
            //if (communicationLanguage != null)
            //{
            //    // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
            //    if (communicationLanguage.Count() > 0)
            //    {
            //        retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
            //    }
            //}
            var dispatchMethod = LookupItemProvider.GetLookupItems().Path(Constants.DISPATCH_METHOD_INDIVIDUAL, PathTypeEnum.Children).OnCurrentSite().Culture(LocalizationContext.CurrentCulture.CultureCode).PublishedVersion();
            if (dispatchMethod != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (dispatchMethod.Count() > 0)
                {
                    retValues = dispatchMethod.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.GetValue("LookupItemName").ToString() }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetTypeofFinancialInstitution()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            var financialInstitution = LookupItemProvider.GetLookupItems().Path(Constants.TYPE_OF_FINANCIAL_INSITUTION, PathTypeEnum.Children).OnCurrentSite().Culture(LocalizationContext.CurrentCulture.CultureCode).PublishedVersion();
            if (financialInstitution != null)
            {
                if (financialInstitution.Count() > 0)
                {
                    retValues = financialInstitution.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.GetValue("LookupItemName").ToString() }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetNFEtype()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            var nfeType = LookupItemProvider.GetLookupItems().Path(Constants.TYPE_OF_ACTIVE_NON_FINANCIAL_ENTITY_NFE, PathTypeEnum.Children).OnCurrentSite().Culture(LocalizationContext.CurrentCulture.CultureCode).PublishedVersion();
            if (nfeType != null)
            {
                if (nfeType.Count() > 0)
                {
                    retValues = nfeType.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.GetValue("LookupItemName").ToString() }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetDeliveryAddress()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var communicationLanguage = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.ADDRESS_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (communicationLanguage != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (communicationLanguage.Count() > 0)
                {
                    retValues = communicationLanguage.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetCollectedBy()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var responsibleOfficer = UserInfoProvider.GetUsers();

            if (responsibleOfficer != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (responsibleOfficer.Count() > 0)
                {
                    retValues = responsibleOfficer.Select(x => new SelectListItem { Value = Convert.ToString(x.UserGUID), Text = x.FullName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<IInputGroupItem> GetApplicationServiceItemGroup()
        {
            // List<IInputGroupItem> retValues = new List<IInputGroupItem>();
            var itemsList = new List<IInputGroupItem>();
            // Gets the custom table
            var applicationService = new MultiDocumentQuery()
                   .OnCurrentSite()
                   .Path(Constants.APPLICATION_SERVICES, PathTypeEnum.Children)
                   .Culture(LocalizationContext.CurrentCulture.CultureCode)
                   .PublishedVersion();
            if (applicationService != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (applicationService.Count() > 0)
                {

                    itemsList.AddRange(applicationService.Select(x => new InputGroupItemModel { Value = Convert.ToString(x.NodeGUID), Label = x.DocumentName }).ToList());
                }
            }
            return itemsList;
        }
        //internal static string GetName(string NodeGuid, string path)
        //{
        //    string name = string.Empty;
        //    var lookupName = new MultiDocumentQuery()
        //            .OnCurrentSite()
        //            .Path(path, PathTypeEnum.Children)
        //            .Culture(LocalizationContext.CurrentCulture.CultureCode)
        //            .PublishedVersion().WhereEquals("NodeGUID", ValidationHelper.GetGuid(NodeGuid, new Guid())).FirstOrDefault();
        //    if (lookupName != null)
        //    {
        //        name = lookupName.DocumentName;

        //    }

        //    return name;
        //}
        internal static string GetName(string NodeGuid)
        {
            string name = string.Empty;
            var lookupName = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion().WhereEquals("NodeGUID", ValidationHelper.GetGuid(NodeGuid, new Guid())).FirstOrDefault();
            if (lookupName != null)
            {
                name = lookupName.DocumentName;

            }

            return name;
        }
        internal static string GetGuidByName(string NodeName, string path)
        {
            string strGuid = string.Empty;
            if (string.IsNullOrEmpty(path))
            {
                var lookupName = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion().WhereEquals("NodeName", NodeName).FirstOrDefault();
                if (lookupName != null)
                {
                    strGuid = lookupName.NodeGUID.ToString();
                }
            }
            else
            {
                var lookupName = new MultiDocumentQuery()
                        .OnCurrentSite()
                        .Path(path, PathTypeEnum.Children)
                        .Culture(LocalizationContext.CurrentCulture.CultureCode)
                        .PublishedVersion().WhereEquals("NodeName", NodeName).FirstOrDefault();
                if (lookupName != null)
                {
                    strGuid = lookupName.NodeGUID.ToString();
                }
            }

            return strGuid;
        }

        internal static List<string> GetGuidsListByName(string NodeName)
        {
            List<string> Guids = new List<string>();
            string strGuid = string.Empty;
            var nodelists = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion().WhereEquals("NodeName", NodeName);
            if (nodelists != null)
            {
                foreach (var nodelist in nodelists)
                {
                    Guids.Add(nodelist.NodeGUID.ToString());
                }
            }
            return Guids;
        }



        internal static string GetNameByValue(List<SelectListItem> items, string value)
        {
            string name = string.Empty;
            if (items != null && items.Any(r => string.Equals(r.Value, value, StringComparison.OrdinalIgnoreCase)))
            {
                name = items.FirstOrDefault(r => string.Equals(r.Value, value, StringComparison.OrdinalIgnoreCase)).Text;
            }

            return name;
        }

        internal static string GetGuidValueByText(List<SelectListItem> items, string text)
        {
            string name = string.Empty;
            if (items != null && items.Any(r => string.Equals(r.Text, text, StringComparison.OrdinalIgnoreCase)))
            {
                name = items.FirstOrDefault(r => string.Equals(r.Text, text, StringComparison.OrdinalIgnoreCase)).Value;
            }

            return name;
        }

        internal static string GetEntityType(string NodeGuid, string path)
        {
            string name = string.Empty;
            var lookupName = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(path, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion().WhereEquals("NodeGUID", ValidationHelper.GetGuid(NodeGuid, new Guid())).FirstOrDefault();
            if (lookupName != null)
            {
                name = ValidationHelper.GetString(lookupName.NodeAlias, "");

            }

            return name;
        }
        internal static TreeNode GetTreeNode(string NodeGuid, string path)
        {

            return new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(path)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion().FirstOrDefault();

        }
        #region Purpose And Activity

        internal static List<IInputGroupItem> GetReasonsForOpeningTheAccountCheckGroup()
        {
            List<IInputGroupItem> retValues = new List<IInputGroupItem>();
            // Gets the custom table
            var lookupReasonsForOpeningTheAccount = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.PURPOSE_AND_REASON_FOR_OPENING_THE_ACCOUNT, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            if (lookupReasonsForOpeningTheAccount != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (lookupReasonsForOpeningTheAccount.Count() > 0)
                {
                    retValues.AddRange(lookupReasonsForOpeningTheAccount.Select(x => new InputGroupItemModel { Value = Convert.ToString(x.NodeGUID), Label = x.DocumentName }).ToList());
                }
            }
            return retValues;
        }

        internal static List<IInputGroupItem> GetExpectedNatureOfInAndOutTransactionsCheckGroup()
        {
            List<IInputGroupItem> retValues = new List<IInputGroupItem>();
            // Gets the custom table
            var lookupReasonsForOpeningTheAccount = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.EXPECTED_NATURE_INCOMING_OUTGOING_TRANSACTION, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            if (lookupReasonsForOpeningTheAccount != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (lookupReasonsForOpeningTheAccount.Count() > 0)
                {
                    retValues.AddRange(lookupReasonsForOpeningTheAccount.Select(x => new InputGroupItemModel { Value = Convert.ToString(x.NodeGUID), Label = x.DocumentName }).ToList());
                }
            }
            return retValues;
        }

        internal static List<IInputGroupItem> GetExpectedFrequencyOfInAndOutTransactionsCheckGroup()
        {
            List<IInputGroupItem> retValues = new List<IInputGroupItem>();
            // Gets the custom table
            var lookupReasonsForOpeningTheAccount = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.EXPECTED_FREQUENCY_INCOMING_OUTGOING_TRANSACTION, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            if (lookupReasonsForOpeningTheAccount != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (lookupReasonsForOpeningTheAccount.Count() > 0)
                {
                    retValues.AddRange(lookupReasonsForOpeningTheAccount.Select(x => new InputGroupItemModel { Value = Convert.ToString(x.NodeGUID), Label = x.DocumentName }).ToList());
                }
            }
            return retValues;
        }
        internal static List<IInputGroupItem> GetHID()
        {
            List<IInputGroupItem> retValues = new List<IInputGroupItem>();
            //// Gets the custom table
            //var HIDValues = new MultiDocumentQuery()
            //         .OnCurrentSite()
            //         .Path(Constants.HID, PathTypeEnum.Children)
            //         .Culture(LocalizationContext.CurrentCulture.CultureCode)
            //         .PublishedVersion();

            //if (HIDValues != null)
            //{
            //    // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
            //    if (HIDValues.Count() > 0)
            //    {
            //        retValues.AddRange(HIDValues.Select(x => new InputGroupItemModel { Value = Convert.ToString(x.NodeGUID), Label = x.DocumentName }).ToList());
            //    }
            //}
            retValues.Add(new InputGroupItemModel { Value = "1", Label = "YES" });
            retValues.Add(new InputGroupItemModel { Value = "2", Label = "NO" });
            return retValues;
        }

        public static List<IInputGroupItem> SignatureMandateTypeGroup()
        {
            List<IInputGroupItem> retValues = new List<IInputGroupItem>();
            // Gets the custom table
            var signatureMandateTypes = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.SIGNATURE_MANDATE_TYPE, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            if (signatureMandateTypes != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (signatureMandateTypes.Count() > 0)
                {
                    retValues.AddRange(signatureMandateTypes.Select(x => new InputGroupItemModel { Value = Convert.ToString(x.NodeGUID), Label = x.DocumentName }).ToList());
                }
            }
            return retValues;
        }

        internal static List<SelectListItem> GetReasonsForOpeningTheAccount()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var lookupReasonsForOpeningTheAccount = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.PURPOSE_AND_REASON_FOR_OPENING_THE_ACCOUNT, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            if (lookupReasonsForOpeningTheAccount != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (lookupReasonsForOpeningTheAccount.Count() > 0)
                    if (lookupReasonsForOpeningTheAccount.Count() > 0)
                    {
                        retValues = lookupReasonsForOpeningTheAccount.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName.ToUpper() }).ToList();
                    }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetReasonsForOpeningTheAccountIndividual()
        {
            List<string> mylist = new List<string>(new string[] { "Credit Facilities", "Deposits / Savings", "Receive Dividends", "Income from Investments", "Investment Income", "Investment Purposes", "Household Expenses", "Payroll" });
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var lookupReasonsForOpeningTheAccount = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.PURPOSE_AND_REASON_FOR_OPENING_THE_ACCOUNT, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            if (lookupReasonsForOpeningTheAccount != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (lookupReasonsForOpeningTheAccount.Count() > 0)
                {
                    retValues = lookupReasonsForOpeningTheAccount.Where(x => mylist.Contains(x.DocumentName.Trim())).Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName.ToUpper() }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetExpectedNatureOfInAndOutTransactions()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var lookupReasonsForOpeningTheAccount = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.EXPECTED_NATURE_INCOMING_OUTGOING_TRANSACTION, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .Published();

            if (lookupReasonsForOpeningTheAccount != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (lookupReasonsForOpeningTheAccount.Count() > 0)
                {
                    retValues = lookupReasonsForOpeningTheAccount.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetExpectedNatureOfInAndOutTransactionsIndividual()
        {
            List<string> mylist = new List<string>(new string[] { "CASH", "CHEQUES", "ELECTRONIC FUND TRANSFERS", "INTERNAL TRANSFERS FROM/TO THIRD PARTIES" });
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var lookupReasonsForOpeningTheAccount = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.EXPECTED_NATURE_INCOMING_OUTGOING_TRANSACTION, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            if (lookupReasonsForOpeningTheAccount != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (lookupReasonsForOpeningTheAccount.Count() > 0)
                {
                    retValues = lookupReasonsForOpeningTheAccount.Where(x => mylist.Contains(x.DocumentName)).Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetExpectedFrequencyOfInAndOutTransactions()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var lookupReasonsForOpeningTheAccount = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.EXPECTED_FREQUENCY_INCOMING_OUTGOING_TRANSACTION, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            if (lookupReasonsForOpeningTheAccount != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (lookupReasonsForOpeningTheAccount.Count() > 0)
                {
                    retValues = lookupReasonsForOpeningTheAccount.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }

        #endregion

        internal static string GetApplicationNumber(int applicationID, string applicationType)
        {
            string zeroformat = "000000000";
            string applicationNoIndex = applicationID.ToString(zeroformat);
            return applicationType[0].ToString() + "-" + applicationNoIndex;
        }
        internal static string GetCountryName(string NodeGuid)
        {
            string addressName = string.Empty;
            var countries = CountryInfoProvider.GetCountries().WhereEquals("CountryGUID", ValidationHelper.GetGuid(NodeGuid, new Guid())).FirstOrDefault();
            if (countries != null)

            {
                addressName = countries.CountryDisplayName;


            }
            return addressName;
        }
        internal static string GetCountryNameById(int CountryID)
        {
            string addressName = string.Empty;
            var countries = CountryInfoProvider.GetCountries().WhereEquals("CountryID", ValidationHelper.GetInteger(CountryID, 0)).FirstOrDefault();
            if (countries != null)

            {
                addressName = countries.CountryDisplayName;


            }
            return addressName;
        }
        internal static string GetUserName(string NodeGuid)
        {
            string fullName = string.Empty;
            var responsibleOfficer = UserInfoProvider.GetUsers().WhereEquals("UserGUID", ValidationHelper.GetGuid(NodeGuid, new Guid())).FirstOrDefault();
            if (responsibleOfficer != null)
            {
                fullName = responsibleOfficer.FullName;
            }
            return fullName;
        }
        internal static string GetUserNameByID(int UserID)
        {
            string fullName = string.Empty;

            var responsibleOfficer = UserInfoProvider.GetUsers().WhereEquals("UserID", ValidationHelper.GetInteger(UserID, 0)).FirstOrDefault();
            if (responsibleOfficer != null)
            {

                fullName = responsibleOfficer.FullName;
            }
            return fullName;
        }
        public static string UploadFileToMedialibrary(string mediaLibraryName, string mediaLibraryFolderName, string originalFileName, string filename, string filePath, string fileTitle)
        {

            var physicalPath = filePath;
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
                                mediaFile.FileTitle = fileTitle;
                                mediaFile.FileDescription = fileTitle;
                                mediaFile.FilePath = "Eurobank/Image/";
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
                CMS.EventLog.EventLogProvider.LogException("Helper_DataAnnotation_ServiceHelper", "uploadFileToMedialibrary", ex);
            }
            return ParmanentPath;
        }

        internal static List<SelectListItem> GetLimitAmount()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.LIMIT_AMOUNT, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetCountryCodePrefix()
        {

            List<SelectListItem> retValues = new List<SelectListItem>();
            var result = LookupItemProvider.GetLookupItems().WhereContains("NodeAliasPath", Constants.COUNTRY_CODE_PREFIX).OrderBy("NodeOrder");
            if (result != null)
            {
                retValues = result.Select(x => new SelectListItem { Value = x.GetValue("LookupItemCode").ToString(), Text = x.DocumentName.ToUpper() }).ToList();
            }
            return retValues;
        }
        internal static string GetValue(string DocumentName, string path)
        {
            string name = string.Empty;
            var lookupName = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.COUNTRY_CODE_PREFIX, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion().WhereEquals("DocumentName", DocumentName).FirstOrDefault();
            if (lookupName != null)
            {
                name = Convert.ToString(lookupName.GetStringValue("LookupItemCode", ""));

            }

            return name;
        }

        internal static List<SelectListItem> GetFATCA_CLASSIFICATION()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.FATCA_CLASSIFICATION, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetUS_ENTITY_TYPE()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.US_ENTITY_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetTYPE_OF_FOREIGN_FINANCIAL_INSTITUTION()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.TYPE_OF_FOREIGN_FINANCIAL_INSTITUTION, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetTYPE_OF_NON_FINANCIAL_FOREIGN_ENTITY_NFFE()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.TYPE_OF_NON_FINANCIAL_FOREIGN_ENTITY_NFFE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetGLOBAL_INTERMEDIARY_IDENTIFICATION_NUMBER_GIIN()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.GLOBAL_INTERMEDIARY_IDENTIFICATION_NUMBER_GIIN, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetEXEMPTION_REASON()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.EXEMPTION_REASON, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetCRS_CLASSIFICATION()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.CRS_CLASSIFICATION, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetTYPE_OF_ACTIVE_NON_FINANCIAL_ENTITY_NFE()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            //var educationlavel = new MultiDocumentQuery()
            //        .OnCurrentSite()
            //        .Path(Constants.TYPE_OF_ACTIVE_NON_FINANCIAL_ENTITY_NFE, PathTypeEnum.Children)
            //        .Culture(LocalizationContext.CurrentCulture.CultureCode)
            //        .PublishedVersion();
            var educationlavel = LookupItemProvider.GetLookupItems().WhereContains("NodeAliasPath", Constants.TYPE_OF_ACTIVE_NON_FINANCIAL_ENTITY_NFE);
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.GetValue("LookupItemName").ToString() }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetNAME_OF_ESTABLISHED_SECURITES_MARKET()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.NAME_OF_ESTABLISHED_SECURITES_MARKET, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetTYPE_OF_FINANCIAL_INSITUTION()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.TYPE_OF_FINANCIAL_INSITUTION, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetPreferred_Mailing_Address()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.PREFERRED_MAILING_ADDRESS, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetAssociatedAccountType()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var lookupAddressType = new MultiDocumentQuery()
                     .OnCurrentSite()
                     .Path(Constants.Associated_Account_Type, PathTypeEnum.Children)
                     .Culture(LocalizationContext.CurrentCulture.CultureCode)
                     .PublishedVersion();

            if (lookupAddressType != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (lookupAddressType.Count() > 0)
                {
                    retValues = lookupAddressType.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetSignatoryGroup()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.SIGNATORY_GROUP, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetMandateType()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var educationlavel = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.MANDATE_TYPE, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (educationlavel != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (educationlavel.Count() > 0)
                {
                    retValues = educationlavel.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        internal static List<SelectListItem> GetCorporateJurisdiction()
        {
            List<SelectListItem> retValues = new List<SelectListItem>();
            // Gets the custom table
            var jurisdiction = new MultiDocumentQuery()
                    .OnCurrentSite()
                    .Path(Constants.Corporate_JURISDICTION, PathTypeEnum.Children)
                    .Culture(LocalizationContext.CurrentCulture.CultureCode)
                    .PublishedVersion();
            if (jurisdiction != null)
            {
                // Gets all data records from the custom table whose 'ItemText' field value starts with 'New text'
                if (jurisdiction.Count() > 0)
                {
                    retValues = jurisdiction.Select(x => new SelectListItem { Value = Convert.ToString(x.NodeGUID), Text = x.DocumentName }).ToList();
                }
            }
            return retValues;
        }
        //internal static DataTable SortDataTable(DataTable dt, string colName, string direction)
        //{
        //    try
        //    {
        //        direction = (string.Equals(direction, "Ascending", StringComparison.OrdinalIgnoreCase) ? "ASC" : "DESC");
        //        dt.DefaultView.Sort = colName + " " + direction;
        //        dt = dt.DefaultView.ToTable();
        //        return dt;
        //    }
        //    catch
        //    {
        //        return new DataTable();
        //    }

        //}
        internal static string GetFilterColumnName(string filterColumnName)
        {
            string retval = string.Empty;
            switch (filterColumnName)
            {
                case "ApplicationDetails_ApplicationTypeName":
                    retval = "Search_Type";
                    break;
                case "FullNameOfApplicant":
                    retval = "Search_Full_Name";
                    break;
                case "ApplicationDetails_Introducer":
                    retval = "Search_Introducer";
                    break;
                case "ApplicationDetails_ResponsibleBankingCenterUnit":
                    retval = "Search_Bank_Center";
                    break;
                case "ApplicationDetails_SubmittedBy":
                    retval = "Search_Submitted_By";
                    break;
                case "ApplicationDetails_SubmittedOn":
                    retval = "Search_Submitted_On";
                    break;
                case "ApplicationDetails_CreatedOn":
                    retval = "Search_Created_On";
                    break;
                case "ApplicationDetails_ApplicationStatusName":
                    retval = "Search_Status";
                    break;
                default:
                    retval = filterColumnName;
                    break;
            }
            return retval;
        }
        internal static int ConvertDateStringToInt(string dateString) //dd/MM/yyyy
        {
            // Split the date string into day, month, and year components
            string[] dateComponents = dateString.Split('/');
            int day = int.Parse(dateComponents[0]);
            int month = int.Parse(dateComponents[1]);
            int year = int.Parse(dateComponents[2]);

            // Combine the components into a single integer in the yyyymmdd format
            int dateInt = (year * 10000) + (month * 100) + day;
            return dateInt;
        }

        internal static string GetIntroducerEmailByName(string name)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(name) && name.IndexOf('-') > -1)
            {
                name = name.Substring(name.IndexOf('-') + 1);
            }

            var intermediaries = IntermediaryProvider.GetIntermediaries().Where(x => string.Equals(x.DocumentName, name, StringComparison.OrdinalIgnoreCase));
            if (intermediaries != null)
            {
                retval = intermediaries.Select(x => x.Email).FirstOrDefault();
            }

            return retval;
        }

        internal static bool IsCountryEU(int? CountryId)
        {
            bool retval = false;
            string customTableClassName = "Eurobank.CountiesCustomFields";
            if (CountryId > 0)
            {
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (customTable != null)
                {
                    var customTableData = CustomTableItemProvider.GetItems(customTableClassName).WhereEquals("CountryID", CountryId).And().WhereEquals("EUFLAG", "Y");
                    if (customTableData != null && customTableData.Any())
                    {
                        retval = true;
                    }
                }
            }

            return retval;
        }

        internal static string RemoveSpecialCharacters(string input)
        {
            // Define a regular expression pattern to match special characters
            string pattern = "[^a-zA-Z0-9]";

            // Use Regex.Replace to replace all occurrences of special characters with an empty string
            string result = Regex.Replace(input, pattern, "");

            return result;
        }

    }
}
