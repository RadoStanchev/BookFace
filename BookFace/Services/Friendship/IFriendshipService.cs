using BookFace.Models.Home.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Services.Friendship
{
    using Friendship = BookFace.Data.Models.Friendship;

    public interface IFriendshipService
    {
        bool Request(string firstId, string secondId);

        bool Block(string firstId, string secondId);

        bool Accept(string firstId, string secondId);

        bool Deny(string firstId, string secondId);

        bool BrakeUp(string firstId, string secondId);

        Friendship CreateFriendship(string firstId, string secondId);

        bool AreFriends(string firstId, string secondId);

        bool AreFriends(Friendship friendship);

        bool CanRequest(string firstId, string secondId);

        bool CanAcceptOrDeny(string firstId, string secondId);

        bool CanAcceptOrDeny(Friendship friendship, bool isPrepared);

        bool CanRequest(Friendship friendship);

        bool PrepareIds(ref string firsrId, ref string secondId);

        IEnumerable<IndexFriendModel> Suggestions(string userId, int count);

    }
}
