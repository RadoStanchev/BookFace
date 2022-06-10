using BookFace.Data;
using BookFace.Models.Comment;
using BookFace.Services.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookFace.Services.Comment
{
    using Comment = BookFace.Data.Models.Comment;

    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext data;

        private readonly IApplicationUserService applicationUserService;
        public CommentService(ApplicationDbContext data, IApplicationUserService applicationUserService)
        {
            this.data = data;
            this.applicationUserService = applicationUserService;
        }

        public HomePostCommentModel Comment(string commentId)
        {
            var comment = data.Comments.FirstOrDefault(x => x.Id == commentId);

            return new HomePostCommentModel
            {
                Content = comment.Content,
                Owner = applicationUserService.Owner(comment.CreatorId),
                DateDiff = comment.CreatedOn.ToString("dddd, dd MMMM yyyy HH:mm"),
            };
        }

        public string CreateComment(string creatorId, string postId, string content)
        {
            var comment = new Comment()
            {
                CreatorId = creatorId,
                Content = content,
                PostId = postId,
                CreatedOn = DateTime.Now,
            };

            data.Comments.Add(comment);
            data.SaveChanges();

            return comment.Id;
        }

        public ICollection<HomePostCommentModel> IndexPostComments(string postId)
        {
            return data.Comments
                        .AsQueryable()
                        .Where(x => x.PostId == postId)
                        .Select(x => new HomePostCommentModel
                        {
                            Content = x.Content,
                            Owner = applicationUserService.Owner(x.CreatorId),
                            DateDiff = x.CreatedOn.ToString("dddd, dd MMMM yyyy HH:mm"),
                        })
                        .ToList();
        }
    }
}
