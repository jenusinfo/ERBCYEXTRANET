using System.Collections.Generic;

using Eurobank.Models;

namespace Eurobank.Widgets
{
    /// <summary>
    /// View model for Articles widget.
    /// </summary>
    public class ArticlesWidgetViewModel
    {
        /// <summary>
        /// Latest articles to display.
        /// </summary>
        public IEnumerable<ArticleViewModel> Articles { get; set; }


        /// <summary>
        /// Number of articles to show.
        /// </summary>
        public int Count { get; set; }
    }
}