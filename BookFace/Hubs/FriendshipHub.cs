using BookFace.Infrastructure.Extensions;
using BookFace.Services.Friendship;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Hubs
{
    public class FriendshipHub : Hub
    {
        private readonly IFriendshipService friendshipService;

        public FriendshipHub(IFriendshipService friendshipService)
        {
            this.friendshipService = friendshipService;
        }
        public async Task SendRequest (string friendId)
        {
            await Task.Run(async () => friendshipService.Request(Context.User.Id(), friendId));
        }

        public async Task SendAccept(string friendId)
        {
            await Task.Run(async () => friendshipService.Accept(Context.User.Id(), friendId));
        }

        public async Task SendBlock(string friendId)
        {
            await Task.Run(async () => friendshipService.Block(Context.User.Id(), friendId));
        }

        public async Task SendDeny(string friendId)
        {
            await Task.Run(async () => friendshipService.Deny(Context.User.Id(), friendId));
        }

        public async Task SendUnBlock(string friendId)
        {
            await Task.Run(async () => friendshipService.UnBlock(Context.User.Id(), friendId));
        }

        public async Task SendBreakUp(string friendId)
        {
            await Task.Run(async () => friendshipService.BreakUp(Context.User.Id(), friendId));
        }
    }
}
