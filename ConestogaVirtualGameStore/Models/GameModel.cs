using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConestogaVirtualGameStore.Models
{
    public class GameModel
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public int YearReleased { get; set; }
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        [DisplayName("Upload Game Image")]
        public IFormFile ImageFile { get; set; }

    }
}
