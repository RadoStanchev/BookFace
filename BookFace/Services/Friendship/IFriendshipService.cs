using BookFace.Data.Enums;
using BookFace.Models.User;
using System.Collections.Generic;

namespace BookFace.Services.Friendship
{
    using Friendship = BookFace.Data.Models.Friendship;

    public interface IFriendshipService
    {
        bool Request(string firstId, string secondId);

        bool Block(string firstId, string secondId);

        bool UnBlock(string firstId, string secondId);

        bool Accept(string firstId, string secondId);

        bool Deny(string firstId, string secondId);

        bool BrakeUp(string firstId, string secondId);

        Friendship CreateFriendship(string firstId, string secondId, FriendshipStatus firstStatus = FriendshipStatus.NoneAction, FriendshipStatus secondStatus = FriendshipStatus.NoneAction);

        bool AreFriends(string firstId, string secondId);

        bool AreFriends(Friendship friendship);

        bool CanRequest(string firstId, string secondId);

        bool CanAcceptOrDeny(string firstId, string secondId);

        bool CanAcceptOrDeny(Friendship friendship, bool isPrepared);

        bool CanBlock(string firstId, string secondId);

        bool CanBlock(Friendship friendship, bool isPrepared);

        bool CanRequest(Friendship friendship);

        bool PrepareIds(ref string firsrId, ref string secondId);

        IEnumerable<FriendModel> Suggestions(string userId, int count);

        IEnumerable<string> MyFriendsId(string userId);

        IEnumerable<string> MyFriendsId(string userId, IEnumerable<Friendship> myFriendships);

        IEnumerable<Friendship> MyFriendships(string userId);

        IEnumerable<FriendModel> People(string userId, string searchTerm, int currentPage, int poeplePerPage);

        int TotalPeople();

        Friendship Friendship(string firstId, string secondId);

    }
}
