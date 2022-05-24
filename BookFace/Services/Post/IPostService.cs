using BookFace.Models.Comment;
using BookFace.Models.Post;
using BookFace.Models.User;
using BookFace.Services.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Services.Post
{
    public interface IPostService
    {
        string CreatePost(string creatorId, string content, string image);

        HomePostModel Post(string postId);

        IEnumerable<HomePostModel> Posts(string userId, int count);
    }
}
