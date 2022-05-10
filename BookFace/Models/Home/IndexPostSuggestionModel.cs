using BookFace.Models.Home.Post;
using BookFace.Models.Home.Suggestion;
using System.Collections.Generic;

namespace BookFace.Models.Home
{
    public class IndexPostSuggestionModel
    {
        public IEnumerable<IndexPostModel> Posts { get; set; }

        public IEnumerable<IndexFriendModel> Suggestions { get; set; }
    }
}
