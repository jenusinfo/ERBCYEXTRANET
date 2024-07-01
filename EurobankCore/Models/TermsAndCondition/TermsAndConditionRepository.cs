using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.TermsAndCondition
{
	public class TermsAndConditionRepository
	{
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        public TermsAndConditionRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageRetriever = pageRetriever;
        }
        public CMS.DocumentEngine.Types.Eurobank.TermsAndCondition GetTermsAndConditionPage()
        {
            return pageRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.TermsAndCondition>(
                query => query
                    //.Path(nodeAliasPath, PathTypeEnum.Children)
                    //.TopN(count)
                    .OrderByDescending("DocumentPublishFrom")
               ).First();
        }
    }
}
