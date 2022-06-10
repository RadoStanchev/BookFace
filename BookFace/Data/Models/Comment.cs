using System;
using System.ComponentModel.DataAnnotations;
using static BookFace.Data.DataConstants.Comment;

namespace BookFace.Data.Models
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        [Required]
        public string PostId { get; set; }

        [Required]
        public Post Post { get; set; }
    }
}