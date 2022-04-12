using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Data.Models
{
    public class Friend
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string UserId { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        public IEnumerable<Friendship> Friendships { get; set; }
    }
}
