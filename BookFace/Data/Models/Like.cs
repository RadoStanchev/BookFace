using System;
using System.ComponentModel.DataAnnotations;

namespace BookFace.Data.Models
{
    public class Like
    {
        public Guid Id { get; set; }

        [Required]
        public Guid PostId { get; set; }

        public Post Post { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
