using ConestogaVirtualGameStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Services
{
    public interface IEventParticipants
    {
        Task<IEnumerable<EventModel>> GetAllRegisteredEvents(string userId);
        Task<IEnumerable<EventModel>> GetAllEvents(string userId);
        void RegisterForEvent(string userId,int eventId);
        void CancelRegistration(string userId, int eventId);
        
    }
}
