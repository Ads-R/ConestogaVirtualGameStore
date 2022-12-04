using Microsoft.EntityFrameworkCore.Query;

namespace ConestogaVirtualGameStore.Models.ViewModels
{
    public class WishGameCount
    {
        public string GameTitle { get; set; }
        //public string ImageName { get; set; }
        public int Count { get; set; }
        
    }
}
