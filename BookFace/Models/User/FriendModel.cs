namespace BookFace.Models.User
{
    public class FriendModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfileImagePath { get; set; }

        public int MutualFriendsCount { get; set; }
    }
}