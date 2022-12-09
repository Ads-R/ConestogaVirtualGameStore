using System;
using System.Collections.Generic;

namespace ConestogaVirtualGameStore.Models.ViewModels
{
    public class OrderViewModel
    {
        public string UserName { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public List<CartItem> Items { get; set; }
        public double Total { get; set; }
    }
}
