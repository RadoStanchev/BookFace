using BookFace.Models.Home.Comment;
using BookFace.Models.Home.Post;
using BookFace.Models.Home.User;
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

        IndexPostModel IndexPost(string postId);
    }
}
