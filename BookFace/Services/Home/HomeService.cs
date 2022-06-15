using BookFace.Models.Home;
using BookFace.Services.Friend;
using BookFace.Services.Friendship;
using BookFace.Services.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Services.Home
{
    public class HomeService : IHomeService
    {
        private readonly IFriendshipService friendshipService;

        private readonly IPostService postService;

        private readonly IFriendService friendService;

        public HomeService(IFriendshipService friendshipService, IPostService postService, IFriendService friendService)
        {
            this.friendshipService = friendshipService;
            this.postService = postService;
            this.friendService = friendService;
        }

        public HomePostSuggestionModel IndexModel(string userId)
        {
            return new HomePostSuggestionModel()
            {
                User = friendService.ChatFriend(userId),
                Suggestions = friendshipService.Suggestions(userId, 10),
                Posts = postService.Posts(userId, 10)
            };
        }
    }
}
