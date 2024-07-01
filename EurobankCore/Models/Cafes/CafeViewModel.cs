using CMS.DocumentEngine.Types.Eurobank;

using Kentico.Content.Web.Mvc;

using Microsoft.Extensions.Localization;

namespace Eurobank.Models
{
    public class CafeViewModel
    {
        public string PhotoPath { get; set; }


        public string Note { get; set; }


        public ContactViewModel Contact { get; set; }


        public static CafeViewModel GetViewModel(Cafe cafe, CountryRepository countryRepository, IStringLocalizer<SharedResources> localizer, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            return new CafeViewModel
            {
                PhotoPath = cafe.Fields.Photo == null ? null : attachmentUrlRetriever.Retrieve(cafe.Fields.Photo).RelativePath,
                Note = cafe.Fields.AdditionalNotes,
                Contact = ContactViewModel.GetViewModel(cafe, countryRepository, localizer)
            };
        }
    }
}