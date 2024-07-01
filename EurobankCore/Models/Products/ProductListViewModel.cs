using System.Collections.Generic;

namespace Eurobank.Models
{
    public class ProductListViewModel
    {
        public IRepositoryFilter Filter { get; set; }

        public IEnumerable<ProductListItemViewModel> Items { get; set; }
    }
}