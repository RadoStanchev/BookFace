using BookFace.Models.Post;
using BookFace.Models.User;
using System.Collections.Generic;

namespace BookFace.Models.Home
{
    public class HomePostSuggestionModel
    {
        public IEnumerable<HomePostModel> Posts { get; set; } = new List<HomePostModel>();

        public IEnumerable<FriendModel> Suggestions { get; set; } = new List<FriendModel>();
    }
}
