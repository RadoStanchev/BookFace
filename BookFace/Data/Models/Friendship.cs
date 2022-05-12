using BookFace.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Data.Models
{
    public class Friendship
    {
        [Required]
        public string FirstUserId { get; set; }

        [Required]
        public ApplicationUser FirstUser { get; set; }

        [Required]
        public string SecondUserId { get; set; }

        [Required]
        public Friend SecondUser { get; set; }

        [Required]
        public FriendshipStatus FirstUserStatus { get; set; }

        [Required]
        public FriendshipStatus SecondUserStatus { get; set; }
    }
}
