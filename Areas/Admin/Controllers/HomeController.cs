using Event_management.BL;
using Event_managment.BL;
using Event_managment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Event_managment.Areas.Admin.Controllers
{
    [Area("admin")]
    public class HomeController : Controller
    {
        private readonly ISummaryService _summaryService;
        private readonly ILocationService _locationService;

        // Constructor
        public HomeController(ISummaryService summaryService, ILocationService locationService)
        {
            _summaryService = summaryService;
            _locationService = locationService;
        }

        // GET: Admin/Home/Index
        public IActionResult Index()
        {
            ViewBag.LstLocations = _locationService.GetAll(); // Fetch all locations for the view
            ViewBag.LstEventParticipantSummary = _summaryService.GetEventParticipationSummary(); // Summary of event participants

            // Check if there are any locations
            var locations = _locationService.GetAll();
            if (!locations.Any())
            {
                return View(new VwVenueCapacityStatus()); // Returning an empty model
            }

            int id = locations.First().LocationId; // Get the first location ID
            VwVenueCapacityStatus venueCapacityStatus = _summaryService.GetVenueCapacityStatus(id); // Get venue capacity status

            return View(venueCapacityStatus); // Return the view with the venue capacity status
        }

        // GET: Admin/Home/Search/{id}
        public IActionResult Search(int id)
        {
            ViewBag.LstLocations = _locationService.GetAll(); // Repopulate locations
            ViewBag.LstEventParticipantSummary = _summaryService.GetEventParticipationSummary(); // Summary of event participants
            VwVenueCapacityStatus venueCapacityStatus = _summaryService.GetVenueCapacityStatus(id); // Get venue capacity status for specific location
            return View("Index", venueCapacityStatus); // Return the index view with capacity status
        }

        // GET: Admin/Home/GetWarnings
        [HttpGet]
        public JsonResult GetWarnings()
        {
            string warnings = _summaryService.CheckVenueCapacityWarnings(); // Check for capacity warnings
            return Json(new { warnings }); // Return warnings as JSON
        }
    }
}