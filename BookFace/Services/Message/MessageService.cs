using BookFace.Data;
using BookFace.Models.Message;
using BookFace.Services.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookFace.Services.Message
{
    using Message = BookFace.Data.Models.Message;
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext data;

        private readonly IApplicationUserService applicationUserService;

        public MessageService(ApplicationDbContext data, IApplicationUserService applicationUserService)
        {
            this.data = data;
            this.applicationUserService = applicationUserService;
        }

        public string CreateMessage(string content, string chatId, string userId)
        {
            var message = new Message
            {
                Content = content,
                ChatId = chatId,
                CreatorId = userId,
                CreatedOn = DateTime.Now,
            };

            data.Messages.Add(message);
            data.SaveChanges();

            return message.Id;
        }

        public MessageModel Message(string messageId)
        {
            var message = data.Messages.FirstOrDefault(x => x.Id == messageId);

            return new MessageModel
            {
                Content = message.Content,
                Owner = applicationUserService.Owner(message.CreatorId),
                CreatorId = message.CreatorId,
                DateDiff = DateTime.Now - message.CreatedOn,
            };
        }

        public IEnumerable<MessageModel> Messages(string chatId)
        {
            return data.Messages
                .Where(x => x.ChatId == chatId)
                .Select(x => new MessageModel
                {
                    Content = x.Content,
                    Owner = applicationUserService.Owner(x.CreatorId),
                    CreatorId = x.CreatorId,
                    DateDiff = DateTime.Now - x.CreatedOn,
                })
                .ToList();
        }
    }
}
