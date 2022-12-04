using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ConestogaVirtualGameStore.Models.ViewModels
{
    public class Sales
    {
        public string Month { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "Total Sales")]
        public double TotalSales { get; set; }

        public int GrandTotalQuantities { get; set; }
        public double GrandTotalSales { get; set; }
    }
}
