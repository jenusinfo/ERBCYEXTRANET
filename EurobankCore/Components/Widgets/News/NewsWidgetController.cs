using Eurobank.Models;
using Eurobank.Widgets;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Components.Widgets.News
{
    public class NewsWidgetController : Controller
    {
        public const string IDENTIFIER = "NewsSection";
        private readonly NewsRepository repository;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;
        public NewsWidgetController(NewsRepository repository, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            this.repository = repository;
            this.pageUrlRetriever = pageUrlRetriever;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetNews(NewsPathSelectorModel newsPathSelectorModel)
        {
           if(newsPathSelectorModel.NodeAliasPath != "")
            {
                var news = repository.GetNewsList(newsPathSelectorModel.NodeAliasPath);
                var NewsList = news.Select(x => NewsViewModel.GetViewModel(x, pageUrlRetriever, attachmentUrlRetriever));
                
                return View("~/Components/Widgets/News/_NewsWidget.cshtml", new NewsWidgetViewModel { NewsList = NewsList, Count = newsPathSelectorModel.count, ViewAllLink= newsPathSelectorModel.ViewAllPagelink });
               
            }
            return View("~/Components/Widgets/News/_NewsWidget.cshtml", new NewsWidgetViewModel { NewsList = null, Count = newsPathSelectorModel.count, ViewAllLink = newsPathSelectorModel.ViewAllPagelink });

        }
    }
}
