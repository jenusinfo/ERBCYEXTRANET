using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.Eurobank;

using Kentico.Content.Web.Mvc;

namespace Eurobank.Models.FAQ
{
    public class FAQRepository
    {

        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextRetriever dataRetriever;
        public FAQRepository(IPageRetriever pageRetriever, IPageDataContextRetriever dataRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.dataRetriever = dataRetriever;
        }
        //public IEnumerable<Faq> GetFaqPageAsync()
        //{
        //    return pageRetriever.Retrieve<Faq>();

        //}
        public CMS.DocumentEngine.Types.Eurobank.FAQ GetFaqPageAsync()
        {
            return dataRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.FAQ>().Page;

        }
        public CMS.DocumentEngine.Types.Eurobank.FAQ GetFAQAsync(string NewsAlias)
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.FAQ>(
                query => query
                  //.WhereEquals(NodeAlias, PathTypeEnum.Children)
                    .OrderBy("NodeOrder")
                    .WhereEquals("NodeAlias", NewsAlias)
               ).First();



        }

    }
}
