using Event_management.BL;
using Event_managment.BL;
using Event_managment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Event_managment.Areas.Admin.Controllers
{
    [Area("admin")]
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly IEventService _eventService;

        // Constructor
        public LocationController(ILocationService locationService, IEventService eventService)
        {
            _locationService = locationService;
            _eventService = eventService;
        }

        // GET: Admin/Location/List
        public IActionResult List()
        {
            List<TbLocation> lstLocations = _locationService.GetAll(); // Fetch all locations
            return View(lstLocations);
        }

        // GET: Admin/Location/Edit/{id}
        public IActionResult Edit(int? id)
        {
            TbLocation location = id != null ? _locationService.GetByID(id.Value) : new TbLocation();
            return View(location); // Return the view for editing
        }

        // POST: Admin/Location/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(TbLocation location)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", location); // Return view with validation errors
            }

            // Check for uniqueness of the location name and address
            if (_locationService.IsLocationUnique(location.LocationName, location.Address, location.LocationId))
            {
                ModelState.AddModelError(string.Empty, "A location with the same name and address already exists.");
                return View("Edit", location); // Return view with error message
            }

            // Validate capacity
            if (!_locationService.IsCapacityValid(location.Capacity, location.LocationId))
            {
                ModelState.AddModelError(string.Empty, "The capacity can't be 0 or less than the current Participants.");
                return View("Edit", location); // Return view with error message
            }

            _locationService.Save(location); // Save the location
            return RedirectToAction("List"); // Redirect to the list of locations
        }

        // GET: Admin/Location/Delete/{id}
        public IActionResult Delete(int id)
        {
            _locationService.Delete(id); // Delete the location
            var eventIds = _locationService.GetRelatedEventsIDs(id);
            foreach (var eventId in eventIds)
            {
                _eventService.Delete(eventId);
            }
            return RedirectToAction("List"); // Redirect to the list
        }
    }
}