using BookFace.Models.Home.Suggestion;
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

        IndexFriendModel IndexFriend(string userId, int mutualFriendsCount);

    }
}
