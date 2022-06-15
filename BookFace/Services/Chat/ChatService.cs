using BookFace.Data;
using BookFace.Models.Chat;
using BookFace.Services.Friend;
using BookFace.Services.Message;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookFace.Services.Chat
{
    using Chat = BookFace.Data.Models.Chat;
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext data;

        private readonly IMessageService messageService;

        private readonly IFriendService friendService;

        public ChatService(ApplicationDbContext data,
            IMessageService messageService,
            IFriendService friendService)
        {
            this.data = data;
            this.messageService = messageService;
            this.friendService = friendService;
        }

        public string ChatId(string firstId, string secondId)
        {
            return data.Friendships.FirstOrDefault(x => (x.FirstUserId == firstId && x.SecondUserId == secondId) || (x.FirstUserId == secondId && x.SecondUserId == firstId)).ChatId;
        }

        public ChatModel ChatModel(string chatId, string friendId)
        {
            var chat = data.Chats.FirstOrDefault(x => x.Id == chatId);

            return new ChatModel
            {
                Id = chat.Id,
                Friend = friendService.ChatFriend(friendId),
                Messages = messageService.Messages(chat.Id)
            };
        }

        public string CreateChat()
        {
            var chat = new Chat();

            data.Chats.Add(chat);
            data.SaveChanges();

            return chat.Id;
        }

        public bool IsSoloChat(string chatId)
        {
            var chat = data.Chats.Include(x => x.Friendship).FirstOrDefault(x => x.Id == chatId);
            return IsSoloChat(chat);
        }

        public bool IsSoloChat(Chat chat)
        {
            return chat.Friendship.FirstUserId == chat.Friendship.SecondUserId;
        }
    }
}
