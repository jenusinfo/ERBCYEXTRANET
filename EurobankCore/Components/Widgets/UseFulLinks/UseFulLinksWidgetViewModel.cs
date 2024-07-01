using Eurobank.Models.UseFulLinks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Widgets
{
    public class UseFulLinksWidgetViewModel
    {
        public IEnumerable<UseFulLinkViewModel> UseFulLink { get; set; }


        /// <summary>
        /// Number of articles to show.
        /// </summary>
        public int Count { get; set; }

        public string NodeAliasPath { get; set; }
    }
}
