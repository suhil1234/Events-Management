using Event_management.BL;
using Event_managment.BL;
using Event_managment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Event_managment.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ParticipantController : Controller
    {
        private readonly IParticipantService _participantService;
        private readonly IEventService _eventService;

        // Constructor
        public ParticipantController(IParticipantService participantService, IEventService eventService)
        {
            _participantService = participantService;
            _eventService = eventService;
        }

        // GET: Admin/Participant/List
        public IActionResult List()
        {
            var participants = _participantService.GetAll(); // Fetch all participants
            return View(participants);
        }

        // GET: Admin/Participant/Edit/{id}
        public IActionResult Edit(int? id)
        {
            TbParticipant participant = new TbParticipant();
            var events = _eventService.GetAll(); // Get all events for the view

            if (!events.Any())
            {
                ModelState.AddModelError(string.Empty, "No Event available. Please add an Event before editing Participants.");
                return View(participant); // Return the view with an error message
            }
            ViewBag.Events = events;

            // If an ID is provided, retrieve the corresponding participant
            if (id != null)
            {
                participant = _participantService.GetByID(id.Value);
            }

            return View(participant); // Return the view for editing
        }

        // POST: Admin/Participant/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(TbParticipant participant, int[] eventIds)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Events = _eventService.GetAll(); // Repopulate events in case of validation errors
                return View("Edit", participant); // Return view with validation errors
            }

            // Check for uniqueness of email and phone number
            if (_participantService.IsEmailUnique(participant.Email, participant.ParticipantId))
            {
                ModelState.AddModelError("Email", "An email address is already registered.");
                ViewBag.Events = _eventService.GetAll(); // Repopulate events
                return View("Edit", participant); // Return view with error message
            }
            if (_participantService.IsPhoneUnique(participant.Phone, participant.ParticipantId))
            {
                ModelState.AddModelError("Phone", "A phone number is already registered.");
                ViewBag.Events = _eventService.GetAll(); // Repopulate events
                return View("Edit", participant); // Return view with error message
            }

            _participantService.Save(participant, eventIds); // Save the participant along with associated events
            return RedirectToAction("List"); // Redirect to the list of participants
        }

        // GET: Admin/Participant/Delete/{id}
        public IActionResult Delete(int id)
        {
            _participantService.Delete(id); // Delete the participant
            return RedirectToAction("List"); // Redirect to the list
        }
    }
}