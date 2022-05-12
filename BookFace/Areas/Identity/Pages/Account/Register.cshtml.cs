using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookFace.Data.Models;
using BookFace.Services.ApplicationUsers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static BookFace.Data.DataConstants.ApplicationUser;
using BookFace.Services.Friend;
using BookFace.Infrastructure.Extensions;
using System;

namespace BookFace.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApplicationUserService applicationUserService;
        private readonly IFriendService friendService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IApplicationUserService applicationUserService,
            IFriendService friendService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.applicationUserService = applicationUserService;
            this.friendService = friendService;
            Input = new InputModel();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }


        public class InputModel
        {
            [Required]
            [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [StringLength(UsernameMaxLength, MinimumLength = UsernameMinLength)]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Avatar")]
            public string ProfileImagePath { get; set; }

            public IEnumerable<string> AllProfileImagePaths { get; set; }
        }

        public void OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Input.AllProfileImagePaths = applicationUserService.GetProfileImagePaths();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            Input.AllProfileImagePaths = applicationUserService.GetProfileImagePaths();
            returnUrl = returnUrl ?? Url.Content("~/");


            if (ModelState.IsValid)
            {
                List<string> allProfileImagePaths = applicationUserService.GetProfileImagePaths().ToList();
                if (allProfileImagePaths.Contains(Input.ProfileImagePath))
                {
                    var user = new ApplicationUser
                    {
                        UserName = Input.UserName,
                        Email = Input.Email,
                        FirstName = Input.FirstName,
                        LastName = Input.LastName,
                        ProfileImagePath = Input.ProfileImagePath,
                    };

                    var result = await userManager.CreateAsync(user, Input.Password);

                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        friendService.CreateFriend(user.Id);
                        return LocalRedirect(returnUrl);
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return Page();
        }
    }
}