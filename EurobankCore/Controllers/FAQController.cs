using Eurobank.Models;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eurobank.Models.FAQ;
using Kentico.Content.Web.Mvc.Routing;
using Eurobank.Controllers;
using Microsoft.AspNetCore.Authorization;

[assembly: RegisterPageRoute(CMS.DocumentEngine.Types.Eurobank.FAQ.CLASS_NAME, typeof(FAQController))]

namespace Eurobank.Controllers
{
    [Authorize]
    public class FAQController : Controller
    {
        private readonly IPageDataContextRetriever dataRetriever;
        private readonly IPageAttachmentUrlRetriever pageAttachmentUrlRetriever;
        private readonly FAQItemRepository afaqItemRepository;
        private readonly ReferenceRepository referenceRepository;


        public FAQController(IPageDataContextRetriever dataRetriever,
            IPageAttachmentUrlRetriever pageAttachmentUrlRetriever,
            FAQItemRepository faqItemRepository,
            ReferenceRepository referenceRepository)
        {
            this.dataRetriever = dataRetriever;
            this.pageAttachmentUrlRetriever = pageAttachmentUrlRetriever;
            this.afaqItemRepository = faqItemRepository;
            this.referenceRepository = referenceRepository;
        }
        public ActionResult Index()
        {
            FaqViewModel FaqViewModel = new FaqViewModel();
            var faqPage = dataRetriever.Retrieve<CMS.DocumentEngine.Types.Eurobank.FAQ>().Page;
            var faqQuestions = afaqItemRepository.GetFaqItemAsync(faqPage.NodeAliasPath);
            if (faqQuestions != null)
            {
                //var faqListQuestions = afaqItemRepository.GetFaqItemAsync(faqPage.NodeAliasPath).Result;

                //List<Faq> faqList = new List<Faq>();
                //foreach (var item in faqQuestions)
                //{
                //    Faq faq = new Faq();
                //    faq.Question = item.GetValue("Questions", "");
                //    faq.Answer = item.GetValue("Answers", "");
                //    faq.NodeId = item.NodeID.ToString();
                //    faqList.Add(faq);
                //}
                //FaqViewModel.FaqList = faqList;
                FaqViewModel.Name = faqPage.GetValue("Name", "");
                FaqViewModel.VisitUs = faqPage.GetValue("VisitUs", "");
                FaqViewModel.Email = faqPage.GetValue("EmailUs", "");
                FaqViewModel.ContactUs = faqPage.GetValue("CallUs", "");
                ViewBag.Title = faqPage.GetValue("Name", "");


            }
            return View(FaqViewModel);
        }
    }
}
