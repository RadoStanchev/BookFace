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
        public Guid Id { get; set; }

        [Required]
        public string RequesterId { get; set; }

        public ApplicationUser Requester { get; set; }

        [Required]
        public string AddresseeId { get; set; }

        public ApplicationUser Addressee { get; set; }

        [Required]
        public FriendshipStatus RequesterStatus { get; set; }

        [Required]
        public FriendshipStatus AddresseeStatus { get; set; }
    }
}
