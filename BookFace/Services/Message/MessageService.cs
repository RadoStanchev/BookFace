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
                Id = Guid.NewGuid(),
                Content = content,
                ChatId = Guid.Parse(chatId),
                CreatorId = userId,
                CreatedOn = DateTime.UtcNow,
            };

            data.Messages.Add(message);
            data.SaveChanges();

            return message.Id.ToString();
        }

        public MessageModel Message(string messageId)
        {
            var messageGuid = Guid.Parse(messageId);
            var message = data.Messages.FirstOrDefault(x => x.Id == messageGuid);

            if (message == null) return null;

            return new MessageModel
            {
                Content = message.Content,
                Owner = applicationUserService.Owner(message.CreatorId),
                CreatorId = message.CreatorId,
                DateDiff = message.CreatedOn.ToString("dddd, dd MMMM yyyy HH:mm"),
            };
        }

        public IEnumerable<MessageModel> Messages(string chatId)
        {
            var chatGuid = Guid.Parse(chatId);
            return data.Messages
                .AsQueryable()
                .Where(x => x.ChatId == chatGuid)
                .OrderBy(x => x.CreatedOn)
                .AsEnumerable()
                .Select(x => new MessageModel
                {
                    Content = x.Content,
                    Owner = applicationUserService.Owner(x.CreatorId),
                    CreatorId = x.CreatorId,
                    DateDiff = x.CreatedOn.ToString("dddd, dd MMMM yyyy HH:mm"),
                })
                .ToList();
        }
    }
}
