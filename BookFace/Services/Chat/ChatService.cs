using BookFace.Data;
using BookFace.Data.Models;
using BookFace.Models.Chat;
using BookFace.Services.ApplicationUsers;
using BookFace.Services.Message;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BookFace.Services.Chat
{
    using Chat = BookFace.Data.Models.Chat;
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext data;
        private readonly IMessageService messageService;
        private readonly IApplicationUserService applicationUserService;

        public ChatService(ApplicationDbContext data,
            IMessageService messageService,
            IApplicationUserService applicationUserService)
        {
            this.data = data;
            this.messageService = messageService;
            this.applicationUserService = applicationUserService;
        }

        public string ChatId(string firstId, string secondId)
        {
            int expectedUsersCount = firstId == secondId ? 1 : 2;

            var chat = data.Chats
                .Include(c => c.Users)
                .FirstOrDefault(c => c.Users.Count == expectedUsersCount && 
                                     c.Users.Any(u => u.UserId == firstId) && 
                                     c.Users.Any(u => u.UserId == secondId));

            return chat?.Id.ToString();
        }

        public ChatModel ChatModel(string chatId, string friendId)
        {
            var chatGuid = Guid.Parse(chatId);
            var chat = data.Chats.FirstOrDefault(x => x.Id == chatGuid);

            return new ChatModel
            {
                Id = chat.Id.ToString(),
                Friend = applicationUserService.ChatFriend(friendId),
                Messages = messageService.Messages(chat.Id.ToString())
            };
        }

        public string CreateChat(string firstId, string secondId)
        {
            var chat = new Chat { Id = Guid.NewGuid() };
            data.Chats.Add(chat);

            data.ChatUsers.Add(new ChatUser { ChatId = chat.Id, UserId = firstId });
            if (firstId != secondId)
            {
                data.ChatUsers.Add(new ChatUser { ChatId = chat.Id, UserId = secondId });
            }

            data.SaveChanges();

            return chat.Id.ToString();
        }

        public bool IsSoloChat(string chatId)
        {
            var chatGuid = Guid.Parse(chatId);
            var chat = data.Chats.Include(x => x.Users).FirstOrDefault(x => x.Id == chatGuid);
            return IsSoloChat(chat);
        }

        public bool IsSoloChat(Chat chat)
        {
            return chat.Users.Count == 1;
        }
    }
}
