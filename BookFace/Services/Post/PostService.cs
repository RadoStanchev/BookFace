using BookFace.Data;
using BookFace.Data.Models;
using BookFace.Models.Post;
using BookFace.Models.User;
using BookFace.Services.ApplicationUsers;
using BookFace.Services.Comment;
using BookFace.Services.Friendship;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookFace.Services.Post
{
    using Post = BookFace.Data.Models.Post;
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext data;
        private readonly IApplicationUserService applicationUserService;
        private readonly IFriendshipService friendshipService;
        private readonly ICommentService commentService;

        public PostService(ApplicationDbContext data, 
            IApplicationUserService applicationUserService, 
            ICommentService commentService,
            IFriendshipService friendshipService)
        {
            this.data = data;
            this.applicationUserService = applicationUserService;
            this.commentService = commentService;
            this.friendshipService = friendshipService;
        }

        public bool CanLike(string postId, string userId)
        {
            var postGuid = Guid.Parse(postId);
            var post = data.Posts.FirstOrDefault(x => x.Id == postGuid);
            var user = data.ApplicationUsers.FirstOrDefault(x => x.Id == userId);

            return CanLike(post, user);
        }

        public bool CanLike(Post post, ApplicationUser user)
        {
            if (post == null || user == null) return false;
            return !data.Likes.Any(l => l.PostId == post.Id && l.UserId == user.Id);
        }

        public string CreatePost(string creatorId, string content, string image)
        {
            var post = new Post()
            {
                Id = Guid.NewGuid(),
                CreatorId = creatorId,
                Content = content,
                Image = image,
                CreatedOn = DateTime.UtcNow,
            };

            data.Posts.Add(post);
            data.SaveChanges();

            return post.Id.ToString();
        }

        public bool UnLikePost(string postId, string userId)
        {
            var postGuid = Guid.Parse(postId);
            var like = data.Likes.FirstOrDefault(l => l.PostId == postGuid && l.UserId == userId);
            
            if (like == null)
            {
                return false;
            }

            data.Likes.Remove(like);
            data.SaveChanges();

            return true;
        }

        public bool LikePost(string postId, string userId)
        {
            var postGuid = Guid.Parse(postId);
            var post = data.Posts.FirstOrDefault(x => x.Id == postGuid);
            var user = data.ApplicationUsers.FirstOrDefault(x => x.Id == userId);

            if (CanLike(post, user) == false)
            {
                return false;
            }

            var like = new Like 
            { 
                Id = Guid.NewGuid(),
                PostId = postGuid, 
                UserId = userId,
                CreatedOn = DateTime.UtcNow
            };

            data.Likes.Add(like);
            data.SaveChanges();

            return true;
        }

        public int LikesCount(string postId)
        {
            var postGuid = Guid.Parse(postId);
            return data.Likes.Count(x => x.PostId == postGuid);
        }

        public HomePostModel Post(string postId, string userId)
        {
            var postGuid = Guid.Parse(postId);
            var post = data.Posts.Include(x => x.Likes).FirstOrDefault(x => x.Id == postGuid);
            if (post == null) return null;

            return new HomePostModel
            {
                Id = post.Id.ToString(),
                Content = post.Content,
                Image = post.Image,
                Comments = commentService.IndexPostComments(postId),
                Owner = applicationUserService.Owner(post.CreatorId),
                DateDiff = post.CreatedOn.ToString("dddd, dd MMMM yyyy HH:mm"),
                IsLiked = post.Likes.Any(l => l.UserId == userId),
            };
        }

        public IEnumerable<HomePostModel> Posts(string userId, int currentPage, int postsPerPage)
        {
            var myFriends = friendshipService.MyFriendsId(userId).ToList();
            myFriends.Add(userId); // Include own posts

            return data.Posts
                    .Include(x => x.Likes)
                    .Include(x => x.Comments)
                    .Where(x => myFriends.Contains(x.CreatorId))
                    .OrderByDescending(x => x.CreatedOn)
                    .Skip((currentPage - 1) * postsPerPage)
                    .Take(postsPerPage)
                    .AsEnumerable()
                    .Select(x => new HomePostModel
                    {
                        Id = x.Id.ToString(),
                        Content = x.Content,
                        Image = x.Image,
                        Comments = commentService.IndexPostComments(x.Comments),
                        Owner = applicationUserService.Owner(x.CreatorId),
                        DateDiff = x.CreatedOn.ToString("dddd, dd MMMM yyyy HH:mm"),
                        IsLiked = x.Likes.Any(l => l.UserId == userId),
                    })
                    .ToList();
        }

        public int TotalPosts(string userId)
        {
            var myFriends = friendshipService.MyFriendsId(userId).ToList();
            myFriends.Add(userId);
            return data.Posts.Where(x => myFriends.Contains(x.CreatorId)).Count();
        }
    }
}
