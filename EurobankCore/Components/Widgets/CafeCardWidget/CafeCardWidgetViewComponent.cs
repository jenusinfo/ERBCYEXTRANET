﻿using System.Linq;

using Eurobank.Models;
using Eurobank.Widgets;

using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

[assembly: RegisterWidget(CafeCardWidgetViewComponent.IDENTIFIER, typeof(CafeCardWidgetViewComponent), "Cafe", typeof(CafeCardProperties), Description = "Displays a cafe with its background image.", IconClass = "icon-cup")]

namespace Eurobank.Widgets
{
    /// <summary>
    /// Controller for cafe card widget.
    /// </summary>
    public class CafeCardWidgetViewComponent : ViewComponent
    {
        /// <summary>
        /// Widget identifier.
        /// </summary>
        public const string IDENTIFIER = "Eurobank.HomePage.CafeCardWidget";


        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;
        private readonly CafeRepository repository;


        /// <summary>
        /// Initializes an instance of <see cref="CafeCardWidgetViewComponent"/> class.
        /// </summary>
        public CafeCardWidgetViewComponent( 
            IPageAttachmentUrlRetriever attachmentUrlRetriever,
            CafeRepository repository)
        {
            this.attachmentUrlRetriever = attachmentUrlRetriever;
            this.repository = repository;
        }


        public ViewViewComponentResult Invoke(CafeCardProperties properties)
        {
            var selectedPage = properties.SelectedCafes.FirstOrDefault();
            var cafe = (selectedPage != null) ? repository.GetCafeByGuid(selectedPage.NodeGuid) : null;

            return View("~/Components/Widgets/CafeCardWidget/_CafeCardWidget.cshtml", CafeCardViewModel.GetViewModel(cafe, attachmentUrlRetriever));
        }
    }
}