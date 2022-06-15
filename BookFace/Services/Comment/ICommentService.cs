using BookFace.Models.Comment;
using BookFace.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Services.Comment
{
    public interface ICommentService
    {
        string CreateComment(string creatorId, string postId, string content);

        ICollection<HomePostCommentModel> IndexPostComments(string postId);

        HomePostCommentModel Comment(string commentId);
    }
}
