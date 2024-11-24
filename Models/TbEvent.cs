using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Event_managment.Models
{
    public partial class TbEvent
    {
        public TbEvent()
        {
            Location = new TbLocation();
            EventParticipants = new HashSet<TbEventParticipant>();
            CreatedAt = DateTime.Now; // Set default value
            UpdatedAt = DateTime.Now; // Set default value
        }

        public int EventId { get; set; }

        [Required(ErrorMessage = "Event Name is required.")]
        public string EventName { get; set; } = string.Empty; // Not nullable

        [Required(ErrorMessage = "Event Date is required.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; } = DateTime.Now; // Not nullable

        [Required(ErrorMessage = "Location ID is required.")]
        public int LocationId { get; set; } // Not nullable

        public string Description { get; set; } = string.Empty; // Not nullable

        public DateTime CreatedAt { get; set; } // Not nullable

        public DateTime UpdatedAt { get; set; } // Not nullable

        [ValidateNever]
        public TbLocation Location { get; set; }
        public bool IsDeleted { get; set; } = false; // Soft delete flag

        [ValidateNever]
        public ICollection<TbEventParticipant> EventParticipants { get; set; }

    }
}