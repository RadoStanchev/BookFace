using BookFace.Models.Home.Comment;
using BookFace.Models.Home.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Services.Comment
{
    public interface ICommentService
    {
        string CreateComment(string creatorId, string postId, string content);

        ICollection<IndexPostCommentModel> IndexPostComments(string postId);
    }
}
