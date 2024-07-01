using CMS.DocumentEngine.Types.Eurobank;

using Kentico.Content.Web.Mvc;

namespace Eurobank.Models
{
    public class ManufactureListItemViewModel
    {
        public string Url { get; }


        public string Name { get; }


        public string ImagePath { get; }


        public ManufactureListItemViewModel(Manufacturer manufacturer, IPageUrlRetriever pageUrlRetriever)
        {
            Name = manufacturer.DocumentName;
            ImagePath = new FileUrl(manufacturer.Logo, true).RelativePath;
            Url = pageUrlRetriever.Retrieve(manufacturer).RelativePath;
        }
    }
}