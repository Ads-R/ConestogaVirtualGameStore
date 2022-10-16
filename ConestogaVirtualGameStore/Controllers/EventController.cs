using ConestogaVirtualGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ConestogaVirtualGameStore.Controllers
{
    [Authorize(Roles = "admin")]
    public class EventController : Controller
    {
        private readonly GameStoreContext _context;
        public EventController(GameStoreContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["NotFound"] = "Cannot find event";
                    return RedirectToAction(nameof(Index));
                }
                var eventModel = await _context.Events.FirstOrDefaultAsync(a => a.EventModelId == id);
                if (eventModel == null)
                {
                    TempData["NotFound"] = "Cannot find event";
                    return RedirectToAction(nameof(Index));
                }
                return View(eventModel);
            }
            catch (Exception x)
            {
                TempData["Exception"] = "An error has occurred. Error message: " + x.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult AddEvent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent([Bind("EventName, EventStartDate, EventEndDate, EventDescription")] EventModel eventModel)
        {
            try
            {
                if (eventModel.EventStartDate <= DateTime.Now)
                {
                    ModelState.AddModelError(nameof(eventModel.EventStartDate), "Start Date must not be in the past or present");
                }
                if (eventModel.EventEndDate <= eventModel.EventStartDate)
                {
                    ModelState.AddModelError(nameof(eventModel.EventEndDate), "End Date must not be earlier than the Start Date");
                }
                if (ModelState.IsValid)
                {
                    _context.Add(eventModel);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Event added successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception x)
            {
                TempData["Exception"] = "An error has occurred. Error message: " + x.Message;
                return RedirectToAction(nameof(Index));
            }
            return View(eventModel);
        }

        public async Task<IActionResult> EditEvent(int? id)
        {
            if (id == null)
            {
                TempData["NotFound"] = "Cannot find event";
                return RedirectToAction(nameof(Index));
            }
            var eventModel = await _context.Events.FindAsync(id);
            if (eventModel == null)
            {
                TempData["NotFound"] = "Cannot find event";
                return RedirectToAction(nameof(Index));
            }
            return View(eventModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent([Bind("EventModelId, EventName, EventStartDate, EventEndDate, EventDescription")] EventModel eventModel)
        {
            try
            {
                EventModel eventData = await _context.Events.Where(a => a.EventModelId == eventModel.EventModelId).FirstOrDefaultAsync();
                if (eventData == null)
                {
                    TempData["NotFound"] = "Cannot find event";
                    return RedirectToAction(nameof(Index));
                }
                if (eventModel.EventStartDate <= DateTime.Now && eventData.EventStartDate > DateTime.Now)
                {
                    ModelState.AddModelError(nameof(eventModel.EventStartDate), "Start Date must not be in the past or present");
                }

                if (eventData.EventStartDate <= DateTime.Now && eventData.EventStartDate != eventModel.EventStartDate)
                {
                    ModelState.AddModelError(nameof(eventModel.EventStartDate), "Cannot change the Start Date once the event has begun");
                }
                if (eventModel.EventEndDate <= eventModel.EventStartDate)
                {
                    ModelState.AddModelError(nameof(eventModel.EventEndDate), "End Date must not be earlier than the Start Date");
                }
                if (eventModel.EventEndDate <= DateTime.Now)
                {
                    ModelState.AddModelError(nameof(eventModel.EventEndDate), "End Date must not be earlier than the Date Now");
                }

                if (ModelState.IsValid)
                {
                    eventData.EventName = eventModel.EventName;
                    eventData.EventStartDate = eventModel.EventStartDate;
                    eventData.EventEndDate = eventModel.EventEndDate;
                    eventData.EventDescription = eventModel.EventDescription;
                    _context.Update(eventData);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Event edited successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception x)
            {
                TempData["Exception"] = "An error has occurred. Error message: " + x.Message;
                return RedirectToAction(nameof(Index));
            }
            return View(eventModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["NotFound"] = "Cannot find event";
                return RedirectToAction(nameof(Index));
            }
            var eventModel = await _context.Events.FirstOrDefaultAsync(a => a.EventModelId == id);
            if (eventModel == null)
            {
                TempData["NotFound"] = "Cannot find event";
                return RedirectToAction(nameof(Index));
            }
            return View(eventModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var eventModel = await _context.Events.FindAsync(id);
                if (eventModel != null)
                {
                    if (eventModel.EventStartDate <= DateTime.Now && eventModel.EventEndDate >= DateTime.Now)
                    {
                        TempData["Failed"] = "Cannot delete ongoing event";
                        return RedirectToAction(nameof(Index));
                    }
                    _context.Events.Remove(eventModel);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Event Deletion Successfull";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception x)
            {
                TempData["Exception"] = "An error has occurred. Error message: " + x.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
