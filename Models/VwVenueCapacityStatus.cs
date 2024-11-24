using System;
using System.ComponentModel.DataAnnotations;

namespace Event_managment.Models
{
    public partial class VwVenueCapacityStatus
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; } // Not nullable
        public int Capacity { get; set; } // Not nullable

        public long CurrentParticipants { get; set; } // Not nullable
        public string CapacityStatus { get; set; } // Not nullable
    }
}