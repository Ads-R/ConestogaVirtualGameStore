using ConestogaVirtualGameStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Services
{
    public class EventParticipantsService : IEventParticipants
    {
        private readonly GameStoreContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        public EventParticipantsService(GameStoreContext context, UserManager<ApplicationUser> uManger)
        {
            _context = context;
            userManager = uManger;

        }
        public void CancelRegistration(string userId, int eventId)
        {
            try
            {
                EventParticipantsModel eventParticipants = _context.EventParticipants.Where(a => a.UserId == userId && a.EventModelId == eventId).FirstOrDefault();
                _context.EventParticipants.Remove(eventParticipants);
                _context.SaveChanges();
            }
            catch (Exception x)
            {
                throw;
            }
        }

        public async Task<IEnumerable<EventModel>> GetAllRegisteredEvents(string userId)
        {
            var eventsJoined = await _context.EventParticipants.Where(a => a.UserId == userId).ToListAsync();
            var allEvents = await _context.Events.Where(a => a.EventEndDate >= DateTime.Now).ToListAsync();
            var allRegisteredEvents = allEvents.Where(d => eventsJoined.Any(e => e.EventModelId == d.EventModelId));
            return allRegisteredEvents;
        }

        public async Task<IEnumerable<EventModel>> GetAllEvents(string userId)
        {
            var eventsJoined = await _context.EventParticipants.Where(a => a.UserId == userId).ToListAsync();
            var allEvents = await _context.Events.Where(a => a.EventEndDate >= DateTime.Now).ToListAsync();
            var allEventsFiltered = allEvents.Where(d => !eventsJoined.Any(e => e.EventModelId == d.EventModelId));
            return allEventsFiltered;
            
        }

        public void RegisterForEvent(string userId, int eventId)
        {

            EventParticipantsModel entry = new EventParticipantsModel()
            {
                UserId = userId,
                EventModelId = eventId
            };
            _context.EventParticipants.Add(entry);
            _context.SaveChanges();

        }
    }
            


}
