using CMS.DocumentEngine.Types.Eurobank;
using Eurobank.Models.FAQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Widgets
{
    public class FAQWidgetViewModel
    {
        public IEnumerable<FaqViewModel> FAQList { get; set; }
        //public IEnumerable<FaqViewModel> FAQPageItem { get; set; }
        //public string Name { get; set; }
        //public string Email { get; set; }
        //public string ContactUs { get; set; }
        //public string VisitUs { get; set; }
        public int Count { get; set; }

        public string NodeAliasPath { get; set; }
    }
}
