using BookFace.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Services.Message
{
    public interface IMessageService
    {
        string CreateMessage(string content, string chatId, string userId);

        IEnumerable<MessageModel> Messages(string chatId);

        MessageModel Message(string messageId);
    }
}
