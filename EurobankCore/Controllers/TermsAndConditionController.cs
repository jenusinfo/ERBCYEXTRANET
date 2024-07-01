using CMS.DocumentEngine.Types.Eurobank;
using Eurobank.Controllers;
using Eurobank.Helpers.CustomHandler;
using Eurobank.Models.TermsAndCondition;
using Eurobank.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
[assembly: RegisterPageRoute(CMS.DocumentEngine.Types.Eurobank.TermsAndCondition.CLASS_NAME, typeof(TermsAndConditionController))]
namespace Eurobank.Controllers
{
    [AuthorizeRoles(Role.PowerUser, Role.NormalUser)]
    [SessionAuthorization]
    public class TermsAndConditionController : Controller
	{
		private readonly IPageDataContextRetriever dataRetriever;
		private readonly IPageAttachmentUrlRetriever pageAttachmentUrlRetriever;
		private readonly TermsAndConditionRepository termsAndConditionRepository;


		public TermsAndConditionController(IPageDataContextRetriever dataRetriever,
			IPageAttachmentUrlRetriever pageAttachmentUrlRetriever,
			TermsAndConditionRepository termsAndConditionRepository)
		{
			this.dataRetriever = dataRetriever;
			this.pageAttachmentUrlRetriever = pageAttachmentUrlRetriever;
			this.termsAndConditionRepository = termsAndConditionRepository;
		}
		public IActionResult Index()
		{
			var termsAndConditionPage = dataRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.TermsAndCondition>().Page;
			//var termsAndConditionPage = termsAndConditionRepository.GetTermsAndConditionPage();
			
			return View(termsAndConditionPage);
		}
	}
}
