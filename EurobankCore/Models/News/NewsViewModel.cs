using CMS.DocumentEngine.Types.Eurobank;
using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models
{
    public class NewsViewModel
    {
        public string Title { get; set; }
        public string Teaser { get; set; }
        public string NewsSummary { get; set; }
        public string NewsLongDescription { get; set; }

        public string NodeAlias { get; set; }
        public string RelatedNews { get; set; }

        public string PublishedMonth { get; set; }
        public string PublishedYear{ get; set; }
        public string PublishedTime{ get; set; }

        public static NewsViewModel GetViewModel(News news, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            return new NewsViewModel
            {

                Title = news.NewsTitle,
                Teaser = news.NewsTeaser.ToString(),
                NewsSummary = news.NewsSummary,
                NewsLongDescription = news.NewsLongDescription,
                NodeAlias= news.NodeAlias,
                //PublishedMonth=news.DocumentPublishFrom.ToString("MMMM dd yyyy"),
                PublishedMonth=news.DocumentPublishFrom.ToString("MMMM dd"),
                PublishedYear = news.DocumentPublishFrom.Date.Year.ToString(),
                PublishedTime= news.DocumentPublishFrom.ToString("hh:mm tt")
            };
        }
    }
}
