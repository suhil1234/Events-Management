using Event_managment.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Event_management.BL
{
    // Interface definition for location operations
    public interface ILocationService
    {
        List<TbLocation> GetAll();
        TbLocation GetByID(int id);
        bool Save(TbLocation location);
        bool Delete(int id);
        bool IsLocationUnique(string locationName, string address, int currentId);
        bool IsCapacityValid(int capacity, int currentId);
        List<int> GetRelatedEventsIDs(int id);
    }

    // Manages location-related data
    public class ClsLocations : ILocationService
    {
        private readonly EventManagement2Context _context; // Dependency injected context

        // Constructor accepting EventManagement2Context via Dependency Injection
        public ClsLocations(EventManagement2Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // Ensure context is not null
        }

        // Retrieves all locations
        public List<TbLocation> GetAll()
        {
            try
            {
                return _context.TbLocations.Where(l => !l.IsDeleted).ToList(); // Only return non-deleted locations
            }
            catch
            {
                return new List<TbLocation>(); // Returns an empty list on failure
            }
        }

        // Fetches a specific location by its ID
        public TbLocation GetByID(int id)
        {
            try
            {
                return _context.TbLocations.FirstOrDefault(a => a.LocationId == id && !a.IsDeleted); // Only fetch non-deleted location
            }
            catch
            {
                return new TbLocation(); // Returns a new instance on failure
            }
        }

        // Adds or updates a location
        public bool Save(TbLocation location)
        {
            try
            {
                if (location.LocationId == 0)
                {
                    _context.TbLocations.Add(location); // Add new location
                }
                else
                {
                    _context.Entry(location).State = EntityState.Modified; // Mark as modified
                }

                _context.SaveChanges(); // Save changes to the database
                return true; // Indicate success
            }
            catch
            {
                return false; // Return false on failure
            }
        }

        // Removes a location
        public bool Delete(int id)
        {
            try
            {
                var location = GetByID(id);
                if (location != null) // Ensure the location exists
                {
                    location.IsDeleted = true; // Mark as deleted
                    _context.Entry(location).State = EntityState.Modified; // Mark for update
                    _context.SaveChanges(); // Save changes
                    return true; // Indicate success
                }
                return false; // Location not found
            }
            catch
            {
                return false; // Return false on failure
            }
        }

        // Checks if a location is unique
        public bool IsLocationUnique(string locationName, string address, int currentId)
        {
            try
            {
                return _context.TbLocations.Any(l => l.LocationName == locationName && l.Address == address && l.LocationId != currentId); // Check for uniqueness
            }
            catch
            {
                return false; // Return false on failure
            }
        }

        // Validates if the capacity is appropriate based on current participants
        public bool IsCapacityValid(int capacity, int currentId)
        {
            try
            {
                if (currentId == 0 && capacity == 0)
                {
                    return false; // Invalid capacity
                }

                var location = _context.VwVenueCapacityStatuses.FirstOrDefault(l => l.LocationId == currentId);
                if (location != null)
                {
                    if (capacity < location.CurrentParticipants)
                    {
                        return false; // Capacity is less than current participants
                    }
                }
                return true; // Capacity is valid
            }
            catch
            {
                return false; // Return false on failure
            }
        }

        // Retrieves related event IDs for a specific location
        public List<int> GetRelatedEventsIDs(int id)
        {
            try
            {
                return _context.TbEvents
                                .Where(e => e.LocationId == id)
                                .Select(e => e.EventId) // Assuming EventId is the ID property
                                .ToList(); // Returns a list of related event IDs
            }
            catch
            {
                return new List<int>(); // Returns a new instance on failure
            }
        }
    }
}