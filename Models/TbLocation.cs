using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Event_managment.Models
{
    public partial class TbLocation
    {
        public TbLocation()
        {
            TbEvents = new HashSet<TbEvent>(); // Initialize the collection
        }

        public int LocationId { get; set; }

        [Required(ErrorMessage = "Location Name is required.")]
        public string LocationName { get; set; } // Not nullable

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } // Not nullable

        [Required(ErrorMessage = "Capacity is required.")]
        public int Capacity { get; set; } // Not nullable

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; } // Not nullable

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; } // Not nullable

        [Required(ErrorMessage = "Zip Code is required.")]
        public string ZipCode { get; set; } // Not nullable
        public bool IsDeleted { get; set; } = false; // Soft delete flag
        public ICollection<TbEvent> TbEvents { get; set; } // Navigation property
    }
}