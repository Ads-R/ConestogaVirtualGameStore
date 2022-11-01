using System.ComponentModel.DataAnnotations;

namespace ConestogaVirtualGameStore.Models.ViewModels
{
    public class RatingViewModel
    {
        public string UserId { get; set; }
        public int GameId { get; set; }
        [Required]
        public string RatingScore { get; set; }
    }
}
