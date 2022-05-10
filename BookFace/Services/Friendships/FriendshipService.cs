using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Services.Friendships
{
    public class FriendshipService : IFriendshipService
    {
        public bool Accept(string senderId, string reciverId)
        {
            throw new NotImplementedException();
        }

        public bool Block(string senderId, string reciverId)
        {
            throw new NotImplementedException();
        }

        public bool BrakeUp(string senderId, string reciverId)
        {
            throw new NotImplementedException();
        }

        public bool CanRequest(string senderId, string reciverId)
        {
            throw new NotImplementedException();
        }

        public bool CreateNewLink(string firstId, string secondId)
        {
            throw new NotImplementedException();
        }

        public bool Denied(string senderId, string reciverId)
        {
            throw new NotImplementedException();
        }

        public bool Request(string senderId, string reciverId)
        {
            throw new NotImplementedException();
        }
    }
}
