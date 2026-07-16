using System;
using System.ComponentModel.DataAnnotations;
using static BookFace.Data.DataConstants.Comment;

namespace BookFace.Data.Models
{
    public class Comment
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        [Required]
        public Guid PostId { get; set; }

        [Required]
        public Post Post { get; set; }
    }
}