using Event_managment.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Event_management.BL
{
    // Interface definition for summary operations
    public interface ISummaryService
    {
        List<VwEventParticipationSummary> GetEventParticipationSummary();
        VwVenueCapacityStatus GetVenueCapacityStatus(int id);
        string CheckVenueCapacityWarnings();
    }

    // Provides summary information regarding events and venue capacities
    public class ClsSummary : ISummaryService
    {
        private readonly EventManagement2Context _context; // Dependency injected context

        // Constructor accepting EventManagement2Context via Dependency Injection
        public ClsSummary(EventManagement2Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // Ensure context is not null
        }

        // Retrieves summaries of event participation
        public List<VwEventParticipationSummary> GetEventParticipationSummary()
        {
            try
            {
                // Filter events that are not deleted
                return _context.TbEvents
                    .Where(e => !e.IsDeleted) // Ensure the event is not marked as deleted
                    .Select(e => new VwEventParticipationSummary
                    {
                        EventId = e.EventId,
                        EventName = e.EventName,
                        ParticipantCount = e.EventParticipants.Count() // Count of participants for each event
                    })
                    .ToList(); // Returns a list of event participation summaries
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine($"An error occurred while retrieving participation summaries: {ex.Message}");
                return new List<VwEventParticipationSummary>(); // Return an empty list on failure
            }
        }

        // Gets the capacity status for a specific venue
        public VwVenueCapacityStatus GetVenueCapacityStatus(int id)
        {
            try
            {
                return _context.VwVenueCapacityStatuses.FirstOrDefault(v => v.LocationId == id); // Returns the capacity status for the specified venue
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while retrieving venue capacity status: {ex.Message}");
                return new VwVenueCapacityStatus(); // Return a new instance on failure
            }
        }

        // Checks all venues for capacity warnings
        public string CheckVenueCapacityWarnings()
        {
            List<string> warnings = new List<string>();

            try
            {
                var locations = _context.VwVenueCapacityStatuses.ToList();

                foreach (var location in locations)
                {
                    if (location == null)
                    {
                        warnings.Add("Warning: A location record is null and cannot be processed.");
                        continue; // Skip to the next iteration
                    }

                    // Check capacity status
                    if (location.CapacityStatus == "Full")
                    {
                        warnings.Add($"Warning: {location.LocationName} is at full capacity!");
                    }
                    else if (location.CapacityStatus == "Warning")
                    {
                        warnings.Add($"Warning: {location.LocationName} is nearing full capacity.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while checking venue capacities: {ex.Message}");
                warnings.Add("Warning: An error occurred while checking venue capacities."); // Add a general warning
            }

            return string.Join(" ", warnings); // Returns a concatenated string of warnings
        }
    }
}