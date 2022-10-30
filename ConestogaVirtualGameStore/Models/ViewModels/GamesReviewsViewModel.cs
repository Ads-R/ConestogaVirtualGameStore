using System.Collections.Generic;

namespace ConestogaVirtualGameStore.Models.ViewModels
{
    public class GamesReviewsViewModel
    {
        public GameModel Game { get; set; }
        public List<ReviewModel> Review { get; set; }
    }
}
