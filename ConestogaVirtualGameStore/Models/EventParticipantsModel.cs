using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ConestogaVirtualGameStore.Models
{
    public class EventParticipantsModel
    {
        public string UserId { get; set; }
        public int EventModelId { get; set; }
        public bool IsRegistered { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual EventModel Event { get; set; }

    }
}
