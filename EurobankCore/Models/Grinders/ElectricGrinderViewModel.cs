using CMS.DocumentEngine.Types.Eurobank;

namespace Eurobank.Models
{
    public class ElectricGrinderViewModel : ITypedProductViewModel
    {
        public int Power { get; set; }


        public static ElectricGrinderViewModel GetViewModel(ElectricGrinder grinder)
        {
            return new ElectricGrinderViewModel
            {
                Power = grinder.ElectricGrinderPower
            };
        }
    }
}