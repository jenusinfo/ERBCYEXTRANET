using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Models.WhatsNew
{
    public class WhatsNewViewModel
    {
        public string Teaser { get; set; }

        public string Title { get; set; }
        public string NodeAlias { get; set; }

        public static WhatsNewViewModel GetViewModel(CMS.DocumentEngine.Types.Eurobank.WhatsNew whatsNew, IPageUrlRetriever pageUrlRetriever, IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            return new WhatsNewViewModel
            {

                Teaser = whatsNew.Teaser.ToString(),
                Title = whatsNew.Name.ToString(),
                NodeAlias = whatsNew.NodeAlias.ToString()

            };
        }
    }
}
