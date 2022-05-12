using BookFace.Data;
using BookFace.Data.Models;
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
        public PostService(ApplicationDbContext data)
        {
            this.data = data;
        }
        public string CreatePost(string creatorId, string content, string image)
        {
            var post = new Post()
            {
                CreatorId = creatorId,
                Content = content,
                Image = image,
            };

            data.Posts.Add(post);
            data.SaveChanges();

            return post.Id;
        }
    }
}
