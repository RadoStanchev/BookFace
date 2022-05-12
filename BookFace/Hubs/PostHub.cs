using BookFace.Services.Post;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Hubs
{
    public class PostHub : Hub
    {
        private readonly IPostService postService;
        public PostHub(IPostService postService)
        {
            this.postService = postService;
        }
        public async Task CreatePost(string creatorId, string content, string image)
        {
            await Task.Run(async () => postService.CreatePost(creatorId, content, image));
        }
    }
}
