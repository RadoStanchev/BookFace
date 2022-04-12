using BookFace.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookFace.Data.Models;
using BookFace.Models.Users;
using BookFace.Services.ApplicationUsers;
using System.Threading.Tasks;

namespace BookFace.Controllers
{
    public class UsersController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly IApplicationUserService applicationUserService;
        public UsersController(SignInManager<ApplicationUser> signInManager, IApplicationUserService applicationUserService)
        {
            this.signInManager = signInManager;
            this.applicationUserService = applicationUserService;
        }
        public IActionResult Logout()
        {
            Task.Run(async () =>
            {
                await signInManager.SignOutAsync();
            })
            .GetAwaiter()
            .GetResult();

            return RedirectToAction(nameof(HomeController.Index), typeof(HomeController).GetControllerName());
        }

        public IActionResult Profile(string userId)
        {
            return View(applicationUserService.Profile(userId));
        }

        public IActionResult Edit([FromQuery]string userId)
        {
            if (userId != User.Id())
            {
                return this.RedirectToAction(nameof(UsersController.Profile), userId);
            }

            return View(applicationUserService.Profile(userId));
        }

        [HttpPost]
        public IActionResult Edit([FromForm] ProfileServiceModel profile)
        {
            if((applicationUserService.OwnerOfEmail(profile.Email) != User.Id() &&
                applicationUserService.OwnerOfEmail(profile.Email) != null) ||(applicationUserService.OwnerOfEmail(profile.Username) != User.Id() &&
                applicationUserService.OwnerOfEmail(profile.Username) != null))
            {
                ModelState.AddModelError("error", "UserName is allready used or Email is allready used");
            }

            applicationUserService.Edit(profile.Id, profile.Username, profile.Email, profile.FirstName, profile.LastName);

            return this.RedirectToAction(nameof(UsersController.Profile), new { userId = profile.Id });
        }
    }
}
