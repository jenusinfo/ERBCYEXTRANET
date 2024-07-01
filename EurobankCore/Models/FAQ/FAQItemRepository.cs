using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eurobank.Models.FAQ
{
    public class FAQItemRepository
    {
        private readonly IPageRetriever pageRetriever;
        public FAQItemRepository(IPageRetriever pageRetriever)
        {
            this.pageRetriever = pageRetriever;
        }

        //public Task<IEnumerable<FAQItem>> GetFaqItemAsync(string nodeAliasPath)
        //{
        //    return pageRetriever.RetrieveAsync<FAQItem>(
        //        query => query
        //            .Path(nodeAliasPath, PathTypeEnum.Children)
        //            .OrderBy("NodeOrder"),
        //        cache => cache
        //            .Key($"{nameof(FAQItemRepository)}|{nameof(GetFaqItemAsync)}|{nodeAliasPath}")
        //            // Include path dependency to flush cache when a new child page is created or page order is changed.
        //            .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children).PageOrder())
        //        );
        //}

        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.FAQItem> GetFaqItemAsync(string nodeAliasPath)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.FAQItem>(
                query => query
                  .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder")

               );



        }
        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.FAQItem> GetFaqItem(string nodeAliasPath,int count)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.FAQItem>(
                query => query
                  .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder").TopN(count)

               );



        }
    }
}
