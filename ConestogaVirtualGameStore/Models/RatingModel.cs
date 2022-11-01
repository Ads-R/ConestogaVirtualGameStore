namespace ConestogaVirtualGameStore.Models
{
    public class RatingModel
    {
        public string UserId { get; set; }
        public int GameId { get; set; }
        public int RatingScore { get; set; }


        public virtual ApplicationUser User { get; set; }
        public virtual GameModel Game { get; set; }
    }
}
