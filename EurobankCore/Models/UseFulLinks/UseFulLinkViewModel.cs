using CMS.DocumentEngine.Types.Eurobank;
using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.UseFulLinks
{
    public class UseFulLinkViewModel
    {
        public string LinkText { get; set; }
        public Boolean LinkOpenInNewTab { get; set; }
        public string LinkURL { get; set; }
        public string NodeAliasPath { get; set; }

        public static UseFulLinkViewModel GetViewModel(Link link, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            return new UseFulLinkViewModel
            {
              
                LinkText = link.LinkText,
                LinkURL = link.LinkURL,
                LinkOpenInNewTab=link.LinkOpenInNewTab
            };
        }
    }
}
