using BookFace.Data;
using BookFace.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Services.Friend
{
    using Friend = BookFace.Data.Models.Friend;
    public class FriendService : IFriendService
    {
        private readonly ApplicationDbContext data;
        public FriendService(ApplicationDbContext data)
        {
            this.data = data;
        }
        public string CreateFriend(string userId)
        {
            var friend = new Friend()
            {
                UserId = userId,
            };

            data.Friends.Add(friend);
            data.SaveChanges();

            return friend.UserId;
        }

        public FriendModel IndexFriend(string userId, int mutualFriendsCount)
        {
            var user = data.ApplicationUsers.FirstOrDefault(x => x.Id == userId);
            return new FriendModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImagePath = user.ProfileImagePath,
                MutualFriendsCount = mutualFriendsCount,
            };
        }

        public bool IsExistingFriend(string userId)
        {
            return data.Friends.FirstOrDefault(x => x.UserId == userId) != null;
        }
    }
}
