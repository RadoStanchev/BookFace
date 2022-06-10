using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static BookFace.Data.DataConstants.Post;

namespace BookFace.Data.Models
{
    public class Post
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [MaxLength(ImageUrlMaxLength)]
        public string Image { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        public IEnumerable<ApplicationUser> Likes { get; set; } = new List<ApplicationUser>();
    }
}
