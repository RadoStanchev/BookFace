using BookFace.Models.User;
using System.Collections.Generic;

namespace BookFace.Models.Friendship
{
    public class FriendshipQueryModel
    {
        public const int PeoplePerPage = 7;

        public IEnumerable<FriendModel> CurrentPeople { get; set; }

        public string SearchTerm { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalPeople { get; set; }
    }
}
