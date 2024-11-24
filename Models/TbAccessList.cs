using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Event_managment.Models
{
    public partial class TbAccessList
    {
        [ValidateNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "IP Address is required.")]
        [RegularExpression(@"^(\d{1,3}\.){3}\d{1,3}$", ErrorMessage = "Invalid IP Address format.")]
        public string IpAddress { get; set; } // Not nullable
        public string? Reason { get; set; } // Nullable

        [Required(ErrorMessage = "Type is required.")]
        [RegularExpression("^(Allow|Deny)$", ErrorMessage = "Type must be either 'Allow' or 'Deny'.")]
        public string Type { get; set; } // Changed to string
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Not nullable
        public bool IsDeleted { get; set; } = false; // Soft delete flag
    }
}