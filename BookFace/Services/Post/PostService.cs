using BookFace.Data;
using BookFace.Data.Models;
using BookFace.Models.Home.Post;
using BookFace.Models.Home.User;
using BookFace.Services.ApplicationUsers;
using BookFace.Services.Comment;
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

        private readonly ICommentService commentService;
        public PostService(ApplicationDbContext data, 
            IApplicationUserService applicationUserService, 
            ICommentService commentService)
        {
            this.data = data;
            this.applicationUserService = applicationUserService;
            this.commentService = commentService;
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

        public IndexPostModel IndexPost(string postId)
        {
            var post = data.Posts.FirstOrDefault(x => x.Id == postId);

            return new IndexPostModel
            {
                Id = post.Id,
                Content = post.Content,
                Image = post.Image,
                Comments = commentService.IndexPostComments(postId),
                Owner = applicationUserService.Owner(post.CreatorId),
            };
        }
    }
}
