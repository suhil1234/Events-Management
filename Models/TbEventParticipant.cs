using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_managment.Models
{
    public partial class TbEventParticipant
    {
        public TbEventParticipant()
        {
            TbEvent = new TbEvent();
            TbParticipant = new TbParticipant();
        }
        [ValidateNever]
        public int EventParticipantId { get; set; }

        [Required(ErrorMessage = "Event ID is required.")]
        [ForeignKey("TbEvent")]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Participant ID is required.")]
        [ForeignKey("TbParticipant")]
        public int ParticipantId { get; set; }

        public bool IsDeleted { get; set; } = false; // Soft delete flag

        [ValidateNever]
        public virtual TbEvent TbEvent { get; set; }
        [ValidateNever]
        public virtual TbParticipant TbParticipant { get; set; }
    }
}