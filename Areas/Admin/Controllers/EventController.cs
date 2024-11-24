using Event_management.BL;
using Event_managment.BL;
using Event_managment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Event_managment.Areas.Admin.Controllers
{
    [Area("admin")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ILocationService _locationService;
        private readonly IParticipantService _participantService;

        // Constructor
        public EventController(IEventService eventService, ILocationService locationService, IParticipantService participantService)
        {
            _eventService = eventService;
            _locationService = locationService;
            _participantService = participantService;
        }

        // GET: Admin/Event/List
        public IActionResult List()
        {
            List<TbEvent> lstEvents = _eventService.GetAll(); // Fetch all events
            return View(lstEvents);
        }

        // GET: Admin/Event/Edit/{id}
        public IActionResult Edit(int? id)
        {
            TbEvent eventItem = new TbEvent();
            var locations = _locationService.GetAll(); // Populate locations for the dropdown

            // Check if there are locations
            if (!locations.Any())
            {
                ModelState.AddModelError(string.Empty, "No locations available. Please add a location before editing events.");
                return View(eventItem); // Return the view with an error message
            }

            ViewBag.LstLocations = locations; // Set locations for the dropdown

            // If an ID is provided, retrieve the corresponding event
            if (id != null)
            {
                eventItem = _eventService.GetByID(id.Value);
                if (eventItem == null)
                {
                    ModelState.AddModelError(string.Empty, "Event not found.");
                }
            }

            return View(eventItem); // Return the view for editing
        }

        // POST: Admin/Event/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(TbEvent eventItem)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LstLocations = _locationService.GetAll(); // Re-populate locations for the dropdown
                return View("Edit", eventItem); // Return view with validation errors
            }

            // Check for uniqueness of the event name and date
            if (_eventService.IsEventUnique(eventItem.EventName, eventItem.EventDate, eventItem.EventId))
            {
                ModelState.AddModelError(string.Empty, "An event with the same name and date already exists.");
                ViewBag.LstLocations = _locationService.GetAll(); // Re-populate locations
                return View("Edit", eventItem); // Return view with error message
            }

            // Save the event
            bool saveResult = _eventService.Save(eventItem);
            if (!saveResult)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the event.");
                ViewBag.LstLocations = _locationService.GetAll(); // Re-populate locations
                return View("Edit", eventItem); // Return view with error message
            }

            return RedirectToAction("List"); // Redirect to the list of events
        }

        // GET: Admin/Event/Delete/{id}
        public IActionResult Delete(int id)
        {
            _eventService.Delete(id); // Delete the event
            var participantIds = _eventService.GetParticipantsByEventId(id);
            foreach (var participantId in participantIds)
            {
                // delete relationship
                _eventService.DeleteEventParticipant(participantId, id);
                // Check if the participant is associated with other events
                if (!_participantService.IsParticipantAssociatedWithOtherEvents(participantId, id))
                {
                    // If not, delete the participant
                    _participantService.Delete(participantId);
                }
            }
            return RedirectToAction("List"); // Redirect to the list
        }
    }
}