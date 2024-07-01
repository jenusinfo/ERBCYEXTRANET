using Eurobank.Controllers;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
[assembly: RegisterPageRoute(CMS.DocumentEngine.Types.Eurobank.PrivacyAndPolicy.CLASS_NAME, typeof(PrivacyPolicyController))]
namespace Eurobank.Controllers
{
	public class PrivacyPolicyController : Controller
	{
		private readonly IPageDataContextRetriever dataRetriever;
		private readonly IPageAttachmentUrlRetriever pageAttachmentUrlRetriever;


		public PrivacyPolicyController(IPageDataContextRetriever dataRetriever,
			IPageAttachmentUrlRetriever pageAttachmentUrlRetriever)
		{
			this.dataRetriever = dataRetriever;
			this.pageAttachmentUrlRetriever = pageAttachmentUrlRetriever;
		}
		public IActionResult Index()
		{
			var privacyAndPolicy = dataRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.PrivacyAndPolicy>().Page;
			//var termsAndConditionPage = termsAndConditionRepository.GetTermsAndConditionPage();

			return View(privacyAndPolicy);
		}
	}
}
