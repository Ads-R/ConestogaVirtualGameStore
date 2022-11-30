namespace ConestogaVirtualGameStore.Models
{
    public class WishListModel
    {
        public string UserId { get; set; }
        public int GameId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual GameModel Game { get; set; }
    }

}
