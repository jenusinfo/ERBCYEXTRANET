using CMS.DocumentEngine.Types.Eurobank;

namespace Eurobank.Models
{
    public class FilterPackViewModel : ITypedProductViewModel
    {
        public int Quantity { get; set; }

        public static FilterPackViewModel GetViewModel(FilterPack filterPack)
        {
            return new FilterPackViewModel
            {
                Quantity = filterPack.FilterPackQuantity
            };
        }
    }
}