using BookFace.Infrastructure.Extensions;
using BookFace.Models.Friendship;
using BookFace.Services.Friendship;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Controllers
{
    [Authorize]
    public class FriendshipsController : Controller
    {
        private readonly IFriendshipService friendshipService;

        public FriendshipsController(IFriendshipService friendshipService)
        {
            this.friendshipService = friendshipService;
        }
        public IActionResult All([FromQuery] FriendshipQueryModel query)
        {
            var people = friendshipService.People(
                User.Id(),
                query.SearchTerm,
                query.CurrentPage,
                FriendshipQueryModel.PeoplePerPage);

            query.CurrentPeople = people;
            query.TotalPeople = friendshipService.TotalPeople();
            return View(query);
        }
    }
}
