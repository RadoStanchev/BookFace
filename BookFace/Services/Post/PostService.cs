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
using System.Threading.Tasks;
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
            var post = data.Posts.FirstOrDefault(x => x.Id == postId);
            var user = data.ApplicationUsers.Include(x => x.Likes).FirstOrDefault(x => x.Id == userId);

            return CanLike(post, user);
        }

        public bool CanLike(Post post, ApplicationUser user)
        {
            return post.Likes.Contains(user) == false;
        }

        public string CreatePost(string creatorId, string content, string image)
        {
            var post = new Post()
            {
                CreatorId = creatorId,
                Content = content,
                Image = image,
                CreatedOn = DateTime.Now,
            };

            data.Posts.Add(post);
            data.SaveChanges();

            return post.Id;
        }

        public bool UnLikePost(string postId, string userId)
        {
            var post = data.Posts.FirstOrDefault(x => x.Id == postId);
            var user = data.ApplicationUsers.Include(x => x.Likes).FirstOrDefault(x => x.Id == userId);

            if (CanLike(post, user))
            {
                return false;
            }

            user.Likes.Remove(post);
            data.SaveChanges();

            return true;
        }

        public bool LikePost(string postId, string userId)
        {
            var post = data.Posts.FirstOrDefault(x => x.Id == postId);
            var user = data.ApplicationUsers.Include(x => x.Likes).FirstOrDefault(x => x.Id == userId);

            if (CanLike(post, user) == false)
            {
                return false;
            }

            user.Likes.Add(post);
            data.SaveChanges();

            return true;
        }

        public int LikesCount(string postId)
        {
            return data.Posts.FirstOrDefault(x => x.Id == postId).Likes.Count();
        }

        public HomePostModel Post(string postId, string userId)
        {
            var post = data.Posts.Include(x => x.Likes).FirstOrDefault(x => x.Id == postId);
            var user = data.ApplicationUsers.FirstOrDefault(x => x.Id == userId);

            return new HomePostModel
            {
                Id = post.Id,
                Content = post.Content,
                Image = post.Image,
                Comments = commentService.IndexPostComments(postId),
                Owner = applicationUserService.Owner(post.CreatorId),
                DateDiff = post.CreatedOn.ToString("dddd, dd MMMM yyyy HH:mm"),
                IsLiked = post.Likes.Contains(user),
            };
        }

        public IEnumerable<HomePostModel> Posts(string userId, int currentPage, int postsPerPage)
        {
            var myFriends = friendshipService.MyFriendsId(userId);
            var user = data.ApplicationUsers.FirstOrDefault(x => x.Id == userId);
            return data.Posts
                    .Include(x => x.Likes)
                    .Include(x => x.Comments)
                    .AsEnumerable()
                    .Where(x => myFriends.Contains(x.CreatorId))
                    .OrderByDescending(x => x.CreatedOn)
                    .Skip((currentPage - 1) * postsPerPage)
                    .Take(postsPerPage)
                    .Select(x => new HomePostModel
                    {
                        Id = x.Id,
                        Content = x.Content,
                        Image = x.Image,
                        Comments = commentService.IndexPostComments(x.Comments),
                        Owner = applicationUserService.Owner(x.CreatorId),
                        DateDiff = x.CreatedOn.ToString("dddd, dd MMMM yyyy HH:mm"),
                        IsLiked = x.Likes.Contains(user),
                    })
                    .ToList();
        }

        public int TotalPosts(string userId)
        {
            var myFriends = friendshipService.MyFriendsId(userId);
            return data.Posts.Where(x => myFriends.Contains(x.CreatorId)).Count();
        }
    }
}
