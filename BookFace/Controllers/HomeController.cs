using BookFace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BookFace.Infrastructure.Extensions;
using BookFace.Models.Home;
using BookFace.Services.Friendship;
using BookFace.Services.ApplicationUsers;
using BookFace.Services.Post;

namespace BookFace.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IFriendshipService friendshipService;

        private readonly IPostService postService;

        private readonly IApplicationUserService applicationUserService;

        public HomeController(
            IFriendshipService friendshipService, 
            IPostService postService, 
            IApplicationUserService applicationUserService)
        {
            this.friendshipService = friendshipService;
            this.postService = postService;
            this.applicationUserService = applicationUserService;
        }

        public IActionResult Index([FromQuery] HomePostSuggestionQueryModel query)
        {
            var model = new HomePostSuggestionQueryModel();
            if (User.Identity.IsAuthenticated)
            {
                model.User = applicationUserService.ChatFriend(User.Id());
                model.Suggestions = friendshipService.Suggestions(User.Id(), 10);
                model.Posts = postService.Posts(User.Id(), query.CurrentPage, HomePostSuggestionQueryModel.PostsPerPage);
                model.TotalPosts = postService.TotalPosts(User.Id());
                model.CurrentPage = query.CurrentPage;
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
