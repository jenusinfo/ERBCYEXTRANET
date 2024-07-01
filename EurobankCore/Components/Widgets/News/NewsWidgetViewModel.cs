using Eurobank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Widgets
{
    public class NewsWidgetViewModel
    {
        public IEnumerable<NewsViewModel> NewsList { get; set; }


        /// <summary>
        /// Number of articles to show.
        /// </summary>
        public int Count { get; set; }

        public string ViewAllLink { get; set; }
    }
}
