using Eurobank.Models;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eurobank.Widgets
{
    public class NewsPathSelectorModel
    {
        [Required(ErrorMessage = "Please enter NodeAliasPath")]
      
        [DisplayName("NodeAliasPath")]
      
        public string NodeAliasPath { get; set; }

       
        //[EditingComponent(PathSelector.IDENTIFIER)]
        // Limits the selection of pages to a subtree rooted at the 'Products' page
       // [EditingComponentProperty(nameof(PathSelectorProperties.RootPath), "/")]
        // Returns a list of path selector items (page paths)
       // public IEnumerable<PathSelectorItem> ViewAllPagelink { get; set; } = Enumerable.Empty<PathSelectorItem>();
        public string ViewAllPagelink { get; set; }

        public int count { get; set; }

        public IEnumerable<NewsViewModel> NewsList { get; set; }
    }
}
