using System;

namespace BookFace.Data.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }

        public string Text { get; set; }
        public DateTime CreatedOn { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}