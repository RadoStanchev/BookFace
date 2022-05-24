using BookFace.Models.Home;
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
        public HomeService(IFriendshipService friendshipService, IPostService postService)
        {
            this.friendshipService = friendshipService;
            this.postService = postService;
        }

        public HomePostSuggestionModel IndexModel(string userId)
        {
            return new HomePostSuggestionModel()
            {
                Suggestions = friendshipService.Suggestions(userId, 10),
                Posts = postService.Posts(userId, 10)
            };
        }
    }
}
