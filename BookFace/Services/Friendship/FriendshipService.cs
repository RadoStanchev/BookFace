using BookFace.Data;
using BookFace.Data.Enums;
using BookFace.Models.Home.Suggestion;
using BookFace.Services.Friend;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookFace.Services.Friendship
{
    using Friendship = BookFace.Data.Models.Friendship;
    public class FriendshipService : IFriendshipService
    {
        private readonly List<FriendshipStatus> badStatuses = new List<FriendshipStatus>()
        {
            FriendshipStatus.Blocked,
            FriendshipStatus.Accepted,
        };

        private readonly ApplicationDbContext data;

        private readonly IFriendService friendService;
        public FriendshipService(ApplicationDbContext data, IFriendService friendService)
        {
            this.data = data;
            this.friendService = friendService;
        }

        public bool Accept(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.FirstUserId == firstId && x.SecondUserId == secondId);

            if (CanAcceptOrDeny(friendship, isPrepared) == false)
            {
                return false;
            }

            if (isPrepared)
            {
                friendship.SecondUserStatus = FriendshipStatus.Accepted;
            }
            else
            {
                friendship.FirstUserStatus = FriendshipStatus.Accepted;
            }

            data.SaveChanges();

            return true;
        }

        public bool AreFriends(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.FirstUserId == firstId && x.SecondUserId == secondId);

            return AreFriends(friendship);
        }

        public bool AreFriends(Friendship friendship)
        {
            return friendship == null || (friendship.FirstUserStatus == FriendshipStatus.Accepted && friendship.SecondUserStatus == FriendshipStatus.Requested) || (friendship.FirstUserStatus == FriendshipStatus.Requested && friendship.SecondUserStatus == FriendshipStatus.Accepted);
        }

        public bool Block(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.FirstUserId == firstId && x.SecondUserId == secondId);

            if (friendship == null)
            {
                friendship = CreateFriendship(firstId, secondId);
            }

            if (friendship == null)
            {
                return false;
            }

            if (isPrepared)
            {
                friendship.SecondUserStatus = FriendshipStatus.Blocked;
            }
            else
            {
                friendship.FirstUserStatus = FriendshipStatus.Blocked;
            }

            data.SaveChanges();

            return true;
        }

        public bool BrakeUp(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.FirstUserId == firstId && x.SecondUserId == secondId);

            if (friendship == null)
            {
                return false;
            }

            if (AreFriends(friendship) == false)
            {
                return false;
            }

            if (isPrepared)
            {
                friendship.SecondUserStatus = FriendshipStatus.BrokeUp;
            }
            else
            {
                friendship.FirstUserStatus = FriendshipStatus.BrokeUp;
            }

            data.SaveChanges();

            return true;
        }

        public bool CanAcceptOrDeny(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.FirstUserId == firstId && x.SecondUserId == secondId);

            return CanAcceptOrDeny(friendship, isPrepared);
        }

        public bool CanAcceptOrDeny(Friendship friendship, bool isPrepared)
        {
            return !(friendship == null || (isPrepared ? friendship.FirstUserStatus != FriendshipStatus.Requested : friendship.SecondUserStatus != FriendshipStatus.Requested));
        }

        public bool CanRequest(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.FirstUserId == firstId && x.SecondUserId == secondId);

            if (friendship == null)
            {
                friendship = CreateFriendship(firstId, secondId);
            }

            return CanRequest(friendship);
        }

        public bool CanRequest(Friendship friendship)
        {
            return !(friendship == null || badStatuses.Contains(friendship.FirstUserStatus) || badStatuses.Contains(friendship.SecondUserStatus));
        }

        public Friendship CreateFriendship(string firstId, string secondId)
        {
            if (friendService.IsExistingFriend(firstId) == false || friendService.IsExistingFriend(secondId) == false)
            {
                return null;
            }

            PrepareIds(ref firstId, ref secondId);

            var friendship = new Friendship
            {
                FirstUserId = firstId,
                SecondUserId = secondId,
                FirstUserStatus = FriendshipStatus.NoneAction,
                SecondUserStatus = FriendshipStatus.NoneAction,
            };

            data.Friendships.Add(friendship);
            data.SaveChanges();

            return friendship;
        }

        public bool Deny(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.FirstUserId == firstId && x.SecondUserId == secondId);

            if (CanAcceptOrDeny(friendship, isPrepared) == false)
            {
                return false;
            }

            if (isPrepared)
            {
                friendship.SecondUserStatus = FriendshipStatus.Denied;
            }
            else
            {
                friendship.FirstUserStatus = FriendshipStatus.Denied;
            }

            data.SaveChanges();

            return true;
        }

        public bool PrepareIds(ref string firsrId, ref string secondId)
        {
            if (firsrId.CompareTo(secondId) < 0)
            {
                var temp = firsrId;
                firsrId = secondId;
                secondId = temp;
                // Prepared
                return true;
            }

            // Not Prepared
            return false;
        }

        public bool Request(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.FirstUserId == firstId && x.SecondUserId == secondId);

            if (friendship == null)
            {
                friendship = CreateFriendship(firstId, secondId);
            }

            if (CanRequest(friendship) == false)
            {
                return false;
            }

            if (isPrepared)
            {
                friendship.SecondUserStatus = FriendshipStatus.Requested;
            }
            else
            {
                friendship.FirstUserStatus = FriendshipStatus.Requested;
            }

            data.SaveChanges();

            return true;
        }

        public IEnumerable<IndexFriendModel> Suggestions(string userId, int count)
        {
            var myFriendships = data.Friends.FirstOrDefault(x => x.UserId == userId).Friendships.ToList();
            myFriendships.AddRange(data.ApplicationUsers.FirstOrDefault(x => x.Id == userId).Friendships.ToList());

            var myFriends = myFriendships.Select(x => x.FirstUserId != userId ? x.FirstUserId : x.SecondUserId);

            var suggestionIds = data.Friendships
                .Include(x => x.FirstUser)
                .Include(x => x.SecondUser)
                .AsEnumerable()
                .Where(x => myFriendships.Contains(x) == false)
                .Where(x => myFriends.Contains(x.FirstUserId) || myFriends.Contains(x.SecondUserId))
                .Select(x => myFriends.Contains(x.FirstUserId) ? x.SecondUserId : x.FirstUserId)
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count())
                .OrderByDescending(x => x.Value)
                .ToList();

            if (suggestionIds.Count() < count)
            {
                int maxCanTake = data.ApplicationUsers.Count() -1 > count ? count - suggestionIds.Count : data.ApplicationUsers.Count();
                    suggestionIds.AddRange(
                        data.Friendships
                         .Include(x => x.FirstUser)
                         .Include(x => x.SecondUser)
                         .AsEnumerable()
                         .Where(x => myFriendships.Contains(x) == false)
                         .Where(x => myFriends.Contains(x.FirstUserId) == false && myFriends.Contains(x.SecondUserId) == false)
                         .Select(x => x.FirstUserId)
                         .Distinct()
                         .ToDictionary(x => x, x => 0)
                         .Take(maxCanTake)
                   );
            }


            return suggestionIds.Select(x => friendService.IndexFriend(x.Key, x.Value));
        }
    }
}
