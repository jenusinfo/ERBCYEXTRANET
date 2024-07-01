using System.Linq;

using CMS.DocumentEngine.Types.Eurobank;
using CMS.Helpers;
using CMS.Search;

using Kentico.Content.Web.Mvc;

namespace Eurobank.Models
{
    public class SearchResultAboutUsItemModel : SearchResultPageItemModel
    {
        public SearchResultAboutUsItemModel(SearchResultItem resultItem, AboutUs aboutUs, AboutUsRepository aboutUsRepository, IPageUrlRetriever pageUrlRetriever)
            : base(resultItem, aboutUs, pageUrlRetriever)
        {
            var sideStories = aboutUsRepository.GetSideStories(aboutUs.NodeAliasPath);
            Content = string.Join(" ", sideStories.Select(story => HTMLHelper.StripTags(story.AboutUsSectionText, false)));
        }
    }
}