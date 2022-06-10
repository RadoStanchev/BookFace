using System;
using System.ComponentModel.DataAnnotations;
using static BookFace.Data.DataConstants.Message;

namespace BookFace.Data.Models
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string CreatorId { get; set; }

        [Required]
        public ApplicationUser Creator { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string ChatId { get; set; }

        [Required]
        public Chat Chat { get; set; }
    }
}