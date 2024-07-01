//using Eurobank.Widgets;
using Eurobank.Models.UseFulLinks;
using Eurobank.Widgets;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


[assembly: RegisterWidget(UseFulLinksWidgetViewComponent.IDENTIFIER, typeof(UseFulLinksWidgetViewComponent), "Use Ful Links", typeof(UseFulLinksWidgetProperties), Description = "Displays the latest articles from the Eurobank site.", IconClass = "icon-l-list-article")]

//[assembly: RegisterWidget(UseFulLinksWidgetViewComponent.IDENTIFIER, "Use Ful Links", typeof(UseFulLinksWidgetProperties), "~/Components/Widgets/UseFulLinks/_UseFulLinksWidgets.cshtml", Description = "Call to action button with configurable target page.", IconClass = "icon-rectangle-a")]


namespace Eurobank.Widgets
{
    public class UseFulLinksWidgetViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "UseFulLinks";


        private readonly UseFulLinksRepository repository;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;
        public UseFulLinksWidgetViewComponent(UseFulLinksRepository repository, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            this.repository = repository;
            this.pageUrlRetriever = pageUrlRetriever;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
        }

        public ViewViewComponentResult Invoke(ComponentViewModel<UseFulLinksWidgetProperties> viewModel)
        {
            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var links = repository.GetUseFulLinks(viewModel.Properties.LinkUrl!= null? viewModel.Properties.LinkUrl.TrimStart('~'):ContentItemIdentifiers.USEFULLINKS, viewModel.Properties.Count);
            var UseFulLink = links.Select(x => UseFulLinkViewModel.GetViewModel(x, pageUrlRetriever, attachmentUrlRetriever));
           
            return View("~/Components/Widgets/UseFulLinks/_UseFulLinksWidgets.cshtml", new UseFulLinksWidgetViewModel { UseFulLink = UseFulLink, Count = viewModel.Properties.Count });
        }

    }
}
