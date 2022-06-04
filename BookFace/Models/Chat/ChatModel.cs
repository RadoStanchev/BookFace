using BookFace.Models.Message;
using BookFace.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Models.Chat
{
    public class ChatModel
    {
        public string Id { get; set; }

        public ChatFriendModel Friend { get; set; }

        public IEnumerable<MessageModel> Messages { get; set; }
    }
}
