
using Eurobank.Models.FAQ;
using Eurobank.Widgets;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;




[assembly: RegisterWidget(FAQWidgetViewComponent.IDENTIFIER, typeof(FAQWidgetViewComponent), "FAQ", typeof(FAQWidgetProperties), Description = "Displays the latest faq from the Eurobank site.", IconClass = "icon-l-list-article")]

namespace Eurobank.Widgets
{
    public class FAQWidgetViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "faq";


        private readonly FAQItemRepository repository;
        private readonly FAQRepository faqrepository;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;
        public FAQWidgetViewComponent(FAQItemRepository repository, FAQRepository faqrepository, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            this.repository = repository;
            this.faqrepository = faqrepository;
            this.pageUrlRetriever = pageUrlRetriever;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
        }

        public ViewViewComponentResult Invoke(ComponentViewModel<FAQWidgetProperties> viewModel)
        {
            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var faq = repository.GetFaqItem(viewModel.Properties.LinkUrl != null ? viewModel.Properties.LinkUrl.TrimStart('~') : ContentItemIdentifiers.FAQ, viewModel.Properties.Count);
            var faqlist = faq.Select(x => FaqViewModel.GetViewModel(x, pageUrlRetriever, attachmentUrlRetriever));

           // var faqpage = faqrepository.GetFaqPageAsync();
            var faqpage = faqrepository.GetFAQAsync("FAQ");
            ViewBag.Name = faqpage.DocumentName;
            ViewBag.ContactUs = faqpage.CallUs;
            ViewBag.EmailUs = faqpage.EmailUs;
            ViewBag.VisitUs = faqpage.VisitUs;

            return View("~/Components/Widgets/FAQ/_FAQWidget.cshtml", new FAQWidgetViewModel { FAQList = faqlist, Count = viewModel.Properties.Count });
        }
    }
}
