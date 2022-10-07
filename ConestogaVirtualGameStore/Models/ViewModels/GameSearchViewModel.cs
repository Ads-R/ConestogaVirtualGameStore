using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ConestogaVirtualGameStore.Models.ViewModels
{
    public class GameSearchViewModel
    {
        public List<GameModel>? Games { get; set; }
        public SelectList? Categories { get; set; }
        public string? GameCategory { get; set; }
        public string? SearchString { get; set; }
    }
}
