using ConestogaVirtualGameStore.Classes;
using ConestogaVirtualGameStore.CustomValidation;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConestogaVirtualGameStore.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Display(Name ="Year Released")]
        [Required]
        public int YearReleased { get; set; }
        [Display(Name = "Retail Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Range(1,1000, ErrorMessage = "Please enter a realistic price. The price you are trying to enter is absurd lol.")]
        [Required]
        public double RetailPrice { get; set; }
        public string Description { get; set; }
        public Genre Category { get; set; }
        public Platforms Platform { get; set; }
        public string ImageName { get; set; }
        public string FileName { get; set; }
        [NotMapped]
        [DisplayName("Upload Game Image. Allowed file formats(jpg,jpeg,png)")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile ImageFile { get; set; }


        public ICollection<ReviewModel> Reviews { get; set; }
        public ICollection<RatingModel> Ratings { get; set; }
        public ICollection<Orders> Orders { get; set; }

        public ICollection<WishListModel> Wish { get; set; }

    }
}
