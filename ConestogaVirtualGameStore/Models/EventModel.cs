using System;
using System.ComponentModel.DataAnnotations;

namespace ConestogaVirtualGameStore.Models
{
    public class EventModel
    {
        public int EventModelId { get; set; }
        [Display(Name ="Event Name")]
        [Required]
        public string EventName { get; set; }
        [Display(Name = "Event Description")]
        public string EventDescription { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dddd, MMMM d, yyyy}")]
        [Display(Name = "Event Start Date")]
        public DateTime EventStartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dddd, MMMM d, yyyy}")]
        [Display(Name = "Event End Date")]
        public DateTime EventEndDate { get; set; }
    }
}
