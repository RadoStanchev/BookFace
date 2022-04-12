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
        public string SenderId { get; set; }

        [Required]
        public ApplicationUser Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public Friend Receiver { get; set; }

        [Required]
        public FriendshipStatus Status { get; set; }
    }
}
