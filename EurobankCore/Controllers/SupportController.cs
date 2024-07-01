using Eurobank.Controllers;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eurobank.Models;

[assembly: RegisterPageRoute(CMS.DocumentEngine.Types.Eurobank.Support.CLASS_NAME, typeof(SupportController))]
namespace Eurobank.Controllers
{
    public class SupportController : Controller
    {
        private readonly IPageDataContextRetriever dataRetriever;
        private readonly IPageAttachmentUrlRetriever pageAttachmentUrlRetriever;
        public SupportController(IPageDataContextRetriever dataRetriever,
            IPageAttachmentUrlRetriever pageAttachmentUrlRetriever
    )
        {
            this.dataRetriever = dataRetriever;
            this.pageAttachmentUrlRetriever = pageAttachmentUrlRetriever;
           
        }
        public ActionResult Index()
        {
            TempData["Active"] = true;
         var supportPage = dataRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.Support>().Page;

            SupportViewModel SupportViewModel = new SupportViewModel();

            SupportViewModel.Name = supportPage.DocumentName;

            return View(SupportViewModel);
        }
    }
}
