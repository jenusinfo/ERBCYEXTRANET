using Eurobank.Controllers;
using Eurobank.Models;
using Eurobank.Models.WhatsNew;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
[assembly: RegisterPageRoute(CMS.DocumentEngine.Types.Eurobank.WhatsNewSection.CLASS_NAME, typeof(WhatsNewController))]
[assembly: RegisterPageRoute(CMS.DocumentEngine.Types.Eurobank.WhatsNew.CLASS_NAME, typeof(WhatsNewController), ActionName = nameof(WhatsNewController.Details))]
namespace Eurobank.Controllers
{
	public class WhatsNewController : Controller
	{
		private readonly IPageDataContextRetriever dataRetriever;
		private readonly IPageAttachmentUrlRetriever pageAttachmentUrlRetriever;
		private readonly WhatsNewSectionRepository whatsNewSectionRepository;
		private readonly ReferenceRepository referenceRepository;
		private readonly WhatsNewRepository whatsNewRepository	;


		public WhatsNewController(IPageDataContextRetriever dataRetriever,
			IPageAttachmentUrlRetriever pageAttachmentUrlRetriever,
			WhatsNewSectionRepository whatsNewSectionRepository,
			ReferenceRepository referenceRepository, WhatsNewRepository whatsNewRepository)
		{
			this.dataRetriever = dataRetriever;
			this.pageAttachmentUrlRetriever = pageAttachmentUrlRetriever;
			this.whatsNewSectionRepository = whatsNewSectionRepository;
			this.referenceRepository = referenceRepository;
			this.whatsNewRepository = whatsNewRepository;
		}
		public IActionResult Index()
		{
			var newsPage = whatsNewSectionRepository.GetWhatsNewsSectionPage();
			var news = whatsNewRepository.GetWhatsNew(newsPage.NodeAliasPath);
			return View(news);
		}
			
		public IActionResult Details(string id)
		{
			var whatsnewsDetails = whatsNewRepository.GetWhatsNewsAsync(id);
			WhatsNewViewModel whatsNewViewModel = new WhatsNewViewModel();
			if(whatsnewsDetails != null)
			{

				whatsNewViewModel.Title = whatsnewsDetails.GetValue("Name", "");
				whatsNewViewModel.Teaser = whatsnewsDetails.GetValue("Teaser", "");
			}

			return View(whatsNewViewModel);
		}
	}
}
