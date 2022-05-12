using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using static BookFace.Data.DataConstants.ApplicationUser;


namespace BookFace.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(ProfileImagePathMaxLength)]
        public string ProfileImagePath { get; set; }

        public string FriendId { get; set; }

        public Friend Friend { get; set; }

        public IEnumerable<Friendship> Friendships { get; set; } = new List<Friendship>();

        public IEnumerable<Post> Posts { get; set; }

        public IEnumerable<Post> Likes { get; set; }
    }
}
