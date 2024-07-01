using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models
{
    public class NewsSectionRepository
    {
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        public NewsSectionRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageRetriever = pageRetriever;
        }
        public CMS.DocumentEngine.Types.Eurobank.NewsSection GetNewsSectionPage()
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.NewsSection>(
                query => query
                    //.Path(nodeAliasPath, PathTypeEnum.Children)
                    //.TopN(count)
                    .OrderByDescending("DocumentPublishFrom")
               ).First();
        }
    }
}
