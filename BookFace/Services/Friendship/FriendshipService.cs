using BookFace.Data;
using BookFace.Data.Enums;
using BookFace.Data.Models;
using BookFace.Models.User;
using BookFace.Services.Chat;
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
            FriendshipStatus.Requested,
        };

        private readonly ApplicationDbContext data;

        private readonly IFriendService friendService;

        private readonly IChatService chatService;

        public FriendshipService(ApplicationDbContext data, IFriendService friendService, IChatService chatService)
        {
            this.data = data;
            this.friendService = friendService;
            this.chatService = chatService;
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
            return friendship != null && ((friendship.FirstUserStatus == FriendshipStatus.Accepted && friendship.SecondUserStatus == FriendshipStatus.Requested) || (friendship.FirstUserStatus == FriendshipStatus.Requested && friendship.SecondUserStatus == FriendshipStatus.Accepted));
        }

        public bool Block(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.FirstUserId == firstId && x.SecondUserId == secondId);

            if (friendship == null)
            {
                friendship = CreateFriendship(firstId, secondId);
            }

            if (CanBlock(friendship, isPrepared) == false)
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
            return friendship != null && (isPrepared ? friendship.FirstUserStatus == FriendshipStatus.Requested : friendship.SecondUserStatus == FriendshipStatus.Requested);
        }

        public bool CanBlock(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.FirstUserId == firstId && x.SecondUserId == secondId);

            return CanBlock(friendship, isPrepared);
        }

        public bool CanBlock(Friendship friendship, bool isPrepared)
        {
            return friendship == null || (isPrepared ? friendship.SecondUserStatus != FriendshipStatus.Blocked : friendship.FirstUserStatus != FriendshipStatus.Blocked);
        }

        public bool CanRequest(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.FirstUserId == firstId && x.SecondUserId == secondId);

            if (friendship == null)
            {
                return true;
            }

            return CanRequest(friendship);
        }

        public bool CanRequest(Friendship friendship)
        {
            return !(friendship == null || badStatuses.Contains(friendship.FirstUserStatus) || badStatuses.Contains(friendship.SecondUserStatus));
        }

        public Friendship CreateFriendship(string firstId, string secondId, FriendshipStatus firstStatus = FriendshipStatus.NoneAction, FriendshipStatus secondStatus = FriendshipStatus.NoneAction)
        {
            if (friendService.IsExistingFriend(firstId) == false || friendService.IsExistingFriend(secondId) == false)
            {
                return null;
            }

            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = new Friendship
            {
                FirstUserId = firstId,
                SecondUserId = secondId,
                FirstUserStatus = isPrepared ? secondStatus : firstStatus,
                SecondUserStatus = isPrepared ? firstStatus : secondStatus,
                ChatId = chatService.CreateChat(),
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

        public IEnumerable<FriendModel> People(string userId, string searchTerm, int currentPage, int poeplePerPage)
        {
            var people = data.Friends
                .Include(x => x.User)
                .Select(x => x.User)
                .Where(x => x.Id != userId)
                .ToList();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTerms = searchTerm.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower());
                Predicate<ApplicationUser> predicate = x =>
                {
                    var names = (x.FirstName + x.LastName).ToLower();
                    return searchTerms.Any(st => names.Contains(st));
                };

                people = people
                    .AsEnumerable()
                    .Where(x => predicate(x))
                    .ToList();

            }

            var myFriends = MyFriendsId(userId);

            return people
                .Skip((currentPage - 1) * poeplePerPage)
                .Take(poeplePerPage)
                .Select(x => new FriendModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ProfileImagePath = x.ProfileImagePath,
                    MutualFriendsCount = MyFriendsId(x.Id).Intersect(myFriends).Count()
                })
                .OrderBy(x => x.MutualFriendsCount);

        }

        public IEnumerable<Friendship> MyFriendships(string userId)
        {
            return data.Friendships.Where(x => x.FirstUserId == userId || x.SecondUserId == userId)
                .AsEnumerable()
                .Where(x => AreFriends(x))
                .ToList();
        }

        public IEnumerable<string> MyFriendsId(string userId)
        {
            return MyFriendsId(userId, MyFriendships(userId));
        }

        public IEnumerable<string> MyFriendsId(string userId, IEnumerable<Friendship> myFriendships)
        {
            return myFriendships
               .Select(x => x.FirstUserId != userId ? x.FirstUserId : x.SecondUserId)
               .Where(x => AreFriends(userId, x))
               .ToList();
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

        public IEnumerable<FriendModel> Suggestions(string userId, int count)
        {
            var myFriendships = MyFriendships(userId);

            var myFriends = MyFriendsId(userId, myFriendships);

            var suggestionIds = data.Friendships
                .Include(x => x.FirstUser)
                .Include(x => x.SecondUser)
                .AsEnumerable()
                .Where(x => myFriendships.Contains(x) == false)
                .Where(x => (myFriends.Contains(x.FirstUserId) || myFriends.Contains(x.SecondUserId)) && AreFriends(x))
                .Select(x => myFriends.Contains(x.FirstUserId) ? x.SecondUserId : x.FirstUserId)
                .GroupBy(x => x)
                .Where(x => CanRequest(userId, x.Key))
                .ToDictionary(x => x.Key, x => x.Count())
                .OrderByDescending(x => x.Value)
                .ToList();

            if (suggestionIds.Count() < count)
            {
                suggestionIds.AddRange(
                    data.Friendships
                     .Include(x => x.FirstUser)
                     .Include(x => x.SecondUser)
                     .AsEnumerable()
                     .Where(x => myFriendships.Contains(x) == false)
                     .Where(x => myFriends.Contains(x.FirstUserId) == false && myFriends.Contains(x.SecondUserId) == false)
                     .Select(x => x.FirstUserId)
                     .Distinct()
                     .Where(x => suggestionIds.Any(y => y.Key == x) == false)
                     .Where(x => CanRequest(userId, x))
                     .ToDictionary(x => x, x => 0)
                     .Take(count - suggestionIds.Count())
                     .ToList());
            }

            if (suggestionIds.Count() < count)
            {
                suggestionIds.AddRange(
                        data.Friends
                         .Include(x => x.Friendships)
                         .AsEnumerable()
                         .Where(x => !x.Friendships.Any())
                         .Select(x => x.UserId)
                         .ToDictionary(x => x, x => 0)
                         .Take(count - suggestionIds.Count())
                         .ToList()
                   );
            }


            return suggestionIds.Select(x => friendService.IndexFriend(x.Key, x.Value));
        }

        public bool UnBlock(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.FirstUserId == firstId && x.SecondUserId == secondId);

            if (CanBlock(friendship, isPrepared) == true)
            {
                return false;
            }

            if (isPrepared)
            {
                friendship.SecondUserStatus = FriendshipStatus.UnBlocked;
            }
            else
            {
                friendship.FirstUserStatus = FriendshipStatus.UnBlocked;
            }

            data.SaveChanges();

            return true;
        }

        public Friendship Friendship(string firstId, string secondId)
        {
            return data.Friendships.FirstOrDefault(x => (x.FirstUserId == firstId && x.SecondUserId == secondId) || (x.FirstUserId == secondId && x.SecondUserId == firstId));
        }

        public int TotalPeople()
        {
            return data.Friends.Count();
        }
    }
}
