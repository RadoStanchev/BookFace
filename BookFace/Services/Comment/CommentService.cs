﻿using BookFace.Data;
using BookFace.Models.Comment;
using BookFace.Models.User;
using BookFace.Services.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public HomeOwnerModel CommentOwner(string userId)
        {
            throw new NotImplementedException();
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
                        .Where(x => x.PostId == postId)
                        .Select(x => new HomePostCommentModel
                        {
                            Content = x.Content,
                            Owner = applicationUserService.Owner(x.CreatorId)
                        })
                        .ToList();
        }
    }
}