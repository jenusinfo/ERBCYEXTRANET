using Eurobank.Models;
using Eurobank.Widgets;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


[assembly: RegisterWidget(NewsWidgetViewComponenet.IDENTIFIER, typeof(NewsWidgetViewComponenet), "News", typeof(NewsWidgetProperties), Description = "Displays the latest news from the Eurobank site.", IconClass = "icon-l-list-article")]

namespace Eurobank.Widgets
{
    public class NewsWidgetViewComponenet : ViewComponent
    {
        public const string IDENTIFIER = "NewsSection";
        private readonly NewsRepository repository;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;
        private readonly IPageDataContextRetriever dataRetriever;
        public NewsWidgetViewComponenet(NewsRepository repository, IPageDataContextRetriever dataRetriever, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            this.repository = repository;
            this.pageUrlRetriever = pageUrlRetriever;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
            this.dataRetriever = dataRetriever;

        }

        public ViewViewComponentResult Invoke(ComponentViewModel<NewsWidgetProperties> viewModel)
        {
            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }
            var NewsPage = repository.GetNewsSection();
            var news = repository.GetNewsList(viewModel.Properties.NodeAliasPath!= null ? viewModel.Properties.NodeAliasPath.TrimStart('~'): ContentItemIdentifiers.NEWS, viewModel.Properties.Count);
            var NewsList = news.Select(x => NewsViewModel.GetViewModel(x, pageUrlRetriever, attachmentUrlRetriever));
            //NewsPathSelectorModel newsPathSelectorModel = new NewsPathSelectorModel();
            //newsPathSelectorModel.count = viewModel.Properties.Count;
           // return View("~/Components/Widgets/News/_NewsPathSelector.cshtml", newsPathSelectorModel);
            return View("~/Components/Widgets/News/_NewsWidget.cshtml", new NewsWidgetViewModel { NewsList = NewsList, Count = viewModel.Properties.Count, ViewAllLink= viewModel.Properties.ViewAllLinkUrl != null ? viewModel.Properties.ViewAllLinkUrl: NewsPage.NodeAliasPath });
        }

    }
}
