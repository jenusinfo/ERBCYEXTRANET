using Eurobank.Models.WhatsNew;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Widgets
{
    public class WhatsNewWidgetViewModel
    {
        public IEnumerable<WhatsNewViewModel> WhatsNewList { get; set; }


        /// <summary>
        /// Number of articles to show.
        /// </summary>
        public int Count { get; set; }

        public string NodeAliasPath { get; set; }
    }
}
