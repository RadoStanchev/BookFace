using BookFace.Infrastructure.Extensions;
using BookFace.Models.Home.Post;
using BookFace.Services.Post;
using BookFace.Services.System;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Hubs
{
    public class PostHub : Hub
    {
        private readonly IPostService postService;

        private readonly IValidator validator;
        public PostHub(IPostService postService, IValidator validator)
        {
            this.postService = postService;
            this.validator = validator;
        }
        public async Task CreatePost(string content, string image)
        {
            var inputPost = new IndexPostInputModel
            {
                Image = image,
                Content = content,
            };

            (var isValid, ICollection<ValidationResult> errors) = validator.IsValid(inputPost);

            if (isValid == false)
            {
                await Clients.Caller.SendAsync("ShowPostErrors", errors.Select(x => x.ErrorMessage).ToList());
            }
            else
            {
                var postId = string.Empty;
                await Task.Run(async () => postId =  postService.CreatePost(Context.User.Id(), content, image));

                await Clients.Caller.SendAsync("ShowPost", postService.IndexPost(postId));
            }
        }
    }
}
