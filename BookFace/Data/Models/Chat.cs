using BookFace.Data.Enums;
using System.Collections.Generic;

namespace BookFace.Data.Models
{
    public class Chat
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public ChatType Type { get; set; }
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}