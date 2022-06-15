using BookFace.Infrastructure.Extensions;
using BookFace.Models.User;
using BookFace.Services.ApplicationUsers;
using BookFace.Services.Chat;
using BookFace.Services.Friendship;
using BookFace.Services.Message;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Hubs
{
    public class ChatHub : Hub
    {
        private readonly static Dictionary<string, UserServiceModel> ConnectionsMap = new Dictionary<string, UserServiceModel>();

        private readonly IChatService chatService;
        private readonly IMessageService messageService;
        private readonly IApplicationUserService applicationUserService;
        private readonly IFriendshipService friendshipService;

        public ChatHub(IMessageService messageService,
            IChatService chatService,
            IApplicationUserService applicationUserService,
            IFriendshipService friendshipService)
        {
            this.messageService = messageService;
            this.chatService = chatService;
            this.friendshipService = friendshipService;
            this.applicationUserService = applicationUserService;
        }

        public override Task OnConnectedAsync()
        {
            var user = applicationUserService.User(Context.User.Id());

            if (!ConnectionsMap.Any(kvp => kvp.Key == Context.User.Id()))
            {
                ConnectionsMap.Add(Context.User.Id(), user);
            }

            var myFriends = friendshipService.MyFriendsId(Context.User.Id());
            var onlineFriends = myFriends.Intersect(ConnectionsMap.Keys);

            Clients.Users(onlineFriends).SendAsync("GetNewOnline", user);

            var onlineFriendsModels = ConnectionsMap.Where(x => onlineFriends.Contains(x.Key)).Select(x => x.Value);

            Clients.Caller.SendAsync("GetAllOnlineFriends", onlineFriendsModels);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectionsMap.Remove(Context.User.Id());

            var myFriends = friendshipService.MyFriendsId(Context.User.Id());
            var onlineFriends = myFriends.Intersect(ConnectionsMap.Keys);

            Clients.Users(onlineFriends).SendAsync("RemoveFriend", Context.User.Id());

            return base.OnDisconnectedAsync(exception);
        }

        public Task SendMessage(string content, string chatId)
        {
            var messageId = messageService.CreateMessage(content, chatId, Context.User.Id());

            var message = messageService.Message(messageId);

            var isSolo = message.CreatorId == Context.User.Id();

            return Clients.Group(chatId).SendAsync("RecieveMessage", message, isSolo);
        }

        public Task JoinGroup(string friendId)
        {
            var chatId = chatService.ChatId(Context.User.Id(), friendId);

            Groups.AddToGroupAsync(Context.ConnectionId, chatId);

            var chatModel = chatService.ChatModel(chatId, friendId);

            var isSolo = friendId == Context.User.Id();

            return Clients.Caller.SendAsync("ShowChat", chatModel, isSolo);
        }
    }
}