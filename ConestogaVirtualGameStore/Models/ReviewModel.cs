using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ConestogaVirtualGameStore.Models
{
    public class ReviewModel
    {
        public string UserId { get; set; }
        public int GameId { get; set; }
        [Required]
        public string ReviewText { get; set; }
        [Display(Name ="Last Modified")]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime DateTime { get; set; }
        public bool IsApproved { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual GameModel Game { get; set; }
    }
}
