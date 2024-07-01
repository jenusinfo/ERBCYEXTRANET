using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Widgets
{
    public class WhatsNewsWidgetProperties : IWidgetProperties
    {
        public int Count { get; set; } = 5;

        public string NodeAliasPath { get; set; }

        [EditingComponent(UrlSelector.IDENTIFIER, Order = 1, Label = "Node Alias Path")]
        [EditingComponentProperty(nameof(UrlSelectorProperties.Placeholder), "Please enter a URL or select a page...")]
        [EditingComponentProperty(nameof(UrlSelectorProperties.Tabs), ContentSelectorTabs.Page)]
        public string LinkUrl { get; set; }


        /// <summary>
        /// Indicates if link should be opened in a new tab.
        /// </summary>
        [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 2, Label = "Open in a new tab")]
        public bool OpenInNewTab { get; set; }
    }
}
