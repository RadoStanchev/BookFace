using BookFace.Models.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Services.Chat
{
    using Chat = BookFace.Data.Models.Chat;
    public interface IChatService
    {
        string CreateChat();

        string ChatId(string firstId, string secondId);

        ChatModel ChatModel(string chatId, string friendId);

        bool IsSoloChat(string chatId);

        bool IsSoloChat(Chat chat);
    }
}
