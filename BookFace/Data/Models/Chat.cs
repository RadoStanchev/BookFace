using BookFace.Data.Enums;
using System;
using System.Collections.Generic;

namespace BookFace.Data.Models
{
    public class Chat
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public string FriendshipId { get; set; }

        public Friendship Friendship { get; set; }
    }
}