namespace Eurobank.Models.Home
{
    public class HomeChartViewModel
    {
        public HomeChartViewModel()
        {
        }

        public HomeChartViewModel(string category, decimal value, string color)
        {
            category = category;
            value = value;
            color = color;
        }

        public string category { get; set; }
        public decimal value { get; set; }
        public string color { get; set; }
    }
}
