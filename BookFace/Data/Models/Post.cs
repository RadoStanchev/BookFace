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
        public Guid Id { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [MaxLength(ImageUrlMaxLength)]
        public string Image { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        public IEnumerable<Like> Likes { get; set; } = new List<Like>();
    }
}
