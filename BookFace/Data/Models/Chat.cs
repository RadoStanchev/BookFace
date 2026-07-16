using BookFace.Data.Enums;
using System;
using System.Collections.Generic;

namespace BookFace.Data.Models
{
    public class Chat
    {
        public Guid Id { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public ICollection<ChatUser> Users { get; set; } = new List<ChatUser>();
    }
}