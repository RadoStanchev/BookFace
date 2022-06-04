using BookFace.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Services.Friend
{
    public interface IFriendService
    {
        string CreateFriend(string userId);

        bool IsExistingFriend(string userId);

        FriendModel IndexFriend(string userId, int mutualFriendsCount);

        ChatFriendModel ChatFriend(string userId);

    }
}
