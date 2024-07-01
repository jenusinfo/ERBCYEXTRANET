using CMS.DocumentEngine.Types.Eurobank;

namespace Eurobank.Models
{
    public class BrewerViewModel : ITypedProductViewModel
    {
        public bool IsDishwasherSafe { get; set; }


        public static BrewerViewModel GetViewModel(Brewer brewer)
        {
            return new BrewerViewModel
            {
                IsDishwasherSafe = brewer.Fields.IsDishwasherSafe
            };
        }
    }
}