using BookFace.Data.Models;
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
    using Post = BookFace.Data.Models.Post;
    public interface IPostService
    {
        string CreatePost(string creatorId, string content, string image);

        HomePostModel Post(string postId, string userId);

        IEnumerable<HomePostModel> Posts(string userId, int currentPage, int postsPerPage);

        int TotalPosts(string userId);

        bool CanLike(string postId, string userId);

        bool CanLike(Post post, ApplicationUser user);

        bool LikePost(string postId, string userId);

        bool DisLikePost(string postId, string userId);

        int LikesCount(string postId);
    }
}
