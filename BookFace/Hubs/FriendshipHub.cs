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
        public async Task SendRequest (string firstId, string secondId)
        {
            await Task.Run(async () => friendshipService.Request(firstId, secondId));
        }
    }
}
