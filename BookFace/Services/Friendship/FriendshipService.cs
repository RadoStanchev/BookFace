using BookFace.Data;
using BookFace.Data.Enums;
using BookFace.Data.Models;
using BookFace.Models.User;
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

        public FriendshipService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public bool Accept(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.RequesterId == firstId && x.AddresseeId == secondId);

            if (CanAcceptOrDeny(friendship, isPrepared) == false)
            {
                return false;
            }

            if (isPrepared)
            {
                friendship.AddresseeStatus = FriendshipStatus.Accepted;
            }
            else
            {
                friendship.RequesterStatus = FriendshipStatus.Accepted;
            }

            data.SaveChanges();

            return true;
        }

        public bool AreFriends(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.RequesterId == firstId && x.AddresseeId == secondId);

            return AreFriends(friendship);
        }

        public bool AreFriends(Friendship friendship)
        {
            return friendship != null && ((friendship.RequesterStatus == FriendshipStatus.Accepted && friendship.AddresseeStatus == FriendshipStatus.Requested) || (friendship.RequesterStatus == FriendshipStatus.Requested && friendship.AddresseeStatus == FriendshipStatus.Accepted));
        }

        public bool Block(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.RequesterId == firstId && x.AddresseeId == secondId);

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
                friendship.AddresseeStatus = FriendshipStatus.Blocked;
            }
            else
            {
                friendship.RequesterStatus = FriendshipStatus.Blocked;
            }

            data.SaveChanges();

            return true;
        }

        public bool BreakUp(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.RequesterId == firstId && x.AddresseeId == secondId);

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
                friendship.AddresseeStatus = FriendshipStatus.BrokeUp;
            }
            else
            {
                friendship.RequesterStatus = FriendshipStatus.BrokeUp;
            }

            data.SaveChanges();

            return true;
        }

        public bool CanAcceptOrDeny(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.RequesterId == firstId && x.AddresseeId == secondId);

            return CanAcceptOrDeny(friendship, isPrepared);
        }

        public bool CanAcceptOrDeny(Friendship friendship, bool isPrepared)
        {
            return friendship != null && (isPrepared ? friendship.RequesterStatus == FriendshipStatus.Requested : friendship.AddresseeStatus == FriendshipStatus.Requested);
        }

        public bool CanBlock(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.RequesterId == firstId && x.AddresseeId == secondId);

            return CanBlock(friendship, isPrepared);
        }

        public bool CanBlock(Friendship friendship, bool isPrepared)
        {
            return friendship == null || (isPrepared ? friendship.AddresseeStatus != FriendshipStatus.Blocked : friendship.RequesterStatus != FriendshipStatus.Blocked);
        }

        public bool CanRequest(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.RequesterId == firstId && x.AddresseeId == secondId);

            if (friendship == null)
            {
                return true;
            }

            return CanRequest(friendship);
        }

        public bool CanRequest(Friendship friendship)
        {
            return !(friendship == null || badStatuses.Contains(friendship.RequesterStatus) || badStatuses.Contains(friendship.AddresseeStatus));
        }

        public Friendship CreateFriendship(string firstId, string secondId, FriendshipStatus firstStatus = FriendshipStatus.NoneAction, FriendshipStatus secondStatus = FriendshipStatus.NoneAction)
        {
            if (!data.ApplicationUsers.Any(x => x.Id == firstId) || !data.ApplicationUsers.Any(x => x.Id == secondId))
            {
                return null;
            }

            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = new Friendship
            {
                Id = Guid.NewGuid(),
                RequesterId = firstId,
                AddresseeId = secondId,
                RequesterStatus = isPrepared ? secondStatus : firstStatus,
                AddresseeStatus = isPrepared ? firstStatus : secondStatus
            };

            data.Friendships.Add(friendship);
            data.SaveChanges();

            return friendship;
        }

        public bool Deny(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.RequesterId == firstId && x.AddresseeId == secondId);

            if (CanAcceptOrDeny(friendship, isPrepared) == false)
            {
                return false;
            }

            if (isPrepared)
            {
                friendship.AddresseeStatus = FriendshipStatus.Denied;
            }
            else
            {
                friendship.RequesterStatus = FriendshipStatus.Denied;
            }

            data.SaveChanges();

            return true;
        }

        public IEnumerable<FriendModel> People(string userId, string searchTerm, int currentPage, int poeplePerPage)
        {
            var people = data.ApplicationUsers
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
            return data.Friendships.Where(x => x.RequesterId == userId || x.AddresseeId == userId)
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
               .Where(x => AreFriends(x))
               .Select(x => x.RequesterId != userId ? x.RequesterId : x.AddresseeId)
               .ToList();
        }

        public bool PrepareIds(ref string firsrId, ref string secondId)
        {
            if (firsrId.CompareTo(secondId) < 0)
            {
                var temp = firsrId;
                firsrId = secondId;
                secondId = temp;
                return true;
            }

            return false;
        }

        public bool Request(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.RequesterId == firstId && x.AddresseeId == secondId);

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
                friendship.AddresseeStatus = FriendshipStatus.Requested;
            }
            else
            {
                friendship.RequesterStatus = FriendshipStatus.Requested;
            }

            data.SaveChanges();

            return true;
        }

        public IEnumerable<FriendModel> Suggestions(string userId, int count)
        {
            var myFriendships = MyFriendships(userId);
            var myFriends = MyFriendsId(userId, myFriendships);

            var suggestionIds = data.Friendships
                .Include(x => x.Requester)
                .Include(x => x.Addressee)
                .AsEnumerable()
                .Where(x => myFriendships.Contains(x) == false)
                .Where(x => (myFriends.Contains(x.RequesterId) || myFriends.Contains(x.AddresseeId)) && AreFriends(x))
                .Select(x => myFriends.Contains(x.RequesterId) ? x.AddresseeId : x.RequesterId)
                .GroupBy(x => x)
                .Where(x => CanRequest(userId, x.Key))
                .ToDictionary(x => x.Key, x => x.Count())
                .OrderByDescending(x => x.Value)
                .ToList();

            if (suggestionIds.Count() < count)
            {
                suggestionIds.AddRange(
                    data.Friendships
                     .Include(x => x.Requester)
                     .Include(x => x.Addressee)
                     .AsEnumerable()
                     .Where(x => myFriendships.Contains(x) == false)
                     .Where(x => myFriends.Contains(x.RequesterId) == false && myFriends.Contains(x.AddresseeId) == false)
                     .Select(x => x.RequesterId)
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
                        data.ApplicationUsers
                         .AsEnumerable()
                         .Where(x => x.Id != userId && !data.Friendships.Any(f => f.RequesterId == x.Id || f.AddresseeId == x.Id))
                         .Select(x => x.Id)
                         .ToDictionary(x => x, x => 0)
                         .Take(count - suggestionIds.Count())
                         .ToList()
                   );
            }

            var appUsers = data.ApplicationUsers.Where(u => suggestionIds.Select(s => s.Key).Contains(u.Id)).ToList();

            return suggestionIds.Select(x => {
                var user = appUsers.FirstOrDefault(u => u.Id == x.Key);
                return new FriendModel {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfileImagePath = user.ProfileImagePath,
                    MutualFriendsCount = x.Value
                };
            });
        }

        public bool UnBlock(string firstId, string secondId)
        {
            var isPrepared = PrepareIds(ref firstId, ref secondId);

            var friendship = data.Friendships.FirstOrDefault(x => x.RequesterId == firstId && x.AddresseeId == secondId);

            if (CanBlock(friendship, isPrepared) == true)
            {
                return false;
            }

            if (isPrepared)
            {
                friendship.AddresseeStatus = FriendshipStatus.UnBlocked;
            }
            else
            {
                friendship.RequesterStatus = FriendshipStatus.UnBlocked;
            }

            data.SaveChanges();

            return true;
        }

        public Friendship Friendship(string firstId, string secondId)
        {
            return data.Friendships.FirstOrDefault(x => (x.RequesterId == firstId && x.AddresseeId == secondId) || (x.RequesterId == secondId && x.AddresseeId == firstId));
        }

        public int TotalPeople()
        {
            return data.ApplicationUsers.Count();
        }
    }
}
