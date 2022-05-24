using BookFace.Data;
using BookFace.Data.Models;
using BookFace.Models.Post;
using BookFace.Models.User;
using BookFace.Services.ApplicationUsers;
using BookFace.Services.Comment;
using BookFace.Services.Friendship;
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

        public HomePostModel Post(string postId)
        {
            var post = data.Posts.FirstOrDefault(x => x.Id == postId);

            return new HomePostModel
            {
                Id = post.Id,
                Content = post.Content,
                Image = post.Image,
                Comments = commentService.IndexPostComments(postId),
                Owner = applicationUserService.Owner(post.CreatorId),
                DateDiff = DateTime.Now - post.CreatedOn,
            };
        }

        public IEnumerable<HomePostModel> Posts(string userId, int count)
        {
            var myFriends = friendshipService.MyFriendsId(userId);
            return data.Posts.
                    Where(x => myFriends.Contains(x.CreatorId))
                    .OrderByDescending(x => x.CreatedOn)
                    .Select(x => new HomePostModel
                    {
                        Id = x.Id,
                        Content = x.Content,
                        Image = x.Image,
                        Comments = commentService.IndexPostComments(x.Id),
                        Owner = applicationUserService.Owner(x.CreatorId),
                        DateDiff = DateTime.Now - x.CreatedOn,
                    })
                    .Take(count);
        }
    }
}
