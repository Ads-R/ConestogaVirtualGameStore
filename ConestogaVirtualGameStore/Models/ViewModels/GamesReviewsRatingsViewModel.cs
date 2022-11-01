using System.Collections.Generic;

namespace ConestogaVirtualGameStore.Models.ViewModels
{
    public class GamesReviewsRatingsViewModel
    {
        public GameModel Game { get; set; }
        public List<ReviewModel> Review { get; set; }
        public int ReviewCount { get; set; }
        public double GameRatingScore { get; set; }
        public int RatingCount { get; set; }
    }
}
