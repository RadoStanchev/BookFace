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
            var onlineFriends = myFriends.Intersect(ConnectionsMap.Keys)
                .Where(x => x != Context.User.Id())
                .ToList();

            Clients.Users(onlineFriends).SendAsync("GetNewOnline", user);

            onlineFriends.Add(Context.User.Id());
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

            var isSolo = chatService.IsSoloChat(chatId);

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

        public Task LeaveGroup(string chatId)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);

            return Clients.Caller.SendAsync("CloseChat");
        }

        public Task Search(string searchTerm)
        {
            var filterFriendsModels = ConnectionsMap.ToList();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTerms = searchTerm.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower());
                Predicate<UserServiceModel> predicate = x =>
                {
                    var names = (x.FirstName + x.LastName).ToLower();
                    return searchTerms.Any(st => names.Contains(st));
                };

                filterFriendsModels = filterFriendsModels.Where(x => predicate(x.Value)).ToList();
            }

            var myFriends = friendshipService.MyFriendsId(Context.User.Id());
            var onlineFriendsModels = filterFriendsModels
                .Where(x => myFriends.Contains(x.Key))
                .Select(x => x.Value)
                .ToList();

            return Clients.Caller.SendAsync("GetAllOnlineFriends", onlineFriendsModels);
        }
    }
}