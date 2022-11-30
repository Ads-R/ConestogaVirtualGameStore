using System;

namespace ConestogaVirtualGameStore.Models
{
    public class Orders
    {
        public int OrdersId { get; set; }
        public string UserId { get; set; }
        public int GameId { get; set; }
        public double Price { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public bool IsPhysicalCopy { get; set; }
        public bool IsProcessed { get; set; }


        public virtual ApplicationUser User { get; set; }
        public virtual GameModel Game { get; set; }
    }
}
