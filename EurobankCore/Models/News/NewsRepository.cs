using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;
using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models
{
    public class NewsRepository
    {
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        public NewsRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageRetriever = pageRetriever;
        }

        public CMS.DocumentEngine.Types.Eurobank.News GetNewsAsync(string nodeAliasPath,string NewsAlias)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.News>(
                query => query
                  .Path(nodeAliasPath, PathTypeEnum.Children)
                    .OrderBy("NodeOrder")
                    .WhereEquals("NodeAlias", NewsAlias)
               ).First();



        }

        public IEnumerable<CMS.DocumentEngine.Types.Eurobank.News> GetNewsList(string nodeAliasPath, int count = 0)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.News>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .TopN(count)
                    .OrderByDescending("DocumentPublishFrom"),
                cache => cache
                    .Key($"{nameof(NewsRepository)}|{nameof(GetNewsList)}|{nodeAliasPath}|{count}")
                    // Include path dependency to flush cache when a new child page is created.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children)));
        }

        public CMS.DocumentEngine.Types.Eurobank.NewsSection GetNewsSection()
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.NewsSection>(
                query => query
                    //.Path(nodeAliasPath, PathTypeEnum.Children)
                    //.TopN(count)
                    .OrderByDescending("DocumentPublishFrom")
               ).First();
        }
        /// <summary>
        /// Returns current article.
        /// </summary>
        //public virtual CMS.DocumentEngine.Types.Eurobank.News GetCurrent()
        //{
        //    var page = pageDataContextRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.NewsSection>().Page;

        //    return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.News>(
        //        query => query
        //            .WhereEquals("NodeID", page.NodeID),
        //        cache => cache
        //            .Key($"{nameof(NewsRepository)}|{nameof(GetCurrent)}|{page.NodeID}")
        //            .Dependencies((articles, deps) => deps.Pages(CMS.DocumentEngine.Types.Eurobank.News.Fields.RelatedArticles)))
        //        .First();
        //}
    }
}
