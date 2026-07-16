using BookFace.Infrastructure.Extensions;
using BookFace.Models.Comment;
using BookFace.Models.Post;
using BookFace.Services.Comment;
using BookFace.Services.ApplicationUsers;
using BookFace.Services.Friendship;
using BookFace.Services.Post;
using BookFace.Services.System;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Hubs
{
    public class PostHub : Hub
    {
        private readonly IPostService postService;
        private readonly ICommentService commentService;
        private readonly IApplicationUserService applicationUserService;
        private readonly IValidator validator;
        private readonly IFriendshipService friendshipService;

        public PostHub(IPostService postService, 
            IValidator validator, 
            ICommentService commentService, 
            IApplicationUserService applicationUserService,
            IFriendshipService friendshipService)
        {
            this.postService = postService;
            this.validator = validator;
            this.commentService = commentService;
            this.applicationUserService = applicationUserService;
            this.friendshipService = friendshipService;
        }

        public Task CreatePost(string content, string image)
        {
            var inputPost = new PostInputModel
            {
                Image = image,
                Content = content,
            };

            (var isValid, ICollection<ValidationResult> errors) = validator.IsValid(inputPost);

            if (isValid == false)
            {
                return Clients.Caller.SendAsync("ShowPostErrors", errors.Select(x => x.ErrorMessage).ToList());
            }

            var postId = postService.CreatePost(Context.User.Id(), content, image);
            var user = applicationUserService.ChatFriend(Context.User.Id());

            var myFriendsId = friendshipService.MyFriendsId(Context.User.Id()).ToList(); 
            myFriendsId.Add(Context.User.Id()); // Also send to the user who created it

            return Clients.Users(myFriendsId).SendAsync("ShowPost", postService.Post(postId, Context.User.Id()), user);
        }

        public Task CreateComment(string content, string postId)
        {
            var inputComment = new CommentInputModel
            {
                PostId = postId,
                Content = content,
            };

            (var isValid, ICollection<ValidationResult> errors) = validator.IsValid(inputComment);

            if (isValid == false)
            {
                return Clients.Caller.SendAsync("ShowCommentErrors", errors.Select(x => x.ErrorMessage).ToList(), postId);
            }

            var commentId = commentService.CreateComment(Context.User.Id(), postId, content);

            return Clients.All.SendAsync("ShowComment", commentService.Comment(commentId), postId);
        }

        public Task LikePost(string postId)
        {
            postService.LikePost(postId, Context.User.Id());
            var count = postService.LikesCount(postId);

            return Clients.All.SendAsync("ChangeLikesCount", postId, count);
        }

        public Task UnLikePost(string postId)
        {
            postService.UnLikePost(postId, Context.User.Id());
            var count = postService.LikesCount(postId);

            return Clients.All.SendAsync("ChangeLikesCount", postId, count);
        }
    }
}
