using System;
using System.ComponentModel.DataAnnotations;

namespace BookFace.Data.Models
{
    public class ChatUser
    {
        [Required]
        public Guid ChatId { get; set; }
        
        public Chat Chat { get; set; }

        [Required]
        public string UserId { get; set; }
        
        public ApplicationUser User { get; set; }
    }
}
