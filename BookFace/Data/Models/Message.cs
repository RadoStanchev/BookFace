using System;
using System.ComponentModel.DataAnnotations;
using static BookFace.Data.DataConstants.Message;

namespace BookFace.Data.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        [Required]
        public string CreatorId { get; set; }

        [Required]
        public ApplicationUser Creator { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required]
        public Guid ChatId { get; set; }

        [Required]
        public Chat Chat { get; set; }
    }
}