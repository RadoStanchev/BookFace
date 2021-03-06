using BookFace.Infrastructure.Extensions;
using BookFace.Models.Comment;
using BookFace.Models.Post;
using BookFace.Services.Comment;
using BookFace.Services.Friend;
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

        private readonly IFriendService friendService;

        private readonly IValidator validator;

        private readonly IFriendshipService friendshipService;
        public PostHub(IPostService postService, 
            IValidator validator, 
            ICommentService commentService, 
            IFriendService friendService,
            IFriendshipService friendshipService)
        {
            this.postService = postService;
            this.validator = validator;
            this.commentService = commentService;
            this.friendService = friendService;
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
            var user = friendService.ChatFriend(Context.User.Id());

            var myFriendsId = friendshipService.MyFriendsId(Context.User.Id()); 

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
