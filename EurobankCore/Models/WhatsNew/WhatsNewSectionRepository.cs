using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.WhatsNew
{
	public class WhatsNewSectionRepository
	{
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        public WhatsNewSectionRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageRetriever = pageRetriever;
        }
        public CMS.DocumentEngine.Types.Eurobank.WhatsNewSection GetWhatsNewsSectionPage()
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.WhatsNewSection>(
                query => query
                    //.Path(nodeAliasPath, PathTypeEnum.Children)
                    //.TopN(count)
                    .OrderByDescending("DocumentPublishFrom")
               ).First();
        }
    }
}
