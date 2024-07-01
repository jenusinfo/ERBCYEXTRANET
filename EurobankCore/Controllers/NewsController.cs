using CMS.DocumentEngine.Types.Eurobank;
using Eurobank.Controllers;
using Eurobank.Models;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[assembly: RegisterPageRoute(CMS.DocumentEngine.Types.Eurobank.NewsSection.CLASS_NAME, typeof(NewsController))]
[assembly: RegisterPageRoute(CMS.DocumentEngine.Types.Eurobank.News.CLASS_NAME, typeof(NewsController), ActionName = nameof(NewsController.Details))]

namespace Eurobank.Controllers
{
	public class NewsController : Controller
	{
		private readonly IPageDataContextRetriever dataRetriever;
		private readonly IPageAttachmentUrlRetriever pageAttachmentUrlRetriever;
		private readonly NewsRepository aNewsRepository;
		private readonly ReferenceRepository referenceRepository;
		private readonly NewsSectionRepository anewsSectionRepository;


		public NewsController(IPageDataContextRetriever dataRetriever,
			IPageAttachmentUrlRetriever pageAttachmentUrlRetriever,
			NewsRepository NewsRepository,
			ReferenceRepository referenceRepository, NewsSectionRepository newsSectionRepository)
		{
			this.dataRetriever = dataRetriever;
			this.pageAttachmentUrlRetriever = pageAttachmentUrlRetriever;
			this.aNewsRepository = NewsRepository;
			this.referenceRepository = referenceRepository;
			this.anewsSectionRepository = newsSectionRepository;
		}

		public ActionResult Index()
		{
			var newsPage = anewsSectionRepository.GetNewsSectionPage();

			var news = aNewsRepository.GetNewsList(newsPage.NodeAliasPath);
			
			return View(news);
		}


		public ActionResult Details(string id)
		{
			var newsPage = anewsSectionRepository.GetNewsSectionPage();

			if(string.IsNullOrEmpty(id))
			{
				var news = dataRetriever.Retrieve<News>().Page;
				id = news.NodeAlias;
			}

			// var newsDetails = aNewsRepository.GetNewsAsync(newsPage.NodeAliasPath, NodeAlias);
			var newsDetails = aNewsRepository.GetNewsAsync(newsPage.NodeAliasPath, id);
			NewsViewModel NewswViewModel = new NewsViewModel();
			if(newsDetails != null)
			{

				NewswViewModel.Title = newsDetails.GetValue("NewsTitle", "");
				NewswViewModel.Teaser = newsDetails.GetValue("NewsTeaser", "");
				NewswViewModel.NewsSummary = newsDetails.GetValue("NewsSummary", "");
				NewswViewModel.NewsLongDescription = newsDetails.GetValue("NewsLongDescription", "");
				NewswViewModel.RelatedNews = newsDetails.GetValue("NewsRelatedNews", "");
			}

			return View(NewswViewModel);
		}
	}
}
