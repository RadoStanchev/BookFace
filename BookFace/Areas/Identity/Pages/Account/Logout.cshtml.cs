using BookFace.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookFace.Controllers;

namespace BookFace.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToAction(nameof(UsersController.Logout), typeof(UsersController).GetControllerName());
        }
    }
}
