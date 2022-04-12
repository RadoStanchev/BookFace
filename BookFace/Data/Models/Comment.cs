using System;

namespace BookFace.Data.Models
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public string PostId { get; set; }

        public Post Post { get; set; }
    }
}