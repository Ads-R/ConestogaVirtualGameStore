using System;
using System.ComponentModel.DataAnnotations;

namespace ConestogaVirtualGameStore.Models
{
    public class EventModel
    {
        public int EventModelId { get; set; }
        [Required]
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dddd, MMMM d, yyyy}")]
        public DateTime EventStartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dddd, MMMM d, yyyy}")]
        public DateTime EventEndDate { get; set; }
    }
}
