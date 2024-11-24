using Event_managment.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Event_management.BL
{
    // Interface definition for event operations
    public interface IEventService
    {
        List<TbEvent> GetAll();
        TbEvent GetByID(int id);
        bool Save(TbEvent eventItem);
        bool Delete(int id);
        bool DeleteEventParticipant(int participantId, int currentEventId);
        bool IsEventUnique(string eventName, DateTime eventDate, int currentEventId);
        List<int> GetParticipantsByEventId(int eventId);
    }

    // The CLsEvents class now implements IEventService
    public class CLsEvents : IEventService
    {
        private readonly EventManagement2Context _context; // Dependency injected context

        // Constructor accepting EventManagement2Context via Dependency Injection
        public CLsEvents(EventManagement2Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // Ensure context is not null
        }

        // Retrieves all events synchronously
        public List<TbEvent> GetAll()
        {
            try
            {
                return _context.TbEvents.Include(e => e.Location)
                    .Where(l => !l.IsDeleted) // Only include non-deleted events
                    .ToList(); // Execute the query and return the list
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving events: {ex.Message}"); // Log error
                return new List<TbEvent>(); // Returns an empty list on failure
            }
        }

        // Fetches a specific event by ID synchronously
        public TbEvent GetByID(int id)
        {
            try
            {
                return _context.TbEvents.FirstOrDefault(a => a.EventId == id && !a.IsDeleted); // Fetch by ID
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving event by ID: {ex.Message}"); // Log error
                return null; // Return null on failure
            }
        }

        // Adds or updates an event synchronously
        public bool Save(TbEvent eventItem)
        {
            var location = _context.TbLocations.Find(eventItem.LocationId); // Get location by ID

            try
            {
                if (eventItem.EventId == 0) // New event
                {
                    eventItem.CreatedAt = DateTime.Now; // Set creation date
                    eventItem.UpdatedAt = DateTime.Now; // Set update date
                    eventItem.Location = location; // Associate location
                    eventItem.LocationId = location.LocationId; // Set location ID
                    _context.TbEvents.Add(eventItem); // Add new event
                }
                else // Existing event
                {
                    eventItem.UpdatedAt = DateTime.Now; // Set update date
                    _context.Entry(eventItem).State = EntityState.Modified; // Mark as modified
                }

                _context.SaveChanges(); // Save changes to the database
                return true; // Indicate success
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving event: {ex.Message}"); // Log error
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}"); // Log inner exception
                }
                return false; // Indicate failure
            }
        }

        // Deletes an event synchronously
        public bool Delete(int id)
        {
            try
            {
                var Event = GetByID(id); // Retrieve event
                if (Event != null) // Ensure event exists
                {
                    Event.IsDeleted = true; // Mark as deleted
                    _context.Entry(Event).State = EntityState.Modified; // Mark for update
                    _context.SaveChanges(); // Save changes to the database
                    return true; // Indicate success
                }
                return false; // Event not found
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting event: {ex.Message}"); // Log error
                return false; // Return false on failure
            }
        }

        // Deletes event participants associated with a specific event
        public bool DeleteEventParticipant(int participantId, int currentEventId)
        {
            try
            {
                var EventParticipants = _context.TbEventParticipants
                    .Where(ep => ep.ParticipantId == participantId && ep.EventId != currentEventId)
                    .ToList(); // Fetch participants

                foreach (var EventParticipant in EventParticipants)
                {
                    EventParticipant.IsDeleted = true; // Mark as deleted
                    _context.Entry(EventParticipant).State = EntityState.Modified; // Mark for update
                }

                _context.SaveChanges(); // Save changes to the database
                return true; // Indicate success
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting event participant: {ex.Message}"); // Log error
                return false; // Return false on failure
            }
        }

        // Checks if an event is unique synchronously
        public bool IsEventUnique(string eventName, DateTime eventDate, int currentEventId)
        {
            try
            {
                return _context.TbEvents
                    .Any(e => e.EventName == eventName && e.EventDate == eventDate && e.EventId != currentEventId); // Check for uniqueness
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking event uniqueness: {ex.Message}"); // Log error
                return false; // Indicate failure
            }
        }

        // Retrieves participant IDs by event ID
        public List<int> GetParticipantsByEventId(int eventId)
        {
            try
            {
                return _context.TbEventParticipants
                    .Where(ep => ep.EventId == eventId)
                    .Select(ep => ep.ParticipantId)
                    .ToList(); // Get participant IDs
            }
            catch
            {
                return new List<int>(); // Returns a new instance on failure
            }
        }
    }
}