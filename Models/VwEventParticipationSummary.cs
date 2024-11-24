using System;
using System.ComponentModel.DataAnnotations;

namespace Event_managment.Models
{
    public partial class VwEventParticipationSummary
    {
        public int EventId { get; set; }
        public string EventName { get; set; } // Not nullable
        public long ParticipantCount { get; set; } // Not nullable
    }
}