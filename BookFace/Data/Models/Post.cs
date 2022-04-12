using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Data.Models
{
    public class Post
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Content { get; set; }

        public string Image { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public IEnumerable<ApplicationUser> Likes { get; set; }
    }
}
