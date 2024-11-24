using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Event_managment.Models
{
    public partial class TbParticipant
    {
        public TbParticipant()
        {
            EventParticipants = new HashSet<TbEventParticipant>();
            CreatedAt = DateTime.Now; // Set default value
            UpdatedAt = DateTime.Now; // Set default value
        }

        public int ParticipantId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty; // Not nullable

        [Required(ErrorMessage = "Participant Name is required.")]
        public string ParticipantName { get; set; } = string.Empty; // Not nullable

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^05\d{8}$", ErrorMessage = "Phone number must be 10 digits and start with '05' (e.g., 05XXXXXXXX).")]
        public string Phone { get; set; } = string.Empty; // Not nullable

        public DateTime CreatedAt { get; set; } // Not nullable

        public DateTime UpdatedAt { get; set; } // Not nullable
        public bool IsDeleted { get; set; } = false; // Soft delete flag
        [Required(ErrorMessage = "Events are required.")]
        public ICollection<TbEventParticipant> EventParticipants { get; set; }
    }
}