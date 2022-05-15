namespace BookFace.Models.Home.User
{
    public class IndexFriendModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfileImagePath { get; set; }

        public int MutualFriendsCount { get; set; }
    }
}