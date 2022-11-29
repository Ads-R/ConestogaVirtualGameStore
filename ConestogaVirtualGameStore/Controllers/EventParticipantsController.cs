using ConestogaVirtualGameStore.Models;
using ConestogaVirtualGameStore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    public class EventParticipantsController : Controller
    {

        private readonly IEventParticipants _eventParticipantsService;
        private readonly UserManager<ApplicationUser> userManager;
        
        public EventParticipantsController(IEventParticipants eventParticipantsService, UserManager<ApplicationUser> uManager)
        {
            _eventParticipantsService = eventParticipantsService;
            userManager = uManager;
          
        }

       
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            var events = await _eventParticipantsService.GetAllEvents(user.Id);

            ViewBag.Events = events;
            return View();

        }
        public async Task<IActionResult> AllRegistrations()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            var registeredEvents = await _eventParticipantsService.GetAllRegisteredEvents(user.Id);

            ViewBag.RegisteredEvents = registeredEvents;
            return View();
        }
            [HttpPost]
        public async Task<IActionResult> Registration(int eventId)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
                _eventParticipantsService.RegisterForEvent(user.Id, eventId);
                TempData["RESuccess"] = "Event registered successfully";
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                TempData["RException"] = x.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeregisterEvent(int eventId)
        {
            try
            {
                ApplicationUser user = await userManager.GetUserAsync(User);
            _eventParticipantsService.CancelRegistration(user.Id, eventId);
                TempData["RESuccess"] = "Event deregistered successfully";
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                TempData["RException"] = x.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
