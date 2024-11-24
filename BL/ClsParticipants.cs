using Event_managment.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Event_management.BL
{
    // Interface definition for participant operations
    public interface IParticipantService
    {
        List<TbParticipant> GetAll();
        TbParticipant GetByID(int id);
        bool Save(TbParticipant participant, int[] eventIds);
        bool Delete(int id);
        bool IsEmailUnique(string email, int currentId);
        bool IsPhoneUnique(string phone, int currentId);
        bool IsParticipantAssociatedWithOtherEvents(int participantId, int currentEventId);
    }

    // The ClsParticipants class now implements IParticipantService
    public class ClsParticipants : IParticipantService
    {
        private readonly EventManagement2Context _context; // Dependency injected context

        // Constructor that accepts EventManagement2Context via Dependency Injection
        public ClsParticipants(EventManagement2Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // Ensure context is not null
        }

        // Retrieves all participants synchronously
        public List<TbParticipant> GetAll()
        {
            try
            {
                return _context.TbParticipants
                    .Where(p => !p.IsDeleted) // Only include non-deleted participants
                    .Include(p => p.EventParticipants
                        .Where(ep => !ep.IsDeleted) // Only include non-deleted EventParticipants
                    )
                    .ThenInclude(ep => ep.TbEvent)
                    .Where(e => !e.IsDeleted) // Only include non-deleted events
                    .ToList(); // Execute the query and return the list
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving participants: {ex.Message}"); // Log error
                return new List<TbParticipant>(); // Returns an empty list on failure
            }
        }

        // Fetches a specific participant by ID synchronously
        public TbParticipant GetByID(int id)
        {
            try
            {
                return _context.TbParticipants
                    .Include(p => p.EventParticipants)
                    .ThenInclude(ep => ep.TbEvent)
                    .FirstOrDefault(p => p.ParticipantId == id && !p.IsDeleted); // Fetch by ID
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving participant by ID: {ex.Message}"); // Log error
                return null; // Return null on failure
            }
        }

        // Adds or updates a participant synchronously
        public bool Save(TbParticipant participant, int[] eventIds)
        {
            if (participant == null) throw new ArgumentNullException(nameof(participant)); // Ensure participant is not null
            if (eventIds == null || eventIds.Length == 0) throw new ArgumentException("Event IDs cannot be null or empty.", nameof(eventIds)); // Check event IDs

            var relatedEvents = _context.TbEvents
                .Where(e => eventIds.Contains(e.EventId))
                .ToList(); // Get events associated with the provided IDs

            try
            {
                if (participant.ParticipantId == 0) // New participant
                {
                    participant.CreatedAt = DateTime.Now; // Set creation date
                    _context.TbParticipants.Add(participant); // Add new participant
                }
                else // Existing participant
                {
                    participant.UpdatedAt = DateTime.Now; // Set update date
                    _context.Entry(participant).State = EntityState.Modified; // Mark as modified

                    var existingRelations = _context.TbEventParticipants
                        .Where(ep => ep.ParticipantId == participant.ParticipantId)
                        .ToList(); // Fetch existing relations

                    foreach (var relation in existingRelations)
                    {
                        if (!eventIds.Contains(relation.EventId))
                        {
                            _context.TbEventParticipants.Remove(relation); // Remove obsolete relations
                        }
                    }
                }

                // Link participant to new events
                foreach (var eventItem in relatedEvents)
                {
                    if (!participant.EventParticipants.Any(ep => ep.EventId == eventItem.EventId))
                    {
                        var eventParticipant = new TbEventParticipant
                        {
                            ParticipantId = participant.ParticipantId,
                            EventId = eventItem.EventId,
                            TbEvent = eventItem,
                            TbParticipant = participant,
                        };
                        _context.TbEventParticipants.Add(eventParticipant); // Add new relation
                    }
                }

                _context.SaveChanges(); // Save changes to the database
                return true; // Indicate success
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency issues
                Console.WriteLine($"Concurrency error: {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                // Handle update issues
                Console.WriteLine($"Database update error: {ex.InnerException?.Message}");
            }
            catch (InvalidOperationException ex)
            {
                // Handle invalid operations
                Console.WriteLine($"Invalid operation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                Console.WriteLine($"Error saving participant: {ex.Message}");
            }
            return false; // Indicate failure
        }

        // Deletes a participant synchronously
        public bool Delete(int id)
        {
            try
            {
                var participant = GetByID(id); // Retrieve entry
                if (participant == null) return false; // Ensure participant exists
                participant.IsDeleted = true; // Mark as deleted
                _context.Entry(participant).State = EntityState.Modified; // Mark for update
                _context.SaveChanges(); // Save changes to the database
                return true; // Indicate success
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting participant: {ex.Message}"); // Log error
                return false; // Return false on failure
            }
        }

        // Checks if an email is unique synchronously
        public bool IsEmailUnique(string email, int currentId)
        {
            return _context.TbParticipants.Any(p => p.Email == email && p.ParticipantId != currentId); // Check for uniqueness
        }

        // Checks if a phone number is unique synchronously
        public bool IsPhoneUnique(string phone, int currentId)
        {
            return _context.TbParticipants.Any(p => p.Phone == phone && p.ParticipantId != currentId); // Check for uniqueness
        }

        // Checks if a participant is associated with other events
        public bool IsParticipantAssociatedWithOtherEvents(int participantId, int currentEventId)
        {
            return _context.TbEventParticipants
                .Any(ep => ep.ParticipantId == participantId && ep.EventId != currentEventId); // Check associations
        }
    }
}