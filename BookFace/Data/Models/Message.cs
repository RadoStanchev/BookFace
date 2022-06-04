using System;

namespace BookFace.Data.Models
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }

        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }

        public string ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}