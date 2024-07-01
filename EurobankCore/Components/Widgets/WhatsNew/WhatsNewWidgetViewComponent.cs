using Eurobank.Models.WhatsNew;
using Eurobank.Widgets;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


[assembly: RegisterWidget(WhatsNewWidgetViewComponent.IDENTIFIER, typeof(WhatsNewWidgetViewComponent), "Whats New", typeof(WhatsNewsWidgetProperties), Description = "Displays the latest articles from the Eurobank site.", IconClass = "icon-l-list-article")]

namespace Eurobank.Widgets
{
    public class WhatsNewWidgetViewComponent : ViewComponent
    {
    
        public const string IDENTIFIER = "WhatsNews";


        private readonly WhatsNewRepository repository;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;
        public WhatsNewWidgetViewComponent(WhatsNewRepository repository, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            this.repository = repository;
            this.pageUrlRetriever = pageUrlRetriever;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
        }

        public ViewViewComponentResult Invoke(ComponentViewModel<WhatsNewsWidgetProperties> viewModel)
        {
            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var whatsNew = repository.GetWhatsNew(viewModel.Properties.LinkUrl != null ? viewModel.Properties.LinkUrl.TrimStart('~') : ContentItemIdentifiers.USEFULLINKS, viewModel.Properties.Count);
            var WhatsNewData = whatsNew.Select(x => WhatsNewViewModel.GetViewModel(x, pageUrlRetriever, attachmentUrlRetriever));

            return View("~/Components/Widgets/WhatsNew/_WhatsNewWidgets.cshtml", new WhatsNewWidgetViewModel { WhatsNewList = WhatsNewData, Count = viewModel.Properties.Count });
        }

    }
}
