using CMS.DocumentEngine.Types.Eurobank;

using Eurobank.PageTemplates;

using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

[assembly: RegisterPageTemplate("Eurobank.ArticleWithSidebar", "Article with sidebar", typeof(ArticleWithSideBarProperties), "~/PageTemplates/ArticleWithSidebar/_ArticleWithSidebar.cshtml", Description = "Displays an article detail with related articles on the right side.", IconClass = "icon-l-text-col")]

namespace Eurobank.PageTemplates
{
    public class ArticleWithSidebarPageTemplateService
    {
        private readonly IPageDataContextRetriever pageDataContextRetriver;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;


        public ArticleWithSidebarPageTemplateService(IPageDataContextRetriever pageDataContextRetriver, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            this.pageDataContextRetriver = pageDataContextRetriver;
            this.pageUrlRetriever = pageUrlRetriever;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
        }


        public ArticleWithSideBarViewModel GetTemplateModel(ArticleWithSideBarProperties templateProperties)
        {
            var article = pageDataContextRetriver.Retrieve<Article>().Page;
            return ArticleWithSideBarViewModel.GetViewModel(article, templateProperties, pageUrlRetriever, attachmentUrlRetriever);
        }
    }
}