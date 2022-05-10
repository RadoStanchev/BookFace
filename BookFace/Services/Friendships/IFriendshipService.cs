using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Services.Friendships
{
    public interface IFriendshipService
    {
        bool CanRequest(string senderId, string reciverId);

        bool Request(string senderId, string reciverId);

        bool Block(string senderId, string reciverId);

        bool Accept(string senderId, string reciverId);

        bool Denied(string senderId, string reciverId);

        bool BrakeUp(string senderId, string reciverId);

        bool CreateNewLink(string firstId, string secondId);

    }
}
